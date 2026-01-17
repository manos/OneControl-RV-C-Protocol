using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkGatewayInformation : MyRvLinkEvent<MyRvLinkGatewayInformation>
{
	private const int MaxPayloadLength = 13;

	private const int ProtocolVersionStartIndex = 1;

	private const int OptionsIndex = 2;

	private const int DeviceCountIndex = 3;

	private const int DeviceTableIdIndex = 4;

	private const int DeviceTableCrcStartIndex = 5;

	private const int DeviceMetadataCrcStartIndex = 9;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.GatewayInformation;

	protected override int MinPayloadLength => 13;

	[field: CompilerGenerated]
	protected override byte[] _rawData
	{
		[CompilerGenerated]
		get;
	}

	public MyRvLinkProtocolVersionMajor ProtocolVersionMajor => (MyRvLinkProtocolVersionMajor)_rawData[1];

	public MyRvLinkGatewayInformationOptions Options => (MyRvLinkGatewayInformationOptions)_rawData[2];

	public bool IsProductionMode => !((global::System.Enum)Options).HasFlag((global::System.Enum)MyRvLinkGatewayInformationOptions.ConfigurationMode);

	public int DeviceCount => _rawData[3];

	public byte DeviceTableId => _rawData[4];

	public uint DeviceTableCrc => ArrayExtension.GetValueUInt32(_rawData, 5, (Endian)0);

	public uint DeviceMetadataTableCrc => ArrayExtension.GetValueUInt32(_rawData, 9, (Endian)0);

	public bool IsExactDeviceTableMatch(byte deviceTableId, uint deviceTableCrc)
	{
		if (DeviceTableId == deviceTableId)
		{
			return DeviceTableCrc == deviceTableCrc;
		}
		return false;
	}

	public MyRvLinkGatewayInformation(byte protocolVersion, MyRvLinkGatewayInformationOptions options, byte deviceCount, byte deviceTableId, uint deviceTableCrc, uint deviceMetadataTableCrc)
	{
		_rawData = new byte[13];
		_rawData[0] = (byte)EventType;
		_rawData[1] = protocolVersion;
		_rawData[2] = (byte)options;
		_rawData[3] = deviceCount;
		_rawData[4] = deviceTableId;
		ArrayExtension.SetValueUInt32(_rawData, deviceTableCrc, 5, (Endian)0);
		ArrayExtension.SetValueUInt32(_rawData, deviceMetadataTableCrc, 9, (Endian)0);
	}

	protected MyRvLinkGatewayInformation(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateEventRawDataBasic(rawData);
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 13)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkGatewayInformation)} received more then {13} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
	}

	public static MyRvLinkGatewayInformation Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkGatewayInformation(rawData);
	}

	public override string ToString()
	{
		return $"Version: 0x{ProtocolVersionMajor:X}, Devices: {DeviceCount}, Table Id: 0x{DeviceTableId:X2} CRC: 0x{DeviceTableCrc:X4}/0x{DeviceMetadataTableCrc:X4}, Options: {EnumExtensions.DebugDumpAsFlags<MyRvLinkGatewayInformationOptions>(Options)}: {ArrayExtension.DebugDump(_rawData, " ", false)}";
	}
}
