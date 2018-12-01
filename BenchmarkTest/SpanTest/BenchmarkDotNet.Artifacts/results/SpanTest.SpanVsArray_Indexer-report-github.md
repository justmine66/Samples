``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.407 (1803/April2018Update/Redstone4)
Intel Core i5-4210U CPU 1.70GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=2.1.500
  [Host]     : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT


```
|           Method |      Mean |     Error |    StdDev |
|----------------- |----------:|----------:|----------:|
| ArrayIndexer_Get | 0.9434 ns | 0.0242 ns | 0.0705 ns |
| ArrayIndexer_Set | 0.9541 ns | 0.0232 ns | 0.0685 ns |
|  SpanIndexer_Get | 0.9098 ns | 0.0356 ns | 0.1050 ns |
|  SpanIndexer_Set | 0.9434 ns | 0.0238 ns | 0.0700 ns |
