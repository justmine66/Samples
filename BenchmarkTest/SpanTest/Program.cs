using BenchmarkDotNet.Running;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SpanTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //BenchmarkRunner.Run<SpanVsArray_Indexer>();
            //BenchmarkRunner.Run<SpanGenerateIdSample>();

            //SpanStructTypeSample.Test();
            //SpanSumSample.Test0();
            //SpanSumSample.Test1();
            //SpanGenerateIdSample.Test();
            //SpanRandomSample.Test();
            //SpanWebSample.Test();

            byte[] buffer = { 1, 2, 3 };
            await SpanAsyncCallSample.DoSomethingAsync(buffer);

            Console.Read();
        }
    }
}
