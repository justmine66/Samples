using System;

namespace Emit.Samples
{
    public static class StringExtension
    {
        public static string CamelCase(this string name)
        {
            var first = name.AsSpan().Slice(0, 1);
            var rest = name.AsSpan().Slice(1, name.Length - 1);

            return $"{first.ToString().ToLower()}{rest.ToString()}";
        }
    }
}
