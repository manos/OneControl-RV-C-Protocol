using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Core.IDS_CAN;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandSetDevicePidWithAddress : MyRvLinkCommand
{
	private const int RequiredPidAddressSize = 2;

	private const int MaxPidValueSize = 4;

	private const int MaxPayloadLength = 15;

	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int PidIdIndex = 5;

	private const int PidSessionIdIndex = 7;

	private const int PidAddressIndex = 9;

	private const int PidValueIndex = 11;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandSetDevicePidWithAddress";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.SetDevicePidWithAddress;

	protected override int MinPayloadLength => 11;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public Pid Pid => (Pid)ArrayExtension.GetValueUInt16(_rawData, 5, (Endian)0);

	public ushort Address => ArrayExtension.GetValueUInt16(_rawData, 9, (Endian)0);

	public SESSION_ID SessionId => SESSION_ID.op_Implicit(ArrayExtension.GetValueUInt16(_rawData, 7, (Endian)0));

	public uint PidValue => DecodePidValue();

	public MyRvLinkCommandSetDevicePidWithAddress(ushort clientCommandId, byte deviceTableId, byte deviceId, Pid pidId, SESSION_ID sessionId, ushort pidAddress, uint pidValue, LogicalDeviceSessionType pidWriteAccess)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected I4, but got Unknown
		int minPayloadLength = MinPayloadLength;
		byte[] array = new byte[4];
		int num = ArrayExtension.SetValueBigEndianRemoveLeadingZeros(array, (ulong)pidValue, 0, 4);
		_rawData = new byte[minPayloadLength + num];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		ArrayExtension.SetValueUInt16(_rawData, (ushort)(int)pidId, 5, (Endian)0);
		ArrayExtension.SetValueUInt16(_rawData, SESSION_ID.op_Implicit(sessionId), 7, (Endian)0);
		ArrayExtension.SetValueUInt16(_rawData, pidAddress, 9, (Endian)0);
		for (int i = 0; i < num; i++)
		{
			_rawData[i + minPayloadLength] = array[i];
		}
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandSetDevicePidWithAddress(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 15)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkCommandSetDevicePidWithAddress)} because greater then {15} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandSetDevicePidWithAddress Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandSetDevicePidWithAddress(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	protected uint DecodePidValue()
	{
		if (_rawData.Length <= MinPayloadLength)
		{
			return 0u;
		}
		int num = MathCommon.Clamp(_rawData.Length - 11, 0, 4) + 11;
		uint num2 = 0u;
		for (int i = 11; i < num; i++)
		{
			num2 <<= 8;
			num2 |= _rawData[i];
		}
		return num2;
	}

	public override IMyRvLinkCommandEvent DecodeCommandEvent(IMyRvLinkCommandEvent commandEvent)
	{
		if (commandEvent is MyRvLinkCommandResponseSuccess myRvLinkCommandResponseSuccess)
		{
			if (myRvLinkCommandResponseSuccess.IsCommandCompleted)
			{
				return new MyRvLinkCommandSetDevicePidWithAddressResponseCompleted(myRvLinkCommandResponseSuccess);
			}
			TaggedLog.Debug(LogTag, $"DecodeCommandEvent for {LogTag} Received UNEXPECTED event of {myRvLinkCommandResponseSuccess}", global::System.Array.Empty<object>());
			return commandEvent;
		}
		return commandEvent;
	}

	public virtual string ToString()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X4} Pid: {Pid} SessionId: {SessionId} Value: 0x{PidValue:X}]";
	}
}
