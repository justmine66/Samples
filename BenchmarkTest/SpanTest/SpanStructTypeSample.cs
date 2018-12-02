using System;
using System.Collections;
using System.Collections.Generic;

namespace SpanTest
{
    class SpanStructTypeSample
    {
        public static void Test()
        {
            var value = new StructType<int>();
            Parse(value);
        }

        static void Parse(IEnumerable<int> collection) { }
    }

    struct StructType<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
