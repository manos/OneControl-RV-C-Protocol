using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice.BlockTransfer;

namespace OneControl.Direct.MyRvLink;

internal class MyRvLinkCommandDeviceBlockWriteData : MyRvLinkCommand
{
	protected const int RequiredHeaderSize = 12;

	private const int MaxDataLength = 128;

	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int BlockIdStartIndex = 5;

	private const int AddressOffsetIndex = 7;

	private const int SizeIndex = 11;

	private const int DataStartIndex = 12;

	private byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandDeviceBlockWriteData";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.DeviceBlockWriteData;

	protected override int MinPayloadLength => 12;

	private int MaxPayloadLength => 140;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public BlockTransferBlockId BlockId => DecodeBlockId(_rawData);

	public byte Size => _rawData[11];

	public uint AddressOffset => DecodeAddressOffset(_rawData);

	public global::System.Collections.Generic.IReadOnlyList<byte> Data => (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 12, _rawData.Length - 12);

	public MyRvLinkCommandDeviceBlockWriteData(ushort clientCommandId, byte deviceTableId, byte deviceId, BlockTransferBlockId blockId, uint addressOffset, byte size, global::System.Collections.Generic.IReadOnlyList<byte>? data = null)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected I4, but got Unknown
		_rawData = new byte[MaxPayloadLength];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		ArrayExtension.SetValueUInt16(_rawData, (ushort)(int)blockId, 5, (Endian)0);
		UpdateCommand(addressOffset, size, data);
	}

	protected MyRvLinkCommandDeviceBlockWriteData(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > MaxPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {MaxPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public void UpdateCommand(uint addressOffset, byte size, global::System.Collections.Generic.IReadOnlyList<byte>? data = null)
	{
		ArrayExtension.SetValueUInt32(_rawData, addressOffset, 7, (Endian)0);
		_rawData[11] = size;
		if (data != null)
		{
			if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)data).Count > 128)
			{
				throw new MyRvLinkException($"Block transfer data size is greater than the maximum of: {128}");
			}
			for (int i = 0; i < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)data).Count; i++)
			{
				_rawData[12 + i] = data[i];
			}
		}
		ValidateCommand(_rawData, ClientCommandId);
	}

	public override IMyRvLinkCommandEvent DecodeCommandEvent(IMyRvLinkCommandEvent commandEvent)
	{
		if (commandEvent is MyRvLinkCommandResponseSuccess myRvLinkCommandResponseSuccess)
		{
			if (myRvLinkCommandResponseSuccess.IsCommandCompleted)
			{
				return new MyRvLinkDeviceBlockWriteDataCompletedCommandResponse(myRvLinkCommandResponseSuccess);
			}
			return new MyRvLinkDeviceBlockWriteDataCommandResponse(myRvLinkCommandResponseSuccess);
		}
		return commandEvent;
	}

	public static MyRvLinkCommandDeviceBlockWriteData Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandDeviceBlockWriteData(rawData);
	}

	protected static BlockTransferBlockId DecodeBlockId(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return (BlockTransferBlockId)ArrayExtension.GetValueUInt16(decodeBuffer, 5, (Endian)0);
	}

	protected static uint DecodeAddressOffset(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return ArrayExtension.GetValueUInt32(decodeBuffer, 7, (Endian)0);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		int num = Size + 12;
		int num2 = Math.Min(num, _rawData.Length);
		if (num2 != num)
		{
			TaggedLog.Warning(LogTag, $"Size truncated from {Size} to {num2}", global::System.Array.Empty<object>());
		}
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, num2);
	}

	public virtual string ToString()
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X2}, Table Id: 0x{DeviceTableId:X2}, DeviceId: 0x{DeviceId:X2}, Block Id: {BlockId} Address Offset: {DecodeAddressOffset(_rawData):X2} Size: {Size} Raw Data: {ArrayExtension.DebugDump(_rawData, " ", false)}";
	}
}
