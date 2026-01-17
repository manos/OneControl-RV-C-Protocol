using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkRealTimeClock : MyRvLinkEvent<MyRvLinkRealTimeClock>
{
	[Flags]
	private enum Flags
	{
		None = 0,
		HasRtcHardware = 1,
		UsesRtcHardware = 2,
		Reserved = 0xFC
	}

	public enum TimeSinceStartUnits
	{
		Seconds = 0,
		Minutes = 1,
		Hours = 16,
		Days = 17
	}

	public readonly struct TimeSinceStart
	{
		internal const ushort UnitsShift = 14;

		internal const ushort UnitsMask = 49152;

		public const ushort MaxValue = 16383;

		[field: CompilerGenerated]
		public ushort Value
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public TimeSinceStartUnits Units
		{
			[CompilerGenerated]
			get;
		}

		private bool IsClockSet
		{
			get
			{
				if (Value != 16383)
				{
					return Units != TimeSinceStartUnits.Days;
				}
				return false;
			}
		}

		public TimeSpan TimeSpan => (TimeSpan)(Units switch
		{
			TimeSinceStartUnits.Seconds => TimeSpan.FromSeconds((double)(int)Value), 
			TimeSinceStartUnits.Minutes => TimeSpan.FromMinutes((double)(int)Value), 
			TimeSinceStartUnits.Hours => TimeSpan.FromHours((double)(int)Value), 
			TimeSinceStartUnits.Days => TimeSpan.FromDays((double)(int)Value), 
			_ => TimeSpan.FromSeconds((double)(int)Value), 
		});

		internal ushort ValueRaw => (ushort)(((ushort)Units << 14) | Value);

		public TimeSinceStart(TimeSpan timespan)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			if (timespan == TimeSpan.MaxValue)
			{
				Value = 16383;
				Units = TimeSinceStartUnits.Days;
				return;
			}
			TimeSpan val = timespan;
			if (((TimeSpan)(ref val)).TotalSeconds < 16383.0)
			{
				Value = (ushort)((TimeSpan)(ref val)).TotalSeconds;
				Units = TimeSinceStartUnits.Seconds;
			}
			else
			{
				TimeSpan val2 = val;
				if (((TimeSpan)(ref val2)).TotalMinutes < 16383.0)
				{
					Value = (ushort)((TimeSpan)(ref val2)).TotalMinutes;
					Units = TimeSinceStartUnits.Minutes;
				}
				else
				{
					TimeSpan val3 = val;
					if (((TimeSpan)(ref val3)).TotalHours < 16383.0)
					{
						Value = (ushort)((TimeSpan)(ref val3)).TotalHours;
						Units = TimeSinceStartUnits.Hours;
					}
					else
					{
						TimeSpan val4 = val;
						if (!(((TimeSpan)(ref val4)).TotalDays < 16383.0))
						{
							throw new ArgumentOutOfRangeException("timespan", "timespan to big to fit in TimeSinceStart");
						}
						Value = (ushort)((TimeSpan)(ref val4)).TotalDays;
						Units = TimeSinceStartUnits.Days;
					}
				}
			}
			if (((TimeSpan)(ref timespan)).TotalSeconds < 16383.0)
			{
				Value = (ushort)((TimeSpan)(ref timespan)).TotalSeconds;
				Units = TimeSinceStartUnits.Seconds;
			}
		}

		internal TimeSinceStart(ushort rawTime)
		{
			Units = (TimeSinceStartUnits)((rawTime & 0xC000) >> 14);
			Value = (ushort)(rawTime & 0x3FFF);
		}

		public string ToString()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return (IsClockSet ? ((object)TimeSpan/*cast due to .constrained prefix*/).ToString() : "Not Set") ?? "";
		}
	}

	public static readonly global::System.DateTime EpochStart = new global::System.DateTime(2000, 1, 1, 0, 0, 0);

	private const int MaxPayloadLength = 9;

	private const int SecondsFromEpicStartIndex = 1;

	private const int ClockSetStartIndex = 5;

	private const int FlagsIndex = 8;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.RealTimeClock;

	protected override int MinPayloadLength => 9;

	[field: CompilerGenerated]
	protected override byte[] _rawData
	{
		[CompilerGenerated]
		get;
	}

	public uint SecondsFromEpoch => ArrayExtension.GetValueUInt32(_rawData, 1, (Endian)0);

	public TimeSinceStart TimeSinceClockSet => new TimeSinceStart(ArrayExtension.GetValueUInt16(_rawData, 5, (Endian)0));

	private Flags FlagsRaw => (Flags)_rawData[8];

	public global::System.DateTime DateTime => EpochStart + TimeSpan.FromSeconds((double)SecondsFromEpoch);

	public MyRvLinkRealTimeClock(uint secondsFromEpoch, TimeSinceStart timeSinceClockSet, bool hasRtcHardware, bool usesRtcHardware)
	{
		_rawData = new byte[9];
		_rawData[0] = (byte)EventType;
		ArrayExtension.SetValueUInt32(_rawData, secondsFromEpoch, 1, (Endian)0);
		ArrayExtension.SetValueUInt16(_rawData, timeSinceClockSet.ValueRaw, 5, (Endian)0);
		Flags flags = Flags.None;
		if (hasRtcHardware)
		{
			EnumExtensions.SetFlag<Flags>(flags, Flags.HasRtcHardware);
		}
		if (usesRtcHardware)
		{
			EnumExtensions.SetFlag<Flags>(flags, Flags.UsesRtcHardware);
		}
		_rawData[8] = (byte)flags;
	}

	protected MyRvLinkRealTimeClock(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateEventRawDataBasic(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 9)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {EventType} received more then {9} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkRealTimeClock Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkRealTimeClock(rawData);
	}

	public override string ToString()
	{
		return $"{EventType} Datetime = {DateTime} Time Since Clock Set = {TimeSinceClockSet} {EnumExtensions.DebugDumpAsFlags<Flags>(FlagsRaw)} Raw Data: {ArrayExtension.DebugDump(_rawData, " ", false)}";
	}
}
