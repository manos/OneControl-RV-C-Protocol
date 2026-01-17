using System;
using System.Runtime.CompilerServices;

namespace ids.portable.common;

public static class DebugUtils
{
	public static ValueTuple<int, string> GetLineInfo([CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = "")
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return new ValueTuple<int, string>(lineNumber, caller);
	}
}
