using System.Collections.Generic;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice.BlockTransfer;

namespace OneControl.Direct.MyRvLink;

internal class MyRvLinkDeviceBlockWriteDataCommandResponse : MyRvLinkCommandResponseSuccess
{
	private const string LogTag = "MyRvLinkDeviceBlockWriteDataCommandResponse";

	protected const int BlockIdIndex = 0;

	protected const int Crc32Index = 2;

	protected const int ResponseDataLength = 6;

	protected override int MinExtendedDataLength => 6;

	public BlockTransferBlockId BlockId => DecodeBlockId();

	public uint Crc32 => DecodeCrc32();

	public MyRvLinkDeviceBlockWriteDataCommandResponse(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkDeviceBlockWriteDataCommandResponse(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	private BlockTransferBlockId DecodeBlockId()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 6)
		{
			throw new MyRvLinkDecoderException("Unable to decode BlockID, received unexpected data size.");
		}
		return (BlockTransferBlockId)ArrayExtension.GetValueUInt16(base.ExtendedData, 0, (Endian)0);
	}

	private uint DecodeCrc32()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 6)
		{
			throw new MyRvLinkDecoderException("Unable to decode crc, received unexpected data size.");
		}
		return ArrayExtension.GetValueUInt32(base.ExtendedData, 2, (Endian)0);
	}

	public override string ToString()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		return $"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkDeviceBlockWriteDataCommandResponse"} BlockId {BlockId} Crc32: {Crc32}";
	}
}
