using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IDS.Portable.Common;

public class OrderedObservableCollection<T> : ComparingObservableCollection<T> where T : IComparable<T>
{
	public OrderedObservableCollection()
		: base(orderAscending: true)
	{
	}

	public OrderedObservableCollection(bool orderAscending)
		: base(orderAscending)
	{
	}

	public OrderedObservableCollection(global::System.Collections.Generic.IEnumerable<T> items, bool orderAscending = true)
		: base(orderAscending)
	{
		using (SuppressEvents(forceRefresh: true))
		{
			global::System.Collections.Generic.IEnumerator<T> enumerator = items.GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					T current = enumerator.Current;
					((Collection<T>)(object)this).Add(current);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
		}
	}

	protected override int CompareItems(T item1, T item2)
	{
		return ((IComparable<T>)item1/*cast due to .constrained prefix*/).CompareTo(item2);
	}
}
