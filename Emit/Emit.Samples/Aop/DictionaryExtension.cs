using System.Collections.Generic;

namespace Emit.Samples.Aop
{
    public static class DictionaryExtension
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> keyValuePairs, TKey key, TValue value = default)
        {
            if (keyValuePairs.ContainsKey(key))
                keyValuePairs[key] = value;
            else
                keyValuePairs.Add(key, value);
        }
    }
}
