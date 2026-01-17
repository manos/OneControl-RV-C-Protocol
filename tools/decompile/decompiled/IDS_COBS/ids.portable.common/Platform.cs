using System;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

public static class Platform
{
	[field: CompilerGenerated]
	public static Func<string, bool>? IsPackageInstalledHandler
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	public static bool IsPackageInstalled(string packageName)
	{
		return IsPackageInstalledHandler?.Invoke(packageName) ?? false;
	}
}
