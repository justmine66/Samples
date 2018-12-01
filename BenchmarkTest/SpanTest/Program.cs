using BenchmarkDotNet.Running;
using System;

namespace SpanTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SpanVsArray_Indexer>();

            Console.Read();
        }
    }
}
