using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Core;
using IDS.Core.IDS_CAN;
using IDS.Core.Types;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceIdsCan : MyRvLinkDevice, IMyRvLinkDeviceForLogicalDevice, IMyRvLinkDevice
{
	private const string LogTag = "MyRvLinkDeviceIdsCan";

	public const MyRvLinkDeviceProtocol DeviceProtocolIdsCan = MyRvLinkDeviceProtocol.IdsCan;

	private MyRvLinkDeviceIdsCanMetadata? _metaData;

	public const byte DeviceEntryPayloadSize = 10;

	internal const int CanDeviceTypeIndex = 2;

	internal const int CanDeviceInstanceIndex = 3;

	internal const int CanProductIdIndex = 4;

	internal const int CanMacIndex = 6;

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

	public MyRvLinkDeviceIdsCanMetadata? MetaData
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

	public override byte EncodeSize => 12;

	public MyRvLinkDeviceIdsCan(DEVICE_TYPE deviceType, int deviceInstance, PRODUCT_ID productId, MAC productMacAddress, MyRvLinkDeviceIdsCanMetadata? metaData = null)
		: base(MyRvLinkDeviceProtocol.IdsCan)
	{
		DeviceType = deviceType;
		DeviceInstance = deviceInstance;
		ProductId = productId;
		ProductMacAddress = productMacAddress;
		MetaData = metaData;
	}

	public void UpdateMetadata(MyRvLinkDeviceIdsCanMetadata metadata)
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
		buffer[2 + offset] = DEVICE_TYPE.op_Implicit(DeviceType);
		buffer[3 + offset] = (byte)DeviceInstance;
		ArrayExtension.SetValueUInt16(buffer, PRODUCT_ID.op_Implicit(ProductId), 4 + offset, (Endian)0);
		ArrayExtension.SetValueUInt48(buffer, PhysicalAddress.op_Implicit((PhysicalAddress)(object)ProductMacAddress), 6 + offset, (Endian)0);
		return EncodeSize;
	}

	public static DEVICE_TYPE DecodeCanDeviceType(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return DEVICE_TYPE.op_Implicit(decodeBuffer[2]);
	}

	public static int DecodeDeviceInstance(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return decodeBuffer[3];
	}

	public static PRODUCT_ID DecodeProductId(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return PRODUCT_ID.op_Implicit(ArrayExtension.GetValueUInt16(decodeBuffer, 4, (Endian)0));
	}

	public static MAC DecodeProductMacAddress(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		return new MAC((UInt48)ArrayExtension.GetValueUInt48(decodeBuffer, 6, (Endian)0));
	}

	public static MyRvLinkDeviceIdsCan Decode(global::System.Collections.Generic.IReadOnlyList<byte> buffer)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		MyRvLinkDeviceProtocol myRvLinkDeviceProtocol = MyRvLinkDevice.DecodeDeviceProtocol(buffer);
		if (myRvLinkDeviceProtocol != MyRvLinkDeviceProtocol.IdsCan)
		{
			throw new ArgumentException($"Invalid device protocol {myRvLinkDeviceProtocol}, expected {2}", "buffer");
		}
		int num = MyRvLinkDevice.DecodePayloadSize(buffer);
		if (num != 10)
		{
			throw new ArgumentException($"Invalid payload size of {num}, expected {10} for {2}", "buffer");
		}
		DEVICE_TYPE deviceType = DecodeCanDeviceType(buffer);
		int deviceInstance = DecodeDeviceInstance(buffer);
		PRODUCT_ID productId = DecodeProductId(buffer);
		MAC productMacAddress = DecodeProductMacAddress(buffer);
		return new MyRvLinkDeviceIdsCan(deviceType, deviceInstance, productId, productMacAddress);
	}

	public override string ToString()
	{
		MyRvLinkDeviceIdsCanMetadata metaData = MetaData;
		return $"{base.ToString()} {DeviceType} {DeviceInstance} {ProductId} {ProductMacAddress} {((object)metaData)?.ToString() ?? "NO META DATA"}";
	}
}
