using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.LogicalDevice;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkHvacStatus : MyRvLinkEventDevicesMultiByte<MyRvLinkHvacStatus>
{
	[CompilerGenerated]
	private sealed class _003CEnumerateStatus_003Ed__9 : global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx>>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx>>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MyRvLinkHvacStatus _003C_003E4__this;

		private int _003Cindex_003E5__2;

		ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx> global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx>>.Current
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
		public _003CEnumerateStatus_003Ed__9(int _003C_003E1__state)
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
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			MyRvLinkHvacStatus myRvLinkHvacStatus = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Cindex_003E5__2 = 2;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Cindex_003E5__2 += myRvLinkHvacStatus.BytesPerDevice;
				break;
			}
			if (_003Cindex_003E5__2 < myRvLinkHvacStatus._rawData.Length)
			{
				byte b = myRvLinkHvacStatus._rawData[_003Cindex_003E5__2];
				LogicalDeviceClimateZoneStatus val = new LogicalDeviceClimateZoneStatus();
				((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(myRvLinkHvacStatus._rawData, _003Cindex_003E5__2 + 1, 8), 8, false);
				LogicalDeviceClimateZoneStatusEx val2 = new LogicalDeviceClimateZoneStatusEx();
				((LogicalDeviceDataPacketMutableDoubleBuffer)val2).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(myRvLinkHvacStatus._rawData, _003Cindex_003E5__2 + 1 + 8, 2), 2, false);
				_003C_003E2__current = new ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx>(b, val, val2);
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
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx>> global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx>>.GetEnumerator()
		{
			_003CEnumerateStatus_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEnumerateStatus_003Ed__9(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx>>)this).GetEnumerator();
		}
	}

	private const int HvacStatusSize = 8;

	private const int HvacStatusSizeEx = 2;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.HvacStatus;

	protected override int BytesPerDevice => 11;

	public MyRvLinkHvacStatus(byte deviceTableId, params ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx?>[] deviceMessages)
		: base(deviceTableId, deviceMessages.Length)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		int num = 2;
		for (int i = 0; i < deviceMessages.Length; i++)
		{
			ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx> val = deviceMessages[i];
			_rawData[num++] = val.Item1;
			((LogicalDeviceDataPacketMutableDoubleBuffer)val.Item2).CopyData(_rawData, num, 8);
			num += 8;
			if (val.Item3 == null)
			{
				global::System.Array.Clear((global::System.Array)_rawData, num, 2);
			}
			else
			{
				((LogicalDeviceDataPacketMutableDoubleBuffer)val.Item3).CopyData(_rawData, num, 2);
			}
			num += 2;
		}
	}

	protected MyRvLinkHvacStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public static MyRvLinkHvacStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkHvacStatus(rawData);
	}

	[IteratorStateMachine(typeof(_003CEnumerateStatus_003Ed__9))]
	public global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx>> EnumerateStatus()
	{
		for (int index = 2; index < _rawData.Length; index += BytesPerDevice)
		{
			byte b = _rawData[index];
			LogicalDeviceClimateZoneStatus val = new LogicalDeviceClimateZoneStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, index + 1, 8), 8, false);
			LogicalDeviceClimateZoneStatusEx val2 = new LogicalDeviceClimateZoneStatusEx();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val2).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, index + 1 + 8, 2), 2, false);
			yield return new ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx>(b, val, val2);
		}
	}

	public LogicalDeviceClimateZoneStatus? GetClimateZoneStatus(int deviceId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		LogicalDeviceClimateZoneStatus val = new LogicalDeviceClimateZoneStatus();
		if (!GetClimateZoneStatus(deviceId, val))
		{
			return null;
		}
		return val;
	}

	public bool GetClimateZoneStatus(int deviceId, LogicalDeviceClimateZoneStatus climateZoneStatus)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 2; i < _rawData.Length; i += BytesPerDevice)
		{
			if (_rawData[i] == deviceId)
			{
				((LogicalDeviceDataPacketMutableDoubleBuffer)climateZoneStatus).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, i + 1, 8), 8, false);
				return true;
			}
		}
		return false;
	}

	public LogicalDeviceClimateZoneStatusEx? GetClimateZoneStatusEx(int deviceId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		LogicalDeviceClimateZoneStatusEx val = new LogicalDeviceClimateZoneStatusEx();
		if (!GetClimateZoneStatusEx(deviceId, val))
		{
			return null;
		}
		return val;
	}

	public bool GetClimateZoneStatusEx(int deviceId, LogicalDeviceClimateZoneStatusEx climateZoneStatusEx)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 2; i < _rawData.Length; i += BytesPerDevice)
		{
			if (_rawData[i] == deviceId)
			{
				((LogicalDeviceDataPacketMutableDoubleBuffer)climateZoneStatusEx).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, i + 1 + 8, 2), 2, false);
				return true;
			}
		}
		return false;
	}

	protected override void DevicesToStringBuilder(StringBuilder stringBuilder)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx>> enumerator = EnumerateStatus().GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx> current = enumerator.Current;
				AppendInterpolatedStringHandler val = new AppendInterpolatedStringHandler(9, 4, stringBuilder);
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted(Environment.NewLine);
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral("    0x");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<byte>(current.Item1, "X2");
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral(": ");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<LogicalDeviceClimateZoneStatus>(current.Item2);
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral(" ");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<LogicalDeviceClimateZoneStatusEx>(current.Item3);
				stringBuilder.Append(ref val);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}
}
