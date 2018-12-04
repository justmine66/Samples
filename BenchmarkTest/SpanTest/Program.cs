using BenchmarkDotNet.Running;
using System;

namespace SpanTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<SpanVsArray_Indexer>();
            //SpanStructTypeSample.Test();
            //SpanSumSample.Test0();
            //SpanSumSample.Test1();
            //SpanGenerateIdSample.Test();
            //SpanRandomSample.Test();
            BenchmarkRunner.Run<SpanGenerateIdSample>();

            Console.Read();
        }
    }
}
