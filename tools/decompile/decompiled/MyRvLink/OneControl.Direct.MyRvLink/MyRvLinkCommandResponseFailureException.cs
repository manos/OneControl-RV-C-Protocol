using System.Runtime.CompilerServices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandResponseFailureException : MyRvLinkException
{
	[field: CompilerGenerated]
	public IMyRvLinkCommandResponseFailure Failure
	{
		[CompilerGenerated]
		get;
	}

	public MyRvLinkCommandResponseFailureException(IMyRvLinkCommandResponseFailure failure)
		: base(((object)failure).ToString())
	{
		Failure = failure;
	}
}
