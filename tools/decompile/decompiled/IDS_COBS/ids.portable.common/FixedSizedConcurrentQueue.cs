using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

public class FixedSizedConcurrentQueue<TValue> : IProducerConsumerCollection<TValue>, global::System.Collections.Generic.IEnumerable<TValue>, global::System.Collections.IEnumerable, global::System.Collections.ICollection, global::System.Collections.Generic.IReadOnlyCollection<TValue>
{
	private readonly ConcurrentQueue<TValue> _queue;

	private readonly object _syncObject = new object();

	public int LimitSize
	{
		[CompilerGenerated]
		get;
	}

	public int Count => _queue.Count;

	bool global::System.Collections.ICollection.IsSynchronized => ((global::System.Collections.ICollection)_queue).IsSynchronized;

	object global::System.Collections.ICollection.SyncRoot => ((global::System.Collections.ICollection)_queue).SyncRoot;

	public bool IsEmpty => _queue.IsEmpty;

	public FixedSizedConcurrentQueue(int limit)
	{
		_queue = new ConcurrentQueue<TValue>();
		LimitSize = limit;
	}

	public FixedSizedConcurrentQueue(int limit, global::System.Collections.Generic.IEnumerable<TValue> collection)
	{
		_queue = new ConcurrentQueue<TValue>(collection);
		LimitSize = limit;
	}

	public void CopyTo(TValue[] array, int index)
	{
		_queue.CopyTo(array, index);
	}

	void global::System.Collections.ICollection.CopyTo(global::System.Array array, int index)
	{
		((global::System.Collections.ICollection)_queue).CopyTo(array, index);
	}

	public void Enqueue(TValue obj)
	{
		_queue.Enqueue(obj);
		lock (_syncObject)
		{
			TValue val = default(TValue);
			while (_queue.Count > LimitSize)
			{
				_queue.TryDequeue(ref val);
			}
		}
	}

	public global::System.Collections.Generic.IEnumerator<TValue> GetEnumerator()
	{
		return _queue.GetEnumerator();
	}

	global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
	{
		return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<TValue>)this).GetEnumerator();
	}

	public TValue[] ToArray()
	{
		return _queue.ToArray();
	}

	public bool TryAdd(TValue item)
	{
		Enqueue(item);
		return true;
	}

	bool IProducerConsumerCollection<TValue>.TryTake(out TValue item)
	{
		return TryDequeue(out item);
	}

	public bool TryDequeue(out TValue result)
	{
		return _queue.TryDequeue(ref result);
	}

	public bool TryPeek(out TValue result)
	{
		return _queue.TryPeek(ref result);
	}
}
