using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SpanTest
{
    public class MemorySample
    {
        private static MemoryPool<byte> _pool = MemoryPool<byte>.Shared;

        public void Worker(Memory<byte> buffer)
        {
            var str = new Memory<char>();
        }

        static async Task<uint> ChecksumReadAsync(Memory<byte> buffer, Stream stream)
        {
            var bytesRead = await stream.ReadAsync(buffer);
            return SafeSum(buffer.Span.Slice(0, bytesRead));
        }

        static uint SafeSum(Span<byte> buffer)
        {
            uint sum = 0;
            foreach (var t in buffer)
            {
                sum += t;
            }

            return sum;
        }
    }
}
