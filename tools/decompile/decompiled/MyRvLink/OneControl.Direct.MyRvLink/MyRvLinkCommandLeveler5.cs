using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using OneControl.Devices.Leveler.Type5;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandLeveler5 : MyRvLinkCommand
{
	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int DeviceCommandStartIndex = 5;

	private const int DeviceCommandByteIndex = 6;

	private const int CommandDataCommandTypeBitIndex = 0;

	private const int MinimumCommandLength = 1;

	private const int CancelCurrentCommandLength = 1;

	private const int AutoLevelCommandLength = 3;

	private const int AutoHitchCommandLength = 1;

	private const int AutoRetractCommandLength = 3;

	private const int AutoExtendCommandLength = 3;

	private const int ManualMovementCommandLength = 3;

	private const int SetZeroPointCommandLength = 3;

	private const int SetConfigCommandLength = 3;

	private const int ManualUnrestrictedMovementCommandLength = 3;

	private const int NumberOfAutoHitchBytesToRemove = 2;

	private readonly byte[] _rawData;

	private const int MaxPayloadLength = 8;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandLeveler5";

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	protected override int MinPayloadLength => 6;

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.Leveler5Command;

	public MyRvLinkCommandLeveler5(ushort clientCommandId, byte deviceTableId, byte deviceId, ILogicalDeviceLevelerCommandType5 command)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected I4, but got Unknown
		global::System.Collections.Generic.IReadOnlyList<byte> readOnlyList = ((IDeviceCommandPacket)command).RawData;
		int num = ((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count;
		int num2;
		if (((IDeviceCommandPacket)command).CommandByte == 0)
		{
			num2 = 5;
			num = 1;
		}
		else
		{
			if (((IDeviceCommandPacket)command).CommandByte != 96)
			{
				throw new MyRvLinkDecoderException($"Invalid/Unknown Leveler Command Code {((IDeviceCommandPacket)command).CommandByte}");
			}
			if (num < 1)
			{
				throw new MyRvLinkDecoderException($"Unable to decode data for {LogTag} because size is {((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count} when expecting at least {6} bytes: {ArrayExtension.DebugDump(readOnlyList, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count, " ", false)}");
			}
			LevelerOperationCode val = (LevelerOperationCode)readOnlyList[0];
			switch ((int)val)
			{
			case 0:
				throw new MyRvLinkDecoderException($"Invalid Leveler Operation Code {0}");
			case 1:
				num2 = 7;
				break;
			case 2:
				num -= 2;
				readOnlyList = ReadOnlyList.ToNewArray<byte>(readOnlyList, 0, num);
				num2 = 5;
				break;
			case 3:
				num2 = 7;
				break;
			case 8:
				num2 = 7;
				break;
			case 4:
				num2 = 7;
				break;
			case 5:
				num2 = 7;
				break;
			case 6:
				num2 = 7;
				break;
			case 7:
				num2 = 7;
				break;
			default:
				throw new MyRvLinkDecoderException($"Unable to decode data for {LogTag} because {readOnlyList[6]} does not match a known command. bytes: {ArrayExtension.DebugDump(readOnlyList, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count, " ", false)}");
			}
		}
		if (num2 != 4 + num)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {LogTag} because size is {((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count} which does not match the expected size for the command: {ArrayExtension.DebugDump(readOnlyList, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count, " ", false)}");
		}
		_rawData = new byte[num2 + 1];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		int num3 = 5;
		global::System.Collections.Generic.IEnumerator<byte> enumerator = ((global::System.Collections.Generic.IEnumerable<byte>)readOnlyList).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				byte current = enumerator.Current;
				_rawData[num3++] = current;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandLeveler5(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 8)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {8} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count < MinPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received less then {MinPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandLeveler5 Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandLeveler5(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public override IMyRvLinkCommandEvent DecodeCommandEvent(IMyRvLinkCommandEvent commandEvent)
	{
		if (!(commandEvent is MyRvLinkCommandResponseSuccess result))
		{
			if (commandEvent is MyRvLinkCommandResponseFailure response)
			{
				return new MyRvLinkCommandLeveler5ResponseFailure(response);
			}
			TaggedLog.Warning(LogTag, $"DecodeCommandEvent unexpected event type for {commandEvent}", global::System.Array.Empty<object>());
			return commandEvent;
		}
		return result;
	}

	public virtual string ToString()
	{
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, Device Id: 0x{DeviceId:X2}]: {ArrayExtension.DebugDump(_rawData, " ", false)} ";
	}
}
