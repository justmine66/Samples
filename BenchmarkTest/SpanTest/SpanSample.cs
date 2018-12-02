using System;

namespace SpanTest
{
    public class SpanSample
    {
        public static void Old()
        {
            var input = "123,456";
            var commaPos = input.IndexOf(',');
            var first = int.Parse(input.Substring(0, commaPos));
            var second = int.Parse(input.Substring(commaPos + 1));
        }

        public static void New()
        {
            var input = "123,456";
            var inputSpan = input.AsSpan();
            var commaPos = input.IndexOf(',');
            var first = int.Parse(inputSpan.Slice(0, commaPos));
            var second = int.Parse(inputSpan.Slice(commaPos + 1));
        }
    }
}
