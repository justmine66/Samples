using Microsoft.IO;

namespace SpanTest
{
    public class RecyclableMemoryStreamSample
    {
        private static readonly RecyclableMemoryStreamManager Pool = new RecyclableMemoryStreamManager();

        public static void Test()
        {
            var sourceBuffer = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            using (var stream = Pool.GetStream(nameof(RecyclableMemoryStreamSample.Test)))
            {
                stream.Write(sourceBuffer, 0, sourceBuffer.Length);
            }
        }
    }
}
