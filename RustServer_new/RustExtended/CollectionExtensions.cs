using System;
using System.Collections.Generic;
using System.Linq;

namespace RustExtended
{
	public static class CollectionExtensions
	{
		public static T[] Add<T>(this T[] array, T item)
		{
			return ((array != null) ? ((IEnumerable<T>)array) : Enumerable.Empty<T>()).Concat(new T[]
			{
				item
			}).ToArray<T>();
		}

		public static T[] AddRange<T>(this T[] array, T[] items)
		{
			return ((array != null) ? ((IEnumerable<T>)array) : Enumerable.Empty<T>()).Concat(items).ToArray<T>();
		}

		public static T[] Remove<T>(this T[] array, T item)
		{
			int num = Array.IndexOf<T>(array, item);
			T[] result;
			if (num == -1)
			{
				result = array;
			}
			else
			{
				result = array.RemoveAt(num);
			}
			return result;
		}

		public static T[] RemoveAt<T>(this T[] array, int index)
		{
			T[] array2 = new T[array.Length - 1];
			if (index > 0)
			{
				Array.Copy(array, 0, array2, 0, index);
			}
			if (index < array.Length - 1)
			{
				Array.Copy(array, index + 1, array2, index, array.Length - index - 1);
			}
			return array2;
		}
	}
}
