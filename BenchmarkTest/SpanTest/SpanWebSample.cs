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
            string data = "1, 2, 3, 4, 5, 6, 7, 8, 9, 10";
            var result0 = StringParseSum(data);
            var result1 = SpanParseSum(data);
            var result2 = SpanParseSumUsingUtf8Parser(data);
            var result3 = Utf8ParserWithPooling(data);

            Console.WriteLine(result0);
            Console.WriteLine(result1);
            Console.WriteLine(result2);
            Console.WriteLine(result3);
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
                var index = span.IndexOf(',');
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
                if (Utf8Parser.TryParse(utf8, out int value,
                    out int bytesConsumed))
                {
                    sum += value;
                }

                if (utf8.Length - 1 < bytesConsumed)
                    break;
                // skip ' , '
                utf8 = utf8.Slice(bytesConsumed + 1);
            }

            return sum;
        }

        public static int Utf8ParserWithPooling(string data)
        {
            var minLength = _encode.GetByteCount(data);
            var array = _pool.Rent(minLength);
            Span<byte> utf8 = array;
            var bytesWritten = _encode.GetBytes(data, utf8);
            utf8 = utf8.Slice(0, bytesWritten);

            var sum = 0;
            while (true)
            {
                if (Utf8Parser.TryParse(utf8, out int value,
                    out int bytesConsumed))
                {
                    sum += value;
                }

                if (utf8.Length - 1 < bytesConsumed)
                    break;

                utf8 = utf8.Slice(bytesConsumed + 1);
            }

            _pool.Return(array);

            return sum;
        }
    }
}
