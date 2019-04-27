using System.IO;

namespace SFtpDownloader
{
    /// <summary>
    /// SFTP 通用配置项
    /// </summary>
    public class SFtpOptions
    {
        /// <summary>
        /// sftp 主机，必须。
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 用户名，必须。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码，密码认证方案下使用，可选。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 私钥，安全密钥认证方案下使用，可选。
        /// </summary>
        public Stream PrivateKey { get; set; }

        /// <summary>
        /// 文件本地目录，可选。
        /// </summary>
        public string LocalDirectory { get; set; }

        /// <summary>
        /// 远程文件目录，必须。
        /// </summary>
        public string RemoteDirectory { get; set; }

        /// <summary>
        /// 认证方案
        /// </summary>
        public AuthenticateScheme AuthScheme { get; set; }

        public enum AuthenticateScheme
        {
            /// <summary>
            /// 密码
            /// </summary>
            Password,
            /// <summary>
            /// 安全密钥
            /// </summary>
            SecurityKey
        }
    }
}