using Microsoft.Extensions.DependencyInjection;

namespace SFtpDownloader
{
    /// <summary>
    /// An interface for configuring mda providers.
    /// </summary>
    public interface IConfigureBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where mda services are configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
