using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using OneControl.Devices.BatteryMonitor;

namespace OneControl.Direct.MyRvLink.Events;

internal class MyRvLinkBatteryMonitorStatus : MyRvLinkEvent<MyRvLinkBatteryMonitorStatus>
{
	private const int PayloadVersion1Length = 13;

	private const int PayloadVersion2Length = 19;

	private const int MaxPayloadLength = 19;

	private const int DeviceTableIdIndex = 1;

	private const int DeviceIdIndex = 2;

	private const int StatusIndex = 3;

	private const int ExtendedStatusIndex = 11;

	private const int BatteryMonitorStatusSize = 8;

	private const int BatteryMonitorExtendedStatusVersion1Size = 2;

	private const int BatteryMonitorExtendedStatusVersion2Size = 8;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.BatteryMonitorStatus;

	[field: CompilerGenerated]
	protected override byte[] _rawData
	{
		[CompilerGenerated]
		get;
	}

	protected override int MinPayloadLength => 2;

	public int DeviceId => _rawData[2];

	public byte DeviceTableId => _rawData[1];

	protected MyRvLinkBatteryMonitorStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateEventRawDataBasic(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 19)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkBatteryMonitorStatus)} received more then {19} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkBatteryMonitorStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkBatteryMonitorStatus(rawData);
	}

	public ValueTuple<LogicalDeviceBatteryMonitorStatus, LogicalDeviceBatteryMonitorStatusExtended> GetStatusAndExtendedStatus()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		LogicalDeviceBatteryMonitorStatus val = new LogicalDeviceBatteryMonitorStatus();
		((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 3, 8), 8, false);
		LogicalDeviceBatteryMonitorStatusExtended val2 = new LogicalDeviceBatteryMonitorStatusExtended();
		switch (_rawData.Length)
		{
		case 13:
			((LogicalDeviceDataPacketMutableDoubleBuffer)val2).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 11, 2), 2, false);
			break;
		case 19:
			((LogicalDeviceDataPacketMutableDoubleBuffer)val2).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 11, 8), 8, false);
			break;
		default:
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkBatteryMonitorStatus)} unexpected payload size {_rawData.Length} bytes: {ArrayExtension.DebugDump(_rawData, " ", false)}");
		}
		return new ValueTuple<LogicalDeviceBatteryMonitorStatus, LogicalDeviceBatteryMonitorStatusExtended>(val, val2);
	}

	public override string ToString()
	{
		return $"DeviceId: {DeviceId} DeviceTableId: {DeviceTableId} Raw data: {ArrayExtension.DebugDump(_rawData, " ", false)}";
	}
}
