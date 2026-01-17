using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace IDS.Portable.Common;

public class CollectionTransformer<TSource, TDestination> : CommonDisposable
{
	private const string LogTag = "CollectionTransformer";

	private readonly Func<TSource, TDestination> _transform;

	private readonly Action? _syncCompleted;

	private readonly ObservableCollection<TSource> _sourceCollection;

	private readonly global::System.Collections.Generic.ICollection<TDestination> _destinationCollection;

	private readonly Dictionary<TSource, TDestination> _conversionDict = new Dictionary<TSource, TDestination>();

	private readonly object lockObject = new object();

	public CollectionTransformer(ObservableCollection<TSource> sourceCollection, global::System.Collections.Generic.ICollection<TDestination> destinationCollection, Func<TSource, TDestination> transform, Action? syncCompleted = null)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		_sourceCollection = sourceCollection;
		_destinationCollection = destinationCollection;
		_transform = transform;
		_syncCompleted = syncCompleted;
		sourceCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(OnSourceCollectionChanged);
		global::System.Collections.Generic.IEnumerator<TSource> enumerator = ((Collection<TSource>)(object)sourceCollection).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				TSource current = enumerator.Current;
				AddToDestination(current);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		Action? syncCompleted2 = _syncCompleted;
		if (syncCompleted2 != null)
		{
			syncCompleted2.Invoke();
		}
	}

	private void AddToDestination(TSource sourceItem)
	{
		lock (lockObject)
		{
			if (!_conversionDict.ContainsKey(sourceItem))
			{
				TDestination val = _transform.Invoke(sourceItem);
				_conversionDict[sourceItem] = val;
				((global::System.Collections.Generic.ICollection<_003F>)_destinationCollection).Add(val);
			}
		}
	}

	private void RemoveFromDestination(TSource sourceItem)
	{
		lock (lockObject)
		{
			TDestination item = default(TDestination);
			if (_conversionDict.ContainsKey(sourceItem) && _conversionDict.TryGetValue(sourceItem, ref item))
			{
				_destinationCollection.TryRemove(item);
				((IDictionary<TSource, TDestination>)(object)_conversionDict).TryRemove<TSource, TDestination>(sourceItem);
			}
		}
	}

	private void RemoveAllItems()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		lock (lockObject)
		{
			using ((_destinationCollection as BaseObservableCollection<TDestination>)?.SuppressEvents())
			{
				Enumerator<TSource> enumerator = new List<TSource>((global::System.Collections.Generic.IEnumerable<TSource>)_conversionDict.Keys).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						TSource current = enumerator.Current;
						RemoveFromDestination(current);
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
			}
		}
	}

	private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected I4, but got Unknown
		NotifyCollectionChangedAction action = eventArgs.Action;
		switch ((int)action)
		{
		case 0:
		case 1:
		case 2:
		{
			lock (lockObject)
			{
				if (eventArgs.OldItems != null)
				{
					foreach (object item in (global::System.Collections.IEnumerable)eventArgs.OldItems)
					{
						if (item is TSource sourceItem)
						{
							RemoveFromDestination(sourceItem);
						}
					}
				}
				if (eventArgs.NewItems != null)
				{
					foreach (object item2 in (global::System.Collections.IEnumerable)eventArgs.NewItems)
					{
						if (item2 is TSource sourceItem2)
						{
							AddToDestination(sourceItem2);
						}
					}
				}
			}
			Action? syncCompleted = _syncCompleted;
			if (syncCompleted != null)
			{
				syncCompleted.Invoke();
			}
			break;
		}
		case 4:
			Sync();
			break;
		case 3:
			break;
		}
	}

	public void Sync()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		lock (lockObject)
		{
			try
			{
				HashSet<TSource> val = new HashSet<TSource>((global::System.Collections.Generic.IEnumerable<TSource>)_conversionDict.Keys);
				global::System.Collections.Generic.IEnumerator<TSource> enumerator = ((Collection<TSource>)(object)_sourceCollection).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
					{
						TSource current = enumerator.Current;
						if (((IReadOnlyDictionary<TSource, TDestination>)(object)_conversionDict).TryGetValue<TSource, TDestination>(current) == null)
						{
							AddToDestination(current);
						}
						else
						{
							((global::System.Collections.Generic.ICollection<TSource>)val).TryRemove(current);
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator)?.Dispose();
				}
				Enumerator<TSource> enumerator2 = val.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						TSource current2 = enumerator2.Current;
						RemoveFromDestination(current2);
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
				}
			}
			catch (global::System.Exception ex)
			{
				TaggedLog.Warning("CollectionTransformer", "Sync failed because {0}", ex.Message);
			}
		}
		Action? syncCompleted = _syncCompleted;
		if (syncCompleted != null)
		{
			syncCompleted.Invoke();
		}
	}

	public override void Dispose(bool disposing)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		try
		{
			_sourceCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnSourceCollectionChanged);
		}
		catch
		{
		}
		RemoveAllItems();
		Action? syncCompleted = _syncCompleted;
		if (syncCompleted != null)
		{
			syncCompleted.Invoke();
		}
	}
}
