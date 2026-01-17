using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using IDS.Portable.LogicalDevice;
using OneControl.Direct.MyRvLink.Devices;

namespace OneControl.Direct.MyRvLink.Cache;

public class DeviceTableIdCache
{
	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDeviceTableIdCacheSerializableAsync_003Ed__3 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<DeviceTableIdCacheSerializable> _003C_003Et__builder;

		public DeviceTableIdCache _003C_003E4__this;

		private TaskAwaiter<DeviceTableIdCacheSerializable?> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DeviceTableIdCache deviceTableIdCache = _003C_003E4__this;
			DeviceTableIdCacheSerializable result2;
			try
			{
				DeviceTableIdCacheSerializable deviceTableIdCacheSerializable;
				TaskAwaiter<DeviceTableIdCacheSerializable> val;
				if (num != 0)
				{
					deviceTableIdCacheSerializable = deviceTableIdCache._deviceTableIdCacheSerializable;
					if (deviceTableIdCacheSerializable != null)
					{
						goto IL_00a7;
					}
					val = DeviceTableIdCacheSerializable.TryLoadAsync(((ILogicalDeviceSource)deviceTableIdCache._directManager).DeviceSourceToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<DeviceTableIdCacheSerializable>, _003CGetDeviceTableIdCacheSerializableAsync_003Ed__3>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<DeviceTableIdCacheSerializable>);
					num = (_003C_003E1__state = -1);
				}
				DeviceTableIdCacheSerializable result = val.GetResult();
				deviceTableIdCacheSerializable = (deviceTableIdCache._deviceTableIdCacheSerializable = result ?? new DeviceTableIdCacheSerializable(((ILogicalDeviceSource)deviceTableIdCache._directManager).DeviceSourceToken));
				goto IL_00a7;
				IL_00a7:
				result2 = deviceTableIdCacheSerializable;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result2);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDevicesForDeviceTableCrcAsync_003Ed__5 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice>> _003C_003Et__builder;

		public DeviceTableIdCache _003C_003E4__this;

		public uint deviceTableCrc;

		private TaskAwaiter<DeviceTableIdCacheSerializable> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DeviceTableIdCache deviceTableIdCache = _003C_003E4__this;
			global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice> result;
			try
			{
				TaskAwaiter<DeviceTableIdCacheSerializable> val;
				if (num != 0)
				{
					val = deviceTableIdCache.GetDeviceTableIdCacheSerializableAsync().GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<DeviceTableIdCacheSerializable>, _003CGetDevicesForDeviceTableCrcAsync_003Ed__5>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<DeviceTableIdCacheSerializable>);
					num = (_003C_003E1__state = -1);
				}
				result = val.GetResult().GetFirstDeviceTableIdSerializableForCrc(deviceTableCrc)?.TryDecode();
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDevicesForDeviceTableIdAsync_003Ed__6 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice>> _003C_003Et__builder;

		public DeviceTableIdCache _003C_003E4__this;

		public byte deviceTableId;

		private TaskAwaiter<DeviceTableIdCacheSerializable> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DeviceTableIdCache deviceTableIdCache = _003C_003E4__this;
			global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice> result;
			try
			{
				TaskAwaiter<DeviceTableIdCacheSerializable> val;
				if (num != 0)
				{
					val = deviceTableIdCache.GetDeviceTableIdCacheSerializableAsync().GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<DeviceTableIdCacheSerializable>, _003CGetDevicesForDeviceTableIdAsync_003Ed__6>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<DeviceTableIdCacheSerializable>);
					num = (_003C_003E1__state = -1);
				}
				result = val.GetResult().GetDeviceTableIdSerializableForTableId(deviceTableId)?.TryDecode();
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CUpdateDevicesAsync_003Ed__4 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public DeviceTableIdCache _003C_003E4__this;

		public uint deviceTableCrc;

		public global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice> deviceList;

		public byte deviceTableId;

		private TaskAwaiter<DeviceTableIdCacheSerializable> _003C_003Eu__1;

		private TaskAwaiter<bool> _003C_003Eu__2;

		private void MoveNext()
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DeviceTableIdCache deviceTableIdCache = _003C_003E4__this;
			bool result2;
			try
			{
				TaskAwaiter<bool> val;
				TaskAwaiter<DeviceTableIdCacheSerializable> val2;
				if (num != 0)
				{
					if (num == 1)
					{
						val = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_0116;
					}
					val2 = deviceTableIdCache.GetDeviceTableIdCacheSerializableAsync().GetAwaiter();
					if (!val2.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val2;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<DeviceTableIdCacheSerializable>, _003CUpdateDevicesAsync_003Ed__4>(ref val2, ref this);
						return;
					}
				}
				else
				{
					val2 = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<DeviceTableIdCacheSerializable>);
					num = (_003C_003E1__state = -1);
				}
				DeviceTableIdCacheSerializable result = val2.GetResult();
				result.Update(deviceTableSerializable: new MyRvLinkDeviceTableSerializable(deviceTableCrc, (global::System.Collections.Generic.IReadOnlyList<MyRvLinkDeviceSerializable>)Enumerable.ToList<MyRvLinkDeviceSerializable>(Enumerable.Select<IMyRvLinkDevice, MyRvLinkDeviceSerializable>((global::System.Collections.Generic.IEnumerable<IMyRvLinkDevice>)deviceList, (Func<IMyRvLinkDevice, MyRvLinkDeviceSerializable>)((IMyRvLinkDevice device) => new MyRvLinkDeviceSerializable(device)))), global::System.DateTime.Now), deviceTableId: deviceTableId);
				val = result.TrySaveAsync().GetAwaiter();
				if (!val.IsCompleted)
				{
					num = (_003C_003E1__state = 1);
					_003C_003Eu__2 = val;
					_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CUpdateDevicesAsync_003Ed__4>(ref val, ref this);
					return;
				}
				goto IL_0116;
				IL_0116:
				result2 = val.GetResult();
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result2);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	private readonly IDirectConnectionMyRvLink _directManager;

	private DeviceTableIdCacheSerializable? _deviceTableIdCacheSerializable;

	public DeviceTableIdCache(IDirectConnectionMyRvLink directManager)
	{
		_directManager = directManager;
	}

	[AsyncStateMachine(typeof(_003CGetDeviceTableIdCacheSerializableAsync_003Ed__3))]
	public async global::System.Threading.Tasks.Task<DeviceTableIdCacheSerializable> GetDeviceTableIdCacheSerializableAsync()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		DeviceTableIdCacheSerializable deviceTableIdCacheSerializable = _deviceTableIdCacheSerializable;
		if (deviceTableIdCacheSerializable == null)
		{
			DeviceTableIdCacheSerializable deviceTableIdCacheSerializable2 = await DeviceTableIdCacheSerializable.TryLoadAsync(((ILogicalDeviceSource)_directManager).DeviceSourceToken);
			DeviceTableIdCache deviceTableIdCache = this;
			DeviceTableIdCacheSerializable obj = deviceTableIdCacheSerializable2 ?? new DeviceTableIdCacheSerializable(((ILogicalDeviceSource)_directManager).DeviceSourceToken);
			DeviceTableIdCacheSerializable deviceTableIdCacheSerializable3 = obj;
			deviceTableIdCache._deviceTableIdCacheSerializable = obj;
			deviceTableIdCacheSerializable = deviceTableIdCacheSerializable3;
		}
		return deviceTableIdCacheSerializable;
	}

	[AsyncStateMachine(typeof(_003CUpdateDevicesAsync_003Ed__4))]
	public async global::System.Threading.Tasks.Task<bool> UpdateDevicesAsync(uint deviceTableCrc, byte deviceTableId, global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice> deviceList)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		DeviceTableIdCacheSerializable obj = await GetDeviceTableIdCacheSerializableAsync();
		MyRvLinkDeviceTableSerializable deviceTableSerializable = new MyRvLinkDeviceTableSerializable(deviceTableCrc, (global::System.Collections.Generic.IReadOnlyList<MyRvLinkDeviceSerializable>)Enumerable.ToList<MyRvLinkDeviceSerializable>(Enumerable.Select<IMyRvLinkDevice, MyRvLinkDeviceSerializable>((global::System.Collections.Generic.IEnumerable<IMyRvLinkDevice>)deviceList, (Func<IMyRvLinkDevice, MyRvLinkDeviceSerializable>)((IMyRvLinkDevice device) => new MyRvLinkDeviceSerializable(device)))), global::System.DateTime.Now);
		obj.Update(deviceTableId, deviceTableSerializable);
		return await obj.TrySaveAsync();
	}

	[AsyncStateMachine(typeof(_003CGetDevicesForDeviceTableCrcAsync_003Ed__5))]
	public async global::System.Threading.Tasks.Task<global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice>?> GetDevicesForDeviceTableCrcAsync(uint deviceTableCrc)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return (await GetDeviceTableIdCacheSerializableAsync()).GetFirstDeviceTableIdSerializableForCrc(deviceTableCrc)?.TryDecode();
	}

	[AsyncStateMachine(typeof(_003CGetDevicesForDeviceTableIdAsync_003Ed__6))]
	public async global::System.Threading.Tasks.Task<global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice>?> GetDevicesForDeviceTableIdAsync(byte deviceTableId)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return (await GetDeviceTableIdCacheSerializableAsync()).GetDeviceTableIdSerializableForTableId(deviceTableId)?.TryDecode();
	}
}
