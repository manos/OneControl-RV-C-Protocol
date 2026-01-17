using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace IDS.Portable.Common.ObservableCollection;

public class ObservableReadOnlyCollection<TCollection, TItem> : ReadOnlyCollection<TItem>, INotifyCollectionChanged, INotifyPropertyChanged, global::System.IDisposable where TCollection : global::System.Collections.Generic.IList<TItem>
{
	protected internal readonly TCollection BackingCollection;

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

	public event PropertyChangedEventHandler PropertyChanged
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			PropertyChangedEventHandler val = this.PropertyChanged;
			PropertyChangedEventHandler val2;
			do
			{
				val2 = val;
				PropertyChangedEventHandler val3 = (PropertyChangedEventHandler)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
				val = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			PropertyChangedEventHandler val = this.PropertyChanged;
			PropertyChangedEventHandler val2;
			do
			{
				val2 = val;
				PropertyChangedEventHandler val3 = (PropertyChangedEventHandler)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
				val = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	public ObservableReadOnlyCollection(TCollection collection)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		((ReadOnlyCollection<_003F>)(object)this)._002Ector((global::System.Collections.Generic.IList<_003F>)(object)collection);
		BackingCollection = collection;
		object obj = collection;
		INotifyCollectionChanged val = (INotifyCollectionChanged)((obj is INotifyCollectionChanged) ? obj : null);
		if (val != null)
		{
			val.CollectionChanged += new NotifyCollectionChangedEventHandler(HandleCollectionChanged);
		}
		object obj2 = collection;
		INotifyPropertyChanged val2 = (INotifyPropertyChanged)((obj2 is INotifyPropertyChanged) ? obj2 : null);
		if (val2 != null)
		{
			val2.PropertyChanged += new PropertyChangedEventHandler(HandlePropertyChanged);
		}
	}

	private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		OnCollectionChanged(e);
	}

	private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		OnPropertyChanged(e);
	}

	protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
	{
		NotifyCollectionChangedEventHandler? obj = this.CollectionChanged;
		if (obj != null)
		{
			obj.Invoke((object)this, args);
		}
	}

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
	{
		PropertyChangedEventHandler? obj = this.PropertyChanged;
		if (obj != null)
		{
			obj.Invoke((object)this, args);
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		if (!disposing)
		{
			return;
		}
		try
		{
			((INotifyCollectionChanged)((ReadOnlyCollection<_003F>)(object)this).Items).CollectionChanged -= new NotifyCollectionChangedEventHandler(HandleCollectionChanged);
		}
		catch
		{
		}
		try
		{
			((INotifyPropertyChanged)((ReadOnlyCollection<_003F>)(object)this).Items).PropertyChanged -= new PropertyChangedEventHandler(HandlePropertyChanged);
		}
		catch
		{
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize((object)this);
	}

	~ObservableReadOnlyCollection()
	{
		try
		{
			Dispose(disposing: false);
		}
		finally
		{
			((object)this).Finalize();
		}
	}
}
