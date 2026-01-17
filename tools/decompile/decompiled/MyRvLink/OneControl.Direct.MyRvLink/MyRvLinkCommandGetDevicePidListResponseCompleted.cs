using System;
using System.Collections.Generic;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetDevicePidListResponseCompleted : MyRvLinkCommandResponseSuccess
{
	private const string LogTag = "MyRvLinkCommandGetDevicePidListResponseCompleted";

	protected const int PidCountIndex = 0;

	protected override int MinExtendedDataLength => 1;

	public byte PidCount => base.ExtendedData[0];

	public MyRvLinkCommandGetDevicePidListResponseCompleted(ushort clientCommandId, byte pidCount)
		: base(clientCommandId, commandCompleted: true, EncodeExtendedData(pidCount))
	{
	}

	public MyRvLinkCommandGetDevicePidListResponseCompleted(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandGetDevicePidListResponseCompleted(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(byte pidCount)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		int num = 1;
		byte[] array = new byte[num];
		array[0] = pidCount;
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(array, 0, num);
	}

	public override string ToString()
	{
		return $"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandGetDevicePidListResponseCompleted"} PID Count: {PidCount}";
	}
}
