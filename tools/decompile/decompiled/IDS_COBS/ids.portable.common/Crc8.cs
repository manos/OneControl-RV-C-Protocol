using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

public struct Crc8
{
	private const byte ResetValue = 85;

	private static readonly byte[] Table;

	private byte? _value;

	public byte Value
	{
		get
		{
			return _value ?? 85;
		}
		private set
		{
			_value = value;
		}
	}

	[MethodImpl((MethodImplOptions)256)]
	public static implicit operator byte(Crc8 crc)
	{
		return crc.Value;
	}

	[MethodImpl((MethodImplOptions)256)]
	public void Reset()
	{
		Value = 85;
	}

	[MethodImpl((MethodImplOptions)256)]
	public void Update(byte b)
	{
		Value = Table[(Value ^ b) & 0xFF];
	}

	[MethodImpl((MethodImplOptions)256)]
	public void Update(global::System.Collections.Generic.IReadOnlyList<byte> buffer)
	{
		Update(buffer, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)buffer).Count, 0);
	}

	[MethodImpl((MethodImplOptions)256)]
	public void Update(global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count)
	{
		Update(buffer, count, 0);
	}

	[MethodImpl((MethodImplOptions)256)]
	public void Update(global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count, int offset)
	{
		Value = Calculate(Value, buffer, count, offset);
	}

	[MethodImpl((MethodImplOptions)256)]
	public static byte Calculate(global::System.Collections.Generic.IReadOnlyList<byte> buffer)
	{
		return Calculate(85, buffer, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)buffer).Count, 0);
	}

	[MethodImpl((MethodImplOptions)256)]
	public static byte Calculate(global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count)
	{
		return Calculate(85, buffer, count, 0);
	}

	[MethodImpl((MethodImplOptions)256)]
	public static byte Calculate(global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count, int offset)
	{
		return Calculate(85, buffer, count, offset);
	}

	private static byte Calculate(byte crc, global::System.Collections.Generic.IReadOnlyList<byte> buffer, int count, int offset)
	{
		if ((count & 1) != 0)
		{
			crc = Table[(crc ^ buffer[offset++]) & 0xFF];
		}
		count >>= 1;
		if ((count & 1) != 0)
		{
			crc = Table[(crc ^ buffer[offset++]) & 0xFF];
			crc = Table[(crc ^ buffer[offset++]) & 0xFF];
		}
		count >>= 1;
		while (count-- > 0)
		{
			crc = Table[(crc ^ buffer[offset++]) & 0xFF];
			crc = Table[(crc ^ buffer[offset++]) & 0xFF];
			crc = Table[(crc ^ buffer[offset++]) & 0xFF];
			crc = Table[(crc ^ buffer[offset++]) & 0xFF];
		}
		return crc;
	}

	public static byte Calculate(global::System.Collections.Generic.IReadOnlyCollection<byte> bytes)
	{
		byte b = 85;
		if (bytes != null)
		{
			global::System.Collections.Generic.IEnumerator<byte> enumerator = ((global::System.Collections.Generic.IEnumerable<byte>)bytes).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					byte current = enumerator.Current;
					b = Table[(b ^ current) & 0xFF];
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
		}
		return b;
	}

	static Crc8()
	{
		byte[] array = new byte[256];
		RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		Table = array;
	}
}
