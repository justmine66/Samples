﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using System;
using System.Linq;

namespace SpanTest
{
    [HardwareCounters(HardwareCounter.CacheMisses)]
    [RyuJitX64Job, LegacyJitX86Job]
    public class DataLocalitySample
    {
        // for smaller arrays we don't get enough of Cache Miss events.
        [Params(
        100,
        1000000,
        10000000,
        100000000)]
        public int Count { get; set; }

        Tuple<int, int>[] arrayOfRef;
        ValueTuple<int, int>[] arrayOfVal;

        [GlobalSetup]
        public void Setup()
        {
            arrayOfRef = Enumerable.Repeat(1, Count).Select((val, index) => Tuple.Create(val, index)).ToArray();
            arrayOfVal = Enumerable.Repeat(1, Count).Select((val, index) => new ValueTuple<int, int>(val, index)).ToArray();
        }

        [Benchmark(Baseline = true)]
        public int IterateValueTypes()
        {
            int item1Sum = 0, item2Sum = 0;

            var array = arrayOfVal;
            for (var i = 0; i < array.Length; i++)
            {
                ref ValueTuple<int, int> reference = ref array[i];// 避免内存复制
                item1Sum += reference.Item1;
                item2Sum += reference.Item2;
            }

            return item1Sum + item2Sum;
        }

        [Benchmark]
        public int IterateReferenceTypes()
        {
            int item1Sum = 0, item2Sum = 0;

            var array = arrayOfRef;
            for (var i = 0; i < array.Length; i++)
            {
                ref Tuple<int, int> reference = ref array[i];
                item1Sum += reference.Item1;
                item2Sum += reference.Item2;
            }

            return item1Sum + item2Sum;
        }
    }
}
