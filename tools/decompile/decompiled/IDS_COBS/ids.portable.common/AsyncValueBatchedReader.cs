using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public class AsyncValueBatchedReader<TValue> : IAsyncValueBatchedReader<TValue>
{
	public delegate global::System.Threading.Tasks.Task<TValue> ReadValueBlockedAsyncHandler(CancellationToken cancellationToken);

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CReadValueAsync_003Ed__15 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<TValue> _003C_003Et__builder;

		public AsyncValueBatchedReader<TValue> _003C_003E4__this;

		public bool forceUpdate;

		public CancellationToken cancellationToken;

		private IAsyncValueCached<TValue> _003CcacheValue_003E5__2;

		private TaskCompletionSource<TValue> _003Ctcs_003E5__3;

		private TaskAwaiter<TValue> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			AsyncValueBatchedReader<TValue> asyncValueBatchedReader = _003C_003E4__this;
			TValue result;
			try
			{
				TaskAwaiter<TValue> val;
				if (num == 0)
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<TValue>);
					num = (_003C_003E1__state = -1);
					goto IL_00ec;
				}
				if (num == 1)
				{
					goto IL_0101;
				}
				_003CcacheValue_003E5__2 = asyncValueBatchedReader.ReadCachedValue;
				if (forceUpdate || _003CcacheValue_003E5__2 == null || !_003CcacheValue_003E5__2.HasValue || _003CcacheValue_003E5__2.NeedsUpdate)
				{
					TaskCompletionSource<TValue> val2 = new TaskCompletionSource<TValue>();
					_003Ctcs_003E5__3 = Interlocked.CompareExchange<TaskCompletionSource<TValue>>(ref asyncValueBatchedReader._readTcs, val2, (TaskCompletionSource<TValue>)null);
					if (_003Ctcs_003E5__3 != null)
					{
						val = _003Ctcs_003E5__3.WaitAsync<TValue>(cancellationToken, asyncValueBatchedReader.ReadValueTimeoutMs).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<TValue>, _003CReadValueAsync_003Ed__15>(ref val, ref this);
							return;
						}
						goto IL_00ec;
					}
					_003Ctcs_003E5__3 = val2;
					goto IL_0101;
				}
				result = _003CcacheValue_003E5__2.Value;
				goto end_IL_000e;
				IL_0101:
				try
				{
					if (num != 1)
					{
						val = asyncValueBatchedReader.ReadValueImplAsync(cancellationToken).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<TValue>, _003CReadValueAsync_003Ed__15>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<TValue>);
						num = (_003C_003E1__state = -1);
					}
					TValue result2 = val.GetResult();
					_003Ctcs_003E5__3.SetResult(result2);
					if (_003CcacheValue_003E5__2 != null)
					{
						_003CcacheValue_003E5__2.Value = result2;
					}
					result = result2;
				}
				catch (global::System.Exception ex)
				{
					_003Ctcs_003E5__3.TrySetException(ex);
					throw;
				}
				finally
				{
					if (num < 0)
					{
						asyncValueBatchedReader._readTcs = null;
					}
				}
				goto end_IL_000e;
				IL_00ec:
				result = val.GetResult();
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003CcacheValue_003E5__2 = null;
				_003Ctcs_003E5__3 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003CcacheValue_003E5__2 = null;
			_003Ctcs_003E5__3 = null;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	private const string LogTag = "AsyncValueBatchedReader";

	public const int DefaultReadTimeoutMs = 3000;

	private TaskCompletionSource<TValue>? _readTcs;

	private readonly ReadValueBlockedAsyncHandler? _readValueBlockedAsync;

	public IAsyncValueCached<TValue>? ReadCachedValue
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public int ReadValueTimeoutMs
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		set;
	}

	public AsyncValueBatchedReader(ReadValueBlockedAsyncHandler readValueAsync, IAsyncValueCached<TValue>? readCachedValue = null, int readTimeoutMs = 3000)
		: this(readCachedValue, readTimeoutMs)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		_readValueBlockedAsync = readValueAsync ?? throw new ArgumentNullException("readValueAsync");
	}

	protected AsyncValueBatchedReader(IAsyncValueCached<TValue>? readCachedValue = null, int readTimeoutMs = 3000)
	{
		ReadValueTimeoutMs = readTimeoutMs;
		ReadCachedValue = readCachedValue;
	}

	[AsyncStateMachine(typeof(AsyncValueBatchedReader<>._003CReadValueAsync_003Ed__15))]
	public virtual async global::System.Threading.Tasks.Task<TValue> ReadValueAsync(CancellationToken cancellationToken, bool forceUpdate = false)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		IAsyncValueCached<TValue> cacheValue = ReadCachedValue;
		if (!forceUpdate && cacheValue != null && cacheValue.HasValue && !cacheValue.NeedsUpdate)
		{
			return cacheValue.Value;
		}
		TaskCompletionSource<TValue> val = new TaskCompletionSource<TValue>();
		TaskCompletionSource<TValue> tcs = Interlocked.CompareExchange<TaskCompletionSource<TValue>>(ref _readTcs, val, (TaskCompletionSource<TValue>)null);
		if (tcs != null)
		{
			return await tcs.WaitAsync<TValue>(cancellationToken, ReadValueTimeoutMs);
		}
		tcs = val;
		try
		{
			TValue val2 = await ReadValueImplAsync(cancellationToken);
			tcs.SetResult(val2);
			if (cacheValue != null)
			{
				cacheValue.Value = val2;
			}
			return val2;
		}
		catch (global::System.Exception ex)
		{
			tcs.TrySetException(ex);
			throw;
		}
		finally
		{
			_readTcs = null;
		}
	}

	protected virtual global::System.Threading.Tasks.Task<TValue> ReadValueImplAsync(CancellationToken cancellationToken)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (_readValueBlockedAsync ?? throw new NotImplementedException("AsyncValueBatchedReader - Derived classes should not call the base implementation"))(cancellationToken);
	}
}
