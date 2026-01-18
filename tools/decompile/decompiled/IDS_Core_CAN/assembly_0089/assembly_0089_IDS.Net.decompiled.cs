using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using IDS.Core.IDS_CAN;
using IDS.Core.Types;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using Microsoft.CodeAnalysis;
using OneControl.Devices;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: AssemblyCompany("IDS.Portable.Devices.JaycoTbbGateway")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0")]
[assembly: AssemblyProduct("IDS.Portable.Devices.JaycoTbbGateway")]
[assembly: AssemblyTitle("IDS.Portable.Devices.JaycoTbbGateway")]
[assembly: AssemblyVersion("1.0.0.0")]
[module: RefSafetyRules(11)]
namespace Microsoft.CodeAnalysis
{
	[CompilerGenerated]
	[Embedded]
	internal sealed class EmbeddedAttribute : Attribute
	{
	}
}
namespace System.Runtime.CompilerServices
{
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	internal sealed class NullableAttribute : Attribute
	{
		public readonly byte[] NullableFlags;

		public NullableAttribute(byte P_0)
		{
			NullableFlags = (byte[])(object)new Byte[1] { P_0 };
		}

		public NullableAttribute(byte[] P_0)
		{
			NullableFlags = P_0;
		}
	}
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	internal sealed class NullableContextAttribute : Attribute
	{
		public readonly byte Flag;

		public NullableContextAttribute(byte P_0)
		{
			Flag = P_0;
		}
	}
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	internal sealed class RefSafetyRulesAttribute : Attribute
	{
		public readonly int Version;

		public RefSafetyRulesAttribute(int P_0)
		{
			Version = P_0;
		}
	}
}
namespace IDS.Portable.Devices.JaycoTbbGateway
{
	public enum JaycoTbbPidRegister : Enum
	{
		PvInputVoltage = 256,
		PvInputCurrent = 257,
		AcInputVoltage = 258,
		AcInputCurrent = 259,
		AuxDcVoltage = 260,
		AuxDcCurrent = 261,
		DcVoltageOutput = 262,
		DcCurrentOutput = 263,
		BatteryChargerVoltage = 264,
		BatteryChargingDischargingCurrent = 265,
		BatteryStateOfCharge = 266,
		BatteryTimeToFullEmpty = 267,
		StatusCmpSmp = 272,
		StatusPump1 = 276,
		Status = 296,
		DcLoadOutputCutoffOrRecover = 516
	}
	[Flags]
	public enum JaycoTbbPidStatusCmpSmp : Enum
	{
		Unknown = 0u,
		BatteryStatusMask = 7u,
		BatteryConnectionConnected = 8u,
		SilentMode = 0x10u,
		DcLoadRelayClosed = 0x20u,
		VcrRelayClosed = 0x40u,
		BatteryStatusConnected = 0x80u,
		BatteryConnectionReversePolarity = 0x100u,
		BatteryLowVoltage = 0x200u,
		BatteryHighVoltage = 0x400u,
		PvConnected = 0x800u,
		PvLowVoltage = 0x1000u,
		PvHighVoltage = 0x2000u,
		AcInputConnected = 0x4000u,
		AcInputLowVoltage = 0x8000u,
		AcInputHighVoltage = 0x10000u,
		Reserved17 = 0x20000u,
		Reserved18 = 0x40000u,
		Reserved19 = 0x80000u,
		HeatSinkOverTemp = 0x100000u,
		AuxDcConnected = 0x200000u,
		AuxDcLowVoltage = 0x400000u,
		AuxDcHighVoltage = 0x800000u,
		OverloadError = 0x1000000u,
		ShortCircuitError = 0x2000000u,
		CalibrationComplete = 0x4000000u,
		Reserve27 = 0x8000000u,
		EngineIgnitionOn = 0x10000000u,
		VcrDisconnectAbnormal = 0x20000000u,
		VcrConnectedAbnormal = 0x40000000u,
		EepromAbnormal = 0x80000000u
	}
	public enum JaycoTbbPidStatusBattery : Enum
	{
		Unknown = 0,
		Absorption = 1,
		Float = 2,
		Discharge = 3,
		EqCharging = 4,
		DcPowerStatus = 5,
		ChargingStop = 7
	}
	[Flags]
	public enum JaycoTbbPidStatusRegister : Enum
	{
		Unknown = 0u,
		WaterTank1FullAlarm = 1u,
		WaterTank1EmptyAlarm = 2u,
		WaterTank2FullAlarm = 4u,
		WaterTank2EmptyAlarm = 8u,
		WaterTank3FullAlarm = 0x10u,
		WaterTank3EmptyAlarm = 0x20u,
		WaterTankSensor1Connected = 0x40u,
		WaterTankSensor2Connected = 0x80u,
		WaterTankSensor3Connected = 0x100u,
		WaterTank4FullAlarm = 0x200u,
		WaterTank4EmptyAlarm = 0x400u,
		WaterTankSensor4Connected = 0x800u,
		Reserved12 = 0x1000u,
		Reserved13 = 0x2000u,
		Reserved14 = 0x4000u,
		Reserved15 = 0x8000u,
		IndoorTempSensorConnected = 0x10000u,
		OutdoorTempSensorConnected = 0x20000u,
		SunshadeLampSwitchEnable = 0x40000u,
		NightModeEnable = 0x80000u,
		NightLightEnable = 0x100000u,
		AmbientTemperatureHigh = 0x200000u,
		CcOvertime = 0x400000u,
		Reserved23 = 0x800000u,
		Reserved24 = 0x1000000u,
		Reserved25 = 0x2000000u,
		Reserved26 = 0x4000000u,
		Reserved27 = 0x8000000u,
		Reserved28 = 0x10000000u,
		Reserved29 = 0x20000000u,
		Reserved30 = 0x40000000u,
		Reserved31 = 0x80000000u
	}
	[DefaultValue(JaycoTbbPidRegisterStatusPump1.Off)]
	public enum JaycoTbbPidRegisterStatusPump1 : Enum
	{
		Off,
		Normal,
		NoLoad,
		OverCurrent,
		OverCurrentProtection,
		ShortCircuitProtection,
		PumpSelectionStatus
	}
	public interface ILogicalDeviceJaycoTbb : ILogicalDeviceWithStatus<LogicalDeviceJaycoTbbStatus>, ILogicalDeviceWithStatus, ILogicalDevice, IComparable, IEquatable<ILogicalDevice>, IComparable<ILogicalDevice>, ICommonDisposable, IDisposable, IDevicesCommon, INotifyPropertyChanged, ILogicalDevicePowerMonitor, IPowerMonitor, IPowerMonitorSolar, IPowerMonitorAc, IPowerMonitorAuxDc, IPowerMonitorDcOutput, IPowerMonitorBatteryCharge
	{
	}
	public class LogicalDeviceJaycoTbb : LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>, ILogicalDeviceJaycoTbb, ILogicalDeviceWithStatus<LogicalDeviceJaycoTbbStatus>, ILogicalDeviceWithStatus, ILogicalDevice, IComparable, IEquatable<ILogicalDevice>, IComparable<ILogicalDevice>, ICommonDisposable, IDisposable, IDevicesCommon, INotifyPropertyChanged, ILogicalDevicePowerMonitor, IPowerMonitor, IPowerMonitorSolar, IPowerMonitorAc, IPowerMonitorAuxDc, IPowerMonitorDcOutput, IPowerMonitorBatteryCharge
	{
		[CompilerGenerated]
		private sealed class <>c__DisplayClass176_0 : Object
		{
			public string[] notifyPropertyNameEnumeration;

			public LogicalDeviceJaycoTbb <>4__this;

			internal void <NotifyPropertyChanged>b__0()
			{
				string[] array = notifyPropertyNameEnumeration;
				foreach (string text in array)
				{
					((CommonNotifyPropertyChanged)<>4__this).NotifyPropertyChanged(text, false);
				}
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <SetDcLoadOutputCutoffAsync>d__172 : ValueType, IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public LogicalDeviceJaycoTbb <>4__this;

			public bool cutoff;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_00a9: Expected O, but got Unknown
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LogicalDeviceJaycoTbb logicalDeviceJaycoTbb = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = ((ILogicalDevicePid<bool>)(object)new LogicalDevicePidAddress<bool, JaycoTbbPidRegister>((ILogicalDevice)(object)logicalDeviceJaycoTbb, PidExtension.ConvertToPid((Pid)333), JaycoTbbPidRegister.DcLoadOutputCutoffOrRecover, (LogicalDeviceSessionType)3, (Func<bool, uint>)PidValueFromBool, (Func<ulong, bool>)null)).WriteAsync(cutoff, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <SetDcLoadOutputCutoffAsync>d__172>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
					}
					((TaskAwaiter)(ref val)).GetResult();
				}
				catch (Exception ex)
				{
					Exception exception = ex;
					<>1__state = -2;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		private const string LogTag = "LogicalDeviceJaycoTbb";

		protected const Pid _tbbRegisterReadPid = (Pid)332;

		protected const Pid _tbbRegisterWritePid = (Pid)333;

		private readonly LogicalDevicePidAutoReaderFloat _solarVoltagePidReader;

		private readonly LogicalDevicePidAutoReaderFloat _solarCurrentPidReader;

		private readonly LogicalDevicePidAutoReaderFloat _acVoltagePidReader;

		private readonly LogicalDevicePidAutoReaderFloat _acCurrentPidReader;

		private readonly LogicalDevicePidAutoReaderFloat _auxDcVoltagePidReader;

		private readonly LogicalDevicePidAutoReaderFloat _auxDcCurrentPidReader;

		private readonly LogicalDevicePidAutoReaderFloat _dcVoltageOutputPidReader;

		private readonly LogicalDevicePidAutoReaderFloat _dcCurrentOutputPidReader;

		private readonly LogicalDevicePidAutoReaderFloat _batteryChargerVoltagePidReader;

		private readonly LogicalDevicePidAutoReaderFloat _batteryChargingDischargingCurrentPidReader;

		private readonly LogicalDevicePidAutoReaderByte _batteryStateOfChargePidReader;

		private readonly LogicalDevicePidAutoReaderTimeSpan _batteryMinutesUntilFullOrEmptyReader;

		private readonly LogicalDevicePidAutoReaderFlags<JaycoTbbPidStatusCmpSmp> _statusCmpSmpReader;

		private readonly LogicalDevicePidAutoReaderFlags<JaycoTbbPidStatusRegister> _statusRegisterReader;

		private readonly LogicalDevicePidAutoReaderFlags<JaycoTbbPidRegisterStatusPump1> _pump1Reader;

		public override bool IsLegacyDeviceHazardous => false;

		public override string DeviceName => String.Concat("Power Monitor ", ((LogicalDevice<ILogicalDeviceCapability>)(object)this).DeviceName);

		protected virtual ILogicalDevicePid<float> MakeSolarVoltagePid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddress<float, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.PvInputVoltage, (Func<uint, float>)PidValueToPointZeroOneFloat, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<float> MakeSolarCurrentPid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddress<float, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.PvInputCurrent, (Func<uint, float>)PidValueToPointZeroOneFloat, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<float> MakeAcVoltagePid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddress<float, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.AcInputVoltage, (Func<uint, float>)PidValueToPointZeroOneFloat, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<float> MakeAcCurrentPid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddress<float, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.AcInputCurrent, (Func<uint, float>)PidValueToPointZeroOneFloat, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<float> MakeAuxDcVoltagePid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddress<float, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.AuxDcVoltage, (Func<uint, float>)PidValueToPointZeroOneFloat, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<float> MakeAuxDcCurrentPid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddress<float, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.AuxDcCurrent, (Func<uint, float>)PidValueToPointZeroOneFloat, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<float> MakeDcVoltageOutputPid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddress<float, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.DcVoltageOutput, (Func<uint, float>)PidValueToPointZeroOneFloat, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<float> MakeDcCurrentOutputPid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddress<float, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.DcCurrentOutput, (Func<uint, float>)PidValueToPointZeroOneFloat, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<float> MakeBatteryChargerVoltagePid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddress<float, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.BatteryChargerVoltage, (Func<uint, float>)PidValueToPointZeroOneFloat, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<float> MakeBatteryChargingDischargingCurrentPid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddress<float, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.BatteryChargingDischargingCurrent, (Func<uint, float>)PidValueToPointZeroOneFloat, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<byte> MakeBatteryStateOfChargePid => (ILogicalDevicePid<byte>)(object)new LogicalDevicePidAddress<byte, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.BatteryStateOfCharge, (Func<uint, byte>)PidValueTo1PercentIncrements, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<TimeSpan> MakeBatteryTimeUntilFullOrEmptyPid => (ILogicalDevicePid<TimeSpan>)(object)new LogicalDevicePidAddress<TimeSpan, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.BatteryTimeToFullEmpty, (Func<uint, TimeSpan>)PidValueToMinutes, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<JaycoTbbPidStatusCmpSmp> MakeStatusCmpSmpPid => (ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>)(object)new LogicalDevicePidAddress<JaycoTbbPidStatusCmpSmp, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.StatusCmpSmp, (Func<uint, JaycoTbbPidStatusCmpSmp>)PidValueToStatusCmpSmp, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<JaycoTbbPidStatusRegister> MakeStatusRegisterPid => (ILogicalDevicePid<JaycoTbbPidStatusRegister>)(object)new LogicalDevicePidAddress<JaycoTbbPidStatusRegister, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.Status, (Func<uint, JaycoTbbPidStatusRegister>)PidValueToStatusRegister, (Func<ulong, bool>)null);

		protected virtual ILogicalDevicePid<JaycoTbbPidRegisterStatusPump1> MakeStatusPump1Pid => (ILogicalDevicePid<JaycoTbbPidRegisterStatusPump1>)(object)new LogicalDevicePidAddress<JaycoTbbPidRegisterStatusPump1, JaycoTbbPidRegister>((ILogicalDevice)(object)this, PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.StatusPump1, (Func<uint, JaycoTbbPidRegisterStatusPump1>)PidValueToStatusPump, (Func<ulong, bool>)null);

		public float SolarVoltage => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_solarVoltagePidReader).Value;

		public TimeSpan SolarVoltageLastUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_solarVoltagePidReader).MetricValueLastUpdated;

		public uint SolarVoltageUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_solarVoltagePidReader).MetricValueUpdateCount;

		public float SolarCurrent => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_solarCurrentPidReader).Value;

		public TimeSpan SolarCurrentLastUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_solarCurrentPidReader).MetricValueLastUpdated;

		public uint SolarCurrentUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_solarCurrentPidReader).MetricValueUpdateCount;

		public float AcVoltage => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_acVoltagePidReader).Value;

		public TimeSpan AcVoltageLastUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_acVoltagePidReader).MetricValueLastUpdated;

		public uint AcVoltageUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_acVoltagePidReader).MetricValueUpdateCount;

		public float AcCurrent => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_acCurrentPidReader).Value;

		public TimeSpan AcCurrentLastUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_acCurrentPidReader).MetricValueLastUpdated;

		public uint AcCurrentUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_acCurrentPidReader).MetricValueUpdateCount;

		public float AuxDcVoltage => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcVoltagePidReader).Value;

		public TimeSpan AuxDcVoltageLastUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcVoltagePidReader).MetricValueLastUpdated;

		public uint AuxDcVoltageUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcVoltagePidReader).MetricValueUpdateCount;

		public float AuxDcCurrent => Math.Abs(((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcCurrentPidReader).Value);

		public TimeSpan AuxDcCurrentLastUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcCurrentPidReader).MetricValueLastUpdated;

		public uint AuxDcCurrentUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcCurrentPidReader).MetricValueUpdateCount;

		public PowerFlow AuxDcPowerFlow
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				if (!((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcVoltagePidReader).HasValueBeenLoaded)
				{
					return (PowerFlow)0;
				}
				float value = ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcVoltagePidReader).Value;
				if (value == 0f)
				{
					return (PowerFlow)1;
				}
				if (value > 0f)
				{
					return (PowerFlow)2;
				}
				return (PowerFlow)3;
			}
		}

		public float DcVoltageOutput => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_dcVoltageOutputPidReader).Value;

		public TimeSpan DcVoltageOutputLastUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_dcVoltageOutputPidReader).MetricValueLastUpdated;

		public uint DcVoltageOutputUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_dcVoltageOutputPidReader).MetricValueUpdateCount;

		public float DcCurrentOutput => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_dcCurrentOutputPidReader).Value;

		public TimeSpan DcCurrentOutputLastUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_dcCurrentOutputPidReader).MetricValueLastUpdated;

		public uint DcCurrentOutputUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_dcCurrentOutputPidReader).MetricValueUpdateCount;

		public float BatteryChargerVoltage => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargerVoltagePidReader).Value;

		public TimeSpan BatteryChargerVoltageLastUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargerVoltagePidReader).MetricValueLastUpdated;

		public uint BatteryChargerVoltageUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargerVoltagePidReader).MetricValueUpdateCount;

		public float BatteryCurrent => Math.Abs(((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargingDischargingCurrentPidReader).Value);

		public TimeSpan BatteryCurrentLastUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargingDischargingCurrentPidReader).MetricValueLastUpdated;

		public uint BatteryCurrentCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargingDischargingCurrentPidReader).MetricValueUpdateCount;

		public PowerFlow BatteryCurrentFlow
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				if (!((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargingDischargingCurrentPidReader).HasValueBeenLoaded)
				{
					return (PowerFlow)0;
				}
				float value = ((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargingDischargingCurrentPidReader).Value;
				if (value == 0f)
				{
					return (PowerFlow)1;
				}
				if (value > 0f)
				{
					return (PowerFlow)2;
				}
				return (PowerFlow)3;
			}
		}

		public byte BatteryStateOfCharge => ((LogicalDevicePidAutoReader<ILogicalDevicePid<byte>, byte>)(object)_batteryStateOfChargePidReader).Value;

		public TimeSpan BatteryStateOfChargeLastUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<byte>, byte>)(object)_batteryStateOfChargePidReader).MetricValueLastUpdated;

		public uint BatteryStateOfChargeUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<byte>, byte>)(object)_batteryStateOfChargePidReader).MetricValueUpdateCount;

		public TimeSpan BatteryMinutesRemaining
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				TimeSpan value = ((LogicalDevicePidAutoReader<ILogicalDevicePid<TimeSpan>, TimeSpan>)(object)_batteryMinutesUntilFullOrEmptyReader).Value;
				return TimeSpan.FromMinutes(Math.Abs(((TimeSpan)(ref value)).TotalMinutes));
			}
		}

		public TimeSpan BatteryMinutesRemainingLastUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<TimeSpan>, TimeSpan>)(object)_batteryMinutesUntilFullOrEmptyReader).MetricValueLastUpdated;

		public uint BatteryMinutesRemainingUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<TimeSpan>, TimeSpan>)(object)_batteryMinutesUntilFullOrEmptyReader).MetricValueUpdateCount;

		public JaycoTbbPidStatusCmpSmp StatusCmpSmp => ((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>, JaycoTbbPidStatusCmpSmp>)(object)_statusCmpSmpReader).Value;

		public TimeSpan StatusCmpSmpUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>, JaycoTbbPidStatusCmpSmp>)(object)_statusCmpSmpReader).MetricValueLastUpdated;

		public uint StatusCmpSmpUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>, JaycoTbbPidStatusCmpSmp>)(object)_statusCmpSmpReader).MetricValueUpdateCount;

		public JaycoTbbPidStatusBattery BatteryStatus => (JaycoTbbPidStatusBattery)(StatusCmpSmp & JaycoTbbPidStatusCmpSmp.BatteryStatusMask);

		public TimeSpan BatteryStatusUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>, JaycoTbbPidStatusCmpSmp>)(object)_statusCmpSmpReader).MetricValueLastUpdated;

		public uint BatteryStatusUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>, JaycoTbbPidStatusCmpSmp>)(object)_statusCmpSmpReader).MetricValueUpdateCount;

		public JaycoTbbPidStatusRegister StatusRegister => ((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusRegister>, JaycoTbbPidStatusRegister>)(object)_statusRegisterReader).Value;

		public TimeSpan StatusRegisterUpdated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusRegister>, JaycoTbbPidStatusRegister>)(object)_statusRegisterReader).MetricValueLastUpdated;

		public uint StatusRegisterUpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusRegister>, JaycoTbbPidStatusRegister>)(object)_statusRegisterReader).MetricValueUpdateCount;

		public JaycoTbbPidRegisterStatusPump1 Pump1 => ((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidRegisterStatusPump1>, JaycoTbbPidRegisterStatusPump1>)(object)_pump1Reader).Value;

		public TimeSpan Pump1Updated => ((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidRegisterStatusPump1>, JaycoTbbPidRegisterStatusPump1>)(object)_pump1Reader).MetricValueLastUpdated;

		public uint Pump1UpdateCount => ((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidRegisterStatusPump1>, JaycoTbbPidRegisterStatusPump1>)(object)_pump1Reader).MetricValueUpdateCount;

		public virtual bool IsDcLoadOutputCutoff
		{
			get
			{
				if (!((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>, JaycoTbbPidStatusCmpSmp>)(object)_statusCmpSmpReader).HasValueBeenLoaded)
				{
					return false;
				}
				return !((Enum)(object)((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>, JaycoTbbPidStatusCmpSmp>)(object)_statusCmpSmpReader).Value).HasFlag((Enum)(object)JaycoTbbPidStatusCmpSmp.DcLoadRelayClosed);
			}
		}

		internal static float PidValueToPointZeroOneFloat(uint rawPidValue)
		{
			return (float)rawPidValue * 0.01f;
		}

		internal static uint PidValueFloatToPointZeroOneUInt32(float pidValue)
		{
			return (uint)(pidValue * 100f);
		}

		internal static byte PidValueTo1PercentIncrements(uint rawPidValue)
		{
			return (byte)MathCommon.Clamp(rawPidValue, 0u, 100u);
		}

		internal static uint PidValueByteTo1PercentIncrements(byte rawPidValue)
		{
			return (uint)MathCommon.Clamp((int)rawPidValue, 0, 100);
		}

		internal static TimeSpan PidValueToMinutes(uint rawPidValue)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			return TimeSpan.FromMinutes((double)rawPidValue);
		}

		internal static uint PidValueFromMinutes(TimeSpan rawPidValue)
		{
			return (uint)((TimeSpan)(ref rawPidValue)).TotalMinutes;
		}

		internal static JaycoTbbPidStatusCmpSmp PidValueToStatusCmpSmp(uint rawPidValue)
		{
			return (JaycoTbbPidStatusCmpSmp)rawPidValue;
		}

		internal static uint PidValueFromStatusCmpSmp(JaycoTbbPidStatusCmpSmp rawPidValue)
		{
			return (uint)rawPidValue;
		}

		internal static JaycoTbbPidStatusRegister PidValueToStatusRegister(uint rawPidValue)
		{
			return (JaycoTbbPidStatusRegister)rawPidValue;
		}

		internal static uint PidValueFromStatusRegister(JaycoTbbPidStatusRegister rawPidValue)
		{
			return (uint)rawPidValue;
		}

		internal static JaycoTbbPidRegisterStatusPump1 PidValueToStatusPump(uint rawPidValue)
		{
			return (JaycoTbbPidRegisterStatusPump1)rawPidValue;
		}

		internal static uint PidValueFromStatusPump(JaycoTbbPidRegisterStatusPump1 rawPidValue)
		{
			return (uint)rawPidValue;
		}

		internal static uint PidValueFromBool(bool value)
		{
			if (!value)
			{
				return 0u;
			}
			return 1u;
		}

		internal static uint ThrowOnRead(uint rawPidValue)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			throw new Exception("PID is not readable");
		}

		internal static TEnumValue PidValueToEnum<TEnumValue>(uint rawPidValue) where TEnumValue : struct, Enum, ValueType
		{
			return Enum<TEnumValue>.TryConvert((object)rawPidValue);
		}

		public LogicalDeviceJaycoTbb(ILogicalDeviceId logicalDeviceId, ILogicalDeviceService deviceService, bool isFunctionClassChangeable = false)
			: base(logicalDeviceId, new LogicalDeviceJaycoTbbStatus(), (ILogicalDeviceCapability)new LogicalDeviceCapability(), deviceService, isFunctionClassChangeable)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Expected O, but got Unknown
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Expected O, but got Unknown
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Expected O, but got Unknown
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Expected O, but got Unknown
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Expected O, but got Unknown
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Expected O, but got Unknown
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Invalid comparison between Unknown and I4
			_solarVoltagePidReader = new LogicalDevicePidAutoReaderFloat(MakeSolarVoltagePid, 10000, 0f, (ValueWasSetAction<ILogicalDevicePid<float>, float>)([CompilerGenerated] (float value, bool changed) =>
			{
				NotifyPropertyChanged("SolarVoltage", "SolarVoltageLastUpdated", "SolarVoltageUpdateCount");
			}));
			_solarCurrentPidReader = new LogicalDevicePidAutoReaderFloat(MakeSolarCurrentPid, 10000, 0f, (ValueWasSetAction<ILogicalDevicePid<float>, float>)([CompilerGenerated] (float value, bool changed) =>
			{
				NotifyPropertyChanged("SolarCurrent", "SolarCurrentLastUpdated", "SolarCurrentUpdateCount");
			}));
			_acVoltagePidReader = new LogicalDevicePidAutoReaderFloat(MakeAcVoltagePid, 10000, 0f, (ValueWasSetAction<ILogicalDevicePid<float>, float>)([CompilerGenerated] (float value, bool changed) =>
			{
				NotifyPropertyChanged("AcVoltage", "AcVoltageLastUpdated", "AcVoltageUpdateCount");
			}));
			_acCurrentPidReader = new LogicalDevicePidAutoReaderFloat(MakeAcCurrentPid, 10000, 0f, (ValueWasSetAction<ILogicalDevicePid<float>, float>)([CompilerGenerated] (float value, bool changed) =>
			{
				NotifyPropertyChanged("AcCurrent", "AcCurrentLastUpdated", "AcCurrentUpdateCount");
			}));
			_auxDcVoltagePidReader = new LogicalDevicePidAutoReaderFloat(MakeAuxDcVoltagePid, 10000, 0f, (ValueWasSetAction<ILogicalDevicePid<float>, float>)([CompilerGenerated] (float value, bool changed) =>
			{
				NotifyPropertyChanged("AuxDcVoltage", "AuxDcVoltageLastUpdated", "AuxDcVoltageUpdateCount");
			}));
			_auxDcCurrentPidReader = new LogicalDevicePidAutoReaderFloat(MakeAuxDcCurrentPid, 10000, 0f, (ValueWasSetAction<ILogicalDevicePid<float>, float>)([CompilerGenerated] (float value, bool changed) =>
			{
				NotifyPropertyChanged("AuxDcCurrent", "AuxDcCurrentLastUpdated", "AuxDcCurrentUpdateCount", "AuxDcPowerFlow");
			}));
			_dcVoltageOutputPidReader = new LogicalDevicePidAutoReaderFloat(MakeDcVoltageOutputPid, 10000, 0f, (ValueWasSetAction<ILogicalDevicePid<float>, float>)([CompilerGenerated] (float value, bool changed) =>
			{
				NotifyPropertyChanged("DcVoltageOutput", "DcVoltageOutputLastUpdated", "DcVoltageOutputUpdateCount");
			}));
			_dcCurrentOutputPidReader = new LogicalDevicePidAutoReaderFloat(MakeDcCurrentOutputPid, 10000, 0f, (ValueWasSetAction<ILogicalDevicePid<float>, float>)([CompilerGenerated] (float value, bool changed) =>
			{
				NotifyPropertyChanged("DcCurrentOutput", "DcCurrentOutputLastUpdated", "DcCurrentOutputUpdateCount");
			}));
			_batteryChargerVoltagePidReader = new LogicalDevicePidAutoReaderFloat(MakeBatteryChargerVoltagePid, 10000, 0f, (ValueWasSetAction<ILogicalDevicePid<float>, float>)([CompilerGenerated] (float value, bool changed) =>
			{
				NotifyPropertyChanged("BatteryChargerVoltage", "BatteryChargerVoltageLastUpdated", "BatteryChargerVoltageUpdateCount");
			}));
			_batteryChargingDischargingCurrentPidReader = new LogicalDevicePidAutoReaderFloat(MakeBatteryChargingDischargingCurrentPid, 10000, 0f, (ValueWasSetAction<ILogicalDevicePid<float>, float>)([CompilerGenerated] (float value, bool changed) =>
			{
				NotifyPropertyChanged("BatteryCurrent", "BatteryCurrentLastUpdated", "BatteryCurrentCount", "BatteryCurrentFlow");
			}));
			_batteryStateOfChargePidReader = new LogicalDevicePidAutoReaderByte(MakeBatteryStateOfChargePid, 10000, (byte)0, (ValueWasSetAction<ILogicalDevicePid<byte>, byte>)([CompilerGenerated] (byte value, bool changed) =>
			{
				NotifyPropertyChanged("BatteryStateOfCharge", "BatteryStateOfChargeLastUpdated", "BatteryStateOfChargeUpdateCount");
			}));
			_batteryMinutesUntilFullOrEmptyReader = new LogicalDevicePidAutoReaderTimeSpan(MakeBatteryTimeUntilFullOrEmptyPid, 10000, default(TimeSpan), (ValueWasSetAction<ILogicalDevicePid<TimeSpan>, TimeSpan>)([CompilerGenerated] (TimeSpan value, bool changed) =>
			{
				NotifyPropertyChanged("BatteryMinutesRemaining", "BatteryMinutesRemainingLastUpdated", "BatteryMinutesRemainingUpdateCount", "BatteryCurrentFlow");
			}));
			_statusCmpSmpReader = new LogicalDevicePidAutoReaderFlags<JaycoTbbPidStatusCmpSmp>(MakeStatusCmpSmpPid, 10000, JaycoTbbPidStatusCmpSmp.Unknown, (ValueWasSetAction<ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>, JaycoTbbPidStatusCmpSmp>)([CompilerGenerated] (JaycoTbbPidStatusCmpSmp value, bool changed) =>
			{
				NotifyPropertyChanged("StatusCmpSmp", "StatusCmpSmpUpdated", "StatusCmpSmpUpdateCount", "BatteryStatus", "BatteryStatusUpdated", "BatteryStatusUpdateCount", "IsDcLoadOutputCutoff");
			}));
			_statusRegisterReader = new LogicalDevicePidAutoReaderFlags<JaycoTbbPidStatusRegister>(MakeStatusRegisterPid, 10000, JaycoTbbPidStatusRegister.Unknown, (ValueWasSetAction<ILogicalDevicePid<JaycoTbbPidStatusRegister>, JaycoTbbPidStatusRegister>)([CompilerGenerated] (JaycoTbbPidStatusRegister value, bool changed) =>
			{
				NotifyPropertyChanged("StatusRegister", "StatusRegisterUpdated", "StatusRegisterUpdateCount");
			}));
			_pump1Reader = new LogicalDevicePidAutoReaderFlags<JaycoTbbPidRegisterStatusPump1>(MakeStatusPump1Pid, 10000, JaycoTbbPidRegisterStatusPump1.Off, (ValueWasSetAction<ILogicalDevicePid<JaycoTbbPidRegisterStatusPump1>, JaycoTbbPidRegisterStatusPump1>)([CompilerGenerated] (JaycoTbbPidRegisterStatusPump1 value, bool changed) =>
			{
				NotifyPropertyChanged("Pump1", "Pump1Updated", "Pump1UpdateCount");
			}));
			if ((int)((LogicalDevice<ILogicalDeviceCapability>)(object)this).ActiveConnection == 1)
			{
				StartAllPidAutoRefresh();
			}
		}

		public override void OnDeviceOnlineChanged()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			((LogicalDevice<ILogicalDeviceCapability>)(object)this).OnDeviceOnlineChanged();
			if ((int)((LogicalDevice<ILogicalDeviceCapability>)(object)this).ActiveConnection == 1)
			{
				StartAllPidAutoRefresh();
			}
			else
			{
				StopAllPidAutoRefresh();
			}
		}

		private void StartAllPidAutoRefresh()
		{
			if (!((CommonDisposableNotifyPropertyChanged)this).IsDisposed)
			{
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_solarVoltagePidReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_solarCurrentPidReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_acVoltagePidReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_acCurrentPidReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcVoltagePidReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcCurrentPidReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_dcVoltageOutputPidReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_dcCurrentOutputPidReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargerVoltagePidReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargingDischargingCurrentPidReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<byte>, byte>)(object)_batteryStateOfChargePidReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<TimeSpan>, TimeSpan>)(object)_batteryMinutesUntilFullOrEmptyReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>, JaycoTbbPidStatusCmpSmp>)(object)_statusCmpSmpReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusRegister>, JaycoTbbPidStatusRegister>)(object)_statusRegisterReader).Start(true);
				((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidRegisterStatusPump1>, JaycoTbbPidRegisterStatusPump1>)(object)_pump1Reader).Start(true);
			}
		}

		private void StopAllPidAutoRefresh()
		{
			((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_solarVoltagePidReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_solarCurrentPidReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_acVoltagePidReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_acCurrentPidReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcVoltagePidReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcCurrentPidReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_dcVoltageOutputPidReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_dcCurrentOutputPidReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargerVoltagePidReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargingDischargingCurrentPidReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<byte>, byte>)(object)_batteryStateOfChargePidReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<TimeSpan>, TimeSpan>)(object)_batteryMinutesUntilFullOrEmptyReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>, JaycoTbbPidStatusCmpSmp>)(object)_statusCmpSmpReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusRegister>, JaycoTbbPidStatusRegister>)(object)_statusRegisterReader).Stop();
			((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidRegisterStatusPump1>, JaycoTbbPidRegisterStatusPump1>)(object)_pump1Reader).Stop();
		}

		public override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			StopAllPidAutoRefresh();
			((CommonDisposableNotifyPropertyChanged)_solarVoltagePidReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_solarCurrentPidReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_acVoltagePidReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_acCurrentPidReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_auxDcVoltagePidReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_auxDcCurrentPidReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_dcVoltageOutputPidReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_dcCurrentOutputPidReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_batteryChargerVoltagePidReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_batteryChargingDischargingCurrentPidReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_batteryStateOfChargePidReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_batteryMinutesUntilFullOrEmptyReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_statusCmpSmpReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_statusRegisterReader).TryDispose();
			((CommonDisposableNotifyPropertyChanged)_pump1Reader).TryDispose();
		}

		[AsyncStateMachine(/*Could not decode attribute arguments.*/)]
		public virtual Task SetDcLoadOutputCutoffAsync(bool cutoff, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			<SetDcLoadOutputCutoffAsync>d__172 <SetDcLoadOutputCutoffAsync>d__ = default(<SetDcLoadOutputCutoffAsync>d__172);
			<SetDcLoadOutputCutoffAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SetDcLoadOutputCutoffAsync>d__.<>4__this = this;
			<SetDcLoadOutputCutoffAsync>d__.cutoff = cutoff;
			<SetDcLoadOutputCutoffAsync>d__.cancellationToken = cancellationToken;
			<SetDcLoadOutputCutoffAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <SetDcLoadOutputCutoffAsync>d__.<>t__builder)).Start<<SetDcLoadOutputCutoffAsync>d__172>(ref <SetDcLoadOutputCutoffAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <SetDcLoadOutputCutoffAsync>d__.<>t__builder)).Task;
		}

		public override void OnDeviceStatusChanged()
		{
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			base.OnDeviceStatusChanged();
			switch ((JaycoTbbPidRegister)((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidAddress)
			{
			case JaycoTbbPidRegister.PvInputVoltage:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_solarVoltagePidReader).Value = PidValueToPointZeroOneFloat(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.PvInputCurrent:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_solarCurrentPidReader).Value = PidValueToPointZeroOneFloat(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.AcInputVoltage:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_acVoltagePidReader).Value = PidValueToPointZeroOneFloat(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.AcInputCurrent:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_acCurrentPidReader).Value = PidValueToPointZeroOneFloat(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.AuxDcVoltage:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcVoltagePidReader).Value = PidValueToPointZeroOneFloat(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.AuxDcCurrent:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_auxDcCurrentPidReader).Value = PidValueToPointZeroOneFloat(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.DcVoltageOutput:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_dcVoltageOutputPidReader).Value = PidValueToPointZeroOneFloat(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.DcCurrentOutput:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_dcCurrentOutputPidReader).Value = PidValueToPointZeroOneFloat(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.BatteryChargerVoltage:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargerVoltagePidReader).Value = PidValueToPointZeroOneFloat(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.BatteryChargingDischargingCurrent:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<float>, float>)(object)_batteryChargingDischargingCurrentPidReader).Value = PidValueToPointZeroOneFloat(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.BatteryStateOfCharge:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<byte>, byte>)(object)_batteryStateOfChargePidReader).Value = PidValueTo1PercentIncrements(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.BatteryTimeToFullEmpty:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<TimeSpan>, TimeSpan>)(object)_batteryMinutesUntilFullOrEmptyReader).Value = PidValueToMinutes(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.StatusCmpSmp:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>, JaycoTbbPidStatusCmpSmp>)(object)_statusCmpSmpReader).Value = PidValueToStatusCmpSmp(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.StatusPump1:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidRegisterStatusPump1>, JaycoTbbPidRegisterStatusPump1>)(object)_pump1Reader).Value = PidValueToStatusPump(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			case JaycoTbbPidRegister.Status:
				((LogicalDevicePidAutoReader<ILogicalDevicePid<JaycoTbbPidStatusRegister>, JaycoTbbPidStatusRegister>)(object)_statusRegisterReader).Value = PidValueToStatusRegister(((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidData);
				break;
			default:
				TaggedLog.Debug("LogicalDeviceJaycoTbb", String.Format("Unexpected status update for {0}", (object)((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)this).DeviceStatus.PidAddress), Array.Empty<object>());
				break;
			}
		}

		protected void NotifyPropertyChanged(params string[] notifyPropertyNameEnumeration)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			MainThread.RequestMainThreadAction(new Action(new <>c__DisplayClass176_0
			{
				notifyPropertyNameEnumeration = notifyPropertyNameEnumeration,
				<>4__this = this
			}.<NotifyPropertyChanged>b__0), (RunBehavior)0);
		}
	}
	public class LogicalDeviceJaycoTbbFactory : DefaultLogicalDeviceFactory
	{
		public override ILogicalDevice MakeLogicalDevice(ILogicalDeviceService service, ILogicalDeviceId logicalDeviceId, Nullable<byte> rawCapability)
		{
			if (!logicalDeviceId.ProductId.IsValid || !logicalDeviceId.DeviceType.IsValid)
			{
				return null;
			}
			if (DEVICE_TYPE.op_Implicit(logicalDeviceId.DeviceType) != 46)
			{
				return null;
			}
			return (ILogicalDevice)(object)new LogicalDeviceJaycoTbb(logicalDeviceId, service);
		}
	}
	public class LogicalDeviceJaycoTbbSim : LogicalDeviceJaycoTbb, ILogicalDeviceSimulated, ILogicalDevice, IComparable, IEquatable<ILogicalDevice>, IComparable<ILogicalDevice>, ICommonDisposable, IDisposable, IDevicesCommon, INotifyPropertyChanged
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <SimulatorAsync>d__37 : ValueType, IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public CancellationToken cancellationtoken;

			public LogicalDeviceJaycoTbbSim <>4__this;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_01be: Expected O, but got Unknown
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_011c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0121: Unknown result type (might be due to invalid IL or missing references)
				//IL_0129: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_0103: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LogicalDeviceJaycoTbbSim logicalDeviceJaycoTbbSim = <>4__this;
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						if (num != 1)
						{
							goto IL_01aa;
						}
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<bool>);
						num = (<>1__state = -1);
						goto IL_0138;
					}
					val = <>u__1;
					<>u__1 = default(TaskAwaiter<bool>);
					num = (<>1__state = -1);
					goto IL_007e;
					IL_0138:
					val.GetResult();
					int num2 = logicalDeviceJaycoTbbSim.BatteryStateOfCharge + logicalDeviceJaycoTbbSim._simBatteryStateOfChargeInc;
					if (num2 < 0)
					{
						num2 = 1;
						logicalDeviceJaycoTbbSim._simBatteryStateOfChargeInc = 1;
					}
					else if (num2 > 100)
					{
						num2 = 99;
						logicalDeviceJaycoTbbSim._simBatteryStateOfChargeInc = -1;
					}
					byte value = LogicalDeviceJaycoTbb.PidValueTo1PercentIncrements((byte)num2);
					logicalDeviceJaycoTbbSim._simStatus.SetPidData(266, value);
					((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)logicalDeviceJaycoTbbSim).UpdateDeviceStatus((IReadOnlyList<byte>)(object)((LogicalDeviceDataPacketMutableDoubleBuffer)logicalDeviceJaycoTbbSim._simStatus).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)logicalDeviceJaycoTbbSim._simStatus).Size);
					goto IL_01aa;
					IL_01aa:
					if (!((CancellationToken)(ref cancellationtoken)).IsCancellationRequested)
					{
						val = TaskExtension.TryDelay(1000, cancellationtoken).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <SimulatorAsync>d__37>(ref val, ref this);
							return;
						}
						goto IL_007e;
					}
					goto end_IL_000e;
					IL_007e:
					val.GetResult();
					float num3 = logicalDeviceJaycoTbbSim.SolarVoltage + 0.1f;
					if (num3 > 13.5f)
					{
						num3 = 10f;
					}
					ushort value2 = (ushort)LogicalDeviceJaycoTbb.PidValueFloatToPointZeroOneUInt32(num3);
					logicalDeviceJaycoTbbSim._simStatus.SetPidData(256, value2);
					((LogicalDevice<LogicalDeviceJaycoTbbStatus, ILogicalDeviceCapability>)(object)logicalDeviceJaycoTbbSim).UpdateDeviceStatus((IReadOnlyList<byte>)(object)((LogicalDeviceDataPacketMutableDoubleBuffer)logicalDeviceJaycoTbbSim._simStatus).Data, ((LogicalDeviceDataPacketMutableDoubleBuffer)logicalDeviceJaycoTbbSim._simStatus).Size);
					val = TaskExtension.TryDelay(250, cancellationtoken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (<>1__state = 1);
						<>u__1 = val;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <SimulatorAsync>d__37>(ref val, ref this);
						return;
					}
					goto IL_0138;
					end_IL_000e:;
				}
				catch (Exception ex)
				{
					Exception exception = ex;
					<>1__state = -2;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		private const string LogTag = "LogicalDeviceJaycoTbbSim";

		private LogicalDeviceJaycoTbbStatus _simStatus = new LogicalDeviceJaycoTbbStatus();

		private const int TickIntervalMs = 1000;

		private const int TickIntervalDelayMs = 250;

		private readonly BackgroundOperation _simulator;

		private int _simBatteryStateOfChargeInc = 1;

		private bool _dcLoadOutputCutoff;

		protected override ILogicalDevicePid<float> MakeSolarVoltagePid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddressSimOfType<float, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.PvInputVoltage, 10.1f, (Func<uint, float>)LogicalDeviceJaycoTbb.PidValueToPointZeroOneFloat, (Func<float, uint>)LogicalDeviceJaycoTbb.PidValueFloatToPointZeroOneUInt32);

		protected override ILogicalDevicePid<float> MakeSolarCurrentPid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddressSimOfType<float, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.PvInputCurrent, 10.2f, (Func<uint, float>)LogicalDeviceJaycoTbb.PidValueToPointZeroOneFloat, (Func<float, uint>)LogicalDeviceJaycoTbb.PidValueFloatToPointZeroOneUInt32);

		protected override ILogicalDevicePid<float> MakeAcVoltagePid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddressSimOfType<float, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.AcInputVoltage, 11.1f, (Func<uint, float>)LogicalDeviceJaycoTbb.PidValueToPointZeroOneFloat, (Func<float, uint>)LogicalDeviceJaycoTbb.PidValueFloatToPointZeroOneUInt32);

		protected override ILogicalDevicePid<float> MakeAcCurrentPid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddressSimOfType<float, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.AcInputCurrent, 11.2f, (Func<uint, float>)LogicalDeviceJaycoTbb.PidValueToPointZeroOneFloat, (Func<float, uint>)LogicalDeviceJaycoTbb.PidValueFloatToPointZeroOneUInt32);

		protected override ILogicalDevicePid<float> MakeAuxDcVoltagePid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddressSimOfType<float, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.AuxDcVoltage, 12.1f, (Func<uint, float>)LogicalDeviceJaycoTbb.PidValueToPointZeroOneFloat, (Func<float, uint>)LogicalDeviceJaycoTbb.PidValueFloatToPointZeroOneUInt32);

		protected override ILogicalDevicePid<float> MakeAuxDcCurrentPid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddressSimOfType<float, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.AuxDcCurrent, 12.2f, (Func<uint, float>)LogicalDeviceJaycoTbb.PidValueToPointZeroOneFloat, (Func<float, uint>)LogicalDeviceJaycoTbb.PidValueFloatToPointZeroOneUInt32);

		protected override ILogicalDevicePid<float> MakeDcVoltageOutputPid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddressSimOfType<float, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.DcVoltageOutput, 13.1f, (Func<uint, float>)LogicalDeviceJaycoTbb.PidValueToPointZeroOneFloat, (Func<float, uint>)LogicalDeviceJaycoTbb.PidValueFloatToPointZeroOneUInt32);

		protected override ILogicalDevicePid<float> MakeDcCurrentOutputPid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddressSimOfType<float, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.DcCurrentOutput, 13.2f, (Func<uint, float>)LogicalDeviceJaycoTbb.PidValueToPointZeroOneFloat, (Func<float, uint>)LogicalDeviceJaycoTbb.PidValueFloatToPointZeroOneUInt32);

		protected override ILogicalDevicePid<float> MakeBatteryChargerVoltagePid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddressSimOfType<float, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.BatteryChargerVoltage, 9.1f, (Func<uint, float>)LogicalDeviceJaycoTbb.PidValueToPointZeroOneFloat, (Func<float, uint>)LogicalDeviceJaycoTbb.PidValueFloatToPointZeroOneUInt32);

		protected override ILogicalDevicePid<float> MakeBatteryChargingDischargingCurrentPid => (ILogicalDevicePid<float>)(object)new LogicalDevicePidAddressSimOfType<float, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.BatteryChargingDischargingCurrent, 9.2f, (Func<uint, float>)LogicalDeviceJaycoTbb.PidValueToPointZeroOneFloat, (Func<float, uint>)LogicalDeviceJaycoTbb.PidValueFloatToPointZeroOneUInt32);

		protected override ILogicalDevicePid<byte> MakeBatteryStateOfChargePid => (ILogicalDevicePid<byte>)(object)new LogicalDevicePidAddressSimOfType<byte, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.BatteryStateOfCharge, (byte)95, (Func<uint, byte>)LogicalDeviceJaycoTbb.PidValueTo1PercentIncrements, (Func<byte, uint>)LogicalDeviceJaycoTbb.PidValueByteTo1PercentIncrements);

		protected override ILogicalDevicePid<TimeSpan> MakeBatteryTimeUntilFullOrEmptyPid => (ILogicalDevicePid<TimeSpan>)(object)new LogicalDevicePidAddressSimOfType<TimeSpan, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.BatteryTimeToFullEmpty, TimeSpan.FromMinutes(5.0), (Func<uint, TimeSpan>)LogicalDeviceJaycoTbb.PidValueToMinutes, (Func<TimeSpan, uint>)LogicalDeviceJaycoTbb.PidValueFromMinutes);

		protected override ILogicalDevicePid<JaycoTbbPidStatusCmpSmp> MakeStatusCmpSmpPid => (ILogicalDevicePid<JaycoTbbPidStatusCmpSmp>)(object)new LogicalDevicePidAddressSimOfType<JaycoTbbPidStatusCmpSmp, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.BatteryTimeToFullEmpty, JaycoTbbPidStatusCmpSmp.OverloadError, (Func<uint, JaycoTbbPidStatusCmpSmp>)LogicalDeviceJaycoTbb.PidValueToStatusCmpSmp, (Func<JaycoTbbPidStatusCmpSmp, uint>)LogicalDeviceJaycoTbb.PidValueFromStatusCmpSmp);

		protected override ILogicalDevicePid<JaycoTbbPidStatusRegister> MakeStatusRegisterPid => (ILogicalDevicePid<JaycoTbbPidStatusRegister>)(object)new LogicalDevicePidAddressSimOfType<JaycoTbbPidStatusRegister, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.BatteryTimeToFullEmpty, JaycoTbbPidStatusRegister.OutdoorTempSensorConnected, (Func<uint, JaycoTbbPidStatusRegister>)LogicalDeviceJaycoTbb.PidValueToStatusRegister, (Func<JaycoTbbPidStatusRegister, uint>)LogicalDeviceJaycoTbb.PidValueFromStatusRegister);

		protected override ILogicalDevicePid<JaycoTbbPidRegisterStatusPump1> MakeStatusPump1Pid => (ILogicalDevicePid<JaycoTbbPidRegisterStatusPump1>)(object)new LogicalDevicePidAddressSimOfType<JaycoTbbPidRegisterStatusPump1, JaycoTbbPidRegister>(PidExtension.ConvertToPid((Pid)332), JaycoTbbPidRegister.BatteryTimeToFullEmpty, JaycoTbbPidRegisterStatusPump1.Off, (Func<uint, JaycoTbbPidRegisterStatusPump1>)LogicalDeviceJaycoTbb.PidValueToStatusPump, (Func<JaycoTbbPidRegisterStatusPump1, uint>)LogicalDeviceJaycoTbb.PidValueFromStatusPump);

		public override bool IsDcLoadOutputCutoff => _dcLoadOutputCutoff;

		public LogicalDeviceJaycoTbbSim(ILogicalDeviceId logicalDeviceId, ILogicalDeviceService deviceService)
			: base(logicalDeviceId, deviceService)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			_simulator = new BackgroundOperation(new BackgroundOperationFunc(SimulatorAsync));
			_simulator.Start();
		}

		[AsyncStateMachine(/*Could not decode attribute arguments.*/)]
		private Task SimulatorAsync(CancellationToken cancellationtoken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<SimulatorAsync>d__37 <SimulatorAsync>d__ = default(<SimulatorAsync>d__37);
			<SimulatorAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SimulatorAsync>d__.<>4__this = this;
			<SimulatorAsync>d__.cancellationtoken = cancellationtoken;
			<SimulatorAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <SimulatorAsync>d__.<>t__builder)).Start<<SimulatorAsync>d__37>(ref <SimulatorAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <SimulatorAsync>d__.<>t__builder)).Task;
		}

		public override Task SetDcLoadOutputCutoffAsync(bool cutoff, CancellationToken cancellationToken)
		{
			((CommonNotifyPropertyChanged)this).SetBackingField<bool>(ref _dcLoadOutputCutoff, cutoff, "IsDcLoadOutputCutoff", Array.Empty<string>());
			TaggedLog.Debug("LogicalDeviceJaycoTbbSim", String.Format("{0} to {1}", (object)"SetDcLoadOutputCutoffAsync", (object)cutoff), Array.Empty<object>());
			return (Task)(object)Task.FromResult<bool>(true);
		}

		public override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			_simulator.Stop();
		}
	}
	public class LogicalDeviceJaycoTbbStatus : LogicalDeviceStatusPacketMutable
	{
		public const int MinimumStatusPacketSize = 3;

		public const int MaximumStatusPacketSize = 6;

		public const int PidAddressStartIndex = 0;

		public const int PidValueStartIndex = 2;

		private byte[]? _tempSetBuffer;

		public ushort PidAddress => ((LogicalDeviceDataPacketMutableDoubleBuffer)this).GetUInt16(0u);

		public uint PidData
		{
			get
			{
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				if (!((LogicalDeviceDataPacketMutableDoubleBuffer)this).HasData)
				{
					return 0u;
				}
				return ((LogicalDeviceDataPacketMutableDoubleBuffer)this).Size switch
				{
					3u => ((LogicalDeviceDataPacketMutableDoubleBuffer)this).Data[2], 
					4u => ((LogicalDeviceDataPacketMutableDoubleBuffer)this).GetUInt16(2u), 
					5u => UInt24.op_Implicit(((LogicalDeviceDataPacketMutableDoubleBuffer)this).GetUInt24(2u)), 
					6u => ((LogicalDeviceDataPacketMutableDoubleBuffer)this).GetUInt32(2u), 
					_ => 0u, 
				};
			}
		}

		private byte[] TempSetBuffer => _tempSetBuffer ?? (_tempSetBuffer = (byte[]?)(object)new Byte[6]);

		public LogicalDeviceJaycoTbbStatus()
			: base(3u, 6u, (byte)0)
		{
		}

		public void SetPidData(ushort address, byte value)
		{
			lock (this)
			{
				ArrayExtension.Clear<byte>(TempSetBuffer);
				ArrayExtension.SetValueUInt16(TempSetBuffer, address, 0, (Endian)0);
				TempSetBuffer[2] = value;
				((LogicalDeviceDataPacketMutableDoubleBuffer)this).Update(TempSetBuffer, 3, false);
			}
		}

		public void SetPidData(ushort address, ushort value)
		{
			lock (this)
			{
				ArrayExtension.Clear<byte>(TempSetBuffer);
				ArrayExtension.SetValueUInt16(TempSetBuffer, address, 0, (Endian)0);
				ArrayExtension.SetValueUInt16(TempSetBuffer, value, 2, (Endian)0);
				((LogicalDeviceDataPacketMutableDoubleBuffer)this).Update(TempSetBuffer, 4, false);
			}
		}

		public void SetPidData(ushort address, UInt24 value)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			lock (this)
			{
				ArrayExtension.Clear<byte>(TempSetBuffer);
				ArrayExtension.SetValueUInt16(TempSetBuffer, address, 0, (Endian)0);
				ArrayExtension.SetValueUInt24(TempSetBuffer, UInt24.op_Implicit(value), 2, (Endian)0);
				((LogicalDeviceDataPacketMutableDoubleBuffer)this).Update(TempSetBuffer, 26, false);
			}
		}

		public void SetPidData(ushort address, uint value)
		{
			lock (this)
			{
				ArrayExtension.Clear<byte>(TempSetBuffer);
				ArrayExtension.SetValueUInt16(TempSetBuffer, address, 0, (Endian)0);
				ArrayExtension.SetValueUInt32(TempSetBuffer, value, 2, (Endian)0);
				((LogicalDeviceDataPacketMutableDoubleBuffer)this).Update(TempSetBuffer, 6, false);
			}
		}

		public LogicalDeviceJaycoTbbStatus(LogicalDeviceJaycoTbbStatus originalStatus)
		{
			byte[] data = ((LogicalDeviceDataPacketMutableDoubleBuffer)originalStatus).Data;
			((LogicalDeviceDataPacketMutableDoubleBuffer)this).Update(data, data.Length, false);
		}
	}
	public static class LogicalDeviceServiceExtension : Object
	{
		public static void RegisterJaycoTbbLogicalDeviceFactories(this ILogicalDeviceService deviceService)
		{
			deviceService.RegisterLogicalDeviceFactory((ILogicalDeviceFactory)(object)new LogicalDeviceJaycoTbbFactory());
			JsonSerializer.AutoRegisterJsonSerializersFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}
