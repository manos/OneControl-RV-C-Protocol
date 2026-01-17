using System.Collections.Generic;
using System.Collections.ObjectModel;
using IDS.Portable.Common;

namespace ids.portable.common.ObservableCollection;

public class SortedObservableCollection<T> : ObservableCollection<T>
{
	private readonly IComparer<T>? _comparer;

	public SortedObservableCollection()
	{
		_comparer = (IComparer<T>?)(object)Comparer<T>.Default;
	}

	public SortedObservableCollection(IComparer<T> comparer)
	{
		_comparer = comparer;
	}

	protected override void InsertItem(int index, T item)
	{
		index = ((global::System.Collections.Generic.IList<T>)this).BinarySearch(item, _comparer);
		base.InsertItem((index >= 0) ? index : (~index), item);
	}

	protected override void SetItem(int index, T item)
	{
		((Collection<T>)(object)this).RemoveAt(index);
		((Collection<T>)(object)this).Add(item);
	}

	protected override void MoveItem(int oldIndex, int newIndex)
	{
		T val = ((Collection<T>)(object)this)[oldIndex];
		((Collection<T>)(object)this).RemoveAt(oldIndex);
		((Collection<T>)(object)this).Add(val);
	}
}
