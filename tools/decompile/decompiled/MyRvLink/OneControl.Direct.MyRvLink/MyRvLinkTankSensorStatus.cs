using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkTankSensorStatus : MyRvLinkEventDevicesMultiByte<MyRvLinkTankSensorStatus>
{
	[CompilerGenerated]
	private sealed class _003CEnumerateStatus_003Ed__7 : global::System.Collections.Generic.IEnumerable<ValueTuple<byte, byte>>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ValueTuple<byte, byte>>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private ValueTuple<byte, byte> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MyRvLinkTankSensorStatus _003C_003E4__this;

		private int _003Cindex_003E5__2;

		ValueTuple<byte, byte> global::System.Collections.Generic.IEnumerator<ValueTuple<byte, byte>>.Current
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
		public _003CEnumerateStatus_003Ed__7(int _003C_003E1__state)
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
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			MyRvLinkTankSensorStatus myRvLinkTankSensorStatus = _003C_003E4__this;
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
				_003Cindex_003E5__2 += myRvLinkTankSensorStatus.BytesPerDevice;
				break;
			}
			if (_003Cindex_003E5__2 < myRvLinkTankSensorStatus._rawData.Length)
			{
				byte b = myRvLinkTankSensorStatus._rawData[_003Cindex_003E5__2];
				byte b2 = myRvLinkTankSensorStatus._rawData[_003Cindex_003E5__2 + 1];
				_003C_003E2__current = new ValueTuple<byte, byte>(b, b2);
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
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, byte>> global::System.Collections.Generic.IEnumerable<ValueTuple<byte, byte>>.GetEnumerator()
		{
			_003CEnumerateStatus_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEnumerateStatus_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<ValueTuple<byte, byte>>)this).GetEnumerator();
		}
	}

	public override MyRvLinkEventType EventType => MyRvLinkEventType.TankSensorStatus;

	protected override int BytesPerDevice => 2;

	public MyRvLinkTankSensorStatus(byte deviceTableId, params ValueTuple<byte, byte>[] devicePositions)
		: base(deviceTableId, devicePositions.Length)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		int num = 2;
		for (int i = 0; i < devicePositions.Length; i++)
		{
			ValueTuple<byte, byte> val = devicePositions[i];
			_rawData[num++] = val.Item1;
			_rawData[num++] = val.Item2;
		}
	}

	protected MyRvLinkTankSensorStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public static MyRvLinkTankSensorStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkTankSensorStatus(rawData);
	}

	[IteratorStateMachine(typeof(_003CEnumerateStatus_003Ed__7))]
	public global::System.Collections.Generic.IEnumerable<ValueTuple<byte, byte>> EnumerateStatus()
	{
		for (int index = 2; index < _rawData.Length; index += BytesPerDevice)
		{
			byte b = _rawData[index];
			byte b2 = _rawData[index + 1];
			yield return new ValueTuple<byte, byte>(b, b2);
		}
	}

	public byte? GetPercent(int deviceId)
	{
		for (int i = 2; i < _rawData.Length; i += BytesPerDevice)
		{
			if (_rawData[i] == deviceId)
			{
				return _rawData[i + 1];
			}
		}
		return null;
	}

	protected override void DevicesToStringBuilder(StringBuilder stringBuilder)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, byte>> enumerator = EnumerateStatus().GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, byte> current = enumerator.Current;
				AppendInterpolatedStringHandler val = new AppendInterpolatedStringHandler(9, 3, stringBuilder);
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted(Environment.NewLine);
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral("    0x");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<byte>(current.Item1, "X2");
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral(": ");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<byte>(current.Item2);
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral("%");
				stringBuilder.Append(ref val);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}
}
