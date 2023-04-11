using System;
using System.Collections.Generic;

namespace ChallengeMod.Utils
{
    internal static class MiscUtils
    {
        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
        {
            key = tuple.Key;
            value = tuple.Value;
        }

        public static T2 GetSafe<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            return default;
        }

        public static Dictionary<T1, T2> CreatePrepopulatedDictionary<T1, T2>(IEnumerable<T1> keys, Func<T2> defaultValue)
        {
            var res = new Dictionary<T1, T2>();

            foreach (var key in keys)
            {
                res.Add(key, defaultValue());
            }

            return res;
        }
    }
}
