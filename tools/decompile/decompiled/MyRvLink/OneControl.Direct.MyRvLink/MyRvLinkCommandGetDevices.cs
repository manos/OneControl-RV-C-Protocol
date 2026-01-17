using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetDevices : MyRvLinkCommand
{
	private const int MaxPayloadLength = 6;

	private const int DeviceTableIdIndex = 3;

	private const int StartDeviceIdIndex = 4;

	private const int MaxDeviceRequestCountIndex = 5;

	private readonly byte[] _rawData;

	private readonly List<IMyRvLinkDevice> _devices = new List<IMyRvLinkDevice>();

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandGetDevices";

	protected override int MinPayloadLength => 6;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.GetDevices;

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
	public uint ResponseReceivedDeviceTableCrc
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public List<IMyRvLinkDevice> Devices
	{
		get
		{
			if (!IsDeviceLoadingComplete(checkForCommandCompleted: true))
			{
				return new List<IMyRvLinkDevice>();
			}
			return _devices;
		}
	}

	public MyRvLinkCommandGetDevices(ushort clientCommandId, byte deviceTableId, byte startDeviceId, int maxDeviceRequestCount)
	{
		_rawData = new byte[6];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = startDeviceId;
		_rawData[5] = (byte)maxDeviceRequestCount;
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandGetDevices(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	protected bool IsDeviceLoadingComplete(bool checkForCommandCompleted)
	{
		if (checkForCommandCompleted && base.ResponseState != MyRvLinkResponseState.Completed)
		{
			return false;
		}
		if (_devices.Count > MaxDeviceRequestCount)
		{
			return false;
		}
		if (ResponseReceivedCount != _devices.Count)
		{
			return false;
		}
		if (_devices.Count <= 0)
		{
			return false;
		}
		if (StartDeviceId == 0 && !(Enumerable.First<IMyRvLinkDevice>((global::System.Collections.Generic.IEnumerable<IMyRvLinkDevice>)_devices) is MyRvLinkDeviceHost))
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
		if (!(commandResponse is MyRvLinkCommandGetDevicesResponse myRvLinkCommandGetDevicesResponse))
		{
			if (!(commandResponse is MyRvLinkCommandGetDevicesResponseCompleted myRvLinkCommandGetDevicesResponseCompleted))
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
					ResponseReceivedCount = myRvLinkCommandGetDevicesResponseCompleted.DeviceCount;
					ResponseReceivedDeviceTableCrc = myRvLinkCommandGetDevicesResponseCompleted.DeviceTableCrc;
					if (IsDeviceLoadingComplete(checkForCommandCompleted: false))
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
				global::System.Collections.Generic.IEnumerator<IMyRvLinkDevice> enumerator = ((global::System.Collections.Generic.IEnumerable<IMyRvLinkDevice>)myRvLinkCommandGetDevicesResponse.Devices).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
					{
						IMyRvLinkDevice current = enumerator.Current;
						_devices.Add(current);
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

	public static MyRvLinkCommandGetDevices Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandGetDevices(rawData);
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
				return new MyRvLinkCommandGetDevicesResponseCompleted(myRvLinkCommandResponseSuccess);
			}
			return new MyRvLinkCommandGetDevicesResponse(myRvLinkCommandResponseSuccess);
		}
		return commandEvent;
	}

	public virtual string ToString()
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder val = new StringBuilder($"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, Start Device Id: 0x{StartDeviceId:X2} Max Request Device Count: {MaxDeviceRequestCount}]: {ArrayExtension.DebugDump(_rawData, " ", false)}");
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		if (IsDeviceLoadingComplete(checkForCommandCompleted: true))
		{
			StringBuilder val2 = val;
			StringBuilder obj = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(41, 3, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    Received Device Count ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<int>(ResponseReceivedCount);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("  Device CRC 0x");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<uint>(ResponseReceivedDeviceTableCrc, "X4");
			obj.Append(ref val3);
			try
			{
				int startDeviceId = StartDeviceId;
				Enumerator<IMyRvLinkDevice> enumerator = Devices.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IMyRvLinkDevice current = enumerator.Current;
						val2 = val;
						StringBuilder obj2 = val2;
						val3 = new AppendInterpolatedStringHandler(8, 3, val2);
						((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
						((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    0x");
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
				((AppendInterpolatedStringHandler)(ref val3))._002Ector(31, 2, val2);
				((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
				((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    ERROR Trying to Get Device ");
				((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
				obj3.Append(ref val3);
			}
		}
		else if (base.ResponseState != MyRvLinkResponseState.Pending)
		{
			StringBuilder val2 = val;
			StringBuilder obj4 = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(43, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    --- Device List Not Valid/Complete --- ");
			obj4.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
