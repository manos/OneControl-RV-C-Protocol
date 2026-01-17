using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ids.portable.common;

namespace IDS.Portable.Common;

public class AsyncValueCached<TValue> : CommonDisposableNotifyPropertyChanged, IAsyncValueCached<TValue>
{
	private Stopwatch? _valueExpireTrackingTimer;

	private readonly object _lock = new object();

	private TValue _value;

	private AsyncValueCachedOperation<TValue>? _asyncUpdateOperationActive;

	public int ValueGoodForMs
	{
		[CompilerGenerated]
		get;
	}

	public TValue Value
	{
		get
		{
			lock (_lock)
			{
				return (_asyncUpdateOperationActive != null) ? _asyncUpdateOperationActive.ValueNew : _value;
			}
		}
		set
		{
			lock (_lock)
			{
				if (_asyncUpdateOperationActive != null)
				{
					_asyncUpdateOperationActive.ValueToRevertTo = value;
				}
				else
				{
					SetRealOrPendingValue(value);
				}
			}
		}
	}

	public ValueTuple<TValue, AsyncValueCachedState> ValueAndState
	{
		get
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			lock (_lock)
			{
				return new ValueTuple<TValue, AsyncValueCachedState>(_value, State);
			}
		}
	}

	public bool HasValue => _valueExpireTrackingTimer != null;

	public bool NeedsUpdate
	{
		get
		{
			lock (_lock)
			{
				int result;
				if (HasValue && _valueExpireTrackingTimer != null && ValueGoodForMs != 0 && !_valueExpireTrackingTimer.IsStopped())
				{
					Stopwatch? valueExpireTrackingTimer = _valueExpireTrackingTimer;
					result = ((((valueExpireTrackingTimer != null) ? new long?(valueExpireTrackingTimer.ElapsedMilliseconds) : ((long?)null)) > ValueGoodForMs) ? 1 : 0);
				}
				else
				{
					result = 1;
				}
				return (byte)result != 0;
			}
		}
	}

	public bool IsAsyncUpdating => _asyncUpdateOperationActive != null;

	public AsyncValueCachedState State
	{
		get
		{
			if (!HasValue)
			{
				return AsyncValueCachedState.NoValue;
			}
			if (IsAsyncUpdating)
			{
				return AsyncValueCachedState.HasValueUpdating;
			}
			if (NeedsUpdate)
			{
				return AsyncValueCachedState.HasValueNeedsUpdate;
			}
			return AsyncValueCachedState.HasValue;
		}
	}

	private void SetRealOrPendingValue(TValue value, bool revertingValue = false)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		lock (_lock)
		{
			if (!revertingValue)
			{
				if (_valueExpireTrackingTimer == null)
				{
					_valueExpireTrackingTimer = new Stopwatch();
				}
				_valueExpireTrackingTimer.Restart();
			}
			SetBackingField(ref _value, value, "Value", "HasValue", "NeedsUpdate", "IsAsyncUpdating", "State");
		}
	}

	public void InvalidateCache()
	{
		Stopwatch? valueExpireTrackingTimer = _valueExpireTrackingTimer;
		if (valueExpireTrackingTimer != null)
		{
			valueExpireTrackingTimer.Stop();
		}
	}

	public AsyncValueCached(int valueGoodForMs)
	{
		ValueGoodForMs = valueGoodForMs;
	}

	public AsyncValueCached(TValue initialValue, int valueGoodForMs)
		: this(valueGoodForMs)
	{
		Value = initialValue;
	}

	public AsyncValueCachedOperation<TValue> AsyncUpdateStart(TValue value)
	{
		lock (_lock)
		{
			_asyncUpdateOperationActive = new AsyncValueCachedOperation<TValue>(Value, value);
			SetRealOrPendingValue(value);
			return _asyncUpdateOperationActive;
		}
	}

	public void AsyncUpdateComplete(AsyncValueCachedOperation<TValue>? asyncUpdateOperation)
	{
		if (asyncUpdateOperation == null)
		{
			return;
		}
		lock (_lock)
		{
			if (_asyncUpdateOperationActive == asyncUpdateOperation)
			{
				_asyncUpdateOperationActive = null;
				SetRealOrPendingValue(asyncUpdateOperation.ValueNew);
			}
		}
	}

	public void AsyncUpdateFailed(AsyncValueCachedOperation<TValue>? asyncUpdateOperation)
	{
		if (asyncUpdateOperation == null)
		{
			return;
		}
		lock (_lock)
		{
			if (_asyncUpdateOperationActive == asyncUpdateOperation)
			{
				_asyncUpdateOperationActive = null;
				SetRealOrPendingValue(asyncUpdateOperation.ValueToRevertTo, revertingValue: true);
			}
		}
	}
}
