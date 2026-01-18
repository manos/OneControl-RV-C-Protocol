using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using IDS.Core.IDS_CAN;
using IDS.Portable.Common;
using IDS.Portable.LogicalDevice;
using IDS.Portable.LogicalDevice.Json;
using IDS.Portable.LogicalDevice.LogicalDevice;
using IDS.Portable.Router.Data;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using OneControl.Devices;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName = ".NET Standard 2.1")]
[assembly: AssemblyCompany("IDS.Portable.Devices.Camera")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("5.5.0.0")]
[assembly: AssemblyInformationalVersion("5.5.0+dc7ee417da01a7c2f9df76bb77d83f69796f89b9")]
[assembly: AssemblyProduct("IDS.Portable.Devices.Camera")]
[assembly: AssemblyTitle("IDS.Portable.Devices.Camera")]
[assembly: AssemblyVersion("5.5.0.0")]
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
namespace IDS.Portable.Devices.Camera
{
	public sealed class CameraPasswordTag : JsonSerializable<CameraPasswordTag>, ILogicalDeviceSnapshotTag, ILogicalDeviceTag, IEquatable<ILogicalDeviceTag>, IJsonSerializerClass
	{
		[JsonProperty]
		public string SerializerClass => ((MemberInfo)((Object)this).GetType()).Name;

		[JsonProperty]
		[field: CompilerGenerated]
		public string Password
		{
			[CompilerGenerated]
			get;
		}

		public bool Equals(ILogicalDeviceTag other)
		{
			if (other is CameraPasswordTag cameraPasswordTag)
			{
				return String.Equals(Password, cameraPasswordTag.Password, (StringComparison)4);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is CameraPasswordTag cameraPasswordTag)
			{
				return String.Equals(Password, cameraPasswordTag.Password, (StringComparison)4);
			}
			return false;
		}

		public override int GetHashCode()
		{
			string password = Password;
			if (password == null)
			{
				return 0;
			}
			return ((Object)password).GetHashCode();
		}

		[JsonConstructor]
		public CameraPasswordTag(string password)
		{
			Password = password ?? String.Empty;
		}

		static CameraPasswordTag()
		{
			Type declaringType = ((MemberInfo)MethodBase.GetCurrentMethod()).DeclaringType;
			TypeRegistry.Register(((MemberInfo)declaringType).Name, declaringType);
		}

		public override string ToString()
		{
			return Password;
		}
	}
	public interface ICamera : IDevicesCommon, INotifyPropertyChanged
	{
		string Ssid { get; }

		string Password { get; }
	}
	public interface ILogicalDeviceCamera : ILogicalDevice, IComparable, IEquatable<ILogicalDevice>, IComparable<ILogicalDevice>, ICommonDisposable, IDisposable, IDevicesCommon, INotifyPropertyChanged, ICamera
	{
	}
	public class LogicalDeviceCamera : LogicalDevice<ILogicalDeviceCapability>, ILogicalDeviceAccessory, ILogicalDevice, IComparable, IEquatable<ILogicalDevice>, IComparable<ILogicalDevice>, ICommonDisposable, IDisposable, IDevicesCommon, INotifyPropertyChanged, IAccessoryDevice, ILogicalDeviceCamera, ICamera
	{
		private const string Tag = "LogicalDeviceCamera";

		public bool AllowAutoOfflineLogicalDeviceRemoval => false;

		public override Version ProtocolVersion => LogicalDeviceConstant.VersionUnknown;

		public override bool IsLegacyDeviceHazardous => false;

		public bool IsAccessoryGatewaySupported => false;

		public override LogicalDeviceActiveConnection ActiveConnection => (LogicalDeviceActiveConnection)1;

		public bool UserIsAuthenticated => true;

		public string DefaultGatewayAddress
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotImplementedException();
			}
		}

		public bool IsOnline
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotImplementedException();
			}
		}

		public string ConnectionNameFriendly => "Camera";

		private ILogicalDeviceTagManager? TagManager
		{
			get
			{
				ILogicalDeviceService deviceService = ((LogicalDevice<ILogicalDeviceCapability>)(object)this).DeviceService;
				if (deviceService == null)
				{
					return null;
				}
				ILogicalDeviceManager deviceManager = deviceService.DeviceManager;
				if (deviceManager == null)
				{
					return null;
				}
				return (ILogicalDeviceTagManager?)(object)deviceManager.TagManager;
			}
		}

		public string Ssid
		{
			get
			{
				LogicalDeviceTagSourceWifiWithPassword? ssidAndPasswordFromTags = GetSsidAndPasswordFromTags();
				return ((ssidAndPasswordFromTags != null) ? ((LogicalDeviceTagSourceWifi)ssidAndPasswordFromTags).Ssid : null) ?? String.Empty;
			}
		}

		public string Password
		{
			get
			{
				LogicalDeviceTagSourceWifiWithPassword? ssidAndPasswordFromTags = GetSsidAndPasswordFromTags();
				return ((ssidAndPasswordFromTags != null) ? ssidAndPasswordFromTags.Password : null) ?? String.Empty;
			}
		}

		public LogicalDeviceCamera(LogicalDeviceCameraId logicalDeviceId, ILogicalDeviceService service)
			: base((ILogicalDeviceId)(object)logicalDeviceId, (ILogicalDeviceCapability)new LogicalDeviceCapability(), service, false)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown


		public override Task<string> GetSoftwarePartNumberAsync(CancellationToken cancelToken)
		{
			return Task.FromResult<string>("Unknown");
		}

		public LogicalDeviceTagSourceWifiWithPassword? GetSsidAndPasswordFromTags()
		{
			if (TagManager == null)
			{
				TaggedLog.Warning("LogicalDeviceCamera", "Could not get wifi tags (no tag manager)", Array.Empty<object>());
				return null;
			}
			return Enumerable.FirstOrDefault<LogicalDeviceTagSourceWifiWithPassword>(TagManager.GetTags<LogicalDeviceTagSourceWifiWithPassword>((ILogicalDevice)(object)this));
		}

		public Task<WifiInfo> GetWifiInfo()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			return Task.FromResult<WifiInfo>(new WifiInfo
			{
				Ssid = Ssid,
				Password = Password
			});
		}
	}
	public class LogicalDeviceCameraFactory : DefaultLogicalDeviceFactory
	{
		public override ILogicalDevice MakeLogicalDevice(ILogicalDeviceService service, ILogicalDeviceId logicalDeviceId, Nullable<byte> rawCapability)
		{
			if (!logicalDeviceId.ProductId.IsValid || !logicalDeviceId.DeviceType.IsValid)
			{
				return null;
			}
			if (!(logicalDeviceId is LogicalDeviceCameraId logicalDeviceId2))
			{
				return null;
			}
			return (ILogicalDevice)(object)new LogicalDeviceCamera(logicalDeviceId2, service);
		}
	}
	public enum LogicalDeviceCameraPartNumberKind : Enum
	{
		Oem,
		Aftermarket
	}
	public static class LogicalDeviceCameraPartNumberKindExtension : Object
	{
		public static PRODUCT_ID ToProductId(this LogicalDeviceCameraPartNumberKind cameraPartNumberKind)
		{
			return (PRODUCT_ID)(cameraPartNumberKind switch
			{
				LogicalDeviceCameraPartNumberKind.Oem => PRODUCT_ID.CAMERA_REAR_OBSERVATION_OEM_ASSEMBLY, 
				LogicalDeviceCameraPartNumberKind.Aftermarket => PRODUCT_ID.CAMERA_REAR_OBSERVATION_AFTERMARKET_ASSEMBLY, 
				_ => PRODUCT_ID.UNKNOWN, 
			});
		}
	}
	public class LogicalDeviceCameraId : LogicalDeviceId
	{
		[JsonProperty]
		public override string SerializerClass => ((MemberInfo)((Object)this).GetType()).Name;

		public LogicalDeviceCameraId(FUNCTION_NAME functionName, LogicalDeviceCameraPartNumberKind cameraPartNumberType, MAC macAddress)
			: base(DEVICE_TYPE.op_Implicit((byte)45), 0, functionName, 0, cameraPartNumberType.ToProductId(), macAddress)
		{
		}

		[JsonConstructor]
		protected LogicalDeviceCameraId(DEVICE_TYPE deviceType, int deviceInstance, FUNCTION_NAME functionName, int functionInstance, PRODUCT_ID productId, MAC productMacAddress)
			: base(deviceType, deviceInstance, functionName, functionInstance, productId, productMacAddress)
		{
		}

		public override ILogicalDeviceId Clone()
		{
			return (ILogicalDeviceId)(object)new LogicalDeviceCameraId(((LogicalDeviceId)this).DeviceType, ((LogicalDeviceId)this).DeviceInstance, ((LogicalDeviceId)this).FunctionName, ((LogicalDeviceId)this).FunctionInstance, ((LogicalDeviceId)this).ProductId, ((LogicalDeviceId)this).ProductMacAddress);
		}

		static LogicalDeviceCameraId()
		{
			Type declaringType = ((MemberInfo)MethodBase.GetCurrentMethod()).DeclaringType;
			if (!(declaringType == (Type)null))
			{
				TypeRegistry.Register(((MemberInfo)declaringType).Name, declaringType);
			}
		}

		public override bool Equals(object obj)
		{
			if (!(obj is LogicalDeviceCameraId))
			{
				return false;
			}
			if (!((LogicalDeviceId)this).Equals(obj))
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return ((LogicalDeviceId)this).GetHashCode();
		}
	}
	public class LogicalDeviceCameraSim : LogicalDeviceCamera, ILogicalDeviceSimulated, ILogicalDevice, IComparable, IEquatable<ILogicalDevice>, IComparable<ILogicalDevice>, ICommonDisposable, IDisposable, IDevicesCommon, INotifyPropertyChanged
	{
		private const string Tag = "LogicalDeviceCameraSim";

		public LogicalDeviceCameraSim(LogicalDeviceCameraId logicalDeviceId, ILogicalDeviceService service)
			: base(logicalDeviceId, service)
		{
		}
	}
	public static class LogicalDeviceServiceExtension : Object
	{
		public static void RegisterCameraLogicalDeviceFactories(this ILogicalDeviceService deviceService)
		{
			deviceService.RegisterLogicalDeviceFactory((ILogicalDeviceFactory)(object)new LogicalDeviceCameraFactory());
			JsonSerializer.AutoRegisterJsonSerializersFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
	public class LogicalDeviceSourceCamera : Object, ILogicalDeviceSource
	{
		private const string LogTag = "LogicalDeviceSourceCamera";

		private readonly List<ILogicalDeviceTag> _deviceSourceTags;

		public IEnumerable<ILogicalDeviceTag> DeviceSourceTags => (IEnumerable<ILogicalDeviceTag>)(object)_deviceSourceTags;

		[field: CompilerGenerated]
		public string DeviceSourceToken
		{
			[CompilerGenerated]
			get;
		}

		public bool IsDeviceSourceActive => true;

		public bool AllowAutoOfflineLogicalDeviceRemoval => false;

		public IEnumerable<ILogicalDeviceTag> MakeDeviceSourceTags(ILogicalDevice? logicalDevice)
		{
			return DeviceSourceTags;
		}

		public LogicalDeviceSourceCamera(string deviceToken, IEnumerable<ILogicalDeviceTag> deviceSourceTags)
		{
			DeviceSourceToken = deviceToken;
			_deviceSourceTags = new List<ILogicalDeviceTag>(deviceSourceTags);
		}
	}
}
