using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandResponseSuccess : MyRvLinkCommandEvent, IMyRvLinkCommandResponseSuccess, IMyRvLinkCommandResponse, IMyRvLinkCommandEvent, IMyRvLinkEvent
{
	protected const int CommandExtraDataStartIndex = 4;

	private byte[]? _encodedData;

	public CommandResult CommandResult => (CommandResult)0;

	[field: CompilerGenerated]
	protected override int MinPayloadLength
	{
		[CompilerGenerated]
		get;
	} = 4;

	protected static MyRvLinkCommandResponseType MakeSuccessCommandResponseType(bool commandCompleted)
	{
		if (!commandCompleted)
		{
			return MyRvLinkCommandResponseType.SuccessMultipleResponse;
		}
		return MyRvLinkCommandResponseType.SuccessCompleted;
	}

	public MyRvLinkCommandResponseSuccess(ushort clientCommandId, bool commandCompleted, global::System.Collections.Generic.IReadOnlyList<byte>? extendedData = null)
		: base(clientCommandId, MakeSuccessCommandResponseType(commandCompleted), 4, extendedData)
	{
	}

	public MyRvLinkCommandResponseSuccess(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData, MyRvLinkCommandEvent.DecodeCommandResponseType(rawData), 4)
	{
	}

	public global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		if (_encodedData != null)
		{
			return _encodedData;
		}
		int count = ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count;
		int num = MinPayloadLength + count;
		_encodedData = new byte[num];
		EncodeBaseEventIntoBuffer(_encodedData);
		return _encodedData;
	}

	public override string ToString()
	{
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 0)
		{
			return $"Command(0x{base.ClientCommandId:X4}) {base.CommandResponseType}: {ArrayExtension.DebugDump(base.ExtendedData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count, " ", false)}";
		}
		return $"Command(0x{base.ClientCommandId:X4}) {base.CommandResponseType}";
	}
}
