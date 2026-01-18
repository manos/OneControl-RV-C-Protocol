using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Java.IO;
using Java.Interop;
using Java.Lang;
using Java.Net;
using Java.Nio.Channels;
using Java.Util;
using Javax.Net;
using Microsoft.Maui.ApplicationModel;
using _Microsoft.Android.Resource.Designer;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: TargetFramework(".NETCoreApp,Version=v8.0", FrameworkDisplayName = ".NET 8.0")]
[assembly: AssemblyCompany("IDS.Net")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("2.2.0.0")]
[assembly: AssemblyInformationalVersion("2.2.0+e2e7c194a13ac65ed2e986006517b428941edde4")]
[assembly: AssemblyProduct("IDS.Net")]
[assembly: AssemblyTitle("IDS.Net")]
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/lci-ids/ids.net.wifi")]
[assembly: TargetPlatform("Android34.0")]
[assembly: SupportedOSPlatform("Android21.0")]
[assembly: AssemblyVersion("2.2.0.0")]
[module: RefSafetyRules(11)]
namespace IDS.Net
{
	internal static class AndroidIPAddressExtensions
	{
		internal static List<IPAddress> GetBroadcastAddresses()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			List<IPAddress> val = new List<IPAddress>();
			IEnumeration networkInterfaces = NetworkInterface.NetworkInterfaces;
			while (networkInterfaces.HasMoreElements)
			{
				global::System.Collections.Generic.IEnumerator<InterfaceAddress> enumerator = ((global::System.Collections.Generic.IEnumerable<InterfaceAddress>)((NetworkInterface)networkInterfaces.NextElement()).InterfaceAddresses).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
					{
						InterfaceAddress current = enumerator.Current;
						InetAddress address = current.Address;
						Inet4Address val2 = (Inet4Address)(object)((address is Inet4Address) ? address : null);
						if (val2 != null)
						{
							IPAddress broadcastAddress = IPAddress.Parse(((InetAddress)val2).HostAddress).GetBroadcastAddress((int)current.NetworkPrefixLength);
							val.Add(broadcastAddress);
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator)?.Dispose();
				}
			}
			return val;
		}

		internal static bool TryGetNicId(IPAddress address, out string? nicId)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			nicId = null;
			IEnumeration networkInterfaces = NetworkInterface.NetworkInterfaces;
			while (networkInterfaces.HasMoreElements)
			{
				NetworkInterface val = (NetworkInterface)networkInterfaces.NextElement();
				global::System.Collections.Generic.IEnumerator<InterfaceAddress> enumerator = ((global::System.Collections.Generic.IEnumerable<InterfaceAddress>)val.InterfaceAddresses).GetEnumerator();
				try
				{
					while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
					{
						InterfaceAddress current = enumerator.Current;
						if (IPAddress.Parse(current.Address.HostAddress).IsInSameSubnet(address, (int)current.NetworkPrefixLength))
						{
							nicId = val.Name;
							return true;
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator)?.Dispose();
				}
			}
			return false;
		}
	}
	public static class IPAddressExtensions
	{
		public static IPAddress GetBroadcastAddress(this IPAddress instance, IPAddress subnetMask)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			uint num = BitConverter.ToUInt32(instance.GetAddressBytes(), 0);
			uint num2 = BitConverter.ToUInt32(subnetMask.GetAddressBytes(), 0);
			return new IPAddress(BitConverter.GetBytes(num | ~num2));
		}

		public static IPAddress GetNetworkAddress(this IPAddress instance, IPAddress subnetMask)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			uint num = BitConverter.ToUInt32(instance.GetAddressBytes(), 0);
			uint num2 = BitConverter.ToUInt32(subnetMask.GetAddressBytes(), 0);
			return new IPAddress(BitConverter.GetBytes(num & num2));
		}

		public static bool IsInSameSubnet(this IPAddress instance, IPAddress other, IPAddress subnetMask)
		{
			IPAddress networkAddress = instance.GetNetworkAddress(subnetMask);
			IPAddress networkAddress2 = other.GetNetworkAddress(subnetMask);
			return ((object)networkAddress).Equals((object)networkAddress2);
		}

		public static IPAddress GetBroadcastAddress(this IPAddress instance, int networkPrefixLength)
		{
			IPAddress subnetMask = GetSubnetMask(networkPrefixLength);
			return instance.GetBroadcastAddress(subnetMask);
		}

		public static IPAddress GetNetworkAddress(this IPAddress instance, int networkPrefixLength)
		{
			IPAddress subnetMask = GetSubnetMask(networkPrefixLength);
			return instance.GetNetworkAddress(subnetMask);
		}

		public static bool IsInSameSubnet(this IPAddress instance, IPAddress other, int networkPrefixLength)
		{
			IPAddress subnetMask = GetSubnetMask(networkPrefixLength);
			return instance.IsInSameSubnet(other, subnetMask);
		}

		private static IPAddress GetSubnetMask(int networkPrefix)
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			int num = -1 << 32 - networkPrefix;
			byte b = (byte)(((num & -16777216) >>> 24) & 0xFF);
			byte b2 = (byte)(((num & 0xFF0000) >>> 16) & 0xFF);
			byte b3 = (byte)(((num & 0xFF00) >>> 8) & 0xFF);
			byte b4 = (byte)(num & 0xFF & 0xFF);
			return new IPAddress(new byte[4] { b, b2, b3, b4 });
		}

		public static List<IPAddress> GetBroadcastAddresses()
		{
			return AndroidIPAddressExtensions.GetBroadcastAddresses();
		}

		public static bool TryGetNicId(IPAddress address, out string? nicId)
		{
			return AndroidIPAddressExtensions.TryGetNicId(address, out nicId);
		}
	}
	public class Resource : Resource
	{
	}
}
namespace IDS.Net.Sockets
{
	internal sealed class AndroidDatagramSocket : DatagramSocket, ISocket, global::System.IDisposable
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ConnectAsync>d__30 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public CancellationToken cancellationToken;

			public IPAddress address;

			public int port;

			public AndroidDatagramSocket <>4__this;

			private CancellationTokenSource <cts>5__2;

			private CancellationTokenRegistration <>7__wrap2;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Expected O, but got Unknown
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Expected O, but got Unknown
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				AndroidDatagramSocket androidDatagramSocket = <>4__this;
				try
				{
					if (num != 0)
					{
						<cts>5__2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
					}
					try
					{
						InetSocketAddress val = default(InetSocketAddress);
						if (num != 0)
						{
							CancellationToken token = <cts>5__2.Token;
							val = new InetSocketAddress(((object)address).ToString(), port);
							<>7__wrap2 = ((CancellationToken)(ref token)).Register(new Action(((DatagramSocket)androidDatagramSocket).Close));
						}
						try
						{
							ConfiguredTaskAwaiter val3;
							if (num != 0)
							{
								ConfiguredTaskAwaitable val2 = ((DatagramSocket)androidDatagramSocket).ConnectAsync((SocketAddress)(object)val).ConfigureAwait(false);
								val3 = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
								if (!((ConfiguredTaskAwaiter)(ref val3)).IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val3;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <ConnectAsync>d__30>(ref val3, ref this);
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
						}
						finally
						{
							if (num < 0)
							{
								((global::System.IDisposable)global::System.Runtime.CompilerServices.Unsafe.As<CancellationTokenRegistration, CancellationTokenRegistration>(ref <>7__wrap2)/*cast due to .constrained prefix*/).Dispose();
							}
						}
						<>7__wrap2 = default(CancellationTokenRegistration);
						if (!((DatagramSocket)androidDatagramSocket).IsConnected && androidDatagramSocket._connectException != null)
						{
							throw androidDatagramSocket._connectException;
						}
					}
					finally
					{
						if (num < 0 && <cts>5__2 != null)
						{
							((global::System.IDisposable)<cts>5__2).Dispose();
						}
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<cts>5__2 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<cts>5__2 = null;
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
		private struct <FlushAsync>d__33 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			private void MoveNext()
			{
				try
				{
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
		private struct <ReadAsync>d__31 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<int> <>t__builder;

			public byte[] buffer;

			public int offset;

			public int count;

			public AndroidDatagramSocket <>4__this;

			private DatagramPacket <packet>5__2;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Expected O, but got Unknown
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				AndroidDatagramSocket androidDatagramSocket = <>4__this;
				int length;
				try
				{
					ConfiguredTaskAwaiter val2;
					if (num != 0)
					{
						<packet>5__2 = new DatagramPacket(buffer, offset, count);
						ConfiguredTaskAwaitable val = ((DatagramSocket)androidDatagramSocket).ReceiveAsync(<packet>5__2).ConfigureAwait(false);
						val2 = ((ConfiguredTaskAwaitable)(ref val)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val2;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <ReadAsync>d__31>(ref val2, ref this);
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
					length = <packet>5__2.Length;
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<packet>5__2 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<packet>5__2 = null;
				<>t__builder.SetResult(length);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <WriteAsync>d__32 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public AndroidDatagramSocket <>4__this;

			public byte[] buffer;

			public int offset;

			public int count;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Expected O, but got Unknown
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				AndroidDatagramSocket androidDatagramSocket = <>4__this;
				try
				{
					ConfiguredTaskAwaiter val2;
					if (num != 0)
					{
						ConfiguredTaskAwaitable val = ((DatagramSocket)androidDatagramSocket).SendAsync(new DatagramPacket(buffer, offset, count)).ConfigureAwait(false);
						val2 = ((ConfiguredTaskAwaitable)(ref val)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <WriteAsync>d__32>(ref val2, ref this);
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

		private global::System.Exception? _connectException;

		private const int UdpReceiveBufferDefaultSize = 131072;

		private const int SIO_UDP_CONNRESET = -1744830452;

		private static readonly byte[] EmptyOptionInValue = new byte[4];

		[field: CompilerGenerated]
		public bool NoDelay
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		} = true;

		public int ReadBufferSize
		{
			get
			{
				return ((DatagramSocket)this).ReceiveBufferSize;
			}
			set
			{
				((DatagramSocket)this).ReceiveBufferSize = value;
			}
		}

		public int WriteBufferSize
		{
			get
			{
				return ((DatagramSocket)this).SendBufferSize;
			}
			set
			{
				((DatagramSocket)this).SendBufferSize = value;
			}
		}

		[field: CompilerGenerated]
		public LingerOptions LingerState
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		} = new LingerOptions(enabled: true, 0);

		public EndPoint LocalEndPoint => (EndPoint)new IPEndPoint(new IPAddress(((DatagramSocket)this).LocalAddress.GetAddress()), ((DatagramSocket)this).LocalPort);

		public EndPoint RemoteEndPoint => (EndPoint)new IPEndPoint(new IPAddress(((DatagramSocket)this).InetAddress.GetAddress()), ((DatagramSocket)this).Port);

		[Export(Throws = new global::System.Type[] { typeof(SocketException) })]
		public AndroidDatagramSocket()
		{
			AndroidSocketInit();
		}

		[Export(Throws = new global::System.Type[] { typeof(SocketException) })]
		public AndroidDatagramSocket(int port)
			: base(port)
		{
			AndroidSocketInit();
		}

		public AndroidDatagramSocket(DatagramSocketImpl datagramSocketImpl)
			: base(datagramSocketImpl)
		{
			AndroidSocketInit();
		}

		[Export(Throws = new global::System.Type[] { typeof(SocketException) })]
		public AndroidDatagramSocket(int port, InetAddress address)
			: base(port, address)
		{
			AndroidSocketInit();
		}

		[Export(Throws = new global::System.Type[] { typeof(SocketException) })]
		public AndroidDatagramSocket(SocketAddress address)
			: base(address)
		{
			AndroidSocketInit();
		}

		public AndroidDatagramSocket(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			AndroidSocketInit();
		}

		private void AndroidSocketInit()
		{
			((DatagramSocket)this).ReuseAddress = true;
			((DatagramSocket)this).ReceiveBufferSize = 131072;
		}

		[Export(Throws = new global::System.Type[]
		{
			typeof(SocketTimeoutException),
			typeof(NoRouteToHostException),
			typeof(IllegalBlockingModeException),
			typeof(IllegalArgumentException),
			typeof(IOException)
		})]
		public override void Connect(SocketAddress endpoint)
		{
			//IL_0011: Expected O, but got Unknown
			//IL_001b: Expected O, but got Unknown
			//IL_0025: Expected O, but got Unknown
			//IL_002f: Expected O, but got Unknown
			//IL_003a: Expected O, but got Unknown
			BindToNetwork(endpoint);
			try
			{
				((DatagramSocket)this).Connect(endpoint);
			}
			catch (SocketTimeoutException ex)
			{
				SocketTimeoutException connectException = ex;
				_connectException = (global::System.Exception?)(object)connectException;
			}
			catch (NoRouteToHostException ex2)
			{
				NoRouteToHostException connectException2 = ex2;
				_connectException = (global::System.Exception?)(object)connectException2;
			}
			catch (IllegalBlockingModeException ex3)
			{
				IllegalBlockingModeException connectException3 = ex3;
				_connectException = (global::System.Exception?)(object)connectException3;
			}
			catch (IllegalArgumentException ex4)
			{
				IllegalArgumentException connectException4 = ex4;
				_connectException = (global::System.Exception?)(object)connectException4;
			}
			catch (IOException ex5)
			{
				IOException connectException5 = ex5;
				_connectException = (global::System.Exception?)(object)connectException5;
			}
		}

		[AsyncStateMachine(typeof(<ConnectAsync>d__30))]
		public global::System.Threading.Tasks.Task ConnectAsync(IPAddress address, int port, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<ConnectAsync>d__30 <ConnectAsync>d__ = default(<ConnectAsync>d__30);
			<ConnectAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ConnectAsync>d__.<>4__this = this;
			<ConnectAsync>d__.address = address;
			<ConnectAsync>d__.port = port;
			<ConnectAsync>d__.cancellationToken = cancellationToken;
			<ConnectAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <ConnectAsync>d__.<>t__builder)).Start<<ConnectAsync>d__30>(ref <ConnectAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <ConnectAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<ReadAsync>d__31))]
		public async global::System.Threading.Tasks.Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			DatagramPacket packet = new DatagramPacket(buffer, offset, count);
			await ((DatagramSocket)this).ReceiveAsync(packet).ConfigureAwait(false);
			return packet.Length;
		}

		[AsyncStateMachine(typeof(<WriteAsync>d__32))]
		public global::System.Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			<WriteAsync>d__32 <WriteAsync>d__ = default(<WriteAsync>d__32);
			<WriteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteAsync>d__.<>4__this = this;
			<WriteAsync>d__.buffer = buffer;
			<WriteAsync>d__.offset = offset;
			<WriteAsync>d__.count = count;
			<WriteAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <WriteAsync>d__.<>t__builder)).Start<<WriteAsync>d__32>(ref <WriteAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <WriteAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<FlushAsync>d__33))]
		public global::System.Threading.Tasks.Task FlushAsync(CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			<FlushAsync>d__33 <FlushAsync>d__ = default(<FlushAsync>d__33);
			<FlushAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<FlushAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <FlushAsync>d__.<>t__builder)).Start<<FlushAsync>d__33>(ref <FlushAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <FlushAsync>d__.<>t__builder)).Task;
		}

		public void Connect(IPAddress address, int port)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			InetSocketAddress val = new InetSocketAddress(((object)address).ToString(), port);
			((DatagramSocket)this).Connect((SocketAddress)(object)val);
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Expected O, but got Unknown
			DatagramPacket val = new DatagramPacket(buffer, offset, count);
			((DatagramSocket)this).Receive(val);
			return val.Length;
		}

		public void Write(byte[] buffer, int offset, int count)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			((DatagramSocket)this).Send(new DatagramPacket(buffer, offset, count));
		}

		public void Flush()
		{
		}

		public void Disconnect()
		{
			((DatagramSocket)this).Close();
		}

		private void BindToNetwork(SocketAddress endpoint)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			if ((int)VERSION.SdkInt < 21 || !AndroidIPAddressExtensions.TryGetNicId(new IPAddress(((InetSocketAddress)endpoint).Address.GetAddress()), out string nicId))
			{
				return;
			}
			Object systemService = Platform.AppContext.GetSystemService("connectivity");
			ConnectivityManager val = (ConnectivityManager)(object)((systemService is ConnectivityManager) ? systemService : null);
			if (val == null)
			{
				return;
			}
			Network[] allNetworks = val.GetAllNetworks();
			if (allNetworks == null)
			{
				return;
			}
			Network[] array = allNetworks;
			foreach (Network val2 in array)
			{
				LinkProperties linkProperties = val.GetLinkProperties(val2);
				string text = ((linkProperties != null) ? linkProperties.InterfaceName : null);
				if (text != null && text.Equals(nicId))
				{
					val2.BindSocket((DatagramSocket)(object)this);
					break;
				}
			}
		}

		public void Bind(EndPoint endPoint)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			IPEndPoint val = (IPEndPoint)(object)((endPoint is IPEndPoint) ? endPoint : null);
			if (val == null)
			{
				throw new ArgumentException("Endpoint must be an ip end point");
			}
			InetSocketAddress val2 = new InetSocketAddress(InetAddress.GetByAddress(val.Address.GetAddressBytes()), val.Port);
			((DatagramSocket)this).Bind((SocketAddress)(object)val2);
		}

		public Stream CreateNetworkStream()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			throw new NotImplementedException("DatagramSocket does not own a stream");
		}
	}
	internal sealed class DatagramSocket : ISocket, global::System.IDisposable
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ConnectAsync>d__21 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DatagramSocket <>4__this;

			public CancellationToken cancellationToken;

			public IPAddress address;

			public int port;

			private CancellationTokenSource <cts>5__2;

			private CancellationTokenRegistration <>7__wrap2;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Expected O, but got Unknown
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_008f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
				//IL_010e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0118: Expected O, but got Unknown
				int num = <>1__state;
				DatagramSocket datagramSocket = <>4__this;
				try
				{
					if (num != 0)
					{
						datagramSocket.DisposedCheck();
						<cts>5__2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
					}
					try
					{
						if (num != 0)
						{
							CancellationToken token = <cts>5__2.Token;
							<>7__wrap2 = ((CancellationToken)(ref token)).Register(new Action(datagramSocket._client.Close));
						}
						try
						{
							TaskAwaiter val;
							if (num != 0)
							{
								val = datagramSocket._client.Client.ConnectAsync(address, port).GetAwaiter();
								if (!((TaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <ConnectAsync>d__21>(ref val, ref this);
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
						finally
						{
							if (num < 0)
							{
								((global::System.IDisposable)global::System.Runtime.CompilerServices.Unsafe.As<CancellationTokenRegistration, CancellationTokenRegistration>(ref <>7__wrap2)/*cast due to .constrained prefix*/).Dispose();
							}
						}
						<>7__wrap2 = default(CancellationTokenRegistration);
						if (datagramSocket._client.Client.Connected)
						{
							datagramSocket._stream = new NetworkStream(datagramSocket._client.Client, false);
						}
					}
					finally
					{
						if (num < 0 && <cts>5__2 != null)
						{
							((global::System.IDisposable)<cts>5__2).Dispose();
						}
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<cts>5__2 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<cts>5__2 = null;
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
		private struct <FlushAsync>d__24 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DatagramSocket <>4__this;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DatagramSocket datagramSocket = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_007d;
					}
					datagramSocket.DisposedCheck();
					if (datagramSocket._stream != null)
					{
						val = ((Stream)datagramSocket._stream).FlushAsync(cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <FlushAsync>d__24>(ref val, ref this);
							return;
						}
						goto IL_007d;
					}
					goto end_IL_000e;
					IL_007d:
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
		private struct <ReadAsync>d__22 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<int> <>t__builder;

			public DatagramSocket <>4__this;

			public byte[] buffer;

			public int offset;

			public int count;

			public CancellationToken cancellationToken;

			private ConfiguredTaskAwaiter<int> <>u__1;

			private void MoveNext()
			{
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008f: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DatagramSocket datagramSocket = <>4__this;
				int result;
				try
				{
					ConfiguredTaskAwaiter<int> val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(ConfiguredTaskAwaiter<int>);
						num = (<>1__state = -1);
						goto IL_009e;
					}
					datagramSocket.DisposedCheck();
					if (datagramSocket._stream != null)
					{
						val = ((Stream)datagramSocket._stream).ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<int>, <ReadAsync>d__22>(ref val, ref this);
							return;
						}
						goto IL_009e;
					}
					result = 0;
					goto end_IL_000e;
					IL_009e:
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

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <WriteAsync>d__23 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DatagramSocket <>4__this;

			public byte[] buffer;

			public int offset;

			public int count;

			public CancellationToken cancellationToken;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DatagramSocket datagramSocket = <>4__this;
				try
				{
					ConfiguredTaskAwaiter val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(ConfiguredTaskAwaiter);
						num = (<>1__state = -1);
						goto IL_009b;
					}
					datagramSocket.DisposedCheck();
					if (datagramSocket._stream != null)
					{
						ConfiguredTaskAwaitable val2 = ((Stream)datagramSocket._stream).WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
						val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <WriteAsync>d__23>(ref val, ref this);
							return;
						}
						goto IL_009b;
					}
					goto end_IL_000e;
					IL_009b:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
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

		private readonly UdpClient _client = new UdpClient();

		private NetworkStream? _stream;

		private bool _isDisposed;

		public bool IsConnected
		{
			get
			{
				UdpClient client = _client;
				bool? obj;
				if (client == null)
				{
					obj = null;
				}
				else
				{
					Socket client2 = client.Client;
					obj = ((client2 != null) ? new bool?(client2.Connected) : ((bool?)null));
				}
				bool? flag = obj;
				return flag == true;
			}
		}

		[field: CompilerGenerated]
		public bool NoDelay
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		} = true;

		public int ReadBufferSize
		{
			get
			{
				return _client.Client.ReceiveBufferSize;
			}
			set
			{
				_client.Client.ReceiveBufferSize = value;
			}
		}

		public int WriteBufferSize
		{
			get
			{
				return _client.Client.SendBufferSize;
			}
			set
			{
				_client.Client.SendBufferSize = value;
			}
		}

		public LingerOptions LingerState
		{
			get
			{
				return new LingerOptions(_client.Client.LingerState.Enabled, _client.Client.LingerState.LingerTime);
			}
			set
			{
				_client.Client.LingerState.Enabled = value.Enabled;
				_client.Client.LingerState.LingerTime = value.LingerTime;
			}
		}

		public EndPoint LocalEndPoint => _client.Client.LocalEndPoint;

		public EndPoint RemoteEndPoint => _client.Client.RemoteEndPoint;

		[AsyncStateMachine(typeof(<ConnectAsync>d__21))]
		public global::System.Threading.Tasks.Task ConnectAsync(IPAddress address, int port, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<ConnectAsync>d__21 <ConnectAsync>d__ = default(<ConnectAsync>d__21);
			<ConnectAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ConnectAsync>d__.<>4__this = this;
			<ConnectAsync>d__.address = address;
			<ConnectAsync>d__.port = port;
			<ConnectAsync>d__.cancellationToken = cancellationToken;
			<ConnectAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <ConnectAsync>d__.<>t__builder)).Start<<ConnectAsync>d__21>(ref <ConnectAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <ConnectAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<ReadAsync>d__22))]
		public async global::System.Threading.Tasks.Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			DisposedCheck();
			if (_stream == null)
			{
				return 0;
			}
			return await ((Stream)_stream).ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
		}

		[AsyncStateMachine(typeof(<WriteAsync>d__23))]
		public global::System.Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			<WriteAsync>d__23 <WriteAsync>d__ = default(<WriteAsync>d__23);
			<WriteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteAsync>d__.<>4__this = this;
			<WriteAsync>d__.buffer = buffer;
			<WriteAsync>d__.offset = offset;
			<WriteAsync>d__.count = count;
			<WriteAsync>d__.cancellationToken = cancellationToken;
			<WriteAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <WriteAsync>d__.<>t__builder)).Start<<WriteAsync>d__23>(ref <WriteAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <WriteAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<FlushAsync>d__24))]
		public global::System.Threading.Tasks.Task FlushAsync(CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<FlushAsync>d__24 <FlushAsync>d__ = default(<FlushAsync>d__24);
			<FlushAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<FlushAsync>d__.<>4__this = this;
			<FlushAsync>d__.cancellationToken = cancellationToken;
			<FlushAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <FlushAsync>d__.<>t__builder)).Start<<FlushAsync>d__24>(ref <FlushAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <FlushAsync>d__.<>t__builder)).Task;
		}

		public void Connect(IPAddress address, int port)
		{
			DisposedCheck();
			_client.Connect(address, port);
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			DisposedCheck();
			if (_stream == null)
			{
				return -1;
			}
			return ((Stream)_stream).Read(buffer, offset, count);
		}

		public void Write(byte[] buffer, int offset, int count)
		{
			DisposedCheck();
			if (_stream != null)
			{
				((Stream)_stream).Write(buffer, offset, count);
			}
		}

		public void Flush()
		{
			DisposedCheck();
			NetworkStream? stream = _stream;
			if (stream != null)
			{
				((Stream)stream).Flush();
			}
		}

		public void Disconnect()
		{
			DisposedCheck();
			_client.Close();
		}

		private void DisposedCheck()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (_isDisposed)
			{
				throw new ObjectDisposedException($"{typeof(Socket)} is disposed!");
			}
		}

		private void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			_isDisposed = true;
			try
			{
				NetworkStream? stream = _stream;
				if (stream != null)
				{
					((Stream)stream).Close();
				}
			}
			catch
			{
			}
			try
			{
				NetworkStream? stream2 = _stream;
				if (stream2 != null)
				{
					((Stream)stream2).Dispose();
				}
			}
			catch
			{
			}
			try
			{
				UdpClient client = _client;
				if (client != null)
				{
					client.Close();
				}
			}
			catch
			{
			}
			try
			{
				UdpClient client2 = _client;
				if (client2 != null)
				{
					client2.Dispose();
				}
			}
			catch
			{
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize((object)this);
		}

		public void Bind(EndPoint endPoint)
		{
			_client.Client.Bind(endPoint);
		}

		public Stream CreateNetworkStream()
		{
			return (Stream)(object)_stream;
		}
	}
	public struct LingerOptions
	{
		[field: CompilerGenerated]
		public bool Enabled
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public int LingerTime
		{
			[CompilerGenerated]
			get;
		}

		public LingerOptions(bool enabled, int lingerTime)
		{
			Enabled = enabled;
			LingerTime = lingerTime;
		}
	}
	public interface ISocket : global::System.IDisposable
	{
		bool IsConnected { get; }

		bool NoDelay { get; set; }

		int ReadBufferSize { get; set; }

		int WriteBufferSize { get; set; }

		LingerOptions LingerState { get; set; }

		EndPoint LocalEndPoint { get; }

		EndPoint RemoteEndPoint { get; }

		global::System.Threading.Tasks.Task ConnectAsync(IPAddress address, int port, CancellationToken cancellationToken);

		global::System.Threading.Tasks.Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

		global::System.Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

		global::System.Threading.Tasks.Task FlushAsync(CancellationToken cancellationToken);

		void Connect(IPAddress address, int port);

		int Read(byte[] buffer, int offset, int count);

		void Write(byte[] buffer, int offset, int count);

		void Flush();

		void Disconnect();

		void Bind(EndPoint endPoint);

		Stream CreateNetworkStream();
	}
	public class JavaSocketInputOutputStream : Stream
	{
		private readonly Stream _inputStream;

		private readonly Stream _outputStream;

		public override bool CanRead => _inputStream.CanRead;

		public override bool CanSeek => _inputStream.CanSeek;

		public override bool CanWrite => _outputStream.CanWrite;

		public override long Length => _inputStream.Length;

		public override long Position
		{
			get
			{
				return _inputStream.Position;
			}
			set
			{
				_inputStream.Position = value;
			}
		}

		public JavaSocketInputOutputStream(Stream inputStream, Stream outputStream)
		{
			_inputStream = inputStream;
			_outputStream = outputStream;
		}

		public override void Flush()
		{
			_inputStream.Flush();
			_outputStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return _inputStream.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return _inputStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			_inputStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_outputStream.Write(buffer, offset, count);
		}
	}
	internal sealed class AndroidSocket : Socket, ISocket, global::System.IDisposable
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ConnectAsync>d__30 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public CancellationToken cancellationToken;

			public IPAddress address;

			public int port;

			public AndroidSocket <>4__this;

			private CancellationTokenSource <cts>5__2;

			private CancellationTokenRegistration <>7__wrap2;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Expected O, but got Unknown
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Expected O, but got Unknown
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				AndroidSocket androidSocket = <>4__this;
				try
				{
					if (num != 0)
					{
						<cts>5__2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
					}
					try
					{
						InetSocketAddress val = default(InetSocketAddress);
						if (num != 0)
						{
							CancellationToken token = <cts>5__2.Token;
							val = new InetSocketAddress(((object)address).ToString(), port);
							<>7__wrap2 = ((CancellationToken)(ref token)).Register(new Action(((Socket)androidSocket).Close));
						}
						try
						{
							ConfiguredTaskAwaiter val3;
							if (num != 0)
							{
								ConfiguredTaskAwaitable val2 = ((Socket)androidSocket).ConnectAsync((SocketAddress)(object)val).ConfigureAwait(false);
								val3 = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
								if (!((ConfiguredTaskAwaiter)(ref val3)).IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val3;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <ConnectAsync>d__30>(ref val3, ref this);
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
						}
						finally
						{
							if (num < 0)
							{
								((global::System.IDisposable)global::System.Runtime.CompilerServices.Unsafe.As<CancellationTokenRegistration, CancellationTokenRegistration>(ref <>7__wrap2)/*cast due to .constrained prefix*/).Dispose();
							}
						}
						<>7__wrap2 = default(CancellationTokenRegistration);
						if (!((Socket)androidSocket).IsConnected && androidSocket._connectException != null)
						{
							throw androidSocket._connectException;
						}
					}
					finally
					{
						if (num < 0 && <cts>5__2 != null)
						{
							((global::System.IDisposable)<cts>5__2).Dispose();
						}
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<cts>5__2 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<cts>5__2 = null;
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
		private struct <FlushAsync>d__33 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public AndroidSocket <>4__this;

			public CancellationToken cancellationToken;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				AndroidSocket androidSocket = <>4__this;
				try
				{
					ConfiguredTaskAwaiter val2;
					if (num != 0)
					{
						ConfiguredTaskAwaitable val = ((Socket)androidSocket).OutputStream.FlushAsync(cancellationToken).ConfigureAwait(false);
						val2 = ((ConfiguredTaskAwaitable)(ref val)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <FlushAsync>d__33>(ref val2, ref this);
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
		private struct <ReadAsync>d__31 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<int> <>t__builder;

			public AndroidSocket <>4__this;

			public byte[] buffer;

			public int offset;

			public int count;

			public CancellationToken cancellationToken;

			private ConfiguredTaskAwaiter<int> <>u__1;

			private void MoveNext()
			{
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				//IL_007a: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				AndroidSocket androidSocket = <>4__this;
				int result;
				try
				{
					ConfiguredTaskAwaiter<int> val;
					if (num != 0)
					{
						val = ((Socket)androidSocket).InputStream.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<int>, <ReadAsync>d__31>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(ConfiguredTaskAwaiter<int>);
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
		private struct <WriteAsync>d__32 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public AndroidSocket <>4__this;

			public byte[] buffer;

			public int offset;

			public int count;

			public CancellationToken cancellationToken;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				AndroidSocket androidSocket = <>4__this;
				try
				{
					ConfiguredTaskAwaiter val2;
					if (num != 0)
					{
						ConfiguredTaskAwaitable val = ((Socket)androidSocket).OutputStream.WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
						val2 = ((ConfiguredTaskAwaitable)(ref val)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <WriteAsync>d__32>(ref val2, ref this);
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

		private global::System.Exception? _connectException;

		public bool NoDelay
		{
			get
			{
				return ((Socket)this).TcpNoDelay;
			}
			set
			{
				((Socket)this).TcpNoDelay = value;
			}
		}

		public int ReadBufferSize
		{
			get
			{
				return ((Socket)this).ReceiveBufferSize;
			}
			set
			{
				((Socket)this).ReceiveBufferSize = value;
			}
		}

		public int WriteBufferSize
		{
			get
			{
				return ((Socket)this).SendBufferSize;
			}
			set
			{
				((Socket)this).SendBufferSize = value;
			}
		}

		public LingerOptions LingerState
		{
			get
			{
				return new LingerOptions(((Socket)this).SoLinger > 0, ((Socket)this).SoLinger);
			}
			set
			{
				((Socket)this).SetSoLinger(value.Enabled, value.LingerTime);
			}
		}

		public EndPoint LocalEndPoint => (EndPoint)new IPEndPoint(new IPAddress(((Socket)this).LocalAddress.GetAddress()), ((Socket)this).LocalPort);

		public EndPoint RemoteEndPoint => (EndPoint)new IPEndPoint(new IPAddress(((Socket)this).InetAddress.GetAddress()), ((Socket)this).Port);

		public AndroidSocket()
		{
			AndroidSocketInit();
		}

		public AndroidSocket(Proxy proxy)
			: base(proxy)
		{
			AndroidSocketInit();
		}

		[Export(Throws = new global::System.Type[]
		{
			typeof(IOException),
			typeof(UnknownHostException)
		})]
		public AndroidSocket(string host, int port)
			: base(host, port)
		{
			AndroidSocketInit();
		}

		[Export(Throws = new global::System.Type[] { typeof(IOException) })]
		public AndroidSocket(InetAddress address, int port)
			: base(address, port)
		{
			AndroidSocketInit();
		}

		[Export(Throws = new global::System.Type[] { typeof(IOException) })]
		public AndroidSocket(string host, int port, bool stream)
			: base(host, port, stream)
		{
			AndroidSocketInit();
		}

		[Export(Throws = new global::System.Type[] { typeof(IOException) })]
		public AndroidSocket(InetAddress host, int port, bool stream)
			: base(host, port, stream)
		{
			AndroidSocketInit();
		}

		[Export(Throws = new global::System.Type[] { typeof(IOException) })]
		public AndroidSocket(string host, int port, InetAddress localAddr, int localPort)
			: base(host, port, localAddr, localPort)
		{
			AndroidSocketInit();
		}

		[Export(Throws = new global::System.Type[] { typeof(IOException) })]
		public AndroidSocket(InetAddress address, int port, InetAddress localAddr, int localPort)
			: base(address, port, localAddr, localPort)
		{
			AndroidSocketInit();
		}

		[Export(Throws = new global::System.Type[] { typeof(SocketException) })]
		public AndroidSocket(SocketImpl impl)
			: base(impl)
		{
			AndroidSocketInit();
		}

		public AndroidSocket(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			AndroidSocketInit();
		}

		private void AndroidSocketInit()
		{
			((Socket)this).TcpNoDelay = true;
			LingerState = new LingerOptions(enabled: true, 0);
		}

		[Export(Throws = new global::System.Type[]
		{
			typeof(SocketTimeoutException),
			typeof(NoRouteToHostException),
			typeof(IllegalBlockingModeException),
			typeof(IllegalArgumentException),
			typeof(IOException)
		})]
		public override void Connect(SocketAddress endpoint)
		{
			((Socket)this).Connect(endpoint, 0);
		}

		[Export(Throws = new global::System.Type[]
		{
			typeof(SocketTimeoutException),
			typeof(NoRouteToHostException),
			typeof(IllegalBlockingModeException),
			typeof(IllegalArgumentException),
			typeof(IOException)
		})]
		public override void Connect(SocketAddress endpoint, int timeout)
		{
			//IL_0012: Expected O, but got Unknown
			//IL_001c: Expected O, but got Unknown
			//IL_0026: Expected O, but got Unknown
			//IL_0030: Expected O, but got Unknown
			//IL_003b: Expected O, but got Unknown
			BindToNetwork(endpoint);
			try
			{
				((Socket)this).Connect(endpoint, timeout);
			}
			catch (SocketTimeoutException ex)
			{
				SocketTimeoutException connectException = ex;
				_connectException = (global::System.Exception?)(object)connectException;
			}
			catch (NoRouteToHostException ex2)
			{
				NoRouteToHostException connectException2 = ex2;
				_connectException = (global::System.Exception?)(object)connectException2;
			}
			catch (IllegalBlockingModeException ex3)
			{
				IllegalBlockingModeException connectException3 = ex3;
				_connectException = (global::System.Exception?)(object)connectException3;
			}
			catch (IllegalArgumentException ex4)
			{
				IllegalArgumentException connectException4 = ex4;
				_connectException = (global::System.Exception?)(object)connectException4;
			}
			catch (IOException ex5)
			{
				IOException connectException5 = ex5;
				_connectException = (global::System.Exception?)(object)connectException5;
			}
		}

		[AsyncStateMachine(typeof(<ConnectAsync>d__30))]
		public global::System.Threading.Tasks.Task ConnectAsync(IPAddress address, int port, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<ConnectAsync>d__30 <ConnectAsync>d__ = default(<ConnectAsync>d__30);
			<ConnectAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ConnectAsync>d__.<>4__this = this;
			<ConnectAsync>d__.address = address;
			<ConnectAsync>d__.port = port;
			<ConnectAsync>d__.cancellationToken = cancellationToken;
			<ConnectAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <ConnectAsync>d__.<>t__builder)).Start<<ConnectAsync>d__30>(ref <ConnectAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <ConnectAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<ReadAsync>d__31))]
		public async global::System.Threading.Tasks.Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			return await ((Socket)this).InputStream.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
		}

		[AsyncStateMachine(typeof(<WriteAsync>d__32))]
		public global::System.Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			<WriteAsync>d__32 <WriteAsync>d__ = default(<WriteAsync>d__32);
			<WriteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteAsync>d__.<>4__this = this;
			<WriteAsync>d__.buffer = buffer;
			<WriteAsync>d__.offset = offset;
			<WriteAsync>d__.count = count;
			<WriteAsync>d__.cancellationToken = cancellationToken;
			<WriteAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <WriteAsync>d__.<>t__builder)).Start<<WriteAsync>d__32>(ref <WriteAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <WriteAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<FlushAsync>d__33))]
		public global::System.Threading.Tasks.Task FlushAsync(CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<FlushAsync>d__33 <FlushAsync>d__ = default(<FlushAsync>d__33);
			<FlushAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<FlushAsync>d__.<>4__this = this;
			<FlushAsync>d__.cancellationToken = cancellationToken;
			<FlushAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <FlushAsync>d__.<>t__builder)).Start<<FlushAsync>d__33>(ref <FlushAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <FlushAsync>d__.<>t__builder)).Task;
		}

		public void Connect(IPAddress address, int port)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			InetSocketAddress val = new InetSocketAddress(((object)address).ToString(), port);
			((Socket)this).Connect((SocketAddress)(object)val);
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			return ((Socket)this).InputStream.Read(buffer, offset, count);
		}

		public void Write(byte[] buffer, int offset, int count)
		{
			((Socket)this).OutputStream.Write(buffer, offset, count);
		}

		public void Flush()
		{
			((Socket)this).InputStream.Flush();
		}

		public void Disconnect()
		{
			((Socket)this).Close();
		}

		private void BindToNetwork(SocketAddress endpoint)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			if ((int)VERSION.SdkInt < 21 || !AndroidIPAddressExtensions.TryGetNicId(new IPAddress(((InetSocketAddress)endpoint).Address.GetAddress()), out string nicId))
			{
				return;
			}
			Object systemService = Platform.AppContext.GetSystemService("connectivity");
			ConnectivityManager val = (ConnectivityManager)(object)((systemService is ConnectivityManager) ? systemService : null);
			if (val == null)
			{
				return;
			}
			Network[] allNetworks = val.GetAllNetworks();
			if (allNetworks == null)
			{
				return;
			}
			Network[] array = allNetworks;
			foreach (Network val2 in array)
			{
				LinkProperties linkProperties = val.GetLinkProperties(val2);
				string text = ((linkProperties != null) ? linkProperties.InterfaceName : null);
				if (text != null && text.Equals(nicId))
				{
					val2.BindSocket((Socket)(object)this);
					break;
				}
			}
		}

		public void Bind(EndPoint endPoint)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			IPEndPoint val = (IPEndPoint)(object)((endPoint is IPEndPoint) ? endPoint : null);
			if (val == null)
			{
				throw new ArgumentException("Endpoint must be an ip end point");
			}
			InetSocketAddress val2 = new InetSocketAddress(InetAddress.GetByAddress(val.Address.GetAddressBytes()), val.Port);
			((Socket)this).Bind((SocketAddress)(object)val2);
		}

		public Stream CreateNetworkStream()
		{
			return (Stream)(object)new JavaSocketInputOutputStream(((Socket)this).InputStream, ((Socket)this).OutputStream);
		}
	}
	internal sealed class Socket : ISocket, global::System.IDisposable
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ConnectAsync>d__20 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public Socket <>4__this;

			public CancellationToken cancellationToken;

			public IPAddress address;

			public int port;

			private CancellationTokenSource <cts>5__2;

			private CancellationTokenRegistration <>7__wrap2;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Expected O, but got Unknown
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				Socket socket = <>4__this;
				try
				{
					if (num != 0)
					{
						socket.DisposedCheck();
						<cts>5__2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
					}
					try
					{
						if (num != 0)
						{
							CancellationToken token = <cts>5__2.Token;
							<>7__wrap2 = ((CancellationToken)(ref token)).Register(new Action(socket._client.Close));
						}
						try
						{
							TaskAwaiter val;
							if (num != 0)
							{
								val = socket._client.ConnectAsync(address, port).GetAwaiter();
								if (!((TaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <ConnectAsync>d__20>(ref val, ref this);
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
						finally
						{
							if (num < 0)
							{
								((global::System.IDisposable)global::System.Runtime.CompilerServices.Unsafe.As<CancellationTokenRegistration, CancellationTokenRegistration>(ref <>7__wrap2)/*cast due to .constrained prefix*/).Dispose();
							}
						}
						<>7__wrap2 = default(CancellationTokenRegistration);
						if (socket._client.Connected)
						{
							socket._stream = socket._client.GetStream();
						}
					}
					finally
					{
						if (num < 0 && <cts>5__2 != null)
						{
							((global::System.IDisposable)<cts>5__2).Dispose();
						}
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<cts>5__2 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<cts>5__2 = null;
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
		private struct <FlushAsync>d__23 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public Socket <>4__this;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				Socket socket = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_007d;
					}
					socket.DisposedCheck();
					if (socket._stream != null)
					{
						val = ((Stream)socket._stream).FlushAsync(cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <FlushAsync>d__23>(ref val, ref this);
							return;
						}
						goto IL_007d;
					}
					goto end_IL_000e;
					IL_007d:
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
		private struct <ReadAsync>d__21 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<int> <>t__builder;

			public Socket <>4__this;

			public byte[] buffer;

			public int offset;

			public int count;

			public CancellationToken cancellationToken;

			private ConfiguredTaskAwaiter<int> <>u__1;

			private void MoveNext()
			{
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008f: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				Socket socket = <>4__this;
				int result;
				try
				{
					ConfiguredTaskAwaiter<int> val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(ConfiguredTaskAwaiter<int>);
						num = (<>1__state = -1);
						goto IL_009e;
					}
					socket.DisposedCheck();
					if (socket._stream != null)
					{
						val = ((Stream)socket._stream).ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<int>, <ReadAsync>d__21>(ref val, ref this);
							return;
						}
						goto IL_009e;
					}
					result = 0;
					goto end_IL_000e;
					IL_009e:
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

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <WriteAsync>d__22 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public Socket <>4__this;

			public byte[] buffer;

			public int offset;

			public int count;

			public CancellationToken cancellationToken;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				Socket socket = <>4__this;
				try
				{
					ConfiguredTaskAwaiter val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(ConfiguredTaskAwaiter);
						num = (<>1__state = -1);
						goto IL_009b;
					}
					socket.DisposedCheck();
					if (socket._stream != null)
					{
						ConfiguredTaskAwaitable val2 = ((Stream)socket._stream).WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
						val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <WriteAsync>d__22>(ref val, ref this);
							return;
						}
						goto IL_009b;
					}
					goto end_IL_000e;
					IL_009b:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
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

		private readonly TcpClient _client = new TcpClient
		{
			NoDelay = true,
			LingerState = new LingerOption(true, 0)
		};

		private NetworkStream? _stream;

		private bool _isDisposed;

		public bool IsConnected
		{
			get
			{
				TcpClient client = _client;
				if (client == null)
				{
					return false;
				}
				return client.Connected;
			}
		}

		public bool NoDelay
		{
			get
			{
				return _client.NoDelay;
			}
			set
			{
				_client.NoDelay = value;
			}
		}

		public int ReadBufferSize
		{
			get
			{
				return _client.ReceiveBufferSize;
			}
			set
			{
				_client.ReceiveBufferSize = value;
			}
		}

		public int WriteBufferSize
		{
			get
			{
				return _client.SendBufferSize;
			}
			set
			{
				_client.SendBufferSize = value;
			}
		}

		public LingerOptions LingerState
		{
			get
			{
				return new LingerOptions(_client.LingerState.Enabled, _client.LingerState.LingerTime);
			}
			set
			{
				_client.LingerState.Enabled = value.Enabled;
				_client.LingerState.LingerTime = value.LingerTime;
			}
		}

		public EndPoint LocalEndPoint => _client.Client.LocalEndPoint;

		public EndPoint RemoteEndPoint => _client.Client.RemoteEndPoint;

		[AsyncStateMachine(typeof(<ConnectAsync>d__20))]
		public global::System.Threading.Tasks.Task ConnectAsync(IPAddress address, int port, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<ConnectAsync>d__20 <ConnectAsync>d__ = default(<ConnectAsync>d__20);
			<ConnectAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ConnectAsync>d__.<>4__this = this;
			<ConnectAsync>d__.address = address;
			<ConnectAsync>d__.port = port;
			<ConnectAsync>d__.cancellationToken = cancellationToken;
			<ConnectAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <ConnectAsync>d__.<>t__builder)).Start<<ConnectAsync>d__20>(ref <ConnectAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <ConnectAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<ReadAsync>d__21))]
		public async global::System.Threading.Tasks.Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			DisposedCheck();
			if (_stream == null)
			{
				return 0;
			}
			return await ((Stream)_stream).ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
		}

		[AsyncStateMachine(typeof(<WriteAsync>d__22))]
		public global::System.Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			<WriteAsync>d__22 <WriteAsync>d__ = default(<WriteAsync>d__22);
			<WriteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteAsync>d__.<>4__this = this;
			<WriteAsync>d__.buffer = buffer;
			<WriteAsync>d__.offset = offset;
			<WriteAsync>d__.count = count;
			<WriteAsync>d__.cancellationToken = cancellationToken;
			<WriteAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <WriteAsync>d__.<>t__builder)).Start<<WriteAsync>d__22>(ref <WriteAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <WriteAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<FlushAsync>d__23))]
		public global::System.Threading.Tasks.Task FlushAsync(CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<FlushAsync>d__23 <FlushAsync>d__ = default(<FlushAsync>d__23);
			<FlushAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<FlushAsync>d__.<>4__this = this;
			<FlushAsync>d__.cancellationToken = cancellationToken;
			<FlushAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <FlushAsync>d__.<>t__builder)).Start<<FlushAsync>d__23>(ref <FlushAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <FlushAsync>d__.<>t__builder)).Task;
		}

		public void Connect(IPAddress address, int port)
		{
			DisposedCheck();
			_client.Connect(address, port);
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			DisposedCheck();
			if (_stream == null)
			{
				return -1;
			}
			return ((Stream)_stream).Read(buffer, offset, count);
		}

		public void Write(byte[] buffer, int offset, int count)
		{
			DisposedCheck();
			if (_stream != null)
			{
				((Stream)_stream).Write(buffer, offset, count);
			}
		}

		public void Flush()
		{
			DisposedCheck();
			NetworkStream? stream = _stream;
			if (stream != null)
			{
				((Stream)stream).Flush();
			}
		}

		public void Disconnect()
		{
			DisposedCheck();
			_client.Close();
		}

		private void DisposedCheck()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (_isDisposed)
			{
				throw new ObjectDisposedException($"{typeof(Socket)} is disposed!");
			}
		}

		private void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			_isDisposed = true;
			try
			{
				NetworkStream? stream = _stream;
				if (stream != null)
				{
					((Stream)stream).Close();
				}
			}
			catch
			{
			}
			try
			{
				NetworkStream? stream2 = _stream;
				if (stream2 != null)
				{
					((Stream)stream2).Dispose();
				}
			}
			catch
			{
			}
			try
			{
				TcpClient client = _client;
				if (client != null)
				{
					client.Close();
				}
			}
			catch
			{
			}
			try
			{
				TcpClient client2 = _client;
				if (client2 != null)
				{
					client2.Dispose();
				}
			}
			catch
			{
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize((object)this);
		}

		public void Bind(EndPoint endPoint)
		{
			_client.Client.Bind(endPoint);
		}

		public Stream CreateNetworkStream()
		{
			return (Stream)(object)_stream;
		}
	}
	public sealed class AndroidSocketFactory : SocketFactory
	{
		private static readonly object _lock = new object();

		private static volatile AndroidSocketFactory? _instance = null;

		public static AndroidSocketFactory Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new AndroidSocketFactory();
					}
				}
				return _instance;
			}
		}

		private AndroidSocketFactory()
		{
		}

		public override Socket CreateSocket()
		{
			return (Socket)(object)new AndroidSocket();
		}

		public override Socket CreateSocket(string host, int port)
		{
			return (Socket)(object)new AndroidSocket(host, port);
		}

		public override Socket CreateSocket(string host, int port, InetAddress localHost, int localPort)
		{
			return (Socket)(object)new AndroidSocket(host, port, localHost, localPort);
		}

		public override Socket CreateSocket(InetAddress host, int port)
		{
			return (Socket)(object)new AndroidSocket(host, port);
		}

		public override Socket CreateSocket(InetAddress address, int port, InetAddress localAddress, int localPort)
		{
			return (Socket)(object)new AndroidSocket(address, port, localAddress, localPort);
		}
	}
	public static class SocketFactory
	{
		public static ISocket GetSocket()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)VERSION.SdkInt < 21)
			{
				return new Socket();
			}
			return new AndroidSocket();
		}

		public static ISocket GetUdpSocket()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)VERSION.SdkInt < 21)
			{
				return new Socket();
			}
			return new AndroidDatagramSocket((SocketAddress)null);
		}
	}
}
