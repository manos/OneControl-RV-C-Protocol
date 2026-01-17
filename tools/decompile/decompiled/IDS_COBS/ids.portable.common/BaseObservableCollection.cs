using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

public class BaseObservableCollection<T> : ObservableCollection<T>
{
	public readonly struct SuppressEventsDisposable : global::System.IDisposable
	{
		private readonly BaseObservableCollection<T> _collection;

		private readonly bool _forceRefresh;

		public SuppressEventsDisposable(BaseObservableCollection<T> collection, bool forceRefresh)
		{
			_forceRefresh = forceRefresh;
			_collection = collection;
			collection._suppressEvents++;
		}

		public void Dispose()
		{
			_collection._suppressEvents--;
			if (!_collection.EventsAreSuppressed && (_forceRefresh || _collection._suppressedEventCount != 0L))
			{
				_collection._suppressedEventCount = 0uL;
				_collection.SendOnCollectionChangedReset();
			}
		}
	}

	private const string LogTag = "BaseObservableCollection";

	internal static readonly NotifyCollectionChangedEventArgs ResetEventArgs = new NotifyCollectionChangedEventArgs((NotifyCollectionChangedAction)4);

	private int _suppressEvents;

	private ulong _suppressedEventCount;

	public bool EventsAreSuppressed => _suppressEvents > 0;

	public SuppressEventsDisposable SuppressEvents(bool forceRefresh = false)
	{
		return new SuppressEventsDisposable(this, forceRefresh);
	}

	protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		if (EventsAreSuppressed)
		{
			_suppressedEventCount++;
			return;
		}
		InvokeOnMainThread((Action)delegate
		{
			base.OnCollectionChanged(e);
		});
	}

	protected void InvokeOnMainThread(Action action)
	{
		MainThread.RequestMainThreadAction(action);
	}

	protected override void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		InvokeOnMainThread((Action)delegate
		{
			base.OnPropertyChanged(e);
		});
	}

	private void SendOnCollectionChangedReset()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		InvokeOnMainThread((Action)([CompilerGenerated] () =>
		{
			base.OnCollectionChanged(ResetEventArgs);
		}));
	}

	public void TryRemoveItemLast()
	{
		if (((Collection<T>)(object)this).Count > 0)
		{
			((Collection<T>)(object)this).RemoveAt(((Collection<T>)(object)this).Count - 1);
		}
	}
}
