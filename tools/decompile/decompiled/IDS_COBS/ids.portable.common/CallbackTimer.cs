using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public sealed class CallbackTimer : CancellationTokenSource
{
	private const string LogTag = "CallbackTimer";

	public bool TimerFired;

	public CallbackTimer(TimerCallback callback, object state, int msDueTime)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			global::System.Threading.Tasks.Task.Delay(msDueTime, ((CancellationTokenSource)this).Token).ContinueWith((Action<global::System.Threading.Tasks.Task, object>)([CompilerGenerated] (global::System.Threading.Tasks.Task t, object? s) =>
			{
				if (!((CancellationTokenSource)this).IsCancellationRequested)
				{
					TimerFired = true;
					Tuple<TimerCallback, object> val = (Tuple<TimerCallback, object>)s;
					val.Item1(val.Item2);
				}
			}), (object)Tuple.Create<TimerCallback, object>(callback, state), CancellationToken.None, (TaskContinuationOptions)917504, TaskScheduler.Default);
		}
		catch (TaskCanceledException)
		{
		}
		catch (global::System.Exception ex2)
		{
			TaggedLog.Error("CallbackTimer", "Exception during timer callback {0}: {1}", ex2.Message, ex2.StackTrace);
		}
	}

	public CallbackTimer(TimerSimpleCallback callback, int msDueTime, bool repeat = false)
	{
		RunCallbackAfterDelay(callback, msDueTime, repeat);
	}

	private void RunCallbackAfterDelay(TimerSimpleCallback callback, int msDueTime, bool repeat)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			global::System.Threading.Tasks.Task.Delay(msDueTime, ((CancellationTokenSource)this).Token).ContinueWith((Action<global::System.Threading.Tasks.Task, object>)delegate
			{
				if (!((CancellationTokenSource)this).IsCancellationRequested && callback != null)
				{
					TimerFired = true;
					callback();
					if (repeat)
					{
						RunCallbackAfterDelay(callback, msDueTime, repeat);
					}
				}
			}, (object)null, CancellationToken.None, (TaskContinuationOptions)917504, TaskScheduler.Default);
		}
		catch (TaskCanceledException)
		{
		}
		catch (global::System.Exception ex2)
		{
			TaggedLog.Error("CallbackTimer", "CallbackTimer - Exception during timer callback {0}: {1}", ex2.Message, ex2.StackTrace);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			((CancellationTokenSource)this).Cancel();
		}
		catch
		{
		}
		((CancellationTokenSource)this).Dispose(disposing);
	}

	public void TryDispose()
	{
		try
		{
			((CancellationTokenSource)this).Dispose();
		}
		catch
		{
		}
	}
}
