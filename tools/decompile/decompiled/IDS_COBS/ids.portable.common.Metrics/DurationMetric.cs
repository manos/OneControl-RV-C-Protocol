using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ids.portable.common.Metrics;

public class DurationMetric : IDurationMetric
{
	private readonly Queue<ValueTuple<long, long>> _dataBuffer;

	private long _totalTicks;

	private int _totalMeasurements;

	[field: CompilerGenerated]
	public TimeSpan DeltaThreshold
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public TimeSpan SamplingWindow
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public int Count
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	public TimeSpan Average
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public DurationMetric(TimeSpan deltaThreshold, TimeSpan samplingWindow)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		DeltaThreshold = deltaThreshold;
		SamplingWindow = samplingWindow;
		_dataBuffer = new Queue<ValueTuple<long, long>>();
		Count = 0;
		Average = TimeSpan.Zero;
	}

	public void Update(TimeSpan delta, TimeSpan timeIndex)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		if (delta > DeltaThreshold)
		{
			Count++;
			_totalTicks += ((TimeSpan)(ref delta)).Ticks;
			_totalMeasurements++;
			_dataBuffer.Enqueue(new ValueTuple<long, long>(((TimeSpan)(ref delta)).Ticks, ((TimeSpan)(ref timeIndex)).Ticks));
		}
		while (_totalMeasurements > 0)
		{
			long num = ((TimeSpan)(ref timeIndex)).Ticks - _dataBuffer.Peek().Item2;
			TimeSpan samplingWindow = SamplingWindow;
			if (num <= ((TimeSpan)(ref samplingWindow)).Ticks)
			{
				break;
			}
			_totalTicks -= _dataBuffer.Dequeue().Item1;
			_totalMeasurements--;
		}
		Average = ((_totalMeasurements > 0) ? TimeSpan.FromTicks(_totalTicks / _totalMeasurements) : TimeSpan.Zero);
	}
}
