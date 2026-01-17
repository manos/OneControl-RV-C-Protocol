using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common.Manifest;

public interface IManifestLogReceiverPlayer
{
	void LoadWebServiceLog(global::System.Collections.Generic.IEnumerable<IManifestLogEntry> webServiceLogEntries);

	global::System.Threading.Tasks.Task<uint> Replay(IManifestLogReceiver manifestLogReceiver, CancellationToken cancellationToken, float speedFactor = 1f);
}
