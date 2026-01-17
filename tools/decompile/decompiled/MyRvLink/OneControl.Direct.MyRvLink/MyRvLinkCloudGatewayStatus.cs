using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.LogicalDevice;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCloudGatewayStatus : MyRvLinkEventDevicesMultiByte<MyRvLinkCloudGatewayStatus>
{
	[CompilerGenerated]
	private sealed class _003CEnumerateStatus_003Ed__10 : global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MyRvLinkCloudGatewayStatus _003C_003E4__this;

		private int _003Cindex_003E5__2;

		ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState> global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>>.Current
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
		public _003CEnumerateStatus_003Ed__10(int _003C_003E1__state)
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
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			MyRvLinkCloudGatewayStatus myRvLinkCloudGatewayStatus = _003C_003E4__this;
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
				_003Cindex_003E5__2 += myRvLinkCloudGatewayStatus.BytesPerDevice;
				break;
			}
			if (_003Cindex_003E5__2 < myRvLinkCloudGatewayStatus._rawData.Length)
			{
				byte b = myRvLinkCloudGatewayStatus._rawData[_003Cindex_003E5__2];
				LogicalDeviceCloudGatewayStatus val = new LogicalDeviceCloudGatewayStatus();
				((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(myRvLinkCloudGatewayStatus._rawData, _003Cindex_003E5__2 + 1, 1), 1, false);
				SoftwareUpdateState val2 = (SoftwareUpdateState)(myRvLinkCloudGatewayStatus._rawData[_003Cindex_003E5__2 + 1] & 3);
				_003C_003E2__current = new ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>(b, val, val2);
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
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>> global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>>.GetEnumerator()
		{
			_003CEnumerateStatus_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEnumerateStatus_003Ed__10(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>>)this).GetEnumerator();
		}
	}

	private const int CloudGatewayStatusSize = 1;

	private const int SoftwareUpdateStateSize = 1;

	private const byte SoftwareUpdateStateMask = 3;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.CloudGatewayStatus;

	protected override int BytesPerDevice => 3;

	public MyRvLinkCloudGatewayStatus(byte deviceTableId, params ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>[] deviceMessages)
		: base(deviceTableId, deviceMessages.Length)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		int num = 2;
		for (int i = 0; i < deviceMessages.Length; i++)
		{
			ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState> val = deviceMessages[i];
			_rawData[num] = val.Item1;
			((LogicalDeviceDataPacketMutableDoubleBuffer)val.Item2).CopyData(_rawData, num + 1, 1);
			_rawData[num + 1] = (byte)val.Item3;
			num += BytesPerDevice;
		}
	}

	protected MyRvLinkCloudGatewayStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public static MyRvLinkCloudGatewayStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCloudGatewayStatus(rawData);
	}

	[IteratorStateMachine(typeof(_003CEnumerateStatus_003Ed__10))]
	public global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>> EnumerateStatus()
	{
		for (int index = 2; index < _rawData.Length; index += BytesPerDevice)
		{
			byte b = _rawData[index];
			LogicalDeviceCloudGatewayStatus val = new LogicalDeviceCloudGatewayStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, index + 1, 1), 1, false);
			SoftwareUpdateState val2 = (SoftwareUpdateState)(_rawData[index + 1] & 3);
			yield return new ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>(b, val, val2);
		}
	}

	public ValueTuple<LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>? GetStatus(byte deviceId)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>> enumerator = EnumerateStatus().GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState> current = enumerator.Current;
				if (current.Item1 == deviceId)
				{
					return new ValueTuple<LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>(current.Item2, current.Item3);
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		return null;
	}

	protected override void DevicesToStringBuilder(StringBuilder stringBuilder)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>> enumerator = EnumerateStatus().GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState> current = enumerator.Current;
				AppendInterpolatedStringHandler val = new AppendInterpolatedStringHandler(8, 3, stringBuilder);
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted(Environment.NewLine);
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral("    0x");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<byte>(current.Item1, "X2");
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral(": ");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<LogicalDeviceCloudGatewayStatus>(current.Item2);
				stringBuilder.Append(ref val);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}
}
