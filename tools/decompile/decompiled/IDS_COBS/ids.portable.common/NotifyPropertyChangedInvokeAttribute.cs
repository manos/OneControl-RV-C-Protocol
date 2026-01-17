using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

[AttributeUsage(/*Could not decode attribute arguments.*/)]
public class NotifyPropertyChangedInvokeAttribute : global::System.Attribute, INotifyPropertyChangedInvokeAttribute
{
	[field: CompilerGenerated]
	public string ProxyName
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	public string SourcePropertyName
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public NotifyPropertyChangedInvokeAttribute(string proxyName, string propertyName)
	{
		ProxyName = proxyName;
		SourcePropertyName = propertyName;
	}

	internal Action MakeInvokeMethod(MethodInfo methodInfo, object destination)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		if (methodInfo == (MethodInfo)null)
		{
			throw new ArgumentNullException("methodInfo");
		}
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		if (((MethodBase)methodInfo).GetParameters().Length != 0)
		{
			throw new global::System.Exception("Given methodInfo must take zero arguments to be invoked");
		}
		return (Action)delegate
		{
			((MethodBase)methodInfo).Invoke(destination, (object[])null);
		};
	}
}
