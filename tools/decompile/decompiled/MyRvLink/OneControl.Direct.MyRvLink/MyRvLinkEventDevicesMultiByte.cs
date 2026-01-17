using System;
using System.Collections.Generic;
using System.Text;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public abstract class MyRvLinkEventDevicesMultiByte<TEvent> : MyRvLinkEventDevices<TEvent> where TEvent : IMyRvLinkEvent
{
	protected const int DeviceTableIdIndex = 1;

	protected const int DeviceStatusStartIndex = 2;

	protected abstract int BytesPerDevice { get; }

	protected sealed override int MinPayloadLength => 2;

	public byte DeviceTableId => _rawData[1];

	public int DeviceCount => DeviceCountFromBufferSize(_rawData.Length);

	protected override int MaxPayloadLength(int deviceCount)
	{
		return MinPayloadLength + deviceCount * BytesPerDevice;
	}

	protected MyRvLinkEventDevicesMultiByte(byte deviceTableId, int deviceCount)
		: base(deviceCount)
	{
		_rawData[1] = deviceTableId;
	}

	protected MyRvLinkEventDevicesMultiByte(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count))
	{
		if (DeviceCount == 0)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {EventType} received {((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count} bytes which isn't enough to hold a single full device status which requires {BytesPerDevice} + {MinPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		int num = MaxPayloadLength(DeviceCount);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > num)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {EventType} received more then {num} bytes (Number of Devices {DeviceCount} Bytes Per Device {BytesPerDevice})  : {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
	}

	protected int DeviceCountFromBufferSize(int rawBufferSize)
	{
		int num = (rawBufferSize - MinPayloadLength) / BytesPerDevice;
		if (num < 0)
		{
			return 0;
		}
		return num;
	}

	public override string ToString()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		StringBuilder val = new StringBuilder($"{EventType} Table Id: 0x{DeviceTableId:X2} Total: {DeviceCount}: {ArrayExtension.DebugDump(_rawData, " ", false)}");
		try
		{
			DevicesToStringBuilder(val);
		}
		catch (global::System.Exception ex)
		{
			StringBuilder val2 = val;
			AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(31, 2, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    ERROR Trying to Get Device ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
			val2.Append(ref val3);
		}
		return ((object)val).ToString();
	}

	protected abstract void DevicesToStringBuilder(StringBuilder stringBuilder);
}
