using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandLeveler3ButtonCommand : MyRvLinkCommand
{
	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int ScreenEnumIndex = 5;

	private const int ButtonState1Index = 6;

	private const int ButtonState2Index = 7;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandLeveler3ButtonCommand";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.Leveler3ButtonCommand;

	protected override int MinPayloadLength => 8;

	private int MaxPayloadLength => MinPayloadLength;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public byte ScreenEnum => _rawData[5];

	public byte ButtonStateData1 => _rawData[6];

	public byte ButtonStateData2 => _rawData[7];

	public int DeviceCount => 1;

	public MyRvLinkCommandLeveler3ButtonCommand(ushort clientCommandId, byte deviceTableId, byte deviceId, LogicalDeviceLevelerCommandType3 command)
	{
		_rawData = new byte[MaxPayloadLength];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		byte[] array = ((LogicalDeviceCommandPacket)command).CopyCurrentData();
		int num = 5;
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			_rawData[num++] = b;
		}
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandLeveler3ButtonCommand(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > MaxPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {MaxPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandLeveler3ButtonCommand Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandLeveler3ButtonCommand(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public virtual string ToString()
	{
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X2}, Table Id: 0x{DeviceTableId:X2}, DeviceId: 0x{DeviceId:X2}, UI Mode: 0x{ScreenEnum:X2}, Button State Data: 0x{ButtonStateData1:X2} 0x{ButtonStateData2:X2} Raw Data: {ArrayExtension.DebugDump(_rawData, " ", false)} ";
	}
}
