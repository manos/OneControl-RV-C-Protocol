using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ids.portable.common.Metrics;

public class FrequencyMetrics : IFrequencyMetricsReadonly
{
	private Stopwatch? _timerFromFirstUpdate;

	private Stopwatch? _timerFromLastUpdate;

	[field: CompilerGenerated]
	public long Count
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	public long MinTimeMs
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	public long MaxTimeMs
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	public double AverageTimeMs
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public double TotalTimeMs
	{
		get
		{
			Stopwatch? timerFromFirstUpdate = _timerFromFirstUpdate;
			return ((double?)((timerFromFirstUpdate != null) ? new long?(timerFromFirstUpdate.ElapsedMilliseconds) : ((long?)null))) ?? 0.0;
		}
	}

	[field: CompilerGenerated]
	public double UpdatesPerSecond
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public void Update()
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		if (_timerFromFirstUpdate == null)
		{
			_timerFromFirstUpdate = Stopwatch.StartNew();
		}
		if (_timerFromLastUpdate == null)
		{
			_timerFromLastUpdate = Stopwatch.StartNew();
		}
		Count++;
		long elapsedMilliseconds = _timerFromLastUpdate.ElapsedMilliseconds;
		MaxTimeMs = ((MaxTimeMs < elapsedMilliseconds) ? elapsedMilliseconds : MaxTimeMs);
		if (elapsedMilliseconds != 0L && elapsedMilliseconds < MinTimeMs)
		{
			MinTimeMs = elapsedMilliseconds;
		}
		AverageTimeMs = (double)_timerFromFirstUpdate.ElapsedMilliseconds / (double)Count;
		TimeSpan elapsed = _timerFromFirstUpdate.Elapsed;
		int num = (int)((TimeSpan)(ref elapsed)).TotalSeconds;
		UpdatesPerSecond = ((num == 0) ? 0f : ((float)Count / (float)num));
		_timerFromLastUpdate.Restart();
	}

	public void Clear()
	{
		Stopwatch? timerFromFirstUpdate = _timerFromFirstUpdate;
		if (timerFromFirstUpdate != null)
		{
			timerFromFirstUpdate.Reset();
		}
		Stopwatch? timerFromLastUpdate = _timerFromLastUpdate;
		if (timerFromLastUpdate != null)
		{
			timerFromLastUpdate.Reset();
		}
		Count = 0L;
		MinTimeMs = 0L;
		MaxTimeMs = 0L;
		AverageTimeMs = 0.0;
	}

	public virtual string ToString()
	{
		return $"Per Second={UpdatesPerSecond:F0} Count={Count} AverageTime={(long)AverageTimeMs}ms MaxTime={MaxTimeMs}ms MinTime={MinTimeMs}ms TotalTime={(long)TotalTimeMs}ms";
	}
}
