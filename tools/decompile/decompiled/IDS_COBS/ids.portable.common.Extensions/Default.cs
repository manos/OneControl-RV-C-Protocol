using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace IDS.Portable.Common.Extensions;

public static class Default<TValue> where TValue : struct, IConvertible
{
	public static TValue Value;

	public static bool HasCustomDefaultValue;

	static Default()
	{
		global::System.Type typeFromHandle = typeof(TValue);
		object[] customAttributes = ((MemberInfo)typeFromHandle).GetCustomAttributes(typeof(DefaultValueAttribute), true);
		object obj = ((customAttributes != null) ? Enumerable.FirstOrDefault<object>((global::System.Collections.Generic.IEnumerable<object>)customAttributes) : null);
		DefaultValueAttribute val = (DefaultValueAttribute)((obj is DefaultValueAttribute) ? obj : null);
		if (val != null)
		{
			if (val.Value is TValue value)
			{
				Value = value;
				HasCustomDefaultValue = true;
			}
			else
			{
				TaggedLog.Error("Default", "Default value isn't of correct type expected '{0}' using system default", ((MemberInfo)typeFromHandle).Name);
			}
		}
	}
}
