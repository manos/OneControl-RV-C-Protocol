using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public class Watchdog : CommonDisposable, IWatchdog, ICommonDisposable, global::System.IDisposable
{
	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CWatchdogBackgroundOperationAsync_003Ed__28 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public Watchdog _003C_003E4__this;

		public CancellationToken cancellationToken;

		private Stopwatch _003CmonitorStartedStopwatch_003E5__2;

		private bool _003Ctriggered_003E5__3;

		private int _003CsleepTime_003E5__4;

		private Stopwatch _003CmonitorTotalDelayTimePassed_003E5__5;

		private int _003CadjustedSleepTime_003E5__6;

		private ConfiguredTaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			Watchdog watchdog = _003C_003E4__this;
			try
			{
				if (num != 0)
				{
					_003CmonitorStartedStopwatch_003E5__2 = Stopwatch.StartNew();
					_003Ctriggered_003E5__3 = false;
					goto IL_0026;
				}
				ConfiguredTaskAwaiter<bool> val = _003C_003Eu__1;
				_003C_003Eu__1 = default(ConfiguredTaskAwaiter<bool>);
				num = (_003C_003E1__state = -1);
				goto IL_01b6;
				IL_01b6:
				val.GetResult();
				if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
				{
					if (_003CsleepTime_003E5__4 - _003CmonitorTotalDelayTimePassed_003E5__5.ElapsedMilliseconds > 10)
					{
						TaggedLog.Warning("Watchdog", $"Watchdog delay finished too soon.  Expected {_003CsleepTime_003E5__4}ms but only {_003CmonitorTotalDelayTimePassed_003E5__5.ElapsedMilliseconds}ms has passed [using time delay of {_003CadjustedSleepTime_003E5__6}ms]");
					}
					goto IL_0262;
				}
				goto IL_0272;
				IL_0272:
				_003CmonitorTotalDelayTimePassed_003E5__5 = null;
				goto IL_0026;
				IL_0026:
				_003CsleepTime_003E5__4 = 0;
				object sync = watchdog._sync;
				bool flag = false;
				TimeSpan val2;
				try
				{
					Monitor.Enter(sync, ref flag);
					if (!watchdog.IsDisposed && !((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
					{
						_003Ctriggered_003E5__3 = watchdog.Triggered;
						if (!_003Ctriggered_003E5__3)
						{
							long elapsedMilliseconds = watchdog._lastPetStopwatch.ElapsedMilliseconds;
							val2 = watchdog.PetTimeout;
							_003CsleepTime_003E5__4 = (int)((TimeSpan)(ref val2)).TotalMilliseconds - (int)elapsedMilliseconds;
							if (watchdog.MaxTimeUntilTriggered > TimeSpan.Zero && _003CsleepTime_003E5__4 > 0)
							{
								val2 = watchdog.MaxTimeUntilTriggered;
								int num2 = (int)((TimeSpan)(ref val2)).TotalMilliseconds - (int)_003CmonitorStartedStopwatch_003E5__2.ElapsedMilliseconds;
								if (num2 < _003CsleepTime_003E5__4)
								{
									_003CsleepTime_003E5__4 = num2;
								}
							}
							if (_003CsleepTime_003E5__4 > 0)
							{
								goto IL_0114;
							}
							bool flag2 = (watchdog.Triggered = true);
							_003Ctriggered_003E5__3 = flag2;
						}
					}
				}
				finally
				{
					if (num < 0 && flag)
					{
						Monitor.Exit(sync);
					}
				}
				if ((!watchdog.IsDisposed && !((CancellationToken)(ref cancellationToken)).IsCancellationRequested) & _003Ctriggered_003E5__3)
				{
					long elapsedMilliseconds2 = _003CmonitorStartedStopwatch_003E5__2.ElapsedMilliseconds;
					val2 = watchdog.PetTimeout;
					if (elapsedMilliseconds2 < (long)((TimeSpan)(ref val2)).TotalMilliseconds)
					{
						global::System.Runtime.CompilerServices.DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new global::System.Runtime.CompilerServices.DefaultInterpolatedStringHandler(75, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Watchdog was triggered too soon.  Triggered after ");
						defaultInterpolatedStringHandler.AppendFormatted<long>(elapsedMilliseconds2);
						defaultInterpolatedStringHandler.AppendLiteral("ms when Pet Timeout is ");
						val2 = watchdog.PetTimeout;
						defaultInterpolatedStringHandler.AppendFormatted<double>(((TimeSpan)(ref val2)).TotalMilliseconds);
						defaultInterpolatedStringHandler.AppendLiteral("ms");
						TaggedLog.Warning("Watchdog", defaultInterpolatedStringHandler.ToStringAndClear());
					}
					try
					{
						Action? triggerCallback = watchdog._triggerCallback;
						if (triggerCallback != null)
						{
							triggerCallback.Invoke();
						}
					}
					catch (global::System.Exception ex)
					{
						TaggedLog.Error("Watchdog", "{0} callback failed {1}\n{2}", ((object)watchdog).GetType().FullName, ex.Message, ex.StackTrace);
					}
					watchdog._triggerTcs?.TrySetResult(true);
				}
				goto end_IL_000e;
				IL_0114:
				_003CmonitorTotalDelayTimePassed_003E5__5 = Stopwatch.StartNew();
				goto IL_0262;
				IL_0262:
				if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
				{
					_003CadjustedSleepTime_003E5__6 = (int)(_003CsleepTime_003E5__4 - _003CmonitorTotalDelayTimePassed_003E5__5.ElapsedMilliseconds);
					if (_003CadjustedSleepTime_003E5__6 > 0)
					{
						val = TaskExtension.TryDelay(_003CadjustedSleepTime_003E5__6, cancellationToken).ConfigureAwait(false).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<bool>, _003CWatchdogBackgroundOperationAsync_003Ed__28>(ref val, ref this);
							return;
						}
						goto IL_01b6;
					}
				}
				goto IL_0272;
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003CmonitorStartedStopwatch_003E5__2 = null;
				((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003CmonitorStartedStopwatch_003E5__2 = null;
			((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetResult();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetStateMachine(stateMachine);
		}
	}

	private const string LogTag = "Watchdog";

	private const double PercentOfTotalTimeToRespond = 1.25;

	public const long PetFailed = -1L;

	private const int MaximumSleepTimeDifferenceMs = 10;

	private readonly object _sync = new object();

	private readonly Action? _triggerCallback;

	private TaskCompletionSource<bool>? _triggerTcs;

	private global::System.Threading.Tasks.Task? _monitoringTask;

	private CancellationTokenSource _monitoringTaskCts = new CancellationTokenSource();

	private readonly Stopwatch _lastPetStopwatch = new Stopwatch();

	[field: CompilerGenerated]
	public bool AutoStartOnFirstPet
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public TimeSpan PetTimeout
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public TimeSpan MaxTimeUntilTriggered
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public bool Triggered
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public Watchdog(int petTimeoutMs, bool autoStartOnFirstPet = false)
		: this(TimeSpan.FromMilliseconds((double)petTimeoutMs), null, autoStartOnFirstPet)
	{
	}//IL_0003: Unknown result type (might be due to invalid IL or missing references)


	public Watchdog(int petTimeoutMs, Action? triggerCallback, bool autoStartOnFirstPet = false)
		: this(TimeSpan.FromMilliseconds((double)petTimeoutMs), triggerCallback, autoStartOnFirstPet)
	{
	}//IL_0003: Unknown result type (might be due to invalid IL or missing references)


	public Watchdog(int petTimeoutMs, int maxTimeUntilTriggered, Action? triggerCallback, bool autoStartOnFirstPet = false)
		: this(TimeSpan.FromMilliseconds((double)petTimeoutMs), TimeSpan.FromMilliseconds((double)maxTimeUntilTriggered), triggerCallback, autoStartOnFirstPet)
	{
	}//IL_0003: Unknown result type (might be due to invalid IL or missing references)
	//IL_000a: Unknown result type (might be due to invalid IL or missing references)


	public Watchdog(TimeSpan petTimeout, Action? triggerCallback, bool autoStartOnFirstPet = false)
		: this(petTimeout, TimeSpan.Zero, triggerCallback, autoStartOnFirstPet)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0002: Unknown result type (might be due to invalid IL or missing references)


	public Watchdog(TimeSpan petTimeout, TimeSpan maxTimeUntilTriggered, Action? triggerCallback, bool autoStartOnFirstPet)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		AutoStartOnFirstPet = autoStartOnFirstPet;
		PetTimeout = petTimeout;
		MaxTimeUntilTriggered = maxTimeUntilTriggered;
		_triggerCallback = triggerCallback;
	}

	[AsyncStateMachine(typeof(_003CWatchdogBackgroundOperationAsync_003Ed__28))]
	private global::System.Threading.Tasks.Task WatchdogBackgroundOperationAsync(CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		_003CWatchdogBackgroundOperationAsync_003Ed__28 _003CWatchdogBackgroundOperationAsync_003Ed__ = default(_003CWatchdogBackgroundOperationAsync_003Ed__28);
		_003CWatchdogBackgroundOperationAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CWatchdogBackgroundOperationAsync_003Ed__._003C_003E4__this = this;
		_003CWatchdogBackgroundOperationAsync_003Ed__.cancellationToken = cancellationToken;
		_003CWatchdogBackgroundOperationAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CWatchdogBackgroundOperationAsync_003Ed__._003C_003Et__builder)).Start<_003CWatchdogBackgroundOperationAsync_003Ed__28>(ref _003CWatchdogBackgroundOperationAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CWatchdogBackgroundOperationAsync_003Ed__._003C_003Et__builder)).Task;
	}

	public void Monitor()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		lock (_sync)
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(((object)this).GetType().FullName);
			}
			if (Triggered)
			{
				throw new InvalidOperationException(((object)this).GetType().FullName + " has already been triggered!");
			}
			if (_monitoringTask != null)
			{
				throw new InvalidOperationException(((object)this).GetType().FullName + " is already monitoring!");
			}
			_lastPetStopwatch.Restart();
			_monitoringTask = WatchdogBackgroundOperationAsync(_monitoringTaskCts.Token);
		}
	}

	public void Cancel()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		lock (_sync)
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(((object)this).GetType().FullName);
			}
			if (_triggerTcs != null && !((global::System.Threading.Tasks.Task)_triggerTcs.Task).IsCompleted)
			{
				_triggerTcs.TrySetException((global::System.Exception)new OperationCanceledException("Watchdog was canceled"));
			}
			Triggered = false;
			_monitoringTaskCts.TryCancelAndDispose();
			_monitoringTaskCts = new CancellationTokenSource();
			_monitoringTask = null;
		}
	}

	public long Pet(bool autoReset = false)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		lock (_sync)
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(((object)this).GetType().FullName);
			}
			if (AutoStartOnFirstPet && _monitoringTask == null)
			{
				Monitor();
			}
			if (Triggered)
			{
				if (!autoReset)
				{
					throw new InvalidOperationException(((object)this).GetType().FullName + " has already been triggered!");
				}
				Triggered = false;
				TaskCompletionSource<bool>? triggerTcs = _triggerTcs;
				if (triggerTcs != null && ((global::System.Threading.Tasks.Task)triggerTcs.Task).IsCompleted)
				{
					_triggerTcs = null;
				}
				_monitoringTask = null;
				Monitor();
				return _lastPetStopwatch.ElapsedMilliseconds;
			}
			long elapsedMilliseconds = _lastPetStopwatch.ElapsedMilliseconds;
			_lastPetStopwatch.Restart();
			return elapsedMilliseconds;
		}
	}

	public long TryPet(bool autoReset = false)
	{
		try
		{
			if (base.IsDisposed)
			{
				return -1L;
			}
			return Pet(autoReset);
		}
		catch
		{
			return -1L;
		}
	}

	public global::System.Threading.Tasks.Task AsTask()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		lock (_sync)
		{
			if (_triggerTcs == null)
			{
				if (base.IsDisposed)
				{
					return global::System.Threading.Tasks.Task.FromException((global::System.Exception)new ObjectDisposedException(((object)this).GetType().FullName, "The Watchdog has been disposed so can't get Task from AsTask"));
				}
				if (Triggered)
				{
					return global::System.Threading.Tasks.Task.FromResult<bool>(true);
				}
				_triggerTcs = new TaskCompletionSource<bool>((object)(TaskContinuationOptions)64);
			}
			return _triggerTcs.Task;
		}
	}

	public override void Dispose(bool disposing)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		lock (_sync)
		{
			_monitoringTaskCts.TryCancelAndDispose();
			_monitoringTask = null;
			if (_triggerTcs != null && !((global::System.Threading.Tasks.Task)_triggerTcs.Task).IsCompleted)
			{
				_triggerTcs.TrySetException((global::System.Exception)new ObjectDisposedException(((object)this).GetType().FullName, "The Watchdog has been disposed"));
			}
		}
	}
}
