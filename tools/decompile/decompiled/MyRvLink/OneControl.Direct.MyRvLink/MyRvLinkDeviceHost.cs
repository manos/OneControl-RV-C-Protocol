using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Core;
using IDS.Core.IDS_CAN;
using IDS.Core.Types;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceHost : MyRvLinkDevice, IMyRvLinkDeviceForLogicalDevice, IMyRvLinkDevice
{
	private const string LogTag = "MyRvLinkDeviceHost";

	public const MyRvLinkDeviceProtocol DeviceProtocolHost = MyRvLinkDeviceProtocol.Host;

	public const int DefaultProxyDeviceIdInstance = 15;

	public const int MacSize = 6;

	public readonly bool HasIdsCanStyleMetadata;

	private static readonly MAC DefaultHostDeviceIdMacLegacy = new MAC((UInt48)863642096980L);

	private MyRvLinkDeviceHostMetadata? _metaData;

	private const byte DeviceEntryPayloadSizeWithoutIdsCanData = 0;

	private const byte DeviceEntryPayloadSizeWithIdsCanData = 10;

	[field: CompilerGenerated]
	public DEVICE_TYPE DeviceType
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public int DeviceInstance
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public PRODUCT_ID ProductId
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public MAC ProductMacAddress
	{
		[CompilerGenerated]
		get;
	}

	public byte RawDefaultCapability => MetaData?.RawDeviceCapability ?? 0;

	[field: CompilerGenerated]
	public static MAC DefaultHostDeviceIdMac
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	} = DefaultHostDeviceIdMacLegacy;

	public MyRvLinkDeviceHostMetadata? MetaData
	{
		get
		{
			return _metaData;
		}
		private set
		{
			_metaData = value;
			UpdateLogicalDeviceId();
		}
	}

	[field: CompilerGenerated]
	public ILogicalDeviceId? LogicalDeviceId
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	private byte DeviceEntryPayloadSize
	{
		get
		{
			if (!HasIdsCanStyleMetadata)
			{
				return 0;
			}
			return 10;
		}
	}

	public override byte EncodeSize => (byte)(2 + DeviceEntryPayloadSize);

	public static void SetDefaultHostDeviceIdMac(Guid? uniqueId)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		if (!uniqueId.HasValue)
		{
			TaggedLog.Warning("MyRvLinkDeviceHost", $"Using hard coded default MAC {DefaultHostDeviceIdMac}", global::System.Array.Empty<object>());
			DefaultHostDeviceIdMac = DefaultHostDeviceIdMacLegacy;
			return;
		}
		Guid value = uniqueId.Value;
		byte[] array = ((Guid)(ref value)).ToByteArray();
		if (array.Length != 16)
		{
			TaggedLog.Warning("MyRvLinkDeviceHost", $"Using hard coded default MAC {DefaultHostDeviceIdMac}, invalid Guid {uniqueId}", global::System.Array.Empty<object>());
			DefaultHostDeviceIdMac = DefaultHostDeviceIdMacLegacy;
			return;
		}
		try
		{
			MAC val = new MAC((UInt48)ArrayExtension.GetValueUInt48(array, 10, (Endian)0));
			TaggedLog.Information("MyRvLinkDeviceHost", $"Host using generated MAC of {val} from {uniqueId}", global::System.Array.Empty<object>());
			DefaultHostDeviceIdMac = val;
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Warning("MyRvLinkDeviceHost", $"Using hard coded default MAC {DefaultHostDeviceIdMac}, invalid Guid {uniqueId} because {ex.Message}", global::System.Array.Empty<object>());
			DefaultHostDeviceIdMac = DefaultHostDeviceIdMacLegacy;
		}
	}

	public MyRvLinkDeviceHost()
		: this(DEVICE_TYPE.op_Implicit((byte)36), 15, PRODUCT_ID.BLUETOOTH_GATEWAY_DAUGHTER_BOARD_RVLINK_ESP32_PROGRAMMED_PCBA, DefaultHostDeviceIdMac)
	{
		HasIdsCanStyleMetadata = false;
	}

	private MyRvLinkDeviceHost(DEVICE_TYPE deviceType, int deviceInstance, PRODUCT_ID productId, MAC productMacAddress, MyRvLinkDeviceHostMetadata? metaData = null)
		: base(MyRvLinkDeviceProtocol.Host)
	{
		HasIdsCanStyleMetadata = true;
		DeviceType = deviceType;
		DeviceInstance = deviceInstance;
		ProductId = productId;
		ProductMacAddress = productMacAddress;
		MetaData = metaData;
	}

	public void UpdateMetadata(MyRvLinkDeviceHostMetadata metadata)
	{
		MetaData = metadata;
	}

	private void UpdateLogicalDeviceId()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		LogicalDeviceId = (ILogicalDeviceId?)((MetaData == null) ? ((LogicalDeviceId)null) : new LogicalDeviceId(MakeDeviceId(), ProductMacAddress));
	}

	private DEVICE_ID MakeDeviceId()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		return new DEVICE_ID(ProductId, (byte)0, DeviceType, DeviceInstance, MetaData?.FunctionName ?? FUNCTION_NAME.UNKNOWN, MetaData?.FunctionInstance ?? 0, (byte?)RawDefaultCapability);
	}

	public override int EncodeIntoBuffer(byte[] buffer, int offset)
	{
		base.EncodeIntoBuffer(buffer, offset);
		if (HasIdsCanStyleMetadata)
		{
			buffer[2 + offset] = DEVICE_TYPE.op_Implicit(DeviceType);
			buffer[3 + offset] = (byte)DeviceInstance;
			ArrayExtension.SetValueUInt16(buffer, PRODUCT_ID.op_Implicit(ProductId), 4 + offset, (Endian)0);
			ArrayExtension.SetValueUInt48(buffer, PhysicalAddress.op_Implicit((PhysicalAddress)(object)ProductMacAddress), 6 + offset, (Endian)0);
		}
		return EncodeSize;
	}

	public static MyRvLinkDeviceHost Decode(global::System.Collections.Generic.IReadOnlyList<byte> buffer)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		MyRvLinkDeviceProtocol myRvLinkDeviceProtocol = MyRvLinkDevice.DecodeDeviceProtocol(buffer);
		if (myRvLinkDeviceProtocol != MyRvLinkDeviceProtocol.Host)
		{
			throw new ArgumentException($"Invalid device protocol {myRvLinkDeviceProtocol}, expected {1}", "buffer");
		}
		int num = MyRvLinkDevice.DecodePayloadSize(buffer);
		switch (num)
		{
		case 0:
			return new MyRvLinkDeviceHost();
		case 10:
		{
			DEVICE_TYPE deviceType = MyRvLinkDeviceIdsCan.DecodeCanDeviceType(buffer);
			int deviceInstance = MyRvLinkDeviceIdsCan.DecodeDeviceInstance(buffer);
			PRODUCT_ID productId = MyRvLinkDeviceIdsCan.DecodeProductId(buffer);
			MAC productMacAddress = MyRvLinkDeviceIdsCan.DecodeProductMacAddress(buffer);
			return new MyRvLinkDeviceHost(deviceType, deviceInstance, productId, productMacAddress);
		}
		default:
			throw new ArgumentException($"Invalid payload size of {num}, expected {0} or {10} for {1}", "buffer");
		}
	}

	public override string ToString()
	{
		MyRvLinkDeviceHostMetadata metaData = MetaData;
		return $"{base.ToString()} {DeviceType} {DeviceInstance} {ProductId} {ProductMacAddress} {((object)metaData)?.ToString() ?? "NO META DATA"}";
	}
}
