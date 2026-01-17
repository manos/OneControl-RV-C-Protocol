using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using IDS.Core.IDS_CAN;
using IDS.Portable.Common;
using IDS.Portable.LogicalDevice;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceManager
{
	private class LogicalDeviceData
	{
		private long _refreshedOnlineTimestampMs;

		private long _refreshedLockoutTimestampMs;

		private bool _isOnline;

		private IN_MOTION_LOCKOUT_LEVEL _lockoutLevel = IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)0);

		[field: CompilerGenerated]
		public ILogicalDevice LogicalDevice
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public bool ShouldRemoveDevice
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public long TimeSinceLastRefreshedOnlineMs => LogicalDeviceFreeRunningTimer.ElapsedMilliseconds - _refreshedOnlineTimestampMs;

		public long TimeSinceLastRefreshedLockoutMs => LogicalDeviceFreeRunningTimer.ElapsedMilliseconds - _refreshedLockoutTimestampMs;

		public bool IsOnline
		{
			get
			{
				return _isOnline;
			}
			set
			{
				_refreshedOnlineTimestampMs = LogicalDeviceFreeRunningTimer.ElapsedMilliseconds;
				if (_isOnline != value)
				{
					TaggedLog.Debug("MyRvLinkDeviceManager", $"LogicalDeviceData update IsOnline to {(value ? "Online" : "Offline")} for {LogicalDevice.LogicalId.ToString((LogicalDeviceIdFormat)2)} last refreshed {TimeSinceLastRefreshedOnlineMs}ms", global::System.Array.Empty<object>());
					_isOnline = value;
					LogicalDevice.UpdateDeviceOnline(value);
				}
			}
		}

		public IN_MOTION_LOCKOUT_LEVEL LockoutLevel
		{
			get
			{
				return _lockoutLevel;
			}
			set
			{
				_refreshedLockoutTimestampMs = LogicalDeviceFreeRunningTimer.ElapsedMilliseconds;
				if (_lockoutLevel != value)
				{
					TaggedLog.Debug("MyRvLinkDeviceManager", $"LogicalDeviceData update LockoutLevel to {value} for {LogicalDevice.LogicalId.ToString((LogicalDeviceIdFormat)2)} last refreshed {TimeSinceLastRefreshedLockoutMs}ms", global::System.Array.Empty<object>());
					_lockoutLevel = value;
					LogicalDevice.UpdateInTransitLockout();
				}
			}
		}

		public LogicalDeviceData(ILogicalDevice logicalDevice)
		{
			LogicalDevice = logicalDevice;
			_refreshedOnlineTimestampMs = LogicalDeviceFreeRunningTimer.ElapsedMilliseconds;
			_refreshedLockoutTimestampMs = LogicalDeviceFreeRunningTimer.ElapsedMilliseconds;
		}
	}

	private const string LogTag = "MyRvLinkDeviceManager";

	private string LogPrefix;

	private readonly object _lock = new object();

	private readonly IDirectConnectionMyRvLink _directConnectionMyRvLink;

	private readonly ConcurrentDictionary<ILogicalDeviceId, LogicalDeviceData> _logicalDeviceDict = new ConcurrentDictionary<ILogicalDeviceId, LogicalDeviceData>();

	private const long AutoOfflineTimeMs = 4000L;

	private const long AutoRemoveInTransitLockoutTimeMs = 4000L;

	[field: CompilerGenerated]
	public ILogicalDeviceService LogicalDeviceService
	{
		[CompilerGenerated]
		get;
	}

	public LogicalDeviceChassisInfo? LogicalDeviceDefaultChassisInfo
	{
		get
		{
			ILogicalDevice obj = Enumerable.FirstOrDefault<ILogicalDevice>(Enumerable.Select<LogicalDeviceData, ILogicalDevice>((global::System.Collections.Generic.IEnumerable<LogicalDeviceData>)_logicalDeviceDict.Values, (Func<LogicalDeviceData, ILogicalDevice>)((LogicalDeviceData logicalDeviceData) => logicalDeviceData.LogicalDevice)), (Func<ILogicalDevice, bool>)((ILogicalDevice logicalDevice) => DEVICE_TYPE.op_Implicit(logicalDevice.LogicalId.DeviceType) == 39));
			return (LogicalDeviceChassisInfo?)(object)((obj is LogicalDeviceChassisInfo) ? obj : null);
		}
	}

	public MyRvLinkDeviceManager(ILogicalDeviceService logicalDeviceService, IDirectConnectionMyRvLink directConnectionMyRvLink)
	{
		_directConnectionMyRvLink = directConnectionMyRvLink;
		LogicalDeviceService = logicalDeviceService;
		LogPrefix = directConnectionMyRvLink.LogPrefix;
	}

	public bool IsLogicalDeviceOnline(ILogicalDeviceId? logicalDeviceId)
	{
		if (logicalDeviceId == null)
		{
			return false;
		}
		return DictionaryExtension.TryGetValue<ILogicalDeviceId, LogicalDeviceData>(_logicalDeviceDict, logicalDeviceId)?.IsOnline ?? false;
	}

	public void UpdateLogicalDeviceOnline(ILogicalDeviceId? logicalDeviceId, bool isOnline)
	{
		if (logicalDeviceId == null)
		{
			return;
		}
		LogicalDeviceData logicalDeviceData = default(LogicalDeviceData);
		if (!_logicalDeviceDict.TryGetValue(logicalDeviceId, ref logicalDeviceData))
		{
			TaggedLog.Debug("MyRvLinkDeviceManager", $"{LogPrefix} Unable to update online state for logical device because it doesn't exist yet: {logicalDeviceId}", global::System.Array.Empty<object>());
			return;
		}
		if (logicalDeviceData.IsOnline != isOnline)
		{
			ILogicalDeviceManager deviceManager = LogicalDeviceService.DeviceManager;
			if (deviceManager != null)
			{
				deviceManager.ContainerDataSourceSync(true);
			}
		}
		logicalDeviceData.IsOnline = isOnline;
	}

	public void TakeDevicesOfflineIfNeeded(bool forceOffline)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		global::System.Collections.Generic.IEnumerator<KeyValuePair<ILogicalDeviceId, LogicalDeviceData>> enumerator = _logicalDeviceDict.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				KeyValuePair<ILogicalDeviceId, LogicalDeviceData> current = enumerator.Current;
				if ((forceOffline || current.Value.TimeSinceLastRefreshedOnlineMs >= 4000) && current.Value.IsOnline)
				{
					flag = true;
					current.Value.IsOnline = false;
					current.Value.LogicalDevice.UpdateInTransitLockout();
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		if (flag)
		{
			ILogicalDeviceManager deviceManager = LogicalDeviceService.DeviceManager;
			if (deviceManager != null)
			{
				deviceManager.ContainerDataSourceSync(true);
			}
		}
	}

	internal void RemoveOfflineDevices()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		global::System.Collections.Generic.IEnumerator<LogicalDeviceData> enumerator = ((global::System.Collections.Generic.IEnumerable<LogicalDeviceData>)_logicalDeviceDict.Values).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				LogicalDeviceData current = enumerator.Current;
				if ((int)((IDevicesCommon)current.LogicalDevice).ActiveConnection == 0)
				{
					current.ShouldRemoveDevice = true;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	public IN_MOTION_LOCKOUT_LEVEL GetInTransitLockoutLevel(ILogicalDeviceId? logicalDeviceId)
	{
		if (logicalDeviceId == null)
		{
			return IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)0);
		}
		return DictionaryExtension.TryGetValue<ILogicalDeviceId, LogicalDeviceData>(_logicalDeviceDict, logicalDeviceId)?.LockoutLevel ?? IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)0);
	}

	public void UpdateInTransitLockoutLevel(ILogicalDeviceId? logicalDeviceId, IN_MOTION_LOCKOUT_LEVEL lockoutLevel)
	{
		if (logicalDeviceId != null)
		{
			LogicalDeviceData logicalDeviceData = default(LogicalDeviceData);
			if (!_logicalDeviceDict.TryGetValue(logicalDeviceId, ref logicalDeviceData))
			{
				TaggedLog.Debug("MyRvLinkDeviceManager", $"{LogPrefix} Unable to update Lockout Level for logical device because it doesn't exist yet: {logicalDeviceId}", global::System.Array.Empty<object>());
			}
			else
			{
				logicalDeviceData.LockoutLevel = lockoutLevel;
			}
		}
	}

	public void RemoveInTransitLockoutLevel(bool forceRemoveLockout)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		global::System.Collections.Generic.IEnumerator<KeyValuePair<ILogicalDeviceId, LogicalDeviceData>> enumerator = _logicalDeviceDict.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				KeyValuePair<ILogicalDeviceId, LogicalDeviceData> current = enumerator.Current;
				if (forceRemoveLockout || current.Value.TimeSinceLastRefreshedLockoutMs >= 4000)
				{
					current.Value.LockoutLevel = IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)0);
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	public ILogicalDevice? GetLogicalDevice(IMyRvLinkDeviceForLogicalDevice device)
	{
		ILogicalDeviceId logicalDeviceId = device.LogicalDeviceId;
		if (logicalDeviceId == null)
		{
			TaggedLog.Debug("MyRvLinkDeviceManager", $"{LogPrefix} Unable to get logical device for {device} because it doesn't have a Logical Device Id yet.", global::System.Array.Empty<object>());
			return null;
		}
		ILogicalDeviceManager deviceManager = LogicalDeviceService.DeviceManager;
		if (deviceManager == null)
		{
			TaggedLog.Warning("MyRvLinkDeviceManager", $"{LogPrefix} Unable to get logical device for {device} because {"DeviceManager"} is null", global::System.Array.Empty<object>());
			return null;
		}
		lock (_lock)
		{
			LogicalDeviceData logicalDeviceData = default(LogicalDeviceData);
			if (_logicalDeviceDict.TryGetValue(logicalDeviceId, ref logicalDeviceData))
			{
				if (logicalDeviceData.ShouldRemoveDevice)
				{
					TaggedLog.Warning("MyRvLinkDeviceManager", $"{LogPrefix} {"GetLogicalDevice"} Found device but it is in the process of being removed: {logicalDeviceId}", global::System.Array.Empty<object>());
					return null;
				}
				if (!((ICommonDisposable)logicalDeviceData.LogicalDevice).IsDisposed)
				{
					return logicalDeviceData.LogicalDevice;
				}
				DictionaryExtension.TryRemove<ILogicalDeviceId, LogicalDeviceData>((IDictionary<ILogicalDeviceId, LogicalDeviceData>)(object)_logicalDeviceDict, logicalDeviceId);
			}
			ILogicalDevice val = deviceManager.FindLogicalDevice(logicalDeviceId) ?? AddLogicalDevice(device);
			if (val == null)
			{
				TaggedLog.Warning("MyRvLinkDeviceManager", $"{LogPrefix} {"GetLogicalDevice"} unable to find or create LogicalDevice {logicalDeviceId}", global::System.Array.Empty<object>());
				DictionaryExtension.TryRemove<ILogicalDeviceId, LogicalDeviceData>((IDictionary<ILogicalDeviceId, LogicalDeviceData>)(object)_logicalDeviceDict, logicalDeviceId);
				return null;
			}
			_logicalDeviceDict[logicalDeviceId] = new LogicalDeviceData(val);
			return val;
		}
	}

	internal ILogicalDevice? AddLogicalDevice(IMyRvLinkDeviceForLogicalDevice device)
	{
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		ILogicalDeviceId logicalDeviceId = device.LogicalDeviceId;
		if (logicalDeviceId == null)
		{
			TaggedLog.Debug("MyRvLinkDeviceManager", $"{LogPrefix} Unable to add logical device for {device} because it doesn't have a Logical Device Id yet.", global::System.Array.Empty<object>());
			return null;
		}
		lock (_lock)
		{
			ILogicalDevice val = null;
			try
			{
				TaggedLog.Debug("MyRvLinkDeviceManager", $"{LogPrefix} AddLogicalDevice find LogicalDevice for {logicalDeviceId} ", global::System.Array.Empty<object>());
				ILogicalDeviceService logicalDeviceService = LogicalDeviceService;
				object obj;
				if (logicalDeviceService == null)
				{
					obj = null;
				}
				else
				{
					ILogicalDeviceManager deviceManager = logicalDeviceService.DeviceManager;
					obj = ((deviceManager != null) ? deviceManager.AddLogicalDevice(logicalDeviceId, (byte?)device.RawDefaultCapability, (ILogicalDeviceSource)_directConnectionMyRvLink, (Func<ILogicalDevice, bool>)((ILogicalDevice attemptAutoRenameForLogicalDevice) => true)) : null);
				}
				val = (ILogicalDevice)obj;
				if (val == null)
				{
					throw new LogicalDeviceException($"logical device was null for {device}", (global::System.Exception)null);
				}
				TaggedLog.Debug("MyRvLinkDeviceManager", $"{LogPrefix} AddLogicalDevice found/created LogicalDevice {val}", global::System.Array.Empty<object>());
				val.UpdateDeviceCapability((byte?)device.RawDefaultCapability);
			}
			catch (global::System.Exception ex)
			{
				TaggedLog.Error("MyRvLinkDeviceManager", LogPrefix + " AddLogicalDevice failed " + ex.Message, global::System.Array.Empty<object>());
			}
			return val;
		}
	}
}
