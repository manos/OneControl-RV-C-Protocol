using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using IDS.Portable.Common;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public abstract class MyRvLinkDevice : IMyRvLinkDevice
{
	[CompilerGenerated]
	private sealed class _003CFindLogicalDevicesMatchingPhysicalHardware_003Ed__15 : global::System.Collections.Generic.IEnumerable<ILogicalDevice>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ILogicalDevice>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private ILogicalDevice _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ILogicalDeviceService deviceService;

		public ILogicalDeviceService _003C_003E3__deviceService;

		public MyRvLinkDevice _003C_003E4__this;

		private IMyRvLinkDeviceForLogicalDevice _003CmyRvLinkDeviceForLogicalDevice_003E5__2;

		private global::System.Collections.Generic.IEnumerator<ILogicalDevice> _003C_003E7__wrap2;

		ILogicalDevice global::System.Collections.Generic.IEnumerator<ILogicalDevice>.Current
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
		public _003CFindLogicalDevicesMatchingPhysicalHardware_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void global::System.IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CmyRvLinkDeviceForLogicalDevice_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				MyRvLinkDevice myRvLinkDevice = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					ILogicalDeviceManager deviceManager = deviceService.DeviceManager;
					if (deviceManager == null)
					{
						return false;
					}
					MyRvLinkDevice myRvLinkDevice2 = myRvLinkDevice;
					_003CmyRvLinkDeviceForLogicalDevice_003E5__2 = myRvLinkDevice2 as IMyRvLinkDeviceForLogicalDevice;
					if (_003CmyRvLinkDeviceForLogicalDevice_003E5__2 == null)
					{
						return false;
					}
					_003C_003E7__wrap2 = deviceManager.LogicalDevices.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (((global::System.Collections.IEnumerator)_003C_003E7__wrap2).MoveNext())
				{
					ILogicalDevice current = _003C_003E7__wrap2.Current;
					if (current.LogicalId.IsMatchingPhysicalHardware(_003CmyRvLinkDeviceForLogicalDevice_003E5__2.ProductId, _003CmyRvLinkDeviceForLogicalDevice_003E5__2.DeviceType, _003CmyRvLinkDeviceForLogicalDevice_003E5__2.DeviceInstance, _003CmyRvLinkDeviceForLogicalDevice_003E5__2.ProductMacAddress))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				return false;
			}
			catch
			{
				//try-fault
				((global::System.IDisposable)this).Dispose();
				throw;
			}
		}

		bool global::System.Collections.IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				((global::System.IDisposable)_003C_003E7__wrap2).Dispose();
			}
		}

		[DebuggerHidden]
		void global::System.Collections.IEnumerator.Reset()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		global::System.Collections.Generic.IEnumerator<ILogicalDevice> global::System.Collections.Generic.IEnumerable<ILogicalDevice>.GetEnumerator()
		{
			_003CFindLogicalDevicesMatchingPhysicalHardware_003Ed__15 _003CFindLogicalDevicesMatchingPhysicalHardware_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CFindLogicalDevicesMatchingPhysicalHardware_003Ed__ = this;
			}
			else
			{
				_003CFindLogicalDevicesMatchingPhysicalHardware_003Ed__ = new _003CFindLogicalDevicesMatchingPhysicalHardware_003Ed__15(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CFindLogicalDevicesMatchingPhysicalHardware_003Ed__.deviceService = _003C_003E3__deviceService;
			return _003CFindLogicalDevicesMatchingPhysicalHardware_003Ed__;
		}

		[DebuggerHidden]
		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<ILogicalDevice>)this).GetEnumerator();
		}
	}

	private const string LogTag = "MyRvLinkDevice";

	public const int EncodeHeaderSize = 2;

	protected const int DeviceProtocolIndex = 0;

	protected const int DeviceEntrySizeIndex = 1;

	[field: CompilerGenerated]
	public MyRvLinkDeviceProtocol Protocol
	{
		[CompilerGenerated]
		get;
	}

	public virtual byte EncodeSize => 2;

	protected MyRvLinkDevice(MyRvLinkDeviceProtocol protocol)
	{
		Protocol = protocol;
	}

	public virtual string ToString()
	{
		return $"{Protocol}";
	}

	public static MyRvLinkDeviceProtocol DecodeDeviceProtocol(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return (MyRvLinkDeviceProtocol)decodeBuffer[0];
	}

	public static int DecodePayloadSize(global::System.Collections.Generic.IReadOnlyList<byte> decodeBuffer)
	{
		return decodeBuffer[1];
	}

	public virtual int EncodeIntoBuffer(byte[] buffer, int offset)
	{
		buffer[offset] = (byte)Protocol;
		buffer[1 + offset] = (byte)(EncodeSize - 2);
		return EncodeSize;
	}

	public static IMyRvLinkDevice TryDecodeFromRawBuffer(global::System.Collections.Generic.IReadOnlyList<byte> buffer)
	{
		MyRvLinkDeviceProtocol myRvLinkDeviceProtocol = MyRvLinkDeviceProtocol.None;
		try
		{
			myRvLinkDeviceProtocol = DecodeDeviceProtocol(buffer);
			return myRvLinkDeviceProtocol switch
			{
				MyRvLinkDeviceProtocol.Host => MyRvLinkDeviceHost.Decode(buffer), 
				MyRvLinkDeviceProtocol.IdsCan => MyRvLinkDeviceIdsCan.Decode(buffer), 
				MyRvLinkDeviceProtocol.None => new MyRvLinkDeviceNone(myRvLinkDeviceProtocol), 
				_ => new MyRvLinkDeviceNone(myRvLinkDeviceProtocol), 
			};
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Debug("MyRvLinkDevice", $"Error trying to decode device for {myRvLinkDeviceProtocol} returning unknown device: {ex.Message}", global::System.Array.Empty<object>());
			return new MyRvLinkDeviceNone(myRvLinkDeviceProtocol);
		}
	}

	[IteratorStateMachine(typeof(_003CFindLogicalDevicesMatchingPhysicalHardware_003Ed__15))]
	public global::System.Collections.Generic.IEnumerable<ILogicalDevice> FindLogicalDevicesMatchingPhysicalHardware(ILogicalDeviceService deviceService)
	{
		ILogicalDeviceManager deviceManager = deviceService.DeviceManager;
		if (deviceManager == null || !(this is IMyRvLinkDeviceForLogicalDevice myRvLinkDeviceForLogicalDevice))
		{
			yield break;
		}
		global::System.Collections.Generic.IEnumerator<ILogicalDevice> enumerator = deviceManager.LogicalDevices.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ILogicalDevice current = enumerator.Current;
				if (current.LogicalId.IsMatchingPhysicalHardware(myRvLinkDeviceForLogicalDevice.ProductId, myRvLinkDeviceForLogicalDevice.DeviceType, myRvLinkDeviceForLogicalDevice.DeviceInstance, myRvLinkDeviceForLogicalDevice.ProductMacAddress))
				{
					yield return current;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}
}
