using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Core.IDS_CAN;
using IDS.Core.Types;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandSetDevicePid : MyRvLinkCommand
{
	private const int MaxPidValueSize = 6;

	private const int MaxPayloadLength = 15;

	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int PidIdIndex = 5;

	private const int PidSessionIdIndex = 7;

	private const int PidValueIndex = 9;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandSetDevicePid";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.SetDevicePid;

	protected override int MinPayloadLength => 9;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public Pid Pid => (Pid)ArrayExtension.GetValueUInt16(_rawData, 5, (Endian)0);

	public SESSION_ID SessionId => SESSION_ID.op_Implicit(ArrayExtension.GetValueUInt16(_rawData, 7, (Endian)0));

	public UInt48 PidValue => DecodePidValue();

	public MyRvLinkCommandSetDevicePid(ushort clientCommandId, byte deviceTableId, byte deviceId, Pid pidId, SESSION_ID sessionId, UInt48 pidValue, LogicalDeviceSessionType pidWriteAccess)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected I4, but got Unknown
		int minPayloadLength = MinPayloadLength;
		byte[] array = new byte[6];
		int num = ArrayExtension.SetValueBigEndianRemoveLeadingZeros(array, UInt48.op_Implicit(pidValue), 0, 6);
		_rawData = new byte[minPayloadLength + num];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		ArrayExtension.SetValueUInt16(_rawData, (ushort)(int)pidId, 5, (Endian)0);
		ArrayExtension.SetValueUInt16(_rawData, SESSION_ID.op_Implicit(sessionId), 7, (Endian)0);
		for (int i = 0; i < num; i++)
		{
			_rawData[i + minPayloadLength] = array[i];
		}
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandSetDevicePid(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 15)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkCommandSetDevicePid)} because greater then {15} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandSetDevicePid Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandSetDevicePid(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	protected UInt48 DecodePidValue()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if (_rawData == null || _rawData.Length <= MinPayloadLength)
		{
			return UInt48.op_Implicit((byte)0);
		}
		UInt48 val = UInt48.op_Implicit((byte)0);
		for (int i = 9; i < _rawData.Length; i++)
		{
			val <<= 8;
			val |= UInt48.op_Implicit(_rawData[i]);
		}
		return val;
	}

	public override IMyRvLinkCommandEvent DecodeCommandEvent(IMyRvLinkCommandEvent commandEvent)
	{
		if (commandEvent is MyRvLinkCommandResponseSuccess myRvLinkCommandResponseSuccess)
		{
			if (myRvLinkCommandResponseSuccess.IsCommandCompleted)
			{
				return new MyRvLinkCommandSetDevicePidResponseCompleted(myRvLinkCommandResponseSuccess);
			}
			TaggedLog.Debug(LogTag, $"DecodeCommandEvent for {LogTag} Received UNEXPECTED event of {myRvLinkCommandResponseSuccess}", global::System.Array.Empty<object>());
			return commandEvent;
		}
		return commandEvent;
	}

	public virtual string ToString()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X4} Pid: {Pid} SessionId: {SessionId} Value: 0x{PidValue:X}]";
	}
}
