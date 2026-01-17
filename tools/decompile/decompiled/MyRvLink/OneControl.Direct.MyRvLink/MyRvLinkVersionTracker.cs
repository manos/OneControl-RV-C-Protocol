using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using IDS.Portable.Common;

namespace OneControl.Direct.MyRvLink;

internal class MyRvLinkVersionTracker : CommonDisposable
{
	private const string LogTag = "MyRvLinkVersionTracker";

	private string LogPrefix;

	private GatewayVersionSupportLevel _detailedGatewayProtocolVersionSupport;

	private global::System.Threading.Tasks.Task<IMyRvLinkCommandResponse>? _getFirmwareInformationResponse;

	private MyRvLinkCommandGetFirmwareInformationResponseSuccessVersion? _responseSuccess;

	[field: CompilerGenerated]
	public IDirectConnectionMyRvLink MyRvLinkService
	{
		[CompilerGenerated]
		get;
	}

	public bool IsVersionSupported
	{
		get
		{
			if (!((CommonDisposable)this).IsDisposed)
			{
				return _detailedGatewayProtocolVersionSupport.IsSupported();
			}
			return false;
		}
	}

	public MyRvLinkVersionTracker(IDirectConnectionMyRvLink myRvLinkService)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		MyRvLinkService = myRvLinkService ?? throw new ArgumentNullException("Invalid IMyRvLinkService", "myRvLinkService");
		LogPrefix = MyRvLinkService.LogPrefix;
	}

	public void GetVersionIfNeeded()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if (MyRvLinkService.HasMinimumExpectedProtocolVersion && _getFirmwareInformationResponse == null)
		{
			MyRvLinkCommandGetFirmwareInformation command = new MyRvLinkCommandGetFirmwareInformation(MyRvLinkService.GetNextCommandId(), FirmwareInformationCode.Version);
			_getFirmwareInformationResponse = MyRvLinkService.SendCommandAsync(command, CancellationToken.None, MyRvLinkSendCommandOption.None, GetVersionResponse);
			MyRvLinkCommandGetFirmwareInformation command2 = new MyRvLinkCommandGetFirmwareInformation(MyRvLinkService.GetNextCommandId(), FirmwareInformationCode.Cpu);
			MyRvLinkService.SendCommandAsync(command2, CancellationToken.None, MyRvLinkSendCommandOption.DontWaitForResponse);
		}
	}

	private void GetVersionResponse(IMyRvLinkCommandResponse response)
	{
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		if (!(response is MyRvLinkCommandGetFirmwareInformationResponseSuccessVersion myRvLinkCommandGetFirmwareInformationResponseSuccessVersion))
		{
			if (response is MyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
			{
				TaggedLog.Warning("MyRvLinkVersionTracker", $"{LogPrefix} Firmware Version FAILED: {myRvLinkCommandResponseFailure}", global::System.Array.Empty<object>());
				if (myRvLinkCommandResponseFailure.FailureCode == MyRvLinkCommandResponseFailureCode.CommandNotSupported)
				{
					_detailedGatewayProtocolVersionSupport = GatewayVersionSupportLevel.NotSupported;
					TaggedLog.Error("MyRvLinkVersionTracker", LogPrefix + " Getting of firmware version not supported, but is now required so this firmware version is assumed not compatible", global::System.Array.Empty<object>());
				}
				else
				{
					TaggedLog.Information("MyRvLinkVersionTracker", LogPrefix + " Clear getting firmware info response so we can try again", global::System.Array.Empty<object>());
					_responseSuccess = null;
					_getFirmwareInformationResponse = null;
				}
			}
			else
			{
				TaggedLog.Error("MyRvLinkVersionTracker", $"{LogPrefix} Firmware Version Unknown/Unexpected Response {((object)response).GetType()} {response.CommandResult}", global::System.Array.Empty<object>());
				_detailedGatewayProtocolVersionSupport = GatewayVersionSupportLevel.NotSupported;
			}
		}
		else
		{
			_responseSuccess = myRvLinkCommandGetFirmwareInformationResponseSuccessVersion;
			_detailedGatewayProtocolVersionSupport = (myRvLinkCommandGetFirmwareInformationResponseSuccessVersion.IsDebugVersion ? GatewayVersionSupportLevel.SupportedForTest : GatewayVersionSupportLevelExtension.MakeVersionSupportedInfo(myRvLinkCommandGetFirmwareInformationResponseSuccessVersion.WlpVersionMajor, myRvLinkCommandGetFirmwareInformationResponseSuccessVersion.WlpVersionMinor));
			TaggedLog.Information("MyRvLinkVersionTracker", $"{LogPrefix} Firmware Version {_detailedGatewayProtocolVersionSupport}: {myRvLinkCommandGetFirmwareInformationResponseSuccessVersion}", global::System.Array.Empty<object>());
			if (!_detailedGatewayProtocolVersionSupport.IsSupported())
			{
				TaggedLog.Warning("MyRvLinkVersionTracker", $"{LogPrefix} This Protocol Firmware Version {myRvLinkCommandGetFirmwareInformationResponseSuccessVersion.WlpVersionMajorRaw}.{myRvLinkCommandGetFirmwareInformationResponseSuccessVersion.WlpVersionMinor} isn't supported.", global::System.Array.Empty<object>());
			}
		}
	}

	public bool IsGatewayVersionValid(MyRvLinkGatewayInformation? gatewayInfo)
	{
		if (gatewayInfo == null)
		{
			return false;
		}
		if (!IsVersionSupported || _responseSuccess == null)
		{
			return false;
		}
		if (_responseSuccess.WlpVersionMajorRaw != (byte)gatewayInfo.ProtocolVersionMajor)
		{
			TaggedLog.Warning("MyRvLinkVersionTracker", $"{LogPrefix} Major version reported in GatewayInfo {gatewayInfo.ProtocolVersionMajor}, doesn't match major version reported in firmware info {_responseSuccess.WlpVersionString}", global::System.Array.Empty<object>());
			return false;
		}
		return true;
	}

	public override void Dispose(bool disposing)
	{
	}
}
