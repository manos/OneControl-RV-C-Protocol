using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace IDS.Portable.Common.Manifest;

[JsonObject(/*Could not decode attribute arguments.*/)]
public struct ManifestLogEntry : IManifestLogEntry
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	private List<IManifestDTC>? _dtcList;

	[JsonProperty]
	[field: CompilerGenerated]
	public global::System.DateTime Timestamp
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	[field: CompilerGenerated]
	public IManifest? Manifest
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	[field: CompilerGenerated]
	public string? ProductUniqueID
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty("DTCListType")]
	[JsonConverter(typeof(ManifestDTCListTypeEnumConverter))]
	[field: CompilerGenerated]
	public ManifestDTCListType DTCsType
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonIgnore]
	public global::System.Collections.Generic.IEnumerable<IManifestDTC>? DTCs => (global::System.Collections.Generic.IEnumerable<IManifestDTC>?)_dtcList;

	[JsonConstructor]
	private ManifestLogEntry(global::System.DateTime timestamp, Manifest manifest, ManifestProduct product, ManifestDTCListType dtcListType, List<ManifestDTC> dtcList)
	{
		Timestamp = timestamp;
		Manifest = manifest;
		ProductUniqueID = product?.UniqueID;
		DTCsType = dtcListType;
		_dtcList = ((dtcList != null) ? Enumerable.ToList<IManifestDTC>(Enumerable.Cast<IManifestDTC>((global::System.Collections.IEnumerable)dtcList)) : null);
	}

	public ManifestLogEntry(IManifest manifest)
	{
		Timestamp = global::System.DateTime.Now;
		Manifest = manifest;
		ProductUniqueID = null;
		DTCsType = ManifestDTCListType.None;
		_dtcList = null;
	}

	public ManifestLogEntry(IManifestProduct product, global::System.Collections.Generic.IEnumerable<IManifestDTC>? dtcCollection, ManifestDTCListType dtcsType)
	{
		Timestamp = global::System.DateTime.Now;
		Manifest = null;
		ProductUniqueID = product.UniqueID;
		DTCsType = dtcsType;
		_dtcList = ((dtcCollection == null) ? new List<IManifestDTC>() : new List<IManifestDTC>(dtcCollection));
	}

	public string ToJSON()
	{
		return JsonConvert.SerializeObject((object)this, (Formatting)1);
	}

	public string ToString()
	{
		try
		{
			return ToJSON();
		}
		catch
		{
			return base.ToString();
		}
	}
}
