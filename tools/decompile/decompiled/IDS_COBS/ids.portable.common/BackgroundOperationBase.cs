using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public abstract class BackgroundOperationBase
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass13_0
	{
		[StructLayout((LayoutKind)3)]
		private struct _003C_003CBackgroundOperationStart_003Eb__0_003Ed : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public _003C_003Ec__DisplayClass13_0 _003C_003E4__this;

			private TaskAwaiter _003C_003Eu__1;

			private void MoveNext()
			{
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				int num = _003C_003E1__state;
				_003C_003Ec__DisplayClass13_0 _003C_003Ec__DisplayClass13_ = _003C_003E4__this;
				try
				{
					try
					{
						TaskAwaiter val;
						if (num == 0)
						{
							val = _003C_003Eu__1;
							_003C_003Eu__1 = default(TaskAwaiter);
							num = (_003C_003E1__state = -1);
							goto IL_008b;
						}
						if (!((CancellationToken)(ref _003C_003Ec__DisplayClass13_.cancelToken)).IsCancellationRequested)
						{
							val = _003C_003Ec__DisplayClass13_._003C_003E4__this.BackgroundOperationAsync(_003C_003Ec__DisplayClass13_.args, _003C_003Ec__DisplayClass13_.cancelToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003C_003CBackgroundOperationStart_003Eb__0_003Ed>(ref val, ref this);
								return;
							}
							goto IL_008b;
						}
						goto end_IL_0011;
						IL_008b:
						((TaskAwaiter)(ref val)).GetResult();
						end_IL_0011:;
					}
					catch (OperationCanceledException)
					{
						_003C_003Ec__DisplayClass13_._003C_003E4__this._cancellationTokenSource.TryCancel();
						TaggedLog.Debug("BackgroundOperationBase", "Background Operation was canceled", string.Empty);
					}
					catch (global::System.Exception ex2)
					{
						TaggedLog.Error("BackgroundOperationBase", "Background Operation threw Exception {0}", ex2.Message);
					}
					finally
					{
						if (num < 0)
						{
							object locker = _003C_003Ec__DisplayClass13_._003C_003E4__this.Locker;
							bool flag = false;
							try
							{
								Monitor.Enter(locker, ref flag);
								_003C_003Ec__DisplayClass13_._003C_003E4__this.Started = false;
								if (((CancellationToken)(ref _003C_003Ec__DisplayClass13_.cancelToken)).IsCancellationRequested && _003C_003Ec__DisplayClass13_._003C_003E4__this._restartRequested)
								{
									_003C_003Ec__DisplayClass13_._003C_003E4__this.BackgroundOperationStart(_003C_003Ec__DisplayClass13_._003C_003E4__this._restartArgs);
								}
							}
							finally
							{
								if (num < 0 && flag)
								{
									Monitor.Exit(locker);
								}
							}
						}
					}
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

		public CancellationToken cancelToken;

		public BackgroundOperationBase _003C_003E4__this;

		public object[] args;

		[AsyncStateMachine(typeof(_003C_003CBackgroundOperationStart_003Eb__0_003Ed))]
		internal global::System.Threading.Tasks.Task? _003CBackgroundOperationStart_003Eb__0()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_003C_003CBackgroundOperationStart_003Eb__0_003Ed _003C_003CBackgroundOperationStart_003Eb__0_003Ed = default(_003C_003CBackgroundOperationStart_003Eb__0_003Ed);
			_003C_003CBackgroundOperationStart_003Eb__0_003Ed._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
			_003C_003CBackgroundOperationStart_003Eb__0_003Ed._003C_003E4__this = this;
			_003C_003CBackgroundOperationStart_003Eb__0_003Ed._003C_003E1__state = -1;
			((AsyncTaskMethodBuilder)(ref _003C_003CBackgroundOperationStart_003Eb__0_003Ed._003C_003Et__builder)).Start<_003C_003CBackgroundOperationStart_003Eb__0_003Ed>(ref _003C_003CBackgroundOperationStart_003Eb__0_003Ed);
			return ((AsyncTaskMethodBuilder)(ref _003C_003CBackgroundOperationStart_003Eb__0_003Ed._003C_003Et__builder)).Task;
		}
	}

	private const string LogTag = "BackgroundOperationBase";

	protected readonly object Locker = new object();

	private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

	private bool _restartRequested;

	private object[]? _restartArgs;

	[field: CompilerGenerated]
	public bool Started
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public bool StartedOrWillStart
	{
		get
		{
			lock (Locker)
			{
				return Started || _restartRequested;
			}
		}
	}

	protected BackgroundOperationBase()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		Started = false;
	}

	protected abstract global::System.Threading.Tasks.Task BackgroundOperationAsync(object[]? args, CancellationToken cancellationToken);

	protected virtual void BackgroundOperationStart(object[]? args)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		_003C_003Ec__DisplayClass13_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass13_0();
		CS_0024_003C_003E8__locals5._003C_003E4__this = this;
		CS_0024_003C_003E8__locals5.args = args;
		lock (Locker)
		{
			if (Started)
			{
				if (!_restartRequested)
				{
					_restartRequested = true;
					_restartArgs = CS_0024_003C_003E8__locals5.args;
				}
				return;
			}
			_restartRequested = false;
			Started = true;
			_cancellationTokenSource.TryCancelAndDispose();
			_cancellationTokenSource = new CancellationTokenSource();
			CS_0024_003C_003E8__locals5.cancelToken = _cancellationTokenSource.Token;
		}
		global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(_003C_003Ec__DisplayClass13_0._003C_003CBackgroundOperationStart_003Eb__0_003Ed))] () =>
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_003C_003Ec__DisplayClass13_0._003C_003CBackgroundOperationStart_003Eb__0_003Ed _003C_003CBackgroundOperationStart_003Eb__0_003Ed = default(_003C_003Ec__DisplayClass13_0._003C_003CBackgroundOperationStart_003Eb__0_003Ed);
			_003C_003CBackgroundOperationStart_003Eb__0_003Ed._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
			_003C_003CBackgroundOperationStart_003Eb__0_003Ed._003C_003E4__this = CS_0024_003C_003E8__locals5;
			_003C_003CBackgroundOperationStart_003Eb__0_003Ed._003C_003E1__state = -1;
			((AsyncTaskMethodBuilder)(ref _003C_003CBackgroundOperationStart_003Eb__0_003Ed._003C_003Et__builder)).Start<_003C_003Ec__DisplayClass13_0._003C_003CBackgroundOperationStart_003Eb__0_003Ed>(ref _003C_003CBackgroundOperationStart_003Eb__0_003Ed);
			return ((AsyncTaskMethodBuilder)(ref _003C_003CBackgroundOperationStart_003Eb__0_003Ed._003C_003Et__builder)).Task;
		}), CancellationToken.None);
	}

	protected virtual void BackgroundOperationStop()
	{
		lock (Locker)
		{
			if (!_restartRequested)
			{
				_restartArgs = null;
			}
			_restartRequested = false;
			_cancellationTokenSource.TryCancelAndDispose();
		}
	}
}
