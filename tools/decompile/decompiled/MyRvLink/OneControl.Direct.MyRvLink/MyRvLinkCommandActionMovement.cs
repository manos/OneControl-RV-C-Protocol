using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using OneControl.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandActionMovement : MyRvLinkCommand
{
	public enum RvLinkMovementCommand : byte
	{
		Stop,
		Invalid,
		Forward,
		Reverse,
		HomeReset,
		AutoForward,
		AutoReverse
	}

	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int DeviceStateIndex = 5;

	public const byte MovementBitMask = 15;

	public const byte Relay1StateBit = 16;

	public const byte Relay2StateBit = 32;

	public const byte RelayEnergizedValidBit = 128;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandActionMovement";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.ActionMovement;

	protected override int MinPayloadLength => 6;

	private int MaxPayloadLength => MinPayloadLength;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public int DeviceCount => 1;

	public RelayHBridgeDirection Direction => (RelayHBridgeDirection)(_rawData[5] & 0xF);

	public bool TurningOnRelay1
	{
		get
		{
			if ((_rawData[5] & 0x80) != 0)
			{
				return (_rawData[5] & 0x10) != 0;
			}
			return false;
		}
	}

	public bool TurningOnRelay2
	{
		get
		{
			if ((_rawData[5] & 0x80) != 0)
			{
				return (_rawData[5] & 0x20) != 0;
			}
			return false;
		}
	}

	public MyRvLinkCommandActionMovement(ushort clientCommandId, byte deviceTableId, byte deviceId, ILogicalDeviceId logicalId, HBridgeCommand command)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected I4, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Invalid comparison between Unknown and I4
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Invalid comparison between Unknown and I4
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Invalid comparison between Unknown and I4
		_rawData = new byte[MaxPayloadLength];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		bool flag = false;
		RelayHBridgeEnergized val;
		RvLinkMovementCommand rvLinkMovementCommand;
		switch ((int)command)
		{
		default:
			if ((int)command == 255)
			{
			}
			goto case 0;
		case 1:
			val = RelayHBridgeDirectionExtension.ConvertToRelayEnergized((RelayHBridgeDirection)2, logicalId);
			rvLinkMovementCommand = RvLinkMovementCommand.Forward;
			flag = true;
			break;
		case 2:
			val = RelayHBridgeDirectionExtension.ConvertToRelayEnergized((RelayHBridgeDirection)3, logicalId);
			rvLinkMovementCommand = RvLinkMovementCommand.Reverse;
			flag = true;
			break;
		case 5:
			val = (RelayHBridgeEnergized)0;
			rvLinkMovementCommand = RvLinkMovementCommand.AutoForward;
			break;
		case 6:
			val = (RelayHBridgeEnergized)0;
			rvLinkMovementCommand = RvLinkMovementCommand.AutoReverse;
			break;
		case 4:
			val = (RelayHBridgeEnergized)0;
			rvLinkMovementCommand = RvLinkMovementCommand.HomeReset;
			break;
		case 0:
		case 3:
			val = (RelayHBridgeEnergized)0;
			rvLinkMovementCommand = RvLinkMovementCommand.Stop;
			flag = true;
			break;
		}
		byte b = (byte)rvLinkMovementCommand;
		if (flag)
		{
			if ((int)val == 1)
			{
				b |= 0x10;
			}
			if ((int)val == -1)
			{
				b |= 0x20;
			}
			b |= 0x80;
		}
		_rawData[5] = b;
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandActionMovement(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count > MaxPayloadLength)
		{
			throw new MyRvLinkDecoderException($"Unable to decode data for {CommandType} received more then {MaxPayloadLength} bytes: {ArrayExtension.DebugDump(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count, " ", false)}");
		}
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandActionMovement Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandActionMovement(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public virtual string ToString()
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}, Table Id: 0x{DeviceTableId:X2}, DeviceId: 0x{DeviceId:X2}, {Direction}, LegacyRelay1: {TurningOnRelay1}, LegacyRelay2: {TurningOnRelay2}]: {ArrayExtension.DebugDump(_rawData, " ", false)} ";
	}
}
