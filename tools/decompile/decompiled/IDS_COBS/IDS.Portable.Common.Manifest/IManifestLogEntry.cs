using System;
using System.Collections.Generic;

namespace IDS.Portable.Common.Manifest;

public interface IManifestLogEntry
{
	global::System.DateTime Timestamp { get; }

	IManifest? Manifest { get; }

	string? ProductUniqueID { get; }

	ManifestDTCListType DTCsType { get; }

	global::System.Collections.Generic.IEnumerable<IManifestDTC>? DTCs { get; }
}
