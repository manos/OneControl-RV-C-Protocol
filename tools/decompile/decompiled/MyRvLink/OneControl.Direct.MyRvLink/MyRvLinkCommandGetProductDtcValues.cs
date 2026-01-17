using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Core.IDS_CAN;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetProductDtcValues : MyRvLinkCommand
{
	private const byte DtcFilterMask = 7;

	private const int MaxPayloadLength = 10;

	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int OptionIndex = 5;

	private const int StartDtcIndex = 6;

	private const int EndDtcIndex = 8;

	private readonly byte[] _rawData;

	private readonly Dictionary<DTC_ID, DtcValue> _dtcDict = new Dictionary<DTC_ID, DtcValue>();

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandGetProductDtcValues";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.GetProductDtcValues;

	protected override int MinPayloadLength => 10;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public LogicalDeviceDtcFilter DtcFilter => (LogicalDeviceDtcFilter)(_rawData[5] & 7);

	public DTC_ID StartDtcId => (DTC_ID)ArrayExtension.GetValueUInt16(_rawData, 6, (Endian)0);

	public DTC_ID EndDtcId => (DTC_ID)ArrayExtension.GetValueUInt16(_rawData, 8, (Endian)0);

	[field: CompilerGenerated]
	public bool IsDeviceLoadingCompleted
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public Dictionary<DTC_ID, DtcValue> DtcDict
	{
		get
		{
			if (!IsDeviceLoadingCompleted)
			{
				return new Dictionary<DTC_ID, DtcValue>();
			}
			return _dtcDict;
		}
	}

	public MyRvLinkCommandGetProductDtcValues(ushort clientCommandId, byte deviceTableId, byte deviceId, LogicalDeviceDtcFilter dtcFilter, DTC_ID startDtcId, DTC_ID endDtcId)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected I4, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected I4, but got Unknown
		_rawData = new byte[10];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		_rawData[5] = (byte)((byte)dtcFilter & 7);
		ArrayExtension.SetValueUInt16(_rawData, (ushort)(int)startDtcId, 6, (Endian)0);
		ArrayExtension.SetValueUInt16(_rawData, (ushort)(int)endDtcId, 8, (Endian)0);
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandGetProductDtcValues(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public override bool ProcessResponse(IMyRvLinkCommandResponse commandResponse)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		if (base.ResponseState != MyRvLinkResponseState.Pending)
		{
			TaggedLog.Debug(LogTag, $"Ignoring Process Command Response because command is {base.ResponseState}: {commandResponse}", global::System.Array.Empty<object>());
			return true;
		}
		if (!(commandResponse is MyRvLinkCommandGetProductDtcValuesResponse myRvLinkCommandGetProductDtcValuesResponse))
		{
			if (!(commandResponse is MyRvLinkCommandGetProductDtcValuesResponseCompleted myRvLinkCommandGetProductDtcValuesResponseCompleted))
			{
				if (commandResponse is MyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
				{
					TaggedLog.Debug(LogTag, $"Command failed {myRvLinkCommandResponseFailure}", global::System.Array.Empty<object>());
					IsDeviceLoadingCompleted = false;
					base.ResponseState = MyRvLinkResponseState.Failed;
				}
				else
				{
					TaggedLog.Debug(LogTag, $"Unexpected response received {commandResponse}", global::System.Array.Empty<object>());
					IsDeviceLoadingCompleted = false;
					base.ResponseState = MyRvLinkResponseState.Failed;
				}
			}
			else
			{
				try
				{
					if (myRvLinkCommandGetProductDtcValuesResponseCompleted.DtcCount != _dtcDict.Count)
					{
						global::System.Collections.Generic.IReadOnlyList<byte> readOnlyList = myRvLinkCommandGetProductDtcValuesResponseCompleted.Encode();
						TaggedLog.Debug(LogTag, $"Should have received {myRvLinkCommandGetProductDtcValuesResponseCompleted.DtcCount} DTCs, but received {_dtcDict.Count} DTCs: Response Complete: {ArrayExtension.DebugDump(readOnlyList, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count, " ", false)}", global::System.Array.Empty<object>());
						_dtcDict.Clear();
						IsDeviceLoadingCompleted = false;
						base.ResponseState = MyRvLinkResponseState.Failed;
					}
					else
					{
						IsDeviceLoadingCompleted = true;
						base.ResponseState = MyRvLinkResponseState.Completed;
					}
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Debug(LogTag, "Command completed, ALL DTCs were not received properly: " + ex.Message, global::System.Array.Empty<object>());
					base.ResponseState = MyRvLinkResponseState.Failed;
				}
			}
		}
		else
		{
			try
			{
				global::System.Collections.Generic.IEnumerator<KeyValuePair<DTC_ID, DtcValue>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<DTC_ID, DtcValue>>)myRvLinkCommandGetProductDtcValuesResponse.DtcDict).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
					{
						KeyValuePair<DTC_ID, DtcValue> current = enumerator.Current;
						if (_dtcDict.ContainsKey(current.Key))
						{
							TaggedLog.Debug(LogTag, $"IGNORING: Duplicate DTC {current.Key}, value already returned in another response", global::System.Array.Empty<object>());
						}
						else
						{
							_dtcDict.Add(current.Key, current.Value);
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator)?.Dispose();
				}
			}
			catch (global::System.Exception ex2)
			{
				TaggedLog.Debug(LogTag, "Command completed, ALL DTCs were not received properly: " + ex2.Message, global::System.Array.Empty<object>());
				base.ResponseState = MyRvLinkResponseState.Failed;
			}
		}
		return base.ResponseState != MyRvLinkResponseState.Pending;
	}

	public static MyRvLinkCommandGetProductDtcValues Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandGetProductDtcValues(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public override IMyRvLinkCommandEvent DecodeCommandEvent(IMyRvLinkCommandEvent commandEvent)
	{
		if (commandEvent is MyRvLinkCommandResponseSuccess myRvLinkCommandResponseSuccess)
		{
			if (myRvLinkCommandResponseSuccess.IsCommandCompleted)
			{
				return new MyRvLinkCommandGetProductDtcValuesResponseCompleted(myRvLinkCommandResponseSuccess);
			}
			return new MyRvLinkCommandGetProductDtcValuesResponse(myRvLinkCommandResponseSuccess);
		}
		return commandEvent;
	}

	public virtual string ToString()
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected I4, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected I4, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder val = new StringBuilder($"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, Device Id: 0x{DeviceId:X2}, Filter: {DtcFilter}, Start: 0x{(int)StartDtcId:X}, End: 0x{(int)EndDtcId:X} ]: {ArrayExtension.DebugDump(_rawData, " ", false)}");
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		if (IsDeviceLoadingCompleted)
		{
			StringBuilder val2 = val;
			StringBuilder obj = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(23, 2, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    Received DTC Count ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<int>(DtcDict.Count);
			obj.Append(ref val3);
			try
			{
				Enumerator<DTC_ID, DtcValue> enumerator = DtcDict.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<DTC_ID, DtcValue> current = enumerator.Current;
						val2 = val;
						StringBuilder obj2 = val2;
						val3 = new AppendInterpolatedStringHandler(25, 5, val2);
						((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
						((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    ");
						((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<DTC_ID>(current.Key);
						((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(": ");
						DtcValue value = current.Value;
						((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>(((DtcValue)(ref value)).PowerCyclesCounter);
						((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(", Active: ");
						value = current.Value;
						((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<bool>(((DtcValue)(ref value)).IsActive);
						((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Stored: ");
						value = current.Value;
						((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<bool>(((DtcValue)(ref value)).IsStored);
						obj2.Append(ref val3);
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
			}
			catch (global::System.Exception ex)
			{
				val2 = val;
				StringBuilder obj3 = val2;
				((AppendInterpolatedStringHandler)(ref val3))._002Ector(28, 2, val2);
				((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
				((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    ERROR Trying to Get DTC ");
				((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
				obj3.Append(ref val3);
			}
		}
		else if (base.ResponseState != MyRvLinkResponseState.Pending)
		{
			StringBuilder val2 = val;
			StringBuilder obj4 = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(40, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    --- DTC List Not Valid/Complete --- ");
			obj4.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
