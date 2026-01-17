using System.Runtime.CompilerServices;

namespace IDS.Portable.Common.Manifest;

public struct ManifestPid : IManifestPid
{
	[field: CompilerGenerated]
	public string Name
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[field: CompilerGenerated]
	public ulong Value
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}
}
