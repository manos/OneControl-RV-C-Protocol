using System;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceServiceNotStartedException : global::System.Exception
{
	public MyRvLinkDeviceServiceNotStartedException(DirectConnectionMyRvLink myRvLink, string message, global::System.Exception? innerException = null)
		: base($"{myRvLink}: {message}: Device Service Not Started", innerException)
	{
	}
}
