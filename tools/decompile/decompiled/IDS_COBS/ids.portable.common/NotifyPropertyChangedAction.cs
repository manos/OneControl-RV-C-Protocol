using System;
using System.ComponentModel;
using System.Reflection;

namespace IDS.Portable.Common;

public class NotifyPropertyChangedAction<TSource, TDestination> : CommonDisposable
{
	private const string LogTag = "NotifyPropertyChangedAction";

	private readonly NotifyPropertyChangedProxySource _proxySource;

	private readonly PropertyInfo _sourceProperty;

	private readonly Func<TSource, TDestination>? _sourceToDestinationConverter;

	private readonly Action<TDestination> _destinationAction;

	public NotifyPropertyChangedAction(INotifyPropertyChanged source, string sourcePropertyName, Action<TDestination> destinationAction, Func<TSource, TDestination>? converter = null)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		if (source == null)
		{
			throw new ArgumentException("Invalid source source");
		}
		global::System.Type type = ((object)source).GetType();
		if (type == (global::System.Type)null)
		{
			throw new ArgumentException("Invalid sourceType source");
		}
		if (string.IsNullOrEmpty(sourcePropertyName))
		{
			throw new ArgumentException("Invalid or null sourcePropertyName");
		}
		_sourceProperty = type.GetProperty(sourcePropertyName);
		if (_sourceProperty == (PropertyInfo)null)
		{
			throw new ArgumentException("Invalid property sourcePropertyName");
		}
		_sourceToDestinationConverter = converter;
		_destinationAction = destinationAction ?? throw new ArgumentNullException("destinationAction");
		_proxySource = new NotifyPropertyChangedProxySource(source);
		_proxySource.AddInvokeFor(sourcePropertyName, new Action(DoSourcePropertyChanged));
		DoSourcePropertyChanged();
	}

	private void DoSourcePropertyChanged()
	{
		try
		{
			if (!base.IsDisposed)
			{
				((Action<_003F>)(object)_destinationAction).Invoke(GetValue());
			}
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Error("NotifyPropertyChangedAction", "{0} Unable to convert property: {1}", "DoSourcePropertyChanged", ex.Message);
		}
	}

	public TDestination GetValue()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(((object)this).ToString());
		}
		PropertyInfo sourceProperty = _sourceProperty;
		object obj = ((sourceProperty != null) ? sourceProperty.GetValue((object)_proxySource.Source, (object[])null) : null);
		if (_sourceToDestinationConverter == null)
		{
			if (obj is TDestination)
			{
				return (TDestination)obj;
			}
			throw new global::System.Exception($"No converter specified and source value can't be auto converted to expected type of {typeof(TDestination)}");
		}
		if (!(obj is TSource val))
		{
			throw new global::System.Exception($"Source value can't be converted to expected type of {typeof(TSource)}");
		}
		return _sourceToDestinationConverter.Invoke(val);
	}

	public override void Dispose(bool disposing)
	{
		_proxySource.TryDispose();
	}
}
