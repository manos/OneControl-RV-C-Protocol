using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

[AttributeUsage(/*Could not decode attribute arguments.*/)]
public class NotifyPropertyChangedInvokeFieldAttribute : global::System.Attribute, INotifyPropertyChangedInvokeAttribute
{
	private static string LogTag = "NotifyPropertyChangedInvokeFieldAttribute";

	[field: CompilerGenerated]
	public string ProxyName
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public string SourcePropertyName
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public string MethodName
	{
		[CompilerGenerated]
		get;
	}

	public NotifyPropertyChangedInvokeFieldAttribute(string proxyName, string propertyName, string invokeMethodName)
	{
		ProxyName = proxyName;
		SourcePropertyName = propertyName;
		MethodName = invokeMethodName;
	}

	internal Action MakeInvokeMethod(FieldInfo fieldInfo, object destination)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		if (fieldInfo == (FieldInfo)null)
		{
			throw new ArgumentNullException("fieldInfo");
		}
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		if (MethodName == null)
		{
			throw new global::System.Exception("MethodName is null.");
		}
		return (Action)delegate
		{
			object value = fieldInfo.GetValue(destination);
			MethodInfo val = value?.GetType().GetMethod(MethodName, global::System.Type.EmptyTypes);
			if (val == (MethodInfo)null)
			{
				TaggedLog.Warning(LogTag, "Invoke Method {0}.{1} failed because method or object is null", ((MemberInfo)fieldInfo).Name, MethodName);
			}
			else
			{
				((MethodBase)val).Invoke(value, (object[])null);
			}
		};
	}
}
