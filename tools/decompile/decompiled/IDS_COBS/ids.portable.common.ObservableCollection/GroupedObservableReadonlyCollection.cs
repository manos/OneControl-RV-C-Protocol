using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using IDS.Portable.Common.ObservableCollection;
using ids.portable.common.Collection;

namespace ids.portable.common.ObservableCollection;

public class GroupedObservableReadonlyCollection<TCollection, TGroupHeader, TItem> : ObservableReadOnlyCollection<ObservableCollection<object>, object> where TCollection : class, global::System.Collections.Generic.IEnumerable<TItem>, INotifyCollectionChanged where TGroupHeader : class, IGroupHeader, IComparable<TGroupHeader> where TItem : class, IComparable<TItem>
{
	private readonly IGroupHeaderFactory<TGroupHeader, TItem> _groupHeaderFactory;

	private readonly IItemDividerFactory? _itemDividerFactory;

	private readonly IGroupFooterFactory? _groupFooterFactory;

	private readonly global::System.Collections.IEnumerable _items;

	private readonly SortedList<TGroupHeader, SortedCollection<TItem>> _groups;

	public GroupedObservableReadonlyCollection(TCollection items, IGroupHeaderFactory<TGroupHeader, TItem> groupHeaderFactory, IItemDividerFactory? itemDividerFactory = null, IGroupFooterFactory? groupFooterFactory = null)
		: base(new ObservableCollection<object>())
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		_groupHeaderFactory = groupHeaderFactory;
		_itemDividerFactory = itemDividerFactory;
		_groupFooterFactory = groupFooterFactory;
		_groups = (SortedList<TGroupHeader, SortedCollection<TItem>>)(object)new SortedList<SortedCollection<TItem>, SortedCollection<_003F>>();
		_items = (global::System.Collections.IEnumerable)items;
		global::System.Collections.IEnumerable items2 = _items;
		INotifyCollectionChanged val = (INotifyCollectionChanged)((items2 is INotifyCollectionChanged) ? items2 : null);
		if (val != null)
		{
			val.CollectionChanged += new NotifyCollectionChangedEventHandler(ItemsCollectionChanged);
		}
		AddItems(_items);
	}

	private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		NotifyCollectionChangedAction action = e.Action;
		if ((int)action > 2)
		{
			if ((int)action == 4)
			{
				ClearItems();
				if (e.NewItems != null)
				{
					AddItems((global::System.Collections.IEnumerable?)e.NewItems);
				}
				else
				{
					AddItems(_items);
				}
			}
		}
		else
		{
			AddItems((global::System.Collections.IEnumerable?)e.NewItems);
			RemoveItems((global::System.Collections.IEnumerable?)e.OldItems);
		}
	}

	private unsafe void AddItems(global::System.Collections.IEnumerable? newItems)
	{
		if (newItems == null)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<TItem> enumerator = ((global::System.Collections.Generic.IEnumerable<_003F>)Enumerable.OfType<TItem>(newItems)).GetEnumerator();
		try
		{
			SortedCollection<TItem> sortedCollection = default(SortedCollection<TItem>);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				TItem current = ((global::System.Collections.Generic.IEnumerator<_003F>)enumerator).Current;
				int num = 0;
				TGroupHeader val = _groupHeaderFactory.Get(current);
				int num2 = 0;
				if (!((SortedList<SortedCollection<TItem>, SortedCollection<_003F>>)(object)_groups).TryGetValue((SortedCollection<TItem>)(object)val, ref *(SortedCollection<_003F>*)(&sortedCollection)))
				{
					sortedCollection = new SortedCollection<TItem>();
					((SortedList<SortedCollection<TItem>, SortedCollection<_003F>>)(object)_groups).Add((SortedCollection<TItem>)(object)val, (SortedCollection<_003F>)(object)sortedCollection);
					num = CalculateBackingCollectionIndex(val, _groups, _itemDividerFactory != null, _groupFooterFactory != null);
					((Collection<object>)(object)BackingCollection).Insert(num, (object)val);
					if (_groupFooterFactory != null)
					{
						((Collection<object>)(object)BackingCollection).Insert(num + 1, (object)_groupFooterFactory.Get(val));
					}
				}
				else
				{
					num = CalculateBackingCollectionIndex(val, _groups, _itemDividerFactory != null, _groupFooterFactory != null);
				}
				sortedCollection.Add(current, out var index);
				num2 = CalculateBackingCollectionIndex(index, num, _itemDividerFactory != null);
				((Collection<object>)(object)BackingCollection).Insert(num2, (object)current);
				if (_itemDividerFactory == null)
				{
					continue;
				}
				IItemDivider itemDivider = _itemDividerFactory.Get(val);
				int num3 = index;
				if (num3 <= 0)
				{
					if (num3 == 0 && ((Collection<_003F>)(object)sortedCollection).Count > 1)
					{
						((Collection<object>)(object)BackingCollection).Insert(num2 + 1, (object)itemDivider);
					}
				}
				else
				{
					((Collection<object>)(object)BackingCollection).Insert(num2, (object)itemDivider);
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	private void RemoveItems(global::System.Collections.IEnumerable? oldItems)
	{
		if (oldItems == null)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<TItem> enumerator = ((global::System.Collections.Generic.IEnumerable<_003F>)Enumerable.OfType<TItem>(oldItems)).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				TItem current = ((global::System.Collections.Generic.IEnumerator<_003F>)enumerator).Current;
				TGroupHeader val = _groupHeaderFactory.Get(current);
				int num = CalculateBackingCollectionIndex(val, _groups, _itemDividerFactory != null, _groupFooterFactory != null);
				SortedCollection<TItem> sortedCollection = ((SortedList<SortedCollection<TItem>, SortedCollection<_003F>>)(object)_groups)[(SortedCollection<TItem>)(object)val];
				int num2 = ((Collection<_003F>)(object)sortedCollection).IndexOf(current);
				int num3 = CalculateBackingCollectionIndex(num2, num, _itemDividerFactory != null);
				((Collection<_003F>)(object)sortedCollection).RemoveAt(num2);
				if (_itemDividerFactory != null && ((Collection<_003F>)(object)sortedCollection).Count != 0)
				{
					((Collection<object>)(object)BackingCollection).RemoveAt(num3);
				}
				((Collection<object>)(object)BackingCollection).RemoveAt(num3);
				if (((Collection<_003F>)(object)sortedCollection).Count == 0)
				{
					((SortedList<SortedCollection<TItem>, SortedCollection<_003F>>)(object)_groups).Remove((SortedCollection<TItem>)(object)val);
					if (_groupFooterFactory != null)
					{
						((Collection<object>)(object)BackingCollection).RemoveAt(num + 1);
					}
					((Collection<object>)(object)BackingCollection).RemoveAt(num);
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	private void ClearItems()
	{
		((SortedList<SortedCollection<TItem>, SortedCollection<_003F>>)(object)_groups).Clear();
		((Collection<object>)(object)BackingCollection).Clear();
	}

	private static int CalculateBackingCollectionIndex(TGroupHeader groupHeader, SortedList<TGroupHeader, SortedCollection<TItem>> groups, bool addItemDividers, bool addGroupFooters)
	{
		int num = ((SortedList<SortedCollection<TItem>, SortedCollection<_003F>>)(object)groups).IndexOfKey((SortedCollection<TItem>)(object)groupHeader);
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			num2 += ((Collection<_003F>)(object)((global::System.Collections.Generic.IList<SortedCollection<_003F>>)((SortedList<SortedCollection<TItem>, SortedCollection<_003F>>)(object)groups).Values)[i]).Count;
			if (addItemDividers)
			{
				num2 += ((Collection<_003F>)(object)((global::System.Collections.Generic.IList<SortedCollection<_003F>>)((SortedList<SortedCollection<TItem>, SortedCollection<_003F>>)(object)groups).Values)[i]).Count - 1;
			}
			if (addGroupFooters)
			{
				num2++;
			}
			if (num > 0)
			{
				num2++;
			}
		}
		return num2;
	}

	private static int CalculateBackingCollectionIndex(int itemIndex, int mainGroupIndex, bool addItemDividers)
	{
		if (!addItemDividers || itemIndex == 0)
		{
			return mainGroupIndex + 1 + itemIndex;
		}
		return mainGroupIndex + 2 * itemIndex;
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		base.Dispose(disposing);
		if (!disposing)
		{
			return;
		}
		ClearItems();
		global::System.Collections.IEnumerable items = _items;
		INotifyCollectionChanged val = (INotifyCollectionChanged)((items is INotifyCollectionChanged) ? items : null);
		if (val == null)
		{
			return;
		}
		try
		{
			val.CollectionChanged -= new NotifyCollectionChangedEventHandler(ItemsCollectionChanged);
		}
		catch
		{
		}
	}
}
