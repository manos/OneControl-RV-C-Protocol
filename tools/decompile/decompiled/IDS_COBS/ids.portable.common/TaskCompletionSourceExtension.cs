using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public static class TaskCompletionSourceExtension
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass1_0<TResult>
	{
		public bool updateTcs;

		public TaskCompletionSource<TResult> tcs;

		public CancellationTokenSource timeoutCancelTokenSource;

		public int timeoutMs;

		public TaskCompletionSource<TResult> overrideTcs;
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CTryWaitAsync_003Ed__0<TResult> : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<TResult> _003C_003Et__builder;

		public TaskCompletionSource<TResult> tcs;

		public CancellationToken cancelToken;

		public int timeoutMs;

		public bool updateTcs;

		public TResult failureResult;

		private TaskAwaiter<TResult> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			TResult result;
			try
			{
				try
				{
					TaskAwaiter<TResult> val;
					if (num != 0)
					{
						val = tcs.WaitAsync<TResult>(cancelToken, timeoutMs, updateTcs).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<TResult>, _003CTryWaitAsync_003Ed__0<TResult>>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<TResult>);
						num = (_003C_003E1__state = -1);
					}
					result = val.GetResult();
				}
				catch (global::System.Exception)
				{
					result = failureResult;
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
	private struct _003CWaitAsync_003Ed__1<TResult> : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<TResult> _003C_003Et__builder;

		public bool updateTcs;

		public TaskCompletionSource<TResult> tcs;

		public int timeoutMs;

		public CancellationToken cancelToken;

		private _003C_003Ec__DisplayClass1_0<TResult> _003C_003E8__1;

		private CancellationTokenSource _003ClinkedTokenSource_003E5__2;

		private CancellationTokenRegistration _003C_003E7__wrap2;

		private TaskAwaiter<global::System.Threading.Tasks.Task<TResult>> _003C_003Eu__1;

		private TaskAwaiter<TResult> _003C_003Eu__2;

		private unsafe void MoveNext()
		{
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Expected O, but got Unknown
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			TResult result;
			try
			{
				if ((uint)num > 1u)
				{
					_003C_003E8__1 = new _003C_003Ec__DisplayClass1_0<TResult>();
					_003C_003E8__1.updateTcs = updateTcs;
					_003C_003E8__1.tcs = tcs;
					_003C_003E8__1.timeoutMs = timeoutMs;
					_003C_003E8__1.overrideTcs = new TaskCompletionSource<TResult>();
					_003C_003E8__1.timeoutCancelTokenSource = ((_003C_003E8__1.timeoutMs <= 0 || _003C_003E8__1.timeoutMs == -1) ? ((CancellationTokenSource)null) : new CancellationTokenSource(_003C_003E8__1.timeoutMs));
				}
				try
				{
					if ((uint)num > 1u)
					{
						CancellationTokenSource obj = _003C_003E8__1.timeoutCancelTokenSource;
						CancellationToken val = ((obj != null) ? obj.Token : CancellationToken.None);
						_003ClinkedTokenSource_003E5__2 = CancellationTokenSource.CreateLinkedTokenSource(cancelToken, val);
					}
					try
					{
						if ((uint)num > 1u)
						{
							CancellationToken token = _003ClinkedTokenSource_003E5__2.Token;
							_003C_003E7__wrap2 = ((CancellationToken)(ref token)).Register(new Action(CancelTcs));
						}
						try
						{
							TaskAwaiter<TResult> val3;
							if (num == 0 || num != 1)
							{
								try
								{
									TaskAwaiter<global::System.Threading.Tasks.Task<TResult>> val2;
									if (num != 0)
									{
										val2 = ((global::System.Threading.Tasks.Task<global::System.Threading.Tasks.Task<global::System.Threading.Tasks.Task<TResult>>>)(object)global::System.Threading.Tasks.Task.WhenAny<TResult>(_003C_003E8__1.tcs.Task, _003C_003E8__1.overrideTcs.Task)).GetAwaiter();
										if (!((TaskAwaiter<global::System.Threading.Tasks.Task<global::System.Threading.Tasks.Task<TResult>>>*)(&val2))->IsCompleted)
										{
											num = (_003C_003E1__state = 0);
											_003C_003Eu__1 = val2;
											_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<global::System.Threading.Tasks.Task<TResult>>, _003CWaitAsync_003Ed__1<TResult>>(ref val2, ref this);
											return;
										}
									}
									else
									{
										val2 = _003C_003Eu__1;
										_003C_003Eu__1 = default(TaskAwaiter<global::System.Threading.Tasks.Task<TResult>>);
										num = (_003C_003E1__state = -1);
									}
									((TaskAwaiter<global::System.Threading.Tasks.Task<global::System.Threading.Tasks.Task<TResult>>>*)(&val2))->GetResult();
								}
								catch
								{
								}
								if (!((global::System.Threading.Tasks.Task)_003C_003E8__1.tcs.Task).IsCompleted)
								{
									CancellationTokenSource obj3 = _003C_003E8__1.timeoutCancelTokenSource;
									if (obj3 != null && obj3.IsCancellationRequested)
									{
										throw new TimeoutException($"WaitAsync timed out after {_003C_003E8__1.timeoutMs}ms");
									}
									throw new OperationCanceledException();
								}
								val3 = _003C_003E8__1.tcs.Task.GetAwaiter();
								if (!val3.IsCompleted)
								{
									num = (_003C_003E1__state = 1);
									_003C_003Eu__2 = val3;
									_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<TResult>, _003CWaitAsync_003Ed__1<TResult>>(ref val3, ref this);
									return;
								}
							}
							else
							{
								val3 = _003C_003Eu__2;
								_003C_003Eu__2 = default(TaskAwaiter<TResult>);
								num = (_003C_003E1__state = -1);
							}
							val3.GetResult();
							result = _003C_003E8__1.tcs.Task.Result;
						}
						finally
						{
							if (num < 0)
							{
								((global::System.IDisposable)global::System.Runtime.CompilerServices.Unsafe.As<CancellationTokenRegistration, CancellationTokenRegistration>(ref _003C_003E7__wrap2)/*cast due to .constrained prefix*/).Dispose();
							}
						}
					}
					finally
					{
						if (num < 0 && _003ClinkedTokenSource_003E5__2 != null)
						{
							((global::System.IDisposable)_003ClinkedTokenSource_003E5__2).Dispose();
						}
					}
				}
				finally
				{
					if (num < 0 && _003C_003E8__1.timeoutCancelTokenSource != null)
					{
						((global::System.IDisposable)_003C_003E8__1.timeoutCancelTokenSource).Dispose();
					}
				}
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003E8__1 = null;
				_003ClinkedTokenSource_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003E8__1 = null;
			_003ClinkedTokenSource_003E5__2 = null;
			_003C_003Et__builder.SetResult(result);
			void CancelTcs()
			{
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Expected O, but got Unknown
				if (((_003C_003Ec__DisplayClass1_0<TResult>)(object)this).updateTcs && !((global::System.Threading.Tasks.Task)((_003C_003Ec__DisplayClass1_0<TResult>)(object)this).tcs.Task).IsCompleted)
				{
					CancellationTokenSource obj4 = ((_003C_003Ec__DisplayClass1_0<TResult>)(object)this).timeoutCancelTokenSource;
					if (obj4 != null && obj4.IsCancellationRequested)
					{
						((_003C_003Ec__DisplayClass1_0<TResult>)(object)this).tcs.TrySetException((global::System.Exception)new TimeoutException($"WaitAsync timed out after {((_003C_003Ec__DisplayClass1_0<TResult>)(object)this).timeoutMs}ms"));
					}
					else
					{
						((_003C_003Ec__DisplayClass1_0<TResult>)(object)this).tcs.TrySetCanceled();
					}
				}
				((_003C_003Ec__DisplayClass1_0<TResult>)(object)this).overrideTcs.TrySetResult(default(TResult));
			}
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[AsyncStateMachine(typeof(_003CTryWaitAsync_003Ed__0<>))]
	public static async global::System.Threading.Tasks.Task<TResult?> TryWaitAsync<TResult>(this TaskCompletionSource<TResult> tcs, CancellationToken cancelToken, int timeoutMs = -1, bool updateTcs = false, TResult? failureResult = default(TResult?))
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return await tcs.WaitAsync<TResult>(cancelToken, timeoutMs, updateTcs);
		}
		catch (global::System.Exception)
		{
			return failureResult;
		}
	}

	[AsyncStateMachine(typeof(_003CWaitAsync_003Ed__1<>))]
	public unsafe static async global::System.Threading.Tasks.Task<TResult> WaitAsync<TResult>(this TaskCompletionSource<TResult> tcs, CancellationToken cancelToken, int timeoutMs = -1, bool updateTcs = false)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		TaskCompletionSource<TResult> overrideTcs = new TaskCompletionSource<TResult>();
		CancellationTokenSource timeoutCancelTokenSource = ((timeoutMs <= 0 || timeoutMs == -1) ? ((CancellationTokenSource)null) : new CancellationTokenSource(timeoutMs));
		try
		{
			CancellationTokenSource obj = timeoutCancelTokenSource;
			CancellationToken val = ((obj != null) ? obj.Token : CancellationToken.None);
			CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancelToken, val);
			try
			{
				CancellationToken token = linkedTokenSource.Token;
				CancellationTokenRegistration val2 = ((CancellationToken)(ref token)).Register(new Action(CancelTcs));
				try
				{
					try
					{
						await global::System.Threading.Tasks.Task.WhenAny<TResult>(tcs.Task, overrideTcs.Task);
					}
					catch
					{
					}
					if (((global::System.Threading.Tasks.Task)tcs.Task).IsCompleted)
					{
						await tcs.Task;
						return tcs.Task.Result;
					}
					CancellationTokenSource obj3 = timeoutCancelTokenSource;
					if (obj3 != null && obj3.IsCancellationRequested)
					{
						throw new TimeoutException($"WaitAsync timed out after {timeoutMs}ms");
					}
					throw new OperationCanceledException();
				}
				finally
				{
					((global::System.IDisposable)(*(CancellationTokenRegistration*)(&val2))/*cast due to .constrained prefix*/).Dispose();
				}
			}
			finally
			{
				((global::System.IDisposable)linkedTokenSource)?.Dispose();
			}
		}
		finally
		{
			if (timeoutCancelTokenSource != null)
			{
				((global::System.IDisposable)timeoutCancelTokenSource).Dispose();
			}
		}
		void CancelTcs()
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			if (updateTcs && !((global::System.Threading.Tasks.Task)tcs.Task).IsCompleted)
			{
				CancellationTokenSource obj4 = timeoutCancelTokenSource;
				if (obj4 != null && obj4.IsCancellationRequested)
				{
					tcs.TrySetException((global::System.Exception)new TimeoutException($"WaitAsync timed out after {timeoutMs}ms"));
				}
				else
				{
					tcs.TrySetCanceled();
				}
			}
			overrideTcs.TrySetResult(default(TResult));
		}
	}
}
