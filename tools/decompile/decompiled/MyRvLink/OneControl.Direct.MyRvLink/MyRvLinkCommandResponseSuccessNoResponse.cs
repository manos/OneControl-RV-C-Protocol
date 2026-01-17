using System.Runtime.CompilerServices;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandResponseSuccessNoResponse : IMyRvLinkCommandResponseSuccess, IMyRvLinkCommandResponse, IMyRvLinkCommandEvent, IMyRvLinkEvent
{
	public MyRvLinkEventType EventType => MyRvLinkEventType.DeviceCommand;

	public CommandResult CommandResult => (CommandResult)0;

	[field: CompilerGenerated]
	public ushort ClientCommandId
	{
		[CompilerGenerated]
		get;
	}

	public bool IsCommandCompleted => false;

	public MyRvLinkCommandResponseSuccessNoResponse()
	{
		ClientCommandId = 65535;
	}
}
