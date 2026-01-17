using System;
using System.Reflection;

namespace IDS.Portable.Common;

public class ResolverNotRegistered : global::System.Exception
{
	public ResolverNotRegistered(global::System.Type type)
		: base("No resolver registered for " + ((MemberInfo)type).Name)
	{
	}
}
