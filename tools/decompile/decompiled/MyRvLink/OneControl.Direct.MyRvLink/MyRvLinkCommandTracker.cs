using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDS.Portable.Common;

namespace OneControl.Direct.MyRvLink;

internal class MyRvLinkCommandTracker : CommonDisposable
{
	private const string LogTag = "MyRvLinkCommandTracker";

	public readonly IMyRvLinkCommand Command;

	private readonly TaskCompletionSource<IMyRvLinkCommandResponse> _waitingForCommandCompletedTcs;

	private TaskCompletionSource<IMyRvLinkCommandResponse>? _waitingForAnyResponseTcs;

	private readonly CancellationTokenSource _cts;

	private readonly CancellationTokenRegistration _cancellationTokenRegistration;

	private Action<IMyRvLinkCommandResponse>? _responseCallback;

	private readonly int _timeoutMs;

	private TaskCompletionSource<IMyRvLinkCommandResponseFailure?>? _waitingForFailureTcs;

	private IMyRvLinkCommandResponse? _lastResponseReceivedWithNoOneWaiting;

	private readonly object _lock = new object();

	public bool IsCompleted => ((global::System.Threading.Tasks.Task)_waitingForCommandCompletedTcs.Task).IsCompleted;

	public MyRvLinkCommandTracker(IMyRvLinkCommand command, CancellationToken cancelToken, int timeoutMs, Action<IMyRvLinkCommandResponse>? responseCallback)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		MyRvLinkCommandTracker myRvLinkCommandTracker = this;
		Command = command;
		_timeoutMs = timeoutMs;
		_waitingForCommandCompletedTcs = new TaskCompletionSource<IMyRvLinkCommandResponse>((TaskCreationOptions)64);
		_responseCallback = responseCallback;
		_cts = CancellationTokenSource.CreateLinkedTokenSource(cancelToken);
		CancellationToken token = _cts.Token;
		_cancellationTokenRegistration = ((CancellationToken)(ref token)).Register((Action)delegate
		{
			if (!myRvLinkCommandTracker.IsCompleted)
			{
				myRvLinkCommandTracker.TrySetFailure(((CancellationToken)(ref cancelToken)).IsCancellationRequested ? MyRvLinkCommandResponseFailureCode.CommandAborted : MyRvLinkCommandResponseFailureCode.CommandTimeout);
			}
		});
		ResetTimer();
	}

	public override void Dispose(bool disposing)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if (!((global::System.Threading.Tasks.Task)_waitingForCommandCompletedTcs.Task).IsCompleted)
		{
			MyRvLinkCommandResponseFailure commandResponse = new MyRvLinkCommandResponseFailure(Command.ClientCommandId, MyRvLinkCommandResponseFailureCode.CommandAborted);
			ProcessResponse(commandResponse, forceCompleteCommand: true);
		}
		IDisposableExtensions.TryDispose((global::System.IDisposable)(object)_cancellationTokenRegistration);
		IDisposableExtensions.TryDispose((global::System.IDisposable)_cts);
		_waitingForFailureTcs?.TrySetCanceled();
		_responseCallback = null;
	}

	public void ResetTimer()
	{
		_cts.CancelAfter(_timeoutMs);
	}

	public global::System.Threading.Tasks.Task<IMyRvLinkCommandResponse> WaitAsync()
	{
		return _waitingForCommandCompletedTcs.Task;
	}

	public global::System.Threading.Tasks.Task<IMyRvLinkCommandResponse> WaitForAnyResponse()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			if (((CommonDisposable)this).IsDisposed)
			{
				throw new ObjectDisposedException("MyRvLinkCommandTracker");
			}
			try
			{
				if (IsCompleted)
				{
					return _waitingForCommandCompletedTcs.Task;
				}
				TaskCompletionSource<IMyRvLinkCommandResponse> waitingForAnyResponseTcs = _waitingForAnyResponseTcs;
				if (waitingForAnyResponseTcs != null && !((global::System.Threading.Tasks.Task)waitingForAnyResponseTcs.Task).IsCompleted)
				{
					return waitingForAnyResponseTcs.Task;
				}
				_waitingForAnyResponseTcs = new TaskCompletionSource<IMyRvLinkCommandResponse>((TaskCreationOptions)64);
				if (_lastResponseReceivedWithNoOneWaiting != null)
				{
					_waitingForAnyResponseTcs.SetResult(_lastResponseReceivedWithNoOneWaiting);
				}
				return _waitingForAnyResponseTcs.Task;
			}
			finally
			{
				_lastResponseReceivedWithNoOneWaiting = null;
			}
		}
	}

	public global::System.Threading.Tasks.Task<IMyRvLinkCommandResponseFailure?> TryWaitForAnyFailure(TimeSpan timeout, CancellationToken cancellationToken)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		TaskCompletionSource<IMyRvLinkCommandResponseFailure> val;
		lock (_lock)
		{
			if (_waitingForFailureTcs != null && !((global::System.Threading.Tasks.Task)_waitingForFailureTcs.Task).IsCompleted)
			{
				return _waitingForFailureTcs.Task;
			}
			val = (_waitingForFailureTcs = new TaskCompletionSource<IMyRvLinkCommandResponseFailure>((TaskCreationOptions)64));
		}
		return TaskCompletionSourceExtension.TryWaitAsync<IMyRvLinkCommandResponseFailure>(val, cancellationToken, (int)((TimeSpan)(ref timeout)).TotalMilliseconds, true, (IMyRvLinkCommandResponseFailure)null);
	}

	public void ProcessResponse(IMyRvLinkCommandResponse commandResponse, bool forceCompleteCommand)
	{
		lock (_lock)
		{
			if (!((CommonDisposable)this).IsDisposed)
			{
				_responseCallback?.Invoke(commandResponse);
			}
			if (forceCompleteCommand || commandResponse.IsCommandCompleted)
			{
				_waitingForCommandCompletedTcs.TrySetResult(commandResponse);
				_lastResponseReceivedWithNoOneWaiting = null;
			}
			_waitingForAnyResponseTcs?.TrySetResult(commandResponse);
			if (_waitingForAnyResponseTcs == null || ((global::System.Threading.Tasks.Task)_waitingForAnyResponseTcs.Task).IsCompleted)
			{
				_lastResponseReceivedWithNoOneWaiting = commandResponse;
			}
			if (!(commandResponse is IMyRvLinkCommandResponseSuccess))
			{
				if (commandResponse is IMyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
				{
					_waitingForFailureTcs?.TrySetResult(myRvLinkCommandResponseFailure);
				}
			}
			else
			{
				_waitingForFailureTcs?.TrySetCanceled();
			}
		}
	}

	public IMyRvLinkCommandResponseFailure TrySetFailure(MyRvLinkCommandResponseFailureCode failureCode, global::System.Collections.Generic.IReadOnlyList<byte>? extendedData = null)
	{
		MyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure = new MyRvLinkCommandResponseFailure(Command.ClientCommandId, failureCode, extendedData);
		ProcessResponse(myRvLinkCommandResponseFailure, forceCompleteCommand: true);
		return myRvLinkCommandResponseFailure;
	}

	public override string ToString()
	{
		return $"[0x{Command.ClientCommandId:X4}] IsComplete: {IsCompleted} IsDisposed: {((CommonDisposable)this).IsDisposed} {Command}";
	}
}
