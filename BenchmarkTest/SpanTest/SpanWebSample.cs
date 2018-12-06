using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text;

namespace SpanTest
{
    public class SpanWebSample
    {
        private static Encoding _encode = Encoding.UTF8;
        private static ArrayPool<byte> _pool = ArrayPool<byte>.Shared;

        public static void Test()
        {
            string data = "1, 2, 3, 4, 5, 6, 7";
            StringParseSum(data); // 28
            SpanParseSum(data); // 28
            SpanParseSumUsingUtf8Parser(data); // 28
            Utf8ParserWithPooling(data); // 28
        }

        public static int StringParseSum(string data)
        {
            int sum = 0;
            // allocates
            var splitString = data.Split(',');
            for (int i = 0; i < splitString.Length; i++)
            {
                sum += int.Parse(splitString[i]);
            }
            return sum;
        }

        public static int SpanParseSum(string data)
        {
            ReadOnlySpan<char> span = data;
            var sum = 0;
            while (true)
            {
                int index = span.IndexOf(',');
                if (index == -1)
                {
                    sum += int.Parse(span);
                    break;
                }
                sum += int.Parse(span.Slice(0, index));
                span = span.Slice(index + 1); 
            }
            return sum;
        }

        public static int SpanParseSumUsingUtf8Parser(string data)
        {
            // allocates
            Span<byte> utf8 = Encoding.UTF8.GetBytes(data);
            int sum = 0;
            while (true)
            {
                Utf8Parser.TryParse(utf8, out int value,
                    out int bytesConsumed);
                sum += value;
                if (utf8.Length - 1 < bytesConsumed)
                    break;
                // skip ' , '
                utf8 = utf8.Slice(bytesConsumed + 1);
            }

            return sum;
        }

        public static void Utf8ParserWithPooling(string data)
        {
            var minLength = _encode.GetByteCount(data);
            var array = _pool.Rent(minLength);
            Span<byte> utf8 = array;
            var bytesWritten = _encode.GetBytes(data, utf8);
            utf8 = utf8.Slice(0, bytesWritten);
        }
    }
}
