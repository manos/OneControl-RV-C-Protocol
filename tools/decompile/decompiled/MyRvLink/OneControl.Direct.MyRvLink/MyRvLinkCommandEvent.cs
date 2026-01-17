using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public abstract class MyRvLinkCommandEvent : IMyRvLinkCommandEvent, IMyRvLinkEvent
{
	private const string LogTag = "MyRvLinkCommandEvent";

	public const byte CommandCompletedMask = 128;

	protected static global::System.Collections.Generic.IReadOnlyList<byte> EmptyExtendedData = global::System.Array.Empty<byte>();

	private readonly int _indexOfExtendedData;

	private readonly byte[]? _extendedRawData;

	private const int EventTypeIndex = 0;

	private const int ClientCommandIdStartIndex = 1;

	private const int CommandEventTypeIndex = 3;

	[field: CompilerGenerated]
	public ushort ClientCommandId
	{
		[CompilerGenerated]
		get;
	}

	public bool IsCommandCompleted => (CommandResponseType & (MyRvLinkCommandResponseType)128) == (MyRvLinkCommandResponseType)128;

	[field: CompilerGenerated]
	protected MyRvLinkCommandResponseType CommandResponseType
	{
		[CompilerGenerated]
		get;
	}

	public global::System.Collections.Generic.IReadOnlyList<byte> ExtendedData
	{
		get
		{
			byte[]? extendedRawData = _extendedRawData;
			return GetExtendedData(0, (extendedRawData != null) ? extendedRawData.Length : 0);
		}
	}

	public int ExtendedDataLength
	{
		get
		{
			byte[]? extendedRawData = _extendedRawData;
			if (extendedRawData == null)
			{
				return 0;
			}
			return extendedRawData.Length;
		}
	}

	[field: CompilerGenerated]
	public MyRvLinkEventType EventType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkEventType.DeviceCommand;

	protected abstract int MinPayloadLength { get; }

	protected virtual int MinExtendedDataLength => 0;

	protected MyRvLinkCommandEvent(ushort clientCommandId, MyRvLinkCommandResponseType commandResponseType, int indexOfExtendedData, global::System.Collections.Generic.IReadOnlyList<byte>? extendedData = null)
	{
		ClientCommandId = clientCommandId;
		CommandResponseType = commandResponseType;
		_indexOfExtendedData = indexOfExtendedData;
		_extendedRawData = ((extendedData != null) ? Enumerable.ToArray<byte>((global::System.Collections.Generic.IEnumerable<byte>)extendedData) : null);
	}

	protected MyRvLinkCommandEvent(global::System.Collections.Generic.IReadOnlyList<byte> rawData, MyRvLinkCommandResponseType commandResponseType, int indexOfExtendedData)
	{
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count < MinPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkGatewayInformation)} received less then {MinPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count < MinPayloadLength + MinExtendedDataLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkGatewayInformation)} received less extended data then required bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		if (DecodeEventType(rawData) != EventType)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkCommandResponseSuccess)} because Event Type isn't {EventType}: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		CommandResponseType = commandResponseType;
		if (DecodeCommandResponseType(rawData) != commandResponseType)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {typeof(MyRvLinkCommandResponseSuccess)} because Command Response Type isn't {commandResponseType}: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		ClientCommandId = DecodeClientCommandId(rawData);
		_indexOfExtendedData = indexOfExtendedData;
		_extendedRawData = TryMakeExtendedData(rawData, indexOfExtendedData);
	}

	public global::System.Collections.Generic.IReadOnlyList<byte> GetExtendedData(int offset)
	{
		return GetExtendedData(offset, ExtendedDataLength - offset);
	}

	public global::System.Collections.Generic.IReadOnlyList<byte> GetExtendedData(int offset, int count)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		if (_extendedRawData == null || count == 0)
		{
			return EmptyExtendedData;
		}
		int num = ExtendedDataLength - offset;
		if (num < 0)
		{
			throw new ArgumentException($"offset beyond end of buffer {ExtendedDataLength} {offset} > {num}");
		}
		if (count > num)
		{
			throw new ArgumentException($"size beyond end of buffer {ExtendedDataLength} {count} > {num}");
		}
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_extendedRawData, offset, count);
	}

	protected static byte[]? TryMakeExtendedData(global::System.Collections.Generic.IReadOnlyList<byte> source, int indexOfExtendedData)
	{
		int num = ((global::System.Collections.Generic.IReadOnlyCollection<byte>)source).Count - indexOfExtendedData;
		if (num >= 0)
		{
			return ReadOnlyList.ToNewArray<byte>(source, indexOfExtendedData, num);
		}
		return null;
	}

	public virtual string ToString()
	{
		return $"Command({ClientCommandId}) Success";
	}

	protected static MyRvLinkEventType DecodeEventType(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return (MyRvLinkEventType)decodeBuffer[0];
	}

	protected static ushort DecodeClientCommandId(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return ArrayExtension.GetValueUInt16(decodeBuffer, 1, (Endian)0);
	}

	protected static MyRvLinkCommandResponseType DecodeCommandResponseType(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return (MyRvLinkCommandResponseType)decodeBuffer[3];
	}

	protected virtual void EncodeBaseEventIntoBuffer(byte[] encodeBuffer)
	{
		encodeBuffer[0] = (byte)EventType;
		ArrayExtension.SetValueUInt16(encodeBuffer, ClientCommandId, 1, (Endian)0);
		encodeBuffer[3] = (byte)CommandResponseType;
		if (_extendedRawData != null)
		{
			ReadOnlyList.ToExistingArray<byte>(ExtendedData, 0, encodeBuffer, _indexOfExtendedData, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)ExtendedData).Count);
		}
	}

	public static IMyRvLinkCommandEvent DecodeCommandEvent(global::System.Collections.Generic.IReadOnlyList<byte> rawData, Func<int, IMyRvLinkCommand?> getPendingCommand)
	{
		MyRvLinkCommandResponseType myRvLinkCommandResponseType = DecodeCommandResponseType(rawData);
		if (rawData == null)
		{
			throw new MyRvLinkDecoderException("Decode unknown for null data");
		}
		IMyRvLinkCommandEvent myRvLinkCommandEvent;
		switch (myRvLinkCommandResponseType)
		{
		case MyRvLinkCommandResponseType.SuccessMultipleResponse:
		case MyRvLinkCommandResponseType.SuccessCompleted:
			myRvLinkCommandEvent = new MyRvLinkCommandResponseSuccess(rawData);
			break;
		case MyRvLinkCommandResponseType.FailureMultipleResponse:
		case MyRvLinkCommandResponseType.FailureCompleted:
			myRvLinkCommandEvent = new MyRvLinkCommandResponseFailure(rawData);
			break;
		default:
		{
			MyRvLinkEventType myRvLinkEventType = (MyRvLinkEventType)((((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > 0) ? rawData[0] : 0);
			throw new MyRvLinkDecoderException($"Decode unknown for {myRvLinkEventType}: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		}
		try
		{
			ushort num = DecodeClientCommandId(rawData);
			IMyRvLinkCommand myRvLinkCommand = getPendingCommand.Invoke((int)num);
			IMyRvLinkCommandEvent result = myRvLinkCommand?.DecodeCommandEvent(myRvLinkCommandEvent) ?? myRvLinkCommandEvent;
			if (myRvLinkCommand == null)
			{
				TaggedLog.Information("MyRvLinkCommandEvent", $"Processing Event Received For CommandId 0x{num:X} found pending command NOT FOUND Raw Response Data: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}", global::System.Array.Empty<object>());
			}
			else
			{
				TaggedLog.Information("MyRvLinkCommandEvent", $"Processing Event Received For CommandId 0x{num:X} found pending command {((object)myRvLinkCommand)?.ToString() ?? "NOT FOUND"} Raw Response Data: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}", global::System.Array.Empty<object>());
			}
			return result;
		}
		catch (global::System.Exception)
		{
			TaggedLog.Error("MyRvLinkCommandEvent", $"Error processing event for command {myRvLinkCommandEvent}", global::System.Array.Empty<object>());
			return myRvLinkCommandEvent;
		}
	}
}
