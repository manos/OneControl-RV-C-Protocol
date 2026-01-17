using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetDevicePidListResponse : MyRvLinkCommandResponseSuccess
{
	public const string LogTag = "MyRvLinkCommandGetDevicePidListResponse";

	private const int PidResultSize = 3;

	private Dictionary<Pid, PidAccess>? _pidDict;

	protected override int MinExtendedDataLength => 3;

	public IReadOnlyDictionary<Pid, PidAccess> PidDict => (IReadOnlyDictionary<Pid, PidAccess>)(object)(_pidDict ?? (_pidDict = DecodePidDict()));

	public MyRvLinkCommandGetDevicePidListResponse(ushort clientCommandId, global::System.Collections.Generic.IReadOnlyList<ValueTuple<Pid, PidAccess>> pidList)
		: base(clientCommandId, commandCompleted: false, EncodeExtendedData(pidList))
	{
	}

	public MyRvLinkCommandGetDevicePidListResponse(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandGetDevicePidListResponse(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	protected Dictionary<Pid, PidAccess> DecodePidDict()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<Pid, PidAccess> val = new Dictionary<Pid, PidAccess>();
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count == 0)
		{
			return new Dictionary<Pid, PidAccess>();
		}
		try
		{
			int num = 0;
			Pid val3 = default(Pid);
			while (num < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count)
			{
				short valueInt = ArrayExtension.GetValueInt16(base.ExtendedData, num, (Endian)0);
				num += 2;
				PidAccess val2 = (PidAccess)(base.ExtendedData[num] & -249);
				num++;
				if (!Enum<Pid>.TryConvert((object)valueInt, ref val3))
				{
					TaggedLog.Warning("MyRvLinkCommandGetDevicePidListResponse", $"Ignoring unknown PID of 0x{valueInt:X}", global::System.Array.Empty<object>());
				}
				else if (val.ContainsKey(val3))
				{
					TaggedLog.Debug("MyRvLinkCommandGetDevicePidListResponse", $"IGNORING: Duplicate PID {val3} returned in response", global::System.Array.Empty<object>());
				}
				else
				{
					val.Add(val3, val2);
				}
			}
			return val;
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Error("MyRvLinkCommandGetDevicePidListResponse", "Error getting PID response: " + ex.Message + " extended: " + ArrayExtension.DebugDump(base.ExtendedData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count, " ", false), global::System.Array.Empty<object>());
			return new Dictionary<Pid, PidAccess>();
		}
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(global::System.Collections.Generic.IReadOnlyList<ValueTuple<Pid, PidAccess>> pidList)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected I4, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		if (((global::System.Collections.Generic.IReadOnlyCollection<ValueTuple<Pid, PidAccess>>)pidList).Count == 0)
		{
			throw new ArgumentOutOfRangeException("pidList", "Should have at least 1 PID in the list.");
		}
		if (((global::System.Collections.Generic.IReadOnlyCollection<ValueTuple<Pid, PidAccess>>)pidList).Count > 65535)
		{
			throw new ArgumentOutOfRangeException("pidList", $"Too many {((global::System.Collections.Generic.IReadOnlyCollection<ValueTuple<Pid, PidAccess>>)pidList).Count} devices specified, only {65535} devices supported.");
		}
		byte[] array = new byte[3 * ((global::System.Collections.Generic.IReadOnlyCollection<ValueTuple<Pid, PidAccess>>)pidList).Count];
		int num = 0;
		global::System.Collections.Generic.IEnumerator<ValueTuple<Pid, PidAccess>> enumerator = ((global::System.Collections.Generic.IEnumerable<ValueTuple<Pid, PidAccess>>)pidList).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ValueTuple<Pid, PidAccess> current = enumerator.Current;
				ArrayExtension.SetValueUInt16(array, (ushort)(int)current.Item1, num, (Endian)0);
				num += 2;
				array[num] = (byte)current.Item2;
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
		StringBuilder val = new StringBuilder($"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandGetDevicePidListResponse"} PID Count: {((global::System.Collections.Generic.IReadOnlyCollection<KeyValuePair<Pid, PidAccess>>)PidDict).Count}");
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		try
		{
			global::System.Collections.Generic.IEnumerator<KeyValuePair<Pid, PidAccess>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<Pid, PidAccess>>)PidDict).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					KeyValuePair<Pid, PidAccess> current = enumerator.Current;
					StringBuilder val2 = val;
					StringBuilder obj = val2;
					val3 = new AppendInterpolatedStringHandler(7, 2, val2);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("\n    ");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<Pid>(current.Key);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(": ");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<PidAccess>(current.Value);
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
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("\n    ERROR Trying to Get PID ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
			obj2.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
