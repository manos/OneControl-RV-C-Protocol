using System;
using System.Collections.Generic;
using System.Text;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandActionSwitchResponseSuccess : MyRvLinkCommandResponseSuccess
{
	public const string LogTag = "MyRvLinkCommandActionSwitchResponseSuccess";

	public const int DeviceIdExtendedIndex = 0;

	public const int ExtendedDataSize = 1;

	protected override int MinExtendedDataLength => 1;

	public bool HasDeviceId
	{
		get
		{
			if (base.ExtendedData != null)
			{
				return ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count != 0;
			}
			return false;
		}
	}

	public byte DeviceId => DecodeDeviceId();

	public MyRvLinkCommandActionSwitchResponseSuccess(ushort clientCommandId, bool commandComplete, byte deviceId)
		: base(clientCommandId, commandComplete, EncodeExtendedData(deviceId))
	{
	}

	public MyRvLinkCommandActionSwitchResponseSuccess(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandActionSwitchResponseSuccess(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	protected byte DecodeDeviceId()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count == 0)
		{
			throw new MyRvLinkDecoderException("Expected extended data to contain DeviceId, but there is no extended data.");
		}
		return base.ExtendedData[0];
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(byte deviceId)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(new byte[1] { deviceId }, 0, 1);
	}

	public override string ToString()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder val = new StringBuilder($"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandActionSwitchResponseSuccess"}");
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		try
		{
			StringBuilder val2 = val;
			StringBuilder obj = val2;
			val3 = new AppendInterpolatedStringHandler(14, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Device Id: 0x");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte>(DeviceId, "X2");
			obj.Append(ref val3);
			if (base.IsCommandCompleted)
			{
				val.Append(" COMPLETED");
			}
		}
		catch (global::System.Exception ex)
		{
			StringBuilder val2 = val;
			StringBuilder obj2 = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(37, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Device Id: ERROR Trying to DeviceId ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
			obj2.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
