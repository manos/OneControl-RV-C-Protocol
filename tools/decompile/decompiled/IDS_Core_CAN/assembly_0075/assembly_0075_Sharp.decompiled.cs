using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using AndroidX.AppCompat.Content.Res;
using AndroidX.Car.App;
using AndroidX.Car.App.Model;
using AndroidX.Car.App.Validation;
using AndroidX.Core.Graphics.Drawable;
using AndroidX.Lifecycle;
using AppAnalyticsLib;
using IDS.Portable.Devices.TPMS;
using IDS.Portable.LogicalDevice;
using Java.Interop;
using Java.Lang;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Storage;
using OneControl.Devices;
using OneControl.Devices.TPMS;
using SkiaSharp;
using Svg.Skia;
using _Microsoft.Android.Resource.Designer;
using ids.auto.Devices.TireLinc;
using ids.auto.Extensions;
using ids.auto.Helper;
using ids.auto.Platforms.Android;
using ids.auto.Resources;
using ids.auto.Resources.Colors;
using ids.auto.Services;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: TargetFramework(".NETCoreApp,Version=v9.0", FrameworkDisplayName = ".NET 9.0")]
[assembly: AssemblyCompany("Lippert")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("Copyright © Lippert 2025")]
[assembly: AssemblyDescription("Added support for Android Auto and iOS CarPlay to a MAUI app.")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0+a378bc947d9816e8227306505fd7ca9079babf60")]
[assembly: AssemblyProduct("ids.auto")]
[assembly: AssemblyTitle("ids.auto")]
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/lci-ids/ids.auto")]
[assembly: TargetPlatform("Android35.0")]
[assembly: SupportedOSPlatform("Android21.0")]
[assembly: AssemblyVersion("1.0.0.0")]
[module: RefSafetyRules(11)]
namespace ids.auto
{
	public class ApplicationContext : IMauiInitializeService
	{
		private static readonly Lazy<ApplicationContext> _instance = new Lazy<ApplicationContext>((Func<ApplicationContext>)(() => new ApplicationContext()), (LazyThreadSafetyMode)2);

		private bool _isInitialized;

		private readonly object _initializationLock = new object();

		public static ApplicationContext Instance => _instance.Value;

		[field: CompilerGenerated]
		public ILoggerFactory? LoggerFactory
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IAppAnalytics? AppAnalytics
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IDeviceDataService? DeviceDataService
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		private ApplicationContext()
		{
		}

		public void Initialize(IServiceProvider services)
		{
			lock (_initializationLock)
			{
				if (_isInitialized)
				{
					return;
				}
				LoggerFactory = ServiceProviderServiceExtensions.GetRequiredService<ILoggerFactory>(services);
				ILogger<ApplicationContext> val = LoggerFactoryExtensions.CreateLogger<ApplicationContext>(LoggerFactory);
				try
				{
					LoggerExtensions.LogInformation((ILogger)(object)val, "Initializing application context", global::System.Array.Empty<object>());
					AppAnalytics = ServiceProviderServiceExtensions.GetService<IAppAnalytics>(services);
					if (AppAnalytics == null)
					{
						LoggerExtensions.LogWarning((ILogger)(object)val, "IAppAnalytics service is not registered", global::System.Array.Empty<object>());
					}
					DeviceDataService = ServiceProviderServiceExtensions.GetService<IDeviceDataService>(services);
					if (DeviceDataService == null)
					{
						LoggerExtensions.LogWarning((ILogger)(object)val, "IDeviceDataService service is not registered", global::System.Array.Empty<object>());
					}
					_isInitialized = true;
					LoggerExtensions.LogInformation((ILogger)(object)val, "ApplicationContext initialized successfully", global::System.Array.Empty<object>());
				}
				catch (global::System.Exception ex)
				{
					LoggerExtensions.LogError((ILogger)(object)val, ex, "Failed to initialize ApplicationContext", global::System.Array.Empty<object>());
					throw;
				}
			}
		}

		public T GetRequiredService<T>(T? service, string serviceName) where T : class
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (service == null)
			{
				throw new InvalidOperationException(serviceName + " is not available. Make sure ApplicationContext has been properly initialized.");
			}
			return service;
		}

		public ILogger<T> GetLogger<T>() where T : class
		{
			return LoggerFactoryExtensions.CreateLogger<T>(GetRequiredService<ILoggerFactory>(LoggerFactory, "LoggerFactory"));
		}
	}
	public static class AutoMauiBuilder
	{
		public static MauiAppBuilder UseAuto(this MauiAppBuilder builder)
		{
			ServiceCollectionServiceExtensions.AddSingleton<IDeviceDataService>(builder.Services, (Func<IServiceProvider, IDeviceDataService>)((IServiceProvider serviceProvider) => new DeviceDataService(ServiceProviderServiceExtensions.GetRequiredService<ILogicalDeviceManager>(serviceProvider))));
			ServiceCollectionServiceExtensions.AddSingleton<IPlatformMessageService, PlatformMessageService>(builder.Services);
			ServiceCollectionDescriptorExtensions.TryAddEnumerable(builder.Services, ServiceDescriptor.Singleton<IMauiInitializeService, ApplicationContext>((Func<IServiceProvider, ApplicationContext>)delegate(IServiceProvider serviceProvider)
			{
				ApplicationContext instance = ApplicationContext.Instance;
				instance.Initialize(serviceProvider);
				return instance;
			}));
			return builder;
		}
	}
	public static class TirePressureHelper
	{
		public const int RefreshIntervalMs = 2000;

		private const string DefaultTitle = "One Control Tire Linc";

		private static readonly global::System.Type TirePositionNameType = typeof(TirePositionName);

		public const int AndroidBaseIndex = 11;

		public const int IosBaseIndex = 9;

		public static string FormatSensorText(string sensorName, string measurementText, int baseIndex)
		{
			return sensorName.Length switch
			{
				5 => sensorName + new string('\u2007', baseIndex + 3) + measurementText, 
				11 => sensorName + new string('\u2007', baseIndex) + measurementText, 
				12 => sensorName + new string('\u2007', baseIndex - 1) + measurementText, 
				14 => sensorName + new string('\u2007', baseIndex - 4) + measurementText, 
				16 => sensorName + new string('\u2007', baseIndex - 4) + measurementText, 
				17 => sensorName + new string('\u2007', baseIndex - 5) + measurementText, 
				18 => sensorName + new string('\u2007', baseIndex - 3) + measurementText, 
				19 => sensorName + new string('\u2007', baseIndex - 4) + measurementText, 
				20 => sensorName + new string('\u2007', baseIndex - 5) + measurementText, 
				_ => sensorName + new string('\u2007', baseIndex - 6) + measurementText, 
			};
		}

		public static string GetSensorPositionName(int index, TirePressureSensor sensor, bool isSimulated)
		{
			try
			{
				return sensor.GetFormattedName();
			}
			catch (global::System.Exception ex)
			{
				Console.WriteLine("Error getting sensor position name: " + ex.Message);
			}
			return string.Empty;
		}

		public static string FormatMeasurementText(TirePressureSensor sensor)
		{
			string obj = (sensor.HasBeenPaired ? sensor.Pressure.ToString() : TPMS.dashdash);
			string text = (sensor.HasBeenPaired ? sensor.Temperature.ToString() : TPMS.dashdash);
			return obj + " PSI | " + text + "°F";
		}

		public static RSSILevelTypes MapRSSIToLevel(sbyte rssiValue)
		{
			if (rssiValue >= -80)
			{
				if (rssiValue >= -60)
				{
					if (rssiValue >= -50)
					{
						return RSSILevelTypes.Excellent;
					}
					return RSSILevelTypes.Good;
				}
				if (rssiValue >= -70)
				{
					return RSSILevelTypes.Fair;
				}
			}
			else if (rssiValue >= -90)
			{
				return RSSILevelTypes.NoSignal;
			}
			return RSSILevelTypes.Weak;
		}

		public static ValueTuple<bool, global::System.Collections.Generic.IList<TirePressureSensor>?, ILogicalDevice?, int, string> CheckSensorsAvailability(IDeviceDataService? deviceDataService, ILogger? logger = null)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			if (deviceDataService == null)
			{
				if (logger != null)
				{
					LoggerExtensions.LogWarning(logger, "Device data service is not available", global::System.Array.Empty<object>());
				}
				return new ValueTuple<bool, global::System.Collections.Generic.IList<TirePressureSensor>, ILogicalDevice, int, string>(false, (global::System.Collections.Generic.IList<TirePressureSensor>)null, (ILogicalDevice)null, 0, TPMS.rv_tire_pressure);
			}
			global::System.Collections.Generic.IList<TirePressureSensor> tirePressureSensors = deviceDataService.GetTirePressureSensors();
			ILogicalDeviceTirePressureMonitor tirePressureMonitor = deviceDataService.GetTirePressureMonitor();
			if (tirePressureMonitor == null)
			{
				return new ValueTuple<bool, global::System.Collections.Generic.IList<TirePressureSensor>, ILogicalDevice, int, string>(false, (global::System.Collections.Generic.IList<TirePressureSensor>)null, (ILogicalDevice)null, 0, TPMS.rv_tire_pressure);
			}
			if (tirePressureSensors != null && ((global::System.Collections.Generic.ICollection<TirePressureSensor>)tirePressureSensors).Count <= 0)
			{
				((ITirePressureMonitor)tirePressureMonitor).ConnectAsync(default(CancellationToken), false);
			}
			int num = Enumerable.Count<VehicleConfiguration>((global::System.Collections.Generic.IEnumerable<VehicleConfiguration>)((ITirePressureMonitor)tirePressureMonitor).VehicleConfigurations, (Func<VehicleConfiguration, bool>)delegate(VehicleConfiguration vehicleConfiguration)
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Invalid comparison between Unknown and I4
				bool flag = vehicleConfiguration != null;
				if (flag)
				{
					VehicleClass vehicleClass = vehicleConfiguration.VehicleClass;
					bool flag2 = (int)vehicleClass <= 1;
					flag = !flag2;
				}
				return flag;
			});
			string text = (((ITirePressureMonitor)tirePressureMonitor).IsOnline ? string.Empty : (" " + TPMS.offline));
			string text2 = ((tirePressureMonitor != null) ? ((ILogicalDevice)(object)tirePressureMonitor).GetDefaultTitle() : null);
			if (num > 0)
			{
				text2 = ((tirePressureMonitor != null) ? ((ITirePressureMonitor)tirePressureMonitor).ActiveVehicleConfiguration.VehicleClass.ToFriendlyString() : null) + text;
			}
			if (tirePressureSensors != null && Enumerable.Any<TirePressureSensor>((global::System.Collections.Generic.IEnumerable<TirePressureSensor>)tirePressureSensors))
			{
				return new ValueTuple<bool, global::System.Collections.Generic.IList<TirePressureSensor>, ILogicalDevice, int, string>(true, tirePressureSensors, (ILogicalDevice)(object)tirePressureMonitor, num, text2);
			}
			if (logger != null)
			{
				LoggerExtensions.LogWarning(logger, "No sensor data available", global::System.Array.Empty<object>());
			}
			return new ValueTuple<bool, global::System.Collections.Generic.IList<TirePressureSensor>, ILogicalDevice, int, string>(false, (global::System.Collections.Generic.IList<TirePressureSensor>)null, (ILogicalDevice)(object)tirePressureMonitor, num, text2);
		}

		private static string GetDefaultTitle(this ILogicalDevice device)
		{
			string text = ((device is LogicalDeviceTpmsLegacyAdapter) ? (" " + TPMS.tirelinc_pro_ext) : string.Empty);
			return ((IDevicesCommon)device).DeviceNameShort + text;
		}

		public static string ConvertSensorStatusToString(TirePressureSensor sensor)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected I4, but got Unknown
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Invalid comparison between Unknown and I4
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Invalid comparison between Unknown and I4
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			TimeSpan val = global::System.DateTime.Now - sensor.StatusLastUpdated;
			string text = string.Empty;
			if (((TimeSpan)(ref val)).TotalMinutes > 1.0)
			{
				text = $"  {TPMS.last_updated} {sensor.StatusLastUpdated: h:mm tt}";
			}
			if (!sensor.HasBeenPaired)
			{
				return TPMS.not_paired;
			}
			if (sensor.IsBatteryLow)
			{
				return TPMS.low_battery + " " + text;
			}
			if (sensor.RSSI <= -100)
			{
				return TPMS.signal_lost + " " + text;
			}
			PressureFaultType pressureFault = sensor.PressureFault;
			switch ((int)pressureFault)
			{
			case 2:
				return TPMS.low_pressure + " " + text;
			case 1:
				return TPMS.high_pressure + " " + text;
			default:
				throw new ArgumentOutOfRangeException();
			case 0:
			case 3:
			{
				TemperatureFaultType temperatureFault = sensor.TemperatureFault;
				if ((int)temperatureFault != 1)
				{
					if ((int)temperatureFault == 3)
					{
						return TPMS.relative_temperature + " " + text;
					}
					return text;
				}
				return TPMS.high_temperature + " " + text;
			}
			}
		}

		public static string GetFormattedName(this TirePressureSensor sensor)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected I4, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Invalid comparison between Unknown and I4
			if (sensor == null)
			{
				return string.Empty;
			}
			TirePositionName tirePositionName = sensor.TirePositionName;
			string text = (int)tirePositionName switch
			{
				0 => TPMS.left, 
				1 => TPMS.right, 
				6 => TPMS.spare, 
				2 => TPMS.outside + " " + TPMS.left, 
				5 => TPMS.outside + " " + TPMS.right, 
				3 => TPMS.inside + " " + TPMS.left, 
				4 => TPMS.inside + " " + TPMS.right, 
				_ => string.Empty, 
			};
			if ((int)sensor.TirePositionName != 6)
			{
				return $"{TPMS.axle} {sensor.AxleNumber + 1} {text}";
			}
			return text;
		}
	}
	[Service(Exported = true)]
	[IntentFilter(new string[] { "androidx.car.app.CarAppService" }, Categories = new string[] { "androidx.car.app.category.IOT" })]
	public class AACarAppService : CarAppService
	{
		public override HostValidator CreateHostValidator()
		{
			return HostValidator.AllowAllHostsValidator;
		}

		public override Session OnCreateSession()
		{
			SetAutoRunning(isRunning: true);
			return (Session)(object)new CarDiagnosticSession();
		}

		public override void OnDestroy()
		{
			SetAutoRunning(isRunning: false);
			((Service)this).OnDestroy();
		}

		public static void SetAutoRunning(bool isRunning)
		{
			Application.Context.GetSharedPreferences("AutoState", (FileCreationMode)0).Edit().PutBoolean("isAutoRunning", isRunning)
				.Apply();
		}

		public static bool IsAutoRunning()
		{
			return Application.Context.GetSharedPreferences("AutoState", (FileCreationMode)0).GetBoolean("isAutoRunning", false);
		}
	}
	public class CarDiagnosticSession : Session
	{
		private readonly object _lockObject = new object();

		public CarDiagnosticSession()
		{
			((Session)this).Lifecycle.AddObserver((ILifecycleObserver)(object)new CarLifecycleObserver());
		}

		public override Screen OnCreateScreen(Intent p0)
		{
			_ = ((Session)this).CarContext.CarAppApiLevel;
			IAppAnalytics? appAnalytics = ApplicationContext.Instance.AppAnalytics;
			if (appAnalytics != null)
			{
				appAnalytics.TrackSceneEvent("create.auto.screen", "TirePressureScreen");
			}
			TirePressureScreen tirePressureScreen = new TirePressureScreen(((Session)this).CarContext);
			PlatformMessageService.RegisterScreen(tirePressureScreen);
			((Screen)(object)tirePressureScreen).GetScreenWidth();
			return (Screen)(object)tirePressureScreen;
		}
	}
	public static class ScreenExtensions
	{
		public static int GetScreenWidth(this Screen screen)
		{
			return GetScreenWidth(screen.CarContext);
		}

		private static int GetScreenWidth([NotNull] CarContext context)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			ArgumentNullException.ThrowIfNull((object)context, "context");
			IWindowManager val = Extensions.JavaCast<IWindowManager>((IJavaObject)(object)((Context)context).GetSystemService("window"));
			if (val == null)
			{
				return 0;
			}
			if ((int)VERSION.SdkInt >= 30)
			{
				WindowMetrics currentWindowMetrics = val.CurrentWindowMetrics;
				Insets insetsIgnoringVisibility = val.CurrentWindowMetrics.WindowInsets.GetInsetsIgnoringVisibility(Type.SystemBars());
				return currentWindowMetrics.Bounds.Width() - insetsIgnoringVisibility.Left - insetsIgnoringVisibility.Right;
			}
			DisplayMetrics val2 = new DisplayMetrics();
			val.DefaultDisplay.GetMetrics(val2);
			return val2.WidthPixels;
		}
	}
	public class TirePressureScreen : Screen, IDefaultLifecycleObserver, ILifecycleObserver, IJavaObject, global::System.IDisposable, IJavaPeerable
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <LoadIconsAsync>d__24 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public TirePressureScreen <>4__this;

			private TaskAwaiter<CarIcon?> <>u__1;

			private void MoveNext()
			{
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0162: Unknown result type (might be due to invalid IL or missing references)
				//IL_0167: Unknown result type (might be due to invalid IL or missing references)
				//IL_016e: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01df: Unknown result type (might be due to invalid IL or missing references)
				//IL_0244: Unknown result type (might be due to invalid IL or missing references)
				//IL_0249: Unknown result type (might be due to invalid IL or missing references)
				//IL_0250: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_0326: Unknown result type (might be due to invalid IL or missing references)
				//IL_032b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0332: Unknown result type (might be due to invalid IL or missing references)
				//IL_0397: Unknown result type (might be due to invalid IL or missing references)
				//IL_039c: Unknown result type (might be due to invalid IL or missing references)
				//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_00be: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_012f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0134: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0211: Unknown result type (might be due to invalid IL or missing references)
				//IL_0216: Unknown result type (might be due to invalid IL or missing references)
				//IL_0273: Unknown result type (might be due to invalid IL or missing references)
				//IL_0282: Unknown result type (might be due to invalid IL or missing references)
				//IL_0287: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0355: Unknown result type (might be due to invalid IL or missing references)
				//IL_0364: Unknown result type (might be due to invalid IL or missing references)
				//IL_0369: Unknown result type (might be due to invalid IL or missing references)
				//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
				//IL_03d9: Expected O, but got Unknown
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0148: Unknown result type (might be due to invalid IL or missing references)
				//IL_0149: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_022a: Unknown result type (might be due to invalid IL or missing references)
				//IL_022b: Unknown result type (might be due to invalid IL or missing references)
				//IL_029b: Unknown result type (might be due to invalid IL or missing references)
				//IL_029c: Unknown result type (might be due to invalid IL or missing references)
				//IL_030c: Unknown result type (might be due to invalid IL or missing references)
				//IL_030d: Unknown result type (might be due to invalid IL or missing references)
				//IL_037d: Unknown result type (might be due to invalid IL or missing references)
				//IL_037e: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				TirePressureScreen tirePressureScreen = <>4__this;
				try
				{
					_ = 7;
					try
					{
						TaskAwaiter<CarIcon> val;
						CarIcon result;
						switch (num)
						{
						default:
							val = CarIconHelper.CreateCarIconAsync("full_signal.svg").GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CarIcon>, <LoadIconsAsync>d__24>(ref val, ref this);
								return;
							}
							goto IL_009b;
						case 0:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<CarIcon>);
							num = (<>1__state = -1);
							goto IL_009b;
						case 1:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<CarIcon>);
							num = (<>1__state = -1);
							goto IL_010c;
						case 2:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<CarIcon>);
							num = (<>1__state = -1);
							goto IL_017d;
						case 3:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<CarIcon>);
							num = (<>1__state = -1);
							goto IL_01ee;
						case 4:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<CarIcon>);
							num = (<>1__state = -1);
							goto IL_025f;
						case 5:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<CarIcon>);
							num = (<>1__state = -1);
							goto IL_02d0;
						case 6:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<CarIcon>);
							num = (<>1__state = -1);
							goto IL_0341;
						case 7:
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<CarIcon>);
								num = (<>1__state = -1);
								break;
							}
							IL_01ee:
							result = val.GetResult();
							tirePressureScreen._weakSignalIcon = result;
							val = CarIconHelper.CreateCarIconAsync("no_signal.svg").GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 4);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CarIcon>, <LoadIconsAsync>d__24>(ref val, ref this);
								return;
							}
							goto IL_025f;
							IL_009b:
							result = val.GetResult();
							tirePressureScreen._fullSignalIcon = result;
							val = CarIconHelper.CreateCarIconAsync("good_signal.svg").GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CarIcon>, <LoadIconsAsync>d__24>(ref val, ref this);
								return;
							}
							goto IL_010c;
							IL_02d0:
							result = val.GetResult();
							tirePressureScreen._warningIconRed = result;
							val = CarIconHelper.CreateCarIconAsync("warning.svg", AutoColors.ErrorOrange).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 6);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CarIcon>, <LoadIconsAsync>d__24>(ref val, ref this);
								return;
							}
							goto IL_0341;
							IL_010c:
							result = val.GetResult();
							tirePressureScreen._goodSignalIcon = result;
							val = CarIconHelper.CreateCarIconAsync("fair_signal.svg").GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 2);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CarIcon>, <LoadIconsAsync>d__24>(ref val, ref this);
								return;
							}
							goto IL_017d;
							IL_025f:
							result = val.GetResult();
							tirePressureScreen._noSignalIcon = result;
							val = CarIconHelper.CreateCarIconAsync("warning.svg", AutoColors.ErrorRed).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 5);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CarIcon>, <LoadIconsAsync>d__24>(ref val, ref this);
								return;
							}
							goto IL_02d0;
							IL_017d:
							result = val.GetResult();
							tirePressureScreen._fairSignalIcon = result;
							val = CarIconHelper.CreateCarIconAsync("weak_signal.svg").GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 3);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CarIcon>, <LoadIconsAsync>d__24>(ref val, ref this);
								return;
							}
							goto IL_01ee;
							IL_0341:
							result = val.GetResult();
							tirePressureScreen._warningIconOrange = result;
							val = CarIconHelper.CreateCarIconAsync("warning.svg", AutoColors.ErrorBlue).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 7);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CarIcon>, <LoadIconsAsync>d__24>(ref val, ref this);
								return;
							}
							break;
						}
						result = val.GetResult();
						tirePressureScreen._warningIconBlue = result;
						tirePressureScreen._areIconsLoaded = true;
						MainThread.BeginInvokeOnMainThread(new Action(((Screen)tirePressureScreen).Invalidate));
					}
					catch (global::System.Exception ex)
					{
						ILogger<TirePressureScreen>? logger = tirePressureScreen._logger;
						if (logger != null)
						{
							LoggerExtensions.LogError((ILogger)(object)logger, ex, "Error loading icons", global::System.Array.Empty<object>());
						}
					}
				}
				catch (global::System.Exception exception)
				{
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

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <StartPeriodicRefreshAsync>d__25 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public CancellationToken cancellationToken;

			public TirePressureScreen <>4__this;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Expected O, but got Unknown
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				TirePressureScreen tirePressureScreen = <>4__this;
				try
				{
					try
					{
						if (num != 0)
						{
							goto IL_009c;
						}
						TaskAwaiter val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_0077;
						IL_0077:
						((TaskAwaiter)(ref val)).GetResult();
						if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
						{
							MainThread.BeginInvokeOnMainThread(new Action(((Screen)tirePressureScreen).Invalidate));
						}
						goto IL_009c;
						IL_009c:
						if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
						{
							val = global::System.Threading.Tasks.Task.Delay(2000, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <StartPeriodicRefreshAsync>d__25>(ref val, ref this);
								return;
							}
							goto IL_0077;
						}
					}
					catch (OperationCanceledException)
					{
					}
					catch (global::System.Exception ex2)
					{
						ILogger<TirePressureScreen>? logger = tirePressureScreen._logger;
						if (logger != null)
						{
							LoggerExtensions.LogError((ILogger)(object)logger, ex2, "Error in periodic refresh", global::System.Array.Empty<object>());
						}
					}
				}
				catch (global::System.Exception exception)
				{
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

		private CarIcon? _fullSignalIcon;

		private CarIcon? _goodSignalIcon;

		private CarIcon? _fairSignalIcon;

		private CarIcon? _weakSignalIcon;

		private CarIcon? _noSignalIcon;

		private CarIcon? _warningIconRed;

		private CarIcon? _warningIconOrange;

		private CarIcon? _warningIconBlue;

		private readonly CarContext _carContext;

		private int _screenWidth;

		private IDeviceDataService? _deviceDataService;

		private ILogger<TirePressureScreen>? _logger;

		private CancellationTokenSource? _refreshCancellationTokenSource;

		private bool _isDisposed;

		private bool _areIconsLoaded;

		public TirePressureScreen(CarContext carContext)
			: base(carContext)
		{
			_carContext = carContext;
			((Screen)this).Lifecycle.AddObserver((ILifecycleObserver)(object)this);
		}

		public void OnStart(ILifecycleOwner owner)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			_deviceDataService = ApplicationContext.Instance.DeviceDataService;
			_logger = ApplicationContext.Instance.GetLogger<TirePressureScreen>();
			ILogger<TirePressureScreen>? logger = _logger;
			if (logger != null)
			{
				LoggerExtensions.LogDebug((ILogger)(object)logger, "TirePressureScreen OnStart called", global::System.Array.Empty<object>());
			}
			_screenWidth = ((Screen)(object)this).GetScreenWidth();
			_refreshCancellationTokenSource = new CancellationTokenSource();
			LoadIconsAsync();
			StartPeriodicRefreshAsync(_refreshCancellationTokenSource.Token);
		}

		public void OnResume(ILifecycleOwner owner)
		{
			ILogger<TirePressureScreen>? logger = _logger;
			if (logger != null)
			{
				LoggerExtensions.LogDebug((ILogger)(object)logger, "TirePressureScreen OnResume called", global::System.Array.Empty<object>());
			}
			((Screen)this).Invalidate();
		}

		public void OnPause(ILifecycleOwner owner)
		{
			ILogger<TirePressureScreen>? logger = _logger;
			if (logger != null)
			{
				LoggerExtensions.LogDebug((ILogger)(object)logger, "TirePressureScreen OnPause called", global::System.Array.Empty<object>());
			}
		}

		public void OnStop(ILifecycleOwner owner)
		{
			ILogger<TirePressureScreen>? logger = _logger;
			if (logger != null)
			{
				LoggerExtensions.LogDebug((ILogger)(object)logger, "TirePressureScreen OnStop called", global::System.Array.Empty<object>());
			}
			CancellationTokenSource? refreshCancellationTokenSource = _refreshCancellationTokenSource;
			if (refreshCancellationTokenSource != null)
			{
				refreshCancellationTokenSource.Cancel();
			}
		}

		public void OnDestroy(ILifecycleOwner owner)
		{
			ILogger<TirePressureScreen>? logger = _logger;
			if (logger != null)
			{
				LoggerExtensions.LogDebug((ILogger)(object)logger, "TirePressureScreen OnDestroy called", global::System.Array.Empty<object>());
			}
			((Object)this).Dispose();
		}

		public override ITemplate OnGetTemplate()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				ValueTuple<bool, global::System.Collections.Generic.IList<TirePressureSensor>?, ILogicalDevice?, int, string> val = TirePressureHelper.CheckSensorsAvailability(_deviceDataService, (ILogger?)(object)_logger);
				global::System.Collections.Generic.IList<TirePressureSensor> item = val.Item2;
				ILogicalDevice item2 = val.Item3;
				int item3 = val.Item4;
				string item4 = val.Item5;
				if (!(item2 is ILogicalDeviceTirePressureMonitor) || !_areIconsLoaded)
				{
					return (ITemplate)(object)new Builder(TPMS.no_sensor_data).SetTitle(item4).Build();
				}
				if (item3 == 0)
				{
					return (ITemplate)(object)new Builder(TPMS.no_trailers).SetTitle(item4).Build();
				}
				if (!_areIconsLoaded)
				{
					return (ITemplate)(object)new Builder(TPMS.resources_loading).SetTitle(TPMS.please_wait).Build();
				}
				Builder val2 = new Builder();
				AddSensorsToItemList(val2, item, item2);
				ItemList singleList = val2.Build();
				return (ITemplate)(object)new Builder().SetTitle(item4).SetSingleList(singleList).Build();
			}
			catch (global::System.Exception ex)
			{
				ILogger<TirePressureScreen>? logger = _logger;
				if (logger != null)
				{
					LoggerExtensions.LogError((ILogger)(object)logger, ex, "Error creating template", global::System.Array.Empty<object>());
				}
				return (ITemplate)(object)new Builder("An error occurred: " + ex.Message).SetTitle("Error").Build();
			}
		}

		private void AddSensorsToItemList(Builder itemListBuilder, global::System.Collections.Generic.IList<TirePressureSensor> sensors, ILogicalDevice? device)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				for (int i = 0; i < ((global::System.Collections.Generic.ICollection<TirePressureSensor>)sensors).Count; i++)
				{
					TirePressureSensor val = sensors[i];
					if (!string.IsNullOrWhiteSpace(((object)val.TirePositionName/*cast due to .constrained prefix*/).ToString()))
					{
						string sensorPositionName = TirePressureHelper.GetSensorPositionName(i, val, device is ILogicalDeviceSimulated);
						string text = TirePressureHelper.FormatMeasurementText(val);
						TirePressureHelper.FormatSensorText(sensorPositionName, text, 11);
						CarIcon image = ConvertSensorStatusToIcon(val);
						itemListBuilder.AddItem((IItem)(object)new Builder().SetTitle(sensorPositionName).AddText(text + " " + TirePressureHelper.ConvertSensorStatusToString(val)).SetImage(image)
							.Build());
					}
				}
			}
			catch (global::System.Exception ex)
			{
				ILogger<TirePressureScreen>? logger = _logger;
				if (logger != null)
				{
					LoggerExtensions.LogError((ILogger)(object)logger, ex, "Error adding sensors to item list", global::System.Array.Empty<object>());
				}
			}
		}

		public void ShowToast(string message)
		{
			CarToast.MakeText(_carContext, message, 1).Show();
		}

		[AsyncStateMachine(typeof(<LoadIconsAsync>d__24))]
		private global::System.Threading.Tasks.Task LoadIconsAsync()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			<LoadIconsAsync>d__24 <LoadIconsAsync>d__ = default(<LoadIconsAsync>d__24);
			<LoadIconsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadIconsAsync>d__.<>4__this = this;
			<LoadIconsAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <LoadIconsAsync>d__.<>t__builder)).Start<<LoadIconsAsync>d__24>(ref <LoadIconsAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <LoadIconsAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<StartPeriodicRefreshAsync>d__25))]
		private global::System.Threading.Tasks.Task StartPeriodicRefreshAsync(CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<StartPeriodicRefreshAsync>d__25 <StartPeriodicRefreshAsync>d__ = default(<StartPeriodicRefreshAsync>d__25);
			<StartPeriodicRefreshAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<StartPeriodicRefreshAsync>d__.<>4__this = this;
			<StartPeriodicRefreshAsync>d__.cancellationToken = cancellationToken;
			<StartPeriodicRefreshAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <StartPeriodicRefreshAsync>d__.<>t__builder)).Start<<StartPeriodicRefreshAsync>d__25>(ref <StartPeriodicRefreshAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <StartPeriodicRefreshAsync>d__.<>t__builder)).Task;
		}

		private CarIcon? ConvertSensorStatusToIcon(TirePressureSensor sensor)
		{
			if (sensor.IsFaulted())
			{
				return _warningIconRed;
			}
			if (sensor.IsBatteryLow || sensor.RSSI <= -100)
			{
				return _warningIconBlue;
			}
			if (!sensor.HasBeenPaired)
			{
				return _warningIconOrange;
			}
			return (CarIcon?)(TirePressureHelper.MapRSSIToLevel(sensor.RSSI) switch
			{
				RSSILevelTypes.Excellent => _fullSignalIcon, 
				RSSILevelTypes.Good => _goodSignalIcon, 
				RSSILevelTypes.Fair => _fairSignalIcon, 
				RSSILevelTypes.Weak => _weakSignalIcon, 
				_ => _noSignalIcon, 
			});
		}

		protected override void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				CancellationTokenSource? refreshCancellationTokenSource = _refreshCancellationTokenSource;
				if (refreshCancellationTokenSource != null)
				{
					refreshCancellationTokenSource.Cancel();
				}
				CancellationTokenSource? refreshCancellationTokenSource2 = _refreshCancellationTokenSource;
				if (refreshCancellationTokenSource2 != null)
				{
					refreshCancellationTokenSource2.Dispose();
				}
				_refreshCancellationTokenSource = null;
				((Screen)this).Lifecycle.RemoveObserver((ILifecycleObserver)(object)this);
				_isDisposed = true;
				((Object)this).Dispose();
			}
		}
	}
	public class Resource : Resource
	{
	}
}
namespace ids.auto.Services
{
	public abstract class BasePlatformMessageService : IPlatformMessageService
	{
		protected readonly IAppAnalytics? AppAnalytics;

		protected BasePlatformMessageService(IAppAnalytics? appAnalytics)
		{
			AppAnalytics = appAnalytics;
		}

		public virtual void SendMessageToScreen(string message)
		{
		}
	}
	public class DeviceDataService : IDeviceDataService
	{
		private readonly ILogicalDeviceManager _logicalDeviceManager;

		public DeviceDataService(ILogicalDeviceManager logicalDeviceManager)
		{
			_logicalDeviceManager = logicalDeviceManager;
		}

		public ILogicalDeviceTirePressureMonitor? GetTirePressureMonitor()
		{
			ILogicalDevice obj = _logicalDeviceManager.FindLogicalDevice((Func<ILogicalDevice, bool>)((ILogicalDevice logicalDevice) => logicalDevice is ILogicalDeviceTirePressureMonitor));
			return (ILogicalDeviceTirePressureMonitor?)(object)((obj is ILogicalDeviceTirePressureMonitor) ? obj : null);
		}

		public global::System.Collections.Generic.IList<TirePressureSensor>? GetTirePressureSensors()
		{
			ILogicalDeviceTirePressureMonitor? tirePressureMonitor = GetTirePressureMonitor();
			if (tirePressureMonitor == null)
			{
				return null;
			}
			return (global::System.Collections.Generic.IList<TirePressureSensor>?)Enumerable.ToList<TirePressureSensor>((global::System.Collections.Generic.IEnumerable<TirePressureSensor>)Enumerable.OrderBy<TirePressureSensor, bool>((global::System.Collections.Generic.IEnumerable<TirePressureSensor>)((ITirePressureMonitor)tirePressureMonitor).TirePressureSensors, (Func<TirePressureSensor, bool>)((TirePressureSensor s) => !s.IsFaulted())));
		}
	}
	public interface IDeviceDataService
	{
		ILogicalDeviceTirePressureMonitor? GetTirePressureMonitor();

		global::System.Collections.Generic.IList<TirePressureSensor>? GetTirePressureSensors();
	}
	public interface IPlatformMessageService
	{
		void SendMessageToScreen(string message);
	}
	internal class PlatformMessageService : BasePlatformMessageService
	{
		private static TirePressureScreen? _carScreen;

		public static void RegisterScreen(TirePressureScreen carScreen)
		{
			_carScreen = carScreen;
		}

		public override void SendMessageToScreen(string message)
		{
			IAppAnalytics? appAnalytics = AppAnalytics;
			if (appAnalytics != null)
			{
				appAnalytics.TrackToastEvent("show.auto.toast", "TirePressureScreen", message);
			}
			_carScreen?.ShowToast(message);
		}

		public PlatformMessageService(IAppAnalytics appAnalytics)
			: base(appAnalytics)
		{
		}
	}
}
namespace ids.auto.Resources
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	public class TPMS
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public static ResourceManager ResourceManager
		{
			get
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				if (object.Equals((object)null, (object)resourceMan))
				{
					resourceMan = new ResourceManager("ids.auto.Resources.TPMS", typeof(TPMS).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		public static string spare => ResourceManager.GetString("spare", resourceCulture);

		public static string tpms_vehicle_class_class_a => ResourceManager.GetString("tpms_vehicle_class_class_a", resourceCulture);

		public static string tpms_vehicle_class_class_c => ResourceManager.GetString("tpms_vehicle_class_class_c", resourceCulture);

		public static string tpms_vehicle_class_custom_vehicle => ResourceManager.GetString("tpms_vehicle_class_custom_vehicle", resourceCulture);

		public static string tpms_vehicle_class_fifth_wheel => ResourceManager.GetString("tpms_vehicle_class_fifth_wheel", resourceCulture);

		public static string tpms_vehicle_class_travel_trailer => ResourceManager.GetString("tpms_vehicle_class_travel_trailer", resourceCulture);

		public static string unknown => ResourceManager.GetString("unknown", resourceCulture);

		public static string tpms_vehicle_class_tow_vehicle => ResourceManager.GetString("tpms_vehicle_class_tow_vehicle", resourceCulture);

		public static string cancel_text => ResourceManager.GetString("cancel_text", resourceCulture);

		public static string no_sensor_data => ResourceManager.GetString("no_sensor_data", resourceCulture);

		public static string rv_tire_pressure => ResourceManager.GetString("rv_tire_pressure", resourceCulture);

		public static string please_wait => ResourceManager.GetString("please_wait", resourceCulture);

		public static string resources_loading => ResourceManager.GetString("resources_loading", resourceCulture);

		public static string low_battery => ResourceManager.GetString("low_battery", resourceCulture);

		public static string signal_lost => ResourceManager.GetString("signal_lost", resourceCulture);

		public static string low_pressure => ResourceManager.GetString("low_pressure", resourceCulture);

		public static string high_pressure => ResourceManager.GetString("high_pressure", resourceCulture);

		public static string high_temperature => ResourceManager.GetString("high_temperature", resourceCulture);

		public static string relative_temperature => ResourceManager.GetString("relative_temperature", resourceCulture);

		public static string left => ResourceManager.GetString("left", resourceCulture);

		public static string right => ResourceManager.GetString("right", resourceCulture);

		public static string inside => ResourceManager.GetString("inside", resourceCulture);

		public static string axle => ResourceManager.GetString("axle", resourceCulture);

		public static string outside => ResourceManager.GetString("outside", resourceCulture);

		public static string dashdash => ResourceManager.GetString("dashdash", resourceCulture);

		public static string not_paired => ResourceManager.GetString("not_paired", resourceCulture);

		public static string no_trailers => ResourceManager.GetString("no_trailers", resourceCulture);

		public static string tirelinc_pro_ext => ResourceManager.GetString("tirelinc_pro_ext", resourceCulture);

		public static string tpms_vehicle_class_truck => ResourceManager.GetString("tpms_vehicle_class_truck", resourceCulture);

		public static string offline => ResourceManager.GetString("offline", resourceCulture);

		public static string last_updated => ResourceManager.GetString("last_updated", resourceCulture);

		internal TPMS()
		{
		}
	}
}
namespace ids.auto.Resources.Colors
{
	public class AutoColors
	{
		public static SKColor ErrorRed => SKColor.Parse("#E50000");

		public static SKColor ErrorOrange => SKColor.Parse("#CC7F13");

		public static SKColor ErrorBlue => SKColor.Parse("#3F95FF");
	}
}
namespace ids.auto.Platforms.Android
{
	public class CarLifecycleObserver : Object, ILifecycleObserver, IJavaObject, global::System.IDisposable, IJavaPeerable
	{
		[OnStop]
		public void OnStop()
		{
			Log.Debug("CarLifecycleObserver", "Session stopped: clearing isAutoRunning");
			AACarAppService.SetAutoRunning(isRunning: false);
		}

		[OnDestroy]
		public void OnDestroy()
		{
			Log.Debug("CarLifecycleObserver", "Session destroyed: clearing isAutoRunning");
			AACarAppService.SetAutoRunning(isRunning: false);
		}
	}
}
namespace ids.auto.Helper
{
	public static class CarIconHelper
	{
		private class SvgWebViewClient : WebViewClient
		{
			private readonly TaskCompletionSource<bool> _completionSource;

			public SvgWebViewClient(TaskCompletionSource<bool> completionSource)
			{
				_completionSource = completionSource;
			}

			public override void OnPageFinished(WebView? view, string? url)
			{
				((WebViewClient)this).OnPageFinished(view, url);
				_completionSource.TrySetResult(true);
			}

			public override void OnReceivedError(WebView? view, ClientError errorCode, string? description, string? failingUrl)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				((WebViewClient)this).OnReceivedError(view, errorCode, description, failingUrl);
				_completionSource.TrySetException(new global::System.Exception("WebView error: " + description));
			}
		}

		public enum ImageType
		{
			Png,
			Svg
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <CreateCarIconAsync>d__4 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<CarIcon> <>t__builder;

			public string assetName;

			public SKColor? color;

			private TaskAwaiter<Bitmap?> <>u__1;

			private void MoveNext()
			{
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_0104: Unknown result type (might be due to invalid IL or missing references)
				//IL_010b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				CarIcon result2;
				try
				{
					_ = 1;
					try
					{
						TaskAwaiter<Bitmap> val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Bitmap>);
							num = (<>1__state = -1);
							goto IL_008f;
						}
						if (num != 1)
						{
							if (assetName.EndsWith(".svg", (StringComparison)5))
							{
								val = MauiAssetLoader.SvgToNativeImageAsync(assetName, color).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Bitmap>, <CreateCarIconAsync>d__4>(ref val, ref this);
									return;
								}
								goto IL_008f;
							}
							val = MauiAssetLoader.ImageToNativeAsync(assetName).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Bitmap>, <CreateCarIconAsync>d__4>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Bitmap>);
							num = (<>1__state = -1);
						}
						Bitmap result = val.GetResult();
						result2 = ((result != null) ? new Builder(IconCompat.CreateWithBitmap(result)).Build() : null);
						goto end_IL_000c;
						IL_008f:
						Bitmap result3 = val.GetResult();
						result2 = ((result3 != null) ? new Builder(IconCompat.CreateWithBitmap(result3)).Build() : null);
						end_IL_000c:;
					}
					catch (global::System.Exception ex)
					{
						LoggerExtensions.LogError(_logger, ex, "Error creating car icon from asset: {ExMessage}", new object[1] { ex.Message });
						result2 = null;
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<>t__builder.SetResult(result2);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <CreateSvgCarIconAsync>d__5 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<CarIcon> <>t__builder;

			public string assetName;

			private Stream <stream>5__2;

			private StreamReader <reader>5__3;

			private TaskAwaiter<Stream> <>u__1;

			private TaskAwaiter<string> <>u__2;

			private TaskAwaiter<Bitmap?> <>u__3;

			private void MoveNext()
			{
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e3: Expected O, but got Unknown
				//IL_012f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0134: Unknown result type (might be due to invalid IL or missing references)
				//IL_013c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0161: Unknown result type (might be due to invalid IL or missing references)
				//IL_0166: Unknown result type (might be due to invalid IL or missing references)
				//IL_0196: Unknown result type (might be due to invalid IL or missing references)
				//IL_019b: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_017b: Unknown result type (might be due to invalid IL or missing references)
				//IL_017d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0114: Unknown result type (might be due to invalid IL or missing references)
				//IL_0116: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				CarIcon result2;
				try
				{
					_ = 2;
					try
					{
						TaskAwaiter<Stream> val;
						if (num != 0)
						{
							if ((uint)(num - 1) <= 1u)
							{
								goto IL_0081;
							}
							val = FileSystem.OpenAppPackageFileAsync(assetName).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Stream>, <CreateSvgCarIconAsync>d__5>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Stream>);
							num = (<>1__state = -1);
						}
						Stream result = val.GetResult();
						<stream>5__2 = result;
						goto IL_0081;
						IL_0081:
						try
						{
							if ((uint)(num - 1) <= 1u)
							{
								goto IL_00e4;
							}
							if (<stream>5__2 != null)
							{
								LoggerExtensions.LogInformation(_logger, "Successfully loaded SVG from app package: {AssetName}", new object[1] { assetName });
								<reader>5__3 = new StreamReader(<stream>5__2);
								goto IL_00e4;
							}
							LoggerExtensions.LogError(_logger, "Failed to load SVG from app package: {AssetName}", new object[1] { assetName });
							result2 = null;
							goto end_IL_0081;
							IL_00e4:
							try
							{
								TaskAwaiter<Bitmap> val2;
								TaskAwaiter<string> val3;
								if (num != 1)
								{
									if (num == 2)
									{
										val2 = <>u__3;
										<>u__3 = default(TaskAwaiter<Bitmap>);
										num = (<>1__state = -1);
										goto IL_01b2;
									}
									val3 = ((TextReader)<reader>5__3).ReadToEndAsync().GetAwaiter();
									if (!val3.IsCompleted)
									{
										num = (<>1__state = 1);
										<>u__2 = val3;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, <CreateSvgCarIconAsync>d__5>(ref val3, ref this);
										return;
									}
								}
								else
								{
									val3 = <>u__2;
									<>u__2 = default(TaskAwaiter<string>);
									num = (<>1__state = -1);
								}
								val2 = RenderSvgToBitmapAsync(val3.GetResult(), 256, 256).GetAwaiter();
								if (!val2.IsCompleted)
								{
									num = (<>1__state = 2);
									<>u__3 = val2;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Bitmap>, <CreateSvgCarIconAsync>d__5>(ref val2, ref this);
									return;
								}
								goto IL_01b2;
								IL_01b2:
								Bitmap result3 = val2.GetResult();
								if (result3 == null)
								{
									LoggerExtensions.LogError(_logger, "Failed to render SVG to bitmap: {AssetName}", new object[1] { assetName });
									result2 = null;
								}
								else
								{
									result2 = new Builder(IconCompat.CreateWithBitmap(result3)).Build();
								}
							}
							finally
							{
								if (num < 0 && <reader>5__3 != null)
								{
									((global::System.IDisposable)<reader>5__3).Dispose();
								}
							}
							end_IL_0081:;
						}
						finally
						{
							if (num < 0 && <stream>5__2 != null)
							{
								((global::System.IDisposable)<stream>5__2).Dispose();
							}
						}
					}
					catch (global::System.Exception ex)
					{
						LoggerExtensions.LogCritical(_logger, ex, "Error creating SVG car icon: {ExMessage}", new object[1] { ex.Message });
						result2 = null;
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<>t__builder.SetResult(result2);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <RenderSvgToBitmapAsync>d__6 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Bitmap> <>t__builder;

			public int width;

			public int height;

			public string svgContent;

			private WebView <webView>5__2;

			private TaskAwaiter<global::System.Threading.Tasks.Task> <>u__1;

			private TaskAwaiter <>u__2;

			private void MoveNext()
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Expected O, but got Unknown
				//IL_0194: Unknown result type (might be due to invalid IL or missing references)
				//IL_0199: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
				//IL_0201: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_015f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0164: Unknown result type (might be due to invalid IL or missing references)
				//IL_022e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0235: Expected O, but got Unknown
				//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01db: Unknown result type (might be due to invalid IL or missing references)
				//IL_0179: Unknown result type (might be due to invalid IL or missing references)
				//IL_017b: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				Bitmap result;
				try
				{
					_ = 1;
					try
					{
						if ((uint)num > 1u)
						{
							Context context = Application.Context;
							<webView>5__2 = new WebView(context);
						}
						try
						{
							TaskAwaiter val;
							TaskAwaiter<global::System.Threading.Tasks.Task> val3;
							if (num != 0)
							{
								if (num == 1)
								{
									val = <>u__2;
									<>u__2 = default(TaskAwaiter);
									num = (<>1__state = -1);
									goto IL_0210;
								}
								<webView>5__2.Settings.JavaScriptEnabled = true;
								((View)<webView>5__2).SetLayerType((LayerType)1, (Paint)null);
								((View)<webView>5__2).SetBackgroundColor(Color.Transparent);
								((View)<webView>5__2).Layout(0, 0, width, height);
								TaskCompletionSource<bool> val2 = new TaskCompletionSource<bool>();
								<webView>5__2.SetWebViewClient((WebViewClient)(object)new SvgWebViewClient(val2));
								string text = $"\n            <html>\n            <head>\n                <meta name='viewport' content='width={width}, height={height}, initial-scale=1.0, user-scalable=no'>\n                <style>\n                    body {{ margin: 0; padding: 0; width: {width}px; height: {height}px; overflow: hidden; background-color: transparent; }}\n                    svg {{ width: 100%; height: 100%; }}\n                </style>\n            </head>\n            <body>\n                {svgContent}\n            </body>\n            </html>";
								<webView>5__2.LoadDataWithBaseURL((string)null, text, "text/html", "UTF-8", (string)null);
								global::System.Threading.Tasks.Task task = global::System.Threading.Tasks.Task.Delay(800);
								val3 = global::System.Threading.Tasks.Task.WhenAny((global::System.Threading.Tasks.Task)val2.Task, task).GetAwaiter();
								if (!val3.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val3;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<global::System.Threading.Tasks.Task>, <RenderSvgToBitmapAsync>d__6>(ref val3, ref this);
									return;
								}
							}
							else
							{
								val3 = <>u__1;
								<>u__1 = default(TaskAwaiter<global::System.Threading.Tasks.Task>);
								num = (<>1__state = -1);
							}
							val3.GetResult();
							val = global::System.Threading.Tasks.Task.Delay(100).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__2 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <RenderSvgToBitmapAsync>d__6>(ref val, ref this);
								return;
							}
							goto IL_0210;
							IL_0210:
							((TaskAwaiter)(ref val)).GetResult();
							Bitmap obj = Bitmap.CreateBitmap(width, height, Config.Argb8888);
							Canvas val4 = new Canvas(obj);
							((View)<webView>5__2).Draw(val4);
							result = obj;
						}
						finally
						{
							if (num < 0 && <webView>5__2 != null)
							{
								((global::System.IDisposable)<webView>5__2).Dispose();
							}
						}
					}
					catch (global::System.Exception ex)
					{
						LoggerExtensions.LogCritical(_logger, ex, "Error rendering SVG to bitmap: {ExMessage}", new object[1] { ex.Message });
						result = null;
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<>t__builder.SetResult(result);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		private static readonly ILogger _logger = (ILogger)(object)ApplicationContext.Instance.GetLogger<TirePressureScreen>();

		public static CarIcon? GetCarIcon(Context context, string imageName, ImageType imageType = ImageType.Png)
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Resources resources = context.Resources;
				int num = ((resources != null) ? resources.GetIdentifier(imageName.ToLower(), "drawable", context.PackageName) : (-1));
				if (num <= 0)
				{
					return null;
				}
				if (imageType == ImageType.Svg)
				{
					try
					{
						Drawable drawable = AppCompatResources.GetDrawable(context, num);
						if (drawable != null)
						{
							Bitmap val = DrawableToBitmap(drawable);
							if (val != null)
							{
								return new Builder(IconCompat.CreateWithBitmap(val)).Build();
							}
						}
					}
					catch (global::System.Exception)
					{
					}
				}
				return new Builder(IconCompat.CreateWithResource(context, num)).Build();
			}
			catch (global::System.Exception ex2)
			{
				LoggerExtensions.LogDebug(_logger, "Error creating car icon: {ExMessage}", new object[1] { ex2.Message });
				return null;
			}
		}

		public static CarIcon? GetLibraryCarIcon(Context context, string imageName, string libraryPackageName, ImageType imageType = ImageType.Png)
		{
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Resources resources = context.Resources;
				int num = ((resources != null) ? resources.GetIdentifier(imageName.ToLower(), "drawable", libraryPackageName) : (-1));
				if (num <= 0)
				{
					LoggerExtensions.LogDebug(_logger, "Library resource not found: {ImageName}", new object[1] { imageName });
					return null;
				}
				if (imageType == ImageType.Svg)
				{
					try
					{
						Drawable drawable = AppCompatResources.GetDrawable(context, num);
						if (drawable != null)
						{
							Bitmap val = DrawableToBitmap(drawable);
							if (val != null)
							{
								return new Builder(IconCompat.CreateWithBitmap(val)).Build();
							}
						}
					}
					catch (global::System.Exception)
					{
					}
				}
				return new Builder(IconCompat.CreateWithResource(context, num)).Build();
			}
			catch (global::System.Exception)
			{
				return null;
			}
		}

		private static Bitmap? DrawableToBitmap(Drawable drawable)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			try
			{
				int num = ((drawable.IntrinsicWidth > 0) ? drawable.IntrinsicWidth : 64);
				int num2 = ((drawable.IntrinsicHeight > 0) ? drawable.IntrinsicHeight : 64);
				Bitmap obj = Bitmap.CreateBitmap(num, num2, Config.Argb8888);
				Canvas val = new Canvas(obj);
				drawable.SetBounds(0, 0, val.Width, val.Height);
				drawable.Draw(val);
				return obj;
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogCritical(_logger, "Error converting drawable to bitmap: {ExMessage}", new object[1] { ex.Message });
				return null;
			}
		}

		[AsyncStateMachine(typeof(<CreateCarIconAsync>d__4))]
		public static async global::System.Threading.Tasks.Task<CarIcon?> CreateCarIconAsync(string assetName, SKColor? color = null)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_ = 1;
			try
			{
				if (assetName.EndsWith(".svg", (StringComparison)5))
				{
					Bitmap val = await MauiAssetLoader.SvgToNativeImageAsync(assetName, color);
					if (val == null)
					{
						return null;
					}
					return new Builder(IconCompat.CreateWithBitmap(val)).Build();
				}
				Bitmap val2 = await MauiAssetLoader.ImageToNativeAsync(assetName);
				if (val2 == null)
				{
					return null;
				}
				return new Builder(IconCompat.CreateWithBitmap(val2)).Build();
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogError(_logger, ex, "Error creating car icon from asset: {ExMessage}", new object[1] { ex.Message });
				return null;
			}
		}

		[AsyncStateMachine(typeof(<CreateSvgCarIconAsync>d__5))]
		public static async global::System.Threading.Tasks.Task<CarIcon?> CreateSvgCarIconAsync(string assetName)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_ = 2;
			try
			{
				Stream stream = await FileSystem.OpenAppPackageFileAsync(assetName);
				try
				{
					if (stream == null)
					{
						LoggerExtensions.LogError(_logger, "Failed to load SVG from app package: {AssetName}", new object[1] { assetName });
						return null;
					}
					LoggerExtensions.LogInformation(_logger, "Successfully loaded SVG from app package: {AssetName}", new object[1] { assetName });
					StreamReader reader = new StreamReader(stream);
					try
					{
						Bitmap val = await RenderSvgToBitmapAsync(await ((TextReader)reader).ReadToEndAsync(), 256, 256);
						if (val == null)
						{
							LoggerExtensions.LogError(_logger, "Failed to render SVG to bitmap: {AssetName}", new object[1] { assetName });
							return null;
						}
						return new Builder(IconCompat.CreateWithBitmap(val)).Build();
					}
					finally
					{
						((global::System.IDisposable)reader)?.Dispose();
					}
				}
				finally
				{
					((global::System.IDisposable)stream)?.Dispose();
				}
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogCritical(_logger, ex, "Error creating SVG car icon: {ExMessage}", new object[1] { ex.Message });
				return null;
			}
		}

		[AsyncStateMachine(typeof(<RenderSvgToBitmapAsync>d__6))]
		private static async global::System.Threading.Tasks.Task<Bitmap?> RenderSvgToBitmapAsync(string svgContent, int width, int height)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_ = 1;
			try
			{
				Context context = Application.Context;
				WebView webView = new WebView(context);
				try
				{
					webView.Settings.JavaScriptEnabled = true;
					((View)webView).SetLayerType((LayerType)1, (Paint)null);
					((View)webView).SetBackgroundColor(Color.Transparent);
					((View)webView).Layout(0, 0, width, height);
					TaskCompletionSource<bool> val = new TaskCompletionSource<bool>();
					webView.SetWebViewClient((WebViewClient)(object)new SvgWebViewClient(val));
					string text = $"\n            <html>\n            <head>\n                <meta name='viewport' content='width={width}, height={height}, initial-scale=1.0, user-scalable=no'>\n                <style>\n                    body {{ margin: 0; padding: 0; width: {width}px; height: {height}px; overflow: hidden; background-color: transparent; }}\n                    svg {{ width: 100%; height: 100%; }}\n                </style>\n            </head>\n            <body>\n                {svgContent}\n            </body>\n            </html>";
					webView.LoadDataWithBaseURL((string)null, text, "text/html", "UTF-8", (string)null);
					global::System.Threading.Tasks.Task task = global::System.Threading.Tasks.Task.Delay(800);
					await global::System.Threading.Tasks.Task.WhenAny((global::System.Threading.Tasks.Task)val.Task, task);
					await global::System.Threading.Tasks.Task.Delay(100);
					Bitmap obj = Bitmap.CreateBitmap(width, height, Config.Argb8888);
					Canvas val2 = new Canvas(obj);
					((View)webView).Draw(val2);
					return obj;
				}
				finally
				{
					((global::System.IDisposable)webView)?.Dispose();
				}
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogCritical(_logger, ex, "Error rendering SVG to bitmap: {ExMessage}", new object[1] { ex.Message });
				return null;
			}
		}
	}
	public class MauiAssetLoader
	{
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			public Stream stream;

			internal Stream <LoadImageAsync>b__0()
			{
				return stream;
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ImageToNativeAsync>d__0 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Bitmap> <>t__builder;

			public string assetName;

			public SKColor? color;

			private TaskAwaiter<Bitmap?> <>u__1;

			private TaskAwaiter<Stream?> <>u__2;

			private void MoveNext()
			{
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00de: Unknown result type (might be due to invalid IL or missing references)
				//IL_009f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				Bitmap result2;
				try
				{
					TaskAwaiter<Bitmap> val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Bitmap>);
						num = (<>1__state = -1);
						goto IL_0087;
					}
					TaskAwaiter<Stream> val2;
					if (num != 1)
					{
						if (assetName.EndsWith(".svg", (StringComparison)5))
						{
							val = SvgToNativeImageAsync(assetName, color).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Bitmap>, <ImageToNativeAsync>d__0>(ref val, ref this);
								return;
							}
							goto IL_0087;
						}
						val2 = LoadAsync(assetName).GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__2 = val2;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Stream>, <ImageToNativeAsync>d__0>(ref val2, ref this);
							return;
						}
					}
					else
					{
						val2 = <>u__2;
						<>u__2 = default(TaskAwaiter<Stream>);
						num = (<>1__state = -1);
					}
					Stream result = val2.GetResult();
					result2 = ((result != null) ? ImageExtensions.AsBitmap(PlatformImage.FromStream(result, (ImageFormat)0)) : null);
					goto end_IL_0007;
					IL_0087:
					result2 = val.GetResult();
					end_IL_0007:;
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<>t__builder.SetResult(result2);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <LoadAsync>d__2 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Stream> <>t__builder;

			public string assetName;

			private TaskAwaiter<Stream> <>u__1;

			private void MoveNext()
			{
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				Stream result;
				try
				{
					try
					{
						TaskAwaiter<Stream> val;
						if (num != 0)
						{
							val = FileSystem.Current.OpenAppPackageFileAsync(assetName).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Stream>, <LoadAsync>d__2>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Stream>);
							num = (<>1__state = -1);
						}
						result = val.GetResult();
					}
					catch (global::System.Exception ex)
					{
						Console.WriteLine((object)ex);
						result = null;
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<>t__builder.SetResult(result);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <LoadImageAsync>d__3 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<ImageSource> <>t__builder;

			public string assetName;

			private <>c__DisplayClass3_0 <>8__1;

			private TaskAwaiter<Stream?> <>u__1;

			private void MoveNext()
			{
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ImageSource result2;
				try
				{
					TaskAwaiter<Stream> val;
					if (num != 0)
					{
						<>8__1 = new <>c__DisplayClass3_0();
						val = LoadAsync(assetName).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Stream>, <LoadImageAsync>d__3>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Stream>);
						num = (<>1__state = -1);
					}
					Stream result = val.GetResult();
					<>8__1.stream = result;
					result2 = ((<>8__1.stream != null) ? ImageSource.FromStream((Func<Stream>)(() => <>8__1.stream)) : ImageSource.FromFile(assetName));
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<>8__1 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<>8__1 = null;
				<>t__builder.SetResult(result2);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <SvgToNativeImageAsync>d__1 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Bitmap> <>t__builder;

			public string assetName;

			public int width;

			public int height;

			public SKColor? color;

			private StreamReader <reader>5__2;

			private TaskAwaiter<Stream?> <>u__1;

			private TaskAwaiter<string> <>u__2;

			private void MoveNext()
			{
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Expected O, but got Unknown
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00df: Unknown result type (might be due to invalid IL or missing references)
				//IL_009d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fd: Expected O, but got Unknown
				//IL_0108: Unknown result type (might be due to invalid IL or missing references)
				//IL_010f: Expected O, but got Unknown
				//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_0137: Unknown result type (might be due to invalid IL or missing references)
				//IL_013c: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_0153: Unknown result type (might be due to invalid IL or missing references)
				//IL_0158: Unknown result type (might be due to invalid IL or missing references)
				//IL_01db: Unknown result type (might be due to invalid IL or missing references)
				//IL_0176: Unknown result type (might be due to invalid IL or missing references)
				//IL_017b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0194: Unknown result type (might be due to invalid IL or missing references)
				//IL_0199: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
				//IL_0210: Expected O, but got Unknown
				//IL_0261: Unknown result type (might be due to invalid IL or missing references)
				//IL_0268: Expected O, but got Unknown
				int num = <>1__state;
				Bitmap result2;
				try
				{
					_ = 1;
					try
					{
						TaskAwaiter<Stream> val;
						if (num != 0)
						{
							if (num == 1)
							{
								goto IL_008e;
							}
							val = LoadAsync(assetName).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Stream>, <SvgToNativeImageAsync>d__1>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Stream>);
							num = (<>1__state = -1);
						}
						Stream result = val.GetResult();
						if (result != null)
						{
							<reader>5__2 = new StreamReader(result);
							goto IL_008e;
						}
						result2 = null;
						goto end_IL_000c;
						IL_008e:
						try
						{
							TaskAwaiter<string> val2;
							if (num != 1)
							{
								val2 = ((TextReader)<reader>5__2).ReadToEndAsync().GetAwaiter();
								if (!val2.IsCompleted)
								{
									num = (<>1__state = 1);
									<>u__2 = val2;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, <SvgToNativeImageAsync>d__1>(ref val2, ref this);
									return;
								}
							}
							else
							{
								val2 = <>u__2;
								<>u__2 = default(TaskAwaiter<string>);
								num = (<>1__state = -1);
							}
							string result3 = val2.GetResult();
							SKSvg val3 = new SKSvg();
							MemoryStream val4 = new MemoryStream(Encoding.UTF8.GetBytes(result3));
							try
							{
								val3.Load((Stream)(object)val4);
								if (val3.Picture == null)
								{
									result2 = null;
								}
								else
								{
									float num2 = 1f;
									SKRect cullRect = val3.Picture.CullRect;
									if (((SKRect)(ref cullRect)).Width > 0f)
									{
										cullRect = val3.Picture.CullRect;
										if (((SKRect)(ref cullRect)).Height > 0f)
										{
											float num3 = width;
											cullRect = val3.Picture.CullRect;
											float num4 = num3 / ((SKRect)(ref cullRect)).Width;
											float num5 = height;
											cullRect = val3.Picture.CullRect;
											num2 = Math.Min(num4, num5 / ((SKRect)(ref cullRect)).Height);
										}
									}
									SKMatrix val5 = SKMatrix.CreateScale(num2, num2);
									SKSurface val6 = SKSurface.Create(new SKImageInfo(width, height));
									try
									{
										SKCanvas canvas = val6.Canvas;
										canvas.Clear(SKColors.Transparent);
										if (color.HasValue)
										{
											SKPaint val7 = new SKPaint
											{
												ColorFilter = SKColorFilter.CreateBlendMode(color.Value, (SKBlendMode)5)
											};
											try
											{
												canvas.DrawPicture(val3.Picture, ref val5, val7);
											}
											finally
											{
												if (num < 0)
												{
													((global::System.IDisposable)val7)?.Dispose();
												}
											}
										}
										else
										{
											canvas.DrawPicture(val3.Picture, ref val5, (SKPaint)null);
										}
										SKImage val8 = val6.Snapshot();
										try
										{
											SKData val9 = val8.Encode((SKEncodedImageFormat)4, 100);
											try
											{
												MemoryStream val10 = new MemoryStream(val9.ToArray());
												try
												{
													Bitmap val11 = BitmapFactory.DecodeStream((Stream)(object)val10);
													result2 = ((val11 != null) ? val11 : null);
												}
												finally
												{
													if (num < 0)
													{
														((global::System.IDisposable)val10)?.Dispose();
													}
												}
											}
											finally
											{
												if (num < 0)
												{
													((global::System.IDisposable)val9)?.Dispose();
												}
											}
										}
										finally
										{
											if (num < 0)
											{
												((global::System.IDisposable)val8)?.Dispose();
											}
										}
									}
									finally
									{
										if (num < 0)
										{
											((global::System.IDisposable)val6)?.Dispose();
										}
									}
								}
							}
							finally
							{
								if (num < 0)
								{
									((global::System.IDisposable)val4)?.Dispose();
								}
							}
						}
						finally
						{
							if (num < 0 && <reader>5__2 != null)
							{
								((global::System.IDisposable)<reader>5__2).Dispose();
							}
						}
						end_IL_000c:;
					}
					catch (global::System.Exception)
					{
						result2 = null;
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<>t__builder.SetResult(result2);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		[AsyncStateMachine(typeof(<ImageToNativeAsync>d__0))]
		public static async global::System.Threading.Tasks.Task<Bitmap?> ImageToNativeAsync(string assetName, SKColor? color = null)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			if (assetName.EndsWith(".svg", (StringComparison)5))
			{
				return await SvgToNativeImageAsync(assetName, color);
			}
			Stream val = await LoadAsync(assetName);
			if (val == null)
			{
				return null;
			}
			return ImageExtensions.AsBitmap(PlatformImage.FromStream(val, (ImageFormat)0));
		}

		[AsyncStateMachine(typeof(<SvgToNativeImageAsync>d__1))]
		public static async global::System.Threading.Tasks.Task<Bitmap?> SvgToNativeImageAsync(string assetName, SKColor? color = null, int width = 44, int height = 44)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_ = 1;
			try
			{
				Stream val = await LoadAsync(assetName);
				if (val == null)
				{
					return null;
				}
				StreamReader reader = new StreamReader(val);
				try
				{
					string text = await ((TextReader)reader).ReadToEndAsync();
					SKSvg val2 = new SKSvg();
					MemoryStream val3 = new MemoryStream(Encoding.UTF8.GetBytes(text));
					try
					{
						val2.Load((Stream)(object)val3);
						if (val2.Picture == null)
						{
							return null;
						}
						float num = 1f;
						SKRect cullRect = val2.Picture.CullRect;
						if (((SKRect)(ref cullRect)).Width > 0f)
						{
							cullRect = val2.Picture.CullRect;
							if (((SKRect)(ref cullRect)).Height > 0f)
							{
								float num2 = width;
								cullRect = val2.Picture.CullRect;
								float num3 = num2 / ((SKRect)(ref cullRect)).Width;
								float num4 = height;
								cullRect = val2.Picture.CullRect;
								num = Math.Min(num3, num4 / ((SKRect)(ref cullRect)).Height);
							}
						}
						SKMatrix val4 = SKMatrix.CreateScale(num, num);
						SKSurface val5 = SKSurface.Create(new SKImageInfo(width, height));
						try
						{
							SKCanvas canvas = val5.Canvas;
							canvas.Clear(SKColors.Transparent);
							if (color.HasValue)
							{
								SKPaint val6 = new SKPaint
								{
									ColorFilter = SKColorFilter.CreateBlendMode(color.Value, (SKBlendMode)5)
								};
								try
								{
									canvas.DrawPicture(val2.Picture, ref val4, val6);
								}
								finally
								{
									((global::System.IDisposable)val6)?.Dispose();
								}
							}
							else
							{
								canvas.DrawPicture(val2.Picture, ref val4, (SKPaint)null);
							}
							SKImage val7 = val5.Snapshot();
							try
							{
								SKData val8 = val7.Encode((SKEncodedImageFormat)4, 100);
								try
								{
									MemoryStream val9 = new MemoryStream(val8.ToArray());
									try
									{
										Bitmap val10 = BitmapFactory.DecodeStream((Stream)(object)val9);
										if (val10 == null)
										{
											return null;
										}
										return val10;
									}
									finally
									{
										((global::System.IDisposable)val9)?.Dispose();
									}
								}
								finally
								{
									((global::System.IDisposable)val8)?.Dispose();
								}
							}
							finally
							{
								((global::System.IDisposable)val7)?.Dispose();
							}
						}
						finally
						{
							((global::System.IDisposable)val5)?.Dispose();
						}
					}
					finally
					{
						((global::System.IDisposable)val3)?.Dispose();
					}
				}
				finally
				{
					((global::System.IDisposable)reader)?.Dispose();
				}
			}
			catch (global::System.Exception)
			{
				return null;
			}
		}

		[AsyncStateMachine(typeof(<LoadAsync>d__2))]
		private static async global::System.Threading.Tasks.Task<Stream?> LoadAsync(string assetName)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				return await FileSystem.Current.OpenAppPackageFileAsync(assetName);
			}
			catch (global::System.Exception ex)
			{
				Console.WriteLine((object)ex);
				return null;
			}
		}

		[AsyncStateMachine(typeof(<LoadImageAsync>d__3))]
		public async global::System.Threading.Tasks.Task<ImageSource?> LoadImageAsync(string assetName)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			Stream stream = await LoadAsync(assetName);
			return (stream != null) ? ImageSource.FromStream((Func<Stream>)(() => stream)) : ImageSource.FromFile(assetName);
		}
	}
	public static class VehicleClassHelper
	{
		public static string ToFriendlyString(this VehicleClass vehicleClass)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected I4, but got Unknown
			return (vehicleClass - 2) switch
			{
				0 => TPMS.tpms_vehicle_class_class_a, 
				1 => TPMS.tpms_vehicle_class_class_c, 
				2 => TPMS.tpms_vehicle_class_fifth_wheel, 
				3 => TPMS.tpms_vehicle_class_travel_trailer, 
				5 => TPMS.tpms_vehicle_class_custom_vehicle, 
				4 => TPMS.tpms_vehicle_class_truck, 
				_ => string.Empty, 
			};
		}
	}
}
namespace ids.auto.Extensions
{
	public static class TirePressureSensorExtensions
	{
		public static bool IsFaulted(this TirePressureSensor sensor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Invalid comparison between Unknown and I4
			if ((int)sensor.BatteryFault == 0 && (int)sensor.PressureFault == 0)
			{
				return (int)sensor.TemperatureFault != 0;
			}
			return true;
		}

		public static string SensorPositionName(this TirePressureSensor sensor)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			return $"Axle {sensor.AxleNumber + 1} {global::System.Enum.GetName(((object)sensor.TirePositionName).GetType(), (object)sensor.TirePositionName)}";
		}
	}
}
namespace ids.auto.Devices.TireLinc
{
	public enum RSSILevelTypes
	{
		Excellent = -50,
		Good = -60,
		Fair = -70,
		Weak = -80,
		Poor = -90,
		NoSignal = -100
	}
}
