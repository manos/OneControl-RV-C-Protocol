using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using OneControl.Devices.AccessoryGateway;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandActionAccessoryGateway : MyRvLinkCommand
{
	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int DeviceCommandIndex = 5;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandActionAccessoryGateway";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.ActionAccessoryGateway;

	protected override int MinPayloadLength => 6;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public LogicalDeviceAccessoryGatewayCommand Command
	{
		get
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			int num = 6;
			return LogicalDeviceAccessoryGatewayCommand.MakeCommand(_rawData[5], (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, num, _rawData.Length - num));
		}
	}

	public MyRvLinkCommandActionAccessoryGateway(ushort clientCommandId, byte deviceTableId, byte deviceId, LogicalDeviceAccessoryGatewayCommand command)
	{
		global::System.Collections.Generic.IReadOnlyList<byte> rawData = ((LogicalDeviceCommandPacket)command).RawData;
		int num = MinPayloadLength + ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count + 1 - 1;
		_rawData = new byte[num];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		_rawData[5] = ((LogicalDeviceCommandPacket)command).CommandByte;
		for (int i = 0; i < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count; i++)
		{
			_rawData[5 + i + 1] = rawData[i];
		}
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandActionAccessoryGateway(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandActionAccessoryGateway Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandActionAccessoryGateway(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public virtual string ToString()
	{
		try
		{
			return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, Device Id: 0x{DeviceId:X2}, Command: {Command}]: {ArrayExtension.DebugDump(_rawData, " ", false)} ";
		}
		catch
		{
			return $"{LogTag}[Client Command: {"MyRvLinkCommandActionAccessoryGateway"}, UNABLE TO DECODE]: {ArrayExtension.DebugDump(_rawData, " ", false)}";
		}
	}
}
