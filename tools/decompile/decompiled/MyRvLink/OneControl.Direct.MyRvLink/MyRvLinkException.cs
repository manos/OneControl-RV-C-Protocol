using System;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkException : global::System.Exception
{
	public MyRvLinkException(string message, global::System.Exception? innerException = null)
		: base(message, innerException)
	{
	}
}
