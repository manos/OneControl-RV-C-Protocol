using System;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceOfflineException : global::System.Exception
{
	public MyRvLinkDeviceOfflineException(DirectConnectionMyRvLink myRvLink, ILogicalDevice logicalDevice, global::System.Exception? innerException = null)
		: base($"{myRvLink}: Device offline for {logicalDevice}", innerException)
	{
	}
}
