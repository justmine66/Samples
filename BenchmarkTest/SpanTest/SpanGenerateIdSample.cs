using BenchmarkDotNet.Attributes;
using System;

namespace SpanTest
{
    [MemoryDiagnoser]
    public class SpanGenerateIdSample
    {
        [Benchmark(OperationsPerInvoke = 100000)]
        [Arguments(32)]
        public string GenerateIdWithManagedMemory(int length)
        {
            var rand = new Random();
            var chars = new char[length];//从堆上分配。
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)(rand.Next(0, 10) + '0');
            }

            return new string(chars);
        }

        [Benchmark(OperationsPerInvoke = 100000)]
        [Arguments(32)]
        public string GenerateIdWithStackMemory(int length)
        {
            var rand = new Random();
            Span<char> chars = stackalloc char[length];//长度(128)较短时，从栈分配。
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)(rand.Next(0, 10) + '0');
            }

            //将堆栈上生成的数据复制到字符串中
            return new string(chars);
        }

        [Benchmark(OperationsPerInvoke = 100000)]
        [Arguments(32)]
        public string GenerateIdNoAllocaingAndZeroCoping(int length)
        {
            var rand = new Random();

            return string.Create(length, rand, (charts, Random) =>
            {
                for (int i = 0; i < charts.Length; i++)
                {
                    charts[i] = (char)(rand.Next(0, 26) + 'a');
                }
            });
        }
    }
}
