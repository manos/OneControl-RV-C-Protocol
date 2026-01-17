using System;
using System.Reflection;
using IDS.Portable.Common.Extensions;

namespace IDS.Portable.Common;

public static class Enum<TValue> where TValue : struct, IConvertible
{
	public static TValue TryConvert(object? fromValue)
	{
		try
		{
			Convert(fromValue, out var toValue, enableDefaultValue: true);
			return toValue;
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Error("EnumExtensions", "TryConvert failed from {0}({1}) to {2}: {3}", (fromValue != null) ? ((MemberInfo)fromValue.GetType()).Name : null, fromValue, ((MemberInfo)typeof(TValue)).Name, ex.Message);
			return Default<TValue>.Value;
		}
	}

	public static bool TryConvert(object fromValue, out TValue toValue)
	{
		try
		{
			return Convert(fromValue, out toValue, enableDefaultValue: true);
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Error("EnumExtensions", "TryConvert failed from {0}({1}) to {2}: {3}", ((MemberInfo)fromValue.GetType()).Name, fromValue, ((MemberInfo)typeof(TValue)).Name, ex.Message);
			toValue = Default<TValue>.Value;
			return false;
		}
	}

	public static bool Convert(object fromValue, out TValue toValue, bool enableDefaultValue = false)
	{
		global::System.Type typeFromHandle = typeof(TValue);
		if (!typeFromHandle.IsEnum)
		{
			throw new TypeIsNotAnEnumException(typeFromHandle);
		}
		if (!((MemberInfo)typeFromHandle).IsDefined(typeof(FlagsAttribute), false))
		{
			if (!(fromValue is string) && fromValue.GetType() != global::System.Enum.GetUnderlyingType(typeFromHandle))
			{
				IConvertible val = (IConvertible)((fromValue is IConvertible) ? fromValue : null);
				if (val != null)
				{
					fromValue = Convert.ChangeType((object)val, global::System.Enum.GetUnderlyingType(typeFromHandle));
				}
			}
			if (!global::System.Enum.IsDefined(typeFromHandle, fromValue))
			{
				if (!enableDefaultValue)
				{
					throw new ConvertObjectToEnumException(typeFromHandle, fromValue);
				}
				toValue = Default<TValue>.Value;
				return false;
			}
		}
		if (fromValue is string text)
		{
			toValue = (TValue)global::System.Enum.Parse(typeFromHandle, text);
		}
		else
		{
			toValue = (TValue)global::System.Enum.ToObject(typeFromHandle, fromValue);
		}
		return true;
	}
}
