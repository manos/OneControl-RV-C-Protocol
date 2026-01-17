using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

public static class JsonSerializer
{
	private const string LogTag = "JsonSerializer";

	private static readonly HashSet<Assembly> AutoRegisteredJsonSerializers = new HashSet<Assembly>();

	public static void AutoRegisterJsonSerializersFromAssembly(Assembly assembly)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		lock (AutoRegisteredJsonSerializers)
		{
			if (AutoRegisteredJsonSerializers.Contains(assembly))
			{
				return;
			}
			_ = assembly.GetName().Name;
			global::System.Type[] types = assembly.GetTypes();
			foreach (global::System.Type type in types)
			{
				if (!type.IsInterface && typeof(IJsonSerializerClass).IsAssignableFrom(type))
				{
					RuntimeHelpers.RunClassConstructor(type.TypeHandle);
				}
			}
			AutoRegisteredJsonSerializers.Add(assembly);
		}
	}
}
