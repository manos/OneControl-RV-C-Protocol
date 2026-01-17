using System;

namespace IDS.Portable.Common;

public class ConstructorNotFoundException : global::System.Exception
{
	private const string ConstructorNotFoundMessage = "Singleton<TSingleton> derived types require a non-public default constructor.";

	public ConstructorNotFoundException()
		: base("Singleton<TSingleton> derived types require a non-public default constructor.")
	{
	}

	public ConstructorNotFoundException(string auxMessage)
		: base("Singleton<TSingleton> derived types require a non-public default constructor. - " + auxMessage)
	{
	}

	public ConstructorNotFoundException(string auxMessage, global::System.Exception inner)
		: base("Singleton<TSingleton> derived types require a non-public default constructor. - " + auxMessage, inner)
	{
	}
}
