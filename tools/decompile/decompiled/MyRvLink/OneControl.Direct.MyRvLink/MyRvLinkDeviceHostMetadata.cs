using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Core.IDS_CAN;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceHostMetadata : MyRvLinkDeviceMetadata, IEquatable<object>
{
	public const MyRvLinkDeviceProtocol DeviceProtocolHost = MyRvLinkDeviceProtocol.Host;

	public const int DefaultProxyFunctionInstance = 15;

	public readonly bool HasIdsCanStyleMetadata;

	private const byte DeviceEntryPayloadSizeWithoutIdsCanMetadata = 0;

	private const byte DeviceEntryPayloadSizeWithIdsCanMetadata = 17;

	[field: CompilerGenerated]
	public FUNCTION_NAME FunctionName
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public int FunctionInstance
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public byte RawDeviceCapability
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public byte IdsCanVersion
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public uint CircuitId
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public string SoftwarePartNumber
	{
		[CompilerGenerated]
		get;
	}

	private byte DeviceEntryPayloadSize
	{
		get
		{
			if (!HasIdsCanStyleMetadata)
			{
				return 0;
			}
			return 17;
		}
	}

	public override byte EncodeSize => (byte)(2 + DeviceEntryPayloadSize);

	public FUNCTION_CLASS PreferredFunctionClass(DEVICE_TYPE deviceType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return FunctionClassExtension.GetPreferredFunctionClass(deviceType, FunctionName);
	}

	public MyRvLinkDeviceHostMetadata()
		: base(MyRvLinkDeviceProtocol.Host)
	{
		HasIdsCanStyleMetadata = false;
		FunctionName = FUNCTION_NAME.op_Implicit((ushort)323);
		FunctionInstance = 15;
		RawDeviceCapability = 0;
		IdsCanVersion = IDS_CAN_VERSION_NUMBER.op_Implicit(IDS_CAN_VERSION_NUMBER.UNKNOWN);
		CircuitId = 0u;
		SoftwarePartNumber = string.Empty;
	}

	private MyRvLinkDeviceHostMetadata(FUNCTION_NAME functionName, int functionInstance, byte rawDeviceCapability, byte idsCanVersion, uint circuitId, string softwarePartnumber)
		: base(MyRvLinkDeviceProtocol.Host)
	{
		HasIdsCanStyleMetadata = true;
		FunctionName = functionName;
		FunctionInstance = functionInstance;
		RawDeviceCapability = rawDeviceCapability;
		IdsCanVersion = idsCanVersion;
		CircuitId = circuitId;
		SoftwarePartNumber = softwarePartnumber;
	}

	public override int EncodeIntoBuffer(byte[] buffer, int offset)
	{
		base.EncodeIntoBuffer(buffer, offset);
		if (HasIdsCanStyleMetadata)
		{
			ArrayExtension.SetValueUInt16(buffer, FUNCTION_NAME.op_Implicit(FunctionName), 2 + offset, (Endian)0);
			buffer[4 + offset] = (byte)FunctionInstance;
			buffer[5 + offset] = RawDeviceCapability;
			buffer[6 + offset] = IdsCanVersion;
			ArrayExtension.SetValueUInt32(buffer, CircuitId, 7 + offset, (Endian)0);
			global::System.Array.Clear((global::System.Array)buffer, 11 + offset, 8);
			byte[] bytes = Encoding.ASCII.GetBytes(SoftwarePartNumber ?? string.Empty);
			Buffer.BlockCopy((global::System.Array)bytes, 0, (global::System.Array)buffer, 11 + offset, Math.Min(bytes.Length, 8));
		}
		return EncodeSize;
	}

	public static MyRvLinkDeviceHostMetadata Decode(global::System.Collections.Generic.IReadOnlyList<byte> buffer)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		MyRvLinkDeviceProtocol myRvLinkDeviceProtocol = MyRvLinkDeviceMetadata.DecodeDeviceProtocol(buffer);
		if (myRvLinkDeviceProtocol != MyRvLinkDeviceProtocol.Host)
		{
			throw new ArgumentException($"Invalid device protocol {myRvLinkDeviceProtocol}, expected {1}", "buffer");
		}
		int num = MyRvLinkDeviceMetadata.DecodePayloadSize(buffer);
		switch (num)
		{
		case 0:
			return new MyRvLinkDeviceHostMetadata();
		case 17:
		{
			FUNCTION_NAME functionName = MyRvLinkDeviceIdsCanMetadata.DecodeFunctionName(buffer);
			int functionInstance = MyRvLinkDeviceIdsCanMetadata.DecodeFunctionInstance(buffer);
			byte rawDeviceCapability = MyRvLinkDeviceIdsCanMetadata.DecodeRawDeviceCapability(buffer);
			byte idsCanVersion = MyRvLinkDeviceIdsCanMetadata.DecodeIdsCanVersion(buffer);
			uint circuitId = MyRvLinkDeviceIdsCanMetadata.DecodeCircuitNumber(buffer);
			string softwarePartnumber = MyRvLinkDeviceIdsCanMetadata.DecodeSoftwarePartNumber(buffer);
			return new MyRvLinkDeviceHostMetadata(functionName, functionInstance, rawDeviceCapability, idsCanVersion, circuitId, softwarePartnumber);
		}
		default:
			throw new ArgumentException($"Invalid payload size of {num}, expected {0} or {17} for {17}", "buffer");
		}
	}

	public override bool Equals(object obj)
	{
		if (obj is MyRvLinkDeviceIdsCanMetadata myRvLinkDeviceIdsCanMetadata && EqualityComparer<FUNCTION_NAME>.Default.Equals(FunctionName, myRvLinkDeviceIdsCanMetadata.FunctionName) && FunctionInstance == myRvLinkDeviceIdsCanMetadata.FunctionInstance && RawDeviceCapability == myRvLinkDeviceIdsCanMetadata.RawDeviceCapability && IdsCanVersion == myRvLinkDeviceIdsCanMetadata.IdsCanVersion)
		{
			return string.Compare(SoftwarePartNumber, myRvLinkDeviceIdsCanMetadata.SoftwarePartNumber, (StringComparison)4) == 0;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Hash<byte>(HashCode.Hash<FUNCTION_NAME>(HashCode.Hash<int>(17, FunctionInstance), FunctionName), RawDeviceCapability);
	}

	public override string ToString()
	{
		IDS_CAN_VERSION_NUMBER val = IDS_CAN_VERSION_NUMBER.op_Implicit(IdsCanVersion);
		return $"{base.ToString()} {FunctionName.Name}(0x{FUNCTION_NAME.op_Implicit(FunctionName):4X}) {FunctionInstance} Capability: {RawDeviceCapability:X1} CanVersion:{val}({IdsCanVersion:X}) CircuitId: 0x{CircuitId:X} SoftwarePartNumber:`{SoftwarePartNumber}`";
	}
}
