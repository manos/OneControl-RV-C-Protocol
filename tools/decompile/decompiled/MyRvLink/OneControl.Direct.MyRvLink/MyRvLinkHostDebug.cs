using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkHostDebug : MyRvLinkEvent<MyRvLinkHostDebug>
{
	public override MyRvLinkEventType EventType => MyRvLinkEventType.HostDebug;

	protected override int MinPayloadLength => 0;

	[field: CompilerGenerated]
	protected override byte[] _rawData
	{
		[CompilerGenerated]
		get;
	}

	protected MyRvLinkHostDebug(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		if (rawData == null)
		{
			_rawData = global::System.Array.Empty<byte>();
			return;
		}
		int count = ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count;
		_rawData = new byte[count];
		for (int i = 0; i < count; i++)
		{
			_rawData[i] = rawData[i];
		}
	}

	public static MyRvLinkHostDebug Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkHostDebug(rawData);
	}

	public override string ToString()
	{
		return $"{EventType} {ArrayExtension.DebugDump(_rawData, " ", false)}";
	}
}
