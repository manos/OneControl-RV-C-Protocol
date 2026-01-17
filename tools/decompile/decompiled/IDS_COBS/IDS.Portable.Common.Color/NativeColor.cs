using System.Runtime.CompilerServices;

namespace IDS.Portable.Common.Color;

public struct NativeColor
{
	[field: CompilerGenerated]
	public HsvColor AsHsvColor
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public RgbColor AsRgbColor
	{
		[CompilerGenerated]
		get;
	}

	public NativeColor(HsvColor hsvColor)
	{
		AsHsvColor = hsvColor;
		AsRgbColor = hsvColor.ToRgb();
	}

	public NativeColor(RgbColor rgbColor)
	{
		AsHsvColor = rgbColor.ToHsv();
		AsRgbColor = rgbColor;
	}
}
