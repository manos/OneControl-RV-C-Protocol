using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Core.IDS_CAN;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandRenameDevice : MyRvLinkCommand
{
	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int ToFunctionNameIndex = 5;

	private const int ToFunctionNameSessionIndex = 7;

	private const int ToFunctionInstanceIndex = 9;

	private const int ToFunctionInstanceSessionIndex = 10;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandRenameDevice";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.RenameDevice;

	protected override int MinPayloadLength => MaxPayloadLength;

	private int MaxPayloadLength => 12;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public FUNCTION_NAME FunctionName => FUNCTION_NAME.op_Implicit(ArrayExtension.GetValueUInt16(_rawData, 5, (Endian)0));

	public SESSION_ID FunctionSession => SESSION_ID.op_Implicit(ArrayExtension.GetValueUInt16(_rawData, 7, (Endian)0));

	public int FunctionInstance => _rawData[9] & 0xF;

	public FUNCTION_NAME FunctionInstanceSession => FUNCTION_NAME.op_Implicit(ArrayExtension.GetValueUInt16(_rawData, 10, (Endian)0));

	public MyRvLinkCommandRenameDevice(ushort clientCommandId, byte deviceTableId, byte deviceId, FUNCTION_NAME functionName, int functionInstance, SESSION_ID sessionId)
	{
		_rawData = new byte[MaxPayloadLength];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		ArrayExtension.SetValueUInt16(_rawData, FUNCTION_NAME.op_Implicit(functionName), 5, (Endian)0);
		ArrayExtension.SetValueUInt16(_rawData, SESSION_ID.op_Implicit(sessionId), 7, (Endian)0);
		_rawData[9] = (byte)(functionInstance & 0xF);
		ArrayExtension.SetValueUInt16(_rawData, SESSION_ID.op_Implicit(sessionId), 10, (Endian)0);
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandRenameDevice(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > MaxPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {MaxPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandRenameDevice Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandRenameDevice(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public virtual string ToString()
	{
		return $"{LogTag}[Client Command {CommandType} Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, Device Id: 0x{DeviceId:X2}, Command: {FunctionName}/{FunctionInstance} {FunctionSession}/{FunctionInstanceSession}]: {ArrayExtension.DebugDump(_rawData, " ", false)} ";
	}
}
