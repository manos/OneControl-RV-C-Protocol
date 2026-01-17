using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandResponseFailure : MyRvLinkCommandEvent, IMyRvLinkCommandResponseFailure, IMyRvLinkCommandResponse, IMyRvLinkCommandEvent, IMyRvLinkEvent
{
	protected const int CommandFailureCodeIndex = 4;

	protected const int CommandExtraDataStartIndex = 5;

	private byte[]? _encodedData;

	public CommandResult CommandResult => ToCommandResult(FailureCode);

	[field: CompilerGenerated]
	public MyRvLinkCommandResponseFailureCode FailureCode
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	protected override int MinPayloadLength
	{
		[CompilerGenerated]
		get;
	} = 5;

	private static CommandResult ToCommandResult(MyRvLinkCommandResponseFailureCode failureCode)
	{
		return (CommandResult)(failureCode switch
		{
			MyRvLinkCommandResponseFailureCode.Success => 0, 
			MyRvLinkCommandResponseFailureCode.Offline => 6, 
			MyRvLinkCommandResponseFailureCode.DeviceInUse => 8, 
			MyRvLinkCommandResponseFailureCode.CommandTimeout => 5, 
			MyRvLinkCommandResponseFailureCode.CommandNotSupported => 10, 
			MyRvLinkCommandResponseFailureCode.CommandAborted => 1, 
			MyRvLinkCommandResponseFailureCode.DeviceHazardousToOperate => 11, 
			MyRvLinkCommandResponseFailureCode.SessionTimeout => 4, 
			MyRvLinkCommandResponseFailureCode.CantOpenSession => 8, 
			_ => 7, 
		});
	}

	public override string ToString()
	{
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 0)
		{
			return $"Command(0x{base.ClientCommandId:X4}) {base.CommandResponseType} {FailureCode}/{CommandResult}: {ArrayExtension.DebugDump(base.ExtendedData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count, " ", false)}";
		}
		return $"Command(0x{base.ClientCommandId:X4}) {base.CommandResponseType} {FailureCode}/{CommandResult}";
	}

	protected static MyRvLinkCommandResponseType MakeFailureCommandResponseType(bool commandCompleted)
	{
		if (!commandCompleted)
		{
			return MyRvLinkCommandResponseType.FailureMultipleResponse;
		}
		return MyRvLinkCommandResponseType.FailureCompleted;
	}

	public MyRvLinkCommandResponseFailure(ushort clientCommandId, bool commandCompleted, MyRvLinkCommandResponseFailureCode failureCode, global::System.Collections.Generic.IReadOnlyList<byte>? extendedData = null)
		: base(clientCommandId, MakeFailureCommandResponseType(commandCompleted), 5, extendedData)
	{
		FailureCode = failureCode;
	}

	public MyRvLinkCommandResponseFailure(ushort clientCommandId, MyRvLinkCommandResponseFailureCode failureCode, global::System.Collections.Generic.IReadOnlyList<byte>? extendedData = null)
		: this(clientCommandId, commandCompleted: true, failureCode, extendedData)
	{
	}

	public MyRvLinkCommandResponseFailure(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData, MyRvLinkCommandEvent.DecodeCommandResponseType(rawData), 5)
	{
		FailureCode = DecodeStandardFailureCode(rawData);
	}

	protected static MyRvLinkCommandResponseFailureCode DecodeStandardFailureCode(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return (MyRvLinkCommandResponseFailureCode)decodeBuffer[4];
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
		_encodedData[4] = (byte)FailureCode;
		return _encodedData;
	}
}
