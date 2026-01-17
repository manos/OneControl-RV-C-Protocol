using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetFirmwareInformation : MyRvLinkCommand
{
	private const int MaxPayloadLength = 4;

	private const int FirmwareInformationCodeIndex = 3;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandGetFirmwareInformation";

	protected override int MinPayloadLength => 4;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.GetFirmwareInformation;

	public FirmwareInformationCode FirmwareInformationCode => (FirmwareInformationCode)_rawData[3];

	public bool IsCommandCompleted
	{
		get
		{
			if (base.ResponseState != MyRvLinkResponseState.Pending)
			{
				return CompletedCommandResponse != null;
			}
			return false;
		}
	}

	[field: CompilerGenerated]
	public IMyRvLinkCommandResponse? CompletedCommandResponse
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public MyRvLinkCommandGetFirmwareInformation(ushort clientCommandId, FirmwareInformationCode firmwareInformationCode)
	{
		_rawData = new byte[4];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = (byte)firmwareInformationCode;
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandGetFirmwareInformation(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 4)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {4} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandGetFirmwareInformation Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandGetFirmwareInformation(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public override bool ProcessResponse(IMyRvLinkCommandResponse commandResponse)
	{
		if (base.ResponseState != MyRvLinkResponseState.Pending)
		{
			TaggedLog.Debug(LogTag, $"Ignoring Process Command Response because command is {base.ResponseState}: {commandResponse}", global::System.Array.Empty<object>());
			return true;
		}
		if (!base.ProcessResponse(commandResponse))
		{
			return false;
		}
		CompletedCommandResponse = commandResponse;
		return true;
	}

	public override IMyRvLinkCommandEvent DecodeCommandEvent(IMyRvLinkCommandEvent commandEvent)
	{
		if (!(commandEvent is MyRvLinkCommandResponseSuccess response))
		{
			if (commandEvent is MyRvLinkCommandResponseFailure commandEvent2)
			{
				return base.DecodeCommandEvent(commandEvent2);
			}
			TaggedLog.Warning(LogTag, $"DecodeCommandEvent unexpected event type for {commandEvent}", global::System.Array.Empty<object>());
			return commandEvent;
		}
		MyRvLinkCommandGetFirmwareInformationResponseSuccess myRvLinkCommandGetFirmwareInformationResponseSuccess = new MyRvLinkCommandGetFirmwareInformationResponseSuccess(response);
		switch (myRvLinkCommandGetFirmwareInformationResponseSuccess.FirmwareInformationCode)
		{
		case FirmwareInformationCode.Version:
			return new MyRvLinkCommandGetFirmwareInformationResponseSuccessVersion(myRvLinkCommandGetFirmwareInformationResponseSuccess);
		case FirmwareInformationCode.Cpu:
			return new MyRvLinkCommandGetFirmwareInformationResponseSuccessCpu(myRvLinkCommandGetFirmwareInformationResponseSuccess);
		default:
			TaggedLog.Warning(LogTag, $"Unknown FirmwareInformationCode {myRvLinkCommandGetFirmwareInformationResponseSuccess.FirmwareInformationCode}: {myRvLinkCommandGetFirmwareInformationResponseSuccess}", global::System.Array.Empty<object>());
			return myRvLinkCommandGetFirmwareInformationResponseSuccess;
		}
	}

	public virtual string ToString()
	{
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, {FirmwareInformationCode} Response: {((object)CompletedCommandResponse)?.ToString() ?? "Pending"}]";
	}
}
