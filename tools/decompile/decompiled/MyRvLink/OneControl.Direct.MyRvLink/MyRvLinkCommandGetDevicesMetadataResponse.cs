using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetDevicesMetadataResponse : MyRvLinkCommandResponseSuccess
{
	public const string LogTag = "MyRvLinkCommandGetDevicesMetadataResponse";

	private List<IMyRvLinkDeviceMetadata>? _devicesMetadata;

	protected const int ExtendedDataHeaderSize = 3;

	protected const int DeviceTableIdIndex = 0;

	protected const int StartingDeviceIdIndex = 1;

	protected const int DeviceCountIndex = 2;

	protected override int MinExtendedDataLength => 3;

	public byte DeviceTableId => base.ExtendedData[0];

	public byte StartDeviceId => base.ExtendedData[1];

	public byte DeviceCount => base.ExtendedData[2];

	public global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDeviceMetadata> DevicesMetadata => (global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDeviceMetadata>)(_devicesMetadata ?? (_devicesMetadata = DecodeDevicesMetadata()));

	public MyRvLinkCommandGetDevicesMetadataResponse(ushort clientCommandId, byte deviceTableId, byte startingDeviceId, global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDeviceMetadata> devicesMetadata)
		: base(clientCommandId, commandCompleted: false, EncodeExtendedData(deviceTableId, startingDeviceId, devicesMetadata))
	{
	}

	public MyRvLinkCommandGetDevicesMetadataResponse(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandGetDevicesMetadataResponse(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	protected List<IMyRvLinkDeviceMetadata> DecodeDevicesMetadata()
	{
		List<IMyRvLinkDeviceMetadata> devicesMetadata = new List<IMyRvLinkDeviceMetadata>();
		if (base.ExtendedData == null)
		{
			return new List<IMyRvLinkDeviceMetadata>();
		}
		int num = 3;
		while (num < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count)
		{
			int num2 = DecodeMetadata(GetExtendedData(num), ref devicesMetadata);
			num += num2;
			if (num2 == 0)
			{
				throw new MyRvLinkDecoderException("Unable to decode device metadata, no bytes were read.");
			}
		}
		if (DeviceCount != devicesMetadata.Count)
		{
			throw new MyRvLinkDecoderException($"Unable to decode devices metadata, expected {DeviceCount} but decoded {devicesMetadata.Count}");
		}
		return devicesMetadata;
	}

	public static int DecodeMetadata(global::System.Collections.Generic.IReadOnlyList<byte> buffer, ref List<IMyRvLinkDeviceMetadata> devicesMetadata)
	{
		if (devicesMetadata == null)
		{
			devicesMetadata = new List<IMyRvLinkDeviceMetadata>();
		}
		int num = MyRvLinkDevice.DecodePayloadSize(buffer);
		IMyRvLinkDeviceMetadata myRvLinkDeviceMetadata = MyRvLinkDeviceMetadata.TryDecodeFromRawBuffer(buffer);
		devicesMetadata.Add(myRvLinkDeviceMetadata);
		return num + 2;
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(byte deviceTableId, byte startingDeviceId, global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDeviceMetadata> devicesMetadata)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		if (((global::System.Collections.Generic.IReadOnlyCollection<IMyRvLinkDeviceMetadata>)devicesMetadata).Count > 255)
		{
			throw new ArgumentOutOfRangeException("devicesMetadata", $"Too many {((global::System.Collections.Generic.IReadOnlyCollection<IMyRvLinkDeviceMetadata>)devicesMetadata).Count} devices specified, only {1} devices supported.");
		}
		byte b = (byte)((global::System.Collections.Generic.IReadOnlyCollection<IMyRvLinkDeviceMetadata>)devicesMetadata).Count;
		int num = 0;
		global::System.Collections.Generic.IEnumerator<IMyRvLinkDeviceMetadata> enumerator = ((global::System.Collections.Generic.IEnumerable<IMyRvLinkDeviceMetadata>)devicesMetadata).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				IMyRvLinkDeviceMetadata current = enumerator.Current;
				num += current.EncodeSize;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		byte[] array = new byte[3 + num];
		array[0] = deviceTableId;
		array[1] = startingDeviceId;
		array[2] = b;
		int num2 = 3;
		enumerator = ((global::System.Collections.Generic.IEnumerable<IMyRvLinkDeviceMetadata>)devicesMetadata).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				int num3 = enumerator.Current.EncodeIntoBuffer(array, num2);
				num2 += num3;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(array, 0, num2);
	}

	public override string ToString()
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder val = new StringBuilder($"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandGetDevicesMetadataResponse"} DeviceTableId: {DeviceTableId} DeviceStartId: {StartDeviceId} Device Count: {DeviceCount}");
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		try
		{
			int startDeviceId = StartDeviceId;
			global::System.Collections.Generic.IEnumerator<IMyRvLinkDeviceMetadata> enumerator = ((global::System.Collections.Generic.IEnumerable<IMyRvLinkDeviceMetadata>)DevicesMetadata).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					IMyRvLinkDeviceMetadata current = enumerator.Current;
					StringBuilder val2 = val;
					StringBuilder obj = val2;
					val3 = new AppendInterpolatedStringHandler(9, 2, val2);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("\n    0x");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<int>(startDeviceId++, "X2");
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(": ");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(((object)current).ToString());
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
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(32, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("\n    ERROR Trying to Get Device ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ex.Message);
			obj2.Append(ref val3);
		}
		return ((object)val).ToString();
	}
}
