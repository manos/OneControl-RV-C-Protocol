using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace IDS.Portable.Common.Manifest;

[JsonObject(/*Could not decode attribute arguments.*/)]
public class MainfestRvConfig : IMainfestRvConfig
{
	[JsonProperty]
	[DefaultValue(false)]
	[field: CompilerGenerated]
	public bool MediaDisabled
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	public MainfestRvConfig()
		: this(mediaDisable: false)
	{
	}

	[JsonConstructor]
	public MainfestRvConfig(bool mediaDisable)
	{
		MediaDisabled = mediaDisable;
	}
}
