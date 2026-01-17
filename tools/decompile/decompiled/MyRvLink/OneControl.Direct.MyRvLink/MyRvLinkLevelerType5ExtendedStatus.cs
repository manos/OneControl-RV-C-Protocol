using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.LogicalDevice;
using OneControl.Devices.Leveler.Type5;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkLevelerType5ExtendedStatus : MyRvLinkEventDevicesMultiByte<MyRvLinkLevelerType5ExtendedStatus>
{
	[CompilerGenerated]
	private sealed class _003CEnumerateStatus_003Ed__12 : global::System.Collections.Generic.IEnumerable<ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5>>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5>>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MyRvLinkLevelerType5ExtendedStatus _003C_003E4__this;

		private int _003Cindex_003E5__2;

		ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5> global::System.Collections.Generic.IEnumerator<ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5>>.Current
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
		public _003CEnumerateStatus_003Ed__12(int _003C_003E1__state)
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
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			MyRvLinkLevelerType5ExtendedStatus myRvLinkLevelerType5ExtendedStatus = _003C_003E4__this;
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
				_003Cindex_003E5__2 += myRvLinkLevelerType5ExtendedStatus.BytesPerDevice;
				break;
			}
			if (_003Cindex_003E5__2 < myRvLinkLevelerType5ExtendedStatus._rawData.Length)
			{
				byte b = myRvLinkLevelerType5ExtendedStatus._rawData[_003Cindex_003E5__2];
				ILogicalDeviceLevelerStatusExtendedType5 val = LogicalDeviceLevelerType5.MakeNewStatusExtendedFromMultiplexedDataImpl((LogicalDeviceLevelerStatusExtendedType5Kind)((myRvLinkLevelerType5ExtendedStatus.EventType != MyRvLinkEventType.AutoOperationProgressStatus) ? myRvLinkLevelerType5ExtendedStatus._rawData[_003Cindex_003E5__2 + 1] : 0));
				int num2 = ((myRvLinkLevelerType5ExtendedStatus.EventType == MyRvLinkEventType.AutoOperationProgressStatus) ? (_003Cindex_003E5__2 + 1) : (_003Cindex_003E5__2 + 1 + 1));
				((IDeviceDataPacketMutable)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(myRvLinkLevelerType5ExtendedStatus._rawData, num2, 8), 8, false);
				_003C_003E2__current = new ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5>(b, val);
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
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5>> global::System.Collections.Generic.IEnumerable<ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5>>.GetEnumerator()
		{
			_003CEnumerateStatus_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEnumerateStatus_003Ed__12(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5>>)this).GetEnumerator();
		}
	}

	private const int LevelerType5ExtendedStatusSize = 8;

	private const int EnhancedByteIndexOffsetFromDeviceId = 1;

	private new const int EventTypeIndex = 0;

	private const byte AutoOperationEnhancedByte = 0;

	private bool _isAutoOperationProgressStatus;

	public override MyRvLinkEventType EventType
	{
		get
		{
			if (_rawData.Length < 0 || _rawData[0] != 21)
			{
				return MyRvLinkEventType.LevelerType5ExtendedStatus;
			}
			return MyRvLinkEventType.AutoOperationProgressStatus;
		}
	}

	protected override int BytesPerDevice
	{
		get
		{
			if (EventType != MyRvLinkEventType.AutoOperationProgressStatus)
			{
				return 10;
			}
			return 9;
		}
	}

	public MyRvLinkLevelerType5ExtendedStatus(byte deviceTableId, params ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5>[] deviceMessages)
		: base(deviceTableId, deviceMessages.Length)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		int num = 2;
		for (int i = 0; i < deviceMessages.Length; i++)
		{
			ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5> val = deviceMessages[i];
			_rawData[num++] = val.Item1;
			((IDeviceDataPacketMutable)val.Item2).CopyData(_rawData, num, 8);
			num += 8;
		}
	}

	protected MyRvLinkLevelerType5ExtendedStatus(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public static MyRvLinkLevelerType5ExtendedStatus Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkLevelerType5ExtendedStatus(rawData);
	}

	[IteratorStateMachine(typeof(_003CEnumerateStatus_003Ed__12))]
	public global::System.Collections.Generic.IEnumerable<ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5>> EnumerateStatus()
	{
		for (int index = 2; index < _rawData.Length; index += BytesPerDevice)
		{
			byte b = _rawData[index];
			ILogicalDeviceLevelerStatusExtendedType5 val = LogicalDeviceLevelerType5.MakeNewStatusExtendedFromMultiplexedDataImpl((LogicalDeviceLevelerStatusExtendedType5Kind)((EventType != MyRvLinkEventType.AutoOperationProgressStatus) ? _rawData[index + 1] : 0));
			int num = ((EventType == MyRvLinkEventType.AutoOperationProgressStatus) ? (index + 1) : (index + 1 + 1));
			((IDeviceDataPacketMutable)val).Update((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, num, 8), 8, false);
			yield return new ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5>(b, val);
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
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5>> enumerator = EnumerateStatus().GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5> current = enumerator.Current;
				AppendInterpolatedStringHandler val = new AppendInterpolatedStringHandler(8, 3, stringBuilder);
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted(Environment.NewLine);
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral("    0x");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<byte>(current.Item1, "X2");
				((AppendInterpolatedStringHandler)(ref val)).AppendLiteral(": ");
				((AppendInterpolatedStringHandler)(ref val)).AppendFormatted<ILogicalDeviceLevelerStatusExtendedType5>(current.Item2);
				stringBuilder.Append(ref val);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}
}
