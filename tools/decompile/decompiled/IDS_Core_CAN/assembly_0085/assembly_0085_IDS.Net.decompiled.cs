using System;
using System.Buffers;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IDS.Core;
using IDS.Core.Events;
using IDS.Core.IDS_CAN;
using IDS.Core.Tasks;
using IDS.Net.Sockets;
using IDS.Portable.CAN;
using IDS.Portable.CAN.Com;
using IDS.Portable.Com;
using IDS.Portable.Common;
using IDS.Portable.LogicalDevice;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using ids.portable.ble.BleManager;
using ids.portable.ble.Exceptions;
using ids.portable.ble.Extensions;
using ids.portable.ble.ScanResults;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: TargetFramework(".NETCoreApp,Version=v9.0", FrameworkDisplayName = ".NET 9.0")]
[assembly: AssemblyCompany("Jared Allen")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("Copyright 2018-2025")]
[assembly: AssemblyDescription("This plugin provides helper classes for getting connected to IDS CAN via WiFi or Bluetooth. It also provides several classes implemented to help initialize the IDS CAN Core")]
[assembly: AssemblyFileVersion("2.2.0.0")]
[assembly: AssemblyInformationalVersion("2.2.0+fcda087733665ff59e66f14b802bc27a77b26758")]
[assembly: AssemblyProduct("IDS.Portable.CAN")]
[assembly: AssemblyTitle("IDS.Portable.CAN")]
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/lci-ids/ids.portable.can.git")]
[assembly: AssemblyVersion("2.2.0.0")]
[module: RefSafetyRules(11)]
namespace Microsoft.Maui.Controls.Generated
{
	[GeneratedCode("Microsoft.Maui.Controls.BindingSourceGen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "1.0.0.0")]
	internal static class GeneratedBindingInterceptors
	{
		private static bool ShouldUseSetter(BindingMode mode, BindableProperty bindableProperty)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Invalid comparison between Unknown and I4
			if ((int)mode != 3 && (int)mode != 1)
			{
				if ((int)mode == 0)
				{
					if ((int)bindableProperty.DefaultBindingMode != 3)
					{
						return (int)bindableProperty.DefaultBindingMode == 1;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		private static bool ShouldUseSetter(BindingMode mode)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Invalid comparison between Unknown and I4
			if ((int)mode != 3 && (int)mode != 1)
			{
				return (int)mode == 0;
			}
			return true;
		}
	}
}
namespace IDS.Portable.Utility
{
	public static class TcpIpGatewayPasswordGenerator
	{
		private static readonly string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		private const int PASSWORD_LENGTH = 8;

		private static string Reverse(string s)
		{
			string text = "";
			for (int num = s.Length - 1; num >= 0; num--)
			{
				text = string.Concat(string.op_Implicit(text), new global::System.ReadOnlySpan<char>(ref s[num]));
			}
			return text;
		}

		public static string GeneratePasswordFromSsid(string ssid)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrWhiteSpace(ssid))
			{
				throw new ArgumentException("Given ssid is null or empty");
			}
			string[] array = ssid.Split('_', (StringSplitOptions)0);
			if (array.Length != 2)
			{
				throw new ArgumentException("Invalid format of ssid '" + ssid + "', expected something like MyRV_20218D0561600026");
			}
			return GeneratePassword(array[1]);
		}

		public static string GeneratePassword(string barcode)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(barcode))
			{
				throw new ArgumentException("IDS barcode cannot be empty");
			}
			if (barcode.Length != 16)
			{
				throw new ArgumentException("IDS barcode must be 16 digits long");
			}
			string text = barcode.Substring(0, 5);
			uint num = barcode[5];
			string text2 = barcode.Substring(6, 10);
			ulong num2 = ulong.Parse(Reverse(text + text2));
			string text3 = "";
			uint length = (uint)Chars.Length;
			while (text3.Length < 8)
			{
				int num3 = (int)((num2 + num) % length);
				num2 /= length;
				text3 = string.Concat(string.op_Implicit(text3), new global::System.ReadOnlySpan<char>(ref Chars[num3]));
			}
			return text3;
		}
	}
}
namespace IDS.Portable.Services.ConnectionServices
{
	public enum ConnectionAdapterStatus
	{
		Ready,
		Canceled,
		Failed,
		Unavailable
	}
	public class DirectIdsCanAdapterManager : Singleton<DirectIdsCanAdapterManager>
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <PrepareConnectionAsync>d__2 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<ConnectionAdapterStatus> <>t__builder;

			public CancellationToken inCancelToken;

			public IDirectIdsCanConnection connection;

			public DirectIdsCanAdapterManager <>4__this;

			private TaskAwaiter<ConnectionAdapterStatus> <>u__1;

			private void MoveNext()
			{
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0095: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectIdsCanAdapterManager directIdsCanAdapterManager = <>4__this;
				ConnectionAdapterStatus result;
				try
				{
					TaskAwaiter<ConnectionAdapterStatus> val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<ConnectionAdapterStatus>);
						num = (<>1__state = -1);
						goto IL_00c9;
					}
					if (((CancellationToken)(ref inCancelToken)).IsCancellationRequested)
					{
						result = ConnectionAdapterStatus.Canceled;
					}
					else if (!(connection is IDirectIdsCanConnectionTcpIpWired))
					{
						if (!(connection is IDirectIdsCanConnectionTcpIpWifi))
						{
							if (connection is IDirectIdsCanConnectionBle bleConnection)
							{
								val = directIdsCanAdapterManager.PrepareBluetoothConnectionAsync(bleConnection, inCancelToken).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ConnectionAdapterStatus>, <PrepareConnectionAsync>d__2>(ref val, ref this);
									return;
								}
								goto IL_00c9;
							}
							result = ((!(connection is IDirectConnectionNone)) ? ConnectionAdapterStatus.Failed : ConnectionAdapterStatus.Ready);
						}
						else
						{
							result = ConnectionAdapterStatus.Ready;
						}
					}
					else
					{
						result = ConnectionAdapterStatus.Ready;
					}
					goto end_IL_000e;
					IL_00c9:
					result = val.GetResult();
					end_IL_000e:;
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

		private const string LogTag = "DirectIdsCanAdapterManager";

		private DirectIdsCanAdapterManager()
		{
		}

		[AsyncStateMachine(typeof(<PrepareConnectionAsync>d__2))]
		public async global::System.Threading.Tasks.Task<ConnectionAdapterStatus> PrepareConnectionAsync(IDirectIdsCanConnection connection, CancellationToken inCancelToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (((CancellationToken)(ref inCancelToken)).IsCancellationRequested)
			{
				return ConnectionAdapterStatus.Canceled;
			}
			if (!(connection is IDirectIdsCanConnectionTcpIpWired))
			{
				if (!(connection is IDirectIdsCanConnectionTcpIpWifi))
				{
					if (!(connection is IDirectIdsCanConnectionBle bleConnection))
					{
						if (connection is IDirectConnectionNone)
						{
							return ConnectionAdapterStatus.Ready;
						}
						return ConnectionAdapterStatus.Failed;
					}
					return await PrepareBluetoothConnectionAsync(bleConnection, inCancelToken);
				}
				return ConnectionAdapterStatus.Ready;
			}
			return ConnectionAdapterStatus.Ready;
		}

		private global::System.Threading.Tasks.Task<ConnectionAdapterStatus> PrepareBluetoothConnectionAsync(IDirectIdsCanConnectionBle bleConnection, CancellationToken token)
		{
			return global::System.Threading.Tasks.Task.FromResult<ConnectionAdapterStatus>(ConnectionAdapterStatus.Ready);
		}
	}
	public interface IDirectIdsCanConnectionManager : IConnectionManager
	{
		ICanDeviceInfo CanDeviceInfo { get; }

		IAdapter? Gateway { get; }
	}
	public class DirectIdsCanManagedConnection : BackgroundOperationDisposable, IDirectIdsCanConnectionManager, IConnectionManager
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <BackgroundOperationAsync>d__27 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DirectIdsCanManagedConnection <>4__this;

			public CancellationToken cancellationToken;

			private int <connectionRetryTimeMs>5__2;

			private int <>7__wrap2;

			private global::System.Exception <ex>5__4;

			private ConnectionAdapterStatus <connectionStatus>5__5;

			private TaskAwaiter<ConnectionAdapterStatus> <>u__1;

			private TaskAwaiter <>u__2;

			private TaskAwaiter<bool> <>u__3;

			private void MoveNext()
			{
				//IL_0448: Unknown result type (might be due to invalid IL or missing references)
				//IL_044d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0455: Unknown result type (might be due to invalid IL or missing references)
				//IL_050c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0511: Unknown result type (might be due to invalid IL or missing references)
				//IL_0519: Unknown result type (might be due to invalid IL or missing references)
				//IL_0409: Unknown result type (might be due to invalid IL or missing references)
				//IL_0413: Unknown result type (might be due to invalid IL or missing references)
				//IL_0418: Unknown result type (might be due to invalid IL or missing references)
				//IL_042d: Unknown result type (might be due to invalid IL or missing references)
				//IL_042f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_014c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0151: Unknown result type (might be due to invalid IL or missing references)
				//IL_0159: Unknown result type (might be due to invalid IL or missing references)
				//IL_01da: Unknown result type (might be due to invalid IL or missing references)
				//IL_01df: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_0279: Unknown result type (might be due to invalid IL or missing references)
				//IL_027e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0286: Unknown result type (might be due to invalid IL or missing references)
				//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_04da: Unknown result type (might be due to invalid IL or missing references)
				//IL_04df: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0103: Unknown result type (might be due to invalid IL or missing references)
				//IL_0109: Invalid comparison between Unknown and I4
				//IL_010d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0117: Unknown result type (might be due to invalid IL or missing references)
				//IL_011c: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_0131: Unknown result type (might be due to invalid IL or missing references)
				//IL_0133: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_023a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0244: Unknown result type (might be due to invalid IL or missing references)
				//IL_0249: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_025e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0260: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectIdsCanManagedConnection directIdsCanManagedConnection = <>4__this;
				try
				{
					TaskAwaiter val;
					TaskAwaiter<bool> val2;
					int num2;
					switch (num)
					{
					default:
						<connectionRetryTimeMs>5__2 = 1000;
						goto IL_048b;
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
						try
						{
							TaskAwaiter<ConnectionAdapterStatus> val3;
							ConnectionAdapterStatus result;
							switch (num)
							{
							default:
								val3 = Singleton<DirectIdsCanAdapterManager>.Instance.PrepareConnectionAsync(directIdsCanManagedConnection.Connection, cancellationToken).GetAwaiter();
								if (!val3.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val3;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<ConnectionAdapterStatus>, <BackgroundOperationAsync>d__27>(ref val3, ref this);
									return;
								}
								goto IL_00c9;
							case 0:
								val3 = <>u__1;
								<>u__1 = default(TaskAwaiter<ConnectionAdapterStatus>);
								num = (<>1__state = -1);
								goto IL_00c9;
							case 1:
								val = <>u__2;
								<>u__2 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_0168;
							case 2:
								val = <>u__2;
								<>u__2 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_01f6;
							case 3:
								val2 = <>u__3;
								<>u__3 = default(TaskAwaiter<bool>);
								num = (<>1__state = -1);
								goto IL_0295;
							case 4:
								{
									val = <>u__2;
									<>u__2 = default(TaskAwaiter);
									num = (<>1__state = -1);
									break;
								}
								IL_016f:
								if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
								{
									if (directIdsCanManagedConnection.Gateway == null)
									{
										throw new global::System.Exception("Gateway isn't connected");
									}
									if (!((IAdapter)directIdsCanManagedConnection.Gateway).IsConnected)
									{
										val = directIdsCanManagedConnection.CloseConnectionAsync().GetAwaiter();
										if (!((TaskAwaiter)(ref val)).IsCompleted)
										{
											num = (<>1__state = 2);
											<>u__2 = val;
											((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <BackgroundOperationAsync>d__27>(ref val, ref this);
											return;
										}
										goto IL_01f6;
									}
									<connectionRetryTimeMs>5__2 = 1000;
									if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
									{
										val2 = TaskExtension.TryDelay(directIdsCanManagedConnection.ConnectionCheckTimeMs, cancellationToken).GetAwaiter();
										if (!val2.IsCompleted)
										{
											num = (<>1__state = 3);
											<>u__3 = val2;
											((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <BackgroundOperationAsync>d__27>(ref val2, ref this);
											return;
										}
										goto IL_0295;
									}
								}
								goto IL_0382;
								IL_00c9:
								result = val3.GetResult();
								<connectionStatus>5__5 = result;
								if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
								{
									result = <connectionStatus>5__5;
									if (result != ConnectionAdapterStatus.Ready)
									{
										if ((uint)(result - 1) > 2u)
										{
											goto IL_0382;
										}
										val = directIdsCanManagedConnection.CloseConnectionAsync().GetAwaiter();
										if (!((TaskAwaiter)(ref val)).IsCompleted)
										{
											num = (<>1__state = 4);
											<>u__2 = val;
											((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <BackgroundOperationAsync>d__27>(ref val, ref this);
											return;
										}
										break;
									}
									if ((int)directIdsCanManagedConnection.Status != 2)
									{
										val = directIdsCanManagedConnection.CreateAndOpenGatewayAsync(cancellationToken).GetAwaiter();
										if (!((TaskAwaiter)(ref val)).IsCompleted)
										{
											num = (<>1__state = 1);
											<>u__2 = val;
											((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <BackgroundOperationAsync>d__27>(ref val, ref this);
											return;
										}
										goto IL_0168;
									}
									goto IL_016f;
								}
								goto end_IL_0048;
								IL_01f6:
								((TaskAwaiter)(ref val)).GetResult();
								if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
								{
									throw new global::System.Exception("Gateway isn't connected");
								}
								goto IL_0382;
								IL_0295:
								val2.GetResult();
								goto IL_0382;
								IL_0168:
								((TaskAwaiter)(ref val)).GetResult();
								goto IL_016f;
							}
							((TaskAwaiter)(ref val)).GetResult();
							throw new global::System.Exception($"Failed to connect due to {<connectionStatus>5__5}");
							end_IL_0048:;
						}
						catch (global::System.Exception ex) when (((Func<bool>)delegate
						{
							// Could not convert BlockContainer to single expression
							<ex>5__4 = ex;
							return !((CancellationToken)(ref cancellationToken)).IsCancellationRequested || !(<ex>5__4 is OperationCanceledException);
						}).Invoke())
						{
							<>7__wrap2 = 1;
							goto IL_0382;
						}
						catch
						{
							goto IL_0382;
						}
						goto IL_049b;
					case 5:
						val2 = <>u__3;
						<>u__3 = default(TaskAwaiter<bool>);
						num = (<>1__state = -1);
						goto IL_0464;
					case 6:
						{
							val = <>u__2;
							<>u__2 = default(TaskAwaiter);
							num = (<>1__state = -1);
							break;
						}
						IL_048b:
						if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
						{
							<>7__wrap2 = 0;
							goto case 0;
						}
						goto IL_049b;
						IL_0464:
						val2.GetResult();
						<connectionRetryTimeMs>5__2 = Math.Min(<connectionRetryTimeMs>5__2 * 2, 4000);
						goto IL_0484;
						IL_049b:
						TaggedLog.Debug("DirectIdsCanManagedConnection", $"Closing Connection {directIdsCanManagedConnection.Connection}", global::System.Array.Empty<object>());
						val = directIdsCanManagedConnection.CloseConnectionAsync().GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 6);
							<>u__2 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <BackgroundOperationAsync>d__27>(ref val, ref this);
							return;
						}
						break;
						IL_0484:
						<ex>5__4 = null;
						goto IL_048b;
						IL_0382:
						num2 = <>7__wrap2;
						if (num2 == 1)
						{
							TaggedLog.Information("DirectIdsCanManagedConnection", $"Will retry connecting to Gateway connection {directIdsCanManagedConnection.Connection} in {<connectionRetryTimeMs>5__2}ms: {<ex>5__4.Message}", global::System.Array.Empty<object>());
							val2 = TaskExtension.TryDelay(<connectionRetryTimeMs>5__2, cancellationToken).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (<>1__state = 5);
								<>u__3 = val2;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <BackgroundOperationAsync>d__27>(ref val2, ref this);
								return;
							}
							goto IL_0464;
						}
						goto IL_0484;
					}
					((TaskAwaiter)(ref val)).GetResult();
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
		private struct <CloseConnectionAsync>d__29 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DirectIdsCanManagedConnection <>4__this;

			private TaskAwaiter<bool> <>u__1;

			private TaskAwaiter <>u__2;

			private void MoveNext()
			{
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Expected O, but got Unknown
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_012a: Unknown result type (might be due to invalid IL or missing references)
				//IL_012f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0137: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
				//IL_010f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0111: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectIdsCanManagedConnection directIdsCanManagedConnection = <>4__this;
				try
				{
					if (num == 0)
					{
						goto IL_0036;
					}
					if (num == 1)
					{
						goto IL_00eb;
					}
					if (directIdsCanManagedConnection.Gateway != null)
					{
						if (((IAdapter)directIdsCanManagedConnection.Gateway).IsConnected)
						{
							goto IL_0036;
						}
						goto IL_00d2;
					}
					goto end_IL_000e;
					IL_0036:
					try
					{
						TaskAwaiter<bool> val;
						if (num != 0)
						{
							val = ((IAdapter)directIdsCanManagedConnection.Gateway).CloseAsync(new AsyncOperation(TimeSpan.FromSeconds(5L), CancellationToken.None)).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <CloseConnectionAsync>d__29>(ref val, ref this);
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
					catch (OperationCanceledException)
					{
					}
					catch (global::System.Exception ex2)
					{
						TaggedLog.Error("DirectIdsCanManagedConnection", "CloseConnectionAsync Exception - (NG) : " + ex2.StackTrace, global::System.Array.Empty<object>());
					}
					goto IL_00d2;
					IL_00d2:
					((global::System.IDisposable)directIdsCanManagedConnection.Gateway)?.Dispose();
					directIdsCanManagedConnection.Gateway = null;
					goto IL_00eb;
					IL_00eb:
					try
					{
						TaskAwaiter val2;
						if (num != 1)
						{
							val2 = directIdsCanManagedConnection.GatewayDidDisconnect().GetAwaiter();
							if (!((TaskAwaiter)(ref val2)).IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__2 = val2;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <CloseConnectionAsync>d__29>(ref val2, ref this);
								return;
							}
						}
						else
						{
							val2 = <>u__2;
							<>u__2 = default(TaskAwaiter);
							num = (<>1__state = -1);
						}
						((TaskAwaiter)(ref val2)).GetResult();
						directIdsCanManagedConnection.DidDisconnectEvent?.Invoke((IConnectionManager)(object)directIdsCanManagedConnection);
					}
					catch (global::System.Exception ex3)
					{
						TaggedLog.Debug("DirectIdsCanManagedConnection", $"Gateway {directIdsCanManagedConnection.Connection} will close event exception {ex3.Message}", global::System.Array.Empty<object>());
					}
					end_IL_000e:;
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
		private struct <CreateAndOpenGatewayAsync>d__28 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DirectIdsCanManagedConnection <>4__this;

			public CancellationToken cancellationToken;

			private IAdapter <gateway>5__2;

			private string <gatewayName>5__3;

			private TaskAwaiter <>u__1;

			private ICanDeviceInfo <>7__wrap3;

			private TaskAwaiter<IPAddress> <>u__2;

			private TaskAwaiter<bool> <>u__3;

			private void MoveNext()
			{
				//IL_03ba: Expected O, but got Unknown
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_016b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0170: Unknown result type (might be due to invalid IL or missing references)
				//IL_0178: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_038f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0394: Unknown result type (might be due to invalid IL or missing references)
				//IL_039c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0345: Unknown result type (might be due to invalid IL or missing references)
				//IL_034b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0350: Unknown result type (might be due to invalid IL or missing references)
				//IL_035a: Expected O, but got Unknown
				//IL_035a: Unknown result type (might be due to invalid IL or missing references)
				//IL_035f: Unknown result type (might be due to invalid IL or missing references)
				//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
				//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0504: Unknown result type (might be due to invalid IL or missing references)
				//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_0136: Unknown result type (might be due to invalid IL or missing references)
				//IL_013b: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_020e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0223: Unknown result type (might be due to invalid IL or missing references)
				//IL_0374: Unknown result type (might be due to invalid IL or missing references)
				//IL_0376: Unknown result type (might be due to invalid IL or missing references)
				//IL_04de: Unknown result type (might be due to invalid IL or missing references)
				//IL_04df: Unknown result type (might be due to invalid IL or missing references)
				//IL_0150: Unknown result type (might be due to invalid IL or missing references)
				//IL_0152: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DirectIdsCanManagedConnection directIdsCanManagedConnection = <>4__this;
				try
				{
					TaskAwaiter val2;
					TaskAwaiter<IPAddress> val;
					IDirectIdsCanConnection connection;
					IAdapter val3;
					IPAddress result;
					switch (num)
					{
					default:
						val2 = directIdsCanManagedConnection.CloseConnectionAsync().GetAwaiter();
						if (!((TaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <CreateAndOpenGatewayAsync>d__28>(ref val2, ref this);
							return;
						}
						goto IL_0078;
					case 0:
						val2 = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_0078;
					case 1:
						val = <>u__2;
						<>u__2 = default(TaskAwaiter<IPAddress>);
						num = (<>1__state = -1);
						goto IL_0187;
					case 2:
						try
						{
							TaskAwaiter<bool> val4;
							if (num != 2)
							{
								TaggedLog.Debug("DirectIdsCanManagedConnection", $"CreateAndOpenGatewayAsync {directIdsCanManagedConnection.Connection} opening connection to adapter", global::System.Array.Empty<object>());
								val4 = ((IAdapter)<gateway>5__2).OpenAsync(new AsyncOperation(TimeSpan.FromSeconds(45.0), cancellationToken)).GetAwaiter();
								if (!val4.IsCompleted)
								{
									num = (<>1__state = 2);
									<>u__3 = val4;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <CreateAndOpenGatewayAsync>d__28>(ref val4, ref this);
									return;
								}
							}
							else
							{
								val4 = <>u__3;
								<>u__3 = default(TaskAwaiter<bool>);
								num = (<>1__state = -1);
							}
							val4.GetResult();
						}
						catch (OperationCanceledException ex)
						{
							OperationCanceledException ex2 = ex;
							if (((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
							{
								TaggedLog.Debug("DirectIdsCanManagedConnection", "CreateAndOpenGatewayAsync Connection Canceled: " + ((global::System.Exception)(object)ex2).Message, global::System.Array.Empty<object>());
							}
							else
							{
								TaggedLog.Debug("DirectIdsCanManagedConnection", "CreateAndOpenGatewayAsync Connection Canceled (may have timed out): " + ((global::System.Exception)(object)ex2).Message, global::System.Array.Empty<object>());
							}
						}
						catch (global::System.Exception ex3)
						{
							TaggedLog.Error("DirectIdsCanManagedConnection", $"CreateAndOpenGatewayAsync Connection Error: Cancellation={((CancellationToken)(ref cancellationToken)).IsCancellationRequested}: {ex3.Message}", global::System.Array.Empty<object>());
						}
						goto IL_0467;
					case 3:
						break;
						IL_0467:
						if (((IAdapter)<gateway>5__2).IsConnected)
						{
							TaggedLog.Information("DirectIdsCanManagedConnection", "Gateway " + <gatewayName>5__3 + " connected", global::System.Array.Empty<object>());
							directIdsCanManagedConnection.Gateway = <gateway>5__2;
							break;
						}
						((global::System.IDisposable)<gateway>5__2).Dispose();
						goto end_IL_000e;
						IL_0078:
						((TaskAwaiter)(ref val2)).GetResult();
						<gateway>5__2 = null;
						connection = directIdsCanManagedConnection.Connection;
						if (connection is IDirectIdsCanConnectionTcpIp directIdsCanConnectionTcpIp)
						{
							TaggedLog.Debug("DirectIdsCanManagedConnection", $"CreateAndOpenGatewayAsync {directIdsCanManagedConnection.Connection} creating adapter", global::System.Array.Empty<object>());
							string connectionIpAddress = ((IEndPointConnectionTcpIp)directIdsCanConnectionTcpIp).ConnectionIpAddress;
							if (string.IsNullOrWhiteSpace(connectionIpAddress))
							{
								val3 = CanAdapterFactory.CreateTcpSocketAdapter(directIdsCanManagedConnection.CanDeviceInfo);
								goto IL_01ac;
							}
							<>7__wrap3 = directIdsCanManagedConnection.CanDeviceInfo;
							val = IPAddressExtension.ParseNameAsync(connectionIpAddress).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__2 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<IPAddress>, <CreateAndOpenGatewayAsync>d__28>(ref val, ref this);
								return;
							}
							goto IL_0187;
						}
						if (connection is IDirectIdsCanConnectionBle directIdsCanConnectionBle)
						{
							TaggedLog.Debug("DirectIdsCanManagedConnection", $"CreateAndOpenGatewayAsync Bluetooth V{directIdsCanConnectionBle.GatewayVersion} adapter", global::System.Array.Empty<object>());
							<gateway>5__2 = CanAdapterFactory.CreateBleAdapter(directIdsCanManagedConnection._bleManager, directIdsCanManagedConnection.CanDeviceInfo, ((IEndPointConnectionBle)directIdsCanConnectionBle).ConnectionGuid, ((IEndPointConnectionBle)directIdsCanConnectionBle).ConnectionId, ((IEndPointConnectionWithPassword)directIdsCanConnectionBle).ConnectionPassword, directIdsCanConnectionBle.GatewayVersion, verbose: false, DtcReadEnabled: true);
							goto IL_0279;
						}
						if (!(connection is IDirectConnectionNone))
						{
							TaggedLog.Error("DirectIdsCanManagedConnection", $"CreateAndOpenGatewayAsync Connection type not supported by Gateway {directIdsCanManagedConnection.Connection}", global::System.Array.Empty<object>());
						}
						goto end_IL_000e;
						IL_0279:
						if (<gateway>5__2 != null)
						{
							<gatewayName>5__3 = ((IAdapter)<gateway>5__2).Name ?? "'unknown'";
							if (!((IAdapter)<gateway>5__2).IsConnected)
							{
								goto case 2;
							}
							goto IL_0467;
						}
						TaggedLog.Debug("DirectIdsCanManagedConnection", $"CreateAndOpenGatewayAsync Unable to create gateway for {directIdsCanManagedConnection.Connection}", global::System.Array.Empty<object>());
						goto end_IL_000e;
						IL_01ac:
						<gateway>5__2 = val3;
						goto IL_0279;
						IL_0187:
						result = val.GetResult();
						val3 = CanAdapterFactory.CreateTcpSocketAdapter(<>7__wrap3, result);
						<>7__wrap3 = null;
						goto IL_01ac;
					}
					try
					{
						if (num != 3)
						{
							val2 = directIdsCanManagedConnection.GatewayDidConnect(<gateway>5__2).GetAwaiter();
							if (!((TaskAwaiter)(ref val2)).IsCompleted)
							{
								num = (<>1__state = 3);
								<>u__1 = val2;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <CreateAndOpenGatewayAsync>d__28>(ref val2, ref this);
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
						directIdsCanManagedConnection.DidConnectEvent?.Invoke((IConnectionManager)(object)directIdsCanManagedConnection);
					}
					catch (global::System.Exception ex4)
					{
						TaggedLog.Debug("DirectIdsCanManagedConnection", $"Gateway {directIdsCanManagedConnection.Connection} did open event exception {ex4.Message}", global::System.Array.Empty<object>());
					}
					end_IL_000e:;
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<gateway>5__2 = null;
					<gatewayName>5__3 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<gateway>5__2 = null;
				<gatewayName>5__3 = null;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		private readonly IBleManager _bleManager;

		private const string LogTag = "DirectIdsCanManagedConnection";

		public const int DefaultConnectionCheckTimeMs = 250;

		public const int StartConnectionRetryTimeMs = 1000;

		public const int MaxConnectionRetryTimeMs = 4000;

		[CompilerGenerated]
		private Action<IConnectionManager>? m_DidConnectEvent;

		[CompilerGenerated]
		private Action<IConnectionManager>? m_DidDisconnectEvent;

		[field: CompilerGenerated]
		public ICanDeviceInfo CanDeviceInfo
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public IDirectIdsCanConnection Connection
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public int ConnectionCheckTimeMs
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public IAdapter? Gateway
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public ConnectionManagerStatus Status
		{
			get
			{
				if (((BackgroundOperationBase)this).Started)
				{
					IAdapter? gateway = Gateway;
					if (gateway != null && ((IAdapter)gateway).IsConnected)
					{
						return (ConnectionManagerStatus)2;
					}
					return (ConnectionManagerStatus)1;
				}
				return (ConnectionManagerStatus)0;
			}
		}

		public event Action<IConnectionManager>? DidConnectEvent
		{
			[CompilerGenerated]
			add
			{
				Action<IConnectionManager> val = this.m_DidConnectEvent;
				Action<IConnectionManager> val2;
				do
				{
					val2 = val;
					Action<IConnectionManager> val3 = (Action<IConnectionManager>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<Action<IConnectionManager>>(ref this.m_DidConnectEvent, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				Action<IConnectionManager> val = this.m_DidConnectEvent;
				Action<IConnectionManager> val2;
				do
				{
					val2 = val;
					Action<IConnectionManager> val3 = (Action<IConnectionManager>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<Action<IConnectionManager>>(ref this.m_DidConnectEvent, val3, val2);
				}
				while (val != val2);
			}
		}

		public event Action<IConnectionManager>? DidDisconnectEvent
		{
			[CompilerGenerated]
			add
			{
				Action<IConnectionManager> val = this.m_DidDisconnectEvent;
				Action<IConnectionManager> val2;
				do
				{
					val2 = val;
					Action<IConnectionManager> val3 = (Action<IConnectionManager>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<Action<IConnectionManager>>(ref this.m_DidDisconnectEvent, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				Action<IConnectionManager> val = this.m_DidDisconnectEvent;
				Action<IConnectionManager> val2;
				do
				{
					val2 = val;
					Action<IConnectionManager> val3 = (Action<IConnectionManager>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<Action<IConnectionManager>>(ref this.m_DidDisconnectEvent, val3, val2);
				}
				while (val != val2);
			}
		}

		public DirectIdsCanManagedConnection(IBleManager bleManager, ICanDeviceInfo canDeviceInfo, IDirectIdsCanConnection connection, int connectionCheckTimeMs = 250)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			_bleManager = bleManager;
			CanDeviceInfo = canDeviceInfo ?? throw new ArgumentNullException("canDeviceInfo");
			Connection = connection ?? throw new ArgumentNullException("connection");
			ConnectionCheckTimeMs = connectionCheckTimeMs;
		}

		[AsyncStateMachine(typeof(<BackgroundOperationAsync>d__27))]
		protected override global::System.Threading.Tasks.Task BackgroundOperationAsync(CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<BackgroundOperationAsync>d__27 <BackgroundOperationAsync>d__ = default(<BackgroundOperationAsync>d__27);
			<BackgroundOperationAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<BackgroundOperationAsync>d__.<>4__this = this;
			<BackgroundOperationAsync>d__.cancellationToken = cancellationToken;
			<BackgroundOperationAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <BackgroundOperationAsync>d__.<>t__builder)).Start<<BackgroundOperationAsync>d__27>(ref <BackgroundOperationAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <BackgroundOperationAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<CreateAndOpenGatewayAsync>d__28))]
		private global::System.Threading.Tasks.Task CreateAndOpenGatewayAsync(CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<CreateAndOpenGatewayAsync>d__28 <CreateAndOpenGatewayAsync>d__ = default(<CreateAndOpenGatewayAsync>d__28);
			<CreateAndOpenGatewayAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CreateAndOpenGatewayAsync>d__.<>4__this = this;
			<CreateAndOpenGatewayAsync>d__.cancellationToken = cancellationToken;
			<CreateAndOpenGatewayAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <CreateAndOpenGatewayAsync>d__.<>t__builder)).Start<<CreateAndOpenGatewayAsync>d__28>(ref <CreateAndOpenGatewayAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <CreateAndOpenGatewayAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<CloseConnectionAsync>d__29))]
		private global::System.Threading.Tasks.Task CloseConnectionAsync()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			<CloseConnectionAsync>d__29 <CloseConnectionAsync>d__ = default(<CloseConnectionAsync>d__29);
			<CloseConnectionAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CloseConnectionAsync>d__.<>4__this = this;
			<CloseConnectionAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <CloseConnectionAsync>d__.<>t__builder)).Start<<CloseConnectionAsync>d__29>(ref <CloseConnectionAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <CloseConnectionAsync>d__.<>t__builder)).Task;
		}

		protected virtual global::System.Threading.Tasks.Task GatewayDidConnect(IAdapter gateway)
		{
			TaggedLog.Debug("DirectIdsCanManagedConnection", $"Gateway {Connection} did connect", global::System.Array.Empty<object>());
			return global::System.Threading.Tasks.Task.CompletedTask;
		}

		protected virtual global::System.Threading.Tasks.Task GatewayDidDisconnect()
		{
			TaggedLog.Debug("DirectIdsCanManagedConnection", $"Gateway {Connection} will close", global::System.Array.Empty<object>());
			return global::System.Threading.Tasks.Task.CompletedTask;
		}

		public override void Dispose(bool disposing)
		{
			((BackgroundOperationDisposable)this).Dispose(disposing);
			this.DidConnectEvent = null;
			this.DidDisconnectEvent = null;
		}

		public override string ToString()
		{
			return $"{Connection}";
		}
	}
	public interface IDirectIdsCanConnection : IDirectConnection, IEndPointConnection
	{
	}
	public interface IDirectIdsCanConnectionTcpIp : IEndPointConnectionTcpIp, IEndPointConnection, IDirectIdsCanConnection, IDirectConnection
	{
	}
	public interface IDirectIdsCanConnectionTcpIpWired : IEndPointConnectionTcpIpWired, IEndPointConnectionTcpIp, IEndPointConnection, IDirectIdsCanConnectionTcpIp, IDirectIdsCanConnection, IDirectConnection
	{
	}
	public interface IDirectIdsCanConnectionTcpIpWifi : IEndPointConnectionTcpIpWifi, IEndPointConnectionTcpIp, IEndPointConnection, IEndPointConnectionWithPassword, IDirectIdsCanConnectionTcpIp, IDirectIdsCanConnection, IDirectConnection
	{
	}
	public interface IDirectIdsCanConnectionBle : IEndPointConnectionBle, IEndPointConnectionWithPassword, IEndPointConnection, IDirectIdsCanConnection, IDirectConnection
	{
		GatewayVersion GatewayVersion { get; }
	}
}
namespace IDS.Portable.Extensions
{
	public static class IAdapterExtensions
	{
		public static void SyncTime(this IAdapter adapter)
		{
			adapter.SyncTime(global::System.DateTime.Now);
		}

		public static void SyncTime(this IAdapter adapter, global::System.DateTime dateTime)
		{
			INetworkTime clock = adapter.Clock;
			if (clock != null)
			{
				clock.SetTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
			}
		}
	}
	public static class MessageBufferExtensions
	{
		public static void ToCANMessage()
		{
		}
	}
}
namespace IDS.Portable.Com
{
	public class DeviceInstanceManager
	{
		private class DeviceInstanceClaim
		{
			public readonly byte DeviceInstance;

			[field: CompilerGenerated]
			public FunctionKey FunctionKey
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				set;
			} = FunctionKey.Unknown;

			[field: CompilerGenerated]
			public AddressClaim? AddressClaim
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				set;
			}

			public DeviceInstanceClaim(byte deviceInstance)
			{
				DeviceInstance = deviceInstance;
			}
		}

		private class AddressClaim
		{
			public enum AddressClaimState
			{
				Unassigned,
				Pending,
				Granted
			}

			private const string LogTag = "AddressClaim";

			private static readonly TimeSpan TIME_UNTIL_AUTO_OFFLINE;

			private static readonly TimeSpan MIN_TIME_UNTIL_GRANTED;

			private static readonly Stopwatch Timer;

			public static readonly AddressClaim[] Claims;

			public readonly byte Address;

			public ProductDeviceFunctionKey Owner;

			public AddressClaimState State;

			public TimeSpan LastClaimedTimestamp;

			public TimeSpan LastOwnerChangeTimestamp;

			public static TimeSpan Now => Timer.Elapsed;

			static AddressClaim()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Expected O, but got Unknown
				TIME_UNTIL_AUTO_OFFLINE = TimeSpan.FromSeconds(4L);
				MIN_TIME_UNTIL_GRANTED = TimeSpan.FromSeconds(4.5);
				Timer = new Stopwatch();
				Claims = new AddressClaim[256];
				Timer.Restart();
				for (int i = 0; i < Claims.Length; i++)
				{
					Claims[i] = new AddressClaim((byte)i);
				}
			}

			private AddressClaim(byte address)
			{
				Address = address;
			}

			public void ClaimOwner(ProductDeviceFunctionKey owner)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				TimeSpan now = Now;
				if (Owner == owner && now - LastClaimedTimestamp <= TIME_UNTIL_AUTO_OFFLINE)
				{
					if (State == AddressClaimState.Pending && now - LastOwnerChangeTimestamp >= MIN_TIME_UNTIL_GRANTED)
					{
						State = AddressClaimState.Granted;
					}
				}
				else
				{
					Owner = owner;
					LastOwnerChangeTimestamp = now;
					State = AddressClaimState.Pending;
				}
				LastClaimedTimestamp = now;
			}

			public bool IsClaimValid(TimeSpan now)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				if (State != AddressClaimState.Granted)
				{
					return false;
				}
				return now - LastClaimedTimestamp <= TIME_UNTIL_AUTO_OFFLINE;
			}
		}

		private struct ProductDeviceKey
		{
			public static readonly ProductDeviceKey Unknown;

			private readonly ulong _productMac;

			private readonly byte _deviceType;

			private readonly int _hashCode;

			public ProductDeviceKey(byte[] productMac, byte deviceType)
			{
				_productMac = ((ulong)productMac[0] << 40) | ((ulong)productMac[1] << 32) | ((ulong)productMac[2] << 24) | ((ulong)productMac[3] << 16) | ((ulong)productMac[4] << 8) | productMac[5];
				_deviceType = deviceType;
				_hashCode = -753605376;
				_hashCode = _hashCode * -1521134295 + _productMac.GetHashCode();
				_hashCode = _hashCode * -1521134295 + _deviceType.GetHashCode();
			}

			public static bool operator ==(ProductDeviceKey a, ProductDeviceKey b)
			{
				return ((object)a/*cast due to .constrained prefix*/).Equals((object)b);
			}

			public static bool operator !=(ProductDeviceKey a, ProductDeviceKey b)
			{
				return !((object)a/*cast due to .constrained prefix*/).Equals((object)b);
			}

			public bool Equals(object obj)
			{
				if (!(obj is ProductDeviceKey productDeviceKey))
				{
					return false;
				}
				if (_productMac == productDeviceKey._productMac)
				{
					return _deviceType == productDeviceKey._deviceType;
				}
				return false;
			}

			public string ToString()
			{
				return $"[{_productMac}, {_deviceType}]";
			}

			public int GetHashCode()
			{
				return _hashCode;
			}
		}

		private struct FunctionKey
		{
			public static readonly FunctionKey Unknown;

			private readonly ushort _functionName;

			private readonly byte _functionInstance;

			private readonly int _hashCode;

			public FunctionKey(byte[] functionName, byte functionInstance)
			{
				_functionName = BitConverter.ToUInt16(functionName, 0);
				_functionInstance = functionInstance;
				_hashCode = (_functionName << 16) | _functionInstance;
			}

			public static bool operator ==(FunctionKey a, FunctionKey b)
			{
				return ((object)a/*cast due to .constrained prefix*/).Equals((object)b);
			}

			public static bool operator !=(FunctionKey a, FunctionKey b)
			{
				return !((object)a/*cast due to .constrained prefix*/).Equals((object)b);
			}

			public bool Equals(object obj)
			{
				if (!(obj is FunctionKey functionKey))
				{
					return false;
				}
				if (_functionName == functionKey._functionName)
				{
					return _functionInstance == functionKey._functionInstance;
				}
				return false;
			}

			public string ToString()
			{
				return $"[{_functionName}, {_functionInstance}]";
			}

			public int GetHashCode()
			{
				return _hashCode;
			}
		}

		private struct ProductDeviceFunctionKey
		{
			public static readonly ProductDeviceFunctionKey Unknown;

			private readonly int _hashCode;

			[field: CompilerGenerated]
			public ProductDeviceKey ProductDeviceKey
			{
				[CompilerGenerated]
				get;
			}

			[field: CompilerGenerated]
			public FunctionKey FunctionKey
			{
				[CompilerGenerated]
				get;
			}

			public ProductDeviceFunctionKey(byte[] productMac, byte deviceType, byte[] functionName, byte functionInstance)
			{
				ProductDeviceKey = new ProductDeviceKey(productMac, deviceType);
				FunctionKey = new FunctionKey(functionName, functionInstance);
				_hashCode = -753605376;
				_hashCode = _hashCode * -1521134295 + ((object)ProductDeviceKey/*cast due to .constrained prefix*/).GetHashCode();
				_hashCode = _hashCode * -1521134295 + ((object)FunctionKey/*cast due to .constrained prefix*/).GetHashCode();
			}

			public static bool operator ==(ProductDeviceFunctionKey a, ProductDeviceFunctionKey b)
			{
				return ((object)a/*cast due to .constrained prefix*/).Equals((object)b);
			}

			public static bool operator !=(ProductDeviceFunctionKey a, ProductDeviceFunctionKey b)
			{
				return !((object)a/*cast due to .constrained prefix*/).Equals((object)b);
			}

			public bool Equals(object obj)
			{
				if (!(obj is ProductDeviceFunctionKey productDeviceFunctionKey))
				{
					return false;
				}
				if (((object)ProductDeviceKey/*cast due to .constrained prefix*/).Equals((object)productDeviceFunctionKey.ProductDeviceKey))
				{
					return ((object)FunctionKey/*cast due to .constrained prefix*/).Equals((object)productDeviceFunctionKey.FunctionKey);
				}
				return false;
			}

			public string ToString()
			{
				return $"[{ProductDeviceKey}, {FunctionKey}]";
			}

			public int GetHashCode()
			{
				return _hashCode;
			}
		}

		private const string LogTag = "DeviceInstanceManager";

		private const int MAX_DEVICE_INSNTANCES = 16;

		private readonly Dictionary<ProductDeviceKey, DeviceInstanceClaim[]> _deviceInstanceClaims = new Dictionary<ProductDeviceKey, DeviceInstanceClaim[]>();

		public bool TryGetDeviceInstance(byte[] macAddress, byte deviceType, byte[] functionName, byte functionInstance, byte canAddress, out byte deviceInstance)
		{
			deviceInstance = 0;
			if (BitConverter.ToUInt16(functionName, 0) == 0)
			{
				deviceInstance = 0;
				return false;
			}
			ProductDeviceFunctionKey owner = new ProductDeviceFunctionKey(macAddress, deviceType, functionName, functionInstance);
			AddressClaim addressClaim = AddressClaim.Claims[canAddress];
			addressClaim.ClaimOwner(owner);
			byte? deviceInstance2 = GetDeviceInstance(addressClaim);
			if (deviceInstance2.HasValue)
			{
				byte valueOrDefault = deviceInstance2.GetValueOrDefault();
				deviceInstance = valueOrDefault;
				return true;
			}
			deviceInstance = 0;
			return false;
		}

		private byte? GetDeviceInstance(AddressClaim addressClaim)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			TimeSpan now = AddressClaim.Now;
			if (addressClaim.State != AddressClaim.AddressClaimState.Granted || !addressClaim.IsClaimValid(now))
			{
				return null;
			}
			DeviceInstanceClaim[] deviceInsanceClaims = GetDeviceInsanceClaims(addressClaim.Owner.ProductDeviceKey);
			DeviceInstanceClaim deviceInstanceClaim = Enumerable.FirstOrDefault<DeviceInstanceClaim>((global::System.Collections.Generic.IEnumerable<DeviceInstanceClaim>)deviceInsanceClaims, (Func<DeviceInstanceClaim, bool>)((DeviceInstanceClaim dic) => dic.AddressClaim == addressClaim));
			if (deviceInstanceClaim == null)
			{
				deviceInstanceClaim = GetAvailableDeviceInstanceClaim(deviceInsanceClaims, addressClaim, now);
			}
			if (deviceInstanceClaim == null)
			{
				TaggedLog.Error("DeviceInstanceManager", "All device instances have been used!", global::System.Array.Empty<object>());
				return null;
			}
			deviceInstanceClaim.AddressClaim = addressClaim;
			deviceInstanceClaim.FunctionKey = addressClaim.Owner.FunctionKey;
			return deviceInstanceClaim.DeviceInstance;
		}

		private DeviceInstanceClaim? GetAvailableDeviceInstanceClaim(DeviceInstanceClaim[] deviceInstanceClaims, AddressClaim addressClaim, TimeSpan now)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			DeviceInstanceClaim deviceInstanceClaim = null;
			foreach (DeviceInstanceClaim deviceInstanceClaim2 in deviceInstanceClaims)
			{
				bool num = deviceInstanceClaim2.AddressClaim?.IsClaimValid(now) ?? false;
				bool flag = (deviceInstanceClaim2.AddressClaim?.Owner.ProductDeviceKey ?? ProductDeviceKey.Unknown) == addressClaim.Owner.ProductDeviceKey;
				if (!(num && flag))
				{
					if (deviceInstanceClaim2.FunctionKey == addressClaim.Owner.FunctionKey)
					{
						return deviceInstanceClaim2;
					}
					if (deviceInstanceClaim2.FunctionKey == FunctionKey.Unknown)
					{
						return deviceInstanceClaim2;
					}
					if (deviceInstanceClaim == null)
					{
						deviceInstanceClaim = deviceInstanceClaim2;
					}
				}
			}
			return deviceInstanceClaim;
		}

		private DeviceInstanceClaim[] GetDeviceInsanceClaims(ProductDeviceKey key)
		{
			DeviceInstanceClaim[] result = default(DeviceInstanceClaim[]);
			if (_deviceInstanceClaims.TryGetValue(key, ref result))
			{
				return result;
			}
			result = new DeviceInstanceClaim[16];
			for (byte b = 0; b < 16; b++)
			{
				result[b] = new DeviceInstanceClaim(b);
			}
			_deviceInstanceClaims.Add(key, result);
			return result;
		}
	}
	public abstract class WriteBuffer
	{
		protected readonly Func<byte[], int, int, CancellationToken, global::System.Threading.Tasks.Task> Writer;

		[field: CompilerGenerated]
		public TimeSpan MaxAwaitMessageInterval
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		protected bool Verbose
		{
			[CompilerGenerated]
			get;
		}

		protected abstract string LogTag { get; }

		protected WriteBuffer(Func<byte[], int, int, CancellationToken, global::System.Threading.Tasks.Task> writer, TimeSpan maxAwaitMessageInterval, bool verbose = false)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Writer = writer;
			MaxAwaitMessageInterval = maxAwaitMessageInterval;
			Verbose = verbose;
		}

		public abstract global::System.Threading.Tasks.Task BufferedWriteAsync(byte[] message, int offset, int count, CancellationToken ct);

		protected void VerboseDebug(string tag, string source, string message)
		{
			if (Verbose)
			{
				Debug(tag, source, message);
			}
		}

		protected void Debug(string tag, string source, string message)
		{
			if (Verbose)
			{
				TaggedLog.Information(tag, "Debug - " + source + ": " + message, global::System.Array.Empty<object>());
			}
		}

		protected void Information(string tag, string source, string message)
		{
			TaggedLog.Information(tag, source + ": " + message, global::System.Array.Empty<object>());
		}

		protected void Warning(string tag, string source, string message)
		{
			TaggedLog.Warning(tag, source + ": " + message, global::System.Array.Empty<object>());
		}

		protected void Error(string tag, string source, string message)
		{
			TaggedLog.Error(tag, source + ": " + message, global::System.Array.Empty<object>());
		}
	}
	public class UnbufferedWriter : WriteBuffer
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <BufferedWriteAsync>d__4 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public byte[] message;

			public int count;

			public UnbufferedWriter <>4__this;

			public int offset;

			public CancellationToken ct;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				UnbufferedWriter unbufferedWriter = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_009d;
					}
					if (message != null && message.Length != 0 && count != 0)
					{
						val = unbufferedWriter.Writer.Invoke(message, offset, count, ct).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <BufferedWriteAsync>d__4>(ref val, ref this);
							return;
						}
						goto IL_009d;
					}
					goto end_IL_000e;
					IL_009d:
					((TaskAwaiter)(ref val)).GetResult();
					end_IL_000e:;
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

		[field: CompilerGenerated]
		protected override string LogTag
		{
			[CompilerGenerated]
			get;
		} = "UnbufferedWriter";

		public UnbufferedWriter(Func<byte[], int, int, CancellationToken, global::System.Threading.Tasks.Task> writer)
			: base(writer, TimeSpan.FromMilliseconds(2147483647L, 0L))
		{
		}//IL_0015: Unknown result type (might be due to invalid IL or missing references)


		[AsyncStateMachine(typeof(<BufferedWriteAsync>d__4))]
		public override global::System.Threading.Tasks.Task BufferedWriteAsync(byte[] message, int offset, int count, CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			<BufferedWriteAsync>d__4 <BufferedWriteAsync>d__ = default(<BufferedWriteAsync>d__4);
			<BufferedWriteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<BufferedWriteAsync>d__.<>4__this = this;
			<BufferedWriteAsync>d__.message = message;
			<BufferedWriteAsync>d__.offset = offset;
			<BufferedWriteAsync>d__.count = count;
			<BufferedWriteAsync>d__.ct = ct;
			<BufferedWriteAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <BufferedWriteAsync>d__.<>t__builder)).Start<<BufferedWriteAsync>d__4>(ref <BufferedWriteAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <BufferedWriteAsync>d__.<>t__builder)).Task;
		}
	}
	public class TemporalAndSizeBoundWriteBuffer : WriteBuffer
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <>c__DisplayClass10_0
		{
			public TemporalAndSizeBoundWriteBuffer <>4__this;

			public byte[] message;

			public int count;

			public int offset;
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <BufferedWriteAsync>d__10 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public TemporalAndSizeBoundWriteBuffer <>4__this;

			public byte[] message;

			public int count;

			public int offset;

			public CancellationToken ct;

			private <>c__DisplayClass10_0 <>8__1;

			private bool <maxSizeExceeded>5__2;

			private long <beforeWriteTimestamp>5__3;

			private long <lastWright>5__4;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0160: Unknown result type (might be due to invalid IL or missing references)
				//IL_0165: Unknown result type (might be due to invalid IL or missing references)
				//IL_016d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0117: Unknown result type (might be due to invalid IL or missing references)
				//IL_0122: Unknown result type (might be due to invalid IL or missing references)
				//IL_0127: Unknown result type (might be due to invalid IL or missing references)
				//IL_012b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0130: Unknown result type (might be due to invalid IL or missing references)
				//IL_0145: Unknown result type (might be due to invalid IL or missing references)
				//IL_0147: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				TemporalAndSizeBoundWriteBuffer temporalAndSizeBoundWriteBuffer = <>4__this;
				try
				{
					ConfiguredTaskAwaiter val2;
					if (num != 0)
					{
						<>8__1.<>4__this = <>4__this;
						<>8__1.message = message;
						<>8__1.count = count;
						<>8__1.offset = offset;
						bool num2 = temporalAndSizeBoundWriteBuffer._bytesBuffered > 0 && (double)(temporalAndSizeBoundWriteBuffer._writeTimer.ElapsedMilliseconds - temporalAndSizeBoundWriteBuffer._lastWriteTime) >= ((TimeSpan)(ref temporalAndSizeBoundWriteBuffer._maxWriteInterval)).TotalMilliseconds;
						<maxSizeExceeded>5__2 = temporalAndSizeBoundWriteBuffer._maxBufferSize <= temporalAndSizeBoundWriteBuffer._bytesBuffered + <>8__1.count;
						if (<>8__1.count > 0 && !<maxSizeExceeded>5__2)
						{
							temporalAndSizeBoundWriteBuffer.<BufferedWriteAsync>g__BufferMessage|10_0(ref <>8__1);
						}
						if (!(num2 | <maxSizeExceeded>5__2))
						{
							goto IL_0280;
						}
						<beforeWriteTimestamp>5__3 = temporalAndSizeBoundWriteBuffer._writeTimer.ElapsedMilliseconds;
						<lastWright>5__4 = temporalAndSizeBoundWriteBuffer._writeTimer.ElapsedMilliseconds - temporalAndSizeBoundWriteBuffer._lastWriteTime;
						ConfiguredTaskAwaitable val = temporalAndSizeBoundWriteBuffer.Writer.Invoke(temporalAndSizeBoundWriteBuffer._writeBuffer, 0, temporalAndSizeBoundWriteBuffer._bytesBuffered, ct).ConfigureAwait(false);
						val2 = ((ConfiguredTaskAwaitable)(ref val)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <BufferedWriteAsync>d__10>(ref val2, ref this);
							return;
						}
					}
					else
					{
						val2 = <>u__1;
						<>u__1 = default(ConfiguredTaskAwaiter);
						num = (<>1__state = -1);
					}
					((ConfiguredTaskAwaiter)(ref val2)).GetResult();
					long num3 = temporalAndSizeBoundWriteBuffer._writeTimer.ElapsedMilliseconds - <beforeWriteTimestamp>5__3;
					bool flag = num3 > 300;
					if (temporalAndSizeBoundWriteBuffer.Verbose || flag)
					{
						temporalAndSizeBoundWriteBuffer.VerboseDebug(temporalAndSizeBoundWriteBuffer.LogTag, "BufferedWriteAsync", BitConverter.ToString(temporalAndSizeBoundWriteBuffer._writeBuffer, 0, temporalAndSizeBoundWriteBuffer._bytesBuffered));
						string text = $"lastWrite={<lastWright>5__4}ms ago, writeTime={num3}ms, bytesWritten={temporalAndSizeBoundWriteBuffer._bytesBuffered}";
						if (flag)
						{
							temporalAndSizeBoundWriteBuffer.Warning(temporalAndSizeBoundWriteBuffer.LogTag, "BufferedWriteAsync", "Long buffer write: " + text);
						}
						else
						{
							temporalAndSizeBoundWriteBuffer.VerboseDebug(temporalAndSizeBoundWriteBuffer.LogTag, "BufferedWriteAsync", "Buffer write: " + text);
						}
					}
					temporalAndSizeBoundWriteBuffer._lastWriteTime = temporalAndSizeBoundWriteBuffer._writeTimer.ElapsedMilliseconds;
					temporalAndSizeBoundWriteBuffer._bytesBuffered = 0;
					goto IL_0280;
					IL_0280:
					if ((<>8__1.count > 0) & <maxSizeExceeded>5__2)
					{
						temporalAndSizeBoundWriteBuffer.<BufferedWriteAsync>g__BufferMessage|10_0(ref <>8__1);
					}
					if (temporalAndSizeBoundWriteBuffer._bytesBuffered == 0)
					{
						temporalAndSizeBoundWriteBuffer._lastWriteTime = temporalAndSizeBoundWriteBuffer._writeTimer.ElapsedMilliseconds;
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<>8__1 = default(<>c__DisplayClass10_0);
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<>8__1 = default(<>c__DisplayClass10_0);
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		private readonly TimeSpan _maxWriteInterval;

		private readonly uint _maxBufferSize;

		private readonly byte[] _writeBuffer;

		private int _bytesBuffered;

		private readonly Stopwatch _writeTimer = new Stopwatch();

		private long _lastWriteTime;

		[field: CompilerGenerated]
		protected override string LogTag
		{
			[CompilerGenerated]
			get;
		} = "TemporalAndSizeBoundWriteBuffer";

		public TemporalAndSizeBoundWriteBuffer(Func<byte[], int, int, CancellationToken, global::System.Threading.Tasks.Task> writer, TimeSpan maxAwaitMessageInterval, TimeSpan maxWriteInterval, uint maxBufferSize, bool verbose = false)
			: base(writer, maxAwaitMessageInterval, verbose)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			_maxWriteInterval = maxWriteInterval;
			_maxBufferSize = maxBufferSize;
			_writeBuffer = new byte[_maxBufferSize];
			_writeTimer.Start();
			_lastWriteTime = _writeTimer.ElapsedMilliseconds;
		}

		[AsyncStateMachine(typeof(<BufferedWriteAsync>d__10))]
		public override global::System.Threading.Tasks.Task BufferedWriteAsync(byte[] message, int offset, int count, CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			<BufferedWriteAsync>d__10 <BufferedWriteAsync>d__ = default(<BufferedWriteAsync>d__10);
			<BufferedWriteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<BufferedWriteAsync>d__.<>4__this = this;
			<BufferedWriteAsync>d__.message = message;
			<BufferedWriteAsync>d__.offset = offset;
			<BufferedWriteAsync>d__.count = count;
			<BufferedWriteAsync>d__.ct = ct;
			<BufferedWriteAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <BufferedWriteAsync>d__.<>t__builder)).Start<<BufferedWriteAsync>d__10>(ref <BufferedWriteAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <BufferedWriteAsync>d__.<>t__builder)).Task;
		}

		[CompilerGenerated]
		private void <BufferedWriteAsync>g__BufferMessage|10_0(ref <>c__DisplayClass10_0 P_0)
		{
			VerboseDebug(LogTag, "BufferedWriteAsync", BitConverter.ToString(P_0.message, 0, P_0.count));
			Buffer.BlockCopy((global::System.Array)P_0.message, P_0.offset, (global::System.Array)_writeBuffer, _bytesBuffered, P_0.count);
			_bytesBuffered += P_0.count;
		}
	}
}
namespace IDS.Portable.CAN
{
	public class AppImageCache : ImageCache<object>
	{
		private static readonly object _sync = new object();

		private static volatile AppImageCache? _instance = null;

		private readonly object _fakeImage;

		public static AppImageCache Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}
				lock (_sync)
				{
					if (_instance == null)
					{
						_instance = new AppImageCache();
					}
				}
				return _instance;
			}
		}

		private AppImageCache()
		{
			_fakeImage = new object();
		}

		protected override object LoadImageResource(global::System.Enum reference)
		{
			return _fakeImage;
		}
	}
	public class CanAdapter : Adapter<MessageBuffer>
	{
		private const int CanBaudRate = 250000;

		private const string LogTag = "CanAdapter";

		private readonly IAdapter<MessageBuffer> _physicalNetworkAdapter;

		private const uint EXTENDED = 2147483648u;

		private readonly Timer _debugDumpTimer = new Timer(true);

		public override IPhysicalAddress MAC => ((IAdapter)_physicalNetworkAdapter).MAC;

		public CanAdapter(IAdapter<MessageBuffer> physicalNetworkAdapter)
			: base("CanAdapter", 250000)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			_physicalNetworkAdapter = physicalNetworkAdapter;
			((DisposableManager)this).AddDisposable((IDisposable)(object)_physicalNetworkAdapter);
			((IEventSender)_physicalNetworkAdapter).Events.Subscribe<AdapterOpenedEvent>((Action<AdapterOpenedEvent>)OnPhysicalNetworkOpened, (SubscriptionType)1, ((Adapter)this).Subscriptions);
			((IEventSender)_physicalNetworkAdapter).Events.Subscribe<AdapterClosedEvent>((Action<AdapterClosedEvent>)OnPhysicalNetworkClosed, (SubscriptionType)1, ((Adapter)this).Subscriptions);
			((IEventSender)_physicalNetworkAdapter).Events.Subscribe<AdapterRxEvent<MessageBuffer>>((Action<AdapterRxEvent<MessageBuffer>>)OnPhysicalNetworkReceived, (SubscriptionType)1, ((Adapter)this).Subscriptions);
		}

		protected override global::System.Threading.Tasks.Task<bool> ConnectAsync(AsyncOperation obj)
		{
			return ((IAdapter)_physicalNetworkAdapter).OpenAsync(obj);
		}

		protected override global::System.Threading.Tasks.Task<bool> DisconnectAsync(AsyncOperation obj)
		{
			return ((IAdapter)_physicalNetworkAdapter).CloseAsync(obj);
		}

		protected override bool TransmitRaw(MessageBuffer idsCanMsgBuff)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed)
			{
				return false;
			}
			MessageBuffer val = ResourcePool<MessageBuffer>.GetObject();
			try
			{
				val.Append((byte)((MessageBuffer)idsCanMsgBuff).Length);
				ID iD = idsCanMsgBuff.ID;
				if (((ID)(ref iD)).IsExtended)
				{
					iD = idsCanMsgBuff.ID;
					val.Append(((ID)(ref iD)).Value | 0x80000000u);
				}
				else
				{
					val.Append((ushort)(uint)idsCanMsgBuff.ID);
				}
				val.Append(((MessageBuffer)idsCanMsgBuff).Data, ((MessageBuffer)idsCanMsgBuff).Length);
				return _physicalNetworkAdapter.Transmit(val);
			}
			catch (global::System.Exception ex)
			{
				TaggedLog.Error("CanAdapter", $"{"TransmitRaw"}: {ex.GetType()} - {ex.Message}\n{ex.StackTrace}", global::System.Array.Empty<object>());
				return false;
			}
			finally
			{
				((Object)val).ReturnToPool();
			}
		}

		private void OnPhysicalNetworkOpened(AdapterOpenedEvent e)
		{
			((Adapter)this).RaiseAdapterOpened();
		}

		private void OnPhysicalNetworkClosed(AdapterClosedEvent e)
		{
			((Adapter)this).RaiseAdapterClosed();
		}

		private void OnPhysicalNetworkReceived(AdapterRxEvent<MessageBuffer> e)
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed)
			{
				return;
			}
			MessageBuffer message = e.Message;
			bool flag = (message.Data[0] & 0x10) != 0;
			byte b = (byte)(message.Data[0] & -17);
			if (b > 8)
			{
				return;
			}
			int num = message.Length - b;
			MessageBuffer val = ResourcePool<MessageBuffer>.GetObject();
			try
			{
				((MessageBuffer)val).Length = b;
				switch (num)
				{
				case 5:
				{
					byte num3 = message.Data[1];
					bool flag2 = (num3 & 0x80) != 0;
					uint num4 = (uint)(((num3 << 24) | (message.Data[2] << 16) | (message.Data[3] << 8) | message.Data[4]) & 0x7FFFFFFF);
					val.ID = new ID(num4, flag2);
					break;
				}
				case 3:
				{
					ushort num2 = (ushort)((message.Data[1] << 8) | message.Data[2]);
					val.ID = new ID((uint)num2, false);
					break;
				}
				default:
					throw new global::System.Exception($"Unexpected CAN ID length of {num}, expected 3 or 5");
				}
				for (int i = 0; i < b; i++)
				{
					((MessageBuffer)val).Data[i] = message.Data[i + num];
				}
				((Adapter<MessageBuffer>)(object)this).RaiseMessageRx(val, flag);
			}
			catch (global::System.Exception ex)
			{
				TaggedLog.Error("CanAdapter", $"{"OnPhysicalNetworkReceived"}: {ex.GetType()} - {ex.Message}", global::System.Array.Empty<object>());
			}
			finally
			{
				((Object)val).ReturnToPool();
			}
		}

		private void DebugDumpMessage11Bit(ID canId, byte[] data, int length, byte? canAddressFilter = null, MESSAGE_TYPE? messageTypeFilter = null)
		{
			MESSAGE_TYPE val = MESSAGE_TYPE.op_Implicit((byte)((((ID)(ref canId)).Value & 0xFF00) >> 8));
			uint num = ((ID)(ref canId)).Value & 0xFF;
			if ((messageTypeFilter == null || val == messageTypeFilter) && (!canAddressFilter.HasValue || num == canAddressFilter))
			{
				string text = BitConverter.ToString(data, 3, length).Replace("-", " ");
				if (string.IsNullOrEmpty(text))
				{
					text = "none";
				}
				TaggedLog.Debug("CanAdapter", $"{TimerExtension.Elapsed_ms(_debugDumpTimer)}\t{val}\t{num:X2}\t--\t--\t{length}\t{text}", global::System.Array.Empty<object>());
			}
		}

		private void DebugDumpMessage29Bit(ID canId, byte[] data, int length, byte? canAddressFilter = null, MESSAGE_TYPE? messageTypeFilter = null)
		{
			uint num = (((ID)(ref canId)).Value & 0x1C000000) >> 26 << 3;
			uint num2 = (((ID)(ref canId)).Value & 0x30000) >> 16;
			MESSAGE_TYPE val = MESSAGE_TYPE.op_Implicit((byte)(num | num2));
			uint num3 = (((ID)(ref canId)).Value & 0x3FC0000) >> 18;
			uint num4 = (((ID)(ref canId)).Value & 0xFF00) >> 8;
			uint num5 = ((ID)(ref canId)).Value & 0xFF;
			if ((messageTypeFilter == null || MESSAGE_TYPE.op_Implicit(val) == (MESSAGE_TYPE.op_Implicit(messageTypeFilter) & 0x7F)) && (!canAddressFilter.HasValue || num3 == canAddressFilter))
			{
				string text = BitConverter.ToString(data, 5, length).Replace("-", " ");
				if (string.IsNullOrEmpty(text))
				{
					text = "none";
				}
				TaggedLog.Debug("CanAdapter", $"{TimerExtension.Elapsed_ms(_debugDumpTimer)}\t{val}\t{num3:X2}\t{num4:X2}\t{num5:X2}\t{length}\t{text}", global::System.Array.Empty<object>());
			}
		}
	}
	public static class CanAdapterFactory
	{
		public static readonly IPAddress DefaultMyrvGatewayIpAddress = IPAddress.Parse("192.168.1.1");

		public static readonly IPAddress DefaultLocalhostIpAddress = IPAddress.Parse("127.0.0.1");

		private const int DefaultPort = 6969;

		private const ADAPTER_OPTIONS DtcsEnabled = (ADAPTER_OPTIONS)32u;

		private const ADAPTER_OPTIONS DtcsDisabled = (ADAPTER_OPTIONS)0u;

		private static bool _macInit = false;

		private static readonly object _macLock = new object();

		private static PhysicalAddress? _mac;

		public static Func<PhysicalAddress> MakeMac = delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			byte[] array = new byte[6];
			new Random().NextBytes(array);
			return new PhysicalAddress(array);
		};

		public static readonly Func<string, string, DEVICE_ID, IAdapter<MessageBuffer>, ADAPTER_OPTIONS, IAdapter> MakeAdapter = delegate(string name, string softwarePartNumber, DEVICE_ID deviceId, IAdapter<MessageBuffer> communicationsAdapter, ADAPTER_OPTIONS options)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			CanAdapter canAdapter = new CanAdapter(communicationsAdapter);
			return (IAdapter)new Adapter(name, softwarePartNumber, deviceId, (Adapter<MessageBuffer>)(object)canAdapter, options);
		};

		private static bool _init = false;

		public static PhysicalAddress? Mac
		{
			get
			{
				if (_macInit)
				{
					return _mac;
				}
				lock (_macLock)
				{
					if (_macInit)
					{
						return _mac;
					}
					_mac = MakeMac.Invoke();
					_macInit = true;
				}
				return _mac;
			}
		}

		public static IAdapter? CreateTcpSocketAdapter(ICanDeviceInfo canDeviceInfo, bool verbose = false)
		{
			return CreateTcpSocketAdapter(canDeviceInfo, canDeviceInfo.UsesLocalHost ? DefaultLocalhostIpAddress : DefaultMyrvGatewayIpAddress, 6969, verbose);
		}

		public static IAdapter? CreateTcpSocketAdapter(ICanDeviceInfo canDeviceInfo, IPAddress ipAddress, int port = 6969, bool verbose = false)
		{
			try
			{
				InitCore();
				if (Mac == null)
				{
					return null;
				}
				TcpCommunicationsAdapter communicationsAdapter = new TcpCommunicationsAdapter(ipAddress, port, Mac, verbose);
				return CreateAdapter(canDeviceInfo, (IAdapter<MessageBuffer>)(object)communicationsAdapter, (ADAPTER_OPTIONS)32);
			}
			catch (global::System.Exception)
			{
				throw;
			}
		}

		public static IAdapter? CreateBleAdapter(IBleManager bleManager, ICanDeviceInfo canDeviceInfo, Guid guid, string name, string passcode, GatewayVersion version, bool verbose = false, bool DtcReadEnabled = false)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				InitCore();
				if (Mac == null)
				{
					return null;
				}
				BleCommunicationsAdapter communicationsAdapter = new BleCommunicationsAdapter(bleManager, guid, name, passcode, version, Mac, verbose);
				ADAPTER_OPTIONS options = (ADAPTER_OPTIONS)(DtcReadEnabled ? 32 : 0);
				return CreateAdapter(canDeviceInfo, (IAdapter<MessageBuffer>)(object)communicationsAdapter, options);
			}
			catch (global::System.Exception)
			{
				throw;
			}
		}

		private static IAdapter CreateAdapter(ICanDeviceInfo canDeviceInfo, IAdapter<MessageBuffer> communicationsAdapter, ADAPTER_OPTIONS options = (ADAPTER_OPTIONS)0u)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			InitCore();
			return MakeAdapter.Invoke("AppAdapter", canDeviceInfo.PartNumber, canDeviceInfo.DeviceId, communicationsAdapter, (ADAPTER_OPTIONS)(1 | options));
		}

		public static void InitCore()
		{
			if (!_init)
			{
				_init = true;
				FreeRunningCounter.Instance = (IFreeRunningCounter)(object)FreeRunningCounter.Instance;
				if (ImageCache.Instance == null)
				{
					ImageCache.Instance = (Interface)(object)AppImageCache.Instance;
				}
				TreeNode.Factory = TreeNode.GenericFactory;
				ResourcePool<MessageBuffer>.Capacity = 999;
				ResourcePool<MessageBuffer>.Capacity = 999;
			}
		}
	}
	public class FreeRunningCounter : IFreeRunningCounter
	{
		private static readonly object _sync = new object();

		private static volatile FreeRunningCounter? _instance = null;

		public static FreeRunningCounter Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}
				lock (_sync)
				{
					if (_instance == null)
					{
						_instance = new FreeRunningCounter();
					}
				}
				return _instance;
			}
		}

		public ulong ClockFrequency_hz => (ulong)Stopwatch.Frequency;

		public long Ticks => Stopwatch.GetTimestamp();

		private FreeRunningCounter()
		{
		}
	}
	public interface ICanDeviceInfo
	{
		string PartNumber { get; }

		DEVICE_ID DeviceId { get; }

		DEVICE_TYPE DeviceType { get; }

		FUNCTION_NAME FunctionName { get; }

		PRODUCT_ID ProductId { get; }

		bool UsesLocalHost { get; }
	}
}
namespace IDS.Portable.CAN.Com
{
	public class BleCommunicationsAdapter : SynchronizedCommunicationsAdapter
	{
		private enum V2MessageType
		{
			Packed = 1,
			ElevenBit,
			TwentyNineBit
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <SynchronizedConnectAsync>d__45 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public CancellationToken cancellationToken;

			public BleCommunicationsAdapter <>4__this;

			private CancellationTokenSource <linkedCts>5__2;

			private CancellationToken <linkedCt>5__3;

			private TaskAwaiter<IDevice?> <>u__1;

			private TaskAwaiter<IService?> <>u__2;

			private TaskAwaiter<int> <>u__3;

			private TaskAwaiter<bool> <>u__4;

			private TaskAwaiter<ICharacteristic?> <>u__5;

			private void MoveNext()
			{
				//IL_010c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0112: Unknown result type (might be due to invalid IL or missing references)
				//IL_011c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0121: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0136: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_0151: Unknown result type (might be due to invalid IL or missing references)
				//IL_0156: Unknown result type (might be due to invalid IL or missing references)
				//IL_015e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0217: Unknown result type (might be due to invalid IL or missing references)
				//IL_021c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0224: Unknown result type (might be due to invalid IL or missing references)
				//IL_031e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0323: Unknown result type (might be due to invalid IL or missing references)
				//IL_032b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0465: Unknown result type (might be due to invalid IL or missing references)
				//IL_046a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0472: Unknown result type (might be due to invalid IL or missing references)
				//IL_0523: Unknown result type (might be due to invalid IL or missing references)
				//IL_0528: Unknown result type (might be due to invalid IL or missing references)
				//IL_0530: Unknown result type (might be due to invalid IL or missing references)
				//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
				//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
				//IL_0264: Unknown result type (might be due to invalid IL or missing references)
				//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_036b: Unknown result type (might be due to invalid IL or missing references)
				//IL_04de: Unknown result type (might be due to invalid IL or missing references)
				//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
				//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
				//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
				//IL_0587: Unknown result type (might be due to invalid IL or missing references)
				//IL_058c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0426: Unknown result type (might be due to invalid IL or missing references)
				//IL_0430: Unknown result type (might be due to invalid IL or missing references)
				//IL_0435: Unknown result type (might be due to invalid IL or missing references)
				//IL_0292: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
				//IL_0508: Unknown result type (might be due to invalid IL or missing references)
				//IL_050a: Unknown result type (might be due to invalid IL or missing references)
				//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_044a: Unknown result type (might be due to invalid IL or missing references)
				//IL_044c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0303: Unknown result type (might be due to invalid IL or missing references)
				//IL_0305: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				BleCommunicationsAdapter bleCommunicationsAdapter = <>4__this;
				bool result;
				try
				{
					if ((uint)num > 5u)
					{
						<linkedCts>5__2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
					}
					try
					{
						TaskAwaiter<IDevice> val5;
						TaskAwaiter<IService> val4;
						TaskAwaiter<int> val3;
						TaskAwaiter<ICharacteristic> val2;
						TaskAwaiter<bool> val;
						ICharacteristic result2;
						IService result3;
						int num2;
						IDevice result4;
						switch (num)
						{
						default:
							<linkedCts>5__2.CancelAfter(30000);
							<linkedCt>5__3 = <linkedCts>5__2.Token;
							if (bleCommunicationsAdapter.GatewayDevice == null)
							{
								try
								{
									bleCommunicationsAdapter._bleManager.DeviceDisconnected -= bleCommunicationsAdapter.OnDeviceDisconnected;
								}
								catch
								{
								}
								try
								{
									bleCommunicationsAdapter._bleManager.DeviceConnectionLost -= bleCommunicationsAdapter.OnDeviceConnectionLost;
								}
								catch
								{
								}
								bleCommunicationsAdapter._bleManager.DeviceDisconnected += bleCommunicationsAdapter.OnDeviceDisconnected;
								bleCommunicationsAdapter._bleManager.DeviceConnectionLost += bleCommunicationsAdapter.OnDeviceConnectionLost;
								bleCommunicationsAdapter.VerboseDebug("SynchronizedConnectAsync", bleCommunicationsAdapter._name + ", subscribed to adapter events");
								val5 = bleCommunicationsAdapter._bleManager.TryConnectToDeviceAsync(bleCommunicationsAdapter._id, <linkedCt>5__3).GetAwaiter();
								if (!val5.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val5;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IDevice>, <SynchronizedConnectAsync>d__45>(ref val5, ref this);
									return;
								}
								goto IL_016d;
							}
							bleCommunicationsAdapter.Warning("SynchronizedConnectAsync", "GatewayDevice is not null, need to clean up");
							result = false;
							goto end_IL_0024;
						case 0:
							val5 = <>u__1;
							<>u__1 = default(TaskAwaiter<IDevice>);
							num = (<>1__state = -1);
							goto IL_016d;
						case 1:
							val4 = <>u__2;
							<>u__2 = default(TaskAwaiter<IService>);
							num = (<>1__state = -1);
							goto IL_0233;
						case 2:
							val3 = <>u__3;
							<>u__3 = default(TaskAwaiter<int>);
							num = (<>1__state = -1);
							goto IL_033a;
						case 3:
							val = <>u__4;
							<>u__4 = default(TaskAwaiter<bool>);
							num = (<>1__state = -1);
							goto IL_0481;
						case 4:
							val2 = <>u__5;
							<>u__5 = default(TaskAwaiter<ICharacteristic>);
							num = (<>1__state = -1);
							goto IL_053f;
						case 5:
							{
								val = <>u__4;
								<>u__4 = default(TaskAwaiter<bool>);
								num = (<>1__state = -1);
								break;
							}
							IL_0481:
							if (val.GetResult())
							{
								bleCommunicationsAdapter.VerboseDebug("SynchronizedConnectAsync", bleCommunicationsAdapter.GatewayDevice.Name + ", unlocked ECU");
								val2 = bleCommunicationsAdapter._bleManager.GetCharacteristicAsync(bleCommunicationsAdapter.GatewayDevice, bleCommunicationsAdapter.GatewayService, GUID_IDS_CAN_READ, <linkedCt>5__3).GetAwaiter();
								if (!val2.IsCompleted)
								{
									num = (<>1__state = 4);
									<>u__5 = val2;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ICharacteristic>, <SynchronizedConnectAsync>d__45>(ref val2, ref this);
									return;
								}
								goto IL_053f;
							}
							bleCommunicationsAdapter.Warning("SynchronizedConnectAsync", bleCommunicationsAdapter._name + ", failed to unlock ECU");
							result = false;
							goto end_IL_0024;
							IL_053f:
							result2 = val2.GetResult();
							bleCommunicationsAdapter.GatewayCanRxCharacteristic = result2;
							if (bleCommunicationsAdapter.GatewayCanRxCharacteristic != null)
							{
								bleCommunicationsAdapter.GatewayCanRxCharacteristic.ValueUpdated += bleCommunicationsAdapter.OnDataReceived;
								val = bleCommunicationsAdapter._bleManager.StartCharacteristicUpdatesAsync(bleCommunicationsAdapter.GatewayCanRxCharacteristic).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 5);
									<>u__4 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <SynchronizedConnectAsync>d__45>(ref val, ref this);
									return;
								}
								break;
							}
							result = false;
							goto end_IL_0024;
							IL_0412:
							val = bleCommunicationsAdapter.UnlockEcuAsync(bleCommunicationsAdapter.GatewayDevice, bleCommunicationsAdapter.GatewayService, bleCommunicationsAdapter._passcode, <linkedCt>5__3).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 3);
								<>u__4 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <SynchronizedConnectAsync>d__45>(ref val, ref this);
								return;
							}
							goto IL_0481;
							IL_0233:
							result3 = val4.GetResult();
							bleCommunicationsAdapter.GatewayService = result3;
							if (bleCommunicationsAdapter.GatewayService != null)
							{
								if (GatewayVersionExtensions.IsLargeMtuSupported(bleCommunicationsAdapter._version))
								{
									bleCommunicationsAdapter.VerboseDebug("SynchronizedConnectAsync", $"BLE Connection {bleCommunicationsAdapter._id}({bleCommunicationsAdapter._name}): Requesting MTU Size {185}");
									val3 = bleCommunicationsAdapter.GatewayDevice.RequestMtuAsync(185).GetAwaiter();
									if (!val3.IsCompleted)
									{
										num = (<>1__state = 2);
										<>u__3 = val3;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<int>, <SynchronizedConnectAsync>d__45>(ref val3, ref this);
										return;
									}
									goto IL_033a;
								}
								goto IL_0412;
							}
							bleCommunicationsAdapter.Error("SynchronizedConnectAsync", "Could not get CAN service from device");
							result = false;
							goto end_IL_0024;
							IL_033a:
							num2 = val3.GetResult();
							if (num2 <= 0)
							{
								num2 = 23;
								bleCommunicationsAdapter.Warning("SynchronizedConnectAsync", $"BLE Connection {bleCommunicationsAdapter._id}({bleCommunicationsAdapter._name}): Requesting MTU Size FAILED, assuming MTU size of {23}");
							}
							bleCommunicationsAdapter.VerboseDebug("SynchronizedConnectAsync", $"BLE Connection {bleCommunicationsAdapter._id}({bleCommunicationsAdapter._name}): Connected with MTU size of {num2}");
							goto IL_0412;
							IL_016d:
							result4 = val5.GetResult();
							bleCommunicationsAdapter.GatewayDevice = result4;
							if (bleCommunicationsAdapter.GatewayDevice != null)
							{
								bleCommunicationsAdapter.VerboseDebug("SynchronizedConnectAsync", bleCommunicationsAdapter.GatewayDevice.Name + ", connected");
								val4 = bleCommunicationsAdapter._bleManager.GetServiceAsync(bleCommunicationsAdapter.GatewayDevice, GUID_IDS_CAN_SERVICE, <linkedCt>5__3).GetAwaiter();
								if (!val4.IsCompleted)
								{
									num = (<>1__state = 1);
									<>u__2 = val4;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IService>, <SynchronizedConnectAsync>d__45>(ref val4, ref this);
									return;
								}
								goto IL_0233;
							}
							bleCommunicationsAdapter.Warning("SynchronizedConnectAsync", bleCommunicationsAdapter._name + ", failed to connect");
							result = false;
							goto end_IL_0024;
						}
						if (!val.GetResult())
						{
							bleCommunicationsAdapter.Warning("SynchronizedConnectAsync", bleCommunicationsAdapter._name + ", failed to start characteristic update");
							result = false;
						}
						else
						{
							bleCommunicationsAdapter.VerboseDebug("SynchronizedConnectAsync", bleCommunicationsAdapter.GatewayDevice.Name + ", gateway connected");
							result = true;
						}
						end_IL_0024:;
					}
					finally
					{
						if (num < 0 && <linkedCts>5__2 != null)
						{
							((global::System.IDisposable)<linkedCts>5__2).Dispose();
						}
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
		private struct <SynchronizedDisconnectAsync>d__46 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public BleCommunicationsAdapter <>4__this;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_0115: Unknown result type (might be due to invalid IL or missing references)
				//IL_011a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0121: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				BleCommunicationsAdapter bleCommunicationsAdapter = <>4__this;
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						try
						{
							bleCommunicationsAdapter._bleManager.DeviceDisconnected -= bleCommunicationsAdapter.OnDeviceDisconnected;
						}
						catch
						{
						}
						try
						{
							bleCommunicationsAdapter._bleManager.DeviceConnectionLost -= bleCommunicationsAdapter.OnDeviceConnectionLost;
						}
						catch
						{
						}
						bleCommunicationsAdapter.VerboseDebug("SynchronizedDisconnectAsync", bleCommunicationsAdapter._name + ", unsubscribed from adapter events");
						if (bleCommunicationsAdapter.GatewayCanRxCharacteristic != null)
						{
							try
							{
								bleCommunicationsAdapter.GatewayCanRxCharacteristic.ValueUpdated -= bleCommunicationsAdapter.OnDataReceived;
							}
							catch
							{
							}
							bleCommunicationsAdapter._bleManager.StopCharacteristicUpdates(bleCommunicationsAdapter.GatewayCanRxCharacteristic);
							bleCommunicationsAdapter.GatewayCanRxCharacteristic = null;
						}
						bleCommunicationsAdapter.VerboseDebug("SynchronizedDisconnectAsync", bleCommunicationsAdapter._name + ", unsubscribed from characteristic");
						if (bleCommunicationsAdapter.GatewayDevice == null)
						{
							goto IL_0153;
						}
						val = TaskExtension.TryAwaitAsync(bleCommunicationsAdapter._bleManager.DisconnectDeviceAsync(bleCommunicationsAdapter.GatewayDevice), true).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <SynchronizedDisconnectAsync>d__46>(ref val, ref this);
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
					bleCommunicationsAdapter.VerboseDebug("SynchronizedDisconnectAsync", bleCommunicationsAdapter._name + ", disconnected");
					goto IL_0153;
					IL_0153:
					IService? gatewayService = bleCommunicationsAdapter.GatewayService;
					if (gatewayService != null)
					{
						IDisposableExtensions.TryDispose((global::System.IDisposable)gatewayService);
					}
					bleCommunicationsAdapter.GatewayService = null;
					bleCommunicationsAdapter.VerboseDebug("SynchronizedDisconnectAsync", bleCommunicationsAdapter._name + ", service disposed");
					IDevice? gatewayDevice = bleCommunicationsAdapter.GatewayDevice;
					if (gatewayDevice != null)
					{
						IDisposableExtensions.TryDispose((global::System.IDisposable)gatewayDevice);
					}
					bleCommunicationsAdapter.GatewayDevice = null;
					bleCommunicationsAdapter.VerboseDebug("SynchronizedDisconnectAsync", bleCommunicationsAdapter._name + ", device disposed");
					int count = bleCommunicationsAdapter._dataReceivedQueue.Count;
					bleCommunicationsAdapter.VerboseDebug("SynchronizedDisconnectAsync", $"Clearing {count} queued messages");
					byte[] array = default(byte[]);
					while (bleCommunicationsAdapter._dataReceivedQueue.Count > 0)
					{
						if (bleCommunicationsAdapter._dataReceivedQueue.TryDequeue(ref array) && array != null)
						{
							ArrayPool<byte>.Shared.Return(array, false);
						}
					}
					bleCommunicationsAdapter.VerboseDebug("SynchronizedDisconnectAsync", $"{count - bleCommunicationsAdapter._dataReceivedQueue.Count} messages dequeued");
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
		private struct <SynchronizedReadAsync>d__49 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<int> <>t__builder;

			public CancellationToken cancellationToken;

			public BleCommunicationsAdapter <>4__this;

			public byte[] buffer;

			private byte[] <data>5__2;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				BleCommunicationsAdapter bleCommunicationsAdapter = <>4__this;
				int result;
				try
				{
					int num2;
					if (num != 0)
					{
						<data>5__2 = null;
						num2 = 0;
					}
					try
					{
						if (num != 0)
						{
							goto IL_008e;
						}
						ConfiguredTaskAwaiter val = <>u__1;
						<>u__1 = default(ConfiguredTaskAwaiter);
						num = (<>1__state = -1);
						goto IL_0087;
						IL_0087:
						((ConfiguredTaskAwaiter)(ref val)).GetResult();
						goto IL_008e;
						IL_008e:
						if (!bleCommunicationsAdapter._dataReceivedQueue.TryDequeue(ref <data>5__2))
						{
							ConfiguredTaskAwaitable val2 = global::System.Threading.Tasks.Task.Delay(5, cancellationToken).ConfigureAwait(false);
							val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
							if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <SynchronizedReadAsync>d__49>(ref val, ref this);
								return;
							}
							goto IL_0087;
						}
						num2 = <data>5__2[0];
						if (buffer.Length < num2)
						{
							throw new IndexOutOfRangeException("Message received is larger than the buffer allocated!!");
						}
						Buffer.BlockCopy((global::System.Array)<data>5__2, 1, (global::System.Array)buffer, 0, num2);
					}
					finally
					{
						if (num < 0 && <data>5__2 != null)
						{
							ArrayPool<byte>.Shared.Return(<data>5__2, false);
						}
					}
					result = num2;
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<data>5__2 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<data>5__2 = null;
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
		private struct <SynchronizedWriteAsync>d__48 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public BleCommunicationsAdapter <>4__this;

			public CancellationToken cancellationToken;

			public int count;

			public byte[] buffer;

			public int offset;

			private TaskAwaiter<ICharacteristic?> <>u__1;

			private ConfiguredTaskAwaiter<bool> <>u__2;

			private TaskAwaiter <>u__3;

			private void MoveNext()
			{
				//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0272: Unknown result type (might be due to invalid IL or missing references)
				//IL_0277: Unknown result type (might be due to invalid IL or missing references)
				//IL_027f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0236: Unknown result type (might be due to invalid IL or missing references)
				//IL_0240: Unknown result type (might be due to invalid IL or missing references)
				//IL_0245: Unknown result type (might be due to invalid IL or missing references)
				//IL_022e: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_025a: Unknown result type (might be due to invalid IL or missing references)
				//IL_025c: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				BleCommunicationsAdapter bleCommunicationsAdapter = <>4__this;
				try
				{
					TaskAwaiter<ICharacteristic> val3;
					ConfiguredTaskAwaiter<bool> val2;
					TaskAwaiter val;
					ICharacteristic result;
					switch (num)
					{
					default:
						if (bleCommunicationsAdapter.Connected)
						{
							if (bleCommunicationsAdapter.GatewayDevice == null)
							{
								throw new BleTxException("GatewayDevice is null");
							}
							if (bleCommunicationsAdapter.GatewayService == null)
							{
								throw new BleTxException("GatewayService is null");
							}
							if (bleCommunicationsAdapter.GatewayCanTxCharacteristic == null)
							{
								val3 = bleCommunicationsAdapter._bleManager.GetCharacteristicAsync(bleCommunicationsAdapter.GatewayDevice, bleCommunicationsAdapter.GatewayService, GUID_IDS_CAN_WRITE, cancellationToken).GetAwaiter();
								if (!val3.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val3;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<ICharacteristic>, <SynchronizedWriteAsync>d__48>(ref val3, ref this);
									return;
								}
								goto IL_00d1;
							}
							goto IL_00e0;
						}
						goto end_IL_000e;
					case 0:
						val3 = <>u__1;
						<>u__1 = default(TaskAwaiter<ICharacteristic>);
						num = (<>1__state = -1);
						goto IL_00d1;
					case 1:
						val2 = <>u__2;
						<>u__2 = default(ConfiguredTaskAwaiter<bool>);
						num = (<>1__state = -1);
						goto IL_020b;
					case 2:
						{
							val = <>u__3;
							<>u__3 = default(TaskAwaiter);
							num = (<>1__state = -1);
							break;
						}
						IL_020b:
						if (!val2.GetResult())
						{
							TaggedLog.Error(bleCommunicationsAdapter.LogTag, "Error writing BLE characteristic.", global::System.Array.Empty<object>());
							throw new BleTxException("Could not write to characteristic");
						}
						val = global::System.Threading.Tasks.Task.Delay(1, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 2);
							<>u__3 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <SynchronizedWriteAsync>d__48>(ref val, ref this);
							return;
						}
						break;
						IL_00d1:
						result = val3.GetResult();
						bleCommunicationsAdapter.GatewayCanTxCharacteristic = result;
						goto IL_00e0;
						IL_00e0:
						if (bleCommunicationsAdapter.GatewayCanTxCharacteristic == null)
						{
							throw new BleTxException("Cannot read CAN write characteristic from gateway");
						}
						if (count < 20 && count >= 1)
						{
							byte[] array = default(byte[]);
							if (!bleCommunicationsAdapter._writeDataBuffers.TryGetValue(count, ref array))
							{
								array = new byte[count];
								bleCommunicationsAdapter._writeDataBuffers[count] = array;
							}
							Buffer.BlockCopy((global::System.Array)buffer, offset, (global::System.Array)array, 0, count);
							val2 = bleCommunicationsAdapter._bleManager.WriteCharacteristicAsync(bleCommunicationsAdapter.GatewayCanTxCharacteristic, array, cancellationToken).ConfigureAwait(false).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__2 = val2;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<bool>, <SynchronizedWriteAsync>d__48>(ref val2, ref this);
								return;
							}
							goto IL_020b;
						}
						bleCommunicationsAdapter.Warning("SynchronizedWriteAsync", $"invalid BLE message size of {count} bytes");
						goto end_IL_000e;
					}
					((TaskAwaiter)(ref val)).GetResult();
					end_IL_000e:;
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
		private struct <UnlockEcuAsync>d__53 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public IDevice device;

			public BleCommunicationsAdapter <>4__this;

			public IService service;

			public CancellationToken ct;

			public string pin;

			private ICharacteristic <unlockEcuCharacteristic>5__2;

			private byte[] <data>5__3;

			private TaskAwaiter<ICharacteristic?> <>u__1;

			private TaskAwaiter<byte[]?> <>u__2;

			private TaskAwaiter<bool> <>u__3;

			private TaskAwaiter <>u__4;

			private void MoveNext()
			{
				//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_0159: Unknown result type (might be due to invalid IL or missing references)
				//IL_015e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0166: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
				//IL_031d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0322: Unknown result type (might be due to invalid IL or missing references)
				//IL_032a: Unknown result type (might be due to invalid IL or missing references)
				//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_041f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0424: Unknown result type (might be due to invalid IL or missing references)
				//IL_042c: Unknown result type (might be due to invalid IL or missing references)
				//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
				//IL_011a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0124: Unknown result type (might be due to invalid IL or missing references)
				//IL_0129: Unknown result type (might be due to invalid IL or missing references)
				//IL_02de: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_0404: Unknown result type (might be due to invalid IL or missing references)
				//IL_0406: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0091: Unknown result type (might be due to invalid IL or missing references)
				//IL_013e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0140: Unknown result type (might be due to invalid IL or missing references)
				//IL_0371: Unknown result type (might be due to invalid IL or missing references)
				//IL_037b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0380: Unknown result type (might be due to invalid IL or missing references)
				//IL_0302: Unknown result type (might be due to invalid IL or missing references)
				//IL_0304: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0265: Unknown result type (might be due to invalid IL or missing references)
				//IL_026f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0274: Unknown result type (might be due to invalid IL or missing references)
				//IL_0395: Unknown result type (might be due to invalid IL or missing references)
				//IL_0397: Unknown result type (might be due to invalid IL or missing references)
				//IL_0289: Unknown result type (might be due to invalid IL or missing references)
				//IL_028b: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				BleCommunicationsAdapter bleCommunicationsAdapter = <>4__this;
				bool result;
				try
				{
					TaskAwaiter<ICharacteristic> val4;
					TaskAwaiter<bool> val3;
					TaskAwaiter val2;
					TaskAwaiter<byte[]> val;
					bool result2;
					ICharacteristic result4;
					byte[] result3;
					switch (num)
					{
					default:
						if (device == null)
						{
							bleCommunicationsAdapter.Error("UnlockEcuAsync", "The device is null, cannot unlock ECU");
							result = false;
						}
						else
						{
							if (service != null)
							{
								val4 = bleCommunicationsAdapter._bleManager.GetCharacteristicAsync(device, service, GUID_IDS_PASSWORD_UNLOCK, ct).GetAwaiter();
								if (!val4.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val4;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ICharacteristic>, <UnlockEcuAsync>d__53>(ref val4, ref this);
									return;
								}
								goto IL_00dd;
							}
							bleCommunicationsAdapter.Error("UnlockEcuAsync", "The service is null, cannot unlock ECU");
							result = false;
						}
						goto end_IL_000e;
					case 0:
						val4 = <>u__1;
						<>u__1 = default(TaskAwaiter<ICharacteristic>);
						num = (<>1__state = -1);
						goto IL_00dd;
					case 1:
						val = <>u__2;
						<>u__2 = default(TaskAwaiter<byte[]>);
						num = (<>1__state = -1);
						goto IL_0175;
					case 2:
						val3 = <>u__3;
						<>u__3 = default(TaskAwaiter<bool>);
						num = (<>1__state = -1);
						goto IL_02c0;
					case 3:
						val3 = <>u__3;
						<>u__3 = default(TaskAwaiter<bool>);
						num = (<>1__state = -1);
						goto IL_0339;
					case 4:
						val2 = <>u__4;
						<>u__4 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_03cc;
					case 5:
						{
							val = <>u__2;
							<>u__2 = default(TaskAwaiter<byte[]>);
							num = (<>1__state = -1);
							break;
						}
						IL_0341:
						if (result2)
						{
							val2 = global::System.Threading.Tasks.Task.Delay(1000, ct).GetAwaiter();
							if (!((TaskAwaiter)(ref val2)).IsCompleted)
							{
								num = (<>1__state = 4);
								<>u__4 = val2;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <UnlockEcuAsync>d__53>(ref val2, ref this);
								return;
							}
							goto IL_03cc;
						}
						bleCommunicationsAdapter.Error("UnlockEcuAsync", "Could not write unlock characteristic to " + device.Name);
						result = false;
						goto end_IL_000e;
						IL_03cc:
						((TaskAwaiter)(ref val2)).GetResult();
						val = bleCommunicationsAdapter._bleManager.ReadCharacteristicAsync(<unlockEcuCharacteristic>5__2, ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 5);
							<>u__2 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<byte[]>, <UnlockEcuAsync>d__53>(ref val, ref this);
							return;
						}
						break;
						IL_0339:
						result2 = val3.GetResult();
						goto IL_0341;
						IL_0175:
						result3 = val.GetResult();
						<data>5__3 = result3;
						if (<data>5__3 == null || <data>5__3.Length < 1)
						{
							bleCommunicationsAdapter.Error("UnlockEcuAsync", "Could not read unlock characteristic from " + device.Name);
							result = false;
						}
						else
						{
							if (<data>5__3[0] <= 0)
							{
								bleCommunicationsAdapter.Debug("UnlockEcuAsync", $"ECU not unlocked (0x{<data>5__3[0]:X}), attempting old style unlock");
								<data>5__3 = Encoding.UTF8.GetBytes(pin);
								val3 = bleCommunicationsAdapter._bleManager.WriteCharacteristicAsync(<unlockEcuCharacteristic>5__2, <data>5__3, ct).GetAwaiter();
								if (!val3.IsCompleted)
								{
									num = (<>1__state = 2);
									<>u__3 = val3;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <UnlockEcuAsync>d__53>(ref val3, ref this);
									return;
								}
								goto IL_02c0;
							}
							bleCommunicationsAdapter.Error("UnlockEcuAsync", device.Name + " is already unlocked");
							result = true;
						}
						goto end_IL_000e;
						IL_02c0:
						result2 = val3.GetResult();
						if (!result2)
						{
							val3 = bleCommunicationsAdapter._bleManager.WriteCharacteristicAsync(<unlockEcuCharacteristic>5__2, <data>5__3, ct).GetAwaiter();
							if (!val3.IsCompleted)
							{
								num = (<>1__state = 3);
								<>u__3 = val3;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <UnlockEcuAsync>d__53>(ref val3, ref this);
								return;
							}
							goto IL_0339;
						}
						goto IL_0341;
						IL_00dd:
						result4 = val4.GetResult();
						<unlockEcuCharacteristic>5__2 = result4;
						if (<unlockEcuCharacteristic>5__2 != null)
						{
							val = bleCommunicationsAdapter._bleManager.ReadCharacteristicAsync(<unlockEcuCharacteristic>5__2, ct).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__2 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<byte[]>, <UnlockEcuAsync>d__53>(ref val, ref this);
								return;
							}
							goto IL_0175;
						}
						bleCommunicationsAdapter.Error("UnlockEcuAsync", "Could not get unlock characteristic, cannot unlock ECU");
						result = false;
						goto end_IL_000e;
					}
					result3 = val.GetResult();
					<data>5__3 = result3;
					if (<data>5__3 == null)
					{
						bleCommunicationsAdapter.Error("UnlockEcuAsync", "Could not read back unlock characteristic after write from " + device.Name);
						result = false;
					}
					else
					{
						result = <data>5__3[0] > 0;
					}
					end_IL_000e:;
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<unlockEcuCharacteristic>5__2 = null;
					<data>5__3 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<unlockEcuCharacteristic>5__2 = null;
				<data>5__3 = null;
				<>t__builder.SetResult(result);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		public static readonly Guid GUID_IDS_CAN_SERVICE = new Guid("00000000-0200-A58E-E411-AFE28044E62C");

		public static readonly Guid GUID_IDS_CAN_READ = new Guid("00000002-0200-a58e-e411-afe28044e62c");

		public static readonly Guid GUID_IDS_READ_VERSION = new Guid("00000004-0200-a58e-e411-afe28044e62c");

		public static readonly Guid GUID_IDS_CAN_WRITE = new Guid("00000001-0200-A58E-E411-AFE28044E62C");

		public static readonly Guid GUID_IDS_PASSWORD_UNLOCK = new Guid("00000005-0200-a58e-e411-afe28044e62c");

		public static readonly byte[] LippertCompanyId = new byte[2] { 199, 5 };

		private const int ConnectTimeoutMs = 30000;

		public const int MinimumBleMtuSize = 23;

		public const int DefaultBleMtuSize = 185;

		private readonly Guid _id;

		private readonly string _name;

		private readonly string _passcode;

		private GatewayVersion _version;

		private IDevice? GatewayDevice;

		private IService? GatewayService;

		private ICharacteristic? GatewayCanRxCharacteristic;

		private ICharacteristic? GatewayCanTxCharacteristic;

		private readonly DeviceInstanceManager? _deviceInstanceManager;

		private readonly IBleManager _bleManager;

		private byte[]? _rawMessage;

		private readonly ConcurrentQueue<byte[]> _dataReceivedQueue = new ConcurrentQueue<byte[]>();

		private const int NetworkMsgSize = 11;

		private const int CircuitIdMsgSize = 7;

		private const int DeviceIdMsgSize = 11;

		private const int ElevenBitMsgMinSize = 3;

		private const int TwentyNineBitMsgMinSize = 5;

		private byte _dataLength;

		private byte _deviceAddress;

		private byte _networkStatus;

		private byte _idsCanVersion;

		private readonly byte[] _deviceMacAddress = new byte[6];

		private readonly byte[] _productId = new byte[2];

		private byte _productInstance;

		private byte _deviceType;

		private readonly byte[] _functionName = new byte[2];

		private byte _deviceInstance;

		private byte _functionInstance;

		private byte _deviceCapabilities;

		private byte _deviceStatusDataLength;

		private readonly byte[] _deviceStatusData = new byte[8];

		private readonly Dictionary<int, byte[]> _writeDataBuffers = new Dictionary<int, byte[]>();

		protected override string LogTag => "BleCommunicationsAdapter";

		public BleCommunicationsAdapter(IBleManager bleManager, Guid id, string name, string passcode, GatewayVersion version, PhysicalAddress mac)
			: this(bleManager, id, name, passcode, version, mac, verbose: false)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public BleCommunicationsAdapter(IBleManager bleManager, Guid id, string name, string passcode, GatewayVersion version, PhysicalAddress mac, bool verbose)
			: base(mac, name, verbose)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Invalid comparison between Unknown and I4
			_bleManager = bleManager;
			_id = id;
			_name = name;
			_passcode = passcode;
			_version = version;
			if ((int)_version == 2)
			{
				_deviceInstanceManager = new DeviceInstanceManager();
			}
			GatewayDevice = null;
		}

		[AsyncStateMachine(typeof(<SynchronizedConnectAsync>d__45))]
		protected override async global::System.Threading.Tasks.Task<bool> SynchronizedConnectAsync(CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			try
			{
				linkedCts.CancelAfter(30000);
				CancellationToken linkedCt = linkedCts.Token;
				if (GatewayDevice != null)
				{
					Warning("SynchronizedConnectAsync", "GatewayDevice is not null, need to clean up");
					return false;
				}
				try
				{
					_bleManager.DeviceDisconnected -= OnDeviceDisconnected;
				}
				catch
				{
				}
				try
				{
					_bleManager.DeviceConnectionLost -= OnDeviceConnectionLost;
				}
				catch
				{
				}
				_bleManager.DeviceDisconnected += OnDeviceDisconnected;
				_bleManager.DeviceConnectionLost += OnDeviceConnectionLost;
				VerboseDebug("SynchronizedConnectAsync", _name + ", subscribed to adapter events");
				GatewayDevice = await _bleManager.TryConnectToDeviceAsync(_id, linkedCt);
				if (GatewayDevice == null)
				{
					Warning("SynchronizedConnectAsync", _name + ", failed to connect");
					return false;
				}
				VerboseDebug("SynchronizedConnectAsync", GatewayDevice.Name + ", connected");
				GatewayService = await _bleManager.GetServiceAsync(GatewayDevice, GUID_IDS_CAN_SERVICE, linkedCt);
				if (GatewayService == null)
				{
					Error("SynchronizedConnectAsync", "Could not get CAN service from device");
					return false;
				}
				if (GatewayVersionExtensions.IsLargeMtuSupported(_version))
				{
					VerboseDebug("SynchronizedConnectAsync", $"BLE Connection {_id}({_name}): Requesting MTU Size {185}");
					int num = await GatewayDevice.RequestMtuAsync(185);
					if (num <= 0)
					{
						num = 23;
						Warning("SynchronizedConnectAsync", $"BLE Connection {_id}({_name}): Requesting MTU Size FAILED, assuming MTU size of {23}");
					}
					VerboseDebug("SynchronizedConnectAsync", $"BLE Connection {_id}({_name}): Connected with MTU size of {num}");
				}
				if (!(await UnlockEcuAsync(GatewayDevice, GatewayService, _passcode, linkedCt)))
				{
					Warning("SynchronizedConnectAsync", _name + ", failed to unlock ECU");
					return false;
				}
				VerboseDebug("SynchronizedConnectAsync", GatewayDevice.Name + ", unlocked ECU");
				GatewayCanRxCharacteristic = await _bleManager.GetCharacteristicAsync(GatewayDevice, GatewayService, GUID_IDS_CAN_READ, linkedCt);
				if (GatewayCanRxCharacteristic == null)
				{
					return false;
				}
				GatewayCanRxCharacteristic.ValueUpdated += OnDataReceived;
				if (!(await _bleManager.StartCharacteristicUpdatesAsync(GatewayCanRxCharacteristic)))
				{
					Warning("SynchronizedConnectAsync", _name + ", failed to start characteristic update");
					return false;
				}
				VerboseDebug("SynchronizedConnectAsync", GatewayDevice.Name + ", gateway connected");
				return true;
			}
			finally
			{
				((global::System.IDisposable)linkedCts)?.Dispose();
			}
		}

		[AsyncStateMachine(typeof(<SynchronizedDisconnectAsync>d__46))]
		protected override global::System.Threading.Tasks.Task SynchronizedDisconnectAsync()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			<SynchronizedDisconnectAsync>d__46 <SynchronizedDisconnectAsync>d__ = default(<SynchronizedDisconnectAsync>d__46);
			<SynchronizedDisconnectAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SynchronizedDisconnectAsync>d__.<>4__this = this;
			<SynchronizedDisconnectAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <SynchronizedDisconnectAsync>d__.<>t__builder)).Start<<SynchronizedDisconnectAsync>d__46>(ref <SynchronizedDisconnectAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <SynchronizedDisconnectAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<SynchronizedWriteAsync>d__48))]
		protected override global::System.Threading.Tasks.Task SynchronizedWriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			<SynchronizedWriteAsync>d__48 <SynchronizedWriteAsync>d__ = default(<SynchronizedWriteAsync>d__48);
			<SynchronizedWriteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SynchronizedWriteAsync>d__.<>4__this = this;
			<SynchronizedWriteAsync>d__.buffer = buffer;
			<SynchronizedWriteAsync>d__.offset = offset;
			<SynchronizedWriteAsync>d__.count = count;
			<SynchronizedWriteAsync>d__.cancellationToken = cancellationToken;
			<SynchronizedWriteAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <SynchronizedWriteAsync>d__.<>t__builder)).Start<<SynchronizedWriteAsync>d__48>(ref <SynchronizedWriteAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <SynchronizedWriteAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<SynchronizedReadAsync>d__49))]
		protected override async global::System.Threading.Tasks.Task<int> SynchronizedReadAsync(byte[] buffer, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			byte[] data = null;
			try
			{
				while (!_dataReceivedQueue.TryDequeue(ref data))
				{
					await global::System.Threading.Tasks.Task.Delay(5, cancellationToken).ConfigureAwait(false);
				}
				int num = data[0];
				if (buffer.Length < num)
				{
					throw new IndexOutOfRangeException("Message received is larger than the buffer allocated!!");
				}
				Buffer.BlockCopy((global::System.Array)data, 1, (global::System.Array)buffer, 0, num);
				return num;
			}
			finally
			{
				if (data != null)
				{
					ArrayPool<byte>.Shared.Return(data, false);
				}
			}
		}

		private void OnDataReceived(object sender, CharacteristicUpdatedEventArgs args)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Invalid comparison between Unknown and I4
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Invalid comparison between Unknown and I4
			try
			{
				_rawMessage = args.Characteristic.Value;
				if (!base.Connected)
				{
					Warning("OnDataReceived", "message received while not connected - " + BitConverter.ToString(_rawMessage));
					return;
				}
				if (_rawMessage.Length == 0)
				{
					Warning("OnDataReceived", "zero length message received");
					return;
				}
				if ((int)_version == 1)
				{
					byte[] array = ArrayPool<byte>.Shared.Rent(1 + _rawMessage.Length);
					array[0] = (byte)_rawMessage.Length;
					Buffer.BlockCopy((global::System.Array)_rawMessage, 0, (global::System.Array)array, 1, _rawMessage.Length);
					_dataReceivedQueue.Enqueue(array);
					if (_rawMessage.Length - (_rawMessage[0] & 0xF) == 3 && _rawMessage[1] == 0)
					{
						array = MakeFakeCircuitIDMessage(_rawMessage[2]);
						_dataReceivedQueue.Enqueue(array);
						Debug("OnDataReceived", BitConverter.ToString(array, 1, (int)array[0]));
					}
					return;
				}
				V2MessageType v2MessageType = (V2MessageType)_rawMessage[0];
				switch (v2MessageType)
				{
				case V2MessageType.Packed:
				{
					_deviceAddress = _rawMessage[1];
					_networkStatus = _rawMessage[2];
					_idsCanVersion = _rawMessage[3];
					Buffer.BlockCopy((global::System.Array)_rawMessage, 4, (global::System.Array)_deviceMacAddress, 0, 6);
					Buffer.BlockCopy((global::System.Array)_rawMessage, 10, (global::System.Array)_productId, 0, 2);
					_productInstance = _rawMessage[12];
					_deviceType = _rawMessage[13];
					Buffer.BlockCopy((global::System.Array)_rawMessage, 14, (global::System.Array)_functionName, 0, 2);
					_deviceInstance = (byte)((0xF0 & _rawMessage[16]) >> 4);
					_functionInstance = (byte)(0xF & _rawMessage[16]);
					_deviceCapabilities = _rawMessage[17];
					_deviceStatusDataLength = _rawMessage[18];
					Buffer.BlockCopy((global::System.Array)_rawMessage, 19, (global::System.Array)_deviceStatusData, 0, (int)_deviceStatusDataLength);
					if ((int)_version == 2 && _deviceInstanceManager != null && !_deviceInstanceManager.TryGetDeviceInstance(_deviceMacAddress, _deviceType, _functionName, _functionInstance, _deviceAddress, out _deviceInstance))
					{
						Watchdog? readMessageWatchdog = _readMessageWatchdog;
						if (readMessageWatchdog != null)
						{
							readMessageWatchdog.TryPet(false);
						}
						break;
					}
					byte[] array4 = ArrayPool<byte>.Shared.Rent(12);
					array4[0] = 11;
					array4[1] = 8;
					array4[2] = 0;
					array4[3] = _deviceAddress;
					array4[4] = _networkStatus;
					array4[5] = _idsCanVersion;
					Buffer.BlockCopy((global::System.Array)_deviceMacAddress, 0, (global::System.Array)array4, 6, _deviceMacAddress.Length);
					_dataReceivedQueue.Enqueue(array4);
					byte[] array5 = ArrayPool<byte>.Shared.Rent(12);
					array5[0] = 11;
					array5[1] = 8;
					array5[2] = 2;
					array5[3] = _deviceAddress;
					Buffer.BlockCopy((global::System.Array)_productId, 0, (global::System.Array)array5, 4, 2);
					array5[6] = _productInstance;
					array5[7] = _deviceType;
					Buffer.BlockCopy((global::System.Array)_functionName, 0, (global::System.Array)array5, 8, 2);
					array5[10] = (byte)((_deviceInstance << 4) | _functionInstance);
					array5[11] = _deviceCapabilities;
					_dataReceivedQueue.Enqueue(array5);
					byte[] array6 = ArrayPool<byte>.Shared.Rent(4 + _deviceStatusDataLength);
					array6[0] = (byte)(3 + _deviceStatusDataLength);
					array6[1] = _deviceStatusDataLength;
					array6[2] = 3;
					array6[3] = _deviceAddress;
					Buffer.BlockCopy((global::System.Array)_deviceStatusData, 0, (global::System.Array)array6, 4, (int)_deviceStatusDataLength);
					_dataReceivedQueue.Enqueue(array6);
					byte[] array7 = MakeFakeCircuitIDMessage(_deviceAddress);
					_dataReceivedQueue.Enqueue(array7);
					byte[] array8 = ArrayPool<byte>.Shared.Rent(12);
					Buffer.BlockCopy((global::System.Array)array4, 0, (global::System.Array)array8, 0, 12);
					_dataReceivedQueue.Enqueue(array8);
					break;
				}
				case V2MessageType.ElevenBit:
				{
					MESSAGE_TYPE.op_Implicit(_rawMessage[3]);
					_deviceAddress = _rawMessage[4];
					_dataLength = _rawMessage[5];
					byte[] array3 = ArrayPool<byte>.Shared.Rent(4 + _dataLength);
					array3[0] = (byte)(3 + _dataLength);
					array3[1] = _dataLength;
					Buffer.BlockCopy((global::System.Array)_rawMessage, 3, (global::System.Array)array3, 2, 2);
					Buffer.BlockCopy((global::System.Array)_rawMessage, 6, (global::System.Array)array3, 4, (int)_dataLength);
					_dataReceivedQueue.Enqueue(array3);
					break;
				}
				case V2MessageType.TwentyNineBit:
				{
					MESSAGE_TYPE.op_Implicit((byte)((_rawMessage[1] & 0x1C) | (_rawMessage[2] & 3)));
					_dataLength = _rawMessage[5];
					byte[] array2 = ArrayPool<byte>.Shared.Rent(6 + _dataLength);
					array2[0] = (byte)(5 + _dataLength);
					array2[1] = _dataLength;
					Buffer.BlockCopy((global::System.Array)_rawMessage, 1, (global::System.Array)array2, 2, 4);
					Buffer.BlockCopy((global::System.Array)_rawMessage, 6, (global::System.Array)array2, 6, (int)_dataLength);
					_dataReceivedQueue.Enqueue(array2);
					break;
				}
				default:
					Error("OnDataReceived", $"unknown V2, type={v2MessageType}: {BitConverter.ToString(_rawMessage)}");
					break;
				}
			}
			catch (global::System.Exception ex)
			{
				Error("OnDataReceived", $"Exception in {"OnDataReceived"} : {ex}");
			}
		}

		private void OnDeviceDisconnected(object sender, DeviceEventArgs args)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			Guid id = args.Device.Id;
			IDevice? gatewayDevice = GatewayDevice;
			Guid? val = ((gatewayDevice != null) ? new Guid?(gatewayDevice.Id) : ((Guid?)null));
			if (val.HasValue && !(id != val.GetValueOrDefault()))
			{
				string name = args.Device.Name;
				IDevice? gatewayDevice2 = GatewayDevice;
				if (!(name != ((gatewayDevice2 != null) ? gatewayDevice2.Name : null)))
				{
					Information("OnDeviceDisconnected", "remote disconnect from " + args.Device.Name + ", disconnecting from device");
					DisconnectAsync(CancellationToken.None);
				}
			}
		}

		private void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs args)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			Guid id = ((DeviceEventArgs)args).Device.Id;
			IDevice? gatewayDevice = GatewayDevice;
			Guid? val = ((gatewayDevice != null) ? new Guid?(gatewayDevice.Id) : ((Guid?)null));
			if (val.HasValue && !(id != val.GetValueOrDefault()))
			{
				string name = ((DeviceEventArgs)args).Device.Name;
				IDevice? gatewayDevice2 = GatewayDevice;
				if (!(name != ((gatewayDevice2 != null) ? gatewayDevice2.Name : null)))
				{
					Error("OnDeviceConnectionLost", "unexpected disconnect from " + ((DeviceEventArgs)args).Device.Name + ", disconnecting from device");
					DisconnectAsync(CancellationToken.None);
				}
			}
		}

		[AsyncStateMachine(typeof(<UnlockEcuAsync>d__53))]
		public async global::System.Threading.Tasks.Task<bool> UnlockEcuAsync(IDevice? device, IService? service, string? pin, CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (device == null)
			{
				Error("UnlockEcuAsync", "The device is null, cannot unlock ECU");
				return false;
			}
			if (service == null)
			{
				Error("UnlockEcuAsync", "The service is null, cannot unlock ECU");
				return false;
			}
			ICharacteristic unlockEcuCharacteristic = await _bleManager.GetCharacteristicAsync(device, service, GUID_IDS_PASSWORD_UNLOCK, ct);
			if (unlockEcuCharacteristic == null)
			{
				Error("UnlockEcuAsync", "Could not get unlock characteristic, cannot unlock ECU");
				return false;
			}
			byte[] data = await _bleManager.ReadCharacteristicAsync(unlockEcuCharacteristic, ct);
			if (data == null || data.Length < 1)
			{
				Error("UnlockEcuAsync", "Could not read unlock characteristic from " + device.Name);
				return false;
			}
			if (data[0] > 0)
			{
				Error("UnlockEcuAsync", device.Name + " is already unlocked");
				return true;
			}
			Debug("UnlockEcuAsync", $"ECU not unlocked (0x{data[0]:X}), attempting old style unlock");
			data = Encoding.UTF8.GetBytes(pin);
			bool flag = await _bleManager.WriteCharacteristicAsync(unlockEcuCharacteristic, data, ct);
			if (!flag)
			{
				flag = await _bleManager.WriteCharacteristicAsync(unlockEcuCharacteristic, data, ct);
			}
			if (!flag)
			{
				Error("UnlockEcuAsync", "Could not write unlock characteristic to " + device.Name);
				return false;
			}
			await global::System.Threading.Tasks.Task.Delay(1000, ct);
			data = await _bleManager.ReadCharacteristicAsync(unlockEcuCharacteristic, ct);
			if (data == null)
			{
				Error("UnlockEcuAsync", "Could not read back unlock characteristic after write from " + device.Name);
				return false;
			}
			return data[0] > 0;
		}

		private byte[] MakeFakeCircuitIDMessage(byte addr)
		{
			byte[] array = ArrayPool<byte>.Shared.Rent(8);
			array[0] = 7;
			array[1] = 4;
			array[2] = 1;
			array[3] = addr;
			array[4] = 0;
			array[5] = 0;
			array[6] = 0;
			array[7] = 0;
			return array;
		}
	}
	public abstract class SynchronizedCommunicationsAdapter : Adapter<MessageBuffer>
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ConnectAsync>d__49 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public AsyncOperation obj;

			public SynchronizedCommunicationsAdapter <>4__this;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				SynchronizedCommunicationsAdapter synchronizedCommunicationsAdapter = <>4__this;
				bool result;
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						AsyncOperation obj = this.obj;
						CancellationToken ct = ((obj != null) ? obj.CancellationToken : CancellationToken.None);
						val = synchronizedCommunicationsAdapter.ConnectAsync(ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <ConnectAsync>d__49>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<bool>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
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
		private struct <ConnectAsync>d__52 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public SynchronizedCommunicationsAdapter <>4__this;

			public CancellationToken ct;

			private bool <locked>5__2;

			private object <>7__wrap2;

			private int <>7__wrap3;

			private bool <>7__wrap4;

			private ConfiguredTaskAwaiter <>u__1;

			private ConfiguredTaskAwaiter<bool> <>u__2;

			private void MoveNext()
			{
				//IL_0379: Unknown result type (might be due to invalid IL or missing references)
				//IL_037e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0385: Unknown result type (might be due to invalid IL or missing references)
				//IL_033d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0342: Unknown result type (might be due to invalid IL or missing references)
				//IL_0346: Unknown result type (might be due to invalid IL or missing references)
				//IL_034b: Unknown result type (might be due to invalid IL or missing references)
				//IL_035f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0360: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00df: Unknown result type (might be due to invalid IL or missing references)
				//IL_0181: Unknown result type (might be due to invalid IL or missing references)
				//IL_0186: Unknown result type (might be due to invalid IL or missing references)
				//IL_018e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				//IL_0143: Unknown result type (might be due to invalid IL or missing references)
				//IL_0148: Unknown result type (might be due to invalid IL or missing references)
				//IL_014c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0151: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0166: Unknown result type (might be due to invalid IL or missing references)
				//IL_0168: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01de: Expected O, but got Unknown
				//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0207: Expected O, but got Unknown
				//IL_0202: Unknown result type (might be due to invalid IL or missing references)
				//IL_020c: Expected O, but got Unknown
				//IL_021a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0237: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				SynchronizedCommunicationsAdapter CS$<>8__locals39 = <>4__this;
				bool result2;
				try
				{
					ConfiguredTaskAwaiter val;
					if ((uint)num > 1u)
					{
						if (num == 2)
						{
							val = <>u__1;
							<>u__1 = default(ConfiguredTaskAwaiter);
							num = (<>1__state = -1);
							goto IL_0394;
						}
						CS$<>8__locals39.VerboseDebugBase("ConnectAsync", "BEGIN");
						<locked>5__2 = false;
						<>7__wrap2 = null;
						<>7__wrap3 = 0;
					}
					ConfiguredTaskAwaitable val3;
					try
					{
						_ = 1;
						try
						{
							if (num == 0)
							{
								val = <>u__1;
								<>u__1 = default(ConfiguredTaskAwaiter);
								num = (<>1__state = -1);
								goto IL_00ee;
							}
							ConfiguredTaskAwaiter<bool> val2;
							if (num == 1)
							{
								val2 = <>u__2;
								<>u__2 = default(ConfiguredTaskAwaiter<bool>);
								num = (<>1__state = -1);
								goto IL_019d;
							}
							if (!CS$<>8__locals39.Connected)
							{
								CS$<>8__locals39.VerboseDebugBase("ConnectAsync", "Acquiring lock");
								val3 = CS$<>8__locals39._lock.WaitAsync(ct).ConfigureAwait(false);
								val = ((ConfiguredTaskAwaitable)(ref val3)).GetAwaiter();
								if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <ConnectAsync>d__52>(ref val, ref this);
									return;
								}
								goto IL_00ee;
							}
							CS$<>8__locals39.DebugBase("ConnectAsync", "already connected");
							<>7__wrap4 = true;
							goto end_IL_0044;
							IL_019d:
							bool result = val2.GetResult();
							CS$<>8__locals39.Connected = result;
							if (!CS$<>8__locals39.Connected)
							{
								<>7__wrap4 = false;
							}
							else
							{
								CancellationTokenSource? cts = CS$<>8__locals39._cts;
								if (cts != null)
								{
									CancellationTokenSourceExtension.TryCancelAndDispose(cts);
								}
								CS$<>8__locals39._cts = new CancellationTokenSource();
								CS$<>8__locals39._ct = CS$<>8__locals39._cts.Token;
								CS$<>8__locals39._readMessageWatchdog = new Watchdog(3000, (Action)([CompilerGenerated] () =>
								{
									//IL_0011: Unknown result type (might be due to invalid IL or missing references)
									CS$<>8__locals39.ErrorBase("ReadTask", "time out");
									CS$<>8__locals39.DisconnectAsync(CancellationToken.None);
								}), false);
								CS$<>8__locals39._writeTask = global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([CompilerGenerated] () => CS$<>8__locals39.WriteTask(CS$<>8__locals39._ct)), CS$<>8__locals39._ct);
								CS$<>8__locals39._readTask = global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([CompilerGenerated] () => CS$<>8__locals39.ReadTask(CS$<>8__locals39._ct)), CS$<>8__locals39._ct);
								CS$<>8__locals39.TryRaiseAdapterOpened();
								CS$<>8__locals39.DebugBase("ConnectAsync", "adapter opened raised");
								<>7__wrap4 = true;
							}
							goto end_IL_0044;
							IL_00ee:
							((ConfiguredTaskAwaiter)(ref val)).GetResult();
							<locked>5__2 = true;
							CS$<>8__locals39.VerboseDebugBase("ConnectAsync", "Lock acquired");
							if (!CS$<>8__locals39.Connected)
							{
								CS$<>8__locals39.ForceReturnPooledMessages();
								val2 = CS$<>8__locals39.SynchronizedConnectAsync(ct).ConfigureAwait(false).GetAwaiter();
								if (!val2.IsCompleted)
								{
									num = (<>1__state = 1);
									<>u__2 = val2;
									<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<bool>, <ConnectAsync>d__52>(ref val2, ref this);
									return;
								}
								goto IL_019d;
							}
							CS$<>8__locals39.DebugBase("ConnectAsync", "already connected");
							<>7__wrap4 = true;
							end_IL_0044:;
						}
						catch (OperationCanceledException)
						{
							if (((CancellationToken)(ref ct)).IsCancellationRequested)
							{
								CS$<>8__locals39.InformationBase("ConnectAsync", "canceled");
							}
							else
							{
								CS$<>8__locals39.ErrorBase("ConnectAsync", "time out");
							}
							goto end_IL_003f;
						}
						catch (global::System.Exception ex2)
						{
							CS$<>8__locals39.ErrorBase("ConnectAsync", $"{ex2.GetType()} - {ex2.Message}\n{ex2.StackTrace}");
							goto end_IL_003f;
						}
						<>7__wrap3 = 1;
						end_IL_003f:;
					}
					catch (object obj)
					{
						<>7__wrap2 = obj;
					}
					if (<locked>5__2)
					{
						if (!CS$<>8__locals39.Connected)
						{
							CS$<>8__locals39.WarningBase("ConnectAsync", "connection not initialized, need to clean up");
							val3 = CS$<>8__locals39.InternalDisconnectAsync(raiseAdapterClosed: false).ConfigureAwait(false);
							val = ((ConfiguredTaskAwaitable)(ref val3)).GetAwaiter();
							if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 2);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <ConnectAsync>d__52>(ref val, ref this);
								return;
							}
							goto IL_0394;
						}
						goto IL_039b;
					}
					goto IL_03c7;
					IL_0394:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
					goto IL_039b;
					IL_03c7:
					CS$<>8__locals39.VerboseDebugBase("ConnectAsync", "END");
					object obj2 = <>7__wrap2;
					if (obj2 != null)
					{
						ExceptionDispatchInfo.Capture((obj2 as global::System.Exception) ?? throw obj2).Throw();
					}
					int num2 = <>7__wrap3;
					if (num2 == 1)
					{
						result2 = <>7__wrap4;
					}
					else
					{
						<>7__wrap2 = null;
						result2 = false;
					}
					goto end_IL_000e;
					IL_039b:
					CS$<>8__locals39.VerboseDebugBase("ConnectAsync", "Releasing lock");
					CS$<>8__locals39._lock.Release();
					CS$<>8__locals39.VerboseDebugBase("ConnectAsync", "Lock released");
					goto IL_03c7;
					end_IL_000e:;
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
		private struct <DisconnectAsync>d__50 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public SynchronizedCommunicationsAdapter <>4__this;

			public AsyncOperation obj;

			private ConfiguredTaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				SynchronizedCommunicationsAdapter synchronizedCommunicationsAdapter = <>4__this;
				bool result;
				try
				{
					ConfiguredTaskAwaiter<bool> val;
					if (num != 0)
					{
						AsyncOperation obj = this.obj;
						val = synchronizedCommunicationsAdapter.DisconnectAsync((obj != null) ? obj.CancellationToken : CancellationToken.None).ConfigureAwait(false).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<bool>, <DisconnectAsync>d__50>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(ConfiguredTaskAwaiter<bool>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
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
		private struct <DisconnectAsync>d__53 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public SynchronizedCommunicationsAdapter <>4__this;

			public CancellationToken ct;

			public bool raiseAdapterClosed;

			private bool <locked>5__2;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0155: Unknown result type (might be due to invalid IL or missing references)
				//IL_015a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0161: Unknown result type (might be due to invalid IL or missing references)
				//IL_0119: Unknown result type (might be due to invalid IL or missing references)
				//IL_011e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0122: Unknown result type (might be due to invalid IL or missing references)
				//IL_0127: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0081: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				//IL_013b: Unknown result type (might be due to invalid IL or missing references)
				//IL_013c: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Unknown result type (might be due to invalid IL or missing references)
				//IL_009b: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				SynchronizedCommunicationsAdapter synchronizedCommunicationsAdapter = <>4__this;
				bool result;
				try
				{
					if ((uint)num > 1u)
					{
						synchronizedCommunicationsAdapter.VerboseDebugBase("DisconnectAsync", "BEGIN");
						<locked>5__2 = false;
					}
					try
					{
						ConfiguredTaskAwaiter val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(ConfiguredTaskAwaiter);
							num = (<>1__state = -1);
							goto IL_00cf;
						}
						if (num == 1)
						{
							val = <>u__1;
							<>u__1 = default(ConfiguredTaskAwaiter);
							num = (<>1__state = -1);
							goto IL_0170;
						}
						ConfiguredTaskAwaitable val2;
						if (synchronizedCommunicationsAdapter.Connected)
						{
							synchronizedCommunicationsAdapter.VerboseDebugBase("DisconnectAsync", "Acquiring lock");
							val2 = synchronizedCommunicationsAdapter._lock.WaitAsync(ct).ConfigureAwait(false);
							val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
							if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <DisconnectAsync>d__53>(ref val, ref this);
								return;
							}
							goto IL_00cf;
						}
						synchronizedCommunicationsAdapter.DebugBase("DisconnectAsync", "already disconnected");
						result = true;
						goto end_IL_000e;
						IL_0170:
						((ConfiguredTaskAwaiter)(ref val)).GetResult();
						goto end_IL_002a;
						IL_00cf:
						((ConfiguredTaskAwaiter)(ref val)).GetResult();
						<locked>5__2 = true;
						synchronizedCommunicationsAdapter.VerboseDebugBase("DisconnectAsync", "Lock acquired");
						if (synchronizedCommunicationsAdapter.Connected)
						{
							val2 = synchronizedCommunicationsAdapter.InternalDisconnectAsync(raiseAdapterClosed).ConfigureAwait(false);
							val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
							if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <DisconnectAsync>d__53>(ref val, ref this);
								return;
							}
							goto IL_0170;
						}
						synchronizedCommunicationsAdapter.DebugBase("DisconnectAsync", "already disconnected");
						result = true;
						goto end_IL_000e;
						end_IL_002a:;
					}
					catch (global::System.Exception ex)
					{
						if (((CancellationToken)(ref synchronizedCommunicationsAdapter._ct)).IsCancellationRequested)
						{
							synchronizedCommunicationsAdapter.InformationBase("DisconnectAsync", "canceled");
						}
						else
						{
							synchronizedCommunicationsAdapter.ErrorBase("DisconnectAsync", $"{ex.GetType()} - {ex.Message}\n{ex.StackTrace}");
						}
					}
					finally
					{
						if (num < 0 && <locked>5__2)
						{
							synchronizedCommunicationsAdapter.VerboseDebugBase("DisconnectAsync", "Releasing lock");
							synchronizedCommunicationsAdapter._lock.Release();
							synchronizedCommunicationsAdapter.VerboseDebugBase("DisconnectAsync", "Lock released");
						}
					}
					synchronizedCommunicationsAdapter.VerboseDebugBase("DisconnectAsync", "END");
					result = <locked>5__2;
					end_IL_000e:;
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
		private struct <InternalDisconnectAsync>d__54 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public SynchronizedCommunicationsAdapter <>4__this;

			public bool raiseAdapterClosed;

			private global::System.Threading.Tasks.Task <writeTask>5__2;

			private global::System.Threading.Tasks.Task <readTask>5__3;

			private ConfiguredTaskAwaiter<global::System.Threading.Tasks.Task> <>u__1;

			private ConfiguredTaskAwaiter <>u__2;

			private void MoveNext()
			{
				//IL_0147: Unknown result type (might be due to invalid IL or missing references)
				//IL_014c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0154: Unknown result type (might be due to invalid IL or missing references)
				//IL_0171: Unknown result type (might be due to invalid IL or missing references)
				//IL_0177: Invalid comparison between Unknown and I4
				//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0201: Unknown result type (might be due to invalid IL or missing references)
				//IL_0209: Unknown result type (might be due to invalid IL or missing references)
				//IL_018f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0195: Invalid comparison between Unknown and I4
				//IL_01be: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
				//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_0109: Unknown result type (might be due to invalid IL or missing references)
				//IL_010e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0112: Unknown result type (might be due to invalid IL or missing references)
				//IL_0117: Unknown result type (might be due to invalid IL or missing references)
				//IL_012c: Unknown result type (might be due to invalid IL or missing references)
				//IL_012e: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				SynchronizedCommunicationsAdapter synchronizedCommunicationsAdapter = <>4__this;
				try
				{
					if ((uint)num > 1u)
					{
						synchronizedCommunicationsAdapter.VerboseDebugBase("InternalDisconnectAsync", "BEGIN");
						synchronizedCommunicationsAdapter.Connected = false;
					}
					try
					{
						ConfiguredTaskAwaiter val;
						ConfiguredTaskAwaiter<global::System.Threading.Tasks.Task> val2;
						if (num != 0)
						{
							if (num == 1)
							{
								val = <>u__2;
								<>u__2 = default(ConfiguredTaskAwaiter);
								num = (<>1__state = -1);
								goto IL_0218;
							}
							synchronizedCommunicationsAdapter.VerboseDebugBase("InternalDisconnectAsync", "Stopping _readMessageWatchdog");
							Watchdog? readMessageWatchdog = synchronizedCommunicationsAdapter._readMessageWatchdog;
							if (readMessageWatchdog != null)
							{
								((CommonDisposable)readMessageWatchdog).TryDispose();
							}
							synchronizedCommunicationsAdapter.VerboseDebugBase("InternalDisconnectAsync", "Stopping WriteTask and ReadTask");
							CancellationTokenSource? cts = synchronizedCommunicationsAdapter._cts;
							if (cts != null)
							{
								CancellationTokenSourceExtension.TryCancelAndDispose(cts);
							}
							synchronizedCommunicationsAdapter._cts = null;
							<writeTask>5__2 = synchronizedCommunicationsAdapter._writeTask ?? global::System.Threading.Tasks.Task.FromResult<bool>(true);
							<readTask>5__3 = synchronizedCommunicationsAdapter._readTask ?? global::System.Threading.Tasks.Task.FromResult<bool>(true);
							<>y__InlineArray2<global::System.Threading.Tasks.Task> buffer = default(<>y__InlineArray2<global::System.Threading.Tasks.Task>);
							<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray2<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(ref buffer, 0) = <writeTask>5__2;
							<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray2<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(ref buffer, 1) = <readTask>5__3;
							global::System.Threading.Tasks.Task task = global::System.Threading.Tasks.Task.WhenAll(<PrivateImplementationDetails>.InlineArrayAsReadOnlySpan<<>y__InlineArray2<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(in buffer, 2));
							global::System.Threading.Tasks.Task task2 = global::System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(30L), CancellationToken.None);
							synchronizedCommunicationsAdapter.VerboseDebugBase("InternalDisconnectAsync", "Waiting for WriteTask and ReadTask to complete");
							val2 = global::System.Threading.Tasks.Task.WhenAny(task, task2).ConfigureAwait(false).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val2;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<global::System.Threading.Tasks.Task>, <InternalDisconnectAsync>d__54>(ref val2, ref this);
								return;
							}
						}
						else
						{
							val2 = <>u__1;
							<>u__1 = default(ConfiguredTaskAwaiter<global::System.Threading.Tasks.Task>);
							num = (<>1__state = -1);
						}
						val2.GetResult();
						if ((int)<writeTask>5__2.Status != 5)
						{
							synchronizedCommunicationsAdapter.Error("InternalDisconnectAsync", "Write task not completed!");
						}
						if ((int)<readTask>5__3.Status != 5)
						{
							synchronizedCommunicationsAdapter.Error("InternalDisconnectAsync", "Read task not completed!");
						}
						synchronizedCommunicationsAdapter.VerboseDebugBase("InternalDisconnectAsync", "Calling disconnect for implementation");
						ConfiguredTaskAwaitable val3 = synchronizedCommunicationsAdapter.SynchronizedDisconnectAsync().ConfigureAwait(false);
						val = ((ConfiguredTaskAwaitable)(ref val3)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__2 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <InternalDisconnectAsync>d__54>(ref val, ref this);
							return;
						}
						goto IL_0218;
						IL_0218:
						((ConfiguredTaskAwaiter)(ref val)).GetResult();
						synchronizedCommunicationsAdapter.VerboseDebugBase("InternalDisconnectAsync", "Clearing messages from _writeBuffer");
						synchronizedCommunicationsAdapter.ForceReturnPooledMessages();
						<writeTask>5__2 = null;
						<readTask>5__3 = null;
					}
					catch (global::System.Exception ex)
					{
						synchronizedCommunicationsAdapter.ErrorBase("InternalDisconnectAsync", $"{ex.GetType()} - {ex.Message}\n{ex.StackTrace}");
					}
					finally
					{
						if (num < 0 && raiseAdapterClosed)
						{
							synchronizedCommunicationsAdapter.TryRaiseAdapterClosed();
							synchronizedCommunicationsAdapter.DebugBase("InternalDisconnectAsync", "adapter closed raised");
						}
					}
					synchronizedCommunicationsAdapter.VerboseDebugBase("InternalDisconnectAsync", "END");
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
		private struct <ReadTask>d__56 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public SynchronizedCommunicationsAdapter <>4__this;

			public CancellationToken ct;

			private MessageBuffer <msgBuf>5__2;

			private MessageBuffer <>7__wrap2;

			private ConfiguredTaskAwaiter<int> <>u__1;

			private void MoveNext()
			{
				//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Expected O, but got Unknown
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				SynchronizedCommunicationsAdapter synchronizedCommunicationsAdapter = <>4__this;
				try
				{
					if (num != 0)
					{
						synchronizedCommunicationsAdapter.VerboseDebugBase("ReadTask", "BEGIN");
						<msgBuf>5__2 = new MessageBuffer(4096);
						Watchdog? readMessageWatchdog = synchronizedCommunicationsAdapter._readMessageWatchdog;
						if (readMessageWatchdog != null)
						{
							readMessageWatchdog.Monitor();
						}
					}
					try
					{
						if (num != 0)
						{
							goto IL_0156;
						}
						ConfiguredTaskAwaiter<int> val = <>u__1;
						<>u__1 = default(ConfiguredTaskAwaiter<int>);
						num = (<>1__state = -1);
						goto IL_00c9;
						IL_00c9:
						int result = val.GetResult();
						int num2 = (<>7__wrap2.Length = result);
						<>7__wrap2 = null;
						if (num2 == 0)
						{
							throw new ArgumentException("0 bytes received from network adapter");
						}
						if (num2 > 2048)
						{
							synchronizedCommunicationsAdapter.Debug("ReadTask", $"Read buffer exceeding halfway point {<msgBuf>5__2.Length}");
						}
						Watchdog? readMessageWatchdog2 = synchronizedCommunicationsAdapter._readMessageWatchdog;
						if (readMessageWatchdog2 != null)
						{
							readMessageWatchdog2.TryPet(false);
						}
						synchronizedCommunicationsAdapter.TryRaiseMessageRx(<msgBuf>5__2, echo: false);
						goto IL_0156;
						IL_0156:
						if (!((CancellationToken)(ref ct)).IsCancellationRequested)
						{
							<>7__wrap2 = <msgBuf>5__2;
							val = synchronizedCommunicationsAdapter.SynchronizedReadAsync(<msgBuf>5__2.Data, ct).ConfigureAwait(false).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<int>, <ReadTask>d__56>(ref val, ref this);
								return;
							}
							goto IL_00c9;
						}
					}
					catch (global::System.Exception ex)
					{
						if (((CancellationToken)(ref ct)).IsCancellationRequested)
						{
							synchronizedCommunicationsAdapter.InformationBase("ReadTask", "canceled");
						}
						else
						{
							synchronizedCommunicationsAdapter.ErrorBase("ReadTask", $"{ex.GetType()} - {ex.Message}\n{ex.StackTrace}");
							synchronizedCommunicationsAdapter.VerboseDebugBase("ReadTask", "Calling DisconnectAsync");
							synchronizedCommunicationsAdapter.DisconnectAsync(CancellationToken.None);
							synchronizedCommunicationsAdapter.VerboseDebugBase("ReadTask", "DisconnectAsync called");
						}
					}
					synchronizedCommunicationsAdapter.VerboseDebugBase("ReadTask", "END");
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<msgBuf>5__2 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<msgBuf>5__2 = null;
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
		private struct <WriteTask>d__55 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public SynchronizedCommunicationsAdapter <>4__this;

			public CancellationToken ct;

			private byte[] <emptyMessage>5__2;

			private byte[] <message>5__3;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_026b: Unknown result type (might be due to invalid IL or missing references)
				//IL_011a: Unknown result type (might be due to invalid IL or missing references)
				//IL_011f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0127: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_0162: Unknown result type (might be due to invalid IL or missing references)
				//IL_016c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0171: Unknown result type (might be due to invalid IL or missing references)
				//IL_0186: Unknown result type (might be due to invalid IL or missing references)
				//IL_0188: Unknown result type (might be due to invalid IL or missing references)
				//IL_00db: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				SynchronizedCommunicationsAdapter synchronizedCommunicationsAdapter = <>4__this;
				try
				{
					if ((uint)num > 1u)
					{
						synchronizedCommunicationsAdapter.VerboseDebugBase("WriteTask", "BEGIN");
						<emptyMessage>5__2 = new byte[0];
						<message>5__3 = null;
						int num2 = 0;
						int num3 = 0;
					}
					try
					{
						TaskAwaiter val;
						if (num != 0)
						{
							if (num != 1)
							{
								goto IL_01c4;
							}
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_01bd;
						}
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_0136;
						IL_01bd:
						((TaskAwaiter)(ref val)).GetResult();
						goto IL_01c4;
						IL_01c4:
						if (!((CancellationToken)(ref ct)).IsCancellationRequested)
						{
							int num2;
							int num3;
							if (synchronizedCommunicationsAdapter._messageQueue.TryDequeue(ref <message>5__3))
							{
								if (<message>5__3.Length <= 4)
								{
									synchronizedCommunicationsAdapter.WarningBase("WriteTask", "0 bytes to write to network adapter");
									<message>5__3 = null;
									num2 = 0;
									num3 = 0;
								}
								else
								{
									num2 = 4;
									num3 = <message>5__3[0] | (<message>5__3[1] << 8) | (<message>5__3[2] << 16) | (<message>5__3[3] << 24);
								}
							}
							else
							{
								<message>5__3 = null;
								num2 = 0;
								num3 = 0;
							}
							val = synchronizedCommunicationsAdapter._writeBuffer.BufferedWriteAsync(<message>5__3 ?? <emptyMessage>5__2, num2, num3, ct).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <WriteTask>d__55>(ref val, ref this);
								return;
							}
							goto IL_0136;
						}
						goto end_IL_003a;
						IL_0136:
						((TaskAwaiter)(ref val)).GetResult();
						if (<message>5__3 != null)
						{
							ArrayPool<byte>.Shared.Return(<message>5__3, false);
							<message>5__3 = null;
							goto IL_01c4;
						}
						val = global::System.Threading.Tasks.Task.Delay(10, ct).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <WriteTask>d__55>(ref val, ref this);
							return;
						}
						goto IL_01bd;
						end_IL_003a:;
					}
					catch (global::System.Exception ex)
					{
						if (((CancellationToken)(ref ct)).IsCancellationRequested)
						{
							synchronizedCommunicationsAdapter.InformationBase("WriteTask", "canceled");
						}
						else
						{
							synchronizedCommunicationsAdapter.ErrorBase("WriteTask", $"{ex.GetType()} - {ex.Message}\n{ex.StackTrace}");
							synchronizedCommunicationsAdapter.VerboseDebugBase("ReadTask", "Calling DisconnectAsync");
							synchronizedCommunicationsAdapter.DisconnectAsync(CancellationToken.None);
							synchronizedCommunicationsAdapter.VerboseDebugBase("ReadTask", "DisconnectAsync called");
						}
					}
					finally
					{
						if (num < 0 && <message>5__3 != null)
						{
							synchronizedCommunicationsAdapter.VerboseDebugBase("WriteTask", "return message to pool on cleanup");
							ArrayPool<byte>.Shared.Return(<message>5__3, false);
						}
					}
					synchronizedCommunicationsAdapter.VerboseDebugBase("WriteTask", "END");
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<emptyMessage>5__2 = null;
					<message>5__3 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<emptyMessage>5__2 = null;
				<message>5__3 = null;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		private const string LogTagBase = "SynchronizedCommunicationsAdapter";

		private static int _instanceCount;

		protected int _instance;

		public const int ReadBufferSize = 4096;

		private const int ReadMessageTimeoutMs = 3000;

		private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

		private readonly WriteBuffer _writeBuffer;

		private CancellationTokenSource? _cts;

		private CancellationToken _ct;

		private readonly ConcurrentQueue<byte[]> _messageQueue = new ConcurrentQueue<byte[]>();

		protected Watchdog? _readMessageWatchdog;

		private global::System.Threading.Tasks.Task? _writeTask;

		private global::System.Threading.Tasks.Task? _readTask;

		[field: CompilerGenerated]
		public bool Connected
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public long RxMsgCnt => ((Adapter)this).RxMetrics.TotalMessages;

		public int RxMsgsPerSec => ((Adapter)this).RxMetrics.MessagesPerSecond;

		public int LastMsgRxMs
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				TimeSpan timeSinceLastMessage = ((Adapter)this).RxMetrics.TimeSinceLastMessage;
				return (int)((TimeSpan)(ref timeSinceLastMessage)).TotalMilliseconds;
			}
		}

		public long TxMsgCnt => ((Adapter)this).TxMetrics.TotalMessages;

		public int TxMsgsPerSec => ((Adapter)this).TxMetrics.MessagesPerSecond;

		public int LastMsgTxMs
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				TimeSpan timeSinceLastMessage = ((Adapter)this).TxMetrics.TimeSinceLastMessage;
				return (int)((TimeSpan)(ref timeSinceLastMessage)).TotalMilliseconds;
			}
		}

		protected abstract string LogTag { get; }

		[field: CompilerGenerated]
		public override IPhysicalAddress MAC
		{
			[CompilerGenerated]
			get;
		}

		protected virtual WriteBuffer RegisterWriteBuffer()
		{
			return new UnbufferedWriter(SynchronizedWriteAsync);
		}

		protected abstract global::System.Threading.Tasks.Task<bool> SynchronizedConnectAsync(CancellationToken linkedCt);

		protected abstract global::System.Threading.Tasks.Task SynchronizedDisconnectAsync();

		protected abstract global::System.Threading.Tasks.Task<int> SynchronizedReadAsync(byte[] buffer, CancellationToken cancellationToken);

		protected abstract global::System.Threading.Tasks.Task SynchronizedWriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

		protected SynchronizedCommunicationsAdapter(PhysicalAddress mac)
			: this(mac, ((object)mac).ToString(), null, null, verbose: false)
		{
		}

		protected SynchronizedCommunicationsAdapter(PhysicalAddress mac, string name)
			: this(mac, name, null, null, verbose: false)
		{
		}

		protected SynchronizedCommunicationsAdapter(PhysicalAddress mac, string name, Encoder<MessageBuffer> encoder)
			: this(mac, name, encoder, null, verbose: false)
		{
		}

		protected SynchronizedCommunicationsAdapter(PhysicalAddress mac, string name, Decoder<MessageBuffer> decoder)
			: this(mac, name, null, decoder, verbose: false)
		{
		}

		protected SynchronizedCommunicationsAdapter(PhysicalAddress mac, string name, bool verbose)
			: this(mac, name, null, null, verbose)
		{
		}

		protected SynchronizedCommunicationsAdapter(PhysicalAddress mac, string name, Encoder<MessageBuffer> encoder, bool verbose)
			: this(mac, name, encoder, null, verbose)
		{
		}

		protected SynchronizedCommunicationsAdapter(PhysicalAddress mac, string name, Decoder<MessageBuffer> decoder, bool verbose)
			: this(mac, name, null, decoder, verbose)
		{
		}

		protected SynchronizedCommunicationsAdapter(PhysicalAddress mac, string name, Encoder<MessageBuffer> encoder, Decoder<MessageBuffer> decoder)
			: this(mac, name, encoder, null, verbose: false)
		{
		}

		protected SynchronizedCommunicationsAdapter(PhysicalAddress mac, string name, Encoder<MessageBuffer>? encoder, Decoder<MessageBuffer>? decoder, bool verbose)
			: base(name, (IMessageEncoder<MessageBuffer>)(object)encoder, (MessageDecoder<MessageBuffer>)(object)decoder, verbose)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			_instance = Interlocked.Increment(ref _instanceCount);
			MAC = (IPhysicalAddress)(object)mac;
			_writeBuffer = GetWriteBufferImplimentation();
		}

		private WriteBuffer GetWriteBufferImplimentation()
		{
			return RegisterWriteBuffer();
		}

		[AsyncStateMachine(typeof(<ConnectAsync>d__49))]
		protected override async global::System.Threading.Tasks.Task<bool> ConnectAsync(AsyncOperation obj)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			CancellationToken ct = ((obj != null) ? obj.CancellationToken : CancellationToken.None);
			return await ConnectAsync(ct);
		}

		[AsyncStateMachine(typeof(<DisconnectAsync>d__50))]
		protected override async global::System.Threading.Tasks.Task<bool> DisconnectAsync(AsyncOperation obj)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await DisconnectAsync((obj != null) ? obj.CancellationToken : CancellationToken.None).ConfigureAwait(false);
		}

		protected override bool TransmitRaw(MessageBuffer messageBuffer)
		{
			if (!Connected)
			{
				return false;
			}
			if (messageBuffer == null || messageBuffer.Length < 1)
			{
				return false;
			}
			int length = messageBuffer.Length;
			byte[] array = ArrayPool<byte>.Shared.Rent(length + 4);
			Buffer.BlockCopy((global::System.Array)messageBuffer.Data, 0, (global::System.Array)array, 4, length);
			array[0] = (byte)length;
			array[1] = (byte)(length >> 8);
			array[2] = (byte)(length >> 16);
			array[3] = (byte)(length >> 24);
			bool flag = false;
			try
			{
				_messageQueue.Enqueue(array);
				flag = true;
			}
			catch
			{
				WarningBase("TransmitRaw", "Could not post message to _writeBuffer");
			}
			finally
			{
				if (!flag)
				{
					ArrayPool<byte>.Shared.Return(array, false);
				}
			}
			return flag;
		}

		[AsyncStateMachine(typeof(<ConnectAsync>d__52))]
		private async global::System.Threading.Tasks.Task<bool> ConnectAsync(CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			VerboseDebugBase("ConnectAsync", "BEGIN");
			bool locked = false;
			object obj = null;
			int num = 0;
			bool result = default(bool);
			try
			{
				_ = 1;
				try
				{
					if (Connected)
					{
						DebugBase("ConnectAsync", "already connected");
						result = true;
					}
					else
					{
						VerboseDebugBase("ConnectAsync", "Acquiring lock");
						await _lock.WaitAsync(ct).ConfigureAwait(false);
						locked = true;
						VerboseDebugBase("ConnectAsync", "Lock acquired");
						if (Connected)
						{
							DebugBase("ConnectAsync", "already connected");
							result = true;
						}
						else
						{
							ForceReturnPooledMessages();
							Connected = await SynchronizedConnectAsync(ct).ConfigureAwait(false);
							if (!Connected)
							{
								result = false;
							}
							else
							{
								CancellationTokenSource? cts = _cts;
								if (cts != null)
								{
									CancellationTokenSourceExtension.TryCancelAndDispose(cts);
								}
								_cts = new CancellationTokenSource();
								_ct = _cts.Token;
								_readMessageWatchdog = new Watchdog(3000, (Action)([CompilerGenerated] () =>
								{
									//IL_0011: Unknown result type (might be due to invalid IL or missing references)
									ErrorBase("ReadTask", "time out");
									DisconnectAsync(CancellationToken.None);
								}), false);
								_writeTask = global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([CompilerGenerated] () => WriteTask(_ct)), _ct);
								_readTask = global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([CompilerGenerated] () => ReadTask(_ct)), _ct);
								TryRaiseAdapterOpened();
								DebugBase("ConnectAsync", "adapter opened raised");
								result = true;
							}
						}
					}
				}
				catch (OperationCanceledException)
				{
					if (((CancellationToken)(ref ct)).IsCancellationRequested)
					{
						InformationBase("ConnectAsync", "canceled");
					}
					else
					{
						ErrorBase("ConnectAsync", "time out");
					}
					goto end_IL_003f;
				}
				catch (global::System.Exception ex2)
				{
					ErrorBase("ConnectAsync", $"{ex2.GetType()} - {ex2.Message}\n{ex2.StackTrace}");
					goto end_IL_003f;
				}
				num = 1;
				end_IL_003f:;
			}
			catch (object obj2)
			{
				obj = obj2;
			}
			if (locked)
			{
				if (!Connected)
				{
					WarningBase("ConnectAsync", "connection not initialized, need to clean up");
					await InternalDisconnectAsync(raiseAdapterClosed: false).ConfigureAwait(false);
				}
				VerboseDebugBase("ConnectAsync", "Releasing lock");
				_lock.Release();
				VerboseDebugBase("ConnectAsync", "Lock released");
			}
			VerboseDebugBase("ConnectAsync", "END");
			object obj3 = obj;
			if (obj3 != null)
			{
				ExceptionDispatchInfo.Capture((obj3 as global::System.Exception) ?? throw obj3).Throw();
			}
			if (num == 1)
			{
				return result;
			}
			return false;
		}

		[AsyncStateMachine(typeof(<DisconnectAsync>d__53))]
		protected async global::System.Threading.Tasks.Task<bool> DisconnectAsync(CancellationToken ct, bool raiseAdapterClosed = true)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			VerboseDebugBase("DisconnectAsync", "BEGIN");
			bool locked = false;
			try
			{
				if (!Connected)
				{
					DebugBase("DisconnectAsync", "already disconnected");
					return true;
				}
				VerboseDebugBase("DisconnectAsync", "Acquiring lock");
				await _lock.WaitAsync(ct).ConfigureAwait(false);
				locked = true;
				VerboseDebugBase("DisconnectAsync", "Lock acquired");
				if (!Connected)
				{
					DebugBase("DisconnectAsync", "already disconnected");
					return true;
				}
				await InternalDisconnectAsync(raiseAdapterClosed).ConfigureAwait(false);
			}
			catch (global::System.Exception ex)
			{
				if (((CancellationToken)(ref _ct)).IsCancellationRequested)
				{
					InformationBase("DisconnectAsync", "canceled");
				}
				else
				{
					ErrorBase("DisconnectAsync", $"{ex.GetType()} - {ex.Message}\n{ex.StackTrace}");
				}
			}
			finally
			{
				if (locked)
				{
					VerboseDebugBase("DisconnectAsync", "Releasing lock");
					_lock.Release();
					VerboseDebugBase("DisconnectAsync", "Lock released");
				}
			}
			VerboseDebugBase("DisconnectAsync", "END");
			return locked;
		}

		[AsyncStateMachine(typeof(<InternalDisconnectAsync>d__54))]
		private global::System.Threading.Tasks.Task InternalDisconnectAsync(bool raiseAdapterClosed)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			<InternalDisconnectAsync>d__54 <InternalDisconnectAsync>d__ = default(<InternalDisconnectAsync>d__54);
			<InternalDisconnectAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InternalDisconnectAsync>d__.<>4__this = this;
			<InternalDisconnectAsync>d__.raiseAdapterClosed = raiseAdapterClosed;
			<InternalDisconnectAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <InternalDisconnectAsync>d__.<>t__builder)).Start<<InternalDisconnectAsync>d__54>(ref <InternalDisconnectAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <InternalDisconnectAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<WriteTask>d__55))]
		private global::System.Threading.Tasks.Task WriteTask(CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<WriteTask>d__55 <WriteTask>d__ = default(<WriteTask>d__55);
			<WriteTask>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteTask>d__.<>4__this = this;
			<WriteTask>d__.ct = ct;
			<WriteTask>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <WriteTask>d__.<>t__builder)).Start<<WriteTask>d__55>(ref <WriteTask>d__);
			return ((AsyncTaskMethodBuilder)(ref <WriteTask>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<ReadTask>d__56))]
		private global::System.Threading.Tasks.Task ReadTask(CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<ReadTask>d__56 <ReadTask>d__ = default(<ReadTask>d__56);
			<ReadTask>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ReadTask>d__.<>4__this = this;
			<ReadTask>d__.ct = ct;
			<ReadTask>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <ReadTask>d__.<>t__builder)).Start<<ReadTask>d__56>(ref <ReadTask>d__);
			return ((AsyncTaskMethodBuilder)(ref <ReadTask>d__.<>t__builder)).Task;
		}

		private void TryRaiseAdapterOpened()
		{
			try
			{
				((Adapter)this).RaiseAdapterOpened();
			}
			catch (global::System.Exception ex)
			{
				ErrorBase("TryRaiseAdapterOpened", $"{ex.GetType()} - {ex.Message}\n{ex.StackTrace}");
			}
		}

		private void TryRaiseAdapterClosed()
		{
			try
			{
				VerboseDebugBase("TryRaiseAdapterClosed", $"{"IsDisposed"}={((Disposable)this).IsDisposed}, {"IsConnected"}={((Adapter)this).IsConnected}");
				((Adapter)this).RaiseAdapterClosed();
			}
			catch (global::System.Exception ex)
			{
				ErrorBase("TryRaiseAdapterClosed", $"{ex.GetType()} - {ex.Message}\n{ex.StackTrace}");
			}
		}

		private void TryRaiseMessageRx(MessageBuffer msgBuf, bool echo)
		{
			try
			{
				base.RaiseMessageRx(msgBuf, echo);
			}
			catch (global::System.Exception ex)
			{
				ErrorBase("TryRaiseMessageRx", $"{ex.GetType()} - {ex.Message}\n{ex.StackTrace}");
			}
		}

		private void ForceReturnPooledMessages()
		{
			int count = _messageQueue.Count;
			VerboseDebugBase("ForceReturnPooledMessages", $"Clearing {count} queued messages");
			byte[] array = default(byte[]);
			while (_messageQueue.Count > 0)
			{
				if (_messageQueue.TryDequeue(ref array))
				{
					ArrayPool<byte>.Shared.Return(array, false);
				}
			}
			VerboseDebugBase("ForceReturnPooledMessages", $"{count - _messageQueue.Count} messages dequeued");
		}

		private void VerboseDebugBase(string source, string message)
		{
			VerboseDebug(LogTag + ".SynchronizedCommunicationsAdapter", source, message);
		}

		private void DebugBase(string source, string message)
		{
			Debug(LogTag + ".SynchronizedCommunicationsAdapter", source, message);
		}

		private void InformationBase(string source, string message)
		{
			Information(LogTag + ".SynchronizedCommunicationsAdapter", source, message);
		}

		private void WarningBase(string source, string message)
		{
			Warning(LogTag + ".SynchronizedCommunicationsAdapter", source, message);
		}

		private void ErrorBase(string source, string message)
		{
			Error(LogTag + ".SynchronizedCommunicationsAdapter", source, message);
		}

		protected void VerboseDebug(string source, string message)
		{
			VerboseDebug(LogTag, source, message);
		}

		protected void Debug(string source, string message)
		{
			Debug(LogTag, source, message);
		}

		protected void Information(string source, string message)
		{
			Information(LogTag, source, message);
		}

		protected void Warning(string source, string message)
		{
			Warning(LogTag, source, message);
		}

		protected void Error(string source, string message)
		{
			Error(LogTag, source, message);
		}

		private void VerboseDebug(string tag, string source, string message)
		{
			if (((Adapter)this).Verbose)
			{
				Debug(tag, source, message);
			}
		}

		private void Debug(string tag, string source, string message)
		{
			TaggedLog.Debug(tag, $"[{_instance}] {source}: {message}", global::System.Array.Empty<object>());
		}

		private void Information(string tag, string source, string message)
		{
			TaggedLog.Information(tag, $"[{_instance}] {source}: {message}", global::System.Array.Empty<object>());
		}

		private void Warning(string tag, string source, string message)
		{
			TaggedLog.Warning(tag, $"[{_instance}] {source}: {message}", global::System.Array.Empty<object>());
		}

		private void Error(string tag, string source, string message)
		{
			TaggedLog.Error(tag, $"[{_instance}] {source}: {message}", global::System.Array.Empty<object>());
		}
	}
	public class TcpCommunicationsAdapter : SynchronizedCommunicationsAdapter
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <SynchronizedConnectAsync>d__14 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public TcpCommunicationsAdapter <>4__this;

			public CancellationToken linkedCt;

			private CancellationTokenSource <timeoutCts>5__2;

			private CancellationTokenSource <timeoutLinkedCts>5__3;

			private bool <isConnected>5__4;

			private ConfiguredTaskAwaiter <>u__1;

			private TaskAwaiter <>u__2;

			private void MoveNext()
			{
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Expected O, but got Unknown
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_015f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0164: Unknown result type (might be due to invalid IL or missing references)
				//IL_016c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_012a: Unknown result type (might be due to invalid IL or missing references)
				//IL_012f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0144: Unknown result type (might be due to invalid IL or missing references)
				//IL_0146: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				TcpCommunicationsAdapter tcpCommunicationsAdapter = <>4__this;
				bool result;
				try
				{
					_ = 1;
					try
					{
						if ((uint)num > 1u)
						{
							tcpCommunicationsAdapter._socket = SocketFactory.GetSocket();
							tcpCommunicationsAdapter._socket.WriteBufferSize = 1024;
							tcpCommunicationsAdapter._socket.ReadBufferSize = 4096;
							tcpCommunicationsAdapter._socket.NoDelay = true;
							<timeoutCts>5__2 = new CancellationTokenSource(10000);
						}
						try
						{
							if ((uint)num > 1u)
							{
								<timeoutLinkedCts>5__3 = CancellationTokenSource.CreateLinkedTokenSource(linkedCt, <timeoutCts>5__2.Token);
							}
							try
							{
								TaskAwaiter val;
								ConfiguredTaskAwaiter val3;
								if (num != 0)
								{
									if (num == 1)
									{
										val = <>u__2;
										<>u__2 = default(TaskAwaiter);
										num = (<>1__state = -1);
										goto IL_017b;
									}
									ConfiguredTaskAwaitable val2 = tcpCommunicationsAdapter._socket.ConnectAsync(tcpCommunicationsAdapter._address, tcpCommunicationsAdapter._port, <timeoutLinkedCts>5__3.Token).ConfigureAwait(false);
									val3 = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
									if (!((ConfiguredTaskAwaiter)(ref val3)).IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val3;
										<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <SynchronizedConnectAsync>d__14>(ref val3, ref this);
										return;
									}
								}
								else
								{
									val3 = <>u__1;
									<>u__1 = default(ConfiguredTaskAwaiter);
									num = (<>1__state = -1);
								}
								((ConfiguredTaskAwaiter)(ref val3)).GetResult();
								<isConnected>5__4 = tcpCommunicationsAdapter._socket.IsConnected;
								if (!<isConnected>5__4)
								{
									val = tcpCommunicationsAdapter.SynchronizedDisconnectAsync().GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 1);
										<>u__2 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <SynchronizedConnectAsync>d__14>(ref val, ref this);
										return;
									}
									goto IL_017b;
								}
								goto IL_0182;
								IL_017b:
								((TaskAwaiter)(ref val)).GetResult();
								goto IL_0182;
								IL_0182:
								result = <isConnected>5__4;
							}
							finally
							{
								if (num < 0 && <timeoutLinkedCts>5__3 != null)
								{
									((global::System.IDisposable)<timeoutLinkedCts>5__3).Dispose();
								}
							}
						}
						finally
						{
							if (num < 0 && <timeoutCts>5__2 != null)
							{
								((global::System.IDisposable)<timeoutCts>5__2).Dispose();
							}
						}
					}
					catch (global::System.Exception ex)
					{
						IDisposableExtensions.TryDispose((global::System.IDisposable)tcpCommunicationsAdapter._socket);
						tcpCommunicationsAdapter._socket = null;
						TaggedLog.Warning(tcpCommunicationsAdapter.LogTag, "Connection failed " + ex.Message, global::System.Array.Empty<object>());
						throw;
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
		private struct <SynchronizedWriteAsync>d__17 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public TcpCommunicationsAdapter <>4__this;

			public byte[] buffer;

			public int offset;

			public int count;

			public CancellationToken cancellationToken;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0095: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00be: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_0106: Unknown result type (might be due to invalid IL or missing references)
				//IL_010d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				TcpCommunicationsAdapter tcpCommunicationsAdapter = <>4__this;
				try
				{
					ConfiguredTaskAwaiter val;
					ConfiguredTaskAwaitable val2;
					if (num != 0)
					{
						if (num == 1)
						{
							val = <>u__1;
							<>u__1 = default(ConfiguredTaskAwaiter);
							num = (<>1__state = -1);
							goto IL_011c;
						}
						if (tcpCommunicationsAdapter._socket == null || !tcpCommunicationsAdapter._socket.IsConnected)
						{
							throw ConnectionAborted;
						}
						val2 = tcpCommunicationsAdapter._socket.WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
						val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <SynchronizedWriteAsync>d__17>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(ConfiguredTaskAwaiter);
						num = (<>1__state = -1);
					}
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
					val2 = tcpCommunicationsAdapter._socket.FlushAsync(cancellationToken).ConfigureAwait(false);
					val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
					if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 1);
						<>u__1 = val;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <SynchronizedWriteAsync>d__17>(ref val, ref this);
						return;
					}
					goto IL_011c;
					IL_011c:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
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

		private static readonly SocketException ConnectionAborted = new SocketException(10053);

		private const int WriteBufferSize = 1024;

		private const int SynchronizedConnectMaxTimeoutSecondsMs = 10000;

		private readonly IPAddress _address;

		private Encoder<MessageBuffer>? _encoder;

		private Decoder<MessageBuffer>? _decoder;

		private readonly int _port;

		private ISocket? _socket;

		[field: CompilerGenerated]
		protected override string LogTag
		{
			[CompilerGenerated]
			get;
		} = "TcpCommunicationsAdapter";

		public TcpCommunicationsAdapter(IPAddress address, int port, PhysicalAddress mac, bool verbose)
			: this(address, port, mac, $"{"TcpCommunicationsAdapter"} - {address}:{port}", new Encoder<MessageBuffer>(), new Decoder<MessageBuffer>(), verbose)
		{
		}

		protected TcpCommunicationsAdapter(IPAddress address, int port, PhysicalAddress mac, string name, Encoder<MessageBuffer> encoder, Decoder<MessageBuffer> decoder, bool verbose)
			: base(mac, $"{"TcpCommunicationsAdapter"} - {address}:{port}", encoder, decoder, verbose)
		{
			_address = address;
			_port = port;
			_encoder = encoder;
			_decoder = decoder;
		}

		protected override WriteBuffer RegisterWriteBuffer()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			return new TemporalAndSizeBoundWriteBuffer(SynchronizedWriteAsync, TimeSpan.FromMilliseconds(10L, 0L), TimeSpan.FromMilliseconds(25L, 0L), 128u, ((Adapter)this).Verbose);
		}

		[AsyncStateMachine(typeof(<SynchronizedConnectAsync>d__14))]
		protected override async global::System.Threading.Tasks.Task<bool> SynchronizedConnectAsync(CancellationToken linkedCt)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			_ = 1;
			try
			{
				_socket = SocketFactory.GetSocket();
				_socket.WriteBufferSize = 1024;
				_socket.ReadBufferSize = 4096;
				_socket.NoDelay = true;
				CancellationTokenSource timeoutCts = new CancellationTokenSource(10000);
				try
				{
					CancellationTokenSource timeoutLinkedCts = CancellationTokenSource.CreateLinkedTokenSource(linkedCt, timeoutCts.Token);
					try
					{
						await _socket.ConnectAsync(_address, _port, timeoutLinkedCts.Token).ConfigureAwait(false);
						bool isConnected = _socket.IsConnected;
						if (!isConnected)
						{
							await SynchronizedDisconnectAsync();
						}
						return isConnected;
					}
					finally
					{
						((global::System.IDisposable)timeoutLinkedCts)?.Dispose();
					}
				}
				finally
				{
					((global::System.IDisposable)timeoutCts)?.Dispose();
				}
			}
			catch (global::System.Exception ex)
			{
				IDisposableExtensions.TryDispose((global::System.IDisposable)_socket);
				_socket = null;
				TaggedLog.Warning(LogTag, "Connection failed " + ex.Message, global::System.Array.Empty<object>());
				throw;
			}
		}

		protected override global::System.Threading.Tasks.Task SynchronizedDisconnectAsync()
		{
			ISocket? socket = _socket;
			if (socket != null)
			{
				socket.Disconnect();
			}
			ISocket? socket2 = _socket;
			if (socket2 != null)
			{
				IDisposableExtensions.TryDispose((global::System.IDisposable)socket2);
			}
			_socket = null;
			return global::System.Threading.Tasks.Task.FromResult<bool>(true);
		}

		[MethodImpl((MethodImplOptions)256)]
		protected override global::System.Threading.Tasks.Task<int> SynchronizedReadAsync(byte[] buffer, CancellationToken cancellationToken)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (_socket == null || !_socket.IsConnected)
			{
				throw ConnectionAborted;
			}
			return _socket.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
		}

		[MethodImpl((MethodImplOptions)256)]
		[AsyncStateMachine(typeof(<SynchronizedWriteAsync>d__17))]
		protected override global::System.Threading.Tasks.Task SynchronizedWriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			<SynchronizedWriteAsync>d__17 <SynchronizedWriteAsync>d__ = default(<SynchronizedWriteAsync>d__17);
			<SynchronizedWriteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SynchronizedWriteAsync>d__.<>4__this = this;
			<SynchronizedWriteAsync>d__.buffer = buffer;
			<SynchronizedWriteAsync>d__.offset = offset;
			<SynchronizedWriteAsync>d__.count = count;
			<SynchronizedWriteAsync>d__.cancellationToken = cancellationToken;
			<SynchronizedWriteAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <SynchronizedWriteAsync>d__.<>t__builder)).Start<<SynchronizedWriteAsync>d__17>(ref <SynchronizedWriteAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <SynchronizedWriteAsync>d__.<>t__builder)).Task;
		}

		public override void Dispose(bool disposing)
		{
			ISocket? socket = _socket;
			if (socket != null)
			{
				socket.Disconnect();
			}
			ISocket? socket2 = _socket;
			if (socket2 != null)
			{
				IDisposableExtensions.TryDispose((global::System.IDisposable)socket2);
			}
			_socket = null;
			Encoder<MessageBuffer>? encoder = _encoder;
			if (encoder != null)
			{
				IDisposableExtensions.TryDispose((global::System.IDisposable)encoder);
			}
			_encoder = null;
			Decoder<MessageBuffer>? decoder = _decoder;
			if (decoder != null)
			{
				IDisposableExtensions.TryDispose((global::System.IDisposable)decoder);
			}
			_decoder = null;
			((Adapter)this).Dispose(disposing);
		}
	}
}
[StructLayout((LayoutKind)3)]
[InlineArray(2)]
internal struct <>y__InlineArray2<T>
{
}
