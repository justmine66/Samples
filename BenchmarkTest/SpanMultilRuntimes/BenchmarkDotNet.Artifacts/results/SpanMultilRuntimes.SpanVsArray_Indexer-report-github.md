``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.407 (1803/April2018Update/Redstone4)
Intel Core i5-4210U CPU 1.70GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
  [Host]        : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3221.0
  .NET 4.6      : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3221.0
  .NET Core 2.1 : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT


```
|           Method |           Job |     Toolchain |      Mean |     Error |    StdDev |    Median |
|----------------- |-------------- |-------------- |----------:|----------:|----------:|----------:|
| ArrayIndexer_Get |      .NET 4.6 |         net46 | 0.8786 ns | 0.0201 ns | 0.0592 ns | 0.8744 ns |
| ArrayIndexer_Set |      .NET 4.6 |         net46 | 0.8625 ns | 0.0268 ns | 0.0791 ns | 0.8361 ns |
|  SpanIndexer_Get |      .NET 4.6 |         net46 | 1.1542 ns | 0.0242 ns | 0.0537 ns | 1.1293 ns |
|  SpanIndexer_Set |      .NET 4.6 |         net46 | 1.1392 ns | 0.0286 ns | 0.0372 ns | 1.1273 ns |
| ArrayIndexer_Get | .NET Core 2.1 | .NET Core 2.1 | 0.7860 ns | 0.0056 ns | 0.0050 ns | 0.7854 ns |
| ArrayIndexer_Set | .NET Core 2.1 | .NET Core 2.1 | 0.7953 ns | 0.0089 ns | 0.0079 ns | 0.7955 ns |
|  SpanIndexer_Get | .NET Core 2.1 | .NET Core 2.1 | 0.7811 ns | 0.0231 ns | 0.0308 ns | 0.7698 ns |
|  SpanIndexer_Set | .NET Core 2.1 | .NET Core 2.1 | 0.7954 ns | 0.0090 ns | 0.0084 ns | 0.7933 ns |
