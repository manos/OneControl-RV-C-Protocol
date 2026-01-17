using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using OneControl.Devices.BootLoader;

namespace OneControl.Direct.MyRvLink.Events;

internal class MyRvLinkBootLoaderStatus : MyRvLinkEvent<MyRvLinkBootLoaderStatus>
{
	private const int MaxPayloadLength = 5;

	private const int DeviceTableIdIndex = 1;

	private const int DeviceIdIndex = 2;

	private const int ReFlashVersionIndex = 3;

	private const int ReFlashVersionSize = 1;

	private const int SPBVersionSize = 1;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.ReFlashBootloader;

	protected override int MinPayloadLength => 2;

	[field: CompilerGenerated]
	protected override byte[] _rawData
	{
		[CompilerGenerated]
		get;
	}

	public int DeviceId => _rawData[2];

	public byte DeviceTableId => _rawData[1];

	protected MyRvLinkBootLoaderStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateEventRawDataBasic(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 5)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkBootLoaderStatus)} received more then {5} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkBootLoaderStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkBootLoaderStatus(rawData);
	}

	public LogicalDeviceReflashBootLoaderStatus GetStatus()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		LogicalDeviceReflashBootLoaderStatus val = new LogicalDeviceReflashBootLoaderStatus();
		((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 3, 2), 2, false);
		return val;
	}

	public override string ToString()
	{
		return $"DeviceId: {DeviceId} DeviceTableId: {DeviceTableId} Raw data: {ArrayExtension.DebugDump(_rawData, " ", false)}";
	}
}
