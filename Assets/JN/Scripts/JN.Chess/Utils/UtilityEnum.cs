using System;
using System.Collections.Generic;

namespace JN.Utils
{
    public static class UtilityEnum
    {

        public static T[] GetEnumValues<T>()
        {
            return (T[])System.Enum.GetValues(typeof(T));
        }

        public static T GetRandomEnum<T>()
        {
            return Utility.PickRandom(UtilityEnum.GetEnumValues<T>());
        }

        public static T GetNextEnum<T>(T t) where T : struct
        {
            if (!typeof(T).IsEnum) { throw new NotSupportedException(); }
            List<T> v = new List<T>(UtilityEnum.GetEnumValues<T>());
            int i = v.IndexOf(t);
            return v[(i + 1) % v.Count];
        }

        public static int GetEnumCount<T>()
        {
            return System.Enum.GetValues(typeof(T)).Length;
        }

        //	public static T ToEnum<T>(this string s){
        //		return (T) System.Enum.Parse(typeof(T), s);
        //	}

        public static T ToEnum<T>(this string val)
        {
            return ToEnum<T>(val, default(T));
        }

        public static T ToEnum<T>(this string val, T defaultVal)
        {
            if (string.IsNullOrEmpty(val)) return defaultVal;

            foreach (T t in Enum.GetValues(typeof(T)))
            {
                if (t.ToString().ToLower().Equals(val.ToLower()))
                    return t;
            }

            return defaultVal;
        }
    }
}

