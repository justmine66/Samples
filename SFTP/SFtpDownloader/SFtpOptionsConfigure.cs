using Microsoft.Extensions.Options;

namespace SFtpDownloader
{
    public class SFtpOptionsConfigure : ConfigureOptions<SFtpOptions>
    {
        public SFtpOptionsConfigure(string host, string userName, string password, string embeddedPrivateKeyFullName, string remoteDirectory, string localDirectory)
            : base(options =>
            {
                options.Host = host;
                options.UserName = userName;
                options.RemoteDirectory = remoteDirectory;
                options.LocalDirectory = localDirectory;

                if (!string.IsNullOrEmpty(password))
                {
                    options.Password = password;
                    options.AuthScheme = SFtpOptions.AuthenticateScheme.Password;
                }


                if (!string.IsNullOrEmpty(embeddedPrivateKeyFullName))
                {
                    options.PrivateKey = PrivateKey.Get(embeddedPrivateKeyFullName);
                    options.AuthScheme = SFtpOptions.AuthenticateScheme.Password;
                }
            })
        {
        }
    }
}
