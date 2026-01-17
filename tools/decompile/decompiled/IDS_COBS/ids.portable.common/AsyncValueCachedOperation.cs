using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

public class AsyncValueCachedOperation<TValue>
{
	public TValue ValueToRevertTo
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	public TValue ValueNew
	{
		[CompilerGenerated]
		get;
	}

	public AsyncValueCachedOperation(TValue valueToRevertTo, TValue valueNew)
	{
		ValueToRevertTo = valueToRevertTo;
		ValueNew = valueNew;
	}
}
