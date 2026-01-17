using System;
using System.Reflection;

namespace IDS.Portable.Common;

public class ConvertObjectToEnumException : global::System.Exception
{
	public ConvertObjectToEnumException(global::System.Type type, object fromValue)
		: base($"Unable to convert {fromValue} into the enum {((MemberInfo)type).Name}")
	{
	}
}
