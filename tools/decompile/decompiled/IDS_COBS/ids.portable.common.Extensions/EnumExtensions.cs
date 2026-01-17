using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace IDS.Portable.Common.Extensions;

public static class EnumExtensions
{
	private const string LogTag = "EnumExtensions";

	public static global::System.Collections.Generic.IEnumerable<TEnum> GetValues<TEnum>() where TEnum : struct, IConvertible
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (!typeof(TEnum).IsEnum)
		{
			throw new ArgumentException($"{typeof(TEnum)} must be an enum type");
		}
		return Enumerable.Cast<TEnum>((global::System.Collections.IEnumerable)global::System.Enum.GetValues(typeof(TEnum)));
	}

	public static T? GetAttribute<T>(this global::System.Enum instance, bool inherit = true) where T : global::System.Attribute
	{
		T result = null;
		try
		{
			result = CustomAttributeExtensions.GetCustomAttribute<T>((MemberInfo)(object)((object)instance).GetType().GetField(((object)instance).ToString()), inherit);
			return result;
		}
		catch
		{
		}
		return result;
	}

	public static TValue GetAttributeValue<TAttribute, TValue>(this global::System.Enum instance, Func<TAttribute, TValue> getValue, bool inherit = true) where TAttribute : global::System.Attribute
	{
		TAttribute attribute = instance.GetAttribute<TAttribute>(inherit);
		return getValue.Invoke(attribute);
	}

	public static TValue GetAttributeValue<TAttribute, TValue>(this global::System.Enum instance, TValue defaultValue = default(TValue), bool inherit = true) where TAttribute : global::System.Attribute, IAttributeValue<TValue>
	{
		TAttribute attribute = instance.GetAttribute<TAttribute>(inherit);
		if (attribute == null)
		{
			return defaultValue;
		}
		return attribute.Value;
	}

	public static bool TryGetDescription(this global::System.Enum instance, out string? description)
	{
		DescriptionAttribute attribute = instance.GetAttribute<DescriptionAttribute>(inherit: true);
		description = ((attribute != null) ? attribute.Description : null);
		return attribute != null;
	}

	public static string? Description(this global::System.Enum instance)
	{
		if (!instance.TryGetDescription(out string description))
		{
			return ((object)instance).ToString();
		}
		return description;
	}

	public static int EnumToInt<TEnum>(this TEnum value) where TEnum : struct, IConvertible
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (!typeof(TEnum).IsEnum)
		{
			throw new ArgumentException($"{typeof(TEnum)} must be an enum type");
		}
		return Convert.ToInt32((object)value);
	}

	public static string DebugDumpAsFlags<TEnum>(this TEnum enumFlags) where TEnum : struct, IConvertible
	{
		return Convert.ToUInt32((object)enumFlags).DebugDumpAsFlags<TEnum>();
	}

	public static string DebugDumpAsFlags<TEnum>(this uint enumFlagsInt) where TEnum : struct, IConvertible
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (!typeof(TEnum).IsEnum)
		{
			throw new ArgumentException($"{typeof(TEnum)} must be an enum type");
		}
		List<string> val = new List<string>();
		global::System.Collections.Generic.IEnumerator<TEnum> enumerator = GetValues<TEnum>().GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				TEnum current = enumerator.Current;
				if ((enumFlagsInt & Convert.ToUInt32((object)current)) != 0)
				{
					val.Add(((IConvertible)current/*cast due to .constrained prefix*/).ToString((IFormatProvider)(object)CultureInfo.InvariantCulture));
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		string text = ((val.Count == 0) ? "none" : string.Join(", ", (global::System.Collections.Generic.IEnumerable<string>)val));
		return $"0x{enumFlagsInt:X4} ({text})";
	}

	public static TEnum UpdateFlag<TEnum>(this TEnum enumFlags, TEnum flags, bool setFlags) where TEnum : struct, IConvertible
	{
		if (!setFlags)
		{
			return enumFlags.ClearFlag(flags);
		}
		return enumFlags.SetFlag(flags);
	}

	public static TEnum SetFlag<TEnum>(this TEnum enumFlags, TEnum flags) where TEnum : struct, IConvertible
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (!typeof(TEnum).IsEnum)
		{
			throw new ArgumentException($"{typeof(TEnum)} must be an enum type");
		}
		int num = Convert.ToInt32((object)enumFlags);
		int num2 = Convert.ToInt32((object)flags);
		if ((num & num2) != num2)
		{
			num |= num2;
			return (TEnum)global::System.Enum.ToObject(typeof(TEnum), num);
		}
		return enumFlags;
	}

	public static TEnum ClearFlag<TEnum>(this TEnum enumFlags, TEnum flag) where TEnum : struct, IConvertible
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (!typeof(TEnum).IsEnum)
		{
			throw new ArgumentException($"{typeof(TEnum)} must be an enum type");
		}
		int num = Convert.ToInt32((object)enumFlags);
		int num2 = Convert.ToInt32((object)flag);
		if ((num & num2) != 0)
		{
			num &= ~num2;
			return (TEnum)global::System.Enum.ToObject(typeof(TEnum), num);
		}
		return enumFlags;
	}
}
