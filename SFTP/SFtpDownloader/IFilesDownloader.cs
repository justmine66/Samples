using System.Threading.Tasks;

namespace SFtpDownloader
{
    /// <summary>
    /// 表示一个文件下载接口。
    /// </summary>
    public interface IFilesDownloader
    {
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="siteId">用于隔离数据的站点标识。</param>
        /// <param name="options">SFtp所需配置项，当需要运行时配置时，使用此参数。</param>
        /// <returns>已下载的文件列表</returns>
        Task<string[]> DownloadAsync(int siteId, SFtpOptions options = null);
    }
}
