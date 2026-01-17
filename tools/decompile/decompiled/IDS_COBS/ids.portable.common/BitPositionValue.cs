using System;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

public struct BitPositionValue
{
	public readonly int NumBits;

	public readonly int StartBitIndex;

	public readonly int StartIndex;

	public readonly int NumBytes;

	public uint BitMask => MakeBitMask(NumBits, StartBitIndex);

	[MethodImpl((MethodImplOptions)256)]
	public static uint MakeBitMask(int numBits, int startBitIndex)
	{
		return (uint)(~(-1 >>> numBits << numBits) << startBitIndex);
	}

	public BitPositionValue(int numBits, int startBitIndex, int startIndex = 0, int numBytes = 1)
		: this(MakeBitMask(numBits, startBitIndex), startIndex, numBytes)
	{
	}

	public BitPositionValue(uint bitMask, int startIndex = 0, int numBytes = 1)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 0;
		if (numBytes <= 0)
		{
			throw new ArgumentException("BitPositionValue must have at least 1 byte of data");
		}
		if (numBytes > 4)
		{
			throw new ArgumentException($"BitPositionValue numBytes must be less then {4}");
		}
		bool flag = false;
		for (int i = 0; i < 32; i++)
		{
			if (((bitMask >> i) & 1) == 0)
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

	public uint DecodeValue(uint encodedValue)
	{
		uint num = (uint)(~(-1 >>> NumBits << NumBits) << StartBitIndex);
		return (encodedValue & num) >> StartBitIndex;
	}

	public uint EncodeValue(uint value, uint mergeValue = 0u)
	{
		uint num = value << StartBitIndex;
		uint num2 = (uint)(~(-1 >>> NumBits << NumBits) << StartBitIndex);
		return (num2 & num) | (mergeValue & ~num2);
	}

	public uint DecodeValueFromBuffer(byte[] dataBuffer)
	{
		return DecodeValueFromBuffer(dataBuffer, StartIndex, NumBytes);
	}

	public uint DecodeValueFromBuffer(byte[] dataBuffer, int startIndex, int numBytes)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (dataBuffer == null || NumBytes <= 0)
		{
			return 0u;
		}
		if (startIndex < 0 || startIndex + numBytes > dataBuffer.Length)
		{
			throw new ArgumentException("DecodeValueFromBuffer can't extend past end of buffer");
		}
		uint num = 0u;
		for (int i = startIndex; i < startIndex + numBytes; i++)
		{
			num <<= 8;
			num |= dataBuffer[i];
		}
		return DecodeValue(num);
	}

	public void EncodeValueToBuffer(uint value, byte[] dataBuffer)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		if (dataBuffer != null && NumBytes > 0)
		{
			if (StartIndex < 0 || StartIndex + NumBytes > dataBuffer.Length)
			{
				throw new ArgumentException("DecodeValueToBuffer can't extend past bounds of buffer");
			}
			uint num = 0u;
			for (int i = StartIndex; i < StartIndex + NumBytes; i++)
			{
				num <<= 8;
				num |= dataBuffer[i];
			}
			uint num2 = EncodeValue(value, num);
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
