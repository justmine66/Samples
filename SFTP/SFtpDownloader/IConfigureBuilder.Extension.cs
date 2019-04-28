using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SFtpDownloader
{
    /// <summary>
    /// Extension methods for setting up mda services in an <see cref="IConfigureBuilder" />.
    /// </summary>
    public static class ConfigureBuilderExtension
    {
        /// <summary>
        /// 使用密码认证方案
        /// </summary>
        /// <param name="builder">配置器</param>
        /// <param name="host">SFTP 主机</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="remoteDirectory">远程目录</param>
        /// <param name="localDirectory">本地目录</param>
        /// <returns>配置器</returns>
        public static IConfigureBuilder UsePwdAuthTScheme(this IConfigureBuilder builder, string host, string userName, string password, string remoteDirectory, string localDirectory = null)
        {
            builder.Services.Add(ServiceDescriptor.Singleton<IConfigureOptions<SFtpOptions>>(
                new SFtpOptionsConfigure(host, userName, password, null, remoteDirectory, localDirectory)));
            return builder;
        }

        /// <summary>
        /// 使用安全密钥认证方案
        /// </summary>
        /// <param name="builder">配置器</param>
        /// <param name="host">SFTP 主机</param>
        /// <param name="userName">用户名</param>
        /// <param name="embeddedPrivateKeyFullName">作嵌入式资源安全私钥的完全限定名</param>
        /// <param name="remoteDirectory">远程目录</param>
        /// <param name="localDirectory">本地目录</param>
        /// <returns>配置器</returns>
        public static IConfigureBuilder UseSecurityKeyAuthTScheme(this IConfigureBuilder builder, string host, string userName, string embeddedPrivateKeyFullName, string remoteDirectory, string localDirectory = null)
        {
            builder.Services.Add(ServiceDescriptor.Singleton<IConfigureOptions<SFtpOptions>>(
                new SFtpOptionsConfigure(host, userName, null, embeddedPrivateKeyFullName, remoteDirectory, localDirectory)));
            return builder;
        }

        /// <summary>
        /// 注册命名策略服务。
        /// </summary>
        /// <typeparam name="TFileNamingStrategyService">命名策略服务</typeparam>
        /// <param name="builder">配置器</param>
        /// <returns>配置器</returns>
        public static IConfigureBuilder AddNamingStrategy<TFileNamingStrategyService>(this IConfigureBuilder builder)
            where TFileNamingStrategyService : class, IFileNamingStrategy
        {
            builder.Services.AddSingleton<IFileNamingStrategy, TFileNamingStrategyService>();

            return builder;
        }
    }
}
