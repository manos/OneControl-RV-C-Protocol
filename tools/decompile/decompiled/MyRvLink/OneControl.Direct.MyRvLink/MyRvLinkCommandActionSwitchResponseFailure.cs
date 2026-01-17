using System;
using System.Collections.Generic;
using System.Text;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandActionSwitchResponseFailure : MyRvLinkCommandResponseFailure
{
	public const string LogTag = "MyRvLinkCommandActionSwitchResponseFailure";

	public const int DeviceIdExtendedIndex = 0;

	public const int OptionalExtendedDataSize = 1;

	protected override int MinExtendedDataLength => 0;

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

	public byte? DeviceId => DecodeDeviceId();

	public MyRvLinkCommandActionSwitchResponseFailure(ushort clientCommandId, MyRvLinkCommandResponseFailureCode failureCode)
		: base(clientCommandId, commandCompleted: true, failureCode)
	{
	}

	public MyRvLinkCommandActionSwitchResponseFailure(ushort clientCommandId, bool commandComplete, MyRvLinkCommandResponseFailureCode failureCode, byte deviceId)
		: base(clientCommandId, commandComplete, failureCode, EncodeExtendedData(deviceId))
	{
	}

	public MyRvLinkCommandActionSwitchResponseFailure(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandActionSwitchResponseFailure(MyRvLinkCommandResponseFailure response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.FailureCode, response.ExtendedData)
	{
	}

	protected byte? DecodeDeviceId()
	{
		if (base.ExtendedData == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count == 0)
		{
			return null;
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
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder val = new StringBuilder($"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandActionSwitchResponseFailure"} Failure Code {base.FailureCode}(0x{base.FailureCode:X2})");
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		try
		{
			byte? deviceId = DeviceId;
			if (deviceId.HasValue)
			{
				StringBuilder val2 = val;
				StringBuilder obj = val2;
				val3 = new AppendInterpolatedStringHandler(14, 1, val2);
				((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Device Id: 0x");
				((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<byte?>(deviceId, "X2");
				obj.Append(ref val3);
			}
		}
		catch (global::System.Exception ex)
		{
			StringBuilder val2 = val;
			StringBuilder obj2 = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(35, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" Device Id: ERROR Getting DeviceId ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
			obj2.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
