using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IDS.Core.Types;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandGetDevicePidWithAddress : MyRvLinkCommand
{
	private const int MaxPayloadLength = 9;

	private const int DeviceTableIdIndex = 3;

	private const int DeviceIdIndex = 4;

	private const int PidIdIndex = 5;

	private const int PidAddressIndex = 7;

	private readonly byte[] _rawData;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandGetDevicePidWithAddress";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.GetDevicePidWithAddress;

	protected override int MinPayloadLength => 9;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public byte DeviceTableId => _rawData[3];

	public byte DeviceId => _rawData[4];

	public Pid Pid => (Pid)ArrayExtension.GetValueUInt16(_rawData, 5, (Endian)0);

	public ushort Address => ArrayExtension.GetValueUInt16(_rawData, 7, (Endian)0);

	[field: CompilerGenerated]
	public UInt48? PidValue
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public MyRvLinkCommandGetDevicePidWithAddress(ushort clientCommandId, byte deviceTableId, byte deviceId, Pid pidId, ushort address)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected I4, but got Unknown
		_rawData = new byte[9];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = deviceTableId;
		_rawData[4] = deviceId;
		ArrayExtension.SetValueUInt16(_rawData, (ushort)(int)pidId, 5, (Endian)0);
		ArrayExtension.SetValueUInt16(_rawData, address, 7, (Endian)0);
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandGetDevicePidWithAddress(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public static MyRvLinkCommandGetDevicePidWithAddress Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandGetDevicePidWithAddress(rawData);
	}

	public override global::System.Collections.Generic.IReadOnlyList<byte> Encode()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_rawData, 0, _rawData.Length);
	}

	public override IMyRvLinkCommandEvent DecodeCommandEvent(IMyRvLinkCommandEvent commandEvent)
	{
		if (commandEvent is MyRvLinkCommandResponseSuccess myRvLinkCommandResponseSuccess)
		{
			if (myRvLinkCommandResponseSuccess.IsCommandCompleted)
			{
				return new MyRvLinkCommandGetDevicePidWithAddressResponseCompleted(myRvLinkCommandResponseSuccess);
			}
			TaggedLog.Debug(LogTag, $"DecodeCommandEvent for {LogTag} Received UNEXPECTED event of {myRvLinkCommandResponseSuccess}", global::System.Array.Empty<object>());
			return commandEvent;
		}
		return commandEvent;
	}

	public override bool ProcessResponse(IMyRvLinkCommandResponse commandResponse)
	{
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		if (base.ResponseState != MyRvLinkResponseState.Pending)
		{
			TaggedLog.Debug(LogTag, $"Ignoring Process Command Response because command is {base.ResponseState}: {commandResponse}", global::System.Array.Empty<object>());
			return true;
		}
		if (!(commandResponse is MyRvLinkCommandGetDevicePidWithAddressResponseCompleted myRvLinkCommandGetDevicePidWithAddressResponseCompleted))
		{
			if (commandResponse is MyRvLinkCommandResponseSuccess myRvLinkCommandResponseSuccess)
			{
				TaggedLog.Debug(LogTag, $"Unexpected success response received {commandResponse} (should have been of type {typeof(MyRvLinkCommandGetDevicePidWithAddressResponseCompleted)})", global::System.Array.Empty<object>());
				return myRvLinkCommandResponseSuccess.IsCommandCompleted;
			}
			if (commandResponse is MyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
			{
				TaggedLog.Debug(LogTag, $"Command failed {myRvLinkCommandResponseFailure}", global::System.Array.Empty<object>());
				base.ResponseState = MyRvLinkResponseState.Failed;
			}
			else
			{
				TaggedLog.Debug(LogTag, $"Unexpected response received {commandResponse}", global::System.Array.Empty<object>());
				base.ResponseState = MyRvLinkResponseState.Failed;
			}
		}
		else
		{
			try
			{
				if (PidValue.HasValue)
				{
					throw new MyRvLinkException($"Already processed response for {this} with value {PidValue}");
				}
				PidValue = UInt48.op_Implicit(myRvLinkCommandGetDevicePidWithAddressResponseCompleted.PidValue);
				base.ResponseState = MyRvLinkResponseState.Completed;
			}
			catch (global::System.Exception ex)
			{
				TaggedLog.Warning(LogTag, "Warning processing response: " + ex.Message, global::System.Array.Empty<object>());
				base.ResponseState = MyRvLinkResponseState.Failed;
			}
		}
		return base.ResponseState != MyRvLinkResponseState.Pending;
	}

	public virtual string ToString()
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		return $"{LogTag}[Client Command Id: 0x{ClientCommandId:X4} Table Id: 0x{DeviceTableId:X2}, Device Id: 0x{DeviceId:X2} Pid: {Pid} Value: {((!PidValue.HasValue) ? "Not Loaded" : $"0x{PidValue:X}")}]";
	}
}
