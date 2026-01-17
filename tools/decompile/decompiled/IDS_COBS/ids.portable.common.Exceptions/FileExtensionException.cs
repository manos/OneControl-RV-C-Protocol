using System;

namespace ids.portable.common.Exceptions;

public class FileExtensionException : global::System.Exception
{
	public FileExtensionException(string message, global::System.Exception innerException)
		: base(message, innerException)
	{
	}
}
