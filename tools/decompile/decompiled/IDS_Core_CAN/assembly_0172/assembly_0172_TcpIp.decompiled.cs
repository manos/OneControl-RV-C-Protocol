using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using IDS.Core.IDS_CAN;
using IDS.Core.Types;
using IDS.Portable.Common;
using IDS.Portable.Common.Color;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using IDS.Portable.LogicalDevice.Json;
using Lumberjack.Components.Abstractions;
using Lumberjack.Serialization.Decoration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OneControl.Devices;
using OneControl.Devices.AccessoryGateway;
using OneControl.Devices.AwningSensor;
using OneControl.Devices.BatteryMonitor;
using OneControl.Devices.BrakingSystem;
using OneControl.Devices.ChassisInfo;
using OneControl.Devices.DoorLock;
using OneControl.Devices.Hvac;
using OneControl.Devices.Leveler.Type3;
using OneControl.Devices.Leveler.Type4;
using OneControl.Devices.Leveler.Type5;
using OneControl.Devices.LightRgb;
using OneControl.Devices.RelayHBridge.Type2;
using OneControl.Devices.TPMS;
using OneControl.Devices.TankSensor;
using OneControl.Devices.TemperatureSensor;
using OneControl.Direct.RvCloudIoT.Component.Serialization.Decoration;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.PidRead;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.RelayBasic;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: TargetFramework(".NETCoreApp,Version=v8.0", FrameworkDisplayName = ".NET 8.0")]
[assembly: AssemblyCompany("Tod Cunningham")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("2022 Lippert Components")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("0.1.0+276.Branch.master.Sha.5398a039984c067bfdaa2155542b1327e982df33.5398a039984c067bfdaa2155542b1327e982df33")]
[assembly: AssemblyProduct("OneControl.Direct.RvCloudIot.Component")]
[assembly: AssemblyTitle("OneControl.Direct.RvCloudIot.Component")]
[assembly: AssemblyVersion("1.0.0.0")]
[module: RefSafetyRules(11)]
namespace OneControl.Direct.RvCloudIoT.Component
{
	public static class LogicalDeviceServiceExtension
	{
		public static void RegisterRvCloudIotServices(this ILogicalDeviceService deviceService)
		{
			JsonSerializer.AutoRegisterJsonSerializersFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
	public class RvCloudIoTComponentException : global::System.Exception
	{
		public RvCloudIoTComponentException(string message, global::System.Exception? innerException = null)
			: base(message, innerException)
		{
		}
	}
	public class RvCloudIoTComponentCommandException : RvCloudIoTComponentException
	{
		public RvCloudIoTComponentCommandException(string message, global::System.Exception? innerException = null)
			: base(message, innerException)
		{
		}
	}
	public class RvCloudIoTComponentCommandInvalidException : RvCloudIoTComponentException
	{
		public RvCloudIoTComponentCommandInvalidException(string message, global::System.Exception? innerException = null)
			: base(message, innerException)
		{
		}
	}
	public class RvCloudIoTComponentCommandNotSupportedException : RvCloudIoTComponentException
	{
		public RvCloudIoTComponentCommandNotSupportedException(string message, global::System.Exception? innerException = null)
			: base(message, innerException)
		{
		}
	}
	public class RvCloudIoTComponentCommandResultException : RvCloudIoTComponentCommandException
	{
		public RvCloudIoTComponentCommandResultException(string message, global::System.Exception? innerException = null)
			: base(message, innerException)
		{
		}
	}
	public class RvCloudIoTComponentCommandResultResponseNullException : RvCloudIoTComponentCommandResultException
	{
		public RvCloudIoTComponentCommandResultResponseNullException(string message, global::System.Exception? innerException = null)
			: base(message, innerException)
		{
		}

		public RvCloudIoTComponentCommandResultResponseNullException(global::System.Exception? innerException = null)
			: this("Command returned null", innerException)
		{
		}
	}
	public class RvCloudIoTComponentCommandResultResponseFailedException : RvCloudIoTComponentCommandResultException
	{
		public RvCloudIoTComponentCommandResultResponseFailedException(string message, global::System.Exception? innerException = null)
			: base(message, innerException)
		{
		}

		public RvCloudIoTComponentCommandResultResponseFailedException(global::System.Exception? innerException = null)
			: this("Command send failed", innerException)
		{
		}
	}
	public class RvCloudIoTComponentCommandResultResponseInvalidException : RvCloudIoTComponentCommandResultException
	{
		public RvCloudIoTComponentCommandResultResponseInvalidException(string message, global::System.Exception? innerException = null)
			: base(message, innerException)
		{
		}

		public RvCloudIoTComponentCommandResultResponseInvalidException(global::System.Exception? innerException = null)
			: this("Command returned invalid/unknown response", innerException)
		{
		}
	}
}
namespace OneControl.Direct.RvCloudIoT.Component.Serialization.Decoration
{
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	public class ComponentSerializationSchemaAttribute : global::System.Attribute, ISerializationSchemaAttribute
	{
		[field: CompilerGenerated]
		protected virtual string SchemaNamePrefix
		{
			[CompilerGenerated]
			get;
		} = "Component.";

		[field: CompilerGenerated]
		public string SchemaName
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public DeviceType? DeviceType
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public string Name
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public Version Version
		{
			[CompilerGenerated]
			get;
		}

		public ComponentSerializationSchemaAttribute(DeviceType deviceType, string name, byte majorVersion, byte minorVersion = 0)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			SchemaName = MakeSchemaName(deviceType, name, majorVersion, minorVersion);
			DeviceType = deviceType;
			Name = name;
			Version = new Version((int)majorVersion, (int)minorVersion);
		}

		public ComponentSerializationSchemaAttribute(string name, byte majorVersion, byte minorVersion = 0)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			SchemaName = MakeSchemaName(name, majorVersion, minorVersion);
			DeviceType = null;
			Name = name;
			Version = new Version((int)majorVersion, (int)minorVersion);
		}

		private string MakeSchemaName(DeviceType deviceType, string commandName, byte majorVersion, byte? minorVersion)
		{
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Expected I4, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected I4, but got Unknown
			if (!(minorVersion > 0))
			{
				return $"{SchemaNamePrefix}{commandName}.{(int)deviceType:X2}.{majorVersion}";
			}
			return $"{SchemaNamePrefix}{commandName}.{(int)deviceType:X2}.{majorVersion}.{minorVersion}";
		}

		private string MakeSchemaName(string commandName, byte majorVersion, byte? minorVersion)
		{
			if (!(minorVersion > 0))
			{
				return $"{SchemaNamePrefix}{commandName}.{majorVersion}";
			}
			return $"{SchemaNamePrefix}{commandName}.{majorVersion}.{minorVersion}";
		}
	}
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	public class ComponentSerializationSchemaCommandAttribute : ComponentSerializationSchemaAttribute, ISerializationSchemaAttribute
	{
		[field: CompilerGenerated]
		protected override string SchemaNamePrefix
		{
			[CompilerGenerated]
			get;
		} = "Component.Command.";

		public ComponentSerializationSchemaCommandAttribute(DeviceType deviceType, string name, byte majorVersion, byte minorVersion = 0)
			: base(deviceType, name, majorVersion, minorVersion)
		{
		}//IL_000c: Unknown result type (might be due to invalid IL or missing references)


		public ComponentSerializationSchemaCommandAttribute(string name, byte majorVersion, byte minorVersion = 0)
			: base(name, majorVersion, minorVersion)
		{
		}
	}
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	public class ComponentSerializationSchemaStatusAttribute : global::System.Attribute, ISerializationSchemaAttribute
	{
		public const int SchemaNameMaxSize = 10;

		[field: CompilerGenerated]
		public string SchemaName
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public DeviceType DeviceType
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public Version Version
		{
			[CompilerGenerated]
			get;
		}

		public ComponentSerializationSchemaStatusAttribute(DeviceType deviceType, byte majorVersion)
			: this(deviceType, majorVersion, 0)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


		public ComponentSerializationSchemaStatusAttribute(DeviceType deviceType, byte majorVersion, byte minorVersion)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			string text = MakeSchemaName(deviceType, majorVersion, minorVersion);
			if (text.Length > 10)
			{
				throw new ArgumentOutOfRangeException("name", $"Given name `{text}` too long, max size is {10}");
			}
			SchemaName = text;
			DeviceType = deviceType;
			Version = new Version((int)majorVersion, (int)minorVersion);
		}

		private static string MakeSchemaName(DeviceType deviceType, byte majorVersion, byte? minorVersion)
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected I4, but got Unknown
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected I4, but got Unknown
			if (!(minorVersion > 0))
			{
				return $"{(int)deviceType:X2}.{majorVersion}";
			}
			return $"{(int)deviceType:X2}.{majorVersion}.{minorVersion}";
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component
{
	public static class ComponentIdEx
	{
		public const int RequiredComponentIdSize = 31;

		public const int DeviceTypeStartIndex = 0;

		public const int DeviceTypeLength = 2;

		public const int DeviceInstanceStartIndex = 3;

		public const int DeviceInstanceLength = 2;

		public const int MacStartIndex = 6;

		public const int MacLength = 12;

		public const int ProductIdIndex = 19;

		public const int ProductIdLength = 4;

		public const int FunctionNameIndex = 24;

		public const int FunctionNameLength = 4;

		public const int FunctionInstanceIndex = 29;

		public const int FunctionInstanceLength = 2;

		public static string MakeComponentId(DEVICE_TYPE deviceType, int deviceInstance, MAC? mac, PRODUCT_ID productId, FUNCTION_NAME functionName, int functionInstance)
		{
			return $"{deviceType.Value:X2}-{deviceInstance:X2}-{LogicalDeviceIdFormatExtension.MacAsHexString(mac)}-{productId.Value:X4}-{functionName.Value:X4}-{functionInstance:X2}";
		}

		public static string MakeComponentId(ILogicalDeviceId logicalDeviceId)
		{
			return MakeComponentId(logicalDeviceId.DeviceType, logicalDeviceId.DeviceInstance, logicalDeviceId.ProductMacAddress, logicalDeviceId.ProductId, logicalDeviceId.FunctionName, logicalDeviceId.FunctionInstance);
		}

		public static DEVICE_TYPE DeviceType(string componentId)
		{
			return DEVICE_TYPE.op_Implicit((byte)int.Parse(componentId.Substring(0, 2), (NumberStyles)515));
		}

		public static int DeviceInstance(string componentId)
		{
			return int.Parse(componentId.Substring(3, 2), (NumberStyles)515);
		}

		public static MAC ProductMacAddress(string componentId)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			return new MAC(UInt48.Parse(componentId.Substring(6, 12), (NumberStyles)515));
		}

		public static PRODUCT_ID ProductId(string componentId)
		{
			return PRODUCT_ID.op_Implicit((ushort)int.Parse(componentId.Substring(19, 4), (NumberStyles)515));
		}

		public static FUNCTION_NAME FunctionName(string componentId)
		{
			return FUNCTION_NAME.op_Implicit((ushort)int.Parse(componentId.Substring(24, 4), (NumberStyles)515));
		}

		public static int FunctionInstance(string componentId)
		{
			return int.Parse(componentId.Substring(29, 2), (NumberStyles)515);
		}

		public static ILogicalDeviceId MakeLogicalDeviceId(string componentId)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			return (ILogicalDeviceId)new LogicalDeviceId(DeviceType(componentId), DeviceInstance(componentId), FunctionName(componentId), FunctionInstance(componentId), ProductId(componentId), ProductMacAddress(componentId));
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentStatus
{
	internal interface IDeviceComponentStatus : IDeviceComponent
	{
		Version? ProtocolVersion { get; }
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public abstract class RvCloudDeviceComponentStatus : IDeviceComponentStatus, IDeviceComponent, IEquatable<RvCloudDeviceComponentStatus>
	{
		private const string LogTag = "RvCloudDeviceComponentStatus";

		[JsonProperty(PropertyName = "cid")]
		[field: CompilerGenerated]
		public string ComponentId
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty(PropertyName = "rcap")]
		[JsonConverter(typeof(JsonStringConverter<byte>))]
		[field: CompilerGenerated]
		public byte RawCapability
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty(PropertyName = "rs")]
		[JsonConverter(typeof(ByteArrayJsonHexStringConverter))]
		[field: CompilerGenerated]
		public byte[] RawStatus
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty(/*Could not decode attribute arguments.*/)]
		[JsonConverter(typeof(JsonStringConverter<byte>))]
		[field: CompilerGenerated]
		public byte? RawStatusEnhanced
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty(/*Could not decode attribute arguments.*/)]
		[JsonConverter(typeof(ByteDictionaryJsonHexStringConverter))]
		[field: CompilerGenerated]
		public Dictionary<byte, byte[]>? RawStatusExtended
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty(/*Could not decode attribute arguments.*/)]
		[JsonConverter(typeof(VersionConverter))]
		[field: CompilerGenerated]
		public Version? ProtocolVersion
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty(/*Could not decode attribute arguments.*/)]
		[JsonConverter(typeof(StringEnumConverter))]
		[field: CompilerGenerated]
		public LogicalDeviceActiveConnection ActiveConnection
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		public DEVICE_TYPE DeviceType => ComponentIdEx.DeviceType(ComponentId);

		[JsonIgnore]
		public int DeviceInstance => ComponentIdEx.DeviceInstance(ComponentId);

		[JsonIgnore]
		public MAC ProductMacAddress => ComponentIdEx.ProductMacAddress(ComponentId);

		[JsonIgnore]
		public PRODUCT_ID ProductId => ComponentIdEx.ProductId(ComponentId);

		[JsonIgnore]
		public FUNCTION_NAME FunctionName => ComponentIdEx.FunctionName(ComponentId);

		[JsonIgnore]
		public int FunctionInstance => ComponentIdEx.FunctionInstance(ComponentId);

		[JsonConstructor]
		protected RvCloudDeviceComponentStatus(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, byte? rawStatusEnhanced, Dictionary<byte, byte[]>? rawStatusExtended, LogicalDeviceActiveConnection activeConnection)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(componentId) || componentId.Length != 31)
			{
				throw new ArgumentException($"Invalid componentId of `{componentId}`, must be {31} characters", "componentId");
			}
			ComponentId = componentId;
			ProtocolVersion = protocolVersion;
			RawCapability = rawCapability;
			RawStatus = rawStatus;
			RawStatusEnhanced = rawStatusEnhanced;
			RawStatusExtended = rawStatusExtended;
			ActiveConnection = activeConnection;
		}

		protected RvCloudDeviceComponentStatus(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: this(componentId, ((ILogicalDevice)logicalDevice).ProtocolVersion, ((ILogicalDevice)logicalDevice).DeviceCapabilityBasic.GetRawValue(), ((IDeviceDataPacket)logicalDevice.RawDeviceStatus).CopyCurrentData(), GetRawStatusEnhanced(logicalDevice), GetRawStatusExtended(logicalDevice), ((IDevicesCommon)logicalDevice).ActiveConnection)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			ActiveConnection = ((IDevicesCommon)logicalDevice).ActiveConnection;
		}

		private static byte? GetRawStatusEnhanced(ILogicalDeviceWithStatus logicalDevice)
		{
			IDeviceDataPacketMutable rawDeviceStatus = logicalDevice.RawDeviceStatus;
			IDeviceDataPacketMutable obj = ((rawDeviceStatus is IDeviceDataPacketMutableExtended) ? rawDeviceStatus : null);
			if (obj == null)
			{
				return null;
			}
			return ((IDeviceDataPacketMutableExtended)obj).ExtendedByte;
		}

		private static Dictionary<byte, byte[]>? GetRawStatusExtended(ILogicalDeviceWithStatus logicalDevice)
		{
			ILogicalDeviceWithStatusExtended val = (ILogicalDeviceWithStatusExtended)(object)((logicalDevice is ILogicalDeviceWithStatusExtended) ? logicalDevice : null);
			if (val == null)
			{
				return null;
			}
			return val.CopyRawDeviceStatusExtendedAsDictionary();
		}

		public abstract HashSet<LogicalDeviceCapabilitySerializable> MakeActiveCapabilitiesSerializable();

		public virtual bool Equals(RvCloudDeviceComponentStatus? other)
		{
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			if (ComponentId == other.ComponentId && RawCapability == other.RawCapability && RawStatusEnhanced == other.RawStatusEnhanced)
			{
				return Enumerable.SequenceEqual<byte>((global::System.Collections.Generic.IEnumerable<byte>)RawStatus, (global::System.Collections.Generic.IEnumerable<byte>)other.RawStatus);
			}
			return false;
		}

		public override bool Equals(object otherObj)
		{
			if (otherObj == null)
			{
				return false;
			}
			if (this == otherObj)
			{
				return true;
			}
			if (!(otherObj is RvCloudDeviceComponentStatus other))
			{
				return false;
			}
			return Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Hash(HashCode.Hash<byte>(HashCode.Hash<string>(17, ComponentId), RawCapability), RawStatus);
		}

		public virtual ILogicalDeviceId MakeLogicalDeviceId()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			return (ILogicalDeviceId)new LogicalDeviceId(DeviceType, DeviceInstance, FunctionName, FunctionInstance, ProductId, ProductMacAddress);
		}

		public virtual ILogicalDeviceWithStatus? GetLogicalDevice(ILogicalDeviceService deviceService, ILogicalDeviceSource deviceSource)
		{
			ILogicalDeviceId val = MakeLogicalDeviceId();
			ILogicalDevice obj = deviceService.DeviceManager.AddLogicalDevice(val, (byte?)RawCapability, deviceSource, (Func<ILogicalDevice, bool>)((ILogicalDevice ld) => true));
			ILogicalDeviceWithStatus val2 = (ILogicalDeviceWithStatus)(object)((obj is ILogicalDeviceWithStatus) ? obj : null);
			if (val2 == null)
			{
				TaggedLog.Warning("RvCloudDeviceComponentStatus", $"Unable to Add Logical Device for {val} as it's not a {"ILogicalDeviceWithStatus"}", global::System.Array.Empty<object>());
				return null;
			}
			return val2;
		}

		public virtual void UpdateLogicalDevice(ILogicalDeviceService deviceService, ILogicalDeviceSource deviceSource, ILogicalDevice logicalDevice)
		{
		}

		public override string ToString()
		{
			global::System.Runtime.CompilerServices.DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new global::System.Runtime.CompilerServices.DefaultInterpolatedStringHandler(28, 3);
			defaultInterpolatedStringHandler.AppendFormatted(ComponentId);
			defaultInterpolatedStringHandler.AppendLiteral(": Status: ");
			defaultInterpolatedStringHandler.AppendFormatted(ArrayExtension.DebugDump(RawStatus, " ", false));
			defaultInterpolatedStringHandler.AppendLiteral(" Status Extended: ");
			Dictionary<byte, byte[]>? rawStatusExtended = RawStatusExtended;
			defaultInterpolatedStringHandler.AppendFormatted(((rawStatusExtended != null) ? DictionaryExtensions.DebugDump(rawStatusExtended) : null) ?? "None");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusAccessoryGateway : RvCloudDeviceComponentStatus<LogicalDeviceAccessoryGatewayStatusSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusAccessoryGateway(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection, byte? rawStatusEnhanced = null)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, rawStatusEnhanced)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusAccessoryGateway(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceAccessoryGatewayStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceAccessoryGatewayStatus val = new LogicalDeviceAccessoryGatewayStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceAccessoryGatewayStatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusAwningSensor : RvCloudDeviceComponentStatus<LogicalDeviceAwningStatusSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusAwningSensor(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusAwningSensor(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceAwningStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceAwningSensorStatus val = new LogicalDeviceAwningSensorStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceAwningStatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusBasicRelayType1 : RvCloudDeviceComponentStatus<LogicalDeviceRelayBasicStatusType1Serializable, LogicalDeviceRelayCapabilityType1>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusBasicRelayType1(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusBasicRelayType1(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceRelayBasicStatusType1Serializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceRelayBasicStatusType1 val = new LogicalDeviceRelayBasicStatusType1();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceRelayBasicStatusType1Serializable((ILogicalDeviceRelayBasicStatus)val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusBasicRelayType2 : RvCloudDeviceComponentStatus<LogicalDeviceRelayBasicStatusType2Serializable, LogicalDeviceRelayCapabilityType2>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusBasicRelayType2(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusBasicRelayType2(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceRelayBasicStatusType2Serializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceRelayBasicStatusType2 val = new LogicalDeviceRelayBasicStatusType2();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceRelayBasicStatusType2Serializable((ILogicalDeviceRelayBasicStatus)val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusBatteryMonitor : RvCloudDeviceComponentStatus<LogicalDeviceBatteryMonitorStatusSerializable, LogicalDeviceBatteryMonitorStatusExtendedSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusBatteryMonitor(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, byte? rawStatusEnhanced, Dictionary<byte, byte[]> rawStatusExtended, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, rawStatusEnhanced, rawStatusExtended, activeConnection)
		{
		}//IL_000a: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusBatteryMonitor(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceBatteryMonitorStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceBatteryMonitorStatus val = new LogicalDeviceBatteryMonitorStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceBatteryMonitorStatusSerializable(val);
		}

		public override LogicalDeviceBatteryMonitorStatusExtendedSerializable MakeStatusExtendedSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			LogicalDeviceBatteryMonitorStatusExtended val = new LogicalDeviceBatteryMonitorStatusExtended();
			if (base.RawStatusExtended != null)
			{
				((LogicalDeviceStatusPacketMutableExtended)val).Update((IReadOnlyDictionary<byte, byte[]>)(object)base.RawStatusExtended, (global::System.DateTime?)null);
			}
			return new LogicalDeviceBatteryMonitorStatusExtendedSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusBrakingSystem : RvCloudDeviceComponentStatus<LogicalDeviceBrakingSystemStatusSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusBrakingSystem(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusBrakingSystem(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceBrakingSystemStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceBrakingSystemStatus val = new LogicalDeviceBrakingSystemStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceBrakingSystemStatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusChassisInfo : RvCloudDeviceComponentStatus<LogicalDeviceChassisInfoStatusSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusChassisInfo(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusChassisInfo(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceChassisInfoStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceChassisInfoStatus val = new LogicalDeviceChassisInfoStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceChassisInfoStatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusClimateZone : RvCloudDeviceComponentStatus<LogicalDeviceClimateZoneStatusSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusClimateZone(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusClimateZone(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceClimateZoneStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceClimateZoneStatus val = new LogicalDeviceClimateZoneStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceClimateZoneStatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusDimmableLight : RvCloudDeviceComponentStatus<LogicalDeviceLightDimmableStatusSerializable, LogicalDeviceLightDimmableCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusDimmableLight(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusDimmableLight(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceLightDimmableStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceLightDimmableStatus val = new LogicalDeviceLightDimmableStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceLightDimmableStatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusDoorLock : RvCloudDeviceComponentStatus<LogicalDeviceDoorLockStatusSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusDoorLock(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusDoorLock(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceDoorLockStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceDoorLockStatus val = new LogicalDeviceDoorLockStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceDoorLockStatusSerializable(val);
		}
	}
	public static class RvCloudDeviceComponentStatusFactory
	{
		public const string LogTag = "RvCloudDeviceComponentStatusFactory";

		public static IDeviceComponent? MakeDeviceComponent(ILogicalDeviceWithStatus logicalDevice)
		{
			string componentId = ComponentIdEx.MakeComponentId(((ILogicalDevice)logicalDevice).LogicalId);
			LogicalDeviceAccessoryGateway val = (LogicalDeviceAccessoryGateway)(object)((logicalDevice is LogicalDeviceAccessoryGateway) ? logicalDevice : null);
			if (val == null)
			{
				LogicalDeviceRelayBasicLatchingType1 val2 = (LogicalDeviceRelayBasicLatchingType1)(object)((logicalDevice is LogicalDeviceRelayBasicLatchingType1) ? logicalDevice : null);
				if (val2 == null)
				{
					LogicalDeviceRelayBasicLatchingType2 val3 = (LogicalDeviceRelayBasicLatchingType2)(object)((logicalDevice is LogicalDeviceRelayBasicLatchingType2) ? logicalDevice : null);
					if (val3 == null)
					{
						LogicalDeviceLightDimmable val4 = (LogicalDeviceLightDimmable)(object)((logicalDevice is LogicalDeviceLightDimmable) ? logicalDevice : null);
						if (val4 == null)
						{
							LogicalDeviceLightRgb val5 = (LogicalDeviceLightRgb)(object)((logicalDevice is LogicalDeviceLightRgb) ? logicalDevice : null);
							if (val5 == null)
							{
								LogicalDeviceClimateZone val6 = (LogicalDeviceClimateZone)(object)((logicalDevice is LogicalDeviceClimateZone) ? logicalDevice : null);
								if (val6 == null)
								{
									LogicalDeviceGeneratorGenie val7 = (LogicalDeviceGeneratorGenie)(object)((logicalDevice is LogicalDeviceGeneratorGenie) ? logicalDevice : null);
									if (val7 == null)
									{
										LogicalDeviceAwningSensor val8 = (LogicalDeviceAwningSensor)(object)((logicalDevice is LogicalDeviceAwningSensor) ? logicalDevice : null);
										if (val8 == null)
										{
											LogicalDeviceTankSensor val9 = (LogicalDeviceTankSensor)(object)((logicalDevice is LogicalDeviceTankSensor) ? logicalDevice : null);
											if (val9 == null)
											{
												LogicalDeviceDoorLock val10 = (LogicalDeviceDoorLock)(object)((logicalDevice is LogicalDeviceDoorLock) ? logicalDevice : null);
												if (val10 == null)
												{
													LogicalDeviceMonitorPanel val11 = (LogicalDeviceMonitorPanel)(object)((logicalDevice is LogicalDeviceMonitorPanel) ? logicalDevice : null);
													if (val11 == null)
													{
														LogicalDeviceHourMeter val12 = (LogicalDeviceHourMeter)(object)((logicalDevice is LogicalDeviceHourMeter) ? logicalDevice : null);
														if (val12 == null)
														{
															LogicalDeviceRelayHBridgeMomentaryType2 val13 = (LogicalDeviceRelayHBridgeMomentaryType2)(object)((logicalDevice is LogicalDeviceRelayHBridgeMomentaryType2) ? logicalDevice : null);
															if (val13 == null)
															{
																LogicalDeviceLevelerType3 val14 = (LogicalDeviceLevelerType3)(object)((logicalDevice is LogicalDeviceLevelerType3) ? logicalDevice : null);
																if (val14 == null)
																{
																	LogicalDeviceLevelerType4 val15 = (LogicalDeviceLevelerType4)(object)((logicalDevice is LogicalDeviceLevelerType4) ? logicalDevice : null);
																	if (val15 == null)
																	{
																		LogicalDeviceLevelerType5 val16 = (LogicalDeviceLevelerType5)(object)((logicalDevice is LogicalDeviceLevelerType5) ? logicalDevice : null);
																		if (val16 == null)
																		{
																			LogicalDeviceChassisInfo val17 = (LogicalDeviceChassisInfo)(object)((logicalDevice is LogicalDeviceChassisInfo) ? logicalDevice : null);
																			if (val17 == null)
																			{
																				LogicalDeviceBrakingSystem val18 = (LogicalDeviceBrakingSystem)(object)((logicalDevice is LogicalDeviceBrakingSystem) ? logicalDevice : null);
																				if (val18 == null)
																				{
																					LogicalDeviceTemperatureSensor val19 = (LogicalDeviceTemperatureSensor)(object)((logicalDevice is LogicalDeviceTemperatureSensor) ? logicalDevice : null);
																					if (val19 == null)
																					{
																						LogicalDeviceBatteryMonitor val20 = (LogicalDeviceBatteryMonitor)(object)((logicalDevice is LogicalDeviceBatteryMonitor) ? logicalDevice : null);
																						if (val20 != null)
																						{
																							return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusBatteryMonitor(componentId, (ILogicalDeviceWithStatus)(object)val20);
																						}
																						return null;
																					}
																					return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusTemperatureSensor(componentId, (ILogicalDeviceWithStatus)(object)val19);
																				}
																				return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusBrakingSystem(componentId, (ILogicalDeviceWithStatus)(object)val18);
																			}
																			return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusChassisInfo(componentId, (ILogicalDeviceWithStatus)(object)val17);
																		}
																		return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusLevelerType5(componentId, (ILogicalDeviceWithStatus)(object)val16);
																	}
																	return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusLevelerType4(componentId, (ILogicalDeviceWithStatus)(object)val15);
																}
																return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusLevelerType3(componentId, (ILogicalDeviceWithStatus)(object)val14);
															}
															return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusRelayHBridgeType2(componentId, (ILogicalDeviceWithStatus)(object)val13);
														}
														return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusHourMeter(componentId, (ILogicalDeviceWithStatus)(object)val12);
													}
													return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusMonitorPanel(componentId, (ILogicalDeviceWithStatus)(object)val11);
												}
												return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusDoorLock(componentId, (ILogicalDeviceWithStatus)(object)val10);
											}
											return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusTankSensor(componentId, (ILogicalDeviceWithStatus)(object)val9);
										}
										return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusAwningSensor(componentId, (ILogicalDeviceWithStatus)(object)val8);
									}
									return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusGeneratorGenie(componentId, (ILogicalDeviceWithStatus)(object)val7);
								}
								return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusClimateZone(componentId, (ILogicalDeviceWithStatus)(object)val6);
							}
							return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusRgbLight(componentId, (ILogicalDeviceWithStatus)(object)val5);
						}
						return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusDimmableLight(componentId, (ILogicalDeviceWithStatus)(object)val4);
					}
					return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusBasicRelayType2(componentId, (ILogicalDeviceWithStatus)(object)val3);
				}
				return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusBasicRelayType1(componentId, (ILogicalDeviceWithStatus)(object)val2);
			}
			return (IDeviceComponent?)(object)new RvCloudDeviceComponentStatusAccessoryGateway(componentId, (ILogicalDeviceWithStatus)(object)val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusGeneratorGenie : RvCloudDeviceComponentStatus<LogicalDeviceGeneratorGenieStatusSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusGeneratorGenie(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection, byte? rawStatusEnhanced = null)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, rawStatusEnhanced)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusGeneratorGenie(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceGeneratorGenieStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceGeneratorGenieStatus val = new LogicalDeviceGeneratorGenieStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceGeneratorGenieStatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusHourMeter : RvCloudDeviceComponentStatus<LogicalDeviceHourMeterStatusSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusHourMeter(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusHourMeter(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceHourMeterStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceHourMeterStatus val = new LogicalDeviceHourMeterStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceHourMeterStatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusLevelerType3 : RvCloudDeviceComponentStatus<LogicalDeviceLevelerType3StatusSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusLevelerType3(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusLevelerType3(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceLevelerType3StatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceLevelerStatusType3 val = new LogicalDeviceLevelerStatusType3();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceLevelerType3StatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusLevelerType4 : RvCloudDeviceComponentStatus<LogicalDeviceLevelerType4StatusSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusLevelerType4(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusLevelerType4(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceLevelerType4StatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceLevelerStatusType4 val = new LogicalDeviceLevelerStatusType4();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceLevelerType4StatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusLevelerType5 : RvCloudDeviceComponentStatus<LogicalDeviceLevelerType5StatusSerializable, LogicalDeviceLevelerStatusExtendedType5Serializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusLevelerType5(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, byte? rawStatusEnhanced, Dictionary<byte, byte[]> rawStatusExtended, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, rawStatusEnhanced, rawStatusExtended, activeConnection)
		{
		}//IL_000a: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusLevelerType5(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceLevelerType5StatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceLevelerStatusType5 val = new LogicalDeviceLevelerStatusType5();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceLevelerType5StatusSerializable(val);
		}

		public override LogicalDeviceLevelerStatusExtendedType5Serializable MakeStatusExtendedSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Invalid comparison between Unknown and I4
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Expected O, but got Unknown
			LogicalDeviceLevelerStatusExtendedType5AutoStep val = new LogicalDeviceLevelerStatusExtendedType5AutoStep();
			LogicalDeviceLevelerStatusExtendedType5JackStrokeLength val2 = new LogicalDeviceLevelerStatusExtendedType5JackStrokeLength();
			if (base.RawStatusExtended != null)
			{
				Enumerator<byte, byte[]> enumerator = base.RawStatusExtended.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<byte, byte[]> current = enumerator.Current;
						LogicalDeviceLevelerStatusExtendedType5Kind val3 = (LogicalDeviceLevelerStatusExtendedType5Kind)current.Key;
						if ((int)val3 != 0)
						{
							if ((int)val3 == 1)
							{
								((LogicalDeviceStatusPacketMutableExtended)val2).Update(current.Value, current.Value.Length, current.Key, (global::System.DateTime?)null);
								continue;
							}
							TaggedLog.Warning("RvCloudDeviceComponentStatus", $"Unknown extended status kind: {current.Key}", global::System.Array.Empty<object>());
						}
						else
						{
							((LogicalDeviceStatusPacketMutableExtended)val).Update(current.Value, current.Value.Length, current.Key, (global::System.DateTime?)null);
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
				return new LogicalDeviceLevelerStatusExtendedType5Serializable(val, val2);
			}
			return new LogicalDeviceLevelerStatusExtendedType5Serializable(val, val2);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusMonitorPanel : RvCloudDeviceComponentStatus<LogicalDeviceMonitorPanelStatusSerializable, LogicalDeviceMonitorPanelCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusMonitorPanel(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection, byte? rawStatusEnhanced = null)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, rawStatusEnhanced)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusMonitorPanel(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceMonitorPanelStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceMonitorPanelStatus val = new LogicalDeviceMonitorPanelStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceMonitorPanelStatusSerializable(val);
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public abstract class RvCloudDeviceComponentStatus<TDeviceStatusSerializable, TDeviceCapability> : RvCloudDeviceComponentStatus where TDeviceStatusSerializable : ILogicalDeviceStatusSerializable where TDeviceCapability : ILogicalDeviceCapability, new()
	{
		public const string LogTag = "RvCloudDeviceComponentStatus";

		protected RvCloudDeviceComponentStatus(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection, byte? rawStatusEnhanced = null)
			: base(componentId, protocolVersion, rawCapability, rawStatus, rawStatusEnhanced, null, activeConnection)
		{
		}//IL_0009: Unknown result type (might be due to invalid IL or missing references)


		protected RvCloudDeviceComponentStatus(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override HashSet<LogicalDeviceCapabilitySerializable> MakeActiveCapabilitiesSerializable()
		{
			TDeviceCapability val = new TDeviceCapability();
			((ILogicalDeviceCapability)val/*cast due to .constrained prefix*/).UpdateDeviceCapability((byte?)base.RawCapability);
			return new HashSet<LogicalDeviceCapabilitySerializable>(((ILogicalDeviceCapability)val).ActiveCapabilities);
		}

		public abstract TDeviceStatusSerializable MakeStatusSerializable();

		public override void UpdateLogicalDevice(ILogicalDeviceService deviceService, ILogicalDeviceSource deviceSource, ILogicalDevice logicalDevice)
		{
			base.UpdateLogicalDevice(deviceService, deviceSource, logicalDevice);
			if ((object)deviceService.GetPrimaryDeviceSourceDirect(logicalDevice) != deviceSource)
			{
				return;
			}
			logicalDevice.UpdateDeviceCapability((byte?)base.RawCapability);
			ILogicalDeviceWithStatus val = (ILogicalDeviceWithStatus)(object)((logicalDevice is ILogicalDeviceWithStatus) ? logicalDevice : null);
			if (val != null)
			{
				val.UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)base.RawStatus, (uint)base.RawStatus.Length);
			}
			if (logicalDevice is ILogicalDeviceWithStatusExtended)
			{
				TaggedLog.Warning("RvCloudDeviceComponentStatus", $"Status data is supported by the logical device, however component implementation doesn't so ignoring extended status for {logicalDevice}: {this}", global::System.Array.Empty<object>());
				return;
			}
			Dictionary<byte, byte[]> rawStatusExtended = base.RawStatusExtended;
			if (rawStatusExtended != null && rawStatusExtended.Count > 0)
			{
				TaggedLog.Warning("RvCloudDeviceComponentStatus", $"RvCloudDeviceComponentStatus is providing extended status for {logicalDevice} which doesn't support extended status: {this}", global::System.Array.Empty<object>());
			}
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public abstract class RvCloudDeviceComponentStatus<TDeviceStatusSerializable, TDeviceStatusExtendedSerializable, TDeviceCapability> : RvCloudDeviceComponentStatus where TDeviceStatusSerializable : ILogicalDeviceStatusSerializable where TDeviceStatusExtendedSerializable : ILogicalDeviceStatusExtendedSerializable where TDeviceCapability : ILogicalDeviceCapability, new()
	{
		public const string LogTag = "RvCloudDeviceComponentStatus";

		protected RvCloudDeviceComponentStatus(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, byte? rawStatusEnhanced, Dictionary<byte, byte[]> rawStatusExtended, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, rawStatusEnhanced, rawStatusExtended, activeConnection)
		{
		}//IL_000a: Unknown result type (might be due to invalid IL or missing references)


		protected RvCloudDeviceComponentStatus(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override HashSet<LogicalDeviceCapabilitySerializable> MakeActiveCapabilitiesSerializable()
		{
			TDeviceCapability val = new TDeviceCapability();
			((ILogicalDeviceCapability)val/*cast due to .constrained prefix*/).UpdateDeviceCapability((byte?)base.RawCapability);
			return new HashSet<LogicalDeviceCapabilitySerializable>(((ILogicalDeviceCapability)val).ActiveCapabilities);
		}

		public abstract TDeviceStatusSerializable MakeStatusSerializable();

		public abstract TDeviceStatusExtendedSerializable MakeStatusExtendedSerializable();

		public override void UpdateLogicalDevice(ILogicalDeviceService deviceService, ILogicalDeviceSource deviceSource, ILogicalDevice logicalDevice)
		{
			base.UpdateLogicalDevice(deviceService, deviceSource, logicalDevice);
			if ((object)deviceService.GetPrimaryDeviceSourceDirect(logicalDevice) != deviceSource)
			{
				return;
			}
			logicalDevice.UpdateDeviceCapability((byte?)base.RawCapability);
			ILogicalDeviceWithStatus val = (ILogicalDeviceWithStatus)(object)((logicalDevice is ILogicalDeviceWithStatus) ? logicalDevice : null);
			if (val != null)
			{
				val.UpdateDeviceStatus((global::System.Collections.Generic.IReadOnlyList<byte>)base.RawStatus, (uint)base.RawStatus.Length);
			}
			ILogicalDeviceWithStatusExtended val2 = (ILogicalDeviceWithStatusExtended)(object)((logicalDevice is ILogicalDeviceWithStatusExtended) ? logicalDevice : null);
			if (val2 != null)
			{
				Dictionary<byte, byte[]> rawStatusExtended = base.RawStatusExtended;
				if (rawStatusExtended != null)
				{
					val2.UpdateDeviceStatusExtended((IReadOnlyDictionary<byte, byte[]>)(object)rawStatusExtended, (Dictionary<byte, global::System.DateTime>)null, true);
					return;
				}
				TaggedLog.Warning("RvCloudDeviceComponentStatus", $"RvCloudDeviceComponentStatus is missing extended status for {logicalDevice}: {this}", global::System.Array.Empty<object>());
			}
			else
			{
				Dictionary<byte, byte[]> rawStatusExtended2 = base.RawStatusExtended;
				if (rawStatusExtended2 != null && rawStatusExtended2.Count > 0)
				{
					TaggedLog.Warning("RvCloudDeviceComponentStatus", $"RvCloudDeviceComponentStatus is providing extended status for {logicalDevice} which doesn't support extended status: {this}", global::System.Array.Empty<object>());
				}
			}
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusRelayHBridgeType2 : RvCloudDeviceComponentStatus<LogicalDeviceRelayHBridgeStatusType2Serializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusRelayHBridgeType2(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusRelayHBridgeType2(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceRelayHBridgeStatusType2Serializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceRelayHBridgeStatusType2 val = new LogicalDeviceRelayHBridgeStatusType2();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceRelayHBridgeStatusType2Serializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusRgbLight : RvCloudDeviceComponentStatus<LogicalDeviceLightRgbStatusSerializable, LogicalDeviceLightRgbCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusRgbLight(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusRgbLight(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceLightRgbStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceLightRgbStatus val = new LogicalDeviceLightRgbStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceLightRgbStatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusTankSensor : RvCloudDeviceComponentStatus<LogicalDeviceTankSensorStatusSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusTankSensor(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusTankSensor(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceTankSensorStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceTankSensorStatus val = new LogicalDeviceTankSensorStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceTankSensorStatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusTemperatureSensor : RvCloudDeviceComponentStatus<LogicalDeviceTemperatureSensorStatusSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusTemperatureSensor(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, activeConnection, (byte?)null)
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusTemperatureSensor(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceTemperatureSensorStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceTemperatureSensorStatus val = new LogicalDeviceTemperatureSensorStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceTemperatureSensorStatusSerializable(val);
		}
	}
	[ComponentSerializationSchemaStatus(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentStatusTpms : RvCloudDeviceComponentStatus<LogicalDeviceTpmsStatusSerializable, LogicalDeviceTpmsStatusExtendedSerializable, LogicalDeviceCapability>
	{
		[JsonConstructor]
		public RvCloudDeviceComponentStatusTpms(string componentId, Version? protocolVersion, byte rawCapability, byte[] rawStatus, byte? rawStatusEnhanced, Dictionary<byte, byte[]> rawStatusExtended, LogicalDeviceActiveConnection activeConnection)
			: base(componentId, protocolVersion, rawCapability, rawStatus, rawStatusEnhanced, rawStatusExtended, activeConnection)
		{
		}//IL_000a: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentStatusTpms(string componentId, ILogicalDeviceWithStatus logicalDevice)
			: base(componentId, logicalDevice)
		{
		}

		public override LogicalDeviceTpmsStatusSerializable MakeStatusSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			LogicalDeviceTpmsStatus val = new LogicalDeviceTpmsStatus();
			((LogicalDeviceDataPacketMutableDoubleBuffer)val).Update(base.RawStatus, base.RawStatus.Length, false);
			return new LogicalDeviceTpmsStatusSerializable(val);
		}

		public override LogicalDeviceTpmsStatusExtendedSerializable MakeStatusExtendedSerializable()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			LogicalDeviceTpmsStatusExtended val = new LogicalDeviceTpmsStatusExtended();
			if (base.RawStatusExtended != null)
			{
				((LogicalDeviceStatusPacketMutableExtended)val).Update((IReadOnlyDictionary<byte, byte[]>)(object)base.RawStatusExtended, (global::System.DateTime?)null);
			}
			return new LogicalDeviceTpmsStatusExtendedSerializable(val);
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentResponse
{
	public interface IRvCloudDeviceComponentResponsePidValue : IDeviceComponent
	{
		Pid Pid { get; }

		ulong Value { get; }
	}
	public interface IRvCloudDeviceComponentResponseMetadata : IDeviceComponent
	{
		string SoftwarePartNumber { get; }

		Version? DeviceProtocolVersion { get; }
	}
	[ComponentSerializationSchema("RvCloudDeviceComponentResponseMetadata", 1, 0)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentResponseMetadata : IRvCloudDeviceComponentResponseMetadata, IDeviceComponent, IEquatable<RvCloudDeviceComponentResponseMetadata>, IJsonSerializerClass
	{
		private string? _serializerClass;

		[JsonProperty]
		[field: CompilerGenerated]
		public string ComponentId
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public string SoftwarePartNumber
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[JsonConverter(typeof(VersionConverter))]
		[field: CompilerGenerated]
		public Version? DeviceProtocolVersion
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		public string SerializerClass => _serializerClass ?? (_serializerClass = GetSchemaName());

		[JsonConstructor]
		public RvCloudDeviceComponentResponseMetadata(string componentId, string softwarePartNumber, Version? deviceProtocolVersion)
		{
			ComponentId = componentId;
			SoftwarePartNumber = softwarePartNumber;
			DeviceProtocolVersion = deviceProtocolVersion;
		}

		public bool Equals(RvCloudDeviceComponentResponseMetadata? other)
		{
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			if (ComponentId == other.ComponentId && SoftwarePartNumber == other.SoftwarePartNumber)
			{
				return DeviceProtocolVersion == other.DeviceProtocolVersion;
			}
			return false;
		}

		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			return Equals((RvCloudDeviceComponentResponseMetadata)obj);
		}

		public override int GetHashCode()
		{
			return (((((object)ComponentId).GetHashCode() * 397) ^ ((object)SoftwarePartNumber).GetHashCode()) * 397) ^ (((object)DeviceProtocolVersion)?.GetHashCode() ?? 0);
		}

		static RvCloudDeviceComponentResponseMetadata()
		{
			RegisterJsonSerializer();
		}

		protected static string GetSchemaName()
		{
			global::System.Type typeFromHandle = typeof(RvCloudDeviceComponentResponseMetadata);
			global::System.Attribute[] customAttributes = global::System.Attribute.GetCustomAttributes((MemberInfo)(object)typeFromHandle, typeof(ComponentSerializationSchemaAttribute), false);
			if (customAttributes.Length == 0)
			{
				throw new global::System.Exception("Component of type " + ((MemberInfo)typeFromHandle).Name + " missing ComponentSerializationSchemaAttribute");
			}
			if (customAttributes.Length > 1)
			{
				throw new global::System.Exception("Component of type " + ((MemberInfo)typeFromHandle).Name + " has multiple ComponentSerializationSchemaAttribute");
			}
			return ((Enumerable.First<global::System.Attribute>((global::System.Collections.Generic.IEnumerable<global::System.Attribute>)customAttributes) as ComponentSerializationSchemaAttribute) ?? throw new global::System.Exception("Component of type " + ((MemberInfo)typeFromHandle).Name + " has unexpected type, expecting ComponentSerializationSchemaAttribute")).Name;
		}

		protected static void RegisterJsonSerializer()
		{
			global::System.Type typeFromHandle = typeof(RvCloudDeviceComponentResponseMetadata);
			string schemaName = GetSchemaName();
			global::System.Type type = TypeRegistry.Lookup(schemaName, true);
			if (type != null && type != typeFromHandle)
			{
				throw new global::System.Exception($"Invalid Registration as {schemaName} is registered to {((MemberInfo)type).Name} and can't change registration to {((MemberInfo)typeFromHandle).Name}");
			}
			TypeRegistry.Register(GetSchemaName(), typeFromHandle);
		}

		public override string ToString()
		{
			return $"Rv Cloud Metadata Response {ComponentId} {SoftwarePartNumber} {((object)DeviceProtocolVersion).ToString() ?? "none"}";
		}
	}
	[ComponentSerializationSchema("RvCloudDeviceComponentResponsePidValue", 1, 0)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentResponsePidValue : IRvCloudDeviceComponentResponsePidValue, IDeviceComponent, IEquatable<RvCloudDeviceComponentResponsePidValue>, IJsonSerializerClass
	{
		private string? _serializerClass;

		[JsonProperty]
		[field: CompilerGenerated]
		public string ComponentId
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		[field: CompilerGenerated]
		public Pid Pid
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public ulong Value
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		public string SerializerClass => _serializerClass ?? (_serializerClass = GetSchemaName());

		[JsonConstructor]
		public RvCloudDeviceComponentResponsePidValue(string componentId, Pid pid, UInt48 value)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			ComponentId = componentId;
			Pid = pid;
			Value = UInt48.op_Implicit(value);
		}

		public bool Equals(RvCloudDeviceComponentResponsePidValue? other)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			if (ComponentId == other.ComponentId && Pid == other.Pid)
			{
				return Value == other.Value;
			}
			return false;
		}

		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			return Equals((RvCloudDeviceComponentResponsePidValue)obj);
		}

		public override int GetHashCode()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected I4, but got Unknown
			return (((((object)ComponentId).GetHashCode() * 397) ^ Pid) * 397) ^ Value.GetHashCode();
		}

		static RvCloudDeviceComponentResponsePidValue()
		{
			RegisterJsonSerializer();
		}

		protected static string GetSchemaName()
		{
			global::System.Type typeFromHandle = typeof(RvCloudDeviceComponentResponsePidValue);
			global::System.Attribute[] customAttributes = global::System.Attribute.GetCustomAttributes((MemberInfo)(object)typeFromHandle, typeof(ComponentSerializationSchemaAttribute), false);
			if (customAttributes.Length == 0)
			{
				throw new global::System.Exception("Component of type " + ((MemberInfo)typeFromHandle).Name + " missing ComponentSerializationSchemaAttribute");
			}
			if (customAttributes.Length > 1)
			{
				throw new global::System.Exception("Component of type " + ((MemberInfo)typeFromHandle).Name + " has multiple ComponentSerializationSchemaAttribute");
			}
			return ((Enumerable.First<global::System.Attribute>((global::System.Collections.Generic.IEnumerable<global::System.Attribute>)customAttributes) as ComponentSerializationSchemaAttribute) ?? throw new global::System.Exception("Component of type " + ((MemberInfo)typeFromHandle).Name + " has unexpected type, expecting ComponentSerializationSchemaAttribute")).Name;
		}

		protected static void RegisterJsonSerializer()
		{
			global::System.Type typeFromHandle = typeof(RvCloudDeviceComponentResponsePidValue);
			string schemaName = GetSchemaName();
			global::System.Type type = TypeRegistry.Lookup(schemaName, true);
			if (type != null && type != typeFromHandle)
			{
				throw new global::System.Exception($"Invalid Registration as {schemaName} is registered to {((MemberInfo)type).Name} and can't change registration to {((MemberInfo)typeFromHandle).Name}");
			}
			TypeRegistry.Register(GetSchemaName(), typeFromHandle);
		}

		public override string ToString()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			return $"Rv Cloud PID Response {Pid} = 0x{Value:X}";
		}
	}
	[ComponentSerializationSchema("RvCloudDeviceComponentResponsePidValueIndex", 1, 0)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentResponsePidValueIndex : IRvCloudDeviceComponentResponsePidValue, IDeviceComponent, IEquatable<RvCloudDeviceComponentResponsePidValueIndex>, IJsonSerializerClass
	{
		private string? _serializerClass;

		[JsonProperty]
		[field: CompilerGenerated]
		public string ComponentId
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		[field: CompilerGenerated]
		public Pid Pid
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public ulong Value
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public ushort Address
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[JsonProperty]
		public string SerializerClass => _serializerClass ?? (_serializerClass = GetSchemaName());

		[JsonConstructor]
		public RvCloudDeviceComponentResponsePidValueIndex(string componentId, Pid pid, UInt48 value, ushort address)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			ComponentId = componentId;
			Pid = pid;
			Value = UInt48.op_Implicit(value);
			Address = address;
		}

		public bool Equals(RvCloudDeviceComponentResponsePidValueIndex? other)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			if (ComponentId == other.ComponentId && Pid == other.Pid && Value == other.Value)
			{
				return Address == other.Address;
			}
			return false;
		}

		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			return ((object)this).Equals((object)(RvCloudDeviceComponentResponsePidValue)obj);
		}

		public override int GetHashCode()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected I4, but got Unknown
			return (((((object)ComponentId).GetHashCode() * 397) ^ Pid) * 397) ^ Value.GetHashCode();
		}

		static RvCloudDeviceComponentResponsePidValueIndex()
		{
			RegisterJsonSerializer();
		}

		protected static string GetSchemaName()
		{
			global::System.Type typeFromHandle = typeof(RvCloudDeviceComponentResponsePidValueIndex);
			global::System.Attribute[] customAttributes = global::System.Attribute.GetCustomAttributes((MemberInfo)(object)typeFromHandle, typeof(ComponentSerializationSchemaAttribute), false);
			if (customAttributes.Length == 0)
			{
				throw new global::System.Exception("Component of type " + ((MemberInfo)typeFromHandle).Name + " missing ComponentSerializationSchemaAttribute");
			}
			if (customAttributes.Length > 1)
			{
				throw new global::System.Exception("Component of type " + ((MemberInfo)typeFromHandle).Name + " has multiple ComponentSerializationSchemaAttribute");
			}
			return ((Enumerable.First<global::System.Attribute>((global::System.Collections.Generic.IEnumerable<global::System.Attribute>)customAttributes) as ComponentSerializationSchemaAttribute) ?? throw new global::System.Exception("Component of type " + ((MemberInfo)typeFromHandle).Name + " has unexpected type, expecting ComponentSerializationSchemaAttribute")).Name;
		}

		protected static void RegisterJsonSerializer()
		{
			global::System.Type typeFromHandle = typeof(RvCloudDeviceComponentResponsePidValueIndex);
			string schemaName = GetSchemaName();
			global::System.Type type = TypeRegistry.Lookup(schemaName, true);
			if (type != null && type != typeFromHandle)
			{
				throw new global::System.Exception($"Invalid Registration as {schemaName} is registered to {((MemberInfo)type).Name} and can't change registration to {((MemberInfo)typeFromHandle).Name}");
			}
			TypeRegistry.Register(GetSchemaName(), typeFromHandle);
		}

		public override string ToString()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			return $"Rv Cloud PID Response {Pid}[{Address}] = 0x{Value:X}";
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands
{
	public interface IDeviceComponentCommand : IDeviceComponent
	{
	}
	public interface IDeviceComponentCommandSerializable : IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public abstract class RvCloudDeviceComponentCommand : IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass, IEquatable<RvCloudDeviceComponentCommand>
	{
		private const string LogTag = "RvCloudDeviceComponentCommand";

		[JsonProperty(PropertyName = "cid")]
		[field: CompilerGenerated]
		public string ComponentId
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		public abstract string SerializerClass { get; }

		[JsonConstructor]
		protected RvCloudDeviceComponentCommand(string componentId)
		{
			ComponentId = componentId;
		}

		public virtual bool Equals(RvCloudDeviceComponentCommand? other)
		{
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			return Enumerable.SequenceEqual<char>((global::System.Collections.Generic.IEnumerable<char>)ComponentId, (global::System.Collections.Generic.IEnumerable<char>)other.ComponentId);
		}

		public override bool Equals(object otherObj)
		{
			if (otherObj == null)
			{
				return false;
			}
			if (this == otherObj)
			{
				return true;
			}
			if (!(otherObj is RvCloudDeviceComponentCommand other))
			{
				return false;
			}
			return Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Hash<string>(17, ComponentId);
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public abstract class RvCloudDeviceComponentCommand<TSerializable, TCommand> : RvCloudDeviceComponentCommand, IEquatable<RvCloudDeviceComponentCommand<TSerializable, TCommand>>, IJsonSerializerClass where TSerializable : IDeviceComponentCommandSerializable where TCommand : IDeviceCommandPacket
	{
		private string? _serializerClass;

		[JsonIgnore]
		public abstract TCommand Command { get; }

		[JsonProperty]
		public sealed override string SerializerClass => _serializerClass ?? (_serializerClass = GetCommandName());

		protected RvCloudDeviceComponentCommand(string componentId)
			: base(componentId)
		{
		}

		public virtual bool Equals(RvCloudDeviceComponentCommand<TSerializable, TCommand>? other)
		{
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			if (base.Equals(other))
			{
				return ((object)Command/*cast due to .constrained prefix*/).Equals((object)other.Command);
			}
			return false;
		}

		public override bool Equals(object otherObj)
		{
			if (otherObj == null)
			{
				return false;
			}
			if (this == otherObj)
			{
				return true;
			}
			if (!(otherObj is RvCloudDeviceComponentCommand<TSerializable, TCommand> other))
			{
				return false;
			}
			return Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Hash<TCommand>(HashCode.Hash<string>(17, base.ComponentId), Command);
		}

		static RvCloudDeviceComponentCommand()
		{
			RegisterJsonSerializer();
		}

		protected static string GetCommandName()
		{
			global::System.Type typeFromHandle = typeof(TSerializable);
			global::System.Attribute[] customAttributes = global::System.Attribute.GetCustomAttributes((MemberInfo)(object)typeFromHandle, typeof(ComponentSerializationSchemaCommandAttribute), false);
			if (customAttributes.Length == 0)
			{
				throw new global::System.Exception("Command Component of type " + ((MemberInfo)typeFromHandle).Name + " missing ComponentSerializationSchemaCommandAttribute");
			}
			if (customAttributes.Length > 1)
			{
				throw new global::System.Exception("Command Component of type " + ((MemberInfo)typeFromHandle).Name + " has multiple ComponentSerializationSchemaCommandAttribute");
			}
			return ((Enumerable.First<global::System.Attribute>((global::System.Collections.Generic.IEnumerable<global::System.Attribute>)customAttributes) as ComponentSerializationSchemaCommandAttribute) ?? throw new global::System.Exception("Command Component of type " + ((MemberInfo)typeFromHandle).Name + " has unexpected type, expecting ComponentSerializationSchemaCommandAttribute")).Name;
		}

		protected static void RegisterJsonSerializer()
		{
			global::System.Type typeFromHandle = typeof(TSerializable);
			string commandName = GetCommandName();
			global::System.Type type = TypeRegistry.Lookup(commandName, true);
			if (type != null && type != typeFromHandle)
			{
				throw new global::System.Exception($"Invalid Registration as {commandName} is registered to {((MemberInfo)type).Name} and can't change registration to {((MemberInfo)typeFromHandle).Name}");
			}
			TypeRegistry.Register(GetCommandName(), typeFromHandle);
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.Rename
{
	public class CommandRename : LogicalDeviceCommandPacket
	{
		public const int FunctionNameSize = 2;

		public const int FunctionInstanceSize = 1;

		public const int FunctionNameStartIndex = 0;

		public const int FunctionInstanceStartIndex = 2;

		public FunctionName FunctionName => (FunctionName)ArrayExtension.GetValueUInt16(((LogicalDeviceCommandPacket)this).Data, 0, (Endian)0);

		public byte FunctionInstance => ((LogicalDeviceCommandPacket)this).Data[2];

		public CommandRename(FunctionName functionName, byte functionInstance)
			: base((byte)0, EncodeValue(functionName, functionInstance), 200)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		private static byte[] EncodeValue(FunctionName functionName, byte functionInstance)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected I4, but got Unknown
			byte[] array = new byte[3];
			ArrayExtension.SetValueUInt16(array, (ushort)(int)functionName, 0, (Endian)0);
			array[2] = functionInstance;
			return array;
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public interface IRvCloudDeviceComponentCommandRename : IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		FunctionName FunctionName { get; }

		byte FunctionInstance { get; }
	}
	[ComponentSerializationSchemaCommand("DeviceComponentCommandRename", 1, 0)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandRename : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandRename, CommandRename>, IRvCloudDeviceComponentCommandRename, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		[field: CompilerGenerated]
		public FunctionName FunctionName
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public byte FunctionInstance
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public override CommandRename Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandRename(string componentId, FunctionName functionName, byte functionInstance)
			: this(componentId, new CommandRename(functionName, functionInstance))
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentCommandRename(string componentId, CommandRename command)
			: base(componentId)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			FunctionName = command.FunctionName;
			FunctionInstance = command.FunctionInstance;
			Command = command;
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.RelayBasicType2
{
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandRelayBasicType2SetState : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandRelayBasicType2SetState, LogicalDeviceRelayBasicLatchingCommandType2>, IRvCloudDeviceComponentCommandRelayBasicType2, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		public bool IsOn => Command.IsOn;

		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceRelayBasicLatchingCommandType2 Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandRelayBasicType2SetState(string componentId, bool isOn)
			: this(componentId, isOn ? LogicalDeviceRelayBasicLatchingCommandType2.MakeLatchTurnOnRelayCommand() : LogicalDeviceRelayBasicLatchingCommandType2.MakeLatchTurnOffRelayCommand())
		{
		}

		public RvCloudDeviceComponentCommandRelayBasicType2SetState(string componentId, LogicalDeviceRelayBasicLatchingCommandType2 command)
			: base(componentId)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (command.ClearingFault)
			{
				throw new ArgumentException($"Command given is not a set state command {command}", "command");
			}
			Command = command;
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.RelayBasic
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public interface IRvCloudDeviceComponentCommandRelayBasicType2 : IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		LogicalDeviceRelayBasicLatchingCommandType2 Command { get; }
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandRelayBasicType2ClearFault : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandRelayBasicType2ClearFault, LogicalDeviceRelayBasicLatchingCommandType2>, IRvCloudDeviceComponentCommandRelayBasicType2, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceRelayBasicLatchingCommandType2 Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandRelayBasicType2ClearFault(string componentId)
			: this(componentId, LogicalDeviceRelayBasicLatchingCommandType2.MakeClearFaultCommand())
		{
		}

		public RvCloudDeviceComponentCommandRelayBasicType2ClearFault(string componentId, LogicalDeviceRelayBasicLatchingCommandType2 command)
			: base(componentId)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (!command.ClearingFault)
			{
				throw new ArgumentException($"Command given is not a clear Fault Command {command}", "command");
			}
			Command = command;
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.RelayBasicType1
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public interface IRvCloudDeviceComponentCommandRelayBasicType1 : IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		LogicalDeviceRelayBasicCommandType1 Command { get; }
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandRelayBasicType1ClearFault : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandRelayBasicType1ClearFault, LogicalDeviceRelayBasicCommandType1>, IRvCloudDeviceComponentCommandRelayBasicType1, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceRelayBasicCommandType1 Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandRelayBasicType1ClearFault(string componentId)
			: this(componentId, LogicalDeviceRelayBasicCommandType1.MakeClearFaultCommand())
		{
		}

		public RvCloudDeviceComponentCommandRelayBasicType1ClearFault(string componentId, LogicalDeviceRelayBasicCommandType1 command)
			: base(componentId)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (!command.ClearingFault)
			{
				throw new ArgumentException($"Command given is not a clear Fault Command {command}", "command");
			}
			Command = command;
		}
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandRelayBasicType1SetState : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandRelayBasicType1SetState, LogicalDeviceRelayBasicCommandType1>, IRvCloudDeviceComponentCommandRelayBasicType1, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		public bool IsOn => Command.IsOn;

		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceRelayBasicCommandType1 Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandRelayBasicType1SetState(string componentId, bool isOn)
			: this(componentId, isOn ? LogicalDeviceRelayBasicCommandType1.MakeLatchTurnOnRelayCommand() : LogicalDeviceRelayBasicCommandType1.MakeLatchTurnOffRelayCommand())
		{
		}

		public RvCloudDeviceComponentCommandRelayBasicType1SetState(string componentId, LogicalDeviceRelayBasicCommandType1 command)
			: base(componentId)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (command.ClearingFault)
			{
				throw new ArgumentException($"Command given is not a set state command {command}", "command");
			}
			Command = command;
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.PidWrite
{
	public class CommandPidWrite : LogicalDeviceCommandPacket
	{
		public const int PidStartIndex = 0;

		public const int PidSize = 2;

		public const int ValueStartIndex = 2;

		public const int ValueSize = 6;

		public const int SessionTypeStartIndex = 8;

		public const int SessionTypeSize = 2;

		public Pid Pid => (Pid)ArrayExtension.GetValueUInt16(((LogicalDeviceCommandPacket)this).Data, 0, (Endian)0);

		public UInt48 Value => (UInt48)ArrayExtension.GetValueUInt48(((LogicalDeviceCommandPacket)this).Data, 2, (Endian)0);

		public LogicalDeviceSessionType PidWriteAccess => (LogicalDeviceSessionType)ArrayExtension.GetValueUInt16(((LogicalDeviceCommandPacket)this).Data, 8, (Endian)0);

		public CommandPidWrite(Pid pid, UInt48 value, LogicalDeviceSessionType pidWriteAccess)
			: base((byte)0, EncodeValue(pid, value, pidWriteAccess), 200)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)


		private static byte[] EncodeValue(Pid pid, UInt48 value, LogicalDeviceSessionType pidWriteAccess)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected I4, but got Unknown
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			byte[] array = new byte[10];
			ArrayExtension.SetValueUInt16(array, (ushort)(int)pid, 0, (Endian)0);
			ArrayExtension.SetValueUInt48(array, UInt48.op_Implicit(value), 2, (Endian)0);
			ArrayExtension.SetValueUInt16(array, (ushort)pidWriteAccess, 8, (Endian)0);
			return array;
		}
	}
	public class CommandPidWriteWithAddress : LogicalDeviceCommandPacket
	{
		public const int PidStartIndex = 0;

		public const int PidSize = 2;

		public const int AddressStartIndex = 2;

		public const int AddressSize = 2;

		public const int ValueStartIndex = 4;

		public const int ValueSize = 4;

		public const int SessionTypeStartIndex = 8;

		public const int SessionTypeSize = 2;

		public Pid Pid => (Pid)ArrayExtension.GetValueUInt16(((LogicalDeviceCommandPacket)this).Data, 0, (Endian)0);

		public ushort Address => ArrayExtension.GetValueUInt16(((LogicalDeviceCommandPacket)this).Data, 2, (Endian)0);

		public UInt48 Value => (UInt48)ArrayExtension.GetValueUInt48(((LogicalDeviceCommandPacket)this).Data, 4, (Endian)0);

		public LogicalDeviceSessionType PidWriteAccess => (LogicalDeviceSessionType)ArrayExtension.GetValueUInt16(((LogicalDeviceCommandPacket)this).Data, 8, (Endian)0);

		public CommandPidWriteWithAddress(Pid pid, ushort address, UInt48 value, LogicalDeviceSessionType pidWriteAccess)
			: base((byte)0, EncodeValue(pid, address, value, pidWriteAccess), 200)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)


		private static byte[] EncodeValue(Pid pid, ushort address, UInt48 value, LogicalDeviceSessionType pidWriteAccess)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected I4, but got Unknown
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			byte[] array = new byte[10];
			ArrayExtension.SetValueUInt16(array, (ushort)(int)pid, 0, (Endian)0);
			ArrayExtension.SetValueUInt16(array, address, 2, (Endian)0);
			ArrayExtension.SetValueUInt48(array, UInt48.op_Implicit(value), 4, (Endian)0);
			ArrayExtension.SetValueUInt16(array, (ushort)pidWriteAccess, 8, (Endian)0);
			return array;
		}
	}
	[ComponentSerializationSchemaCommand("DeviceComponentCommandPidWrite", 1, 0)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandPidWrite : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandPidWrite, CommandPidWrite>, IRvCloudDeviceComponentCommandPid, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		[field: CompilerGenerated]
		public Pid Pid
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public ulong Value
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		[field: CompilerGenerated]
		public LogicalDeviceSessionType PidWriteAccess
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public override CommandPidWrite Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandPidWrite(string componentId, Pid pid, UInt48 value, LogicalDeviceSessionType pidWriteAccess)
			: this(componentId, new CommandPidWrite(pid, value, pidWriteAccess))
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentCommandPidWrite(string componentId, CommandPidWrite command)
			: base(componentId)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			Pid = command.Pid;
			Value = UInt48.op_Implicit(command.Value);
			PidWriteAccess = command.PidWriteAccess;
			Command = command;
		}
	}
	[ComponentSerializationSchemaCommand("DeviceComponentCommandPidWriteWithAddress", 1, 0)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandPidWriteWithAddress : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandPidWriteWithAddress, CommandPidWriteWithAddress>, IRvCloudDeviceComponentCommandPid, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		[field: CompilerGenerated]
		public Pid Pid
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public ushort Address
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public ulong Value
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		[field: CompilerGenerated]
		public LogicalDeviceSessionType PidWriteAccess
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public override CommandPidWriteWithAddress Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandPidWriteWithAddress(string componentId, Pid pid, ushort address, UInt48 value, LogicalDeviceSessionType pidWriteAccess)
			: this(componentId, new CommandPidWriteWithAddress(pid, address, value, pidWriteAccess))
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentCommandPidWriteWithAddress(string componentId, CommandPidWriteWithAddress command)
			: base(componentId)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			Pid = command.Pid;
			Address = command.Address;
			Value = UInt48.op_Implicit(command.Value);
			PidWriteAccess = command.PidWriteAccess;
			Command = command;
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.PidRead
{
	public class CommandPidRead : LogicalDeviceCommandPacket
	{
		public const int PidStartIndex = 0;

		public const int PidSize = 2;

		public Pid Pid => (Pid)ArrayExtension.GetValueUInt16(((LogicalDeviceCommandPacket)this).Data, 0, (Endian)0);

		public CommandPidRead(Pid pid)
			: base((byte)0, EncodeValue(pid), 200)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		private static byte[] EncodeValue(Pid pid)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected I4, but got Unknown
			byte[] array = new byte[2];
			ArrayExtension.SetValueUInt16(array, (ushort)(int)pid, 0, (Endian)0);
			return array;
		}
	}
	public class CommandPidReadWithAddress : LogicalDeviceCommandPacket
	{
		public const int PidStartIndex = 0;

		public const int PidSize = 2;

		public const int AddressStartIndex = 2;

		public const int AddressSize = 2;

		public Pid Pid => (Pid)ArrayExtension.GetValueUInt16(((LogicalDeviceCommandPacket)this).Data, 0, (Endian)0);

		public ushort Address => ArrayExtension.GetValueUInt16(((LogicalDeviceCommandPacket)this).Data, 2, (Endian)0);

		public CommandPidReadWithAddress(Pid pid, ushort address)
			: base((byte)0, EncodeValue(pid, address), 200)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		private static byte[] EncodeValue(Pid pid, ushort address)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected I4, but got Unknown
			byte[] array = new byte[4];
			ArrayExtension.SetValueUInt16(array, (ushort)(int)pid, 0, (Endian)0);
			ArrayExtension.SetValueUInt16(array, address, 2, (Endian)0);
			return array;
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public interface IRvCloudDeviceComponentCommandPid : IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		Pid Pid { get; }
	}
	[ComponentSerializationSchemaCommand("DeviceComponentCommandPidRead", 1, 0)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandPidRead : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandPidRead, CommandPidRead>, IRvCloudDeviceComponentCommandPid, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		[field: CompilerGenerated]
		public Pid Pid
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public override CommandPidRead Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandPidRead(string componentId, Pid pid)
			: this(componentId, new CommandPidRead(pid))
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentCommandPidRead(string componentId, CommandPidRead command)
			: base(componentId)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			Pid = command.Pid;
			Command = command;
		}
	}
	[ComponentSerializationSchemaCommand("DeviceComponentCommandPidReadWithAddress", 1, 0)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandPidReadWithAddress : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandPidReadWithAddress, CommandPidReadWithAddress>, IRvCloudDeviceComponentCommandPid, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		[field: CompilerGenerated]
		public Pid Pid
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public ushort Address
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public override CommandPidReadWithAddress Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandPidReadWithAddress(string componentId, Pid pid, ushort address)
			: this(componentId, new CommandPidReadWithAddress(pid, address))
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentCommandPidReadWithAddress(string componentId, CommandPidReadWithAddress command)
			: base(componentId)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			Pid = command.Pid;
			Address = command.Address;
			Command = command;
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.Metadata
{
	public class CommandMetadataGet : LogicalDeviceCommandPacket
	{
		public CommandMetadataGet()
			: base((byte)0, 200)
		{
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public interface IRvCloudDeviceComponentCommandMetadata : IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
	}
	[ComponentSerializationSchemaCommand("DeviceComponentCommandMetadataGet", 1, 0)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandMetadataGet : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandMetadataGet, CommandMetadataGet>, IRvCloudDeviceComponentCommandMetadata, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override CommandMetadataGet Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandMetadataGet(string componentId)
			: this(componentId, new CommandMetadataGet())
		{
		}

		public RvCloudDeviceComponentCommandMetadataGet(string componentId, CommandMetadataGet command)
			: base(componentId)
		{
			Command = command;
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.LightRgb
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public interface IRvCloudDeviceComponentCommandLightRgb : IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		LogicalDeviceLightRgbCommand Command { get; }
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandLightRgbRestore : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandLightRgbRestore, LogicalDeviceLightRgbCommand>, IRvCloudDeviceComponentCommandLightRgb, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceLightRgbCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandLightRgbRestore(string componentId)
			: this(componentId, LogicalDeviceLightRgbCommand.MakeRestoreCommand())
		{
		}

		public RvCloudDeviceComponentCommandLightRgbRestore(string componentId, LogicalDeviceLightRgbCommand command)
			: base(componentId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if ((int)command.Mode != 127)
			{
				throw new ArgumentException($"Command given is not a restore Command {command}");
			}
			Command = command;
		}
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandLightRgbSetStateBlink : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandLightRgbSetStateBlink, LogicalDeviceLightRgbCommand>, IRvCloudDeviceComponentCommandLightRgb, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		public byte Red
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				RgbColor color = Command.Color;
				return ((RgbColor)(ref color)).Red;
			}
		}

		[JsonProperty]
		public byte Green
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				RgbColor color = Command.Color;
				return ((RgbColor)(ref color)).Green;
			}
		}

		[JsonProperty]
		public byte Blue
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				RgbColor color = Command.Color;
				return ((RgbColor)(ref color)).Blue;
			}
		}

		[JsonProperty]
		public byte AutoOffDuration => Command.AutoOffDuration;

		[JsonProperty]
		public byte OnInterval => Command.OnInterval;

		[JsonProperty]
		public byte OffInterval => Command.OnInterval;

		[JsonIgnore]
		public RgbColor Color => new RgbColor(Red, Green, Blue);

		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceLightRgbCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandLightRgbSetStateBlink(string componentId, byte red, byte green, byte blue, byte autoOffDuration, byte onInterval, byte offInterval)
			: this(componentId, LogicalDeviceLightRgbCommand.MakeBlinkCommand(new RgbColor(red, green, blue), autoOffDuration, onInterval, offInterval))
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentCommandLightRgbSetStateBlink(string componentId, LogicalDeviceLightRgbCommand command)
			: base(componentId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Invalid comparison between Unknown and I4
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if ((int)command.Mode != 2)
			{
				throw new ArgumentException($"Command given is not an ON Command {command}", "command");
			}
			Command = command;
		}
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandLightRgbSetStateOff : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandLightRgbSetStateOff, LogicalDeviceLightRgbCommand>, IRvCloudDeviceComponentCommandLightRgb, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceLightRgbCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandLightRgbSetStateOff(string componentId)
			: this(componentId, new LogicalDeviceLightRgbCommand())
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown


		public RvCloudDeviceComponentCommandLightRgbSetStateOff(string componentId, LogicalDeviceLightRgbCommand command)
			: base(componentId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if ((int)command.Mode != 0)
			{
				throw new ArgumentException($"Command given is not an Off Command {command}", "command");
			}
			Command = command;
		}
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandLightRgbSetStateOn : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandLightRgbSetStateOn, LogicalDeviceLightRgbCommand>, IRvCloudDeviceComponentCommandLightRgb, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		public byte Red
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				RgbColor color = Command.Color;
				return ((RgbColor)(ref color)).Red;
			}
		}

		[JsonProperty]
		public byte Green
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				RgbColor color = Command.Color;
				return ((RgbColor)(ref color)).Green;
			}
		}

		[JsonProperty]
		public byte Blue
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				RgbColor color = Command.Color;
				return ((RgbColor)(ref color)).Blue;
			}
		}

		[JsonProperty]
		public byte AutoOffDuration => Command.AutoOffDuration;

		[JsonIgnore]
		public RgbColor Color => new RgbColor(Red, Green, Blue);

		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceLightRgbCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandLightRgbSetStateOn(string componentId, byte red, byte green, byte blue, byte autoOffDuration)
			: this(componentId, LogicalDeviceLightRgbCommand.MakeOnCommand(new RgbColor(red, green, blue), autoOffDuration, (LogicalDeviceLightRgbStatus)null))
		{
		}//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentCommandLightRgbSetStateOn(string componentId, LogicalDeviceLightRgbCommand command)
			: base(componentId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Invalid comparison between Unknown and I4
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if ((int)command.Mode != 1)
			{
				throw new ArgumentException($"Command given is not an ON Command {command}", "command");
			}
			Command = command;
		}
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandLightRgbSetStateTransition : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandLightRgbSetStateTransition, LogicalDeviceLightRgbCommand>, IRvCloudDeviceComponentCommandLightRgb, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public RgbLightTransitionMode Mode => LogicalDeviceLightRgbMode.RgbLightTransitionModeFromByte((byte)(int)Command.Mode);

		[JsonProperty]
		public byte AutoOffDuration => Command.AutoOffDuration;

		[JsonProperty]
		public ushort Interval => Command.Interval;

		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceLightRgbCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandLightRgbSetStateTransition(string componentId, RgbLightTransitionMode mode, byte autoOffDuration, ushort interval)
			: this(componentId, LogicalDeviceLightRgbCommand.MakeTrasitionModeCommand(mode, autoOffDuration, interval))
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		public RvCloudDeviceComponentCommandLightRgbSetStateTransition(string componentId, LogicalDeviceLightRgbCommand command)
			: base(componentId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected I4, but got Unknown
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			LogicalDeviceLightRgbMode.RgbLightTransitionModeFromByte((byte)(int)command.Mode);
			Command = command;
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.LightDimmable
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public interface IRvCloudDeviceComponentCommandLightDimmable : IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		LogicalDeviceLightDimmableCommand Command { get; }
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandLightDimmableRestore : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandLightDimmableRestore, LogicalDeviceLightDimmableCommand>, IRvCloudDeviceComponentCommandLightDimmable, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceLightDimmableCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandLightDimmableRestore(string componentId)
			: this(componentId, LogicalDeviceLightDimmableCommand.MakeRestoreCommand())
		{
		}

		public RvCloudDeviceComponentCommandLightDimmableRestore(string componentId, LogicalDeviceLightDimmableCommand command)
			: base(componentId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if ((int)command.Command != 127)
			{
				throw new ArgumentException($"Command given is not a restore Command {command}", "command");
			}
			Command = command;
		}
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandLightDimmableSetState : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandLightDimmableSetState, LogicalDeviceLightDimmableCommand>, IRvCloudDeviceComponentCommandLightDimmable, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public DimmableLightMode Mode => DimmableLightCommandExtension.ConvertToMode(Command.Command, (DimmableLightMode)1);

		[JsonProperty]
		public byte MaxBrightness => Command.MaxBrightness;

		[JsonProperty]
		public byte Duration => Command.Duration;

		[JsonProperty]
		public int CycleTime1 => Command.CycleTime1;

		[JsonProperty]
		public int CycleTime2 => Command.CycleTime2;

		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceLightDimmableCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandLightDimmableSetState(string componentId, DimmableLightMode mode, byte maxBrightness, byte duration, int cycleTime1, int cycleTime2)
			: this(componentId, new LogicalDeviceLightDimmableCommand(mode, maxBrightness, duration, cycleTime1, cycleTime2))
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown


		public RvCloudDeviceComponentCommandLightDimmableSetState(string componentId, LogicalDeviceLightDimmableCommand command)
			: base(componentId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if ((int)command.Command == 127)
			{
				throw new ArgumentException($"Command given is invalid: {command}", "command");
			}
			Command = command;
		}
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandLightDimmableSetStateOff : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandLightDimmableSetStateOff, LogicalDeviceLightDimmableCommand>, IRvCloudDeviceComponentCommandLightDimmable, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceLightDimmableCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandLightDimmableSetStateOff(string componentId)
			: this(componentId, new LogicalDeviceLightDimmableCommand((DimmableLightCommand)0, (byte)0, (byte)0, 0, 0))
		{
		}//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown


		public RvCloudDeviceComponentCommandLightDimmableSetStateOff(string componentId, LogicalDeviceLightDimmableCommand command)
			: base(componentId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if ((int)command.Command != 0)
			{
				throw new ArgumentException($"Command given is not Off: {command}", "command");
			}
			Command = command;
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.GeneratorGenie
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public interface IRvCloudDeviceComponentCommandGeneratorGenie : IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		LogicalDeviceGeneratorGenieCommand Command { get; }
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandGeneratorGenieOff : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandGeneratorGenieOff, LogicalDeviceGeneratorGenieCommand>, IRvCloudDeviceComponentCommandGeneratorGenie, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceGeneratorGenieCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandGeneratorGenieOff(string componentId)
			: this(componentId, new LogicalDeviceGeneratorGenieCommand((GeneratorGenieCommand)1))
		{
		}//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown


		public RvCloudDeviceComponentCommandGeneratorGenieOff(string componentId, LogicalDeviceGeneratorGenieCommand command)
			: base(componentId)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (!command.IsOffCommand)
			{
				throw new ArgumentException($"Command given is not an Off Command {command}", "command");
			}
			Command = command;
		}
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandGeneratorGenieOn : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandGeneratorGenieOn, LogicalDeviceGeneratorGenieCommand>, IRvCloudDeviceComponentCommandGeneratorGenie, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceGeneratorGenieCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandGeneratorGenieOn(string componentId)
			: this(componentId, new LogicalDeviceGeneratorGenieCommand((GeneratorGenieCommand)2))
		{
		}//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown


		public RvCloudDeviceComponentCommandGeneratorGenieOn(string componentId, LogicalDeviceGeneratorGenieCommand command)
			: base(componentId)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (!command.IsOnCommand)
			{
				throw new ArgumentException($"Command given is not an On Command {command}", "command");
			}
			Command = command;
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.ClimateZone
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public interface IRvCloudDeviceComponentCommandClimateZone : IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		LogicalDeviceClimateZoneCommand Command { get; }
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandClimateZone : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandClimateZone, LogicalDeviceClimateZoneCommand>, IRvCloudDeviceComponentCommandClimateZone, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public ClimateZoneHeatMode HeatMode => Command.HeatMode;

		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public ClimateZoneHeatSource HeatSource => Command.HeatSource;

		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public ClimateZoneFanMode FanMode => Command.FanMode;

		[JsonProperty]
		public byte LowTripTemperatureFahrenheit => Command.LowTripTemperatureFahrenheit;

		[JsonProperty]
		public byte HighTripTemperatureFahrenheit => Command.HighTripTemperatureFahrenheit;

		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceClimateZoneCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandClimateZone(string componentId, ClimateZoneHeatMode heatMode, ClimateZoneHeatSource heatSource, ClimateZoneFanMode fanMode, byte lowTripTemperatureFahrenheit, byte highTripTemperatureFahrenheit)
			: this(componentId, new LogicalDeviceClimateZoneCommand(heatMode, heatSource, fanMode, lowTripTemperatureFahrenheit, highTripTemperatureFahrenheit))
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown


		public RvCloudDeviceComponentCommandClimateZone(string componentId, LogicalDeviceClimateZoneCommand command)
			: base(componentId)
		{
			Command = command;
		}
	}
}
namespace OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.AccessoryGateway
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public interface IRvCloudDeviceComponentCommandAccessoryGateway : IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		LogicalDeviceAccessoryGatewayCommand Command { get; }
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandAccessoryGatewayClearDevices : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandAccessoryGatewayClearDevices, LogicalDeviceAccessoryGatewayCommand>, IRvCloudDeviceComponentCommandAccessoryGateway, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceAccessoryGatewayCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public LogicalDeviceAccessoryGatewayCommandClearDevices CommandClearDevices
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandAccessoryGatewayClearDevices(string componentId)
			: this(componentId, new LogicalDeviceAccessoryGatewayCommandClearDevices())
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown


		public RvCloudDeviceComponentCommandAccessoryGatewayClearDevices(string componentId, LogicalDeviceAccessoryGatewayCommandClearDevices command)
			: base(componentId)
		{
			Command = (LogicalDeviceAccessoryGatewayCommand)(object)command;
			CommandClearDevices = command;
		}
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandAccessoryGatewayLinkDevice : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandAccessoryGatewayLinkDevice, LogicalDeviceAccessoryGatewayCommand>, IRvCloudDeviceComponentCommandAccessoryGateway, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		[JsonConverter(typeof(MacJsonHexStringConverter))]
		public MAC Mac => CommandLinkDevice.Mac;

		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceAccessoryGatewayCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public LogicalDeviceAccessoryGatewayCommandLinkDevice CommandLinkDevice
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandAccessoryGatewayLinkDevice(string componentId, MAC mac)
			: this(componentId, new LogicalDeviceAccessoryGatewayCommandLinkDevice(mac))
		{
		}//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown


		public RvCloudDeviceComponentCommandAccessoryGatewayLinkDevice(string componentId, LogicalDeviceAccessoryGatewayCommandLinkDevice command)
			: base(componentId)
		{
			Command = (LogicalDeviceAccessoryGatewayCommand)(object)command;
			CommandLinkDevice = command;
		}
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandAccessoryGatewayRebootIntoFlashLoader : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandAccessoryGatewayRebootIntoFlashLoader, LogicalDeviceAccessoryGatewayCommand>, IRvCloudDeviceComponentCommandAccessoryGateway, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceAccessoryGatewayCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public LogicalDeviceAccessoryGatewayCommandRebootIntoFlashLoader CommandRebootIntoFlashLoader
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandAccessoryGatewayRebootIntoFlashLoader(string componentId)
			: this(componentId, new LogicalDeviceAccessoryGatewayCommandRebootIntoFlashLoader())
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown


		public RvCloudDeviceComponentCommandAccessoryGatewayRebootIntoFlashLoader(string componentId, LogicalDeviceAccessoryGatewayCommandRebootIntoFlashLoader command)
			: base(componentId)
		{
			Command = (LogicalDeviceAccessoryGatewayCommand)(object)command;
			CommandRebootIntoFlashLoader = command;
		}
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandAccessoryGatewaySetAutoLinkMode : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandAccessoryGatewaySetAutoLinkMode, LogicalDeviceAccessoryGatewayCommand>, IRvCloudDeviceComponentCommandAccessoryGateway, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		public bool IsLinkModeActive => CommandSetAutoLinkMode.IsLinkModeActive;

		[JsonProperty]
		[JsonConverter(typeof(DEVICE_TYPE_JsonConverter))]
		public DEVICE_TYPE DeviceType => CommandSetAutoLinkMode.DeviceType;

		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceAccessoryGatewayCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public LogicalDeviceAccessoryGatewayCommandSetAutoLinkMode CommandSetAutoLinkMode
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandAccessoryGatewaySetAutoLinkMode(string componentId, bool isLinkModeActive, DEVICE_TYPE deviceType)
			: this(componentId, new LogicalDeviceAccessoryGatewayCommandSetAutoLinkMode(isLinkModeActive, deviceType))
		{
		}//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown


		public RvCloudDeviceComponentCommandAccessoryGatewaySetAutoLinkMode(string componentId, LogicalDeviceAccessoryGatewayCommandSetAutoLinkMode command)
			: base(componentId)
		{
			Command = (LogicalDeviceAccessoryGatewayCommand)(object)command;
			CommandSetAutoLinkMode = command;
		}
	}
	[ComponentSerializationSchemaCommand(/*Could not decode attribute arguments.*/)]
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvCloudDeviceComponentCommandAccessoryGatewayUnlinkDevice : RvCloudDeviceComponentCommand<RvCloudDeviceComponentCommandAccessoryGatewayUnlinkDevice, LogicalDeviceAccessoryGatewayCommand>, IRvCloudDeviceComponentCommandAccessoryGateway, IDeviceComponentCommandSerializable, IDeviceComponentCommand, IDeviceComponent, IJsonSerializerClass
	{
		[JsonProperty]
		[JsonConverter(typeof(MacJsonHexStringConverter))]
		public MAC Mac => CommandLinkDevice.Mac;

		[JsonIgnore]
		[field: CompilerGenerated]
		public override LogicalDeviceAccessoryGatewayCommand Command
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public LogicalDeviceAccessoryGatewayCommandUnLinkDevice CommandLinkDevice
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		public RvCloudDeviceComponentCommandAccessoryGatewayUnlinkDevice(string componentId, MAC mac)
			: this(componentId, new LogicalDeviceAccessoryGatewayCommandUnLinkDevice(mac))
		{
		}//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown


		public RvCloudDeviceComponentCommandAccessoryGatewayUnlinkDevice(string componentId, LogicalDeviceAccessoryGatewayCommandUnLinkDevice command)
			: base(componentId)
		{
			Command = (LogicalDeviceAccessoryGatewayCommand)(object)command;
			CommandLinkDevice = command;
		}
	}
}
