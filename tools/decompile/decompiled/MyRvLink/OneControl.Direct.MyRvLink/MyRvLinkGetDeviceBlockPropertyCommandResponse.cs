using System.Collections.Generic;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using IDS.Portable.LogicalDevice.BlockTransfer;

namespace OneControl.Direct.MyRvLink;

internal class MyRvLinkGetDeviceBlockPropertyCommandResponse : MyRvLinkCommandResponseSuccess
{
	private const string LogTag = "MyRvLinkGetDeviceBlockPropertyCommandResponse";

	private const int BlockIdIndex = 0;

	private const int PropertyIndex = 2;

	private const int ResponseDataIndex = 3;

	private const int FlagDataLength = 4;

	private const int ReadSessionIdDataLength = 5;

	private const int WriteSessionIdDataLength = 5;

	private const int BlockCapacityDataLength = 7;

	private const int CurrentBlockSizeIdDataLength = 7;

	private const int CrcDataLength = 7;

	private const int BlockStartAddressDataLength = 7;

	protected override int MinExtendedDataLength => 1;

	public BlockTransferPropertyFlags Flags => DecodeBlockFlags();

	public LogicalDeviceSessionType ReadSessionId => DecodeReadSessionId();

	public LogicalDeviceSessionType WriteSessionId => DecodeWriteSessionId();

	public uint BlockCapacity => DecodeBlockCapacity();

	public uint CurrentBlockSize => DecodeCurrentBlockSize();

	public uint Crc => DecodeCrc();

	public uint BlockStartAddress => DecodeBlockStartAddress();

	public BlockTransferBlockId BlockId => DecodeBlockId(base.ExtendedData);

	public BlockTransferPropertyId Property => (BlockTransferPropertyId)base.ExtendedData[2];

	public MyRvLinkGetDeviceBlockPropertyCommandResponse(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkGetDeviceBlockPropertyCommandResponse(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	private BlockTransferPropertyFlags DecodeBlockFlags()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 4)
		{
			throw new MyRvLinkDecoderException("Unable to decode flags, received unexpected data size.");
		}
		return (BlockTransferPropertyFlags)base.ExtendedData[3];
	}

	private LogicalDeviceSessionType DecodeReadSessionId()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 5)
		{
			throw new MyRvLinkDecoderException("Unable to decode read session id, received unexpected data size.");
		}
		return (LogicalDeviceSessionType)ArrayExtension.GetValueUInt16(base.ExtendedData, 3, (Endian)0);
	}

	private LogicalDeviceSessionType DecodeWriteSessionId()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 5)
		{
			throw new MyRvLinkDecoderException("Unable to decode write session id, received unexpected data size.");
		}
		return (LogicalDeviceSessionType)ArrayExtension.GetValueUInt16(base.ExtendedData, 3, (Endian)0);
	}

	private uint DecodeBlockCapacity()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 7)
		{
			throw new MyRvLinkDecoderException("Unable to decode block capacity, received unexpected data size.");
		}
		return ArrayExtension.GetValueUInt32(base.ExtendedData, 3, (Endian)0);
	}

	private uint DecodeCurrentBlockSize()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 7)
		{
			throw new MyRvLinkDecoderException("Unable to decode current block size, received unexpected data size.");
		}
		return ArrayExtension.GetValueUInt32(base.ExtendedData, 3, (Endian)0);
	}

	private uint DecodeCrc()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 7)
		{
			throw new MyRvLinkDecoderException("Unable to decode crc, received unexpected data size.");
		}
		return ArrayExtension.GetValueUInt32(base.ExtendedData, 3, (Endian)0);
	}

	private uint DecodeBlockStartAddress()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 7)
		{
			throw new MyRvLinkDecoderException("Unable to decode block start address, received unexpected data size.");
		}
		return ArrayExtension.GetValueUInt32(base.ExtendedData, 3, (Endian)0);
	}

	protected static BlockTransferBlockId DecodeBlockId(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return (BlockTransferBlockId)ArrayExtension.GetValueUInt16(decodeBuffer, 0, (Endian)0);
	}

	public override string ToString()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		return $"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkGetDeviceBlockPropertyCommandResponse"} BlockID {BlockId} Property {base.ExtendedData}";
	}
}
