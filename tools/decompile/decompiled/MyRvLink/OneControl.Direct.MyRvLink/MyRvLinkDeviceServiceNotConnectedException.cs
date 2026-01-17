using System;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceServiceNotConnectedException : global::System.Exception
{
	public MyRvLinkDeviceServiceNotConnectedException(DirectConnectionMyRvLink myRvLink, string message, global::System.Exception? innerException = null)
		: base($"{myRvLink}: {message}: Device Service Not Connected", innerException)
	{
	}
}
