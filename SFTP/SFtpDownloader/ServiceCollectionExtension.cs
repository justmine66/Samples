using Microsoft.Extensions.DependencyInjection;
using System;

namespace SFtpDownloader
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSFtpServices(this IServiceCollection container)
        {
            return AddSFtpServices(container, builder => { });
        }

        public static IServiceCollection AddSFtpServices(this IServiceCollection container, Action<IConfigureBuilder> configure)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.AddOptions();

            container.AddSingleton(typeof(IDataCache<>), typeof(DataCacheImpl<>));
            container.AddSingleton<IFilesManager, FilesManagerImpl>();
            container.AddSingleton<IFilesDownloader, SFTPFilesDownloader>();

            configure(new ConfigureBuilder(container));

            return container;
        }
    }
}
