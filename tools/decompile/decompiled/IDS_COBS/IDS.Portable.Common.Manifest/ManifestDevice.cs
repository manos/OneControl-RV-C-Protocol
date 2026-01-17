using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace IDS.Portable.Common.Manifest;

[JsonObject(/*Could not decode attribute arguments.*/)]
public class ManifestDevice : IManifestDevice, IComparable<IManifestDevice>
{
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
	public byte Instance
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public string FunctionName
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public ushort FunctionTypeID
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public string FunctionClass
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public byte FunctionInstance
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public int Capabilities
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public uint Circuit
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public bool IsOnline
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	[field: CompilerGenerated]
	public Dictionary<string, string>? CustomAttribute
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	[field: CompilerGenerated]
	public Dictionary<ushort, IManifestPid>? Pids
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	[JsonConstructor]
	public ManifestDevice(string name, ushort typeID, byte instance, string functionName, ushort functionTypeID, string functionClass, byte functionInstance, int capabilities, uint circuit, bool isOnline, Dictionary<string, string>? customAttribute = null, Dictionary<ushort, ManifestPid>? pids = null)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		Name = name;
		TypeID = typeID;
		Instance = instance;
		FunctionName = functionName;
		FunctionTypeID = functionTypeID;
		FunctionClass = functionClass;
		FunctionInstance = functionInstance;
		Capabilities = capabilities;
		Circuit = circuit;
		IsOnline = isOnline;
		CustomAttribute = customAttribute;
		Pids = null;
		if (pids == null)
		{
			return;
		}
		Pids = new Dictionary<ushort, IManifestPid>();
		Enumerator<ushort, ManifestPid> enumerator = pids.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ushort, ManifestPid> current = enumerator.Current;
				Pids.Add(current.Key, (IManifestPid)current.Value);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
	}

	public void SetCustomAttribute(string attribute, string value)
	{
		if (CustomAttribute == null)
		{
			CustomAttribute = new Dictionary<string, string>();
		}
		CustomAttribute[attribute] = value;
	}

	public string? TryGetCustomAttribute(string attribute)
	{
		if (CustomAttribute == null || !CustomAttribute.ContainsKey(attribute))
		{
			return null;
		}
		return CustomAttribute[attribute];
	}

	public override string ToString()
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

	public int CompareTo(IManifestDevice other)
	{
		if (this == other)
		{
			return 0;
		}
		if (other == null)
		{
			return 1;
		}
		int num = string.Compare(Name, other.Name, (StringComparison)4);
		if (num != 0)
		{
			return num;
		}
		return Instance.CompareTo(other.Instance);
	}
}
