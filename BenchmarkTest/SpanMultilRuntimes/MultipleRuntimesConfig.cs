using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;

namespace SpanMultilRuntimes
{
    public class MultipleRuntimesConfig : ManualConfig
    {
        public MultipleRuntimesConfig()
        {
            // Span NOT supported by Runtime
            Add(Job.Default
                    .With(CsProjClassicNetToolchain.Net46) 
                    .WithId(".NET 4.6"));

            // Span SUPPORTED by Runtime
            Add(Job.Default
                    .With(CsProjCoreToolchain.NetCoreApp21) 
                    .WithId(".NET Core 2.1"));
        }
    }
}
