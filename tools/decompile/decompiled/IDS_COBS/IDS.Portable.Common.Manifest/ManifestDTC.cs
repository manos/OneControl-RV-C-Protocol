using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace IDS.Portable.Common.Manifest;

[JsonObject(/*Could not decode attribute arguments.*/)]
public class ManifestDTC : IManifestDTC
{
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
	public string Name
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public bool IsActive
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public bool IsStored
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public int PowerCyclesCounter
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonConstructor]
	public ManifestDTC(ushort typeID, string name, bool isActive, bool isStored, int powerCyclesCounter)
	{
		TypeID = typeID;
		Name = name;
		IsActive = isActive;
		IsStored = isStored;
		PowerCyclesCounter = powerCyclesCounter;
	}

	public virtual string ToString()
	{
		try
		{
			return JsonConvert.SerializeObject((object)this);
		}
		catch
		{
			return base.ToString();
		}
	}
}
