using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using IDS.Portable.Common;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkSessionManager : CommonDisposable, ILogicalDeviceSessionManager, ICommonDisposable, global::System.IDisposable
{
	private const string LogTag = "MyRvLinkSessionManager";

	private readonly IDirectConnectionMyRvLink _directManager;

	private readonly ConcurrentDictionary<ValueTuple<LogicalDeviceSessionType, ILogicalDevice>, LogicalDeviceSessionMyRvLink> _sessionDict;

	public MyRvLinkSessionManager(IDirectConnectionMyRvLink directManager)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_directManager = directManager ?? throw new ArgumentNullException("directManager");
		_sessionDict = new ConcurrentDictionary<ValueTuple<LogicalDeviceSessionType, ILogicalDevice>, LogicalDeviceSessionMyRvLink>();
	}

	public bool IsSessionActive(LogicalDeviceSessionType sessionType, ILogicalDevice logicalDevice)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		LogicalDeviceSessionMyRvLink logicalDeviceSessionMyRvLink = default(LogicalDeviceSessionMyRvLink);
		if (!_sessionDict.TryGetValue(new ValueTuple<LogicalDeviceSessionType, ILogicalDevice>(sessionType, logicalDevice), ref logicalDeviceSessionMyRvLink))
		{
			return false;
		}
		if (logicalDeviceSessionMyRvLink.IsActivated && _directManager != logicalDevice.DeviceService.GetPrimaryDeviceSourceDirect(logicalDevice))
		{
			TaggedLog.Information("MyRvLinkSessionManager", "MyRvLinkSessionManager Auto deactivating session because device no longer being controlled via this RvLink", global::System.Array.Empty<object>());
			DeactivateSession(sessionType, logicalDevice, closeSession: true);
		}
		return logicalDeviceSessionMyRvLink.IsActivated;
	}

	public global::System.Threading.Tasks.Task<ILogicalDeviceSession> ActivateSessionAsync(LogicalDeviceSessionType sessionType, ILogicalDevice logicalDevice, CancellationToken cancelToken, uint msSessionKeepAliveTime = 15000u, uint msSessionGetTimeout = 3000u)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		if (!((ILogicalDeviceSourceDirect)_directManager).DeviceService.SessionsEnabled)
		{
			throw new ActivateSessionDisabledException("MyRvLinkSessionManager");
		}
		if (((CommonDisposable)this).IsDisposed)
		{
			throw new ObjectDisposedException("ActivateSessionAsync not possible because MyRvLinkSessionManager Is Disposed");
		}
		if ((int)((IDevicesCommon)logicalDevice).ActiveConnection == 2)
		{
			throw new ActivateSessionRemoteActiveException("MyRvLinkSessionManager");
		}
		if (!((ILogicalDeviceSourceDirect)_directManager).IsLogicalDeviceOnline(logicalDevice))
		{
			throw new PhysicalDeviceNotFoundException("MyRvLinkSessionManager", logicalDevice, "");
		}
		if (!_directManager.GetMyRvDeviceFromLogicalDevice(logicalDevice).HasValue)
		{
			throw new PhysicalDeviceNotFoundException("MyRvLinkSessionManager", logicalDevice, "");
		}
		if (((ICommonDisposable)logicalDevice).IsDisposed)
		{
			throw new ObjectDisposedException("logicalDevice", "Logical Device Is Disposed");
		}
		if (InTransitLockoutStatusExtension.IsInLockout(logicalDevice.InTransitLockout))
		{
			throw new ActivateSessionEnforcedInTransitLockout("MyRvLinkSessionManager", logicalDevice);
		}
		LogicalDeviceSessionMyRvLink session = default(LogicalDeviceSessionMyRvLink);
		if (!_sessionDict.TryGetValue(new ValueTuple<LogicalDeviceSessionType, ILogicalDevice>(sessionType, logicalDevice), ref session))
		{
			session = new LogicalDeviceSessionMyRvLink();
			_sessionDict.AddOrUpdate(new ValueTuple<LogicalDeviceSessionType, ILogicalDevice>(sessionType, logicalDevice), session, (Func<ValueTuple<LogicalDeviceSessionType, ILogicalDevice>, LogicalDeviceSessionMyRvLink, LogicalDeviceSessionMyRvLink>)((ValueTuple<LogicalDeviceSessionType, ILogicalDevice> key, LogicalDeviceSessionMyRvLink oldValue) => session));
		}
		session.ActivateSession();
		logicalDevice.UpdateSessionChanged(LogicalDevicePidWriteAccessExtension.ToIdsCanSessionId(sessionType));
		return global::System.Threading.Tasks.Task.FromResult<ILogicalDeviceSession>((ILogicalDeviceSession)(object)session);
	}

	public void DeactivateSession(LogicalDeviceSessionType sessionType, ILogicalDevice logicalDevice, bool closeSession)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		LogicalDeviceSessionMyRvLink logicalDeviceSessionMyRvLink = default(LogicalDeviceSessionMyRvLink);
		if (_sessionDict.TryGetValue(new ValueTuple<LogicalDeviceSessionType, ILogicalDevice>(sessionType, logicalDevice), ref logicalDeviceSessionMyRvLink))
		{
			logicalDeviceSessionMyRvLink.DeactivateSession();
			logicalDevice.UpdateSessionChanged(LogicalDevicePidWriteAccessExtension.ToIdsCanSessionId(sessionType));
		}
	}

	public void CloseAllSessions()
	{
		_sessionDict.Clear();
	}

	public override void Dispose(bool disposing)
	{
		CloseAllSessions();
	}
}
