using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public class BackgroundOperation : BackgroundOperationBase, IBackgroundOperation
{
	public delegate global::System.Threading.Tasks.Task BackgroundOperationFunc(CancellationToken cancellationToken);

	public delegate void BackgroundOperationAction(CancellationToken cancellationToken);

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CBackgroundOperationAsync_003Ed__7 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public BackgroundOperation _003C_003E4__this;

		public CancellationToken cancellationToken;

		private TaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			BackgroundOperation backgroundOperation = _003C_003E4__this;
			try
			{
				TaskAwaiter val;
				if (num == 0)
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_0077;
				}
				if (backgroundOperation._backgroundOperation != null)
				{
					val = backgroundOperation._backgroundOperation(cancellationToken).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CBackgroundOperationAsync_003Ed__7>(ref val, ref this);
						return;
					}
					goto IL_0077;
				}
				goto end_IL_000e;
				IL_0077:
				((TaskAwaiter)(ref val)).GetResult();
				end_IL_000e:;
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

	private const string LogTag = "BackgroundOperation";

	private readonly BackgroundOperationFunc? _backgroundOperation;

	protected BackgroundOperation()
	{
		_backgroundOperation = null;
	}

	public BackgroundOperation(BackgroundOperationFunc operation)
		: this()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_backgroundOperation = operation ?? throw new ArgumentNullException("operation");
	}

	public BackgroundOperation(BackgroundOperationAction action)
		: this()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (action == null)
		{
			throw new ArgumentNullException("action");
		}
		_backgroundOperation = delegate(CancellationToken cancelToken)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			action?.Invoke(cancelToken);
			return global::System.Threading.Tasks.Task.CompletedTask;
		};
	}

	[AsyncStateMachine(typeof(_003CBackgroundOperationAsync_003Ed__7))]
	protected virtual global::System.Threading.Tasks.Task BackgroundOperationAsync(CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		_003CBackgroundOperationAsync_003Ed__7 _003CBackgroundOperationAsync_003Ed__ = default(_003CBackgroundOperationAsync_003Ed__7);
		_003CBackgroundOperationAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CBackgroundOperationAsync_003Ed__._003C_003E4__this = this;
		_003CBackgroundOperationAsync_003Ed__.cancellationToken = cancellationToken;
		_003CBackgroundOperationAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CBackgroundOperationAsync_003Ed__._003C_003Et__builder)).Start<_003CBackgroundOperationAsync_003Ed__7>(ref _003CBackgroundOperationAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CBackgroundOperationAsync_003Ed__._003C_003Et__builder)).Task;
	}

	protected override global::System.Threading.Tasks.Task BackgroundOperationAsync(object[]? args, CancellationToken cancellationToken)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return BackgroundOperationAsync(cancellationToken);
	}

	public virtual void Start()
	{
		BackgroundOperationStart(null);
	}

	public virtual void Stop()
	{
		BackgroundOperationStop();
	}
}
public class BackgroundOperation<TBackgroundArg1> : BackgroundOperationBase, IBackgroundOperation<TBackgroundArg1>
{
	public delegate global::System.Threading.Tasks.Task BackgroundOperationFunc(TBackgroundArg1 arg1, CancellationToken cancellationToken);

	public delegate void BackgroundOperationAction(TBackgroundArg1 arg1, CancellationToken cancellationToken);

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CBackgroundOperationAsync_003Ed__7 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public BackgroundOperation<TBackgroundArg1> _003C_003E4__this;

		public TBackgroundArg1 arg1;

		public CancellationToken cancellationToken;

		private TaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			BackgroundOperation<TBackgroundArg1> backgroundOperation = _003C_003E4__this;
			try
			{
				TaskAwaiter val;
				if (num == 0)
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_0080;
				}
				if (backgroundOperation._backgroundOperation != null)
				{
					val = backgroundOperation._backgroundOperation(arg1, cancellationToken).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CBackgroundOperationAsync_003Ed__7>(ref val, ref this);
						return;
					}
					goto IL_0080;
				}
				goto end_IL_000e;
				IL_0080:
				((TaskAwaiter)(ref val)).GetResult();
				end_IL_000e:;
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

	private const string LogTag = "BackgroundOperation";

	private readonly BackgroundOperationFunc? _backgroundOperation;

	protected BackgroundOperation()
	{
		_backgroundOperation = null;
	}

	public BackgroundOperation(BackgroundOperationFunc operation)
		: this()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_backgroundOperation = operation ?? throw new ArgumentNullException("operation");
	}

	public BackgroundOperation(BackgroundOperationAction action)
		: this()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (action == null)
		{
			throw new ArgumentNullException("action");
		}
		_backgroundOperation = delegate(TBackgroundArg1 arg1, CancellationToken cancelToken)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			action?.Invoke(arg1, cancelToken);
			return global::System.Threading.Tasks.Task.CompletedTask;
		};
	}

	[AsyncStateMachine(typeof(BackgroundOperation<>._003CBackgroundOperationAsync_003Ed__7))]
	protected virtual global::System.Threading.Tasks.Task BackgroundOperationAsync(TBackgroundArg1 arg1, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		_003CBackgroundOperationAsync_003Ed__7 _003CBackgroundOperationAsync_003Ed__ = default(_003CBackgroundOperationAsync_003Ed__7);
		_003CBackgroundOperationAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CBackgroundOperationAsync_003Ed__._003C_003E4__this = this;
		_003CBackgroundOperationAsync_003Ed__.arg1 = arg1;
		_003CBackgroundOperationAsync_003Ed__.cancellationToken = cancellationToken;
		_003CBackgroundOperationAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CBackgroundOperationAsync_003Ed__._003C_003Et__builder)).Start<_003CBackgroundOperationAsync_003Ed__7>(ref _003CBackgroundOperationAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CBackgroundOperationAsync_003Ed__._003C_003Et__builder)).Task;
	}

	protected sealed override global::System.Threading.Tasks.Task BackgroundOperationAsync(object[] args, CancellationToken cancellationToken)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (args.Length != 1 || !(args[0] is TBackgroundArg1 arg))
		{
			object[] args2 = new string[1] { "TBackgroundArg1" };
			TaggedLog.Error("BackgroundOperation", "Invalid argument, expected 1 parameter of type {0}", args2);
			throw new ArgumentException("Invalid argument, expected 1 parameter of type {nameof(TBackgroundArg1)}", "args");
		}
		return BackgroundOperationAsync(arg, cancellationToken);
	}

	public virtual void Start(TBackgroundArg1 arg1)
	{
		BackgroundOperationStart(new object[1] { arg1 });
	}

	public virtual void Stop()
	{
		BackgroundOperationStop();
	}
}
public class BackgroundOperation<TBackgroundArg1, TBackgroundArg2> : BackgroundOperationBase, IBackgroundOperation<TBackgroundArg1, TBackgroundArg2>
{
	public delegate global::System.Threading.Tasks.Task BackgroundOperationFunc(TBackgroundArg1 arg1, TBackgroundArg2 arg2, CancellationToken cancellationToken);

	public delegate void BackgroundOperationAction(TBackgroundArg1 arg1, TBackgroundArg2 arg2, CancellationToken cancellationToken);

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CBackgroundOperationAsync_003Ed__7 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public BackgroundOperation<TBackgroundArg1, TBackgroundArg2> _003C_003E4__this;

		public TBackgroundArg1 arg1;

		public TBackgroundArg2 arg2;

		public CancellationToken cancellationToken;

		private TaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			BackgroundOperation<TBackgroundArg1, TBackgroundArg2> backgroundOperation = _003C_003E4__this;
			try
			{
				TaskAwaiter val;
				if (num == 0)
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_0086;
				}
				if (backgroundOperation._backgroundOperation != null)
				{
					val = backgroundOperation._backgroundOperation(arg1, arg2, cancellationToken).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CBackgroundOperationAsync_003Ed__7>(ref val, ref this);
						return;
					}
					goto IL_0086;
				}
				goto end_IL_000e;
				IL_0086:
				((TaskAwaiter)(ref val)).GetResult();
				end_IL_000e:;
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

	private const string LogTag = "BackgroundOperation";

	private readonly BackgroundOperationFunc? _backgroundOperation;

	protected BackgroundOperation()
	{
		_backgroundOperation = null;
	}

	public BackgroundOperation(BackgroundOperationFunc operation)
		: this()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_backgroundOperation = operation ?? throw new ArgumentNullException("operation");
	}

	public BackgroundOperation(BackgroundOperationAction action)
		: this()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (action == null)
		{
			throw new ArgumentNullException("action");
		}
		_backgroundOperation = delegate(TBackgroundArg1 arg1, TBackgroundArg2 arg2, CancellationToken cancelToken)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			action(arg1, arg2, cancelToken);
			return global::System.Threading.Tasks.Task.CompletedTask;
		};
	}

	[AsyncStateMachine(typeof(BackgroundOperation<, >._003CBackgroundOperationAsync_003Ed__7))]
	protected virtual global::System.Threading.Tasks.Task BackgroundOperationAsync(TBackgroundArg1 arg1, TBackgroundArg2 arg2, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		_003CBackgroundOperationAsync_003Ed__7 _003CBackgroundOperationAsync_003Ed__ = default(_003CBackgroundOperationAsync_003Ed__7);
		_003CBackgroundOperationAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CBackgroundOperationAsync_003Ed__._003C_003E4__this = this;
		_003CBackgroundOperationAsync_003Ed__.arg1 = arg1;
		_003CBackgroundOperationAsync_003Ed__.arg2 = arg2;
		_003CBackgroundOperationAsync_003Ed__.cancellationToken = cancellationToken;
		_003CBackgroundOperationAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CBackgroundOperationAsync_003Ed__._003C_003Et__builder)).Start<_003CBackgroundOperationAsync_003Ed__7>(ref _003CBackgroundOperationAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CBackgroundOperationAsync_003Ed__._003C_003Et__builder)).Task;
	}

	protected sealed override global::System.Threading.Tasks.Task BackgroundOperationAsync(object[]? args, CancellationToken cancellationToken)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (args != null && args.Length == 2 && args[0] is TBackgroundArg1 arg && args[1] is TBackgroundArg2 arg2)
		{
			return BackgroundOperationAsync(arg, arg2, cancellationToken);
		}
		TaggedLog.Error("BackgroundOperation", "Invalid argument, expected 2 parameters of type {0} and {1}", "TBackgroundArg1", "TBackgroundArg2");
		throw new ArgumentException("Invalid argument, expected 2 parameters of type TBackgroundArg1 and TBackgroundArg2", "args");
	}

	public virtual void Start(TBackgroundArg1 arg1, TBackgroundArg2 arg2)
	{
		BackgroundOperationStart(new object[2] { arg1, arg2 });
	}

	public virtual void Stop()
	{
		BackgroundOperationStop();
	}
}
