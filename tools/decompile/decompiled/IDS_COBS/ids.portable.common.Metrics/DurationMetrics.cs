using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ids.portable.common.Metrics;

public class DurationMetrics
{
	public struct Duration : global::System.IDisposable
	{
		private DurationMetrics? _metrics;

		[field: CompilerGenerated]
		public TimeSpan Began
		{
			[CompilerGenerated]
			get;
		}

		public Duration(DurationMetrics metrics)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			_metrics = metrics;
			Began = _metrics.Now;
		}

		public void Dispose()
		{
			_metrics?.Update(this);
			_metrics = null;
		}
	}

	private readonly Stopwatch _timer = new Stopwatch();

	public readonly global::System.Collections.Generic.IReadOnlyList<IDurationMetric> Metrics;

	public TimeSpan Now => TimeSpan.FromTicks(_timer.ElapsedTicks);

	public DurationMetrics(global::System.Collections.Generic.IReadOnlyList<IDurationMetric> metrics)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		Metrics = metrics;
		_timer.Restart();
	}

	public Duration StartTimingMetric()
	{
		return new Duration(this);
	}

	private void Update(Duration duration)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		TimeSpan delta = Now - duration.Began;
		global::System.Collections.Generic.IEnumerator<IDurationMetric> enumerator = ((global::System.Collections.Generic.IEnumerable<IDurationMetric>)Metrics).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				((DurationMetric)enumerator.Current).Update(delta, duration.Began);
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}
}
