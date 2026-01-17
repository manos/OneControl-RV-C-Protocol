using System.Runtime.CompilerServices;
using IDS.Portable.LogicalDevice.Json;
using Newtonsoft.Json;

namespace OneControl.Direct.MyRvLink.Devices;

[JsonObject(/*Could not decode attribute arguments.*/)]
public class MyRvLinkDeviceMetadataSerializable
{
	[JsonProperty]
	[JsonConverter(typeof(ByteArrayJsonHexStringConverter))]
	[field: CompilerGenerated]
	public byte[] RawData
	{
		[CompilerGenerated]
		get;
	}

	public MyRvLinkDeviceMetadataSerializable(IMyRvLinkDeviceMetadata device)
	{
		RawData = new byte[device.EncodeSize];
		device.EncodeIntoBuffer(RawData, 0);
	}

	[JsonConstructor]
	public MyRvLinkDeviceMetadataSerializable(byte[] rawData)
	{
		RawData = rawData;
	}

	public IMyRvLinkDeviceMetadata TryDecode()
	{
		return MyRvLinkDeviceMetadata.TryDecodeFromRawBuffer(RawData);
	}
}
