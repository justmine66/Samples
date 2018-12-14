using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

namespace SpanTest
{
    public class PassingByReference
    {
        struct BigStruct
        {
            public int Int1, Int2, Int3, Int4, Int5;
        }

        private BigStruct _field = new BigStruct();

        [Benchmark(OperationsPerInvoke = 16, Baseline = true)]
        public void PassByValue()
        {
            // access the field only once to not influence the benchmark too much.
            var copy = _field; 
            Method(copy); Method(copy); Method(copy); Method(copy);
            Method(copy); Method(copy); Method(copy); Method(copy);
            Method(copy); Method(copy); Method(copy); Method(copy);
            Method(copy); Method(copy); Method(copy); Method(copy);
        }

        [Benchmark(OperationsPerInvoke = 16)]
        public void PassByReference()
        {
            // access the field only once to not influence the benchmark too much
            ref var local = ref _field; 
            Method(ref local); Method(ref local); Method(ref local); Method(ref local);
            Method(ref local); Method(ref local); Method(ref local); Method(ref local);
            Method(ref local); Method(ref local); Method(ref local); Method(ref local);
            Method(ref local); Method(ref local); Method(ref local); Method(ref local);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void Method(BigStruct value) { }

        [MethodImpl(MethodImplOptions.NoInlining)]
        void Method(ref BigStruct value) { }
    }
}
