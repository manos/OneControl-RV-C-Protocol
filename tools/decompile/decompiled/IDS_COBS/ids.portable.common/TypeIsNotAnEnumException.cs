using System;
using System.Reflection;

namespace IDS.Portable.Common;

public class TypeIsNotAnEnumException : global::System.Exception
{
	public TypeIsNotAnEnumException(global::System.Type type)
		: base("The type " + ((MemberInfo)type).Name + " is not an enum")
	{
	}
}
