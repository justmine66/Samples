using System;
using Polly;

namespace SFtpDownloader
{
    public class PolicyFactory
    {
        public static Policy CreateRetry(int retryCount, Action<Exception> action)
        {
            var policy = Policy.Handle<Exception>()
                .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromMinutes(Math.Pow(2, retryAttempt)),
                    (ex, time) => action(ex));

            return policy;
        }
    }
}
