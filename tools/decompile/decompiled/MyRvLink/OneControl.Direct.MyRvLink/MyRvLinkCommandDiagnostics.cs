using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandDiagnostics : MyRvLinkCommand
{
	private const int MaxPayloadLength = 9;

	private const int DiagnosticCommandTypeIndex = 3;

	private const int DiagnosticCommandStateIndex = 4;

	private const int DiagnosticEventTypeIndex = 5;

	private const int DiagnosticEventStateIndex = 6;

	private const int DiagnosticHostValue = 7;

	private const int DiagnosticDeviceLinkId = 8;

	private readonly byte[] _rawData;

	private MyRvLinkCommandDiagnosticsResponseCompleted? _response;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkCommandDiagnostics";

	[field: CompilerGenerated]
	public override MyRvLinkCommandType CommandType
	{
		[CompilerGenerated]
		get;
	} = MyRvLinkCommandType.Diagnostics;

	protected override int MinPayloadLength => 9;

	public override ushort ClientCommandId => MyRvLinkCommand.DecodeClientCommandId(_rawData);

	public global::System.Collections.Generic.IReadOnlyList<MyRvLinkCommandType>? EnabledDiagnosticCommands => _response?.EnabledDiagnosticCommands;

	public global::System.Collections.Generic.IReadOnlyList<MyRvLinkEventType>? EnabledDiagnosticEvents => _response?.EnabledDiagnosticEvents;

	public MyRvLinkCommandType DiagnosticCommandType => (MyRvLinkCommandType)_rawData[3];

	public DiagnosticState DiagnosticCommandState => (DiagnosticState)_rawData[4];

	public MyRvLinkEventType DiagnosticEventType => (MyRvLinkEventType)_rawData[5];

	public DiagnosticState DiagnosticEventState => (DiagnosticState)_rawData[6];

	public MyRvLinkCommandDiagnostics(ushort clientCommandId, MyRvLinkCommandType diagCommandType, DiagnosticState diagCommandState, MyRvLinkEventType diagEventType, DiagnosticState diagEventState)
	{
		_rawData = new byte[9];
		ArrayExtension.SetValueUInt16(_rawData, clientCommandId, 0, (Endian)0);
		_rawData[2] = (byte)CommandType;
		_rawData[3] = (byte)diagCommandType;
		_rawData[4] = (byte)diagCommandState;
		_rawData[5] = (byte)diagEventType;
		_rawData[6] = (byte)diagEventState;
		ValidateCommand(_rawData, clientCommandId);
	}

	protected MyRvLinkCommandDiagnostics(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		ValidateCommand(rawData);
		_rawData = ReadOnlyList.ToNewArray<byte>(rawData, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count);
	}

	public override bool ProcessResponse(IMyRvLinkCommandResponse commandResponse)
	{
		if (base.ResponseState != MyRvLinkResponseState.Pending)
		{
			TaggedLog.Debug(LogTag, $"Ignoring Process Command Response because command is {base.ResponseState}: {commandResponse}", global::System.Array.Empty<object>());
			return true;
		}
		if (!(commandResponse is MyRvLinkCommandDiagnosticsResponseCompleted response))
		{
			if (commandResponse is MyRvLinkCommandResponseSuccess myRvLinkCommandResponseSuccess)
			{
				TaggedLog.Debug(LogTag, $"Unexpected success response received {commandResponse} (should have been of type {typeof(MyRvLinkCommandDiagnosticsResponseCompleted)})", global::System.Array.Empty<object>());
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
				if (_response != null)
				{
					throw new MyRvLinkException($"Already processed response for {this} with value {_response}");
				}
				_response = response;
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

	public static MyRvLinkCommandDiagnostics Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkCommandDiagnostics(rawData);
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
				return new MyRvLinkCommandDiagnosticsResponseCompleted(myRvLinkCommandResponseSuccess);
			}
			TaggedLog.Debug(LogTag, $"DecodeCommandEvent for {LogTag} Received UNEXPECTED event of {myRvLinkCommandResponseSuccess}", global::System.Array.Empty<object>());
			return commandEvent;
		}
		return commandEvent;
	}

	public virtual string ToString()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		StringBuilder val = new StringBuilder($"{LogTag}[Client Command Id: 0x{ClientCommandId:X4}");
		val.Append((DiagnosticCommandType == MyRvLinkCommandType.Unknown) ? " No Command Diagnostics" : $" {DiagnosticCommandType} {DiagnosticCommandState}");
		val.Append(",");
		val.Append((DiagnosticEventType == MyRvLinkEventType.Unknown) ? " No Event Diagnostics" : $" {DiagnosticEventType} {DiagnosticEventState}");
		val.Append(",");
		val.Append((_response == null) ? " No Response]" : ("] " + ((object)_response).ToString()));
		return ((object)val).ToString();
	}
}
