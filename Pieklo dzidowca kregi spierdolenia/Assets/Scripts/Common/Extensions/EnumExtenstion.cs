using System;

namespace jbzd.Common.Extensions
{
    public static class EnumExtenstion
    {
        /// <summary>
        /// Get next item in enum by index. If it's last enum it will start over.
        /// </summary>
        public static T Next<T>(this T src) where T : Enum
        {
            var arr = (T[])Enum.GetValues(src.GetType());
            var j = Array.IndexOf(arr, src) + 1;
            return (arr.Length==j) ? arr[0] : arr[j];
        }
        
        /// <summary>
        /// Get previous item in enum by index. If it was first enum it will start return last.
        /// </summary>
        public static T Previous<T>(this T src) where T : Enum
        {
            var arr = (T[])Enum.GetValues(src.GetType());
            var j = Array.IndexOf(arr, src) - 1;
            return j < 0 ? arr[^1] : arr[j];
        }
        
        /// <summary>
        /// Checks if enum value is last in type.
        /// </summary>
        public static bool IsLastValueFromEnum<T>(this T src) where T : Enum
        {
            var arr = (T[])Enum.GetValues(src.GetType());
            var j = Array.IndexOf(arr, src) + 1;
            return j == arr.Length;
        }
    }
}