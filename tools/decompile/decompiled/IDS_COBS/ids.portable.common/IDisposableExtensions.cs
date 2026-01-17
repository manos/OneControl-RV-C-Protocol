using System;

namespace IDS.Portable.Common;

public static class IDisposableExtensions
{
	public static void TryDispose(this global::System.IDisposable instance)
	{
		try
		{
			instance.Dispose();
		}
		catch
		{
		}
	}
}
