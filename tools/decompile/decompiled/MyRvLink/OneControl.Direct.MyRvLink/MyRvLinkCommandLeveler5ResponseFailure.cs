using System;
using System.Collections.Generic;
using System.Text;
using IDS.Core.IDS_CAN;
using IDS.Portable.Common.Extensions;
using OneControl.Devices.Leveler.Type5;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandLeveler5ResponseFailure : MyRvLinkCommandResponseFailure
{
	public const string LogTag = "MyRvLinkCommandActionSwitchResponseFailure";

	public const int DeviceIdExtendedIndex = 0;

	public const int OptionalExtendedDataSize = 2;

	protected override int MinExtendedDataLength => 0;

	public bool HasUserMessageDtc
	{
		get
		{
			global::System.Collections.Generic.IReadOnlyList<byte> extendedData = base.ExtendedData;
			if (extendedData == null)
			{
				return false;
			}
			return ((global::System.Collections.Generic.IReadOnlyCollection<byte>)extendedData).Count == 2;
		}
	}

	public LevelerFaultType5? LevelerFault => DecodeUserMessageDtc();

	public MyRvLinkCommandLeveler5ResponseFailure(ushort clientCommandId, MyRvLinkCommandResponseFailureCode failureCode)
		: base(clientCommandId, commandCompleted: true, failureCode)
	{
	}

	public MyRvLinkCommandLeveler5ResponseFailure(ushort clientCommandId, bool commandComplete, MyRvLinkCommandResponseFailureCode failureCode, DTC_ID userMessageDtc)
		: base(clientCommandId, commandComplete, failureCode, EncodeExtendedData(userMessageDtc))
	{
	}//IL_0004: Unknown result type (might be due to invalid IL or missing references)


	public MyRvLinkCommandLeveler5ResponseFailure(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandLeveler5ResponseFailure(MyRvLinkCommandResponseFailure response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.FailureCode, response.ExtendedData)
	{
	}

	protected LevelerFaultType5? DecodeUserMessageDtc()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (!HasUserMessageDtc)
		{
			return null;
		}
		return new LevelerFaultType5(ArrayExtension.GetValueUInt16(base.ExtendedData, 0, (Endian)0));
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(DTC_ID userMessageDtc)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected I4, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		byte[] array = new byte[2];
		ArrayExtension.SetValueUInt16(array, (ushort)(int)userMessageDtc, 0, (Endian)0);
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(array, 0, 2);
	}

	public override string ToString()
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder val = new StringBuilder($"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandActionSwitchResponseFailure"} Failure Code {base.FailureCode}(0x{base.FailureCode:X2})");
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		try
		{
			LevelerFaultType5? levelerFault = LevelerFault;
			if (!levelerFault.HasValue)
			{
				val.Append(" User Message: none");
			}
			else
			{
				StringBuilder val2 = val;
				StringBuilder obj = val2;
				val3 = new AppendInterpolatedStringHandler(16, 1, val2);
				((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" User Message: ");
				object obj2;
				if (!levelerFault.HasValue)
				{
					obj2 = null;
				}
				else
				{
					LevelerFaultType5 valueOrDefault = levelerFault.GetValueOrDefault();
					obj2 = ((LevelerFaultType5)(ref valueOrDefault)).ToDebugString();
				}
				((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted((string)obj2);
				((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(")");
				obj.Append(ref val3);
			}
		}
		catch (global::System.Exception ex)
		{
			StringBuilder val2 = val;
			StringBuilder obj3 = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(22, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" User Message: none (");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(")");
			obj3.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
