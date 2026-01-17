using System;
using System.Collections;
using System.Collections.Generic;

namespace IDS.Portable.Common.Manifest;

public interface IManifestProduct : global::System.Collections.Generic.IEnumerable<IManifestDevice>, global::System.Collections.IEnumerable, IComparable<IManifestProduct>
{
	string UniqueID { get; }

	string Name { get; }

	ushort TypeID { get; }

	int AssemblyPartNumber { get; }

	string SoftwarePartNumber { get; set; }

	Version ProtocolVersion { get; }

	global::System.Collections.Generic.IEnumerable<IManifestDevice> Devices { get; }

	void AddManifestDevice(IManifestDevice manifestDevice);
}
