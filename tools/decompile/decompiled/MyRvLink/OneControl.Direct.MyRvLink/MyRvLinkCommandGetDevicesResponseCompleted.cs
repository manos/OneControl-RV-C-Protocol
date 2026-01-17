using System;
using System.Collections.Generic;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetDevicesResponseCompleted : MyRvLinkCommandResponseSuccess
{
	protected const int DeviceTableCrcIndex = 0;

	protected const int DeviceCountIndex = 4;

	protected override int MinExtendedDataLength => 5;

	public uint DeviceTableCrc => ArrayExtension.GetValueUInt32(base.ExtendedData, 0, (Endian)0);

	public byte DeviceCount => base.ExtendedData[4];

	public MyRvLinkCommandGetDevicesResponseCompleted(ushort clientCommandId, uint deviceTableCrc, byte deviceCount)
		: base(clientCommandId, commandCompleted: true, EncodeExtendedData(deviceTableCrc, deviceCount))
	{
	}

	public MyRvLinkCommandGetDevicesResponseCompleted(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandGetDevicesResponseCompleted(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(uint deviceTableCrc, byte deviceCount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		int num = 5;
		byte[] array = new byte[num];
		ArrayExtension.SetValueUInt32(array, deviceTableCrc, 0, (Endian)0);
		array[4] = deviceCount;
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(array, 0, num);
	}

	public override string ToString()
	{
		return $"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandGetDevicesResponseCompleted"} ResponseReceivedDeviceTableCrc: {DeviceTableCrc:x4} DeviceCount: {DeviceCount}";
	}
}
