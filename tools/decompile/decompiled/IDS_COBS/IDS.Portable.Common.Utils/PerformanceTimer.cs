using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace IDS.Portable.Common.Utils;

public struct PerformanceTimer : global::System.IDisposable
{
	private static ObjectPool<Stopwatch> _stopwatchObjectPool = ObjectPool<Stopwatch>.MakeObjectPool<Stopwatch>();

	public Func<string> MakeWatchDetailDescription;

	private Stopwatch? _stopwatch;

	[field: CompilerGenerated]
	public PerformanceTimerOption Option
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public string LogTag
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public bool IsConfigured
	{
		[CompilerGenerated]
		get;
	}

	public bool IsRunning
	{
		get
		{
			Stopwatch? stopwatch = _stopwatch;
			if (stopwatch == null)
			{
				return false;
			}
			return stopwatch.IsRunning;
		}
	}

	[field: CompilerGenerated]
	private TimeSpan? StopTimeWarning
	{
		[CompilerGenerated]
		get;
	}

	public PerformanceTimer(string logTag, string message, TimeSpan? stopTimeWarning = null, PerformanceTimerOption option = PerformanceTimerOption.Verbose | PerformanceTimerOption.AutoStartOnCreate)
		: this(logTag, (object)message, stopTimeWarning, option)
	{
	}

	public PerformanceTimer(string logTag, object detailObject, TimeSpan? stopTimeWarning = null, PerformanceTimerOption option = PerformanceTimerOption.Verbose | PerformanceTimerOption.AutoStartOnCreate)
	{
		Option = option;
		LogTag = logTag;
		if (detailObject is string)
		{
			MakeWatchDetailDescription = () => (detailObject as string) ?? "Null Reference Error detailObject";
		}
		else if (detailObject != null)
		{
			MakeWatchDetailDescription = () => ((MemberInfo)detailObject.GetType()).Name;
		}
		else
		{
			MakeWatchDetailDescription = () => string.Empty;
		}
		_stopwatch = null;
		StopTimeWarning = stopTimeWarning;
		IsConfigured = true;
		if (((global::System.Enum)option).HasFlag((global::System.Enum)PerformanceTimerOption.AutoStartOnCreate))
		{
			Start();
		}
	}

	public void Start()
	{
		if (IsRunning)
		{
			TaggedLog.Warning(LogTag, "Ignoring Start as timer already started {0}", MakeWatchDetailDescription.Invoke());
			return;
		}
		_stopwatch = _stopwatchObjectPool.TakeObject();
		_stopwatch.Restart();
		if (((global::System.Enum)Option).HasFlag((global::System.Enum)PerformanceTimerOption.OnShowStart))
		{
			TaggedLog.Information(LogTag, "TIMER START {0}", MakeWatchDetailDescription.Invoke());
		}
	}

	public TimeSpan Stop()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		Stopwatch val = Interlocked.Exchange<Stopwatch>(ref _stopwatch, (Stopwatch)null);
		if (val == null || !val.IsRunning)
		{
			return TimeSpan.Zero;
		}
		val.Stop();
		TimeSpan elapsed = val.Elapsed;
		val.Reset();
		_stopwatchObjectPool.PutObject(val);
		val = null;
		TimeSpan? stopTimeWarning = StopTimeWarning;
		if (stopTimeWarning.HasValue)
		{
			TimeSpan valueOrDefault = stopTimeWarning.GetValueOrDefault();
			if (elapsed > valueOrDefault)
			{
				TaggedLog.Warning(LogTag, "TIMER STOPPED({0}ms > {1}ms) {2}", ((TimeSpan)(ref elapsed)).TotalMilliseconds, ((TimeSpan)(ref valueOrDefault)).TotalMilliseconds, MakeWatchDetailDescription.Invoke());
				goto IL_00ee;
			}
		}
		if (((global::System.Enum)Option).HasFlag((global::System.Enum)PerformanceTimerOption.OnShowStopTotalTimeInMs))
		{
			TaggedLog.Information(LogTag, "TIMER STOPPED({0}ms {1})", ((TimeSpan)(ref elapsed)).TotalMilliseconds, MakeWatchDetailDescription.Invoke());
		}
		goto IL_00ee;
		IL_00ee:
		return elapsed;
	}

	public void Mark(string message)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Stopwatch stopwatch = _stopwatch;
		TimeSpan? val = ((stopwatch != null) ? new TimeSpan?(stopwatch.Elapsed) : ((TimeSpan?)null));
		if (stopwatch != null && stopwatch.IsRunning)
		{
			TaggedLog.Information(LogTag, "TIMER MARK({0} - {1}ms)", message, val);
		}
	}

	public void Dispose()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Stop();
	}
}
