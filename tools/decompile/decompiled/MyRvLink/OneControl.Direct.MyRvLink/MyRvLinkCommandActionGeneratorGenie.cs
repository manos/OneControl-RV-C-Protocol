using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandActionGeneratorGenie : MyRvLinkCommand
{
	private enum RvLinkGeneratorCommand
	{
		Off = 0,
		On = 1,
		Prime = 3
	}

	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int DeviceCommandIndex = 5;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandActionGeneratorGenie";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.ActionGeneratorGenie;

	protected override int MinPayloadLength => 6;

	private int MaxPayloadLength => MinPayloadLength;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public GeneratorGenieCommand Command => (GeneratorGenieCommand)_rawData[5];

	public MyRvLinkCommandActionGeneratorGenie(ushort clientCommandId, byte deviceTableId, byte deviceId, GeneratorGenieCommand command)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		_rawData = new byte[MaxPayloadLength];
		RvLinkGeneratorCommand rvLinkGeneratorCommand;
		if ((int)command != 1)
		{
			if ((int)command == 2)
			{
				rvLinkGeneratorCommand = RvLinkGeneratorCommand.On;
			}
			else
			{
				TaggedLog.Warning(LogTag, $"Unknown Generator Genie Command {command} turning generator OFF", global::System.Array.Empty<object>());
				rvLinkGeneratorCommand = RvLinkGeneratorCommand.Off;
			}
		}
		else
		{
			rvLinkGeneratorCommand = RvLinkGeneratorCommand.Off;
		}
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		_rawData[5] = (byte)rvLinkGeneratorCommand;
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandActionGeneratorGenie(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > MaxPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {MaxPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandActionGeneratorGenie Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandActionGeneratorGenie(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public virtual string ToString()
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, Device Id: 0x{DeviceId:X2}, Command: {Command}]: {ArrayExtension.DebugDump(_rawData, " ", false)} ";
	}
}
