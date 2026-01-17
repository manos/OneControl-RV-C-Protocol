using System;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public static class MainThread
{
	public enum RunBehavior
	{
		Inline,
		Queue
	}

	private const string LogTag = "MainThread";

	private static int? _mainThreadId;

	public static Func<Func<Action, bool>>? RequestMainThreadActionFactory;

	public static ThreadContext CurrentThreadContext
	{
		get
		{
			if (!_mainThreadId.HasValue)
			{
				return ThreadContext.Unknown;
			}
			if (_mainThreadId.Value != Environment.CurrentManagedThreadId)
			{
				return ThreadContext.Other;
			}
			return ThreadContext.Main;
		}
	}

	public static bool RequestMainThreadAction(Action action, RunBehavior runBehavior = RunBehavior.Inline)
	{
		Func<Action, bool> val = RequestMainThreadActionFactory?.Invoke();
		if (val == null)
		{
			TaggedLog.Warning("MainThread", "WARNING: RequestMainThreadAction is NULL -- executing action in current context.", string.Empty);
			if (action != null)
			{
				action.Invoke();
			}
			return false;
		}
		switch (CurrentThreadContext)
		{
		case ThreadContext.Main:
			if (runBehavior == RunBehavior.Inline)
			{
				if (action != null)
				{
					action.Invoke();
				}
				return false;
			}
			return val.Invoke(action);
		default:
			return val.Invoke(action);
		}
	}

	public static global::System.Threading.Tasks.Task<bool> RequestMainThreadActionAsync(Action action)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
		RequestMainThreadAction((Action)delegate
		{
			try
			{
				Action obj = action;
				if (obj != null)
				{
					obj.Invoke();
				}
				tcs.TrySetResult(true);
			}
			catch
			{
				tcs.TrySetResult(false);
			}
		});
		return tcs.Task;
	}

	public static void UpdateMainThreadContext()
	{
		int? mainThreadId = _mainThreadId;
		_mainThreadId = Environment.CurrentManagedThreadId;
		if (mainThreadId.HasValue)
		{
			_ = mainThreadId == _mainThreadId;
		}
	}
}
