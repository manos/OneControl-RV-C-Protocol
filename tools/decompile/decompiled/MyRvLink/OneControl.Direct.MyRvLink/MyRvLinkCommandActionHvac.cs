using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandActionHvac : MyRvLinkCommand
{
	private const int ClimateZoneCommandLength = 3;

	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int DeviceCommandStartIndex = 5;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandActionHvac";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.ActionHvac;

	protected override int MinPayloadLength => 8;

	private int MaxPayloadLength => MinPayloadLength;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public LogicalDeviceClimateZoneCommand Command => new LogicalDeviceClimateZoneCommand((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 5, 3));

	public MyRvLinkCommandActionHvac(ushort clientCommandId, byte deviceTableId, byte deviceId, LogicalDeviceClimateZoneCommand command)
	{
		global::System.Collections.Generic.IReadOnlyList<byte> dataMinimum = command.DataMinimum;
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)dataMinimum).Count != 3)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {LogTag} because size is {((global::System.Collections.Generic.IReadOnlyCollection<byte>)dataMinimum).Count} when expecting {3} bytes: {ArrayExtension.DebugDump(dataMinimum, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)dataMinimum).Count, " ", false)}");
		}
		_rawData = new byte[MaxPayloadLength];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		int num = 5;
		global::System.Collections.Generic.IEnumerator<byte> enumerator = ((global::System.Collections.Generic.IEnumerable<byte>)dataMinimum).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				byte current = enumerator.Current;
				_rawData[num++] = current;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandActionHvac(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > MaxPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {MaxPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandActionHvac Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandActionHvac(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public virtual string ToString()
	{
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, Device Id: 0x{DeviceId:X2}, Command: {Command}]: {ArrayExtension.DebugDump(_rawData, " ", false)} ";
	}
}
