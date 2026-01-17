using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using IDS.Portable.Common;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetDevicesResponse : MyRvLinkCommandResponseSuccess
{
	public const string LogTag = "MyRvLinkCommandGetDevicesResponse";

	private List<IMyRvLinkDevice>? _devices;

	protected const int ExtendedDataHeaderSize = 3;

	protected const int DeviceTableIdIndex = 0;

	protected const int StartingDeviceIdIndex = 1;

	protected const int DeviceCountIndex = 2;

	protected override int MinExtendedDataLength => 3;

	public byte DeviceTableId => base.ExtendedData[0];

	public byte StartDeviceId => base.ExtendedData[1];

	public byte DeviceCount => base.ExtendedData[2];

	public global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice> Devices => (global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice>)(_devices ?? (_devices = DecodeDevices()));

	public MyRvLinkCommandGetDevicesResponse(ushort clientCommandId, byte deviceTableId, byte startingDeviceId, global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice> devices)
		: base(clientCommandId, commandCompleted: false, EncodeExtendedData(deviceTableId, startingDeviceId, devices))
	{
	}

	public MyRvLinkCommandGetDevicesResponse(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandGetDevicesResponse(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	protected List<IMyRvLinkDevice> DecodeDevices()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		List<IMyRvLinkDevice> devices = new List<IMyRvLinkDevice>();
		if (base.ExtendedData == null)
		{
			return new List<IMyRvLinkDevice>();
		}
		int num = 3;
		while (num < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count)
		{
			int item = DecodeDevice(GetExtendedData(num), ref devices).Item1;
			num += item;
			if (item == 0)
			{
				throw new MyRvLinkDecoderException("Unable to decode device, no bytes were read.");
			}
		}
		if (DeviceCount != devices.Count)
		{
			throw new MyRvLinkDecoderException($"Unable to decode devices, expected {DeviceCount} but decoded {devices.Count}");
		}
		return devices;
	}

	public static ValueTuple<int, IMyRvLinkDevice> DecodeDevice(global::System.Collections.Generic.IReadOnlyList<byte> buffer, ref List<IMyRvLinkDevice> devices)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (devices == null)
		{
			devices = new List<IMyRvLinkDevice>();
		}
		int num = MyRvLinkDevice.DecodePayloadSize(buffer);
		IMyRvLinkDevice myRvLinkDevice = MyRvLinkDevice.TryDecodeFromRawBuffer(buffer);
		if (myRvLinkDevice is MyRvLinkDeviceNone)
		{
			TaggedLog.Warning("MyRvLinkCommandGetDevicesResponse", $"Added an unknown device {myRvLinkDevice}", global::System.Array.Empty<object>());
		}
		devices.Add(myRvLinkDevice);
		return new ValueTuple<int, IMyRvLinkDevice>(num + 2, myRvLinkDevice);
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(byte deviceTableId, byte startingDeviceId, global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDevice> devices)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		if (((global::System.Collections.Generic.IReadOnlyCollection<IMyRvLinkDevice>)devices).Count > 255)
		{
			throw new ArgumentOutOfRangeException("devices", $"Too many {((global::System.Collections.Generic.IReadOnlyCollection<IMyRvLinkDevice>)devices).Count} devices specified, only {1} devices supported.");
		}
		byte b = (byte)((global::System.Collections.Generic.IReadOnlyCollection<IMyRvLinkDevice>)devices).Count;
		int num = 0;
		global::System.Collections.Generic.IEnumerator<IMyRvLinkDevice> enumerator = ((global::System.Collections.Generic.IEnumerable<IMyRvLinkDevice>)devices).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				IMyRvLinkDevice current = enumerator.Current;
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
		enumerator = ((global::System.Collections.Generic.IEnumerable<IMyRvLinkDevice>)devices).GetEnumerator();
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
		StringBuilder val = new StringBuilder($"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandGetDevicesResponse"} DeviceTableId: {DeviceTableId} DeviceStartId: {StartDeviceId} Device Count: {DeviceCount}");
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		try
		{
			int startDeviceId = StartDeviceId;
			global::System.Collections.Generic.IEnumerator<IMyRvLinkDevice> enumerator = ((global::System.Collections.Generic.IEnumerable<IMyRvLinkDevice>)Devices).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					IMyRvLinkDevice current = enumerator.Current;
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
