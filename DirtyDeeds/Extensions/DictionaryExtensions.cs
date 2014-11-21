using System.Collections.Concurrent;

namespace DD.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            while (!dict.TryAdd(key, value)) { }
        }

        public static void Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            if (!dict.ContainsKey(key))
                return;

            TValue value;
            while (!dict.TryRemove(key, out value)) { }
        }
    }
}
