using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IDS.Portable.Common.Manifest;

public class ManifestDTCListTypeEnumConverter : StringEnumConverter
{
	private const string LogTag = "ManifestDTCListTypeEnumConverter";

	public override object? ReadJson(JsonReader reader, global::System.Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (string.IsNullOrWhiteSpace(reader.Value?.ToString()))
		{
			return ManifestDTCListType.None;
		}
		try
		{
			return ((StringEnumConverter)this).ReadJson(reader, objectType, existingValue, serializer);
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Error("ManifestDTCListTypeEnumConverter", "Unknown DTCListType, defaulting to {0}: {1}", ManifestDTCListType.None, ex.Message);
			return ManifestDTCListType.None;
		}
	}
}
