using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkRvStatus : MyRvLinkEvent<MyRvLinkRvStatus>
{
	[Flags]
	private enum MyRvLinkRvStatusFeature
	{
		None = 0,
		VoltageAvailable = 1,
		ExternalTemperatureAvailable = 2
	}

	private const ushort InvalidVoltageFixedPoint = 65535;

	private const ushort InvalidTemperatureFixedPoint = 32767;

	private const int MaxPayloadLength = 6;

	private const int AverageVoltageStartIndex = 1;

	private const int ExternalTemperatureCelsiusStartIndex = 3;

	private const int FeatureIndex = 5;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.RvStatus;

	protected override int MinPayloadLength => 6;

	[field: CompilerGenerated]
	protected override byte[] _rawData
	{
		[CompilerGenerated]
		get;
	}

	public float BatteryVoltage => ArrayExtension.GetFixedPointFloat(_rawData, 1u, (FixedPointType)0);

	public float ExternalTemperatureCelsius => ArrayExtension.GetFixedPointFloat(_rawData, 3u, (FixedPointType)2);

	public bool IsBatteryVoltageAvailable => (_rawData[5] & 1) != 0;

	public bool IsBatteryVoltageValid
	{
		get
		{
			if (IsBatteryVoltageAvailable)
			{
				return ArrayExtension.GetValueUInt16(_rawData, 1, (Endian)0) != 65535;
			}
			return false;
		}
	}

	public bool IsExternalTemperatureAvailable => (_rawData[5] & 2) != 0;

	public bool IsExternalTemperatureValid
	{
		get
		{
			if (IsExternalTemperatureAvailable)
			{
				return ArrayExtension.GetValueUInt16(_rawData, 3, (Endian)0) != 32767;
			}
			return false;
		}
	}

	public MyRvLinkRvStatus(float? batteryVoltage, float? externalTemperatureCelsius, bool cloudConnectedToLan, bool cloudConnectedToWan, bool cloudGatewayAvailable)
	{
		_rawData = new byte[6];
		_rawData[0] = (byte)EventType;
		if (!batteryVoltage.HasValue)
		{
			ArrayExtension.SetValueUInt16(_rawData, (ushort)65535, 1, (Endian)0);
		}
		else
		{
			ArrayExtension.SetFixedPointFloat(_rawData, batteryVoltage.Value, 1u, (FixedPointType)0);
		}
		if (!externalTemperatureCelsius.HasValue)
		{
			ArrayExtension.SetValueUInt16(_rawData, (ushort)32767, 3, (Endian)0);
		}
		else
		{
			ArrayExtension.SetFixedPointFloat(_rawData, externalTemperatureCelsius.Value, 3u, (FixedPointType)0);
		}
		MyRvLinkRvStatusFeature myRvLinkRvStatusFeature = MyRvLinkRvStatusFeature.None;
		if (batteryVoltage.HasValue)
		{
			myRvLinkRvStatusFeature = EnumExtensions.SetFlag<MyRvLinkRvStatusFeature>(myRvLinkRvStatusFeature, MyRvLinkRvStatusFeature.VoltageAvailable);
		}
		if (externalTemperatureCelsius.HasValue)
		{
			myRvLinkRvStatusFeature = EnumExtensions.SetFlag<MyRvLinkRvStatusFeature>(myRvLinkRvStatusFeature, MyRvLinkRvStatusFeature.ExternalTemperatureAvailable);
		}
		_rawData[5] = (byte)myRvLinkRvStatusFeature;
	}

	protected MyRvLinkRvStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (rawData == null)
		{
			throw new ArgumentNullException("rawData");
		}
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count < MinPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {EventType} received less then {MinPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 6)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {EventType} received more then {6} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		if (EventType != (MyRvLinkEventType)rawData[0])
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {EventType} event type doesn't match {EventType}: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkRvStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkRvStatus(rawData);
	}

	public float? GetVoltage()
	{
		if (IsBatteryVoltageAvailable)
		{
			return BatteryVoltage;
		}
		return null;
	}

	public float? GetTemperature()
	{
		if (IsExternalTemperatureValid)
		{
			return ExternalTemperatureCelsius;
		}
		return null;
	}

	public override string ToString()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		StringBuilder val = new StringBuilder($"{EventType} {ArrayExtension.DebugDump(_rawData, " ", false)}");
		val.Append(IsBatteryVoltageValid ? $"{Environment.NewLine}    Average Battery Voltage: {BatteryVoltage:F2} V" : (Environment.NewLine + "    Average Battery Voltage: --.-- V"));
		val.Append(IsExternalTemperatureValid ? $"{Environment.NewLine}    External Temperature: {ExternalTemperatureCelsius:F2} °C" : (Environment.NewLine + "    External Temperature: --.-- °C"));
		return ((object)val).ToString();
	}
}
