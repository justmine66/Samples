namespace SFtpDownloader
{
    public static class SFtpOptionsExtension
    {
        public static bool IsValid(this SFtpOptions options)
        {
            var isAuthenticated = false;
            switch (options.AuthScheme)
            {
                case SFtpOptions.AuthenticateScheme.Password:
                    isAuthenticated = !string.IsNullOrEmpty(options?.Password);
                    break;
                case SFtpOptions.AuthenticateScheme.SecurityKey:
                    isAuthenticated = options.PrivateKey != null;
                    break;
            }

            return isAuthenticated &&
                   !string.IsNullOrEmpty(options?.Host) &&
                   !string.IsNullOrEmpty(options?.UserName) &&
                   !string.IsNullOrEmpty(options?.RemoteDirectory);
        }
    }

}
