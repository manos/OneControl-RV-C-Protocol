using System;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

[AttributeUsage(/*Could not decode attribute arguments.*/)]
public class NotifyPropertyChangedAutoProxyIgnoreAttribute : global::System.Attribute
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
	public string PropertyName
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public NotifyPropertyChangedAutoProxyIgnoreAttribute(string proxyName, [CallerMemberName] string propertyName = "")
	{
		ProxyName = proxyName;
		PropertyName = propertyName;
	}
}
