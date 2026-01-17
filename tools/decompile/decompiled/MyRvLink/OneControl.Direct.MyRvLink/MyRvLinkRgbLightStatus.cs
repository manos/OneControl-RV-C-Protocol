using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.LogicalDevice;
using OneControl.Devices.LightRgb;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkRgbLightStatus : MyRvLinkEventDevicesMultiByte<MyRvLinkRgbLightStatus>
{
	[CompilerGenerated]
	private sealed class _003CEnumerateStatus_003Ed__8 : global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceLightRgbStatus>>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceLightRgbStatus>>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private ValueTuple<byte, LogicalDeviceLightRgbStatus> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MyRvLinkRgbLightStatus _003C_003E4__this;

		private int _003Cindex_003E5__2;

		ValueTuple<byte, LogicalDeviceLightRgbStatus> global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceLightRgbStatus>>.Current
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
		public _003CEnumerateStatus_003Ed__8(int _003C_003E1__state)
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
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			MyRvLinkRgbLightStatus myRvLinkRgbLightStatus = _003C_003E4__this;
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
				_003Cindex_003E5__2 += myRvLinkRgbLightStatus.BytesPerDevice;
				break;
			}
			if (_003Cindex_003E5__2 < myRvLinkRgbLightStatus._rawData.Length)
			{
				byte b = myRvLinkRgbLightStatus._rawData[_003Cindex_003E5__2];
				LogicalDeviceLightRgbStatus val = new LogicalDeviceLightRgbStatus();
				((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(myRvLinkRgbLightStatus._rawData, _003Cindex_003E5__2 + 1, 8), 8, false);
				_003C_003E2__current = new ValueTuple<byte, LogicalDeviceLightRgbStatus>(b, val);
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
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceLightRgbStatus>> global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceLightRgbStatus>>.GetEnumerator()
		{
			_003CEnumerateStatus_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEnumerateStatus_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceLightRgbStatus>>)this).GetEnumerator();
		}
	}

	private const int RgbLightStatusSize = 8;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.RgbLightStatus;

	protected override int BytesPerDevice => 9;

	public MyRvLinkRgbLightStatus(byte deviceTableId, params ValueTuple<byte, LogicalDeviceLightRgbStatus>[] deviceMessages)
		: base(deviceTableId, deviceMessages.Length)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		int num = 2;
		for (int i = 0; i < deviceMessages.Length; i++)
		{
			ValueTuple<byte, LogicalDeviceLightRgbStatus> val = deviceMessages[i];
			_rawData[num++] = val.Item1;
			((LogicalDeviceDataPacketMutableDoubleBuffer)val.Item2).CopyData(_rawData, num, 8);
			num += 8;
		}
	}

	protected MyRvLinkRgbLightStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public static MyRvLinkRgbLightStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkRgbLightStatus(rawData);
	}

	[IteratorStateMachine(typeof(_003CEnumerateStatus_003Ed__8))]
	public global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceLightRgbStatus>> EnumerateStatus()
	{
		for (int index = 2; index < _rawData.Length; index += BytesPerDevice)
		{
			byte b = _rawData[index];
			LogicalDeviceLightRgbStatus val = new LogicalDeviceLightRgbStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, index + 1, 8), 8, false);
			yield return new ValueTuple<byte, LogicalDeviceLightRgbStatus>(b, val);
		}
	}

	public LogicalDeviceLightRgbStatus? GetRgbStatus(int deviceId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		LogicalDeviceLightRgbStatus val = new LogicalDeviceLightRgbStatus();
		if (!GetRgbStatus(deviceId, val))
		{
			return null;
		}
		return val;
	}

	public bool GetRgbStatus(int deviceId, LogicalDeviceLightRgbStatus rgbStatus)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 2; i < _rawData.Length; i += BytesPerDevice)
		{
			if (_rawData[i] == deviceId)
			{
				((LogicalDeviceDataPacketMutableDoubleBuffer)rgbStatus).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, i + 1, 8), 8, false);
				return true;
			}
		}
		return false;
	}

	protected override void DevicesToStringBuilder(StringBuilder stringBuilder)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceLightRgbStatus>> enumerator = EnumerateStatus().GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceLightRgbStatus> current = enumerator.Current;
				AppendInterpolatedStringHandler val = new AppendInterpolatedStringHandler(8, 3, stringBuilder);
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted(Environment.NewLine);
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral("    0x");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<byte>(current.Item1, "X2");
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral(": ");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<LogicalDeviceLightRgbStatus>(current.Item2);
				stringBuilder.Append(ref val);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}
}
