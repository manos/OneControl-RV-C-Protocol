using System;
using System.Collections.Generic;
using System.Text;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetFirmwareInformationResponseSuccessCpu : MyRvLinkCommandGetFirmwareInformationResponseSuccess
{
	public const int CpuModelIndexEx = 1;

	public const int CpuFeaturesIndexEx = 2;

	public const int CpuCoresIndexEx = 6;

	public const int CpuRevisionIndexEx = 7;

	public const int XtalFrequencyIndexEx = 8;

	public const int CpuFrequencyIndexEx = 9;

	public override FirmwareInformationCode FirmwareInformationCode => FirmwareInformationCode.Cpu;

	protected override int MinExtendedDataLength => 15;

	public byte CpuModelRaw => base.ExtendedData[1];

	public uint CpuFeaturesRaw => ArrayExtension.GetValueUInt32(base.ExtendedData, 2, (Endian)0);

	public byte CpuCores => base.ExtendedData[6];

	public byte CpuRevision => base.ExtendedData[7];

	public byte XtalFrequency => base.ExtendedData[8];

	public ushort CpuFrequency => ArrayExtension.GetValueUInt16(base.ExtendedData, 9, (Endian)0);

	public MyRvLinkCommandGetFirmwareInformationResponseSuccessCpu(MyRvLinkCommandGetFirmwareInformationResponseSuccess response)
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
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
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
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" CpuModel: 0x");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>(CpuModelRaw, "X2");
			obj2.Append(ref val3);
			val2 = val;
			StringBuilder obj3 = val2;
			val3 = new AppendInterpolatedStringHandler(16, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" CpuFeatures: 0x");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<uint>(CpuFeaturesRaw, "X8");
			obj3.Append(ref val3);
			val2 = val;
			StringBuilder obj4 = val2;
			val3 = new AppendInterpolatedStringHandler(11, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" CpuCores: ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>(CpuCores);
			obj4.Append(ref val3);
			val2 = val;
			StringBuilder obj5 = val2;
			val3 = new AppendInterpolatedStringHandler(16, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" CpuRevision: 0x");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>(CpuRevision);
			obj5.Append(ref val3);
			val2 = val;
			StringBuilder obj6 = val2;
			val3 = new AppendInterpolatedStringHandler(20, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" XtalFrequency: ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>(XtalFrequency);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Mhz");
			obj6.Append(ref val3);
			val2 = val;
			StringBuilder obj7 = val2;
			val3 = new AppendInterpolatedStringHandler(19, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" CpuFrequency: ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<ushort>(CpuFrequency);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Mhz");
			obj7.Append(ref val3);
			val2 = val;
			StringBuilder obj8 = val2;
			val3 = new AppendInterpolatedStringHandler(11, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Raw Data: ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ArrayExtension.DebugDump(base.FirmwareInformationRaw, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.FirmwareInformationRaw).Count, " ", false));
			obj8.Append(ref val3);
			if (base.IsCommandCompleted)
			{
				val.Append(" COMPLETED");
			}
		}
		catch (global::System.Exception ex)
		{
			StringBuilder val2 = val;
			StringBuilder obj9 = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(33, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" ERROR Trying to decode response ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
			obj9.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
