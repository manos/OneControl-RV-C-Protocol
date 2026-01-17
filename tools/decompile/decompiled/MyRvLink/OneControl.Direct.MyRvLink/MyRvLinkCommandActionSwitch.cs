using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandActionSwitch : MyRvLinkCommand
{
	[CompilerGenerated]
	private sealed class _003CGetDeviceIds_003Ed__28 : global::System.Collections.Generic.IEnumerable<byte>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<byte>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private byte _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MyRvLinkCommandActionSwitch _003C_003E4__this;

		private int _003Cindex_003E5__2;

		byte global::System.Collections.Generic.IEnumerator<byte>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object global::System.Collections.IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetDeviceIds_003Ed__28(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void global::System.IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			MyRvLinkCommandActionSwitch myRvLinkCommandActionSwitch = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (myRvLinkCommandActionSwitch.DeviceCount <= 0)
				{
					return false;
				}
				_003Cindex_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Cindex_003E5__2++;
				break;
			}
			if (_003Cindex_003E5__2 < myRvLinkCommandActionSwitch.DeviceCount)
			{
				_003C_003E2__current = myRvLinkCommandActionSwitch._rawData[_003Cindex_003E5__2 + 5];
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool global::System.Collections.IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void global::System.Collections.IEnumerator.Reset()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		global::System.Collections.Generic.IEnumerator<byte> global::System.Collections.Generic.IEnumerable<byte>.GetEnumerator()
		{
			_003CGetDeviceIds_003Ed__28 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetDeviceIds_003Ed__28(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<byte>)this).GetEnumerator();
		}
	}

	private const int DeviceTableIdIndex = 3;

	private const int DeviceStateIndex = 4;

	private const int FirstDeviceIdIndex = 5;

	private readonly byte[] _rawData;

	private readonly HashSet<byte> _successList = new HashSet<byte>();

	private readonly ConcurrentDictionary<byte, MyRvLinkCommandActionSwitchResponseFailure> _failureDict = new ConcurrentDictionary<byte, MyRvLinkCommandActionSwitchResponseFailure>();

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandActionSwitch";

	protected override int MinPayloadLength => 5;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.ActionSwitch;

	public byte DeviceTableId => _rawData[3];

	public int DeviceCount => GetDeviceCount(_rawData);

	public MyRvLinkCommandActionSwitchState SwitchState => MyRvLinkCommandActionSwitchStateExtension.Decode(_rawData[4]);

	public bool IsCommandCompleted => base.ResponseState != MyRvLinkResponseState.Pending;

	private int MaxPayloadLength(int deviceCount)
	{
		return MinPayloadLength + deviceCount;
	}

	private static int GetDeviceCount(global::System.Collections.Generic.IReadOnlyList<byte> data)
	{
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)data).Count > 5)
		{
			return ((global::System.Collections.Generic.IReadOnlyCollection<byte>)data).Count - 5;
		}
		return 0;
	}

	public MyRvLinkCommandActionSwitch(ushort clientCommandId, byte deviceTableId, MyRvLinkCommandActionSwitchState switchState, params byte[] switchDeviceIdList)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		int num = switchDeviceIdList.Length;
		if (num > 255)
		{
			throw new ArgumentOutOfRangeException("switchDeviceIdList", "Too Many Switches Specified");
		}
		if (num == 0)
		{
			throw new ArgumentOutOfRangeException("switchDeviceIdList", "Must specify at least 1 device");
		}
		_rawData = new byte[MaxPayloadLength(num)];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = switchState.Encode();
		int num2 = 5;
		foreach (byte b in switchDeviceIdList)
		{
			_rawData[num2] = b;
			num2++;
		}
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandActionSwitch(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		int deviceCount = GetDeviceCount(rawData);
		if (deviceCount <= 0)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} must contain at least 1 device bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		int num = MaxPayloadLength(deviceCount);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > num)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {num} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	[IteratorStateMachine(typeof(_003CGetDeviceIds_003Ed__28))]
	public global::System.Collections.Generic.IEnumerable<byte> GetDeviceIds()
	{
		if (DeviceCount > 0)
		{
			for (int index = 0; index < DeviceCount; index++)
			{
				yield return _rawData[index + 5];
			}
		}
	}

	public global::System.Collections.Generic.IEnumerable<byte> GetSuccessDeviceIds()
	{
		return Enumerable.AsEnumerable<byte>((global::System.Collections.Generic.IEnumerable<byte>)_successList);
	}

	public global::System.Collections.Generic.IEnumerable<MyRvLinkCommandActionSwitchResponseFailure> GetFailedDeviceIds2()
	{
		return Enumerable.AsEnumerable<MyRvLinkCommandActionSwitchResponseFailure>((global::System.Collections.Generic.IEnumerable<MyRvLinkCommandActionSwitchResponseFailure>)_failureDict.Values);
	}

	public static MyRvLinkCommandActionSwitch Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandActionSwitch(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public override bool ProcessResponse(IMyRvLinkCommandResponse commandResponse)
	{
		if (base.ResponseState != MyRvLinkResponseState.Pending)
		{
			TaggedLog.Debug(LogTag, $"Ignoring Process Command Response because command is {base.ResponseState}: {commandResponse}", global::System.Array.Empty<object>());
			return true;
		}
		if (!(commandResponse is MyRvLinkCommandActionSwitchResponseSuccess myRvLinkCommandActionSwitchResponseSuccess))
		{
			if (!(commandResponse is MyRvLinkCommandActionSwitchResponseFailure myRvLinkCommandActionSwitchResponseFailure))
			{
				if (!(commandResponse is MyRvLinkCommandResponseSuccess myRvLinkCommandResponseSuccess))
				{
					if (commandResponse is MyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
					{
						TaggedLog.Warning(LogTag, $"Command failed (BUT UNEXPECTED TYPE) {myRvLinkCommandResponseFailure}", global::System.Array.Empty<object>());
						if (myRvLinkCommandResponseFailure.IsCommandCompleted)
						{
							base.ResponseState = MyRvLinkResponseState.Failed;
						}
					}
					else
					{
						TaggedLog.Debug(LogTag, $"Unexpected response received {commandResponse}", global::System.Array.Empty<object>());
						if (commandResponse.IsCommandCompleted)
						{
							base.ResponseState = MyRvLinkResponseState.Failed;
						}
					}
				}
				else
				{
					TaggedLog.Warning(LogTag, $"Command success (BUT UNEXPECTED TYPE) {myRvLinkCommandResponseSuccess}", global::System.Array.Empty<object>());
					if (myRvLinkCommandResponseSuccess.IsCommandCompleted)
					{
						base.ResponseState = MyRvLinkResponseState.Completed;
					}
				}
			}
			else
			{
				try
				{
					byte? deviceId = myRvLinkCommandActionSwitchResponseFailure.DeviceId;
					if (deviceId.HasValue)
					{
						if (_successList.Contains(deviceId.Value))
						{
							throw new MyRvLinkException($"Device Id 0x{deviceId:X2} was previously reported as successful");
						}
						MyRvLinkCommandActionSwitchResponseFailure myRvLinkCommandActionSwitchResponseFailure2 = default(MyRvLinkCommandActionSwitchResponseFailure);
						if (_failureDict.TryGetValue(deviceId.Value, ref myRvLinkCommandActionSwitchResponseFailure2))
						{
							throw new MyRvLinkException($"Device Id 0x{deviceId:X2} was previously reported with a failure ({myRvLinkCommandActionSwitchResponseFailure2})");
						}
						_failureDict[deviceId.Value] = myRvLinkCommandActionSwitchResponseFailure;
					}
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Debug(LogTag, $"Invalid Failure response {myRvLinkCommandActionSwitchResponseFailure}: {ex.Message}", global::System.Array.Empty<object>());
				}
				if (myRvLinkCommandActionSwitchResponseFailure.IsCommandCompleted)
				{
					base.ResponseState = MyRvLinkResponseState.Failed;
				}
			}
		}
		else
		{
			try
			{
				if (!myRvLinkCommandActionSwitchResponseSuccess.HasDeviceId)
				{
					throw new MyRvLinkException("Missing Device Id");
				}
				byte deviceId2 = myRvLinkCommandActionSwitchResponseSuccess.DeviceId;
				if (_successList.Contains(deviceId2))
				{
					throw new MyRvLinkException($"Device Id 0x{deviceId2:X2} was previously reported as successful");
				}
				MyRvLinkCommandActionSwitchResponseFailure myRvLinkCommandActionSwitchResponseFailure3 = default(MyRvLinkCommandActionSwitchResponseFailure);
				if (_failureDict.TryGetValue(deviceId2, ref myRvLinkCommandActionSwitchResponseFailure3))
				{
					throw new MyRvLinkException($"Device Id was 0x{deviceId2:X2}  previously reported with a failure {myRvLinkCommandActionSwitchResponseFailure3}");
				}
				_successList.Add(deviceId2);
			}
			catch (global::System.Exception ex2)
			{
				TaggedLog.Debug(LogTag, $"Invalid Success response {myRvLinkCommandActionSwitchResponseSuccess}: {ex2.Message}", global::System.Array.Empty<object>());
			}
			if (myRvLinkCommandActionSwitchResponseSuccess.IsCommandCompleted)
			{
				base.ResponseState = MyRvLinkResponseState.Completed;
			}
		}
		if (commandResponse.IsCommandCompleted && !DidGetResponseForAllDevices())
		{
			TaggedLog.Warning(LogTag, "Command completed, but didn't receive responses from all devices\n" + ((object)this).ToString(), global::System.Array.Empty<object>());
		}
		return base.ResponseState != MyRvLinkResponseState.Pending;
	}

	public bool DidGetResponseForAllDevices()
	{
		if (!IsCommandCompleted)
		{
			return false;
		}
		return _successList.Count + _failureDict.Count == DeviceCount;
	}

	public override IMyRvLinkCommandEvent DecodeCommandEvent(IMyRvLinkCommandEvent commandEvent)
	{
		if (!(commandEvent is MyRvLinkCommandResponseSuccess response))
		{
			if (commandEvent is MyRvLinkCommandResponseFailure response2)
			{
				return new MyRvLinkCommandActionSwitchResponseFailure(response2);
			}
			TaggedLog.Warning(LogTag, $"DecodeCommandEvent unexpected event type for {commandEvent}", global::System.Array.Empty<object>());
			return commandEvent;
		}
		return new MyRvLinkCommandActionSwitchResponseSuccess(response);
	}

	public virtual string ToString()
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		StringBuilder val = new StringBuilder($"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, Device Count: {DeviceCount}, Set State: {EnumExtensions.DebugDumpAsFlags<MyRvLinkCommandActionSwitchState>(SwitchState)}]: ");
		if (DeviceCount <= 0)
		{
			val.Append("No Devices");
		}
		else
		{
			byte[] array = Enumerable.ToArray<byte>(GetDeviceIds());
			AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
			MyRvLinkCommandActionSwitchResponseFailure myRvLinkCommandActionSwitchResponseFailure = default(MyRvLinkCommandActionSwitchResponseFailure);
			foreach (byte b in array)
			{
				if (!IsCommandCompleted)
				{
					StringBuilder val2 = val;
					StringBuilder obj = val2;
					((AppendInterpolatedStringHandler)(ref val3))._002Ector(30, 2, val2);
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    Device ID 0x");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>(b, "X2");
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Not Completed");
					obj.Append(ref val3);
				}
				else if (_successList.Contains(b))
				{
					StringBuilder val2 = val;
					StringBuilder obj2 = val2;
					((AppendInterpolatedStringHandler)(ref val3))._002Ector(24, 2, val2);
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    Device ID 0x");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>(b, "X2");
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Success");
					obj2.Append(ref val3);
				}
				else if (_failureDict.TryGetValue(b, ref myRvLinkCommandActionSwitchResponseFailure))
				{
					StringBuilder val2 = val;
					StringBuilder obj3 = val2;
					((AppendInterpolatedStringHandler)(ref val3))._002Ector(17, 3, val2);
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    Device ID 0x");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>(b, "X2");
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" ");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<MyRvLinkCommandActionSwitchResponseFailure>(myRvLinkCommandActionSwitchResponseFailure);
					obj3.Append(ref val3);
				}
				else
				{
					StringBuilder val2 = val;
					StringBuilder obj4 = val2;
					((AppendInterpolatedStringHandler)(ref val3))._002Ector(67, 2, val2);
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    Device ID 0x");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>(b, "X2");
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Completed but didn't report success/failure status");
					obj4.Append(ref val3);
				}
			}
			if (IsCommandCompleted && !DidGetResponseForAllDevices())
			{
				StringBuilder val2 = val;
				StringBuilder obj5 = val2;
				((AppendInterpolatedStringHandler)(ref val3))._002Ector(44, 1, val2);
				((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
				((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    DIDN'T RECEIVE RESPONSES FOR ALL DEVICES");
				obj5.Append(ref val3);
			}
		}
		return ((object)val).ToString();
	}
}
