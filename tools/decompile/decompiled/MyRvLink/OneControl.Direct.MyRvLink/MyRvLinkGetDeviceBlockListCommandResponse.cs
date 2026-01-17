using System.Collections.Generic;
using System.Linq;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice.BlockTransfer;

namespace OneControl.Direct.MyRvLink;

internal class MyRvLinkGetDeviceBlockListCommandResponse : MyRvLinkCommandResponseSuccess
{
	private const string LogTag = "MyRvLinkGetDeviceBlockListCommandResponse";

	private List<BlockTransferBlockId>? _blockIds;

	protected const int BlockSentCountIndex = 0;

	protected const int BlockSentCountLength = 1;

	protected const int BlockIdIndex = 1;

	protected const int BlockIdLength = 2;

	protected override int MinExtendedDataLength => 1;

	public global::System.Collections.Generic.IReadOnlyList<BlockTransferBlockId> BlockIds => (global::System.Collections.Generic.IReadOnlyList<BlockTransferBlockId>)(_blockIds ?? (_blockIds = DecodeBlockIds()));

	public MyRvLinkGetDeviceBlockListCommandResponse(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkGetDeviceBlockListCommandResponse(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	protected List<BlockTransferBlockId> DecodeBlockIds()
	{
		HashSet<BlockTransferBlockId> val = new HashSet<BlockTransferBlockId>();
		if (base.ExtendedData == null)
		{
			return Enumerable.ToList<BlockTransferBlockId>((global::System.Collections.Generic.IEnumerable<BlockTransferBlockId>)val);
		}
		byte b = base.ExtendedData[0];
		if (base.ExtendedDataLength != b * 2 + 1)
		{
			throw new MyRvLinkDecoderException($"Unable to decode block ids, expected {b} but received {b * 2}");
		}
		for (int i = 1; i < base.ExtendedDataLength; i += 2)
		{
			ushort valueUInt = ArrayExtension.GetValueUInt16(base.ExtendedData, i, (Endian)0);
			val.Add((BlockTransferBlockId)valueUInt);
		}
		return Enumerable.ToList<BlockTransferBlockId>((global::System.Collections.Generic.IEnumerable<BlockTransferBlockId>)val);
	}

	public override string ToString()
	{
		return $"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkGetDeviceBlockListCommandResponse"} Blocks Sent: {base.ExtendedData[0]}";
	}
}
