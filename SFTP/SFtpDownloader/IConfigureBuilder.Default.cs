using Microsoft.Extensions.DependencyInjection;

namespace SFtpDownloader
{
    /// <summary>
    /// An interface for configuring mda providers.
    /// </summary>
    internal class ConfigureBuilder : IConfigureBuilder
    {
        public ConfigureBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
