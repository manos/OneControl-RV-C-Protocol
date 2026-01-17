using System;
using System.Collections.Generic;

namespace OneControl.Direct.MyRvLink;

public static class MyRvLinkCommandFactory
{
	public static IMyRvLinkCommand Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (rawData == null)
		{
			throw new ArgumentNullException("rawData");
		}
		MyRvLinkCommandType myRvLinkCommandType = MyRvLinkCommand.DecodeCommandType(rawData);
		return myRvLinkCommandType switch
		{
			MyRvLinkCommandType.GetDevices => MyRvLinkCommandGetDevices.Decode(rawData), 
			MyRvLinkCommandType.GetDevicesMetadata => MyRvLinkCommandGetDevicesMetadata.Decode(rawData), 
			MyRvLinkCommandType.RemoveOfflineDevices => MyRvLinkCommandRemoveOfflineDevices.Decode(rawData), 
			MyRvLinkCommandType.GetProductDtcValues => MyRvLinkCommandGetProductDtcValues.Decode(rawData), 
			MyRvLinkCommandType.GetDevicePidList => MyRvLinkCommandGetDevicePidList.Decode(rawData), 
			MyRvLinkCommandType.GetDevicePid => MyRvLinkCommandGetDevicePid.Decode(rawData), 
			MyRvLinkCommandType.SetDevicePid => MyRvLinkCommandSetDevicePid.Decode(rawData), 
			MyRvLinkCommandType.GetDevicePidWithAddress => MyRvLinkCommandGetDevicePidWithAddress.Decode(rawData), 
			MyRvLinkCommandType.SetDevicePidWithAddress => MyRvLinkCommandSetDevicePidWithAddress.Decode(rawData), 
			MyRvLinkCommandType.SoftwareUpdateAuthorization => MyRvLinkCommandSoftwareUpdateAuthorization.Decode(rawData), 
			MyRvLinkCommandType.ActionGeneratorGenie => MyRvLinkCommandActionGeneratorGenie.Decode(rawData), 
			MyRvLinkCommandType.ActionDimmable => MyRvLinkCommandActionDimmable.Decode(rawData), 
			MyRvLinkCommandType.ActionRgb => MyRvLinkCommandActionRgb.Decode(rawData), 
			MyRvLinkCommandType.ActionHvac => MyRvLinkCommandActionHvac.Decode(rawData), 
			MyRvLinkCommandType.ActionSwitch => MyRvLinkCommandActionSwitch.Decode(rawData), 
			MyRvLinkCommandType.ActionMovement => MyRvLinkCommandActionMovement.Decode(rawData), 
			MyRvLinkCommandType.ActionAccessoryGateway => MyRvLinkCommandActionAccessoryGateway.Decode(rawData), 
			MyRvLinkCommandType.Leveler5Command => MyRvLinkCommandLeveler5.Decode(rawData), 
			MyRvLinkCommandType.Leveler4ButtonCommand => MyRvLinkCommandLeveler4ButtonCommand.Decode(rawData), 
			MyRvLinkCommandType.Leveler3ButtonCommand => MyRvLinkCommandLeveler3ButtonCommand.Decode(rawData), 
			MyRvLinkCommandType.Leveler1ButtonCommand => MyRvLinkCommandLeveler1ButtonCommand.Decode(rawData), 
			MyRvLinkCommandType.Diagnostics => MyRvLinkCommandDiagnostics.Decode(rawData), 
			_ => throw new MyRvLinkDecoderException($"Command Not Implemented {myRvLinkCommandType} see {"MyRvLinkCommandFactory"}.cs"), 
		};
	}
}
