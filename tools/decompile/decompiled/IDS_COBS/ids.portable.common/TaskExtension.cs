using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public static class TaskExtension
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass16_0
	{
		public TaskCompletionSource<bool> tcs;

		internal void _003CWhenCanceled_003Eb__0(object? s)
		{
			tcs.SetResult(true);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CTryAwaitAsync_003Ed__12 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public global::System.Threading.Tasks.Task instance;

		public bool configureAwait;

		private ConfiguredTaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			bool result;
			try
			{
				try
				{
					ConfiguredTaskAwaiter val2;
					if (num != 0)
					{
						ConfiguredTaskAwaitable val = instance.ConfigureAwait(configureAwait);
						val2 = ((ConfiguredTaskAwaitable)(ref val)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, _003CTryAwaitAsync_003Ed__12>(ref val2, ref this);
							return;
						}
					}
					else
					{
						val2 = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
					}
					((ConfiguredTaskAwaiter)(ref val2)).GetResult();
					result = true;
				}
				catch
				{
					result = false;
				}
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
	private struct _003CTryAwaitAsync_003Ed__13<TValue> : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<TValue> _003C_003Et__builder;

		public global::System.Threading.Tasks.Task<TValue> instance;

		public bool configureAwait;

		public TValue @default;

		private ConfiguredTaskAwaiter<TValue> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			TValue result;
			try
			{
				try
				{
					ConfiguredTaskAwaiter<TValue> val;
					if (num != 0)
					{
						val = instance.ConfigureAwait(configureAwait).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<TValue>, _003CTryAwaitAsync_003Ed__13<TValue>>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaiter<TValue>);
						num = (_003C_003E1__state = -1);
					}
					result = val.GetResult();
				}
				catch
				{
					result = @default;
				}
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
	private struct _003CTryDelay_003Ed__2 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public int millisecondsDelay;

		public CancellationToken cancelToken;

		private TaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			bool result;
			try
			{
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = global::System.Threading.Tasks.Task.Delay(millisecondsDelay, cancelToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CTryDelay_003Ed__2>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
					}
					((TaskAwaiter)(ref val)).GetResult();
					result = true;
				}
				catch
				{
					goto IL_0077;
				}
				goto end_IL_0007;
				IL_0077:
				result = false;
				end_IL_0007:;
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
	private struct _003CTryDelay_003Ed__3 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public TimeSpan timespan;

		public CancellationToken cancelToken;

		private TaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			bool result;
			try
			{
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = global::System.Threading.Tasks.Task.Delay(timespan, cancelToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CTryDelay_003Ed__3>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
					}
					((TaskAwaiter)(ref val)).GetResult();
					result = true;
				}
				catch
				{
					goto IL_0077;
				}
				goto end_IL_0007;
				IL_0077:
				result = false;
				end_IL_0007:;
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
	private struct _003CWhenCanceled_003Ed__16 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public CancellationToken cancellationToken;

		private CancellationTokenRegistration _003C_003E7__wrap1;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			try
			{
				_003C_003Ec__DisplayClass16_0 _003C_003Ec__DisplayClass16_ = default(_003C_003Ec__DisplayClass16_0);
				if (num != 0)
				{
					_003C_003Ec__DisplayClass16_ = new _003C_003Ec__DisplayClass16_0
					{
						tcs = new TaskCompletionSource<bool>()
					};
					_003C_003E7__wrap1 = ((CancellationToken)(ref cancellationToken)).Register((Action<object>)_003C_003Ec__DisplayClass16_._003CWhenCanceled_003Eb__0, (object)null);
				}
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						val = _003C_003Ec__DisplayClass16_.tcs.Task.GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CWhenCanceled_003Ed__16>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
					}
					val.GetResult();
				}
				catch
				{
				}
				finally
				{
					if (num < 0)
					{
						((global::System.IDisposable)global::System.Runtime.CompilerServices.Unsafe.As<CancellationTokenRegistration, CancellationTokenRegistration>(ref _003C_003E7__wrap1)/*cast due to .constrained prefix*/).Dispose();
					}
				}
				_003C_003E7__wrap1 = default(CancellationTokenRegistration);
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

	public static readonly global::System.Threading.Tasks.Task CompletedTask = global::System.Threading.Tasks.Task.FromResult<bool>(false);

	public static global::System.Threading.Tasks.Task Delay(TimeSpan timeSpan, CancellationToken cancelToken)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return global::System.Threading.Tasks.Task.Delay((int)((TimeSpan)(ref timeSpan)).TotalMilliseconds, cancelToken);
	}

	[AsyncStateMachine(typeof(_003CTryDelay_003Ed__2))]
	public static async global::System.Threading.Tasks.Task<bool> TryDelay(int millisecondsDelay, CancellationToken cancelToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			await global::System.Threading.Tasks.Task.Delay(millisecondsDelay, cancelToken);
			return true;
		}
		catch
		{
		}
		return false;
	}

	[AsyncStateMachine(typeof(_003CTryDelay_003Ed__3))]
	public static async global::System.Threading.Tasks.Task<bool> TryDelay(TimeSpan timespan, CancellationToken cancelToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			await global::System.Threading.Tasks.Task.Delay(timespan, cancelToken);
			return true;
		}
		catch
		{
		}
		return false;
	}

	[MethodImpl((MethodImplOptions)256)]
	public static global::System.Threading.Tasks.Task OnSuccess(this global::System.Threading.Tasks.Task thisTask, Action<global::System.Threading.Tasks.Task> action, CancellationToken token)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		thisTask.ContinueWith(action, token, (TaskContinuationOptions)393216, TaskScheduler.Default);
		return thisTask;
	}

	[MethodImpl((MethodImplOptions)256)]
	public static global::System.Threading.Tasks.Task OnSuccess(this global::System.Threading.Tasks.Task thisTask, Action<global::System.Threading.Tasks.Task> action)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return thisTask.OnSuccess(action, CancellationToken.None);
	}

	[MethodImpl((MethodImplOptions)256)]
	public static global::System.Threading.Tasks.Task OnFailure(this global::System.Threading.Tasks.Task thisTask, Action<global::System.Threading.Tasks.Task> action, CancellationToken token)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		thisTask.ContinueWith(action, token, (TaskContinuationOptions)327680, TaskScheduler.Default);
		return thisTask;
	}

	[MethodImpl((MethodImplOptions)256)]
	public static global::System.Threading.Tasks.Task OnFailure(this global::System.Threading.Tasks.Task thisTask, Action<global::System.Threading.Tasks.Task> action)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return thisTask.OnFailure(action, CancellationToken.None);
	}

	[MethodImpl((MethodImplOptions)256)]
	public static global::System.Threading.Tasks.Task OnFailureLog(this global::System.Threading.Tasks.Task thisTask, string tag, string message, CancellationToken token)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		thisTask.OnFailure(delegate
		{
			TaggedLog.Error(tag, message, string.Empty);
		}, token);
		return thisTask;
	}

	[MethodImpl((MethodImplOptions)256)]
	public static global::System.Threading.Tasks.Task OnFailureLog(this global::System.Threading.Tasks.Task thisTask, string tag, string message)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return thisTask.OnFailureLog(tag, message, CancellationToken.None);
	}

	[MethodImpl((MethodImplOptions)256)]
	public static global::System.Threading.Tasks.Task OnCancellation(this global::System.Threading.Tasks.Task thisTask, Action<global::System.Threading.Tasks.Task> action, CancellationToken token)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		thisTask.ContinueWith(action, token, (TaskContinuationOptions)196608, TaskScheduler.Default);
		return thisTask;
	}

	[MethodImpl((MethodImplOptions)256)]
	public static global::System.Threading.Tasks.Task OnCancellation(this global::System.Threading.Tasks.Task thisTask, Action<global::System.Threading.Tasks.Task> action)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return thisTask.OnCancellation(action, CancellationToken.None);
	}

	[AsyncStateMachine(typeof(_003CTryAwaitAsync_003Ed__12))]
	public static async global::System.Threading.Tasks.Task<bool> TryAwaitAsync(this global::System.Threading.Tasks.Task instance, bool configureAwait = true)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			await instance.ConfigureAwait(configureAwait);
			return true;
		}
		catch
		{
			return false;
		}
	}

	[AsyncStateMachine(typeof(_003CTryAwaitAsync_003Ed__13<>))]
	public static async global::System.Threading.Tasks.Task<TValue> TryAwaitAsync<TValue>(this global::System.Threading.Tasks.Task<TValue> instance, TValue @default = default(TValue), bool configureAwait = true)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return await instance.ConfigureAwait(configureAwait);
		}
		catch
		{
			return @default;
		}
	}

	public static global::System.Threading.Tasks.Task<TResult> ContinueWith<TResult>(this global::System.Threading.Tasks.Task instance, Func<TResult> onCompletion, Func<TResult> onCancellation, Func<AggregateException, TResult> onException, TaskCreationOptions taskCreationOptions = (TaskCreationOptions)0)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>(taskCreationOptions);
		if (instance == null)
		{
			throw new ArgumentException("instance cannot be null!");
		}
		if (onCompletion == null)
		{
			throw new ArgumentException("onCompletion cannot be null!");
		}
		if (onCancellation == null)
		{
			throw new ArgumentException("onCancellation cannot be null!");
		}
		if (onException == null)
		{
			throw new ArgumentException("onException cannot be null!");
		}
		TResult result;
		instance.ContinueWith((Action<global::System.Threading.Tasks.Task, object>)delegate(global::System.Threading.Tasks.Task task, object? obj)
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			try
			{
				if (task.IsFaulted)
				{
					result = onException.Invoke(task.Exception);
				}
				else if (task.IsCanceled)
				{
					result = onCancellation.Invoke();
				}
				else
				{
					result = onCompletion.Invoke();
				}
				tcs.TrySetResult(result);
			}
			catch (global::System.Exception ex)
			{
				tcs.TrySetException((global::System.Exception)new InvalidOperationException("Exception was not handled in callback method!", ex));
			}
		}, (object)(TaskContinuationOptions)0, TaskScheduler.Default);
		return tcs.Task;
	}

	public static global::System.Threading.Tasks.Task<TResult> ContinueWith<TResult>(this global::System.Threading.Tasks.Task<TResult> instance, Func<TResult> onCancellation, Func<AggregateException, TResult> onException, TaskCreationOptions taskCreationOptions = (TaskCreationOptions)0)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>(taskCreationOptions);
		if (instance == null)
		{
			throw new ArgumentException("instance cannot be null!");
		}
		if (onCancellation == null)
		{
			throw new ArgumentException("onCancellation cannot be null!");
		}
		if (onException == null)
		{
			throw new ArgumentException("onException cannot be null!");
		}
		TResult result;
		instance.ContinueWith((Action<global::System.Threading.Tasks.Task<TResult>, object>)delegate(global::System.Threading.Tasks.Task<TResult> task, object? obj)
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			try
			{
				if (((global::System.Threading.Tasks.Task)task).IsFaulted)
				{
					result = onException.Invoke(((global::System.Threading.Tasks.Task)task).Exception);
				}
				else if (((global::System.Threading.Tasks.Task)task).IsCanceled)
				{
					result = onCancellation.Invoke();
				}
				else
				{
					result = task.Result;
				}
				tcs.TrySetResult(result);
			}
			catch (global::System.Exception ex)
			{
				tcs.TrySetException((global::System.Exception)new InvalidOperationException("Exception was not handled in callback method!", ex));
			}
		}, (object)(TaskContinuationOptions)0, TaskScheduler.Default);
		return tcs.Task;
	}

	[AsyncStateMachine(typeof(_003CWhenCanceled_003Ed__16))]
	public static global::System.Threading.Tasks.Task WhenCanceled(this CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		_003CWhenCanceled_003Ed__16 _003CWhenCanceled_003Ed__ = default(_003CWhenCanceled_003Ed__16);
		_003CWhenCanceled_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CWhenCanceled_003Ed__.cancellationToken = cancellationToken;
		_003CWhenCanceled_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CWhenCanceled_003Ed__._003C_003Et__builder)).Start<_003CWhenCanceled_003Ed__16>(ref _003CWhenCanceled_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CWhenCanceled_003Ed__._003C_003Et__builder)).Task;
	}
}
