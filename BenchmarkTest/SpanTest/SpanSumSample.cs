using System;
using System.Runtime.InteropServices;

namespace SpanTest
{
    public class SpanSumSample
    {
        public static void Test0()
        {
            // managed memory
            var arrayMemory = new byte[100];
            var result = SafeSum(arrayMemory);
            Console.WriteLine("managed memory: " + result);
        }

        public static void Test1()
        {
            // native memory
            var nativeMemory = Marshal.AllocHGlobal(100);

            try
            {
                Span<byte> nativeSpan;
                unsafe
                {
                    nativeSpan = new Span<byte>(nativeMemory.ToPointer(), 100);
                }
                var result = SafeSum(nativeSpan);
                Console.WriteLine("native memory: " + result);
            }
            finally
            {
                Marshal.FreeHGlobal(nativeMemory);
            }
        }

        // this method does not care what kind of memory it works on
        static ulong SafeSum(Span<byte> bytes)
        {
            ulong sum = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                sum += bytes[i];
            }
            return sum;
        }
    }
}
