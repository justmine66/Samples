﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace SFtpDownloader.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
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
                .Build();

            await host.RunConsoleAsync();
        }
    }
}
