using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetDevicesMetadata : MyRvLinkCommand
{
	private const int MaxPayloadLength = 6;

	private const int DeviceTableIdIndex = 3;

	private const int StartDeviceIdIndex = 4;

	private const int MaxDeviceRequestCountIndex = 5;

	private readonly byte[] _rawData;

	private readonly List<IMyRvLinkDeviceMetadata> _devicesMetadata = new List<IMyRvLinkDeviceMetadata>();

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandGetDevicesMetadata";

	protected override int MinPayloadLength => 6;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.GetDevicesMetadata;

	public byte DeviceTableId => DecodeDeviceTableId(_rawData);

	public byte StartDeviceId => DecodeStartDeviceId(_rawData);

	public int MaxDeviceRequestCount => DecodeMaxDeviceRequestCount(_rawData);

	[field: CompilerGenerated]
	public int ResponseReceivedCount
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	} = -1;

	[field: CompilerGenerated]
	public uint ResponseReceivedMetadataTableCrc
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public List<IMyRvLinkDeviceMetadata> DevicesMetadata
	{
		get
		{
			if (!IsMetadataLoadingComplete(checkForCommandCompleted: true))
			{
				return new List<IMyRvLinkDeviceMetadata>();
			}
			return _devicesMetadata;
		}
	}

	public MyRvLinkCommandGetDevicesMetadata(ushort clientCommandId, byte deviceTableId, byte startDeviceId, int maxDeviceRequestCount)
	{
		_rawData = new byte[6];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = startDeviceId;
		_rawData[5] = (byte)maxDeviceRequestCount;
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandGetDevicesMetadata(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	protected bool IsMetadataLoadingComplete(bool checkForCommandCompleted)
	{
		if (checkForCommandCompleted && base.ResponseState != MyRvLinkResponseState.Completed)
		{
			return false;
		}
		if (_devicesMetadata.Count > MaxDeviceRequestCount)
		{
			return false;
		}
		if (ResponseReceivedCount != _devicesMetadata.Count)
		{
			return false;
		}
		if (_devicesMetadata.Count <= 0)
		{
			return false;
		}
		if (StartDeviceId == 0 && !(Enumerable.First<IMyRvLinkDeviceMetadata>((global::System.Collections.Generic.IEnumerable<IMyRvLinkDeviceMetadata>)_devicesMetadata) is MyRvLinkDeviceHostMetadata))
		{
			return false;
		}
		return true;
	}

	public override bool ProcessResponse(IMyRvLinkCommandResponse commandResponse)
	{
		if (base.ResponseState != MyRvLinkResponseState.Pending)
		{
			TaggedLog.Debug(LogTag, $"Ignoring Process Command Response because command is {base.ResponseState}: {commandResponse}", global::System.Array.Empty<object>());
			return true;
		}
		if (!(commandResponse is MyRvLinkCommandGetDevicesMetadataResponse myRvLinkCommandGetDevicesMetadataResponse))
		{
			if (!(commandResponse is MyRvLinkCommandGetDevicesMetadataResponseCompleted myRvLinkCommandGetDevicesMetadataResponseCompleted))
			{
				if (commandResponse is MyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
				{
					TaggedLog.Debug(LogTag, $"Command failed {myRvLinkCommandResponseFailure}", global::System.Array.Empty<object>());
					base.ResponseState = MyRvLinkResponseState.Failed;
				}
				else
				{
					TaggedLog.Debug(LogTag, $"Unexpected response received {commandResponse}", global::System.Array.Empty<object>());
					base.ResponseState = MyRvLinkResponseState.Failed;
				}
			}
			else
			{
				try
				{
					ResponseReceivedCount = myRvLinkCommandGetDevicesMetadataResponseCompleted.DeviceCount;
					ResponseReceivedMetadataTableCrc = myRvLinkCommandGetDevicesMetadataResponseCompleted.DeviceMetadataTableCrc;
					if (IsMetadataLoadingComplete(checkForCommandCompleted: false))
					{
						base.ResponseState = MyRvLinkResponseState.Completed;
					}
					else
					{
						TaggedLog.Debug(LogTag, "Command completed, ALL devices were not received properly", global::System.Array.Empty<object>());
						base.ResponseState = MyRvLinkResponseState.Failed;
					}
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Debug(LogTag, "Unexpected response received, expected MyRvLinkCommandGetDevicesResponseCompleted: " + ex.Message, global::System.Array.Empty<object>());
					base.ResponseState = MyRvLinkResponseState.Failed;
				}
			}
		}
		else
		{
			try
			{
				global::System.Collections.Generic.IEnumerator<IMyRvLinkDeviceMetadata> enumerator = ((global::System.Collections.Generic.IEnumerable<IMyRvLinkDeviceMetadata>)myRvLinkCommandGetDevicesMetadataResponse.DevicesMetadata).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
					{
						IMyRvLinkDeviceMetadata current = enumerator.Current;
						_devicesMetadata.Add(current);
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator)?.Dispose();
				}
			}
			catch (global::System.Exception ex2)
			{
				TaggedLog.Debug(LogTag, "Unable to decode response " + ex2.Message, global::System.Array.Empty<object>());
				base.ResponseState = MyRvLinkResponseState.Failed;
			}
		}
		return base.ResponseState != MyRvLinkResponseState.Pending;
	}

	protected static byte DecodeDeviceTableId(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return decodeBuffer[3];
	}

	protected static byte DecodeStartDeviceId(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return decodeBuffer[4];
	}

	protected static byte DecodeMaxDeviceRequestCount(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return decodeBuffer[5];
	}

	public static MyRvLinkCommandGetDevicesMetadata Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandGetDevicesMetadata(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public override IMyRvLinkCommandEvent DecodeCommandEvent(IMyRvLinkCommandEvent commandEvent)
	{
		if (commandEvent is MyRvLinkCommandResponseSuccess myRvLinkCommandResponseSuccess)
		{
			if (myRvLinkCommandResponseSuccess.IsCommandCompleted)
			{
				return new MyRvLinkCommandGetDevicesMetadataResponseCompleted(myRvLinkCommandResponseSuccess);
			}
			return new MyRvLinkCommandGetDevicesMetadataResponse(myRvLinkCommandResponseSuccess);
		}
		return commandEvent;
	}

	public virtual string ToString()
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder val = new StringBuilder($"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, Start Device Id: 0x{StartDeviceId:X2} Max Request Device Count: {MaxDeviceRequestCount}]: {ArrayExtension.DebugDump(_rawData, " ", false)}");
		if (IsMetadataLoadingComplete(checkForCommandCompleted: true))
		{
			StringBuilder val2 = val;
			StringBuilder obj = val2;
			AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(51, 2, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("\n    Received Device Metadata Count ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<int>(ResponseReceivedCount);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("  Device CRC 0x");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<uint>(ResponseReceivedMetadataTableCrc, "X8");
			obj.Append(ref val3);
			try
			{
				int startDeviceId = StartDeviceId;
				Enumerator<IMyRvLinkDeviceMetadata> enumerator = DevicesMetadata.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IMyRvLinkDeviceMetadata current = enumerator.Current;
						val2 = val;
						StringBuilder obj2 = val2;
						val3 = new AppendInterpolatedStringHandler(9, 2, val2);
						((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("\n    0x");
						((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<int>(startDeviceId++, "X2");
						((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(": ");
						((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(((object)current).ToString());
						obj2.Append(ref val3);
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
			}
			catch (global::System.Exception ex)
			{
				val2 = val;
				StringBuilder obj3 = val2;
				((AppendInterpolatedStringHandler)(ref val3))._002Ector(42, 1, val2);
				((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("\n    ERROR Trying to Get Device Metadata: ");
				((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
				obj3.Append(ref val3);
			}
		}
		else if (base.ResponseState != MyRvLinkResponseState.Pending)
		{
			val.Append("\n    --- Device Metadata List Not Valid/Complete --- ");
		}
		return ((object)val).ToString();
	}
}
