using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceMetadata : IMyRvLinkDeviceMetadata
{
	private const string LogTag = "MyRvLinkDeviceMetadata";

	public const int EncodeHeaderSize = 2;

	protected const int DeviceProtocolIndex = 0;

	protected const int DeviceEntrySizeIndex = 1;

	[field: CompilerGenerated]
	public MyRvLinkDeviceProtocol Protocol
	{
		[CompilerGenerated]
		get;
	}

	public virtual byte EncodeSize => 2;

	public MyRvLinkDeviceMetadata(MyRvLinkDeviceProtocol protocol)
	{
		Protocol = protocol;
	}

	public virtual string ToString()
	{
		return $"{Protocol}";
	}

	public static MyRvLinkDeviceProtocol DecodeDeviceProtocol(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return (MyRvLinkDeviceProtocol)decodeBuffer[0];
	}

	public static int DecodePayloadSize(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return decodeBuffer[1];
	}

	public virtual int EncodeIntoBuffer(byte[] buffer, int offset)
	{
		buffer[offset] = (byte)Protocol;
		buffer[1 + offset] = (byte)(EncodeSize - 2);
		return EncodeSize;
	}

	public static IMyRvLinkDeviceMetadata TryDecodeFromRawBuffer(global::System.Collections.Generic.IReadOnlyList<byte> buffer)
	{
		MyRvLinkDeviceProtocol myRvLinkDeviceProtocol = MyRvLinkDeviceProtocol.None;
		try
		{
			myRvLinkDeviceProtocol = MyRvLinkDevice.DecodeDeviceProtocol(buffer);
			return myRvLinkDeviceProtocol switch
			{
				MyRvLinkDeviceProtocol.Host => MyRvLinkDeviceHostMetadata.Decode(buffer), 
				MyRvLinkDeviceProtocol.IdsCan => MyRvLinkDeviceIdsCanMetadata.Decode(buffer), 
				MyRvLinkDeviceProtocol.None => new MyRvLinkDeviceMetadata(myRvLinkDeviceProtocol), 
				_ => new MyRvLinkDeviceMetadata(myRvLinkDeviceProtocol), 
			};
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Debug("MyRvLinkDeviceMetadata", $"Error trying to decode device METADATA for {myRvLinkDeviceProtocol} returning unknown device: {ex.Message}", global::System.Array.Empty<object>());
			return new MyRvLinkDeviceMetadata(myRvLinkDeviceProtocol);
		}
	}
}
