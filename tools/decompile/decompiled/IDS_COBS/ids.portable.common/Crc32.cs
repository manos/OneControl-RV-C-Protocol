using System;
using System.Collections;
using System.Collections.Generic;

namespace ids.portable.common;

public struct Crc32
{
	private const uint ResetValue = 4294967295u;

	private const uint CrcPolynomial = 1947962583u;

	private uint? _value;

	public uint Value
	{
		get
		{
			return (uint)(((int?)_value) ?? (-1));
		}
		private set
		{
			_value = value;
		}
	}

	public static implicit operator uint(Crc32 crc)
	{
		return crc.Value;
	}

	public void Reset()
	{
		Value = 4294967295u;
	}

	public void Update(byte b)
	{
		Value = Update(Value, b);
	}

	public void Update(global::System.Collections.Generic.IReadOnlyList<byte> buffer)
	{
		Update(buffer, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)buffer).Count, 0);
	}

	public void Update(global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count)
	{
		Update(buffer, count, 0);
	}

	public void Update(global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count, int offset)
	{
		Value = Calculate(Value, buffer, count, offset);
	}

	public static uint Calculate(global::System.Collections.Generic.IReadOnlyCollection<byte> bytes)
	{
		uint num = 4294967295u;
		global::System.Collections.Generic.IEnumerator<byte> enumerator = ((global::System.Collections.Generic.IEnumerable<byte>)bytes).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				byte current = enumerator.Current;
				num = Update(num, current);
			}
			return num;
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	public static uint Calculate(global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count)
	{
		return Calculate(4294967295u, buffer, count, 0);
	}

	public static uint Calculate(global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count, int offset)
	{
		return Calculate(4294967295u, buffer, count, offset);
	}

	private static uint Calculate(uint crc, global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count, int offset)
	{
		while (count-- > 0)
		{
			crc = Update(crc, buffer[offset++]);
		}
		return crc;
	}

	private static uint Update(uint crc, byte data)
	{
		crc ^= (uint)(data << 24);
		for (int i = 0; i < 8; i++)
		{
			crc = (((crc & 0x80000000u) != 2147483648u) ? (crc << 1) : ((crc << 1) ^ 0x741B8CD7));
		}
		return crc;
	}
}
