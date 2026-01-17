using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public class AwaitableValue<TValue> : CommonDisposable
{
	private class ValueRequest : CommonDisposable
	{
		private readonly TaskCompletionSource<TValue> _tcs;

		private readonly CancellationTokenSource _cts;

		private readonly CancellationTokenRegistration _ctsCanceledTokenReg;

		public global::System.Threading.Tasks.Task<TValue> Task => _tcs.Task;

		public ValueRequest(CancellationToken sourceCancellationToken)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			_tcs = new TaskCompletionSource<TValue>();
			_cts = CancellationTokenSource.CreateLinkedTokenSource(sourceCancellationToken);
			CancellationToken token = _cts.Token;
			_ctsCanceledTokenReg = ((CancellationToken)(ref token)).Register((Action)([CompilerGenerated] () =>
			{
				_tcs.TrySetCanceled();
			}));
			_tcs.Task.ConfigureAwait(false);
		}

		public void SetResult(TValue value)
		{
			_tcs.TrySetResult(value);
		}

		public override void Dispose(bool disposing)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((global::System.IDisposable)(object)_ctsCanceledTokenReg).TryDispose();
			_cts.TryCancelAndDispose();
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetAsync_003Ed__10 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<TValue> _003C_003Et__builder;

		public TimeSpan timeout;

		public AwaitableValue<TValue> _003C_003E4__this;

		private CancellationTokenSource _003Ccts_003E5__2;

		private TaskAwaiter<TValue> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			AwaitableValue<TValue> awaitableValue = _003C_003E4__this;
			TValue result;
			try
			{
				if (num != 0)
				{
					_003Ccts_003E5__2 = new CancellationTokenSource(timeout);
				}
				try
				{
					TaskAwaiter<TValue> val;
					if (num != 0)
					{
						val = awaitableValue.GetAsync(_003Ccts_003E5__2.Token).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<TValue>, _003CGetAsync_003Ed__10>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<TValue>);
						num = (_003C_003E1__state = -1);
					}
					result = val.GetResult();
				}
				finally
				{
					if (num < 0 && _003Ccts_003E5__2 != null)
					{
						((global::System.IDisposable)_003Ccts_003E5__2).Dispose();
					}
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

	private bool _valueSet;

	private TValue _value;

	private readonly List<ValueRequest> _pendingRequestList = (List<ValueRequest>)(object)new List<AwaitableValue<ValueRequest>.ValueRequest>();

	private readonly global::System.Collections.Generic.IEnumerable<TValue> _acceptedValues;

	private readonly object _syncSet = new object();

	public AwaitableValue()
	{
	}

	public AwaitableValue(TValue acceptVal)
	{
		List<TValue> obj = new List<TValue>();
		obj.Add(acceptVal);
		_acceptedValues = (global::System.Collections.Generic.IEnumerable<TValue>)obj;
	}

	public AwaitableValue(global::System.Collections.Generic.IEnumerable<TValue> acceptedValues)
	{
		_acceptedValues = acceptedValues;
	}

	public global::System.Threading.Tasks.Task<TValue> GetAsync(int msTimeout)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return this.GetAsync(new TimeSpan(0, 0, 0, 0, msTimeout));
	}

	[AsyncStateMachine(typeof(AwaitableValue<>._003CGetAsync_003Ed__10))]
	public async global::System.Threading.Tasks.Task<TValue> GetAsync(TimeSpan timeout)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		CancellationTokenSource cts = new CancellationTokenSource(timeout);
		try
		{
			return await GetAsync(cts.Token);
		}
		finally
		{
			((global::System.IDisposable)cts)?.Dispose();
		}
	}

	public global::System.Threading.Tasks.Task<TValue> GetAsync(CancellationToken ct)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		lock (_syncSet)
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException("AwaitableValue");
			}
			if (_valueSet)
			{
				return global::System.Threading.Tasks.Task.FromResult<TValue>(_value);
			}
			ValueRequest valueRequest = new ValueRequest(ct);
			((List<AwaitableValue<ValueRequest>.ValueRequest>)(object)_pendingRequestList).Add((AwaitableValue<ValueRequest>.ValueRequest)(object)valueRequest);
			return valueRequest.Task;
		}
	}

	public void Reset()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		lock (_syncSet)
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException("AwaitableValue");
			}
			_valueSet = false;
		}
	}

	public unsafe void Set(TValue value)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		lock (_syncSet)
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException("AwaitableValue");
			}
			global::System.Collections.Generic.IEnumerable<TValue> acceptedValues = _acceptedValues;
			if (acceptedValues != null && !Enumerable.Contains<TValue>(acceptedValues, value))
			{
				return;
			}
			_valueSet = true;
			_value = value;
			Enumerator<ValueRequest> enumerator = ((List<AwaitableValue<ValueRequest>.ValueRequest>)(object)_pendingRequestList).GetEnumerator();
			try
			{
				while (((Enumerator<AwaitableValue<ValueRequest>.ValueRequest>*)(&enumerator))->MoveNext())
				{
					ValueRequest current = ((Enumerator<AwaitableValue<ValueRequest>.ValueRequest>*)(&enumerator))->Current;
					current.SetResult(value);
					current.TryDispose();
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
			((List<AwaitableValue<ValueRequest>.ValueRequest>)(object)_pendingRequestList).Clear();
		}
	}

	public unsafe override void Dispose(bool disposing)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		lock (_syncSet)
		{
			Enumerator<ValueRequest> enumerator = ((List<AwaitableValue<ValueRequest>.ValueRequest>)(object)_pendingRequestList).GetEnumerator();
			try
			{
				while (((Enumerator<AwaitableValue<ValueRequest>.ValueRequest>*)(&enumerator))->MoveNext())
				{
					((Enumerator<AwaitableValue<ValueRequest>.ValueRequest>*)(&enumerator))->Current.TryDispose();
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
			((List<AwaitableValue<ValueRequest>.ValueRequest>)(object)_pendingRequestList).Clear();
		}
	}
}
