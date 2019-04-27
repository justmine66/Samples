using System.Threading.Tasks;

namespace SFtpDownloader
{
    public interface IFilesManager
    {
        /// <summary>
        /// 判断文件是否已经被下载
        /// </summary>
        /// <param name="siteId">站点标识</param>
        /// <param name="localDirectory">存放文件的本地目录</param>
        /// /// <param name="report">文件文件名称</param>
        /// <returns></returns>
        Task<bool> IsDownloadedAsync(int siteId, string localDirectory, string report);

        /// <summary>
        /// 刷新保存点。
        /// </summary>
        /// <param name="siteId">站点标识</param>
        /// <returns>是否刷新成功</returns>
        bool TryRefreshSavePoint(int siteId);

        /// <summary>
        /// 尝试移除保存点。
        /// </summary>
        /// <param name="siteId">站点标识</param>
        /// <param name="files">要移除的文件</param>
        /// <returns>是否移除成功</returns>
        bool TryRemoveSavePoint(int siteId, params string[] files);
    }

}
