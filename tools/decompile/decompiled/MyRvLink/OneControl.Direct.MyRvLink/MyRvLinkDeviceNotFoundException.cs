using System;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceNotFoundException : global::System.Exception
{
	public MyRvLinkDeviceNotFoundException(DirectConnectionMyRvLink myRvLink, ILogicalDevice logicalDevice, global::System.Exception? innerException = null)
		: base($"{myRvLink}: Unable to find device for {logicalDevice}", innerException)
	{
	}
}
