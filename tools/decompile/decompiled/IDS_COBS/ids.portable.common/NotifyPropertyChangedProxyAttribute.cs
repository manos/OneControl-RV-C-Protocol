using System;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

[AttributeUsage(/*Could not decode attribute arguments.*/)]
public class NotifyPropertyChangedProxyAttribute : global::System.Attribute
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

	[field: CompilerGenerated]
	public string? DestinationPropertyName
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public NotifyPropertyChangedProxyAttribute(string proxyName, [CallerMemberName] string propertyName = "")
		: this(proxyName, propertyName, propertyName)
	{
	}

	public NotifyPropertyChangedProxyAttribute(string proxyName, string sourcePropertyName, [CallerMemberName] string destinationPropertyName = "")
	{
		ProxyName = proxyName;
		SourcePropertyName = sourcePropertyName;
		DestinationPropertyName = destinationPropertyName;
	}
}
