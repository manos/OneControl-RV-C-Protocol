using System;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

public readonly struct BitPositionValue64
{
	public readonly int NumBits;

	public readonly int StartBitIndex;

	public readonly int StartIndex;

	public readonly int NumBytes;

	public ulong BitMask => MakeBitMask(NumBits, StartBitIndex);

	[MethodImpl((MethodImplOptions)256)]
	public static ulong MakeBitMask(int numBits, int startBitIndex)
	{
		return ~(18446744073709551615uL >> numBits << numBits) << startBitIndex;
	}

	public BitPositionValue64(int numBits, int startBitIndex, int startIndex = 0, int numBytes = 1)
		: this(MakeBitMask(numBits, startBitIndex), startIndex, numBytes)
	{
	}

	public BitPositionValue64(ulong bitMask, int startIndex = 0, int numBytes = 1)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 0;
		if (numBytes <= 0)
		{
			throw new ArgumentException("BitPositionValue must have at least 1 byte of data");
		}
		if (numBytes > 8)
		{
			throw new ArgumentException($"BitPositionValue numBytes must be less then {8}");
		}
		bool flag = false;
		for (int i = 0; i < 64; i++)
		{
			if (((bitMask >> i) & 1) == 0L)
			{
				if (flag)
				{
					break;
				}
				num2++;
				continue;
			}
			if (!flag)
			{
				flag = true;
				num2 = i;
			}
			num++;
		}
		NumBits = num;
		StartBitIndex = num2;
		StartIndex = startIndex;
		NumBytes = numBytes;
	}

	public ulong DecodeValue(ulong encodedValue)
	{
		ulong num = ~(18446744073709551615uL >> NumBits << NumBits) << StartBitIndex;
		return (encodedValue & num) >> StartBitIndex;
	}

	public ulong EncodeValue(ulong value, ulong mergeValue = 0uL)
	{
		ulong num = value << StartBitIndex;
		ulong num2 = ~(18446744073709551615uL >> NumBits << NumBits) << StartBitIndex;
		return (num2 & num) | (mergeValue & ~num2);
	}

	public ulong DecodeValueFromBuffer(byte[] dataBuffer)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if (dataBuffer == null || NumBytes <= 0)
		{
			return 0uL;
		}
		if (StartIndex < 0 || StartIndex + NumBytes > dataBuffer.Length)
		{
			throw new ArgumentException("DecodeValueFromBuffer can't extend past end of buffer");
		}
		ulong num = 0uL;
		for (int i = StartIndex; i < StartIndex + NumBytes; i++)
		{
			num <<= 8;
			num |= dataBuffer[i];
		}
		return DecodeValue(num);
	}

	public void EncodeValueToBuffer(ulong value, byte[] dataBuffer)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		if (dataBuffer != null && NumBytes > 0)
		{
			if (StartIndex < 0 || StartIndex + NumBytes > dataBuffer.Length)
			{
				throw new ArgumentException("DecodeValueToBuffer can't extend past bounds of buffer");
			}
			ulong num = 0uL;
			for (int i = StartIndex; i < StartIndex + NumBytes; i++)
			{
				num <<= 8;
				num |= dataBuffer[i];
			}
			ulong num2 = EncodeValue(value, num);
			for (int num3 = StartIndex + NumBytes - 1; num3 >= StartIndex; num3--)
			{
				dataBuffer[num3] = (byte)(num2 & 0xFF);
				num2 >>= 8;
			}
		}
	}

	public string ToString()
	{
		return $"0b{Convert.ToString((long)BitMask, 2)}(0x{BitMask:X})/{StartIndex}/{NumBytes}";
	}
}
