using System;
using System.Collections.Generic;
using System.Reflection;

namespace IDS.Portable.Common;

public static class TypeRegistry
{
	public const string LogTag = "TypeRegistry";

	private static readonly Dictionary<string, global::System.Type> TypeDict = new Dictionary<string, global::System.Type>();

	public static void Register(string typeName, global::System.Type type)
	{
		if (type == (global::System.Type)null)
		{
			((IDictionary<string, global::System.Type>)(object)TypeDict)?.TryRemove<string, global::System.Type>(typeName);
		}
		else
		{
			TypeDict[typeName] = type;
		}
	}

	public static global::System.Type? Lookup(string typeName, bool autoRegister = true)
	{
		global::System.Type type = ((IReadOnlyDictionary<string, global::System.Type>)(object)TypeDict).TryGetValue<string, global::System.Type>(typeName);
		if (type == (global::System.Type)null)
		{
			type = global::System.Type.GetType(typeName) ?? FindType(typeName);
			if (type != (global::System.Type)null)
			{
				TaggedLog.Debug("TypeRegistry", "Type {0} not registered but was auto resolved.", type);
				if (autoRegister)
				{
					TaggedLog.Debug("TypeRegistry", "{0} = {1} is being auto registered.", typeName, type);
					Register(typeName, type);
				}
			}
		}
		return type;
	}

	public static global::System.Type? FindType(string typeName)
	{
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		foreach (Assembly val in assemblies)
		{
			global::System.Type type = val.GetType(typeName);
			if (type != (global::System.Type)null)
			{
				TaggedLog.Debug("TypeRegistry", "Found type {0} in assembly {1}", typeName, val.GetName().Name);
				return type;
			}
		}
		return null;
	}
}
