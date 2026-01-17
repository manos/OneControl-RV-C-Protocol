using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using IDS.Core.IDS_CAN;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetProductDtcValuesResponse : MyRvLinkCommandResponseSuccess
{
	public const string LogTag = "MyRvLinkCommandGetProductDtcValuesResponse";

	private const int DtcResultSize = 3;

	private Dictionary<DTC_ID, DtcValue>? _dtcDict;

	protected override int MinExtendedDataLength => 3;

	public IReadOnlyDictionary<DTC_ID, DtcValue> DtcDict => (IReadOnlyDictionary<DTC_ID, DtcValue>)(object)(_dtcDict ?? (_dtcDict = DecodeDtcDict()));

	public MyRvLinkCommandGetProductDtcValuesResponse(ushort clientCommandId, global::System.Collections.Generic.IReadOnlyList<ValueTuple<DTC_ID, DtcValue>> dtcList)
		: base(clientCommandId, commandCompleted: false, EncodeExtendedData(dtcList))
	{
	}

	public MyRvLinkCommandGetProductDtcValuesResponse(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandGetProductDtcValuesResponse(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	protected Dictionary<DTC_ID, DtcValue> DecodeDtcDict()
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<DTC_ID, DtcValue> val = new Dictionary<DTC_ID, DtcValue>();
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count == 0)
		{
			return new Dictionary<DTC_ID, DtcValue>();
		}
		try
		{
			int num = 0;
			DTC_ID val2 = default(DTC_ID);
			while (num < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count)
			{
				short valueInt = ArrayExtension.GetValueInt16(base.ExtendedData, num, (Endian)0);
				num += 2;
				byte b = base.ExtendedData[num];
				num++;
				if (!Enum<DTC_ID>.TryConvert((object)valueInt, ref val2))
				{
					TaggedLog.Warning("MyRvLinkCommandGetProductDtcValuesResponse", $"Ignoring unknown PID of 0x{valueInt:X}", global::System.Array.Empty<object>());
				}
				else if (val.ContainsKey(val2))
				{
					TaggedLog.Debug("MyRvLinkCommandGetProductDtcValuesResponse", $"IGNORING: Duplicate DTC {val2} returned in response", global::System.Array.Empty<object>());
				}
				else
				{
					val.Add(val2, new DtcValue(b));
				}
			}
			return val;
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Error("MyRvLinkCommandGetProductDtcValuesResponse", "Error getting DTC response: " + ex.Message + " extended: " + ArrayExtension.DebugDump(base.ExtendedData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count, " ", false), global::System.Array.Empty<object>());
			return new Dictionary<DTC_ID, DtcValue>();
		}
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(global::System.Collections.Generic.IReadOnlyList<ValueTuple<DTC_ID, DtcValue>> dtcList)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected I4, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		if (((global::System.Collections.Generic.IReadOnlyCollection<ValueTuple<DTC_ID, DtcValue>>)dtcList).Count == 0)
		{
			throw new ArgumentOutOfRangeException("dtcList", "Should have at least 1 DTC in the list.");
		}
		if (((global::System.Collections.Generic.IReadOnlyCollection<ValueTuple<DTC_ID, DtcValue>>)dtcList).Count > 65535)
		{
			throw new ArgumentOutOfRangeException("dtcList", $"Too many {((global::System.Collections.Generic.IReadOnlyCollection<ValueTuple<DTC_ID, DtcValue>>)dtcList).Count} devices specified, only {65535} devices supported.");
		}
		byte[] array = new byte[3 * ((global::System.Collections.Generic.IReadOnlyCollection<ValueTuple<DTC_ID, DtcValue>>)dtcList).Count];
		int num = 0;
		global::System.Collections.Generic.IEnumerator<ValueTuple<DTC_ID, DtcValue>> enumerator = ((global::System.Collections.Generic.IEnumerable<ValueTuple<DTC_ID, DtcValue>>)dtcList).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<DTC_ID, DtcValue> current = enumerator.Current;
				ArrayExtension.SetValueUInt16(array, (ushort)(int)current.Item1, num, (Endian)0);
				num += 2;
				array[num] = ((DtcValue)(ref current.Item2)).ToRawValue();
				num++;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(array, 0, num);
	}

	public override string ToString()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder val = new StringBuilder($"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandGetProductDtcValuesResponse"} DTC Count: {((global::System.Collections.Generic.IReadOnlyCollection<KeyValuePair<DTC_ID, DtcValue>>)DtcDict).Count}");
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		try
		{
			global::System.Collections.Generic.IEnumerator<KeyValuePair<DTC_ID, DtcValue>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<DTC_ID, DtcValue>>)DtcDict).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					KeyValuePair<DTC_ID, DtcValue> current = enumerator.Current;
					StringBuilder val2 = val;
					StringBuilder obj = val2;
					val3 = new AppendInterpolatedStringHandler(7, 2, val2);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("\n    ");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<DTC_ID>(current.Key);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(": ");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<DtcValue>(current.Value);
					obj.Append(ref val3);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
		}
		catch (global::System.Exception ex)
		{
			StringBuilder val2 = val;
			StringBuilder obj2 = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(29, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("\n    ERROR Trying to Get DTC ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
			obj2.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
