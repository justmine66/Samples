using System;
using System.Buffers;
using System.Runtime;

namespace SpanTest
{
    interface IIndexable<T>
    {
        ref T this[int i] { get; }
    }

    public class Indexer
    {
        ref int First<T>(T arg) 
            where T : IIndexable<int>
        {
            // is this safe to return by reference?
            return ref arg[0];
        }
    }

    public class RefSample
    {
        int[] _arr = new int[10];
        void foo(long x) { }
        void foo(int x) { }
        ref int bar() { return ref _arr[0]; }
        void foo() { }

        void TakesTwoRefs(ref Exception s, ref int i)
        {
            GC.Collect();
        }

        ref int Callee(ref int arg)
        {
            return ref arg;
        }

        int Caller()
        {
            int s = 42;

            // DANGER!! returning a reference to the local data
            return Callee(ref s);
        }
    }
}
