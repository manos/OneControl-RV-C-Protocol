using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;

namespace IDS.Portable.Common;

public static class List
{
	[CompilerGenerated]
	private sealed class _003CRemoveDuplicates_003Ed__2<TValue> : global::System.Collections.Generic.IEnumerable<TValue>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<TValue>, global::System.Collections.IEnumerator, global::System.IDisposable
	{
		private int _003C_003E1__state;

		private TValue _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<TValue> list;

		public List<TValue> _003C_003E3__list;

		private HashSet<TValue> _003Creturned_003E5__2;

		private Enumerator<TValue> _003C_003E7__wrap2;

		TValue global::System.Collections.Generic.IEnumerator<TValue>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object global::System.Collections.IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CRemoveDuplicates_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void global::System.IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
		}

		private bool MoveNext()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (list == null)
					{
						return false;
					}
					_003Creturned_003E5__2 = new HashSet<TValue>();
					_003C_003E7__wrap2 = list.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					TValue current = _003C_003E7__wrap2.Current;
					if (!_003Creturned_003E5__2.Contains(current))
					{
						_003Creturned_003E5__2.Add(current);
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = default(Enumerator<TValue>);
				return false;
			}
			catch
			{
				//try-fault
				((global::System.IDisposable)this).Dispose();
				throw;
			}
		}

		bool global::System.Collections.IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((global::System.IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void global::System.Collections.IEnumerator.Reset()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		global::System.Collections.Generic.IEnumerator<TValue> global::System.Collections.Generic.IEnumerable<TValue>.GetEnumerator()
		{
			_003CRemoveDuplicates_003Ed__2<TValue> _003CRemoveDuplicates_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRemoveDuplicates_003Ed__ = this;
			}
			else
			{
				_003CRemoveDuplicates_003Ed__ = new _003CRemoveDuplicates_003Ed__2<TValue>(0);
			}
			_003CRemoveDuplicates_003Ed__.list = _003C_003E3__list;
			return _003CRemoveDuplicates_003Ed__;
		}

		[DebuggerHidden]
		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<TValue>)this).GetEnumerator();
		}
	}

	private static readonly byte[] ConversionBuffer = new byte[16];

	public static List<TValue> Remove<TValue>(this List<TValue> list, Func<TValue, bool> removeItemFilter)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		List<TValue> val = new List<TValue>();
		if (removeItemFilter == null)
		{
			return val;
		}
		Enumerator<TValue> enumerator = list.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				TValue current = enumerator.Current;
				if (removeItemFilter.Invoke(current))
				{
					val.Add(current);
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
		list.Remove((global::System.Collections.Generic.IEnumerable<TValue>)val);
		return val;
	}

	public static void Remove<TValue>(this List<TValue> list, global::System.Collections.Generic.IEnumerable<TValue> itemsToRemove)
	{
		if (itemsToRemove == null)
		{
			return;
		}
		global::System.Collections.Generic.IEnumerator<TValue> enumerator = itemsToRemove.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				TValue current = enumerator.Current;
				try
				{
					list.Remove(current);
				}
				catch
				{
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}

	[IteratorStateMachine(typeof(_003CRemoveDuplicates_003Ed__2<>))]
	public static global::System.Collections.Generic.IEnumerable<TValue> RemoveDuplicates<TValue>(this List<TValue> list)
	{
		if (list == null)
		{
			yield break;
		}
		HashSet<TValue> returned = new HashSet<TValue>();
		Enumerator<TValue> enumerator = list.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				TValue current = enumerator.Current;
				if (!returned.Contains(current))
				{
					returned.Add(current);
					yield return current;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
	}

	public static int BinarySearch<T>(this global::System.Collections.Generic.IList<T> collection, T? value)
	{
		return collection.BinarySearch(value, (IComparer<T>?)(object)Comparer<T>.Default);
	}

	public static int BinarySearch<T>(this global::System.Collections.Generic.IList<T> collection, T? value, IComparer<T>? comparer)
	{
		if (comparer == null)
		{
			comparer = (IComparer<T>?)(object)Comparer<T>.Default;
		}
		int num = 0;
		int num2 = ((global::System.Collections.Generic.ICollection<T>)collection).Count - 1;
		while (num <= num2)
		{
			int num3 = num + (num2 - num >> 1);
			int num4 = comparer.Compare(collection[num3], value);
			if (num4 >= 0)
			{
				if (num4 == 0)
				{
					return num3;
				}
				num2 = num3 - 1;
			}
			else
			{
				num = num3 + 1;
			}
		}
		return ~num;
	}

	public static List<byte> AppendValueByte(this List<byte> list, byte value)
	{
		list.Add(value);
		return list;
	}

	public static List<byte> AppendValueUInt16(this List<byte> list, ushort value)
	{
		lock (ConversionBuffer)
		{
			ConversionBuffer.SetValueUInt16(value, 0);
			list.Add(ConversionBuffer[0]);
			list.Add(ConversionBuffer[1]);
			return list;
		}
	}

	public static List<byte> AppendValueUInt24(this List<byte> list, uint value)
	{
		lock (ConversionBuffer)
		{
			ConversionBuffer.SetValueUInt24(value, 0);
			list.Add(ConversionBuffer[0]);
			list.Add(ConversionBuffer[1]);
			list.Add(ConversionBuffer[3]);
			return list;
		}
	}

	public static List<byte> AppendValueUInt32(this List<byte> list, uint value)
	{
		lock (ConversionBuffer)
		{
			ConversionBuffer.SetValueUInt32(value, 0);
			list.Add(ConversionBuffer[0]);
			list.Add(ConversionBuffer[1]);
			list.Add(ConversionBuffer[2]);
			list.Add(ConversionBuffer[3]);
			return list;
		}
	}

	public static List<byte> AppendValueUInt48(this List<byte> list, ulong value)
	{
		lock (ConversionBuffer)
		{
			ConversionBuffer.SetValueUInt48(value, 0);
			list.Add(ConversionBuffer[0]);
			list.Add(ConversionBuffer[1]);
			list.Add(ConversionBuffer[2]);
			list.Add(ConversionBuffer[3]);
			list.Add(ConversionBuffer[4]);
			list.Add(ConversionBuffer[5]);
			return list;
		}
	}

	public static List<byte> AppendValueFixedPointFloat(this List<byte> list, float value, FixedPointType fixedPoint)
	{
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		lock (ConversionBuffer)
		{
			ConversionBuffer.SetFixedPointFloat(value, 0u, fixedPoint);
			switch (fixedPoint)
			{
			case FixedPointType.UnsignedBigEndian8x8:
			case FixedPointType.SignedBigEndian8x8:
				list.Add(ConversionBuffer[0]);
				list.Add(ConversionBuffer[1]);
				break;
			case FixedPointType.UnsignedBigEndian16x16:
			case FixedPointType.SignedBigEndian16x16:
				list.Add(ConversionBuffer[0]);
				list.Add(ConversionBuffer[1]);
				list.Add(ConversionBuffer[3]);
				list.Add(ConversionBuffer[4]);
				break;
			default:
				throw new NotImplementedException($"Unknown {"FixedPointType"} of {fixedPoint}");
			}
		}
		return list;
	}
}
