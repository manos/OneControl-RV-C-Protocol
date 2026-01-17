using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice.BlockTransfer;

namespace OneControl.Direct.MyRvLink;

internal class MyRvLinkCommandStopDeviceBlockTransfer : MyRvLinkCommand
{
	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int BlockIdStartIndex = 5;

	private const int OptionsIndex = 7;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandStopDeviceBlockTransfer";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.StopDeviceBlockTransfer;

	protected override int MinPayloadLength => 8;

	private int MaxPayloadLength => 8;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public BlockTransferBlockId BlockId => DecodeBlockId(_rawData);

	public BlockTransferStopOptions Options => (BlockTransferStopOptions)_rawData[7];

	public MyRvLinkCommandStopDeviceBlockTransfer(ushort clientCommandId, byte deviceTableId, byte deviceId, BlockTransferBlockId blockId, BlockTransferStopOptions options)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected I4, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected I4, but got Unknown
		_rawData = new byte[MaxPayloadLength];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		ArrayExtension.SetValueUInt16(_rawData, (ushort)(int)blockId, 5, (Endian)0);
		_rawData[7] = (byte)(int)options;
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandStopDeviceBlockTransfer(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > MaxPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {MaxPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandStopDeviceBlockTransfer Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandStopDeviceBlockTransfer(rawData);
	}

	protected static BlockTransferBlockId DecodeBlockId(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return (BlockTransferBlockId)ArrayExtension.GetValueUInt16(decodeBuffer, 5, (Endian)0);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public virtual string ToString()
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X2}, Table Id: 0x{DeviceTableId:X2}, DeviceId: 0x{DeviceId:X2}, Block Id: {BlockId} Options: {Options} Raw Data: {ArrayExtension.DebugDump(_rawData, " ", false)}";
	}
}
