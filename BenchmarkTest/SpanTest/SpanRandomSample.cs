using System;
using System.Collections.Generic;
using System.Text;

namespace SpanTest
{
    public class SpanRandomSample
    {
        public static void Test()
        {
            var rnd = new Random();
            var length = 10;
            var b = new byte[length];
            rnd.NextBytes(b);
            for (var i = 0; i < length; i++)
                Console.WriteLine("{0}: {1}", i, b[i]);
        }
    }
}
