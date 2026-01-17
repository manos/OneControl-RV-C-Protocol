using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace IDS.Portable.Common.Manifest;

public class ManifestLogSerializer
{
	public static string MakeJSONFromWebLog(global::System.Collections.Generic.IEnumerable<IManifestLogEntry> webLog)
	{
		return JsonConvert.SerializeObject((object)webLog, (Formatting)1);
	}

	public static List<IManifestLogEntry> MakeWebLogFromJSON(string json)
	{
		return Enumerable.ToList<IManifestLogEntry>(Enumerable.Cast<IManifestLogEntry>((global::System.Collections.IEnumerable)JsonConvert.DeserializeObject<List<ManifestLogEntry>>(json)));
	}
}
