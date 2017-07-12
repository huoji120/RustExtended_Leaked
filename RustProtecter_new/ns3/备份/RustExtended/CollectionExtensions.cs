namespace RustExtended
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class CollectionExtensions
    {
        public static T[] Add<T>(this T[] array, T item)
        {
            return (array ?? Enumerable.Empty<T>()).Concat<T>(new T[] { item }).ToArray<T>();
        }

        public static T[] AddRange<T>(this T[] array, T[] items)
        {
            return (array ?? Enumerable.Empty<T>()).Concat<T>(items).ToArray<T>();
        }

        public static T[] Remove<T>(this T[] array, T item)
        {
            int index = Array.IndexOf<T>(array, item);
            if (index == -1)
            {
                return array;
            }
            return array.RemoveAt<T>(index);
        }

        public static T[] RemoveAt<T>(this T[] array, int index)
        {
            T[] destinationArray = new T[array.Length - 1];
            if (index > 0)
            {
                Array.Copy(array, 0, destinationArray, 0, index);
            }
            if (index < (array.Length - 1))
            {
                Array.Copy(array, index + 1, destinationArray, index, (array.Length - index) - 1);
            }
            return destinationArray;
        }
    }
}

