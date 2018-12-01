``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.407 (1803/April2018Update/Redstone4)
Intel Core i5-4210U CPU 1.70GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
  [Host]        : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3221.0
  .NET 4.6      : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3221.0
  .NET Core 2.1 : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT


```
|          Method |           Job |     Toolchain |      Mean |     Error |    StdDev |
|---------------- |-------------- |-------------- |----------:|----------:|----------:|
| SpanIndexer_Get |      .NET 4.6 |         net46 | 1.3166 ns | 0.0473 ns | 0.1349 ns |
| SpanIndexer_Set |      .NET 4.6 |         net46 | 1.3141 ns | 0.0406 ns | 0.1173 ns |
| SpanIndexer_Get | .NET Core 2.1 | .NET Core 2.1 | 0.8213 ns | 0.0164 ns | 0.0454 ns |
| SpanIndexer_Set | .NET Core 2.1 | .NET Core 2.1 | 0.8037 ns | 0.0160 ns | 0.0219 ns |
