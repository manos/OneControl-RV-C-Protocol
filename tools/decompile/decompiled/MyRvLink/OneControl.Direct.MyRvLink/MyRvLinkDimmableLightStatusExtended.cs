using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDimmableLightStatusExtended : MyRvLinkEvent<MyRvLinkDimmableLightStatusExtended>
{
	private const int PayloadLength = 11;

	private const int MaxPayloadLength = 11;

	private const int DeviceTableIdIndex = 1;

	private const int DeviceIdIndex = 2;

	private const int StatusIndex = 3;

	private const int StatusExtendedSize = 8;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.DimmableLightExtendedStatus;

	[field: CompilerGenerated]
	protected override byte[] _rawData
	{
		[CompilerGenerated]
		get;
	}

	protected override int MinPayloadLength => 11;

	public int DeviceId => _rawData[2];

	public byte DeviceTableId => _rawData[1];

	protected MyRvLinkDimmableLightStatusExtended(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateEventRawDataBasic(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 11)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkDimmableLightStatusExtended)} received more then {11} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkDimmableLightStatusExtended Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkDimmableLightStatusExtended(rawData);
	}

	public LogicalDeviceLightDimmableStatusExtended GetExtendedStatus()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		LogicalDeviceLightDimmableStatusExtended val = new LogicalDeviceLightDimmableStatusExtended();
		((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 3, 8), 8, false);
		return val;
	}

	public override string ToString()
	{
		return $"DeviceId: {DeviceId} DeviceTableId: {DeviceTableId} Raw data: {ArrayExtension.DebugDump(_rawData, " ", false)}";
	}
}
