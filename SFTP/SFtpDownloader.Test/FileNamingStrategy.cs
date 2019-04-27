using System;

namespace SFtpDownloader.Test
{
public class FileNamingStrategy : IFileNamingStrategy
{
    public string GetReportRegexName()
    {
        return $"test.csv";
    }
}
}
