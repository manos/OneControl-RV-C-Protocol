using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandActionDimmable : MyRvLinkCommand
{
	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int DeviceCommandIndex = 5;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandActionDimmable";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.ActionDimmable;

	protected override int MinPayloadLength => 6;

	private int MaxPayloadLength => 12;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public LogicalDeviceLightDimmableCommand Command => new LogicalDeviceLightDimmableCommand((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 5, _rawData.Length - 5));

	public MyRvLinkCommandActionDimmable(ushort clientCommandId, byte deviceTableId, byte deviceId, LogicalDeviceLightDimmableCommand command)
	{
		global::System.Collections.Generic.IReadOnlyList<byte> dataMinimum = command.DataMinimum;
		int num = MinPayloadLength + ((global::System.Collections.Generic.IReadOnlyCollection<byte>)dataMinimum).Count - 1;
		_rawData = new byte[num];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		int num2 = 5;
		global::System.Collections.Generic.IEnumerator<byte> enumerator = ((global::System.Collections.Generic.IEnumerable<byte>)dataMinimum).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				byte current = enumerator.Current;
				_rawData[num2++] = current;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandActionDimmable(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > MaxPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {MaxPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandActionDimmable Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandActionDimmable(rawData);
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
