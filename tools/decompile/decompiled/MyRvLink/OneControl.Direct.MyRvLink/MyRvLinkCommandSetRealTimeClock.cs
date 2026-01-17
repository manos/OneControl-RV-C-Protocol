using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandSetRealTimeClock : MyRvLinkCommand
{
	private const int PayloadLength = 10;

	private const int MonthIndex = 3;

	private const int DayIndex = 4;

	private const int YearIndex = 5;

	private const int HourIndex = 7;

	private const int MinutesIndex = 8;

	private const int SecondsIndex = 9;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandSetRealTimeClock";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.SetRealTimeClock;

	protected override int MinPayloadLength => 10;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte Month => _rawData[3];

	public byte Day => _rawData[4];

	public ushort Year => ArrayExtension.GetValueUInt16(_rawData, 5, (Endian)0);

	public byte Hour => _rawData[7];

	public byte Minutes => _rawData[8];

	public byte Seconds => _rawData[9];

	public global::System.DateTime DateTime => new global::System.DateTime((int)Year, (int)Month, (int)Day, (int)Hour, (int)Minutes, (int)Seconds);

	public MyRvLinkCommandSetRealTimeClock(ushort clientCommandId, global::System.DateTime dateTime)
	{
		_rawData = new byte[10];
		_rawData[2] = (byte)CommandType;
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[3] = (byte)dateTime.Month;
		_rawData[4] = (byte)dateTime.Day;
		ArrayExtension.SetValueUInt16(_rawData, (ushort)dateTime.Year, 5, (Endian)0);
		_rawData[7] = (byte)dateTime.Hour;
		_rawData[8] = (byte)dateTime.Minute;
		_rawData[9] = (byte)dateTime.Second;
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandSetRealTimeClock(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 10)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {10} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandSetRealTimeClock Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandSetRealTimeClock(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public virtual string ToString()
	{
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Datetime: {DateTime}]: {ArrayExtension.DebugDump(_rawData, " ", false)} ";
	}
}
