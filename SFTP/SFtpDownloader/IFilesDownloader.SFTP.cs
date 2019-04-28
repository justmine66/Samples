using ComponentPro.IO;
using ComponentPro.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFtpDownloader
{
    /// <summary>
    /// 表示一个SFTP文件下载器实现类。
    /// </summary>
    public class SFTPFilesDownloader : IFilesDownloader
    {
        private readonly ILogger<SFTPFilesDownloader> _logger;
        private readonly IFileNamingStrategy _namingStrategy;
        private readonly IFilesManager _manager;
        private readonly IOptions<SFtpOptions> _options;

        private int _isDownloading = 0;

        public SFTPFilesDownloader(
            ILogger<SFTPFilesDownloader> logger,
            IFileNamingStrategy namingStrategy,
            IOptions<SFtpOptions> options,
            IFilesManager manager)
        {
            _logger = logger;
            _namingStrategy = namingStrategy;
            _manager = manager;
            _options = options;
        }

        public async Task<string[]> DownloadAsync(int siteId, SFtpOptions options = null)
        {
            var policy = PolicyFactory.CreateRetry(5, (e) =>
            {
                _logger.LogError($"Retrying, ex: {e}.");
            });

            var files = await policy.Execute(async () => await DoDownloadAsync(siteId, options));
            if (files?.Length > 0)
                return files;

            _logger.LogWarning($"No files found from sftp server.");
            return null;
        }

        private async Task<string[]> DoDownloadAsync(int siteId, SFtpOptions options)
        {
            var opts = _options.Value ?? options;
            if (!opts.IsValid())
            {
                _logger.LogError($"{nameof(SFtpOptions)} is invalid, config: {opts}.");
                return null;
            }
            if (siteId <= 0)
            {
                _logger.LogError($"The {nameof(siteId)}[{siteId}] must be provided.");
                return null;
            }

            if (Interlocked.CompareExchange(ref _isDownloading, 1, 0) != 0)
            {
                _logger.LogWarning($"{nameof(SFTPFilesDownloader)} is downloading, please try again later.");
                return null;
            }

            string[] files = null;
            try
            {
                var sftp = new Sftp();

                _logger.LogInformation($"Connecting sftp server[{opts.Host}]...");
                if (string.IsNullOrEmpty(opts.Host))
                {
                    _logger.LogError($"The {nameof(opts.Host)}[{opts.Host}] must be provided.");
                    return null;
                }

                await sftp.ConnectAsync(opts.Host, 22);

                _logger.LogInformation($"Authenticating sftp user[UserName: {opts.UserName}]...");
                switch (opts.AuthScheme)
                {
                    case SFtpOptions.AuthenticateScheme.Password:
                        if (string.IsNullOrEmpty(opts.Password))
                        {
                            _logger.LogError($"In the password authenticate scheme, the password must be provided.");
                            return null;
                        }
                        await sftp.AuthenticateAsync(opts.UserName, opts.Password);
                        break;
                    case SFtpOptions.AuthenticateScheme.SecurityKey:
                        if (opts.PrivateKey == null ||
                            !opts.PrivateKey.CanRead)
                        {
                            _logger.LogError($"In the security sey authenticate scheme, the Private Key must be provided.");
                            return null;
                        }
                        var privateKey = new SecureShellPrivateKey(opts.PrivateKey);
                        await sftp.AuthenticateAsync(opts.UserName, privateKey);
                        break;
                    default:
                        throw new InvalidOperationException($"Only these authenticate schemes[{nameof(SFtpOptions.AuthenticateScheme.Password)},{nameof(SFtpOptions.AuthenticateScheme.SecurityKey)}] supported.");
                }

                var filter = _namingStrategy.GetFileRegexName();
                files = await sftp.ListNameAsync(opts.RemoteDirectory, new NameRegexSearchCondition(filter));
                if (files.Length <= 0)
                {
                    _logger.LogWarning($"No reports files found from sftp server.");
                    return null;
                }

                _logger.LogInformation($"The reports[{files.Aggregate((x, y) => $"{x},{y}")}] will be downloaded.");

                if (string.IsNullOrEmpty(opts.LocalDirectory))
                    opts.LocalDirectory = Path.Combine(Directory.GetCurrentDirectory(), "reports", siteId.ToString());

                if (!Directory.Exists(opts.LocalDirectory)) Directory.CreateDirectory(opts.LocalDirectory);

                var hasNewFile = false;
                foreach (var file in files)
                {
                    var isDownloaded = await _manager.IsDownloadedAsync(siteId, opts.LocalDirectory, file);
                    if (isDownloaded) continue;

                    _logger.LogInformation($"The file[{file}] downloading...");

                    var localPath = Path.Combine(opts.LocalDirectory, file);
                    if (File.Exists(localPath)) File.Delete(localPath);

                    using (var stream = new FileStream(localPath, FileMode.Create))
                    {
                        var remoteDir = opts.RemoteDirectory.TrimStart('/').TrimEnd('/').Replace('\\', '/');
                        var remotePath = $"/{remoteDir}/{file}";
                        await sftp.DownloadFileAsync(remotePath, stream);
                    }

                    hasNewFile = true;
                    _logger.LogInformation($"The file[{localPath}] was downloaded successfully.");
                }

                await sftp.DisconnectAsync();

                if (hasNewFile) _manager.TryRefreshSavePoint(siteId);

                return files.Select(it => Path.Combine(opts.LocalDirectory, it)).ToArray();
            }
            catch (Exception e)
            {
                var filesToRemove = files?.Select(it => Path.Combine(opts.LocalDirectory, it)).ToArray();
                _manager.TryRemoveSavePoint(siteId, filesToRemove);
                _logger.LogError($"{nameof(DownloadAsync)} has a unknown exception, ex: {e}");
                return null;
            }
            finally
            {
                _isDownloading = 0;
            }
        }
    }
}
