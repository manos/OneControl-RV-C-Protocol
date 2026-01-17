using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ids.portable.common;

public struct Crc32Le
{
	private static readonly uint[] Crc32LeTable;

	public const uint ResetValue = 4294967295u;

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

	public static implicit operator uint(Crc32Le crc)
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
		Update(buffer, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)buffer).Count, 0u);
	}

	public void Update(global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count)
	{
		Update(buffer, count, 0u);
	}

	public void Update(global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count, uint offset)
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
		return Calculate(4294967295u, buffer, count, 0u);
	}

	public static uint Calculate(global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count, uint offset)
	{
		return Calculate(4294967295u, buffer, count, offset);
	}

	public static uint Calculate(uint crc, global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count, uint offset)
	{
		crc = Crc32_le(crc, (byte[])buffer, (uint)count, offset);
		return crc;
	}

	private static uint Calculate(uint crc, byte[] buf, int count)
	{
		crc = Crc32_le(crc, buf, (uint)count, 0u);
		return crc;
	}

	public static uint Update(uint crc, byte data)
	{
		return Crc32_le(crc, new byte[1] { data }, 1u, 0u);
	}

	public static uint Crc32_le(uint crc, byte[] buf, uint len, uint offset)
	{
		crc = ~crc;
		if (offset + len <= buf.Length)
		{
			for (uint num = offset; num < offset + len; num++)
			{
				crc = Crc32LeTable[(crc ^ buf[num]) & 0xFF] ^ (crc >> 8);
			}
		}
		return ~crc;
	}

	static Crc32Le()
	{
		uint[] array = new uint[256];
		RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		Crc32LeTable = array;
	}
}
