using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Utils;

namespace IDS.Portable.Common;

public class NotifyPropertyChangedProxySource : CommonDisposable
{
	private const string LogTag = "NotifyPropertyChangedProxySource";

	private readonly object lockObject = new object();

	private readonly INotifyPropertyChanged _source;

	private readonly ProxyOnPropertyChanged? _destinationOnPropertyChanged;

	private readonly bool _onlyAllowInvokes;

	private readonly Dictionary<string, HashSet<string>> _propertyNameMap = new Dictionary<string, HashSet<string>>();

	private readonly Dictionary<string, HashSet<Action>> _propertyActionDict = new Dictionary<string, HashSet<Action>>();

	private string? _anyDestinationPropertyName;

	private Watchdog? _aggregateNotificationWatchdog;

	private HashSet<string>? _aggregatedPropertyNames;

	private static readonly ObjectPool<HashSet<string>> _aggregateNotificationPool = ObjectPool<HashSet<string>>.MakeObjectPool<HashSet<string>>();

	public INotifyPropertyChanged Source => _source;

	public NotifyPropertyChangedProxySource(INotifyPropertyChanged source, ProxyOnPropertyChanged destinationOnPropertyChanged, List<string>? properyNameList = null)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		_source = source;
		_destinationOnPropertyChanged = destinationOnPropertyChanged;
		_source.PropertyChanged += new PropertyChangedEventHandler(PropertyChangedEventHandler);
		_onlyAllowInvokes = false;
		if (properyNameList == null)
		{
			return;
		}
		Enumerator<string> enumerator = properyNameList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				AddProxyFor(current);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
	}

	public NotifyPropertyChangedProxySource(INotifyPropertyChanged source)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		_source = source;
		_destinationOnPropertyChanged = null;
		_source.PropertyChanged += new PropertyChangedEventHandler(PropertyChangedEventHandler);
		_onlyAllowInvokes = true;
	}

	public NotifyPropertyChangedProxySource(INotifyPropertyChanged source, ProxyOnPropertyChanged destinationOnPropertyChanged, INotifyPropertyChanged destination, string proxyName)
		: this(source, destinationOnPropertyChanged)
	{
		global::System.Type type = ((object)source).GetType();
		bool flag = HasAutoProxy(((object)destination).GetType(), proxyName);
		global::System.Collections.Generic.IEnumerator<PropertyInfo> enumerator = RuntimeReflectionExtensions.GetRuntimeProperties(((object)destination).GetType()).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				PropertyInfo current = enumerator.Current;
				if (current == (PropertyInfo)null)
				{
					continue;
				}
				if (flag && type.GetProperty(((MemberInfo)current).Name) != (PropertyInfo)null)
				{
					bool flag2 = false;
					global::System.Collections.Generic.IEnumerator<NotifyPropertyChangedAutoProxyIgnoreAttribute> enumerator2 = CustomAttributeExtensions.GetCustomAttributes<NotifyPropertyChangedAutoProxyIgnoreAttribute>((MemberInfo)(object)current).GetEnumerator();
					try
					{
						while (((global::System.Collections.IEnumerator)enumerator2).MoveNext())
						{
							NotifyPropertyChangedAutoProxyIgnoreAttribute current2 = enumerator2.Current;
							if (!(current2.ProxyName != proxyName) || !(current2.PropertyName != ((MemberInfo)current).Name))
							{
								flag2 = true;
								TaggedLog.Debug("NotifyPropertyChangedProxySource", "{0} - AUTO property proxy {1} ignored by request!", destination, ((MemberInfo)current).Name);
							}
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator2)?.Dispose();
					}
					if (!flag2)
					{
						TaggedLog.Debug("NotifyPropertyChangedProxySource", "{0} - AUTO adding property proxy {1} as it exists in both the source and destination!", destination, ((MemberInfo)current).Name);
						AddProxyFor(((MemberInfo)current).Name, ((MemberInfo)current).Name);
					}
				}
				global::System.Collections.Generic.IEnumerator<NotifyPropertyChangedProxyAttribute> enumerator3 = CustomAttributeExtensions.GetCustomAttributes<NotifyPropertyChangedProxyAttribute>((MemberInfo)(object)current).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator3).MoveNext())
					{
						NotifyPropertyChangedProxyAttribute current3 = enumerator3.Current;
						try
						{
							if (!(current3.ProxyName != proxyName))
							{
								AddProxyFor(current3.SourcePropertyName, current3.DestinationPropertyName);
							}
						}
						catch (global::System.Exception ex)
						{
							TaggedLog.Error("NotifyPropertyChangedProxySource", "Ignoring attributed invoke for {0} because {1}", current3.SourcePropertyName, ex.Message);
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator3)?.Dispose();
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		global::System.Collections.Generic.IEnumerator<MethodInfo> enumerator4 = RuntimeReflectionExtensions.GetRuntimeMethods(((object)destination).GetType()).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator4).MoveNext())
			{
				MethodInfo current4 = enumerator4.Current;
				global::System.Collections.Generic.IEnumerator<NotifyPropertyChangedInvokeAttribute> enumerator5 = CustomAttributeExtensions.GetCustomAttributes<NotifyPropertyChangedInvokeAttribute>((MemberInfo)(object)current4).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator5).MoveNext())
					{
						NotifyPropertyChangedInvokeAttribute current5 = enumerator5.Current;
						try
						{
							if (!(current5.ProxyName != proxyName))
							{
								AddInvokeFor(current5.SourcePropertyName, current5.MakeInvokeMethod(current4, destination));
							}
						}
						catch (global::System.Exception ex2)
						{
							TaggedLog.Error("NotifyPropertyChangedProxySource", "Ignoring attributed method invoke for {0} because {1}", ((MemberInfo)current4).Name, ex2.Message);
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator5)?.Dispose();
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator4)?.Dispose();
		}
		global::System.Collections.Generic.IEnumerator<FieldInfo> enumerator6 = RuntimeReflectionExtensions.GetRuntimeFields(((object)destination).GetType()).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator6).MoveNext())
			{
				FieldInfo current6 = enumerator6.Current;
				global::System.Collections.Generic.IEnumerator<NotifyPropertyChangedInvokeFieldAttribute> enumerator7 = CustomAttributeExtensions.GetCustomAttributes<NotifyPropertyChangedInvokeFieldAttribute>((MemberInfo)(object)current6).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator7).MoveNext())
					{
						NotifyPropertyChangedInvokeFieldAttribute current7 = enumerator7.Current;
						try
						{
							if (!(current7.ProxyName != proxyName))
							{
								AddInvokeFor(current7.SourcePropertyName, current7.MakeInvokeMethod(current6, destination));
							}
						}
						catch (global::System.Exception ex3)
						{
							TaggedLog.Error("NotifyPropertyChangedProxySource", "Ignoring attributed field invoke for {0}.{1} because {2}", ((MemberInfo)current6).Name, current7.MethodName ?? "Unknown", ex3.Message);
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator7)?.Dispose();
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator6)?.Dispose();
		}
	}

	virtual ~NotifyPropertyChangedProxySource()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		try
		{
			_source.PropertyChanged -= new PropertyChangedEventHandler(PropertyChangedEventHandler);
		}
		catch
		{
		}
		finally
		{
			((object)this).Finalize();
		}
	}

	private static bool HasAutoProxy(global::System.Type destinationType, string proxyName)
	{
		global::System.Collections.Generic.IEnumerator<NotifyPropertyChangedAutoProxyAttribute> enumerator = CustomAttributeExtensions.GetCustomAttributes<NotifyPropertyChangedAutoProxyAttribute>((MemberInfo)(object)destinationType).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				NotifyPropertyChangedAutoProxyAttribute current = enumerator.Current;
				if (current != null && !(current.ProxyName != proxyName))
				{
					return true;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		return false;
	}

	public void AddProxyFor(string propertyName)
	{
		AddProxyFor(propertyName, propertyName);
	}

	public void AddProxyFor(string sourcePropertyName, string destinationPropertyName)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(sourcePropertyName) || string.IsNullOrEmpty(destinationPropertyName))
		{
			throw new ArgumentException("Invalid source property name of method");
		}
		if (_onlyAllowInvokes || _destinationOnPropertyChanged == null)
		{
			TaggedLog.Error("NotifyPropertyChangedProxySource", "AddProxyFor ignored as NotifyPropertyChangedProxySource isn't configured to handle property proxies", string.Empty);
		}
		else
		{
			HashSet<string> val = ((IReadOnlyDictionary<string, HashSet<string>>)(object)_propertyNameMap).TryGetValue<string, HashSet<string>>(sourcePropertyName) ?? new HashSet<string>();
			val.Add(destinationPropertyName);
			_propertyNameMap[sourcePropertyName] = val;
		}
	}

	public void AddInvokeFor(string sourcePropertyName, Action method)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(sourcePropertyName))
		{
			throw new ArgumentException("Source property name is null or empty");
		}
		if (method == null)
		{
			throw new ArgumentException("Action method is null");
		}
		HashSet<Action> val = default(HashSet<Action>);
		if (!_propertyActionDict.TryGetValue(sourcePropertyName, ref val))
		{
			val = new HashSet<Action>();
			_propertyActionDict[sourcePropertyName] = val;
		}
		val.Add(method);
	}

	public void SetDestinationProxyForAnySourceProperty(string destinationPropertyName)
	{
		if (_onlyAllowInvokes || _destinationOnPropertyChanged == null)
		{
			TaggedLog.Error("NotifyPropertyChangedProxySource", "SetDestinationProxyForAnySourceProperty ignored as NotifyPropertyChangedProxySource isn't configured to handle property proxies", string.Empty);
		}
		else
		{
			_anyDestinationPropertyName = destinationPropertyName;
		}
	}

	private void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs eventArgs)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		if (base.IsDisposed)
		{
			TaggedLog.Debug("NotifyPropertyChangedProxySource", "NotifyPropertyChangedProxySource proxy trigger for {0} IGNORED as we are disposed!", eventArgs.PropertyName);
			return;
		}
		HashSet<Action> val = default(HashSet<Action>);
		if (_propertyActionDict.TryGetValue(eventArgs.PropertyName, ref val))
		{
			Enumerator<Action> enumerator = val.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Action current = enumerator.Current;
					if (current != null)
					{
						current.Invoke();
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
		}
		if (_onlyAllowInvokes || _destinationOnPropertyChanged == null)
		{
			if (!_onlyAllowInvokes)
			{
				TaggedLog.Debug("NotifyPropertyChangedProxySource", "NotifyPropertyChangedProxySource proxy trigger for {0} IGNORED as destination is NULL", eventArgs.PropertyName);
			}
			return;
		}
		HashSet<string> val2 = ((IReadOnlyDictionary<string, HashSet<string>>)(object)_propertyNameMap).TryGetValue<string, HashSet<string>>(eventArgs.PropertyName);
		if (val2 != null)
		{
			Enumerator<string> enumerator2 = val2.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string current2 = enumerator2.Current;
					SendDestinationOnPropertyChanged(current2);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
			}
		}
		if (_anyDestinationPropertyName != null)
		{
			SendDestinationOnPropertyChanged(_anyDestinationPropertyName);
		}
	}

	[MethodImpl((MethodImplOptions)256)]
	private void SendDestinationOnPropertyChanged(string propertyName)
	{
		_destinationOnPropertyChanged?.Invoke(propertyName);
	}

	public override void Dispose(bool disposing)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		try
		{
			_source.PropertyChanged -= new PropertyChangedEventHandler(PropertyChangedEventHandler);
		}
		catch
		{
		}
		_propertyNameMap.Clear();
		_propertyActionDict.Clear();
		lock (lockObject)
		{
			_aggregateNotificationWatchdog?.Dispose();
			_aggregateNotificationWatchdog = null;
			if (_aggregatedPropertyNames != null)
			{
				_aggregatedPropertyNames.Clear();
				_aggregateNotificationPool.PutObject(_aggregatedPropertyNames);
				_aggregatedPropertyNames = null;
			}
		}
	}
}
