using System.Collections.Generic;

namespace IDS.Portable.Common.Manifest;

public class ManifestLogReceiverNone : IManifestLogReceiver
{
	public void LogManifest(IManifest manifest)
	{
	}

	public void LogCurrentDTCs(IManifestProduct product, global::System.Collections.Generic.IEnumerable<IManifestDTC>? DTCs = null)
	{
	}

	public void LogChangedDTCs(IManifestProduct product, global::System.Collections.Generic.IEnumerable<IManifestDTC> DTCChanges)
	{
	}
}
