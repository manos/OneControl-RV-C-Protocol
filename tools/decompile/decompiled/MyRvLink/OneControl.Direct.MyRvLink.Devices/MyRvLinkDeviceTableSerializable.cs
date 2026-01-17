using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace OneControl.Direct.MyRvLink.Devices;

[JsonObject(/*Could not decode attribute arguments.*/)]
public class MyRvLinkDeviceTableSerializable
{
	public const string LogTag = "MyRvLinkDeviceTableSerializable";

	[JsonProperty]
	[field: CompilerGenerated]
	public global::System.DateTime CreatedTimeStamp
	{
		[CompilerGenerated]
		get;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public uint DeviceTableCrc
	{
		[CompilerGenerated]
		get;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public global::System.Collections.Generic.IReadOnlyList<MyRvLinkDeviceSerializable> DevicesSerializable
	{
		[CompilerGenerated]
		get;
	}

	[JsonConstructor]
	public MyRvLinkDeviceTableSerializable(uint deviceTableCrc, global::System.Collections.Generic.IReadOnlyList<MyRvLinkDeviceSerializable> devicesSerializable, global::System.DateTime createdTimeStamp)
	{
		DeviceTableCrc = deviceTableCrc;
		DevicesSerializable = (global::System.Collections.Generic.IReadOnlyList<MyRvLinkDeviceSerializable>)Enumerable.ToList<MyRvLinkDeviceSerializable>((global::System.Collections.Generic.IEnumerable<MyRvLinkDeviceSerializable>)devicesSerializable);
		CreatedTimeStamp = createdTimeStamp;
	}

	public global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice> TryDecode()
	{
		return (global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice>)Enumerable.ToList<IMyRvLinkDevice>(Enumerable.Select<MyRvLinkDeviceSerializable, IMyRvLinkDevice>((global::System.Collections.Generic.IEnumerable<MyRvLinkDeviceSerializable>)DevicesSerializable, (Func<MyRvLinkDeviceSerializable, IMyRvLinkDevice>)((MyRvLinkDeviceSerializable device) => device.TryDecode())));
	}
}
