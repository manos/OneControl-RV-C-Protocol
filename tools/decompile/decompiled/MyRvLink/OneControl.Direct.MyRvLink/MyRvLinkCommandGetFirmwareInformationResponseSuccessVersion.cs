using System;
using System.Collections.Generic;
using System.Text;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetFirmwareInformationResponseSuccessVersion : MyRvLinkCommandGetFirmwareInformationResponseSuccess
{
	public const int WlpVersionMajorIndexEx = 1;

	public const int WlpVersionMinorIndexEx = 2;

	public const int IdsCanVersionRawIndexEx = 3;

	public const int WlpDiagnosticRawIndexEx = 4;

	public const int VersionMajorIndexEx = 5;

	public const int VersionMinorIndexEx = 9;

	public const byte MinorVersionFlagBitmask = 128;

	public const byte MinorVersionValueBitmask = 127;

	public override FirmwareInformationCode FirmwareInformationCode => FirmwareInformationCode.Version;

	protected override int MinExtendedDataLength => 13;

	public MyRvLinkProtocolVersionMajor WlpVersionMajor => (MyRvLinkProtocolVersionMajor)WlpVersionMajorRaw;

	public byte WlpVersionMajorRaw => base.ExtendedData[1];

	public byte WlpVersionMinor => (byte)(base.ExtendedData[2] & 0x7F);

	public bool IsDebugVersion => (base.ExtendedData[2] & 0x80) != 0;

	public string WlpVersionString => $"{WlpVersionMajorRaw:D3}.{WlpVersionMinor:D3} {(IsDebugVersion ? "DEBUG" : "Release")}";

	public byte IdsCanVersionRaw => base.ExtendedData[3];

	public byte DiagnosticCodeRaw => base.ExtendedData[4];

	public Version Version => new Version((int)ArrayExtension.GetValueUInt32(base.ExtendedData, 5, (Endian)0), (int)ArrayExtension.GetValueUInt32(base.ExtendedData, 9, (Endian)0));

	public MyRvLinkCommandGetFirmwareInformationResponseSuccessVersion(MyRvLinkCommandGetFirmwareInformationResponseSuccess response)
		: base(response)
	{
		if (response.FirmwareInformationCode != FirmwareInformationCode)
		{
			throw new MyRvLinkDecoderException($"Expected {FirmwareInformationCode} but received {response.FirmwareInformationCode}");
		}
	}

	public override string ToString()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder val = new StringBuilder($"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandGetFirmwareInformationResponseSuccess"}");
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		try
		{
			StringBuilder val2 = val;
			StringBuilder obj = val2;
			val3 = new AppendInterpolatedStringHandler(31, 2, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" FirmwareInformationCode: 0x");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>((byte)FirmwareInformationCode, "X2");
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" (");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<FirmwareInformationCode>(FirmwareInformationCode);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(")");
			obj.Append(ref val3);
			val2 = val;
			StringBuilder obj2 = val2;
			val3 = new AppendInterpolatedStringHandler(13, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" WlpVersion: ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(WlpVersionString);
			obj2.Append(ref val3);
			val2 = val;
			StringBuilder obj3 = val2;
			val3 = new AppendInterpolatedStringHandler(18, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" IdsCanVersion: 0x");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>(IdsCanVersionRaw, "X2");
			obj3.Append(ref val3);
			val2 = val;
			StringBuilder obj4 = val2;
			val3 = new AppendInterpolatedStringHandler(19, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" DiagnosticCode: 0x");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>(DiagnosticCodeRaw, "X2");
			obj4.Append(ref val3);
			val2 = val;
			StringBuilder obj5 = val2;
			val3 = new AppendInterpolatedStringHandler(10, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Version: ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<Version>(Version);
			obj5.Append(ref val3);
			val2 = val;
			StringBuilder obj6 = val2;
			val3 = new AppendInterpolatedStringHandler(11, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Raw Data: ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ArrayExtension.DebugDump(base.FirmwareInformationRaw, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.FirmwareInformationRaw).Count, " ", false));
			obj6.Append(ref val3);
			if (base.IsCommandCompleted)
			{
				val.Append(" COMPLETED");
			}
		}
		catch (global::System.Exception ex)
		{
			StringBuilder val2 = val;
			StringBuilder obj7 = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(33, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" ERROR Trying to decode response ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
			obj7.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
