using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.LogicalDevice;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkGeneratorGenieStatus : MyRvLinkEventDevicesMultiByte<MyRvLinkGeneratorGenieStatus>
{
	[CompilerGenerated]
	private sealed class _003CEnumerateStatus_003Ed__8 : global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceGeneratorGenieStatus>>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceGeneratorGenieStatus>>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private ValueTuple<byte, LogicalDeviceGeneratorGenieStatus> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MyRvLinkGeneratorGenieStatus _003C_003E4__this;

		private int _003Cindex_003E5__2;

		ValueTuple<byte, LogicalDeviceGeneratorGenieStatus> global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceGeneratorGenieStatus>>.Current
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
			MyRvLinkGeneratorGenieStatus myRvLinkGeneratorGenieStatus = _003C_003E4__this;
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
				_003Cindex_003E5__2 += myRvLinkGeneratorGenieStatus.BytesPerDevice;
				break;
			}
			if (_003Cindex_003E5__2 < myRvLinkGeneratorGenieStatus._rawData.Length)
			{
				byte b = myRvLinkGeneratorGenieStatus._rawData[_003Cindex_003E5__2];
				LogicalDeviceGeneratorGenieStatus val = new LogicalDeviceGeneratorGenieStatus();
				((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(myRvLinkGeneratorGenieStatus._rawData, _003Cindex_003E5__2 + 1, 5), 5, false);
				_003C_003E2__current = new ValueTuple<byte, LogicalDeviceGeneratorGenieStatus>(b, val);
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
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceGeneratorGenieStatus>> global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceGeneratorGenieStatus>>.GetEnumerator()
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
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceGeneratorGenieStatus>>)this).GetEnumerator();
		}
	}

	private const int GeneratorStatusSize = 5;

	public override MyRvLinkEventType EventType => MyRvLinkEventType.GeneratorGenieStatus;

	protected override int BytesPerDevice => 6;

	public MyRvLinkGeneratorGenieStatus(byte deviceTableId, params ValueTuple<byte, LogicalDeviceGeneratorGenieStatus>[] deviceMessages)
		: base(deviceTableId, deviceMessages.Length)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		int num = 2;
		for (int i = 0; i < deviceMessages.Length; i++)
		{
			ValueTuple<byte, LogicalDeviceGeneratorGenieStatus> val = deviceMessages[i];
			_rawData[num++] = val.Item1;
			((LogicalDeviceDataPacketMutableDoubleBuffer)val.Item2).CopyData(_rawData, num, 5);
			num += 5;
		}
	}

	protected MyRvLinkGeneratorGenieStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public static MyRvLinkGeneratorGenieStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkGeneratorGenieStatus(rawData);
	}

	[IteratorStateMachine(typeof(_003CEnumerateStatus_003Ed__8))]
	public global::System.Collections.Generic.IEnumerable<ValueTuple<byte, LogicalDeviceGeneratorGenieStatus>> EnumerateStatus()
	{
		for (int index = 2; index < _rawData.Length; index += BytesPerDevice)
		{
			byte b = _rawData[index];
			LogicalDeviceGeneratorGenieStatus val = new LogicalDeviceGeneratorGenieStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, index + 1, 5), 5, false);
			yield return new ValueTuple<byte, LogicalDeviceGeneratorGenieStatus>(b, val);
		}
	}

	public LogicalDeviceGeneratorGenieStatus? GetGeneratorStatus(int deviceId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		LogicalDeviceGeneratorGenieStatus val = new LogicalDeviceGeneratorGenieStatus();
		if (!GetGeneratorGenieStatus(deviceId, val))
		{
			return null;
		}
		return val;
	}

	public bool GetGeneratorGenieStatus(int deviceId, LogicalDeviceGeneratorGenieStatus generatorStatus)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 2; i < _rawData.Length; i += BytesPerDevice)
		{
			if (_rawData[i] == deviceId)
			{
				((LogicalDeviceDataPacketMutableDoubleBuffer)generatorStatus).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, i + 1, 5), 5, false);
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
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceGeneratorGenieStatus>> enumerator = EnumerateStatus().GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceGeneratorGenieStatus> current = enumerator.Current;
				AppendInterpolatedStringHandler val = new AppendInterpolatedStringHandler(8, 3, stringBuilder);
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted(Environment.NewLine);
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral("    0x");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<byte>(current.Item1, "X2");
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral(": ");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<LogicalDeviceGeneratorGenieStatus>(current.Item2);
				stringBuilder.Append(ref val);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}
}
