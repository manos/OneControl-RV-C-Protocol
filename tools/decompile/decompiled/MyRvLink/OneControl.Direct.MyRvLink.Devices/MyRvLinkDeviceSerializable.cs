using System.Runtime.CompilerServices;
using IDS.Portable.LogicalDevice.Json;
using Newtonsoft.Json;

namespace OneControl.Direct.MyRvLink.Devices;

[JsonObject(/*Could not decode attribute arguments.*/)]
public class MyRvLinkDeviceSerializable
{
	[JsonProperty]
	[JsonConverter(typeof(ByteArrayJsonHexStringConverter))]
	[field: CompilerGenerated]
	public byte[] RawData
	{
		[CompilerGenerated]
		get;
	}

	public MyRvLinkDeviceSerializable(IMyRvLinkDevice device)
	{
		RawData = new byte[device.EncodeSize];
		device.EncodeIntoBuffer(RawData, 0);
	}

	[JsonConstructor]
	public MyRvLinkDeviceSerializable(byte[] rawData)
	{
		RawData = rawData;
	}

	public IMyRvLinkDevice TryDecode()
	{
		return MyRvLinkDevice.TryDecodeFromRawBuffer(RawData);
	}
}
