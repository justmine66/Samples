using System;
using System.Buffers;
using System.Threading.Tasks;

namespace SpanTest
{
    public class SpanAsyncCallSample
    {
        private static MemoryPool<byte> _memPool =
        MemoryPool<byte>.Shared;

        public async Task UsageWithLifeAsync(int size)
        {
            using (var array = _memPool.Rent(size))
            {
                await DoSomethingAsync(array.Memory);
            }
        }

        public static async Task DoSomethingAsync(Memory<byte> buffer)
        {
            buffer.Span[0] = 0;
            await Something();
            buffer.Span[0] = 1;
        }

        static private Task Something()
        {
            return Task.CompletedTask;
        }
    }
}
