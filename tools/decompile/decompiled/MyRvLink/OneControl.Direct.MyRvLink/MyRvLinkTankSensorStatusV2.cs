using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

internal class MyRvLinkTankSensorStatusV2 : MyRvLinkEvent<MyRvLinkTankSensorStatusV2>
{
	public const string LogTag = "MyRvLinkTankSensorStatusV2";

	public const int DeviceTableIdIndex = 1;

	public const int DeviceIdIndex = 2;

	public const int StatusIndex = 3;

	public const int HeaderSize = 3;

	public const int StatusMinSize = 1;

	public const int StatusMaxSize = 8;

	private const int _minPayloadLength = 4;

	private const int _maxPayloadLength = 11;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.TankSensorStatusV2;

	protected override int MinPayloadLength => 4;

	[field: CompilerGenerated]
	protected override byte[] _rawData
	{
		[CompilerGenerated]
		get;
	}

	public byte DeviceId => _rawData[2];

	public byte DeviceTableId => _rawData[1];

	protected MyRvLinkTankSensorStatusV2(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateEventRawDataBasic(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 11)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {"MyRvLinkTankSensorStatusV2"} received more than {11} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkTankSensorStatusV2 Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkTankSensorStatusV2(rawData);
	}

	public LogicalDeviceTankSensorStatus? GetTankSensorStatus()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		LogicalDeviceTankSensorStatus val = new LogicalDeviceTankSensorStatus();
		try
		{
			switch (_rawData.Length)
			{
			case 4:
				((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 3, 1), 1, false);
				break;
			case 11:
				((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 3, 8), 8, false);
				break;
			default:
				throw new MyRvLinkDecoderException($"Unable to decode data for {"MyRvLinkTankSensorStatusV2"} into {"LogicalDeviceTankSensorStatus"} because size of {_rawData.Length} was unexpected.");
			}
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Warning("MyRvLinkTankSensorStatusV2", "Unable to update status " + ex.Message, global::System.Array.Empty<object>());
		}
		return val;
	}

	public override string ToString()
	{
		return $"DeviceId: {DeviceId} DeviceTableId: {DeviceTableId} Raw data: {ArrayExtension.DebugDump(_rawData, " ", false)}";
	}
}
