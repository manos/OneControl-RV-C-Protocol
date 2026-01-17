using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetDevicePidList : MyRvLinkCommand
{
	private const int MaxPayloadLength = 9;

	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int StartPidIndex = 5;

	private const int EndPidIndex = 7;

	private readonly byte[] _rawData;

	private readonly Dictionary<Pid, PidAccess> _pidDict = new Dictionary<Pid, PidAccess>();

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandGetDevicePidList";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.GetDevicePidList;

	protected override int MinPayloadLength => 9;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public Pid StartPidId => (Pid)ArrayExtension.GetValueUInt16(_rawData, 5, (Endian)0);

	public Pid EndPidId => (Pid)ArrayExtension.GetValueUInt16(_rawData, 7, (Endian)0);

	[field: CompilerGenerated]
	public bool IsDeviceLoadingCompleted
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public Dictionary<Pid, PidAccess> PidDict
	{
		get
		{
			if (!IsDeviceLoadingCompleted)
			{
				return new Dictionary<Pid, PidAccess>();
			}
			return _pidDict;
		}
	}

	public MyRvLinkCommandGetDevicePidList(ushort clientCommandId, byte deviceTableId, byte deviceId, Pid startPidId, Pid endPidId = (Pid)0)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected I4, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected I4, but got Unknown
		_rawData = new byte[9];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		ArrayExtension.SetValueUInt16(_rawData, (ushort)(int)startPidId, 5, (Endian)0);
		ArrayExtension.SetValueUInt16(_rawData, (ushort)(int)endPidId, 7, (Endian)0);
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandGetDevicePidList(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public override bool ProcessResponse(IMyRvLinkCommandResponse commandResponse)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		if (base.ResponseState != MyRvLinkResponseState.Pending)
		{
			TaggedLog.Debug(LogTag, $"Ignoring Process Command Response because command is {base.ResponseState}: {commandResponse}", global::System.Array.Empty<object>());
			return true;
		}
		if (!(commandResponse is MyRvLinkCommandGetDevicePidListResponse myRvLinkCommandGetDevicePidListResponse))
		{
			if (!(commandResponse is MyRvLinkCommandGetDevicePidListResponseCompleted myRvLinkCommandGetDevicePidListResponseCompleted))
			{
				if (commandResponse is MyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
				{
					TaggedLog.Debug(LogTag, $"Command failed {myRvLinkCommandResponseFailure}", global::System.Array.Empty<object>());
					IsDeviceLoadingCompleted = false;
					base.ResponseState = MyRvLinkResponseState.Failed;
				}
				else
				{
					TaggedLog.Debug(LogTag, $"Unexpected response received {commandResponse}", global::System.Array.Empty<object>());
					IsDeviceLoadingCompleted = false;
					base.ResponseState = MyRvLinkResponseState.Failed;
				}
			}
			else
			{
				try
				{
					if (myRvLinkCommandGetDevicePidListResponseCompleted.PidCount != _pidDict.Count)
					{
						TaggedLog.Debug(LogTag, $"Command failed didn't receive expected number of DTCs. Response received {_pidDict.Count} PIDs and expected {myRvLinkCommandGetDevicePidListResponseCompleted.PidCount}", global::System.Array.Empty<object>());
						_pidDict.Clear();
						IsDeviceLoadingCompleted = false;
						base.ResponseState = MyRvLinkResponseState.Failed;
					}
					else
					{
						IsDeviceLoadingCompleted = true;
						base.ResponseState = MyRvLinkResponseState.Completed;
					}
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Debug(LogTag, "Command completed, ALL PIDs were not received properly: " + ex.Message, global::System.Array.Empty<object>());
					base.ResponseState = MyRvLinkResponseState.Failed;
				}
			}
		}
		else
		{
			try
			{
				global::System.Collections.Generic.IEnumerator<KeyValuePair<Pid, PidAccess>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<Pid, PidAccess>>)myRvLinkCommandGetDevicePidListResponse.PidDict).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
					{
						KeyValuePair<Pid, PidAccess> current = enumerator.Current;
						if (_pidDict.ContainsKey(current.Key))
						{
							TaggedLog.Debug(LogTag, $"IGNORING: Duplicate PID {current.Key}, value already returned in another response", global::System.Array.Empty<object>());
						}
						else
						{
							_pidDict.Add(current.Key, current.Value);
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator)?.Dispose();
				}
			}
			catch (global::System.Exception ex2)
			{
				TaggedLog.Debug(LogTag, "Command completed, ALL PIDs were not received properly: " + ex2.Message, global::System.Array.Empty<object>());
				base.ResponseState = MyRvLinkResponseState.Failed;
			}
		}
		return base.ResponseState != MyRvLinkResponseState.Pending;
	}

	public static MyRvLinkCommandGetDevicePidList Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandGetDevicePidList(rawData);
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
				return new MyRvLinkCommandGetDevicePidListResponseCompleted(myRvLinkCommandResponseSuccess);
			}
			return new MyRvLinkCommandGetDevicePidListResponse(myRvLinkCommandResponseSuccess);
		}
		return commandEvent;
	}

	public virtual string ToString()
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder val = new StringBuilder($"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, Start Device Id: 0x{DeviceId:X2}, Start: {StartPidId}, End: {EndPidId} ]: {ArrayExtension.DebugDump(_rawData, " ", false)}");
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		if (IsDeviceLoadingCompleted)
		{
			StringBuilder val2 = val;
			StringBuilder obj = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(23, 2, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    Received PID Count ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<int>(PidDict.Count);
			obj.Append(ref val3);
			try
			{
				Enumerator<Pid, PidAccess> enumerator = PidDict.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<Pid, PidAccess> current = enumerator.Current;
						val2 = val;
						StringBuilder obj2 = val2;
						val3 = new AppendInterpolatedStringHandler(6, 3, val2);
						((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
						((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    ");
						((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<Pid>(current.Key);
						((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(": ");
						((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(EnumExtensions.DebugDumpAsFlags<PidAccess>(current.Value));
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
				((AppendInterpolatedStringHandler)(ref val3))._002Ector(28, 2, val2);
				((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
				((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    ERROR Trying to Get PID ");
				((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
				obj3.Append(ref val3);
			}
		}
		else if (base.ResponseState != MyRvLinkResponseState.Pending)
		{
			StringBuilder val2 = val;
			StringBuilder obj4 = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(40, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    --- PID List Not Valid/Complete --- ");
			obj4.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
