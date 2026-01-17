using System;
using System.Collections;
using System.Collections.Generic;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetDevicePidWithAddressResponseCompleted : MyRvLinkCommandResponseSuccess
{
	public const string LogTag = "MyRvLinkCommandGetDevicePidWithAddressResponseCompleted";

	private const int RequiredAddressBytes = 2;

	private const int MaxValueBytes = 4;

	private const int MinExtendedDataSize = 2;

	private const int MaxExtendedDataSize = 6;

	private const int ExtendedDataAddressStartIndex = 0;

	private const int ExtendedDataValueStartIndex = 2;

	protected override int MinExtendedDataLength => 2;

	public uint PidValue => DecodePidValue();

	public uint PidAddress => DecodePidAddress();

	public MyRvLinkCommandGetDevicePidWithAddressResponseCompleted(ushort clientCommandId, ushort pidAddress, uint pidValue)
		: base(clientCommandId, commandCompleted: true, EncodeExtendedData(pidAddress, pidValue))
	{
	}

	public MyRvLinkCommandGetDevicePidWithAddressResponseCompleted(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandGetDevicePidWithAddressResponseCompleted(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	protected uint DecodePidValue()
	{
		int num = MathCommon.Clamp(((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count - 2, 0, 4);
		if (num == 0)
		{
			return 0u;
		}
		uint num2 = 0u;
		global::System.Collections.Generic.IEnumerator<byte> enumerator = ((global::System.Collections.Generic.IEnumerable<byte>)GetExtendedData(2, num)).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				byte current = enumerator.Current;
				num2 <<= 8;
				num2 |= current;
			}
			return num2;
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	protected uint DecodePidAddress()
	{
		global::System.Collections.Generic.IReadOnlyList<byte> extendedData = base.ExtendedData;
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)extendedData).Count < 2)
		{
			return 0u;
		}
		return ArrayExtension.GetValueUInt16(extendedData, 0, (Endian)0);
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(ushort address, uint pidValue)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		byte[] array = new byte[6];
		ArrayExtension.SetValueUInt16(array, address, 0, (Endian)0);
		int num = ArrayExtension.SetValueBigEndianRemoveLeadingZeros(array, (ulong)pidValue, 2, 4);
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(array, 0, 2 + num);
	}

	public override string ToString()
	{
		return $"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandGetDevicePidWithAddressResponseCompleted"} PID Value: 0x{PidValue:X}";
	}
}
