using System;
using System.Reflection;

namespace IDS.Portable.Common;

public class ResolverDefaultConstructorNotFound : global::System.Exception
{
	public ResolverDefaultConstructorNotFound(global::System.Type type)
		: base("Default constructor not found for " + ((MemberInfo)type).Name)
	{
	}
}
