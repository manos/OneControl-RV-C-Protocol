using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public class BindableAsyncValueProxy<TValue> : CommonNotifyPropertyChanged where TValue : IEquatable<TValue>
{
	private IBindableAsyncValue<TValue>? _backingStore;

	public IBindableAsyncValue<TValue>? BackingStore
	{
		get
		{
			return _backingStore;
		}
		set
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			if (_backingStore != value)
			{
				if (_backingStore != null)
				{
					((INotifyPropertyChanged)_backingStore).PropertyChanged -= new PropertyChangedEventHandler(BackingStoreOnPropertyChanged);
				}
				_backingStore = value;
				if (_backingStore != null)
				{
					((INotifyPropertyChanged)_backingStore).PropertyChanged += new PropertyChangedEventHandler(BackingStoreOnPropertyChanged);
				}
				OnPropertyChanged("HasValueBeenLoaded");
				OnPropertyChanged("IsReading");
				OnPropertyChanged("IsWriting");
				OnPropertyChanged("LastValue");
				OnPropertyChanged("Value");
			}
		}
	}

	private IBindableAsyncValue<TValue>? ResolvedBackingStore
	{
		get
		{
			if (_backingStore == null || _backingStore.IsDisposed)
			{
				return null;
			}
			return _backingStore;
		}
	}

	public bool HasValueBeenLoaded => ResolvedBackingStore?.HasValueBeenLoaded ?? false;

	public bool IsReading => ResolvedBackingStore?.IsReading ?? false;

	public bool IsWriting => ResolvedBackingStore?.IsWriting ?? false;

	public TValue LastValue
	{
		get
		{
			if (ResolvedBackingStore != null)
			{
				return ResolvedBackingStore.LastValue;
			}
			return default(TValue);
		}
	}

	public TValue Value
	{
		get
		{
			if (ResolvedBackingStore != null)
			{
				return ResolvedBackingStore.Value;
			}
			return default(TValue);
		}
	}

	~BindableAsyncValueProxy()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		try
		{
			if (_backingStore != null)
			{
				((INotifyPropertyChanged)_backingStore).PropertyChanged -= new PropertyChangedEventHandler(BackingStoreOnPropertyChanged);
			}
			_backingStore = null;
		}
		finally
		{
			((object)this).Finalize();
		}
	}

	public global::System.Threading.Tasks.Task LoadAsync(CancellationToken cancellationToken)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		return (ResolvedBackingStore ?? throw new global::System.Exception("Bindable Delayed Device Value Proxy Not Bound to Backing Store")).LoadAsync(cancellationToken);
	}

	public global::System.Threading.Tasks.Task SaveAsync(CancellationToken cancellationToken)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		return (ResolvedBackingStore ?? throw new global::System.Exception("Bindable Delayed Device Value Proxy Not Bound to Backing Store")).SaveAsync(cancellationToken);
	}

	private void BackingStoreOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
	{
		string propertyName = propertyChangedEventArgs.PropertyName;
		if (!(propertyName != "HasValueBeenLoaded") && !(propertyName != "IsReading") && !(propertyName != "IsWriting") && !(propertyName != "LastValue") && !(propertyName != "Value"))
		{
			OnPropertyChanged(propertyName);
		}
	}
}
