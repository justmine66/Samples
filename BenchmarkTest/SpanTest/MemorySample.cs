using System;
using System.Buffers;

namespace SpanTest
{
    public class MemorySample
    {
        private static MemoryPool<byte> _memPool =
        MemoryPool<byte>.Shared;
        private static ArrayPool<byte> _arrPool = ArrayPool<byte>.Shared;

        public void Worker(Memory<byte> buffer)
        {

        }

        public void Usage(int size)
        {
            var array = _arrPool.Rent(size);
            Memory<byte> buffer = array;
            DoSomething(buffer);
            _arrPool.Return(array);
        }

        public void UsageWithLife(int size)
        {
            using (var array = _memPool.Rent(size))
            {
                DoSomething(array.Memory);
            }
        }

        public void DoSomething<T>(Memory<T> memory)
        {

        }
    }
}
