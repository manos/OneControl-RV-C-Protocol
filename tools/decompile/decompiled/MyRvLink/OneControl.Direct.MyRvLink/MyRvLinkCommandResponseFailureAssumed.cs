using System.Runtime.CompilerServices;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandResponseFailureAssumed : IMyRvLinkCommandResponseFailure, IMyRvLinkCommandResponse, IMyRvLinkCommandEvent, IMyRvLinkEvent
{
	public MyRvLinkEventType EventType => MyRvLinkEventType.DeviceCommand;

	public CommandResult CommandResult => (CommandResult)12;

	[field: CompilerGenerated]
	public ushort ClientCommandId
	{
		[CompilerGenerated]
		get;
	}

	public bool IsCommandCompleted => false;

	[field: CompilerGenerated]
	public MyRvLinkCommandResponseFailureCode FailureCode
	{
		[CompilerGenerated]
		get;
	}

	public MyRvLinkCommandResponseFailureAssumed(ushort clientCommandId, MyRvLinkCommandResponseFailureCode originalFailureCode)
	{
		ClientCommandId = clientCommandId;
		FailureCode = originalFailureCode;
	}
}
