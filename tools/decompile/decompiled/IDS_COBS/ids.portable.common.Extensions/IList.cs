using System;
using System.Collections.Generic;
using System.Linq;

namespace IDS.Portable.Common.Extensions;

public static class IList
{
	public static TItem[] Slice<TItem>(this TItem[] instance, int startIndex)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (startIndex < 0 || startIndex >= instance.Length)
		{
			throw new ArgumentOutOfRangeException("startIndex", "startIndex is less than zero or greater than the length of this instance");
		}
		return instance.Slice(startIndex, instance.Length - startIndex);
	}

	public static TItem[] Slice<TItem>(this TItem[] instance, int startIndex, int length)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (startIndex + length > instance.Length)
		{
			throw new ArgumentException("startIndex plus length indicates a position not within this instance");
		}
		if (startIndex < 0 || length < 0)
		{
			throw new ArgumentException("startIndex or length is less than zero");
		}
		TItem[] array = new TItem[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = instance[i + startIndex];
		}
		return array;
	}

	public static bool EqualsAll<T>(this global::System.Collections.Generic.IList<T>? a, global::System.Collections.Generic.IList<T>? b)
	{
		if (a == null || b == null)
		{
			if (a == null)
			{
				return b == null;
			}
			return false;
		}
		if (((global::System.Collections.Generic.ICollection<T>)a).Count == ((global::System.Collections.Generic.ICollection<T>)b).Count)
		{
			return Enumerable.All<T>((global::System.Collections.Generic.IEnumerable<T>)a, (Func<T, bool>)((global::System.Collections.Generic.ICollection<T>)b).Contains);
		}
		return false;
	}

	public static bool TryTakeFirst<T>(this global::System.Collections.Generic.IList<T> list, out T? item)
	{
		if (((global::System.Collections.Generic.ICollection<T>)list).Count == 0)
		{
			item = default(T);
			return false;
		}
		item = list[0];
		list.RemoveAt(0);
		return true;
	}
}
