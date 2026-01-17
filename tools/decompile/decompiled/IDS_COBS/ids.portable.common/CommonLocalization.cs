using System;
using System.Runtime.CompilerServices;

namespace ids.portable.common;

public static class CommonLocalization
{
	[field: CompilerGenerated]
	public static Func<string?, string?>? LocalizationMethod
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	public static string? Localize(string? originalStr)
	{
		return LocalizationMethod?.Invoke(originalStr) ?? originalStr;
	}
}
