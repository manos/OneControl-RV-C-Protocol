using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Color;
using IDS.Portable.Common.Extensions;
using OneControl.Devices.LightRgb;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandActionRgb : MyRvLinkCommand
{
	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int DeviceCommandIndex = 5;

	private readonly byte[] _rawData;

	private const int DeviceRedColorByteIndex = 6;

	private const int DeviceGreenColorByteIndex = 7;

	private const int DeviceBlueColorByteIndex = 8;

	private const int DeviceAutoOffModeOnByteIndex = 6;

	private const int DeviceAutoOffModeBlinkByteIndex = 7;

	private const int DeviceAutoOffModeRainbowByteIndex = 8;

	private const int DeviceInterval1ModeBlinkByteIndex = 10;

	private const int DeviceInterval2ModeBlinkByteIndex = 11;

	private const int DeviceInterval1ModeRainbowByteIndex = 7;

	private const int DeviceInterval2ModeRainbowByteIndex = 8;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandActionRgb";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.ActionRgb;

	protected override int MinPayloadLength => 6;

	private int MaxPayloadLength => 12;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public RgbLightMode Mode => (RgbLightMode)_rawData[5];

	public RgbColor? Color
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected I4, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			RgbLightMode mode = Mode;
			return (int)mode switch
			{
				0 => new RgbColor(_rawData[6], _rawData[7], _rawData[8]), 
				1 => new RgbColor(_rawData[6], _rawData[7], _rawData[8]), 
				2 => new RgbColor(_rawData[6], _rawData[7], _rawData[8]), 
				_ => null, 
			};
		}
	}

	public byte? AutoOffDuration
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Invalid comparison between Unknown and I4
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			RgbLightMode mode = Mode;
			if ((int)mode != 1)
			{
				if ((int)mode != 2)
				{
					if ((int)mode == 8)
					{
						return _rawData[8];
					}
					return null;
				}
				return _rawData[7];
			}
			return _rawData[6];
		}
	}

	public ushort? Interval
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)Mode == 8)
			{
				return (ushort)((_rawData[7] << 8) | _rawData[8]);
			}
			return null;
		}
	}

	public byte? OnInterval
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)Mode == 2)
			{
				return _rawData[10];
			}
			return null;
		}
	}

	public byte? OffInterval
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)Mode == 2)
			{
				return _rawData[11];
			}
			return null;
		}
	}

	public string CommandLogString
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected I4, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Invalid comparison between Unknown and I4
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			RgbLightMode mode = Mode;
			switch ((int)mode)
			{
			default:
				if ((int)mode == 8)
				{
					return $"{Mode} {AutoOffDuration} {Interval}";
				}
				return $"{Mode} (Unknown mode)";
			case 0:
				return $"{Mode}";
			case 1:
				return $"{Mode} {Color} {AutoOffDuration}";
			case 2:
				return $"{Mode} {Color} {AutoOffDuration} {OnInterval}/{OffInterval}";
			}
		}
	}

	public MyRvLinkCommandActionRgb(ushort clientCommandId, byte deviceTableId, byte deviceId, LogicalDeviceLightRgbCommand command)
	{
		global::System.Collections.Generic.IReadOnlyList<byte> dataMinimum = command.DataMinimum;
		int num = MinPayloadLength + ((global::System.Collections.Generic.IReadOnlyCollection<byte>)dataMinimum).Count - 1;
		_rawData = new byte[num];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		int num2 = 5;
		global::System.Collections.Generic.IEnumerator<byte> enumerator = ((global::System.Collections.Generic.IEnumerable<byte>)dataMinimum).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				byte current = enumerator.Current;
				_rawData[num2++] = current;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandActionRgb(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > MaxPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {MaxPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandActionRgb Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandActionRgb(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public virtual string ToString()
	{
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, Device Id: 0x{DeviceId:X2}, Command: {CommandLogString}]: {ArrayExtension.DebugDump(_rawData, " ", false)} ";
	}
}
