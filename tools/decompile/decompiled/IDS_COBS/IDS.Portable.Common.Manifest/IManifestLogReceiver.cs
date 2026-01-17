using System.Collections.Generic;

namespace IDS.Portable.Common.Manifest;

public interface IManifestLogReceiver
{
	void LogManifest(IManifest manifest);

	void LogCurrentDTCs(IManifestProduct product, global::System.Collections.Generic.IEnumerable<IManifestDTC>? DTCs = null);

	void LogChangedDTCs(IManifestProduct product, global::System.Collections.Generic.IEnumerable<IManifestDTC> DTCChanges);
}
