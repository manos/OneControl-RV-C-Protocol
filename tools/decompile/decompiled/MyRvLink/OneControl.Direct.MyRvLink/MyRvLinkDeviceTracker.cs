using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using IDS.Core.IDS_CAN;
using IDS.Portable.Common;
using IDS.Portable.Devices.JaycoTbbGateway;
using IDS.Portable.LogicalDevice;
using OneControl.Devices;
using OneControl.Devices.AccessoryGateway;
using OneControl.Devices.AwningSensor;
using OneControl.Devices.BatteryMonitor;
using OneControl.Devices.BootLoader;
using OneControl.Devices.BrakingSystem;
using OneControl.Devices.DoorLock;
using OneControl.Devices.Leveler.Type5;
using OneControl.Devices.LightRgb;
using OneControl.Devices.TemperatureSensor;
using OneControl.Direct.MyRvLink.Events;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceTracker : CommonDisposable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass29_0
	{
		[StructLayout((LayoutKind)3)]
		private struct _003C_003CGetDevicesIfNeeded_003Eb__0_003Ed : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public _003C_003Ec__DisplayClass29_0 _003C_003E4__this;

			private TaskAwaiter<global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice>?> _003C_003Eu__1;

			private ConfiguredTaskAwaiter<List<IMyRvLinkDevice>> _003C_003Eu__2;

			private TaskAwaiter<bool> _003C_003Eu__3;

			private void MoveNext()
			{
				//IL_0081: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_0251: Unknown result type (might be due to invalid IL or missing references)
				//IL_0256: Unknown result type (might be due to invalid IL or missing references)
				//IL_0286: Unknown result type (might be due to invalid IL or missing references)
				//IL_028b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0293: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_026b: Unknown result type (might be due to invalid IL or missing references)
				//IL_026d: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
				int num = _003C_003E1__state;
				_003C_003Ec__DisplayClass29_0 _003C_003Ec__DisplayClass29_ = _003C_003E4__this;
				try
				{
					if (num != 0)
					{
						if ((uint)(num - 1) <= 1u)
						{
							goto IL_018e;
						}
						_003C_003Ec__DisplayClass29_._003C_003E4__this.DeviceList = new List<IMyRvLinkDevice>();
					}
					try
					{
						TaskAwaiter<global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice>> val;
						if (num != 0)
						{
							val = _003C_003Ec__DisplayClass29_._003C_003E4__this.MyRvLinkService.DeviceTableIdCache.GetDevicesForDeviceTableCrcAsync(_003C_003Ec__DisplayClass29_._003C_003E4__this.DeviceTableCrc).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice>>, _003C_003CGetDevicesIfNeeded_003Eb__0_003Ed>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = _003C_003Eu__1;
							_003C_003Eu__1 = default(TaskAwaiter<global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice>>);
							num = (_003C_003E1__state = -1);
						}
						global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice> result = val.GetResult();
						if (result == null)
						{
							throw new global::System.Exception("Device info not found in device cache.");
						}
						_003C_003Ec__DisplayClass29_._003C_003E4__this.DeviceList = new List<IMyRvLinkDevice>((global::System.Collections.Generic.IEnumerable<IMyRvLinkDevice>)result);
						TaggedLog.Information("MyRvLinkDeviceTracker", $"{_003C_003Ec__DisplayClass29_._003C_003E4__this.LogPrefix} Getting Cached Devices for 0x{_003C_003Ec__DisplayClass29_._003C_003E4__this.DeviceTableCrc:x8} has {_003C_003Ec__DisplayClass29_._003C_003E4__this.DeviceList.Count} devices", global::System.Array.Empty<object>());
					}
					catch (global::System.Exception ex)
					{
						TaggedLog.Error("MyRvLinkDeviceTracker", _003C_003Ec__DisplayClass29_._003C_003E4__this.LogPrefix + " Unable to load devices " + ex.Message, global::System.Array.Empty<object>());
					}
					if (_003C_003Ec__DisplayClass29_._003C_003E4__this.DeviceList.Count == 0)
					{
						goto IL_018e;
					}
					goto IL_02db;
					IL_02db:
					CancellationTokenSource? commandGetDevicesTcs = _003C_003Ec__DisplayClass29_._003C_003E4__this._commandGetDevicesTcs;
					if (commandGetDevicesTcs != null)
					{
						CancellationTokenSourceExtension.TryCancelAndDispose(commandGetDevicesTcs);
					}
					_003C_003Ec__DisplayClass29_._003C_003E4__this._commandGetDevicesTcs = null;
					goto end_IL_000e;
					IL_018e:
					try
					{
						TaskAwaiter<bool> val2;
						ConfiguredTaskAwaiter<List<IMyRvLinkDevice>> val3;
						if (num != 1)
						{
							if (num == 2)
							{
								val2 = _003C_003Eu__3;
								_003C_003Eu__3 = default(TaskAwaiter<bool>);
								num = (_003C_003E1__state = -1);
								goto IL_02a2;
							}
							val3 = _003C_003Ec__DisplayClass29_._003C_003E4__this.GetDevicesAsync(_003C_003Ec__DisplayClass29_.commandCancellationToken).ConfigureAwait(false).GetAwaiter();
							if (!val3.IsCompleted)
							{
								num = (_003C_003E1__state = 1);
								_003C_003Eu__2 = val3;
								((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<List<IMyRvLinkDevice>>, _003C_003CGetDevicesIfNeeded_003Eb__0_003Ed>(ref val3, ref this);
								return;
							}
						}
						else
						{
							val3 = _003C_003Eu__2;
							_003C_003Eu__2 = default(ConfiguredTaskAwaiter<List<IMyRvLinkDevice>>);
							num = (_003C_003E1__state = -1);
						}
						List<IMyRvLinkDevice> result2 = val3.GetResult();
						_003C_003Ec__DisplayClass29_._003C_003E4__this.DeviceList = result2;
						val2 = _003C_003Ec__DisplayClass29_._003C_003E4__this.MyRvLinkService.DeviceTableIdCache.UpdateDevicesAsync(_003C_003Ec__DisplayClass29_._003C_003E4__this.DeviceTableCrc, _003C_003Ec__DisplayClass29_._003C_003E4__this.DeviceTableId, (global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice>)_003C_003Ec__DisplayClass29_._003C_003E4__this.DeviceList).GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (_003C_003E1__state = 2);
							_003C_003Eu__3 = val2;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003C_003CGetDevicesIfNeeded_003Eb__0_003Ed>(ref val2, ref this);
							return;
						}
						goto IL_02a2;
						IL_02a2:
						val2.GetResult();
					}
					catch (global::System.Exception ex2)
					{
						TaggedLog.Debug("MyRvLinkDeviceTracker", _003C_003Ec__DisplayClass29_._003C_003E4__this.LogPrefix + " Get Devices failed: " + ex2.Message, global::System.Array.Empty<object>());
					}
					goto IL_02db;
					end_IL_000e:;
				}
				catch (global::System.Exception exception)
				{
					_003C_003E1__state = -2;
					((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetStateMachine(stateMachine);
			}
		}

		public MyRvLinkDeviceTracker _003C_003E4__this;

		public CancellationToken commandCancellationToken;

		[AsyncStateMachine(typeof(_003C_003CGetDevicesIfNeeded_003Eb__0_003Ed))]
		internal global::System.Threading.Tasks.Task? _003CGetDevicesIfNeeded_003Eb__0()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_003C_003CGetDevicesIfNeeded_003Eb__0_003Ed _003C_003CGetDevicesIfNeeded_003Eb__0_003Ed = default(_003C_003CGetDevicesIfNeeded_003Eb__0_003Ed);
			_003C_003CGetDevicesIfNeeded_003Eb__0_003Ed._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
			_003C_003CGetDevicesIfNeeded_003Eb__0_003Ed._003C_003E4__this = this;
			_003C_003CGetDevicesIfNeeded_003Eb__0_003Ed._003C_003E1__state = -1;
			((AsyncTaskMethodBuilder)(ref _003C_003CGetDevicesIfNeeded_003Eb__0_003Ed._003C_003Et__builder)).Start<_003C_003CGetDevicesIfNeeded_003Eb__0_003Ed>(ref _003C_003CGetDevicesIfNeeded_003Eb__0_003Ed);
			return ((AsyncTaskMethodBuilder)(ref _003C_003CGetDevicesIfNeeded_003Eb__0_003Ed._003C_003Et__builder)).Task;
		}
	}

	[CompilerGenerated]
	private sealed class _003CEnumerateLogicalDevices_003Ed__37 : global::System.Collections.Generic.IEnumerable<ValueTuple<byte, byte, ILogicalDevice>>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ValueTuple<byte, byte, ILogicalDevice>>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private ValueTuple<byte, byte, ILogicalDevice> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MyRvLinkDeviceTracker _003C_003E4__this;

		private Func<ILogicalDevice, bool> filter;

		public Func<ILogicalDevice, bool> _003C_003E3__filter;

		private List<IMyRvLinkDevice> _003CdeviceList_003E5__2;

		private byte _003CdeviceId_003E5__3;

		ValueTuple<byte, byte, ILogicalDevice> global::System.Collections.Generic.IEnumerator<ValueTuple<byte, byte, ILogicalDevice>>.Current
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
		public _003CEnumerateLogicalDevices_003Ed__37(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void global::System.IDisposable.Dispose()
		{
			_003CdeviceList_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			MyRvLinkDeviceTracker myRvLinkDeviceTracker = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00b0;
			}
			_003C_003E1__state = -1;
			_003CdeviceList_003E5__2 = myRvLinkDeviceTracker.DeviceList;
			if (!myRvLinkDeviceTracker.IsActive || _003CdeviceList_003E5__2 == null)
			{
				return false;
			}
			_003CdeviceId_003E5__3 = 0;
			goto IL_00bf;
			IL_00b0:
			_003CdeviceId_003E5__3++;
			goto IL_00bf;
			IL_00bf:
			if (_003CdeviceId_003E5__3 < _003CdeviceList_003E5__2.Count)
			{
				if (_003CdeviceList_003E5__2[(int)_003CdeviceId_003E5__3] is IMyRvLinkDeviceForLogicalDevice device)
				{
					ILogicalDevice logicalDevice = myRvLinkDeviceTracker._myRvLinkDeviceManager.GetLogicalDevice(device);
					if (logicalDevice != null && !((ICommonDisposable)logicalDevice).IsDisposed && filter.Invoke(logicalDevice))
					{
						_003C_003E2__current = new ValueTuple<byte, byte, ILogicalDevice>(myRvLinkDeviceTracker.DeviceTableId, _003CdeviceId_003E5__3, logicalDevice);
						_003C_003E1__state = 1;
						return true;
					}
				}
				goto IL_00b0;
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
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, byte, ILogicalDevice>> global::System.Collections.Generic.IEnumerable<ValueTuple<byte, byte, ILogicalDevice>>.GetEnumerator()
		{
			_003CEnumerateLogicalDevices_003Ed__37 _003CEnumerateLogicalDevices_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CEnumerateLogicalDevices_003Ed__ = this;
			}
			else
			{
				_003CEnumerateLogicalDevices_003Ed__ = new _003CEnumerateLogicalDevices_003Ed__37(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CEnumerateLogicalDevices_003Ed__.filter = _003C_003E3__filter;
			return _003CEnumerateLogicalDevices_003Ed__;
		}

		[DebuggerHidden]
		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<ValueTuple<byte, byte, ILogicalDevice>>)this).GetEnumerator();
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDevicesAsync_003Ed__30 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<List<IMyRvLinkDevice>> _003C_003Et__builder;

		public MyRvLinkDeviceTracker _003C_003E4__this;

		public CancellationToken cancellationToken;

		private MyRvLinkCommandGetDevices _003CcommandGetDevices_003E5__2;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			MyRvLinkDeviceTracker myRvLinkDeviceTracker = _003C_003E4__this;
			List<IMyRvLinkDevice> result2;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					_003CcommandGetDevices_003E5__2 = new MyRvLinkCommandGetDevices(myRvLinkDeviceTracker.MyRvLinkService.GetNextCommandId(), myRvLinkDeviceTracker.DeviceTableId, 0, 255);
					val = myRvLinkDeviceTracker.MyRvLinkService.SendCommandAsync(_003CcommandGetDevices_003E5__2, cancellationToken, MyRvLinkSendCommandOption.None).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CGetDevicesAsync_003Ed__30>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
					num = (_003C_003E1__state = -1);
				}
				IMyRvLinkCommandResponse result = val.GetResult();
				if (result is MyRvLinkCommandResponseFailure)
				{
					throw new MyRvLinkException($"Failed {result}");
				}
				if (_003CcommandGetDevices_003E5__2.ResponseState == MyRvLinkResponseState.Failed)
				{
					throw new MyRvLinkException($"Get Device Command Failed {_003CcommandGetDevices_003E5__2.ResponseState}");
				}
				if (_003CcommandGetDevices_003E5__2.ResponseReceivedDeviceTableCrc != myRvLinkDeviceTracker.DeviceTableCrc)
				{
					throw new MyRvLinkException("Response didn't match expected Device Table CRC, discarding response");
				}
				List<IMyRvLinkDevice> devices = _003CcommandGetDevices_003E5__2.Devices;
				if (devices == null || devices.Count == 0)
				{
					throw new MyRvLinkException("No Devices Found");
				}
				result2 = devices;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003CcommandGetDevices_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003CcommandGetDevices_003E5__2 = null;
			_003C_003Et__builder.SetResult(result2);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	private const string LogTag = "MyRvLinkDeviceTracker";

	private string LogPrefix;

	private MyRvLinkDeviceMetadataTracker? _deviceMetadataTracker;

	private readonly MyRvLinkDeviceManager _myRvLinkDeviceManager;

	private CancellationTokenSource? _commandGetDevicesTcs;

	private IN_MOTION_LOCKOUT_LEVEL _cachedInMotionLockoutLevel = IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)0);

	[field: CompilerGenerated]
	public IDirectConnectionMyRvLink MyRvLinkService
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public byte DeviceTableId
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	public uint DeviceTableCrc
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public List<IMyRvLinkDevice> DeviceList
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	} = new List<IMyRvLinkDevice>();

	public bool IsActive
	{
		get
		{
			if (!((CommonDisposable)this).IsDisposed && DeviceTableCrc == MyRvLinkService.GatewayInfo?.DeviceTableCrc && MyRvLinkService.IsFirmwareVersionSupported)
			{
				return ((ILogicalDeviceSourceConnection)MyRvLinkService).IsConnected;
			}
			return false;
		}
	}

	[field: CompilerGenerated]
	public bool IsDeviceLoadComplete
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public IN_MOTION_LOCKOUT_LEVEL CachedInMotionLockoutLevel
	{
		get
		{
			if (!IsActive)
			{
				return IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)0);
			}
			return _cachedInMotionLockoutLevel;
		}
		set
		{
			if (_cachedInMotionLockoutLevel != value)
			{
				_cachedInMotionLockoutLevel = value;
				((ILogicalDeviceSourceDirect)MyRvLinkService).DeviceService.UpdateInMotionLockoutLevel();
			}
		}
	}

	public MyRvLinkDeviceTracker(IDirectConnectionMyRvLink myRvLinkService, byte deviceTableId, uint deviceTableCrc)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		MyRvLinkService = myRvLinkService ?? throw new ArgumentNullException("Invalid IMyRvLinkService", "myRvLinkService");
		LogPrefix = MyRvLinkService.LogPrefix;
		if (myRvLinkService.GatewayInfo == null)
		{
			throw new ArgumentNullException("Invalid GatewayInfo", "GatewayInfo");
		}
		DeviceTableId = deviceTableId;
		DeviceTableCrc = deviceTableCrc;
		TaggedLog.Debug("MyRvLinkDeviceTracker", $"{LogPrefix} CREATED Device Tracker for {myRvLinkService} with TagList: {LogicalDeviceTagManager.DebugTagsAsString((global::System.Collections.Generic.IEnumerable<ILogicalDeviceTag>)((ILogicalDeviceSourceConnection)MyRvLinkService).ConnectionTagList)}", global::System.Array.Empty<object>());
		_myRvLinkDeviceManager = new MyRvLinkDeviceManager(((ILogicalDeviceSourceDirect)MyRvLinkService).DeviceService, MyRvLinkService);
	}

	public void UpdateDeviceIdIfNeeded(byte deviceTableId, uint deviceTableCrc)
	{
		if (!((CommonDisposable)this).IsDisposed && DeviceTableCrc == deviceTableCrc && DeviceTableId != deviceTableId)
		{
			TaggedLog.Debug("MyRvLinkDeviceTracker", $"{LogPrefix} Updated Device Table Id from {DeviceTableId} to {deviceTableId} for Device Table CRC 0x{deviceTableCrc:X}", global::System.Array.Empty<object>());
			DeviceTableId = deviceTableId;
		}
	}

	public bool IsLogicalDeviceOnline(ILogicalDevice? logicalDevice)
	{
		if (IsActive && logicalDevice != null && ((ILogicalDeviceSourceDirect)MyRvLinkService).IsLogicalDeviceSupported(logicalDevice))
		{
			return _myRvLinkDeviceManager.IsLogicalDeviceOnline((logicalDevice != null) ? logicalDevice.LogicalId : null);
		}
		return false;
	}

	public IN_MOTION_LOCKOUT_LEVEL GetLogicalDeviceInTransitLockoutLevel(ILogicalDevice? logicalDevice)
	{
		if (!IsActive || logicalDevice == null || !((ILogicalDeviceSourceDirect)MyRvLinkService).IsLogicalDeviceSupported(logicalDevice))
		{
			return IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)0);
		}
		return _myRvLinkDeviceManager.GetInTransitLockoutLevel((logicalDevice != null) ? logicalDevice.LogicalId : null);
	}

	public void GetDevicesIfNeeded()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		_003C_003Ec__DisplayClass29_0 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass29_0();
		CS_0024_003C_003E8__locals4._003C_003E4__this = this;
		if (!IsActive)
		{
			CancellationTokenSource? commandGetDevicesTcs = _commandGetDevicesTcs;
			if (commandGetDevicesTcs != null)
			{
				CancellationTokenSourceExtension.TryCancelAndDispose(commandGetDevicesTcs);
			}
		}
		else if (DeviceList.Count <= 0 && _commandGetDevicesTcs == null)
		{
			CancellationTokenSource val = new CancellationTokenSource();
			CancellationTokenSource obj = Interlocked.Exchange<CancellationTokenSource>(ref _commandGetDevicesTcs, val);
			if (obj != null)
			{
				CancellationTokenSourceExtension.TryCancelAndDispose(obj);
			}
			CS_0024_003C_003E8__locals4.commandCancellationToken = val.Token;
			global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(_003C_003Ec__DisplayClass29_0._003C_003CGetDevicesIfNeeded_003Eb__0_003Ed))] () =>
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				_003C_003Ec__DisplayClass29_0._003C_003CGetDevicesIfNeeded_003Eb__0_003Ed _003C_003CGetDevicesIfNeeded_003Eb__0_003Ed = default(_003C_003Ec__DisplayClass29_0._003C_003CGetDevicesIfNeeded_003Eb__0_003Ed);
				_003C_003CGetDevicesIfNeeded_003Eb__0_003Ed._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
				_003C_003CGetDevicesIfNeeded_003Eb__0_003Ed._003C_003E4__this = CS_0024_003C_003E8__locals4;
				_003C_003CGetDevicesIfNeeded_003Eb__0_003Ed._003C_003E1__state = -1;
				((AsyncTaskMethodBuilder)(ref _003C_003CGetDevicesIfNeeded_003Eb__0_003Ed._003C_003Et__builder)).Start<_003C_003Ec__DisplayClass29_0._003C_003CGetDevicesIfNeeded_003Eb__0_003Ed>(ref _003C_003CGetDevicesIfNeeded_003Eb__0_003Ed);
				return ((AsyncTaskMethodBuilder)(ref _003C_003CGetDevicesIfNeeded_003Eb__0_003Ed._003C_003Et__builder)).Task;
			}), CS_0024_003C_003E8__locals4.commandCancellationToken);
		}
	}

	[AsyncStateMachine(typeof(_003CGetDevicesAsync_003Ed__30))]
	private async global::System.Threading.Tasks.Task<List<IMyRvLinkDevice>> GetDevicesAsync(CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		MyRvLinkCommandGetDevices commandGetDevices = new MyRvLinkCommandGetDevices(MyRvLinkService.GetNextCommandId(), DeviceTableId, 0, 255);
		IMyRvLinkCommandResponse myRvLinkCommandResponse = await MyRvLinkService.SendCommandAsync(commandGetDevices, cancellationToken, MyRvLinkSendCommandOption.None);
		if (myRvLinkCommandResponse is MyRvLinkCommandResponseFailure)
		{
			throw new MyRvLinkException($"Failed {myRvLinkCommandResponse}");
		}
		if (commandGetDevices.ResponseState == MyRvLinkResponseState.Failed)
		{
			throw new MyRvLinkException($"Get Device Command Failed {commandGetDevices.ResponseState}");
		}
		if (commandGetDevices.ResponseReceivedDeviceTableCrc != DeviceTableCrc)
		{
			throw new MyRvLinkException("Response didn't match expected Device Table CRC, discarding response");
		}
		List<IMyRvLinkDevice> devices = commandGetDevices.Devices;
		if (devices == null || devices.Count == 0)
		{
			throw new MyRvLinkException("No Devices Found");
		}
		return devices;
	}

	public void UpdateMetadataIfNeeded(uint deviceMetadataTableCrc)
	{
		if (deviceMetadataTableCrc != _deviceMetadataTracker?.DeviceMetadataTableCrc)
		{
			MyRvLinkDeviceMetadataTracker? deviceMetadataTracker = _deviceMetadataTracker;
			if (deviceMetadataTracker != null)
			{
				((CommonDisposable)deviceMetadataTracker).TryDispose();
			}
			_deviceMetadataTracker = null;
		}
		if (_deviceMetadataTracker == null)
		{
			_deviceMetadataTracker = new MyRvLinkDeviceMetadataTracker(this, deviceMetadataTableCrc);
		}
		GetDevicesMetadataIfNeeded();
	}

	public void GetDevicesMetadataIfNeeded()
	{
		_deviceMetadataTracker?.GetDevicesMetadataIfNeeded();
	}

	public void UpdateMetadata(List<IMyRvLinkDeviceMetadata> deviceMetadataList, uint deviceMetadataTableCrc)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (!IsActive)
		{
			return;
		}
		if (deviceMetadataList.Count != deviceList.Count)
		{
			TaggedLog.Error("MyRvLinkDeviceTracker", LogPrefix + " Unable to update Devices with their metadata because the number of devices don't match.", global::System.Array.Empty<object>());
			return;
		}
		for (int i = 0; i < deviceMetadataList.Count; i++)
		{
			IMyRvLinkDevice myRvLinkDevice = deviceList[i];
			if (!(myRvLinkDevice is MyRvLinkDeviceHost myRvLinkDeviceHost))
			{
				if (!(myRvLinkDevice is MyRvLinkDeviceIdsCan myRvLinkDeviceIdsCan))
				{
					continue;
				}
				if (!(deviceMetadataList[i] is MyRvLinkDeviceIdsCanMetadata myRvLinkDeviceIdsCanMetadata))
				{
					TaggedLog.Error("MyRvLinkDeviceTracker", LogPrefix + " Unable to update IDS CAN device meta-data because metadata is of the incorrect type", global::System.Array.Empty<object>());
					continue;
				}
				myRvLinkDeviceIdsCan.UpdateMetadata(myRvLinkDeviceIdsCanMetadata);
				ILogicalDevice? obj = _myRvLinkDeviceManager.AddLogicalDevice(myRvLinkDeviceIdsCan);
				if (obj != null)
				{
					obj.UpdateCircuitId(CIRCUIT_ID.op_Implicit(myRvLinkDeviceIdsCanMetadata.CircuitId));
				}
			}
			else if (!(deviceMetadataList[i] is MyRvLinkDeviceHostMetadata myRvLinkDeviceHostMetadata))
			{
				TaggedLog.Error("MyRvLinkDeviceTracker", LogPrefix + " Unable to update host device meta-data because metadata is of the incorrect type", global::System.Array.Empty<object>());
			}
			else
			{
				myRvLinkDeviceHost.UpdateMetadata(myRvLinkDeviceHostMetadata);
				ILogicalDevice? obj2 = _myRvLinkDeviceManager.AddLogicalDevice(myRvLinkDeviceHost);
				if (obj2 != null)
				{
					obj2.UpdateCircuitId(CIRCUIT_ID.op_Implicit(myRvLinkDeviceHostMetadata.CircuitId));
				}
			}
		}
		TaggedLog.Information("MyRvLinkDeviceTracker", $"{LogPrefix} DeviceTableId: 0x{DeviceTableId:X}", global::System.Array.Empty<object>());
		for (int j = 0; j < deviceMetadataList.Count; j++)
		{
			IMyRvLinkDevice myRvLinkDevice2 = deviceList[j];
			if (!(myRvLinkDevice2 is MyRvLinkDeviceHost myRvLinkDeviceHost2))
			{
				if (myRvLinkDevice2 is MyRvLinkDeviceIdsCan myRvLinkDeviceIdsCan2)
				{
					if (!(deviceMetadataList[j] is MyRvLinkDeviceIdsCanMetadata metadata))
					{
						TaggedLog.Information("MyRvLinkDeviceTracker", $"{LogPrefix} DeviceId 0x{j:X2}: NON-IDS CAN Metadata not fully loaded", global::System.Array.Empty<object>());
					}
					else
					{
						myRvLinkDeviceIdsCan2.UpdateMetadata(metadata);
						TaggedLog.Information("MyRvLinkDeviceTracker", $"{LogPrefix} DeviceId 0x{j:X2}: IDS CAN Device {myRvLinkDeviceIdsCan2}", global::System.Array.Empty<object>());
					}
				}
				else
				{
					TaggedLog.Information("MyRvLinkDeviceTracker", $"{LogPrefix} DeviceId 0x{j:X2}: Unknown Device Type", global::System.Array.Empty<object>());
				}
			}
			else if (!(deviceMetadataList[j] is MyRvLinkDeviceHostMetadata metadata2))
			{
				TaggedLog.Information("MyRvLinkDeviceTracker", $"{LogPrefix} DeviceId 0x{j:X2}: HOST Metadata not fully loaded", global::System.Array.Empty<object>());
			}
			else
			{
				myRvLinkDeviceHost2.UpdateMetadata(metadata2);
				TaggedLog.Information("MyRvLinkDeviceTracker", $"{LogPrefix} DeviceId 0x{j:X2}: HOST Device {myRvLinkDeviceHost2}", global::System.Array.Empty<object>());
			}
		}
		IsDeviceLoadComplete = true;
	}

	public byte? GetMyRvDeviceIdFromLogicalDevice(ILogicalDevice logicalDevice)
	{
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (!IsActive || deviceList == null || logicalDevice == null || ((ICommonDisposable)logicalDevice).IsDisposed)
		{
			return null;
		}
		for (byte b = 0; b < deviceList.Count; b++)
		{
			if (deviceList[(int)b] is IMyRvLinkDeviceForLogicalDevice device && _myRvLinkDeviceManager.GetLogicalDevice(device) == logicalDevice)
			{
				return b;
			}
		}
		return null;
	}

	public IMyRvLinkDeviceForLogicalDevice? GetMyRvDeviceFromLogicalDevice(ILogicalDevice logicalDevice)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (!IsActive || deviceList == null || logicalDevice == null || ((ICommonDisposable)logicalDevice).IsDisposed)
		{
			return null;
		}
		Enumerator<IMyRvLinkDevice> enumerator = deviceList.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is IMyRvLinkDeviceForLogicalDevice myRvLinkDeviceForLogicalDevice && _myRvLinkDeviceManager.GetLogicalDevice(myRvLinkDeviceForLogicalDevice) == logicalDevice)
				{
					return myRvLinkDeviceForLogicalDevice;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
		return null;
	}

	public ILogicalDevice? GetLogicalDeviceFromMyRvDevice(byte deviceTableId, byte deviceId)
	{
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceTableId != DeviceTableId || deviceList == null)
		{
			return null;
		}
		IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
		if (!Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)deviceId, ref myRvLinkDevice))
		{
			return null;
		}
		if (!(myRvLinkDevice is IMyRvLinkDeviceForLogicalDevice device))
		{
			return null;
		}
		return _myRvLinkDeviceManager.GetLogicalDevice(device);
	}

	[IteratorStateMachine(typeof(_003CEnumerateLogicalDevices_003Ed__37))]
	public global::System.Collections.Generic.IEnumerable<ValueTuple<byte, byte, ILogicalDevice>> EnumerateLogicalDevices(Func<ILogicalDevice, bool> filter)
	{
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (!IsActive || deviceList == null)
		{
			yield break;
		}
		for (byte deviceId = 0; deviceId < deviceList.Count; deviceId++)
		{
			if (deviceList[(int)deviceId] is IMyRvLinkDeviceForLogicalDevice device)
			{
				ILogicalDevice logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
				if (logicalDevice != null && !((ICommonDisposable)logicalDevice).IsDisposed && filter.Invoke(logicalDevice))
				{
					yield return new ValueTuple<byte, byte, ILogicalDevice>(DeviceTableId, deviceId, logicalDevice);
				}
			}
		}
	}

	internal void TakeDevicesOfflineIfNeeded()
	{
		_myRvLinkDeviceManager.TakeDevicesOfflineIfNeeded(forceOffline: false);
	}

	public void RemoveOfflineDevices()
	{
		_myRvLinkDeviceManager.RemoveOfflineDevices();
	}

	internal void RemoveInTransitLockoutLevel()
	{
		_myRvLinkDeviceManager.RemoveInTransitLockoutLevel(forceRemoveLockout: false);
	}

	public override void Dispose(bool disposing)
	{
		MyRvLinkDeviceMetadataTracker? deviceMetadataTracker = _deviceMetadataTracker;
		if (deviceMetadataTracker != null)
		{
			((CommonDisposable)deviceMetadataTracker).TryDispose();
		}
		_deviceMetadataTracker = null;
		CancellationTokenSource? commandGetDevicesTcs = _commandGetDevicesTcs;
		if (commandGetDevicesTcs != null)
		{
			CancellationTokenSourceExtension.TryCancelAndDispose(commandGetDevicesTcs);
		}
		_myRvLinkDeviceManager.TakeDevicesOfflineIfNeeded(forceOffline: true);
		_myRvLinkDeviceManager.RemoveInTransitLockoutLevel(forceRemoveLockout: true);
		TaggedLog.Debug("MyRvLinkDeviceTracker", $"{"MyRvLinkDeviceTracker"} DISPOSED: {this}", global::System.Array.Empty<object>());
	}

	public override string ToString()
	{
		return $"DeviceTableId: {DeviceTableId} Crc: {DeviceTableCrc} IsDisposed: {((CommonDisposable)this).IsDisposed}";
	}

	internal void UpdateAccessoryGatewayStatus(MyRvLinkAccessoryGatewayStatus accessoryGatewayStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || accessoryGatewayStatus == null || accessoryGatewayStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceAccessoryGatewayStatus>> enumerator = accessoryGatewayStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceAccessoryGatewayStatus> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceAccessoryGatewayStatus item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceAccessoryGateway val = (ILogicalDeviceAccessoryGateway)(object)((logicalDevice is ILogicalDeviceAccessoryGateway) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateAccessoryGatewayStatus"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceAccessoryGateway"}", global::System.Array.Empty<object>());
					}
					else if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceAccessoryGatewayStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Size);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateAutoOperationProgressStatus(MyRvLinkLevelerType5ExtendedStatus progressStatus)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || progressStatus == null)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5>> enumerator = progressStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, ILogicalDeviceLevelerStatusExtendedType5> current = enumerator.Current;
				byte item = current.Item1;
				ILogicalDeviceLevelerStatusExtendedType5 item2 = current.Item2;
				if (!Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) || !(myRvLinkDevice is MyRvLinkDeviceIdsCan device))
				{
					continue;
				}
				ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
				ILogicalDeviceDirectLevelerType5 val = (ILogicalDeviceDirectLevelerType5)(object)((logicalDevice is ILogicalDeviceDirectLevelerType5) ? logicalDevice : null);
				if (val != null)
				{
					if (!(item2 is LogicalDeviceLevelerStatusExtendedType5AutoStep) || !((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceLevelerType5)val).DeviceStatusExtendedAutoStep).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(LogicalDeviceLevelerStatusExtendedType5AutoStep)item2))
					{
						if (item2 is LogicalDeviceLevelerStatusExtendedType5JackStrokeLength)
						{
							((ILogicalDeviceWithStatusExtended)val).UpdateDeviceStatusExtended((global::System.Collections.Generic.IReadOnlyList<byte>)((IDeviceDataPacketMutable)item2).Data, ((IDeviceDataPacket)item2).Size, (byte)1, (global::System.DateTime?)null);
						}
						else
						{
							((ILogicalDeviceWithStatusExtended)val).UpdateDeviceStatusExtended((global::System.Collections.Generic.IReadOnlyList<byte>)((IDeviceDataPacketMutable)item2).Data, ((IDeviceDataPacket)item2).Size, (byte)0, (global::System.DateTime?)null);
						}
					}
				}
				else
				{
					TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateAutoOperationProgressStatus"} Unable to update Auto Operation Progress status to {item2} because the logical device was unexpected for {this}.", global::System.Array.Empty<object>());
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateAwningSensorStatus(MyRvLinkAwningSensorStatus awningSensorStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || awningSensorStatus == null || awningSensorStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceAwningSensorStatus>> enumerator = awningSensorStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceAwningSensorStatus> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceAwningSensorStatus item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceAwningSensor val = (ILogicalDeviceAwningSensor)(object)((logicalDevice is ILogicalDeviceAwningSensor) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateAwningSensorStatus"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceAwningSensor"}", global::System.Array.Empty<object>());
					}
					else if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceAwningSensorStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Size);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateBatteryMonitorStatus(MyRvLinkBatteryMonitorStatus batteryMonitorStatus)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
		if (deviceList == null || batteryMonitorStatus == null || batteryMonitorStatus.DeviceTableId != DeviceTableId || !Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, batteryMonitorStatus.DeviceId, ref myRvLinkDevice) || !(myRvLinkDevice is MyRvLinkDeviceIdsCan device))
		{
			return;
		}
		ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
		ILogicalDeviceBatteryMonitor val = (ILogicalDeviceBatteryMonitor)(object)((logicalDevice is ILogicalDeviceBatteryMonitor) ? logicalDevice : null);
		if (val == null)
		{
			TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateBatteryMonitorStatus"} Unable to update status to {batteryMonitorStatus} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceBatteryMonitor"}", global::System.Array.Empty<object>());
			return;
		}
		ValueTuple<LogicalDeviceBatteryMonitorStatus, LogicalDeviceBatteryMonitorStatusExtended> statusAndExtendedStatus = batteryMonitorStatus.GetStatusAndExtendedStatus();
		LogicalDeviceBatteryMonitorStatus item = statusAndExtendedStatus.Item1;
		LogicalDeviceBatteryMonitorStatusExtended item2 = statusAndExtendedStatus.Item2;
		if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceBatteryMonitorStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item))
		{
			((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)item).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)item).Size);
		}
		ILogicalDeviceWithStatusExtended val2 = (ILogicalDeviceWithStatusExtended)(object)val;
		if (val2 != null && !((object)val2.RawDeviceStatusExtended).Equals((object)item2))
		{
			val2.UpdateDeviceStatusExtended((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Data, (uint)((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Data.Length, ((LogicalDeviceStatusPacketMutableExtended)item2).ExtendedByte, (global::System.DateTime?)null);
		}
	}

	internal void UpdateBootLoaderStatus(MyRvLinkBootLoaderStatus bootLoaderStatus)
	{
		List<IMyRvLinkDevice> deviceList = DeviceList;
		IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
		if (deviceList == null || bootLoaderStatus == null || bootLoaderStatus.DeviceTableId != DeviceTableId || !Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, bootLoaderStatus.DeviceId, ref myRvLinkDevice) || !(myRvLinkDevice is MyRvLinkDeviceIdsCan device))
		{
			return;
		}
		ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
		ILogicalDeviceReflashBootloader val = (ILogicalDeviceReflashBootloader)(object)((logicalDevice is ILogicalDeviceReflashBootloader) ? logicalDevice : null);
		if (val == null)
		{
			TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateBootLoaderStatus"} Unable to update status to {bootLoaderStatus} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceReflashBootloader"}", global::System.Array.Empty<object>());
		}
		else
		{
			LogicalDeviceReflashBootLoaderStatus status = bootLoaderStatus.GetStatus();
			if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceReflashBootLoaderStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)status))
			{
				((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)status).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)status).Size);
			}
		}
	}

	internal void UpdateBrakingSystemStatus(MyRvLinkBrakingSystemStatus absStatus)
	{
		List<IMyRvLinkDevice> deviceList = DeviceList;
		IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
		if (deviceList == null || absStatus.DeviceTableId != DeviceTableId || !Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)absStatus.DeviceId, ref myRvLinkDevice) || !(myRvLinkDevice is MyRvLinkDeviceIdsCan device))
		{
			return;
		}
		ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
		ILogicalDeviceBrakingSystem val = (ILogicalDeviceBrakingSystem)(object)((logicalDevice is ILogicalDeviceBrakingSystem) ? logicalDevice : null);
		if (val == null)
		{
			TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateBrakingSystemStatus"} Unable to update status to {absStatus} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceBrakingSystem"}", global::System.Array.Empty<object>());
		}
		else
		{
			LogicalDeviceBrakingSystemStatus brakingSystemStatus = absStatus.GetBrakingSystemStatus();
			if (brakingSystemStatus != null && !((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceBrakingSystemStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)brakingSystemStatus))
			{
				((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)brakingSystemStatus).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)brakingSystemStatus).Size);
			}
		}
	}

	internal void UpdateCloudGatewayStatus(MyRvLinkCloudGatewayStatus cloudGatewayStatus)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || cloudGatewayStatus == null)
		{
			TaggedLog.Debug("MyRvLinkDeviceTracker", "Ignoring MyRvLinkDeviceTracker no device list or TableId's don't match", global::System.Array.Empty<object>());
		}
		else
		{
			if (cloudGatewayStatus.DeviceTableId != DeviceTableId)
			{
				return;
			}
			global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState>> enumerator = cloudGatewayStatus.EnumerateStatus().GetEnumerator();
			try
			{
				IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					ValueTuple<byte, LogicalDeviceCloudGatewayStatus, SoftwareUpdateState> current = enumerator.Current;
					byte item = current.Item1;
					LogicalDeviceCloudGatewayStatus item2 = current.Item2;
					if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
					{
						ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
						ILogicalDeviceCloudGateway val = (ILogicalDeviceCloudGateway)(object)((logicalDevice is ILogicalDeviceCloudGateway) ? logicalDevice : null);
						if (val == null)
						{
							TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateGeneratorGenieStatus"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceRemoteCloudGateway"}", global::System.Array.Empty<object>());
							break;
						}
						if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceCloudGatewayStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
						{
							((ILogicalDeviceWithStatusUpdate<LogicalDeviceCloudGatewayStatus>)(object)val).UpdateDeviceStatus(item2);
						}
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
		}
	}

	internal void UpdateDimmableLightStatus(MyRvLinkDimmableLightStatus dimmableLightStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || dimmableLightStatus == null || dimmableLightStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceLightDimmableStatus>> enumerator = dimmableLightStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceLightDimmableStatus> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceLightDimmableStatus item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceRemoteLightDimmable val = (ILogicalDeviceRemoteLightDimmable)(object)((logicalDevice is ILogicalDeviceRemoteLightDimmable) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateRgbLightStatus"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceRemoteLightDimmable"}", global::System.Array.Empty<object>());
						break;
					}
					if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceLightDimmableStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatusUpdate<LogicalDeviceLightDimmableStatus>)(object)val).UpdateDeviceStatus(item2);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateDimmableLightExtended(MyRvLinkDimmableLightStatusExtended dimmableLightStatusExtended)
	{
		List<IMyRvLinkDevice> deviceList = DeviceList;
		IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
		if (deviceList == null || dimmableLightStatusExtended == null || dimmableLightStatusExtended.DeviceTableId != DeviceTableId || !Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, dimmableLightStatusExtended.DeviceId, ref myRvLinkDevice) || !(myRvLinkDevice is MyRvLinkDeviceIdsCan device))
		{
			return;
		}
		ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
		ILogicalDeviceLightDimmable val = (ILogicalDeviceLightDimmable)(object)((logicalDevice is ILogicalDeviceLightDimmable) ? logicalDevice : null);
		if (val == null)
		{
			TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateDimmableLightExtended"} Unable to update status to {dimmableLightStatusExtended} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceLightDimmable"}", global::System.Array.Empty<object>());
		}
		else
		{
			LogicalDeviceLightDimmableStatusExtended extendedStatus = dimmableLightStatusExtended.GetExtendedStatus();
			if (!((object)((ILogicalDeviceWithStatusExtended)val).RawDeviceStatusExtended).Equals((object)extendedStatus))
			{
				((ILogicalDeviceWithStatusExtended)val).UpdateDeviceStatusExtended((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)extendedStatus).Data, (uint)((LogicalDeviceDataPacketMutableDoubleBuffer)extendedStatus).Data.Length, ((LogicalDeviceStatusPacketMutableExtended)extendedStatus).ExtendedByte, (global::System.DateTime?)null);
			}
		}
	}

	internal void UpdateDoorLockStatus(MyRvLinkDoorLockStatus doorLockStatus)
	{
		List<IMyRvLinkDevice> deviceList = DeviceList;
		IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
		if (deviceList == null || doorLockStatus == null || doorLockStatus.DeviceTableId != DeviceTableId || !Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, doorLockStatus.DeviceId, ref myRvLinkDevice) || !(myRvLinkDevice is MyRvLinkDeviceIdsCan device))
		{
			return;
		}
		ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
		ILogicalDeviceDoorLock val = (ILogicalDeviceDoorLock)(object)((logicalDevice is ILogicalDeviceDoorLock) ? logicalDevice : null);
		if (val == null)
		{
			TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateDoorLockStatus"} Unable to update status to {doorLockStatus} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceDoorLock"}", global::System.Array.Empty<object>());
		}
		else
		{
			LogicalDeviceDoorLockStatus status = doorLockStatus.GetStatus();
			if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceDoorLockStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)status))
			{
				((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)status).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)status).Size);
			}
		}
	}

	internal void UpdateGeneratorGenieStatus(MyRvLinkGeneratorGenieStatus generatorGenieStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || generatorGenieStatus == null || generatorGenieStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceGeneratorGenieStatus>> enumerator = generatorGenieStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceGeneratorGenieStatus> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceGeneratorGenieStatus item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceGeneratorGenieDirect val = (ILogicalDeviceGeneratorGenieDirect)(object)((logicalDevice is ILogicalDeviceGeneratorGenieDirect) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateGeneratorGenieStatus"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceGeneratorGenieRemote"}", global::System.Array.Empty<object>());
						break;
					}
					if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceGeneratorGenieStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatusUpdate<LogicalDeviceGeneratorGenieStatus>)(object)val).UpdateDeviceStatus(item2);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateHourMeterStatus(MyRvLinkHourMeterStatus? tankSensorStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || tankSensorStatus == null || tankSensorStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceHourMeterStatus>> enumerator = tankSensorStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceHourMeterStatus> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceHourMeterStatus item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceHourMeterDirect val = (ILogicalDeviceHourMeterDirect)(object)((logicalDevice is ILogicalDeviceHourMeterDirect) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateHourMeterStatus"} Unable to update status to {tankSensorStatus} because can't find logical device for {this}", global::System.Array.Empty<object>());
					}
					else
					{
						((ILogicalDeviceWithStatusUpdate<LogicalDeviceHourMeterStatus>)(object)val).UpdateDeviceStatus(item2);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateHvacStatus(MyRvLinkHvacStatus hvacStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || hvacStatus == null || hvacStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx>> enumerator = hvacStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceClimateZoneStatus, LogicalDeviceClimateZoneStatusEx> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceClimateZoneStatus item2 = current.Item2;
				LogicalDeviceClimateZoneStatusEx item3 = current.Item3;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceClimateZoneDirect val = (ILogicalDeviceClimateZoneDirect)(object)((logicalDevice is ILogicalDeviceClimateZoneDirect) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateHvacStatus"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceClimateZoneRemote"}", global::System.Array.Empty<object>());
						break;
					}
					if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceClimateZoneStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatusUpdate<LogicalDeviceClimateZoneStatus>)(object)val).UpdateDeviceStatus(item2);
					}
					ILogicalDeviceWithStatusExtended val2 = (ILogicalDeviceWithStatusExtended)(object)((val is ILogicalDeviceWithStatusExtended) ? val : null);
					if (val2 != null && !((object)val2.RawDeviceStatusExtended).Equals((object)item3))
					{
						val2.UpdateDeviceStatusExtended((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)item3).Data, (uint)((LogicalDeviceDataPacketMutableDoubleBuffer)item3).Data.Length, ((LogicalDeviceStatusPacketMutableExtended)item3).ExtendedByte, (global::System.DateTime?)null);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateJaycoTbbStatus(MyRvLinkJaycoTbbStatus jaycoTbbStatus)
	{
		List<IMyRvLinkDevice> deviceList = DeviceList;
		IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
		if (deviceList == null || jaycoTbbStatus == null || jaycoTbbStatus.DeviceTableId != DeviceTableId || !Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, jaycoTbbStatus.DeviceId, ref myRvLinkDevice) || !(myRvLinkDevice is MyRvLinkDeviceIdsCan device))
		{
			return;
		}
		ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
		ILogicalDeviceJaycoTbb val = (ILogicalDeviceJaycoTbb)(object)((logicalDevice is ILogicalDeviceJaycoTbb) ? logicalDevice : null);
		if (val == null)
		{
			TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateJaycoTbbStatus"} Unable to update status to {jaycoTbbStatus} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceJaycoTbb"}", global::System.Array.Empty<object>());
		}
		else
		{
			LogicalDeviceJaycoTbbStatus status = jaycoTbbStatus.GetStatus();
			if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceJaycoTbbStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)status))
			{
				((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)status).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)status).Size);
			}
		}
	}

	internal void UpdateLeveler1Status(MyRvLinkLeveler1Status levelerStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || levelerStatus == null || levelerStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceLevelerStatusType1>> enumerator = levelerStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceLevelerStatusType1> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceLevelerStatusType1 item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceDirectLevelerType1 val = (ILogicalDeviceDirectLevelerType1)(object)((logicalDevice is ILogicalDeviceDirectLevelerType1) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateLeveler1Status"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceDirectLevelerType1"}", global::System.Array.Empty<object>());
					}
					else if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceLevelerStatusType1>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Size);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateLeveler3Status(MyRvLinkLeveler3Status levelerStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || levelerStatus == null || levelerStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceLevelerStatusType3>> enumerator = levelerStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceLevelerStatusType3> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceLevelerStatusType3 item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceDirectLevelerType3 val = (ILogicalDeviceDirectLevelerType3)(object)((logicalDevice is ILogicalDeviceDirectLevelerType3) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateLeveler3Status"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceDirectLevelerType3"}", global::System.Array.Empty<object>());
					}
					else if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceLevelerStatusType3>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Size);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateLeveler4Status(MyRvLinkLeveler4Status levelerStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || levelerStatus == null || levelerStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceLevelerStatusType4>> enumerator = levelerStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceLevelerStatusType4> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceLevelerStatusType4 item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceDirectLevelerType4 val = (ILogicalDeviceDirectLevelerType4)(object)((logicalDevice is ILogicalDeviceDirectLevelerType4) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateLeveler4Status"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceDirectLevelerType4"}", global::System.Array.Empty<object>());
					}
					else if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceLevelerStatusType4>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Size);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateLeveler5Status(MyRvLinkLeveler5Status levelerStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || levelerStatus == null || levelerStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceLevelerStatusType5>> enumerator = levelerStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceLevelerStatusType5> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceLevelerStatusType5 item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceDirectLevelerType5 val = (ILogicalDeviceDirectLevelerType5)(object)((logicalDevice is ILogicalDeviceDirectLevelerType5) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateLeveler5Status"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceDirectLevelerType5"}", global::System.Array.Empty<object>());
					}
					else if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceLevelerStatusType5>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Size);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateLevelerConsoleText(MyRvLinkLevelerConsoleText levelerConsoleText)
	{
		List<IMyRvLinkDevice> deviceList = DeviceList;
		IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
		if (deviceList == null || levelerConsoleText == null || levelerConsoleText.DeviceTableId != DeviceTableId || !Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)levelerConsoleText.DeviceId, ref myRvLinkDevice) || !(myRvLinkDevice is MyRvLinkDeviceIdsCan device))
		{
			return;
		}
		ILogicalDevice logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
		ILogicalDeviceLevelerType1 val = (ILogicalDeviceLevelerType1)(object)((logicalDevice is ILogicalDeviceLevelerType1) ? logicalDevice : null);
		if (val == null)
		{
			ILogicalDeviceLevelerType3 val2 = (ILogicalDeviceLevelerType3)(object)((logicalDevice is ILogicalDeviceLevelerType3) ? logicalDevice : null);
			if (val2 == null)
			{
				ILogicalDeviceLevelerType4 val3 = (ILogicalDeviceLevelerType4)(object)((logicalDevice is ILogicalDeviceLevelerType4) ? logicalDevice : null);
				if (val3 != null)
				{
					val3.UpdateDeviceConsoleText(levelerConsoleText.GetConsoleMessages());
					return;
				}
				TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateLevelerConsoleText"} Unable to update leveler console messages because can't find logical device for {this} OR device isn't a Leveler 3 or 4.", global::System.Array.Empty<object>());
			}
			else
			{
				val2.UpdateDeviceConsoleText(levelerConsoleText.GetConsoleMessages());
			}
		}
		else
		{
			val.UpdateDeviceConsoleText(levelerConsoleText.GetConsoleMessages());
		}
	}

	internal void UpdateLockStatus(MyRvLinkDeviceLockStatus? deviceLockStatus)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (DeviceList == null)
		{
			return;
		}
		if (deviceLockStatus == null)
		{
			_myRvLinkDeviceManager.RemoveInTransitLockoutLevel(forceRemoveLockout: true);
		}
		else
		{
			if (deviceLockStatus.DeviceTableId != DeviceTableId)
			{
				return;
			}
			byte systemLockoutLevel = deviceLockStatus.SystemLockoutLevel;
			CachedInMotionLockoutLevel = IN_MOTION_LOCKOUT_LEVEL.op_Implicit(systemLockoutLevel);
			global::System.Collections.Generic.IEnumerator<ValueTuple<byte, bool>> enumerator = deviceLockStatus.EnumerateIsDeviceLocked().GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					ValueTuple<byte, bool> current = enumerator.Current;
					byte item = current.Item1;
					bool item2 = current.Item2;
					ILogicalDevice logicalDeviceFromMyRvDevice = GetLogicalDeviceFromMyRvDevice(deviceLockStatus.DeviceTableId, item);
					if (logicalDeviceFromMyRvDevice != null)
					{
						_myRvLinkDeviceManager.UpdateInTransitLockoutLevel(logicalDeviceFromMyRvDevice.LogicalId, IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)(item2 ? systemLockoutLevel : 0)));
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
			((LogicalDevice<LogicalDeviceChassisInfoStatus, ILogicalDeviceCapability>)(object)_myRvLinkDeviceManager.LogicalDeviceDefaultChassisInfo)?.UpdateDeviceStatus(deviceLockStatus.ChassisInfoStatus);
		}
	}

	internal void UpdateMonitorPanelStatus(MyRvLinkMonitorPanelStatus? monitorPanelStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || monitorPanelStatus == null || monitorPanelStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceMonitorPanelStatus>> enumerator = monitorPanelStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceMonitorPanelStatus> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceMonitorPanelStatus item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceMonitorPanelDirect val = (ILogicalDeviceMonitorPanelDirect)(object)((logicalDevice is ILogicalDeviceMonitorPanelDirect) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateMonitorPanelStatus"} Unable to update status to {monitorPanelStatus} because can't find logical device for {this}", global::System.Array.Empty<object>());
					}
					else
					{
						((ILogicalDeviceWithStatusUpdate<LogicalDeviceMonitorPanelStatus>)(object)val).UpdateDeviceStatus(item2);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateOnlineStatus(MyRvLinkDeviceOnlineStatus? deviceOnlineStatus)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (DeviceList == null)
		{
			return;
		}
		if (deviceOnlineStatus == null)
		{
			TaggedLog.Debug("MyRvLinkDeviceTracker", "Taking all devices offline", global::System.Array.Empty<object>());
			_myRvLinkDeviceManager.TakeDevicesOfflineIfNeeded(forceOffline: true);
		}
		else
		{
			if (deviceOnlineStatus.DeviceTableId != DeviceTableId)
			{
				return;
			}
			global::System.Collections.Generic.IEnumerator<ValueTuple<byte, bool>> enumerator = deviceOnlineStatus.EnumerateIsDeviceOnline().GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					ValueTuple<byte, bool> current = enumerator.Current;
					byte item = current.Item1;
					bool item2 = current.Item2;
					ILogicalDevice logicalDeviceFromMyRvDevice = GetLogicalDeviceFromMyRvDevice(deviceOnlineStatus.DeviceTableId, item);
					_myRvLinkDeviceManager.UpdateLogicalDeviceOnline((logicalDeviceFromMyRvDevice != null) ? logicalDeviceFromMyRvDevice.LogicalId : null, item2);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
		}
	}

	internal void UpdateRelayBasicLatchingStatus(MyRvLinkRelayBasicLatchingStatusType2? latchingRelayStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || latchingRelayStatus == null || latchingRelayStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceRelayBasicStatusType2>> enumerator = latchingRelayStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceRelayBasicStatusType2> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceRelayBasicStatusType2 item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					if (!(_myRvLinkDeviceManager.GetLogicalDevice(device) is ILogicalDeviceLatchingRelay<LogicalDeviceRelayBasicStatusType2> val))
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"MyRvLinkRelayBasicLatchingStatusType2"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceRelayHBridge"}", global::System.Array.Empty<object>());
					}
					else if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceRelayBasicStatusType2>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatusUpdate<LogicalDeviceRelayBasicStatusType2>)(object)val).UpdateDeviceStatus(item2);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateRelayBasicLatchingStatus(MyRvLinkRelayBasicLatchingStatusType1? latchingRelayStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || latchingRelayStatus == null || latchingRelayStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceRelayBasicStatusType1>> enumerator = latchingRelayStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceRelayBasicStatusType1> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceRelayBasicStatusType1 item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					if (!(_myRvLinkDeviceManager.GetLogicalDevice(device) is ILogicalDeviceLatchingRelay<LogicalDeviceRelayBasicStatusType1> val))
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"MyRvLinkRelayBasicLatchingStatusType2"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceRelayHBridge"}", global::System.Array.Empty<object>());
					}
					else if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceRelayBasicStatusType1>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatusUpdate<LogicalDeviceRelayBasicStatusType1>)(object)val).UpdateDeviceStatus(item2);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateRelayHBridgeMomentaryStatus(MyRvLinkRelayHBridgeMomentaryStatusType1? momentaryRelayStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || momentaryRelayStatus == null || momentaryRelayStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceRelayHBridgeStatusType1>> enumerator = momentaryRelayStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceRelayHBridgeStatusType1> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceRelayHBridgeStatusType1 item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					if (!(_myRvLinkDeviceManager.GetLogicalDevice(device) is ILogicalDeviceRelayHBridge<LogicalDeviceRelayHBridgeStatusType1> val))
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"MyRvLinkRelayHBridgeMomentaryStatusType1"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceRelayHBridge"}", global::System.Array.Empty<object>());
					}
					else if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceRelayHBridgeStatusType1>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatusUpdate<LogicalDeviceRelayHBridgeStatusType1>)(object)val).UpdateDeviceStatus(item2);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateRelayHBridgeMomentaryStatus(MyRvLinkRelayHBridgeMomentaryStatusType2? momentaryRelayStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || momentaryRelayStatus == null || momentaryRelayStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceRelayHBridgeStatusType2>> enumerator = momentaryRelayStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceRelayHBridgeStatusType2> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceRelayHBridgeStatusType2 item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					if (!(_myRvLinkDeviceManager.GetLogicalDevice(device) is ILogicalDeviceRelayHBridge<LogicalDeviceRelayHBridgeStatusType2> val))
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"MyRvLinkRelayBasicLatchingStatusType2"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceRelayHBridge"}", global::System.Array.Empty<object>());
					}
					else if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceRelayHBridgeStatusType2>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatusUpdate<LogicalDeviceRelayHBridgeStatusType2>)(object)val).UpdateDeviceStatus(item2);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateRgbLightStatus(MyRvLinkRgbLightStatus rgbLightStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || rgbLightStatus == null || rgbLightStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceLightRgbStatus>> enumerator = rgbLightStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceLightRgbStatus> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceLightRgbStatus item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceLightRgbDirect val = (ILogicalDeviceLightRgbDirect)(object)((logicalDevice is ILogicalDeviceLightRgbDirect) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateRgbLightStatus"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceLightRgbDirect"}", global::System.Array.Empty<object>());
						break;
					}
					if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceLightRgbStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatusUpdate<LogicalDeviceLightRgbStatus>)(object)val).UpdateDeviceStatus(item2);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateSessionStatus(MyRvLinkDeviceSessionStatus? deviceOnlineStatus)
	{
		if (DeviceList != null)
		{
			_ = deviceOnlineStatus?.DeviceTableId == DeviceTableId;
		}
	}

	internal void UpdateTankSensorStatus(MyRvLinkTankSensorStatus? tankSensorStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || tankSensorStatus == null || tankSensorStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, byte>> enumerator = tankSensorStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, byte> current = enumerator.Current;
				byte item = current.Item1;
				byte item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceDirectTankSensor val = (ILogicalDeviceDirectTankSensor)(object)((logicalDevice is ILogicalDeviceDirectTankSensor) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateTankSensorStatus"} Unable to update status because can't find logical device for {this} OR device isn't a {"ILogicalDeviceDirectTankSensor"}", global::System.Array.Empty<object>());
						break;
					}
					LogicalDeviceTankSensorStatus val2 = new LogicalDeviceTankSensorStatus();
					val2.SetLevel(item2);
					if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceTankSensorStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)val2))
					{
						((ILogicalDeviceWithStatusUpdate<LogicalDeviceTankSensorStatus>)(object)val).UpdateDeviceStatus(val2);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	internal void UpdateTankSensorStatusV2(MyRvLinkTankSensorStatusV2? tankSensorStatus)
	{
		List<IMyRvLinkDevice> deviceList = DeviceList;
		IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
		if (deviceList == null || tankSensorStatus == null || tankSensorStatus.DeviceTableId != DeviceTableId || !Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)tankSensorStatus.DeviceId, ref myRvLinkDevice) || !(myRvLinkDevice is MyRvLinkDeviceIdsCan device))
		{
			return;
		}
		ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
		ILogicalDeviceTankSensor val = (ILogicalDeviceTankSensor)(object)((logicalDevice is ILogicalDeviceTankSensor) ? logicalDevice : null);
		if (val == null)
		{
			TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateTankSensorStatusV2"} Unable to update status to {tankSensorStatus} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceTankSensor"}", global::System.Array.Empty<object>());
		}
		else
		{
			LogicalDeviceTankSensorStatus tankSensorStatus2 = tankSensorStatus.GetTankSensorStatus();
			if (tankSensorStatus2 != null && !((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceTankSensorStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)tankSensorStatus2))
			{
				((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)tankSensorStatus2).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)tankSensorStatus2).Size);
			}
		}
	}

	internal void UpdateTemperatureSensorStatus(MyRvLinkTemperatureSensorStatus temperatureSensorStatus)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> deviceList = DeviceList;
		if (deviceList == null || temperatureSensorStatus == null || temperatureSensorStatus.DeviceTableId != DeviceTableId)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<ValueTuple<byte, LogicalDeviceTemperatureSensorStatus>> enumerator = temperatureSensorStatus.EnumerateStatus().GetEnumerator();
		try
		{
			IMyRvLinkDevice myRvLinkDevice = default(IMyRvLinkDevice);
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<byte, LogicalDeviceTemperatureSensorStatus> current = enumerator.Current;
				byte item = current.Item1;
				LogicalDeviceTemperatureSensorStatus item2 = current.Item2;
				if (Collection.TryGetValueAtIndex<IMyRvLinkDevice>((global::System.Collections.Generic.ICollection<IMyRvLinkDevice>)deviceList, (int)item, ref myRvLinkDevice) && myRvLinkDevice is MyRvLinkDeviceIdsCan device)
				{
					ILogicalDevice? logicalDevice = _myRvLinkDeviceManager.GetLogicalDevice(device);
					ILogicalDeviceTemperatureSensor val = (ILogicalDeviceTemperatureSensor)(object)((logicalDevice is ILogicalDeviceTemperatureSensor) ? logicalDevice : null);
					if (val == null)
					{
						TaggedLog.Warning("MyRvLinkDeviceTracker", $"{"UpdateTemperatureSensorStatus"} Unable to update status to {item2} because can't find logical device for {this} OR device isn't a {"ILogicalDeviceTemperatureSensor"}", global::System.Array.Empty<object>());
					}
					else if (!((LogicalDeviceDataPacketMutableDoubleBuffer)((ILogicalDeviceWithStatus<LogicalDeviceTemperatureSensorStatus>)(object)val).DeviceStatus).EqualsData((LogicalDeviceDataPacketMutableDoubleBuffer)(object)item2))
					{
						((ILogicalDeviceWithStatus)val).UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)item2).Size);
					}
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}
}
