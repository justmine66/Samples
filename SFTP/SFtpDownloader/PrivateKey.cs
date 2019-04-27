using System.IO;
using System.Reflection;

namespace SFtpDownloader
{
    public static class PrivateKey
    {
        public static Stream Get(string embeddedPrivateKeyFullName)
        {
            var assembly = typeof(PrivateKey).GetTypeInfo().Assembly;
            return assembly.GetManifestResourceStream(embeddedPrivateKeyFullName);
        }
    }
}
