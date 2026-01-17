using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace IDS.Portable.Common;

public sealed class HexBinaryArray
{
	[field: CompilerGenerated]
	public byte[] HexBytes
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public string HexString
	{
		[CompilerGenerated]
		get;
	}

	public HexBinaryArray(byte[] value)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		HexBytes = value;
		StringBuilder val = new StringBuilder
		{
			Length = 0
		};
		foreach (byte b in value)
		{
			val.Append(b.ToString("X2"));
		}
		HexString = ((object)val).ToString();
	}

	public HexBinaryArray(string value)
	{
		HexBytes = FromBinHexString(value);
		HexString = value;
	}

	public static byte[] FromBinHexString(string value)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrWhiteSpace(value))
		{
			return new byte[0];
		}
		char[] array = value.ToCharArray();
		byte[] array2 = new byte[array.Length / 2 + array.Length % 2];
		int num = array.Length;
		if (num % 2 != 0)
		{
			throw new ArgumentException("FromBinHexString invalid value " + value);
		}
		int num2 = 0;
		for (int i = 0; i < num - 1; i += 2)
		{
			array2[num2] = FromHex(array[i], value);
			array2[num2] <<= 4;
			array2[num2] += FromHex(array[i + 1], value);
			num2++;
		}
		return array2;
	}

	private static byte FromHex(char hexDigit, string value)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return byte.Parse(hexDigit.ToString(), (NumberStyles)515, (IFormatProvider)(object)CultureInfo.InvariantCulture);
		}
		catch (FormatException)
		{
			throw new ArgumentException($"FromHex invalid {hexDigit} for {value}");
		}
	}

	public string ToString()
	{
		return HexString;
	}

	public static implicit operator string(HexBinaryArray v)
	{
		return v.HexString;
	}

	public static implicit operator byte[](HexBinaryArray v)
	{
		return v.HexBytes;
	}
}
