using System.Collections.Generic;
using System.Collections.ObjectModel;
using IDS.Portable.Common;

namespace ids.portable.common.Collection;

public class SortedCollection<T> : Collection<T>
{
	private readonly IComparer<T>? _comparer;

	public SortedCollection()
	{
		_comparer = (IComparer<T>?)(object)Comparer<T>.Default;
	}

	public SortedCollection(IComparer<T> comparer)
	{
		_comparer = comparer;
	}

	public void Add(T item, out int index)
	{
		index = ((global::System.Collections.Generic.IList<T>)this).BinarySearch(item, _comparer);
		index = ((index >= 0) ? index : (~index));
		base.InsertItem(index, item);
	}

	protected override void InsertItem(int index, T item)
	{
		base.Add(item);
	}

	protected override void SetItem(int index, T item)
	{
		base.RemoveAt(index);
		base.Add(item);
	}
}
