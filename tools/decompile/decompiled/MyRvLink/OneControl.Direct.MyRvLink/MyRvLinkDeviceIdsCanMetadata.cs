using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Core.IDS_CAN;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceIdsCanMetadata : MyRvLinkDeviceMetadata, IEquatable<object>
{
	public const MyRvLinkDeviceProtocol DeviceProtocolIdsCan = MyRvLinkDeviceProtocol.IdsCan;

	public const byte MetadataEntryPayloadSize = 17;

	internal const int CanFunctionNameIndex = 2;

	internal const int CanFunctionInstanceIndex = 4;

	internal const int CanRawDeviceCapabilityIndex = 5;

	internal const int CanVersionIndex = 6;

	internal const int CanCircuitIdIndex = 7;

	internal const int CanSoftwarePartNumberIndex = 11;

	internal const int CanSoftwarePartNumberStringLength = 8;

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

	public override byte EncodeSize => 19;

	public MyRvLinkDeviceIdsCanMetadata(FUNCTION_NAME functionName, int functionInstance, byte rawDeviceCapability, byte idsCanVersion, uint circuitId, string softwarePartnumber)
		: base(MyRvLinkDeviceProtocol.IdsCan)
	{
		FunctionName = functionName;
		FunctionInstance = functionInstance;
		RawDeviceCapability = rawDeviceCapability;
		IdsCanVersion = idsCanVersion;
		CircuitId = circuitId;
		SoftwarePartNumber = softwarePartnumber;
	}

	public FUNCTION_CLASS PreferredFunctionClass(DEVICE_TYPE deviceType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return FunctionClassExtension.GetPreferredFunctionClass(deviceType, FunctionName);
	}

	public FUNCTION_CLASS PreferredFunctionClass(MyRvLinkDeviceIdsCan idsCanPhysical)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return PreferredFunctionClass(idsCanPhysical.DeviceType);
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

	public override int EncodeIntoBuffer(byte[] buffer, int offset)
	{
		base.EncodeIntoBuffer(buffer, offset);
		ArrayExtension.SetValueUInt16(buffer, FUNCTION_NAME.op_Implicit(FunctionName), 2 + offset, (Endian)0);
		buffer[4 + offset] = (byte)FunctionInstance;
		buffer[5 + offset] = RawDeviceCapability;
		buffer[6 + offset] = IdsCanVersion;
		ArrayExtension.SetValueUInt32(buffer, CircuitId, 7 + offset, (Endian)0);
		global::System.Array.Clear((global::System.Array)buffer, 11 + offset, 8);
		byte[] bytes = Encoding.ASCII.GetBytes(SoftwarePartNumber ?? string.Empty);
		Buffer.BlockCopy((global::System.Array)bytes, 0, (global::System.Array)buffer, 11 + offset, Math.Min(bytes.Length, 8));
		return EncodeSize;
	}

	public static FUNCTION_NAME DecodeFunctionName(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return FUNCTION_NAME.op_Implicit(ArrayExtension.GetValueUInt16(decodeBuffer, 2, (Endian)0));
	}

	public static int DecodeFunctionInstance(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return decodeBuffer[4];
	}

	public static byte DecodeRawDeviceCapability(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return decodeBuffer[5];
	}

	public static byte DecodeIdsCanVersion(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return decodeBuffer[6];
	}

	public static uint DecodeCircuitNumber(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return ArrayExtension.GetValueUInt32(decodeBuffer, 7, (Endian)0);
	}

	public static string DecodeSoftwarePartNumber(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		byte[] array = new byte[8];
		int i;
		for (i = 11; i < 19 && i < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)decodeBuffer).Count && decodeBuffer[i] != 0; i++)
		{
			array[i - 11] = decodeBuffer[i];
		}
		int num = i - 11;
		if (num <= 0)
		{
			return string.Empty;
		}
		return Encoding.UTF8.GetString(array, 0, num);
	}

	public static MyRvLinkDeviceIdsCanMetadata Decode(global::System.Collections.Generic.IReadOnlyList<byte> buffer)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		MyRvLinkDeviceProtocol myRvLinkDeviceProtocol = MyRvLinkDeviceMetadata.DecodeDeviceProtocol(buffer);
		if (myRvLinkDeviceProtocol != MyRvLinkDeviceProtocol.IdsCan)
		{
			throw new ArgumentException($"Invalid device protocol {myRvLinkDeviceProtocol}, expected {2}", "buffer");
		}
		int num = MyRvLinkDeviceMetadata.DecodePayloadSize(buffer);
		if (num != 17)
		{
			throw new ArgumentException($"Invalid payload size of {num}, expected {17} for {2}", "buffer");
		}
		FUNCTION_NAME functionName = DecodeFunctionName(buffer);
		int functionInstance = DecodeFunctionInstance(buffer);
		byte rawDeviceCapability = DecodeRawDeviceCapability(buffer);
		byte idsCanVersion = DecodeIdsCanVersion(buffer);
		uint circuitId = DecodeCircuitNumber(buffer);
		string softwarePartnumber = DecodeSoftwarePartNumber(buffer);
		return new MyRvLinkDeviceIdsCanMetadata(functionName, functionInstance, rawDeviceCapability, idsCanVersion, circuitId, softwarePartnumber);
	}

	public override string ToString()
	{
		IDS_CAN_VERSION_NUMBER val = IDS_CAN_VERSION_NUMBER.op_Implicit(IdsCanVersion);
		return $"{base.ToString()} {FunctionName.Name}(0x{FUNCTION_NAME.op_Implicit(FunctionName):4X}) {FunctionInstance} Capability: {RawDeviceCapability:X1} CanVersion:{val}({IdsCanVersion:X}) CircuitId: 0x{CircuitId:X} SoftwarePartNumber:`{SoftwarePartNumber}`";
	}
}
