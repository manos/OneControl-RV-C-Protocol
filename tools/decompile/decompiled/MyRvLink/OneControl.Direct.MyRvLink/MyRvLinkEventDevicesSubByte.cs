using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public abstract class MyRvLinkEventDevicesSubByte<TEvent> : MyRvLinkEventDevices<TEvent> where TEvent : IMyRvLinkEvent
{
	protected enum AllowedDevicesPerByte
	{
		Two = 2,
		Four = 4,
		Eight = 8
	}

	[CompilerGenerated]
	private sealed class _003CEnumerateStatus_003Ed__22 : global::System.Collections.Generic.IEnumerable<ValueTuple<byte, byte>>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ValueTuple<byte, byte>>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private ValueTuple<byte, byte> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private int startDeviceId;

		public int _003C_003E3__startDeviceId;

		public MyRvLinkEventDevicesSubByte<TEvent> _003C_003E4__this;

		private int _003CendDeviceId_003E5__2;

		private byte _003CdeviceId_003E5__3;

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
		public _003CEnumerateStatus_003Ed__22(int _003C_003E1__state)
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
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			MyRvLinkEventDevicesSubByte<TEvent> myRvLinkEventDevicesSubByte = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CendDeviceId_003E5__2 = startDeviceId + myRvLinkEventDevicesSubByte.DeviceCount;
				_003CdeviceId_003E5__3 = (byte)startDeviceId;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003CdeviceId_003E5__3++;
				break;
			}
			if (_003CdeviceId_003E5__3 < _003CendDeviceId_003E5__2)
			{
				_003C_003E2__current = new ValueTuple<byte, byte>(_003CdeviceId_003E5__3, myRvLinkEventDevicesSubByte.GetDeviceStatus(_003CdeviceId_003E5__3, startDeviceId));
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
			_003CEnumerateStatus_003Ed__22 _003CEnumerateStatus_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CEnumerateStatus_003Ed__ = this;
			}
			else
			{
				_003CEnumerateStatus_003Ed__ = new _003CEnumerateStatus_003Ed__22(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CEnumerateStatus_003Ed__.startDeviceId = _003C_003E3__startDeviceId;
			return _003CEnumerateStatus_003Ed__;
		}

		[DebuggerHidden]
		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<ValueTuple<byte, byte>>)this).GetEnumerator();
		}
	}

	protected abstract AllowedDevicesPerByte DevicesPerByte { get; }

	protected int DeviceBitsPerStatus => DevicesPerByte switch
	{
		AllowedDevicesPerByte.Two => 4, 
		AllowedDevicesPerByte.Four => 2, 
		AllowedDevicesPerByte.Eight => 1, 
		_ => throw new MyRvLinkException("Unsupported DevicesPerByte"), 
	};

	protected int DeviceStatusBitMask => DevicesPerByte switch
	{
		AllowedDevicesPerByte.Two => 15, 
		AllowedDevicesPerByte.Four => 3, 
		AllowedDevicesPerByte.Eight => 1, 
		_ => throw new MyRvLinkException("Unsupported DevicesPerByte"), 
	};

	protected abstract int DeviceTableIdIndex { get; }

	protected abstract int DeviceCountIndex { get; }

	protected abstract int DeviceStatusStartIndex { get; }

	public byte DeviceTableId => _rawData[DeviceTableIdIndex];

	public int DeviceCount => _rawData[DeviceCountIndex];

	protected override int MaxPayloadLength(int deviceCount)
	{
		return MinPayloadLength + (int)Math.Ceiling((double)deviceCount / (double)DevicesPerByte);
	}

	protected MyRvLinkEventDevicesSubByte(byte deviceTableId, int deviceCount)
		: base(deviceCount)
	{
		_rawData[DeviceTableIdIndex] = deviceTableId;
		_rawData[DeviceCountIndex] = (byte)deviceCount;
	}

	protected MyRvLinkEventDevicesSubByte(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count))
	{
		byte deviceCount = rawData[DeviceCountIndex];
		int num = MaxPayloadLength(deviceCount);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > num)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {EventType} received more then {num} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
	}

	private ValueTuple<int, int> GetStatusIndex(int deviceId, int startDeviceId)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		int num = deviceId - startDeviceId;
		int num2 = num / (int)DevicesPerByte;
		int num3 = (int)(DevicesPerByte - 1 - num % (int)DevicesPerByte) * DeviceBitsPerStatus;
		return new ValueTuple<int, int>(num2, num3);
	}

	public byte GetDeviceStatus(byte deviceId, int startDeviceId)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		ValueTuple<int, int> statusIndex = GetStatusIndex(deviceId, startDeviceId);
		int num = statusIndex.Item1 + DeviceStatusStartIndex;
		if (statusIndex.Item1 < 0)
		{
			return 0;
		}
		if (num >= _rawData.Length)
		{
			return 0;
		}
		return (byte)((_rawData[num] >> statusIndex.Item2) & (uint)DeviceStatusBitMask);
	}

	[IteratorStateMachine(typeof(MyRvLinkEventDevicesSubByte<>._003CEnumerateStatus_003Ed__22))]
	protected global::System.Collections.Generic.IEnumerable<ValueTuple<byte, byte>> EnumerateStatus(int startDeviceId)
	{
		int endDeviceId = startDeviceId + DeviceCount;
		for (byte deviceId = (byte)startDeviceId; deviceId < endDeviceId; deviceId++)
		{
			yield return new ValueTuple<byte, byte>(deviceId, GetDeviceStatus(deviceId, startDeviceId));
		}
	}

	public void SetDeviceStatus(byte deviceId, byte status, int startDeviceId)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		ValueTuple<int, int> statusIndex = GetStatusIndex(deviceId, startDeviceId);
		int num = statusIndex.Item1 + DeviceStatusStartIndex;
		if (statusIndex.Item1 >= 0 && num < _rawData.Length)
		{
			byte b = (byte)(status & DeviceStatusBitMask);
			byte b2 = _rawData[num];
			b2 = (byte)(b2 & ~(DeviceStatusBitMask << statusIndex.Item2));
			if (b != 0)
			{
				b2 = (byte)(b2 | (b << statusIndex.Item2));
			}
			_rawData[num] = b2;
		}
	}

	public override string ToString()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		StringBuilder val = new StringBuilder($"{EventType} Table Id: 0x{DeviceTableId:X2} Total: {DeviceCount}: {ArrayExtension.DebugDump(_rawData, " ", false)}");
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

	protected abstract void DevicesToStringBuilder(StringBuilder stringBuilder);
}
