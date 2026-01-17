using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandLeveler4ButtonCommand : MyRvLinkCommand
{
	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int DeviceModeIndex = 5;

	private const int UiModeIndex = 6;

	private const int UiButtonData1Index = 7;

	private const int UiButtonData2Index = 8;

	private const int UiButtonData3Index = 9;

	public const int UIButtonDataSize = 3;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandLeveler4ButtonCommand";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.Leveler4ButtonCommand;

	protected override int MinPayloadLength => 10;

	private int MaxPayloadLength => MinPayloadLength;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public byte DeviceMode => _rawData[5];

	public byte UiMode => _rawData[6];

	public byte UiButtonData1 => _rawData[7];

	public byte UiButtonData2 => _rawData[8];

	public byte UiButtonData3 => _rawData[9];

	public int DeviceCount => 1;

	public MyRvLinkCommandLeveler4ButtonCommand(ushort clientCommandId, byte deviceTableId, byte deviceId, ILogicalDeviceLevelerCommandType4 command)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		_rawData = new byte[MaxPayloadLength];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		_rawData[5] = ((IDeviceCommandPacket)command).CommandByte;
		ILogicalDeviceLevelerCommandButtonPressedType4 val = (ILogicalDeviceLevelerCommandButtonPressedType4)(object)((command is ILogicalDeviceLevelerCommandButtonPressedType4) ? command : null);
		if (val == null)
		{
			if (!(command is LogicalDeviceLevelerCommandAbortType4))
			{
				LogicalDeviceLevelerCommandBackType4 val2 = (LogicalDeviceLevelerCommandBackType4)(object)((command is LogicalDeviceLevelerCommandBackType4) ? command : null);
				if (val2 == null)
				{
					if (!(command is LogicalDeviceLevelerCommandHomeType4))
					{
						throw new MyRvLinkDecoderException($"Unable to decode command {command}");
					}
				}
				else
				{
					_rawData[6] = (byte)val2.ScreenSelected;
				}
			}
		}
		else
		{
			_rawData[6] = (byte)((ILogicalDeviceLevelerCommandWithScreenSelectionType4)val).ScreenSelected;
			global::System.Collections.Generic.IReadOnlyList<byte> rawButtonData = ((ILogicalDeviceLevelerCommandWithScreenSelectionType4)val).RawButtonData;
			if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawButtonData).Count != 3)
			{
				throw new MyRvLinkDecoderException($"Unable to decode command {command}, expected button data to be {3} but was {((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawButtonData).Count}");
			}
			for (int i = 0; i < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawButtonData).Count; i++)
			{
				_rawData[7 + i] = rawButtonData[i];
			}
		}
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandLeveler4ButtonCommand(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > MaxPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {MaxPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandLeveler4ButtonCommand Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandLeveler4ButtonCommand(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public virtual string ToString()
	{
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, DeviceId: 0x{DeviceId:X2}, Device Mode: 0x{DeviceMode:X2}, UI Mode: 0x{UiMode:X2}, UI Button Data: 0x{UiButtonData1:X2} 0x{UiButtonData2:X2} 0x{UiButtonData3:X2} Raw Data: {ArrayExtension.DebugDump(_rawData, " ", false)} ";
	}
}
