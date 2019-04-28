using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SFtpDownloader.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureServices(services => services.AddSFtpServices(builder =>
                {
                    builder.AddNamingStrategy<FileNamingStrategy>();
                    // 密码认证方案
                    builder.UsePwdAuthTScheme("sftp-host", "userName", "pwd", "/upload/");
                    // 安全密钥认证方案
                    builder.UseSecurityKeyAuthTScheme("sftp-host", "userName", "SFtpDownloader.test.ppk", "/upload/");
                }))
                .ConfigureLogging(builder =>
                {
                    builder.AddConsole();
                })
                .RunConsoleAsync();
        }
    }
}
