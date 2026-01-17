using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ids.portable.common.Collection;

public class ConcurrentHashSet<T> : global::System.Collections.Generic.ICollection<T>, global::System.Collections.Generic.IEnumerable<T>, global::System.Collections.IEnumerable
{
	private readonly ConcurrentDictionary<T, byte> _collection = new ConcurrentDictionary<T, byte>();

	public int Count => _collection.Count;

	public bool IsReadOnly => false;

	public bool Contains(T item)
	{
		return _collection.ContainsKey(item);
	}

	public void Clear()
	{
		_collection.Clear();
	}

	public void Add(T item)
	{
		_collection.AddOrUpdate(item, (byte)0, (Func<T, byte, byte>)((T k, byte v) => 0));
	}

	public bool Remove(T item)
	{
		byte b = default(byte);
		return _collection.TryRemove(item, ref b);
	}

	global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
	{
		return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<T>)_collection.Keys).GetEnumerator();
	}

	public global::System.Collections.Generic.IEnumerator<T> GetEnumerator()
	{
		return ((global::System.Collections.Generic.IEnumerable<T>)_collection.Keys).GetEnumerator();
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		_collection.Keys.CopyTo(array, arrayIndex);
	}
}
