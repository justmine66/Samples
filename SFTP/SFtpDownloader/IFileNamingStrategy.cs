namespace SFtpDownloader
{
    /// <summary>
    /// 文件命名策略接口
    /// </summary>
    /// <remarks>
    /// 文件下载器将根据名称策略来下载文件。
    /// </remarks>
    public interface IFileNamingStrategy
    {
        /// <summary>
        /// 获取文件正则命名
        /// </summary>
        string GetFileRegexName();
    }

}
