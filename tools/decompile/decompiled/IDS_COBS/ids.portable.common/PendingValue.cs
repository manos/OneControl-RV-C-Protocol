using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace IDS.Portable.Common;

public abstract class PendingValue<TValue> : CommonDisposable, IPendingValue<TValue>
{
	public delegate void ValueChangedHandler(TValue oldValue, TValue newValue);

	private const string LogTag = "PendingValue";

	public const int DefaultMaxPendingValueTimeMs = 5000;

	private Watchdog? _resetActiveValueWatchdog;

	private bool _pendingValueEnabled;

	private ValueChangedHandler? _valueChangedHandler;

	private readonly object _locker = new object();

	private TValue _lastKnownValue;

	protected virtual int MaxPendingValueTimeMs
	{
		[CompilerGenerated]
		get;
	}

	public TValue Value
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return CalculateValue().Item1;
		}
		set
		{
			SetPendingValue(value);
		}
	}

	protected abstract TValue AssignedValue { get; }

	protected TValue CurrentPendingValue
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public bool IsValuePending
	{
		get
		{
			if (_pendingValueEnabled)
			{
				return !AreEqual(_lastKnownValue, AssignedValue);
			}
			return false;
		}
	}

	public event ValueChangedHandler? ValueChanged
	{
		[CompilerGenerated]
		add
		{
			ValueChangedHandler valueChangedHandler = this.ValueChanged;
			ValueChangedHandler valueChangedHandler2;
			do
			{
				valueChangedHandler2 = valueChangedHandler;
				ValueChangedHandler valueChangedHandler3 = (ValueChangedHandler)global::System.Delegate.Combine((global::System.Delegate)valueChangedHandler2, (global::System.Delegate)value);
				valueChangedHandler = Interlocked.CompareExchange<ValueChangedHandler>(ref this.ValueChanged, valueChangedHandler3, valueChangedHandler2);
			}
			while (valueChangedHandler != valueChangedHandler2);
		}
		[CompilerGenerated]
		remove
		{
			ValueChangedHandler valueChangedHandler = this.ValueChanged;
			ValueChangedHandler valueChangedHandler2;
			do
			{
				valueChangedHandler2 = valueChangedHandler;
				ValueChangedHandler valueChangedHandler3 = (ValueChangedHandler)global::System.Delegate.Remove((global::System.Delegate)valueChangedHandler2, (global::System.Delegate)value);
				valueChangedHandler = Interlocked.CompareExchange<ValueChangedHandler>(ref this.ValueChanged, valueChangedHandler3, valueChangedHandler2);
			}
			while (valueChangedHandler != valueChangedHandler2);
		}
	}

	public event ValueChangedHandler? PendingValueChanged
	{
		[CompilerGenerated]
		add
		{
			ValueChangedHandler valueChangedHandler = this.PendingValueChanged;
			ValueChangedHandler valueChangedHandler2;
			do
			{
				valueChangedHandler2 = valueChangedHandler;
				ValueChangedHandler valueChangedHandler3 = (ValueChangedHandler)global::System.Delegate.Combine((global::System.Delegate)valueChangedHandler2, (global::System.Delegate)value);
				valueChangedHandler = Interlocked.CompareExchange<ValueChangedHandler>(ref this.PendingValueChanged, valueChangedHandler3, valueChangedHandler2);
			}
			while (valueChangedHandler != valueChangedHandler2);
		}
		[CompilerGenerated]
		remove
		{
			ValueChangedHandler valueChangedHandler = this.PendingValueChanged;
			ValueChangedHandler valueChangedHandler2;
			do
			{
				valueChangedHandler2 = valueChangedHandler;
				ValueChangedHandler valueChangedHandler3 = (ValueChangedHandler)global::System.Delegate.Remove((global::System.Delegate)valueChangedHandler2, (global::System.Delegate)value);
				valueChangedHandler = Interlocked.CompareExchange<ValueChangedHandler>(ref this.PendingValueChanged, valueChangedHandler3, valueChangedHandler2);
			}
			while (valueChangedHandler != valueChangedHandler2);
		}
	}

	protected PendingValue(TValue value, int maxPendingValueTimeMs, ValueChangedHandler? valueChanged)
	{
		MaxPendingValueTimeMs = maxPendingValueTimeMs;
		CurrentPendingValue = value;
		_pendingValueEnabled = false;
		_lastKnownValue = value;
		_valueChangedHandler = valueChanged;
		if (valueChanged != null)
		{
			ValueChanged += valueChanged;
		}
	}

	public static implicit operator TValue(PendingValue<TValue> pendingValue)
	{
		return pendingValue.Value;
	}

	protected bool AreEqual(TValue first, TValue second)
	{
		ref TValue reference = ref first;
		TValue val = default(TValue);
		if (val == null)
		{
			val = reference;
			reference = ref val;
			if (val == null)
			{
				return second == null;
			}
		}
		object obj = second;
		return ((object)reference/*cast due to .constrained prefix*/).Equals(obj);
	}

	protected ValueTuple<TValue, bool> CalculateValue()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		lock (_locker)
		{
			TValue val = (_pendingValueEnabled ? CurrentPendingValue : AssignedValue);
			if (AreEqual(_lastKnownValue, val))
			{
				return new ValueTuple<TValue, bool>(_lastKnownValue, false);
			}
			TValue lastKnownValue = _lastKnownValue;
			_lastKnownValue = val;
			if (!base.IsDisposed)
			{
				OnValueChanged(lastKnownValue, _lastKnownValue);
			}
			return new ValueTuple<TValue, bool>(_lastKnownValue, true);
		}
	}

	protected virtual void OnValueChanged(TValue oldValue, TValue newValue)
	{
		try
		{
			this.ValueChanged?.Invoke(oldValue, newValue);
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Error("PendingValue", "Invoking value changed handler for pending value {0}", ex.Message);
		}
	}

	public void SetPendingValue(TValue value)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		lock (_locker)
		{
			if (base.IsDisposed)
			{
				TaggedLog.Debug("PendingValue", "Unable to set bending value {0} because object has been disposed", value);
				return;
			}
			CurrentPendingValue = value;
			if (AreEqual(AssignedValue, value))
			{
				return;
			}
			_pendingValueEnabled = true;
			CalculateValue();
			if (_resetActiveValueWatchdog == null)
			{
				_resetActiveValueWatchdog = new Watchdog(MaxPendingValueTimeMs, new Action(CancelPendingValue), autoStartOnFirstPet: true);
			}
			_resetActiveValueWatchdog.TryPet(autoReset: true);
			try
			{
				this.PendingValueChanged?.Invoke(AssignedValue, value);
			}
			catch (global::System.Exception ex)
			{
				TaggedLog.Error("PendingValue", "Invoking pending value changed handler for value {0} {1} => {2}: {3}", value, AssignedValue, value, ex.Message);
			}
		}
	}

	private void CancelPendingValue()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		lock (_locker)
		{
			_pendingValueEnabled = false;
			CalculateValue();
		}
	}

	public void TryCancelPendingValue()
	{
		lock (_locker)
		{
			_resetActiveValueWatchdog?.TryDispose();
			_resetActiveValueWatchdog = null;
			CancelPendingValue();
		}
	}

	public override void Dispose(bool disposing)
	{
		lock (_locker)
		{
			TryCancelPendingValue();
			if (_valueChangedHandler != null)
			{
				try
				{
					ValueChanged -= _valueChangedHandler;
				}
				catch
				{
				}
				_valueChangedHandler = null;
			}
			this.ValueChanged = null;
			this.PendingValueChanged = null;
		}
	}
}
