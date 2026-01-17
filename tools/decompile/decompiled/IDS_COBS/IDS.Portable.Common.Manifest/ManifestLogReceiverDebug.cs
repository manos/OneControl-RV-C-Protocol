using System.Collections.Generic;

namespace IDS.Portable.Common.Manifest;

public class ManifestLogReceiverDebug : IManifestLogReceiver
{
	private const string LogTag = "ManifestLogReceiverDebug";

	public void LogManifest(IManifest manifest)
	{
		if (manifest != null)
		{
			ManifestLogEntry manifestLogEntry = new ManifestLogEntry(manifest);
			AddLogEntry(manifestLogEntry);
		}
		else
		{
			TaggedLog.Error("ManifestLogReceiverDebug", "{0} Received null manifest", "LogManifest");
		}
	}

	public void LogCurrentDTCs(IManifestProduct product, global::System.Collections.Generic.IEnumerable<IManifestDTC>? DTCs = null)
	{
		if (product != null)
		{
			ManifestLogEntry manifestLogEntry = new ManifestLogEntry(product, DTCs, ManifestDTCListType.Current);
			AddLogEntry(manifestLogEntry);
		}
	}

	public void LogChangedDTCs(IManifestProduct product, global::System.Collections.Generic.IEnumerable<IManifestDTC> DTCChanges)
	{
		if (product != null)
		{
			ManifestLogEntry manifestLogEntry = new ManifestLogEntry(product, DTCChanges, ManifestDTCListType.Delta);
			AddLogEntry(manifestLogEntry);
		}
	}

	public void AddLogEntry(IManifestLogEntry logEntry)
	{
		TaggedLog.Debug("ManifestLogReceiverDebug", "ManifestLogReceiverDebug: {0}", logEntry);
	}
}
