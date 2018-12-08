using System;
using System.Buffers;

namespace SpanTest
{
    public class MemorySample
    {
        private static MemoryPool<byte> _pool =
        MemoryPool<byte>.Shared;

        public void Worker(Memory<byte> buffer)
        {

        }

        public void UserCode()
        {
            using (IMemoryOwner<byte> buffer = _pool.Rent(1024))
            {
                Worker(buffer.Memory);
            }
        }
    }
}
