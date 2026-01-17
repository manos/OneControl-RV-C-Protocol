using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IDS.Core.IDS_CAN;
using IDS.Core.Types;
using IDS.Portable.Cloud;
using IDS.Portable.Cloud.Authentication;
using IDS.Portable.Cloud.Salesforce.DataObjects;
using IDS.Portable.Common;
using IDS.Portable.LogicalDevice;
using IDS.Portable.LogicalDevice.LogicalDeviceEx.Reactive;
using IDS.Portable.LogicalDevice.LogicalDeviceSource;
using Lumberjack.Cloud.Api.Client;
using Lumberjack.Cloud.Api.Models.Vehicle;
using Lumberjack.Cloud.Api.Models.Vehicle.Commands;
using Lumberjack.Cloud.Api.Models.Vehicle.Components.Abstractions;
using Lumberjack.Components.Abstractions;
using Lumberjack.Devices.Delegate;
using Lumberjack.Devices.Delegate.Extensions.Microsoft.DependencyInjection;
using Lumberjack.Devices.Messaging.Reception.Abstractions;
using Lumberjack.Serialization.Converters;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneControl.Devices;
using OneControl.Devices.LightRgb;
using OneControl.Direct.RvCloudIoT.Component;
using OneControl.Direct.RvCloudIoT.LogicalDeviceSession;
using OneControl.Direct.RvCloudIot.Component;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.ClimateZone;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.GeneratorGenie;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.LightDimmable;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.LightRgb;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.Metadata;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.PidRead;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.PidWrite;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.RelayBasicType1;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.RelayBasicType2;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentCommands.Rename;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentResponse;
using OneControl.Direct.RvCloudIot.Component.DeviceComponentStatus;
using Polly.Registry;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: TargetFramework(".NETCoreApp,Version=v8.0", FrameworkDisplayName = ".NET 8.0")]
[assembly: AssemblyCompany("Tod Cunningham")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("2022 Lippert Components")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("0.1.0+276.Branch.master.Sha.5398a039984c067bfdaa2155542b1327e982df33.5398a039984c067bfdaa2155542b1327e982df33")]
[assembly: AssemblyProduct("OneControl.Direct.RvCloudIoT")]
[assembly: AssemblyTitle("OneControl.Direct.RvCloudIoT")]
[assembly: AssemblyVersion("1.0.0.0")]
[module: RefSafetyRules(11)]
namespace OneControl.Direct.RvCloudIoT
{
	public interface IDirectConnectionRvCloudIoT : ILogicalDeviceSourceDirectConnectionRvCloudIoT, ILogicalDeviceSourceCloudConnection, ILogicalDeviceSourceDirectConnection, ILogicalDeviceSourceDirect, ILogicalDeviceSource, ILogicalDeviceSourceConnection
	{
		string LogPrefix { get; }
	}
	public class DirectConnectionRvCloudIoT : BackgroundOperation, IDirectConnectionRvCloudIoT, ILogicalDeviceSourceDirectConnectionRvCloudIoT, ILogicalDeviceSourceCloudConnection, ILogicalDeviceSourceDirectConnection, ILogicalDeviceSourceDirect, ILogicalDeviceSource, ILogicalDeviceSourceConnection, IAuthTokenProvider, IDirectCommandClimateZone, ILogicalDeviceSourceDirectMetadata, ILogicalDeviceSourceDirectPid, IDirectCommandGeneratorGenie, IDirectCommandLightDimmable, IDirectCommandLightRgb, ILogicalDeviceSourceDirectRename, ICloudToDeviceMessageHandler, IDirectCommandSwitch
	{
		internal class MetadataTracker
		{
			public record struct Key(string ComponentId)
			{
				[CompilerGenerated]
				public override readonly string ToString()
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					//IL_0006: Expected O, but got Unknown
					StringBuilder val = new StringBuilder();
					val.Append("Key");
					val.Append(" { ");
					if (PrintMembers(val))
					{
						val.Append(' ');
					}
					val.Append('}');
					return ((object)val).ToString();
				}

				[CompilerGenerated]
				private readonly bool PrintMembers(StringBuilder builder)
				{
					builder.Append("ComponentId = ");
					builder.Append((object)ComponentId);
					return true;
				}
			}

			private readonly TaskCompletionSource<RvCloudDeviceComponentResponseMetadata> _taskCompletionSource = new TaskCompletionSource<RvCloudDeviceComponentResponseMetadata>();

			public static TimeSpan RequestTimeout => TimeSpan.FromSeconds(10.0);

			public static Key MakeKey(string componentId)
			{
				return new Key(componentId);
			}

			public global::System.Threading.Tasks.Task<RvCloudDeviceComponentResponseMetadata?> WaitForResultAsync(CancellationToken cancellationToken)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				TaskCompletionSource<RvCloudDeviceComponentResponseMetadata> taskCompletionSource = _taskCompletionSource;
				TimeSpan requestTimeout = RequestTimeout;
				return TaskCompletionSourceExtension.TryWaitAsync<RvCloudDeviceComponentResponseMetadata>(taskCompletionSource, cancellationToken, (int)((TimeSpan)(ref requestTimeout)).TotalMilliseconds, true, (RvCloudDeviceComponentResponseMetadata)null);
			}

			public void TrySetResult(RvCloudDeviceComponentResponseMetadata value)
			{
				_taskCompletionSource.TrySetResult(value);
			}

			public void TrySetException(global::System.Exception ex)
			{
				_taskCompletionSource.TrySetException(ex);
			}
		}

		internal class PidReadTracker
		{
			public record struct Key(string ComponentId, Pid Pid)
			{
				[CompilerGenerated]
				public override readonly string ToString()
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					//IL_0006: Expected O, but got Unknown
					StringBuilder val = new StringBuilder();
					val.Append("Key");
					val.Append(" { ");
					if (PrintMembers(val))
					{
						val.Append(' ');
					}
					val.Append('}');
					return ((object)val).ToString();
				}

				[CompilerGenerated]
				private readonly bool PrintMembers(StringBuilder builder)
				{
					//IL_0027: Unknown result type (might be due to invalid IL or missing references)
					//IL_002c: Unknown result type (might be due to invalid IL or missing references)
					builder.Append("ComponentId = ");
					builder.Append((object)ComponentId);
					builder.Append(", Pid = ");
					builder.Append(((object)Pid/*cast due to .constrained prefix*/).ToString());
					return true;
				}

				[CompilerGenerated]
				public readonly void Deconstruct(out string ComponentId, out Pid Pid)
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Expected I4, but got Unknown
					ComponentId = this.ComponentId;
					Pid = (Pid)(int)this.Pid;
				}
			}

			private readonly TaskCompletionSource<UInt48> _pidValueTaskCompletionSource = new TaskCompletionSource<UInt48>();

			public static TimeSpan RequestTimeout => TimeSpan.FromSeconds(10.0);

			public static Key MakeKey(string componentId, Pid pid)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return new Key(componentId, pid);
			}

			public global::System.Threading.Tasks.Task<UInt48> WaitForResultAsync(CancellationToken cancellationToken)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				TaskCompletionSource<UInt48> pidValueTaskCompletionSource = _pidValueTaskCompletionSource;
				TimeSpan requestTimeout = RequestTimeout;
				return TaskCompletionSourceExtension.TryWaitAsync<UInt48>(pidValueTaskCompletionSource, cancellationToken, (int)((TimeSpan)(ref requestTimeout)).TotalMilliseconds, true, default(UInt48));
			}

			public void TrySetResult(UInt48 value)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_pidValueTaskCompletionSource.TrySetResult(value);
			}

			public void TrySetException(global::System.Exception ex)
			{
				_pidValueTaskCompletionSource.TrySetException(ex);
			}
		}

		internal class PidIndexReadTracker
		{
			public record struct Key(string ComponentId, Pid Pid, ushort Address)
			{
				[CompilerGenerated]
				public override readonly string ToString()
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					//IL_0006: Expected O, but got Unknown
					StringBuilder val = new StringBuilder();
					val.Append("Key");
					val.Append(" { ");
					if (PrintMembers(val))
					{
						val.Append(' ');
					}
					val.Append('}');
					return ((object)val).ToString();
				}

				[CompilerGenerated]
				private readonly bool PrintMembers(StringBuilder builder)
				{
					//IL_0027: Unknown result type (might be due to invalid IL or missing references)
					//IL_002c: Unknown result type (might be due to invalid IL or missing references)
					builder.Append("ComponentId = ");
					builder.Append((object)ComponentId);
					builder.Append(", Pid = ");
					builder.Append(((object)Pid/*cast due to .constrained prefix*/).ToString());
					builder.Append(", Address = ");
					builder.Append(((object)Address/*cast due to .constrained prefix*/).ToString());
					return true;
				}

				[CompilerGenerated]
				public readonly void Deconstruct(out string ComponentId, out Pid Pid, out ushort Address)
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Expected I4, but got Unknown
					ComponentId = this.ComponentId;
					Pid = (Pid)(int)this.Pid;
					Address = this.Address;
				}
			}

			private readonly TaskCompletionSource<uint> _pidValueTaskCompletionSource = new TaskCompletionSource<uint>();

			public static TimeSpan RequestTimeout => TimeSpan.FromSeconds(10.0);

			public static Key MakeKey(string componentId, Pid pid, ushort index)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return new Key(componentId, pid, index);
			}

			public global::System.Threading.Tasks.Task<uint> WaitForResultAsync(CancellationToken cancellationToken)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				TaskCompletionSource<uint> pidValueTaskCompletionSource = _pidValueTaskCompletionSource;
				TimeSpan requestTimeout = RequestTimeout;
				return TaskCompletionSourceExtension.TryWaitAsync<uint>(pidValueTaskCompletionSource, cancellationToken, (int)((TimeSpan)(ref requestTimeout)).TotalMilliseconds, true, 0u);
			}

			public void TrySetResult(uint value)
			{
				_pidValueTaskCompletionSource.TrySetResult(value);
			}

			public void TrySetException(global::System.Exception ex)
			{
				_pidValueTaskCompletionSource.TrySetException(ex);
			}
		}

		[CompilerGenerated]
		private sealed class <>c__DisplayClass61_0
		{
			public IDelegateDeviceClient deviceClient;

			public CancellationToken cancellationToken;

			internal void <DebugSetupReportingOfStatusChanges>b__0(ILogicalDeviceWithStatus device)
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				IDeviceComponent val = RvCloudDeviceComponentStatusFactory.MakeDeviceComponent(device);
				if (val != null)
				{
					deviceClient.ReportComponentStatusAsync(val, cancellationToken).RunSynchronously();
				}
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <BackgroundOperationAsync>d__59 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DirectConnectionRvCloudIoT <>4__this;

			public CancellationToken cancellationToken;

			private IDelegateDeviceClient <deviceClient>5__2;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0186: Unknown result type (might be due to invalid IL or missing references)
				//IL_018b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0193: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_030d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0312: Unknown result type (might be due to invalid IL or missing references)
				//IL_031a: Unknown result type (might be due to invalid IL or missing references)
				//IL_037a: Unknown result type (might be due to invalid IL or missing references)
				//IL_037f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0387: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0129: Unknown result type (might be due to invalid IL or missing references)
				//IL_012e: Unknown result type (might be due to invalid IL or missing references)
				//IL_013a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0147: Unknown result type (might be due to invalid IL or missing references)
				//IL_0151: Expected O, but got Unknown
				//IL_0151: Unknown result type (might be due to invalid IL or missing references)
				//IL_0156: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_016b: Unknown result type (might be due to invalid IL or missing references)
				//IL_016d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0238: Unknown result type (might be due to invalid IL or missing references)
				//IL_023d: Unknown result type (might be due to invalid IL or missing references)
				//IL_023f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0256: Expected I4, but got Unknown
				//IL_0261: Unknown result type (might be due to invalid IL or missing references)
				//IL_026b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0270: Unknown result type (might be due to invalid IL or missing references)
				//IL_033b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0345: Unknown result type (might be due to invalid IL or missing references)
				//IL_034a: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_0285: Unknown result type (might be due to invalid IL or missing references)
				//IL_0287: Unknown result type (might be due to invalid IL or missing references)
				//IL_035f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0361: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				try
				{
					if ((uint)num > 4u)
					{
						TaggedLog.Information("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix} Starting Service for {directConnectionRvCloudIoT.CloudCredential}", global::System.Array.Empty<object>());
						<deviceClient>5__2 = null;
					}
					try
					{
						TaskAwaiter val;
						switch (num)
						{
						default:
						{
							directConnectionRvCloudIoT._connectionStatus = (ConnectionStatus)1;
							ServiceProvider val2 = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider((IServiceCollection)(object)directConnectionRvCloudIoT._serviceCollection);
							<deviceClient>5__2 = ServiceProviderServiceExtensions.GetService<IDelegateDeviceClient>((IServiceProvider)(object)val2) ?? throw new global::System.Exception();
							<deviceClient>5__2.ClientConnectionStatusChanged += directConnectionRvCloudIoT.LumberjackConnectionChanged;
							directConnectionRvCloudIoT.ApiProvider = ServiceProviderServiceExtensions.GetRequiredService<ILumberjackDeviceApi>((IServiceProvider)(object)val2);
							TaggedLog.Information("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix}: Starting Device Client UCI={directConnectionRvCloudIoT._uniqueVehicleIdentifier} and UUID={directConnectionRvCloudIoT._phoneUuid}", global::System.Array.Empty<object>());
							val = <deviceClient>5__2.StartAsync(new DelegateStartOptions
							{
								VehicleIdentifier = directConnectionRvCloudIoT._uniqueVehicleIdentifier,
								DeviceIdentifier = directConnectionRvCloudIoT._phoneUuid
							}, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <BackgroundOperationAsync>d__59>(ref val, ref this);
								return;
							}
							goto IL_01a2;
						}
						case 0:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_01a2;
						case 1:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_020c;
						case 2:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_02bc;
						case 3:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_0329;
						case 4:
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_0396;
							}
							IL_02bc:
							((TaskAwaiter)(ref val)).GetResult();
							goto IL_03ce;
							IL_0335:
							val = global::System.Threading.Tasks.Task.Delay(10000, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 4);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <BackgroundOperationAsync>d__59>(ref val, ref this);
								return;
							}
							goto IL_0396;
							IL_0329:
							((TaskAwaiter)(ref val)).GetResult();
							goto IL_03ce;
							IL_01a2:
							((TaskAwaiter)(ref val)).GetResult();
							val = directConnectionRvCloudIoT.GetAllDeviceComponentsAsync(directConnectionRvCloudIoT.ApiProvider, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <BackgroundOperationAsync>d__59>(ref val, ref this);
								return;
							}
							goto IL_020c;
							IL_0396:
							((TaskAwaiter)(ref val)).GetResult();
							goto IL_03ce;
							IL_020c:
							((TaskAwaiter)(ref val)).GetResult();
							TaggedLog.Information("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " - Monitoring Connection status", global::System.Array.Empty<object>());
							goto IL_03ce;
							IL_03ce:
							if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
							{
								ConnectionStatus connectionStatus = directConnectionRvCloudIoT._connectionStatus;
								switch ((int)connectionStatus)
								{
								case 0:
									break;
								case 2:
									goto IL_02c8;
								case 1:
									goto IL_0335;
								default:
									throw new global::System.Exception($"Unable to recover from connection status {directConnectionRvCloudIoT._connectionStatus}");
								}
								val = global::System.Threading.Tasks.Task.Delay(10000, cancellationToken).GetAwaiter();
								if (!((TaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 2);
									<>u__1 = val;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <BackgroundOperationAsync>d__59>(ref val, ref this);
									return;
								}
								goto IL_02bc;
							}
							TaggedLog.Information("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " - Monitoring Connection Stopped", global::System.Array.Empty<object>());
							break;
							IL_02c8:
							val = global::System.Threading.Tasks.Task.Delay(10000, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 3);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <BackgroundOperationAsync>d__59>(ref val, ref this);
								return;
							}
							goto IL_0329;
						}
					}
					catch (global::System.Exception ex)
					{
						TaggedLog.Error("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Service failed " + ex.Message, global::System.Array.Empty<object>());
						global::System.Exception innerException = ex.InnerException;
						if (innerException != null)
						{
							TaggedLog.Error("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix} Service failed details {innerException.Message}\n{innerException.StackTrace}", global::System.Array.Empty<object>());
						}
					}
					finally
					{
						if (num < 0)
						{
							TaggedLog.Information("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix} Stopping Service for {directConnectionRvCloudIoT._globalDeviceEndpoint} {directConnectionRvCloudIoT._idScope} {directConnectionRvCloudIoT._apiBaseAddress}", global::System.Array.Empty<object>());
							try
							{
								IDelegateDeviceClient obj = <deviceClient>5__2;
								if (obj != null)
								{
									obj.Stop();
								}
							}
							catch
							{
							}
							directConnectionRvCloudIoT.ApiProvider = null;
							directConnectionRvCloudIoT.DidDisconnectEvent?.Invoke((ILogicalDeviceSourceDirectConnection)(object)directConnectionRvCloudIoT);
							UpdateDeviceSourceReachabilityEventHandler? obj3 = directConnectionRvCloudIoT.UpdateDeviceSourceReachabilityEvent;
							if (obj3 != null)
							{
								obj3.Invoke((ILogicalDeviceSourceDirect)(object)directConnectionRvCloudIoT);
							}
						}
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<deviceClient>5__2 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<deviceClient>5__2 = null;
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
		private struct <DebugSetupReportingOfStatusChanges>d__61 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public IDelegateDeviceClient deviceClient;

			public CancellationToken cancellationToken;

			private global::System.IDisposable <subscription>5__2;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0095: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				try
				{
					<>c__DisplayClass61_0 <>c__DisplayClass61_ = default(<>c__DisplayClass61_0);
					if (num != 0)
					{
						<>c__DisplayClass61_ = new <>c__DisplayClass61_0
						{
							deviceClient = deviceClient,
							cancellationToken = cancellationToken
						};
						<subscription>5__2 = ObservableExtensions.Subscribe<ILogicalDeviceWithStatus>(Observable.OfType<ILogicalDeviceWithStatus>((IObservable<object>)(object)LogicalDeviceExReactiveStatusChanged.SharedExtension), (Action<ILogicalDeviceWithStatus>)<>c__DisplayClass61_.<DebugSetupReportingOfStatusChanges>b__0);
					}
					try
					{
						TaskAwaiter<bool> val;
						if (num != 0)
						{
							val = TaskExtension.TryDelay(-1, <>c__DisplayClass61_.cancellationToken).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <DebugSetupReportingOfStatusChanges>d__61>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<bool>);
							num = (<>1__state = -1);
						}
						val.GetResult();
					}
					finally
					{
						if (num < 0 && <subscription>5__2 != null)
						{
							<subscription>5__2.Dispose();
						}
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<subscription>5__2 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<subscription>5__2 = null;
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
		private struct <GetAllDeviceComponentsAsync>d__60 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DirectConnectionRvCloudIoT <>4__this;

			public ILumberjackDeviceApi apiProvider;

			public CancellationToken cancellationToken;

			private TaskAwaiter<VehicleView> <>u__1;

			private void MoveNext()
			{
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				try
				{
					TaskAwaiter<VehicleView> val;
					if (num != 0)
					{
						TaggedLog.Debug("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " - Found Components for " + directConnectionRvCloudIoT._uniqueVehicleIdentifier, global::System.Array.Empty<object>());
						val = apiProvider.GetVehicleViewAsync(directConnectionRvCloudIoT._uniqueVehicleIdentifier, cancellationToken).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<VehicleView>, <GetAllDeviceComponentsAsync>d__60>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<VehicleView>);
						num = (<>1__state = -1);
					}
					Enumerator<IVehicleViewComponent> enumerator = val.GetResult().Components.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							IVehicleViewComponent current = enumerator.Current;
							if (current != null)
							{
								IDeviceComponent deviceComponent = current.DeviceComponent;
								RvCloudDeviceComponentStatus val2 = (RvCloudDeviceComponentStatus)(object)((deviceComponent is RvCloudDeviceComponentStatus) ? deviceComponent : null);
								if (val2 != null)
								{
									TaggedLog.Debug("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix} - Found Device - {current.Name} - {current.FunctionClass} - {current.Identifier} with Component Status {val2}", global::System.Array.Empty<object>());
									directConnectionRvCloudIoT.ApplyComponentStatus(val2);
								}
								else
								{
									TaggedLog.Debug("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix} - Found Device - {current.Name} - {current.FunctionClass} - {current.Identifier} with Component Status UNKNOWN", global::System.Array.Empty<object>());
								}
							}
							else
							{
								TaggedLog.Debug("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix} - Found Device - {current.Name} - {current.FunctionClass} - {current.Identifier}", global::System.Array.Empty<object>());
							}
						}
					}
					finally
					{
						if (num < 0)
						{
							((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
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
		private struct <GetSoftwarePartNumberAsync>d__70 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<string> <>t__builder;

			public DirectConnectionRvCloudIoT <>4__this;

			public ILogicalDevice logicalDevice;

			public CancellationToken cancelToken;

			private MetadataTracker.Key <commandKey>5__2;

			private MetadataTracker <tracker>5__3;

			private TaskAwaiter <>u__1;

			private ConfiguredTaskAwaiter<RvCloudDeviceComponentResponseMetadata?> <>u__2;

			private void MoveNext()
			{
				//IL_0171: Expected O, but got Unknown
				//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ac: Expected O, but got Unknown
				//IL_01df: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Expected O, but got Unknown
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0133: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				//IL_0140: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
				//IL_0103: Unknown result type (might be due to invalid IL or missing references)
				//IL_0081: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_0118: Unknown result type (might be due to invalid IL or missing references)
				//IL_011a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				string softwarePartNumber;
				try
				{
					ILumberjackDeviceApi apiProviderForOnlineDevicePidOperations = default(ILumberjackDeviceApi);
					RvCloudDeviceComponentCommandMetadataGet componentCommand = default(RvCloudDeviceComponentCommandMetadataGet);
					MetadataTracker metadataTracker = default(MetadataTracker);
					if ((uint)num > 1u)
					{
						apiProviderForOnlineDevicePidOperations = directConnectionRvCloudIoT.GetApiProviderForOnlineDevicePidOperations(logicalDevice);
						string text = ComponentIdEx.MakeComponentId(logicalDevice.LogicalId);
						componentCommand = new RvCloudDeviceComponentCommandMetadataGet(text);
						<commandKey>5__2 = MetadataTracker.MakeKey(text);
						metadataTracker = new MetadataTracker();
						<tracker>5__3 = directConnectionRvCloudIoT._metadataTrackers.GetOrAdd(<commandKey>5__2, metadataTracker);
					}
					try
					{
						ConfiguredTaskAwaiter<RvCloudDeviceComponentResponseMetadata> val;
						TaskAwaiter val2;
						if (num != 0)
						{
							if (num == 1)
							{
								val = <>u__2;
								<>u__2 = default(ConfiguredTaskAwaiter<RvCloudDeviceComponentResponseMetadata>);
								num = (<>1__state = -1);
								goto IL_014f;
							}
							if (<tracker>5__3 != metadataTracker)
							{
								goto IL_00e3;
							}
							val2 = directConnectionRvCloudIoT.SendCommandAsync(apiProviderForOnlineDevicePidOperations, (RvCloudDeviceComponentCommand)(object)componentCommand, cancelToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val2)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val2;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <GetSoftwarePartNumberAsync>d__70>(ref val2, ref this);
								return;
							}
						}
						else
						{
							val2 = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
						}
						((TaskAwaiter)(ref val2)).GetResult();
						goto IL_00e3;
						IL_014f:
						softwarePartNumber = (val.GetResult() ?? throw new global::System.Exception("Unable to get metadata")).SoftwarePartNumber;
						goto end_IL_0068;
						IL_00e3:
						val = <tracker>5__3.WaitForResultAsync(cancelToken).ConfigureAwait(false).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__2 = val;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<RvCloudDeviceComponentResponseMetadata>, <GetSoftwarePartNumberAsync>d__70>(ref val, ref this);
							return;
						}
						goto IL_014f;
						end_IL_0068:;
					}
					catch (OperationCanceledException ex)
					{
						OperationCanceledException ex2 = ex;
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because it was canceled: " + ((global::System.Exception)(object)ex2).Message, global::System.Array.Empty<object>());
						<tracker>5__3.TrySetException((global::System.Exception)(object)ex2);
						throw new LogicalDevicePidCanceledException();
					}
					catch (TimeoutException ex3)
					{
						TimeoutException ex4 = ex3;
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of timeout: " + ((global::System.Exception)(object)ex4).Message, global::System.Array.Empty<object>());
						<tracker>5__3.TrySetException((global::System.Exception)(object)ex4);
						throw new LogicalDevicePidTimeoutException();
					}
					catch (global::System.Exception ex5)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of " + ex5.Message, global::System.Array.Empty<object>());
						<tracker>5__3.TrySetException(ex5);
						throw;
					}
					finally
					{
						if (num < 0)
						{
							DictionaryExtension.TryRemove<MetadataTracker.Key, MetadataTracker>((IDictionary<MetadataTracker.Key, MetadataTracker>)(object)directConnectionRvCloudIoT._metadataTrackers, <commandKey>5__2);
						}
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<commandKey>5__2 = default(MetadataTracker.Key);
					<tracker>5__3 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<commandKey>5__2 = default(MetadataTracker.Key);
				<tracker>5__3 = null;
				<>t__builder.SetResult(softwarePartNumber);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <HandleMessageAsync>d__87 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<CloudToDeviceMessageHandlerResult> <>t__builder;

			public CloudToDeviceMessage message;

			public DirectConnectionRvCloudIoT <>4__this;

			private void MoveNext()
			{
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fd: Expected O, but got Unknown
				//IL_0112: Unknown result type (might be due to invalid IL or missing references)
				//IL_011c: Expected O, but got Unknown
				//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_029e: Unknown result type (might be due to invalid IL or missing references)
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				CloudToDeviceMessageHandlerResult result;
				try
				{
					try
					{
						string text = default(string);
						if (message.MessageSchema == "Lumberjack.MessageSchema.DeviceConnection" && message.Properties.TryGetValue("ConnectionState", ref text))
						{
							if (!(text == "deviceDisconnected"))
							{
								if (text == "deviceConnected")
								{
									TaggedLog.Information("DirectConnectionRvCloudIoT", "deviceConnected message received.", global::System.Array.Empty<object>());
									directConnectionRvCloudIoT.DidConnectEvent?.Invoke((ILogicalDeviceSourceDirectConnection)(object)directConnectionRvCloudIoT);
									result = CloudToDeviceMessageHandlerResult.Complete;
								}
								else
								{
									result = CloudToDeviceMessageHandlerResult.Reject;
								}
							}
							else
							{
								TaggedLog.Information("DirectConnectionRvCloudIoT", "Taking devices offline.", global::System.Array.Empty<object>());
								directConnectionRvCloudIoT._logicalDevicesOnlineManager.AutoTakeDevicesOffline(forceAllOffline: true);
								directConnectionRvCloudIoT.DidDisconnectEvent?.Invoke((ILogicalDeviceSourceDirectConnection)(object)directConnectionRvCloudIoT);
								result = CloudToDeviceMessageHandlerResult.Complete;
							}
						}
						else if (message.MessageSchema != "Lumberjack.MessageSchema.ComponentInformation")
						{
							result = CloudToDeviceMessageHandlerResult.Reject;
						}
						else
						{
							JsonSerializerSettings val = new JsonSerializerSettings();
							val.ContractResolver = (IContractResolver)new SchemaAwareContractResolver(new global::System.Type[1] { typeof(IDeviceComponent) });
							JsonSerializerSettings val2 = val;
							IDeviceComponent val3 = JsonConvert.DeserializeObject<IDeviceComponent>(message.Body, val2);
							RvCloudDeviceComponentStatus val4 = (RvCloudDeviceComponentStatus)(object)((val3 is RvCloudDeviceComponentStatus) ? val3 : null);
							if (val4 != null)
							{
								directConnectionRvCloudIoT.ApplyComponentStatus(val4);
								goto end_IL_0007;
							}
							RvCloudDeviceComponentResponsePidValue val5 = (RvCloudDeviceComponentResponsePidValue)(object)((val3 is RvCloudDeviceComponentResponsePidValue) ? val3 : null);
							if (val5 == null)
							{
								RvCloudDeviceComponentResponsePidValueIndex val6 = (RvCloudDeviceComponentResponsePidValueIndex)(object)((val3 is RvCloudDeviceComponentResponsePidValueIndex) ? val3 : null);
								if (val6 == null)
								{
									RvCloudDeviceComponentResponseMetadata val7 = (RvCloudDeviceComponentResponseMetadata)(object)((val3 is RvCloudDeviceComponentResponseMetadata) ? val3 : null);
									if (val7 == null)
									{
										throw new RvCloudIoTException("Unable to deserialize " + ((MemberInfo)((object)val3).GetType()).Name + ": " + message.MessageSchema);
									}
									TaggedLog.Information("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix} Received: {val7}", global::System.Array.Empty<object>());
									MetadataTracker.Key key = MetadataTracker.MakeKey(val7.ComponentId);
									MetadataTracker metadataTracker = default(MetadataTracker);
									if (directConnectionRvCloudIoT._metadataTrackers.TryGetValue(key, ref metadataTracker))
									{
										metadataTracker.TrySetResult(val7);
										DictionaryExtension.TryRemove<MetadataTracker.Key, MetadataTracker>((IDictionary<MetadataTracker.Key, MetadataTracker>)(object)directConnectionRvCloudIoT._metadataTrackers, key);
										TaggedLog.Information("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix} Applied: {val7}", global::System.Array.Empty<object>());
										goto end_IL_0007;
									}
									result = CloudToDeviceMessageHandlerResult.Abandon;
								}
								else
								{
									TaggedLog.Information("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix} Received: {val6}", global::System.Array.Empty<object>());
									PidIndexReadTracker.Key key2 = PidIndexReadTracker.MakeKey(val6.ComponentId, val6.Pid, val6.Address);
									PidIndexReadTracker pidIndexReadTracker = default(PidIndexReadTracker);
									if (directConnectionRvCloudIoT._pidIndexReadTrackers.TryGetValue(key2, ref pidIndexReadTracker))
									{
										pidIndexReadTracker.TrySetResult((uint)val6.Value);
										DictionaryExtension.TryRemove<PidIndexReadTracker.Key, PidIndexReadTracker>((IDictionary<PidIndexReadTracker.Key, PidIndexReadTracker>)(object)directConnectionRvCloudIoT._pidIndexReadTrackers, key2);
										TaggedLog.Information("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix} Applied: {val6}", global::System.Array.Empty<object>());
										goto end_IL_0007;
									}
									result = CloudToDeviceMessageHandlerResult.Abandon;
								}
							}
							else
							{
								TaggedLog.Information("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix} Received: {val5}", global::System.Array.Empty<object>());
								PidReadTracker.Key key3 = PidReadTracker.MakeKey(val5.ComponentId, val5.Pid);
								PidReadTracker pidReadTracker = default(PidReadTracker);
								if (directConnectionRvCloudIoT._pidReadTrackers.TryGetValue(key3, ref pidReadTracker))
								{
									pidReadTracker.TrySetResult((UInt48)val5.Value);
									DictionaryExtension.TryRemove<PidReadTracker.Key, PidReadTracker>((IDictionary<PidReadTracker.Key, PidReadTracker>)(object)directConnectionRvCloudIoT._pidReadTrackers, key3);
									TaggedLog.Information("DirectConnectionRvCloudIoT", $"{directConnectionRvCloudIoT.LogPrefix} Applied: {val5}", global::System.Array.Empty<object>());
									goto end_IL_0007;
								}
								result = CloudToDeviceMessageHandlerResult.Abandon;
							}
						}
						goto end_IL_0007_2;
						end_IL_0007:;
					}
					catch (global::System.Exception ex)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", $"Error processing message for {message.MessageSchema}: {message.Body} because {ex.Message}", global::System.Array.Empty<object>());
						result = CloudToDeviceMessageHandlerResult.Reject;
						goto end_IL_0007_2;
					}
					result = CloudToDeviceMessageHandlerResult.Complete;
					end_IL_0007_2:;
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
		private struct <PidReadAsync>d__74 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<UInt48> <>t__builder;

			public DirectConnectionRvCloudIoT <>4__this;

			public ILogicalDevice logicalDevice;

			public Pid pid;

			public CancellationToken cancellationToken;

			private PidReadTracker.Key <pidKey>5__2;

			private PidReadTracker <pidTracker>5__3;

			private TaskAwaiter <>u__1;

			private ConfiguredTaskAwaiter<UInt48> <>u__2;

			private void MoveNext()
			{
				//IL_016a: Expected O, but got Unknown
				//IL_0196: Unknown result type (might be due to invalid IL or missing references)
				//IL_019e: Expected O, but got Unknown
				//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_0206: Unknown result type (might be due to invalid IL or missing references)
				//IL_0218: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Expected O, but got Unknown
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_013f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0144: Unknown result type (might be due to invalid IL or missing references)
				//IL_014c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_0106: Unknown result type (might be due to invalid IL or missing references)
				//IL_010a: Unknown result type (might be due to invalid IL or missing references)
				//IL_010f: Unknown result type (might be due to invalid IL or missing references)
				//IL_015d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0162: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0124: Unknown result type (might be due to invalid IL or missing references)
				//IL_0126: Unknown result type (might be due to invalid IL or missing references)
				//IL_0281: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				UInt48 result;
				try
				{
					ILumberjackDeviceApi apiProviderForOnlineDevicePidOperations = default(ILumberjackDeviceApi);
					RvCloudDeviceComponentCommandPidRead componentCommand = default(RvCloudDeviceComponentCommandPidRead);
					PidReadTracker pidReadTracker = default(PidReadTracker);
					if ((uint)num > 1u)
					{
						apiProviderForOnlineDevicePidOperations = directConnectionRvCloudIoT.GetApiProviderForOnlineDevicePidOperations(logicalDevice);
						string text = ComponentIdEx.MakeComponentId(logicalDevice.LogicalId);
						componentCommand = new RvCloudDeviceComponentCommandPidRead(text, pid);
						<pidKey>5__2 = PidReadTracker.MakeKey(text, pid);
						pidReadTracker = new PidReadTracker();
						<pidTracker>5__3 = directConnectionRvCloudIoT._pidReadTrackers.GetOrAdd(<pidKey>5__2, pidReadTracker);
					}
					try
					{
						ConfiguredTaskAwaiter<UInt48> val;
						TaskAwaiter val2;
						if (num != 0)
						{
							if (num == 1)
							{
								val = <>u__2;
								<>u__2 = default(ConfiguredTaskAwaiter<UInt48>);
								num = (<>1__state = -1);
								goto IL_015b;
							}
							if (<pidTracker>5__3 != pidReadTracker)
							{
								goto IL_00ef;
							}
							val2 = directConnectionRvCloudIoT.SendCommandAsync(apiProviderForOnlineDevicePidOperations, (RvCloudDeviceComponentCommand)(object)componentCommand, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val2)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val2;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <PidReadAsync>d__74>(ref val2, ref this);
								return;
							}
						}
						else
						{
							val2 = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
						}
						((TaskAwaiter)(ref val2)).GetResult();
						goto IL_00ef;
						IL_015b:
						result = val.GetResult();
						goto end_IL_0074;
						IL_00ef:
						val = <pidTracker>5__3.WaitForResultAsync(cancellationToken).ConfigureAwait(false).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__2 = val;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<UInt48>, <PidReadAsync>d__74>(ref val, ref this);
							return;
						}
						goto IL_015b;
						end_IL_0074:;
					}
					catch (OperationCanceledException ex)
					{
						OperationCanceledException ex2 = ex;
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
						<pidTracker>5__3.TrySetException((global::System.Exception)(object)ex2);
						throw new LogicalDevicePidCanceledException();
					}
					catch (TimeoutException ex3)
					{
						TimeoutException ex4 = ex3;
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
						<pidTracker>5__3.TrySetException((global::System.Exception)(object)ex4);
						throw new LogicalDevicePidTimeoutException();
					}
					catch (global::System.Exception ex5)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of " + ex5.Message, global::System.Array.Empty<object>());
						<pidTracker>5__3.TrySetException(ex5);
						throw new LogicalDevicePidValueReadException(PidExtension.ConvertToPid(pid), logicalDevice, ex5);
					}
					finally
					{
						if (num < 0)
						{
							DictionaryExtension.TryRemove<PidReadTracker.Key, PidReadTracker>((IDictionary<PidReadTracker.Key, PidReadTracker>)(object)directConnectionRvCloudIoT._pidReadTrackers, <pidKey>5__2);
						}
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<pidKey>5__2 = default(PidReadTracker.Key);
					<pidTracker>5__3 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<pidKey>5__2 = default(PidReadTracker.Key);
				<pidTracker>5__3 = null;
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
		private struct <PidReadAsync>d__79 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<uint> <>t__builder;

			public DirectConnectionRvCloudIoT <>4__this;

			public ILogicalDevice logicalDevice;

			public Pid pid;

			public ushort address;

			public CancellationToken cancellationToken;

			private PidIndexReadTracker.Key <pidKey>5__2;

			private PidIndexReadTracker <pidTracker>5__3;

			private TaskAwaiter <>u__1;

			private ConfiguredTaskAwaiter<uint> <>u__2;

			private void MoveNext()
			{
				//IL_0176: Expected O, but got Unknown
				//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01aa: Expected O, but got Unknown
				//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0212: Unknown result type (might be due to invalid IL or missing references)
				//IL_0224: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Expected O, but got Unknown
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_014b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0150: Unknown result type (might be due to invalid IL or missing references)
				//IL_0158: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				//IL_010d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0112: Unknown result type (might be due to invalid IL or missing references)
				//IL_0116: Unknown result type (might be due to invalid IL or missing references)
				//IL_011b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0130: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				uint result;
				try
				{
					ILumberjackDeviceApi apiProviderForOnlineDevicePidOperations = default(ILumberjackDeviceApi);
					RvCloudDeviceComponentCommandPidReadWithAddress componentCommand = default(RvCloudDeviceComponentCommandPidReadWithAddress);
					PidIndexReadTracker pidIndexReadTracker = default(PidIndexReadTracker);
					if ((uint)num > 1u)
					{
						apiProviderForOnlineDevicePidOperations = directConnectionRvCloudIoT.GetApiProviderForOnlineDevicePidOperations(logicalDevice);
						string text = ComponentIdEx.MakeComponentId(logicalDevice.LogicalId);
						componentCommand = new RvCloudDeviceComponentCommandPidReadWithAddress(text, pid, address);
						<pidKey>5__2 = PidIndexReadTracker.MakeKey(text, pid, address);
						pidIndexReadTracker = new PidIndexReadTracker();
						<pidTracker>5__3 = directConnectionRvCloudIoT._pidIndexReadTrackers.GetOrAdd(<pidKey>5__2, pidIndexReadTracker);
					}
					try
					{
						ConfiguredTaskAwaiter<uint> val;
						TaskAwaiter val2;
						if (num != 0)
						{
							if (num == 1)
							{
								val = <>u__2;
								<>u__2 = default(ConfiguredTaskAwaiter<uint>);
								num = (<>1__state = -1);
								goto IL_0167;
							}
							if (<pidTracker>5__3 != pidIndexReadTracker)
							{
								goto IL_00fb;
							}
							val2 = directConnectionRvCloudIoT.SendCommandAsync(apiProviderForOnlineDevicePidOperations, (RvCloudDeviceComponentCommand)(object)componentCommand, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val2)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val2;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <PidReadAsync>d__79>(ref val2, ref this);
								return;
							}
						}
						else
						{
							val2 = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
						}
						((TaskAwaiter)(ref val2)).GetResult();
						goto IL_00fb;
						IL_0167:
						result = val.GetResult();
						goto end_IL_0080;
						IL_00fb:
						val = <pidTracker>5__3.WaitForResultAsync(cancellationToken).ConfigureAwait(false).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__2 = val;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<uint>, <PidReadAsync>d__79>(ref val, ref this);
							return;
						}
						goto IL_0167;
						end_IL_0080:;
					}
					catch (OperationCanceledException ex)
					{
						OperationCanceledException ex2 = ex;
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
						<pidTracker>5__3.TrySetException((global::System.Exception)(object)ex2);
						throw new LogicalDevicePidCanceledException();
					}
					catch (TimeoutException ex3)
					{
						TimeoutException ex4 = ex3;
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
						<pidTracker>5__3.TrySetException((global::System.Exception)(object)ex4);
						throw new LogicalDevicePidTimeoutException();
					}
					catch (global::System.Exception ex5)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of " + ex5.Message, global::System.Array.Empty<object>());
						<pidTracker>5__3.TrySetException(ex5);
						throw new LogicalDevicePidValueReadException(PidExtension.ConvertToPid(pid), logicalDevice, ex5);
					}
					finally
					{
						if (num < 0)
						{
							DictionaryExtension.TryRemove<PidIndexReadTracker.Key, PidIndexReadTracker>((IDictionary<PidIndexReadTracker.Key, PidIndexReadTracker>)(object)directConnectionRvCloudIoT._pidIndexReadTrackers, <pidKey>5__2);
						}
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<pidKey>5__2 = default(PidIndexReadTracker.Key);
					<pidTracker>5__3 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<pidKey>5__2 = default(PidIndexReadTracker.Key);
				<pidTracker>5__3 = null;
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
		private struct <PidWriteAsync>d__76 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DirectConnectionRvCloudIoT <>4__this;

			public ILogicalDevice logicalDevice;

			public Pid pid;

			public UInt48 value;

			public LogicalDeviceSessionType pidWriteAccess;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
				//IL_012a: Unknown result type (might be due to invalid IL or missing references)
				//IL_013b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0147: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Expected O, but got Unknown
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				try
				{
					ILumberjackDeviceApi apiProviderForOnlineDevicePidOperations = default(ILumberjackDeviceApi);
					if (num != 0)
					{
						apiProviderForOnlineDevicePidOperations = directConnectionRvCloudIoT.GetApiProviderForOnlineDevicePidOperations(logicalDevice);
					}
					try
					{
						TaskAwaiter val;
						if (num != 0)
						{
							RvCloudDeviceComponentCommandPidWrite componentCommand = new RvCloudDeviceComponentCommandPidWrite(ComponentIdEx.MakeComponentId(logicalDevice.LogicalId), pid, value, pidWriteAccess);
							val = directConnectionRvCloudIoT.SendCommandAsync(apiProviderForOnlineDevicePidOperations, (RvCloudDeviceComponentCommand)(object)componentCommand, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <PidWriteAsync>d__76>(ref val, ref this);
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
					catch (OperationCanceledException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
						throw new LogicalDevicePidCanceledException();
					}
					catch (TimeoutException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
						throw new LogicalDevicePidTimeoutException();
					}
					catch (global::System.Exception ex3)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of " + ex3.Message, global::System.Array.Empty<object>());
						throw new LogicalDevicePidValueWriteException(PidExtension.ConvertToPid(pid), logicalDevice, UInt48.op_Implicit(value), ex3);
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
		private struct <PidWriteAsync>d__80 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DirectConnectionRvCloudIoT <>4__this;

			public ILogicalDevice logicalDevice;

			public Pid pid;

			public ushort address;

			public uint value;

			public LogicalDeviceSessionType pidWriteAccess;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0106: Unknown result type (might be due to invalid IL or missing references)
				//IL_0135: Unknown result type (might be due to invalid IL or missing references)
				//IL_014e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0098: Unknown result type (might be due to invalid IL or missing references)
				//IL_009d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Expected O, but got Unknown
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				try
				{
					ILumberjackDeviceApi apiProviderForOnlineDevicePidOperations = default(ILumberjackDeviceApi);
					if (num != 0)
					{
						apiProviderForOnlineDevicePidOperations = directConnectionRvCloudIoT.GetApiProviderForOnlineDevicePidOperations(logicalDevice);
					}
					try
					{
						TaskAwaiter val;
						if (num != 0)
						{
							RvCloudDeviceComponentCommandPidWriteWithAddress componentCommand = new RvCloudDeviceComponentCommandPidWriteWithAddress(ComponentIdEx.MakeComponentId(logicalDevice.LogicalId), pid, address, UInt48.op_Implicit(value), pidWriteAccess);
							val = directConnectionRvCloudIoT.SendCommandAsync(apiProviderForOnlineDevicePidOperations, (RvCloudDeviceComponentCommand)(object)componentCommand, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <PidWriteAsync>d__80>(ref val, ref this);
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
					catch (OperationCanceledException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
						throw new LogicalDevicePidCanceledException();
					}
					catch (TimeoutException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
						throw new LogicalDevicePidTimeoutException();
					}
					catch (global::System.Exception ex3)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of " + ex3.Message, global::System.Array.Empty<object>());
						throw new LogicalDevicePidValueWriteException(PidExtension.ConvertToPid(pid), logicalDevice, (ulong)value, ex3);
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
		private struct <RenameLogicalDevice>d__85 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ILogicalDevice logicalDevice;

			public DirectConnectionRvCloudIoT <>4__this;

			public FUNCTION_NAME toName;

			public byte toFunctionInstance;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0169: Expected O, but got Unknown
				//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0100: Unknown result type (might be due to invalid IL or missing references)
				//IL_0108: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b7: Expected O, but got Unknown
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b8: Expected O, but got Unknown
				//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				try
				{
					try
					{
						TaskAwaiter val;
						if (num != 0)
						{
							if (logicalDevice == null || !directConnectionRvCloudIoT.IsLogicalDeviceSupported(logicalDevice))
							{
								throw new LogicalDeviceSourceDirectRenameNotSupportedException((global::System.Exception)null);
							}
							if (!((BackgroundOperationBase)directConnectionRvCloudIoT).Started)
							{
								throw new LogicalDeviceSourceDirectRenameException("Device Source not Started", (global::System.Exception)null);
							}
							if (!directConnectionRvCloudIoT.IsConnected)
							{
								throw new LogicalDeviceSourceDirectRenameException("Device Source Offline", (global::System.Exception)null);
							}
							ILumberjackDeviceApi apiProvider = directConnectionRvCloudIoT.ApiProvider;
							if (apiProvider == null)
							{
								throw new LogicalDeviceSourceDirectRenameException("Device Offline", (global::System.Exception)null);
							}
							if (!directConnectionRvCloudIoT.IsLogicalDeviceOnline(logicalDevice))
							{
								throw new LogicalDeviceSourceDirectRenameException("Device Offline", (global::System.Exception)null);
							}
							RvCloudDeviceComponentCommandRename componentCommand = new RvCloudDeviceComponentCommandRename(ComponentIdEx.MakeComponentId(logicalDevice.LogicalId), new CommandRename(FunctionNameExtension.ToFunctionName(toName), toFunctionInstance));
							val = directConnectionRvCloudIoT.SendCommandAsync(apiProvider, (RvCloudDeviceComponentCommand)(object)componentCommand, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <RenameLogicalDevice>d__85>(ref val, ref this);
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
					catch (OperationCanceledException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
						throw;
					}
					catch (TimeoutException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
						throw;
					}
					catch (LogicalDeviceSourceDirectRenameException ex3)
					{
						LogicalDeviceSourceDirectRenameException ex4 = ex3;
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of " + ((global::System.Exception)(object)ex4).Message, global::System.Array.Empty<object>());
						throw;
					}
					catch (global::System.Exception ex5)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of " + ex5.Message, global::System.Array.Empty<object>());
						throw new LogicalDeviceSourceDirectRenameException("Unable to send rename command", ex5);
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
		private struct <SendCommandAsync>d__66 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ILumberjackDeviceApi apiProvider;

			public DirectConnectionRvCloudIoT <>4__this;

			public RvCloudDeviceComponentCommand componentCommand;

			public CancellationToken cancellationToken;

			private TaskAwaiter<ComponentCommandResponse> <>u__1;

			private void MoveNext()
			{
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ab: Invalid comparison between Unknown and I4
				//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				try
				{
					TaskAwaiter<ComponentCommandResponse> val;
					if (num != 0)
					{
						val = apiProvider.SendCommandsAsync(directConnectionRvCloudIoT._uniqueVehicleIdentifier, (RvCloudDeviceComponentCommand[])(object)new RvCloudDeviceComponentCommand[1] { componentCommand }, cancellationToken).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<ComponentCommandResponse>, <SendCommandAsync>d__66>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<ComponentCommandResponse>);
						num = (<>1__state = -1);
					}
					ComponentCommandState status = (Enumerable.FirstOrDefault<ComponentCommandResult>((global::System.Collections.Generic.IEnumerable<ComponentCommandResult>)val.GetResult().Results) ?? throw new RvCloudIoTComponentCommandResultResponseNullException((global::System.Exception)null)).Status;
					if ((int)status == 0)
					{
						throw new RvCloudIoTComponentCommandResultResponseFailedException((global::System.Exception)null);
					}
					if ((int)status != 1)
					{
						throw new RvCloudIoTComponentCommandResultResponseInvalidException((global::System.Exception)null);
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
		private struct <SendDirectCommandClimateZoneAsync>d__67 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<CommandResult> <>t__builder;

			public DirectConnectionRvCloudIoT <>4__this;

			public ILogicalDeviceClimateZone logicalDevice;

			public LogicalDeviceClimateZoneCommand command;

			public CancellationToken cancelToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_011d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0141: Unknown result type (might be due to invalid IL or missing references)
				//IL_016d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0197: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00df: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Expected O, but got Unknown
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				//IL_009d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				CommandResult result;
				try
				{
					try
					{
						TaskAwaiter val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_00ee;
						}
						if (!directConnectionRvCloudIoT.IsLogicalDeviceSupported((ILogicalDevice?)(object)logicalDevice))
						{
							result = (CommandResult)13;
						}
						else if (!((BackgroundOperationBase)directConnectionRvCloudIoT).Started)
						{
							result = (CommandResult)6;
						}
						else if (!directConnectionRvCloudIoT.IsConnected)
						{
							result = (CommandResult)6;
						}
						else
						{
							ILumberjackDeviceApi apiProvider = directConnectionRvCloudIoT.ApiProvider;
							if (apiProvider == null)
							{
								result = (CommandResult)6;
							}
							else
							{
								if (directConnectionRvCloudIoT.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
								{
									RvCloudDeviceComponentCommand componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandClimateZone(ComponentIdEx.MakeComponentId(((ILogicalDevice)logicalDevice).LogicalId), command);
									val = directConnectionRvCloudIoT.SendCommandAsync(apiProvider, componentCommand, cancelToken).GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <SendDirectCommandClimateZoneAsync>d__67>(ref val, ref this);
										return;
									}
									goto IL_00ee;
								}
								result = (CommandResult)6;
							}
						}
						goto end_IL_0011;
						IL_00ee:
						((TaskAwaiter)(ref val)).GetResult();
						result = (CommandResult)0;
						end_IL_0011:;
					}
					catch (OperationCanceledException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
						result = (CommandResult)1;
					}
					catch (TimeoutException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
						result = (CommandResult)5;
					}
					catch (global::System.Exception ex3)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of " + ex3.Message, global::System.Array.Empty<object>());
						result = (CommandResult)7;
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
		private struct <SendDirectCommandGeneratorGenie>d__81 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<CommandResult> <>t__builder;

			public DirectConnectionRvCloudIoT <>4__this;

			public ILogicalDeviceGeneratorGenie logicalDevice;

			public GeneratorGenieCommand command;

			public CancellationToken cancelToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_016d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0191: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_0122: Unknown result type (might be due to invalid IL or missing references)
				//IL_0127: Unknown result type (might be due to invalid IL or missing references)
				//IL_012f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0146: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Invalid comparison between Unknown and I4
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a0: Expected O, but got Unknown
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Invalid comparison between Unknown and I4
				//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ab: Expected O, but got Unknown
				//IL_0107: Unknown result type (might be due to invalid IL or missing references)
				//IL_0109: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				CommandResult result;
				try
				{
					try
					{
						TaskAwaiter val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_013e;
						}
						if (!directConnectionRvCloudIoT.IsLogicalDeviceSupported((ILogicalDevice?)(object)logicalDevice))
						{
							result = (CommandResult)13;
						}
						else if (!((BackgroundOperationBase)directConnectionRvCloudIoT).Started)
						{
							result = (CommandResult)6;
						}
						else if (!directConnectionRvCloudIoT.IsConnected)
						{
							result = (CommandResult)6;
						}
						else
						{
							ILumberjackDeviceApi apiProvider = directConnectionRvCloudIoT.ApiProvider;
							if (apiProvider == null)
							{
								result = (CommandResult)6;
							}
							else
							{
								if (directConnectionRvCloudIoT.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
								{
									string text = ComponentIdEx.MakeComponentId(((ILogicalDevice)logicalDevice).LogicalId);
									GeneratorGenieCommand val2 = command;
									RvCloudDeviceComponentCommand componentCommand;
									if ((int)val2 != 1)
									{
										if ((int)val2 != 2)
										{
											throw new RvCloudIoTComponentCommandInvalidException($"Unknown Generator Genie Command Mode for {command}", (global::System.Exception)null);
										}
										componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandGeneratorGenieOn(text);
									}
									else
									{
										componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandGeneratorGenieOff(text);
									}
									val = directConnectionRvCloudIoT.SendCommandAsync(apiProvider, componentCommand, cancelToken).GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <SendDirectCommandGeneratorGenie>d__81>(ref val, ref this);
										return;
									}
									goto IL_013e;
								}
								result = (CommandResult)6;
							}
						}
						goto end_IL_0011;
						IL_013e:
						((TaskAwaiter)(ref val)).GetResult();
						result = (CommandResult)0;
						end_IL_0011:;
					}
					catch (OperationCanceledException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
						result = (CommandResult)1;
					}
					catch (TimeoutException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
						result = (CommandResult)5;
					}
					catch (global::System.Exception ex3)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of " + ex3.Message, global::System.Array.Empty<object>());
						result = (CommandResult)7;
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
		private struct <SendDirectCommandLightDimmable>d__82 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<CommandResult> <>t__builder;

			public DirectConnectionRvCloudIoT <>4__this;

			public ILogicalDeviceLightDimmable logicalDevice;

			public LogicalDeviceLightDimmableCommand command;

			public CancellationToken cancelToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_019b: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0215: Unknown result type (might be due to invalid IL or missing references)
				//IL_0150: Unknown result type (might be due to invalid IL or missing references)
				//IL_0155: Unknown result type (might be due to invalid IL or missing references)
				//IL_015d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0174: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Expected O, but got Unknown
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Invalid comparison between Unknown and I4
				//IL_0111: Unknown result type (might be due to invalid IL or missing references)
				//IL_011b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0120: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c3: Expected O, but got Unknown
				//IL_009b: Unknown result type (might be due to invalid IL or missing references)
				//IL_009f: Invalid comparison between Unknown and I4
				//IL_0135: Unknown result type (might be due to invalid IL or missing references)
				//IL_0137: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d4: Expected O, but got Unknown
				//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_0106: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				CommandResult result;
				try
				{
					try
					{
						TaskAwaiter val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_016c;
						}
						if (!directConnectionRvCloudIoT.IsLogicalDeviceSupported((ILogicalDevice?)(object)logicalDevice))
						{
							result = (CommandResult)13;
						}
						else if (!((BackgroundOperationBase)directConnectionRvCloudIoT).Started)
						{
							result = (CommandResult)6;
						}
						else if (!directConnectionRvCloudIoT.IsConnected)
						{
							result = (CommandResult)6;
						}
						else
						{
							ILumberjackDeviceApi apiProvider = directConnectionRvCloudIoT.ApiProvider;
							if (apiProvider == null)
							{
								result = (CommandResult)6;
							}
							else
							{
								if (directConnectionRvCloudIoT.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
								{
									string text = ComponentIdEx.MakeComponentId(((ILogicalDevice)logicalDevice).LogicalId);
									DimmableLightCommand val2 = command.Command;
									RvCloudDeviceComponentCommand componentCommand;
									if ((int)val2 != 0)
									{
										if (val2 - 1 > 2)
										{
											if ((int)val2 != 127)
											{
												throw new RvCloudIoTComponentCommandInvalidException($"Unknown Dimmable Command Mode for {command.Command}", (global::System.Exception)null);
											}
											componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightDimmableRestore(text, command);
										}
										else
										{
											componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightDimmableSetState(text, command);
										}
									}
									else
									{
										componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightDimmableSetStateOff(text, command);
									}
									val = directConnectionRvCloudIoT.SendCommandAsync(apiProvider, componentCommand, cancelToken).GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <SendDirectCommandLightDimmable>d__82>(ref val, ref this);
										return;
									}
									goto IL_016c;
								}
								result = (CommandResult)6;
							}
						}
						goto end_IL_0011;
						IL_016c:
						((TaskAwaiter)(ref val)).GetResult();
						result = (CommandResult)0;
						end_IL_0011:;
					}
					catch (OperationCanceledException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
						result = (CommandResult)1;
					}
					catch (TimeoutException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
						result = (CommandResult)5;
					}
					catch (global::System.Exception ex3)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of " + ex3.Message, global::System.Array.Empty<object>());
						result = (CommandResult)7;
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
		private struct <SendDirectCommandLightRgb>d__83 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<CommandResult> <>t__builder;

			public DirectConnectionRvCloudIoT <>4__this;

			public ILogicalDeviceLightRgb logicalDevice;

			public LogicalDeviceLightRgbCommand command;

			public CancellationToken cancelToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0201: Unknown result type (might be due to invalid IL or missing references)
				//IL_022d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0257: Unknown result type (might be due to invalid IL or missing references)
				//IL_0192: Unknown result type (might be due to invalid IL or missing references)
				//IL_0197: Unknown result type (might be due to invalid IL or missing references)
				//IL_019f: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bb: Expected I4, but got Unknown
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d2: Expected O, but got Unknown
				//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e3: Expected O, but got Unknown
				//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f4: Expected O, but got Unknown
				//IL_0136: Unknown result type (might be due to invalid IL or missing references)
				//IL_0148: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
				//IL_0105: Expected O, but got Unknown
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Invalid comparison between Unknown and I4
				//IL_0153: Unknown result type (might be due to invalid IL or missing references)
				//IL_015d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0162: Unknown result type (might be due to invalid IL or missing references)
				//IL_010f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0116: Expected O, but got Unknown
				//IL_0177: Unknown result type (might be due to invalid IL or missing references)
				//IL_0179: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				CommandResult result;
				try
				{
					try
					{
						TaskAwaiter val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_01ae;
						}
						if (!directConnectionRvCloudIoT.IsLogicalDeviceSupported((ILogicalDevice?)(object)logicalDevice))
						{
							result = (CommandResult)13;
						}
						else if (!((BackgroundOperationBase)directConnectionRvCloudIoT).Started)
						{
							result = (CommandResult)6;
						}
						else if (!directConnectionRvCloudIoT.IsConnected)
						{
							result = (CommandResult)6;
						}
						else
						{
							ILumberjackDeviceApi apiProvider = directConnectionRvCloudIoT.ApiProvider;
							if (apiProvider == null)
							{
								result = (CommandResult)6;
							}
							else
							{
								if (directConnectionRvCloudIoT.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
								{
									string text = ComponentIdEx.MakeComponentId(((ILogicalDevice)logicalDevice).LogicalId);
									RgbLightModeCommand mode = command.Mode;
									RvCloudDeviceComponentCommand componentCommand;
									switch ((int)mode)
									{
									default:
										if ((int)mode != 127)
										{
											goto case 3;
										}
										componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightRgbRestore(text, command);
										break;
									case 0:
										componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightRgbSetStateOff(text, command);
										break;
									case 1:
										componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightRgbSetStateOn(text, command);
										break;
									case 2:
										componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightRgbSetStateBlink(text, command);
										break;
									case 4:
									case 5:
									case 6:
									case 7:
									case 8:
										componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightRgbSetStateTransition(text, command);
										break;
									case 3:
										throw new RvCloudIoTComponentCommandInvalidException($"Unknown RGB Command Mode for {command.Mode}", (global::System.Exception)null);
									}
									val = directConnectionRvCloudIoT.SendCommandAsync(apiProvider, componentCommand, cancelToken).GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <SendDirectCommandLightRgb>d__83>(ref val, ref this);
										return;
									}
									goto IL_01ae;
								}
								result = (CommandResult)6;
							}
						}
						goto end_IL_0011;
						IL_01ae:
						((TaskAwaiter)(ref val)).GetResult();
						result = (CommandResult)0;
						end_IL_0011:;
					}
					catch (OperationCanceledException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
						result = (CommandResult)1;
					}
					catch (TimeoutException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
						result = (CommandResult)5;
					}
					catch (global::System.Exception ex3)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of " + ex3.Message, global::System.Array.Empty<object>());
						result = (CommandResult)7;
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
		private struct <SendDirectCommandRelayBasicSwitch>d__89 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<CommandResult> <>t__builder;

			public DirectConnectionRvCloudIoT <>4__this;

			public ILogicalDeviceSwitchable logicalDevice;

			public bool turnOn;

			public CancellationToken cancelToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0249: Unknown result type (might be due to invalid IL or missing references)
				//IL_026d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0299: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
				//IL_0203: Unknown result type (might be due to invalid IL or missing references)
				//IL_020b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0222: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e1: Expected O, but got Unknown
				//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e3: Expected O, but got Unknown
				//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0107: Unknown result type (might be due to invalid IL or missing references)
				//IL_0111: Expected O, but got Unknown
				//IL_010c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0113: Expected O, but got Unknown
				//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fd: Expected O, but got Unknown
				//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Expected O, but got Unknown
				//IL_0135: Unknown result type (might be due to invalid IL or missing references)
				//IL_013f: Expected O, but got Unknown
				//IL_013a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0141: Expected O, but got Unknown
				//IL_0127: Unknown result type (might be due to invalid IL or missing references)
				//IL_012e: Expected O, but got Unknown
				//IL_015d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0167: Expected O, but got Unknown
				//IL_0162: Unknown result type (might be due to invalid IL or missing references)
				//IL_0169: Expected O, but got Unknown
				//IL_0152: Unknown result type (might be due to invalid IL or missing references)
				//IL_0159: Expected O, but got Unknown
				//IL_0180: Unknown result type (might be due to invalid IL or missing references)
				//IL_0187: Expected O, but got Unknown
				//IL_0175: Unknown result type (might be due to invalid IL or missing references)
				//IL_017c: Expected O, but got Unknown
				//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectConnectionRvCloudIoT directConnectionRvCloudIoT = <>4__this;
				CommandResult result;
				try
				{
					try
					{
						TaskAwaiter val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_021a;
						}
						if (!directConnectionRvCloudIoT.IsLogicalDeviceSupported((ILogicalDevice?)(object)logicalDevice))
						{
							result = (CommandResult)13;
						}
						else if (!((BackgroundOperationBase)directConnectionRvCloudIoT).Started)
						{
							result = (CommandResult)6;
						}
						else if (!directConnectionRvCloudIoT.IsConnected)
						{
							result = (CommandResult)6;
						}
						else
						{
							ILumberjackDeviceApi apiProvider = directConnectionRvCloudIoT.ApiProvider;
							if (apiProvider == null)
							{
								result = (CommandResult)6;
							}
							else
							{
								if (directConnectionRvCloudIoT.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
								{
									string text = ComponentIdEx.MakeComponentId(((ILogicalDevice)logicalDevice).LogicalId);
									RvCloudDeviceComponentCommand componentCommand;
									if (!(logicalDevice is LogicalDeviceRelayBasicLatchingType1))
									{
										if (logicalDevice is LogicalDeviceRelayBasicLatchingType2)
										{
											componentCommand = (RvCloudDeviceComponentCommand)((!turnOn) ? new RvCloudDeviceComponentCommandRelayBasicType2SetState(text, new LogicalDeviceRelayBasicLatchingCommandType2((EnhancedCommand)0)) : new RvCloudDeviceComponentCommandRelayBasicType2SetState(text, new LogicalDeviceRelayBasicLatchingCommandType2((EnhancedCommand)1)));
										}
										else if (logicalDevice is ILogicalDeviceLightDimmable)
										{
											componentCommand = (RvCloudDeviceComponentCommand)((!turnOn) ? ((object)new RvCloudDeviceComponentCommandLightDimmableSetStateOff(text, new LogicalDeviceLightDimmableCommand())) : ((object)new RvCloudDeviceComponentCommandLightDimmableRestore(text, LogicalDeviceLightDimmableCommand.MakeRestoreCommand())));
										}
										else if (logicalDevice is ILogicalDeviceLightRgb)
										{
											componentCommand = (RvCloudDeviceComponentCommand)((!turnOn) ? ((object)new RvCloudDeviceComponentCommandLightRgbSetStateOff(text, new LogicalDeviceLightRgbCommand())) : ((object)new RvCloudDeviceComponentCommandLightRgbRestore(text, LogicalDeviceLightRgbCommand.MakeRestoreCommand())));
										}
										else
										{
											if (!(logicalDevice is ILogicalDeviceGeneratorGenie))
											{
												throw new RvCloudIoTComponentCommandNotSupportedException($"Switchable command not supported/implemented for {logicalDevice}", (global::System.Exception)null);
											}
											componentCommand = (RvCloudDeviceComponentCommand)((!turnOn) ? ((object)new RvCloudDeviceComponentCommandGeneratorGenieOff(text)) : ((object)new RvCloudDeviceComponentCommandGeneratorGenieOn(text)));
										}
									}
									else
									{
										componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandRelayBasicType1SetState(text, new LogicalDeviceRelayBasicCommandType1(turnOn));
									}
									val = directConnectionRvCloudIoT.SendCommandAsync(apiProvider, componentCommand, cancelToken).GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <SendDirectCommandRelayBasicSwitch>d__89>(ref val, ref this);
										return;
									}
									goto IL_021a;
								}
								result = (CommandResult)6;
							}
						}
						goto end_IL_0011;
						IL_021a:
						((TaskAwaiter)(ref val)).GetResult();
						result = (CommandResult)0;
						end_IL_0011:;
					}
					catch (OperationCanceledException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
						result = (CommandResult)1;
					}
					catch (TimeoutException)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
						result = (CommandResult)5;
					}
					catch (global::System.Exception ex3)
					{
						TaggedLog.Warning("DirectConnectionRvCloudIoT", directConnectionRvCloudIoT.LogPrefix + " Unable to send command because of " + ex3.Message, global::System.Array.Empty<object>());
						result = (CommandResult)7;
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

		private const string LogTag = "DirectConnectionRvCloudIoT";

		private const int RetryDelayMs = 10000;

		private const int ValidateConnectionDelayMs = 10000;

		private readonly ServiceCollection _serviceCollection = new ServiceCollection();

		private readonly LogicalDevicesOnlineManager _logicalDevicesOnlineManager = new LogicalDevicesOnlineManager();

		private readonly string _globalDeviceEndpoint;

		private readonly string _idScope;

		private readonly string _apiBaseAddress;

		private readonly string _phoneUuid;

		private readonly string _uniqueVehicleIdentifier;

		public const string DeviceSourceTokenPrefix = "RvCloudIot";

		[CompilerGenerated]
		private Action<ILogicalDeviceSourceDirectConnection>? m_DidConnectEvent;

		[CompilerGenerated]
		private Action<ILogicalDeviceSourceDirectConnection>? m_DidDisconnectEvent;

		[CompilerGenerated]
		private UpdateDeviceSourceReachabilityEventHandler? m_UpdateDeviceSourceReachabilityEvent;

		private ConnectionStatus _connectionStatus;

		private readonly ConcurrentDictionary<MetadataTracker.Key, MetadataTracker> _metadataTrackers = new ConcurrentDictionary<MetadataTracker.Key, MetadataTracker>();

		private ConcurrentDictionary<PidReadTracker.Key, PidReadTracker> _pidReadTrackers = new ConcurrentDictionary<PidReadTracker.Key, PidReadTracker>();

		private readonly ConcurrentDictionary<PidIndexReadTracker.Key, PidIndexReadTracker> _pidIndexReadTrackers = new ConcurrentDictionary<PidIndexReadTracker.Key, PidIndexReadTracker>();

		[field: CompilerGenerated]
		public string LogPrefix
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public ILogicalDeviceService DeviceService
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public RvCloudIoTCloudCredential CloudCredential
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public ILogicalDeviceSessionManager? SessionManager
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		protected ILumberjackDeviceApi? ApiProvider
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public string DeviceSourceToken
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public global::System.Collections.Generic.IReadOnlyList<ILogicalDeviceTag> ConnectionTagList
		{
			[CompilerGenerated]
			get;
		}

		public bool AllowAutoOfflineLogicalDeviceRemoval => true;

		public bool IsDeviceSourceActive => (int)_connectionStatus == 1;

		public IN_MOTION_LOCKOUT_LEVEL InTransitLockoutLevel => IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)0);

		public bool IsConnected => (int)_connectionStatus == 1;

		public event Action<ILogicalDeviceSourceDirectConnection>? DidConnectEvent
		{
			[CompilerGenerated]
			add
			{
				Action<ILogicalDeviceSourceDirectConnection> val = this.m_DidConnectEvent;
				Action<ILogicalDeviceSourceDirectConnection> val2;
				do
				{
					val2 = val;
					Action<ILogicalDeviceSourceDirectConnection> val3 = (Action<ILogicalDeviceSourceDirectConnection>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<Action<ILogicalDeviceSourceDirectConnection>>(ref this.m_DidConnectEvent, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				Action<ILogicalDeviceSourceDirectConnection> val = this.m_DidConnectEvent;
				Action<ILogicalDeviceSourceDirectConnection> val2;
				do
				{
					val2 = val;
					Action<ILogicalDeviceSourceDirectConnection> val3 = (Action<ILogicalDeviceSourceDirectConnection>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<Action<ILogicalDeviceSourceDirectConnection>>(ref this.m_DidConnectEvent, val3, val2);
				}
				while (val != val2);
			}
		}

		public event Action<ILogicalDeviceSourceDirectConnection>? DidDisconnectEvent
		{
			[CompilerGenerated]
			add
			{
				Action<ILogicalDeviceSourceDirectConnection> val = this.m_DidDisconnectEvent;
				Action<ILogicalDeviceSourceDirectConnection> val2;
				do
				{
					val2 = val;
					Action<ILogicalDeviceSourceDirectConnection> val3 = (Action<ILogicalDeviceSourceDirectConnection>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<Action<ILogicalDeviceSourceDirectConnection>>(ref this.m_DidDisconnectEvent, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				Action<ILogicalDeviceSourceDirectConnection> val = this.m_DidDisconnectEvent;
				Action<ILogicalDeviceSourceDirectConnection> val2;
				do
				{
					val2 = val;
					Action<ILogicalDeviceSourceDirectConnection> val3 = (Action<ILogicalDeviceSourceDirectConnection>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<Action<ILogicalDeviceSourceDirectConnection>>(ref this.m_DidDisconnectEvent, val3, val2);
				}
				while (val != val2);
			}
		}

		public event UpdateDeviceSourceReachabilityEventHandler UpdateDeviceSourceReachabilityEvent
		{
			[CompilerGenerated]
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				UpdateDeviceSourceReachabilityEventHandler val = this.m_UpdateDeviceSourceReachabilityEvent;
				UpdateDeviceSourceReachabilityEventHandler val2;
				do
				{
					val2 = val;
					UpdateDeviceSourceReachabilityEventHandler val3 = (UpdateDeviceSourceReachabilityEventHandler)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<UpdateDeviceSourceReachabilityEventHandler>(ref this.m_UpdateDeviceSourceReachabilityEvent, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				UpdateDeviceSourceReachabilityEventHandler val = this.m_UpdateDeviceSourceReachabilityEvent;
				UpdateDeviceSourceReachabilityEventHandler val2;
				do
				{
					val2 = val;
					UpdateDeviceSourceReachabilityEventHandler val3 = (UpdateDeviceSourceReachabilityEventHandler)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<UpdateDeviceSourceReachabilityEventHandler>(ref this.m_UpdateDeviceSourceReachabilityEvent, val3, val2);
				}
				while (val != val2);
			}
		}

		public DirectConnectionRvCloudIoT(ILogicalDeviceService deviceService, RvCloudIoTCloudCredential cloudCredential, string phoneUuid, string uniqueVehicleIdentifier)
			: this(deviceService, cloudCredential, MakeDeviceSourceToken(cloudCredential), phoneUuid, null)
		{
		}

		public DirectConnectionRvCloudIoT(ILogicalDeviceService deviceService, RvCloudIoTCloudCredential cloudCredential, string deviceSourceToken, string phoneUuid, ILogicalDeviceTag? logicalDeviceTag = null)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			string gatewayId = ((CloudGatewayCredential)cloudCredential).GatewayId;
			LogicalDeviceServiceExtension.RegisterRvCloudIotServices(deviceService);
			LogPrefix = $"[{((CloudGatewayCredential)cloudCredential).Environment}:{((CloudGatewayCredential)cloudCredential).Credential.Username}:{((CloudGatewayCredential)cloudCredential).GatewayId}]";
			DeviceService = deviceService ?? throw new ArgumentNullException("deviceService");
			CloudCredential = cloudCredential ?? throw new ArgumentNullException("cloudCredential");
			DeviceSourceToken = deviceSourceToken;
			List<ILogicalDeviceTag> obj;
			if (logicalDeviceTag != null)
			{
				obj = new List<ILogicalDeviceTag>();
				obj.Add(logicalDeviceTag);
			}
			else
			{
				obj = new List<ILogicalDeviceTag>();
			}
			ConnectionTagList = (global::System.Collections.Generic.IReadOnlyList<ILogicalDeviceTag>)obj;
			_globalDeviceEndpoint = ((CloudGatewayCredential)CloudCredential).Environment.GetGlobalDeviceEndpointRvCloudIoT() ?? throw new global::System.Exception($"{((CloudGatewayCredential)CloudCredential).Environment} doesn't support globalDeviceEndpoint");
			_idScope = ((CloudGatewayCredential)CloudCredential).Environment.GetIdScopeRvCloudIoT() ?? throw new global::System.Exception($"{((CloudGatewayCredential)CloudCredential).Environment} doesn't support idScope");
			_apiBaseAddress = ((CloudGatewayCredential)CloudCredential).Environment.GetApiBaseAddressRvCloudIoT() ?? throw new global::System.Exception($"{((CloudGatewayCredential)CloudCredential).Environment} doesn't support apiBaseAddress");
			_phoneUuid = phoneUuid ?? throw new ArgumentNullException("phoneUuid");
			_uniqueVehicleIdentifier = gatewayId ?? throw new ArgumentNullException("uniqueVehicleIdentifier");
			TaggedLog.Information("DirectConnectionRvCloudIoT", $"{LogPrefix}: RvCloud IOT Information\n    DeviceSourceToken = {deviceSourceToken}\n    idScope = {_idScope}\n    PhoneUuid = {_phoneUuid}\n    UCI = {_uniqueVehicleIdentifier}", global::System.Array.Empty<object>());
			SessionManager = (ILogicalDeviceSessionManager?)(object)new RvCloudIoTSessionManager(this);
			ServiceCollectionExtensions.AddDelegateDevice((IServiceCollection)(object)_serviceCollection, (Action<DelegateDeviceSettings>)([CompilerGenerated] (DelegateDeviceSettings c) =>
			{
				c.GlobalDeviceEndpoint = _globalDeviceEndpoint;
				c.ApiBaseAddress = _apiBaseAddress;
				c.IdScope = _idScope;
			}), (Action<PolicyRegistry>)delegate
			{
			});
			ServiceCollectionServiceExtensions.AddSingleton<IAuthTokenProvider>((IServiceCollection)(object)_serviceCollection, (IAuthTokenProvider)(object)this);
			ServiceCollectionServiceExtensions.AddSingleton<ICloudToDeviceMessageHandler>((IServiceCollection)(object)_serviceCollection, (ICloudToDeviceMessageHandler)(object)this);
		}

		public static string MakeDeviceSourceToken(RvCloudIoTCloudCredential cloudCredential)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			return $"{"RvCloudIot"}:{((CloudGatewayCredential)cloudCredential).Environment}:{((CloudGatewayCredential)cloudCredential).Credential.Username}:{((CloudGatewayCredential)cloudCredential).GatewayId}";
		}

		public global::System.Collections.Generic.IEnumerable<ILogicalDeviceTag> MakeDeviceSourceTags(ILogicalDevice? logicalDevice)
		{
			return (global::System.Collections.Generic.IEnumerable<ILogicalDeviceTag>)ConnectionTagList;
		}

		public bool IsLogicalDeviceSupported(ILogicalDevice? logicalDevice)
		{
			return _logicalDevicesOnlineManager.GetLogicalDeviceOnlineStatus(logicalDevice) != LogicalDeviceOnlineStatus.Unknown;
		}

		public bool IsLogicalDeviceOnline(ILogicalDevice? logicalDevice)
		{
			return _logicalDevicesOnlineManager.GetLogicalDeviceOnlineStatus(logicalDevice) == LogicalDeviceOnlineStatus.Online;
		}

		public IN_MOTION_LOCKOUT_LEVEL GetLogicalDeviceInTransitLockoutLevel(ILogicalDevice? logicalDevice)
		{
			IsLogicalDeviceOnline(logicalDevice);
			return IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)0);
		}

		public bool IsLogicalDeviceHazardous(ILogicalDevice? logicalDevice)
		{
			return IN_MOTION_LOCKOUT_LEVEL.op_Implicit(GetLogicalDeviceInTransitLockoutLevel(logicalDevice)) != 0;
		}

		public LogicalDeviceReachability DeviceSourceReachability(ILogicalDevice logicalDevice)
		{
			switch (_logicalDevicesOnlineManager.GetLogicalDeviceOnlineStatus(logicalDevice))
			{
			case LogicalDeviceOnlineStatus.Online:
				if (!IsDeviceSourceActive)
				{
					return (LogicalDeviceReachability)0;
				}
				return (LogicalDeviceReachability)1;
			case LogicalDeviceOnlineStatus.Offline:
				return (LogicalDeviceReachability)0;
			default:
				return (LogicalDeviceReachability)2;
			}
		}

		[AsyncStateMachine(typeof(<BackgroundOperationAsync>d__59))]
		protected override global::System.Threading.Tasks.Task BackgroundOperationAsync(CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<BackgroundOperationAsync>d__59 <BackgroundOperationAsync>d__ = default(<BackgroundOperationAsync>d__59);
			<BackgroundOperationAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<BackgroundOperationAsync>d__.<>4__this = this;
			<BackgroundOperationAsync>d__.cancellationToken = cancellationToken;
			<BackgroundOperationAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <BackgroundOperationAsync>d__.<>t__builder)).Start<<BackgroundOperationAsync>d__59>(ref <BackgroundOperationAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <BackgroundOperationAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<GetAllDeviceComponentsAsync>d__60))]
		public global::System.Threading.Tasks.Task GetAllDeviceComponentsAsync(ILumberjackDeviceApi apiProvider, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			<GetAllDeviceComponentsAsync>d__60 <GetAllDeviceComponentsAsync>d__ = default(<GetAllDeviceComponentsAsync>d__60);
			<GetAllDeviceComponentsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<GetAllDeviceComponentsAsync>d__.<>4__this = this;
			<GetAllDeviceComponentsAsync>d__.apiProvider = apiProvider;
			<GetAllDeviceComponentsAsync>d__.cancellationToken = cancellationToken;
			<GetAllDeviceComponentsAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <GetAllDeviceComponentsAsync>d__.<>t__builder)).Start<<GetAllDeviceComponentsAsync>d__60>(ref <GetAllDeviceComponentsAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <GetAllDeviceComponentsAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<DebugSetupReportingOfStatusChanges>d__61))]
		public global::System.Threading.Tasks.Task DebugSetupReportingOfStatusChanges(IDelegateDeviceClient deviceClient, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<DebugSetupReportingOfStatusChanges>d__61 <DebugSetupReportingOfStatusChanges>d__ = default(<DebugSetupReportingOfStatusChanges>d__61);
			<DebugSetupReportingOfStatusChanges>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DebugSetupReportingOfStatusChanges>d__.deviceClient = deviceClient;
			<DebugSetupReportingOfStatusChanges>d__.cancellationToken = cancellationToken;
			<DebugSetupReportingOfStatusChanges>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <DebugSetupReportingOfStatusChanges>d__.<>t__builder)).Start<<DebugSetupReportingOfStatusChanges>d__61>(ref <DebugSetupReportingOfStatusChanges>d__);
			return ((AsyncTaskMethodBuilder)(ref <DebugSetupReportingOfStatusChanges>d__.<>t__builder)).Task;
		}

		private void LumberjackConnectionChanged(object sender, ConnectionStatusChangedEventArgs e)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected I4, but got Unknown
			TaggedLog.Information("DirectConnectionRvCloudIoT", $"Connection Status Changes {e.Status} because {e.Reason}", global::System.Array.Empty<object>());
			_connectionStatus = e.Status;
			ConnectionStatus connectionStatus = _connectionStatus;
			switch ((int)connectionStatus)
			{
			case 1:
				this.DidConnectEvent?.Invoke((ILogicalDeviceSourceDirectConnection)(object)this);
				break;
			default:
				_logicalDevicesOnlineManager.AutoTakeDevicesOffline(forceAllOffline: true);
				this.DidDisconnectEvent?.Invoke((ILogicalDeviceSourceDirectConnection)(object)this);
				break;
			}
			UpdateDeviceSourceReachabilityEventHandler? obj = this.UpdateDeviceSourceReachabilityEvent;
			if (obj != null)
			{
				obj.Invoke((ILogicalDeviceSourceDirect)(object)this);
			}
		}

		public override string ToString()
		{
			return "DirectConnectionRvCloudIoT: DeviceSourceToken = " + DeviceSourceToken;
		}

		public string GetAuthToken()
		{
			return ((CloudGatewayCredential)CloudCredential).Credential.AccessToken;
		}

		[AsyncStateMachine(typeof(<SendCommandAsync>d__66))]
		public global::System.Threading.Tasks.Task SendCommandAsync(ILumberjackDeviceApi apiProvider, RvCloudDeviceComponentCommand componentCommand, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<SendCommandAsync>d__66 <SendCommandAsync>d__ = default(<SendCommandAsync>d__66);
			<SendCommandAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SendCommandAsync>d__.<>4__this = this;
			<SendCommandAsync>d__.apiProvider = apiProvider;
			<SendCommandAsync>d__.componentCommand = componentCommand;
			<SendCommandAsync>d__.cancellationToken = cancellationToken;
			<SendCommandAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <SendCommandAsync>d__.<>t__builder)).Start<<SendCommandAsync>d__66>(ref <SendCommandAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <SendCommandAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<SendDirectCommandClimateZoneAsync>d__67))]
		public async global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandClimateZoneAsync(ILogicalDeviceClimateZone logicalDevice, LogicalDeviceClimateZoneCommand command, CancellationToken cancelToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (!IsLogicalDeviceSupported((ILogicalDevice?)(object)logicalDevice))
				{
					return (CommandResult)13;
				}
				if (!((BackgroundOperationBase)this).Started)
				{
					return (CommandResult)6;
				}
				if (!IsConnected)
				{
					return (CommandResult)6;
				}
				ILumberjackDeviceApi apiProvider = ApiProvider;
				if (apiProvider == null)
				{
					return (CommandResult)6;
				}
				if (!IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
				{
					return (CommandResult)6;
				}
				RvCloudDeviceComponentCommand componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandClimateZone(ComponentIdEx.MakeComponentId(((ILogicalDevice)logicalDevice).LogicalId), command);
				await SendCommandAsync(apiProvider, componentCommand, cancelToken);
				return (CommandResult)0;
			}
			catch (OperationCanceledException)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
				return (CommandResult)1;
			}
			catch (TimeoutException)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
				return (CommandResult)5;
			}
			catch (global::System.Exception ex3)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of " + ex3.Message, global::System.Array.Empty<object>());
				return (CommandResult)7;
			}
		}

		[AsyncStateMachine(typeof(<GetSoftwarePartNumberAsync>d__70))]
		public async global::System.Threading.Tasks.Task<string> GetSoftwarePartNumberAsync(ILogicalDevice logicalDevice, CancellationToken cancelToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			ILumberjackDeviceApi apiProviderForOnlineDevicePidOperations = GetApiProviderForOnlineDevicePidOperations(logicalDevice);
			string text = ComponentIdEx.MakeComponentId(logicalDevice.LogicalId);
			RvCloudDeviceComponentCommandMetadataGet componentCommand = new RvCloudDeviceComponentCommandMetadataGet(text);
			MetadataTracker.Key commandKey = MetadataTracker.MakeKey(text);
			MetadataTracker metadataTracker = new MetadataTracker();
			MetadataTracker tracker = _metadataTrackers.GetOrAdd(commandKey, metadataTracker);
			try
			{
				if (tracker == metadataTracker)
				{
					await SendCommandAsync(apiProviderForOnlineDevicePidOperations, (RvCloudDeviceComponentCommand)(object)componentCommand, cancelToken);
				}
				return ((await tracker.WaitForResultAsync(cancelToken).ConfigureAwait(false)) ?? throw new global::System.Exception("Unable to get metadata")).SoftwarePartNumber;
			}
			catch (OperationCanceledException ex)
			{
				OperationCanceledException ex2 = ex;
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because it was canceled: " + ((global::System.Exception)(object)ex2).Message, global::System.Array.Empty<object>());
				tracker.TrySetException((global::System.Exception)(object)ex2);
				throw new LogicalDevicePidCanceledException();
			}
			catch (TimeoutException ex3)
			{
				TimeoutException ex4 = ex3;
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of timeout: " + ((global::System.Exception)(object)ex4).Message, global::System.Array.Empty<object>());
				tracker.TrySetException((global::System.Exception)(object)ex4);
				throw new LogicalDevicePidTimeoutException();
			}
			catch (global::System.Exception ex5)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of " + ex5.Message, global::System.Array.Empty<object>());
				tracker.TrySetException(ex5);
				throw;
			}
			finally
			{
				DictionaryExtension.TryRemove<MetadataTracker.Key, MetadataTracker>((IDictionary<MetadataTracker.Key, MetadataTracker>)(object)_metadataTrackers, commandKey);
			}
		}

		public Version? GetDeviceProtocolVersion(ILogicalDevice logicalDevice)
		{
			return _logicalDevicesOnlineManager.GetLastKnowProtocolVersion(logicalDevice);
		}

		[AsyncStateMachine(typeof(<PidReadAsync>d__74))]
		public async global::System.Threading.Tasks.Task<UInt48> PidReadAsync(ILogicalDevice logicalDevice, Pid pid, Action<float, string> readProgress, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			ILumberjackDeviceApi apiProviderForOnlineDevicePidOperations = GetApiProviderForOnlineDevicePidOperations(logicalDevice);
			string text = ComponentIdEx.MakeComponentId(logicalDevice.LogicalId);
			RvCloudDeviceComponentCommandPidRead componentCommand = new RvCloudDeviceComponentCommandPidRead(text, pid);
			PidReadTracker.Key pidKey = PidReadTracker.MakeKey(text, pid);
			PidReadTracker pidReadTracker = new PidReadTracker();
			PidReadTracker pidTracker = _pidReadTrackers.GetOrAdd(pidKey, pidReadTracker);
			try
			{
				if (pidTracker == pidReadTracker)
				{
					await SendCommandAsync(apiProviderForOnlineDevicePidOperations, (RvCloudDeviceComponentCommand)(object)componentCommand, cancellationToken);
				}
				return await pidTracker.WaitForResultAsync(cancellationToken).ConfigureAwait(false);
			}
			catch (OperationCanceledException ex)
			{
				OperationCanceledException ex2 = ex;
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
				pidTracker.TrySetException((global::System.Exception)(object)ex2);
				throw new LogicalDevicePidCanceledException();
			}
			catch (TimeoutException ex3)
			{
				TimeoutException ex4 = ex3;
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
				pidTracker.TrySetException((global::System.Exception)(object)ex4);
				throw new LogicalDevicePidTimeoutException();
			}
			catch (global::System.Exception ex5)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of " + ex5.Message, global::System.Array.Empty<object>());
				pidTracker.TrySetException(ex5);
				throw new LogicalDevicePidValueReadException(PidExtension.ConvertToPid(pid), logicalDevice, ex5);
			}
			finally
			{
				DictionaryExtension.TryRemove<PidReadTracker.Key, PidReadTracker>((IDictionary<PidReadTracker.Key, PidReadTracker>)(object)_pidReadTrackers, pidKey);
			}
		}

		private ILumberjackDeviceApi GetApiProviderForOnlineDevicePidOperations(ILogicalDevice logicalDevice)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			if (!IsLogicalDeviceSupported(logicalDevice))
			{
				throw new LogicalDevicePidException($"Logical device {logicalDevice} not supported", (global::System.Exception)null);
			}
			if (!((BackgroundOperationBase)this).Started)
			{
				throw new LogicalDevicePidException($"Logical device {logicalDevice} is offline (service not started)", (global::System.Exception)null);
			}
			if (!IsConnected)
			{
				throw new LogicalDevicePidException($"Logical device {logicalDevice} is offline (service not connected)", (global::System.Exception)null);
			}
			ILumberjackDeviceApi? result = ApiProvider ?? throw new LogicalDevicePidException($"Logical device {logicalDevice} is offline (missing API provider)", (global::System.Exception)null);
			if (!IsLogicalDeviceOnline(logicalDevice))
			{
				throw new LogicalDevicePidException($"Logical device {logicalDevice} is offline", (global::System.Exception)null);
			}
			return result;
		}

		[AsyncStateMachine(typeof(<PidWriteAsync>d__76))]
		public global::System.Threading.Tasks.Task PidWriteAsync(ILogicalDevice logicalDevice, Pid pid, UInt48 value, LogicalDeviceSessionType pidWriteAccess, Action<float, string> writeProgress, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			<PidWriteAsync>d__76 <PidWriteAsync>d__ = default(<PidWriteAsync>d__76);
			<PidWriteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PidWriteAsync>d__.<>4__this = this;
			<PidWriteAsync>d__.logicalDevice = logicalDevice;
			<PidWriteAsync>d__.pid = pid;
			<PidWriteAsync>d__.value = value;
			<PidWriteAsync>d__.pidWriteAccess = pidWriteAccess;
			<PidWriteAsync>d__.cancellationToken = cancellationToken;
			<PidWriteAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <PidWriteAsync>d__.<>t__builder)).Start<<PidWriteAsync>d__76>(ref <PidWriteAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <PidWriteAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<PidReadAsync>d__79))]
		public async global::System.Threading.Tasks.Task<uint> PidReadAsync(ILogicalDevice logicalDevice, Pid pid, ushort address, Action<float, string> readProgress, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			ILumberjackDeviceApi apiProviderForOnlineDevicePidOperations = GetApiProviderForOnlineDevicePidOperations(logicalDevice);
			string text = ComponentIdEx.MakeComponentId(logicalDevice.LogicalId);
			RvCloudDeviceComponentCommandPidReadWithAddress componentCommand = new RvCloudDeviceComponentCommandPidReadWithAddress(text, pid, address);
			PidIndexReadTracker.Key pidKey = PidIndexReadTracker.MakeKey(text, pid, address);
			PidIndexReadTracker pidIndexReadTracker = new PidIndexReadTracker();
			PidIndexReadTracker pidTracker = _pidIndexReadTrackers.GetOrAdd(pidKey, pidIndexReadTracker);
			try
			{
				if (pidTracker == pidIndexReadTracker)
				{
					await SendCommandAsync(apiProviderForOnlineDevicePidOperations, (RvCloudDeviceComponentCommand)(object)componentCommand, cancellationToken);
				}
				return await pidTracker.WaitForResultAsync(cancellationToken).ConfigureAwait(false);
			}
			catch (OperationCanceledException ex)
			{
				OperationCanceledException ex2 = ex;
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
				pidTracker.TrySetException((global::System.Exception)(object)ex2);
				throw new LogicalDevicePidCanceledException();
			}
			catch (TimeoutException ex3)
			{
				TimeoutException ex4 = ex3;
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
				pidTracker.TrySetException((global::System.Exception)(object)ex4);
				throw new LogicalDevicePidTimeoutException();
			}
			catch (global::System.Exception ex5)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of " + ex5.Message, global::System.Array.Empty<object>());
				pidTracker.TrySetException(ex5);
				throw new LogicalDevicePidValueReadException(PidExtension.ConvertToPid(pid), logicalDevice, ex5);
			}
			finally
			{
				DictionaryExtension.TryRemove<PidIndexReadTracker.Key, PidIndexReadTracker>((IDictionary<PidIndexReadTracker.Key, PidIndexReadTracker>)(object)_pidIndexReadTrackers, pidKey);
			}
		}

		[AsyncStateMachine(typeof(<PidWriteAsync>d__80))]
		public global::System.Threading.Tasks.Task PidWriteAsync(ILogicalDevice logicalDevice, Pid pid, ushort address, uint value, LogicalDeviceSessionType pidWriteAccess, Action<float, string> writeProgress, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			<PidWriteAsync>d__80 <PidWriteAsync>d__ = default(<PidWriteAsync>d__80);
			<PidWriteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PidWriteAsync>d__.<>4__this = this;
			<PidWriteAsync>d__.logicalDevice = logicalDevice;
			<PidWriteAsync>d__.pid = pid;
			<PidWriteAsync>d__.address = address;
			<PidWriteAsync>d__.value = value;
			<PidWriteAsync>d__.pidWriteAccess = pidWriteAccess;
			<PidWriteAsync>d__.cancellationToken = cancellationToken;
			<PidWriteAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <PidWriteAsync>d__.<>t__builder)).Start<<PidWriteAsync>d__80>(ref <PidWriteAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <PidWriteAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<SendDirectCommandGeneratorGenie>d__81))]
		public async global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandGeneratorGenie(ILogicalDeviceGeneratorGenie logicalDevice, GeneratorGenieCommand command, CancellationToken cancelToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (!IsLogicalDeviceSupported((ILogicalDevice?)(object)logicalDevice))
				{
					return (CommandResult)13;
				}
				if (!((BackgroundOperationBase)this).Started)
				{
					return (CommandResult)6;
				}
				if (!IsConnected)
				{
					return (CommandResult)6;
				}
				ILumberjackDeviceApi apiProvider = ApiProvider;
				if (apiProvider == null)
				{
					return (CommandResult)6;
				}
				if (!IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
				{
					return (CommandResult)6;
				}
				string text = ComponentIdEx.MakeComponentId(((ILogicalDevice)logicalDevice).LogicalId);
				RvCloudDeviceComponentCommand componentCommand;
				if ((int)command != 1)
				{
					if ((int)command != 2)
					{
						throw new RvCloudIoTComponentCommandInvalidException($"Unknown Generator Genie Command Mode for {command}", (global::System.Exception)null);
					}
					componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandGeneratorGenieOn(text);
				}
				else
				{
					componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandGeneratorGenieOff(text);
				}
				await SendCommandAsync(apiProvider, componentCommand, cancelToken);
				return (CommandResult)0;
			}
			catch (OperationCanceledException)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
				return (CommandResult)1;
			}
			catch (TimeoutException)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
				return (CommandResult)5;
			}
			catch (global::System.Exception ex3)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of " + ex3.Message, global::System.Array.Empty<object>());
				return (CommandResult)7;
			}
		}

		[AsyncStateMachine(typeof(<SendDirectCommandLightDimmable>d__82))]
		public async global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandLightDimmable(ILogicalDeviceLightDimmable logicalDevice, LogicalDeviceLightDimmableCommand command, CancellationToken cancelToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (!IsLogicalDeviceSupported((ILogicalDevice?)(object)logicalDevice))
				{
					return (CommandResult)13;
				}
				if (!((BackgroundOperationBase)this).Started)
				{
					return (CommandResult)6;
				}
				if (!IsConnected)
				{
					return (CommandResult)6;
				}
				ILumberjackDeviceApi apiProvider = ApiProvider;
				if (apiProvider == null)
				{
					return (CommandResult)6;
				}
				if (!IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
				{
					return (CommandResult)6;
				}
				string text = ComponentIdEx.MakeComponentId(((ILogicalDevice)logicalDevice).LogicalId);
				DimmableLightCommand command2 = command.Command;
				RvCloudDeviceComponentCommand componentCommand;
				if ((int)command2 != 0)
				{
					if (command2 - 1 > 2)
					{
						if ((int)command2 != 127)
						{
							throw new RvCloudIoTComponentCommandInvalidException($"Unknown Dimmable Command Mode for {command.Command}", (global::System.Exception)null);
						}
						componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightDimmableRestore(text, command);
					}
					else
					{
						componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightDimmableSetState(text, command);
					}
				}
				else
				{
					componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightDimmableSetStateOff(text, command);
				}
				await SendCommandAsync(apiProvider, componentCommand, cancelToken);
				return (CommandResult)0;
			}
			catch (OperationCanceledException)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
				return (CommandResult)1;
			}
			catch (TimeoutException)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
				return (CommandResult)5;
			}
			catch (global::System.Exception ex3)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of " + ex3.Message, global::System.Array.Empty<object>());
				return (CommandResult)7;
			}
		}

		[AsyncStateMachine(typeof(<SendDirectCommandLightRgb>d__83))]
		public async global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandLightRgb(ILogicalDeviceLightRgb logicalDevice, LogicalDeviceLightRgbCommand command, CancellationToken cancelToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (!IsLogicalDeviceSupported((ILogicalDevice?)(object)logicalDevice))
				{
					return (CommandResult)13;
				}
				if (!((BackgroundOperationBase)this).Started)
				{
					return (CommandResult)6;
				}
				if (!IsConnected)
				{
					return (CommandResult)6;
				}
				ILumberjackDeviceApi apiProvider = ApiProvider;
				if (apiProvider == null)
				{
					return (CommandResult)6;
				}
				if (!IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
				{
					return (CommandResult)6;
				}
				string text = ComponentIdEx.MakeComponentId(((ILogicalDevice)logicalDevice).LogicalId);
				RgbLightModeCommand mode = command.Mode;
				RvCloudDeviceComponentCommand componentCommand;
				switch ((int)mode)
				{
				default:
					if ((int)mode != 127)
					{
						goto case 3;
					}
					componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightRgbRestore(text, command);
					break;
				case 0:
					componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightRgbSetStateOff(text, command);
					break;
				case 1:
					componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightRgbSetStateOn(text, command);
					break;
				case 2:
					componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightRgbSetStateBlink(text, command);
					break;
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
					componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandLightRgbSetStateTransition(text, command);
					break;
				case 3:
					throw new RvCloudIoTComponentCommandInvalidException($"Unknown RGB Command Mode for {command.Mode}", (global::System.Exception)null);
				}
				await SendCommandAsync(apiProvider, componentCommand, cancelToken);
				return (CommandResult)0;
			}
			catch (OperationCanceledException)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
				return (CommandResult)1;
			}
			catch (TimeoutException)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
				return (CommandResult)5;
			}
			catch (global::System.Exception ex3)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of " + ex3.Message, global::System.Array.Empty<object>());
				return (CommandResult)7;
			}
		}

		public bool IsLogicalDeviceRenameSupported(ILogicalDevice? logicalDevice)
		{
			return IsLogicalDeviceSupported(logicalDevice);
		}

		[AsyncStateMachine(typeof(<RenameLogicalDevice>d__85))]
		public global::System.Threading.Tasks.Task RenameLogicalDevice(ILogicalDevice? logicalDevice, FUNCTION_NAME toName, byte toFunctionInstance, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			<RenameLogicalDevice>d__85 <RenameLogicalDevice>d__ = default(<RenameLogicalDevice>d__85);
			<RenameLogicalDevice>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RenameLogicalDevice>d__.<>4__this = this;
			<RenameLogicalDevice>d__.logicalDevice = logicalDevice;
			<RenameLogicalDevice>d__.toName = toName;
			<RenameLogicalDevice>d__.toFunctionInstance = toFunctionInstance;
			<RenameLogicalDevice>d__.cancellationToken = cancellationToken;
			<RenameLogicalDevice>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <RenameLogicalDevice>d__.<>t__builder)).Start<<RenameLogicalDevice>d__85>(ref <RenameLogicalDevice>d__);
			return ((AsyncTaskMethodBuilder)(ref <RenameLogicalDevice>d__.<>t__builder)).Task;
		}

		public bool CanHandleMessage(CloudToDeviceMessage message)
		{
			return true;
		}

		[AsyncStateMachine(typeof(<HandleMessageAsync>d__87))]
		public async global::System.Threading.Tasks.Task<CloudToDeviceMessageHandlerResult> HandleMessageAsync(CloudToDeviceMessage message, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				string text = default(string);
				if (message.MessageSchema == "Lumberjack.MessageSchema.DeviceConnection" && message.Properties.TryGetValue("ConnectionState", ref text))
				{
					if (!(text == "deviceDisconnected"))
					{
						if (text == "deviceConnected")
						{
							TaggedLog.Information("DirectConnectionRvCloudIoT", "deviceConnected message received.", global::System.Array.Empty<object>());
							this.DidConnectEvent?.Invoke((ILogicalDeviceSourceDirectConnection)(object)this);
							return CloudToDeviceMessageHandlerResult.Complete;
						}
						return CloudToDeviceMessageHandlerResult.Reject;
					}
					TaggedLog.Information("DirectConnectionRvCloudIoT", "Taking devices offline.", global::System.Array.Empty<object>());
					_logicalDevicesOnlineManager.AutoTakeDevicesOffline(forceAllOffline: true);
					this.DidDisconnectEvent?.Invoke((ILogicalDeviceSourceDirectConnection)(object)this);
					return CloudToDeviceMessageHandlerResult.Complete;
				}
				if (message.MessageSchema != "Lumberjack.MessageSchema.ComponentInformation")
				{
					return CloudToDeviceMessageHandlerResult.Reject;
				}
				JsonSerializerSettings val = new JsonSerializerSettings();
				val.ContractResolver = (IContractResolver)new SchemaAwareContractResolver(new global::System.Type[1] { typeof(IDeviceComponent) });
				JsonSerializerSettings val2 = val;
				IDeviceComponent val3 = JsonConvert.DeserializeObject<IDeviceComponent>(message.Body, val2);
				RvCloudDeviceComponentStatus val4 = (RvCloudDeviceComponentStatus)(object)((val3 is RvCloudDeviceComponentStatus) ? val3 : null);
				if (val4 == null)
				{
					RvCloudDeviceComponentResponsePidValue val5 = (RvCloudDeviceComponentResponsePidValue)(object)((val3 is RvCloudDeviceComponentResponsePidValue) ? val3 : null);
					if (val5 == null)
					{
						RvCloudDeviceComponentResponsePidValueIndex val6 = (RvCloudDeviceComponentResponsePidValueIndex)(object)((val3 is RvCloudDeviceComponentResponsePidValueIndex) ? val3 : null);
						if (val6 == null)
						{
							RvCloudDeviceComponentResponseMetadata val7 = (RvCloudDeviceComponentResponseMetadata)(object)((val3 is RvCloudDeviceComponentResponseMetadata) ? val3 : null);
							if (val7 == null)
							{
								throw new RvCloudIoTException("Unable to deserialize " + ((MemberInfo)((object)val3).GetType()).Name + ": " + message.MessageSchema);
							}
							TaggedLog.Information("DirectConnectionRvCloudIoT", $"{LogPrefix} Received: {val7}", global::System.Array.Empty<object>());
							MetadataTracker.Key key = MetadataTracker.MakeKey(val7.ComponentId);
							MetadataTracker metadataTracker = default(MetadataTracker);
							if (!_metadataTrackers.TryGetValue(key, ref metadataTracker))
							{
								return CloudToDeviceMessageHandlerResult.Abandon;
							}
							metadataTracker.TrySetResult(val7);
							DictionaryExtension.TryRemove<MetadataTracker.Key, MetadataTracker>((IDictionary<MetadataTracker.Key, MetadataTracker>)(object)_metadataTrackers, key);
							TaggedLog.Information("DirectConnectionRvCloudIoT", $"{LogPrefix} Applied: {val7}", global::System.Array.Empty<object>());
						}
						else
						{
							TaggedLog.Information("DirectConnectionRvCloudIoT", $"{LogPrefix} Received: {val6}", global::System.Array.Empty<object>());
							PidIndexReadTracker.Key key2 = PidIndexReadTracker.MakeKey(val6.ComponentId, val6.Pid, val6.Address);
							PidIndexReadTracker pidIndexReadTracker = default(PidIndexReadTracker);
							if (!_pidIndexReadTrackers.TryGetValue(key2, ref pidIndexReadTracker))
							{
								return CloudToDeviceMessageHandlerResult.Abandon;
							}
							pidIndexReadTracker.TrySetResult((uint)val6.Value);
							DictionaryExtension.TryRemove<PidIndexReadTracker.Key, PidIndexReadTracker>((IDictionary<PidIndexReadTracker.Key, PidIndexReadTracker>)(object)_pidIndexReadTrackers, key2);
							TaggedLog.Information("DirectConnectionRvCloudIoT", $"{LogPrefix} Applied: {val6}", global::System.Array.Empty<object>());
						}
					}
					else
					{
						TaggedLog.Information("DirectConnectionRvCloudIoT", $"{LogPrefix} Received: {val5}", global::System.Array.Empty<object>());
						PidReadTracker.Key key3 = PidReadTracker.MakeKey(val5.ComponentId, val5.Pid);
						PidReadTracker pidReadTracker = default(PidReadTracker);
						if (!_pidReadTrackers.TryGetValue(key3, ref pidReadTracker))
						{
							return CloudToDeviceMessageHandlerResult.Abandon;
						}
						pidReadTracker.TrySetResult((UInt48)val5.Value);
						DictionaryExtension.TryRemove<PidReadTracker.Key, PidReadTracker>((IDictionary<PidReadTracker.Key, PidReadTracker>)(object)_pidReadTrackers, key3);
						TaggedLog.Information("DirectConnectionRvCloudIoT", $"{LogPrefix} Applied: {val5}", global::System.Array.Empty<object>());
					}
				}
				else
				{
					ApplyComponentStatus(val4);
				}
			}
			catch (global::System.Exception ex)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", $"Error processing message for {message.MessageSchema}: {message.Body} because {ex.Message}", global::System.Array.Empty<object>());
				return CloudToDeviceMessageHandlerResult.Reject;
			}
			return CloudToDeviceMessageHandlerResult.Complete;
		}

		public void ApplyComponentStatus(RvCloudDeviceComponentStatus componentStatus)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (componentStatus == null)
			{
				throw new ArgumentNullException("componentStatus");
			}
			ILogicalDeviceWithStatus logicalDevice = componentStatus.GetLogicalDevice(DeviceService, (ILogicalDeviceSource)(object)this);
			if (logicalDevice == null)
			{
				throw new RvCloudIoTComponentException($"Unable to create/get logical device for {this}", (global::System.Exception)null);
			}
			if ((int)componentStatus.ActiveConnection == 0)
			{
				_logicalDevicesOnlineManager.TakeLogicalDeviceOffline((ILogicalDevice)(object)logicalDevice);
			}
			else
			{
				_logicalDevicesOnlineManager.TakeLogicalDeviceOnline((ILogicalDevice)(object)logicalDevice, componentStatus.ProtocolVersion);
			}
			componentStatus.UpdateLogicalDevice(DeviceService, (ILogicalDeviceSource)(object)this, (ILogicalDevice)(object)logicalDevice);
		}

		[AsyncStateMachine(typeof(<SendDirectCommandRelayBasicSwitch>d__89))]
		public async global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandRelayBasicSwitch(ILogicalDeviceSwitchable logicalDevice, bool turnOn, CancellationToken cancelToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (!IsLogicalDeviceSupported((ILogicalDevice?)(object)logicalDevice))
				{
					return (CommandResult)13;
				}
				if (!((BackgroundOperationBase)this).Started)
				{
					return (CommandResult)6;
				}
				if (!IsConnected)
				{
					return (CommandResult)6;
				}
				ILumberjackDeviceApi apiProvider = ApiProvider;
				if (apiProvider == null)
				{
					return (CommandResult)6;
				}
				if (!IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
				{
					return (CommandResult)6;
				}
				string text = ComponentIdEx.MakeComponentId(((ILogicalDevice)logicalDevice).LogicalId);
				RvCloudDeviceComponentCommand componentCommand;
				if (!(logicalDevice is LogicalDeviceRelayBasicLatchingType1))
				{
					if (logicalDevice is LogicalDeviceRelayBasicLatchingType2)
					{
						componentCommand = (RvCloudDeviceComponentCommand)((!turnOn) ? new RvCloudDeviceComponentCommandRelayBasicType2SetState(text, new LogicalDeviceRelayBasicLatchingCommandType2((EnhancedCommand)0)) : new RvCloudDeviceComponentCommandRelayBasicType2SetState(text, new LogicalDeviceRelayBasicLatchingCommandType2((EnhancedCommand)1)));
					}
					else if (logicalDevice is ILogicalDeviceLightDimmable)
					{
						componentCommand = (RvCloudDeviceComponentCommand)((!turnOn) ? ((object)new RvCloudDeviceComponentCommandLightDimmableSetStateOff(text, new LogicalDeviceLightDimmableCommand())) : ((object)new RvCloudDeviceComponentCommandLightDimmableRestore(text, LogicalDeviceLightDimmableCommand.MakeRestoreCommand())));
					}
					else if (logicalDevice is ILogicalDeviceLightRgb)
					{
						componentCommand = (RvCloudDeviceComponentCommand)((!turnOn) ? ((object)new RvCloudDeviceComponentCommandLightRgbSetStateOff(text, new LogicalDeviceLightRgbCommand())) : ((object)new RvCloudDeviceComponentCommandLightRgbRestore(text, LogicalDeviceLightRgbCommand.MakeRestoreCommand())));
					}
					else
					{
						if (!(logicalDevice is ILogicalDeviceGeneratorGenie))
						{
							throw new RvCloudIoTComponentCommandNotSupportedException($"Switchable command not supported/implemented for {logicalDevice}", (global::System.Exception)null);
						}
						componentCommand = (RvCloudDeviceComponentCommand)((!turnOn) ? ((object)new RvCloudDeviceComponentCommandGeneratorGenieOff(text)) : ((object)new RvCloudDeviceComponentCommandGeneratorGenieOn(text)));
					}
				}
				else
				{
					componentCommand = (RvCloudDeviceComponentCommand)new RvCloudDeviceComponentCommandRelayBasicType1SetState(text, new LogicalDeviceRelayBasicCommandType1(turnOn));
				}
				await SendCommandAsync(apiProvider, componentCommand, cancelToken);
				return (CommandResult)0;
			}
			catch (OperationCanceledException)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because it was canceled", global::System.Array.Empty<object>());
				return (CommandResult)1;
			}
			catch (TimeoutException)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of timeout", global::System.Array.Empty<object>());
				return (CommandResult)5;
			}
			catch (global::System.Exception ex3)
			{
				TaggedLog.Warning("DirectConnectionRvCloudIoT", LogPrefix + " Unable to send command because of " + ex3.Message, global::System.Array.Empty<object>());
				return (CommandResult)7;
			}
		}
	}
	public interface ILogicalDeviceSourceDirectConnectionRvCloudIoT : ILogicalDeviceSourceCloudConnection, ILogicalDeviceSourceDirectConnection, ILogicalDeviceSourceDirect, ILogicalDeviceSource, ILogicalDeviceSourceConnection
	{
	}
	public static class LogicalDeviceRemoteEnvironmentRvCloudIoTEx
	{
		public static string? GetGlobalDeviceEndpointRvCloudIoT(this CloudEnvironment environment)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected I4, but got Unknown
			return (int)environment switch
			{
				0 => "global.azure-devices-provisioning.net", 
				1 => "global.azure-devices-provisioning.net", 
				2 => "global.azure-devices-provisioning.net", 
				3 => "global.azure-devices-provisioning.net", 
				_ => null, 
			};
		}

		public static string? GetIdScopeRvCloudIoT(this CloudEnvironment environment)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected I4, but got Unknown
			return (int)environment switch
			{
				0 => "0ne009A3E44", 
				1 => "0ne009A26ED", 
				2 => "0ne009A1BA1", 
				3 => "0ne0099AB12", 
				_ => null, 
			};
		}

		public static string? GetApiBaseAddressRvCloudIoT(this CloudEnvironment environment)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected I4, but got Unknown
			return (int)environment switch
			{
				0 => "https://api.onecontrol.lci1.com/ca/api", 
				1 => "https://api.onecontrolstage.lci1.com/ca/api", 
				2 => "https://api.onecontrolqa.lci1.com/ca/api", 
				3 => "https://api.onecontroldev.lci1.com/ca/api", 
				_ => null, 
			};
		}
	}
	public enum LogicalDeviceOnlineStatus
	{
		Unknown,
		Online,
		Offline
	}
	internal class LogicalDevicesOnlineManager
	{
		public const int WaitTimeForDevicesToBeTakenOfflineMs = 2147483647;

		private readonly ConcurrentDictionary<ILogicalDevice, global::System.DateTime?> _logicalDeviceLastOnlineTimestampDict = new ConcurrentDictionary<ILogicalDevice, global::System.DateTime?>();

		private readonly ConcurrentDictionary<ILogicalDevice, Version?> _logicalDeviceLastKnowProtocolVersion = new ConcurrentDictionary<ILogicalDevice, Version>();

		private readonly Watchdog _takeDevicesOfflineWatchdog;

		private readonly object _lock = new object();

		public LogicalDevicesOnlineManager()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			_takeDevicesOfflineWatchdog = new Watchdog(2147483647, new Action(AutoTakeDevicesOffline), true);
		}

		public void TakeLogicalDeviceOnline(ILogicalDevice logicalDevice, Version? protocolVersion)
		{
			lock (_lock)
			{
				_logicalDeviceLastOnlineTimestampDict[logicalDevice] = global::System.DateTime.Now;
				if (protocolVersion != null)
				{
					_logicalDeviceLastKnowProtocolVersion[logicalDevice] = protocolVersion;
				}
				_takeDevicesOfflineWatchdog.TryPet(true);
			}
			logicalDevice.UpdateDeviceOnline(true);
		}

		public void TakeLogicalDeviceOffline(ILogicalDevice logicalDevice)
		{
			lock (_lock)
			{
				_logicalDeviceLastOnlineTimestampDict[logicalDevice] = null;
			}
			logicalDevice.UpdateDeviceOnline(false);
		}

		public Version? GetLastKnowProtocolVersion(ILogicalDevice logicalDevice)
		{
			Version result = default(Version);
			if (!_logicalDeviceLastKnowProtocolVersion.TryGetValue(logicalDevice, ref result))
			{
				return null;
			}
			return result;
		}

		public LogicalDeviceOnlineStatus GetLogicalDeviceOnlineStatus(ILogicalDevice? logicalDevice)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (logicalDevice == null)
			{
				return LogicalDeviceOnlineStatus.Unknown;
			}
			lock (_lock)
			{
				global::System.DateTime? dateTime = default(global::System.DateTime?);
				if (!_logicalDeviceLastOnlineTimestampDict.TryGetValue(logicalDevice, ref dateTime))
				{
					return LogicalDeviceOnlineStatus.Unknown;
				}
				if (!dateTime.HasValue)
				{
					return LogicalDeviceOnlineStatus.Offline;
				}
				TimeSpan val = global::System.DateTime.Now - dateTime.Value;
				if (((TimeSpan)(ref val)).TotalMilliseconds > 2147483647.0)
				{
					return LogicalDeviceOnlineStatus.Offline;
				}
				return LogicalDeviceOnlineStatus.Online;
			}
		}

		private void AutoTakeDevicesOffline()
		{
			AutoTakeDevicesOffline(forceAllOffline: false);
		}

		public void AutoTakeDevicesOffline(bool forceAllOffline)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			List<ILogicalDevice> val = null;
			lock (_lock)
			{
				global::System.DateTime now = global::System.DateTime.Now;
				global::System.Collections.Generic.IEnumerator<KeyValuePair<ILogicalDevice, global::System.DateTime?>> enumerator = _logicalDeviceLastOnlineTimestampDict.GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
					{
						KeyValuePair<ILogicalDevice, global::System.DateTime?> current = enumerator.Current;
						global::System.DateTime? value = current.Value;
						if (!value.HasValue)
						{
							continue;
						}
						if (!forceAllOffline)
						{
							TimeSpan val2 = now - value.Value;
							if (((TimeSpan)(ref val2)).TotalMilliseconds <= 2147483647.0)
							{
								continue;
							}
						}
						if (val == null)
						{
							val = new List<ILogicalDevice>();
						}
						val.Add(current.Key);
						_logicalDeviceLastOnlineTimestampDict[current.Key] = null;
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator)?.Dispose();
				}
			}
			if (val == null)
			{
				return;
			}
			Enumerator<ILogicalDevice> enumerator2 = val.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.UpdateDeviceOnline(false);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
			}
		}
	}
	public class RvCloudIoTCloudCredential : CloudGatewayCredential
	{
		public RvCloudIoTCloudCredential(IOAuthCredential credential, string gatewayId, CloudEnvironment environment)
			: base(credential, gatewayId, (GatewayAssetRemoteProvider)1, environment)
		{
		}//IL_0004: Unknown result type (might be due to invalid IL or missing references)

	}
	public class RvCloudIoTException : global::System.Exception
	{
		public RvCloudIoTException(string message, global::System.Exception? innerException = null)
			: base(message, innerException)
		{
		}
	}
	public class RvCloudIoTDeviceServiceNotStartedException : RvCloudIoTException
	{
		public RvCloudIoTDeviceServiceNotStartedException(IDirectConnectionRvCloudIoT rvCloudIoT, global::System.Exception? innerException = null)
			: base($"{rvCloudIoT}: Device Service Not Started", innerException)
		{
		}
	}
	public class RvCloudIoTDeviceServiceNotConnectedException : RvCloudIoTException
	{
		public RvCloudIoTDeviceServiceNotConnectedException(IDirectConnectionRvCloudIoT rvCloudIoT, global::System.Exception? innerException = null)
			: base($"{rvCloudIoT}: Device Service Not Connected", innerException)
		{
		}
	}
	public class RvCloudIoTDeviceOfflineException : RvCloudIoTException
	{
		public RvCloudIoTDeviceOfflineException(IDirectConnectionRvCloudIoT rvCloudIoT, ILogicalDevice logicalDevice, global::System.Exception? innerException = null)
			: base($"{rvCloudIoT}: Device offline for {logicalDevice}", innerException)
		{
		}
	}
	public class RvCloudIoTDeviceNotFoundException : RvCloudIoTException
	{
		public RvCloudIoTDeviceNotFoundException(IDirectConnectionRvCloudIoT rvCloudIoT, ILogicalDevice logicalDevice, global::System.Exception? innerException = null)
			: base($"{rvCloudIoT}: Unable to find device for {logicalDevice}", innerException)
		{
		}
	}
	public class RvCloudIoTDeviceOperationNotSupported : RvCloudIoTException
	{
		public RvCloudIoTDeviceOperationNotSupported(IDirectConnectionRvCloudIoT rvCloudIoT, string operation, ILogicalDevice? logicalDevice, global::System.Exception? innerException = null)
			: base($"{rvCloudIoT}: Operation {operation} not supported for the device {((object)logicalDevice)?.ToString() ?? "Unknown"}", innerException)
		{
		}
	}
}
namespace OneControl.Direct.RvCloudIoT.LogicalDeviceSession
{
	internal interface ILogicalDeviceSessionRvCloudIoT : ILogicalDeviceSession
	{
		bool IsActivated { get; }
	}
	internal class LogicalDeviceSessionRvCloudIoT : ILogicalDeviceSessionRvCloudIoT, ILogicalDeviceSession
	{
		private long _lastActivatedTimestampMs;

		public bool IsActivated => _lastActivatedTimestampMs != 0;

		internal void ActivateSession()
		{
			_lastActivatedTimestampMs = LogicalDeviceFreeRunningTimer.ElapsedMilliseconds;
		}

		internal void DeactivateSession()
		{
			_lastActivatedTimestampMs = 0L;
		}
	}
	internal class RvCloudIoTSessionManager : CommonDisposable, ILogicalDeviceSessionManager, ICommonDisposable, global::System.IDisposable
	{
		private const string LogTag = "RvCloudIoTSessionManager";

		private readonly IDirectConnectionRvCloudIoT _directManager;

		private readonly ConcurrentDictionary<ValueTuple<LogicalDeviceSessionType, ILogicalDevice>, LogicalDeviceSessionRvCloudIoT> _sessionDict;

		public RvCloudIoTSessionManager(IDirectConnectionRvCloudIoT directManager)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			_directManager = directManager ?? throw new ArgumentNullException("directManager");
			_sessionDict = new ConcurrentDictionary<ValueTuple<LogicalDeviceSessionType, ILogicalDevice>, LogicalDeviceSessionRvCloudIoT>();
		}

		public bool IsSessionActive(LogicalDeviceSessionType sessionType, ILogicalDevice logicalDevice)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			LogicalDeviceSessionRvCloudIoT logicalDeviceSessionRvCloudIoT = default(LogicalDeviceSessionRvCloudIoT);
			if (!_sessionDict.TryGetValue(new ValueTuple<LogicalDeviceSessionType, ILogicalDevice>(sessionType, logicalDevice), ref logicalDeviceSessionRvCloudIoT))
			{
				return false;
			}
			if (logicalDeviceSessionRvCloudIoT.IsActivated && _directManager != logicalDevice.DeviceService.GetPrimaryDeviceSourceDirect(logicalDevice))
			{
				TaggedLog.Information("RvCloudIoTSessionManager", "RvCloudIoTSessionManager Auto deactivating session because device no longer being controlled via this RvLink", global::System.Array.Empty<object>());
				DeactivateSession(sessionType, logicalDevice, closeSession: true);
			}
			return logicalDeviceSessionRvCloudIoT.IsActivated;
		}

		public global::System.Threading.Tasks.Task<ILogicalDeviceSession> ActivateSessionAsync(LogicalDeviceSessionType sessionType, ILogicalDevice logicalDevice, CancellationToken cancelToken, uint msSessionKeepAliveTime = 15000u, uint msSessionGetTimeout = 3000u)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Invalid comparison between Unknown and I4
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			if (!((ILogicalDeviceSourceDirect)_directManager).DeviceService.SessionsEnabled)
			{
				throw new ActivateSessionDisabledException("RvCloudIoTSessionManager");
			}
			if (((CommonDisposable)this).IsDisposed)
			{
				throw new ObjectDisposedException("ActivateSessionAsync not possible because RvCloudIoTSessionManager Is Disposed");
			}
			if ((int)((IDevicesCommon)logicalDevice).ActiveConnection == 2)
			{
				throw new ActivateSessionRemoteActiveException("RvCloudIoTSessionManager");
			}
			if (!((ILogicalDeviceSourceDirect)_directManager).IsLogicalDeviceOnline(logicalDevice))
			{
				throw new PhysicalDeviceNotFoundException("RvCloudIoTSessionManager", logicalDevice, "");
			}
			if (((ICommonDisposable)logicalDevice).IsDisposed)
			{
				throw new ObjectDisposedException("logicalDevice", "Logical Device Is Disposed");
			}
			if (InTransitLockoutStatusExtension.IsInLockout(logicalDevice.InTransitLockout))
			{
				throw new ActivateSessionEnforcedInTransitLockout("RvCloudIoTSessionManager", logicalDevice);
			}
			LogicalDeviceSessionRvCloudIoT session = default(LogicalDeviceSessionRvCloudIoT);
			if (!_sessionDict.TryGetValue(new ValueTuple<LogicalDeviceSessionType, ILogicalDevice>(sessionType, logicalDevice), ref session))
			{
				session = new LogicalDeviceSessionRvCloudIoT();
				_sessionDict.AddOrUpdate(new ValueTuple<LogicalDeviceSessionType, ILogicalDevice>(sessionType, logicalDevice), session, (Func<ValueTuple<LogicalDeviceSessionType, ILogicalDevice>, LogicalDeviceSessionRvCloudIoT, LogicalDeviceSessionRvCloudIoT>)((ValueTuple<LogicalDeviceSessionType, ILogicalDevice> key, LogicalDeviceSessionRvCloudIoT oldValue) => session));
			}
			session.ActivateSession();
			logicalDevice.UpdateSessionChanged(LogicalDevicePidWriteAccessExtension.ToIdsCanSessionId(sessionType));
			return global::System.Threading.Tasks.Task.FromResult<ILogicalDeviceSession>((ILogicalDeviceSession)(object)session);
		}

		public void DeactivateSession(LogicalDeviceSessionType sessionType, ILogicalDevice logicalDevice, bool closeSession)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			LogicalDeviceSessionRvCloudIoT logicalDeviceSessionRvCloudIoT = default(LogicalDeviceSessionRvCloudIoT);
			if (_sessionDict.TryGetValue(new ValueTuple<LogicalDeviceSessionType, ILogicalDevice>(sessionType, logicalDevice), ref logicalDeviceSessionRvCloudIoT))
			{
				logicalDeviceSessionRvCloudIoT.DeactivateSession();
				logicalDevice.UpdateSessionChanged(LogicalDevicePidWriteAccessExtension.ToIdsCanSessionId(sessionType));
			}
		}

		public void CloseAllSessions()
		{
			_sessionDict.Clear();
		}

		public override void Dispose(bool disposing)
		{
			CloseAllSessions();
		}
	}
}
