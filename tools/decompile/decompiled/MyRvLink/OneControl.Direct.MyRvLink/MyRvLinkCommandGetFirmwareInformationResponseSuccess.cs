using System;
using System.Collections.Generic;
using System.Text;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetFirmwareInformationResponseSuccess : MyRvLinkCommandResponseSuccess
{
	public const string LogTag = "MyRvLinkCommandGetFirmwareInformationResponseSuccess";

	public const int FirmwareInformationCodeIndexEx = 0;

	public const int FirmwareInformationStartIndexEx = 1;

	public const int ExtendedDataSizeMin = 2;

	protected override int MinExtendedDataLength => 2;

	public virtual FirmwareInformationCode FirmwareInformationCode => DecodeFirmwareInformationCode();

	public global::System.Collections.Generic.IReadOnlyList<byte> FirmwareInformationRaw => DecodeFirmwareInformation();

	public MyRvLinkCommandGetFirmwareInformationResponseSuccess(ushort clientCommandId, bool commandComplete, FirmwareInformationCode firmwareInformationCode, byte[] rawFirmwareInformation)
		: base(clientCommandId, commandComplete, EncodeExtendedData(firmwareInformationCode, rawFirmwareInformation))
	{
	}

	public MyRvLinkCommandGetFirmwareInformationResponseSuccess(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandGetFirmwareInformationResponseSuccess(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	protected FirmwareInformationCode DecodeFirmwareInformationCode()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count <= 0)
		{
			throw new MyRvLinkDecoderException("Expected extended data to contain FirmwareInformationCode, but there is no extended data.");
		}
		return (FirmwareInformationCode)base.ExtendedData[0];
	}

	protected global::System.Collections.Generic.IReadOnlyList<byte> DecodeFirmwareInformation()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count <= 1)
		{
			throw new MyRvLinkDecoderException("Expected extended data to contain FirmwareInformationCode, but there is no extended data.");
		}
		return GetExtendedData(1, base.ExtendedDataLength - 1);
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(FirmwareInformationCode firmwareInformationCode, byte[] rawFirmwareInformation)
	{
		byte[] array = new byte[rawFirmwareInformation.Length + 1];
		array[0] = (byte)firmwareInformationCode;
		Buffer.BlockCopy((global::System.Array)rawFirmwareInformation, 0, (global::System.Array)array, 1, rawFirmwareInformation.Length);
		return array;
	}

	public override string ToString()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder val = new StringBuilder($"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandGetFirmwareInformationResponseSuccess"}");
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		try
		{
			StringBuilder val2 = val;
			StringBuilder obj = val2;
			val3 = new AppendInterpolatedStringHandler(30, 2, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" FirmwareInformationCode: 0x");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>((byte)FirmwareInformationCode, "X2");
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("(");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<FirmwareInformationCode>(FirmwareInformationCode);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(")");
			obj.Append(ref val3);
			val2 = val;
			StringBuilder obj2 = val2;
			val3 = new AppendInterpolatedStringHandler(2, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(": ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ArrayExtension.DebugDump(FirmwareInformationRaw, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)FirmwareInformationRaw).Count, " ", false));
			obj2.Append(ref val3);
		}
		catch (global::System.Exception ex)
		{
			StringBuilder val2 = val;
			StringBuilder obj3 = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(33, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" ERROR Trying to decode response ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
			obj3.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
