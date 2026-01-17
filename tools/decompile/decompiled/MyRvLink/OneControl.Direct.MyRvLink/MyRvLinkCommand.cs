using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public abstract class MyRvLinkCommand : IMyRvLinkCommand
{
	private const string LogTag = "MyRvLinkCommand";

	protected const int CommandTypeIndex = 2;

	protected const int ClientCommandIdStartIndex = 0;

	protected abstract int MinPayloadLength { get; }

	public abstract MyRvLinkCommandType CommandType { get; }

	public abstract ushort ClientCommandId { get; }

	[field: CompilerGenerated]
	public MyRvLinkResponseState ResponseState
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		protected set;
	}

	protected virtual void ValidateCommand(global::System.Collections.Generic.IReadOnlyList<byte> rawData, ushort? clientCommandId = null)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (rawData == null)
		{
			throw new ArgumentNullException("rawData", "No data was given to validate!");
		}
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count < MinPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkCommandGetDevices)} because less then {MinPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		if (CommandType != DecodeCommandType(rawData))
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkCommandGetDevices)} command type doesn't match {CommandType}: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		if (clientCommandId.HasValue && clientCommandId.Value != DecodeClientCommandId(rawData))
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkCommandGetDevices)} client command id doesn't match {CommandType}: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
	}

	public virtual bool ProcessResponse(IMyRvLinkCommandResponse commandResponse)
	{
		if (ResponseState != MyRvLinkResponseState.Pending)
		{
			TaggedLog.Debug("MyRvLinkCommand", $"Ignoring Process Command Response because command is {ResponseState}: {commandResponse}", global::System.Array.Empty<object>());
			return true;
		}
		if (!(commandResponse is MyRvLinkCommandResponseSuccess myRvLinkCommandResponseSuccess))
		{
			if (commandResponse is MyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
			{
				TaggedLog.Debug("MyRvLinkCommand", $"Command failed {myRvLinkCommandResponseFailure} Completed: {myRvLinkCommandResponseFailure.IsCommandCompleted} Type: {((object)myRvLinkCommandResponseFailure).GetType()}", global::System.Array.Empty<object>());
				if (myRvLinkCommandResponseFailure.IsCommandCompleted)
				{
					ResponseState = MyRvLinkResponseState.Failed;
				}
			}
			else
			{
				TaggedLog.Debug("MyRvLinkCommand", $"Unexpected response received {commandResponse}", global::System.Array.Empty<object>());
				ResponseState = MyRvLinkResponseState.Failed;
			}
		}
		else
		{
			TaggedLog.Debug("MyRvLinkCommand", $"Command Success Completed={myRvLinkCommandResponseSuccess.IsCommandCompleted}", global::System.Array.Empty<object>());
			if (myRvLinkCommandResponseSuccess.IsCommandCompleted)
			{
				ResponseState = MyRvLinkResponseState.Completed;
			}
		}
		return ResponseState != MyRvLinkResponseState.Pending;
	}

	protected static ushort DecodeClientCommandId(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return ArrayExtension.GetValueUInt16(decodeBuffer, 0, (Endian)0);
	}

	public static MyRvLinkCommandType DecodeCommandType(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return (MyRvLinkCommandType)decodeBuffer[2];
	}

	public abstract global::System.Collections.Generic.IReadOnlyList<byte> Encode();

	public virtual IMyRvLinkCommandEvent DecodeCommandEvent(IMyRvLinkCommandEvent commandEvent)
	{
		return commandEvent;
	}
}
