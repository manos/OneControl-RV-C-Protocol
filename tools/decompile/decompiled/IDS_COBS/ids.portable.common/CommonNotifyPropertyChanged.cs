using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace IDS.Portable.Common;

public class CommonNotifyPropertyChanged : INotifyPropertyChanged
{
	[CompilerGenerated]
	private PropertyChangedEventHandler? m_PropertyChanged;

	public event PropertyChangedEventHandler PropertyChanged
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			PropertyChangedEventHandler val = this.m_PropertyChanged;
			PropertyChangedEventHandler val2;
			do
			{
				val2 = val;
				PropertyChangedEventHandler val3 = (PropertyChangedEventHandler)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
				val = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.m_PropertyChanged, val3, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			PropertyChangedEventHandler val = this.m_PropertyChanged;
			PropertyChangedEventHandler val2;
			do
			{
				val2 = val;
				PropertyChangedEventHandler val3 = (PropertyChangedEventHandler)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
				val = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.m_PropertyChanged, val3, val2);
			}
			while (val != val2);
		}
	}

	protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		NotifyPropertyChanged(propertyName);
	}

	protected void NotifyPropertyChanged(string propertyName, bool notifyOnMainThread = true)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		if (notifyOnMainThread)
		{
			MainThread.RequestMainThreadAction((Action)delegate
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Expected O, but got Unknown
				PropertyChangedEventHandler? obj2 = this.PropertyChanged;
				if (obj2 != null)
				{
					obj2.Invoke((object)this, new PropertyChangedEventArgs(propertyName));
				}
			});
		}
		else
		{
			PropertyChangedEventHandler? obj = this.PropertyChanged;
			if (obj != null)
			{
				obj.Invoke((object)this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}

	protected bool SetBackingField<TValue>(ref TValue field, TValue value, [CallerMemberName] string notifyPropertyName = "", params string[] notifyPropertyNameEnumeration)
	{
		if (EqualityComparer<TValue>.Default.Equals(field, value))
		{
			return false;
		}
		field = value;
		if (!string.IsNullOrEmpty(notifyPropertyName))
		{
			OnPropertyChanged(notifyPropertyName);
		}
		if (notifyPropertyNameEnumeration == null)
		{
			return true;
		}
		foreach (string text in notifyPropertyNameEnumeration)
		{
			if (!string.IsNullOrEmpty(text))
			{
				OnPropertyChanged(text);
			}
		}
		return true;
	}

	protected void RemoveAllPropertyChangedEventHandler()
	{
		this.PropertyChanged = null;
	}
}
