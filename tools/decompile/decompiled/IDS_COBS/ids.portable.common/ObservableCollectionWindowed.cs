using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace IDS.Portable.Common;

public class ObservableCollectionWindowed<TValue, TCollection> : ObservableCollectionWindowed<TValue> where TCollection : ObservableCollection<TValue>, new()
{
	public ObservableCollectionWindowed(int windowSize, int windowStartIndex = 0)
		: base((ObservableCollection<TValue>)new TCollection(), windowSize, windowStartIndex)
	{
	}
}
public class ObservableCollectionWindowed<TValue> : BaseObservableCollection<TValue>
{
	public readonly ObservableCollection<TValue> BackingCollection;

	private int _windowSize;

	private int _windowStartIndex;

	public int WindowSize
	{
		get
		{
			return _windowSize;
		}
		set
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (value < 0)
			{
				throw new ArgumentException("WindowSize can't be less then 0");
			}
			if (_windowSize != value)
			{
				_windowSize = value;
				SyncWithBackingCollection(clearFirst: true);
			}
		}
	}

	private int MaxVisibleCount => Math.Min(WindowSize, Math.Max(((Collection<TValue>)(object)BackingCollection).Count - WindowStartIndex, 0));

	public int WindowStartIndex
	{
		get
		{
			return _windowStartIndex;
		}
		set
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (value < 0)
			{
				throw new ArgumentException("WindowStartIndex can't be less then 0");
			}
			if (_windowStartIndex != value)
			{
				_windowStartIndex = value;
				SyncWithBackingCollection(clearFirst: true);
			}
		}
	}

	public ObservableCollectionWindowed(ObservableCollection<TValue> backingCollection, int windowSize, int windowStartIndex = 0)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		BackingCollection = backingCollection;
		_windowSize = windowSize;
		_windowStartIndex = windowStartIndex;
		SyncWithBackingCollection(clearFirst: false);
		BackingCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnBackingCollectionChanged);
	}

	protected override void InsertItem(int index, TValue item)
	{
		((Collection<TValue>)(object)BackingCollection).Insert(index, item);
	}

	protected override void RemoveItem(int index)
	{
		((Collection<TValue>)(object)BackingCollection).RemoveAt(index);
	}

	protected override void SetItem(int index, TValue item)
	{
		((Collection<TValue>)(object)BackingCollection)[index] = item;
	}

	public void SyncWithBackingCollection(bool clearFirst)
	{
		if (clearFirst)
		{
			for (int num = ((Collection<TValue>)(object)this).Count - 1; num >= 0; num--)
			{
				((ObservableCollection<TValue>)this).RemoveItem(num);
			}
		}
		int num2 = WindowStartIndex;
		int num3 = 0;
		while (num3 < WindowSize && num2 < ((Collection<TValue>)(object)BackingCollection).Count)
		{
			TValue val = ((Collection<TValue>)(object)BackingCollection)[num2];
			if (num3 >= ((Collection<TValue>)(object)this).Count)
			{
				((ObservableCollection<TValue>)this).InsertItem(((Collection<TValue>)(object)this).Count, val);
			}
			else if (!object.Equals((object)((Collection<TValue>)(object)this)[num3], (object)val))
			{
				((ObservableCollection<TValue>)this).InsertItem(num3, val);
			}
			num3++;
			num2++;
		}
		int num4 = ((Collection<TValue>)(object)this).Count - 1;
		while (num4 >= 0 && (num4 >= MaxVisibleCount || num4 >= WindowSize))
		{
			((ObservableCollection<TValue>)this).RemoveItem(num4);
			num4--;
		}
	}

	private void OnBackingCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		NotifyCollectionChangedAction action = eventArgs.Action;
		if ((int)action > 3)
		{
			if ((int)action == 4)
			{
				((Collection<TValue>)(object)this).Clear();
			}
		}
		else
		{
			SyncWithBackingCollection(clearFirst: false);
		}
	}
}
