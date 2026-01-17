using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceSessionStatus : MyRvLinkEventDevicesSubByte<MyRvLinkDeviceSessionStatus>
{
	[CompilerGenerated]
	private sealed class _003CEnumerateIsDeviceSessionOpen_003Ed__18 : global::System.Collections.Generic.IEnumerable<ValueTuple<byte, bool>>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ValueTuple<byte, bool>>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private ValueTuple<byte, bool> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MyRvLinkDeviceSessionStatus _003C_003E4__this;

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
		public _003CEnumerateIsDeviceSessionOpen_003Ed__18(int _003C_003E1__state)
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
			MyRvLinkDeviceSessionStatus myRvLinkDeviceSessionStatus = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CendDeviceId_003E5__2 = myRvLinkDeviceSessionStatus.StartDeviceId + myRvLinkDeviceSessionStatus.DeviceCount;
				_003CdeviceId_003E5__3 = myRvLinkDeviceSessionStatus.StartDeviceId;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003CdeviceId_003E5__3++;
				break;
			}
			if (_003CdeviceId_003E5__3 < _003CendDeviceId_003E5__2)
			{
				_003C_003E2__current = new ValueTuple<byte, bool>(_003CdeviceId_003E5__3, myRvLinkDeviceSessionStatus.IsDeviceSessionOpen(_003CdeviceId_003E5__3));
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
			_003CEnumerateIsDeviceSessionOpen_003Ed__18 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEnumerateIsDeviceSessionOpen_003Ed__18(0)
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

	public override MyRvLinkEventType EventType => MyRvLinkEventType.DeviceSessionStatus;

	protected override AllowedDevicesPerByte DevicesPerByte => AllowedDevicesPerByte.Eight;

	protected override int MinPayloadLength => 3;

	[field: CompilerGenerated]
	public byte StartDeviceId
	{
		[CompilerGenerated]
		get;
	}

	protected override int DeviceTableIdIndex => 1;

	protected override int DeviceCountIndex => 2;

	protected override int DeviceStatusStartIndex => 3;

	public MyRvLinkDeviceSessionStatus(byte deviceTableId, byte deviceCount)
		: base(deviceTableId, (int)deviceCount)
	{
	}

	protected MyRvLinkDeviceSessionStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base((global::System.Collections.Generic.IReadOnlyList<byte>)ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count))
	{
	}

	public static MyRvLinkDeviceSessionStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkDeviceSessionStatus(rawData);
	}

	[IteratorStateMachine(typeof(_003CEnumerateIsDeviceSessionOpen_003Ed__18))]
	public global::System.Collections.Generic.IEnumerable<ValueTuple<byte, bool>> EnumerateIsDeviceSessionOpen()
	{
		int endDeviceId = StartDeviceId + base.DeviceCount;
		for (byte deviceId = StartDeviceId; deviceId < endDeviceId; deviceId++)
		{
			yield return new ValueTuple<byte, bool>(deviceId, IsDeviceSessionOpen(deviceId));
		}
	}

	public bool IsDeviceSessionOpen(byte deviceId)
	{
		return GetDeviceStatus(deviceId, StartDeviceId) != 0;
	}

	public void SetDeviceSessionOpen(byte deviceId, bool isSessionOpen)
	{
		SetDeviceStatus(deviceId, isSessionOpen ? ((byte)1) : ((byte)0), StartDeviceId);
	}

	public void SetAllDevicesSessionOpenTo(bool isSessionOpen)
	{
		int num = StartDeviceId + base.DeviceCount;
		for (byte b = StartDeviceId; b < num; b++)
		{
			SetDeviceSessionOpen(b, isSessionOpen);
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
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, bool>> enumerator = EnumerateIsDeviceSessionOpen().GetEnumerator();
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
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted(current.Item2 ? "Open" : "NotOpen");
				stringBuilder.Append(ref val);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}
}
