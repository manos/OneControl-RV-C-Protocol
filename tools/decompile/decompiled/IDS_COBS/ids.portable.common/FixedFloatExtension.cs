using System;

namespace IDS.Portable.Common;

public static class FixedFloatExtension
{
	public static void SetFixedPointFloat(this FixedPointType fixedPointType, byte[] buffer, uint startOffset, float realNumber)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		switch (fixedPointType)
		{
		case FixedPointType.UnsignedBigEndian8x8:
			FixedPointUnsignedBigEndian8X8.FromFloat(buffer, startOffset, realNumber);
			return;
		case FixedPointType.UnsignedBigEndian16x16:
			FixedPointUnsignedBigEndian16X16.FromFloat(buffer, startOffset, realNumber);
			return;
		case FixedPointType.SignedBigEndian8x8:
			FixedPointSignedBigEndian8X8.FromFloat(buffer, startOffset, realNumber);
			return;
		case FixedPointType.SignedBigEndian16x16:
			FixedPointSignedBigEndian16X16.FromFloat(buffer, startOffset, realNumber);
			return;
		}
		throw new NotImplementedException($"Unknown {"FixedPointType"} of {fixedPointType}");
	}

	public static float GetFixedPointFloat(this FixedPointType fixedPointType, byte[] buffer, uint startOffset)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		return fixedPointType switch
		{
			FixedPointType.UnsignedBigEndian8x8 => FixedPointUnsignedBigEndian8X8.ToFloat(buffer, startOffset), 
			FixedPointType.UnsignedBigEndian16x16 => FixedPointUnsignedBigEndian16X16.ToFloat(buffer, startOffset), 
			FixedPointType.SignedBigEndian8x8 => FixedPointSignedBigEndian8X8.ToFloat(buffer, startOffset), 
			FixedPointType.SignedBigEndian16x16 => FixedPointSignedBigEndian16X16.ToFloat(buffer, startOffset), 
			_ => throw new NotImplementedException($"Unknown {"fixedPointType"} of {fixedPointType}"), 
		};
	}

	public static float FixedPointToFloat(this FixedPointType fixedPointType, ulong fixedPointNumber)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		return fixedPointType switch
		{
			FixedPointType.UnsignedBigEndian8x8 => FixedPointUnsignedBigEndian8X8.ToFloat((ushort)(fixedPointNumber & 0xFFFF)), 
			FixedPointType.UnsignedBigEndian16x16 => FixedPointUnsignedBigEndian16X16.ToFloat((uint)(fixedPointNumber & 0xFFFFFFFFu)), 
			FixedPointType.SignedBigEndian8x8 => FixedPointSignedBigEndian8X8.ToFloat((short)(fixedPointNumber & 0xFFFF)), 
			FixedPointType.SignedBigEndian16x16 => FixedPointSignedBigEndian16X16.ToFloat((int)(fixedPointNumber & 0xFFFFFFFFu)), 
			_ => throw new NotImplementedException($"Unknown {"fixedPointType"} of {fixedPointType}"), 
		};
	}

	public static ulong ToFixedPointAsULong(this FixedPointType fixedPointType, float realNumber)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		return fixedPointType switch
		{
			FixedPointType.UnsignedBigEndian8x8 => FixedPointUnsignedBigEndian8X8.ToFixedPoint(realNumber), 
			FixedPointType.UnsignedBigEndian16x16 => FixedPointUnsignedBigEndian16X16.ToFixedPoint(realNumber), 
			FixedPointType.SignedBigEndian8x8 => (ulong)FixedPointSignedBigEndian8X8.ToFixedPoint(realNumber), 
			FixedPointType.SignedBigEndian16x16 => (ulong)FixedPointSignedBigEndian16X16.ToFixedPoint(realNumber), 
			_ => throw new NotImplementedException($"Unknown {"fixedPointType"} of {fixedPointType}"), 
		};
	}
}
