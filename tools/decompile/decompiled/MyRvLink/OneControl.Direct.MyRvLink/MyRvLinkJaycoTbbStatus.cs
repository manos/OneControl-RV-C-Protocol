using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using IDS.Portable.Devices.JaycoTbbGateway;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkJaycoTbbStatus : MyRvLinkEvent<MyRvLinkJaycoTbbStatus>
{
	private const int MaxPayloadLength = 9;

	private const int DeviceTableIdIndex = 1;

	private const int DeviceIdIndex = 2;

	private const int AddressIndex = 3;

	private const int DataIndex = 5;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.JaycoTbbStatus;

	[field: CompilerGenerated]
	protected override byte[] _rawData
	{
		[CompilerGenerated]
		get;
	}

	protected override int MinPayloadLength => 2;

	public int DeviceId => _rawData[2];

	public byte DeviceTableId => _rawData[1];

	public ushort Address => ArrayExtension.GetValueUInt16(_rawData, 3, (Endian)0);

	public uint Data => _rawData.Length switch
	{
		6 => _rawData[5], 
		7 => ArrayExtension.GetValueUInt16(_rawData, 5, (Endian)0), 
		8 => (uint)((_rawData[5] << 16) | (_rawData[6] << 8) | _rawData[7]), 
		9 => ArrayExtension.GetValueUInt32(_rawData, 5, (Endian)0), 
		_ => 0u, 
	};

	protected MyRvLinkJaycoTbbStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateEventRawDataBasic(rawData);
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 9)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkJaycoTbbStatus)} received more then {9} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
	}

	public static MyRvLinkJaycoTbbStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkJaycoTbbStatus(rawData);
	}

	public LogicalDeviceJaycoTbbStatus GetStatus()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		LogicalDeviceJaycoTbbStatus val = new LogicalDeviceJaycoTbbStatus();
		int num = _rawData.Length - 3;
		((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 3, num), num, false);
		return val;
	}

	public override string ToString()
	{
		return $"DeviceId: {DeviceId} DeviceTableId: {DeviceTableId} Address: {Address} Data: {Data} Raw data: {ArrayExtension.DebugDump(_rawData, " ", false)}";
	}
}
