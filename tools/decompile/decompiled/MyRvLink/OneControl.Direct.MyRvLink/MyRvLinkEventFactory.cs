using System;
using System.Collections.Generic;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using OneControl.Direct.MyRvLink.Events;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkEventFactory : Singleton<MyRvLinkEventFactory>
{
	public const string LogTag = "MyRvLinkEventFactory";

	public const int EventTypeIndex = 0;

	private MyRvLinkEventFactory()
	{
	}

	public IMyRvLinkEvent? TryDecodeEvent(global::System.Collections.Generic.IReadOnlyList<byte> eventBytes, bool showErrors, Func<int, IMyRvLinkCommand?> getPendingCommand)
	{
		if (eventBytes == null || ((global::System.Collections.Generic.IReadOnlyCollection<byte>)eventBytes).Count <= 0)
		{
			return null;
		}
		try
		{
			MyRvLinkEventType myRvLinkEventType = (MyRvLinkEventType)eventBytes[0];
			return myRvLinkEventType switch
			{
				MyRvLinkEventType.GatewayInformation => MyRvLinkGatewayInformation.Decode(eventBytes), 
				MyRvLinkEventType.DeviceCommand => MyRvLinkCommandEvent.DecodeCommandEvent(eventBytes, getPendingCommand), 
				MyRvLinkEventType.DeviceOnlineStatus => MyRvLinkDeviceOnlineStatus.Decode(eventBytes), 
				MyRvLinkEventType.DeviceLockStatus => MyRvLinkDeviceLockStatus.Decode(eventBytes), 
				MyRvLinkEventType.RelayBasicLatchingStatusType1 => MyRvLinkRelayBasicLatchingStatusType1.Decode(eventBytes), 
				MyRvLinkEventType.RelayBasicLatchingStatusType2 => MyRvLinkRelayBasicLatchingStatusType2.Decode(eventBytes), 
				MyRvLinkEventType.RvStatus => MyRvLinkRvStatus.Decode(eventBytes), 
				MyRvLinkEventType.DimmableLightStatus => MyRvLinkDimmableLightStatus.Decode(eventBytes), 
				MyRvLinkEventType.RgbLightStatus => MyRvLinkRgbLightStatus.Decode(eventBytes), 
				MyRvLinkEventType.GeneratorGenieStatus => MyRvLinkGeneratorGenieStatus.Decode(eventBytes), 
				MyRvLinkEventType.HvacStatus => MyRvLinkHvacStatus.Decode(eventBytes), 
				MyRvLinkEventType.TankSensorStatus => MyRvLinkTankSensorStatus.Decode(eventBytes), 
				MyRvLinkEventType.TankSensorStatusV2 => MyRvLinkTankSensorStatusV2.Decode(eventBytes), 
				MyRvLinkEventType.RelayHBridgeMomentaryStatusType1 => MyRvLinkRelayHBridgeMomentaryStatusType1.Decode(eventBytes), 
				MyRvLinkEventType.RelayHBridgeMomentaryStatusType2 => MyRvLinkRelayHBridgeMomentaryStatusType2.Decode(eventBytes), 
				MyRvLinkEventType.HourMeterStatus => MyRvLinkHourMeterStatus.Decode(eventBytes), 
				MyRvLinkEventType.Leveler4DeviceStatus => MyRvLinkLeveler4Status.Decode(eventBytes), 
				MyRvLinkEventType.Leveler5DeviceStatus => MyRvLinkLeveler5Status.Decode(eventBytes), 
				MyRvLinkEventType.AutoOperationProgressStatus => MyRvLinkLevelerType5ExtendedStatus.Decode(eventBytes), 
				MyRvLinkEventType.LevelerType5ExtendedStatus => MyRvLinkLevelerType5ExtendedStatus.Decode(eventBytes), 
				MyRvLinkEventType.LevelerConsoleText => MyRvLinkLevelerConsoleText.Decode(eventBytes), 
				MyRvLinkEventType.Leveler1DeviceStatus => MyRvLinkLeveler1Status.Decode(eventBytes), 
				MyRvLinkEventType.Leveler3DeviceStatus => MyRvLinkLeveler3Status.Decode(eventBytes), 
				MyRvLinkEventType.RealTimeClock => MyRvLinkRealTimeClock.Decode(eventBytes), 
				MyRvLinkEventType.MonitorPanelStatus => MyRvLinkMonitorPanelStatus.Decode(eventBytes), 
				MyRvLinkEventType.AccessoryGatewayStatus => MyRvLinkAccessoryGatewayStatus.Decode(eventBytes), 
				MyRvLinkEventType.HostDebug => MyRvLinkHostDebug.Decode(eventBytes), 
				MyRvLinkEventType.DeviceSessionStatus => MyRvLinkDeviceSessionStatus.Decode(eventBytes), 
				MyRvLinkEventType.TemperatureSensorStatus => MyRvLinkTemperatureSensorStatus.Decode(eventBytes), 
				MyRvLinkEventType.JaycoTbbStatus => MyRvLinkJaycoTbbStatus.Decode(eventBytes), 
				MyRvLinkEventType.AwningSensorStatus => MyRvLinkAwningSensorStatus.Decode(eventBytes), 
				MyRvLinkEventType.CloudGatewayStatus => MyRvLinkCloudGatewayStatus.Decode(eventBytes), 
				MyRvLinkEventType.BrakingSystemStatus => MyRvLinkBrakingSystemStatus.Decode(eventBytes), 
				MyRvLinkEventType.BatteryMonitorStatus => MyRvLinkBatteryMonitorStatus.Decode(eventBytes), 
				MyRvLinkEventType.ReFlashBootloader => MyRvLinkBootLoaderStatus.Decode(eventBytes), 
				MyRvLinkEventType.DoorLockStatus => MyRvLinkDoorLockStatus.Decode(eventBytes), 
				MyRvLinkEventType.DimmableLightExtendedStatus => MyRvLinkDimmableLightStatusExtended.Decode(eventBytes), 
				_ => throw new MyRvLinkDecoderException($"Unknown Event {myRvLinkEventType}(0x{(byte)myRvLinkEventType:X2})"), 
			};
		}
		catch (global::System.Exception ex)
		{
			if (showErrors)
			{
				TaggedLog.Error("MyRvLinkEventFactory", "Error processing event " + ArrayExtension.DebugDump(eventBytes, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)eventBytes).Count, " ", false) + ": " + ex.Message, global::System.Array.Empty<object>());
			}
			return null;
		}
	}
}
