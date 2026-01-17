using System;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

[AttributeUsage(/*Could not decode attribute arguments.*/)]
public class NotifyPropertyChangedAutoProxyAttribute : global::System.Attribute
{
	[field: CompilerGenerated]
	public string ProxyName
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public NotifyPropertyChangedAutoProxyAttribute(string proxyName)
	{
		ProxyName = proxyName;
	}
}
