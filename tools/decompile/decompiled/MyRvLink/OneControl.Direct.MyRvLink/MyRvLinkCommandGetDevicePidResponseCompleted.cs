using System;
using System.Collections;
using System.Collections.Generic;
using IDS.Core.Types;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetDevicePidResponseCompleted : MyRvLinkCommandResponseSuccess
{
	public const string LogTag = "MyRvLinkCommandGetDevicePidResponseCompleted";

	private const int MaxPidValueSize = 6;

	private const int MinPidValueSize = 0;

	protected override int MinExtendedDataLength => 0;

	public UInt48 PidValue => DecodePidValue();

	public MyRvLinkCommandGetDevicePidResponseCompleted(ushort clientCommandId, UInt48 pidValue)
		: base(clientCommandId, commandCompleted: true, EncodeExtendedData(pidValue))
	{
	}//IL_0003: Unknown result type (might be due to invalid IL or missing references)


	public MyRvLinkCommandGetDevicePidResponseCompleted(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandGetDevicePidResponseCompleted(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	protected UInt48 DecodePidValue()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count == 0)
		{
			return UInt48.op_Implicit((byte)0);
		}
		UInt48 val = UInt48.op_Implicit((byte)0);
		global::System.Collections.Generic.IEnumerator<byte> enumerator = ((global::System.Collections.Generic.IEnumerable<byte>)base.ExtendedData).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				byte current = enumerator.Current;
				val <<= 8;
				val |= UInt48.op_Implicit(current);
			}
			return val;
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(UInt48 pidValue)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		byte[] array = new byte[6];
		int num = ArrayExtension.SetValueBigEndianRemoveLeadingZeros(array, UInt48.op_Implicit(pidValue), 0, 6);
		if (num == 0)
		{
			return global::System.Array.Empty<byte>();
		}
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(array, 0, num);
	}

	public override string ToString()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		return $"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandGetDevicePidResponseCompleted"} PID Value: 0x{PidValue:X}";
	}
}
