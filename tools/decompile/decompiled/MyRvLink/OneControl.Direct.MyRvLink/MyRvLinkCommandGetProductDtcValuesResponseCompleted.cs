using System;
using System.Collections.Generic;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetProductDtcValuesResponseCompleted : MyRvLinkCommandResponseSuccess
{
	protected const int DtcCountIndex = 0;

	protected override int MinExtendedDataLength => 1;

	public byte DtcCount => base.ExtendedData[0];

	public MyRvLinkCommandGetProductDtcValuesResponseCompleted(ushort clientCommandId, byte dtcCount)
		: base(clientCommandId, commandCompleted: true, EncodeExtendedData(dtcCount))
	{
	}

	public MyRvLinkCommandGetProductDtcValuesResponseCompleted(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandGetProductDtcValuesResponseCompleted(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(byte dtcCount)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		int num = 1;
		byte[] array = new byte[num];
		array[0] = dtcCount;
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(array, 0, num);
	}

	public override string ToString()
	{
		return $"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandGetDevicesMetadataResponseCompleted"} DTC Count: {DtcCount}";
	}
}
