using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFtpDownloader.Test
{
    public class Engine : IHostedService
    {
private readonly ILogger _logger;
private readonly IFilesDownloader _downloader;

public Engine(ILogger<Engine> logger, IFilesDownloader downloader)
{
    _logger = logger;
    _downloader = downloader;
}

public async Task StartAsync(CancellationToken cancellationToken)
{
    var files = await _downloader.DownloadAsync(1);
    _logger.LogInformation($"The files downloaded: {files.Aggregate((x, y) => $"{x},{y}")}.");
}

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
