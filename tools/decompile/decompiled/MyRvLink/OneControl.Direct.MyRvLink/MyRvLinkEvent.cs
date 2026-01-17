using System;
using System.Collections.Generic;
using System.Linq;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public abstract class MyRvLinkEvent<TEvent> : IMyRvLinkEvent, IEquatable<TEvent> where TEvent : IMyRvLinkEvent
{
	protected const int EventTypeIndex = 0;

	public abstract MyRvLinkEventType EventType { get; }

	protected abstract int MinPayloadLength { get; }

	protected abstract byte[] _rawData { get; }

	protected void ValidateEventRawDataBasic(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (rawData == null)
		{
			throw new ArgumentNullException("rawData");
		}
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count < MinPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {EventType} received less then {MinPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		if (EventType != (MyRvLinkEventType)rawData[0])
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {EventType} event type doesn't match {EventType}: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
	}

	public global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public bool Equals(TEvent other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == (object)other)
		{
			return true;
		}
		if (!(other is MyRvLinkEvent<TEvent> myRvLinkEvent))
		{
			return false;
		}
		if (!Enumerable.SequenceEqual<byte>((global::System.Collections.Generic.IEnumerable<byte>)_rawData, (global::System.Collections.Generic.IEnumerable<byte>)myRvLinkEvent._rawData))
		{
			return false;
		}
		return true;
	}

	public override bool Equals(object? obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != typeof(TEvent))
		{
			return false;
		}
		return Equals((TEvent)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Hash(17, _rawData);
	}
}
