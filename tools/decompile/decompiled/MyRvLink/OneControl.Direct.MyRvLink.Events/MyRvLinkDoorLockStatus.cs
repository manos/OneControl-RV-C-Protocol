using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using OneControl.Devices.DoorLock;

namespace OneControl.Direct.MyRvLink.Events;

internal class MyRvLinkDoorLockStatus : MyRvLinkEvent<MyRvLinkDoorLockStatus>
{
	private const int MaxPayloadLength = 7;

	private const int DeviceTableIdIndex = 1;

	private const int DeviceIdIndex = 2;

	private const int StatusIndex = 3;

	private const int DoorLockStatusSize = 4;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.DoorLockStatus;

	[field: CompilerGenerated]
	protected override byte[] _rawData
	{
		[CompilerGenerated]
		get;
	}

	protected override int MinPayloadLength => 2;

	public int DeviceId => _rawData[2];

	public byte DeviceTableId => _rawData[1];

	protected MyRvLinkDoorLockStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateEventRawDataBasic(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 7)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkDoorLockStatus)} received more then {7} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkDoorLockStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkDoorLockStatus(rawData);
	}

	public LogicalDeviceDoorLockStatus GetStatus()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		LogicalDeviceDoorLockStatus val = new LogicalDeviceDoorLockStatus();
		((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 3, 4), 4, false);
		return val;
	}

	public override string ToString()
	{
		return $"DeviceId: {DeviceId} DeviceTableId: {DeviceTableId} Raw data: {ArrayExtension.DebugDump(_rawData, " ", false)}";
	}
}
