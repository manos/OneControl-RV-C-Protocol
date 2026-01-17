using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public class Sync
{
	private class CheckedLockRecord
	{
		public string Tag = "CheckedLockRecord";

		public string? Description;

		[field: CompilerGenerated]
		public Stopwatch Stopwatch
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		} = new Stopwatch();
	}

	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		[StructLayout((LayoutKind)3)]
		private struct _003C_003CCheckedLock_003Eb__7_0_003Ed : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncVoidMethodBuilder _003C_003Et__builder;

			private TaskAwaiter _003C_003Eu__1;

			private void MoveNext()
			{
				//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_007a: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_0096: Unknown result type (might be due to invalid IL or missing references)
				int num = _003C_003E1__state;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						object lockCheckSync = _lockCheckSync;
						bool flag = false;
						try
						{
							Monitor.Enter(lockCheckSync, ref flag);
							Enumerator<CheckedLockRecord> enumerator = _activeLockRecordList.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									_ = enumerator.Current.Stopwatch.ElapsedMilliseconds;
									_ = 2000;
								}
							}
							finally
							{
								if (num < 0)
								{
									((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
								}
							}
						}
						finally
						{
							if (num < 0 && flag)
							{
								Monitor.Exit(lockCheckSync);
							}
						}
						val = global::System.Threading.Tasks.Task.Delay(500).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							((AsyncVoidMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003C_003CCheckedLock_003Eb__7_0_003Ed>(ref val, ref this);
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
				}
				catch (global::System.Exception exception)
				{
					_003C_003E1__state = -2;
					((AsyncVoidMethodBuilder)(ref _003C_003Et__builder)).SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				((AsyncVoidMethodBuilder)(ref _003C_003Et__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncVoidMethodBuilder)(ref _003C_003Et__builder)).SetStateMachine(stateMachine);
			}
		}

		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Action _003C_003E9__7_0;

		[AsyncStateMachine(typeof(_003C_003CCheckedLock_003Eb__7_0_003Ed))]
		internal void _003CCheckedLock_003Eb__7_0()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_003C_003CCheckedLock_003Eb__7_0_003Ed _003C_003CCheckedLock_003Eb__7_0_003Ed = default(_003C_003CCheckedLock_003Eb__7_0_003Ed);
			_003C_003CCheckedLock_003Eb__7_0_003Ed._003C_003Et__builder = AsyncVoidMethodBuilder.Create();
			_003C_003CCheckedLock_003Eb__7_0_003Ed._003C_003E1__state = -1;
			((AsyncVoidMethodBuilder)(ref _003C_003CCheckedLock_003Eb__7_0_003Ed._003C_003Et__builder)).Start<_003C_003CCheckedLock_003Eb__7_0_003Ed>(ref _003C_003CCheckedLock_003Eb__7_0_003Ed);
		}
	}

	private const string LogTag = "Sync";

	private const int CheckTimeoutMs = 2000;

	private static object _lockCheckSync = new object();

	private static global::System.Threading.Tasks.Task? _lockCheckerTask;

	private static List<CheckedLockRecord> _availableLockRecordList = new List<CheckedLockRecord>();

	private static List<CheckedLockRecord> _activeLockRecordList = new List<CheckedLockRecord>();

	public static void CheckedLock(object obj, string tag, [CallerMemberName] string description = "")
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		CheckedLockRecord checkedLockRecord = null;
		try
		{
			lock (_lockCheckSync)
			{
				if (_lockCheckerTask == null)
				{
					object obj2 = _003C_003Ec._003C_003E9__7_0;
					if (obj2 == null)
					{
						Action val = [AsyncStateMachine(typeof(_003C_003Ec._003C_003CCheckedLock_003Eb__7_0_003Ed))] () =>
						{
							//IL_0002: Unknown result type (might be due to invalid IL or missing references)
							//IL_0007: Unknown result type (might be due to invalid IL or missing references)
							_003C_003Ec._003C_003CCheckedLock_003Eb__7_0_003Ed _003C_003CCheckedLock_003Eb__7_0_003Ed = default(_003C_003Ec._003C_003CCheckedLock_003Eb__7_0_003Ed);
							_003C_003CCheckedLock_003Eb__7_0_003Ed._003C_003Et__builder = AsyncVoidMethodBuilder.Create();
							_003C_003CCheckedLock_003Eb__7_0_003Ed._003C_003E1__state = -1;
							((AsyncVoidMethodBuilder)(ref _003C_003CCheckedLock_003Eb__7_0_003Ed._003C_003Et__builder)).Start<_003C_003Ec._003C_003CCheckedLock_003Eb__7_0_003Ed>(ref _003C_003CCheckedLock_003Eb__7_0_003Ed);
						};
						_003C_003Ec._003C_003E9__7_0 = val;
						obj2 = (object)val;
					}
					_lockCheckerTask = new global::System.Threading.Tasks.Task((Action)obj2);
				}
				if (_availableLockRecordList.Count > 0)
				{
					checkedLockRecord = _availableLockRecordList[0];
					_availableLockRecordList.RemoveAt(0);
				}
				else
				{
					checkedLockRecord = new CheckedLockRecord();
				}
				_activeLockRecordList.Add(checkedLockRecord);
				checkedLockRecord.Tag = tag;
				checkedLockRecord.Description = description;
				checkedLockRecord.Stopwatch.Restart();
			}
			lock (obj)
			{
			}
			lock (_lockCheckSync)
			{
				checkedLockRecord.Tag = tag;
				checkedLockRecord.Description = description;
				checkedLockRecord.Stopwatch.Stop();
				_activeLockRecordList.Remove(checkedLockRecord);
				_availableLockRecordList.Add(checkedLockRecord);
			}
		}
		catch (global::System.Exception)
		{
		}
	}

	public static void AnnotatedLock(object obj, string tag, Action method, [CallerMemberName] string description = "")
	{
		TaggedLog.Debug("Sync", "{0} - {1} Start()", tag, description);
		lock (obj)
		{
			method.Invoke();
		}
		TaggedLog.Debug("Sync", "{0} - {1} Stop()", tag, description);
	}

	public static void Lock(object obj, string tag, Action method, [CallerMemberName] string description = "")
	{
		lock (obj)
		{
			method.Invoke();
		}
	}

	public static TResult AnnotatedLock<TResult>(object obj, string tag, Func<TResult> method, [CallerMemberName] string description = "")
	{
		TaggedLog.Debug("Sync", "{0} - {1} Start()", tag, description);
		TResult result;
		lock (obj)
		{
			result = method.Invoke();
		}
		TaggedLog.Debug("Sync", "{0} - {1} Stop()", tag, description);
		return result;
	}

	public static TResult Lock<TResult>(object obj, string tag, Func<TResult> method, [CallerMemberName] string description = "")
	{
		lock (obj)
		{
			return method.Invoke();
		}
	}
}
