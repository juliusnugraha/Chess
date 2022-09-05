using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JN.Utils
{
    public enum SortType
    {
        Ascending,
        Descending
    }

    public enum SortBy
    {
        None,
        Id,
        Name
    }

    public static class Utility
    {

        public static bool Roll(int chance)
        {
            return UnityEngine.Random.Range(0, 100) < chance;
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static T PickRandom<T>(this T[] arr)
        {
            return arr[Random(0, arr.Length)];
        }

        public static bool IsNullOrEmpty<T>(this T[] arr)
        {
            return arr == null ? true : arr.Length == 0;
        }

        public static T2[] ToArray<T1, T2>(this T1[] arr, Func<T1, T2> func)
        {
            List<T2> l = new List<T2>();
            foreach (T1 t in arr)
            {
                l.Add(func(t));
            }
            return l.ToArray();
        }

        public static T SafeGet<T>(this T[] arr, int i)
        {
            if (arr != null)
            {
                if (i < arr.Length)
                {
                    return arr[i];
                }
            }
            return default(T);
        }

        public static TValue SafeGet<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            return dict.ContainsKey(key) ? dict[key] : default(TValue);
        }

        public static T[] CreateArray<T>(T val, int count)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < count; i++)
            {
                list.Add(val);
            }
            return list.ToArray();
        }

        public static List<T> CleanList<T>(List<T> list)
        {
            // remove nul reff
            List<T> listCleaned = new List<T>();
            list.RemoveAll(item => item == null);

            // remove duplicate object
            listCleaned = list.Distinct().ToList();

            return listCleaned;
        }

        public static T ListFindMaxFromIntValue<T>(List<T> list, Converter<T, int> projection) where T : new()
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
            }

            T resultItem = default(T);
            int maxValue = int.MinValue;
            foreach (T item in list)
            {
                int value = projection(item);
                if (value > maxValue)
                {
                    maxValue = value;
                    resultItem = item;
                }
            }

            return resultItem;
        }

        public static int ListFindMaxIntValue<T>(List<T> list, Converter<T, int> projection)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
            }

            int maxValue = int.MinValue;
            foreach (T item in list)
            {
                maxValue = Math.Max(maxValue, projection(item));
            }

            return maxValue;
        }

        public static int ComparerByNameAscending(string name1, string name2)
        {
            if (name1 == null && name2 == null) return 0;
            else if (name1 == null) return -1;
            else if (name2 == null) return 1;
            else return name1.CompareTo(name2);
        }

        public static void SafeInvoke(this Action a)
        {
            if (a != null) { a(); }
        }

        public static void SafeInvoke<T>(this Action<T> a, T val)
        {
            if (a != null) { a(val); }
        }

        public static void SafeInvoke<T1, T2>(this Action<T1, T2> a, T1 val1, T2 val2)
        {
            if (a != null) { a(val1, val2); }
        }

        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> a, T1 val1, T2 val2, T3 val3)
        {
            if (a != null) { a(val1, val2, val3); }
        }
    }
}
