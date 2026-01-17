using System;
using System.Reflection;

namespace IDS.Portable.Common;

public class Singleton<TSingleton> where TSingleton : class
{
	protected static readonly object SingletonLocker = new object();

	private static volatile TSingleton _instance;

	public static TSingleton Instance
	{
		get
		{
			if (_instance != null)
			{
				return _instance;
			}
			lock (SingletonLocker)
			{
				if (_instance != null)
				{
					return _instance;
				}
				TaggedLog.Information("Singleton", "Attempting to make a singleton of type " + ((MemberInfo)typeof(TSingleton)).Name);
				try
				{
					_instance = MakeSingleton();
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Error("Singleton", "Unable to create singleton instance for {0} {1}\n{2}", ((MemberInfo)typeof(TSingleton)).Name, ex.Message, ex.StackTrace);
					throw;
				}
				return _instance;
			}
		}
	}

	private static TSingleton MakeSingleton()
	{
		ConstructorInfo[] constructors = typeof(TSingleton).GetConstructors((BindingFlags)36);
		if (!global::System.Array.Exists<ConstructorInfo>(constructors, (Predicate<ConstructorInfo>)((ConstructorInfo ci) => ((MethodBase)ci).GetParameters().Length == 0)))
		{
			throw new ConstructorNotFoundException("Non-public ctor() not found.");
		}
		return global::System.Array.Find<ConstructorInfo>(constructors, (Predicate<ConstructorInfo>)((ConstructorInfo ci) => ((MethodBase)ci).GetParameters().Length == 0)).Invoke(new object[0]) as TSingleton;
	}
}
