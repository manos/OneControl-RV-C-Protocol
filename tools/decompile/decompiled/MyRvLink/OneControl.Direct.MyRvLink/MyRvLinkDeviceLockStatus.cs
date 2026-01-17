using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.Common.Extensions;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceLockStatus : MyRvLinkEventDevicesSubByte<MyRvLinkDeviceLockStatus>
{
	[Flags]
	private enum MyRvLinkDeviceChassisStatus : byte
	{
		None = 0,
		ParkBreakReleased = 1,
		ParkBreakEngaged = 2,
		IgnitionOn = 4,
		IgnitionOff = 8,
		ParkingBreakMask = 3,
		IgnitionPowerMask = 0xC
	}

	[CompilerGenerated]
	private sealed class _003CEnumerateIsDeviceLocked_003Ed__33 : global::System.Collections.Generic.IEnumerable<ValueTuple<byte, bool>>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ValueTuple<byte, bool>>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private ValueTuple<byte, bool> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MyRvLinkDeviceLockStatus _003C_003E4__this;

		private int _003CendDeviceId_003E5__2;

		private byte _003CdeviceId_003E5__3;

		ValueTuple<byte, bool> global::System.Collections.Generic.IEnumerator<ValueTuple<byte, bool>>.Current
		{
			[DebuggerHidden]
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _003C_003E2__current;
			}
		}

		object global::System.Collections.IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CEnumerateIsDeviceLocked_003Ed__33(int _003C_003E1__state)
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
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			MyRvLinkDeviceLockStatus myRvLinkDeviceLockStatus = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CendDeviceId_003E5__2 = myRvLinkDeviceLockStatus.StartDeviceId + myRvLinkDeviceLockStatus.DeviceCount;
				_003CdeviceId_003E5__3 = myRvLinkDeviceLockStatus.StartDeviceId;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003CdeviceId_003E5__3++;
				break;
			}
			if (_003CdeviceId_003E5__3 < _003CendDeviceId_003E5__2)
			{
				_003C_003E2__current = new ValueTuple<byte, bool>(_003CdeviceId_003E5__3, myRvLinkDeviceLockStatus.IsDeviceLocked(_003CdeviceId_003E5__3));
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
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, bool>> global::System.Collections.Generic.IEnumerable<ValueTuple<byte, bool>>.GetEnumerator()
		{
			_003CEnumerateIsDeviceLocked_003Ed__33 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEnumerateIsDeviceLocked_003Ed__33(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<ValueTuple<byte, bool>>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CEnumerateIsDeviceLockedDiff_003Ed__34 : global::System.Collections.Generic.IEnumerable<ValueTuple<byte, bool>>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ValueTuple<byte, bool>>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private ValueTuple<byte, bool> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private MyRvLinkDeviceLockStatus otherDeviceStatus;

		public MyRvLinkDeviceLockStatus _003C_003E3__otherDeviceStatus;

		public MyRvLinkDeviceLockStatus _003C_003E4__this;

		private int _003CendDeviceId_003E5__2;

		private byte _003CdeviceId_003E5__3;

		ValueTuple<byte, bool> global::System.Collections.Generic.IEnumerator<ValueTuple<byte, bool>>.Current
		{
			[DebuggerHidden]
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _003C_003E2__current;
			}
		}

		object global::System.Collections.IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CEnumerateIsDeviceLockedDiff_003Ed__34(int _003C_003E1__state)
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
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			MyRvLinkDeviceLockStatus myRvLinkDeviceLockStatus = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00cd;
			}
			_003C_003E1__state = -1;
			if (otherDeviceStatus != null && (myRvLinkDeviceLockStatus.DeviceTableId != otherDeviceStatus.DeviceTableId || myRvLinkDeviceLockStatus.DeviceCount != otherDeviceStatus.DeviceCount))
			{
				otherDeviceStatus = null;
			}
			_003CendDeviceId_003E5__2 = myRvLinkDeviceLockStatus.StartDeviceId + myRvLinkDeviceLockStatus.DeviceCount;
			_003CdeviceId_003E5__3 = myRvLinkDeviceLockStatus.StartDeviceId;
			goto IL_00dc;
			IL_00dc:
			if (_003CdeviceId_003E5__3 < _003CendDeviceId_003E5__2)
			{
				bool flag = myRvLinkDeviceLockStatus.IsDeviceLocked(_003CdeviceId_003E5__3);
				if (otherDeviceStatus == null || flag != otherDeviceStatus.IsDeviceLocked(_003CdeviceId_003E5__3))
				{
					_003C_003E2__current = new ValueTuple<byte, bool>(_003CdeviceId_003E5__3, myRvLinkDeviceLockStatus.IsDeviceLocked(_003CdeviceId_003E5__3));
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00cd;
			}
			return false;
			IL_00cd:
			_003CdeviceId_003E5__3++;
			goto IL_00dc;
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
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, bool>> global::System.Collections.Generic.IEnumerable<ValueTuple<byte, bool>>.GetEnumerator()
		{
			_003CEnumerateIsDeviceLockedDiff_003Ed__34 _003CEnumerateIsDeviceLockedDiff_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CEnumerateIsDeviceLockedDiff_003Ed__ = this;
			}
			else
			{
				_003CEnumerateIsDeviceLockedDiff_003Ed__ = new _003CEnumerateIsDeviceLockedDiff_003Ed__34(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CEnumerateIsDeviceLockedDiff_003Ed__.otherDeviceStatus = _003C_003E3__otherDeviceStatus;
			return _003CEnumerateIsDeviceLockedDiff_003Ed__;
		}

		[DebuggerHidden]
		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<ValueTuple<byte, bool>>)this).GetEnumerator();
		}
	}

	private const int SystemLockoutLevelIndex = 1;

	private const int ChassisInfoIndex = 2;

	private const int TowableInfoIndex = 3;

	private const int TowableBatteryVoltageIndex = 4;

	private const int TowableBrakeVoltageIndex = 5;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.DeviceLockStatus;

	protected override AllowedDevicesPerByte DevicesPerByte => AllowedDevicesPerByte.Eight;

	protected override int MinPayloadLength => 8;

	[field: CompilerGenerated]
	public byte StartDeviceId
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public LogicalDeviceChassisInfoStatus ChassisInfoStatus
	{
		[CompilerGenerated]
		get;
	}

	protected override int DeviceTableIdIndex => 6;

	protected override int DeviceCountIndex => 7;

	protected override int DeviceStatusStartIndex => 8;

	public byte SystemLockoutLevel => _rawData[1];

	public MyRvLinkIgnitionStatus IgnitionStatus
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected I4, but got Unknown
			IgnitionPowerSignal ignitionPowerSignal = ChassisInfoStatus.IgnitionPowerSignal;
			return (int)ignitionPowerSignal switch
			{
				1 => MyRvLinkIgnitionStatus.On, 
				2 => MyRvLinkIgnitionStatus.Off, 
				0 => MyRvLinkIgnitionStatus.Unknown, 
				3 => MyRvLinkIgnitionStatus.Unknown, 
				_ => MyRvLinkIgnitionStatus.Unknown, 
			};
		}
	}

	public MyRvLinkParkingBreakStatus ParkingBreakStatus
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected I4, but got Unknown
			ParkBrake parkBreak = ChassisInfoStatus.ParkBreak;
			return (int)parkBreak switch
			{
				2 => MyRvLinkParkingBreakStatus.Engaged, 
				1 => MyRvLinkParkingBreakStatus.Released, 
				0 => MyRvLinkParkingBreakStatus.Unknown, 
				3 => MyRvLinkParkingBreakStatus.Unknown, 
				_ => MyRvLinkParkingBreakStatus.Unknown, 
			};
		}
	}

	public MyRvLinkDeviceLockStatus(byte deviceTableId, byte deviceCount, byte lockoutLevel, MyRvLinkParkingBreakStatus parkingBreakStatus, MyRvLinkIgnitionStatus ignitionStatus)
		: base(deviceTableId, (int)deviceCount)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		_rawData[1] = lockoutLevel;
		MyRvLinkDeviceChassisStatus myRvLinkDeviceChassisStatus = MyRvLinkDeviceChassisStatus.None;
		switch (parkingBreakStatus)
		{
		case MyRvLinkParkingBreakStatus.Engaged:
			myRvLinkDeviceChassisStatus |= MyRvLinkDeviceChassisStatus.ParkBreakEngaged;
			break;
		case MyRvLinkParkingBreakStatus.Released:
			myRvLinkDeviceChassisStatus |= MyRvLinkDeviceChassisStatus.ParkBreakReleased;
			break;
		}
		switch (ignitionStatus)
		{
		case MyRvLinkIgnitionStatus.On:
			myRvLinkDeviceChassisStatus |= MyRvLinkDeviceChassisStatus.IgnitionOn;
			break;
		case MyRvLinkIgnitionStatus.Off:
			myRvLinkDeviceChassisStatus |= MyRvLinkDeviceChassisStatus.IgnitionOff;
			break;
		}
		_rawData[2] = (byte)myRvLinkDeviceChassisStatus;
		_rawData[3] = 0;
		_rawData[4] = 255;
		_rawData[5] = 255;
		ChassisInfoStatus = new LogicalDeviceChassisInfoStatus(_rawData[2], _rawData[3], _rawData[4], _rawData[5]);
	}

	protected MyRvLinkDeviceLockStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base((global::System.Collections.Generic.IReadOnlyList<byte>)ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count))
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		ChassisInfoStatus = new LogicalDeviceChassisInfoStatus(rawData[2], rawData[3], rawData[4], rawData[5]);
	}

	public static MyRvLinkDeviceLockStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkDeviceLockStatus(rawData);
	}

	[IteratorStateMachine(typeof(_003CEnumerateIsDeviceLocked_003Ed__33))]
	public global::System.Collections.Generic.IEnumerable<ValueTuple<byte, bool>> EnumerateIsDeviceLocked()
	{
		int endDeviceId = StartDeviceId + base.DeviceCount;
		for (byte deviceId = StartDeviceId; deviceId < endDeviceId; deviceId++)
		{
			yield return new ValueTuple<byte, bool>(deviceId, IsDeviceLocked(deviceId));
		}
	}

	[IteratorStateMachine(typeof(_003CEnumerateIsDeviceLockedDiff_003Ed__34))]
	public global::System.Collections.Generic.IEnumerable<ValueTuple<byte, bool>> EnumerateIsDeviceLockedDiff(MyRvLinkDeviceLockStatus? otherDeviceStatus)
	{
		if (otherDeviceStatus != null && (base.DeviceTableId != otherDeviceStatus.DeviceTableId || base.DeviceCount != otherDeviceStatus.DeviceCount))
		{
			otherDeviceStatus = null;
		}
		int endDeviceId = StartDeviceId + base.DeviceCount;
		for (byte deviceId = StartDeviceId; deviceId < endDeviceId; deviceId++)
		{
			bool flag = IsDeviceLocked(deviceId);
			if (otherDeviceStatus == null || flag != otherDeviceStatus.IsDeviceLocked(deviceId))
			{
				yield return new ValueTuple<byte, bool>(deviceId, IsDeviceLocked(deviceId));
			}
		}
	}

	public bool IsDeviceLocked(byte deviceId)
	{
		return GetDeviceStatus(deviceId, StartDeviceId) != 0;
	}

	public void SetDeviceLocked(byte deviceId, bool isLocked)
	{
		SetDeviceStatus(deviceId, isLocked ? ((byte)1) : ((byte)0), StartDeviceId);
	}

	public void SetAllDevicesLocked(bool isLocked)
	{
		int num = StartDeviceId + base.DeviceCount;
		for (byte b = StartDeviceId; b < num; b++)
		{
			SetDeviceLocked(b, isLocked);
		}
	}

	protected override void DevicesToStringBuilder(StringBuilder stringBuilder)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, bool>> enumerator = EnumerateIsDeviceLocked().GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, bool> current = enumerator.Current;
				AppendInterpolatedStringHandler val = new AppendInterpolatedStringHandler(8, 3, stringBuilder);
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted(Environment.NewLine);
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral("    0x");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<byte>(current.Item1, "X2");
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral(": ");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted(current.Item2 ? "Locked" : "Not Locked");
				stringBuilder.Append(ref val);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	public override string ToString()
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		StringBuilder val = new StringBuilder($"{EventType} Table Id: 0x{base.DeviceTableId:X2} SystemLockoutLevel: {SystemLockoutLevel} IgnitionStatus: {IgnitionStatus} ParkingBreakStatus: {ParkingBreakStatus} TotalDevices: {base.DeviceCount}: {ArrayExtension.DebugDump(_rawData, " ", false)}");
		try
		{
			DevicesToStringBuilder(val);
		}
		catch (global::System.Exception ex)
		{
			StringBuilder val2 = val;
			AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(31, 2, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    ERROR Trying to Get Device ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
			val2.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
