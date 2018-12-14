using System;
using System.Collections.Generic;
using System.Text;

namespace SpanTest
{
    public class SpanParsingSample
    {
        public void Old()
        {
            var json = new byte[100];
            var text = Encoding.UTF8.GetString(json);//非常昂贵：复制，转码，对象分配。
            int.TryParse(text, out var value);
        }
    }
}
