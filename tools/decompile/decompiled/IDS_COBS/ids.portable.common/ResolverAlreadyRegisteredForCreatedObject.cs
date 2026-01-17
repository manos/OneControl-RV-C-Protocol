using System;
using System.Reflection;

namespace IDS.Portable.Common;

public class ResolverAlreadyRegisteredForCreatedObject : global::System.Exception
{
	public ResolverAlreadyRegisteredForCreatedObject(global::System.Type type)
		: base("Can't register a new resolver for " + ((MemberInfo)type).Name + " because instance already created!")
	{
	}
}
