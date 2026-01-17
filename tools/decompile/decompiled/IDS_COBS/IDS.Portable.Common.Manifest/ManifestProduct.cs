using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IDS.Portable.Common.Manifest;

[JsonObject(/*Could not decode attribute arguments.*/)]
public class ManifestProduct : IManifestProduct, global::System.Collections.Generic.IEnumerable<IManifestDevice>, global::System.Collections.IEnumerable, IComparable<IManifestProduct>
{
	[JsonProperty(PropertyName = "DeviceList")]
	private List<IManifestDevice> _deviceList;

	[JsonProperty]
	[field: CompilerGenerated]
	public string UniqueID
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public string Name
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public ushort TypeID
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public int AssemblyPartNumber
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public string SoftwarePartNumber
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[JsonProperty]
	[JsonConverter(typeof(VersionConverter))]
	[field: CompilerGenerated]
	public Version ProtocolVersion
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public global::System.Collections.Generic.IEnumerable<IManifestDevice> Devices => (global::System.Collections.Generic.IEnumerable<IManifestDevice>)_deviceList;

	public global::System.Collections.Generic.IEnumerator<IManifestDevice> GetEnumerator()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IEnumerator<IManifestDevice>)(object)_deviceList.GetEnumerator();
	}

	global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
	{
		return (global::System.Collections.IEnumerator)GetEnumerator();
	}

	[JsonConstructor]
	public ManifestProduct(string uniqueID, string name, ushort typeID, int assemblyPartNumber, string softwarePartNumber, Version protocolVersion, List<ManifestDevice> deviceList)
	{
		UniqueID = uniqueID;
		Name = name;
		TypeID = typeID;
		AssemblyPartNumber = assemblyPartNumber;
		SoftwarePartNumber = softwarePartNumber;
		ProtocolVersion = protocolVersion;
		_deviceList = new List<IManifestDevice>((global::System.Collections.Generic.IEnumerable<IManifestDevice>)deviceList);
	}

	public ManifestProduct(string uniqueID, string name, ushort typeID, int assemblyPartNumber, string softwarePartNumber, Version protocolVersion)
		: this(uniqueID, name, typeID, assemblyPartNumber, softwarePartNumber, protocolVersion, new List<ManifestDevice>())
	{
	}

	public void AddManifestDevice(IManifestDevice manifestDevice)
	{
		if (manifestDevice != null)
		{
			_deviceList.Add(manifestDevice);
		}
	}

	public override string ToString()
	{
		try
		{
			return JsonConvert.SerializeObject((object)this, (Formatting)1);
		}
		catch
		{
			return base.ToString();
		}
	}

	public int CompareTo(IManifestProduct other)
	{
		if (this == other)
		{
			return 0;
		}
		if (other == null)
		{
			return 1;
		}
		int num = string.Compare(UniqueID, other.UniqueID, (StringComparison)4);
		if (num != 0)
		{
			return num;
		}
		return TypeID.CompareTo(other.TypeID);
	}
}
