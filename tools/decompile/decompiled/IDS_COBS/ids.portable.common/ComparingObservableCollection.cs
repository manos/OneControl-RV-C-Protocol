using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace IDS.Portable.Common;

public class ComparingObservableCollection<T> : BaseObservableCollection<T>
{
	protected readonly bool OrderAscending = true;

	private readonly IComparer<T>? _comparer;

	protected bool UseComparer
	{
		get
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			if (_comparer == null && !IntrospectionExtensions.GetTypeInfo(typeof(IComparable<T>)).IsAssignableFrom(IntrospectionExtensions.GetTypeInfo(typeof(T))))
			{
				throw new ArgumentException("IComparer<T> is null and T is not IComparable");
			}
			return _comparer != null;
		}
	}

	public ComparingObservableCollection(Func<T, T, int> comparerFunc, bool orderAscending = true)
		: this((IComparer<T>?)(object)new CustomCompare<T>(comparerFunc), orderAscending)
	{
	}

	public ComparingObservableCollection(bool orderAscending = true)
		: this((IComparer<T>?)null, orderAscending)
	{
	}

	public ComparingObservableCollection(IComparer<T>? comparer, bool orderAscending = true)
	{
		_comparer = comparer;
		OrderAscending = orderAscending;
	}

	public ComparingObservableCollection(global::System.Collections.Generic.IEnumerable<T> items, IComparer<T> comparer, bool orderAscending = true)
	{
		_comparer = comparer;
		OrderAscending = orderAscending;
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

	protected virtual int CompareItems(T item1, T item2)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		return ((!UseComparer) ? new int?(((IComparable<T>)(object)item1).CompareTo(item2)) : _comparer?.Compare(item1, item2)) ?? throw new ArgumentException("Null argument not supported.");
	}

	protected override void InsertItem(int index, T item)
	{
		((ObservableCollection<T>)this).InsertItem(OrderAscending ? OrderByAscending(item) : OrderByDescending(item), item);
	}

	protected override void SetItem(int index, T item)
	{
		if (CompareItems(((Collection<T>)(object)this)[index], item) == 0)
		{
			((ObservableCollection<T>)this).SetItem(index, item);
			return;
		}
		((Collection<T>)(object)this).RemoveAt(index);
		((Collection<T>)(object)this).InsertItem(index, item);
	}

	protected override void MoveItem(int oldIndex, int newIndex)
	{
		if (CompareItems(((Collection<T>)(object)this)[oldIndex], ((Collection<T>)(object)this)[newIndex]) == 0)
		{
			((ObservableCollection<T>)this).MoveItem(oldIndex, newIndex);
		}
	}

	private int OrderByAscending(T item)
	{
		int num = 0;
		int num2 = ((Collection<T>)(object)this).Count - 1;
		while (num <= num2)
		{
			int num3 = (num + num2) / 2;
			if (CompareItems(item, ((Collection<T>)(object)this)[num3]) < 0)
			{
				num2 = num3 - 1;
			}
			else
			{
				num = num3 + 1;
			}
		}
		return num;
	}

	private int OrderByDescending(T item)
	{
		int num = 0;
		int num2 = ((Collection<T>)(object)this).Count - 1;
		while (num <= num2)
		{
			int num3 = (num + num2) / 2;
			if (CompareItems(item, ((Collection<T>)(object)this)[num3]) > 0)
			{
				num2 = num3 - 1;
			}
			else
			{
				num = num3 + 1;
			}
		}
		return num;
	}

	public void Sort()
	{
		List<T> val = (UseComparer ? Enumerable.ToList<T>((global::System.Collections.Generic.IEnumerable<T>)(OrderAscending ? Enumerable.OrderBy<T, T>((global::System.Collections.Generic.IEnumerable<T>)this, (Func<T, T>)((T x) => x), _comparer) : Enumerable.OrderByDescending<T, T>((global::System.Collections.Generic.IEnumerable<T>)this, (Func<T, T>)((T x) => x), _comparer))) : Enumerable.ToList<T>((global::System.Collections.Generic.IEnumerable<T>)(OrderAscending ? Enumerable.OrderBy<T, T>((global::System.Collections.Generic.IEnumerable<T>)this, (Func<T, T>)((T x) => x)) : Enumerable.OrderByDescending<T, T>((global::System.Collections.Generic.IEnumerable<T>)this, (Func<T, T>)((T x) => x)))));
		for (int num = 0; num < val.Count; num++)
		{
			int num2 = ((Collection<T>)(object)this).IndexOf(val[num]);
			int num3 = num;
			if (num2 != num3)
			{
				T val2 = ((Collection<T>)(object)this)[num2];
				((Collection<T>)(object)this).RemoveAt(num2);
				((ObservableCollection<T>)this).InsertItem(num3, val2);
			}
		}
	}
}
