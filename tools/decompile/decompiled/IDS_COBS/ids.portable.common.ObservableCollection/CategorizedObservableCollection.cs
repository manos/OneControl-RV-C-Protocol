using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace IDS.Portable.Common.ObservableCollection;

public class CategorizedObservableCollection<TCollection, TICategory, TCategory, TIItem, TItem> : CommonDisposable, global::System.Collections.Generic.IList<TCollection>, global::System.Collections.Generic.ICollection<TCollection>, global::System.Collections.Generic.IEnumerable<TCollection>, global::System.Collections.IEnumerable, INotifyCollectionChanged where TICategory : TCollection, ICategory<TCategory> where TCategory : IComparable<TCategory>, IEquatable<TCategory> where TIItem : TCollection, IItem<TICategory, TCategory, TItem> where TItem : IComparable<TItem>, IEquatable<TItem>
{
	private class CategorizableComparer<TICategory, TCategory, TIItem, TItem> : IComparer<TCollection> where TICategory : TCollection, ICategory<TCategory> where TCategory : IComparable<TCategory>, IEquatable<TCategory> where TIItem : TCollection, IItem<TICategory, TCategory, TItem> where TItem : IComparable<TItem>, IEquatable<TItem>
	{
		private readonly bool _ascendingCategories;

		private readonly bool _ascendingItems;

		public CategorizableComparer(bool ascendingCategories = true, bool ascendingItems = true)
		{
			_ascendingCategories = ascendingCategories;
			_ascendingItems = ascendingItems;
		}

		public int Compare(TCollection x, TCollection y)
		{
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			if (x is TIItem && y is TIItem)
			{
				TIItem val = (TIItem)(object)x;
				TIItem val2 = (TIItem)(object)y;
				int num = (_ascendingCategories ? ((IComparable<_003F>)val.Category.CategoryKey/*cast due to .constrained prefix*/).CompareTo(val2.Category.CategoryKey) : ((IComparable<_003F>)val2.Category.CategoryKey/*cast due to .constrained prefix*/).CompareTo(val.Category.CategoryKey));
				if (num == 0)
				{
					if (!_ascendingItems)
					{
						return ((IComparable<_003F>)val2.ItemKey/*cast due to .constrained prefix*/).CompareTo(val.ItemKey);
					}
					return ((IComparable<_003F>)val.ItemKey/*cast due to .constrained prefix*/).CompareTo(val2.ItemKey);
				}
				return num;
			}
			if (x is TICategory && y is TIItem)
			{
				TICategory val3 = (TICategory)(object)x;
				TIItem val4 = (TIItem)(object)y;
				int num2 = (_ascendingCategories ? ((IComparable<_003F>)val3.CategoryKey/*cast due to .constrained prefix*/).CompareTo(val4.Category.CategoryKey) : ((IComparable<_003F>)val4.Category.CategoryKey/*cast due to .constrained prefix*/).CompareTo(val3.CategoryKey));
				if (num2 == 0)
				{
					if (!_ascendingCategories)
					{
						return 1;
					}
					return -1;
				}
				return num2;
			}
			if (x is TIItem && y is TICategory)
			{
				TIItem val5 = (TIItem)(object)x;
				TICategory val6 = (TICategory)(object)y;
				int num3 = (_ascendingCategories ? ((IComparable<_003F>)val5.Category.CategoryKey/*cast due to .constrained prefix*/).CompareTo(val6.CategoryKey) : ((IComparable<_003F>)val6.CategoryKey/*cast due to .constrained prefix*/).CompareTo(val5.Category.CategoryKey));
				if (num3 == 0)
				{
					if (!_ascendingCategories)
					{
						return -1;
					}
					return 1;
				}
				return num3;
			}
			if (x is TICategory && y is TICategory)
			{
				TICategory val7 = (TICategory)(object)x;
				TICategory val8 = (TICategory)(object)y;
				if (!_ascendingCategories)
				{
					return ((IComparable<_003F>)val8.CategoryKey/*cast due to .constrained prefix*/).CompareTo(val7.CategoryKey);
				}
				return ((IComparable<_003F>)val7.CategoryKey/*cast due to .constrained prefix*/).CompareTo(val8.CategoryKey);
			}
			throw new ArgumentException("");
		}
	}

	private readonly bool _autoRemoveCategory;

	private readonly ComparingObservableCollection<TCollection> _collection;

	private readonly Dictionary<TCategory, int> _categories;

	TCollection global::System.Collections.Generic.IList<TCollection>.this[int index]
	{
		get
		{
			return ((Collection<TCollection>)(object)_collection)[index];
		}
		set
		{
			((Collection<TCollection>)(object)_collection)[index] = value;
		}
	}

	public int Count => ((Collection<TCollection>)(object)_collection).Count;

	public bool IsReadOnly => false;

	public event NotifyCollectionChangedEventHandler CollectionChanged
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			NotifyCollectionChangedEventHandler val = this.CollectionChanged;
			NotifyCollectionChangedEventHandler val2;
			do
			{
				val2 = val;
				NotifyCollectionChangedEventHandler val3 = (NotifyCollectionChangedEventHandler)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
				val = Interlocked.CompareExchange<NotifyCollectionChangedEventHandler>(ref this.CollectionChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			NotifyCollectionChangedEventHandler val = this.CollectionChanged;
			NotifyCollectionChangedEventHandler val2;
			do
			{
				val2 = val;
				NotifyCollectionChangedEventHandler val3 = (NotifyCollectionChangedEventHandler)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
				val = Interlocked.CompareExchange<NotifyCollectionChangedEventHandler>(ref this.CollectionChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public CategorizedObservableCollection(bool ascendingCategories = true, bool ascendingItems = true, bool autoRemoveCategory = true)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		_autoRemoveCategory = autoRemoveCategory;
		CategorizableComparer<TICategory, TCategory, TIItem, TItem> comparer = new CategorizableComparer<TICategory, TCategory, TIItem, TItem>(ascendingCategories, ascendingItems);
		_collection = new ComparingObservableCollection<TCollection>((IComparer<TCollection>?)(object)comparer);
		((ObservableCollection<TCollection>)_collection).CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
		_categories = (Dictionary<TCategory, int>)(object)new Dictionary<_003F, int>();
	}

	public void Sort()
	{
		Sort(new TCategory[0]);
	}

	public void Sort(TICategory cleanCategory)
	{
		Sort(new TCategory[1] { cleanCategory.CategoryKey });
	}

	public void Sort(bool cleanCategories)
	{
		Sort(cleanCategories ? Enumerable.ToArray<TCategory>((global::System.Collections.Generic.IEnumerable<TCategory>)((Dictionary<_003F, int>)(object)_categories).Keys) : new TCategory[0]);
	}

	protected virtual void Sort(TCategory[] cleanCategories)
	{
		_collection.Sort();
		int num = 0;
		while (num < ((Collection<TCollection>)(object)_collection).Count)
		{
			TCollection val = ((Collection<TCollection>)(object)_collection)[num];
			if (val is TICategory val2 && Enumerable.Contains<TCategory>((global::System.Collections.Generic.IEnumerable<TCategory>)cleanCategories, val2.CategoryKey))
			{
				if (num == ((Collection<TCollection>)(object)_collection).Count - 1)
				{
					((Dictionary<_003F, int>)(object)_categories).Remove(val2.CategoryKey);
					((Collection<TCollection>)(object)_collection).RemoveAt(num);
				}
				else if (((Collection<TCollection>)(object)_collection)[num + 1] is TICategory)
				{
					((Dictionary<_003F, int>)(object)_categories).Remove(val2.CategoryKey);
					((Collection<TCollection>)(object)_collection).RemoveAt(num);
					continue;
				}
			}
			num++;
		}
	}

	public override void Dispose(bool disposing)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		((ObservableCollection<TCollection>)_collection).CollectionChanged -= new NotifyCollectionChangedEventHandler(OnCollectionChanged);
		Clear();
	}

	public int IndexOf(TCollection item)
	{
		if (item is TIItem item2)
		{
			return IndexOf(item2);
		}
		if (item is TICategory item3)
		{
			return IndexOf(item3);
		}
		return -1;
	}

	public void Insert(int index, TCollection item)
	{
		Add(item);
	}

	public void RemoveAt(int index)
	{
		Remove(((Collection<TCollection>)(object)_collection)[index]);
	}

	protected int IndexOf(TIItem item)
	{
		for (int i = 0; i < ((Collection<TCollection>)(object)_collection).Count; i++)
		{
			TCollection val = ((Collection<TCollection>)(object)_collection)[i];
			if (val is TIItem val2 && ((object)item/*cast due to .constrained prefix*/).Equals((object)val2))
			{
				return i;
			}
		}
		return -1;
	}

	protected int IndexOf(TICategory item)
	{
		for (int i = 0; i < ((Collection<TCollection>)(object)_collection).Count; i++)
		{
			TCollection val = ((Collection<TCollection>)(object)_collection)[i];
			if (val is TICategory val2 && ((object)item/*cast due to .constrained prefix*/).Equals((object)val2))
			{
				return i;
			}
		}
		return -1;
	}

	global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
	{
		return (global::System.Collections.IEnumerator)GetEnumerator();
	}

	public global::System.Collections.Generic.IEnumerator<TCollection> GetEnumerator()
	{
		return ((Collection<TCollection>)(object)_collection).GetEnumerator();
	}

	public void Add(TCollection item)
	{
		if (item is TIItem item2)
		{
			AddItem(item2);
		}
		if (item is TICategory category)
		{
			AddCategory(category);
		}
	}

	public void Clear()
	{
		((Collection<TCollection>)(object)_collection).Clear();
		((Dictionary<_003F, int>)(object)_categories).Clear();
	}

	public bool Contains(TCollection item)
	{
		return ((Collection<TCollection>)(object)_collection).Contains(item);
	}

	public void CopyTo(TCollection[] array, int arrayIndex)
	{
		((Collection<TCollection>)(object)_collection).CopyTo(array, arrayIndex);
	}

	public bool Remove(TCollection item)
	{
		if (item is TIItem item2)
		{
			return RemoveItem(item2);
		}
		if (item is TICategory category)
		{
			return RemoveCategory(category);
		}
		return false;
	}

	protected virtual void AddItem(TIItem item)
	{
		AddCategory(item.Category);
		Dictionary<TCategory, int> categories = _categories;
		TCategory categoryKey = item.Category.CategoryKey;
		int num = ((Dictionary<_003F, int>)(object)categories)[categoryKey];
		((Dictionary<_003F, int>)(object)categories)[categoryKey] = num + 1;
		((Collection<TCollection>)(object)_collection).Add((TCollection)(object)item);
	}

	protected virtual void AddCategory(TICategory category)
	{
		if (!((Dictionary<_003F, int>)(object)_categories).ContainsKey(category.CategoryKey))
		{
			((Dictionary<_003F, int>)(object)_categories).Add(category.CategoryKey, 0);
			((Collection<TCollection>)(object)_collection).Add((TCollection)(object)category);
		}
	}

	protected virtual bool RemoveItem(TIItem item)
	{
		if (!((Collection<TCollection>)(object)_collection).Contains((TCollection)(object)item))
		{
			return false;
		}
		((Collection<TCollection>)(object)_collection).Remove((TCollection)(object)item);
		if (((Dictionary<_003F, int>)(object)_categories).ContainsKey(item.Category.CategoryKey))
		{
			Dictionary<TCategory, int> categories = _categories;
			TCategory categoryKey = item.Category.CategoryKey;
			int num = ((Dictionary<_003F, int>)(object)categories)[categoryKey];
			((Dictionary<_003F, int>)(object)categories)[categoryKey] = num - 1;
		}
		if (_autoRemoveCategory && ((Dictionary<_003F, int>)(object)_categories)[item.Category.CategoryKey] < 1)
		{
			RemoveCategory(item.Category);
		}
		return true;
	}

	protected virtual bool RemoveCategory(TICategory category)
	{
		if (!((Dictionary<_003F, int>)(object)_categories).ContainsKey(category.CategoryKey))
		{
			return false;
		}
		if (((Dictionary<_003F, int>)(object)_categories)[category.CategoryKey] > 0)
		{
			int num = 0;
			while (num < ((Collection<TCollection>)(object)_collection).Count)
			{
				TCollection val = ((Collection<TCollection>)(object)_collection)[num];
				if (val is TIItem val2 && ((IComparable<_003F>)val2.Category.CategoryKey/*cast due to .constrained prefix*/).CompareTo(category.CategoryKey) == 0)
				{
					((Collection<TCollection>)(object)_collection).RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
		}
		((Dictionary<_003F, int>)(object)_categories).Remove(category.CategoryKey);
		((Collection<TCollection>)(object)_collection).Remove((TCollection)(object)category);
		return true;
	}

	private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
	{
		NotifyCollectionChangedEventHandler? obj = this.CollectionChanged;
		if (obj != null)
		{
			obj.Invoke(sender, notifyCollectionChangedEventArgs);
		}
	}
}
