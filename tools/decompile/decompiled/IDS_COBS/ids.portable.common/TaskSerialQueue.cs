using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using IDS.Portable.Common.Extensions;

namespace IDS.Portable.Common;

public class TaskSerialQueue
{
	public class TaskSerialLock : CommonDisposable
	{
		private readonly TaskSerialQueue _taskSerialQueue;

		private readonly CancellationTokenSource _cts;

		private readonly TaskCompletionSource<bool> _lockGrantedTcs = new TaskCompletionSource<bool>((TaskCreationOptions)64);

		[field: CompilerGenerated]
		internal CancellationToken CancelToken
		{
			[CompilerGenerated]
			get;
		}

		internal global::System.Threading.Tasks.Task WaitForLockAsync()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return _lockGrantedTcs.WaitAsync<bool>(CancelToken, -1, updateTcs: true);
		}

		internal void GrantLock()
		{
			_lockGrantedTcs.TrySetResult(true);
		}

		internal TaskSerialLock(TaskSerialQueue taskSerialQueue, CancellationToken cancelToken, TimeSpan timeout)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			_taskSerialQueue = taskSerialQueue;
			_cts = CancellationTokenSource.CreateLinkedTokenSource(cancelToken);
			if (timeout < TimeSpan.MaxValue)
			{
				_cts.CancelAfter(timeout);
			}
			CancelToken = _cts.Token;
		}

		public override void Dispose(bool disposing)
		{
			_taskSerialQueue.LockNoLongerNeeded(this);
			_cts.TryCancelAndDispose();
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetLockAsync_003Ed__11 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<TaskSerialLock> _003C_003Et__builder;

		public TaskSerialQueue _003C_003E4__this;

		public CancellationToken cancelToken;

		public TimeSpan timeout;

		private TaskSerialLock _003CserialLock_003E5__2;

		private TaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			TaskSerialQueue taskSerialQueue = _003C_003E4__this;
			TaskSerialLock result;
			try
			{
				if (num != 0)
				{
					_003CserialLock_003E5__2 = new TaskSerialLock(taskSerialQueue, cancelToken, timeout);
					List<TaskSerialLock> taskQueue = taskSerialQueue._taskQueue;
					bool flag = false;
					try
					{
						Monitor.Enter((object)taskQueue, ref flag);
						if (taskSerialQueue._taskQueue.Count > taskSerialQueue.MaxQueueSize)
						{
							IndexOutOfRangeException ex = new IndexOutOfRangeException($"TaskSerialQueue: Unable to run queued task because maximum queue Size of {taskSerialQueue.MaxQueueSize} reached");
							TaggedLog.Warning("TaskSerialQueue", ((global::System.Exception)(object)ex).Message, string.Empty);
							throw ex;
						}
						if (taskSerialQueue._currentLock != null)
						{
							taskSerialQueue._taskQueue.Add(_003CserialLock_003E5__2);
							goto end_IL_0036;
						}
						taskSerialQueue._currentLock = _003CserialLock_003E5__2;
						taskSerialQueue._currentLock.GrantLock();
						result = _003CserialLock_003E5__2;
						goto end_IL_000e;
						end_IL_0036:;
					}
					finally
					{
						if (num < 0 && flag)
						{
							Monitor.Exit((object)taskQueue);
						}
					}
				}
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = _003CserialLock_003E5__2.WaitForLockAsync().GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CGetLockAsync_003Ed__11>(ref val, ref this);
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
					result = _003CserialLock_003E5__2;
				}
				catch
				{
					_003CserialLock_003E5__2.TryDispose();
					throw;
				}
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003CserialLock_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003CserialLock_003E5__2 = null;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	private const string LogTag = "TaskSerialQueue";

	private readonly List<TaskSerialLock> _taskQueue = new List<TaskSerialLock>();

	private TaskSerialLock? _currentLock;

	[field: CompilerGenerated]
	public int MaxQueueSize
	{
		[CompilerGenerated]
		get;
	}

	public TaskSerialQueue(int maxQueueSize = 2147483647)
	{
		MaxQueueSize = maxQueueSize;
	}

	[Obsolete("GetLock is deprecated, please use GetLockAsync instead.")]
	public global::System.Threading.Tasks.Task<TaskSerialLock> GetLock(CancellationToken cancelToken)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return GetLockAsync(cancelToken, TimeSpan.MaxValue);
	}

	[Obsolete("GetLock is deprecated, please use GetLockAsync instead.")]
	public global::System.Threading.Tasks.Task<TaskSerialLock> GetLock(CancellationToken cancelToken, TimeSpan timeout)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return GetLockAsync(cancelToken, timeout);
	}

	public global::System.Threading.Tasks.Task<TaskSerialLock> GetLockAsync(CancellationToken cancelToken)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return GetLockAsync(cancelToken, TimeSpan.MaxValue);
	}

	[AsyncStateMachine(typeof(_003CGetLockAsync_003Ed__11))]
	public async global::System.Threading.Tasks.Task<TaskSerialLock> GetLockAsync(CancellationToken cancelToken, TimeSpan timeout)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		TaskSerialLock serialLock = new TaskSerialLock(this, cancelToken, timeout);
		lock (_taskQueue)
		{
			if (_taskQueue.Count > MaxQueueSize)
			{
				IndexOutOfRangeException ex = new IndexOutOfRangeException($"TaskSerialQueue: Unable to run queued task because maximum queue Size of {MaxQueueSize} reached");
				TaggedLog.Warning("TaskSerialQueue", ((global::System.Exception)(object)ex).Message, string.Empty);
				throw ex;
			}
			if (_currentLock == null)
			{
				_currentLock = serialLock;
				_currentLock.GrantLock();
				return serialLock;
			}
			_taskQueue.Add(serialLock);
		}
		try
		{
			await serialLock.WaitForLockAsync();
			return serialLock;
		}
		catch
		{
			serialLock.TryDispose();
			throw;
		}
	}

	internal void LockNoLongerNeeded(TaskSerialLock serialLock)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		lock (_taskQueue)
		{
			if (_currentLock != serialLock)
			{
				((global::System.Collections.Generic.ICollection<TaskSerialLock>)_taskQueue).TryRemove(serialLock);
				return;
			}
			_currentLock = null;
			TaskSerialLock item;
			while (((global::System.Collections.Generic.IList<TaskSerialLock>)_taskQueue).TryTakeFirst(out item))
			{
				if (!item.IsDisposed)
				{
					CancellationToken cancelToken = item.CancelToken;
					if (!((CancellationToken)(ref cancelToken)).IsCancellationRequested)
					{
						_currentLock = item;
						item.GrantLock();
						break;
					}
				}
				TaggedLog.Debug("TaskSerialQueue", "TaskSerialQueue: Removed queued lock request because it was canceled ({0})", _taskQueue.Count);
			}
		}
	}
}
