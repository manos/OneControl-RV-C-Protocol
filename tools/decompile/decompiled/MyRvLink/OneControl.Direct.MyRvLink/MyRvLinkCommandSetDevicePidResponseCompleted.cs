using System;
using System.Collections;
using System.Collections.Generic;
using IDS.Core.Types;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandSetDevicePidResponseCompleted : MyRvLinkCommandResponseSuccess
{
	public const string LogTag = "MyRvLinkCommandSetDevicePidResponseCompleted";

	private const int RequiredAddressBytes = 2;

	private const int MaxValueBytes = 6;

	private const int MinExtendedDataSize = 2;

	private const int MaxExtendedDataSize = 8;

	private const int ExtendedDataValueStartIndex = 0;

	protected override int MinExtendedDataLength => 2;

	public UInt48 PidValue => DecodePidValue();

	public MyRvLinkCommandSetDevicePidResponseCompleted(ushort clientCommandId, ushort pidAddress, uint pidValue)
		: base(clientCommandId, commandCompleted: true, EncodeExtendedData(pidAddress, pidValue))
	{
	}

	public MyRvLinkCommandSetDevicePidResponseCompleted(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandSetDevicePidResponseCompleted(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	protected UInt48 DecodePidValue()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		int num = MathCommon.Clamp(((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count, 0, 6);
		if (num == 0)
		{
			return UInt48.op_Implicit((byte)0);
		}
		uint num2 = 0u;
		global::System.Collections.Generic.IEnumerator<byte> enumerator = ((global::System.Collections.Generic.IEnumerable<byte>)GetExtendedData(0, num)).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				byte current = enumerator.Current;
				num2 <<= 8;
				num2 |= current;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		return UInt48.op_Implicit(num2);
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(ushort address, uint pidValue)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		byte[] array = new byte[8];
		int num = ArrayExtension.SetValueBigEndianRemoveLeadingZeros(array, (ulong)pidValue, 0, 6);
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(array, 0, 2 + num);
	}

	public override string ToString()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		return $"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandSetDevicePidResponseCompleted"} PID Value: 0x{PidValue:X}";
	}
}
