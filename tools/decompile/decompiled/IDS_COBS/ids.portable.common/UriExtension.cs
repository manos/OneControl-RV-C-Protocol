using System;

namespace IDS.Portable.Common;

public static class UriExtension
{
	public static Uri? TryMakeUri(string uriString)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return string.IsNullOrWhiteSpace(uriString) ? ((Uri)null) : new Uri(uriString);
		}
		catch
		{
			return null;
		}
	}
}
