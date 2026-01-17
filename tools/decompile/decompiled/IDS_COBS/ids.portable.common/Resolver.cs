using System;
using Serilog;

namespace IDS.Portable.Common;

public static class Resolver<TRegistered>
{
	private static bool _created = false;

	private static TRegistered _resolved;

	private static Func<TRegistered>? _instanceCreator;

	private static readonly object lockObject = new object();

	public static TRegistered Resolve => PerformResolve();

	private static TRegistered PerformResolve()
	{
		if (_created)
		{
			return _resolved;
		}
		lock (lockObject)
		{
			if (_instanceCreator == null)
			{
				throw new ResolverNotRegistered(typeof(TRegistered));
			}
			if (_created)
			{
				return _resolved;
			}
			_resolved = _instanceCreator.Invoke();
			if (_resolved == null)
			{
				Log.Warning($"Resolver<{typeof(TRegistered)}> resolved to a null value.");
			}
			_created = true;
			return _resolved;
		}
	}

	public static void Register(Func<TRegistered> instanceCreator)
	{
		LazyConstructAndRegister(instanceCreator);
		PerformResolve();
	}

	public static void Register<TImplementation>() where TImplementation : TRegistered, new()
	{
		LazyConstructAndRegister<TImplementation>();
		PerformResolve();
	}

	public static void LazyConstructAndRegister(Func<TRegistered> instanceCreator)
	{
		lock (typeof(TRegistered))
		{
			if (_created)
			{
				throw new ResolverAlreadyRegisteredForCreatedObject(typeof(TRegistered));
			}
			_instanceCreator = instanceCreator ?? throw new ResolverNotRegistered(typeof(TRegistered));
		}
	}

	public static void LazyConstructAndRegister<TImplementation>() where TImplementation : TRegistered, new()
	{
		lock (typeof(TRegistered))
		{
			if (_created)
			{
				throw new ResolverAlreadyRegisteredForCreatedObject(typeof(TRegistered));
			}
			_instanceCreator = () => (TRegistered)(object)new TImplementation();
		}
	}
}
