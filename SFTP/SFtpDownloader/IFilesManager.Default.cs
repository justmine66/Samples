using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SFtpDownloader
{
    public class FilesManagerImpl : IFilesManager
    {
        private readonly IDataCache<string[]> _cache;

        private static string CacheKeyFormat = "source.reports.{0}";

        public FilesManagerImpl(IDataCache<string[]> cache)
        {
            _cache = cache;
        }

        public Task<bool> IsDownloadedAsync(int tenantId, string localDirectory, string report)
        {
            var cacheKey = string.Format(CacheKeyFormat, tenantId);
            var reports = _cache.Get(cacheKey);
            if (reports?.Length > 0 && reports.Contains(report))
                return Task.FromResult(true);

            if (!Directory.Exists(localDirectory))
                return Task.FromResult(false);

            reports = Directory.GetFiles(localDirectory);

            var isDownload = false;
            foreach (var downloadReport in reports)
            {
                isDownload = downloadReport.Contains(report);
                if (!isDownload) continue;
                _cache.TryAdd(cacheKey, reports);
            }

            return Task.FromResult(isDownload);
        }

        public bool TryRefreshSavePoint(int tenantId)
        {
            var cacheKey = string.Format(CacheKeyFormat, tenantId);
            return _cache.TryRemove(cacheKey);
        }

        public bool TryRemoveSavePoint(int tenantId, params string[] files)
        {
            if (files == null) return false;

            var cacheKey = string.Format(CacheKeyFormat, tenantId);
            _cache.TryRemove(cacheKey);

            foreach (var file in files)
                if (File.Exists(file))
                    File.Delete(file);

            return true;
        }
    }

}
