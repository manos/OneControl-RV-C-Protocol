using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

public static class INotifyPropertyChangedExtension
{
	public static void NotifyMainThread(this INotifyPropertyChanged sender, PropertyChangedEventHandler? handler, [CallerMemberName] string propertyName = "")
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		MainThread.RequestMainThreadAction((Action)delegate
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			PropertyChangedEventHandler? obj = handler;
			if (obj != null)
			{
				obj.Invoke((object)sender, new PropertyChangedEventArgs(propertyName));
			}
		});
	}

	public static void Notify(this INotifyPropertyChanged sender, PropertyChangedEventHandler? handler, [CallerMemberName] string propertyName = "")
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		if (handler != null)
		{
			handler.Invoke((object)sender, new PropertyChangedEventArgs(propertyName));
		}
	}

	public static void UpdateAndThenNotifyMainThreadIfNeeded<TProperty>(this INotifyPropertyChanged sender, ref TProperty backingStore, TProperty newValue, PropertyChangedEventHandler? handler, [CallerMemberName] string propertyName = "") where TProperty : IEquatable<TProperty>
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		if (((IEquatable<TProperty>)backingStore/*cast due to .constrained prefix*/).Equals(newValue))
		{
			return;
		}
		backingStore = newValue;
		MainThread.RequestMainThreadAction((Action)delegate
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			PropertyChangedEventHandler? obj = handler;
			if (obj != null)
			{
				obj.Invoke((object)sender, new PropertyChangedEventArgs(propertyName));
			}
		});
	}

	public static void UpdateAndNotifyIfNeeded<TProperty>(this INotifyPropertyChanged sender, ref TProperty backingStore, TProperty newValue, PropertyChangedEventHandler? handler, [CallerMemberName] string propertyName = "") where TProperty : IEquatable<TProperty>
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		if (!((IEquatable<TProperty>)backingStore/*cast due to .constrained prefix*/).Equals(newValue))
		{
			backingStore = newValue;
			if (handler != null)
			{
				handler.Invoke((object)sender, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
