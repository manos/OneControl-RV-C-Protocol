using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using OneControl.Devices.BrakingSystem;

namespace OneControl.Direct.MyRvLink;

internal class MyRvLinkBrakingSystemStatus : MyRvLinkEvent<MyRvLinkBrakingSystemStatus>
{
	public const string LogTag = "MyRvLinkBrakingSystemStatus";

	public const int DeviceTableIdIndex = 1;

	public const int DeviceIdIndex = 2;

	public const int StatusIndex = 3;

	public const int AbsStatusPayloadSizeV1 = 6;

	public const int AbsStatusPayloadSizeV2 = 8;

	public const int AbsStatusSizeV1 = 9;

	public const int AbsStatusSizeV2 = 11;

	private const int MaxPayloadLength = 11;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.BrakingSystemStatus;

	protected override int MinPayloadLength => 9;

	[field: CompilerGenerated]
	protected override byte[] _rawData
	{
		[CompilerGenerated]
		get;
	}

	public byte DeviceId => _rawData[2];

	public byte DeviceTableId => _rawData[1];

	protected MyRvLinkBrakingSystemStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateEventRawDataBasic(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 11)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkBrakingSystemStatus)} received more than {11} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkBrakingSystemStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkBrakingSystemStatus(rawData);
	}

	public LogicalDeviceBrakingSystemStatus? GetBrakingSystemStatus()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		LogicalDeviceBrakingSystemStatus val = new LogicalDeviceBrakingSystemStatus();
		try
		{
			switch (_rawData.Length)
			{
			case 9:
				((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 3, 6), 6, false);
				break;
			case 11:
				((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 3, 8), 8, false);
				break;
			default:
				throw new MyRvLinkDecoderException($" received invalid data size of {_rawData.Length} when {9} or {11} was expected, raw bytes: {ArrayExtension.DebugDump(_rawData, " ", false)}");
			}
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Warning("MyRvLinkBrakingSystemStatus", $"Unable to parse braking system status for {typeof(MyRvLinkBrakingSystemStatus)} {ex.Message}", global::System.Array.Empty<object>());
			return null;
		}
		return val;
	}

	public override string ToString()
	{
		return $"DeviceId: {DeviceId} DeviceTableId: {DeviceTableId} Raw data: {ArrayExtension.DebugDump(_rawData, " ", false)}";
	}
}
