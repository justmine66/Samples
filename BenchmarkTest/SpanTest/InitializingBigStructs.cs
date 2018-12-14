using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace SpanTest
{
    public class InitializingBigStructs
    {
        struct BigStruct
        {
            public int Int1, Int2, Int3, Int4, Int5;
        }

        private BigStruct[] _array;

        [GlobalSetup]
        public void Setup() => _array = new BigStruct[1000];

        [Benchmark]
        public void ByValue()
        {
            var variable = _array;
            for (var i = 0; i < variable.Length; i++)
            {
                var value = variable[i]; // copy the value 1st time

                value.Int1 = 1;
                value.Int2 = 2;
                value.Int3 = 3;
                value.Int4 = 4;
                value.Int5 = 5;

                variable[i] = value; // copy the value 2nd time
            }
        }

        [Benchmark(Baseline = true)]
        public void ByReference()
        {
            var variable = _array;
            for (var i = 0; i < variable.Length; i++)
            {
                ref var reference = ref variable[i]; // create local alias to array storage.

                reference.Int1 = 1;
                reference.Int2 = 2;
                reference.Int3 = 3;
                reference.Int4 = 4;
                reference.Int5 = 5;
            }
        }

        [Benchmark]
        public void ByReferenceOldWay()
        {
            for (var i = 0; i < _array.Length; i++)
            {
                Init(ref _array[i]);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Init(ref BigStruct reference)
        {
            reference.Int1 = 1;
            reference.Int2 = 2;
            reference.Int3 = 3;
            reference.Int4 = 4;
            reference.Int5 = 5;
        }

        [Benchmark]
        public unsafe void ByReferenceUnsafe()
        {
            var variable = _array;
            fixed (BigStruct* pinned = variable)
            {
                for (var i = 0; i < variable.Length; i++)
                {
                    var pointer = &pinned[i];
                    (*pointer).Int1 = 1;
                    (*pointer).Int2 = 2;
                    (*pointer).Int3 = 3;
                    (*pointer).Int4 = 4;
                    (*pointer).Int5 = 5;
                }
            }
        }
    }
}
