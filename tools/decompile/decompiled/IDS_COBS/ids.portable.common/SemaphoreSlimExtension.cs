using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public static class SemaphoreSlimExtension
{
	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CLockAsync_003Ed__0 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<SemaphoreSlimLock> _003C_003Et__builder;

		public SemaphoreSlim semaphore;

		public CancellationToken cancellationToken;

		private ConfiguredTaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			SemaphoreSlimLock result;
			try
			{
				ConfiguredTaskAwaiter val2;
				if (num != 0)
				{
					ConfiguredTaskAwaitable val = semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
					val2 = ((ConfiguredTaskAwaitable)(ref val)).GetAwaiter();
					if (!((ConfiguredTaskAwaiter)(ref val2)).IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val2;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, _003CLockAsync_003Ed__0>(ref val2, ref this);
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
				result = new SemaphoreSlimLock(semaphore, hasLock: true);
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
	private struct _003CTryLockAsync_003Ed__1 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<SemaphoreSlimLock> _003C_003Et__builder;

		public SemaphoreSlim semaphore;

		public CancellationToken cancellationToken;

		private TaskAwaiter<SemaphoreSlimLock> _003C_003Eu__1;

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
			SemaphoreSlimLock result;
			try
			{
				try
				{
					TaskAwaiter<SemaphoreSlimLock> val;
					if (num != 0)
					{
						val = semaphore.LockAsync(cancellationToken).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<SemaphoreSlimLock>, _003CTryLockAsync_003Ed__1>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<SemaphoreSlimLock>);
						num = (_003C_003E1__state = -1);
					}
					result = val.GetResult();
				}
				catch
				{
					result = new SemaphoreSlimLock(semaphore, hasLock: false);
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

	[AsyncStateMachine(typeof(_003CLockAsync_003Ed__0))]
	public static async global::System.Threading.Tasks.Task<SemaphoreSlimLock> LockAsync(this SemaphoreSlim semaphore, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
		return new SemaphoreSlimLock(semaphore, hasLock: true);
	}

	[AsyncStateMachine(typeof(_003CTryLockAsync_003Ed__1))]
	public static async global::System.Threading.Tasks.Task<SemaphoreSlimLock> TryLockAsync(this SemaphoreSlim semaphore, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return await semaphore.LockAsync(cancellationToken);
		}
		catch
		{
			return new SemaphoreSlimLock(semaphore, hasLock: false);
		}
	}
}
