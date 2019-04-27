using System;

namespace SFtpDownloader.Test
{
public class FileNamingStrategy : IFileNamingStrategy
{
    public string GetFileRegexName()
    {
        return $"test.csv";
    }
}
}
