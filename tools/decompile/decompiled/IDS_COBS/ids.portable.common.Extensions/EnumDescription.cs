using System;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common.Extensions;

[Obsolete("EnumDescription is deprecated, please use the system attribute 'Description' for this functionality and use TryGetDescription/Description instead of GetAttributeValue")]
[AttributeUsage(/*Could not decode attribute arguments.*/)]
public class EnumDescription : global::System.Attribute, IAttributeValue<string>
{
	[field: CompilerGenerated]
	public string Description
	{
		[CompilerGenerated]
		get;
	}

	public string Value => Description;

	public EnumDescription(string description)
	{
		Description = description;
	}
}
