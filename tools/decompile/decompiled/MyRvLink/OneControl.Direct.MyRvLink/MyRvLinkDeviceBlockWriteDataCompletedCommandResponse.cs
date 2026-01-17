using System.Collections.Generic;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice.BlockTransfer;

namespace OneControl.Direct.MyRvLink;

internal class MyRvLinkDeviceBlockWriteDataCompletedCommandResponse : MyRvLinkCommandResponseSuccess
{
	private const string LogTag = "MyRvLinkGetDeviceBlockListCommandResponse";

	protected const int BlockIdIndex = 0;

	protected const int ResponseDataLength = 2;

	protected override int MinExtendedDataLength => 2;

	public BlockTransferBlockId BlockId => DecodeBlockId();

	public MyRvLinkDeviceBlockWriteDataCompletedCommandResponse(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkDeviceBlockWriteDataCompletedCommandResponse(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	private BlockTransferBlockId DecodeBlockId()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 2)
		{
			throw new MyRvLinkDecoderException("Unable to decode BlockID, received unexpected data size.");
		}
		return (BlockTransferBlockId)ArrayExtension.GetValueUInt16(base.ExtendedData, 0, (Endian)0);
	}

	public override string ToString()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		return $"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkGetDeviceBlockListCommandResponse"} BlockId: {BlockId}";
	}
}
