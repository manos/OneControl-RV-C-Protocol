using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace IDS.Portable.Common;

public static class DictionaryExtension
{
	public static void TryRemove<TKey, TItem>(this IDictionary<TKey, TItem> dictionary, TKey key) where TKey : notnull
	{
		try
		{
			if (dictionary.ContainsKey(key))
			{
				dictionary.Remove(key);
			}
		}
		catch
		{
		}
	}

	public static TItem TryGetValue<TKey, TItem>(this IReadOnlyDictionary<TKey, TItem> dictionary, TKey key) where TKey : notnull
	{
		TItem result = default(TItem);
		dictionary.TryGetValue(key, ref result);
		return result;
	}

	public static TItem TryGetValue<TKey, TItem>(this ConcurrentDictionary<TKey, TItem> dictionary, TKey key) where TKey : notnull
	{
		TItem result = default(TItem);
		dictionary.TryGetValue(key, ref result);
		return result;
	}

	public static TItem TryGetWithCustomDefaultValue<TKey, TItem>(this IReadOnlyDictionary<TKey, TItem> dictionary, TKey key, TItem defaultValue) where TKey : notnull
	{
		TItem result = default(TItem);
		if (!dictionary.TryGetValue(key, ref result))
		{
			return defaultValue;
		}
		return result;
	}

	public static TItem TryGetWithCustomDefaultValue<TKey, TItem>(this ConcurrentDictionary<TKey, TItem> dictionary, TKey key, TItem defaultValue) where TKey : notnull
	{
		TItem result = default(TItem);
		if (!dictionary.TryGetValue(key, ref result))
		{
			return defaultValue;
		}
		return result;
	}

	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> sourceDict)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<TKey, TValue> val = new Dictionary<TKey, TValue>();
		global::System.Collections.Generic.IEnumerator<KeyValuePair<TKey, TValue>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<TKey, TValue>>)sourceDict).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				KeyValuePair<TKey, TValue> current = enumerator.Current;
				val.Add(current.Key, current.Value);
			}
			return val;
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> source, IReadOnlyDictionary<TKey, TValue> collection)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (source == null || collection == null || ((global::System.Collections.Generic.IReadOnlyCollection<KeyValuePair<TKey, TValue>>)collection).Count <= 0)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<KeyValuePair<TKey, TValue>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<TKey, TValue>>)collection).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				KeyValuePair<TKey, TValue> current = enumerator.Current;
				source[current.Key] = current.Value;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}
}
