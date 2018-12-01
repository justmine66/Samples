using BenchmarkDotNet.Running;
using System;

namespace SpanMultilRuntimes
{
    class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<SpanIndexer>();
            BenchmarkRunner.Run<SpanVsArray_Indexer>();

            Console.Read();
        }
    }
}
