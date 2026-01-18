using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IDS.Core.Collections;
using IDS.Core.Events;
using IDS.Core.IDS_CAN.Devices;
using IDS.Core.Tasks;
using IDS.Core.Types;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: TargetFramework(".NETCoreApp,Version=v7.0", FrameworkDisplayName = ".NET 7.0")]
[assembly: AssemblyCompany("Innovative Design Solutions, Inc.\\r\\n6801 15 Mile Road\\r\\nSterling Heights, MI 48312\\r\\nwww.idselectronics.com")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("Copyright © 2012-2025 Innovative Design Solutions, Inc.")]
[assembly: AssemblyDescription("IDS Core IDS-CAN Library")]
[assembly: AssemblyFileVersion("4.6.4.0")]
[assembly: AssemblyInformationalVersion("4.6.4.0+e7121ede06f05d042cf24e77bdcf920e85f9188f")]
[assembly: AssemblyProduct("IDS.Core.IDS_CAN")]
[assembly: AssemblyTitle("IDS.Core.IDS_CAN")]
[assembly: AssemblyVersion("4.6.4.0")]
[module: RefSafetyRules(11)]
namespace Microsoft.CodeAnalysis
{
	[CompilerGenerated]
	[Embedded]
	internal sealed class EmbeddedAttribute : System.Attribute
	{
	}
}
namespace System.Runtime.CompilerServices
{
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	internal sealed class NullableAttribute : System.Attribute
	{
		public readonly byte[] NullableFlags;

		public NullableAttribute(byte P_0)
		{
			NullableFlags = new byte[1] { P_0 };
		}

		public NullableAttribute(byte[] P_0)
		{
			NullableFlags = P_0;
		}
	}
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	internal sealed class NullableContextAttribute : System.Attribute
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
	internal sealed class RefSafetyRulesAttribute : System.Attribute
	{
		public readonly int Version;

		public RefSafetyRulesAttribute(int P_0)
		{
			Version = P_0;
		}
	}
}
namespace IDS.Core.IDS_CAN
{
	[Flags]
	public enum ADAPTER_OPTIONS : uint
	{
		NONE = 0u,
		ENABLE_LOCAL_HOST = 1u,
		AUTO_READ_DEVICE_PID_LIST = 2u,
		AUTO_READ_DEVICE_SESSION_LIST = 4u,
		AUTO_READ_DEVICE_BLOCK_LIST = 8u,
		AUTO_READ_DEVICE_PART_NUMBER = 0x10u,
		AUTO_READ_DTC_COUNT = 0x20u
	}
	public interface IAdapter : IAdapter<MessageBuffer>, IAdapter<MessageBuffer>, IAdapter, IEventSender, IDisposableManager, IDisposable, System.IDisposable, IAdapter
	{
		ADAPTER_OPTIONS Options { get; set; }

		IDeviceDiscoverer Devices { get; }

		IProductManager Products { get; }

		ILocalProductManager LocalProducts { get; }

		ICircuitManager Circuits { get; }

		ILocalDevice LocalHost { get; }

		INetworkTime Clock { get; }

		IN_MOTION_LOCKOUT_LEVEL NetworkLevelInMotionLockoutLevel { get; }

		ADDRESS GetUnusedDeviceAddress();

		void EnableLocalHost();

		void DisableLocalHost();
	}
	public class Adapter : Adapter<MessageBuffer>, IAdapter, IAdapter<MessageBuffer>, IAdapter<MessageBuffer>, IAdapter, IEventSender, IDisposableManager, IDisposable, System.IDisposable, IAdapter
	{
		public enum ICON
		{
			CROSS,
			LED_OFF,
			LED_BLUE,
			LED_GREEN,
			LED_YELLOW,
			LED_ORANGE,
			LED_RED,
			LED_PURPLE,
			INFO,
			QUESTION,
			EXCLAMATION,
			CLOSE,
			PLUS,
			CHECKMARK,
			NEXT,
			UP,
			DOWN,
			STAR
		}

		public abstract class Object : DisposableManager
		{
			[field: CompilerGenerated]
			public IAdapter Adapter
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			public ILocalDevice LocalHost => Adapter?.LocalHost;

			[field: CompilerGenerated]
			protected SubscriptionManager Subscriptions
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			} = new SubscriptionManager();

			public Object(Adapter adapter)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Expected O, but got Unknown
				Adapter = adapter;
				((DisposableManager)adapter).AddDisposable((IDisposable)(object)this);
			}

			public override void Dispose(bool disposing)
			{
				((DisposableManager)this).Dispose(disposing);
				if (disposing)
				{
					Adapter = null;
					SubscriptionManager subscriptions = Subscriptions;
					if (subscriptions != null)
					{
						((Disposable)subscriptions).Dispose();
					}
					Subscriptions = null;
				}
			}
		}

		public abstract class BackgroundTaskObject : Object
		{
			public BackgroundTaskObject(Adapter adapter)
				: base(adapter)
			{
				adapter.BackgroundTasks.Add(this);
			}

			public abstract void BackgroundTask();
		}

		private class AddressDetectManager : BackgroundTaskObject
		{
			private class AddressDetector
			{
				private readonly IAdapter Adapter;

				private readonly ADDRESS Address;

				private List<ADDRESS> UnusedAddressList = new List<ADDRESS>();

				private Timer Timeout = new Timer(true);

				private bool Detected;

				public AddressDetector(ADDRESS address, AddressDetectManager mgr)
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0017: Expected O, but got Unknown
					Address = address;
					UnusedAddressList = mgr.UnusedAddressList;
					Adapter = mgr.Adapter;
				}

				public void Touch()
				{
					if (!Address.IsValidDeviceAddress)
					{
						return;
					}
					Timeout.Reset();
					if (Detected)
					{
						return;
					}
					lock (UnusedAddressList)
					{
						if (!Detected)
						{
							Detected = true;
							UnusedAddressList.Remove(Address);
						}
					}
				}

				public void Kill()
				{
					if (!Detected)
					{
						return;
					}
					lock (UnusedAddressList)
					{
						if (Detected)
						{
							Detected = false;
							UnusedAddressList.Add(Address);
							if (Adapter.Devices.GetDeviceByAddress(Address) is RemoteDevice remoteDevice)
							{
								remoteDevice.GoOffline();
							}
						}
					}
				}

				public void BackgroundTask()
				{
					//IL_001b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0020: Unknown result type (might be due to invalid IL or missing references)
					if (Detected && (!((IAdapter)Adapter).IsConnected || Timeout.ElapsedTime >= ADDRESS_DETECTED_TIMEOUT))
					{
						Kill();
					}
				}
			}

			private AddressDetector[] Address = new AddressDetector[256];

			private List<ADDRESS> UnusedAddressList = new List<ADDRESS>();

			private Timer ListenTime = new Timer(true);

			private IN_MOTION_LOCKOUT_LEVEL mInMotionLockoutLevel = (byte)0;

			private Timer[] InMotionLockoutTimer = (Timer[])(object)new Timer[4];

			private NetworkInMotionLockoutLevelChangedEvent InMotionLockoutEvent;

			public IN_MOTION_LOCKOUT_LEVEL InMotionLockoutLevel
			{
				get
				{
					return mInMotionLockoutLevel;
				}
				private set
				{
					if (mInMotionLockoutLevel != value)
					{
						IN_MOTION_LOCKOUT_LEVEL prev = mInMotionLockoutLevel;
						mInMotionLockoutLevel = value;
						InMotionLockoutEvent.Publish(value, prev);
					}
				}
			}

			public AddressDetectManager(Adapter adapter)
				: base(adapter)
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Expected O, but got Unknown
				//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f4: Expected O, but got Unknown
				for (int i = 0; i < Address.Length; i++)
				{
					Address[i] = new AddressDetector((byte)i, this);
				}
				List<ADDRESS> val = new List<ADDRESS>();
				System.Collections.Generic.IEnumerator<ADDRESS> enumerator = ADDRESS.GetEnumerator().GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						ADDRESS current = enumerator.Current;
						if (current.IsValidDeviceAddress)
						{
							val.Add(current);
						}
					}
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
				while (val.Count > 0)
				{
					int num = ThreadLocalRandom.Next(val.Count);
					ADDRESS aDDRESS = val[num];
					val.RemoveAt(num);
					UnusedAddressList.Add(aDDRESS);
				}
				for (int j = 0; j < InMotionLockoutTimer.Length; j++)
				{
					InMotionLockoutTimer[j] = new Timer(true);
				}
				((IEventSender)base.Adapter).Events.Subscribe<AdapterOpenedEvent>((Action<AdapterOpenedEvent>)OnAdapterOpened, (SubscriptionType)1, base.Subscriptions);
				((IEventSender)base.Adapter).Events.Subscribe<AdapterClosedEvent>((Action<AdapterClosedEvent>)OnAdapterClosed, (SubscriptionType)1, base.Subscriptions);
				((IEventSender)base.Adapter).Events.Subscribe<AdapterRxEvent>((Action<AdapterRxEvent>)OnAdapterRx, (SubscriptionType)1, base.Subscriptions);
				InMotionLockoutEvent = new NetworkInMotionLockoutLevelChangedEvent(base.Adapter);
			}

			public override void Dispose(bool disposing)
			{
				if (disposing)
				{
					UnusedAddressList = null;
					Address = null;
				}
			}

			private void OnAdapterOpened(AdapterOpenedEvent message)
			{
				ListenTime.Reset();
				AddressDetector[] address = Address;
				for (int i = 0; i < address.Length; i++)
				{
					address[i].Kill();
				}
			}

			private void OnAdapterClosed(AdapterClosedEvent message)
			{
				AddressDetector[] address = Address;
				for (int i = 0; i < address.Length; i++)
				{
					address[i].Kill();
				}
			}

			private void OnAdapterRx(AdapterRxEvent rx)
			{
				if (!((IAdapter)base.Adapter).IsConnected)
				{
					return;
				}
				if (rx.SourceAddress.IsValidDeviceAddress)
				{
					Address[(byte)rx.SourceAddress].Touch();
				}
				if ((byte)rx.MessageType != 0 || rx.Count != 8)
				{
					return;
				}
				if (rx.SourceAddress == ADDRESS.BROADCAST)
				{
					ADDRESS aDDRESS = rx[0];
					if (aDDRESS.IsValidDeviceAddress)
					{
						Address[(byte)aDDRESS].Touch();
					}
				}
				else if (rx.SourceAddress.IsValidDeviceAddress)
				{
					IN_MOTION_LOCKOUT_LEVEL inMotionLockoutLevel = new NETWORK_STATUS(rx[0], rx[1]).InMotionLockoutLevel;
					InMotionLockoutTimer[(byte)inMotionLockoutLevel].Reset();
					if ((byte)InMotionLockoutLevel < (byte)inMotionLockoutLevel)
					{
						InMotionLockoutLevel = inMotionLockoutLevel;
					}
				}
			}

			public ADDRESS GetUnusedDeviceAddress()
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				if (!((IAdapter)base.Adapter).IsConnected)
				{
					return ADDRESS.INVALID;
				}
				if (ListenTime.ElapsedTime < BUS_LISTEN_TIME)
				{
					return ADDRESS.INVALID;
				}
				lock (UnusedAddressList)
				{
					if (UnusedAddressList.Count <= 0)
					{
						return ADDRESS.INVALID;
					}
					ADDRESS aDDRESS = UnusedAddressList[0];
					UnusedAddressList.RemoveAt(0);
					UnusedAddressList.Add(aDDRESS);
					return aDDRESS;
				}
			}

			public override void BackgroundTask()
			{
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				if ((byte)InMotionLockoutLevel > 0)
				{
					byte b = InMotionLockoutLevel;
					if (InMotionLockoutTimer[b].ElapsedTime >= IN_MOTION_LOCKOUT_TIMEOUT)
					{
						while (true)
						{
							if (--b <= 0)
							{
								InMotionLockoutLevel = (byte)0;
								break;
							}
							if (InMotionLockoutTimer[b].ElapsedTime < IN_MOTION_LOCKOUT_TIMEOUT)
							{
								InMotionLockoutLevel = b;
								break;
							}
						}
					}
				}
				AddressDetector[] address = Address;
				for (int i = 0; i < address.Length; i++)
				{
					address[i].BackgroundTask();
				}
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ConnectAsync>d__10 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public Adapter <>4__this;

			public AsyncOperation obj;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				Adapter adapter = <>4__this;
				bool result;
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						val = ((Adapter)adapter.Bridge).OpenAsync(obj).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <ConnectAsync>d__10>(ref val, ref this);
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
				catch (System.Exception exception)
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
		private struct <DisconnectAsync>d__11 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public Adapter <>4__this;

			public AsyncOperation obj;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				Adapter adapter = <>4__this;
				bool result;
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						val = ((Adapter)adapter.Bridge).CloseAsync(obj).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <DisconnectAsync>d__11>(ref val, ref this);
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
				catch (System.Exception exception)
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

		private const int MIN_BACKGROUND_MESSAGE_TX_RATE = 25;

		private const int DEFAULT_BACKGROUND_MESSAGE_TX_RATE = 400;

		private static readonly TimeSpan BUS_LISTEN_TIME = TimeSpan.FromSeconds(1.0);

		private static readonly TimeSpan ADDRESS_DETECTED_TIMEOUT = TimeSpan.FromSeconds(5.0);

		private static readonly TimeSpan IN_MOTION_LOCKOUT_TIMEOUT = TimeSpan.FromSeconds(5.0);

		private Adapter<MessageBuffer> Bridge;

		private PeriodicTask mBackgroundTask;

		private readonly List<BackgroundTaskObject> BackgroundTasks = new List<BackgroundTaskObject>();

		private readonly AddressDetectManager AddressDetector;

		private readonly AdapterRxEvent RxEvent;

		private readonly MAC mMAC;

		private readonly LocalProduct mLocalProduct;

		private readonly LocalDevice mLocalHost;

		private readonly ProductManager mRemoteProductManager;

		private LocalProductManager mLocalProductManager;

		[field: CompilerGenerated]
		public ADAPTER_OPTIONS Options
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public override IPhysicalAddress MAC => (IPhysicalAddress)(object)mMAC;

		[field: CompilerGenerated]
		public IDeviceDiscoverer Devices
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public IProductManager Products => mRemoteProductManager;

		public ILocalProductManager LocalProducts => mLocalProductManager;

		[field: CompilerGenerated]
		public ICircuitManager Circuits
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public int NumDevicesDetectedOnNetwork => Devices.NumDevicesDetectedOnNetwork;

		public ILocalDevice LocalHost => mLocalHost;

		[field: CompilerGenerated]
		public INetworkTime Clock
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public IN_MOTION_LOCKOUT_LEVEL NetworkLevelInMotionLockoutLevel => AddressDetector.InMotionLockoutLevel;

		private int BackgroundTxMessagesPerSecond
		{
			get
			{
				return ((Adapter)this).BackgroundTxMessagesPerSecond;
			}
			set
			{
				((Adapter)this).BackgroundTxMessagesPerSecond = Math.Max(25, value);
			}
		}

		[AsyncStateMachine(typeof(<ConnectAsync>d__10))]
		protected override async System.Threading.Tasks.Task<bool> ConnectAsync(AsyncOperation obj)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await ((Adapter)Bridge).OpenAsync(obj);
		}

		[AsyncStateMachine(typeof(<DisconnectAsync>d__11))]
		protected override async System.Threading.Tasks.Task<bool> DisconnectAsync(AsyncOperation obj)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await ((Adapter)Bridge).CloseAsync(obj);
		}

		protected override bool TransmitRaw(MessageBuffer message)
		{
			return ((Adapter<MessageBuffer>)(object)Bridge).Transmit(message);
		}

		public ADDRESS GetUnusedDeviceAddress()
		{
			return AddressDetector.GetUnusedDeviceAddress();
		}

		public Adapter(string name, string software_part_number, DEVICE_ID id, Adapter<MessageBuffer> bridge, ADAPTER_OPTIONS options)
			: base(name, bridge.BaudRate)
		{
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Expected O, but got Unknown
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			Options = options;
			if (((Adapter)bridge).IsConnected)
			{
				throw new InvalidOperationException("new IDS_CAN.Adapter(): CAN bridge must be in the disconnected state during construction");
			}
			Bridge = bridge;
			((DisposableManager)this).AddDisposable((IDisposable)(object)bridge);
			mMAC = new MAC(((Adapter)bridge).MAC);
			mLocalProductManager = new LocalProductManager(this);
			mLocalProduct = new LocalProduct(this, new MAC(((Adapter)bridge).MAC), id.ProductID, (byte)28, software_part_number);
			mLocalHost = CreateDefaultLocalHost(mLocalProduct, id.DeviceType, id.DeviceInstance, id.FunctionName, id.FunctionInstance, id.DeviceCapabilities);
			((DisposableManager)this).AddDisposable((IDisposable)(object)mLocalHost);
			Clock = new NetworkTime(this);
			AddressDetector = new AddressDetectManager(this);
			Devices = new DeviceDiscoverer(this);
			Circuits = new CircuitManager(this);
			mRemoteProductManager = new ProductManager(this);
			RxEvent = new AdapterRxEvent(this);
			((Adapter)bridge).Events.Subscribe<AdapterOpenedEvent>((Action<AdapterOpenedEvent>)OnBridgeAdapterOpened, (SubscriptionType)1, ((Adapter)this).Subscriptions);
			((Adapter)bridge).Events.Subscribe<AdapterClosedEvent>((Action<AdapterClosedEvent>)OnBridgeAdapterClosed, (SubscriptionType)1, ((Adapter)this).Subscriptions);
			((Adapter)bridge).Events.Subscribe<AdapterRxEvent<MessageBuffer>>((Action<AdapterRxEvent<MessageBuffer>>)OnBridgeAdapterRx, (SubscriptionType)1, ((Adapter)this).Subscriptions);
			((Adapter)this).Events.Subscribe<AdapterOpenedEvent>((Action<AdapterOpenedEvent>)OnAdapterOpened, (SubscriptionType)1, ((Adapter)this).Subscriptions);
			((Adapter)this).Events.Subscribe<AdapterRxEvent<MessageBuffer>>((Action<AdapterRxEvent<MessageBuffer>>)OnAdapterMessageRx, (SubscriptionType)1, ((Adapter)this).Subscriptions);
			mBackgroundTask = new PeriodicTask(new Action(AdapterBackgroundTask), TimeSpan.FromMilliseconds(40.0), (Type)0, true);
			mLocalHost.EnableDevice = ((System.Enum)Options).HasFlag((System.Enum)ADAPTER_OPTIONS.ENABLE_LOCAL_HOST);
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((Disposable)mBackgroundTask).Dispose();
				LocalProductManager localProductManager = mLocalProductManager;
				if (localProductManager != null)
				{
					((Disposable)localProductManager).Dispose();
				}
				mLocalProductManager = null;
			}
			((Adapter)this).Dispose(disposing);
		}

		protected virtual LocalDevice CreateDefaultLocalHost(LocalProduct local_product, DEVICE_TYPE device_type, int device_instance, FUNCTION_NAME function_name, int function_instance, byte? capabilties)
		{
			return local_product.CreateDevice(device_type, device_instance, function_name, function_instance, capabilties, LOCAL_DEVICE_OPTIONS.NONE);
		}

		public void EnableLocalHost()
		{
			mLocalHost.EnableDevice = true;
		}

		public void DisableLocalHost()
		{
			mLocalHost.EnableDevice = false;
		}

		private void OnBridgeAdapterOpened(AdapterOpenedEvent message)
		{
			((Adapter)this).RaiseAdapterOpened();
		}

		private void OnBridgeAdapterClosed(AdapterClosedEvent message)
		{
			((Adapter)this).RaiseAdapterClosed();
		}

		private void OnBridgeAdapterRx(AdapterRxEvent<MessageBuffer> rx)
		{
			((Adapter<MessageBuffer>)(object)this).RaiseMessageRx(rx.Message, rx.Echo);
		}

		private void OnAdapterOpened(AdapterOpenedEvent message)
		{
			if (BackgroundTxMessagesPerSecond < 25)
			{
				BackgroundTxMessagesPerSecond = 400;
			}
		}

		private void OnAdapterMessageRx(AdapterRxEvent<MessageBuffer> args)
		{
			RxEvent.Publish((IMessage)(object)args.Message, args.Echo);
		}

		private void AdapterBackgroundTask()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed)
			{
				return;
			}
			try
			{
				Enumerator<BackgroundTaskObject> enumerator = BackgroundTasks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						BackgroundTaskObject current = enumerator.Current;
						try
						{
							current?.BackgroundTask();
						}
						catch
						{
						}
					}
				}
				finally
				{
					((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
			}
			catch
			{
			}
		}
	}
	public interface IDeviceBLOCK : IEventSender
	{
		BLOCK_ID ID { get; }

		IRemoteDevice Device { get; }

		string Name { get; }

		bool IsValueValid { get; }

		string ValueString { get; }

		byte GetData(int nb);

		bool RequestRead();

		System.Threading.Tasks.Task<BLOCKPropertyValue> ReadPropertyAsync(byte Property, AsyncOperation operation);

		System.Threading.Tasks.Task<bool> StartReadData(uint Offset, byte Size_Msg, byte DelayMs, AsyncOperation operation);

		System.Threading.Tasks.Task<bool> ReadDataBufferReadyAsync(AsyncOperation operation);
	}
	internal class DeviceBLOCK : Disposable, IDeviceBLOCK, IEventSender
	{
		public enum BLOCK_STATE : byte
		{
			BLOCK_IDLE,
			BLOCK_READ_PROPERTIES,
			BLOCK_START_TO_READ_DATA,
			BLOCK_WAIT_END_TRANSFER
		}

		private class AsyncSignal
		{
			public bool OperationComplete;
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ReadDataBufferReadyAsync>d__74 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public AsyncOperation operation;

			public DeviceBLOCK <>4__this;

			private AsyncSignal <signal>5__2;

			private Timer <tx_timer>5__3;

			private Timer <progress_timer>5__4;

			private bool <skip>5__5;

			private TimeSpan <ReadRequestTime>5__6;

			private TimeSpan <ReportProgressTime>5__7;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0162: Unknown result type (might be due to invalid IL or missing references)
				//IL_0167: Unknown result type (might be due to invalid IL or missing references)
				//IL_016f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Expected O, but got Unknown
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Expected O, but got Unknown
				//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
				//IL_0124: Unknown result type (might be due to invalid IL or missing references)
				//IL_0129: Unknown result type (might be due to invalid IL or missing references)
				//IL_012d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				//IL_0205: Unknown result type (might be due to invalid IL or missing references)
				//IL_020b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0147: Unknown result type (might be due to invalid IL or missing references)
				//IL_0149: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DeviceBLOCK deviceBLOCK = <>4__this;
				bool result;
				try
				{
					if (num != 0)
					{
						operation.ReportProgress(0f, "Reading...");
						<signal>5__2 = new AsyncSignal();
						if (deviceBLOCK.ReadSignals == null)
						{
							object obj = Lock;
							bool flag = false;
							try
							{
								Monitor.Enter(obj, ref flag);
								if (deviceBLOCK.ReadSignals == null)
								{
									deviceBLOCK.ReadSignals = new ConcurrentQueue<AsyncSignal>();
								}
							}
							finally
							{
								if (num < 0 && flag)
								{
									Monitor.Exit(obj);
								}
							}
						}
						deviceBLOCK.ReadSignals.Enqueue(<signal>5__2);
						<tx_timer>5__3 = new Timer(true);
						<progress_timer>5__4 = new Timer(true);
						<tx_timer>5__3.ElapsedTime = TimeSpan.FromSeconds(1.0);
						<progress_timer>5__4.ElapsedTime = TimeSpan.FromSeconds(1.0);
						<skip>5__5 = true;
						<ReadRequestTime>5__6 = TimeSpan.FromMilliseconds(330.0);
						<ReportProgressTime>5__7 = TimeSpan.FromMilliseconds(250.0);
						goto IL_0237;
					}
					ConfiguredTaskAwaiter val = <>u__1;
					<>u__1 = default(ConfiguredTaskAwaiter);
					num = (<>1__state = -1);
					goto IL_017e;
					IL_0237:
					if (!<signal>5__2.OperationComplete)
					{
						operation.ThrowIfCancellationRequested();
						if (<skip>5__5)
						{
							<skip>5__5 = false;
							goto IL_0185;
						}
						ConfiguredTaskAwaitable val2 = System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
						val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <ReadDataBufferReadyAsync>d__74>(ref val, ref this);
							return;
						}
						goto IL_017e;
					}
					operation.ReportProgress(100f, "Success!");
					result = true;
					goto end_IL_000e;
					IL_0185:
					if (((Disposable)deviceBLOCK).IsDisposed)
					{
						operation.ReportProgress("ReadAsync failed: BLOCK is disposed");
						result = false;
					}
					else
					{
						if (deviceBLOCK.Device.IsOnline)
						{
							if (<tx_timer>5__3.ElapsedTime >= <ReadRequestTime>5__6 && deviceBLOCK.IsReadDataReady)
							{
								<signal>5__2.OperationComplete = true;
								<tx_timer>5__3.Reset();
							}
							if (<progress_timer>5__4.ElapsedTime > <ReportProgressTime>5__7)
							{
								<progress_timer>5__4.Reset();
								operation.ReportProgress(0f, "Reading...");
							}
							goto IL_0237;
						}
						operation.ReportProgress("ReadAsync failed: Device is offline");
						result = false;
					}
					goto end_IL_000e;
					IL_017e:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
					goto IL_0185;
					end_IL_000e:;
				}
				catch (System.Exception exception)
				{
					<>1__state = -2;
					<signal>5__2 = null;
					<tx_timer>5__3 = null;
					<progress_timer>5__4 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<signal>5__2 = null;
				<tx_timer>5__3 = null;
				<progress_timer>5__4 = null;
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
		private struct <ReadPropertyAsync>d__72 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<BLOCKPropertyValue> <>t__builder;

			public AsyncOperation operation;

			public DeviceBLOCK <>4__this;

			public byte Property;

			private BLOCKPropertyValue <TmpReturVal>5__2;

			private AsyncSignal <signal>5__3;

			private Timer <tx_timer>5__4;

			private Timer <progress_timer>5__5;

			private bool <skip>5__6;

			private TimeSpan <ReadRequestTime>5__7;

			private TimeSpan <ReportProgressTime>5__8;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0182: Unknown result type (might be due to invalid IL or missing references)
				//IL_0187: Unknown result type (might be due to invalid IL or missing references)
				//IL_018f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Expected O, but got Unknown
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Expected O, but got Unknown
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				//IL_0111: Unknown result type (might be due to invalid IL or missing references)
				//IL_0116: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
				//IL_0144: Unknown result type (might be due to invalid IL or missing references)
				//IL_0149: Unknown result type (might be due to invalid IL or missing references)
				//IL_014d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0152: Unknown result type (might be due to invalid IL or missing references)
				//IL_0229: Unknown result type (might be due to invalid IL or missing references)
				//IL_022f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0167: Unknown result type (might be due to invalid IL or missing references)
				//IL_0169: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DeviceBLOCK deviceBLOCK = <>4__this;
				BLOCKPropertyValue result;
				try
				{
					if (num != 0)
					{
						operation.ReportProgress(0f, "Reading...");
						<TmpReturVal>5__2 = new BLOCKPropertyValue(0uL, Isvaluevalid: false);
						deviceBLOCK.SetPropertyValue(Property, <TmpReturVal>5__2);
						<signal>5__3 = new AsyncSignal();
						if (deviceBLOCK.ReadSignals == null)
						{
							object obj = Lock;
							bool flag = false;
							try
							{
								Monitor.Enter(obj, ref flag);
								if (deviceBLOCK.ReadSignals == null)
								{
									deviceBLOCK.ReadSignals = new ConcurrentQueue<AsyncSignal>();
								}
							}
							finally
							{
								if (num < 0 && flag)
								{
									Monitor.Exit(obj);
								}
							}
						}
						deviceBLOCK.ReadSignals.Enqueue(<signal>5__3);
						<tx_timer>5__4 = new Timer(true);
						<progress_timer>5__5 = new Timer(true);
						<tx_timer>5__4.ElapsedTime = TimeSpan.FromSeconds(1.0);
						<progress_timer>5__5.ElapsedTime = TimeSpan.FromSeconds(1.0);
						<skip>5__6 = true;
						<ReadRequestTime>5__7 = TimeSpan.FromMilliseconds(330.0);
						<ReportProgressTime>5__8 = TimeSpan.FromMilliseconds(250.0);
						goto IL_025b;
					}
					ConfiguredTaskAwaiter val = <>u__1;
					<>u__1 = default(ConfiguredTaskAwaiter);
					num = (<>1__state = -1);
					goto IL_019e;
					IL_025b:
					if (!<signal>5__3.OperationComplete)
					{
						operation.ThrowIfCancellationRequested();
						if (<skip>5__6)
						{
							<skip>5__6 = false;
							goto IL_01a5;
						}
						ConfiguredTaskAwaitable val2 = System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
						val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <ReadPropertyAsync>d__72>(ref val, ref this);
							return;
						}
						goto IL_019e;
					}
					operation.ReportProgress(100f, "Success!");
					result = deviceBLOCK.GetPropertyValue(Property);
					goto end_IL_000e;
					IL_01a5:
					if (((Disposable)deviceBLOCK).IsDisposed)
					{
						operation.ReportProgress("ReadAsync failed: BLOCK is disposed");
						result = <TmpReturVal>5__2;
					}
					else
					{
						if (deviceBLOCK.Device.IsOnline)
						{
							if (<tx_timer>5__4.ElapsedTime >= <ReadRequestTime>5__7 && deviceBLOCK.RequestRead(Property))
							{
								<tx_timer>5__4.Reset();
							}
							if (<progress_timer>5__5.ElapsedTime > <ReportProgressTime>5__8)
							{
								<progress_timer>5__5.Reset();
								operation.ReportProgress(0f, "Reading...");
							}
							goto IL_025b;
						}
						operation.ReportProgress("ReadAsync failed: Device is offline");
						result = <TmpReturVal>5__2;
					}
					goto end_IL_000e;
					IL_019e:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
					goto IL_01a5;
					end_IL_000e:;
				}
				catch (System.Exception exception)
				{
					<>1__state = -2;
					<signal>5__3 = null;
					<tx_timer>5__4 = null;
					<progress_timer>5__5 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<signal>5__3 = null;
				<tx_timer>5__4 = null;
				<progress_timer>5__5 = null;
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
		private struct <StartReadData>d__73 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public AsyncOperation operation;

			public DeviceBLOCK <>4__this;

			public uint Offset;

			public byte Size_Msg;

			public byte DelayMs;

			private AsyncSignal <signal>5__2;

			private Timer <tx_timer>5__3;

			private Timer <progress_timer>5__4;

			private bool <skip>5__5;

			private TimeSpan <ReadRequestTime>5__6;

			private TimeSpan <ReportProgressTime>5__7;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0162: Unknown result type (might be due to invalid IL or missing references)
				//IL_0167: Unknown result type (might be due to invalid IL or missing references)
				//IL_016f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Expected O, but got Unknown
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Expected O, but got Unknown
				//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
				//IL_0124: Unknown result type (might be due to invalid IL or missing references)
				//IL_0129: Unknown result type (might be due to invalid IL or missing references)
				//IL_012d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				//IL_020b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0211: Unknown result type (might be due to invalid IL or missing references)
				//IL_0147: Unknown result type (might be due to invalid IL or missing references)
				//IL_0149: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DeviceBLOCK deviceBLOCK = <>4__this;
				bool result;
				try
				{
					if (num != 0)
					{
						operation.ReportProgress(0f, "Reading...");
						<signal>5__2 = new AsyncSignal();
						if (deviceBLOCK.ReadSignals == null)
						{
							object obj = Lock;
							bool flag = false;
							try
							{
								Monitor.Enter(obj, ref flag);
								if (deviceBLOCK.ReadSignals == null)
								{
									deviceBLOCK.ReadSignals = new ConcurrentQueue<AsyncSignal>();
								}
							}
							finally
							{
								if (num < 0 && flag)
								{
									Monitor.Exit(obj);
								}
							}
						}
						deviceBLOCK.ReadSignals.Enqueue(<signal>5__2);
						<tx_timer>5__3 = new Timer(true);
						<progress_timer>5__4 = new Timer(true);
						<tx_timer>5__3.ElapsedTime = TimeSpan.FromSeconds(1.0);
						<progress_timer>5__4.ElapsedTime = TimeSpan.FromSeconds(1.0);
						<skip>5__5 = true;
						<ReadRequestTime>5__6 = TimeSpan.FromMilliseconds(330.0);
						<ReportProgressTime>5__7 = TimeSpan.FromMilliseconds(250.0);
						goto IL_023d;
					}
					ConfiguredTaskAwaiter val = <>u__1;
					<>u__1 = default(ConfiguredTaskAwaiter);
					num = (<>1__state = -1);
					goto IL_017e;
					IL_023d:
					if (!<signal>5__2.OperationComplete)
					{
						operation.ThrowIfCancellationRequested();
						if (<skip>5__5)
						{
							<skip>5__5 = false;
							goto IL_0185;
						}
						ConfiguredTaskAwaitable val2 = System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
						val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <StartReadData>d__73>(ref val, ref this);
							return;
						}
						goto IL_017e;
					}
					operation.ReportProgress(100f, "Success!");
					result = true;
					goto end_IL_000e;
					IL_0185:
					if (((Disposable)deviceBLOCK).IsDisposed)
					{
						operation.ReportProgress("ReadAsync failed: BLOCK is disposed");
						result = false;
					}
					else
					{
						if (deviceBLOCK.Device.IsOnline)
						{
							if (<tx_timer>5__3.ElapsedTime >= <ReadRequestTime>5__6 && deviceBLOCK.StartReadData(Offset, Size_Msg, DelayMs))
							{
								<tx_timer>5__3.Reset();
							}
							if (<progress_timer>5__4.ElapsedTime > <ReportProgressTime>5__7)
							{
								<progress_timer>5__4.Reset();
								operation.ReportProgress(0f, "Reading...");
							}
							goto IL_023d;
						}
						operation.ReportProgress("ReadAsync failed: Device is offline");
						result = false;
					}
					goto end_IL_000e;
					IL_017e:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
					goto IL_0185;
					end_IL_000e:;
				}
				catch (System.Exception exception)
				{
					<>1__state = -2;
					<signal>5__2 = null;
					<tx_timer>5__3 = null;
					<progress_timer>5__4 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<signal>5__2 = null;
				<tx_timer>5__3 = null;
				<progress_timer>5__4 = null;
				<>t__builder.SetResult(result);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		public byte[] Data;

		public BLOCKPropertyValue[] PropertyValues = new BLOCKPropertyValue[8];

		public int NbData;

		public bool IsReadDataReady;

		private bool ValueValid;

		private static readonly object Lock = new object();

		private ConcurrentQueue<AsyncSignal> ReadSignals;

		[field: CompilerGenerated]
		public IEventPublisher Events
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IRemoteDevice Device
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public BLOCK_ID ID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public uint blockoffset
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ushort actualbulktransfersize
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ushort EndBulkXferOffset
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public uint EndBulkXferCRC32
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public byte State
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public byte Response
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public string Name => ID.Name;

		public bool IsValueValid
		{
			get
			{
				ValueValid &= Device.IsOnline;
				return ValueValid;
			}
		}

		public string ValueString
		{
			get
			{
				if (!IsValueValid)
				{
					return "UNKNOWN";
				}
				return ID.Name;
			}
		}

		public byte GetData(int nb)
		{
			return Data[nb];
		}

		public void Setblockoffset(uint Param)
		{
			blockoffset = Param;
		}

		public void Setactualbulktransfersize(ushort Param)
		{
			actualbulktransfersize = Param;
		}

		public void SetEndBulkXferOffset(ushort Param)
		{
			EndBulkXferOffset = Param;
		}

		public void SetEndBulkXferCRC32(uint Param)
		{
			EndBulkXferCRC32 = Param;
		}

		public void SetState(byte Param)
		{
			State = Param;
		}

		public void SetResponse(byte Param)
		{
			Response = Param;
		}

		public override string ToString()
		{
			return Name;
		}

		public DeviceBLOCK(IRemoteDevice device, BLOCK_ID id, bool Init)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			Device = device;
			ID = id;
			if (Init)
			{
				Events = null;
			}
			else
			{
				Events = (IEventPublisher)new EventPublisher("IDS.Core.IDS_CAN.BLOCK.Events");
			}
		}

		public void OnMessageEndBulkXferRx(ushort BlockOffset, uint Crc32)
		{
			IsReadDataReady = true;
			SetEndBulkXferOffset(BlockOffset);
			SetEndBulkXferCRC32(Crc32);
		}

		public void ResetNbData()
		{
			NbData = 0;
			IsReadDataReady = false;
		}

		public BLOCKPropertyValue GetPropertyValue(byte Property)
		{
			return PropertyValues[Property];
		}

		public void SetPropertyValue(byte Property, BLOCKPropertyValue Param)
		{
			PropertyValues[Property] = Param;
		}

		public static implicit operator BLOCK_ID(DeviceBLOCK value)
		{
			return value.ID;
		}

		public bool RequestRead()
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed)
			{
				return false;
			}
			if (!Device.Adapter.LocalHost.IsOnline)
			{
				return false;
			}
			if (!Device.IsOnline)
			{
				return false;
			}
			return Device.Adapter.LocalHost.Transmit29((byte)128, 34, Device, PAYLOAD.FromArgs(new object[1] { BLOCK_ID.op_Implicit(ID) }));
		}

		public bool RequestRead(byte Property)
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed)
			{
				return false;
			}
			if (!Device.Adapter.LocalHost.IsOnline)
			{
				return false;
			}
			if (!Device.IsOnline)
			{
				return false;
			}
			return Device.Adapter.LocalHost.Transmit29((byte)128, 33, Device, PAYLOAD.FromArgs(new object[2]
			{
				BLOCK_ID.op_Implicit(ID),
				Property
			}));
		}

		public bool StartReadData(uint Offset, byte Size_Msg, byte DelayMs)
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed)
			{
				return false;
			}
			if (!Device.Adapter.LocalHost.IsOnline)
			{
				return false;
			}
			if (!Device.IsOnline)
			{
				return false;
			}
			return Device.Adapter.LocalHost.Transmit29((byte)128, 34, Device, PAYLOAD.FromArgs(new object[4]
			{
				BLOCK_ID.op_Implicit(ID),
				Offset,
				Size_Msg,
				DelayMs
			}));
		}

		public void SetReadWriteBuffer(ulong capacity)
		{
			Data = new byte[capacity];
		}

		public void UpdateDataBuffer(IMessage message)
		{
			if (!((Disposable)this).IsDisposed && Data != null)
			{
				for (int i = 0; i < ((IByteList)message).Length; i++)
				{
					Data[NbData++] = ((System.Collections.Generic.IReadOnlyList<byte>)message)[i];
				}
			}
		}

		public void OnMessagePropertyRx(IMessage message)
		{
			if (((Disposable)this).IsDisposed)
			{
				return;
			}
			byte b = ((System.Collections.Generic.IReadOnlyList<byte>)message)[2];
			ulong num = 0uL;
			for (int i = 3; i < ((IByteList)message).Length; i++)
			{
				num <<= 8;
				num += ((System.Collections.Generic.IReadOnlyList<byte>)message)[i];
			}
			PropertyValues[b].IsValueValid = true;
			PropertyValues[b].PropertyValue = num;
			if (ReadSignals != null)
			{
				AsyncSignal asyncSignal = default(AsyncSignal);
				while (ReadSignals.TryDequeue(ref asyncSignal))
				{
					asyncSignal.OperationComplete = true;
				}
			}
		}

		public void OnMessageStartReadDataRx(uint blockoffset, ushort bulktransfersize)
		{
			if (((Disposable)this).IsDisposed)
			{
				return;
			}
			Setblockoffset(blockoffset);
			Setactualbulktransfersize(bulktransfersize);
			ValueValid = true;
			if (ReadSignals != null)
			{
				AsyncSignal asyncSignal = default(AsyncSignal);
				while (ReadSignals.TryDequeue(ref asyncSignal))
				{
					asyncSignal.OperationComplete = true;
				}
			}
		}

		[AsyncStateMachine(typeof(<ReadPropertyAsync>d__72))]
		public async System.Threading.Tasks.Task<BLOCKPropertyValue> ReadPropertyAsync(byte Property, AsyncOperation operation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			operation.ReportProgress(0f, "Reading...");
			BLOCKPropertyValue TmpReturVal = new BLOCKPropertyValue(0uL, Isvaluevalid: false);
			SetPropertyValue(Property, TmpReturVal);
			AsyncSignal signal = new AsyncSignal();
			if (ReadSignals == null)
			{
				lock (Lock)
				{
					if (ReadSignals == null)
					{
						ReadSignals = new ConcurrentQueue<AsyncSignal>();
					}
				}
			}
			ReadSignals.Enqueue(signal);
			Timer tx_timer = new Timer(true);
			Timer progress_timer = new Timer(true);
			tx_timer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			progress_timer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			bool skip = true;
			TimeSpan ReadRequestTime = TimeSpan.FromMilliseconds(330.0);
			TimeSpan ReportProgressTime = TimeSpan.FromMilliseconds(250.0);
			while (!signal.OperationComplete)
			{
				operation.ThrowIfCancellationRequested();
				if (!skip)
				{
					await System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
				}
				else
				{
					skip = false;
				}
				if (((Disposable)this).IsDisposed)
				{
					operation.ReportProgress("ReadAsync failed: BLOCK is disposed");
					return TmpReturVal;
				}
				if (!Device.IsOnline)
				{
					operation.ReportProgress("ReadAsync failed: Device is offline");
					return TmpReturVal;
				}
				if (tx_timer.ElapsedTime >= ReadRequestTime && RequestRead(Property))
				{
					tx_timer.Reset();
				}
				if (progress_timer.ElapsedTime > ReportProgressTime)
				{
					progress_timer.Reset();
					operation.ReportProgress(0f, "Reading...");
				}
			}
			operation.ReportProgress(100f, "Success!");
			return GetPropertyValue(Property);
		}

		[AsyncStateMachine(typeof(<StartReadData>d__73))]
		public async System.Threading.Tasks.Task<bool> StartReadData(uint Offset, byte Size_Msg, byte DelayMs, AsyncOperation operation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			operation.ReportProgress(0f, "Reading...");
			AsyncSignal signal = new AsyncSignal();
			if (ReadSignals == null)
			{
				lock (Lock)
				{
					if (ReadSignals == null)
					{
						ReadSignals = new ConcurrentQueue<AsyncSignal>();
					}
				}
			}
			ReadSignals.Enqueue(signal);
			Timer tx_timer = new Timer(true);
			Timer progress_timer = new Timer(true);
			tx_timer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			progress_timer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			bool skip = true;
			TimeSpan ReadRequestTime = TimeSpan.FromMilliseconds(330.0);
			TimeSpan ReportProgressTime = TimeSpan.FromMilliseconds(250.0);
			while (!signal.OperationComplete)
			{
				operation.ThrowIfCancellationRequested();
				if (!skip)
				{
					await System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
				}
				else
				{
					skip = false;
				}
				if (((Disposable)this).IsDisposed)
				{
					operation.ReportProgress("ReadAsync failed: BLOCK is disposed");
					return false;
				}
				if (!Device.IsOnline)
				{
					operation.ReportProgress("ReadAsync failed: Device is offline");
					return false;
				}
				if (tx_timer.ElapsedTime >= ReadRequestTime && StartReadData(Offset, Size_Msg, DelayMs))
				{
					tx_timer.Reset();
				}
				if (progress_timer.ElapsedTime > ReportProgressTime)
				{
					progress_timer.Reset();
					operation.ReportProgress(0f, "Reading...");
				}
			}
			operation.ReportProgress(100f, "Success!");
			return true;
		}

		[AsyncStateMachine(typeof(<ReadDataBufferReadyAsync>d__74))]
		public async System.Threading.Tasks.Task<bool> ReadDataBufferReadyAsync(AsyncOperation operation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			operation.ReportProgress(0f, "Reading...");
			AsyncSignal signal = new AsyncSignal();
			if (ReadSignals == null)
			{
				lock (Lock)
				{
					if (ReadSignals == null)
					{
						ReadSignals = new ConcurrentQueue<AsyncSignal>();
					}
				}
			}
			ReadSignals.Enqueue(signal);
			Timer tx_timer = new Timer(true);
			Timer progress_timer = new Timer(true);
			tx_timer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			progress_timer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			bool skip = true;
			TimeSpan ReadRequestTime = TimeSpan.FromMilliseconds(330.0);
			TimeSpan ReportProgressTime = TimeSpan.FromMilliseconds(250.0);
			while (!signal.OperationComplete)
			{
				operation.ThrowIfCancellationRequested();
				if (!skip)
				{
					await System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
				}
				else
				{
					skip = false;
				}
				if (((Disposable)this).IsDisposed)
				{
					operation.ReportProgress("ReadAsync failed: BLOCK is disposed");
					return false;
				}
				if (!Device.IsOnline)
				{
					operation.ReportProgress("ReadAsync failed: Device is offline");
					return false;
				}
				if (tx_timer.ElapsedTime >= ReadRequestTime && IsReadDataReady)
				{
					signal.OperationComplete = true;
					tx_timer.Reset();
				}
				if (progress_timer.ElapsedTime > ReportProgressTime)
				{
					progress_timer.Reset();
					operation.ReportProgress(0f, "Reading...");
				}
			}
			operation.ReportProgress(100f, "Success!");
			return true;
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((System.IDisposable)Events)?.Dispose();
				ReadSignals = null;
			}
		}
	}
	public struct BLOCKValue
	{
		[field: CompilerGenerated]
		public IRemoteDevice Device
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public BLOCK_ID ID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public bool IsValueValid
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public byte PropertyReadWrite
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ushort PropertySessionRead
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ushort PropertySessionWrite
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ulong PropertyBlockCapacity
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ulong PropertyCurrentBlockSize
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public uint PropertyCRC32
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public uint PropertyCRC32Verify
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ulong PropertySetAddress
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public uint BlockOffset
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ushort ActualBulkTransferSize
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ushort EndBulkXferOffset
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public uint EndBulkXferCRC32
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public byte Response
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public byte[] BlockData
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		internal BLOCKValue(IRemoteDevice device, BLOCK_ID id, bool Isvaluevalid, byte propertyreadwrite, ushort propertysessionread, ushort propertysessionwrite, ulong propertyblockcapacity, ulong propertycurrentblocksize, uint propertyCRC32, uint propertyCRC32verify, ulong propertySetAddress, uint blockoffset, ushort actualbulktransfersize, ushort endbulkxferoffset, uint endbulkxfercrc32, byte response, byte[] blockdata)
		{
			Device = device;
			ID = id;
			IsValueValid = Isvaluevalid;
			PropertyReadWrite = propertyreadwrite;
			PropertySessionRead = propertysessionread;
			PropertySessionWrite = propertysessionwrite;
			PropertyBlockCapacity = propertyblockcapacity;
			PropertyCurrentBlockSize = propertycurrentblocksize;
			PropertyCRC32 = propertyCRC32;
			PropertyCRC32Verify = propertyCRC32verify;
			PropertySetAddress = propertySetAddress;
			BlockOffset = blockoffset;
			ActualBulkTransferSize = actualbulktransfersize;
			EndBulkXferOffset = endbulkxferoffset;
			EndBulkXferCRC32 = endbulkxfercrc32;
			Response = response;
			BlockData = blockdata;
		}
	}
	public interface IBLOCKManager : System.Collections.Generic.IEnumerable<IDeviceBLOCK>, System.Collections.IEnumerable
	{
		IRemoteDevice Device { get; }

		int Count { get; }

		bool DeviceQueryComplete { get; }

		void QueryDevice();

		bool Contains(BLOCK_ID id);

		IDeviceBLOCK GetBLOCK(BLOCK_ID id);

		System.Threading.Tasks.Task<BLOCKValue> ReadPropertyAsync(BLOCK_ID id, AsyncOperation operation);

		System.Threading.Tasks.Task<BLOCKValue> StartReadData(BLOCK_ID id, uint Offset, byte Size_Msg, byte DelayMs, AsyncOperation operation);

		System.Threading.Tasks.Task<BLOCKValue> ReadDataBufferReadyAsync(BLOCK_ID id, AsyncOperation operation);
	}
	public struct BLOCKPropertyValue
	{
		[field: CompilerGenerated]
		public ulong PropertyValue
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public bool IsValueValid
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		internal BLOCKPropertyValue(ulong Propertyvalue, bool Isvaluevalid)
		{
			PropertyValue = Propertyvalue;
			IsValueValid = Isvaluevalid;
		}
	}
	[DefaultMember("Item")]
	internal class BLOCKManager : RemoteDevice.Child, IBLOCKManager, System.Collections.Generic.IEnumerable<IDeviceBLOCK>, System.Collections.IEnumerable
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ReadDataBufferReadyAsync>d__20 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<BLOCKValue> <>t__builder;

			public BLOCKManager <>4__this;

			public BLOCK_ID id;

			public AsyncOperation operation;

			private DeviceBLOCK <block>5__2;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				BLOCKManager bLOCKManager = <>4__this;
				BLOCKValue result;
				try
				{
					TaskAwaiter<bool> val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<bool>);
						num = (<>1__state = -1);
						goto IL_00d7;
					}
					byte[] blockdata = new byte[0];
					if (bLOCKManager.Device != null && bLOCKManager.Device.IsOnline && id != BLOCK_ID.UNKNOWN && bLOCKManager.Contains(id))
					{
						<block>5__2 = bLOCKManager[id];
						<block>5__2.SetState(3);
						val = <block>5__2.ReadDataBufferReadyAsync(operation).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <ReadDataBufferReadyAsync>d__20>(ref val, ref this);
							return;
						}
						goto IL_00d7;
					}
					result = new BLOCKValue(bLOCKManager.Device, id, Isvaluevalid: false, 0, 0, 0, 0uL, 0uL, 0u, 0u, 0uL, 0u, 0, 0, 0u, 0, blockdata);
					goto end_IL_000e;
					IL_00d7:
					bool flag = val.GetResult();
					if (flag)
					{
						System.Array.Resize<byte>(ref <block>5__2.Data, <block>5__2.NbData);
						uint num2 = 0u;
						if (<block>5__2.Data.Length != 0)
						{
							num2 = CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyCollection<byte>)(object)<block>5__2.Data);
						}
						if (num2 != <block>5__2.EndBulkXferCRC32)
						{
							<block>5__2.SetResponse(18);
							flag = false;
						}
					}
					<block>5__2.SetState(0);
					result = new BLOCKValue(bLOCKManager.Device, id, flag, (byte)<block>5__2.PropertyValues[0].PropertyValue, (ushort)<block>5__2.PropertyValues[1].PropertyValue, (ushort)<block>5__2.PropertyValues[2].PropertyValue, <block>5__2.PropertyValues[3].PropertyValue, <block>5__2.PropertyValues[4].PropertyValue, (uint)<block>5__2.PropertyValues[5].PropertyValue, (uint)<block>5__2.PropertyValues[6].PropertyValue, (uint)<block>5__2.PropertyValues[7].PropertyValue, <block>5__2.blockoffset, <block>5__2.actualbulktransfersize, <block>5__2.EndBulkXferOffset, <block>5__2.EndBulkXferCRC32, <block>5__2.Response, <block>5__2.Data);
					end_IL_000e:;
				}
				catch (System.Exception exception)
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
		private struct <ReadPropertyAsync>d__18 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<BLOCKValue> <>t__builder;

			public BLOCKManager <>4__this;

			public BLOCK_ID id;

			public AsyncOperation operation;

			private byte[] <EmptyBuf>5__2;

			private DeviceBLOCK <block>5__3;

			private BLOCKValue <TmpBLOCKValue>5__4;

			private int <TmpIndex>5__5;

			private TaskAwaiter<BLOCKPropertyValue> <>u__1;

			private void MoveNext()
			{
				//IL_013a: Unknown result type (might be due to invalid IL or missing references)
				//IL_013f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0147: Unknown result type (might be due to invalid IL or missing references)
				//IL_0105: Unknown result type (might be due to invalid IL or missing references)
				//IL_010a: Unknown result type (might be due to invalid IL or missing references)
				//IL_011f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0121: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				BLOCKManager bLOCKManager = <>4__this;
				BLOCKValue result;
				try
				{
					BLOCKPropertyValue bLOCKPropertyValue;
					if (num != 0)
					{
						<EmptyBuf>5__2 = new byte[0];
						if (bLOCKManager.Device != null && bLOCKManager.Device.IsOnline)
						{
							<block>5__3 = bLOCKManager[id];
							if (bLOCKManager[id] != null)
							{
								<block>5__3.SetState(1);
								bLOCKPropertyValue = new BLOCKPropertyValue(0uL, Isvaluevalid: false);
								<TmpBLOCKValue>5__4 = default(BLOCKValue);
								<TmpBLOCKValue>5__4.Device = bLOCKManager.Device;
								<TmpBLOCKValue>5__4.ID = id;
								<TmpBLOCKValue>5__4.IsValueValid = false;
								<TmpBLOCKValue>5__4.EndBulkXferOffset = 0;
								<TmpBLOCKValue>5__4.EndBulkXferCRC32 = 0u;
								<TmpIndex>5__5 = 0;
								goto IL_0272;
							}
							goto IL_0296;
						}
						goto IL_029d;
					}
					TaskAwaiter<BLOCKPropertyValue> val = <>u__1;
					<>u__1 = default(TaskAwaiter<BLOCKPropertyValue>);
					num = (<>1__state = -1);
					goto IL_0156;
					IL_0272:
					if (<TmpIndex>5__5 < 7)
					{
						bLOCKPropertyValue.IsValueValid = false;
						bLOCKPropertyValue.PropertyValue = 0uL;
						val = <block>5__3.ReadPropertyAsync(bLOCKManager.property[<TmpIndex>5__5], operation).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<BLOCKPropertyValue>, <ReadPropertyAsync>d__18>(ref val, ref this);
							return;
						}
						goto IL_0156;
					}
					<block>5__3.SetState(0);
					<TmpBLOCKValue>5__4 = default(BLOCKValue);
					goto IL_0296;
					IL_029d:
					result = new BLOCKValue(bLOCKManager.Device, id, Isvaluevalid: false, 0, 0, 0, 0uL, 0uL, 0u, 0u, 0uL, 0u, 0, 0, 0u, 0, <EmptyBuf>5__2);
					goto end_IL_000e;
					IL_0296:
					<block>5__3 = null;
					goto IL_029d;
					IL_0156:
					bLOCKPropertyValue = val.GetResult();
					<TmpBLOCKValue>5__4.Response = <block>5__3.Response;
					if (bLOCKPropertyValue.IsValueValid)
					{
						ulong propertyValue = bLOCKPropertyValue.PropertyValue;
						switch (<TmpIndex>5__5)
						{
						case 0:
							<TmpBLOCKValue>5__4.PropertyReadWrite = (byte)propertyValue;
							break;
						case 1:
							<TmpBLOCKValue>5__4.PropertySessionRead = (ushort)propertyValue;
							break;
						case 2:
							<TmpBLOCKValue>5__4.PropertySessionWrite = (ushort)propertyValue;
							break;
						case 3:
							<TmpBLOCKValue>5__4.PropertyBlockCapacity = propertyValue;
							break;
						case 4:
							<TmpBLOCKValue>5__4.PropertyCurrentBlockSize = propertyValue;
							break;
						case 5:
							<TmpBLOCKValue>5__4.PropertyCRC32 = (uint)propertyValue;
							break;
						case 6:
							<TmpBLOCKValue>5__4.PropertyCRC32Verify = (uint)propertyValue;
							<TmpBLOCKValue>5__4.IsValueValid = true;
							<TmpBLOCKValue>5__4.BlockData = <EmptyBuf>5__2;
							<block>5__3.SetState(0);
							result = <TmpBLOCKValue>5__4;
							goto end_IL_000e;
						}
					}
					<TmpIndex>5__5++;
					goto IL_0272;
					end_IL_000e:;
				}
				catch (System.Exception exception)
				{
					<>1__state = -2;
					<EmptyBuf>5__2 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<EmptyBuf>5__2 = null;
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
		private struct <StartReadData>d__19 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<BLOCKValue> <>t__builder;

			public BLOCKManager <>4__this;

			public BLOCK_ID id;

			public uint Offset;

			public byte Size_Msg;

			public byte DelayMs;

			public AsyncOperation operation;

			private DeviceBLOCK <block>5__2;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_010c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0111: Unknown result type (might be due to invalid IL or missing references)
				//IL_0119: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				BLOCKManager bLOCKManager = <>4__this;
				BLOCKValue result;
				try
				{
					TaskAwaiter<bool> val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<bool>);
						num = (<>1__state = -1);
						goto IL_0128;
					}
					byte[] blockdata = new byte[0];
					bool isvaluevalid;
					if (bLOCKManager.Device != null && bLOCKManager.Device.IsOnline && id != BLOCK_ID.UNKNOWN && bLOCKManager.Contains(id))
					{
						<block>5__2 = bLOCKManager[id];
						<block>5__2.SetState(2);
						isvaluevalid = false;
						if (<block>5__2.PropertyValues[4].PropertyValue != 0L)
						{
							<block>5__2.SetReadWriteBuffer(<block>5__2.PropertyValues[4].PropertyValue);
							val = <block>5__2.StartReadData(Offset, Size_Msg, DelayMs, operation).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <StartReadData>d__19>(ref val, ref this);
								return;
							}
							goto IL_0128;
						}
						goto IL_0131;
					}
					result = new BLOCKValue(bLOCKManager.Device, id, Isvaluevalid: false, 0, 0, 0, 0uL, 0uL, 0u, 0u, 0uL, 0u, 0, 0, 0u, 0, blockdata);
					goto end_IL_000e;
					IL_0128:
					isvaluevalid = val.GetResult();
					goto IL_0131;
					IL_0131:
					<block>5__2.SetState(0);
					result = new BLOCKValue(bLOCKManager.Device, id, isvaluevalid, (byte)<block>5__2.PropertyValues[0].PropertyValue, (ushort)<block>5__2.PropertyValues[1].PropertyValue, (ushort)<block>5__2.PropertyValues[2].PropertyValue, <block>5__2.PropertyValues[3].PropertyValue, <block>5__2.PropertyValues[4].PropertyValue, (uint)<block>5__2.PropertyValues[5].PropertyValue, (uint)<block>5__2.PropertyValues[6].PropertyValue, (uint)<block>5__2.PropertyValues[7].PropertyValue, <block>5__2.blockoffset, <block>5__2.actualbulktransfersize, <block>5__2.EndBulkXferOffset, <block>5__2.EndBulkXferCRC32, <block>5__2.Response, <block>5__2.Data);
					end_IL_000e:;
				}
				catch (System.Exception exception)
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

		private static readonly List<DeviceBLOCK> EmptyList = new List<DeviceBLOCK>();

		private static readonly TimeSpan READ_LIST_TX_RETRY_TIME = TimeSpan.FromMilliseconds(500.0);

		private readonly Dictionary<BLOCK_ID, DeviceBLOCK> Dictionary = new Dictionary<BLOCK_ID, DeviceBLOCK>();

		private readonly Dictionary<BLOCK_ID, DeviceBLOCK> HiddenBLOCKs = new Dictionary<BLOCK_ID, DeviceBLOCK>();

		private readonly List<DeviceBLOCK> SortedList = new List<DeviceBLOCK>();

		private readonly Timer TxTimer = new Timer(true);

		private bool NeedsRead = true;

		private bool ReadListFromDevice;

		private ushort BlockIndex;

		private ushort ReportedCount;

		private BLOCK_ID BlockIdRef = BLOCK_ID.UNKNOWN;

		private byte[] property;

		public int Count
		{
			get
			{
				if (NeedsRead)
				{
					return 0;
				}
				return SortedList.Count;
			}
		}

		public bool DeviceQueryComplete => !NeedsRead;

		private DeviceBLOCK this[BLOCK_ID id]
		{
			get
			{
				DeviceBLOCK result = default(DeviceBLOCK);
				Dictionary.TryGetValue(id, ref result);
				return result;
			}
		}

		public BLOCKManager(RemoteDevice device)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			byte[] array = new byte[8];
			RuntimeHelpers.InitializeArray((System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			property = array;
			base..ctor(device);
			Clear();
			((IEventSender)base.Adapter).Events.Subscribe<TransmitTurnEvent>((Action<TransmitTurnEvent>)OnTransmitNextMessage, (SubscriptionType)0, Subscriptions);
			ReadListFromDevice = ((System.Enum)base.Adapter.Options).HasFlag((System.Enum)ADAPTER_OPTIONS.AUTO_READ_DEVICE_BLOCK_LIST);
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Clear();
			}
		}

		public System.Collections.Generic.IEnumerator<IDeviceBLOCK> GetEnumerator()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (NeedsRead)
			{
				return (System.Collections.Generic.IEnumerator<IDeviceBLOCK>)(object)EmptyList.GetEnumerator();
			}
			return (System.Collections.Generic.IEnumerator<IDeviceBLOCK>)(object)SortedList.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator();
		}

		[AsyncStateMachine(typeof(<ReadPropertyAsync>d__18))]
		public async System.Threading.Tasks.Task<BLOCKValue> ReadPropertyAsync(BLOCK_ID id, AsyncOperation operation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			byte[] EmptyBuf = new byte[0];
			if (base.Device != null && base.Device.IsOnline)
			{
				DeviceBLOCK block = this[id];
				if (this[id] != null)
				{
					block.SetState(1);
					BLOCKPropertyValue bLOCKPropertyValue = new BLOCKPropertyValue(0uL, Isvaluevalid: false);
					BLOCKValue TmpBLOCKValue = new BLOCKValue
					{
						Device = base.Device,
						ID = id,
						IsValueValid = false,
						EndBulkXferOffset = 0,
						EndBulkXferCRC32 = 0u
					};
					for (int TmpIndex = 0; TmpIndex < 7; TmpIndex++)
					{
						bLOCKPropertyValue.IsValueValid = false;
						bLOCKPropertyValue.PropertyValue = 0uL;
						bLOCKPropertyValue = await block.ReadPropertyAsync(property[TmpIndex], operation);
						TmpBLOCKValue.Response = block.Response;
						if (bLOCKPropertyValue.IsValueValid)
						{
							ulong propertyValue = bLOCKPropertyValue.PropertyValue;
							switch (TmpIndex)
							{
							case 0:
								TmpBLOCKValue.PropertyReadWrite = (byte)propertyValue;
								break;
							case 1:
								TmpBLOCKValue.PropertySessionRead = (ushort)propertyValue;
								break;
							case 2:
								TmpBLOCKValue.PropertySessionWrite = (ushort)propertyValue;
								break;
							case 3:
								TmpBLOCKValue.PropertyBlockCapacity = propertyValue;
								break;
							case 4:
								TmpBLOCKValue.PropertyCurrentBlockSize = propertyValue;
								break;
							case 5:
								TmpBLOCKValue.PropertyCRC32 = (uint)propertyValue;
								break;
							case 6:
								TmpBLOCKValue.PropertyCRC32Verify = (uint)propertyValue;
								TmpBLOCKValue.IsValueValid = true;
								TmpBLOCKValue.BlockData = EmptyBuf;
								block.SetState(0);
								return TmpBLOCKValue;
							}
						}
					}
					block.SetState(0);
				}
			}
			return new BLOCKValue(base.Device, id, Isvaluevalid: false, 0, 0, 0, 0uL, 0uL, 0u, 0u, 0uL, 0u, 0, 0, 0u, 0, EmptyBuf);
		}

		[AsyncStateMachine(typeof(<StartReadData>d__19))]
		public async System.Threading.Tasks.Task<BLOCKValue> StartReadData(BLOCK_ID id, uint Offset, byte Size_Msg, byte DelayMs, AsyncOperation operation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			byte[] blockdata = new byte[0];
			if (base.Device != null && base.Device.IsOnline && id != BLOCK_ID.UNKNOWN && Contains(id))
			{
				DeviceBLOCK block = this[id];
				block.SetState(2);
				bool isvaluevalid = false;
				if (block.PropertyValues[4].PropertyValue != 0L)
				{
					block.SetReadWriteBuffer(block.PropertyValues[4].PropertyValue);
					isvaluevalid = await block.StartReadData(Offset, Size_Msg, DelayMs, operation);
				}
				block.SetState(0);
				return new BLOCKValue(base.Device, id, isvaluevalid, (byte)block.PropertyValues[0].PropertyValue, (ushort)block.PropertyValues[1].PropertyValue, (ushort)block.PropertyValues[2].PropertyValue, block.PropertyValues[3].PropertyValue, block.PropertyValues[4].PropertyValue, (uint)block.PropertyValues[5].PropertyValue, (uint)block.PropertyValues[6].PropertyValue, (uint)block.PropertyValues[7].PropertyValue, block.blockoffset, block.actualbulktransfersize, block.EndBulkXferOffset, block.EndBulkXferCRC32, block.Response, block.Data);
			}
			return new BLOCKValue(base.Device, id, Isvaluevalid: false, 0, 0, 0, 0uL, 0uL, 0u, 0u, 0uL, 0u, 0, 0, 0u, 0, blockdata);
		}

		[AsyncStateMachine(typeof(<ReadDataBufferReadyAsync>d__20))]
		public async System.Threading.Tasks.Task<BLOCKValue> ReadDataBufferReadyAsync(BLOCK_ID id, AsyncOperation operation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			byte[] blockdata = new byte[0];
			if (base.Device != null && base.Device.IsOnline && id != BLOCK_ID.UNKNOWN && Contains(id))
			{
				DeviceBLOCK block = this[id];
				block.SetState(3);
				bool flag = await block.ReadDataBufferReadyAsync(operation);
				if (flag)
				{
					System.Array.Resize<byte>(ref block.Data, block.NbData);
					uint num = 0u;
					if (block.Data.Length != 0)
					{
						num = CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyCollection<byte>)(object)block.Data);
					}
					if (num != block.EndBulkXferCRC32)
					{
						block.SetResponse(18);
						flag = false;
					}
				}
				block.SetState(0);
				return new BLOCKValue(base.Device, id, flag, (byte)block.PropertyValues[0].PropertyValue, (ushort)block.PropertyValues[1].PropertyValue, (ushort)block.PropertyValues[2].PropertyValue, block.PropertyValues[3].PropertyValue, block.PropertyValues[4].PropertyValue, (uint)block.PropertyValues[5].PropertyValue, (uint)block.PropertyValues[6].PropertyValue, (uint)block.PropertyValues[7].PropertyValue, block.blockoffset, block.actualbulktransfersize, block.EndBulkXferOffset, block.EndBulkXferCRC32, block.Response, block.Data);
			}
			return new BLOCKValue(base.Device, id, Isvaluevalid: false, 0, 0, 0, 0uL, 0uL, 0u, 0u, 0uL, 0u, 0, 0, 0u, 0, blockdata);
		}

		public void QueryDevice()
		{
			if (NeedsRead)
			{
				ReadListFromDevice = true;
			}
		}

		public bool Contains(BLOCK_ID id)
		{
			return this[id] != null;
		}

		public IDeviceBLOCK GetBLOCK(BLOCK_ID id)
		{
			return this[id];
		}

		public bool IsBLOCKSupported(BLOCK_ID id)
		{
			return Dictionary.ContainsKey(id);
		}

		private void Clear()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			TxTimer.ElapsedTime = TimeSpan.FromSeconds(-0.25);
			BlockIndex = 0;
			SortedList.Clear();
			Enumerator<BLOCK_ID, DeviceBLOCK> enumerator = Dictionary.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					((Disposable)enumerator.Current).Dispose();
				}
			}
			finally
			{
				((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
			Dictionary.Clear();
			enumerator = HiddenBLOCKs.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					((Disposable)enumerator.Current).Dispose();
				}
			}
			finally
			{
				((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
			HiddenBLOCKs.Clear();
		}

		public override void BackgroundTask()
		{
		}

		public override void OnDeviceTx(AdapterRxEvent tx)
		{
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			if (!base.Device.IsOnline || tx.TargetAddress != base.Adapter.LocalHost.Address)
			{
				return;
			}
			switch (tx.MessageType)
			{
			case 159:
			{
				if (BlockIdRef == BLOCK_ID.UNKNOWN)
				{
					break;
				}
				DeviceBLOCK deviceBLOCK5 = this[BlockIdRef];
				if (deviceBLOCK5 != null)
				{
					_ = deviceBLOCK5.State;
					if (0 == 0 && this[BlockIdRef]?.State != 0)
					{
						_ = tx.MessageData;
						this[BlockIdRef]?.UpdateDataBuffer((IMessage)(object)tx);
					}
				}
				break;
			}
			case 129:
				switch (tx.MessageData)
				{
				case 32:
				{
					if (!ReadListFromDevice || tx.Count != 8 || CommExtensions.GetUINT16((IByteList)(object)tx, 0) != BlockIndex)
					{
						break;
					}
					int num = 2;
					int i = BlockIndex * 3;
					if (BlockIndex == 0)
					{
						ReportedCount = CommExtensions.GetUINT16((IByteList)(object)tx, num);
						SortedList.Clear();
						Dictionary.Clear();
						num += 2;
					}
					else
					{
						i--;
					}
					for (; i < ReportedCount; i++)
					{
						if (num >= 8)
						{
							break;
						}
						BLOCK_ID val = BLOCK_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)tx, num));
						if (val != BLOCK_ID.UNKNOWN)
						{
							if (Dictionary.ContainsKey(val))
							{
								Clear();
								return;
							}
							DeviceBLOCK deviceBLOCK3 = new DeviceBLOCK(base.Device, val, Init: false);
							SortedList.Add(deviceBLOCK3);
							Dictionary.Add(val, deviceBLOCK3);
						}
						num += 2;
					}
					if (i >= ReportedCount)
					{
						SortedList.Sort((Comparison<DeviceBLOCK>)((DeviceBLOCK first, DeviceBLOCK second) => first.ID.Value.CompareTo(second.ID.Value)));
						NeedsRead = (ReadListFromDevice = false);
					}
					BlockIndex++;
					TxTimer.ElapsedTime = TimeSpan.FromSeconds(1.0);
					break;
				}
				case 33:
				{
					ushort uINT7 = CommExtensions.GetUINT16((IByteList)(object)tx, 0);
					DeviceBLOCK deviceBLOCK4 = this[BLOCK_ID.op_Implicit(uINT7)];
					if (deviceBLOCK4 == null)
					{
						break;
					}
					_ = deviceBLOCK4.State;
					if (0 == 0 && this[BLOCK_ID.op_Implicit(uINT7)]?.State != 0)
					{
						if (tx.Count == 8)
						{
							this[BLOCK_ID.op_Implicit(uINT7)]?.OnMessagePropertyRx((IMessage)(object)tx);
						}
						else
						{
							this[BLOCK_ID.op_Implicit(uINT7)]?.SetResponse(CommExtensions.GetUINT8((IByteList)(object)tx, 3));
						}
					}
					break;
				}
				case 34:
				{
					ushort uINT4 = CommExtensions.GetUINT16((IByteList)(object)tx, 0);
					DeviceBLOCK deviceBLOCK2 = this[BLOCK_ID.op_Implicit(uINT4)];
					if (deviceBLOCK2 == null)
					{
						break;
					}
					_ = deviceBLOCK2.State;
					if (0 == 0 && this[BLOCK_ID.op_Implicit(uINT4)]?.State != 0)
					{
						if (tx.Count == 8)
						{
							uint uINT5 = CommExtensions.GetUINT32((IByteList)(object)tx, 2);
							ushort uINT6 = CommExtensions.GetUINT16((IByteList)(object)tx, 6);
							this[BLOCK_ID.op_Implicit(uINT4)]?.OnMessageStartReadDataRx(uINT5, uINT6);
							this[BLOCK_ID.op_Implicit(uINT4)]?.ResetNbData();
							BlockIdRef = BLOCK_ID.op_Implicit(uINT4);
						}
						else
						{
							this[BLOCK_ID.op_Implicit(uINT4)]?.SetResponse(CommExtensions.GetUINT8((IByteList)(object)tx, 6));
						}
					}
					break;
				}
				case 37:
				{
					ushort uINT = CommExtensions.GetUINT16((IByteList)(object)tx, 0);
					DeviceBLOCK deviceBLOCK = this[BLOCK_ID.op_Implicit(uINT)];
					if (deviceBLOCK == null)
					{
						break;
					}
					_ = deviceBLOCK.State;
					if (0 == 0 && this[BLOCK_ID.op_Implicit(uINT)]?.State != 0)
					{
						if (tx.Count == 8)
						{
							ushort uINT2 = CommExtensions.GetUINT16((IByteList)(object)tx, 2);
							uint uINT3 = CommExtensions.GetUINT32((IByteList)(object)tx, 4);
							this[BLOCK_ID.op_Implicit(uINT)]?.OnMessageEndBulkXferRx(uINT2, uINT3);
						}
						else
						{
							this[BLOCK_ID.op_Implicit(uINT)]?.SetResponse(CommExtensions.GetUINT8((IByteList)(object)tx, 4));
						}
						BlockIdRef = BLOCK_ID.UNKNOWN;
					}
					break;
				}
				}
				break;
			}
		}

		private void OnTransmitNextMessage(TransmitTurnEvent message)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			if (ReadListFromDevice && base.Adapter.LocalHost.IsOnline && base.Device.IsOnline && !(TxTimer.ElapsedTime < READ_LIST_TX_RETRY_TIME))
			{
				message.Handled = base.Adapter.LocalHost.Transmit29((byte)128, 32, base.Device, PAYLOAD.FromArgs(new object[1] { BlockIndex }));
				if (message.Handled)
				{
					TxTimer.Reset();
				}
			}
		}
	}
	public interface ICircuit : System.Collections.Generic.IEnumerable<IRemoteDevice>, System.Collections.IEnumerable
	{
		CIRCUIT_ID CircuitID { get; }

		int DeviceCount { get; }

		bool IsEmpty { get; }
	}
	internal class Circuit : ICircuit, System.Collections.Generic.IEnumerable<IRemoteDevice>, System.Collections.IEnumerable
	{
		[CompilerGenerated]
		private sealed class <GetEnumerator>d__10 : System.Collections.Generic.IEnumerator<IRemoteDevice>, System.Collections.IEnumerator, System.IDisposable
		{
			private int <>1__state;

			private IRemoteDevice <>2__current;

			public Circuit <>4__this;

			private System.Collections.Generic.IEnumerator<IRemoteDevice> <>7__wrap1;

			IRemoteDevice System.Collections.Generic.IEnumerator<IRemoteDevice>.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			object System.Collections.IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			[DebuggerHidden]
			public <GetEnumerator>d__10(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}

			[DebuggerHidden]
			void System.IDisposable.Dispose()
			{
				int num = <>1__state;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						<>m__Finally1();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int num = <>1__state;
					Circuit circuit = <>4__this;
					switch (num)
					{
					default:
						return false;
					case 0:
						<>1__state = -1;
						<>7__wrap1 = circuit.Members.GetEnumerator();
						<>1__state = -3;
						break;
					case 1:
						<>1__state = -3;
						break;
					}
					while (((System.Collections.IEnumerator)<>7__wrap1).MoveNext())
					{
						IRemoteDevice current = <>7__wrap1.Current;
						if (current != null && current.IsOnline)
						{
							<>2__current = current;
							<>1__state = 1;
							return true;
						}
					}
					<>m__Finally1();
					<>7__wrap1 = null;
					return false;
				}
				catch
				{
					//try-fault
					((System.IDisposable)this).Dispose();
					throw;
				}
			}

			bool System.Collections.IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void <>m__Finally1()
			{
				<>1__state = -1;
				if (<>7__wrap1 != null)
				{
					((System.IDisposable)<>7__wrap1).Dispose();
				}
			}

			[DebuggerHidden]
			void System.Collections.IEnumerator.Reset()
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotSupportedException();
			}
		}

		private readonly ConcurrentHashSet<IRemoteDevice> Members = new ConcurrentHashSet<IRemoteDevice>();

		[field: CompilerGenerated]
		public CIRCUIT_ID CircuitID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public int DeviceCount => Members.Count;

		public bool IsEmpty => Members.Count == 0;

		public Circuit(CIRCUIT_ID circuit_id)
		{
			CircuitID = circuit_id;
		}

		[IteratorStateMachine(typeof(<GetEnumerator>d__10))]
		public System.Collections.Generic.IEnumerator<IRemoteDevice> GetEnumerator()
		{
			System.Collections.Generic.IEnumerator<IRemoteDevice> enumerator = Members.GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					IRemoteDevice current = enumerator.Current;
					if (current != null && current.IsOnline)
					{
						yield return current;
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator();
		}

		public bool Cleanup()
		{
			bool flag = false;
			System.Collections.Generic.IEnumerator<IRemoteDevice> enumerator = Members.GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					IRemoteDevice current = enumerator.Current;
					if (!current.IsOnline || (uint)current.CircuitID != (uint)CircuitID)
					{
						flag |= Members.Remove(current);
					}
				}
				return flag;
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		public bool AddDeviceToCircuit(IRemoteDevice device)
		{
			if ((uint?)device?.CircuitID != (uint)CircuitID)
			{
				return false;
			}
			if (Members.Contains(device))
			{
				return false;
			}
			Members.Add(device);
			return true;
		}
	}
	public interface ICircuitManager : System.Collections.Generic.IEnumerable<ICircuit>, System.Collections.IEnumerable
	{
		IAdapter Adapter { get; }

		int Count { get; }

		CIRCUIT_ID GetRandomUnusedCircuitID();
	}
	[DefaultMember("Item")]
	internal class CircuitManager : Adapter.BackgroundTaskObject, ICircuitManager, System.Collections.Generic.IEnumerable<ICircuit>, System.Collections.IEnumerable
	{
		private readonly ConcurrentDictionary<CIRCUIT_ID, Circuit> Circuits = new ConcurrentDictionary<CIRCUIT_ID, Circuit>();

		private readonly CircuitListChangedEvent CircuitListChangedEvent;

		public int Count => Circuits.Count;

		public Circuit this[CIRCUIT_ID id]
		{
			get
			{
				if (((Disposable)this).IsDisposed)
				{
					return null;
				}
				Circuit result = default(Circuit);
				Circuits.TryGetValue(id, ref result);
				return result;
			}
		}

		public int TotalDevices
		{
			get
			{
				int num = 0;
				System.Collections.Generic.IEnumerator<ICircuit> enumerator = GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						Circuit circuit = (Circuit)enumerator.Current;
						num += circuit.DeviceCount;
					}
					return num;
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
			}
		}

		public CircuitManager(Adapter adapter)
			: base(adapter)
		{
			CircuitListChangedEvent = new CircuitListChangedEvent(this);
			((IEventSender)base.Adapter).Events.Subscribe<AdapterOpenedEvent>((Action<AdapterOpenedEvent>)OnAdapterOpened, (SubscriptionType)1, base.Subscriptions);
			((IEventSender)base.Adapter).Events.Subscribe<AdapterClosedEvent>((Action<AdapterClosedEvent>)OnAdapterClosed, (SubscriptionType)1, base.Subscriptions);
			((IEventSender)base.Adapter).Events.Subscribe<RemoteDeviceOnlineEvent>((Action<RemoteDeviceOnlineEvent>)OnRemoteDeviceOnline, (SubscriptionType)1, base.Subscriptions);
			((IEventSender)base.Adapter).Events.Subscribe<RemoteDeviceOfflineEvent>((Action<RemoteDeviceOfflineEvent>)OnRemoteDeviceOffline, (SubscriptionType)1, base.Subscriptions);
			((IEventSender)base.Adapter).Events.Subscribe<CircuitIDChangedEvent>((Action<CircuitIDChangedEvent>)OnCircuitIDChanged, (SubscriptionType)1, base.Subscriptions);
		}

		private void PublishChangeEvent()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			System.Threading.Tasks.Task.Run((Action)([CompilerGenerated] () =>
			{
				((Event)CircuitListChangedEvent).Publish();
			}));
		}

		public System.Collections.Generic.IEnumerator<ICircuit> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerator<ICircuit>)((System.Collections.Generic.IEnumerable<Circuit>)Circuits.Values).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator();
		}

		private void OnAdapterOpened(AdapterOpenedEvent message)
		{
			Clear();
		}

		private void OnAdapterClosed(AdapterClosedEvent message)
		{
			Clear();
		}

		private void OnRemoteDeviceOnline(RemoteDeviceOnlineEvent message)
		{
			if (LocateOrCreateCircuit(message.Device.CircuitID).AddDeviceToCircuit(message.Device))
			{
				PublishChangeEvent();
			}
		}

		private void OnRemoteDeviceOffline(RemoteDeviceOfflineEvent message)
		{
			Circuit circuit = this[message.Device.CircuitID];
			if (circuit != null && circuit.Cleanup())
			{
				PublishChangeEvent();
			}
		}

		private void OnCircuitIDChanged(CircuitIDChangedEvent message)
		{
			bool? obj = this[message.Prev]?.Cleanup();
			if ((LocateOrCreateCircuit(message.Device.CircuitID).AddDeviceToCircuit(message.Device) | obj) == true)
			{
				PublishChangeEvent();
			}
		}

		private Circuit LocateOrCreateCircuit(CIRCUIT_ID id)
		{
			if (((Disposable)this).IsDisposed)
			{
				return null;
			}
			Circuit circuit = this[id];
			if (circuit != null)
			{
				return circuit;
			}
			return Circuits.GetOrAdd(id, (Func<CIRCUIT_ID, Circuit>)((CIRCUIT_ID k) => new Circuit(id)));
		}

		public bool ContainsCircuit(CIRCUIT_ID id)
		{
			return Circuits.ContainsKey(id);
		}

		public void Clear()
		{
			int count = Circuits.Count;
			if (count == 0)
			{
				return;
			}
			Circuits.Clear();
			if (!((Disposable)this).IsDisposed)
			{
				LocateOrCreateCircuit(0u);
				if (count != 1)
				{
					PublishChangeEvent();
				}
			}
		}

		public override void BackgroundTask()
		{
			if (((Disposable)this).IsDisposed)
			{
				return;
			}
			if (!((IAdapter)base.Adapter).IsConnected)
			{
				Clear();
				return;
			}
			bool flag = false;
			System.Collections.Generic.IEnumerator<Circuit> enumerator = ((System.Collections.Generic.IEnumerable<Circuit>)Circuits.Values).GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					Circuit current = enumerator.Current;
					flag |= current.Cleanup();
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			System.Collections.Generic.IEnumerator<IRemoteDevice> enumerator2 = ((System.Collections.Generic.IEnumerable<IRemoteDevice>)base.Adapter.Devices).GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator2).MoveNext())
				{
					IRemoteDevice current2 = enumerator2.Current;
					flag |= LocateOrCreateCircuit(current2.CircuitID).AddDeviceToCircuit(current2);
				}
			}
			finally
			{
				((System.IDisposable)enumerator2)?.Dispose();
			}
			enumerator = ((System.Collections.Generic.IEnumerable<Circuit>)Circuits.Values).GetEnumerator();
			try
			{
				Circuit circuit = default(Circuit);
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					Circuit current3 = enumerator.Current;
					if (current3.IsEmpty && (uint)current3.CircuitID != 0)
					{
						Circuits.TryRemove(current3.CircuitID, ref circuit);
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			if (flag)
			{
				PublishChangeEvent();
			}
		}

		public bool DoesCircuitExist(CIRCUIT_ID id)
		{
			return this[id] != null;
		}

		public CIRCUIT_ID GetRandomUnusedCircuitID()
		{
			CIRCUIT_ID cIRCUIT_ID;
			do
			{
				cIRCUIT_ID = (uint)(4294967295.0 * ThreadLocalRandom.NextDouble() + 1.0);
			}
			while ((uint)cIRCUIT_ID == 0 || DoesCircuitExist(cIRCUIT_ID));
			return cIRCUIT_ID;
		}

		public override string ToString()
		{
			string text = $"Network contains {TotalDevices} devices in {Count} circuits";
			System.Collections.Generic.IEnumerator<ICircuit> enumerator = GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					Circuit circuit = (Circuit)enumerator.Current;
					text += $"\n\tCircuit ID {circuit.CircuitID} >> ";
					int num = 0;
					System.Collections.Generic.IEnumerator<IRemoteDevice> enumerator2 = circuit.GetEnumerator();
					try
					{
						while (((System.Collections.IEnumerator)enumerator2).MoveNext())
						{
							IRemoteDevice current = enumerator2.Current;
							if (num++ > 0)
							{
								text += ", ";
							}
							text += current.ToShortString(show_address: false);
						}
					}
					finally
					{
						((System.IDisposable)enumerator2)?.Dispose();
					}
				}
				return text;
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((Disposable)base.Subscriptions).Dispose();
				Clear();
			}
		}
	}
	public sealed class ADDRESS
	{
		private static readonly ADDRESS[] Table = new ADDRESS[256];

		private static readonly List<ADDRESS> List = new List<ADDRESS>();

		public static readonly ADDRESS INVALID = new ADDRESS(null, "INVALID");

		public static readonly ADDRESS BROADCAST = new ADDRESS((byte)0, "BROADCAST");

		public static readonly ADDRESS ADDRESS_01 = new ADDRESS(1);

		public static readonly ADDRESS ADDRESS_02 = new ADDRESS(2);

		public static readonly ADDRESS ADDRESS_03 = new ADDRESS(3);

		public static readonly ADDRESS ADDRESS_04 = new ADDRESS(4);

		public static readonly ADDRESS ADDRESS_05 = new ADDRESS(5);

		public static readonly ADDRESS ADDRESS_06 = new ADDRESS(6);

		public static readonly ADDRESS ADDRESS_07 = new ADDRESS(7);

		public static readonly ADDRESS ADDRESS_08 = new ADDRESS(8);

		public static readonly ADDRESS ADDRESS_09 = new ADDRESS(9);

		public static readonly ADDRESS ADDRESS_0A = new ADDRESS(10);

		public static readonly ADDRESS ADDRESS_0B = new ADDRESS(11);

		public static readonly ADDRESS ADDRESS_0C = new ADDRESS(12);

		public static readonly ADDRESS ADDRESS_0D = new ADDRESS(13);

		public static readonly ADDRESS ADDRESS_0E = new ADDRESS(14);

		public static readonly ADDRESS ADDRESS_0F = new ADDRESS(15);

		public static readonly ADDRESS ADDRESS_10 = new ADDRESS(16);

		public static readonly ADDRESS ADDRESS_11 = new ADDRESS(17);

		public static readonly ADDRESS ADDRESS_12 = new ADDRESS(18);

		public static readonly ADDRESS ADDRESS_13 = new ADDRESS(19);

		public static readonly ADDRESS ADDRESS_14 = new ADDRESS(20);

		public static readonly ADDRESS ADDRESS_15 = new ADDRESS(21);

		public static readonly ADDRESS ADDRESS_16 = new ADDRESS(22);

		public static readonly ADDRESS ADDRESS_17 = new ADDRESS(23);

		public static readonly ADDRESS ADDRESS_18 = new ADDRESS(24);

		public static readonly ADDRESS ADDRESS_19 = new ADDRESS(25);

		public static readonly ADDRESS ADDRESS_1A = new ADDRESS(26);

		public static readonly ADDRESS ADDRESS_1B = new ADDRESS(27);

		public static readonly ADDRESS ADDRESS_1C = new ADDRESS(28);

		public static readonly ADDRESS ADDRESS_1D = new ADDRESS(29);

		public static readonly ADDRESS ADDRESS_1E = new ADDRESS(30);

		public static readonly ADDRESS ADDRESS_1F = new ADDRESS(31);

		public static readonly ADDRESS ADDRESS_20 = new ADDRESS(32);

		public static readonly ADDRESS ADDRESS_21 = new ADDRESS(33);

		public static readonly ADDRESS ADDRESS_22 = new ADDRESS(34);

		public static readonly ADDRESS ADDRESS_23 = new ADDRESS(35);

		public static readonly ADDRESS ADDRESS_24 = new ADDRESS(36);

		public static readonly ADDRESS ADDRESS_25 = new ADDRESS(37);

		public static readonly ADDRESS ADDRESS_26 = new ADDRESS(38);

		public static readonly ADDRESS ADDRESS_27 = new ADDRESS(39);

		public static readonly ADDRESS ADDRESS_28 = new ADDRESS(40);

		public static readonly ADDRESS ADDRESS_29 = new ADDRESS(41);

		public static readonly ADDRESS ADDRESS_2A = new ADDRESS(42);

		public static readonly ADDRESS ADDRESS_2B = new ADDRESS(43);

		public static readonly ADDRESS ADDRESS_2C = new ADDRESS(44);

		public static readonly ADDRESS ADDRESS_2D = new ADDRESS(45);

		public static readonly ADDRESS ADDRESS_2E = new ADDRESS(46);

		public static readonly ADDRESS ADDRESS_2F = new ADDRESS(47);

		public static readonly ADDRESS ADDRESS_30 = new ADDRESS(48);

		public static readonly ADDRESS ADDRESS_31 = new ADDRESS(49);

		public static readonly ADDRESS ADDRESS_32 = new ADDRESS(50);

		public static readonly ADDRESS ADDRESS_33 = new ADDRESS(51);

		public static readonly ADDRESS ADDRESS_34 = new ADDRESS(52);

		public static readonly ADDRESS ADDRESS_35 = new ADDRESS(53);

		public static readonly ADDRESS ADDRESS_36 = new ADDRESS(54);

		public static readonly ADDRESS ADDRESS_37 = new ADDRESS(55);

		public static readonly ADDRESS ADDRESS_38 = new ADDRESS(56);

		public static readonly ADDRESS ADDRESS_39 = new ADDRESS(57);

		public static readonly ADDRESS ADDRESS_3A = new ADDRESS(58);

		public static readonly ADDRESS ADDRESS_3B = new ADDRESS(59);

		public static readonly ADDRESS ADDRESS_3C = new ADDRESS(60);

		public static readonly ADDRESS ADDRESS_3D = new ADDRESS(61);

		public static readonly ADDRESS ADDRESS_3E = new ADDRESS(62);

		public static readonly ADDRESS ADDRESS_3F = new ADDRESS(63);

		public static readonly ADDRESS ADDRESS_40 = new ADDRESS(64);

		public static readonly ADDRESS ADDRESS_41 = new ADDRESS(65);

		public static readonly ADDRESS ADDRESS_42 = new ADDRESS(66);

		public static readonly ADDRESS ADDRESS_43 = new ADDRESS(67);

		public static readonly ADDRESS ADDRESS_44 = new ADDRESS(68);

		public static readonly ADDRESS ADDRESS_45 = new ADDRESS(69);

		public static readonly ADDRESS ADDRESS_46 = new ADDRESS(70);

		public static readonly ADDRESS ADDRESS_47 = new ADDRESS(71);

		public static readonly ADDRESS ADDRESS_48 = new ADDRESS(72);

		public static readonly ADDRESS ADDRESS_49 = new ADDRESS(73);

		public static readonly ADDRESS ADDRESS_4A = new ADDRESS(74);

		public static readonly ADDRESS ADDRESS_4B = new ADDRESS(75);

		public static readonly ADDRESS ADDRESS_4C = new ADDRESS(76);

		public static readonly ADDRESS ADDRESS_4D = new ADDRESS(77);

		public static readonly ADDRESS ADDRESS_4E = new ADDRESS(78);

		public static readonly ADDRESS ADDRESS_4F = new ADDRESS(79);

		public static readonly ADDRESS ADDRESS_50 = new ADDRESS(80);

		public static readonly ADDRESS ADDRESS_51 = new ADDRESS(81);

		public static readonly ADDRESS ADDRESS_52 = new ADDRESS(82);

		public static readonly ADDRESS ADDRESS_53 = new ADDRESS(83);

		public static readonly ADDRESS ADDRESS_54 = new ADDRESS(84);

		public static readonly ADDRESS ADDRESS_55 = new ADDRESS(85);

		public static readonly ADDRESS ADDRESS_56 = new ADDRESS(86);

		public static readonly ADDRESS ADDRESS_57 = new ADDRESS(87);

		public static readonly ADDRESS ADDRESS_58 = new ADDRESS(88);

		public static readonly ADDRESS ADDRESS_59 = new ADDRESS(89);

		public static readonly ADDRESS ADDRESS_5A = new ADDRESS(90);

		public static readonly ADDRESS ADDRESS_5B = new ADDRESS(91);

		public static readonly ADDRESS ADDRESS_5C = new ADDRESS(92);

		public static readonly ADDRESS ADDRESS_5D = new ADDRESS(93);

		public static readonly ADDRESS ADDRESS_5E = new ADDRESS(94);

		public static readonly ADDRESS ADDRESS_5F = new ADDRESS(95);

		public static readonly ADDRESS ADDRESS_60 = new ADDRESS(96);

		public static readonly ADDRESS ADDRESS_61 = new ADDRESS(97);

		public static readonly ADDRESS ADDRESS_62 = new ADDRESS(98);

		public static readonly ADDRESS ADDRESS_63 = new ADDRESS(99);

		public static readonly ADDRESS ADDRESS_64 = new ADDRESS(100);

		public static readonly ADDRESS ADDRESS_65 = new ADDRESS(101);

		public static readonly ADDRESS ADDRESS_66 = new ADDRESS(102);

		public static readonly ADDRESS ADDRESS_67 = new ADDRESS(103);

		public static readonly ADDRESS ADDRESS_68 = new ADDRESS(104);

		public static readonly ADDRESS ADDRESS_69 = new ADDRESS(105);

		public static readonly ADDRESS ADDRESS_6A = new ADDRESS(106);

		public static readonly ADDRESS ADDRESS_6B = new ADDRESS(107);

		public static readonly ADDRESS ADDRESS_6C = new ADDRESS(108);

		public static readonly ADDRESS ADDRESS_6D = new ADDRESS(109);

		public static readonly ADDRESS ADDRESS_6E = new ADDRESS(110);

		public static readonly ADDRESS ADDRESS_6F = new ADDRESS(111);

		public static readonly ADDRESS ADDRESS_70 = new ADDRESS(112);

		public static readonly ADDRESS ADDRESS_71 = new ADDRESS(113);

		public static readonly ADDRESS ADDRESS_72 = new ADDRESS(114);

		public static readonly ADDRESS ADDRESS_73 = new ADDRESS(115);

		public static readonly ADDRESS ADDRESS_74 = new ADDRESS(116);

		public static readonly ADDRESS ADDRESS_75 = new ADDRESS(117);

		public static readonly ADDRESS ADDRESS_76 = new ADDRESS(118);

		public static readonly ADDRESS ADDRESS_77 = new ADDRESS(119);

		public static readonly ADDRESS ADDRESS_78 = new ADDRESS(120);

		public static readonly ADDRESS ADDRESS_79 = new ADDRESS(121);

		public static readonly ADDRESS ADDRESS_7A = new ADDRESS(122);

		public static readonly ADDRESS ADDRESS_7B = new ADDRESS(123);

		public static readonly ADDRESS ADDRESS_7C = new ADDRESS(124);

		public static readonly ADDRESS ADDRESS_7D = new ADDRESS(125);

		public static readonly ADDRESS ADDRESS_7E = new ADDRESS(126);

		public static readonly ADDRESS ADDRESS_7F = new ADDRESS(127);

		public static readonly ADDRESS ADDRESS_80 = new ADDRESS(128);

		public static readonly ADDRESS ADDRESS_81 = new ADDRESS(129);

		public static readonly ADDRESS ADDRESS_82 = new ADDRESS(130);

		public static readonly ADDRESS ADDRESS_83 = new ADDRESS(131);

		public static readonly ADDRESS ADDRESS_84 = new ADDRESS(132);

		public static readonly ADDRESS ADDRESS_85 = new ADDRESS(133);

		public static readonly ADDRESS ADDRESS_86 = new ADDRESS(134);

		public static readonly ADDRESS ADDRESS_87 = new ADDRESS(135);

		public static readonly ADDRESS ADDRESS_88 = new ADDRESS(136);

		public static readonly ADDRESS ADDRESS_89 = new ADDRESS(137);

		public static readonly ADDRESS ADDRESS_8A = new ADDRESS(138);

		public static readonly ADDRESS ADDRESS_8B = new ADDRESS(139);

		public static readonly ADDRESS ADDRESS_8C = new ADDRESS(140);

		public static readonly ADDRESS ADDRESS_8D = new ADDRESS(141);

		public static readonly ADDRESS ADDRESS_8E = new ADDRESS(142);

		public static readonly ADDRESS ADDRESS_8F = new ADDRESS(143);

		public static readonly ADDRESS ADDRESS_90 = new ADDRESS(144);

		public static readonly ADDRESS ADDRESS_91 = new ADDRESS(145);

		public static readonly ADDRESS ADDRESS_92 = new ADDRESS(146);

		public static readonly ADDRESS ADDRESS_93 = new ADDRESS(147);

		public static readonly ADDRESS ADDRESS_94 = new ADDRESS(148);

		public static readonly ADDRESS ADDRESS_95 = new ADDRESS(149);

		public static readonly ADDRESS ADDRESS_96 = new ADDRESS(150);

		public static readonly ADDRESS ADDRESS_97 = new ADDRESS(151);

		public static readonly ADDRESS ADDRESS_98 = new ADDRESS(152);

		public static readonly ADDRESS ADDRESS_99 = new ADDRESS(153);

		public static readonly ADDRESS ADDRESS_9A = new ADDRESS(154);

		public static readonly ADDRESS ADDRESS_9B = new ADDRESS(155);

		public static readonly ADDRESS ADDRESS_9C = new ADDRESS(156);

		public static readonly ADDRESS ADDRESS_9D = new ADDRESS(157);

		public static readonly ADDRESS ADDRESS_9E = new ADDRESS(158);

		public static readonly ADDRESS ADDRESS_9F = new ADDRESS(159);

		public static readonly ADDRESS ADDRESS_A0 = new ADDRESS(160);

		public static readonly ADDRESS ADDRESS_A1 = new ADDRESS(161);

		public static readonly ADDRESS ADDRESS_A2 = new ADDRESS(162);

		public static readonly ADDRESS ADDRESS_A3 = new ADDRESS(163);

		public static readonly ADDRESS ADDRESS_A4 = new ADDRESS(164);

		public static readonly ADDRESS ADDRESS_A5 = new ADDRESS(165);

		public static readonly ADDRESS ADDRESS_A6 = new ADDRESS(166);

		public static readonly ADDRESS ADDRESS_A7 = new ADDRESS(167);

		public static readonly ADDRESS ADDRESS_A8 = new ADDRESS(168);

		public static readonly ADDRESS ADDRESS_A9 = new ADDRESS(169);

		public static readonly ADDRESS ADDRESS_AA = new ADDRESS(170);

		public static readonly ADDRESS ADDRESS_AB = new ADDRESS(171);

		public static readonly ADDRESS ADDRESS_AC = new ADDRESS(172);

		public static readonly ADDRESS ADDRESS_AD = new ADDRESS(173);

		public static readonly ADDRESS ADDRESS_AE = new ADDRESS(174);

		public static readonly ADDRESS ADDRESS_AF = new ADDRESS(175);

		public static readonly ADDRESS ADDRESS_B0 = new ADDRESS(176);

		public static readonly ADDRESS ADDRESS_B1 = new ADDRESS(177);

		public static readonly ADDRESS ADDRESS_B2 = new ADDRESS(178);

		public static readonly ADDRESS ADDRESS_B3 = new ADDRESS(179);

		public static readonly ADDRESS ADDRESS_B4 = new ADDRESS(180);

		public static readonly ADDRESS ADDRESS_B5 = new ADDRESS(181);

		public static readonly ADDRESS ADDRESS_B6 = new ADDRESS(182);

		public static readonly ADDRESS ADDRESS_B7 = new ADDRESS(183);

		public static readonly ADDRESS ADDRESS_B8 = new ADDRESS(184);

		public static readonly ADDRESS ADDRESS_B9 = new ADDRESS(185);

		public static readonly ADDRESS ADDRESS_BA = new ADDRESS(186);

		public static readonly ADDRESS ADDRESS_BB = new ADDRESS(187);

		public static readonly ADDRESS ADDRESS_BC = new ADDRESS(188);

		public static readonly ADDRESS ADDRESS_BD = new ADDRESS(189);

		public static readonly ADDRESS ADDRESS_BE = new ADDRESS(190);

		public static readonly ADDRESS ADDRESS_BF = new ADDRESS(191);

		public static readonly ADDRESS ADDRESS_C0 = new ADDRESS(192);

		public static readonly ADDRESS ADDRESS_C1 = new ADDRESS(193);

		public static readonly ADDRESS ADDRESS_C2 = new ADDRESS(194);

		public static readonly ADDRESS ADDRESS_C3 = new ADDRESS(195);

		public static readonly ADDRESS ADDRESS_C4 = new ADDRESS(196);

		public static readonly ADDRESS ADDRESS_C5 = new ADDRESS(197);

		public static readonly ADDRESS ADDRESS_C6 = new ADDRESS(198);

		public static readonly ADDRESS ADDRESS_C7 = new ADDRESS(199);

		public static readonly ADDRESS ADDRESS_C8 = new ADDRESS(200);

		public static readonly ADDRESS ADDRESS_C9 = new ADDRESS(201);

		public static readonly ADDRESS ADDRESS_CA = new ADDRESS(202);

		public static readonly ADDRESS ADDRESS_CB = new ADDRESS(203);

		public static readonly ADDRESS ADDRESS_CC = new ADDRESS(204);

		public static readonly ADDRESS ADDRESS_CD = new ADDRESS(205);

		public static readonly ADDRESS ADDRESS_CE = new ADDRESS(206);

		public static readonly ADDRESS ADDRESS_CF = new ADDRESS(207);

		public static readonly ADDRESS ADDRESS_D0 = new ADDRESS(208);

		public static readonly ADDRESS ADDRESS_D1 = new ADDRESS(209);

		public static readonly ADDRESS ADDRESS_D2 = new ADDRESS(210);

		public static readonly ADDRESS ADDRESS_D3 = new ADDRESS(211);

		public static readonly ADDRESS ADDRESS_D4 = new ADDRESS(212);

		public static readonly ADDRESS ADDRESS_D5 = new ADDRESS(213);

		public static readonly ADDRESS ADDRESS_D6 = new ADDRESS(214);

		public static readonly ADDRESS ADDRESS_D7 = new ADDRESS(215);

		public static readonly ADDRESS ADDRESS_D8 = new ADDRESS(216);

		public static readonly ADDRESS ADDRESS_D9 = new ADDRESS(217);

		public static readonly ADDRESS ADDRESS_DA = new ADDRESS(218);

		public static readonly ADDRESS ADDRESS_DB = new ADDRESS(219);

		public static readonly ADDRESS ADDRESS_DC = new ADDRESS(220);

		public static readonly ADDRESS ADDRESS_DD = new ADDRESS(221);

		public static readonly ADDRESS ADDRESS_DE = new ADDRESS(222);

		public static readonly ADDRESS ADDRESS_DF = new ADDRESS(223);

		public static readonly ADDRESS ADDRESS_E0 = new ADDRESS(224);

		public static readonly ADDRESS ADDRESS_E1 = new ADDRESS(225);

		public static readonly ADDRESS ADDRESS_E2 = new ADDRESS(226);

		public static readonly ADDRESS ADDRESS_E3 = new ADDRESS(227);

		public static readonly ADDRESS ADDRESS_E4 = new ADDRESS(228);

		public static readonly ADDRESS ADDRESS_E5 = new ADDRESS(229);

		public static readonly ADDRESS ADDRESS_E6 = new ADDRESS(230);

		public static readonly ADDRESS ADDRESS_E7 = new ADDRESS(231);

		public static readonly ADDRESS ADDRESS_E8 = new ADDRESS(232);

		public static readonly ADDRESS ADDRESS_E9 = new ADDRESS(233);

		public static readonly ADDRESS ADDRESS_EA = new ADDRESS(234);

		public static readonly ADDRESS ADDRESS_EB = new ADDRESS(235);

		public static readonly ADDRESS ADDRESS_EC = new ADDRESS(236);

		public static readonly ADDRESS ADDRESS_ED = new ADDRESS(237);

		public static readonly ADDRESS ADDRESS_EE = new ADDRESS(238);

		public static readonly ADDRESS ADDRESS_EF = new ADDRESS(239);

		public static readonly ADDRESS ADDRESS_F0 = new ADDRESS(240);

		public static readonly ADDRESS ADDRESS_F1 = new ADDRESS(241);

		public static readonly ADDRESS ADDRESS_F2 = new ADDRESS(242);

		public static readonly ADDRESS ADDRESS_F3 = new ADDRESS(243);

		public static readonly ADDRESS ADDRESS_F4 = new ADDRESS(244);

		public static readonly ADDRESS ADDRESS_F5 = new ADDRESS(245);

		public static readonly ADDRESS ADDRESS_F6 = new ADDRESS(246);

		public static readonly ADDRESS ADDRESS_F7 = new ADDRESS(247);

		public static readonly ADDRESS ADDRESS_F8 = new ADDRESS(248);

		public static readonly ADDRESS ADDRESS_F9 = new ADDRESS(249);

		public static readonly ADDRESS ADDRESS_FA = new ADDRESS(250);

		public static readonly ADDRESS ADDRESS_FB = new ADDRESS(251);

		public static readonly ADDRESS ADDRESS_FC = new ADDRESS(252);

		public static readonly ADDRESS ADDRESS_FD = new ADDRESS(253);

		public static readonly ADDRESS ADDRESS_FE = new ADDRESS(254);

		public static readonly ADDRESS ADDRESS_FF = new ADDRESS(255);

		public readonly byte? Value;

		public readonly string Name;

		public bool IsValidAddress => Value >= 0;

		public bool IsValidDeviceAddress => Value > 0;

		public static System.Collections.Generic.IEnumerable<ADDRESS> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerable<ADDRESS>)List;
		}

		private ADDRESS(byte value)
			: this(value, ByteExtensions.HexString(value) + "h")
		{
		}

		private ADDRESS(byte? value, string name)
		{
			Value = value;
			Name = name.Trim();
			if (value.HasValue)
			{
				List.Add(this);
				Table[value.Value] = this;
			}
		}

		public static implicit operator byte(ADDRESS address)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (!address.Value.HasValue)
			{
				throw new InvalidCastException("Cannot convert ADDRESS.INVALID to a byte");
			}
			return address.Value.Value;
		}

		public static implicit operator ADDRESS(byte value)
		{
			return Table[value];
		}

		public string ToString()
		{
			return Name;
		}
	}
	public struct CIRCUIT_ID
	{
		private uint Value;

		private CIRCUIT_ID(uint value)
		{
			Value = value;
		}

		public static implicit operator uint(CIRCUIT_ID circuit_id)
		{
			return circuit_id.Value;
		}

		public static implicit operator CIRCUIT_ID(uint value)
		{
			return new CIRCUIT_ID(value);
		}

		public string ToString()
		{
			uint value = Value;
			if (value == 0)
			{
				return "none";
			}
			byte b = (byte)(value >> 24);
			byte b2 = (byte)(value >> 16);
			byte b3 = (byte)(value >> 8);
			byte b4 = (byte)value;
			return $"{ByteExtensions.HexString(b)}:{ByteExtensions.HexString(b2)}:{ByteExtensions.HexString(b3)}:{ByteExtensions.HexString(b4)}";
		}
	}
	public sealed class MESSAGE_TYPE
	{
		private static readonly MESSAGE_TYPE[] Table;

		private static readonly List<MESSAGE_TYPE> List;

		private const byte EXTENDED = 128;

		public const byte NETWORK = 0;

		public const byte CIRCUIT_ID = 1;

		public const byte DEVICE_ID = 2;

		public const byte DEVICE_STATUS = 3;

		public const byte PRODUCT_STATUS = 6;

		public const byte TIME = 7;

		public const byte REQUEST = 128;

		public const byte RESPONSE = 129;

		public const byte COMMAND = 130;

		public const byte EXT_STATUS = 131;

		public const byte TEXT_CONSOLE = 132;

		public const byte GROUP_ID = 133;

		public const byte DAQ = 155;

		public const byte IOT = 157;

		public const byte BULK_XFER = 159;

		private static readonly MESSAGE_TYPE INVALID;

		private readonly byte Value;

		public readonly string Name;

		public bool IsBroadcast => (Value & 0x80) == 0;

		public bool IsPointToPoint => (Value & 0x80) != 0;

		public static System.Collections.Generic.IEnumerable<MESSAGE_TYPE> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerable<MESSAGE_TYPE>)List;
		}

		static MESSAGE_TYPE()
		{
			Table = new MESSAGE_TYPE[256];
			List = new List<MESSAGE_TYPE>();
			INVALID = new MESSAGE_TYPE(-1, "INVALID");
			for (int i = 0; i < Table.Length; i++)
			{
				Table[i] = INVALID;
			}
			new MESSAGE_TYPE(0, "NETWORK");
			new MESSAGE_TYPE(1, "CIRCUIT_ID");
			new MESSAGE_TYPE(2, "DEVICE_ID");
			new MESSAGE_TYPE(3, "DEVICE_STATUS");
			new MESSAGE_TYPE(4, "RESERVED_100");
			new MESSAGE_TYPE(5, "RESERVED_101");
			new MESSAGE_TYPE(6, "PRODUCT_STATUS");
			new MESSAGE_TYPE(7, "TIME");
			new MESSAGE_TYPE(128, "REQUEST");
			new MESSAGE_TYPE(129, "RESPONSE");
			new MESSAGE_TYPE(130, "COMMAND");
			new MESSAGE_TYPE(131, "EXT_STATUS");
			new MESSAGE_TYPE(132, "TEXT_CONSOLE");
			new MESSAGE_TYPE(133, "GROUP_ID");
			new MESSAGE_TYPE(134, "RESERVED_00110");
			new MESSAGE_TYPE(135, "RESERVED_00111");
			new MESSAGE_TYPE(136, "RESERVED_01000");
			new MESSAGE_TYPE(137, "RESERVED_01001");
			new MESSAGE_TYPE(138, "RESERVED_01010");
			new MESSAGE_TYPE(139, "RESERVED_01011");
			new MESSAGE_TYPE(140, "RESERVED_01100");
			new MESSAGE_TYPE(141, "RESERVED_01101");
			new MESSAGE_TYPE(142, "RESERVED_01110");
			new MESSAGE_TYPE(143, "RESERVED_01111");
			new MESSAGE_TYPE(144, "RESERVED_10000");
			new MESSAGE_TYPE(145, "RESERVED_10001");
			new MESSAGE_TYPE(146, "RESERVED_10010");
			new MESSAGE_TYPE(147, "RESERVED_10011");
			new MESSAGE_TYPE(148, "RESERVED_10100");
			new MESSAGE_TYPE(149, "RESERVED_10101");
			new MESSAGE_TYPE(150, "RESERVED_10110");
			new MESSAGE_TYPE(151, "RESERVED_10111");
			new MESSAGE_TYPE(152, "RESERVED_11000");
			new MESSAGE_TYPE(153, "RESERVED_11001");
			new MESSAGE_TYPE(154, "RESERVED_11010");
			new MESSAGE_TYPE(155, "DAQ");
			new MESSAGE_TYPE(156, "RESERVED_11100");
			new MESSAGE_TYPE(157, "IOT");
			new MESSAGE_TYPE(158, "RESERVED_11110");
			new MESSAGE_TYPE(159, "BULK_XFER");
		}

		private MESSAGE_TYPE(int value, string name)
		{
			Value = (byte)value;
			Name = name.Trim();
			if (value >= 0)
			{
				Table[value] = this;
				List.Add(this);
			}
		}

		public static implicit operator byte(MESSAGE_TYPE msg)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (msg == INVALID)
			{
				throw new InvalidCastException("Cannot convert MESSAGE_TYPE.INVALID to a byte");
			}
			return msg.Value;
		}

		public static implicit operator MESSAGE_TYPE(byte value)
		{
			return Table[value];
		}

		public string ToString()
		{
			return Name;
		}
	}
	public struct CAN_ID
	{
		[field: CompilerGenerated]
		public MESSAGE_TYPE MessageType
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public ADDRESS SourceAddress
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public ADDRESS TargetAddress
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public byte MessageData
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public CAN_ID(MESSAGE_TYPE type, ADDRESS source_address)
		{
			MessageType = type;
			SourceAddress = source_address;
			TargetAddress = ADDRESS.BROADCAST;
			MessageData = 0;
		}

		public CAN_ID(MESSAGE_TYPE type, ADDRESS source_address, ADDRESS target_address, byte ext_data)
		{
			MessageType = type;
			SourceAddress = source_address;
			TargetAddress = target_address;
			MessageData = ext_data;
		}

		public CAN_ID(ID id)
		{
			if (((ID)(ref id)).IsExtended)
			{
				uint value = ((ID)(ref id)).Value;
				SourceAddress = (byte)(value >> 18);
				TargetAddress = (byte)(value >> 8);
				MessageData = (byte)value;
				MessageType = (byte)(0x80 | ((value >> 24) & 0x1C) | ((value >> 16) & 3));
			}
			else
			{
				uint value2 = ((ID)(ref id)).Value;
				SourceAddress = (byte)value2;
				MessageType = (byte)((value2 >> 8) & 7);
				TargetAddress = ADDRESS.BROADCAST;
				MessageData = 0;
			}
		}

		public static implicit operator CAN_ID(ID id)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new CAN_ID(id);
		}

		public static implicit operator ID(CAN_ID id)
		{
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (id.MessageType.IsPointToPoint)
			{
				return new ID((uint)(0 | (((byte)id.MessageType & 0x1C) << 24) | ((byte)id.SourceAddress << 18) | (((byte)id.MessageType & 3) << 16) | ((byte)id.TargetAddress << 8) | id.MessageData), true);
			}
			return new ID((uint)(0 | (((byte)id.MessageType & 7) << 8) | (byte)id.SourceAddress), false);
		}
	}
	public sealed class IDS_CAN_VERSION_NUMBER
	{
		private static readonly IDS_CAN_VERSION_NUMBER[] Table;

		private static readonly List<IDS_CAN_VERSION_NUMBER> List;

		public static readonly IDS_CAN_VERSION_NUMBER UNKNOWN;

		public const byte VERSION_0_9 = 0;

		public const byte VERSION_1_0 = 1;

		public const byte VERSION_1_1 = 2;

		public const byte VERSION_1_2 = 3;

		public const byte VERSION_1_3 = 4;

		public const byte VERSION_1_4 = 5;

		public const byte VERSION_1_5 = 6;

		public const byte VERSION_1_6 = 7;

		public const byte VERSION_2_0 = 8;

		public const byte VERSION_2_1 = 9;

		public const byte VERSION_2_2 = 10;

		public const byte VERSION_2_3 = 11;

		public const byte VERSION_2_4 = 12;

		public const byte VERSION_2_5 = 13;

		public const byte VERSION_2_6 = 14;

		public const byte VERSION_2_7 = 15;

		public const byte VERSION_2_8 = 16;

		public const byte VERSION_2_9 = 17;

		public const byte VERSION_3_0 = 18;

		public const byte VERSION_3_1 = 19;

		public const byte VERSION_3_2 = 20;

		public const byte VERSION_3_3 = 21;

		public const byte VERSION_3_4 = 22;

		public const byte VERSION_3_5 = 23;

		public const byte VERSION_3_6 = 24;

		public const byte VERSION_3_7 = 25;

		public const byte VERSION_3_8 = 26;

		public const byte VERSION_3_9 = 27;

		public const byte VERSION_4_0 = 28;

		public const byte VERSION_4_1 = 29;

		public const byte VERSION_4_2 = 30;

		public const byte VERSION_4_3 = 31;

		public const byte VERSION_4_4 = 32;

		public const byte VERSION_4_5 = 33;

		public const byte VERSION_4_6 = 34;

		public const byte VERSION_4_7 = 35;

		public const byte VERSION_LATEST = 28;

		public readonly byte Value;

		public readonly string Name;

		public readonly int Major;

		public readonly int Minor;

		public static System.Collections.Generic.IEnumerable<IDS_CAN_VERSION_NUMBER> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerable<IDS_CAN_VERSION_NUMBER>)List;
		}

		static IDS_CAN_VERSION_NUMBER()
		{
			Table = new IDS_CAN_VERSION_NUMBER[256];
			List = new List<IDS_CAN_VERSION_NUMBER>();
			UNKNOWN = new IDS_CAN_VERSION_NUMBER(-1, -1, -1);
			new IDS_CAN_VERSION_NUMBER(0, 0, 9);
			new IDS_CAN_VERSION_NUMBER(1, 1, 0);
			new IDS_CAN_VERSION_NUMBER(2, 1, 1);
			new IDS_CAN_VERSION_NUMBER(3, 1, 2);
			new IDS_CAN_VERSION_NUMBER(4, 1, 3);
			new IDS_CAN_VERSION_NUMBER(5, 1, 4);
			new IDS_CAN_VERSION_NUMBER(6, 1, 5);
			new IDS_CAN_VERSION_NUMBER(7, 1, 6);
			new IDS_CAN_VERSION_NUMBER(8, 2, 0);
			new IDS_CAN_VERSION_NUMBER(9, 2, 1);
			new IDS_CAN_VERSION_NUMBER(10, 2, 2);
			new IDS_CAN_VERSION_NUMBER(11, 2, 3);
			new IDS_CAN_VERSION_NUMBER(12, 2, 4);
			new IDS_CAN_VERSION_NUMBER(13, 2, 5);
			new IDS_CAN_VERSION_NUMBER(14, 2, 6);
			new IDS_CAN_VERSION_NUMBER(15, 2, 7);
			new IDS_CAN_VERSION_NUMBER(16, 2, 8);
			new IDS_CAN_VERSION_NUMBER(17, 2, 9);
			new IDS_CAN_VERSION_NUMBER(18, 3, 0);
			new IDS_CAN_VERSION_NUMBER(19, 3, 1);
			new IDS_CAN_VERSION_NUMBER(20, 3, 2);
			new IDS_CAN_VERSION_NUMBER(21, 3, 3);
			new IDS_CAN_VERSION_NUMBER(22, 3, 4);
			new IDS_CAN_VERSION_NUMBER(23, 3, 5);
			new IDS_CAN_VERSION_NUMBER(24, 3, 6);
			new IDS_CAN_VERSION_NUMBER(25, 3, 7);
			new IDS_CAN_VERSION_NUMBER(26, 3, 8);
			new IDS_CAN_VERSION_NUMBER(27, 3, 9);
			new IDS_CAN_VERSION_NUMBER(28, 4, 0);
			new IDS_CAN_VERSION_NUMBER(29, 4, 1);
			new IDS_CAN_VERSION_NUMBER(30, 4, 2);
			new IDS_CAN_VERSION_NUMBER(31, 4, 3);
			new IDS_CAN_VERSION_NUMBER(32, 4, 4);
			new IDS_CAN_VERSION_NUMBER(33, 4, 5);
			new IDS_CAN_VERSION_NUMBER(34, 4, 6);
			new IDS_CAN_VERSION_NUMBER(35, 4, 7);
		}

		private IDS_CAN_VERSION_NUMBER(byte value)
		{
			Value = value;
			Name = "Version " + ByteExtensions.HexString(value) + "h";
			Major = -1;
			Minor = -1;
			Table[value] = this;
		}

		private IDS_CAN_VERSION_NUMBER(int value, int major, int minor)
		{
			if (value < 0 || value > 255)
			{
				Value = 0;
				Major = -1;
				Minor = -1;
				Name = "UNKNOWN";
			}
			else
			{
				Value = (byte)value;
				Major = major;
				Minor = minor;
				Name = "Version " + Major + "." + Minor;
				List.Add(this);
				Table[value] = this;
			}
		}

		public static implicit operator byte(IDS_CAN_VERSION_NUMBER version)
		{
			return version.Value;
		}

		public static implicit operator IDS_CAN_VERSION_NUMBER(byte value)
		{
			if (Table[value] != null)
			{
				return Table[value];
			}
			return new IDS_CAN_VERSION_NUMBER(value);
		}

		public string ToString()
		{
			return Name;
		}
	}
	public sealed class IN_MOTION_LOCKOUT_LEVEL
	{
		public const byte LEVEL_0_NO_LOCKOUT = 0;

		public const byte LEVEL_1_MOBILE_DEVICE_LOCKOUT = 1;

		public const byte LEVEL_2_NETWORK_LOCKOUT = 2;

		public const byte LEVEL_3_FULL_LOCKOUT = 3;

		private static readonly IN_MOTION_LOCKOUT_LEVEL[] Array;

		private readonly byte Value;

		private readonly string Name;

		static IN_MOTION_LOCKOUT_LEVEL()
		{
			Array = new IN_MOTION_LOCKOUT_LEVEL[4];
			new IN_MOTION_LOCKOUT_LEVEL(0, "NO_LOCKOUT");
			new IN_MOTION_LOCKOUT_LEVEL(1, "MOBILE_DEVICE_LOCKOUT");
			new IN_MOTION_LOCKOUT_LEVEL(2, "NETWORK_LOCKOUT");
			new IN_MOTION_LOCKOUT_LEVEL(3, "FULL_LOCKOUT");
		}

		private IN_MOTION_LOCKOUT_LEVEL(byte value, string name)
		{
			Value = value;
			Name = name;
			Array[value] = this;
		}

		public static implicit operator byte(IN_MOTION_LOCKOUT_LEVEL level)
		{
			return level.Value;
		}

		public static implicit operator IN_MOTION_LOCKOUT_LEVEL(byte value)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (value > 3)
			{
				throw new InvalidCastException($"Cannot convert {value} to a IN_MOTION_LOCKOUT_LEVEL");
			}
			return Array[value];
		}

		public string ToString()
		{
			return Name;
		}
	}
	public sealed class SOFTWARE_UPDATE_STATE
	{
		public const byte NO_UPDATE_AVAILBLE = 0;

		public const byte UPDATE_AVAILBLE = 1;

		public const byte UPDATE_AUTHORIZED = 2;

		public const byte UPDATE_IN_PROGRESS = 3;

		private static readonly SOFTWARE_UPDATE_STATE[] Array;

		private readonly byte Value;

		private readonly string Name;

		static SOFTWARE_UPDATE_STATE()
		{
			Array = new SOFTWARE_UPDATE_STATE[4];
			new SOFTWARE_UPDATE_STATE(0, "NO_UPDATE_AVAILBLE");
			new SOFTWARE_UPDATE_STATE(1, "UPDATE_AVAILBLE");
			new SOFTWARE_UPDATE_STATE(2, "UPDATE_AUTHORIZED");
			new SOFTWARE_UPDATE_STATE(3, "UPDATE_IN_PROGRESS");
		}

		private SOFTWARE_UPDATE_STATE(byte value, string name)
		{
			Value = value;
			Name = name;
			Array[value] = this;
		}

		public static implicit operator byte(SOFTWARE_UPDATE_STATE level)
		{
			return level.Value;
		}

		public static implicit operator SOFTWARE_UPDATE_STATE(byte value)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (value > 3)
			{
				throw new InvalidCastException($"Cannot convert {value} to a SOFTWARE_UPDATE_STATE");
			}
			return Array[value];
		}

		public string ToString()
		{
			return Name;
		}
	}
	public struct NETWORK_STATUS
	{
		public byte Value;

		public bool HasActiveDTCs => (Value & 1) != 0;

		public bool HasStoredDTCs => (Value & 2) != 0;

		public bool HasDTCs => (Value & 3) != 0;

		public bool HasOpenSessions => (Value & 4) != 0;

		public IN_MOTION_LOCKOUT_LEVEL InMotionLockoutLevel => (byte)((Value >> 3) & 3);

		public bool HasExtemdedCloudCapabilities => (Value & 0x40) != 0;

		public bool IsHazardousDevice => (Value & 0x80) != 0;

		public NETWORK_STATUS(byte value)
		{
			Value = value;
		}

		public NETWORK_STATUS(byte value, IDS_CAN_VERSION_NUMBER version)
		{
			if ((byte)version <= 16)
			{
				value &= 7;
			}
			else if ((byte)version <= 17)
			{
				value &= 0xF;
			}
			else if ((byte)version <= 18)
			{
				value &= 0x9F;
			}
			else if ((byte)version <= 19)
			{
				value &= 0xDF;
			}
			Value = value;
		}

		public static implicit operator byte(NETWORK_STATUS s)
		{
			return s.Value;
		}

		public static implicit operator NETWORK_STATUS(byte value)
		{
			return new NETWORK_STATUS(value);
		}
	}
	[JsonConverter(typeof(DeviceIdConverter))]
	public struct DEVICE_ID
	{
		private class DeviceIdConverter : JsonConverter
		{
			public override bool CanConvert(System.Type objectType)
			{
				return objectType == typeof(DEVICE_ID);
			}

			public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Invalid comparison between Unknown and I4
				JToken val = JToken.Load(reader);
				if ((int)val.Type == 10)
				{
					return null;
				}
				return new DEVICE_ID(((object)val).ToString());
			}

			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				if (value is DEVICE_ID dEVICE_ID)
				{
					JToken.FromObject((object)dEVICE_ID.JsonString).WriteTo(writer, System.Array.Empty<JsonConverter>());
					return;
				}
				throw new ArgumentException();
			}
		}

		public PRODUCT_ID ProductID;

		public DEVICE_TYPE DeviceType;

		public FUNCTION_NAME FunctionName;

		private byte mDeviceInstance;

		private byte mFunctionInstance;

		[field: CompilerGenerated]
		public byte ProductInstance
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public int DeviceInstance
		{
			get
			{
				return mDeviceInstance;
			}
			set
			{
				mDeviceInstance = (byte)(value & 0xF);
			}
		}

		public int FunctionInstance
		{
			get
			{
				return mFunctionInstance;
			}
			set
			{
				mFunctionInstance = (byte)(value & 0xF);
			}
		}

		[field: CompilerGenerated]
		public byte? DeviceCapabilities
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		private string JsonString
		{
			get
			{
				ulong num = PRODUCT_ID.op_Implicit(ProductID);
				num <<= 8;
				num |= ProductInstance;
				num <<= 8;
				num |= DEVICE_TYPE.op_Implicit(DeviceType);
				num <<= 4;
				num |= (byte)DeviceInstance;
				num <<= 16;
				num |= FUNCTION_NAME.op_Implicit(FunctionName);
				num <<= 4;
				num |= (byte)FunctionInstance;
				if (DeviceCapabilities.HasValue)
				{
					num <<= 8;
					return (num | DeviceCapabilities.Value).ToString("X16");
				}
				return num.ToString("X14");
			}
		}

		public bool IsValid
		{
			get
			{
				if (ProductID.IsValid)
				{
					return DeviceType.IsValid;
				}
				return false;
			}
		}

		public string ProductString
		{
			get
			{
				if (ProductID == null)
				{
					ProductID = PRODUCT_ID.UNKNOWN;
				}
				return $"{ProductID} @{ByteExtensions.HexString(ProductInstance)}h";
			}
		}

		public string DeviceString
		{
			get
			{
				if (DeviceType == null)
				{
					DeviceType = DEVICE_TYPE.op_Implicit((byte)0);
				}
				if (DeviceInstance > 0)
				{
					return $"{DeviceType} #{DeviceInstance}";
				}
				return ((object)DeviceType).ToString();
			}
		}

		public string FunctionString
		{
			get
			{
				if (FunctionName == null)
				{
					FunctionName = FUNCTION_NAME.UNKNOWN;
				}
				if (FunctionInstance > 0)
				{
					if (FunctionName.Name.Contains("{0}"))
					{
						return FunctionName.Name.Replace("{0}", FunctionInstance.ToString()) ?? "";
					}
					return $"{FunctionName} {FunctionInstance}";
				}
				return ((object)FunctionName).ToString();
			}
		}

		public ICON Icon
		{
			get
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				if (!DeviceType.IsValid)
				{
					return (ICON)0;
				}
				if (FunctionName.IsValid)
				{
					return FunctionName.Icon;
				}
				return DeviceType.Icon;
			}
		}

		public DEVICE_ID(PRODUCT_ID product_id, byte product_instance, DEVICE_TYPE device_type, int device_instance, FUNCTION_NAME function_name, int function_instance, byte? device_capabilities)
		{
			ProductID = product_id;
			ProductInstance = product_instance;
			DeviceType = device_type;
			mDeviceInstance = (byte)(device_instance & 0xF);
			FunctionName = function_name;
			mFunctionInstance = (byte)(function_instance & 0xF);
			DeviceCapabilities = device_capabilities;
		}

		private DEVICE_ID(string json)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			ulong num = ulong.Parse(json, (NumberStyles)515);
			switch (json.Length)
			{
			default:
				throw new ArgumentException();
			case 16:
				DeviceCapabilities = (byte)num;
				num >>= 8;
				break;
			case 14:
				DeviceCapabilities = null;
				break;
			}
			mFunctionInstance = (byte)(num & 0xF);
			num >>= 4;
			FunctionName = FUNCTION_NAME.op_Implicit((ushort)num);
			num >>= 16;
			mDeviceInstance = (byte)(num & 0xF);
			num >>= 4;
			DeviceType = DEVICE_TYPE.op_Implicit((byte)num);
			num >>= 8;
			ProductInstance = (byte)num;
			num >>= 8;
			ProductID = PRODUCT_ID.op_Implicit((ushort)num);
		}

		public bool Equals(object obj)
		{
			if (!(obj is DEVICE_ID dEVICE_ID))
			{
				return false;
			}
			if (ProductID != dEVICE_ID.ProductID)
			{
				return false;
			}
			if (ProductInstance != dEVICE_ID.ProductInstance)
			{
				return false;
			}
			if (DeviceType != dEVICE_ID.DeviceType)
			{
				return false;
			}
			if (DeviceInstance != dEVICE_ID.DeviceInstance)
			{
				return false;
			}
			if (FunctionName != dEVICE_ID.FunctionName)
			{
				return false;
			}
			if (FunctionInstance != dEVICE_ID.FunctionInstance)
			{
				return false;
			}
			if (DeviceCapabilities != dEVICE_ID.DeviceCapabilities)
			{
				return false;
			}
			return true;
		}

		public int GetHashCode()
		{
			return ((((((17 * 23 + ((object)ProductID).GetHashCode()) * 23 + ProductInstance.GetHashCode()) * 23 + ((object)DeviceType).GetHashCode()) * 23 + DeviceInstance.GetHashCode()) * 23 + ((object)FunctionName).GetHashCode()) * 23 + FunctionInstance.GetHashCode()) * 23 + ((object)DeviceCapabilities/*cast due to .constrained prefix*/).GetHashCode();
		}

		public static bool operator ==(DEVICE_ID s1, DEVICE_ID s2)
		{
			return ((object)s1/*cast due to .constrained prefix*/).Equals((object)s2);
		}

		public static bool operator !=(DEVICE_ID s1, DEVICE_ID s2)
		{
			return !(s1 == s2);
		}

		public string ToString()
		{
			return $"{FunctionString}, {DeviceString}, {ProductString}";
		}

		public void Clear()
		{
			ProductID = PRODUCT_ID.UNKNOWN;
			ProductInstance = 0;
			DeviceType = DEVICE_TYPE.op_Implicit((byte)0);
			DeviceInstance = 0;
			FunctionName = FUNCTION_NAME.UNKNOWN;
			FunctionInstance = 0;
			DeviceCapabilities = null;
		}
	}
	public sealed class REQUEST
	{
		private static readonly REQUEST[] Table;

		private static readonly List<REQUEST> List;

		public static readonly REQUEST INVALID;

		public const byte PART_NUMBER_READ = 0;

		public const byte MUTE_DEVICE = 1;

		public const byte IN_MOTION_LOCKOUT = 2;

		public const byte SOFTWARE_UPDATE_AUTHORIZATION = 3;

		public const byte NOTIFICATION_ALERT = 4;

		public const byte PID_READ_LIST = 16;

		public const byte PID_READ_WRITE = 17;

		public const byte GET_PID_PROPERTIES = 18;

		public const byte READ_BLOCK_LIST = 32;

		public const byte READ_BLOCK_PROPERTIES = 33;

		public const byte READ_BLOCK_DATA = 34;

		public const byte BEGIN_BLOCK_WRITE = 35;

		public const byte BEGIN_BLOCK_WRITE_BULK_XFER = 36;

		public const byte END_BLOCK_BULK_XFER = 37;

		public const byte END_BLOCK_WRITE = 38;

		public const byte SET_BLOCK_ADDRESS = 39;

		public const byte SET_BLOCK_SIZE = 40;

		public const byte READ_CONTINUOUS_DTCS = 48;

		public const byte CONTINUOUS_DTC_COMMAND = 49;

		public const byte SESSION_READ_LIST = 64;

		public const byte SESSION_READ_STATUS = 65;

		public const byte SESSION_REQUEST_SEED = 66;

		public const byte SESSION_TRANSMIT_KEY = 67;

		public const byte SESSION_HEARTBEAT = 68;

		public const byte SESSION_END = 69;

		public const byte IDS_CAN_REQUEST_DAQ_NUM_CHANNELS = 81;

		public const byte IDS_CAN_REQUEST_DAQ_AUTO_TX_SETTINGS = 82;

		public const byte IDS_CAN_REQUEST_DAQ_CHANNEL_SETTINGS = 83;

		public const byte IDS_CAN_REQUEST_DAQ_PID_ADDRESS = 84;

		public const byte IDS_CAN_REQUEST_LEVELER_TYPE_5_CONTROL = 96;

		public readonly byte Value;

		public readonly string Name;

		public bool IsValid => this != INVALID;

		public static System.Collections.Generic.IEnumerable<REQUEST> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerable<REQUEST>)List;
		}

		static REQUEST()
		{
			Table = new REQUEST[256];
			List = new List<REQUEST>();
			INVALID = new REQUEST(-1, "INVALID");
			new REQUEST(0, "PART_NUMBER_READ");
			new REQUEST(1, "MUTE_DEVICE");
			new REQUEST(2, "IN_MOTION_LOCKOUT");
			new REQUEST(3, "SOFTWARE_UPDATE_AUTHORIZATION");
			new REQUEST(4, "NOTIFICATION_ALERT");
			new REQUEST(16, "PID_READ_LIST");
			new REQUEST(17, "PID_READ_WRITE");
			new REQUEST(32, "BLOCK_READ_LIST");
			new REQUEST(33, "BLOCK_READ_PROPERTIES");
			new REQUEST(34, "BLOCK_READ_DATA");
			new REQUEST(35, "BEGIN_BLOCK_WRITE");
			new REQUEST(36, "BEGIN_BLOCK_WRITE_BULK_XFER");
			new REQUEST(37, "BLOCK_END_BULK_XFER");
			new REQUEST(38, "END_BLOCK_WRITE");
			new REQUEST(39, "SET_BLOCK_ADDRESS");
			new REQUEST(40, "SET_BLOCK_SIZE");
			new REQUEST(48, "READ_CONTINUOUS_DTCS");
			new REQUEST(49, "CONTINUOUS_DTC_COMMAND");
			new REQUEST(64, "SESSION_READ_LIST");
			new REQUEST(65, "SESSION_READ_STATUS");
			new REQUEST(66, "SESSION_REQUEST_SEED");
			new REQUEST(67, "SESSION_TRANSMIT_KEY");
			new REQUEST(68, "SESSION_HEARTBEAT");
			new REQUEST(69, "SESSION_END");
			new REQUEST(81, "IDS_CAN_REQUEST_DAQ_NUM_CHANNELS");
			new REQUEST(82, "IDS_CAN_REQUEST_DAQ_AUTO_TX_SETTINGS");
			new REQUEST(83, "IDS_CAN_REQUEST_DAQ_CHANNEL_SETTINGS");
			new REQUEST(84, "IDS_CAN_REQUEST_DAQ_PID_ADDRESS");
			new REQUEST(96, "IDS_CAN_REQUEST_LEVELER_TYPE_5_CONTROL");
			for (int i = 0; i < Table.Length; i++)
			{
				if (Table[i] == null)
				{
					Table[i] = new REQUEST((byte)i, "UNKNOWN_" + ByteExtensions.HexString((byte)i));
				}
			}
		}

		private REQUEST(int value, string name)
		{
			Name = name.Trim();
			Value = (byte)value;
			if (value >= 0)
			{
				List.Add(this);
				Table[value] = this;
			}
		}

		public static implicit operator byte(REQUEST msg)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (msg == INVALID)
			{
				throw new InvalidCastException("Cannot convert REQUEST.INVALID to a byte");
			}
			return msg.Value;
		}

		public static implicit operator REQUEST(byte value)
		{
			return Table[value];
		}

		public string ToString()
		{
			return Name;
		}
	}
	public enum RESPONSE : byte
	{
		SUCCESS,
		REQUEST_NOT_SUPPORTED,
		BAD_REQUEST,
		VALUE_OUT_OF_RANGE,
		UNKNOWN_ID,
		VALUE_TOO_LARGE,
		INVALID_ADDRESS,
		READ_ONLY,
		WRITE_ONLY,
		CONDITIONS_NOT_CORRECT,
		FEATURE_NOT_SUPPORTED,
		BUSY,
		SEED_NOT_REQUESTED,
		KEY_NOT_CORRECT,
		SESSION_NOT_OPEN,
		TIMEOUT,
		REMOTE_REQUEST_NOT_SUPPORTED,
		IN_MOTION_LOCKOUT_ACTIVE,
		CRC_INVALID,
		CANCELLED,
		ABORTED,
		FAILED,
		IN_PROGRESS
	}
	public static class IconExtensions
	{
		private static readonly Dictionary<ushort, ICON> Lookup;

		static IconExtensions()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected I4, but got Unknown
			Lookup = new Dictionary<ushort, ICON>();
			foreach (ICON value in System.Enum.GetValues(typeof(ICON)))
			{
				Lookup.Add((ushort)(int)value, value);
			}
		}

		public static ICON ToICON(this ushort value)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			ICON result = default(ICON);
			if (Lookup.TryGetValue(value, ref result))
			{
				return result;
			}
			return (ICON)0;
		}

		public static ushort Value(this ICON icon)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Expected I4, but got Unknown
			return (ushort)(int)icon;
		}
	}
	public interface IBusEndpoint
	{
		IAdapter Adapter { get; }

		ADDRESS Address { get; }

		bool IsOnline { get; }
	}
	public interface IUniqueDeviceInfo : IUniqueProductInfo
	{
		DEVICE_TYPE DeviceType { get; }

		int DeviceInstance { get; }
	}
	public interface IDevice : IBusEndpoint, IUniqueDeviceInfo, IUniqueProductInfo
	{
		IProduct Product { get; }

		IDS_CAN_VERSION_NUMBER ProtocolVersion { get; }

		NETWORK_STATUS NetworkStatus { get; }

		CIRCUIT_ID CircuitID { get; }

		PAYLOAD DeviceStatus { get; }

		byte ProductInstance { get; }

		FUNCTION_NAME FunctionName { get; }

		int FunctionInstance { get; }

		byte? DeviceCapabilities { get; }

		string SoftwarePartNumber { get; }

		ITextConsole TextConsole { get; }
	}
	public interface IDeviceStatusParams
	{
		void SetPayload(PAYLOAD payload);
	}
	public class DeviceDisplayAttribute : System.Attribute
	{
		[field: CompilerGenerated]
		public string DisplayName
		{
			[CompilerGenerated]
			get;
		}

		public DeviceDisplayAttribute(string displayName = null)
		{
			DisplayName = displayName;
		}
	}
	public static class DeviceExtensions
	{
		public static DEVICE_ID GetDeviceID(this IDevice device)
		{
			return new DEVICE_ID(device.ProductID, device.ProductInstance, device.DeviceType, device.DeviceInstance, device.FunctionName, device.FunctionInstance, device.DeviceCapabilities);
		}

		public static ulong GetDeviceUniqueID(this IUniqueDeviceInfo device)
		{
			return ((((((((((((((((ulong)((PhysicalAddress)device.MAC)[0] << 8) | ((PhysicalAddress)device.MAC)[1]) << 8) | ((PhysicalAddress)device.MAC)[2]) << 8) | ((PhysicalAddress)device.MAC)[3]) << 8) | ((PhysicalAddress)device.MAC)[4]) << 8) | ((PhysicalAddress)device.MAC)[5]) << 4) | (byte)(PRODUCT_ID.op_Implicit(device.ProductID) & 0xF)) << 8) | DEVICE_TYPE.op_Implicit(device.DeviceType)) << 4) | (byte)(device.DeviceInstance & 0xF);
		}

		public static UInt128 GetFeatureUniqueID(this IDevice device)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			return (((UInt128.op_Implicit(device.GetDeviceUniqueID()) << 16) | UInt128.op_Implicit(FUNCTION_NAME.op_Implicit(device.FunctionName))) << 4) | UInt128.op_Implicit((byte)(device.FunctionInstance & 0xF));
		}

		public static string FriendlyName(this IDevice device)
		{
			FUNCTION_NAME val = device.FunctionName ?? FUNCTION_NAME.UNKNOWN;
			if (device.FunctionInstance > 0)
			{
				if (val.Name.Contains("{0}"))
				{
					return val.Name.Replace("{0}", device.FunctionInstance.ToString()) ?? "";
				}
				return $"{val} {device.FunctionInstance}";
			}
			return ((object)val).ToString();
		}

		public static string ToShortString(this IDevice device, bool show_address)
		{
			if (!show_address)
			{
				return $"{device.FriendlyName()} ({device.DeviceType})";
			}
			return $"{device.FriendlyName()} ({device.DeviceType} @{device.Address})";
		}
	}
	public interface IDeviceDiscoverer : System.Collections.Generic.IEnumerable<IRemoteDevice>, System.Collections.IEnumerable
	{
		IAdapter Adapter { get; }

		int NumDevicesDetectedOnNetwork { get; }

		IRemoteDevice GetDeviceByAddress(ADDRESS address);

		IRemoteDevice GetDeviceByUniqueID(ulong unique_id);

		System.Collections.Generic.IEnumerable<IRemoteDevice> GetAllDevicesMatchingFilter(Func<IDevice, bool> filter);
	}
	internal class DeviceDiscoverer : Adapter.BackgroundTaskObject, IDeviceDiscoverer, System.Collections.Generic.IEnumerable<IRemoteDevice>, System.Collections.IEnumerable
	{
		private class DeviceCache : Disposable
		{
			private static readonly TimeSpan DEVICE_TIMEOUT = TimeSpan.FromSeconds(5.0);

			private static readonly TimeSpan FORCE_STATUS_MESSAGE_TIME = TimeSpan.FromSeconds(3.5);

			private readonly object Lock = new object();

			private readonly DeviceDiscoverer Parent;

			private readonly Adapter Adapter;

			private readonly ADDRESS Address;

			private readonly MAC MAC = new MAC();

			private readonly MAC TempMAC = new MAC();

			private RemoteDevice mDevice;

			private IDS_CAN_VERSION_NUMBER ProtocolVersion = IDS_CAN_VERSION_NUMBER.UNKNOWN;

			private DEVICE_ID? DeviceID;

			private CIRCUIT_ID? CircuitID;

			private PAYLOAD? DeviceStatus;

			private Timer LastNetworkMsgTime = new Timer(true);

			private Timer DeviceDetectedTime = new Timer(true);

			private bool HaveMAC => ProtocolVersion != IDS_CAN_VERSION_NUMBER.UNKNOWN;

			public RemoteDevice Device
			{
				get
				{
					return mDevice;
				}
				private set
				{
					//IL_0102: Unknown result type (might be due to invalid IL or missing references)
					//IL_010c: Expected O, but got Unknown
					if (mDevice == value)
					{
						return;
					}
					RemoteDevice remoteDevice;
					lock (Lock)
					{
						remoteDevice = Interlocked.Exchange<RemoteDevice>(ref mDevice, value);
						if (remoteDevice != null)
						{
							RemoteDevice remoteDevice2 = default(RemoteDevice);
							Parent.Dictionary.TryRemove(remoteDevice.GetDeviceUniqueID(), ref remoteDevice2);
						}
						if (value != null)
						{
							Parent.Dictionary.AddOrUpdate(value.GetDeviceUniqueID(), value, (Func<ulong, RemoteDevice, RemoteDevice>)((ulong key, RemoteDevice val) => value));
						}
						if (value != null)
						{
							if (remoteDevice == null)
							{
								Interlocked.Increment(ref Parent.mNumDevicesDetectedOnNetwork);
							}
						}
						else if (remoteDevice != null)
						{
							Interlocked.Decrement(ref Parent.mNumDevicesDetectedOnNetwork);
						}
					}
					if (((Disposable)this).IsDisposed)
					{
						if (remoteDevice != null)
						{
							((Disposable)remoteDevice).Dispose();
						}
						return;
					}
					remoteDevice?.GoOffline();
					if (value != null)
					{
						System.Threading.Tasks.Task.Run((Action)delegate
						{
							((Adapter)Adapter).Events.Publish<RemoteDeviceOnlineEvent>(new RemoteDeviceOnlineEvent((IEventSender)(object)Adapter, value));
						});
					}
				}
			}

			public DeviceCache(DeviceDiscoverer parent, Adapter adapter, ADDRESS address)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Expected O, but got Unknown
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Expected O, but got Unknown
				Parent = parent;
				Adapter = adapter;
				Address = address;
			}

			public override void Dispose(bool disposing)
			{
				if (disposing)
				{
					TakeDeviceOffline();
				}
			}

			public void ForceDeviceOnline(ILocalDevice device)
			{
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				if (!((Disposable)this).IsDisposed)
				{
					TakeDeviceOffline();
					((PhysicalAddress)MAC).CopyFrom((IPhysicalAddress)(object)device.MAC);
					ProtocolVersion = device.ProtocolVersion;
					DeviceID = device.GetDeviceID();
					CircuitID = device.CircuitID;
					DeviceStatus = device.DeviceStatus;
					DeviceDetectedTime.Reset();
					LastNetworkMsgTime.Reset();
					TryCreateRemoteDevice();
				}
			}

			private void TryCreateRemoteDevice()
			{
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				RemoteDevice remoteDevice = null;
				try
				{
					remoteDevice = new RemoteDevice(Adapter, Address, MAC, ProtocolVersion, DeviceID.Value, CircuitID.Value, DeviceStatus.Value);
				}
				catch
				{
					return;
				}
				Device = remoteDevice;
			}

			public void TakeDeviceOffline()
			{
				Device = null;
				((PhysicalAddress)MAC).Clear();
				ProtocolVersion = IDS_CAN_VERSION_NUMBER.UNKNOWN;
				CircuitID = null;
				DeviceID = null;
				DeviceStatus = null;
			}

			public void BackgroundTask()
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				if (HaveMAC && LastNetworkMsgTime.ElapsedTime >= DEVICE_TIMEOUT)
				{
					TakeDeviceOffline();
					return;
				}
				RemoteDevice device = Device;
				if (device != null)
				{
					if (device.IsOnline)
					{
						device.BackgroundTask();
					}
					else
					{
						TakeDeviceOffline();
					}
				}
			}

			public void OnAdapterMessageRx(AdapterRxEvent rx)
			{
				//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
				if (((Disposable)this).IsDisposed || !((IAdapter)Parent.Adapter).IsConnected)
				{
					return;
				}
				switch (rx.MessageType)
				{
				case 0:
				{
					if (!TempMAC.UnloadFromMessage(rx))
					{
						break;
					}
					IDS_CAN_VERSION_NUMBER iDS_CAN_VERSION_NUMBER = rx[1];
					LastNetworkMsgTime.Reset();
					if ((PhysicalAddress)(object)MAC != (PhysicalAddress)(object)TempMAC || ProtocolVersion != iDS_CAN_VERSION_NUMBER)
					{
						TakeDeviceOffline();
						((PhysicalAddress)MAC).CopyFrom((IPhysicalAddress)(object)TempMAC);
						ProtocolVersion = iDS_CAN_VERSION_NUMBER;
						DeviceDetectedTime.Reset();
						break;
					}
					if (!DeviceStatus.HasValue && DeviceDetectedTime.ElapsedTime >= FORCE_STATUS_MESSAGE_TIME)
					{
						DeviceStatus = default(PAYLOAD);
					}
					if (Device == null && ProtocolVersion != IDS_CAN_VERSION_NUMBER.UNKNOWN && DeviceID.HasValue && CircuitID.HasValue && DeviceStatus.HasValue)
					{
						TryCreateRemoteDevice();
					}
					break;
				}
				case 1:
					if (HaveMAC && rx.Count >= 4)
					{
						CircuitID = (CIRCUIT_ID)CommExtensions.GetUINT32((IByteList)(object)rx, 0);
					}
					break;
				case 2:
					if (HaveMAC && rx.Count >= 7)
					{
						if (rx.Count >= 8)
						{
							DeviceID = new DEVICE_ID(PRODUCT_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0)), rx[2], DEVICE_TYPE.op_Implicit(rx[3]), rx[6] >> 4, FUNCTION_NAME.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 4)), rx[6] & 0xF, rx[7]);
						}
						else
						{
							DeviceID = new DEVICE_ID(PRODUCT_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0)), rx[2], DEVICE_TYPE.op_Implicit(rx[3]), rx[6] >> 4, FUNCTION_NAME.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 4)), rx[6] & 0xF, null);
						}
						RemoteDevice device = Device;
						if (device != null && (device.ProductID != DeviceID?.ProductID || device.DeviceType != DeviceID?.DeviceType || device.DeviceInstance != DeviceID?.DeviceInstance))
						{
							TakeDeviceOffline();
						}
					}
					break;
				case 3:
					if (HaveMAC)
					{
						DeviceStatus = rx.Payload;
					}
					break;
				}
				Device?.OnAdapterMessageRx(rx);
			}

			public void OnBroadcastAddressClaim(AdapterRxEvent rx)
			{
				if (rx.SourceAddress == ADDRESS.BROADCAST && (byte)rx.MessageType == 0 && rx.Count == 8 && rx[0] == (byte)Address && HaveMAC && TempMAC.UnloadFromMessage(rx) && ((PhysicalAddress)TempMAC).CompareTo((PhysicalAddress)(object)MAC) < 0)
				{
					TakeDeviceOffline();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GetAllDevicesMatchingFilter>d__11 : System.Collections.Generic.IEnumerable<IRemoteDevice>, System.Collections.IEnumerable, System.Collections.Generic.IEnumerator<IRemoteDevice>, System.Collections.IEnumerator, System.IDisposable
		{
			private int <>1__state;

			private IRemoteDevice <>2__current;

			private int <>l__initialThreadId;

			public DeviceDiscoverer <>4__this;

			private Func<IDevice, bool> filter;

			public Func<IDevice, bool> <>3__filter;

			private System.Collections.Generic.IEnumerator<RemoteDevice> <>7__wrap1;

			IRemoteDevice System.Collections.Generic.IEnumerator<IRemoteDevice>.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			object System.Collections.IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			[DebuggerHidden]
			public <GetAllDevicesMatchingFilter>d__11(int <>1__state)
			{
				this.<>1__state = <>1__state;
				<>l__initialThreadId = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void System.IDisposable.Dispose()
			{
				int num = <>1__state;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						<>m__Finally1();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int num = <>1__state;
					DeviceDiscoverer deviceDiscoverer = <>4__this;
					switch (num)
					{
					default:
						return false;
					case 0:
						<>1__state = -1;
						<>7__wrap1 = deviceDiscoverer.GetEnumerator();
						<>1__state = -3;
						break;
					case 1:
						<>1__state = -3;
						break;
					}
					while (((System.Collections.IEnumerator)<>7__wrap1).MoveNext())
					{
						RemoteDevice current = <>7__wrap1.Current;
						if (current != null && filter.Invoke((IDevice)current))
						{
							<>2__current = current;
							<>1__state = 1;
							return true;
						}
					}
					<>m__Finally1();
					<>7__wrap1 = null;
					return false;
				}
				catch
				{
					//try-fault
					((System.IDisposable)this).Dispose();
					throw;
				}
			}

			bool System.Collections.IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void <>m__Finally1()
			{
				<>1__state = -1;
				if (<>7__wrap1 != null)
				{
					((System.IDisposable)<>7__wrap1).Dispose();
				}
			}

			[DebuggerHidden]
			void System.Collections.IEnumerator.Reset()
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			System.Collections.Generic.IEnumerator<IRemoteDevice> System.Collections.Generic.IEnumerable<IRemoteDevice>.GetEnumerator()
			{
				<GetAllDevicesMatchingFilter>d__11 <GetAllDevicesMatchingFilter>d__;
				if (<>1__state == -2 && <>l__initialThreadId == Environment.CurrentManagedThreadId)
				{
					<>1__state = 0;
					<GetAllDevicesMatchingFilter>d__ = this;
				}
				else
				{
					<GetAllDevicesMatchingFilter>d__ = new <GetAllDevicesMatchingFilter>d__11(0)
					{
						<>4__this = <>4__this
					};
				}
				<GetAllDevicesMatchingFilter>d__.filter = <>3__filter;
				return <GetAllDevicesMatchingFilter>d__;
			}

			[DebuggerHidden]
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return (System.Collections.IEnumerator)((System.Collections.Generic.IEnumerable<IRemoteDevice>)this).GetEnumerator();
			}
		}

		[CompilerGenerated]
		private sealed class <GetEnumerator>d__10 : System.Collections.Generic.IEnumerator<RemoteDevice>, System.Collections.IEnumerator, System.IDisposable
		{
			private int <>1__state;

			private RemoteDevice <>2__current;

			public DeviceDiscoverer <>4__this;

			private DeviceCache[] <>7__wrap1;

			private int <>7__wrap2;

			RemoteDevice System.Collections.Generic.IEnumerator<RemoteDevice>.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			object System.Collections.IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			[DebuggerHidden]
			public <GetEnumerator>d__10(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}

			[DebuggerHidden]
			void System.IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int num = <>1__state;
				DeviceDiscoverer deviceDiscoverer = <>4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					<>1__state = -1;
					goto IL_007c;
				}
				<>1__state = -1;
				if (((IAdapter)deviceDiscoverer.Adapter).IsConnected)
				{
					<>7__wrap1 = deviceDiscoverer.Cache;
					<>7__wrap2 = 0;
					goto IL_008a;
				}
				goto IL_00a1;
				IL_007c:
				<>7__wrap2++;
				goto IL_008a;
				IL_008a:
				if (<>7__wrap2 < <>7__wrap1.Length)
				{
					RemoteDevice remoteDevice = <>7__wrap1[<>7__wrap2]?.Device;
					if (remoteDevice != null && remoteDevice.IsOnline)
					{
						<>2__current = remoteDevice;
						<>1__state = 1;
						return true;
					}
					goto IL_007c;
				}
				<>7__wrap1 = null;
				goto IL_00a1;
				IL_00a1:
				return false;
			}

			bool System.Collections.IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void System.Collections.IEnumerator.Reset()
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotSupportedException();
			}
		}

		private readonly DeviceCache[] Cache = new DeviceCache[256];

		private readonly ConcurrentDictionary<ulong, RemoteDevice> Dictionary = new ConcurrentDictionary<ulong, RemoteDevice>();

		private int mNumDevicesDetectedOnNetwork;

		public int NumDevicesDetectedOnNetwork => mNumDevicesDetectedOnNetwork;

		public DeviceDiscoverer(Adapter adapter)
			: base(adapter)
		{
			System.Collections.Generic.IEnumerator<ADDRESS> enumerator = ADDRESS.GetEnumerator().GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					ADDRESS current = enumerator.Current;
					if (current.IsValidDeviceAddress)
					{
						Cache[(byte)current] = new DeviceCache(this, adapter, current);
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			((IEventSender)base.Adapter).Events.Subscribe<AdapterOpenedEvent>((Action<AdapterOpenedEvent>)OnAdapterOpened, (SubscriptionType)1, base.Subscriptions);
			((IEventSender)base.Adapter).Events.Subscribe<AdapterClosedEvent>((Action<AdapterClosedEvent>)OnAdapterClosed, (SubscriptionType)1, base.Subscriptions);
			((IEventSender)base.Adapter).Events.Subscribe<AdapterRxEvent>((Action<AdapterRxEvent>)OnAdapterRx, (SubscriptionType)1, base.Subscriptions);
			((IEventSender)base.Adapter).Events.Subscribe<LocalDeviceOnlineEvent>((Action<LocalDeviceOnlineEvent>)OnLocalDeviceOnline, (SubscriptionType)1, base.Subscriptions);
			((IEventSender)base.Adapter).Events.Subscribe<LocalDeviceOfflineEvent>((Action<LocalDeviceOfflineEvent>)OnLocalDeviceOffline, (SubscriptionType)1, base.Subscriptions);
		}

		public override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			DeviceCache[] cache = Cache;
			foreach (DeviceCache obj in cache)
			{
				if (obj != null)
				{
					((Disposable)obj).Dispose();
				}
			}
		}

		System.Collections.Generic.IEnumerator<IRemoteDevice> System.Collections.Generic.IEnumerable<IRemoteDevice>.GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerator<IRemoteDevice>)GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator();
		}

		[IteratorStateMachine(typeof(<GetEnumerator>d__10))]
		public System.Collections.Generic.IEnumerator<RemoteDevice> GetEnumerator()
		{
			if (!((IAdapter)base.Adapter).IsConnected)
			{
				yield break;
			}
			DeviceCache[] cache = Cache;
			for (int i = 0; i < cache.Length; i++)
			{
				RemoteDevice remoteDevice = cache[i]?.Device;
				if (remoteDevice != null && remoteDevice.IsOnline)
				{
					yield return remoteDevice;
				}
			}
		}

		[IteratorStateMachine(typeof(<GetAllDevicesMatchingFilter>d__11))]
		public System.Collections.Generic.IEnumerable<IRemoteDevice> GetAllDevicesMatchingFilter(Func<IDevice, bool> filter)
		{
			System.Collections.Generic.IEnumerator<RemoteDevice> enumerator = GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					RemoteDevice current = enumerator.Current;
					if (current != null && filter.Invoke((IDevice)current))
					{
						yield return current;
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		public IRemoteDevice GetDeviceByAddress(ADDRESS address)
		{
			if (address == null || !address.IsValidDeviceAddress)
			{
				return null;
			}
			return Cache[(byte)address]?.Device;
		}

		public IRemoteDevice GetDeviceByUniqueID(ulong unique_id)
		{
			RemoteDevice result = default(RemoteDevice);
			Dictionary.TryGetValue(unique_id, ref result);
			return result;
		}

		private void KillAllDevices()
		{
			DeviceCache[] cache = Cache;
			for (int i = 0; i < cache.Length; i++)
			{
				cache[i]?.TakeDeviceOffline();
			}
		}

		private void OnAdapterOpened(AdapterOpenedEvent message)
		{
			KillAllDevices();
		}

		private void OnAdapterClosed(AdapterClosedEvent message)
		{
			KillAllDevices();
		}

		private void OnLocalDeviceOnline(LocalDeviceOnlineEvent message)
		{
			Cache[(byte)message.Device.Address]?.ForceDeviceOnline(message.Device);
		}

		private void OnLocalDeviceOffline(LocalDeviceOfflineEvent message)
		{
			Cache[(byte)message.PrevAddress]?.TakeDeviceOffline();
		}

		private void OnAdapterRx(AdapterRxEvent rx)
		{
			if (rx.SourceAddress == ADDRESS.BROADCAST && (byte)rx.MessageType == 0 && rx.Count == 8)
			{
				ADDRESS aDDRESS = rx[0];
				Cache[(byte)aDDRESS]?.OnBroadcastAddressClaim(rx);
			}
			Cache[(byte)rx.SourceAddress]?.OnAdapterMessageRx(rx);
		}

		public override void BackgroundTask()
		{
			DeviceCache[] cache = Cache;
			for (int i = 0; i < cache.Length; i++)
			{
				cache[i]?.BackgroundTask();
			}
		}
	}
	public interface IProductDTC
	{
		IRemoteProduct Product { get; }

		DTC_ID ID { get; }

		bool IsActive { get; }

		bool IsStored { get; }

		int PowerCyclesCounter { get; }

		string Name { get; }
	}
	internal class ProductDTC : IProductDTC
	{
		[field: CompilerGenerated]
		public IRemoteProduct Product
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public bool IsActive => (Status & 0x80) != 0;

		public bool IsStored => Status != 0;

		public int PowerCyclesCounter => Status & 0x7F;

		[field: CompilerGenerated]
		public DTC_ID ID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public string Name => ((object)ID/*cast due to .constrained prefix*/).ToString();

		[field: CompilerGenerated]
		public byte Status
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public ProductDTC(IRemoteProduct product, DTC_ID id, byte status)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			Product = product;
			ID = id;
			Status = status;
		}

		public virtual string ToString()
		{
			return Name;
		}

		public static implicit operator DTC_ID(ProductDTC value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return value.ID;
		}

		public bool Update(byte status)
		{
			bool isActive = IsActive;
			bool isStored = IsStored;
			int powerCyclesCounter = PowerCyclesCounter;
			Status = status;
			if (IsActive == isActive && IsStored == isStored)
			{
				return PowerCyclesCounter != powerCyclesCounter;
			}
			return true;
		}
	}
	public interface IDTCManager : System.Collections.Generic.IEnumerable<IProductDTC>, System.Collections.IEnumerable
	{
		IRemoteProduct Product { get; }

		bool AreSupported { get; }

		bool HasActiveDTCs { get; }

		bool HasStoredDTCs { get; }

		int Count { get; }

		System.Collections.Generic.IEnumerator<IProductDTC> ActiveDTCs { get; }

		int ActiveCount { get; }

		System.Collections.Generic.IEnumerator<IProductDTC> StoredDTCs { get; }

		int StoredCount { get; }

		void QueryProduct();

		bool Contains(DTC_ID id);
	}
	[DefaultMember("Item")]
	internal class DTCManager : Disposable, IDTCManager, System.Collections.Generic.IEnumerable<IProductDTC>, System.Collections.IEnumerable
	{
		[CompilerGenerated]
		private sealed class <GetActiveEnumerator>d__40 : System.Collections.Generic.IEnumerator<IProductDTC>, System.Collections.IEnumerator, System.IDisposable
		{
			private int <>1__state;

			private IProductDTC <>2__current;

			public DTCManager <>4__this;

			private System.Collections.Generic.IEnumerator<IProductDTC> <>7__wrap1;

			IProductDTC System.Collections.Generic.IEnumerator<IProductDTC>.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			object System.Collections.IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			[DebuggerHidden]
			public <GetActiveEnumerator>d__40(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}

			[DebuggerHidden]
			void System.IDisposable.Dispose()
			{
				int num = <>1__state;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						<>m__Finally1();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int num = <>1__state;
					DTCManager dTCManager = <>4__this;
					switch (num)
					{
					default:
						return false;
					case 0:
						<>1__state = -1;
						<>7__wrap1 = dTCManager.GetEnumerator();
						<>1__state = -3;
						break;
					case 1:
						<>1__state = -3;
						break;
					}
					while (((System.Collections.IEnumerator)<>7__wrap1).MoveNext())
					{
						IProductDTC current = <>7__wrap1.Current;
						if (current != null && current.IsActive)
						{
							<>2__current = current;
							<>1__state = 1;
							return true;
						}
					}
					<>m__Finally1();
					<>7__wrap1 = null;
					return false;
				}
				catch
				{
					//try-fault
					((System.IDisposable)this).Dispose();
					throw;
				}
			}

			bool System.Collections.IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void <>m__Finally1()
			{
				<>1__state = -1;
				if (<>7__wrap1 != null)
				{
					((System.IDisposable)<>7__wrap1).Dispose();
				}
			}

			[DebuggerHidden]
			void System.Collections.IEnumerator.Reset()
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotSupportedException();
			}
		}

		[CompilerGenerated]
		private sealed class <GetStoredEnumerator>d__41 : System.Collections.Generic.IEnumerator<IProductDTC>, System.Collections.IEnumerator, System.IDisposable
		{
			private int <>1__state;

			private IProductDTC <>2__current;

			public DTCManager <>4__this;

			private System.Collections.Generic.IEnumerator<IProductDTC> <>7__wrap1;

			IProductDTC System.Collections.Generic.IEnumerator<IProductDTC>.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			object System.Collections.IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			[DebuggerHidden]
			public <GetStoredEnumerator>d__41(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}

			[DebuggerHidden]
			void System.IDisposable.Dispose()
			{
				int num = <>1__state;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						<>m__Finally1();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int num = <>1__state;
					DTCManager dTCManager = <>4__this;
					switch (num)
					{
					default:
						return false;
					case 0:
						<>1__state = -1;
						<>7__wrap1 = dTCManager.GetEnumerator();
						<>1__state = -3;
						break;
					case 1:
						<>1__state = -3;
						break;
					}
					while (((System.Collections.IEnumerator)<>7__wrap1).MoveNext())
					{
						IProductDTC current = <>7__wrap1.Current;
						if (current != null && current.IsStored)
						{
							<>2__current = current;
							<>1__state = 1;
							return true;
						}
					}
					<>m__Finally1();
					<>7__wrap1 = null;
					return false;
				}
				catch
				{
					//try-fault
					((System.IDisposable)this).Dispose();
					throw;
				}
			}

			bool System.Collections.IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void <>m__Finally1()
			{
				<>1__state = -1;
				if (<>7__wrap1 != null)
				{
					((System.IDisposable)<>7__wrap1).Dispose();
				}
			}

			[DebuggerHidden]
			void System.Collections.IEnumerator.Reset()
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotSupportedException();
			}
		}

		private static readonly TimeSpan TIMEOUT = TimeSpan.FromSeconds(1.0);

		private readonly IAdapter Adapter;

		private ProductDTC[] DTCs;

		private readonly List<IProductDTC> SortedList = new List<IProductDTC>();

		private readonly Dictionary<DTC_ID, ProductDTC> Lookup = new Dictionary<DTC_ID, ProductDTC>();

		private readonly SubscriptionManager Subscriptions = new SubscriptionManager();

		private readonly Timer RetryTimer = new Timer(true);

		private readonly Timer OperationTimer = new Timer(true);

		private bool ReadingList;

		private ushort DtcIndex;

		private ProductDTCsChangedEvent ProductDTCsChangedEvent;

		[field: CompilerGenerated]
		public IRemoteProduct Product
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public bool AreSupported
		{
			get
			{
				ProductDTC[] dTCs = DTCs;
				if (dTCs == null)
				{
					return false;
				}
				return dTCs.Length != 0;
			}
		}

		public bool HasActiveDTCs => GetDeviceAtProductAddress()?.NetworkStatus.HasActiveDTCs ?? false;

		public bool HasStoredDTCs => GetDeviceAtProductAddress()?.NetworkStatus.HasStoredDTCs ?? false;

		public int Count => SortedList.Count;

		public System.Collections.Generic.IEnumerator<IProductDTC> ActiveDTCs => GetActiveEnumerator();

		public System.Collections.Generic.IEnumerator<IProductDTC> StoredDTCs => GetStoredEnumerator();

		[field: CompilerGenerated]
		public int ActiveCount
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public int StoredCount
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		private ProductDTC this[DTC_ID id]
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				ProductDTC result = default(ProductDTC);
				Lookup.TryGetValue(id, ref result);
				return result;
			}
		}

		public DTCManager(IRemoteProduct product)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			Product = product;
			Adapter = Product.Adapter;
			ProductDTCsChangedEvent = new ProductDTCsChangedEvent(Product);
			RetryTimer.ElapsedTime = TIMEOUT;
			((IEventSender)Adapter).Events.Subscribe<TransmitTurnEvent>((Action<TransmitTurnEvent>)OnTransmitNextMessage, (SubscriptionType)0, Subscriptions);
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((Disposable)Subscriptions).Dispose();
			}
		}

		private IRemoteDevice GetDeviceAtProductAddress()
		{
			return Adapter.Devices.GetDeviceByAddress(Product.Address);
		}

		public System.Collections.Generic.IEnumerator<IProductDTC> GetEnumerator()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return (System.Collections.Generic.IEnumerator<IProductDTC>)(object)SortedList.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator();
		}

		[IteratorStateMachine(typeof(<GetActiveEnumerator>d__40))]
		public System.Collections.Generic.IEnumerator<IProductDTC> GetActiveEnumerator()
		{
			System.Collections.Generic.IEnumerator<IProductDTC> enumerator = GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					IProductDTC current = enumerator.Current;
					if (current != null && current.IsActive)
					{
						yield return current;
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		[IteratorStateMachine(typeof(<GetStoredEnumerator>d__41))]
		public System.Collections.Generic.IEnumerator<IProductDTC> GetStoredEnumerator()
		{
			System.Collections.Generic.IEnumerator<IProductDTC> enumerator = GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					IProductDTC current = enumerator.Current;
					if (current != null && current.IsStored)
					{
						yield return current;
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		private void Publish(IProductDTC dtc)
		{
			int num = 0;
			int num2 = 0;
			ProductDTC[] dTCs = DTCs;
			foreach (ProductDTC obj in dTCs)
			{
				if (obj.IsActive)
				{
					num++;
				}
				if (obj.IsStored)
				{
					num2++;
				}
			}
			ActiveCount = num;
			StoredCount = num2;
			ProductDTCsChangedEvent.Publish(dtc);
		}

		public void QueryProduct()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (!ReadingList && (DTCs == null || DTCs.Length != 0))
			{
				OperationTimer.Reset();
				ReadingList = true;
				DtcIndex = 0;
				RetryTimer.ElapsedTime = TIMEOUT;
			}
		}

		public bool Contains(DTC_ID id)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return this[id] != null;
		}

		public IProductDTC GetDTC(DTC_ID id)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return this[id];
		}

		public void OnProductTx(AdapterRxEvent tx)
		{
			IRemoteDevice deviceAtProductAddress = GetDeviceAtProductAddress();
			if (deviceAtProductAddress != null && deviceAtProductAddress.IsOnline && (byte)tx.MessageType == 129 && tx.TargetAddress == Adapter.LocalHost.Address)
			{
				switch (tx.MessageData)
				{
				case 49:
					OnContinuousDTCCommand(tx);
					break;
				case 48:
					OnReadContinuousDTCs(tx);
					break;
				}
			}
		}

		private void BadResponse(REQUEST request, RESPONSE response)
		{
			ReadingList = false;
			DTCs = new ProductDTC[0];
			SortedList.Clear();
			ActiveCount = 0;
			StoredCount = 0;
		}

		private void OnContinuousDTCCommand(AdapterRxEvent tx)
		{
			if (tx.Count == 1)
			{
				if (DTCs == null)
				{
					BadResponse((byte)49, (RESPONSE)tx[0]);
				}
			}
			else if (tx.Count == 8)
			{
				int uINT = CommExtensions.GetUINT16((IByteList)(object)tx, 2);
				if (DTCs == null)
				{
					DTCs = new ProductDTC[uINT];
				}
			}
		}

		private void OnReadContinuousDTCs(AdapterRxEvent tx)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			if (!AreSupported || !ReadingList || DTCs == null)
			{
				return;
			}
			switch (tx.Count)
			{
			case 1:
				BadResponse((byte)48, (RESPONSE)tx[0]);
				break;
			case 8:
			{
				ushort uINT = CommExtensions.GetUINT16((IByteList)(object)tx, 0);
				int num = uINT * 2;
				int num2 = 2;
				while (num2 < 8 && num < DTCs.Length)
				{
					DTC_ID val = (DTC_ID)CommExtensions.GetUINT16((IByteList)(object)tx, num2);
					if ((int)val != 0)
					{
						byte status = tx[num2 + 2];
						if (DTCs[num] == null)
						{
							DTCs[num] = new ProductDTC(Product, val, status);
						}
						else if (DTCs[num].Update(status) && Count > 0)
						{
							Publish(DTCs[num]);
						}
					}
					num2 += 3;
					num++;
				}
				if (num >= DTCs.Length)
				{
					ReadingList = false;
					if (SortedList.Count == DTCs.Length)
					{
						break;
					}
					SortedList.Clear();
					ProductDTC[] dTCs = DTCs;
					foreach (ProductDTC productDTC in dTCs)
					{
						Lookup.Add(productDTC.ID, productDTC);
						SortedList.Add((IProductDTC)productDTC);
					}
					SortedList.Sort((Comparison<IProductDTC>)((IProductDTC first, IProductDTC second) => ((System.Enum)first.ID/*cast due to .constrained prefix*/).CompareTo((object)second.ID)));
					System.Collections.Generic.IEnumerator<IProductDTC> enumerator = GetEnumerator();
					try
					{
						while (((System.Collections.IEnumerator)enumerator).MoveNext())
						{
							IProductDTC current = enumerator.Current;
							Publish(current);
						}
						break;
					}
					finally
					{
						((System.IDisposable)enumerator)?.Dispose();
					}
				}
				if (ReadingList)
				{
					if (uINT == DtcIndex)
					{
						DtcIndex++;
					}
					RetryTimer.ElapsedTime = TIMEOUT;
				}
				break;
			}
			}
		}

		private void OnTransmitNextMessage(TransmitTurnEvent e)
		{
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			if (!Product.IsOnline || !Adapter.LocalHost.IsOnline)
			{
				return;
			}
			IRemoteDevice deviceAtProductAddress = GetDeviceAtProductAddress();
			if (deviceAtProductAddress == null || !deviceAtProductAddress.IsOnline)
			{
				return;
			}
			if (Count > 0)
			{
				if (!deviceAtProductAddress.NetworkStatus.HasDTCs)
				{
					if (ActiveCount <= 0 && StoredCount <= 0)
					{
						return;
					}
					ProductDTC[] dTCs = DTCs;
					foreach (ProductDTC productDTC in dTCs)
					{
						if (productDTC.Update(0))
						{
							Publish(productDTC);
						}
					}
					return;
				}
				if (!HasActiveDTCs && ActiveCount > 0)
				{
					ProductDTC[] dTCs = DTCs;
					foreach (ProductDTC productDTC2 in dTCs)
					{
						if (productDTC2.Update((byte)(productDTC2.Status & 0x7F)))
						{
							Publish(productDTC2);
						}
					}
				}
			}
			if (RetryTimer.ElapsedTime < TIMEOUT)
			{
				return;
			}
			if (DTCs == null)
			{
				if (ReadingList || ((System.Enum)Adapter.Options).HasFlag((System.Enum)ADAPTER_OPTIONS.AUTO_READ_DTC_COUNT))
				{
					e.Handled = Adapter.LocalHost.Transmit29((byte)128, 49, deviceAtProductAddress, PAYLOAD.FromArgs(new object[1] { (byte)0 }));
					if (e.Handled)
					{
						RetryTimer.Reset();
					}
				}
			}
			else if (ReadingList && DTCs.Length != 0)
			{
				e.Handled = Adapter.LocalHost.Transmit29((byte)128, 48, deviceAtProductAddress, PAYLOAD.FromArgs(new object[1] { DtcIndex }));
				if (e.Handled)
				{
					RetryTimer.Reset();
				}
			}
		}
	}
	[DefaultMember("Item")]
	public class AdapterRxEvent : Event, IMessage, IMessage, IByteList, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, ITimeStamp, IReadOnlyPacket
	{
		public readonly IAdapter Adapter;

		private IMessage Message;

		private CAN_ID IdsCanId;

		[field: CompilerGenerated]
		public bool Echo
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public MESSAGE_TYPE MessageType => IdsCanId.MessageType;

		public ADDRESS SourceAddress => IdsCanId.SourceAddress;

		public ADDRESS TargetAddress => IdsCanId.TargetAddress;

		public byte MessageData => IdsCanId.MessageData;

		[field: CompilerGenerated]
		public PAYLOAD Payload
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public int Length => ((IByteList)Message).Length;

		public int Count => ((System.Collections.Generic.IReadOnlyCollection<byte>)Message).Count;

		public ID ID => ((IReadOnlyPacket)Message).ID;

		public TimeSpan Timestamp => ((ITimeStamp)Message).Timestamp;

		public byte this[int index] => ((System.Collections.Generic.IReadOnlyList<byte>)Message)[index];

		public IDevice SourceDevice => Adapter.Devices.GetDeviceByAddress(SourceAddress);

		public IDevice TargetDevice => Adapter.Devices.GetDeviceByAddress(TargetAddress);

		public AdapterRxEvent(IAdapter a)
			: base((object)a)
		{
			Adapter = a;
		}

		public void Publish(IMessage msg, bool echo)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			Message = msg;
			Echo = echo;
			IdsCanId = ((IReadOnlyPacket)msg).ID;
			Payload = new PAYLOAD((System.Collections.Generic.IReadOnlyList<byte>)msg);
			((Event)this).Publish();
		}

		public void CopyTo(byte[] array, int index)
		{
			((IByteList)Message).CopyTo(array, index);
		}

		public void CopyRangeTo(int sourceIndex, int count, byte[] array, int destIndex)
		{
			((IByteList)Message).CopyRangeTo(sourceIndex, count, array, destIndex);
		}

		public System.Collections.Generic.IEnumerator<byte> GetEnumerator()
		{
			return ((System.Collections.Generic.IEnumerable<byte>)Message).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)((System.Collections.Generic.IEnumerable<byte>)Message).GetEnumerator();
		}

		public override string ToString()
		{
			return ((object)Message).ToString();
		}

		public string ToString(bool dataonly)
		{
			return ((IByteList)Message).ToString(dataonly);
		}
	}
	[DefaultMember("Item")]
	public class LocalDeviceRxEvent : Event, IMessage, IMessage, IByteList, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, ITimeStamp, IReadOnlyPacket
	{
		public readonly ILocalDevice LocalDevice;

		private AdapterRxEvent Rx;

		public bool Echo => Rx.Echo;

		public MESSAGE_TYPE MessageType => Rx.MessageType;

		public ADDRESS SourceAddress => Rx.SourceAddress;

		public ADDRESS TargetAddress => Rx.TargetAddress;

		public byte MessageData => Rx.MessageData;

		public PAYLOAD Payload => Rx.Payload;

		public int Length => Rx.Length;

		public ID ID => ((IReadOnlyPacket)Rx).ID;

		public TimeSpan Timestamp => Rx.Timestamp;

		public int Count => ((System.Collections.Generic.IReadOnlyCollection<byte>)Rx).Count;

		public byte this[int index] => Rx[index];

		public LocalDeviceRxEvent(ILocalDevice localdevice)
			: base((object)localdevice)
		{
			LocalDevice = localdevice;
		}

		public void Publish(AdapterRxEvent rx)
		{
			if (LocalDevice.IsOnline)
			{
				Rx = rx;
				((Event)this).Publish();
			}
		}

		public void CopyTo(byte[] array, int index)
		{
			Rx.CopyTo(array, index);
		}

		public void CopyRangeTo(int sourceIndex, int count, byte[] array, int destIndex)
		{
			Rx.CopyRangeTo(sourceIndex, count, array, destIndex);
		}

		public System.Collections.Generic.IEnumerator<byte> GetEnumerator()
		{
			return ((System.Collections.Generic.IEnumerable<byte>)Rx).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)((System.Collections.Generic.IEnumerable<byte>)Rx).GetEnumerator();
		}

		public override string ToString()
		{
			return ((object)Rx).ToString();
		}

		public string ToString(bool dataonly)
		{
			return Rx.ToString(dataonly);
		}
	}
	public class LocalDeviceOnlineEvent : Event
	{
		public readonly ILocalDevice Device;

		public LocalDeviceOnlineEvent(ILocalDevice device)
			: base((object)device)
		{
			Device = device;
		}
	}
	public class LocalDeviceOfflineEvent : Event
	{
		public readonly ILocalDevice Device;

		[field: CompilerGenerated]
		public ADDRESS PrevAddress
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public LocalDeviceOfflineEvent(ILocalDevice device)
			: base((object)device)
		{
			Device = device;
		}

		public void Publish(ADDRESS a)
		{
			PrevAddress = a;
			((Event)this).Publish();
		}
	}
	public class RemoteDeviceOnlineEvent : Event
	{
		public readonly IRemoteDevice Device;

		public RemoteDeviceOnlineEvent(IEventSender sender, IRemoteDevice device)
			: base((object)sender)
		{
			Device = device;
		}
	}
	public class RemoteDeviceOfflineEvent : Event
	{
		public readonly IRemoteDevice Device;

		[field: CompilerGenerated]
		public ADDRESS PrevAddress
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public RemoteDeviceOfflineEvent(IEventSender sender, IRemoteDevice device)
			: base((object)sender)
		{
			Device = device;
		}

		public void Publish(ADDRESS a)
		{
			PrevAddress = a;
			((Event)this).Publish();
		}
	}
	public class DeviceIDChangedEvent : Event
	{
		public readonly IRemoteDevice Device;

		public DeviceIDChangedEvent(IRemoteDevice device)
			: base((object)device.Adapter)
		{
			Device = device;
		}
	}
	public class CircuitIDChangedEvent : Event
	{
		public readonly IRemoteDevice Device;

		[field: CompilerGenerated]
		public CIRCUIT_ID Prev
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public CircuitIDChangedEvent(IRemoteDevice device)
			: base((object)device.Adapter)
		{
			Device = device;
		}

		public void Publish(CIRCUIT_ID prev)
		{
			Prev = prev;
			((Event)this).Publish();
		}
	}
	public class CircuitListChangedEvent : Event
	{
		public readonly ICircuitManager Circuits;

		public CircuitListChangedEvent(ICircuitManager circuits)
			: base((object)circuits.Adapter)
		{
			Circuits = circuits;
		}
	}
	public class ProductListChangedEvent : Event
	{
		public readonly IProductManager Products;

		public ProductListChangedEvent(IProductManager products)
			: base((object)products.Adapter)
		{
			Products = products;
		}
	}
	public class ClientSessionOpenEvent : Event
	{
		public readonly ISessionClient Session;

		public ClientSessionOpenEvent(ISessionClient session)
			: base((object)session)
		{
			Session = session;
		}
	}
	public class ClientSessionClosedEvent : Event
	{
		public readonly ISessionClient Session;

		public ClientSessionClosedEvent(ISessionClient session)
			: base((object)session)
		{
			Session = session;
		}
	}
	public class PIDUpdatedEvent : Event
	{
		public readonly IDevicePID PID;

		[field: CompilerGenerated]
		public ulong Value
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public PIDUpdatedEvent(IDevicePID pid)
			: base((object)pid)
		{
			PID = pid;
		}

		public void Publish(ulong value)
		{
			Value = value;
			((Event)this).Publish();
		}
	}
	public class PIDValueChangedEvent : Event
	{
		public readonly IDevicePID PID;

		[field: CompilerGenerated]
		public ulong Value
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public PIDValueChangedEvent(IDevicePID pid)
			: base((object)pid)
		{
			PID = pid;
		}

		public void Publish(ulong value)
		{
			Value = value;
			((Event)this).Publish();
		}
	}
	public class TextConsoleTextChangedEvent : Event
	{
		public readonly IRemoteDevice Device;

		public TextConsoleTextChangedEvent(IRemoteDevice device)
			: base((object)device)
		{
			Device = device;
		}
	}
	public class TextConsoleSizeChangedEvent : Event
	{
		public readonly IRemoteDevice Device;

		public TextConsoleSizeChangedEvent(IRemoteDevice device)
			: base((object)device)
		{
			Device = device;
		}
	}
	public class ProductDTCsChangedEvent : Event
	{
		public readonly IRemoteProduct Product;

		[field: CompilerGenerated]
		public IProductDTC DTC
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public ProductDTCsChangedEvent(IRemoteProduct product)
			: base((object)product.Adapter)
		{
			Product = product;
		}

		public void Publish(IProductDTC dtc)
		{
			DTC = dtc;
			((Event)this).Publish();
		}
	}
	public class NetworkInMotionLockoutLevelChangedEvent : Event
	{
		public readonly IAdapter Adapter;

		[field: CompilerGenerated]
		public IN_MOTION_LOCKOUT_LEVEL CurrentLockoutLevel
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		} = (byte)0;

		[field: CompilerGenerated]
		public IN_MOTION_LOCKOUT_LEVEL PreviousLockoutLevel
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		} = (byte)0;

		public NetworkInMotionLockoutLevelChangedEvent(IAdapter a)
			: base((object)a)
		{
			Adapter = a;
		}

		public void Publish(IN_MOTION_LOCKOUT_LEVEL current, IN_MOTION_LOCKOUT_LEVEL prev)
		{
			CurrentLockoutLevel = current;
			PreviousLockoutLevel = prev;
			((Event)this).Publish();
		}
	}
	public class DeviceInMotionLockoutLevelChangedEvent : Event
	{
		public readonly IAdapter Adapter;

		public readonly IDevice Device;

		[field: CompilerGenerated]
		public IN_MOTION_LOCKOUT_LEVEL CurrentLockoutLevel
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		} = (byte)0;

		[field: CompilerGenerated]
		public IN_MOTION_LOCKOUT_LEVEL PreviousLockoutLevel
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		} = (byte)0;

		public DeviceInMotionLockoutLevelChangedEvent(IDevice device)
			: base((object)device.Adapter)
		{
			Device = device;
			Adapter = device.Adapter;
		}

		public void Publish(IN_MOTION_LOCKOUT_LEVEL current, IN_MOTION_LOCKOUT_LEVEL prev)
		{
			CurrentLockoutLevel = current;
			PreviousLockoutLevel = prev;
			((Event)this).Publish();
		}
	}
	public interface ILocalDeviceAsyncMessaging
	{
		System.Threading.Tasks.Task<Tuple<RESPONSE, MessageBuffer>> TransmitRequestAsync(AsyncOperation operation, IBusEndpoint target, REQUEST request, PAYLOAD payload, Func<LocalDeviceRxEvent, RESPONSE?> validator);
	}
	public class LocalDevice : DisposableManager, ILocalDevice, IDevice, IBusEndpoint, IUniqueDeviceInfo, IUniqueProductInfo, ILocalDeviceAsyncMessaging, IPidClient, IBlockClient, IEventSender, IDisposableManager, IDisposable, System.IDisposable
	{
		private class ReusableSubscription : Object
		{
			private readonly object Mutex = new object();

			private LocalDevice Device;

			private SubscriptionToken Subscription;

			private Func<LocalDeviceRxEvent, bool> RxValidator;

			protected override void ResetPoolObjectState()
			{
				lock (Mutex)
				{
					RxValidator = null;
				}
			}

			public void SetDelegate(LocalDevice device, Func<LocalDeviceRxEvent, bool> rx_validator)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				lock (Mutex)
				{
					((Object)this).Retain();
					if (RxValidator != null)
					{
						throw new InvalidOperationException("ReusableLocalDeviceRxEvent delegate is already in use");
					}
					RxValidator = rx_validator;
					if (Subscription == null)
					{
						Device = device;
						Subscription = device.Events.Subscribe<LocalDeviceRxEvent>((Action<LocalDeviceRxEvent>)OnLocalDeviceRxEvent, (SubscriptionType)0);
					}
				}
			}

			private void OnLocalDeviceRxEvent(LocalDeviceRxEvent rx)
			{
				if (RxValidator == null)
				{
					return;
				}
				lock (Mutex)
				{
					Func<LocalDeviceRxEvent, bool> rxValidator = RxValidator;
					if (rxValidator != null && rxValidator.Invoke(rx))
					{
						RxValidator = null;
						((Object)this).ReturnToPool();
					}
				}
			}
		}

		private class BlockClient : IBlockClient
		{
			private class BlockOperationProgressReporter
			{
				public enum TYPE
				{
					READER,
					WRITER
				}

				public readonly AsyncOperation Operation;

				public readonly IBlock Block;

				public readonly LocalDevice Client;

				public readonly IDevice Target;

				private readonly Timer UpdateTime = new Timer(true);

				private readonly Timer OperationTime = new Timer(true);

				private readonly string s1;

				private readonly string s2;

				public BlockOperationProgressReporter(AsyncOperation operation, LocalDevice client, IBlock block, TYPE type)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_000c: Expected O, but got Unknown
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0018: Expected O, but got Unknown
					//IL_0106: Unknown result type (might be due to invalid IL or missing references)
					Operation = operation;
					Block = block;
					Client = client;
					Target = block.Device;
					if (type == TYPE.READER)
					{
						s1 = $"Reading block <{block.ID}> from device @{block.Device.Address}";
						s2 = "Download";
					}
					else
					{
						s1 = $"Writing block <{block.ID}> to device @{block.Device.Address}";
						s2 = "Upload";
					}
					UpdateTime.ElapsedTime = TimeSpan.FromSeconds(1.0);
				}

				public void ReportProgress(ulong num_bytes, ulong total_bytes)
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_000b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					//IL_0037: Unknown result type (might be due to invalid IL or missing references)
					//IL_0125: Unknown result type (might be due to invalid IL or missing references)
					//IL_012a: Unknown result type (might be due to invalid IL or missing references)
					//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
					TimeSpan elapsedTime = UpdateTime.ElapsedTime;
					if (((TimeSpan)(ref elapsedTime)).TotalMilliseconds >= 500.0)
					{
						UpdateTime.Reset();
						elapsedTime = OperationTime.ElapsedTime;
						double totalSeconds = ((TimeSpan)(ref elapsedTime)).TotalSeconds;
						if (num_bytes == 0L || total_bytes == 0L || totalSeconds < 1.0)
						{
							Operation.ReportProgress(0f, $"{s1} ({$"{num_bytes:#,###0}"} of {$"{total_bytes:#,###0}"} bytes)\n{s2} speed = ???\nElapsed Time = {OperationTime}, Remaining Time = ???");
						}
						else
						{
							float num = 100f * (float)num_bytes / (float)total_bytes;
							double num2 = (double)num_bytes / totalSeconds;
							TimeSpan val = TimeSpan.FromSeconds((double)total_bytes / num2 - totalSeconds);
							Operation.ReportProgress(num, $"{s1} ({$"{num_bytes:#,###0}"} of {$"{total_bytes:#,###0}"} bytes)\n{s2} speed = {$"{Math.Round(num2):#,###0}"} bytes per second\nElapsed Time = {OperationTime}, Remaining Time = {Timer.FormatString(val)}");
						}
					}
				}
			}

			private class BlockReadManager : BlockOperationProgressReporter
			{
				[CompilerGenerated]
				private sealed class <>c__DisplayClass10_0
				{
					public bool message_sent;

					public BlockReadManager <>4__this;

					public uint bytes_read;

					public TaskCompletionSource<RESPONSE> tcs;

					public uint Read_xfer_size;

					public bool WaitFewSecondsToMute;

					public bool BulkSuccess;

					public byte sequence;

					internal bool <ReadPartialBlockAsync>b__0(LocalDeviceRxEvent rx)
					{
						if (!message_sent)
						{
							return false;
						}
						if (rx.SourceAddress != <>4__this.Target.Address)
						{
							return false;
						}
						<>4__this.ReportProgress(<>4__this.Offset + bytes_read, <>4__this.Block.Size);
						switch (rx.MessageType)
						{
						case 129:
							switch (rx.MessageData)
							{
							case 34:
								if (rx.Length == 1)
								{
									tcs.SetResult((RESPONSE)rx[0]);
									return true;
								}
								if (rx.Length >= 7 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == BLOCK_ID.op_Implicit(<>4__this.Block.ID) && CommExtensions.GetUINT32((IByteList)(object)rx, 2) == <>4__this.Offset)
								{
									if (rx.Length == 7)
									{
										tcs.SetResult((RESPONSE)rx[6]);
										return true;
									}
									if (rx.Length == 8)
									{
										if ((<>4__this.Block.Flags & BLOCK_FLAGS.USE_SET_SIZE) != BLOCK_FLAGS.NONE)
										{
											if (CommExtensions.GetUINT16((IByteList)(object)rx, 6) == (<>4__this.Block.Size & 0xFFFF))
											{
												Read_xfer_size = (uint)<>4__this.Block.Size;
												if (<>4__this.Offset == 0)
												{
													WaitFewSecondsToMute = true;
												}
											}
										}
										else
										{
											Read_xfer_size = CommExtensions.GetUINT16((IByteList)(object)rx, 6);
										}
										int num2 = (int)(Read_xfer_size + 7) / 8 * 8;
										System.Array.Resize<byte>(ref <>4__this.Buffer, num2);
									}
								}
								return false;
							case 37:
								if (rx.Length == 8 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == BLOCK_ID.op_Implicit(<>4__this.Block.ID) && CommExtensions.GetUINT16((IByteList)(object)rx, 2) == (ushort)<>4__this.Offset)
								{
									uint num = CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyList<byte>)<>4__this.Buffer, (int)bytes_read, <>4__this.Offset);
									uint uINT = CommExtensions.GetUINT32((IByteList)(object)rx, 4);
									bool flag = false;
									if (num != uINT)
									{
										if (CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyList<byte>)<>4__this.Buffer, (int)Read_xfer_size, 0u) == uINT)
										{
											flag = true;
										}
									}
									else
									{
										flag = true;
									}
									if (!flag)
									{
										BulkSuccess = false;
										tcs.SetResult(RESPONSE.IN_PROGRESS);
									}
									else
									{
										BulkSuccess = true;
										if (bytes_read + <>4__this.Offset >= Read_xfer_size)
										{
											tcs.SetResult(RESPONSE.SUCCESS);
										}
										else
										{
											tcs.SetResult(RESPONSE.IN_PROGRESS);
										}
									}
									return true;
								}
								return false;
							default:
								return false;
							}
						case 159:
							if (Read_xfer_size != 0 && rx.MessageData == sequence)
							{
								for (int i = 0; i < rx.Length; i++)
								{
									<>4__this.Buffer[<>4__this.Offset + bytes_read] = rx[i];
									bytes_read++;
								}
								sequence++;
							}
							return false;
						default:
							return false;
						}
					}
				}

				[StructLayout((LayoutKind)3)]
				[CompilerGenerated]
				private struct <ReadBlockAsync>d__11 : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> <>t__builder;

					public BlockReadManager <>4__this;

					public bool BlockUseSession;

					public ISessionClient session;

					private TaskAwaiter<RESPONSE> <>u__1;

					private void MoveNext()
					{
						//IL_005a: Unknown result type (might be due to invalid IL or missing references)
						//IL_005f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0067: Unknown result type (might be due to invalid IL or missing references)
						//IL_0025: Unknown result type (might be due to invalid IL or missing references)
						//IL_002a: Unknown result type (might be due to invalid IL or missing references)
						//IL_003f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0041: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						BlockReadManager blockReadManager = <>4__this;
						Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>> result2;
						try
						{
							if (num != 0)
							{
								goto IL_008b;
							}
							TaskAwaiter<RESPONSE> val = <>u__1;
							<>u__1 = default(TaskAwaiter<RESPONSE>);
							num = (<>1__state = -1);
							goto IL_0076;
							IL_0076:
							RESPONSE result = val.GetResult();
							if (result == RESPONSE.SUCCESS)
							{
								goto IL_008b;
							}
							result2 = Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(result, (System.Collections.Generic.IReadOnlyList<byte>)null);
							goto end_IL_000e;
							IL_008b:
							if (!blockReadManager.TransferComplete && !blockReadManager.Operation.IsCancellationRequested)
							{
								val = blockReadManager.ReadPartialBlockAsync(BlockUseSession, session).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<RESPONSE>, <ReadBlockAsync>d__11>(ref val, ref this);
									return;
								}
								goto IL_0076;
							}
							result2 = ((!blockReadManager.TransferComplete) ? Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(RESPONSE.FAILED, (System.Collections.Generic.IReadOnlyList<byte>)null) : (blockReadManager.CrcValid ? Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(RESPONSE.SUCCESS, (System.Collections.Generic.IReadOnlyList<byte>)blockReadManager.Buffer) : Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(RESPONSE.CRC_INVALID, (System.Collections.Generic.IReadOnlyList<byte>)null)));
							end_IL_000e:;
						}
						catch (System.Exception exception)
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
				private struct <ReadPartialBlockAsync>d__10 : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder<RESPONSE> <>t__builder;

					public BlockReadManager <>4__this;

					public bool BlockUseSession;

					public ISessionClient session;

					private <>c__DisplayClass10_0 <>8__1;

					private int <estimated_xfers>5__2;

					private uint <ErrorReadBulk>5__3;

					private ReusableSubscription <rx_listener>5__4;

					private TaskAwaiter <>u__1;

					private TaskAwaiter<System.Threading.Tasks.Task> <>u__2;

					private void MoveNext()
					{
						//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
						//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
						//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
						//IL_041c: Unknown result type (might be due to invalid IL or missing references)
						//IL_0421: Unknown result type (might be due to invalid IL or missing references)
						//IL_0429: Unknown result type (might be due to invalid IL or missing references)
						//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
						//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
						//IL_0200: Unknown result type (might be due to invalid IL or missing references)
						//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
						//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
						//IL_0294: Unknown result type (might be due to invalid IL or missing references)
						//IL_0299: Unknown result type (might be due to invalid IL or missing references)
						//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
						//IL_0401: Unknown result type (might be due to invalid IL or missing references)
						//IL_0403: Unknown result type (might be due to invalid IL or missing references)
						//IL_0371: Unknown result type (might be due to invalid IL or missing references)
						//IL_0376: Unknown result type (might be due to invalid IL or missing references)
						//IL_038b: Unknown result type (might be due to invalid IL or missing references)
						//IL_038d: Unknown result type (might be due to invalid IL or missing references)
						//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
						//IL_0250: Unknown result type (might be due to invalid IL or missing references)
						//IL_025f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0264: Unknown result type (might be due to invalid IL or missing references)
						//IL_01be: Unknown result type (might be due to invalid IL or missing references)
						//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
						//IL_0279: Unknown result type (might be due to invalid IL or missing references)
						//IL_027b: Unknown result type (might be due to invalid IL or missing references)
						//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
						//IL_01da: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						BlockReadManager blockReadManager = <>4__this;
						RESPONSE result;
						try
						{
							TaskAwaiter val;
							switch (num)
							{
							default:
								<>8__1 = new <>c__DisplayClass10_0();
								<>8__1.<>4__this = <>4__this;
								if (blockReadManager.Offset < blockReadManager.Block.Size)
								{
									uint num2 = (uint)(blockReadManager.Block.Size - blockReadManager.Offset);
									uint num3 = 1 + num2 / 8;
									<estimated_xfers>5__2 = (int)Math.Min(255u, num3);
									new object();
									<>8__1.tcs = new TaskCompletionSource<RESPONSE>();
									<>8__1.message_sent = false;
									<>8__1.Read_xfer_size = 0u;
									<>8__1.bytes_read = 0u;
									<>8__1.sequence = 0;
									<>8__1.BulkSuccess = false;
									<>8__1.WaitFewSecondsToMute = false;
									<ErrorReadBulk>5__3 = 0u;
									<rx_listener>5__4 = blockReadManager.Client.ReusableSubscriptionPool.Get();
									<rx_listener>5__4.SetDelegate(blockReadManager.Client, delegate(LocalDeviceRxEvent rx)
									{
										if (!<>8__1.message_sent)
										{
											return false;
										}
										if (rx.SourceAddress != <>8__1.<>4__this.Target.Address)
										{
											return false;
										}
										<>8__1.<>4__this.ReportProgress(<>8__1.<>4__this.Offset + <>8__1.bytes_read, <>8__1.<>4__this.Block.Size);
										switch (rx.MessageType)
										{
										case 129:
											switch (rx.MessageData)
											{
											case 34:
												if (rx.Length == 1)
												{
													<>8__1.tcs.SetResult((RESPONSE)rx[0]);
													return true;
												}
												if (rx.Length >= 7 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == BLOCK_ID.op_Implicit(<>8__1.<>4__this.Block.ID) && CommExtensions.GetUINT32((IByteList)(object)rx, 2) == <>8__1.<>4__this.Offset)
												{
													if (rx.Length == 7)
													{
														<>8__1.tcs.SetResult((RESPONSE)rx[6]);
														return true;
													}
													if (rx.Length == 8)
													{
														if ((<>8__1.<>4__this.Block.Flags & BLOCK_FLAGS.USE_SET_SIZE) != BLOCK_FLAGS.NONE)
														{
															if (CommExtensions.GetUINT16((IByteList)(object)rx, 6) == (<>8__1.<>4__this.Block.Size & 0xFFFF))
															{
																<>8__1.Read_xfer_size = (uint)<>8__1.<>4__this.Block.Size;
																if (<>8__1.<>4__this.Offset == 0)
																{
																	<>8__1.WaitFewSecondsToMute = true;
																}
															}
														}
														else
														{
															<>8__1.Read_xfer_size = CommExtensions.GetUINT16((IByteList)(object)rx, 6);
														}
														int num5 = (int)(<>8__1.Read_xfer_size + 7) / 8 * 8;
														System.Array.Resize<byte>(ref <>8__1.<>4__this.Buffer, num5);
													}
												}
												return false;
											case 37:
												if (rx.Length == 8 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == BLOCK_ID.op_Implicit(<>8__1.<>4__this.Block.ID) && CommExtensions.GetUINT16((IByteList)(object)rx, 2) == (ushort)<>8__1.<>4__this.Offset)
												{
													uint num4 = CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyList<byte>)<>8__1.<>4__this.Buffer, (int)<>8__1.bytes_read, <>8__1.<>4__this.Offset);
													uint uINT = CommExtensions.GetUINT32((IByteList)(object)rx, 4);
													bool flag = false;
													if (num4 != uINT)
													{
														if (CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyList<byte>)<>8__1.<>4__this.Buffer, (int)<>8__1.Read_xfer_size, 0u) == uINT)
														{
															flag = true;
														}
													}
													else
													{
														flag = true;
													}
													if (!flag)
													{
														<>8__1.BulkSuccess = false;
														<>8__1.tcs.SetResult(RESPONSE.IN_PROGRESS);
													}
													else
													{
														<>8__1.BulkSuccess = true;
														if (<>8__1.bytes_read + <>8__1.<>4__this.Offset >= <>8__1.Read_xfer_size)
														{
															<>8__1.tcs.SetResult(RESPONSE.SUCCESS);
														}
														else
														{
															<>8__1.tcs.SetResult(RESPONSE.IN_PROGRESS);
														}
													}
													return true;
												}
												return false;
											default:
												return false;
											}
										case 159:
											if (<>8__1.Read_xfer_size != 0 && rx.MessageData == <>8__1.sequence)
											{
												for (int i = 0; i < rx.Length; i++)
												{
													<>8__1.<>4__this.Buffer[<>8__1.<>4__this.Offset + <>8__1.bytes_read] = rx[i];
													<>8__1.bytes_read++;
												}
												<>8__1.sequence++;
											}
											return false;
										default:
											return false;
										}
									});
									goto case 0;
								}
								result = RESPONSE.VALUE_OUT_OF_RANGE;
								goto end_IL_000e;
							case 0:
							case 1:
								try
								{
									TaskAwaiter<System.Threading.Tasks.Task> val2;
									if (num != 0)
									{
										if (num != 1)
										{
											goto IL_02b8;
										}
										val2 = <>u__2;
										<>u__2 = default(TaskAwaiter<System.Threading.Tasks.Task>);
										num = (<>1__state = -1);
										goto IL_02b0;
									}
									val = <>u__1;
									<>u__1 = default(TaskAwaiter);
									num = (<>1__state = -1);
									goto IL_020f;
									IL_02b0:
									val2.GetResult();
									goto IL_02b8;
									IL_02b8:
									if (blockReadManager.Client.IsOnline && blockReadManager.Target.IsOnline && !blockReadManager.Operation.IsCancellationRequested && !((System.Threading.Tasks.Task)<>8__1.tcs.Task).IsCompleted)
									{
										if (BlockUseSession)
										{
											session.TryOpenSession();
										}
										if (!blockReadManager.Client.Transmit29((byte)128, 34, blockReadManager.Target, PAYLOAD.FromArgs(new object[4]
										{
											BLOCK_ID.op_Implicit(blockReadManager.Block.ID),
											blockReadManager.Offset,
											(byte)255,
											blockReadManager.Delay_ms
										})))
										{
											val = System.Threading.Tasks.Task.Delay(5).GetAwaiter();
											if (!((TaskAwaiter)(ref val)).IsCompleted)
											{
												num = (<>1__state = 0);
												<>u__1 = val;
												<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <ReadPartialBlockAsync>d__10>(ref val, ref this);
												return;
											}
											goto IL_020f;
										}
										<>8__1.message_sent = true;
										val2 = System.Threading.Tasks.Task.WhenAny((System.Threading.Tasks.Task)<>8__1.tcs.Task, System.Threading.Tasks.Task.Delay(10000 + blockReadManager.Delay_ms * <estimated_xfers>5__2, blockReadManager.Operation.CancellationToken)).GetAwaiter();
										if (!val2.IsCompleted)
										{
											num = (<>1__state = 1);
											<>u__2 = val2;
											<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<System.Threading.Tasks.Task>, <ReadPartialBlockAsync>d__10>(ref val2, ref this);
											return;
										}
										goto IL_02b0;
									}
									goto end_IL_0127;
									IL_020f:
									((TaskAwaiter)(ref val)).GetResult();
									goto IL_02b8;
									end_IL_0127:;
								}
								catch (TimeoutException)
								{
								}
								catch (OperationCanceledException)
								{
								}
								finally
								{
									if (num < 0)
									{
										((Object)<rx_listener>5__4).ReturnToPool();
									}
								}
								if (((System.Threading.Tasks.Task)<>8__1.tcs.Task).IsCompleted)
								{
									if (!<>8__1.WaitFewSecondsToMute)
									{
										break;
									}
									if (BlockUseSession)
									{
										session.TryOpenSession();
									}
									val = System.Threading.Tasks.Task.Delay(1500).GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 2);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <ReadPartialBlockAsync>d__10>(ref val, ref this);
										return;
									}
									goto IL_03c2;
								}
								result = (blockReadManager.Operation.IsCancellationRequested ? RESPONSE.CANCELLED : RESPONSE.FAILED);
								goto end_IL_000e;
							case 2:
								val = <>u__1;
								<>u__1 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_03c2;
							case 3:
								{
									val = <>u__1;
									<>u__1 = default(TaskAwaiter);
									num = (<>1__state = -1);
									goto IL_0438;
								}
								IL_0438:
								((TaskAwaiter)(ref val)).GetResult();
								if (BlockUseSession)
								{
									session.TryOpenSession();
								}
								<>8__1.WaitFewSecondsToMute = false;
								break;
								IL_03c2:
								((TaskAwaiter)(ref val)).GetResult();
								if (BlockUseSession)
								{
									session.TryOpenSession();
								}
								val = System.Threading.Tasks.Task.Delay(1500).GetAwaiter();
								if (!((TaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 3);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <ReadPartialBlockAsync>d__10>(ref val, ref this);
									return;
								}
								goto IL_0438;
							}
							if (<>8__1.tcs.Task.Result != RESPONSE.SUCCESS)
							{
								goto IL_04b7;
							}
							if (BlockUseSession)
							{
								session.TryOpenSession();
							}
							if (<>8__1.bytes_read + blockReadManager.Offset < <>8__1.Read_xfer_size)
							{
								goto IL_04b7;
							}
							blockReadManager.TransferComplete = true;
							result = RESPONSE.SUCCESS;
							goto end_IL_000e;
							IL_04b7:
							if (<>8__1.tcs.Task.Result == RESPONSE.IN_PROGRESS)
							{
								if (BlockUseSession)
								{
									session.TryOpenSession();
								}
								if (<>8__1.Read_xfer_size >= <>8__1.bytes_read + blockReadManager.Offset && <>8__1.bytes_read != 0)
								{
									if (<>8__1.BulkSuccess)
									{
										blockReadManager.Offset += <>8__1.bytes_read;
										if (blockReadManager.Delay_ms > 0)
										{
											blockReadManager.Delay_ms--;
										}
									}
									else
									{
										<ErrorReadBulk>5__3++;
										blockReadManager.Delay_ms += 2;
									}
									result = RESPONSE.SUCCESS;
								}
								else
								{
									result = RESPONSE.FAILED;
								}
							}
							else
							{
								result = <>8__1.tcs.Task.Result;
							}
							end_IL_000e:;
						}
						catch (System.Exception exception)
						{
							<>1__state = -2;
							<>8__1 = null;
							<rx_listener>5__4 = null;
							<>t__builder.SetException(exception);
							return;
						}
						<>1__state = -2;
						<>8__1 = null;
						<rx_listener>5__4 = null;
						<>t__builder.SetResult(result);
					}

					[DebuggerHidden]
					private void SetStateMachine(IAsyncStateMachine stateMachine)
					{
						<>t__builder.SetStateMachine(stateMachine);
					}
				}

				private byte Delay_ms;

				private byte[] Buffer;

				private uint Offset;

				[field: CompilerGenerated]
				public bool TransferComplete
				{
					[CompilerGenerated]
					get;
					[CompilerGenerated]
					private set;
				}

				public bool CrcValid
				{
					get
					{
						if (!TransferComplete)
						{
							return false;
						}
						if (Buffer.Length == 0)
						{
							return true;
						}
						System.Array.Resize<byte>(ref Buffer, (int)Block.Size);
						uint num = CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyList<byte>)Buffer, Buffer.Length, 0u);
						return Block.CRC == num;
					}
				}

				public BlockReadManager(AsyncOperation operation, LocalDevice client, IBlock block, int bulk_xfer_delay_ms)
					: base(operation, client, block, TYPE.READER)
				{
					//IL_0048: Unknown result type (might be due to invalid IL or missing references)
					if (!block.IsReadable())
					{
						throw new NotImplementedException($"Block <{block.ID}>is read-only");
					}
					if (bulk_xfer_delay_ms >= 255)
					{
						Delay_ms = 255;
					}
					else if (bulk_xfer_delay_ms >= 0)
					{
						Delay_ms = (byte)bulk_xfer_delay_ms;
					}
					else
					{
						Delay_ms = 0;
					}
					uint num = (uint)((int)Block.Size + 7) / 8u * 8;
					Buffer = new byte[num];
				}

				[AsyncStateMachine(typeof(<ReadPartialBlockAsync>d__10))]
				private async System.Threading.Tasks.Task<RESPONSE> ReadPartialBlockAsync(bool BlockUseSession, ISessionClient session)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					if (Offset >= Block.Size)
					{
						return RESPONSE.VALUE_OUT_OF_RANGE;
					}
					uint num = (uint)(Block.Size - Offset);
					uint num2 = 1 + num / 8;
					int estimated_xfers = (int)Math.Min(255u, num2);
					new object();
					TaskCompletionSource<RESPONSE> tcs = new TaskCompletionSource<RESPONSE>();
					bool message_sent = false;
					uint Read_xfer_size = 0u;
					uint bytes_read = 0u;
					byte sequence = 0;
					bool BulkSuccess = false;
					bool WaitFewSecondsToMute = false;
					ReusableSubscription rx_listener = Client.ReusableSubscriptionPool.Get();
					rx_listener.SetDelegate(Client, delegate(LocalDeviceRxEvent rx)
					{
						if (!message_sent)
						{
							return false;
						}
						if (rx.SourceAddress != Target.Address)
						{
							return false;
						}
						ReportProgress(Offset + bytes_read, Block.Size);
						switch (rx.MessageType)
						{
						case 129:
							switch (rx.MessageData)
							{
							case 34:
								if (rx.Length == 1)
								{
									tcs.SetResult((RESPONSE)rx[0]);
									return true;
								}
								if (rx.Length >= 7 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == BLOCK_ID.op_Implicit(Block.ID) && CommExtensions.GetUINT32((IByteList)(object)rx, 2) == Offset)
								{
									if (rx.Length == 7)
									{
										tcs.SetResult((RESPONSE)rx[6]);
										return true;
									}
									if (rx.Length == 8)
									{
										if ((Block.Flags & BLOCK_FLAGS.USE_SET_SIZE) != BLOCK_FLAGS.NONE)
										{
											if (CommExtensions.GetUINT16((IByteList)(object)rx, 6) == (Block.Size & 0xFFFF))
											{
												Read_xfer_size = (uint)Block.Size;
												if (Offset == 0)
												{
													WaitFewSecondsToMute = true;
												}
											}
										}
										else
										{
											Read_xfer_size = CommExtensions.GetUINT16((IByteList)(object)rx, 6);
										}
										int num4 = (int)(Read_xfer_size + 7) / 8 * 8;
										System.Array.Resize<byte>(ref Buffer, num4);
									}
								}
								return false;
							case 37:
								if (rx.Length == 8 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == BLOCK_ID.op_Implicit(Block.ID) && CommExtensions.GetUINT16((IByteList)(object)rx, 2) == (ushort)Offset)
								{
									uint num3 = CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyList<byte>)Buffer, (int)bytes_read, Offset);
									uint uINT = CommExtensions.GetUINT32((IByteList)(object)rx, 4);
									bool flag = false;
									if (num3 != uINT)
									{
										if (CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyList<byte>)Buffer, (int)Read_xfer_size, 0u) == uINT)
										{
											flag = true;
										}
									}
									else
									{
										flag = true;
									}
									if (!flag)
									{
										BulkSuccess = false;
										tcs.SetResult(RESPONSE.IN_PROGRESS);
									}
									else
									{
										BulkSuccess = true;
										if (bytes_read + Offset >= Read_xfer_size)
										{
											tcs.SetResult(RESPONSE.SUCCESS);
										}
										else
										{
											tcs.SetResult(RESPONSE.IN_PROGRESS);
										}
									}
									return true;
								}
								return false;
							default:
								return false;
							}
						case 159:
							if (Read_xfer_size != 0 && rx.MessageData == sequence)
							{
								for (int i = 0; i < rx.Length; i++)
								{
									Buffer[Offset + bytes_read] = rx[i];
									bytes_read++;
								}
								sequence++;
							}
							return false;
						default:
							return false;
						}
					});
					try
					{
						while (Client.IsOnline && Target.IsOnline && !Operation.IsCancellationRequested && !((System.Threading.Tasks.Task)tcs.Task).IsCompleted)
						{
							if (BlockUseSession)
							{
								session.TryOpenSession();
							}
							if (!Client.Transmit29((byte)128, 34, Target, PAYLOAD.FromArgs(new object[4]
							{
								BLOCK_ID.op_Implicit(Block.ID),
								Offset,
								(byte)255,
								Delay_ms
							})))
							{
								await System.Threading.Tasks.Task.Delay(5);
								continue;
							}
							message_sent = true;
							await System.Threading.Tasks.Task.WhenAny((System.Threading.Tasks.Task)tcs.Task, System.Threading.Tasks.Task.Delay(10000 + Delay_ms * estimated_xfers, Operation.CancellationToken));
						}
					}
					catch (TimeoutException)
					{
					}
					catch (OperationCanceledException)
					{
					}
					finally
					{
						((Object)rx_listener).ReturnToPool();
					}
					if (!((System.Threading.Tasks.Task)tcs.Task).IsCompleted)
					{
						return Operation.IsCancellationRequested ? RESPONSE.CANCELLED : RESPONSE.FAILED;
					}
					if (WaitFewSecondsToMute)
					{
						if (BlockUseSession)
						{
							session.TryOpenSession();
						}
						await System.Threading.Tasks.Task.Delay(1500);
						if (BlockUseSession)
						{
							session.TryOpenSession();
						}
						await System.Threading.Tasks.Task.Delay(1500);
						if (BlockUseSession)
						{
							session.TryOpenSession();
						}
						WaitFewSecondsToMute = false;
					}
					if (tcs.Task.Result == RESPONSE.SUCCESS)
					{
						if (BlockUseSession)
						{
							session.TryOpenSession();
						}
						if (bytes_read + Offset >= Read_xfer_size)
						{
							TransferComplete = true;
							return RESPONSE.SUCCESS;
						}
					}
					if (tcs.Task.Result == RESPONSE.IN_PROGRESS)
					{
						if (BlockUseSession)
						{
							session.TryOpenSession();
						}
						if (Read_xfer_size >= bytes_read + Offset && bytes_read != 0)
						{
							if (BulkSuccess)
							{
								Offset += bytes_read;
								if (Delay_ms > 0)
								{
									Delay_ms--;
								}
							}
							else
							{
								Delay_ms += 2;
							}
							return RESPONSE.SUCCESS;
						}
						return RESPONSE.FAILED;
					}
					return tcs.Task.Result;
				}

				[AsyncStateMachine(typeof(<ReadBlockAsync>d__11))]
				public async System.Threading.Tasks.Task<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> ReadBlockAsync(bool BlockUseSession, ISessionClient session)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					while (!TransferComplete && !Operation.IsCancellationRequested)
					{
						RESPONSE rESPONSE = await ReadPartialBlockAsync(BlockUseSession, session);
						if (rESPONSE != RESPONSE.SUCCESS)
						{
							return Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(rESPONSE, (System.Collections.Generic.IReadOnlyList<byte>)null);
						}
					}
					if (!TransferComplete)
					{
						return Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(RESPONSE.FAILED, (System.Collections.Generic.IReadOnlyList<byte>)null);
					}
					if (!CrcValid)
					{
						return Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(RESPONSE.CRC_INVALID, (System.Collections.Generic.IReadOnlyList<byte>)null);
					}
					return Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(RESPONSE.SUCCESS, (System.Collections.Generic.IReadOnlyList<byte>)Buffer);
				}
			}

			private class BlockWriteManager : BlockOperationProgressReporter
			{
				[CompilerGenerated]
				private sealed class <>c__DisplayClass10_0
				{
					public BlockWriteManager <>4__this;

					public bool BlockUseSession;

					public ISessionClient session;

					internal RESPONSE? <Request23BeginBlockWriteAsync>b__0(LocalDeviceRxEvent rx)
					{
						if (rx.Length < 2)
						{
							return null;
						}
						if (CommExtensions.GetUINT16((IByteList)(object)rx, 0) != BLOCK_ID.op_Implicit(<>4__this.Block.ID))
						{
							return null;
						}
						if (rx.Length == 3)
						{
							RESPONSE rESPONSE = (RESPONSE)rx[2];
							if (rESPONSE == RESPONSE.IN_PROGRESS)
							{
								if (BlockUseSession)
								{
									session.TryOpenSession();
								}
								return null;
							}
							return rESPONSE;
						}
						if (rx.Length != 7)
						{
							return null;
						}
						return RESPONSE.SUCCESS;
					}
				}

				[CompilerGenerated]
				private sealed class <>c__DisplayClass13_0
				{
					public BlockWriteManager <>4__this;

					public bool BlockUseSession;

					public ISessionClient session;

					internal RESPONSE? <Request25EndBulkTransferAsync>b__0(LocalDeviceRxEvent rx)
					{
						if (rx.Length < 5)
						{
							return null;
						}
						if (CommExtensions.GetUINT16((IByteList)(object)rx, 0) != BLOCK_ID.op_Implicit(<>4__this.Block.ID))
						{
							return null;
						}
						if (CommExtensions.GetUINT16((IByteList)(object)rx, 2) != (ushort)<>4__this.CurrentBlockOffset)
						{
							return null;
						}
						if (rx.Length == 5)
						{
							RESPONSE rESPONSE = (RESPONSE)rx[4];
							if (rESPONSE == RESPONSE.IN_PROGRESS)
							{
								if (BlockUseSession)
								{
									session.TryOpenSession();
								}
								return null;
							}
							return rESPONSE;
						}
						if (rx.Length != 8)
						{
							return null;
						}
						return RESPONSE.SUCCESS;
					}
				}

				[CompilerGenerated]
				private sealed class <>c__DisplayClass14_0
				{
					public BlockWriteManager <>4__this;

					public bool BlockUseSession;

					public ISessionClient session;

					internal RESPONSE? <Request26EndBlockWriteAsync>b__0(LocalDeviceRxEvent rx)
					{
						if (rx.Length != 3)
						{
							return null;
						}
						if (CommExtensions.GetUINT16((IByteList)(object)rx, 0) != BLOCK_ID.op_Implicit(<>4__this.Block.ID))
						{
							return null;
						}
						RESPONSE rESPONSE = (RESPONSE)rx[2];
						if (rESPONSE == RESPONSE.IN_PROGRESS)
						{
							if (BlockUseSession)
							{
								session.TryOpenSession();
							}
							return null;
						}
						return rESPONSE;
					}
				}

				[StructLayout((LayoutKind)3)]
				[CompilerGenerated]
				private struct <DoBulkTransferAsync>d__12 : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder<RESPONSE> <>t__builder;

					public int count;

					public BlockWriteManager <>4__this;

					public int offset;

					private int <bytes_left>5__2;

					private int <i>5__3;

					private byte <sequence>5__4;

					private byte <error_count>5__5;

					private PAYLOAD <payload>5__6;

					private int <len>5__7;

					private TaskAwaiter <>u__1;

					private void MoveNext()
					{
						//IL_017d: Unknown result type (might be due to invalid IL or missing references)
						//IL_0182: Unknown result type (might be due to invalid IL or missing references)
						//IL_018a: Unknown result type (might be due to invalid IL or missing references)
						//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
						//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
						//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
						//IL_0088: Unknown result type (might be due to invalid IL or missing references)
						//IL_0122: Unknown result type (might be due to invalid IL or missing references)
						//IL_0148: Unknown result type (might be due to invalid IL or missing references)
						//IL_014d: Unknown result type (might be due to invalid IL or missing references)
						//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
						//IL_01be: Unknown result type (might be due to invalid IL or missing references)
						//IL_0162: Unknown result type (might be due to invalid IL or missing references)
						//IL_0164: Unknown result type (might be due to invalid IL or missing references)
						//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
						//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						BlockWriteManager blockWriteManager = <>4__this;
						RESPONSE result;
						try
						{
							TaskAwaiter val;
							if (num == 0)
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_0199;
							}
							if (num == 1)
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_020a;
							}
							if (count <= 0)
							{
								result = RESPONSE.FAILED;
							}
							else if (count > blockWriteManager.TotalBytesRemaining)
							{
								result = RESPONSE.FAILED;
							}
							else
							{
								if (offset < blockWriteManager.TotalNumberOfBytesToWrite)
								{
									<bytes_left>5__2 = count;
									<i>5__3 = offset;
									<sequence>5__4 = 0;
									<error_count>5__5 = 0;
									<payload>5__6 = default(PAYLOAD);
									goto IL_026b;
								}
								result = RESPONSE.FAILED;
							}
							goto end_IL_000e;
							IL_020a:
							((TaskAwaiter)(ref val)).GetResult();
							goto IL_0211;
							IL_0199:
							((TaskAwaiter)(ref val)).GetResult();
							goto IL_026b;
							IL_026b:
							if (blockWriteManager.Client.IsOnline && <error_count>5__5 < 5 && !blockWriteManager.Operation.IsCancellationRequested)
							{
								blockWriteManager.ReportProgress((uint)<i>5__3, blockWriteManager.TotalNumberOfBytesToWrite);
								<len>5__7 = Math.Min(8, <bytes_left>5__2);
								if (<len>5__7 > 0)
								{
									((PAYLOAD)(ref <payload>5__6)).Length = 0;
									for (int i = 0; i < <len>5__7; i++)
									{
										((PAYLOAD)(ref <payload>5__6)).Append(blockWriteManager.Data[<i>5__3 + i]);
									}
									if (!blockWriteManager.Client.Transmit29((byte)159, <sequence>5__4, blockWriteManager.Target, <payload>5__6))
									{
										<error_count>5__5++;
										val = System.Threading.Tasks.Task.Delay(25).GetAwaiter();
										if (!((TaskAwaiter)(ref val)).IsCompleted)
										{
											num = (<>1__state = 0);
											<>u__1 = val;
											<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <DoBulkTransferAsync>d__12>(ref val, ref this);
											return;
										}
										goto IL_0199;
									}
									if (blockWriteManager.BulkTransferPacketDelay_ms > 0)
									{
										val = System.Threading.Tasks.Task.Delay(blockWriteManager.BulkTransferPacketDelay_ms).GetAwaiter();
										if (!((TaskAwaiter)(ref val)).IsCompleted)
										{
											num = (<>1__state = 1);
											<>u__1 = val;
											<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <DoBulkTransferAsync>d__12>(ref val, ref this);
											return;
										}
										goto IL_020a;
									}
									goto IL_0211;
								}
								result = RESPONSE.FAILED;
							}
							else
							{
								result = RESPONSE.FAILED;
							}
							goto end_IL_000e;
							IL_0211:
							<error_count>5__5 = 0;
							<sequence>5__4++;
							<i>5__3 += <len>5__7;
							<bytes_left>5__2 -= <len>5__7;
							if (<bytes_left>5__2 == 0)
							{
								result = RESPONSE.SUCCESS;
							}
							else
							{
								if (<bytes_left>5__2 >= 0)
								{
									goto IL_026b;
								}
								result = RESPONSE.FAILED;
							}
							end_IL_000e:;
						}
						catch (System.Exception exception)
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
				private struct <Request23BeginBlockWriteAsync>d__10 : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder<RESPONSE> <>t__builder;

					public BlockWriteManager <>4__this;

					public bool BlockUseSession;

					public ISessionClient session;

					private Tuple<RESPONSE, MessageBuffer> <result>5__2;

					private TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> <>u__1;

					private void MoveNext()
					{
						//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
						//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
						//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
						//IL_0097: Unknown result type (might be due to invalid IL or missing references)
						//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
						//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
						//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
						//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						BlockWriteManager blockWriteManager = <>4__this;
						RESPONSE result2;
						try
						{
							<>c__DisplayClass10_0 <>c__DisplayClass10_ = default(<>c__DisplayClass10_0);
							if (num != 0)
							{
								<>c__DisplayClass10_ = new <>c__DisplayClass10_0
								{
									<>4__this = <>4__this,
									BlockUseSession = BlockUseSession,
									session = session
								};
								<result>5__2 = null;
							}
							try
							{
								TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> val;
								if (num != 0)
								{
									uint count = (uint)((System.Collections.Generic.IReadOnlyCollection<byte>)blockWriteManager.Data).Count;
									val = blockWriteManager.Client.TransmitRequestAsync(blockWriteManager.Operation, blockWriteManager.Target, (byte)35, PAYLOAD.FromArgs(new object[2]
									{
										BLOCK_ID.op_Implicit(blockWriteManager.Block.ID),
										count
									}), <>c__DisplayClass10_.<Request23BeginBlockWriteAsync>b__0).GetAwaiter();
									if (!val.IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>, <Request23BeginBlockWriteAsync>d__10>(ref val, ref this);
										return;
									}
								}
								else
								{
									val = <>u__1;
									<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>);
									num = (<>1__state = -1);
								}
								Tuple<RESPONSE, MessageBuffer> result = val.GetResult();
								<result>5__2 = result;
								if (<result>5__2.Item1 != RESPONSE.SUCCESS)
								{
									result2 = <result>5__2.Item1;
								}
								else
								{
									MessageBuffer item = <result>5__2.Item2;
									if (item == null || ((MessageBuffer)item).Length != 7)
									{
										result2 = RESPONSE.FAILED;
									}
									else if (CommExtensions.GetUINT16((IByteList)(object)<result>5__2.Item2, 0) != BLOCK_ID.op_Implicit(blockWriteManager.Block.ID))
									{
										result2 = RESPONSE.FAILED;
									}
									else
									{
										blockWriteManager.TotalNumberOfBytesToWrite = CommExtensions.GetUINT32((IByteList)(object)<result>5__2.Item2, 2);
										blockWriteManager.MinBulkTransferPacketDelay_ms = Math.Max(blockWriteManager.MinBulkTransferPacketDelay_ms, (int)CommExtensions.GetUINT8((IByteList)(object)<result>5__2.Item2, 6));
										blockWriteManager.BulkTransferPacketDelay_ms = blockWriteManager.MinBulkTransferPacketDelay_ms;
										result2 = RESPONSE.SUCCESS;
									}
								}
							}
							finally
							{
								if (num < 0)
								{
									Tuple<RESPONSE, MessageBuffer> obj = <result>5__2;
									if (obj != null)
									{
										MessageBuffer item2 = obj.Item2;
										if (item2 != null)
										{
											((Object)item2).ReturnToPool();
										}
									}
								}
							}
						}
						catch (System.Exception exception)
						{
							<>1__state = -2;
							<result>5__2 = null;
							<>t__builder.SetException(exception);
							return;
						}
						<>1__state = -2;
						<result>5__2 = null;
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
				private struct <Request24BeginBulkTransferAsync>d__11 : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder<RESPONSE> <>t__builder;

					public BlockWriteManager <>4__this;

					private Tuple<RESPONSE, MessageBuffer> <result>5__2;

					private TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> <>u__1;

					private void MoveNext()
					{
						//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
						//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
						//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
						//IL_009d: Unknown result type (might be due to invalid IL or missing references)
						//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
						//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
						//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
						//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						BlockWriteManager CS$<>8__locals14 = <>4__this;
						RESPONSE result2;
						try
						{
							if (num != 0)
							{
								<result>5__2 = null;
							}
							try
							{
								TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> val;
								if (num != 0)
								{
									ushort num2 = 65535;
									if ((byte)CS$<>8__locals14.Block.Device.ProtocolVersion >= 25)
									{
										num2 = (ushort)(CS$<>8__locals14.TotalBytesRemaining - 1);
									}
									CS$<>8__locals14.bulkXferSize = 0u;
									val = CS$<>8__locals14.Client.TransmitRequestAsync(CS$<>8__locals14.Operation, CS$<>8__locals14.Target, (byte)36, PAYLOAD.FromArgs(new object[3]
									{
										BLOCK_ID.op_Implicit(CS$<>8__locals14.Block.ID),
										CS$<>8__locals14.CurrentBlockOffset,
										num2
									}), [CompilerGenerated] (LocalDeviceRxEvent rx) =>
									{
										if (rx.Length < 2)
										{
											return (RESPONSE?)null;
										}
										if (CommExtensions.GetUINT16((IByteList)(object)rx, 0) != BLOCK_ID.op_Implicit(CS$<>8__locals14.Block.ID))
										{
											return (RESPONSE?)null;
										}
										if (rx.Length == 3)
										{
											return (RESPONSE)rx[2];
										}
										if (rx.Length < 6)
										{
											return (RESPONSE?)null;
										}
										if (CommExtensions.GetUINT32((IByteList)(object)rx, 2) != CS$<>8__locals14.CurrentBlockOffset)
										{
											return (RESPONSE?)null;
										}
										return (rx.Length != 8) ? ((RESPONSE?)null) : new RESPONSE?(RESPONSE.SUCCESS);
									}).GetAwaiter();
									if (!val.IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>, <Request24BeginBulkTransferAsync>d__11>(ref val, ref this);
										return;
									}
								}
								else
								{
									val = <>u__1;
									<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>);
									num = (<>1__state = -1);
								}
								Tuple<RESPONSE, MessageBuffer> result = val.GetResult();
								<result>5__2 = result;
								if (<result>5__2.Item1 != RESPONSE.SUCCESS)
								{
									result2 = <result>5__2.Item1;
								}
								else
								{
									MessageBuffer item = <result>5__2.Item2;
									if (item == null || ((MessageBuffer)item).Length != 8)
									{
										result2 = RESPONSE.FAILED;
									}
									else if (CommExtensions.GetUINT16((IByteList)(object)<result>5__2.Item2, 0) != BLOCK_ID.op_Implicit(CS$<>8__locals14.Block.ID))
									{
										result2 = RESPONSE.FAILED;
									}
									else if (CommExtensions.GetUINT32((IByteList)(object)<result>5__2.Item2, 2) != CS$<>8__locals14.CurrentBlockOffset)
									{
										result2 = RESPONSE.FAILED;
									}
									else
									{
										CS$<>8__locals14.bulkXferSize = Math.Min((uint)CommExtensions.GetUINT16((IByteList)(object)<result>5__2.Item2, 6), CS$<>8__locals14.TotalBytesRemaining);
										result2 = RESPONSE.SUCCESS;
									}
								}
							}
							finally
							{
								if (num < 0)
								{
									Tuple<RESPONSE, MessageBuffer> obj = <result>5__2;
									if (obj != null)
									{
										MessageBuffer item2 = obj.Item2;
										if (item2 != null)
										{
											((Object)item2).ReturnToPool();
										}
									}
								}
							}
						}
						catch (System.Exception exception)
						{
							<>1__state = -2;
							<result>5__2 = null;
							<>t__builder.SetException(exception);
							return;
						}
						<>1__state = -2;
						<result>5__2 = null;
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
				private struct <Request25EndBulkTransferAsync>d__13 : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder<RESPONSE> <>t__builder;

					public BlockWriteManager <>4__this;

					public bool BlockUseSession;

					public ISessionClient session;

					private Tuple<RESPONSE, MessageBuffer> <result>5__2;

					private uint <xfer_crc>5__3;

					private TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> <>u__1;

					private void MoveNext()
					{
						//IL_0105: Unknown result type (might be due to invalid IL or missing references)
						//IL_010a: Unknown result type (might be due to invalid IL or missing references)
						//IL_0112: Unknown result type (might be due to invalid IL or missing references)
						//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
						//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
						//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
						//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
						//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						BlockWriteManager blockWriteManager = <>4__this;
						RESPONSE result2;
						try
						{
							<>c__DisplayClass13_0 <>c__DisplayClass13_ = default(<>c__DisplayClass13_0);
							if (num != 0)
							{
								<>c__DisplayClass13_ = new <>c__DisplayClass13_0
								{
									<>4__this = <>4__this,
									BlockUseSession = BlockUseSession,
									session = session
								};
								<result>5__2 = null;
							}
							try
							{
								TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> val;
								if (num != 0)
								{
									<xfer_crc>5__3 = CRC32_LE.Calculate(blockWriteManager.Data, (int)blockWriteManager.bulkXferSize, blockWriteManager.CurrentBlockOffset);
									val = blockWriteManager.Client.TransmitRequestAsync(blockWriteManager.Operation, blockWriteManager.Target, (byte)37, PAYLOAD.FromArgs(new object[3]
									{
										BLOCK_ID.op_Implicit(blockWriteManager.Block.ID),
										(ushort)blockWriteManager.CurrentBlockOffset,
										<xfer_crc>5__3
									}), <>c__DisplayClass13_.<Request25EndBulkTransferAsync>b__0).GetAwaiter();
									if (!val.IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>, <Request25EndBulkTransferAsync>d__13>(ref val, ref this);
										return;
									}
								}
								else
								{
									val = <>u__1;
									<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>);
									num = (<>1__state = -1);
								}
								Tuple<RESPONSE, MessageBuffer> result = val.GetResult();
								<result>5__2 = result;
								if (<result>5__2.Item1 != RESPONSE.SUCCESS)
								{
									result2 = <result>5__2.Item1;
								}
								else
								{
									MessageBuffer item = <result>5__2.Item2;
									result2 = ((item != null && ((MessageBuffer)item).Length == 8) ? ((CommExtensions.GetUINT16((IByteList)(object)<result>5__2.Item2, 0) != BLOCK_ID.op_Implicit(blockWriteManager.Block.ID)) ? RESPONSE.FAILED : ((CommExtensions.GetUINT16((IByteList)(object)<result>5__2.Item2, 2) != (ushort)blockWriteManager.CurrentBlockOffset) ? RESPONSE.FAILED : ((CommExtensions.GetUINT32((IByteList)(object)<result>5__2.Item2, 4) != <xfer_crc>5__3) ? RESPONSE.CRC_INVALID : RESPONSE.SUCCESS))) : RESPONSE.FAILED);
								}
							}
							finally
							{
								if (num < 0)
								{
									Tuple<RESPONSE, MessageBuffer> obj = <result>5__2;
									if (obj != null)
									{
										MessageBuffer item2 = obj.Item2;
										if (item2 != null)
										{
											((Object)item2).ReturnToPool();
										}
									}
								}
							}
						}
						catch (System.Exception exception)
						{
							<>1__state = -2;
							<result>5__2 = null;
							<>t__builder.SetException(exception);
							return;
						}
						<>1__state = -2;
						<result>5__2 = null;
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
				private struct <Request26EndBlockWriteAsync>d__14 : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder<RESPONSE> <>t__builder;

					public BlockWriteManager <>4__this;

					public bool BlockUseSession;

					public ISessionClient session;

					private Tuple<RESPONSE, MessageBuffer> <result>5__2;

					private TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> <>u__1;

					private void MoveNext()
					{
						//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
						//IL_00de: Unknown result type (might be due to invalid IL or missing references)
						//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
						//IL_008e: Unknown result type (might be due to invalid IL or missing references)
						//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
						//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
						//IL_00be: Unknown result type (might be due to invalid IL or missing references)
						//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						BlockWriteManager blockWriteManager = <>4__this;
						RESPONSE result2;
						try
						{
							<>c__DisplayClass14_0 <>c__DisplayClass14_ = default(<>c__DisplayClass14_0);
							if (num != 0)
							{
								<>c__DisplayClass14_ = new <>c__DisplayClass14_0
								{
									<>4__this = <>4__this,
									BlockUseSession = BlockUseSession,
									session = session
								};
								<result>5__2 = null;
							}
							try
							{
								TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> val;
								if (num != 0)
								{
									val = blockWriteManager.Client.TransmitRequestAsync(blockWriteManager.Operation, blockWriteManager.Target, (byte)38, PAYLOAD.FromArgs(new object[2]
									{
										BLOCK_ID.op_Implicit(blockWriteManager.Block.ID),
										blockWriteManager.TotalCRC
									}), <>c__DisplayClass14_.<Request26EndBlockWriteAsync>b__0).GetAwaiter();
									if (!val.IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>, <Request26EndBlockWriteAsync>d__14>(ref val, ref this);
										return;
									}
								}
								else
								{
									val = <>u__1;
									<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>);
									num = (<>1__state = -1);
								}
								Tuple<RESPONSE, MessageBuffer> result = val.GetResult();
								<result>5__2 = result;
								if (<result>5__2.Item1 != RESPONSE.SUCCESS)
								{
									result2 = <result>5__2.Item1;
								}
								else
								{
									MessageBuffer item = <result>5__2.Item2;
									result2 = ((item != null && ((MessageBuffer)item).Length == 3) ? ((RESPONSE)((CommExtensions.GetUINT16((IByteList)(object)<result>5__2.Item2, 0) == BLOCK_ID.op_Implicit(blockWriteManager.Block.ID)) ? CommExtensions.GetUINT8((IByteList)(object)<result>5__2.Item2, 2) : 21)) : RESPONSE.FAILED);
								}
							}
							finally
							{
								if (num < 0)
								{
									Tuple<RESPONSE, MessageBuffer> obj = <result>5__2;
									if (obj != null)
									{
										MessageBuffer item2 = obj.Item2;
										if (item2 != null)
										{
											((Object)item2).ReturnToPool();
										}
									}
								}
							}
						}
						catch (System.Exception exception)
						{
							<>1__state = -2;
							<result>5__2 = null;
							<>t__builder.SetException(exception);
							return;
						}
						<>1__state = -2;
						<result>5__2 = null;
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
				private struct <WriteBlockAsync>d__9 : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder<RESPONSE> <>t__builder;

					public BlockWriteManager <>4__this;

					public bool BlockUseSession;

					public ISessionClient session;

					private TaskAwaiter<RESPONSE> <>u__1;

					private TaskAwaiter <>u__2;

					private void MoveNext()
					{
						//IL_0093: Unknown result type (might be due to invalid IL or missing references)
						//IL_0098: Unknown result type (might be due to invalid IL or missing references)
						//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
						//IL_0106: Unknown result type (might be due to invalid IL or missing references)
						//IL_010b: Unknown result type (might be due to invalid IL or missing references)
						//IL_0113: Unknown result type (might be due to invalid IL or missing references)
						//IL_0171: Unknown result type (might be due to invalid IL or missing references)
						//IL_0176: Unknown result type (might be due to invalid IL or missing references)
						//IL_017e: Unknown result type (might be due to invalid IL or missing references)
						//IL_022d: Unknown result type (might be due to invalid IL or missing references)
						//IL_0232: Unknown result type (might be due to invalid IL or missing references)
						//IL_023a: Unknown result type (might be due to invalid IL or missing references)
						//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
						//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
						//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
						//IL_0332: Unknown result type (might be due to invalid IL or missing references)
						//IL_0337: Unknown result type (might be due to invalid IL or missing references)
						//IL_033f: Unknown result type (might be due to invalid IL or missing references)
						//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
						//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
						//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
						//IL_047e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0483: Unknown result type (might be due to invalid IL or missing references)
						//IL_048b: Unknown result type (might be due to invalid IL or missing references)
						//IL_052d: Unknown result type (might be due to invalid IL or missing references)
						//IL_0532: Unknown result type (might be due to invalid IL or missing references)
						//IL_053a: Unknown result type (might be due to invalid IL or missing references)
						//IL_005e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0063: Unknown result type (might be due to invalid IL or missing references)
						//IL_013c: Unknown result type (might be due to invalid IL or missing references)
						//IL_0141: Unknown result type (might be due to invalid IL or missing references)
						//IL_0078: Unknown result type (might be due to invalid IL or missing references)
						//IL_007a: Unknown result type (might be due to invalid IL or missing references)
						//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
						//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
						//IL_0156: Unknown result type (might be due to invalid IL or missing references)
						//IL_0158: Unknown result type (might be due to invalid IL or missing references)
						//IL_026e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0273: Unknown result type (might be due to invalid IL or missing references)
						//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
						//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
						//IL_0288: Unknown result type (might be due to invalid IL or missing references)
						//IL_028a: Unknown result type (might be due to invalid IL or missing references)
						//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
						//IL_0302: Unknown result type (might be due to invalid IL or missing references)
						//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
						//IL_0500: Unknown result type (might be due to invalid IL or missing references)
						//IL_0449: Unknown result type (might be due to invalid IL or missing references)
						//IL_044e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0317: Unknown result type (might be due to invalid IL or missing references)
						//IL_0319: Unknown result type (might be due to invalid IL or missing references)
						//IL_0515: Unknown result type (might be due to invalid IL or missing references)
						//IL_0517: Unknown result type (might be due to invalid IL or missing references)
						//IL_039a: Unknown result type (might be due to invalid IL or missing references)
						//IL_039f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0463: Unknown result type (might be due to invalid IL or missing references)
						//IL_0465: Unknown result type (might be due to invalid IL or missing references)
						//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
						//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
						//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
						//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
						//IL_0212: Unknown result type (might be due to invalid IL or missing references)
						//IL_0214: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						BlockWriteManager blockWriteManager = <>4__this;
						RESPONSE result2;
						try
						{
							TaskAwaiter val2;
							TaskAwaiter<RESPONSE> val;
							RESPONSE result;
							switch (num)
							{
							default:
								blockWriteManager.ReportProgress(0uL, (uint)((System.Collections.Generic.IReadOnlyCollection<byte>)blockWriteManager.Data).Count);
								val = blockWriteManager.Request23BeginBlockWriteAsync(BlockUseSession, session).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<RESPONSE>, <WriteBlockAsync>d__9>(ref val, ref this);
									return;
								}
								goto IL_00af;
							case 0:
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<RESPONSE>);
								num = (<>1__state = -1);
								goto IL_00af;
							case 1:
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<RESPONSE>);
								num = (<>1__state = -1);
								goto IL_0122;
							case 2:
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<RESPONSE>);
								num = (<>1__state = -1);
								goto IL_018d;
							case 3:
								val2 = <>u__2;
								<>u__2 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_0249;
							case 4:
								val2 = <>u__2;
								<>u__2 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_02bf;
							case 5:
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<RESPONSE>);
								num = (<>1__state = -1);
								goto IL_034e;
							case 6:
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<RESPONSE>);
								num = (<>1__state = -1);
								goto IL_03eb;
							case 7:
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<RESPONSE>);
								num = (<>1__state = -1);
								goto IL_049a;
							case 8:
								{
									val = <>u__1;
									<>u__1 = default(TaskAwaiter<RESPONSE>);
									num = (<>1__state = -1);
									break;
								}
								IL_0249:
								((TaskAwaiter)(ref val2)).GetResult();
								if (BlockUseSession)
								{
									session.TryOpenSession();
								}
								val2 = System.Threading.Tasks.Task.Delay(1500).GetAwaiter();
								if (!((TaskAwaiter)(ref val2)).IsCompleted)
								{
									num = (<>1__state = 4);
									<>u__2 = val2;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <WriteBlockAsync>d__9>(ref val2, ref this);
									return;
								}
								goto IL_02bf;
								IL_00af:
								result = val.GetResult();
								if (result == RESPONSE.BUSY)
								{
									val = blockWriteManager.Request26EndBlockWriteAsync(BlockUseSession, session).GetAwaiter();
									if (!val.IsCompleted)
									{
										num = (<>1__state = 1);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<RESPONSE>, <WriteBlockAsync>d__9>(ref val, ref this);
										return;
									}
									goto IL_0122;
								}
								goto IL_0195;
								IL_03eb:
								if (val.GetResult() != RESPONSE.SUCCESS)
								{
									blockWriteManager.BulkTransferPacketDelay_ms += 10;
								}
								else if (blockWriteManager.BulkTransferPacketDelay_ms > blockWriteManager.MinBulkTransferPacketDelay_ms)
								{
									blockWriteManager.BulkTransferPacketDelay_ms--;
								}
								if (BlockUseSession)
								{
									session.TryOpenSession();
								}
								val = blockWriteManager.Request25EndBulkTransferAsync(BlockUseSession, session).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 7);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<RESPONSE>, <WriteBlockAsync>d__9>(ref val, ref this);
									return;
								}
								goto IL_049a;
								IL_04c9:
								if (blockWriteManager.TotalBytesRemaining == 0)
								{
									if (BlockUseSession)
									{
										session.TryOpenSession();
									}
									val = blockWriteManager.Request26EndBlockWriteAsync(BlockUseSession, session).GetAwaiter();
									if (!val.IsCompleted)
									{
										num = (<>1__state = 8);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<RESPONSE>, <WriteBlockAsync>d__9>(ref val, ref this);
										return;
									}
									break;
								}
								if (!blockWriteManager.Operation.IsCancellationRequested)
								{
									val = blockWriteManager.Request24BeginBulkTransferAsync().GetAwaiter();
									if (!val.IsCompleted)
									{
										num = (<>1__state = 5);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<RESPONSE>, <WriteBlockAsync>d__9>(ref val, ref this);
										return;
									}
									goto IL_034e;
								}
								result2 = RESPONSE.CANCELLED;
								goto end_IL_000e;
								IL_0122:
								val.GetResult();
								val = blockWriteManager.Request23BeginBlockWriteAsync(BlockUseSession, session).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 2);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<RESPONSE>, <WriteBlockAsync>d__9>(ref val, ref this);
									return;
								}
								goto IL_018d;
								IL_02bf:
								((TaskAwaiter)(ref val2)).GetResult();
								if (BlockUseSession)
								{
									session.TryOpenSession();
								}
								goto IL_04c9;
								IL_018d:
								result = val.GetResult();
								goto IL_0195;
								IL_0195:
								if (result != RESPONSE.SUCCESS)
								{
									result2 = result;
								}
								else
								{
									if (((System.Collections.Generic.IReadOnlyCollection<byte>)blockWriteManager.Data).Count == blockWriteManager.TotalNumberOfBytesToWrite)
									{
										blockWriteManager.CurrentBlockOffset = 0u;
										blockWriteManager.TotalBytesRemaining = blockWriteManager.TotalNumberOfBytesToWrite;
										if (blockWriteManager.TotalNumberOfBytesToWrite != 0)
										{
											if (BlockUseSession)
											{
												session.TryOpenSession();
											}
											val2 = System.Threading.Tasks.Task.Delay(1500).GetAwaiter();
											if (!((TaskAwaiter)(ref val2)).IsCompleted)
											{
												num = (<>1__state = 3);
												<>u__2 = val2;
												<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <WriteBlockAsync>d__9>(ref val2, ref this);
												return;
											}
											goto IL_0249;
										}
										goto IL_04c9;
									}
									result2 = RESPONSE.FAILED;
								}
								goto end_IL_000e;
								IL_049a:
								if (val.GetResult() == RESPONSE.SUCCESS)
								{
									blockWriteManager.CurrentBlockOffset += blockWriteManager.bulkXferSize;
									blockWriteManager.TotalBytesRemaining -= blockWriteManager.bulkXferSize;
								}
								goto IL_04c9;
								IL_034e:
								result = val.GetResult();
								if (result != RESPONSE.SUCCESS || blockWriteManager.bulkXferSize == 0)
								{
									result2 = result;
								}
								else
								{
									if (blockWriteManager.bulkXferSize != 0 && blockWriteManager.bulkXferSize <= blockWriteManager.TotalBytesRemaining)
									{
										val = blockWriteManager.DoBulkTransferAsync((int)blockWriteManager.bulkXferSize, (int)blockWriteManager.CurrentBlockOffset).GetAwaiter();
										if (!val.IsCompleted)
										{
											num = (<>1__state = 6);
											<>u__1 = val;
											<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<RESPONSE>, <WriteBlockAsync>d__9>(ref val, ref this);
											return;
										}
										goto IL_03eb;
									}
									result2 = RESPONSE.FAILED;
								}
								goto end_IL_000e;
							}
							result2 = val.GetResult();
							end_IL_000e:;
						}
						catch (System.Exception exception)
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

				private readonly System.Collections.Generic.IReadOnlyList<byte> Data;

				private readonly uint TotalCRC;

				private int MinBulkTransferPacketDelay_ms;

				private uint TotalNumberOfBytesToWrite;

				private int BulkTransferPacketDelay_ms;

				private uint bulkXferSize;

				private uint CurrentBlockOffset;

				private uint TotalBytesRemaining;

				public BlockWriteManager(AsyncOperation operation, LocalDevice client, IBlock block, System.Collections.Generic.IReadOnlyList<byte> data, int bulk_xfer_delay_ms)
					: base(operation, client, block, TYPE.WRITER)
				{
					Data = data;
					MinBulkTransferPacketDelay_ms = bulk_xfer_delay_ms;
					TotalCRC = CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyCollection<byte>)data);
				}

				[AsyncStateMachine(typeof(<WriteBlockAsync>d__9))]
				public async System.Threading.Tasks.Task<RESPONSE> WriteBlockAsync(bool BlockUseSession, ISessionClient session)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					ReportProgress(0uL, (uint)((System.Collections.Generic.IReadOnlyCollection<byte>)Data).Count);
					RESPONSE rESPONSE = await Request23BeginBlockWriteAsync(BlockUseSession, session);
					if (rESPONSE == RESPONSE.BUSY)
					{
						await Request26EndBlockWriteAsync(BlockUseSession, session);
						rESPONSE = await Request23BeginBlockWriteAsync(BlockUseSession, session);
					}
					if (rESPONSE != RESPONSE.SUCCESS)
					{
						return rESPONSE;
					}
					if (((System.Collections.Generic.IReadOnlyCollection<byte>)Data).Count != TotalNumberOfBytesToWrite)
					{
						return RESPONSE.FAILED;
					}
					CurrentBlockOffset = 0u;
					TotalBytesRemaining = TotalNumberOfBytesToWrite;
					if (TotalNumberOfBytesToWrite != 0)
					{
						if (BlockUseSession)
						{
							session.TryOpenSession();
						}
						await System.Threading.Tasks.Task.Delay(1500);
						if (BlockUseSession)
						{
							session.TryOpenSession();
						}
						await System.Threading.Tasks.Task.Delay(1500);
						if (BlockUseSession)
						{
							session.TryOpenSession();
						}
					}
					while (TotalBytesRemaining != 0)
					{
						if (Operation.IsCancellationRequested)
						{
							return RESPONSE.CANCELLED;
						}
						rESPONSE = await Request24BeginBulkTransferAsync();
						if (rESPONSE != RESPONSE.SUCCESS || bulkXferSize == 0)
						{
							return rESPONSE;
						}
						if (bulkXferSize == 0 || bulkXferSize > TotalBytesRemaining)
						{
							return RESPONSE.FAILED;
						}
						if (await DoBulkTransferAsync((int)bulkXferSize, (int)CurrentBlockOffset) != RESPONSE.SUCCESS)
						{
							BulkTransferPacketDelay_ms += 10;
						}
						else if (BulkTransferPacketDelay_ms > MinBulkTransferPacketDelay_ms)
						{
							BulkTransferPacketDelay_ms--;
						}
						if (BlockUseSession)
						{
							session.TryOpenSession();
						}
						if (await Request25EndBulkTransferAsync(BlockUseSession, session) == RESPONSE.SUCCESS)
						{
							CurrentBlockOffset += bulkXferSize;
							TotalBytesRemaining -= bulkXferSize;
						}
					}
					if (BlockUseSession)
					{
						session.TryOpenSession();
					}
					return await Request26EndBlockWriteAsync(BlockUseSession, session);
				}

				[AsyncStateMachine(typeof(<Request23BeginBlockWriteAsync>d__10))]
				private async System.Threading.Tasks.Task<RESPONSE> Request23BeginBlockWriteAsync(bool BlockUseSession, ISessionClient session)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					Tuple<RESPONSE, MessageBuffer> result = null;
					try
					{
						uint count = (uint)((System.Collections.Generic.IReadOnlyCollection<byte>)Data).Count;
						result = await Client.TransmitRequestAsync(Operation, Target, (byte)35, PAYLOAD.FromArgs(new object[2]
						{
							BLOCK_ID.op_Implicit(Block.ID),
							count
						}), delegate(LocalDeviceRxEvent rx)
						{
							if (rx.Length < 2)
							{
								return (RESPONSE?)null;
							}
							if (CommExtensions.GetUINT16((IByteList)(object)rx, 0) != BLOCK_ID.op_Implicit(Block.ID))
							{
								return (RESPONSE?)null;
							}
							if (rx.Length == 3)
							{
								RESPONSE rESPONSE = (RESPONSE)rx[2];
								if (rESPONSE == RESPONSE.IN_PROGRESS)
								{
									if (BlockUseSession)
									{
										session.TryOpenSession();
									}
									return (RESPONSE?)null;
								}
								return rESPONSE;
							}
							return (rx.Length != 7) ? ((RESPONSE?)null) : new RESPONSE?(RESPONSE.SUCCESS);
						});
						if (result.Item1 != RESPONSE.SUCCESS)
						{
							return result.Item1;
						}
						MessageBuffer item = result.Item2;
						if (item == null || ((MessageBuffer)item).Length != 7)
						{
							return RESPONSE.FAILED;
						}
						if (CommExtensions.GetUINT16((IByteList)(object)result.Item2, 0) != BLOCK_ID.op_Implicit(Block.ID))
						{
							return RESPONSE.FAILED;
						}
						TotalNumberOfBytesToWrite = CommExtensions.GetUINT32((IByteList)(object)result.Item2, 2);
						MinBulkTransferPacketDelay_ms = Math.Max(MinBulkTransferPacketDelay_ms, (int)CommExtensions.GetUINT8((IByteList)(object)result.Item2, 6));
						BulkTransferPacketDelay_ms = MinBulkTransferPacketDelay_ms;
						return RESPONSE.SUCCESS;
					}
					finally
					{
						Tuple<RESPONSE, MessageBuffer> obj = result;
						if (obj != null)
						{
							MessageBuffer item2 = obj.Item2;
							if (item2 != null)
							{
								((Object)item2).ReturnToPool();
							}
						}
					}
				}

				[AsyncStateMachine(typeof(<Request24BeginBulkTransferAsync>d__11))]
				private async System.Threading.Tasks.Task<RESPONSE> Request24BeginBulkTransferAsync()
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					Tuple<RESPONSE, MessageBuffer> result = null;
					try
					{
						ushort num = 65535;
						if ((byte)Block.Device.ProtocolVersion >= 25)
						{
							num = (ushort)(TotalBytesRemaining - 1);
						}
						bulkXferSize = 0u;
						result = await Client.TransmitRequestAsync(Operation, Target, (byte)36, PAYLOAD.FromArgs(new object[3]
						{
							BLOCK_ID.op_Implicit(Block.ID),
							CurrentBlockOffset,
							num
						}), [CompilerGenerated] (LocalDeviceRxEvent rx) =>
						{
							if (rx.Length < 2)
							{
								return (RESPONSE?)null;
							}
							if (CommExtensions.GetUINT16((IByteList)(object)rx, 0) != BLOCK_ID.op_Implicit(Block.ID))
							{
								return (RESPONSE?)null;
							}
							if (rx.Length == 3)
							{
								return (RESPONSE)rx[2];
							}
							if (rx.Length < 6)
							{
								return (RESPONSE?)null;
							}
							if (CommExtensions.GetUINT32((IByteList)(object)rx, 2) != CurrentBlockOffset)
							{
								return (RESPONSE?)null;
							}
							return (rx.Length != 8) ? ((RESPONSE?)null) : new RESPONSE?(RESPONSE.SUCCESS);
						});
						if (result.Item1 != RESPONSE.SUCCESS)
						{
							return result.Item1;
						}
						MessageBuffer item = result.Item2;
						if (item == null || ((MessageBuffer)item).Length != 8)
						{
							return RESPONSE.FAILED;
						}
						if (CommExtensions.GetUINT16((IByteList)(object)result.Item2, 0) != BLOCK_ID.op_Implicit(Block.ID))
						{
							return RESPONSE.FAILED;
						}
						if (CommExtensions.GetUINT32((IByteList)(object)result.Item2, 2) != CurrentBlockOffset)
						{
							return RESPONSE.FAILED;
						}
						bulkXferSize = Math.Min((uint)CommExtensions.GetUINT16((IByteList)(object)result.Item2, 6), TotalBytesRemaining);
						return RESPONSE.SUCCESS;
					}
					finally
					{
						Tuple<RESPONSE, MessageBuffer> obj = result;
						if (obj != null)
						{
							MessageBuffer item2 = obj.Item2;
							if (item2 != null)
							{
								((Object)item2).ReturnToPool();
							}
						}
					}
				}

				[AsyncStateMachine(typeof(<DoBulkTransferAsync>d__12))]
				private async System.Threading.Tasks.Task<RESPONSE> DoBulkTransferAsync(int count, int offset)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					if (count <= 0)
					{
						return RESPONSE.FAILED;
					}
					if (count > TotalBytesRemaining)
					{
						return RESPONSE.FAILED;
					}
					if (offset >= TotalNumberOfBytesToWrite)
					{
						return RESPONSE.FAILED;
					}
					int bytes_left = count;
					int i = offset;
					byte sequence = 0;
					byte error_count = 0;
					PAYLOAD payload = default(PAYLOAD);
					while (Client.IsOnline && error_count < 5 && !Operation.IsCancellationRequested)
					{
						ReportProgress((uint)i, TotalNumberOfBytesToWrite);
						int len = Math.Min(8, bytes_left);
						if (len <= 0)
						{
							return RESPONSE.FAILED;
						}
						((PAYLOAD)(ref payload)).Length = 0;
						for (int j = 0; j < len; j++)
						{
							((PAYLOAD)(ref payload)).Append(Data[i + j]);
						}
						if (!Client.Transmit29((byte)159, sequence, Target, payload))
						{
							error_count++;
							await System.Threading.Tasks.Task.Delay(25);
							continue;
						}
						if (BulkTransferPacketDelay_ms > 0)
						{
							await System.Threading.Tasks.Task.Delay(BulkTransferPacketDelay_ms);
						}
						error_count = 0;
						sequence++;
						i += len;
						bytes_left -= len;
						if (bytes_left == 0)
						{
							return RESPONSE.SUCCESS;
						}
						if (bytes_left >= 0)
						{
							continue;
						}
						return RESPONSE.FAILED;
					}
					return RESPONSE.FAILED;
				}

				[AsyncStateMachine(typeof(<Request25EndBulkTransferAsync>d__13))]
				private async System.Threading.Tasks.Task<RESPONSE> Request25EndBulkTransferAsync(bool BlockUseSession, ISessionClient session)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					Tuple<RESPONSE, MessageBuffer> result = null;
					try
					{
						uint xfer_crc = CRC32_LE.Calculate(Data, (int)bulkXferSize, CurrentBlockOffset);
						result = await Client.TransmitRequestAsync(Operation, Target, (byte)37, PAYLOAD.FromArgs(new object[3]
						{
							BLOCK_ID.op_Implicit(Block.ID),
							(ushort)CurrentBlockOffset,
							xfer_crc
						}), delegate(LocalDeviceRxEvent rx)
						{
							if (rx.Length < 5)
							{
								return (RESPONSE?)null;
							}
							if (CommExtensions.GetUINT16((IByteList)(object)rx, 0) != BLOCK_ID.op_Implicit(Block.ID))
							{
								return (RESPONSE?)null;
							}
							if (CommExtensions.GetUINT16((IByteList)(object)rx, 2) != (ushort)CurrentBlockOffset)
							{
								return (RESPONSE?)null;
							}
							if (rx.Length == 5)
							{
								RESPONSE rESPONSE = (RESPONSE)rx[4];
								if (rESPONSE == RESPONSE.IN_PROGRESS)
								{
									if (BlockUseSession)
									{
										session.TryOpenSession();
									}
									return (RESPONSE?)null;
								}
								return rESPONSE;
							}
							return (rx.Length != 8) ? ((RESPONSE?)null) : new RESPONSE?(RESPONSE.SUCCESS);
						});
						if (result.Item1 != RESPONSE.SUCCESS)
						{
							return result.Item1;
						}
						MessageBuffer item = result.Item2;
						if (item == null || ((MessageBuffer)item).Length != 8)
						{
							return RESPONSE.FAILED;
						}
						if (CommExtensions.GetUINT16((IByteList)(object)result.Item2, 0) != BLOCK_ID.op_Implicit(Block.ID))
						{
							return RESPONSE.FAILED;
						}
						if (CommExtensions.GetUINT16((IByteList)(object)result.Item2, 2) != (ushort)CurrentBlockOffset)
						{
							return RESPONSE.FAILED;
						}
						if (CommExtensions.GetUINT32((IByteList)(object)result.Item2, 4) != xfer_crc)
						{
							return RESPONSE.CRC_INVALID;
						}
						return RESPONSE.SUCCESS;
					}
					finally
					{
						Tuple<RESPONSE, MessageBuffer> obj = result;
						if (obj != null)
						{
							MessageBuffer item2 = obj.Item2;
							if (item2 != null)
							{
								((Object)item2).ReturnToPool();
							}
						}
					}
				}

				[AsyncStateMachine(typeof(<Request26EndBlockWriteAsync>d__14))]
				private async System.Threading.Tasks.Task<RESPONSE> Request26EndBlockWriteAsync(bool BlockUseSession, ISessionClient session)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					Tuple<RESPONSE, MessageBuffer> result = null;
					try
					{
						result = await Client.TransmitRequestAsync(Operation, Target, (byte)38, PAYLOAD.FromArgs(new object[2]
						{
							BLOCK_ID.op_Implicit(Block.ID),
							TotalCRC
						}), delegate(LocalDeviceRxEvent rx)
						{
							if (rx.Length != 3)
							{
								return (RESPONSE?)null;
							}
							if (CommExtensions.GetUINT16((IByteList)(object)rx, 0) != BLOCK_ID.op_Implicit(Block.ID))
							{
								return (RESPONSE?)null;
							}
							RESPONSE rESPONSE = (RESPONSE)rx[2];
							if (rESPONSE == RESPONSE.IN_PROGRESS)
							{
								if (BlockUseSession)
								{
									session.TryOpenSession();
								}
								return (RESPONSE?)null;
							}
							return rESPONSE;
						});
						if (result.Item1 != RESPONSE.SUCCESS)
						{
							return result.Item1;
						}
						MessageBuffer item = result.Item2;
						if (item == null || ((MessageBuffer)item).Length != 3)
						{
							return RESPONSE.FAILED;
						}
						if (CommExtensions.GetUINT16((IByteList)(object)result.Item2, 0) != BLOCK_ID.op_Implicit(Block.ID))
						{
							return RESPONSE.FAILED;
						}
						return (RESPONSE)CommExtensions.GetUINT8((IByteList)(object)result.Item2, 2);
					}
					finally
					{
						Tuple<RESPONSE, MessageBuffer> obj = result;
						if (obj != null)
						{
							MessageBuffer item2 = obj.Item2;
							if (item2 != null)
							{
								((Object)item2).ReturnToPool();
							}
						}
					}
				}
			}

			[CompilerGenerated]
			private sealed class <>c__DisplayClass6_0
			{
				public int index;

				public Func<LocalDeviceRxEvent, RESPONSE?> <>9__1;

				internal RESPONSE? <ReadBlockListAsync>b__1(LocalDeviceRxEvent rx)
				{
					if (rx.Length == 8 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == index)
					{
						return RESPONSE.SUCCESS;
					}
					if (rx.Length == 1)
					{
						return (RESPONSE)rx[0];
					}
					return null;
				}
			}

			[CompilerGenerated]
			private sealed class <>c__DisplayClass7_0
			{
				public BLOCK_ID block;

				public int property;

				internal RESPONSE? <ReadBlockPropertyAsync>b__0(LocalDeviceRxEvent rx)
				{
					if (rx.Length == 1)
					{
						return (RESPONSE)rx[0];
					}
					if (rx.Length >= 4 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == BLOCK_ID.op_Implicit(block) && rx[2] == property)
					{
						if (rx.Length == 4)
						{
							RESPONSE rESPONSE = (RESPONSE)rx[3];
							if (rESPONSE == RESPONSE.IN_PROGRESS)
							{
								return null;
							}
							return rESPONSE;
						}
						if (rx.Length == 8)
						{
							return RESPONSE.SUCCESS;
						}
					}
					return null;
				}
			}

			[StructLayout((LayoutKind)3)]
			[CompilerGenerated]
			private struct <ReadBlockDataAsync>d__10 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> <>t__builder;

				public IBlock block;

				public ISessionClient session;

				public AsyncOperation operation;

				public BlockClient <>4__this;

				public int bulk_xfer_delay_ms;

				private TaskAwaiter <>u__1;

				private TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> <>u__2;

				private void MoveNext()
				{
					//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
					//IL_0167: Unknown result type (might be due to invalid IL or missing references)
					//IL_016c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0174: Unknown result type (might be due to invalid IL or missing references)
					//IL_0088: Unknown result type (might be due to invalid IL or missing references)
					//IL_008d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0135: Unknown result type (might be due to invalid IL or missing references)
					//IL_013a: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
					//IL_014f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0151: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					BlockClient blockClient = <>4__this;
					Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>> result;
					try
					{
						TaskAwaiter val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_00d9;
						}
						TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> val2;
						if (num == 1)
						{
							val2 = <>u__2;
							<>u__2 = default(TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>>);
							num = (<>1__state = -1);
							goto IL_0183;
						}
						bool blockUseSession;
						if (block.IsReadable())
						{
							blockUseSession = false;
							if (session != null && (byte)block.Device.ProtocolVersion >= 24 && session.SessionID != SESSION_ID.UNKNOWN)
							{
								goto IL_00e0;
							}
							goto IL_010c;
						}
						result = Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(RESPONSE.WRITE_ONLY, (System.Collections.Generic.IReadOnlyList<byte>)null);
						goto end_IL_000e;
						IL_010c:
						val2 = new BlockReadManager(operation, blockClient.Device, block, bulk_xfer_delay_ms).ReadBlockAsync(blockUseSession, session).GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__2 = val2;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>>, <ReadBlockDataAsync>d__10>(ref val2, ref this);
							return;
						}
						goto IL_0183;
						IL_00d9:
						((TaskAwaiter)(ref val)).GetResult();
						goto IL_00e0;
						IL_00e0:
						if (!session.IsOpen)
						{
							session.TryOpenSession();
							val = System.Threading.Tasks.Task.Delay(100).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <ReadBlockDataAsync>d__10>(ref val, ref this);
								return;
							}
							goto IL_00d9;
						}
						if (session.IsOpen)
						{
							blockUseSession = true;
							goto IL_010c;
						}
						result = Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(RESPONSE.SESSION_NOT_OPEN, (System.Collections.Generic.IReadOnlyList<byte>)null);
						goto end_IL_000e;
						IL_0183:
						result = val2.GetResult();
						end_IL_000e:;
					}
					catch (System.Exception exception)
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
			private struct <ReadBlockDataAsync>d__11 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> <>t__builder;

				public BlockClient <>4__this;

				public AsyncOperation operation;

				public IDevice target;

				public BLOCK_ID block;

				public int bulk_xfer_delay_ms;

				public ISessionClient session;

				private TaskAwaiter<Tuple<RESPONSE, IBlock>> <>u__1;

				private TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> <>u__2;

				private void MoveNext()
				{
					//IL_0065: Unknown result type (might be due to invalid IL or missing references)
					//IL_006a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0072: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
					//IL_0100: Unknown result type (might be due to invalid IL or missing references)
					//IL_0030: Unknown result type (might be due to invalid IL or missing references)
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
					//IL_004a: Unknown result type (might be due to invalid IL or missing references)
					//IL_004c: Unknown result type (might be due to invalid IL or missing references)
					//IL_00db: Unknown result type (might be due to invalid IL or missing references)
					//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					BlockClient blockClient = <>4__this;
					Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>> result2;
					try
					{
						TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> val;
						TaskAwaiter<Tuple<RESPONSE, IBlock>> val2;
						if (num != 0)
						{
							if (num == 1)
							{
								val = <>u__2;
								<>u__2 = default(TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>>);
								num = (<>1__state = -1);
								goto IL_010f;
							}
							val2 = blockClient.ReadBlockPropertiesAsync(operation, target, block).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val2;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, IBlock>>, <ReadBlockDataAsync>d__11>(ref val2, ref this);
								return;
							}
						}
						else
						{
							val2 = <>u__1;
							<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, IBlock>>);
							num = (<>1__state = -1);
						}
						Tuple<RESPONSE, IBlock> result = val2.GetResult();
						if (result.Item2 != null)
						{
							val = blockClient.ReadBlockDataAsync(operation, result.Item2, bulk_xfer_delay_ms, session).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__2 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>>, <ReadBlockDataAsync>d__11>(ref val, ref this);
								return;
							}
							goto IL_010f;
						}
						result2 = Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(result.Item1, (System.Collections.Generic.IReadOnlyList<byte>)null);
						goto end_IL_000e;
						IL_010f:
						result2 = val.GetResult();
						end_IL_000e:;
					}
					catch (System.Exception exception)
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
			private struct <ReadBlockListAsync>d__6 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>> <>t__builder;

				public IDevice target;

				public BlockClient <>4__this;

				public AsyncOperation operation;

				private <>c__DisplayClass6_0 <>8__1;

				private ulong <signature>5__2;

				private List<BLOCK_ID> <list>5__3;

				private int <total>5__4;

				private Tuple<RESPONSE, MessageBuffer> <result>5__5;

				private TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> <>u__1;

				private void MoveNext()
				{
					//IL_012d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0132: Unknown result type (might be due to invalid IL or missing references)
					//IL_013a: Unknown result type (might be due to invalid IL or missing references)
					//IL_00be: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
					//IL_0112: Unknown result type (might be due to invalid IL or missing references)
					//IL_0114: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					BlockClient blockClient = <>4__this;
					Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>> result;
					try
					{
						if (num == 0)
						{
							goto IL_0085;
						}
						<>8__1 = new <>c__DisplayClass6_0();
						<signature>5__2 = target.GetDeviceUniqueID();
						System.Collections.Generic.IReadOnlyList<BLOCK_ID> readOnlyList = default(System.Collections.Generic.IReadOnlyList<BLOCK_ID>);
						if (!blockClient.BlockListCache.TryGetValue(<signature>5__2, ref readOnlyList))
						{
							<list>5__3 = new List<BLOCK_ID>();
							<>8__1.index = 0;
							<total>5__4 = 0;
							operation.ReportProgress("Reading number of blocks...");
							goto IL_007d;
						}
						result = Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>(RESPONSE.SUCCESS, readOnlyList);
						goto end_IL_000e;
						IL_007d:
						<result>5__5 = null;
						goto IL_0085;
						IL_0085:
						try
						{
							TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> val;
							if (num != 0)
							{
								val = blockClient.Device.TransmitRequestAsync(operation, target, (byte)32, PAYLOAD.FromArgs(new object[1] { (ushort)<>8__1.index }), delegate(LocalDeviceRxEvent rx)
								{
									if (rx.Length == 8 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == <>8__1.index)
									{
										return RESPONSE.SUCCESS;
									}
									return (rx.Length == 1) ? new RESPONSE?((RESPONSE)rx[0]) : ((RESPONSE?)null);
								}).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>, <ReadBlockListAsync>d__6>(ref val, ref this);
									return;
								}
							}
							else
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>);
								num = (<>1__state = -1);
							}
							Tuple<RESPONSE, MessageBuffer> result2 = val.GetResult();
							<result>5__5 = result2;
							MessageBuffer item = <result>5__5.Item2;
							if (item == null)
							{
								result = Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>(<result>5__5.Item1, (System.Collections.Generic.IReadOnlyList<BLOCK_ID>)null);
							}
							else
							{
								int num2 = <>8__1.index;
								<>8__1.index = num2 + 1;
								if (num2 == 0)
								{
									<total>5__4 = CommExtensions.GetUINT16((IByteList)(object)item, 2);
								}
								else if (<list>5__3.Count < <total>5__4)
								{
									<list>5__3.Add(BLOCK_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)item, 2)));
								}
								if (<list>5__3.Count < <total>5__4)
								{
									<list>5__3.Add(BLOCK_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)item, 4)));
								}
								if (<list>5__3.Count < <total>5__4)
								{
									<list>5__3.Add(BLOCK_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)item, 6)));
								}
								operation.ReportProgress(100f * (float)<list>5__3.Count / (float)<total>5__4, $"Read {<list>5__3.Count} of {<total>5__4} blocks...");
								if (<list>5__3.Count < <total>5__4)
								{
									goto end_IL_0085;
								}
								<list>5__3.Sort((Comparison<BLOCK_ID>)((BLOCK_ID first, BLOCK_ID second) => first.Value.CompareTo(second.Value)));
								blockClient.BlockListCache.TryAdd(<signature>5__2, (System.Collections.Generic.IReadOnlyList<BLOCK_ID>)<list>5__3);
								operation.ReportProgress(100f, "Success!");
								result = Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>(RESPONSE.SUCCESS, (System.Collections.Generic.IReadOnlyList<BLOCK_ID>)<list>5__3);
							}
							goto end_IL_000e;
							end_IL_0085:;
						}
						finally
						{
							if (num < 0)
							{
								Tuple<RESPONSE, MessageBuffer> obj = <result>5__5;
								if (obj != null)
								{
									MessageBuffer item2 = obj.Item2;
									if (item2 != null)
									{
										((Object)item2).ReturnToPool();
									}
								}
							}
						}
						<result>5__5 = null;
						goto IL_007d;
						end_IL_000e:;
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						<>8__1 = null;
						<list>5__3 = null;
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<>8__1 = null;
					<list>5__3 = null;
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
			private struct <ReadBlockPropertiesAsync>d__8 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<Tuple<RESPONSE, IBlock>> <>t__builder;

				public AsyncOperation operation;

				public BLOCK_ID block;

				public BlockClient <>4__this;

				public IDevice target;

				private BLOCK_FLAGS <flags>5__2;

				private SESSION_ID <read_session_id>5__3;

				private SESSION_ID <write_session_id>5__4;

				private ulong <capacity>5__5;

				private ulong <size>5__6;

				private uint <crc>5__7;

				private TaskAwaiter<Tuple<RESPONSE, ulong?>> <>u__1;

				private void MoveNext()
				{
					//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
					//IL_019d: Unknown result type (might be due to invalid IL or missing references)
					//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
					//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
					//IL_0284: Unknown result type (might be due to invalid IL or missing references)
					//IL_0289: Unknown result type (might be due to invalid IL or missing references)
					//IL_0291: Unknown result type (might be due to invalid IL or missing references)
					//IL_0349: Unknown result type (might be due to invalid IL or missing references)
					//IL_034e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0356: Unknown result type (might be due to invalid IL or missing references)
					//IL_0408: Unknown result type (might be due to invalid IL or missing references)
					//IL_040d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0415: Unknown result type (might be due to invalid IL or missing references)
					//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
					//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
					//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
					//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
					//IL_05af: Unknown result type (might be due to invalid IL or missing references)
					//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
					//IL_008a: Unknown result type (might be due to invalid IL or missing references)
					//IL_008f: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
					//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
					//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
					//IL_0492: Unknown result type (might be due to invalid IL or missing references)
					//IL_0497: Unknown result type (might be due to invalid IL or missing references)
					//IL_0168: Unknown result type (might be due to invalid IL or missing references)
					//IL_016d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0314: Unknown result type (might be due to invalid IL or missing references)
					//IL_0319: Unknown result type (might be due to invalid IL or missing references)
					//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
					//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
					//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
					//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
					//IL_0575: Unknown result type (might be due to invalid IL or missing references)
					//IL_057a: Unknown result type (might be due to invalid IL or missing references)
					//IL_024f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0254: Unknown result type (might be due to invalid IL or missing references)
					//IL_0182: Unknown result type (might be due to invalid IL or missing references)
					//IL_0184: Unknown result type (might be due to invalid IL or missing references)
					//IL_032e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0330: Unknown result type (might be due to invalid IL or missing references)
					//IL_058f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0591: Unknown result type (might be due to invalid IL or missing references)
					//IL_0269: Unknown result type (might be due to invalid IL or missing references)
					//IL_026b: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					BlockClient blockClient = <>4__this;
					Tuple<RESPONSE, IBlock> result2;
					try
					{
						TaskAwaiter<Tuple<RESPONSE, ulong?>> val;
						Tuple<RESPONSE, ulong?> result;
						Tuple<RESPONSE, ulong?> result3;
						ulong startaddress;
						Tuple<RESPONSE, ulong?> result4;
						Tuple<RESPONSE, ulong?> result5;
						Tuple<RESPONSE, ulong?> result6;
						Tuple<RESPONSE, ulong?> result7;
						Tuple<RESPONSE, ulong?> result8;
						switch (num)
						{
						default:
							operation.ReportProgress($"Reading block {block} information...");
							val = blockClient.ReadBlockPropertyAsync(operation, target, block, 0).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, ulong?>>, <ReadBlockPropertiesAsync>d__8>(ref val, ref this);
								return;
							}
							goto IL_00db;
						case 0:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, ulong?>>);
							num = (<>1__state = -1);
							goto IL_00db;
						case 1:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, ulong?>>);
							num = (<>1__state = -1);
							goto IL_01b9;
						case 2:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, ulong?>>);
							num = (<>1__state = -1);
							goto IL_02a0;
						case 3:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, ulong?>>);
							num = (<>1__state = -1);
							goto IL_0365;
						case 4:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, ulong?>>);
							num = (<>1__state = -1);
							goto IL_0424;
						case 5:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, ulong?>>);
							num = (<>1__state = -1);
							goto IL_04e3;
						case 6:
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, ulong?>>);
								num = (<>1__state = -1);
								goto IL_05c6;
							}
							IL_0203:
							operation.ReportProgress(33.333332f, (string)null);
							<write_session_id>5__4 = null;
							if (((System.Enum)<flags>5__2).HasFlag((System.Enum)BLOCK_FLAGS.WRITABLE))
							{
								val = blockClient.ReadBlockPropertyAsync(operation, target, block, 2).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 2);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, ulong?>>, <ReadBlockPropertiesAsync>d__8>(ref val, ref this);
									return;
								}
								goto IL_02a0;
							}
							goto IL_02ea;
							IL_00db:
							result = val.GetResult();
							if (result.Item2.HasValue)
							{
								operation.ReportProgress(16.666666f, (string)null);
								<flags>5__2 = (BLOCK_FLAGS)result.Item2.Value;
								<read_session_id>5__3 = null;
								if (((System.Enum)<flags>5__2).HasFlag((System.Enum)BLOCK_FLAGS.READABLE))
								{
									val = blockClient.ReadBlockPropertyAsync(operation, target, block, 1).GetAwaiter();
									if (!val.IsCompleted)
									{
										num = (<>1__state = 1);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, ulong?>>, <ReadBlockPropertiesAsync>d__8>(ref val, ref this);
										return;
									}
									goto IL_01b9;
								}
								goto IL_0203;
							}
							result2 = Tuple.Create<RESPONSE, IBlock>(result.Item1, (IBlock)null);
							goto end_IL_000e;
							IL_04e3:
							result3 = val.GetResult();
							if (result3.Item2.HasValue)
							{
								<crc>5__7 = (uint)result3.Item2.Value;
								operation.ReportProgress(100f, "Success!");
								startaddress = 0uL;
								if (!((System.Enum)<flags>5__2).HasFlag((System.Enum)BLOCK_FLAGS.USE_SET_START_ADDRESS))
								{
									break;
								}
								val = blockClient.ReadBlockPropertyAsync(operation, target, block, 7).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 6);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, ulong?>>, <ReadBlockPropertiesAsync>d__8>(ref val, ref this);
									return;
								}
								goto IL_05c6;
							}
							result2 = Tuple.Create<RESPONSE, IBlock>(result3.Item1, (IBlock)null);
							goto end_IL_000e;
							IL_0365:
							result4 = val.GetResult();
							if (result4.Item2.HasValue)
							{
								<capacity>5__5 = result4.Item2.Value;
								operation.ReportProgress(66.666664f, (string)null);
								val = blockClient.ReadBlockPropertyAsync(operation, target, block, 4).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 4);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, ulong?>>, <ReadBlockPropertiesAsync>d__8>(ref val, ref this);
									return;
								}
								goto IL_0424;
							}
							result2 = Tuple.Create<RESPONSE, IBlock>(result4.Item1, (IBlock)null);
							goto end_IL_000e;
							IL_02a0:
							result5 = val.GetResult();
							if (result5.Item2.HasValue)
							{
								<write_session_id>5__4 = SESSION_ID.op_Implicit((ushort)result5.Item2.Value);
								goto IL_02ea;
							}
							result2 = Tuple.Create<RESPONSE, IBlock>(result5.Item1, (IBlock)null);
							goto end_IL_000e;
							IL_0424:
							result6 = val.GetResult();
							if (result6.Item2.HasValue)
							{
								<size>5__6 = result6.Item2.Value;
								operation.ReportProgress(83.333336f, (string)null);
								val = blockClient.ReadBlockPropertyAsync(operation, target, block, 5).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 5);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, ulong?>>, <ReadBlockPropertiesAsync>d__8>(ref val, ref this);
									return;
								}
								goto IL_04e3;
							}
							result2 = Tuple.Create<RESPONSE, IBlock>(result6.Item1, (IBlock)null);
							goto end_IL_000e;
							IL_01b9:
							result7 = val.GetResult();
							if (result7.Item2.HasValue)
							{
								<read_session_id>5__3 = SESSION_ID.op_Implicit((ushort)result7.Item2.Value);
								goto IL_0203;
							}
							result2 = Tuple.Create<RESPONSE, IBlock>(result7.Item1, (IBlock)null);
							goto end_IL_000e;
							IL_05c6:
							result8 = val.GetResult();
							if (result8.Item2.HasValue)
							{
								startaddress = result8.Item2.Value;
								operation.ReportProgress(83.333336f, (string)null);
								break;
							}
							result2 = Tuple.Create<RESPONSE, IBlock>(result8.Item1, (IBlock)null);
							goto end_IL_000e;
							IL_02ea:
							operation.ReportProgress(50f, (string)null);
							val = blockClient.ReadBlockPropertyAsync(operation, target, block, 3).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 3);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, ulong?>>, <ReadBlockPropertiesAsync>d__8>(ref val, ref this);
								return;
							}
							goto IL_0365;
						}
						SetBlock setBlock = new SetBlock(target, block, <flags>5__2, <read_session_id>5__3, <write_session_id>5__4, <capacity>5__5, <size>5__6, <crc>5__7, startaddress);
						result2 = Tuple.Create<RESPONSE, IBlock>(RESPONSE.SUCCESS, (IBlock)setBlock);
						end_IL_000e:;
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						<read_session_id>5__3 = null;
						<write_session_id>5__4 = null;
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<read_session_id>5__3 = null;
					<write_session_id>5__4 = null;
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
			private struct <ReadBlockPropertyAsync>d__7 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<Tuple<RESPONSE, ulong?>> <>t__builder;

				public BLOCK_ID block;

				public int property;

				public BlockClient <>4__this;

				public AsyncOperation operation;

				public IDevice dest;

				private Tuple<RESPONSE, MessageBuffer> <result>5__2;

				private TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> <>u__1;

				private void MoveNext()
				{
					//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
					//IL_007e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0094: Unknown result type (might be due to invalid IL or missing references)
					//IL_0099: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					BlockClient blockClient = <>4__this;
					Tuple<RESPONSE, ulong?> result2;
					try
					{
						<>c__DisplayClass7_0 <>c__DisplayClass7_ = default(<>c__DisplayClass7_0);
						if (num != 0)
						{
							<>c__DisplayClass7_ = new <>c__DisplayClass7_0
							{
								block = block,
								property = property
							};
							<result>5__2 = null;
						}
						try
						{
							TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> val;
							if (num != 0)
							{
								val = blockClient.Device.TransmitRequestAsync(operation, dest, (byte)33, PAYLOAD.FromArgs(new object[2]
								{
									BLOCK_ID.op_Implicit(<>c__DisplayClass7_.block),
									(byte)<>c__DisplayClass7_.property
								}), <>c__DisplayClass7_.<ReadBlockPropertyAsync>b__0).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>, <ReadBlockPropertyAsync>d__7>(ref val, ref this);
									return;
								}
							}
							else
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>);
								num = (<>1__state = -1);
							}
							Tuple<RESPONSE, MessageBuffer> result = val.GetResult();
							<result>5__2 = result;
							if (<result>5__2.Item2 == null)
							{
								result2 = Tuple.Create<RESPONSE, ulong?>(<result>5__2.Item1, (ulong?)null);
							}
							else
							{
								ulong num2 = 0uL;
								for (int i = 3; i < ((MessageBuffer)<result>5__2.Item2).Length; i++)
								{
									num2 <<= 8;
									num2 += CommExtensions.GetUINT8((IByteList)(object)<result>5__2.Item2, i);
								}
								result2 = Tuple.Create<RESPONSE, ulong?>(RESPONSE.SUCCESS, (ulong?)num2);
							}
						}
						finally
						{
							if (num < 0)
							{
								Tuple<RESPONSE, MessageBuffer> obj = <result>5__2;
								if (obj != null)
								{
									MessageBuffer item = obj.Item2;
									if (item != null)
									{
										((Object)item).ReturnToPool();
									}
								}
							}
						}
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						<result>5__2 = null;
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<result>5__2 = null;
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
			private struct <RecalculateBlockCrcAsync>d__9 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<Tuple<RESPONSE, uint?>> <>t__builder;

				public AsyncOperation operation;

				public BLOCK_ID block;

				public BlockClient <>4__this;

				public IDevice target;

				private TaskAwaiter<Tuple<RESPONSE, ulong?>> <>u__1;

				private void MoveNext()
				{
					//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
					//IL_006e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0073: Unknown result type (might be due to invalid IL or missing references)
					//IL_0088: Unknown result type (might be due to invalid IL or missing references)
					//IL_008a: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					BlockClient blockClient = <>4__this;
					Tuple<RESPONSE, uint?> result2;
					try
					{
						TaskAwaiter<Tuple<RESPONSE, ulong?>> val;
						if (num != 0)
						{
							operation.ReportProgress($"Computing block {block} CRC...");
							val = blockClient.ReadBlockPropertyAsync(operation, target, block, 6).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, ulong?>>, <RecalculateBlockCrcAsync>d__9>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, ulong?>>);
							num = (<>1__state = -1);
						}
						Tuple<RESPONSE, ulong?> result = val.GetResult();
						if (!result.Item2.HasValue)
						{
							result2 = Tuple.Create<RESPONSE, uint?>(result.Item1, (uint?)null);
						}
						else
						{
							operation.ReportProgress(100f, "Success!");
							result2 = Tuple.Create<RESPONSE, uint?>(RESPONSE.SUCCESS, (uint?)(uint)result.Item2.Value);
						}
					}
					catch (System.Exception exception)
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
			private struct <WriteBlockDataAsync>d__12 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<RESPONSE> <>t__builder;

				public ISessionClient session;

				public IBlock block;

				public AsyncOperation operation;

				public BlockClient <>4__this;

				public System.Collections.Generic.IReadOnlyList<byte> data;

				public int bulk_xfer_delay_ms;

				private TaskAwaiter <>u__1;

				private TaskAwaiter<RESPONSE> <>u__2;

				private void MoveNext()
				{
					//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
					//IL_014d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0152: Unknown result type (might be due to invalid IL or missing references)
					//IL_015a: Unknown result type (might be due to invalid IL or missing references)
					//IL_011b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0120: Unknown result type (might be due to invalid IL or missing references)
					//IL_006e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0073: Unknown result type (might be due to invalid IL or missing references)
					//IL_0135: Unknown result type (might be due to invalid IL or missing references)
					//IL_0137: Unknown result type (might be due to invalid IL or missing references)
					//IL_0088: Unknown result type (might be due to invalid IL or missing references)
					//IL_008a: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					BlockClient blockClient = <>4__this;
					RESPONSE result;
					try
					{
						bool blockUseSession;
						TaskAwaiter<RESPONSE> val;
						if (num != 0)
						{
							if (num != 1)
							{
								blockUseSession = false;
								if (session != null && (byte)block.Device.ProtocolVersion >= 24 && session.SessionID != SESSION_ID.UNKNOWN)
								{
									goto IL_00c6;
								}
								goto IL_00ec;
							}
							val = <>u__2;
							<>u__2 = default(TaskAwaiter<RESPONSE>);
							num = (<>1__state = -1);
							goto IL_0169;
						}
						TaskAwaiter val2 = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_00bf;
						IL_0169:
						result = val.GetResult();
						goto end_IL_000e;
						IL_00bf:
						((TaskAwaiter)(ref val2)).GetResult();
						goto IL_00c6;
						IL_00ec:
						val = new BlockWriteManager(operation, blockClient.Device, block, data, bulk_xfer_delay_ms).WriteBlockAsync(blockUseSession, session).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__2 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<RESPONSE>, <WriteBlockDataAsync>d__12>(ref val, ref this);
							return;
						}
						goto IL_0169;
						IL_00c6:
						if (!session.IsOpen)
						{
							session.TryOpenSession();
							val2 = System.Threading.Tasks.Task.Delay(100).GetAwaiter();
							if (!((TaskAwaiter)(ref val2)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val2;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <WriteBlockDataAsync>d__12>(ref val2, ref this);
								return;
							}
							goto IL_00bf;
						}
						if (session.IsOpen)
						{
							blockUseSession = true;
							goto IL_00ec;
						}
						result = RESPONSE.SESSION_NOT_OPEN;
						end_IL_000e:;
					}
					catch (System.Exception exception)
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

			private readonly LocalDevice Device;

			private ConcurrentDictionary<ulong, System.Collections.Generic.IReadOnlyList<BLOCK_ID>> BlockListCache = new ConcurrentDictionary<ulong, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>();

			public BlockClient(LocalDevice device)
			{
				Device = device;
			}

			[AsyncStateMachine(typeof(<ReadBlockListAsync>d__6))]
			public async System.Threading.Tasks.Task<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>> ReadBlockListAsync(AsyncOperation operation, IDevice target)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				ulong signature = target.GetDeviceUniqueID();
				System.Collections.Generic.IReadOnlyList<BLOCK_ID> readOnlyList = default(System.Collections.Generic.IReadOnlyList<BLOCK_ID>);
				if (BlockListCache.TryGetValue(signature, ref readOnlyList))
				{
					return Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>(RESPONSE.SUCCESS, readOnlyList);
				}
				List<BLOCK_ID> list = new List<BLOCK_ID>();
				int index = 0;
				int total = 0;
				operation.ReportProgress("Reading number of blocks...");
				while (true)
				{
					Tuple<RESPONSE, MessageBuffer> result = null;
					try
					{
						result = await Device.TransmitRequestAsync(operation, target, (byte)32, PAYLOAD.FromArgs(new object[1] { (ushort)index }), delegate(LocalDeviceRxEvent rx)
						{
							if (rx.Length == 8 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == index)
							{
								return RESPONSE.SUCCESS;
							}
							return (rx.Length == 1) ? new RESPONSE?((RESPONSE)rx[0]) : ((RESPONSE?)null);
						});
						MessageBuffer item = result.Item2;
						if (item == null)
						{
							return Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>(result.Item1, (System.Collections.Generic.IReadOnlyList<BLOCK_ID>)null);
						}
						if (index++ == 0)
						{
							total = CommExtensions.GetUINT16((IByteList)(object)item, 2);
						}
						else if (list.Count < total)
						{
							list.Add(BLOCK_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)item, 2)));
						}
						if (list.Count < total)
						{
							list.Add(BLOCK_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)item, 4)));
						}
						if (list.Count < total)
						{
							list.Add(BLOCK_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)item, 6)));
						}
						operation.ReportProgress(100f * (float)list.Count / (float)total, $"Read {list.Count} of {total} blocks...");
						if (list.Count >= total)
						{
							list.Sort((Comparison<BLOCK_ID>)((BLOCK_ID first, BLOCK_ID second) => first.Value.CompareTo(second.Value)));
							BlockListCache.TryAdd(signature, (System.Collections.Generic.IReadOnlyList<BLOCK_ID>)list);
							operation.ReportProgress(100f, "Success!");
							return Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>(RESPONSE.SUCCESS, (System.Collections.Generic.IReadOnlyList<BLOCK_ID>)list);
						}
					}
					finally
					{
						Tuple<RESPONSE, MessageBuffer> obj = result;
						if (obj != null)
						{
							MessageBuffer item2 = obj.Item2;
							if (item2 != null)
							{
								((Object)item2).ReturnToPool();
							}
						}
					}
				}
			}

			[AsyncStateMachine(typeof(<ReadBlockPropertyAsync>d__7))]
			private async System.Threading.Tasks.Task<Tuple<RESPONSE, ulong?>> ReadBlockPropertyAsync(AsyncOperation operation, IDevice dest, BLOCK_ID block, int property)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				Tuple<RESPONSE, MessageBuffer> result = null;
				try
				{
					result = await Device.TransmitRequestAsync(operation, dest, (byte)33, PAYLOAD.FromArgs(new object[2]
					{
						BLOCK_ID.op_Implicit(block),
						(byte)property
					}), delegate(LocalDeviceRxEvent rx)
					{
						if (rx.Length == 1)
						{
							return (RESPONSE)rx[0];
						}
						if (rx.Length >= 4 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == BLOCK_ID.op_Implicit(block) && rx[2] == property)
						{
							if (rx.Length == 4)
							{
								RESPONSE rESPONSE = (RESPONSE)rx[3];
								if (rESPONSE == RESPONSE.IN_PROGRESS)
								{
									return (RESPONSE?)null;
								}
								return rESPONSE;
							}
							if (rx.Length == 8)
							{
								return RESPONSE.SUCCESS;
							}
						}
						return (RESPONSE?)null;
					});
					if (result.Item2 == null)
					{
						return Tuple.Create<RESPONSE, ulong?>(result.Item1, (ulong?)null);
					}
					ulong num = 0uL;
					for (int num2 = 3; num2 < ((MessageBuffer)result.Item2).Length; num2++)
					{
						num <<= 8;
						num += CommExtensions.GetUINT8((IByteList)(object)result.Item2, num2);
					}
					return Tuple.Create<RESPONSE, ulong?>(RESPONSE.SUCCESS, (ulong?)num);
				}
				finally
				{
					Tuple<RESPONSE, MessageBuffer> obj = result;
					if (obj != null)
					{
						MessageBuffer item = obj.Item2;
						if (item != null)
						{
							((Object)item).ReturnToPool();
						}
					}
				}
			}

			[AsyncStateMachine(typeof(<ReadBlockPropertiesAsync>d__8))]
			public async System.Threading.Tasks.Task<Tuple<RESPONSE, IBlock>> ReadBlockPropertiesAsync(AsyncOperation operation, IDevice target, BLOCK_ID block)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				operation.ReportProgress($"Reading block {block} information...");
				Tuple<RESPONSE, ulong?> val = await ReadBlockPropertyAsync(operation, target, block, 0);
				if (!val.Item2.HasValue)
				{
					return Tuple.Create<RESPONSE, IBlock>(val.Item1, (IBlock)null);
				}
				operation.ReportProgress(16.666666f, (string)null);
				BLOCK_FLAGS flags = (BLOCK_FLAGS)val.Item2.Value;
				SESSION_ID read_session_id = null;
				if (((System.Enum)flags).HasFlag((System.Enum)BLOCK_FLAGS.READABLE))
				{
					Tuple<RESPONSE, ulong?> val2 = await ReadBlockPropertyAsync(operation, target, block, 1);
					if (!val2.Item2.HasValue)
					{
						return Tuple.Create<RESPONSE, IBlock>(val2.Item1, (IBlock)null);
					}
					read_session_id = SESSION_ID.op_Implicit((ushort)val2.Item2.Value);
				}
				operation.ReportProgress(33.333332f, (string)null);
				SESSION_ID write_session_id = null;
				if (((System.Enum)flags).HasFlag((System.Enum)BLOCK_FLAGS.WRITABLE))
				{
					Tuple<RESPONSE, ulong?> val3 = await ReadBlockPropertyAsync(operation, target, block, 2);
					if (!val3.Item2.HasValue)
					{
						return Tuple.Create<RESPONSE, IBlock>(val3.Item1, (IBlock)null);
					}
					write_session_id = SESSION_ID.op_Implicit((ushort)val3.Item2.Value);
				}
				operation.ReportProgress(50f, (string)null);
				Tuple<RESPONSE, ulong?> val4 = await ReadBlockPropertyAsync(operation, target, block, 3);
				if (!val4.Item2.HasValue)
				{
					return Tuple.Create<RESPONSE, IBlock>(val4.Item1, (IBlock)null);
				}
				ulong capacity = val4.Item2.Value;
				operation.ReportProgress(66.666664f, (string)null);
				Tuple<RESPONSE, ulong?> val5 = await ReadBlockPropertyAsync(operation, target, block, 4);
				if (!val5.Item2.HasValue)
				{
					return Tuple.Create<RESPONSE, IBlock>(val5.Item1, (IBlock)null);
				}
				ulong size = val5.Item2.Value;
				operation.ReportProgress(83.333336f, (string)null);
				Tuple<RESPONSE, ulong?> val6 = await ReadBlockPropertyAsync(operation, target, block, 5);
				if (!val6.Item2.HasValue)
				{
					return Tuple.Create<RESPONSE, IBlock>(val6.Item1, (IBlock)null);
				}
				uint crc = (uint)val6.Item2.Value;
				operation.ReportProgress(100f, "Success!");
				ulong startaddress = 0uL;
				if (((System.Enum)flags).HasFlag((System.Enum)BLOCK_FLAGS.USE_SET_START_ADDRESS))
				{
					Tuple<RESPONSE, ulong?> val7 = await ReadBlockPropertyAsync(operation, target, block, 7);
					if (!val7.Item2.HasValue)
					{
						return Tuple.Create<RESPONSE, IBlock>(val7.Item1, (IBlock)null);
					}
					startaddress = val7.Item2.Value;
					operation.ReportProgress(83.333336f, (string)null);
				}
				SetBlock setBlock = new SetBlock(target, block, flags, read_session_id, write_session_id, capacity, size, crc, startaddress);
				return Tuple.Create<RESPONSE, IBlock>(RESPONSE.SUCCESS, (IBlock)setBlock);
			}

			[AsyncStateMachine(typeof(<RecalculateBlockCrcAsync>d__9))]
			public async System.Threading.Tasks.Task<Tuple<RESPONSE, uint?>> RecalculateBlockCrcAsync(AsyncOperation operation, IDevice target, BLOCK_ID block)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				operation.ReportProgress($"Computing block {block} CRC...");
				Tuple<RESPONSE, ulong?> val = await ReadBlockPropertyAsync(operation, target, block, 6);
				if (!val.Item2.HasValue)
				{
					return Tuple.Create<RESPONSE, uint?>(val.Item1, (uint?)null);
				}
				operation.ReportProgress(100f, "Success!");
				return Tuple.Create<RESPONSE, uint?>(RESPONSE.SUCCESS, (uint?)(uint)val.Item2.Value);
			}

			[AsyncStateMachine(typeof(<ReadBlockDataAsync>d__10))]
			public async System.Threading.Tasks.Task<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> ReadBlockDataAsync(AsyncOperation operation, IBlock block, int bulk_xfer_delay_ms, ISessionClient session)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				if (!block.IsReadable())
				{
					return Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(RESPONSE.WRITE_ONLY, (System.Collections.Generic.IReadOnlyList<byte>)null);
				}
				bool blockUseSession = false;
				if (session != null && (byte)block.Device.ProtocolVersion >= 24 && session.SessionID != SESSION_ID.UNKNOWN)
				{
					while (!session.IsOpen)
					{
						session.TryOpenSession();
						await System.Threading.Tasks.Task.Delay(100);
					}
					if (!session.IsOpen)
					{
						return Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(RESPONSE.SESSION_NOT_OPEN, (System.Collections.Generic.IReadOnlyList<byte>)null);
					}
					blockUseSession = true;
				}
				return await new BlockReadManager(operation, Device, block, bulk_xfer_delay_ms).ReadBlockAsync(blockUseSession, session);
			}

			[AsyncStateMachine(typeof(<ReadBlockDataAsync>d__11))]
			public async System.Threading.Tasks.Task<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> ReadBlockDataAsync(AsyncOperation operation, IDevice target, BLOCK_ID block, int bulk_xfer_delay_ms, ISessionClient session)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				Tuple<RESPONSE, IBlock> val = await ReadBlockPropertiesAsync(operation, target, block);
				if (val.Item2 == null)
				{
					return Tuple.Create<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>(val.Item1, (System.Collections.Generic.IReadOnlyList<byte>)null);
				}
				return await ReadBlockDataAsync(operation, val.Item2, bulk_xfer_delay_ms, session);
			}

			[AsyncStateMachine(typeof(<WriteBlockDataAsync>d__12))]
			public async System.Threading.Tasks.Task<RESPONSE> WriteBlockDataAsync(AsyncOperation operation, IBlock block, System.Collections.Generic.IReadOnlyList<byte> data, int bulk_xfer_delay_ms, ISessionClient session)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				bool blockUseSession = false;
				if (session != null && (byte)block.Device.ProtocolVersion >= 24 && session.SessionID != SESSION_ID.UNKNOWN)
				{
					while (!session.IsOpen)
					{
						session.TryOpenSession();
						await System.Threading.Tasks.Task.Delay(100);
					}
					if (!session.IsOpen)
					{
						return RESPONSE.SESSION_NOT_OPEN;
					}
					blockUseSession = true;
				}
				return await new BlockWriteManager(operation, Device, block, data, bulk_xfer_delay_ms).WriteBlockAsync(blockUseSession, session);
			}
		}

		public class LocalBlock : ILocalBlock, IBlock
		{
			private readonly object Mutex = new object();

			private BlockWriter Writer;

			private System.Collections.Generic.IReadOnlyList<byte> mData;

			[field: CompilerGenerated]
			public LocalDevice Device
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			IDevice IBlock.Device => Device;

			[field: CompilerGenerated]
			public BLOCK_ID ID
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			[field: CompilerGenerated]
			public BLOCK_FLAGS Flags
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			[field: CompilerGenerated]
			public ulong Capacity
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			public ulong Size => (uint)((System.Collections.Generic.IReadOnlyCollection<byte>)Data).Count;

			[field: CompilerGenerated]
			public uint CRC
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				set;
			}

			[field: CompilerGenerated]
			public ulong StartAddress
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				set;
			}

			[field: CompilerGenerated]
			public ulong SetSize
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				set;
			}

			[field: CompilerGenerated]
			public SESSION_ID ReadSessionID
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			[field: CompilerGenerated]
			public SESSION_ID WriteSessionID
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			public System.Collections.Generic.IReadOnlyList<byte> Data
			{
				get
				{
					return mData;
				}
				protected set
				{
					//IL_001f: Unknown result type (might be due to invalid IL or missing references)
					if (value == null)
					{
						value = new byte[0];
					}
					if ((uint)((System.Collections.Generic.IReadOnlyCollection<byte>)value).Count > Capacity)
					{
						throw new ArgumentException("Data is too large for buffer");
					}
					mData = value;
					CRC = CalculateCRC();
				}
			}

			public LocalBlock(LocalDevice device, BLOCK_ID id, ulong capacity, SESSION_ID read_session, SESSION_ID write_session)
				: this(device, id, capacity, read_session, write_session, new byte[0])
			{
			}

			public LocalBlock(LocalDevice device, BLOCK_ID id, ulong capacity, SESSION_ID read_session, SESSION_ID write_session, System.Collections.Generic.IReadOnlyList<byte> data)
			{
				Device = device;
				ID = id;
				Capacity = capacity;
				ReadSessionID = read_session;
				WriteSessionID = write_session;
				BLOCK_FLAGS bLOCK_FLAGS = BLOCK_FLAGS.NONE;
				if (this.IsReadable())
				{
					bLOCK_FLAGS |= BLOCK_FLAGS.READABLE;
				}
				if (this.IsWritable())
				{
					bLOCK_FLAGS |= BLOCK_FLAGS.WRITABLE;
				}
				Flags = bLOCK_FLAGS;
				Data = data;
			}

			public uint CalculateCRC()
			{
				return CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyCollection<byte>)Data);
			}

			public virtual bool WriteData(System.Collections.Generic.IReadOnlyList<byte> buf)
			{
				if (this.IsWritable() && (uint)(((System.Collections.Generic.IReadOnlyCollection<byte>)buf)?.Count).Value <= Capacity)
				{
					Data = Enumerable.ToArray<byte>((System.Collections.Generic.IEnumerable<byte>)buf);
					return true;
				}
				return false;
			}

			internal BlockWriter GetBlockWriter()
			{
				if (Writer == null && this.IsWritable())
				{
					lock (Mutex)
					{
						if (Writer == null)
						{
							Writer = new BlockWriter(this);
						}
					}
				}
				return Writer;
			}
		}

		internal class BlockWriter
		{
			private enum STATE
			{
				IDLE,
				WAITING_FOR_BULK_XFER,
				BULK_XFER
			}

			private class BusEndpoint : IBusEndpoint
			{
				[field: CompilerGenerated]
				public IAdapter Adapter
				{
					[CompilerGenerated]
					get;
					[CompilerGenerated]
					private set;
				}

				[field: CompilerGenerated]
				public ADDRESS Address
				{
					[CompilerGenerated]
					get;
					[CompilerGenerated]
					private set;
				}

				public bool IsOnline => true;

				public BusEndpoint(IAdapter adapter, ADDRESS address)
				{
					Adapter = adapter;
					Address = address;
				}
			}

			[CompilerGenerated]
			private sealed class <>c__DisplayClass16_0
			{
				[StructLayout((LayoutKind)3)]
				private struct <<BeginBulkTransfer>b__0>d : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder <>t__builder;

					public <>c__DisplayClass16_0 <>4__this;

					private <>c__DisplayClass16_1 <>8__1;

					private ReusableSubscription <rx_listener>5__2;

					private BusEndpoint <dest>5__3;

					private RESPONSE <response>5__4;

					private int <retry>5__5;

					private TaskAwaiter <>u__1;

					private TaskAwaiter<System.Threading.Tasks.Task> <>u__2;

					private void MoveNext()
					{
						//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
						//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
						//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
						//IL_0273: Unknown result type (might be due to invalid IL or missing references)
						//IL_0278: Unknown result type (might be due to invalid IL or missing references)
						//IL_0280: Unknown result type (might be due to invalid IL or missing references)
						//IL_03db: Unknown result type (might be due to invalid IL or missing references)
						//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
						//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
						//IL_050a: Unknown result type (might be due to invalid IL or missing references)
						//IL_050f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0516: Unknown result type (might be due to invalid IL or missing references)
						//IL_0156: Unknown result type (might be due to invalid IL or missing references)
						//IL_023e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0243: Unknown result type (might be due to invalid IL or missing references)
						//IL_0391: Unknown result type (might be due to invalid IL or missing references)
						//IL_049e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0196: Unknown result type (might be due to invalid IL or missing references)
						//IL_019b: Unknown result type (might be due to invalid IL or missing references)
						//IL_0258: Unknown result type (might be due to invalid IL or missing references)
						//IL_025a: Unknown result type (might be due to invalid IL or missing references)
						//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
						//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
						//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
						//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
						//IL_01af: Unknown result type (might be due to invalid IL or missing references)
						//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
						//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
						//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
						//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
						//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						<>c__DisplayClass16_0 <>c__DisplayClass16_ = <>4__this;
						try
						{
							if ((uint)num > 3u)
							{
								<rx_listener>5__2 = <>c__DisplayClass16_.<>4__this.Device.ReusableSubscriptionPool.Get();
							}
							try
							{
								TaskAwaiter val;
								TaskAwaiter<System.Threading.Tasks.Task> val2;
								switch (num)
								{
								default:
									<>8__1 = new <>c__DisplayClass16_1();
									<>8__1.CS$<>8__locals1 = <>c__DisplayClass16_;
									<>8__1.tcs = new TaskCompletionSource<RESPONSE>();
									<dest>5__3 = new BusEndpoint(<>c__DisplayClass16_.<>4__this.Device.Adapter, <>c__DisplayClass16_.client);
									<>c__DisplayClass16_.<>4__this.LastRxTimer.Reset();
									<>8__1.message_sent = false;
									<>8__1.sequence = 0;
									<>8__1.bytes_written = 0u;
									<>8__1.bulkXferCrc = 0u;
									<rx_listener>5__2.SetDelegate(<>c__DisplayClass16_.<>4__this.Device, delegate(LocalDeviceRxEvent rx)
									{
										if (<>8__1.CS$<>8__locals1.<>4__this.State != STATE.BULK_XFER)
										{
											return true;
										}
										if (!<>8__1.message_sent)
										{
											return false;
										}
										if (rx.SourceAddress != <>8__1.CS$<>8__locals1.<>4__this.Client)
										{
											return false;
										}
										switch (rx.MessageType)
										{
										case 128:
											if (rx.MessageData == 37)
											{
												if (rx.Length != 8)
												{
													<>8__1.tcs.SetResult(RESPONSE.BAD_REQUEST);
												}
												else if (CommExtensions.GetUINT16((IByteList)(object)rx, 0) != BLOCK_ID.op_Implicit(<>8__1.CS$<>8__locals1.<>4__this.Block.ID) || CommExtensions.GetUINT16((IByteList)(object)rx, 2) != (ushort)<>8__1.CS$<>8__locals1.offset)
												{
													<>8__1.tcs.SetResult(RESPONSE.VALUE_OUT_OF_RANGE);
												}
												else
												{
													<>8__1.bulkXferCrc = CommExtensions.GetUINT32((IByteList)(object)rx, 4);
													<>8__1.tcs.SetResult(RESPONSE.SUCCESS);
												}
												return true;
											}
											break;
										case 159:
											if (rx.MessageData == <>8__1.sequence)
											{
												<>8__1.CS$<>8__locals1.<>4__this.LastRxTimer.Reset();
												for (int i = 0; i < rx.Length; i++)
												{
													if (<>8__1.bytes_written < <>8__1.CS$<>8__locals1.accepted_bulk_xfer_size)
													{
														<>8__1.CS$<>8__locals1.<>4__this.WriteBuf[<>8__1.CS$<>8__locals1.offset + <>8__1.bytes_written] = rx[i];
														<>8__1.bytes_written++;
													}
												}
												<>8__1.sequence++;
											}
											break;
										}
										return false;
									});
									<retry>5__5 = 0;
									goto IL_00fa;
								case 0:
									val = <>u__1;
									<>u__1 = default(TaskAwaiter);
									num = (<>1__state = -1);
									goto IL_01e4;
								case 1:
									val2 = <>u__2;
									<>u__2 = default(TaskAwaiter<System.Threading.Tasks.Task>);
									num = (<>1__state = -1);
									goto IL_028f;
								case 2:
									val = <>u__1;
									<>u__1 = default(TaskAwaiter);
									num = (<>1__state = -1);
									goto IL_03f6;
								case 3:
									{
										val = <>u__1;
										<>u__1 = default(TaskAwaiter);
										num = (<>1__state = -1);
										goto IL_0525;
									}
									IL_00fa:
									if (!<>c__DisplayClass16_.<>4__this.Device.Transmit29((byte)129, 36, <dest>5__3, PAYLOAD.FromArgs(new object[3]
									{
										BLOCK_ID.op_Implicit(<>c__DisplayClass16_.<>4__this.Block.ID),
										<>c__DisplayClass16_.offset,
										<>c__DisplayClass16_.accepted_bulk_xfer_size
									})))
									{
										if (!<>c__DisplayClass16_.<>4__this.Device.IsOnline || ++<retry>5__5 > 5)
										{
											break;
										}
										val = System.Threading.Tasks.Task.Delay(5).GetAwaiter();
										if (!((TaskAwaiter)(ref val)).IsCompleted)
										{
											num = (<>1__state = 0);
											<>u__1 = val;
											((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<BeginBulkTransfer>b__0>d>(ref val, ref this);
											return;
										}
										goto IL_01e4;
									}
									<>8__1.message_sent = true;
									goto IL_0297;
									IL_03f6:
									((TaskAwaiter)(ref val)).GetResult();
									<retry>5__5++;
									goto IL_040d;
									IL_028f:
									val2.GetResult();
									goto IL_0297;
									IL_0525:
									((TaskAwaiter)(ref val)).GetResult();
									<retry>5__5++;
									goto IL_053c;
									IL_040d:
									if (<retry>5__5 >= 5 || !<>c__DisplayClass16_.<>4__this.Device.IsOnline || <>c__DisplayClass16_.<>4__this.Device.Transmit29((byte)129, 37, <dest>5__3, PAYLOAD.FromArgs(new object[3]
									{
										BLOCK_ID.op_Implicit(<>c__DisplayClass16_.<>4__this.Block.ID),
										(ushort)<>c__DisplayClass16_.offset,
										(byte)<response>5__4
									})))
									{
										break;
									}
									val = System.Threading.Tasks.Task.Delay(5).GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 2);
										<>u__1 = val;
										((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<BeginBulkTransfer>b__0>d>(ref val, ref this);
										return;
									}
									goto IL_03f6;
									IL_01e4:
									((TaskAwaiter)(ref val)).GetResult();
									goto IL_00fa;
									IL_053c:
									if (<retry>5__5 < 5 && <>c__DisplayClass16_.<>4__this.Device.IsOnline)
									{
										if (<>c__DisplayClass16_.<>4__this.Device.Transmit29((byte)129, 37, <dest>5__3, PAYLOAD.FromArgs(new object[3]
										{
											BLOCK_ID.op_Implicit(<>c__DisplayClass16_.<>4__this.Block.ID),
											(ushort)<>c__DisplayClass16_.offset,
											<>8__1.bulkXferCrc
										})))
										{
											<>c__DisplayClass16_.<>4__this.BlockOffset = <>c__DisplayClass16_.<>4__this.BlockOffset + <>8__1.bytes_written;
											break;
										}
										val = System.Threading.Tasks.Task.Delay(5).GetAwaiter();
										if (!((TaskAwaiter)(ref val)).IsCompleted)
										{
											num = (<>1__state = 3);
											<>u__1 = val;
											((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<BeginBulkTransfer>b__0>d>(ref val, ref this);
											return;
										}
										goto IL_0525;
									}
									break;
									IL_0297:
									if (!((System.Threading.Tasks.Task)<>8__1.tcs.Task).IsCompleted)
									{
										if (!<>c__DisplayClass16_.<>4__this.Device.IsOnline)
										{
											break;
										}
										val2 = System.Threading.Tasks.Task.WhenAny((System.Threading.Tasks.Task)<>8__1.tcs.Task, System.Threading.Tasks.Task.Delay(10000 + <>c__DisplayClass16_.accepted_bulk_xfer_size)).GetAwaiter();
										if (!val2.IsCompleted)
										{
											num = (<>1__state = 1);
											<>u__2 = val2;
											((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<System.Threading.Tasks.Task>, <<BeginBulkTransfer>b__0>d>(ref val2, ref this);
											return;
										}
										goto IL_028f;
									}
									<response>5__4 = <>8__1.tcs.Task.Result;
									if (<response>5__4 == RESPONSE.SUCCESS && (<>8__1.bytes_written != <>c__DisplayClass16_.accepted_bulk_xfer_size || <>8__1.bulkXferCrc != CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyList<byte>)<>c__DisplayClass16_.<>4__this.WriteBuf, (int)<>8__1.bytes_written, <>c__DisplayClass16_.offset)))
									{
										<response>5__4 = RESPONSE.CRC_INVALID;
									}
									if (<response>5__4 != RESPONSE.SUCCESS)
									{
										<retry>5__5 = 0;
										goto IL_040d;
									}
									<retry>5__5 = 0;
									goto IL_053c;
								}
							}
							catch (TimeoutException)
							{
							}
							catch (OperationCanceledException)
							{
							}
							finally
							{
								if (num < 0)
								{
									ReusableSubscription reusableSubscription = <rx_listener>5__2;
									if (reusableSubscription != null)
									{
										((Object)reusableSubscription).ReturnToPool();
									}
									<>c__DisplayClass16_.<>4__this.State = STATE.WAITING_FOR_BULK_XFER;
								}
							}
						}
						catch (System.Exception exception)
						{
							<>1__state = -2;
							<rx_listener>5__2 = null;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
							return;
						}
						<>1__state = -2;
						<rx_listener>5__2 = null;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
					}

					[DebuggerHidden]
					private void SetStateMachine(IAsyncStateMachine stateMachine)
					{
						((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
					}
				}

				public BlockWriter <>4__this;

				public ADDRESS client;

				public uint offset;

				public ushort accepted_bulk_xfer_size;

				[AsyncStateMachine(typeof(<<BeginBulkTransfer>b__0>d))]
				internal System.Threading.Tasks.Task? <BeginBulkTransfer>b__0()
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					<<BeginBulkTransfer>b__0>d <<BeginBulkTransfer>b__0>d = default(<<BeginBulkTransfer>b__0>d);
					<<BeginBulkTransfer>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<BeginBulkTransfer>b__0>d.<>4__this = this;
					<<BeginBulkTransfer>b__0>d.<>1__state = -1;
					((AsyncTaskMethodBuilder)(ref <<BeginBulkTransfer>b__0>d.<>t__builder)).Start<<<BeginBulkTransfer>b__0>d>(ref <<BeginBulkTransfer>b__0>d);
					return ((AsyncTaskMethodBuilder)(ref <<BeginBulkTransfer>b__0>d.<>t__builder)).Task;
				}
			}

			[CompilerGenerated]
			private sealed class <>c__DisplayClass16_1
			{
				public bool message_sent;

				public TaskCompletionSource<RESPONSE> tcs;

				public uint bulkXferCrc;

				public byte sequence;

				public uint bytes_written;

				public <>c__DisplayClass16_0 CS$<>8__locals1;

				internal bool <BeginBulkTransfer>b__1(LocalDeviceRxEvent rx)
				{
					if (CS$<>8__locals1.<>4__this.State != STATE.BULK_XFER)
					{
						return true;
					}
					if (!message_sent)
					{
						return false;
					}
					if (rx.SourceAddress != CS$<>8__locals1.<>4__this.Client)
					{
						return false;
					}
					switch (rx.MessageType)
					{
					case 128:
						if (rx.MessageData == 37)
						{
							if (rx.Length != 8)
							{
								tcs.SetResult(RESPONSE.BAD_REQUEST);
							}
							else if (CommExtensions.GetUINT16((IByteList)(object)rx, 0) != BLOCK_ID.op_Implicit(CS$<>8__locals1.<>4__this.Block.ID) || CommExtensions.GetUINT16((IByteList)(object)rx, 2) != (ushort)CS$<>8__locals1.offset)
							{
								tcs.SetResult(RESPONSE.VALUE_OUT_OF_RANGE);
							}
							else
							{
								bulkXferCrc = CommExtensions.GetUINT32((IByteList)(object)rx, 4);
								tcs.SetResult(RESPONSE.SUCCESS);
							}
							return true;
						}
						break;
					case 159:
					{
						if (rx.MessageData != sequence)
						{
							break;
						}
						CS$<>8__locals1.<>4__this.LastRxTimer.Reset();
						for (int i = 0; i < rx.Length; i++)
						{
							if (bytes_written < CS$<>8__locals1.accepted_bulk_xfer_size)
							{
								CS$<>8__locals1.<>4__this.WriteBuf[CS$<>8__locals1.offset + bytes_written] = rx[i];
								bytes_written++;
							}
						}
						sequence++;
						break;
					}
					}
					return false;
				}
			}

			private const byte BULK_XFER_DELAY_MS = 0;

			private readonly LocalDevice Device;

			private readonly LocalBlock Block;

			private Timer LastRxTimer = new Timer(true);

			private STATE State;

			private ADDRESS Client = ADDRESS.BROADCAST;

			private uint BlockOffset;

			private byte[] WriteBuf;

			private bool IsBusy => State != STATE.IDLE;

			public BlockWriter(LocalBlock block)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Expected O, but got Unknown
				Device = block.Device;
				Block = block;
			}

			private void Abort()
			{
				State = STATE.IDLE;
				Client = ADDRESS.BROADCAST;
				WriteBuf = null;
			}

			private void SanityCheck()
			{
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				if (!IsBusy)
				{
					return;
				}
				if (!Block.IsWritable())
				{
					Abort();
					return;
				}
				TimeSpan elapsedTime = LastRxTimer.ElapsedTime;
				if (((TimeSpan)(ref elapsedTime)).TotalSeconds > 60.0)
				{
					Abort();
					return;
				}
				if (WriteBuf == null)
				{
					Abort();
					return;
				}
				if (!Client.IsValidDeviceAddress)
				{
					Abort();
					return;
				}
				if (Block.WriteSessionID != SESSION_ID.UNKNOWN && Device.GetLocalSessionClientAddress(Block.WriteSessionID) != Client)
				{
					Abort();
				}
				if (BlockOffset > WriteBuf.Length)
				{
					Abort();
				}
			}

			public RESPONSE BeginWrite(ADDRESS client, uint requested_size, out uint accepted_size, out byte bulk_xfer_delay_ms)
			{
				accepted_size = 0u;
				bulk_xfer_delay_ms = 0;
				SanityCheck();
				if (!Block.IsWritable())
				{
					return RESPONSE.READ_ONLY;
				}
				if (IsBusy)
				{
					return RESPONSE.BUSY;
				}
				if (requested_size > Block.Capacity)
				{
					return RESPONSE.VALUE_TOO_LARGE;
				}
				if (requested_size > 2147483647)
				{
					return RESPONSE.VALUE_TOO_LARGE;
				}
				if (Block.WriteSessionID != SESSION_ID.UNKNOWN && Device.GetLocalSessionClientAddress(Block.WriteSessionID) != client)
				{
					return RESPONSE.SESSION_NOT_OPEN;
				}
				LastRxTimer.Reset();
				Client = client;
				accepted_size = requested_size;
				BlockOffset = 0u;
				WriteBuf = new byte[accepted_size];
				State = STATE.WAITING_FOR_BULK_XFER;
				return RESPONSE.SUCCESS;
			}

			public RESPONSE BeginBulkTransfer(ADDRESS client, uint offset, ushort requested_bulk_xfer_size)
			{
				<>c__DisplayClass16_0 CS$<>8__locals12 = new <>c__DisplayClass16_0();
				CS$<>8__locals12.<>4__this = this;
				CS$<>8__locals12.client = client;
				CS$<>8__locals12.offset = offset;
				SanityCheck();
				if (IsBusy && Client != CS$<>8__locals12.client)
				{
					return RESPONSE.BUSY;
				}
				if (State != STATE.WAITING_FOR_BULK_XFER)
				{
					return RESPONSE.CONDITIONS_NOT_CORRECT;
				}
				if (BlockOffset != CS$<>8__locals12.offset)
				{
					return RESPONSE.VALUE_OUT_OF_RANGE;
				}
				if (CS$<>8__locals12.offset > WriteBuf.Length)
				{
					return RESPONSE.VALUE_TOO_LARGE;
				}
				ulong num = (uint)WriteBuf.Length - CS$<>8__locals12.offset;
				CS$<>8__locals12.accepted_bulk_xfer_size = requested_bulk_xfer_size;
				if (CS$<>8__locals12.accepted_bulk_xfer_size > num)
				{
					CS$<>8__locals12.accepted_bulk_xfer_size = (ushort)num;
				}
				if (CS$<>8__locals12.accepted_bulk_xfer_size <= 0)
				{
					return RESPONSE.VALUE_OUT_OF_RANGE;
				}
				State = STATE.BULK_XFER;
				System.Threading.Tasks.Task.Run((Func<System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(<>c__DisplayClass16_0.<<BeginBulkTransfer>b__0>d))] () =>
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					<>c__DisplayClass16_0.<<BeginBulkTransfer>b__0>d <<BeginBulkTransfer>b__0>d = default(<>c__DisplayClass16_0.<<BeginBulkTransfer>b__0>d);
					<<BeginBulkTransfer>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<BeginBulkTransfer>b__0>d.<>4__this = CS$<>8__locals12;
					<<BeginBulkTransfer>b__0>d.<>1__state = -1;
					((AsyncTaskMethodBuilder)(ref <<BeginBulkTransfer>b__0>d.<>t__builder)).Start<<>c__DisplayClass16_0.<<BeginBulkTransfer>b__0>d>(ref <<BeginBulkTransfer>b__0>d);
					return ((AsyncTaskMethodBuilder)(ref <<BeginBulkTransfer>b__0>d.<>t__builder)).Task;
				}));
				return RESPONSE.SUCCESS;
			}

			public RESPONSE EndBlockWrite(ADDRESS client, uint crc)
			{
				SanityCheck();
				if (IsBusy && Client != client)
				{
					return RESPONSE.BUSY;
				}
				try
				{
					if (State != STATE.WAITING_FOR_BULK_XFER)
					{
						return RESPONSE.CONDITIONS_NOT_CORRECT;
					}
					if (BlockOffset != WriteBuf.Length || crc != CRC32_LE.Calculate((System.Collections.Generic.IReadOnlyCollection<byte>)(object)WriteBuf))
					{
						return RESPONSE.CRC_INVALID;
					}
					return (!Block.WriteData(WriteBuf)) ? RESPONSE.FAILED : RESPONSE.SUCCESS;
				}
				finally
				{
					Abort();
				}
			}
		}

		[DefaultMember("Item")]
		private class BlockServer
		{
			private class BusEndpoint : IBusEndpoint
			{
				[field: CompilerGenerated]
				public IAdapter Adapter
				{
					[CompilerGenerated]
					get;
					[CompilerGenerated]
					private set;
				}

				[field: CompilerGenerated]
				public ADDRESS Address
				{
					[CompilerGenerated]
					get;
					[CompilerGenerated]
					private set;
				}

				public bool IsOnline => true;

				public BusEndpoint(IAdapter adapter, ADDRESS address)
				{
					Adapter = adapter;
					Address = address;
				}
			}

			[CompilerGenerated]
			private sealed class <>c__DisplayClass10_0
			{
				[StructLayout((LayoutKind)3)]
				private struct <<Request22ReadBlockData>b__0>d : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder <>t__builder;

					public <>c__DisplayClass10_0 <>4__this;

					private BusEndpoint <dest>5__2;

					private PAYLOAD <payload>5__3;

					private byte <sequence>5__4;

					private uint <crc>5__5;

					private int <retry>5__6;

					private TaskAwaiter <>u__1;

					private int <bytes_left>5__7;

					private int <retry>5__8;

					private void MoveNext()
					{
						//IL_0112: Unknown result type (might be due to invalid IL or missing references)
						//IL_0117: Unknown result type (might be due to invalid IL or missing references)
						//IL_011e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0296: Unknown result type (might be due to invalid IL or missing references)
						//IL_029b: Unknown result type (might be due to invalid IL or missing references)
						//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
						//IL_0303: Unknown result type (might be due to invalid IL or missing references)
						//IL_0308: Unknown result type (might be due to invalid IL or missing references)
						//IL_030f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0466: Unknown result type (might be due to invalid IL or missing references)
						//IL_046b: Unknown result type (might be due to invalid IL or missing references)
						//IL_0472: Unknown result type (might be due to invalid IL or missing references)
						//IL_009f: Unknown result type (might be due to invalid IL or missing references)
						//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
						//IL_013f: Unknown result type (might be due to invalid IL or missing references)
						//IL_00df: Unknown result type (might be due to invalid IL or missing references)
						//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
						//IL_0436: Unknown result type (might be due to invalid IL or missing references)
						//IL_043b: Unknown result type (might be due to invalid IL or missing references)
						//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
						//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
						//IL_0223: Unknown result type (might be due to invalid IL or missing references)
						//IL_044f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0450: Unknown result type (might be due to invalid IL or missing references)
						//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
						//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
						//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
						//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
						//IL_0263: Unknown result type (might be due to invalid IL or missing references)
						//IL_0268: Unknown result type (might be due to invalid IL or missing references)
						//IL_027c: Unknown result type (might be due to invalid IL or missing references)
						//IL_027d: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						<>c__DisplayClass10_0 <>c__DisplayClass10_ = <>4__this;
						try
						{
							TaskAwaiter val;
							switch (num)
							{
							default:
								<dest>5__2 = new BusEndpoint(<>c__DisplayClass10_.<>4__this.Device.Adapter, <>c__DisplayClass10_.dest_addr);
								<retry>5__6 = 0;
								goto IL_004c;
							case 0:
								val = <>u__1;
								<>u__1 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_012d;
							case 1:
								val = <>u__1;
								<>u__1 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_02b1;
							case 2:
								val = <>u__1;
								<>u__1 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_031e;
							case 3:
								{
									val = <>u__1;
									<>u__1 = default(TaskAwaiter);
									num = (<>1__state = -1);
									goto IL_0481;
								}
								IL_004c:
								if (!<>c__DisplayClass10_.<>4__this.Device.Transmit29((byte)129, 34, <dest>5__2, PAYLOAD.FromArgs(new object[3]
								{
									BLOCK_ID.op_Implicit(<>c__DisplayClass10_.id),
									<>c__DisplayClass10_.start_offset,
									(ushort)<>c__DisplayClass10_.bytes_to_send
								})))
								{
									if (!<>c__DisplayClass10_.<>4__this.Device.IsOnline || ++<retry>5__6 > 5)
									{
										break;
									}
									val = System.Threading.Tasks.Task.Delay(5).GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<Request22ReadBlockData>b__0>d>(ref val, ref this);
										return;
									}
									goto IL_012d;
								}
								<payload>5__3 = default(PAYLOAD);
								<sequence>5__4 = 0;
								<retry>5__6 = (int)<>c__DisplayClass10_.start_offset;
								<bytes_left>5__7 = (int)<>c__DisplayClass10_.bytes_to_send;
								<retry>5__8 = 0;
								goto IL_036f;
								IL_0325:
								<retry>5__8 = 0;
								<sequence>5__4++;
								<retry>5__6 += ((PAYLOAD)(ref <payload>5__3)).Length;
								<bytes_left>5__7 -= ((PAYLOAD)(ref <payload>5__3)).Length;
								goto IL_036f;
								IL_02b1:
								((TaskAwaiter)(ref val)).GetResult();
								goto IL_036f;
								IL_0481:
								((TaskAwaiter)(ref val)).GetResult();
								goto IL_03a4;
								IL_031e:
								((TaskAwaiter)(ref val)).GetResult();
								goto IL_0325;
								IL_012d:
								((TaskAwaiter)(ref val)).GetResult();
								goto IL_004c;
								IL_03a4:
								if (<>c__DisplayClass10_.<>4__this.Device.Transmit29((byte)129, 37, <dest>5__2, PAYLOAD.FromArgs(new object[3]
								{
									BLOCK_ID.op_Implicit(<>c__DisplayClass10_.id),
									(ushort)<>c__DisplayClass10_.start_offset,
									<crc>5__5
								})) || !<>c__DisplayClass10_.<>4__this.Device.IsOnline || ++<retry>5__8 > 5)
								{
									break;
								}
								val = System.Threading.Tasks.Task.Delay(5).GetAwaiter();
								if (!((TaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 3);
									<>u__1 = val;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<Request22ReadBlockData>b__0>d>(ref val, ref this);
									return;
								}
								goto IL_0481;
								IL_036f:
								if (<bytes_left>5__7 > 0)
								{
									if (<>c__DisplayClass10_.block.ReadSessionID != SESSION_ID.UNKNOWN && <>c__DisplayClass10_.<>4__this.Device.GetLocalSessionClientAddress(<>c__DisplayClass10_.block.ReadSessionID) != <>c__DisplayClass10_.dest_addr)
									{
										break;
									}
									((PAYLOAD)(ref <payload>5__3)).Length = Math.Min(<bytes_left>5__7, 8);
									for (int i = 0; i < ((PAYLOAD)(ref <payload>5__3)).Length; i++)
									{
										((PAYLOAD)(ref <payload>5__3))[i] = <>c__DisplayClass10_.block.Data[<retry>5__6 + i];
									}
									if (!<>c__DisplayClass10_.<>4__this.Device.Transmit29((byte)159, <sequence>5__4, <dest>5__2, <payload>5__3))
									{
										if (!<>c__DisplayClass10_.<>4__this.Device.IsOnline || ++<retry>5__8 > 5)
										{
											break;
										}
										val = System.Threading.Tasks.Task.Delay(5).GetAwaiter();
										if (!((TaskAwaiter)(ref val)).IsCompleted)
										{
											num = (<>1__state = 1);
											<>u__1 = val;
											((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<Request22ReadBlockData>b__0>d>(ref val, ref this);
											return;
										}
										goto IL_02b1;
									}
									if (<>c__DisplayClass10_.xfer_delay_ms != 0)
									{
										val = System.Threading.Tasks.Task.Delay(<>c__DisplayClass10_.xfer_delay_ms).GetAwaiter();
										if (!((TaskAwaiter)(ref val)).IsCompleted)
										{
											num = (<>1__state = 2);
											<>u__1 = val;
											((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<Request22ReadBlockData>b__0>d>(ref val, ref this);
											return;
										}
										goto IL_031e;
									}
									goto IL_0325;
								}
								<crc>5__5 = CRC32_LE.Calculate(<>c__DisplayClass10_.block.Data, (int)<>c__DisplayClass10_.bytes_to_send, <>c__DisplayClass10_.start_offset);
								<retry>5__8 = 0;
								goto IL_03a4;
							}
						}
						catch (System.Exception exception)
						{
							<>1__state = -2;
							<dest>5__2 = null;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
							return;
						}
						<>1__state = -2;
						<dest>5__2 = null;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
					}

					[DebuggerHidden]
					private void SetStateMachine(IAsyncStateMachine stateMachine)
					{
						((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
					}
				}

				public BlockServer <>4__this;

				public ADDRESS dest_addr;

				public BLOCK_ID id;

				public uint start_offset;

				public uint bytes_to_send;

				public LocalBlock block;

				public int xfer_delay_ms;

				[AsyncStateMachine(typeof(<<Request22ReadBlockData>b__0>d))]
				internal System.Threading.Tasks.Task? <Request22ReadBlockData>b__0()
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					<<Request22ReadBlockData>b__0>d <<Request22ReadBlockData>b__0>d = default(<<Request22ReadBlockData>b__0>d);
					<<Request22ReadBlockData>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<Request22ReadBlockData>b__0>d.<>4__this = this;
					<<Request22ReadBlockData>b__0>d.<>1__state = -1;
					((AsyncTaskMethodBuilder)(ref <<Request22ReadBlockData>b__0>d.<>t__builder)).Start<<<Request22ReadBlockData>b__0>d>(ref <<Request22ReadBlockData>b__0>d);
					return ((AsyncTaskMethodBuilder)(ref <<Request22ReadBlockData>b__0>d.<>t__builder)).Task;
				}
			}

			private readonly LocalDevice Device;

			private Dictionary<BLOCK_ID, LocalBlock> Dict = new Dictionary<BLOCK_ID, LocalBlock>();

			private List<LocalBlock> List = new List<LocalBlock>();

			private LocalBlock this[BLOCK_ID id]
			{
				get
				{
					LocalBlock result = default(LocalBlock);
					Dict.TryGetValue(id, ref result);
					return result;
				}
			}

			public BlockServer(LocalDevice device)
			{
				Device = device;
				Device.mRequestServer.AddRequestHandler((byte)32, Request20ReadBlockList);
				Device.mRequestServer.AddRequestHandler((byte)33, Request21ReadBlockProperties);
				Device.mRequestServer.AddRequestHandler((byte)34, Request22ReadBlockData);
				Device.mRequestServer.AddRequestHandler((byte)35, Request23BeginBlockWrite);
				Device.mRequestServer.AddRequestHandler((byte)36, Request24BeginBlockWriteBulkXfer);
				Device.mRequestServer.AddRequestHandler((byte)38, Request26EndBlockWrite);
			}

			public void Add(LocalBlock block)
			{
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				if (Dict.ContainsKey(block.ID))
				{
					throw new InvalidOperationException($"BLOCK_ID {block.ID} already exists within DeviceSimulator");
				}
				Dict.Add(block.ID, block);
				List.Add(block);
			}

			private PAYLOAD? Request20ReadBlockList(AdapterRxEvent rx)
			{
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Length != 2)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				int num = CommExtensions.GetUINT16((IByteList)(object)rx, 0);
				PAYLOAD val = PAYLOAD.FromArgs(new object[1] { (ushort)num });
				if (num == 0)
				{
					((PAYLOAD)(ref val)).Append((ushort)List.Count);
				}
				else
				{
					num = num * 3 - 1;
				}
				while (((PAYLOAD)(ref val)).Length < 8)
				{
					LocalBlock localBlock = null;
					if (num < List.Count)
					{
						localBlock = List[num++];
					}
					((PAYLOAD)(ref val)).Append(BLOCK_ID.op_Implicit(localBlock?.ID ?? BLOCK_ID.op_Implicit((ushort)0)));
				}
				return val;
			}

			private PAYLOAD? Request21ReadBlockProperties(AdapterRxEvent rx)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_015a: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Length != 3)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				BLOCK_ID val = BLOCK_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0));
				LocalBlock localBlock = this[val];
				if (localBlock == null)
				{
					return PAYLOAD.FromArgs(new object[3]
					{
						BLOCK_ID.op_Implicit(val),
						rx[2],
						(byte)4
					});
				}
				ulong num;
				switch (rx[2])
				{
				default:
					return PAYLOAD.FromArgs(new object[3]
					{
						BLOCK_ID.op_Implicit(val),
						rx[2],
						(byte)10
					});
				case 0:
					num = (ulong)localBlock.Flags;
					break;
				case 1:
					num = SESSION_ID.op_Implicit(localBlock.ReadSessionID);
					break;
				case 2:
					num = SESSION_ID.op_Implicit(localBlock.WriteSessionID);
					break;
				case 3:
					num = localBlock.Capacity;
					break;
				case 4:
					num = localBlock.Size;
					break;
				case 5:
					num = localBlock.CRC;
					break;
				case 6:
					num = localBlock.CRC;
					break;
				}
				return PAYLOAD.FromArgs(new object[4]
				{
					BLOCK_ID.op_Implicit(val),
					rx[2],
					(byte)(num >> 32),
					(uint)num
				});
			}

			private PAYLOAD? Request22ReadBlockData(AdapterRxEvent rx)
			{
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0104: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_0170: Unknown result type (might be due to invalid IL or missing references)
				//IL_0212: Unknown result type (might be due to invalid IL or missing references)
				<>c__DisplayClass10_0 CS$<>8__locals29 = new <>c__DisplayClass10_0();
				CS$<>8__locals29.<>4__this = this;
				if (rx.Length != 8)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				CS$<>8__locals29.dest_addr = rx.SourceAddress;
				CS$<>8__locals29.id = BLOCK_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0));
				CS$<>8__locals29.start_offset = CommExtensions.GetUINT32((IByteList)(object)rx, 2);
				uint num = (uint)(rx[6] + 1);
				CS$<>8__locals29.xfer_delay_ms = CommExtensions.GetUINT8((IByteList)(object)rx, 7);
				CS$<>8__locals29.block = this[CS$<>8__locals29.id];
				if (CS$<>8__locals29.block == null)
				{
					return PAYLOAD.FromArgs(new object[3]
					{
						BLOCK_ID.op_Implicit(CS$<>8__locals29.id),
						CS$<>8__locals29.start_offset,
						(byte)4
					});
				}
				if (!CS$<>8__locals29.block.IsReadable())
				{
					return PAYLOAD.FromArgs(new object[3]
					{
						BLOCK_ID.op_Implicit(CS$<>8__locals29.id),
						CS$<>8__locals29.start_offset,
						(byte)8
					});
				}
				if (CS$<>8__locals29.block.ReadSessionID != SESSION_ID.UNKNOWN && Device.GetLocalSessionClientAddress(CS$<>8__locals29.block.ReadSessionID) != CS$<>8__locals29.dest_addr)
				{
					return PAYLOAD.FromArgs(new object[3]
					{
						BLOCK_ID.op_Implicit(CS$<>8__locals29.id),
						CS$<>8__locals29.start_offset,
						(byte)14
					});
				}
				if (CS$<>8__locals29.start_offset >= CS$<>8__locals29.block.Size)
				{
					return PAYLOAD.FromArgs(new object[3]
					{
						BLOCK_ID.op_Implicit(CS$<>8__locals29.id),
						CS$<>8__locals29.start_offset,
						(byte)5
					});
				}
				ulong num2 = CS$<>8__locals29.block.Size - CS$<>8__locals29.start_offset;
				CS$<>8__locals29.bytes_to_send = num * 8;
				if (CS$<>8__locals29.bytes_to_send >= num2)
				{
					CS$<>8__locals29.bytes_to_send = (uint)num2;
				}
				if (CS$<>8__locals29.bytes_to_send == 0)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)21 });
				}
				System.Threading.Tasks.Task.Run((Func<System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(<>c__DisplayClass10_0.<<Request22ReadBlockData>b__0>d))] () =>
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					<>c__DisplayClass10_0.<<Request22ReadBlockData>b__0>d <<Request22ReadBlockData>b__0>d = default(<>c__DisplayClass10_0.<<Request22ReadBlockData>b__0>d);
					<<Request22ReadBlockData>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<Request22ReadBlockData>b__0>d.<>4__this = CS$<>8__locals29;
					<<Request22ReadBlockData>b__0>d.<>1__state = -1;
					((AsyncTaskMethodBuilder)(ref <<Request22ReadBlockData>b__0>d.<>t__builder)).Start<<>c__DisplayClass10_0.<<Request22ReadBlockData>b__0>d>(ref <<Request22ReadBlockData>b__0>d);
					return ((AsyncTaskMethodBuilder)(ref <<Request22ReadBlockData>b__0>d.<>t__builder)).Task;
				}));
				return null;
			}

			private PAYLOAD? Request23BeginBlockWrite(AdapterRxEvent rx)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0100: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Length != 6)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				BLOCK_ID val = BLOCK_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0));
				LocalBlock localBlock = this[val];
				if (localBlock == null)
				{
					return PAYLOAD.FromArgs(new object[2]
					{
						BLOCK_ID.op_Implicit(val),
						(byte)4
					});
				}
				BlockWriter blockWriter = localBlock.GetBlockWriter();
				if (blockWriter == null)
				{
					return PAYLOAD.FromArgs(new object[2]
					{
						BLOCK_ID.op_Implicit(val),
						(byte)7
					});
				}
				uint accepted_size;
				byte bulk_xfer_delay_ms;
				RESPONSE rESPONSE = blockWriter.BeginWrite(rx.SourceAddress, CommExtensions.GetUINT32((IByteList)(object)rx, 2), out accepted_size, out bulk_xfer_delay_ms);
				if (rESPONSE != RESPONSE.SUCCESS)
				{
					return PAYLOAD.FromArgs(new object[2]
					{
						BLOCK_ID.op_Implicit(val),
						(byte)rESPONSE
					});
				}
				return PAYLOAD.FromArgs(new object[3]
				{
					BLOCK_ID.op_Implicit(val),
					accepted_size,
					bulk_xfer_delay_ms
				});
			}

			private PAYLOAD? Request24BeginBlockWriteBulkXfer(AdapterRxEvent rx)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Length != 8)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				BLOCK_ID val = BLOCK_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0));
				LocalBlock localBlock = this[val];
				if (localBlock == null)
				{
					return PAYLOAD.FromArgs(new object[2]
					{
						BLOCK_ID.op_Implicit(val),
						(byte)4
					});
				}
				BlockWriter blockWriter = localBlock.GetBlockWriter();
				if (blockWriter == null)
				{
					return PAYLOAD.FromArgs(new object[2]
					{
						BLOCK_ID.op_Implicit(val),
						(byte)7
					});
				}
				RESPONSE rESPONSE = blockWriter.BeginBulkTransfer(rx.SourceAddress, CommExtensions.GetUINT32((IByteList)(object)rx, 2), CommExtensions.GetUINT16((IByteList)(object)rx, 6));
				if (rESPONSE != RESPONSE.SUCCESS)
				{
					return PAYLOAD.FromArgs(new object[2]
					{
						BLOCK_ID.op_Implicit(val),
						(byte)rESPONSE
					});
				}
				return null;
			}

			private PAYLOAD? Request26EndBlockWrite(AdapterRxEvent rx)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Length != 6)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				BLOCK_ID val = BLOCK_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0));
				LocalBlock localBlock = this[val];
				if (localBlock == null)
				{
					return PAYLOAD.FromArgs(new object[2]
					{
						BLOCK_ID.op_Implicit(val),
						(byte)4
					});
				}
				BlockWriter blockWriter = localBlock.GetBlockWriter();
				if (blockWriter == null)
				{
					return PAYLOAD.FromArgs(new object[2]
					{
						BLOCK_ID.op_Implicit(val),
						(byte)7
					});
				}
				RESPONSE rESPONSE = blockWriter.EndBlockWrite(rx.SourceAddress, CommExtensions.GetUINT32((IByteList)(object)rx, 2));
				_ = 21;
				return PAYLOAD.FromArgs(new object[2]
				{
					BLOCK_ID.op_Implicit(val),
					(byte)rESPONSE
				});
			}
		}

		private class MuteManager
		{
			private readonly LocalDevice Device;

			private bool mIsMuted;

			private Timer MuteTimer = new Timer(true);

			public bool IsMuted
			{
				get
				{
					return mIsMuted;
				}
				set
				{
					mIsMuted = value;
					Device.AddressClaim.Enabled = Device.EnableDevice && !IsMuted;
				}
			}

			public MuteManager(LocalDevice device)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Expected O, but got Unknown
				Device = device;
				Device.mRequestServer.AddRequestHandler((byte)1, Request01MuteDevice);
			}

			private PAYLOAD? Request01MuteDevice(AdapterRxEvent rx)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0077: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00de: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Count != 2)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				switch (rx[0])
				{
				default:
					return PAYLOAD.FromArgs(new object[1] { (byte)10 });
				case 0:
					IsMuted = false;
					return PAYLOAD.FromArgs(new object[2]
					{
						(byte)0,
						(byte)0
					});
				case 1:
					Mute(TimeSpan.FromSeconds((double)(int)rx[1]));
					if (IsMuted)
					{
						return PAYLOAD.FromArgs(new object[2]
						{
							(byte)1,
							rx[1]
						});
					}
					return PAYLOAD.FromArgs(new object[2]
					{
						(byte)0,
						(byte)0
					});
				case 2:
					if (IsMuted)
					{
						Mute(TimeSpan.FromSeconds((double)(int)rx[1]));
					}
					return null;
				}
			}

			private void Mute(TimeSpan time)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				if (time > TimeSpan.Zero)
				{
					MuteTimer.ElapsedTime = -time;
					IsMuted = true;
				}
			}

			public void BackgroundTask()
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				if (IsMuted && MuteTimer.ElapsedTime >= TimeSpan.Zero)
				{
					IsMuted = false;
				}
			}
		}

		private class AddressClaimManager : Disposable
		{
			private readonly LocalDevice Device;

			private readonly IAdapter Adapter;

			private readonly Timer ClaimTimer = new Timer(true);

			private readonly MAC TempMAC = new MAC();

			private ADDRESS AddressBeingClaimed = ADDRESS.INVALID;

			private ADDRESS mAddress = ADDRESS.INVALID;

			private int RetransmitAddressClaim;

			private bool mEnabled;

			public bool Enabled
			{
				get
				{
					return mEnabled;
				}
				set
				{
					if (mEnabled != value)
					{
						mEnabled = value;
						KillAddressClaim();
					}
				}
			}

			public ADDRESS Address
			{
				get
				{
					return mAddress;
				}
				private set
				{
					if (((Disposable)this).IsDisposed || ((IDisposable)Adapter).IsDisposed)
					{
						value = ADDRESS.INVALID;
					}
					ADDRESS aDDRESS = mAddress;
					if (aDDRESS != value)
					{
						mAddress = value;
						Device.OnAddressChanged(value, aDDRESS);
					}
				}
			}

			public AddressClaimManager(LocalDevice device)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Expected O, but got Unknown
				Device = device;
				Adapter = device.Adapter;
			}

			public override void Dispose(bool disposing)
			{
				if (disposing)
				{
					AddressBeingClaimed = ADDRESS.INVALID;
					Address = ADDRESS.INVALID;
				}
			}

			public void OnAdapterOpened(AdapterOpenedEvent message)
			{
				if (!((Disposable)this).IsDisposed && !((IDisposable)Adapter).IsDisposed)
				{
					KillAddressClaim();
				}
			}

			public void OnAdapterClosed(AdapterClosedEvent message)
			{
				if (!((Disposable)this).IsDisposed && !((IDisposable)Adapter).IsDisposed)
				{
					KillAddressClaim();
				}
			}

			public void OnAdapterRx(AdapterRxEvent rx)
			{
				if (((Disposable)this).IsDisposed || ((IDisposable)Adapter).IsDisposed || !Enabled || (byte)rx.MessageType != 0 || rx.Count != 8 || !AddressBeingClaimed.IsValidDeviceAddress || (rx.SourceAddress != AddressBeingClaimed && (rx.SourceAddress != ADDRESS.BROADCAST || rx[0] != (byte)AddressBeingClaimed)) || !TempMAC.UnloadFromMessage(rx))
				{
					return;
				}
				switch (((PhysicalAddress)Device.MAC).CompareTo((PhysicalAddress)(object)TempMAC))
				{
				case 1:
					KillAddressClaim();
					break;
				case 0:
					if (rx[1] != (byte)Device.ProtocolVersion)
					{
						KillAddressClaim();
					}
					break;
				case -1:
					RetransmitAddressClaim = 3;
					break;
				}
			}

			private void KillAddressClaim()
			{
				AddressBeingClaimed = ADDRESS.INVALID;
				RetransmitAddressClaim = 0;
				Address = ADDRESS.INVALID;
			}

			public void BackgroundTask()
			{
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				if (((Disposable)this).IsDisposed || ((IDisposable)Adapter).IsDisposed)
				{
					return;
				}
				if (!((IAdapter)Adapter).IsConnected || !Enabled)
				{
					KillAddressClaim();
				}
				else if (!Address.IsValidDeviceAddress)
				{
					if (!AddressBeingClaimed.IsValidDeviceAddress)
					{
						ADDRESS aDDRESS = ChooseAddressToClaim();
						if (aDDRESS.IsValidDeviceAddress && TransmitAddressClaim(aDDRESS))
						{
							AddressBeingClaimed = aDDRESS;
							ClaimTimer.Reset();
						}
					}
					else if (ClaimTimer.ElapsedTime > ADDRESS_CLAIM_TIMEOUT)
					{
						Address = AddressBeingClaimed;
					}
				}
				else if (RetransmitAddressClaim > 0 && TransmitAddressClaim(Address))
				{
					RetransmitAddressClaim--;
				}
			}

			private ADDRESS ChooseAddressToClaim()
			{
				ADDRESS unusedDeviceAddress = Adapter.GetUnusedDeviceAddress();
				if (!unusedDeviceAddress.IsValidDeviceAddress)
				{
					return ADDRESS.INVALID;
				}
				System.Collections.Generic.IEnumerator<IDevice> enumerator = ((System.Collections.Generic.IEnumerable<IDevice>)Device.Product).GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						if ((enumerator.Current as LocalDevice)?.AddressClaim.AddressBeingClaimed == unusedDeviceAddress)
						{
							return ADDRESS.INVALID;
						}
					}
					return unusedDeviceAddress;
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
			}

			private bool TransmitAddressClaim(ADDRESS address)
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				if (!address.IsValidDeviceAddress || !Enabled)
				{
					return false;
				}
				return Device.Transmit(new CAN_ID((byte)0, ADDRESS.BROADCAST), PAYLOAD.FromArgs(new object[3]
				{
					(byte)address,
					(byte)Device.ProtocolVersion,
					(UInt48)Device.MAC
				}));
			}
		}

		private class InMotionLockoutManager
		{
			private static readonly TimeSpan ARMED_TIMEOUT = TimeSpan.FromSeconds(5.0);

			private static readonly TimeSpan CONTENTION_TIMEOUT = TimeSpan.FromSeconds(5.0);

			private static readonly TimeSpan ALL_CLEAR_TIME = TimeSpan.FromSeconds(2.2);

			private readonly LocalDevice Device;

			private Timer ArmedTimer = new Timer(true);

			private Timer AllClearTimer = new Timer(true);

			private Timer ContentionTimer = new Timer(true);

			private DeviceInMotionLockoutLevelChangedEvent InMotionLockoutEvent;

			private IN_MOTION_LOCKOUT_LEVEL mLockoutLevel = (byte)0;

			private IN_MOTION_LOCKOUT_LEVEL HighestLevelSeen = (byte)0;

			public IN_MOTION_LOCKOUT_LEVEL LockoutLevel
			{
				get
				{
					return mLockoutLevel;
				}
				private set
				{
					if (mLockoutLevel != value)
					{
						ArmedTimer.Stop();
						ContentionTimer.Stop();
						IN_MOTION_LOCKOUT_LEVEL prev = mLockoutLevel;
						mLockoutLevel = value;
						InMotionLockoutEvent.Publish(value, prev);
					}
				}
			}

			[field: CompilerGenerated]
			public IN_MOTION_LOCKOUT_LEVEL ProposedLevel
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			private bool IsArmed
			{
				get
				{
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					if (ArmedTimer.IsRunning)
					{
						return ArmedTimer.ElapsedTime <= ARMED_TIMEOUT;
					}
					return false;
				}
			}

			public bool IsInContention
			{
				get
				{
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					if (ContentionTimer.IsRunning)
					{
						return ContentionTimer.ElapsedTime <= CONTENTION_TIMEOUT;
					}
					return false;
				}
			}

			public InMotionLockoutManager(LocalDevice device)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Expected O, but got Unknown
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Expected O, but got Unknown
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Expected O, but got Unknown
				Device = device;
				ArmedTimer.Stop();
				ContentionTimer.Stop();
				InMotionLockoutEvent = new DeviceInMotionLockoutLevelChangedEvent(device);
				device.mRequestServer.AddRequestHandler((byte)2, OnRequest02InMotionLockout);
			}

			public void IncreaseLockoutLevel(IN_MOTION_LOCKOUT_LEVEL level)
			{
				if ((byte)LockoutLevel < (byte)level)
				{
					LockoutLevel = level;
				}
			}

			private PAYLOAD? OnRequest02InMotionLockout(AdapterRxEvent rx)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				//IL_012f: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Count != 1)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				if (rx.TargetAddress != ADDRESS.BROADCAST)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)6 });
				}
				switch (rx[0])
				{
				default:
					return PAYLOAD.FromArgs(new object[2]
					{
						rx[0],
						(byte)2
					});
				case 1:
				case 2:
				case 3:
					IncreaseLockoutLevel(rx[0]);
					return PAYLOAD.FromArgs(new object[2]
					{
						rx[0],
						(byte)0
					});
				case 85:
					if (!IsInContention)
					{
						ArmedTimer.Reset();
					}
					return PAYLOAD.FromArgs(new object[2]
					{
						rx[0],
						(byte)0
					});
				case 170:
					if (!IsArmed)
					{
						return PAYLOAD.FromArgs(new object[2]
						{
							rx[0],
							(byte)9
						});
					}
					ArmedTimer.Stop();
					if ((byte)LockoutLevel > 0)
					{
						ProposedLevel = Device.IsOkToClearInMotionLockout;
						if ((byte)ProposedLevel < (byte)LockoutLevel)
						{
							ContentionTimer.Reset();
						}
					}
					return PAYLOAD.FromArgs(new object[2]
					{
						rx[0],
						(byte)0
					});
				}
			}

			public RESPONSE ProcessLockoutRequest(byte cmd)
			{
				switch (cmd)
				{
				default:
					return RESPONSE.BAD_REQUEST;
				case 1:
				case 2:
				case 3:
					IncreaseLockoutLevel(cmd);
					return RESPONSE.SUCCESS;
				case 85:
					if (!IsInContention)
					{
						ArmedTimer.Reset();
					}
					return RESPONSE.SUCCESS;
				case 170:
					if (!IsArmed)
					{
						return RESPONSE.CONDITIONS_NOT_CORRECT;
					}
					ArmedTimer.Stop();
					if ((byte)LockoutLevel > 0)
					{
						ProposedLevel = Device.IsOkToClearInMotionLockout;
						if ((byte)ProposedLevel < (byte)LockoutLevel)
						{
							ContentionTimer.Reset();
						}
					}
					return RESPONSE.SUCCESS;
				}
			}

			public void OnAdapterRx(AdapterRxEvent rx)
			{
				if ((byte)rx.MessageType == 0 && rx.Count == 8 && rx.SourceAddress.IsValidDeviceAddress)
				{
					IN_MOTION_LOCKOUT_LEVEL inMotionLockoutLevel = new NETWORK_STATUS(rx[0], rx[1]).InMotionLockoutLevel;
					if ((byte)HighestLevelSeen < (byte)inMotionLockoutLevel)
					{
						HighestLevelSeen = inMotionLockoutLevel;
					}
					if ((byte)LockoutLevel < (byte)inMotionLockoutLevel)
					{
						LockoutLevel = inMotionLockoutLevel;
					}
					if ((byte)inMotionLockoutLevel > 0)
					{
						AllClearTimer.Reset();
					}
				}
			}

			public void BackgroundTask()
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_0081: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				if (ArmedTimer.IsRunning && ArmedTimer.ElapsedTime > ARMED_TIMEOUT)
				{
					ArmedTimer.Stop();
				}
				if (!ContentionTimer.IsRunning)
				{
					return;
				}
				TimeSpan elapsedTime = ContentionTimer.ElapsedTime;
				if (elapsedTime < CONTENTION_TIMEOUT)
				{
					if (elapsedTime < CONTENTION_TIMEOUT - ALL_CLEAR_TIME)
					{
						HighestLevelSeen = (byte)0;
					}
					if (AllClearTimer.ElapsedTime > ALL_CLEAR_TIME)
					{
						LockoutLevel = ProposedLevel;
						ContentionTimer.Stop();
					}
				}
				else
				{
					ContentionTimer.Stop();
					if ((byte)ProposedLevel > (byte)HighestLevelSeen)
					{
						LockoutLevel = ProposedLevel;
					}
					else
					{
						LockoutLevel = HighestLevelSeen;
					}
				}
			}
		}

		private class PidClient : IPidClient
		{
			[CompilerGenerated]
			private sealed class <>c__DisplayClass3_0
			{
				public int index;

				public Func<LocalDeviceRxEvent, RESPONSE?> <>9__1;

				internal RESPONSE? <ReadPidListAsync>b__1(LocalDeviceRxEvent rx)
				{
					if (rx.Length == 8 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == index)
					{
						return RESPONSE.SUCCESS;
					}
					return null;
				}
			}

			[CompilerGenerated]
			private sealed class <>c__DisplayClass4_0
			{
				public PID pid;

				internal RESPONSE? <ReadPidAsync>b__0(LocalDeviceRxEvent rx)
				{
					if (rx.Length >= 3)
					{
						ushort uINT = CommExtensions.GetUINT16((IByteList)(object)rx, 0);
						if (uINT == PID.op_Implicit(pid))
						{
							return RESPONSE.SUCCESS;
						}
						if (uINT == 0 && rx.Length == 5 && PID.op_Implicit(pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
						{
							return (RESPONSE)rx[4];
						}
						if (uINT == 0 && rx.Length == 7 && PID.op_Implicit(pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
						{
							return (RESPONSE)rx[4];
						}
					}
					return null;
				}
			}

			[CompilerGenerated]
			private sealed class <>c__DisplayClass5_0
			{
				public PID pid;

				internal RESPONSE? <ReadPidAsync>b__0(LocalDeviceRxEvent rx)
				{
					if (rx.Length >= 3)
					{
						ushort uINT = CommExtensions.GetUINT16((IByteList)(object)rx, 0);
						if (uINT == PID.op_Implicit(pid))
						{
							return RESPONSE.SUCCESS;
						}
						if (uINT == 0 && rx.Length == 5 && PID.op_Implicit(pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
						{
							return (RESPONSE)rx[4];
						}
						if (uINT == 0 && rx.Length == 7 && PID.op_Implicit(pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
						{
							return (RESPONSE)rx[4];
						}
					}
					return null;
				}
			}

			[CompilerGenerated]
			private sealed class <>c__DisplayClass7_0
			{
				public ISessionClient session;

				public PID pid;

				internal RESPONSE? <WritePidAsync>b__0(LocalDeviceRxEvent rx)
				{
					if (session != null)
					{
						session.TryOpenSession();
					}
					if (rx.Length >= 3)
					{
						ushort uINT = CommExtensions.GetUINT16((IByteList)(object)rx, 0);
						if (uINT == PID.op_Implicit(pid))
						{
							return RESPONSE.SUCCESS;
						}
						if (uINT == 0 && rx.Length == 5 && PID.op_Implicit(pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
						{
							return (RESPONSE)rx[4];
						}
						if (uINT == 0 && rx.Length == 7 && PID.op_Implicit(pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
						{
							return (RESPONSE)rx[4];
						}
					}
					return null;
				}
			}

			[StructLayout((LayoutKind)3)]
			[CompilerGenerated]
			private struct <ReadPidAsync>d__4 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<Tuple<RESPONSE?, UInt48?>> <>t__builder;

				public PID pid;

				public AsyncOperation operation;

				public PidClient <>4__this;

				public IDevice tgtDevice;

				private Tuple<RESPONSE, MessageBuffer> <result>5__2;

				private TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> <>u__1;

				private void MoveNext()
				{
					//IL_00da: Unknown result type (might be due to invalid IL or missing references)
					//IL_00df: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
					//IL_008f: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
					//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
					//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
					//IL_0147: Unknown result type (might be due to invalid IL or missing references)
					//IL_014c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0153: Unknown result type (might be due to invalid IL or missing references)
					//IL_0156: Unknown result type (might be due to invalid IL or missing references)
					//IL_015b: Unknown result type (might be due to invalid IL or missing references)
					//IL_015d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0171: Unknown result type (might be due to invalid IL or missing references)
					//IL_0176: Unknown result type (might be due to invalid IL or missing references)
					//IL_017b: Unknown result type (might be due to invalid IL or missing references)
					//IL_019d: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					PidClient pidClient = <>4__this;
					Tuple<RESPONSE?, UInt48?> result2;
					try
					{
						<>c__DisplayClass4_0 <>c__DisplayClass4_ = default(<>c__DisplayClass4_0);
						if (num != 0)
						{
							<>c__DisplayClass4_ = new <>c__DisplayClass4_0
							{
								pid = pid
							};
							operation.ReportProgress("Reading PID " + <>c__DisplayClass4_.pid.Value.ToString("X4") + "h from device...");
							<result>5__2 = null;
						}
						try
						{
							TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> val;
							if (num != 0)
							{
								val = pidClient.Device.TransmitRequestAsync(operation, tgtDevice, (byte)17, PAYLOAD.FromArgs(new object[1] { PID.op_Implicit(<>c__DisplayClass4_.pid) }), <>c__DisplayClass4_.<ReadPidAsync>b__0).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>, <ReadPidAsync>d__4>(ref val, ref this);
									return;
								}
							}
							else
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>);
								num = (<>1__state = -1);
							}
							Tuple<RESPONSE, MessageBuffer> result = val.GetResult();
							<result>5__2 = result;
							if (<result>5__2.Item1 != RESPONSE.SUCCESS || <result>5__2.Item2 == null)
							{
								result2 = Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)<result>5__2.Item1, (UInt48?)null);
							}
							else
							{
								UInt48 val2 = UInt48.op_Implicit((byte)0);
								for (int i = 2; i < ((MessageBuffer)<result>5__2.Item2).Length; i++)
								{
									val2 <<= 8;
									val2 += UInt48.op_Implicit(CommExtensions.GetUINT8((IByteList)(object)<result>5__2.Item2, i));
								}
								result2 = Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)RESPONSE.SUCCESS, (UInt48?)val2);
							}
						}
						finally
						{
							if (num < 0)
							{
								Tuple<RESPONSE, MessageBuffer> obj = <result>5__2;
								if (obj != null)
								{
									MessageBuffer item = obj.Item2;
									if (item != null)
									{
										((Object)item).ReturnToPool();
									}
								}
							}
						}
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						<result>5__2 = null;
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<result>5__2 = null;
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
			private struct <ReadPidAsync>d__5 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<Tuple<RESPONSE?, UInt48?>> <>t__builder;

				public PID pid;

				public AsyncOperation operation;

				public bool withadd;

				public ushort add;

				public PidClient <>4__this;

				public IDevice tgtDevice;

				private Tuple<RESPONSE, MessageBuffer> <result>5__2;

				private TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> <>u__1;

				private void MoveNext()
				{
					//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
					//IL_0080: Unknown result type (might be due to invalid IL or missing references)
					//IL_0085: Unknown result type (might be due to invalid IL or missing references)
					//IL_0115: Unknown result type (might be due to invalid IL or missing references)
					//IL_011a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0122: Unknown result type (might be due to invalid IL or missing references)
					//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
					//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
					//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
					//IL_0182: Unknown result type (might be due to invalid IL or missing references)
					//IL_0187: Unknown result type (might be due to invalid IL or missing references)
					//IL_018e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0191: Unknown result type (might be due to invalid IL or missing references)
					//IL_0196: Unknown result type (might be due to invalid IL or missing references)
					//IL_0198: Unknown result type (might be due to invalid IL or missing references)
					//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
					//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
					//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
					//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					PidClient pidClient = <>4__this;
					Tuple<RESPONSE?, UInt48?> result2;
					try
					{
						<>c__DisplayClass5_0 <>c__DisplayClass5_ = default(<>c__DisplayClass5_0);
						PAYLOAD payload = default(PAYLOAD);
						if (num != 0)
						{
							<>c__DisplayClass5_ = new <>c__DisplayClass5_0
							{
								pid = pid
							};
							operation.ReportProgress("Reading PID " + <>c__DisplayClass5_.pid.Value.ToString("X4") + "h from device...");
							payload = ((!withadd) ? PAYLOAD.FromArgs(new object[1] { PID.op_Implicit(<>c__DisplayClass5_.pid) }) : PAYLOAD.FromArgs(new object[1] { (uint)((PID.op_Implicit(<>c__DisplayClass5_.pid) << 16) | add) }));
							<result>5__2 = null;
						}
						try
						{
							TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> val;
							if (num != 0)
							{
								val = pidClient.Device.TransmitRequestAsync(operation, tgtDevice, (byte)17, payload, <>c__DisplayClass5_.<ReadPidAsync>b__0).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>, <ReadPidAsync>d__5>(ref val, ref this);
									return;
								}
							}
							else
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>);
								num = (<>1__state = -1);
							}
							Tuple<RESPONSE, MessageBuffer> result = val.GetResult();
							<result>5__2 = result;
							if (<result>5__2.Item1 != RESPONSE.SUCCESS || <result>5__2.Item2 == null)
							{
								result2 = Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)<result>5__2.Item1, (UInt48?)null);
							}
							else
							{
								UInt48 val2 = UInt48.op_Implicit((byte)0);
								for (int i = 2; i < ((MessageBuffer)<result>5__2.Item2).Length; i++)
								{
									val2 <<= 8;
									val2 += UInt48.op_Implicit(CommExtensions.GetUINT8((IByteList)(object)<result>5__2.Item2, i));
								}
								result2 = Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)RESPONSE.SUCCESS, (UInt48?)val2);
							}
						}
						finally
						{
							if (num < 0)
							{
								Tuple<RESPONSE, MessageBuffer> obj = <result>5__2;
								if (obj != null)
								{
									MessageBuffer item = obj.Item2;
									if (item != null)
									{
										((Object)item).ReturnToPool();
									}
								}
							}
						}
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						<result>5__2 = null;
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<result>5__2 = null;
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
			private struct <ReadPidAsync>d__6 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<Tuple<RESPONSE?, UInt48?>> <>t__builder;

				public PidInfo pidInfo;

				public PidClient <>4__this;

				public AsyncOperation operation;

				private TaskAwaiter<Tuple<RESPONSE?, UInt48?>> <>u__1;

				private void MoveNext()
				{
					//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
					//IL_013c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0141: Unknown result type (might be due to invalid IL or missing references)
					//IL_0149: Unknown result type (might be due to invalid IL or missing references)
					//IL_010a: Unknown result type (might be due to invalid IL or missing references)
					//IL_010f: Unknown result type (might be due to invalid IL or missing references)
					//IL_008a: Unknown result type (might be due to invalid IL or missing references)
					//IL_008f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0124: Unknown result type (might be due to invalid IL or missing references)
					//IL_0126: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					PidClient pidClient = <>4__this;
					Tuple<RESPONSE?, UInt48?> result;
					try
					{
						TaskAwaiter<Tuple<RESPONSE?, UInt48?>> val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Tuple<RESPONSE?, UInt48?>>);
							num = (<>1__state = -1);
							goto IL_00db;
						}
						if (num == 1)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<Tuple<RESPONSE?, UInt48?>>);
							num = (<>1__state = -1);
							goto IL_0158;
						}
						if (pidInfo.IsReadable)
						{
							if (pidInfo.IsWithAddress)
							{
								val = pidClient.ReadPidAsync(operation, pidInfo.Device, pidInfo.ID, pidInfo.IsWithAddress, pidInfo.PID_Address).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE?, UInt48?>>, <ReadPidAsync>d__6>(ref val, ref this);
									return;
								}
								goto IL_00db;
							}
							val = pidClient.ReadPidAsync(operation, pidInfo.Device, pidInfo.ID).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE?, UInt48?>>, <ReadPidAsync>d__6>(ref val, ref this);
								return;
							}
							goto IL_0158;
						}
						result = Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)RESPONSE.WRITE_ONLY, (UInt48?)null);
						goto end_IL_000e;
						IL_0158:
						result = val.GetResult();
						goto end_IL_000e;
						IL_00db:
						result = val.GetResult();
						end_IL_000e:;
					}
					catch (System.Exception exception)
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
			private struct <ReadPidListAsync>d__3 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<Tuple<RESPONSE?, PidList>> <>t__builder;

				public IDevice tgtDevice;

				public PidClient <>4__this;

				public AsyncOperation operation;

				private <>c__DisplayClass3_0 <>8__1;

				private ulong <signature>5__2;

				private List<PidInfo> <list>5__3;

				private int <total>5__4;

				private Tuple<RESPONSE, MessageBuffer> <result>5__5;

				private TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> <>u__1;

				private void MoveNext()
				{
					//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
					//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
					//IL_01da: Unknown result type (might be due to invalid IL or missing references)
					//IL_015e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0198: Unknown result type (might be due to invalid IL or missing references)
					//IL_019d: Unknown result type (might be due to invalid IL or missing references)
					//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
					//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					PidClient pidClient = <>4__this;
					Tuple<RESPONSE?, PidList> result;
					try
					{
						if (num == 0)
						{
							goto IL_0125;
						}
						<>8__1 = new <>c__DisplayClass3_0();
						PidList pidList = null;
						<signature>5__2 = tgtDevice.GetDeviceUniqueID();
						if (!pidClient.PidListCache.TryGetValue(<signature>5__2, ref pidList) || pidList.Device == tgtDevice)
						{
							<list>5__3 = new List<PidInfo>();
							<>8__1.index = 0;
							<total>5__4 = 0;
							operation.ReportProgress("Reading PID list from device...");
							goto IL_011d;
						}
						List<PidInfo> val = new List<PidInfo>();
						System.Collections.Generic.IEnumerator<PidInfo> enumerator = pidList.GetEnumerator();
						try
						{
							while (((System.Collections.IEnumerator)enumerator).MoveNext())
							{
								PidInfo current = enumerator.Current;
								val.Add(new PidInfo(tgtDevice, current.ID, current.Flags));
							}
						}
						finally
						{
							if (num < 0)
							{
								((System.IDisposable)enumerator)?.Dispose();
							}
						}
						pidList = new PidList(tgtDevice, (System.Collections.Generic.IEnumerable<PidInfo>)val);
						if (tgtDevice.IsOnline)
						{
							pidClient.PidListCache[<signature>5__2] = pidList;
						}
						result = Tuple.Create<RESPONSE?, PidList>((RESPONSE?)RESPONSE.SUCCESS, pidList);
						goto end_IL_000e;
						IL_011d:
						<result>5__5 = null;
						goto IL_0125;
						IL_0125:
						try
						{
							TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> val2;
							if (num != 0)
							{
								val2 = pidClient.Device.TransmitRequestAsync(operation, tgtDevice, (byte)16, PAYLOAD.FromArgs(new object[1] { (ushort)<>8__1.index }), (LocalDeviceRxEvent rx) => (rx.Length == 8 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == <>8__1.index) ? new RESPONSE?(RESPONSE.SUCCESS) : ((RESPONSE?)null)).GetAwaiter();
								if (!val2.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val2;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>, <ReadPidListAsync>d__3>(ref val2, ref this);
									return;
								}
							}
							else
							{
								val2 = <>u__1;
								<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>);
								num = (<>1__state = -1);
							}
							Tuple<RESPONSE, MessageBuffer> result2 = val2.GetResult();
							<result>5__5 = result2;
							if (<result>5__5.Item1 != RESPONSE.SUCCESS || <result>5__5.Item2 == null)
							{
								result = Tuple.Create<RESPONSE?, PidList>((RESPONSE?)<result>5__5.Item1, (PidList)null);
							}
							else
							{
								int num2 = <>8__1.index;
								<>8__1.index = num2 + 1;
								if (num2 == 0)
								{
									<total>5__4 = CommExtensions.GetUINT16((IByteList)(object)<result>5__5.Item2, 2);
								}
								else if (<list>5__3.Count < <total>5__4)
								{
									<list>5__3.Add(new PidInfo(tgtDevice, PID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)<result>5__5.Item2, 2)), CommExtensions.GetUINT8((IByteList)(object)<result>5__5.Item2, 4)));
								}
								if (<list>5__3.Count < <total>5__4)
								{
									<list>5__3.Add(new PidInfo(tgtDevice, PID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)<result>5__5.Item2, 5)), CommExtensions.GetUINT8((IByteList)(object)<result>5__5.Item2, 7)));
								}
								if (<list>5__3.Count < <total>5__4)
								{
									operation.ReportProgress(100f * (float)<list>5__3.Count / (float)<total>5__4, $"Read {<list>5__3.Count} of {<total>5__4} PIDs...");
									goto end_IL_0125;
								}
								<list>5__3.Sort((Comparison<PidInfo>)((PidInfo first, PidInfo second) => first.ID.Value.CompareTo(second.ID.Value)));
								pidList = new PidList(tgtDevice, (System.Collections.Generic.IEnumerable<PidInfo>)<list>5__3);
								pidClient.PidListCache[<signature>5__2] = pidList;
								operation.ReportProgress(100f, "Success!");
								result = Tuple.Create<RESPONSE?, PidList>((RESPONSE?)RESPONSE.SUCCESS, pidList);
							}
							goto end_IL_000e;
							end_IL_0125:;
						}
						finally
						{
							if (num < 0)
							{
								Tuple<RESPONSE, MessageBuffer> obj = <result>5__5;
								if (obj != null)
								{
									MessageBuffer item = obj.Item2;
									if (item != null)
									{
										((Object)item).ReturnToPool();
									}
								}
							}
						}
						<result>5__5 = null;
						goto IL_011d;
						end_IL_000e:;
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						<>8__1 = null;
						<list>5__3 = null;
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<>8__1 = null;
					<list>5__3 = null;
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
			private struct <WritePidAsync>d__7 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<Tuple<RESPONSE?, UInt48?>> <>t__builder;

				public ISessionClient session;

				public PID pid;

				public AsyncOperation operation;

				public UInt48 value;

				private <>c__DisplayClass7_0 <>8__1;

				public PidClient <>4__this;

				public IDevice tgtDevice;

				private Tuple<RESPONSE, MessageBuffer> <result>5__2;

				private TaskAwaiter <>u__1;

				private TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> <>u__2;

				private void MoveNext()
				{
					//IL_012b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0130: Unknown result type (might be due to invalid IL or missing references)
					//IL_0138: Unknown result type (might be due to invalid IL or missing references)
					//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
					//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
					//IL_0205: Unknown result type (might be due to invalid IL or missing references)
					//IL_019d: Unknown result type (might be due to invalid IL or missing references)
					//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
					//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
					//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
					//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
					//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
					//IL_01df: Unknown result type (might be due to invalid IL or missing references)
					//IL_0110: Unknown result type (might be due to invalid IL or missing references)
					//IL_0112: Unknown result type (might be due to invalid IL or missing references)
					//IL_0266: Unknown result type (might be due to invalid IL or missing references)
					//IL_026b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0277: Unknown result type (might be due to invalid IL or missing references)
					//IL_027d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0282: Unknown result type (might be due to invalid IL or missing references)
					//IL_0289: Unknown result type (might be due to invalid IL or missing references)
					//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
					//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
					//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
					//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					PidClient pidClient = <>4__this;
					Tuple<RESPONSE?, UInt48?> result2;
					try
					{
						if ((uint)num > 1u)
						{
							<>8__1 = new <>c__DisplayClass7_0();
							<>8__1.session = session;
							<>8__1.pid = pid;
							operation.ReportProgress($"Writing PID {<>8__1.pid.Value.ToString("X4")}h = {((object)System.Runtime.CompilerServices.Unsafe.As<UInt48, UInt48>(ref value)/*cast due to .constrained prefix*/).ToString()} to device...");
							<result>5__2 = null;
						}
						try
						{
							TaskAwaiter<Tuple<RESPONSE, MessageBuffer>> val;
							if (num != 0)
							{
								if (num != 1)
								{
									if (<>8__1.session != null)
									{
										goto IL_014e;
									}
									goto IL_0163;
								}
								val = <>u__2;
								<>u__2 = default(TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>);
								num = (<>1__state = -1);
								goto IL_0214;
							}
							TaskAwaiter val2 = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_0147;
							IL_0163:
							val = pidClient.Device.TransmitRequestAsync(operation, tgtDevice, (byte)17, PAYLOAD.FromArgs(new object[2]
							{
								PID.op_Implicit(<>8__1.pid),
								value
							}), delegate(LocalDeviceRxEvent rx)
							{
								if (<>8__1.session != null)
								{
									<>8__1.session.TryOpenSession();
								}
								if (rx.Length >= 3)
								{
									ushort uINT = CommExtensions.GetUINT16((IByteList)(object)rx, 0);
									if (uINT == PID.op_Implicit(<>8__1.pid))
									{
										return RESPONSE.SUCCESS;
									}
									if (uINT == 0 && rx.Length == 5 && PID.op_Implicit(<>8__1.pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
									{
										return (RESPONSE)rx[4];
									}
									if (uINT == 0 && rx.Length == 7 && PID.op_Implicit(<>8__1.pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
									{
										return (RESPONSE)rx[4];
									}
								}
								return (RESPONSE?)null;
							}).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__2 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, MessageBuffer>>, <WritePidAsync>d__7>(ref val, ref this);
								return;
							}
							goto IL_0214;
							IL_014e:
							if (!<>8__1.session.IsOpen)
							{
								<>8__1.session.TryOpenSession();
								val2 = System.Threading.Tasks.Task.Delay(15).GetAwaiter();
								if (!((TaskAwaiter)(ref val2)).IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val2;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <WritePidAsync>d__7>(ref val2, ref this);
									return;
								}
								goto IL_0147;
							}
							goto IL_0163;
							IL_0214:
							Tuple<RESPONSE, MessageBuffer> result = val.GetResult();
							<result>5__2 = result;
							if (<result>5__2.Item1 != RESPONSE.SUCCESS || <result>5__2.Item2 == null)
							{
								result2 = Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)<result>5__2.Item1, (UInt48?)null);
							}
							else
							{
								value = UInt48.op_Implicit((byte)0);
								for (int num2 = 2; num2 < ((MessageBuffer)<result>5__2.Item2).Length; num2++)
								{
									value <<= 8;
									value += UInt48.op_Implicit(CommExtensions.GetUINT8((IByteList)(object)<result>5__2.Item2, num2));
								}
								result2 = Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)RESPONSE.SUCCESS, (UInt48?)value);
							}
							goto end_IL_00c2;
							IL_0147:
							((TaskAwaiter)(ref val2)).GetResult();
							goto IL_014e;
							end_IL_00c2:;
						}
						finally
						{
							if (num < 0)
							{
								Tuple<RESPONSE, MessageBuffer> obj = <result>5__2;
								if (obj != null)
								{
									MessageBuffer item = obj.Item2;
									if (item != null)
									{
										((Object)item).ReturnToPool();
									}
								}
							}
						}
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						<>8__1 = null;
						<result>5__2 = null;
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<>8__1 = null;
					<result>5__2 = null;
					<>t__builder.SetResult(result2);
				}

				[DebuggerHidden]
				private void SetStateMachine(IAsyncStateMachine stateMachine)
				{
					<>t__builder.SetStateMachine(stateMachine);
				}
			}

			private readonly LocalDevice Device;

			private ConcurrentDictionary<ulong, PidList> PidListCache = new ConcurrentDictionary<ulong, PidList>();

			public PidClient(LocalDevice device)
			{
				Device = device;
			}

			[AsyncStateMachine(typeof(<ReadPidListAsync>d__3))]
			public async System.Threading.Tasks.Task<Tuple<RESPONSE?, PidList>> ReadPidListAsync(AsyncOperation operation, IDevice tgtDevice)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				PidList pidList = null;
				ulong signature = tgtDevice.GetDeviceUniqueID();
				if (PidListCache.TryGetValue(signature, ref pidList) && pidList.Device != tgtDevice)
				{
					List<PidInfo> val = new List<PidInfo>();
					System.Collections.Generic.IEnumerator<PidInfo> enumerator = pidList.GetEnumerator();
					try
					{
						while (((System.Collections.IEnumerator)enumerator).MoveNext())
						{
							PidInfo current = enumerator.Current;
							val.Add(new PidInfo(tgtDevice, current.ID, current.Flags));
						}
					}
					finally
					{
						((System.IDisposable)enumerator)?.Dispose();
					}
					pidList = new PidList(tgtDevice, (System.Collections.Generic.IEnumerable<PidInfo>)val);
					if (tgtDevice.IsOnline)
					{
						PidListCache[signature] = pidList;
					}
					return Tuple.Create<RESPONSE?, PidList>((RESPONSE?)RESPONSE.SUCCESS, pidList);
				}
				List<PidInfo> list = new List<PidInfo>();
				int index = 0;
				int total = 0;
				operation.ReportProgress("Reading PID list from device...");
				while (true)
				{
					Tuple<RESPONSE, MessageBuffer> result = null;
					try
					{
						result = await Device.TransmitRequestAsync(operation, tgtDevice, (byte)16, PAYLOAD.FromArgs(new object[1] { (ushort)index }), (LocalDeviceRxEvent rx) => (rx.Length == 8 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == index) ? new RESPONSE?(RESPONSE.SUCCESS) : ((RESPONSE?)null));
						if (result.Item1 != RESPONSE.SUCCESS || result.Item2 == null)
						{
							return Tuple.Create<RESPONSE?, PidList>((RESPONSE?)result.Item1, (PidList)null);
						}
						if (index++ == 0)
						{
							total = CommExtensions.GetUINT16((IByteList)(object)result.Item2, 2);
						}
						else if (list.Count < total)
						{
							list.Add(new PidInfo(tgtDevice, PID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)result.Item2, 2)), CommExtensions.GetUINT8((IByteList)(object)result.Item2, 4)));
						}
						if (list.Count < total)
						{
							list.Add(new PidInfo(tgtDevice, PID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)result.Item2, 5)), CommExtensions.GetUINT8((IByteList)(object)result.Item2, 7)));
						}
						if (list.Count >= total)
						{
							list.Sort((Comparison<PidInfo>)((PidInfo first, PidInfo second) => first.ID.Value.CompareTo(second.ID.Value)));
							pidList = new PidList(tgtDevice, (System.Collections.Generic.IEnumerable<PidInfo>)list);
							PidListCache[signature] = pidList;
							operation.ReportProgress(100f, "Success!");
							return Tuple.Create<RESPONSE?, PidList>((RESPONSE?)RESPONSE.SUCCESS, pidList);
						}
						operation.ReportProgress(100f * (float)list.Count / (float)total, $"Read {list.Count} of {total} PIDs...");
					}
					finally
					{
						Tuple<RESPONSE, MessageBuffer> obj = result;
						if (obj != null)
						{
							MessageBuffer item = obj.Item2;
							if (item != null)
							{
								((Object)item).ReturnToPool();
							}
						}
					}
				}
			}

			[AsyncStateMachine(typeof(<ReadPidAsync>d__4))]
			public async System.Threading.Tasks.Task<Tuple<RESPONSE?, UInt48?>> ReadPidAsync(AsyncOperation operation, IDevice tgtDevice, PID pid)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				operation.ReportProgress("Reading PID " + pid.Value.ToString("X4") + "h from device...");
				Tuple<RESPONSE, MessageBuffer> result = null;
				try
				{
					result = await Device.TransmitRequestAsync(operation, tgtDevice, (byte)17, PAYLOAD.FromArgs(new object[1] { PID.op_Implicit(pid) }), delegate(LocalDeviceRxEvent rx)
					{
						if (rx.Length >= 3)
						{
							ushort uINT = CommExtensions.GetUINT16((IByteList)(object)rx, 0);
							if (uINT == PID.op_Implicit(pid))
							{
								return RESPONSE.SUCCESS;
							}
							if (uINT == 0 && rx.Length == 5 && PID.op_Implicit(pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
							{
								return (RESPONSE)rx[4];
							}
							if (uINT == 0 && rx.Length == 7 && PID.op_Implicit(pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
							{
								return (RESPONSE)rx[4];
							}
						}
						return (RESPONSE?)null;
					});
					if (result.Item1 != RESPONSE.SUCCESS || result.Item2 == null)
					{
						return Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)result.Item1, (UInt48?)null);
					}
					UInt48 val = UInt48.op_Implicit((byte)0);
					for (int num = 2; num < ((MessageBuffer)result.Item2).Length; num++)
					{
						val <<= 8;
						val += UInt48.op_Implicit(CommExtensions.GetUINT8((IByteList)(object)result.Item2, num));
					}
					return Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)RESPONSE.SUCCESS, (UInt48?)val);
				}
				finally
				{
					Tuple<RESPONSE, MessageBuffer> obj = result;
					if (obj != null)
					{
						MessageBuffer item = obj.Item2;
						if (item != null)
						{
							((Object)item).ReturnToPool();
						}
					}
				}
			}

			[AsyncStateMachine(typeof(<ReadPidAsync>d__5))]
			public async System.Threading.Tasks.Task<Tuple<RESPONSE?, UInt48?>> ReadPidAsync(AsyncOperation operation, IDevice tgtDevice, PID pid, bool withadd, ushort add)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				operation.ReportProgress("Reading PID " + pid.Value.ToString("X4") + "h from device...");
				PAYLOAD payload = ((!withadd) ? PAYLOAD.FromArgs(new object[1] { PID.op_Implicit(pid) }) : PAYLOAD.FromArgs(new object[1] { (uint)((PID.op_Implicit(pid) << 16) | add) }));
				Tuple<RESPONSE, MessageBuffer> result = null;
				try
				{
					result = await Device.TransmitRequestAsync(operation, tgtDevice, (byte)17, payload, delegate(LocalDeviceRxEvent rx)
					{
						if (rx.Length >= 3)
						{
							ushort uINT = CommExtensions.GetUINT16((IByteList)(object)rx, 0);
							if (uINT == PID.op_Implicit(pid))
							{
								return RESPONSE.SUCCESS;
							}
							if (uINT == 0 && rx.Length == 5 && PID.op_Implicit(pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
							{
								return (RESPONSE)rx[4];
							}
							if (uINT == 0 && rx.Length == 7 && PID.op_Implicit(pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
							{
								return (RESPONSE)rx[4];
							}
						}
						return (RESPONSE?)null;
					});
					if (result.Item1 != RESPONSE.SUCCESS || result.Item2 == null)
					{
						return Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)result.Item1, (UInt48?)null);
					}
					UInt48 val = UInt48.op_Implicit((byte)0);
					for (int num = 2; num < ((MessageBuffer)result.Item2).Length; num++)
					{
						val <<= 8;
						val += UInt48.op_Implicit(CommExtensions.GetUINT8((IByteList)(object)result.Item2, num));
					}
					return Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)RESPONSE.SUCCESS, (UInt48?)val);
				}
				finally
				{
					Tuple<RESPONSE, MessageBuffer> obj = result;
					if (obj != null)
					{
						MessageBuffer item = obj.Item2;
						if (item != null)
						{
							((Object)item).ReturnToPool();
						}
					}
				}
			}

			[AsyncStateMachine(typeof(<ReadPidAsync>d__6))]
			public async System.Threading.Tasks.Task<Tuple<RESPONSE?, UInt48?>> ReadPidAsync(AsyncOperation operation, PidInfo pidInfo)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				if (!pidInfo.IsReadable)
				{
					return Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)RESPONSE.WRITE_ONLY, (UInt48?)null);
				}
				if (pidInfo.IsWithAddress)
				{
					return await ReadPidAsync(operation, pidInfo.Device, pidInfo.ID, pidInfo.IsWithAddress, pidInfo.PID_Address);
				}
				return await ReadPidAsync(operation, pidInfo.Device, pidInfo.ID);
			}

			[AsyncStateMachine(typeof(<WritePidAsync>d__7))]
			public unsafe async System.Threading.Tasks.Task<Tuple<RESPONSE?, UInt48?>> WritePidAsync(AsyncOperation operation, IDevice tgtDevice, PID pid, UInt48 value, ISessionClient session)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				operation.ReportProgress($"Writing PID {pid.Value.ToString("X4")}h = {((object)(*(UInt48*)(&value))/*cast due to .constrained prefix*/).ToString()} to device...");
				Tuple<RESPONSE, MessageBuffer> result = null;
				try
				{
					if (session != null)
					{
						while (!session.IsOpen)
						{
							session.TryOpenSession();
							await System.Threading.Tasks.Task.Delay(15);
						}
					}
					result = await Device.TransmitRequestAsync(operation, tgtDevice, (byte)17, PAYLOAD.FromArgs(new object[2]
					{
						PID.op_Implicit(pid),
						value
					}), delegate(LocalDeviceRxEvent rx)
					{
						if (session != null)
						{
							session.TryOpenSession();
						}
						if (rx.Length >= 3)
						{
							ushort uINT = CommExtensions.GetUINT16((IByteList)(object)rx, 0);
							if (uINT == PID.op_Implicit(pid))
							{
								return RESPONSE.SUCCESS;
							}
							if (uINT == 0 && rx.Length == 5 && PID.op_Implicit(pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
							{
								return (RESPONSE)rx[4];
							}
							if (uINT == 0 && rx.Length == 7 && PID.op_Implicit(pid) == CommExtensions.GetUINT16((IByteList)(object)rx, 2))
							{
								return (RESPONSE)rx[4];
							}
						}
						return (RESPONSE?)null;
					});
					if (result.Item1 != RESPONSE.SUCCESS || result.Item2 == null)
					{
						return Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)result.Item1, (UInt48?)null);
					}
					value = UInt48.op_Implicit((byte)0);
					for (int num = 2; num < ((MessageBuffer)result.Item2).Length; num++)
					{
						value <<= 8;
						value += UInt48.op_Implicit(CommExtensions.GetUINT8((IByteList)(object)result.Item2, num));
					}
					return Tuple.Create<RESPONSE?, UInt48?>((RESPONSE?)RESPONSE.SUCCESS, (UInt48?)value);
				}
				finally
				{
					Tuple<RESPONSE, MessageBuffer> obj = result;
					if (obj != null)
					{
						MessageBuffer item = obj.Item2;
						if (item != null)
						{
							((Object)item).ReturnToPool();
						}
					}
				}
			}
		}

		[DefaultMember("Item")]
		private class PidServer
		{
			private class LocalPid
			{
				private readonly Func<UInt48> ReadDelegate;

				private readonly Action<UInt48> WriteDelegate;

				private const byte READONLY = 1;

				private const byte WRITEONLY = 2;

				private const byte READWRITE = 3;

				private const byte NONVOLATILE = 4;

				private const byte WITHADDRESS = 8;

				[field: CompilerGenerated]
				public PID ID
				{
					[CompilerGenerated]
					get;
					[CompilerGenerated]
					private set;
				}

				[field: CompilerGenerated]
				public byte Flags
				{
					[CompilerGenerated]
					get;
					[CompilerGenerated]
					private set;
				}

				public UInt48? RawValue
				{
					get
					{
						//IL_0014: Unknown result type (might be due to invalid IL or missing references)
						return ReadDelegate?.Invoke();
					}
					set
					{
						//IL_0016: Unknown result type (might be due to invalid IL or missing references)
						if (value.HasValue)
						{
							WriteDelegate?.Invoke(value.Value);
						}
					}
				}

				public bool IsReadable => (Flags & 1) != 0;

				public bool IsWritable => (Flags & 2) != 0;

				public bool IsNonVolatile => (Flags & 4) != 0;

				public bool IsWithAddress => (Flags & 8) != 0;

				public LocalPid(PID id, Func<UInt48> read_delegate, Action<UInt48> write_delegate)
				{
					ID = id;
					ReadDelegate = read_delegate;
					WriteDelegate = write_delegate;
					if (read_delegate != null)
					{
						Flags = (byte)((write_delegate == null) ? 1 : 3);
					}
					else if (write_delegate != null)
					{
						Flags = 2;
					}
					else
					{
						Flags = 0;
					}
				}
			}

			private readonly LocalDevice Device;

			private Dictionary<PID, LocalPid> PidDict = new Dictionary<PID, LocalPid>();

			private List<LocalPid> PidList = new List<LocalPid>();

			private LocalPid this[PID id]
			{
				get
				{
					LocalPid result = default(LocalPid);
					PidDict.TryGetValue(id, ref result);
					return result;
				}
			}

			public PidServer(LocalDevice device)
			{
				Device = device;
				Device.mRequestServer.AddRequestHandler((byte)16, Request10PidReadList);
				Device.mRequestServer.AddRequestHandler((byte)17, Request11PidReadWrite);
				Device.mRequestServer.AddRequestHandler((byte)18, Request12GetPidProperties);
			}

			public void Add(PID id, Func<UInt48> read_delegate, Action<UInt48> write_delegate)
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				if (PidDict.ContainsKey(id))
				{
					throw new InvalidOperationException("PID already exists in LocalDevice.PidServer");
				}
				LocalPid localPid = new LocalPid(id, read_delegate, write_delegate);
				PidDict.Add(id, localPid);
				PidList.Add(localPid);
			}

			public bool ContainsPid(PID id)
			{
				return this[id] != null;
			}

			public UInt48? ReadPidValue(PID id)
			{
				return this[id]?.RawValue;
			}

			private PAYLOAD? Request10PidReadList(AdapterRxEvent rx)
			{
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Count != 2)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				int num = CommExtensions.GetUINT16((IByteList)(object)rx, 0);
				PAYLOAD val = PAYLOAD.FromArgs(new object[1] { (ushort)num });
				if (num == 0)
				{
					((PAYLOAD)(ref val)).Append((ushort)PidList.Count);
					((PAYLOAD)(ref val)).Append((byte)0);
				}
				else
				{
					num = num * 2 - 1;
				}
				while (((PAYLOAD)(ref val)).Length < 8)
				{
					LocalPid localPid = null;
					if (num < PidList.Count)
					{
						localPid = PidList[num++];
					}
					((PAYLOAD)(ref val)).Append(PID.op_Implicit(localPid?.ID ?? PID.op_Implicit((ushort)0)));
					((PAYLOAD)(ref val)).Append(localPid?.Flags ?? 0);
				}
				return val;
			}

			private PAYLOAD? Request11PidReadWrite(AdapterRxEvent rx)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
				//IL_0186: Unknown result type (might be due to invalid IL or missing references)
				//IL_018b: Unknown result type (might be due to invalid IL or missing references)
				//IL_010d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0112: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0119: Unknown result type (might be due to invalid IL or missing references)
				//IL_011c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0121: Unknown result type (might be due to invalid IL or missing references)
				//IL_0123: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				//IL_0137: Unknown result type (might be due to invalid IL or missing references)
				//IL_013c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0149: Unknown result type (might be due to invalid IL or missing references)
				//IL_016f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0171: Unknown result type (might be due to invalid IL or missing references)
				//IL_0167: Unknown result type (might be due to invalid IL or missing references)
				//IL_016c: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_01db: Unknown result type (might be due to invalid IL or missing references)
				//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Count < 2)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				PID val = PID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0));
				LocalPid localPid = this[val];
				if (localPid == null)
				{
					return PAYLOAD.FromArgs(new object[3]
					{
						(ushort)0,
						PID.op_Implicit(val),
						(byte)4
					});
				}
				UInt48 val2 = UInt48.op_Implicit((byte)0);
				if (rx.Count == 2 || rx.Count == 4)
				{
					if (!localPid.IsReadable)
					{
						return PAYLOAD.FromArgs(new object[3]
						{
							(ushort)0,
							PID.op_Implicit(val),
							(byte)8
						});
					}
					val2 = localPid.RawValue.Value;
				}
				else
				{
					if (!localPid.IsWritable)
					{
						return PAYLOAD.FromArgs(new object[3]
						{
							(ushort)0,
							PID.op_Implicit(val),
							(byte)7
						});
					}
					UInt48 val3 = UInt48.op_Implicit((byte)0);
					int num = 2;
					while (num < rx.Count)
					{
						val3 <<= 8;
						val3 |= UInt48.op_Implicit(rx[num++]);
					}
					localPid.RawValue = val3;
					val2 = ((!localPid.IsReadable) ? val3 : localPid.RawValue.Value);
				}
				PAYLOAD val4 = PAYLOAD.FromArgs(new object[1] { PID.op_Implicit(val) });
				bool flag = false;
				int num2 = 0;
				while (num2 < 6)
				{
					if (num2 == 5)
					{
						flag = true;
					}
					if (num2 == 7)
					{
						flag = true;
					}
					flag |= (UInt48.op_Implicit(val2) & 0xFF0000000000L) != 0;
					if (flag)
					{
						((PAYLOAD)(ref val4)).Append((byte)(val2 >> 40));
					}
					num2++;
					val2 <<= 8;
				}
				return val4;
			}

			private PAYLOAD? Request12GetPidProperties(AdapterRxEvent rx)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Count != 2)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				PID val = PID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0));
				LocalPid localPid = this[val];
				if (localPid == null)
				{
					return PAYLOAD.FromArgs(new object[2]
					{
						PID.op_Implicit(val),
						(byte)4
					});
				}
				SESSION_ID uNKNOWN = SESSION_ID.UNKNOWN;
				return PAYLOAD.FromArgs(new object[3]
				{
					PID.op_Implicit(val),
					localPid.Flags,
					SESSION_ID.op_Implicit(uNKNOWN)
				});
			}
		}

		private class RequestServer
		{
			private readonly LocalDevice Device;

			private readonly Func<AdapterRxEvent, PAYLOAD?>[] RequestDelegateTable = new Func<AdapterRxEvent, PAYLOAD?>[256];

			public RequestServer(LocalDevice device)
			{
				Device = device;
				AddRequestHandler((byte)0, Request00PartNumberRead);
				AddRequestHandler((byte)3, Request03SoftwareUpdateAuthorization);
			}

			public void AddRequestHandler(REQUEST num, Func<AdapterRxEvent, PAYLOAD?> handler)
			{
				RequestDelegateTable[(byte)num] = handler;
			}

			public void ProcessRequest(AdapterRxEvent rx)
			{
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_0095: Unknown result type (might be due to invalid IL or missing references)
				//IL_013b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0142: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
				REQUEST rEQUEST = rx.MessageData;
				PAYLOAD? val = null;
				val = ((RequestDelegateTable[(byte)rEQUEST] == null) ? new PAYLOAD?(PAYLOAD.FromArgs(new object[1] { (byte)1 })) : RequestDelegateTable[(byte)rEQUEST]?.Invoke(rx));
				if (!val.HasValue)
				{
					return;
				}
				ADDRESS address = Device.Address;
				if (!address.IsValidDeviceAddress)
				{
					return;
				}
				if (val.HasValue)
				{
					PAYLOAD valueOrDefault = val.GetValueOrDefault();
					if (((PAYLOAD)(ref valueOrDefault)).Length == 1)
					{
						byte? obj;
						if (!val.HasValue)
						{
							obj = null;
						}
						else
						{
							valueOrDefault = val.GetValueOrDefault();
							obj = ((PAYLOAD)(ref valueOrDefault))[0];
						}
						if (obj != 0 && rx.TargetAddress == ADDRESS.BROADCAST)
						{
							return;
						}
					}
				}
				Device.Transmit(new CAN_ID((byte)129, address, rx.SourceAddress, rEQUEST), val.Value);
			}

			private PAYLOAD? Request00PartNumberRead(AdapterRxEvent rx)
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Count != 0)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				string softwarePartNumber = Device.SoftwarePartNumber;
				PAYLOAD val = default(PAYLOAD);
				((PAYLOAD)(ref val))..ctor(8);
				for (int i = 0; i < ((PAYLOAD)(ref val)).Length && i < softwarePartNumber.Length; i++)
				{
					((PAYLOAD)(ref val))[i] = (byte)softwarePartNumber[i];
				}
				return val;
			}

			private PAYLOAD? Request03SoftwareUpdateAuthorization(AdapterRxEvent rx)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_016d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0133: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Count != 1)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				if (Device.Address != Device.Product.Address)
				{
					if (rx.TargetAddress == ADDRESS.BROADCAST)
					{
						return PAYLOAD.FromArgs(new object[1] { (byte)0 });
					}
					return PAYLOAD.FromArgs(new object[2]
					{
						rx[0],
						(byte)6
					});
				}
				switch (rx[0])
				{
				default:
					return PAYLOAD.FromArgs(new object[2]
					{
						rx[0],
						(byte)2
					});
				case 0:
					Device.mLocalProduct.IsSoftwareUpdateAuthorized = false;
					return PAYLOAD.FromArgs(new object[2]
					{
						rx[0],
						(byte)0
					});
				case 1:
					if (!Device.mLocalProduct.IsSoftwareUpdateAvailable)
					{
						return PAYLOAD.FromArgs(new object[2]
						{
							rx[0],
							(byte)9
						});
					}
					Device.mLocalProduct.IsSoftwareUpdateAuthorized = true;
					return PAYLOAD.FromArgs(new object[2]
					{
						rx[0],
						(byte)0
					});
				}
			}
		}

		[DefaultMember("Item")]
		private class SessionServer : Disposable
		{
			public class LocalSession : Disposable, ISession
			{
				private class ClientEndpoint : IBusEndpoint
				{
					private LocalSession mHostedSession;

					private ADDRESS _address = ADDRESS.INVALID;

					[field: CompilerGenerated]
					public IAdapter Adapter
					{
						[CompilerGenerated]
						get;
						[CompilerGenerated]
						private set;
					}

					public ADDRESS Address
					{
						get
						{
							return _address;
						}
						set
						{
							//IL_0057: Unknown result type (might be due to invalid IL or missing references)
							if (_address != value)
							{
								if (_address.IsValidDeviceAddress)
								{
									ushort num = SESSION_ID.op_Implicit(mHostedSession.SessionID);
									mHostedSession.LocalHost.Transmit29((byte)129, 69, this, PAYLOAD.FromArgs(new object[2]
									{
										num,
										(byte)0
									}));
								}
								_address = value;
							}
						}
					}

					public bool IsOnline
					{
						get
						{
							if (((IAdapter)Adapter).IsConnected)
							{
								return Address.IsValidDeviceAddress;
							}
							return false;
						}
					}

					public ClientEndpoint(IAdapter adapter, ADDRESS address, LocalSession sessionHost)
					{
						Adapter = adapter;
						Address = address;
						mHostedSession = sessionHost;
					}
				}

				private static readonly TimeSpan SESSION_TIMEOUT = TimeSpan.FromSeconds(5.0);

				private static readonly TimeSpan SEED_TIMEOUT = TimeSpan.FromSeconds(3.5);

				private readonly ILocalDevice LocalHost;

				private ClientEndpoint mClient;

				private Timer mOpenTime = new Timer(true);

				private Timer HeartbeatTimeout = new Timer(true);

				private Timer SeedTimeout = new Timer(true);

				private ADDRESS AddressSeedWasSentTo;

				private uint Seed;

				[field: CompilerGenerated]
				public SESSION_ID SessionID
				{
					[CompilerGenerated]
					get;
					[CompilerGenerated]
					private set;
				}

				public IBusEndpoint Host => LocalHost;

				public IBusEndpoint Client => mClient;

				public bool IsOpen => Client.Address.IsValidDeviceAddress;

				public TimeSpan OpenTime
				{
					get
					{
						//IL_0014: Unknown result type (might be due to invalid IL or missing references)
						//IL_0008: Unknown result type (might be due to invalid IL or missing references)
						if (!IsOpen)
						{
							return TimeSpan.Zero;
						}
						return mOpenTime.ElapsedTime;
					}
				}

				public void CloseSession()
				{
					mClient.Address = ADDRESS.INVALID;
				}

				public LocalSession(LocalDevice localhost, SESSION_ID session)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_000c: Expected O, but got Unknown
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0018: Expected O, but got Unknown
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0024: Expected O, but got Unknown
					SessionID = session;
					LocalHost = localhost;
					mClient = new ClientEndpoint(localhost.Adapter, ADDRESS.INVALID, this);
				}

				public override void Dispose(bool disposing)
				{
					if (disposing)
					{
						mClient.Address = ADDRESS.INVALID;
					}
				}

				public void BackgroundTask()
				{
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					if (mClient.Address.IsValidDeviceAddress && HeartbeatTimeout.ElapsedTime >= SESSION_TIMEOUT)
					{
						mClient.Address = ADDRESS.INVALID;
					}
				}

				public PAYLOAD? ProcessRequest(AdapterRxEvent rx, REQUEST request)
				{
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					//IL_0056: Unknown result type (might be due to invalid IL or missing references)
					//IL_0094: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
					//IL_0163: Unknown result type (might be due to invalid IL or missing references)
					//IL_028a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0462: Unknown result type (might be due to invalid IL or missing references)
					//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
					//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
					//IL_0531: Unknown result type (might be due to invalid IL or missing references)
					//IL_0110: Unknown result type (might be due to invalid IL or missing references)
					//IL_0115: Unknown result type (might be due to invalid IL or missing references)
					//IL_0125: Unknown result type (might be due to invalid IL or missing references)
					//IL_0194: Unknown result type (might be due to invalid IL or missing references)
					//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
					//IL_024c: Unknown result type (might be due to invalid IL or missing references)
					//IL_030e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0313: Unknown result type (might be due to invalid IL or missing references)
					//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
					//IL_033d: Unknown result type (might be due to invalid IL or missing references)
					//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
					//IL_0382: Unknown result type (might be due to invalid IL or missing references)
					//IL_0431: Unknown result type (might be due to invalid IL or missing references)
					//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
					if (rx.Count < 2)
					{
						return PAYLOAD.FromArgs(new object[1] { (byte)2 });
					}
					SESSION_ID val = SESSION_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0));
					if (SessionID != val)
					{
						return PAYLOAD.FromArgs(new object[2]
						{
							SESSION_ID.op_Implicit(val),
							(byte)4
						});
					}
					switch (request)
					{
					default:
						return PAYLOAD.FromArgs(new object[1] { (byte)1 });
					case 65:
					{
						if (rx.Count != 2)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)2
							});
						}
						ADDRESS address = Client.Address;
						object[] obj = new object[3]
						{
							SESSION_ID.op_Implicit(val),
							(byte)(address.IsValidDeviceAddress ? address : ADDRESS.BROADCAST),
							null
						};
						TimeSpan openTime = OpenTime;
						obj[2] = (uint)((TimeSpan)(ref openTime)).TotalMilliseconds;
						return PAYLOAD.FromArgs(obj);
					}
					case 66:
						if (rx.Count != 2 || !rx.SourceAddress.IsValidDeviceAddress)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)2
							});
						}
						if (IsOpen)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)11
							});
						}
						if (SESSION_ID.op_Implicit(SessionID) == 4 && LocalHost.NetworkStatus.IsHazardousDevice && (byte)LocalHost.NetworkStatus.InMotionLockoutLevel >= 2)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)9
							});
						}
						Seed = (uint)ThreadLocalRandom.Next();
						AddressSeedWasSentTo = rx.SourceAddress;
						SeedTimeout.Reset();
						return PAYLOAD.FromArgs(new object[2]
						{
							SESSION_ID.op_Implicit(val),
							Seed
						});
					case 67:
					{
						if (rx.Count != 6 || !rx.SourceAddress.IsValidDeviceAddress)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)2
							});
						}
						if (IsOpen)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)9
							});
						}
						if (rx.SourceAddress != AddressSeedWasSentTo)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)12
							});
						}
						AddressSeedWasSentTo = ADDRESS.INVALID;
						if (SeedTimeout.ElapsedTime > SEED_TIMEOUT)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)12
							});
						}
						uint uINT = CommExtensions.GetUINT32((IByteList)(object)rx, 2);
						uint num = SessionID.Encrypt(Seed);
						if (uINT != num)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)13
							});
						}
						if (SESSION_ID.op_Implicit(SessionID) == 4 && LocalHost.NetworkStatus.IsHazardousDevice && (byte)LocalHost.NetworkStatus.InMotionLockoutLevel >= 2)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)9
							});
						}
						mClient.Address = rx.SourceAddress;
						mOpenTime.Reset();
						HeartbeatTimeout.Reset();
						return PAYLOAD.FromArgs(new object[1] { SESSION_ID.op_Implicit(val) });
					}
					case 68:
						if (rx.Count != 2)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)2
							});
						}
						if (!IsOpen || rx.SourceAddress != Client.Address)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)14
							});
						}
						HeartbeatTimeout.Reset();
						return null;
					case 69:
						if (rx.Count != 2)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)2
							});
						}
						if (!IsOpen || rx.SourceAddress != Client.Address)
						{
							return PAYLOAD.FromArgs(new object[2]
							{
								SESSION_ID.op_Implicit(val),
								(byte)14
							});
						}
						mClient.Address = ADDRESS.INVALID;
						return null;
					}
				}
			}

			private readonly LocalDevice Device;

			private Dictionary<SESSION_ID, LocalSession> SessionDict = new Dictionary<SESSION_ID, LocalSession>();

			private List<SESSION_ID> SessionList = new List<SESSION_ID>();

			public LocalSession this[SESSION_ID id]
			{
				get
				{
					LocalSession result = null;
					SessionDict?.TryGetValue(id, ref result);
					return result;
				}
			}

			public bool IsAnySessionOpen
			{
				get
				{
					//IL_000b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					Enumerator<SESSION_ID, LocalSession> enumerator = SessionDict.Values.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.IsOpen)
							{
								return true;
							}
						}
					}
					finally
					{
						((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
					}
					return false;
				}
			}

			public SessionServer(LocalDevice device)
			{
				Device = device;
				Device.mRequestServer.AddRequestHandler((byte)64, Request41ReadSessionList);
				Device.mRequestServer.AddRequestHandler((byte)65, [CompilerGenerated] (AdapterRxEvent rx) => ProcessRequest(rx, (byte)65));
				Device.mRequestServer.AddRequestHandler((byte)66, [CompilerGenerated] (AdapterRxEvent rx) => ProcessRequest(rx, (byte)66));
				Device.mRequestServer.AddRequestHandler((byte)67, [CompilerGenerated] (AdapterRxEvent rx) => ProcessRequest(rx, (byte)67));
				Device.mRequestServer.AddRequestHandler((byte)68, [CompilerGenerated] (AdapterRxEvent rx) => ProcessRequest(rx, (byte)68));
				Device.mRequestServer.AddRequestHandler((byte)69, [CompilerGenerated] (AdapterRxEvent rx) => ProcessRequest(rx, (byte)69));
			}

			public override void Dispose(bool disposing)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				if (!disposing)
				{
					return;
				}
				Enumerator<SESSION_ID, LocalSession> enumerator = SessionDict.Values.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						((Disposable)enumerator.Current).Dispose();
					}
				}
				finally
				{
					((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
				SessionDict = null;
				SessionList = null;
			}

			public void CloseCommandSessions()
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Enumerator<SESSION_ID, LocalSession> enumerator = SessionDict.Values.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						LocalSession current = enumerator.Current;
						if (SESSION_ID.op_Implicit(current.SessionID) == 4 && current.IsOpen)
						{
							current.CloseSession();
						}
					}
				}
				finally
				{
					((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
			}

			public ISession AddSessionSupport(SESSION_ID session)
			{
				lock (SessionDict)
				{
					LocalSession localSession = default(LocalSession);
					if (!SessionDict.TryGetValue(session, ref localSession))
					{
						localSession = new LocalSession(Device, session);
						SessionDict.Add(session, localSession);
						SessionList.Add(session);
					}
					return localSession;
				}
			}

			private PAYLOAD? Request41ReadSessionList(AdapterRxEvent rx)
			{
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Count != 2)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				int num = CommExtensions.GetUINT16((IByteList)(object)rx, 0);
				PAYLOAD val = PAYLOAD.FromArgs(new object[1] { (ushort)num });
				if (num == 0)
				{
					((PAYLOAD)(ref val)).Append((ushort)SessionList.Count);
				}
				else
				{
					num = num * 3 - 1;
				}
				while (((PAYLOAD)(ref val)).Length < 8)
				{
					ushort num2 = 0;
					if (num < SessionList.Count)
					{
						num2 = SESSION_ID.op_Implicit(SessionList[num++]);
					}
					((PAYLOAD)(ref val)).Append(num2);
				}
				return val;
			}

			private PAYLOAD? ProcessRequest(AdapterRxEvent rx, REQUEST request)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				if (rx.Count < 2)
				{
					return PAYLOAD.FromArgs(new object[1] { (byte)2 });
				}
				SESSION_ID val = SESSION_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0));
				LocalSession localSession = default(LocalSession);
				SessionDict.TryGetValue(val, ref localSession);
				if (localSession == null)
				{
					return PAYLOAD.FromArgs(new object[2]
					{
						SESSION_ID.op_Implicit(val),
						(byte)4
					});
				}
				return localSession.ProcessRequest(rx, request);
			}

			public void BackgroundTask()
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Enumerator<SESSION_ID, LocalSession> enumerator = SessionDict.Values.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.BackgroundTask();
					}
				}
				finally
				{
					((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			public bool message_sent;

			public IBusEndpoint target;

			public REQUEST request;

			public Func<LocalDeviceRxEvent, RESPONSE?> validator;

			public object mutex;

			public MessageBuffer retained_message;

			public TaskCompletionSource<RESPONSE> tcs;

			internal bool <TransmitRequestAsync>b__0(LocalDeviceRxEvent rx)
			{
				if (!message_sent)
				{
					return false;
				}
				if (rx.SourceAddress != target.Address)
				{
					return false;
				}
				if ((byte)rx.MessageType != 129)
				{
					return false;
				}
				if (rx.MessageData != (byte)request)
				{
					return false;
				}
				if (validator != null)
				{
					lock (mutex)
					{
						RESPONSE? rESPONSE = validator?.Invoke(rx);
						if (rESPONSE.HasValue)
						{
							validator = null;
							if (rESPONSE == RESPONSE.SUCCESS)
							{
								MessageBuffer val = ResourcePool<MessageBuffer>.GetObject();
								val.CopyFrom((IMessage)(object)rx);
								retained_message = val;
							}
							tcs.SetResult(rESPONSE.Value);
							return true;
						}
					}
				}
				return false;
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ReadBlockDataAsync>d__10 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> <>t__builder;

			public LocalDevice <>4__this;

			public AsyncOperation operation;

			public IBlock block;

			public int bulk_xfer_delay_ms;

			public ISessionClient session;

			private TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> <>u__1;

			private void MoveNext()
			{
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>> result;
				try
				{
					TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> val;
					if (num != 0)
					{
						val = localDevice.mBlockClient.ReadBlockDataAsync(operation, block, bulk_xfer_delay_ms, session).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>>, <ReadBlockDataAsync>d__10>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
				}
				catch (System.Exception exception)
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
		private struct <ReadBlockDataAsync>d__9 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> <>t__builder;

			public LocalDevice <>4__this;

			public AsyncOperation operation;

			public IDevice target;

			public BLOCK_ID block;

			public int bulk_xfer_delay_ms;

			public ISessionClient session;

			private TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> <>u__1;

			private void MoveNext()
			{
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>> result;
				try
				{
					TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> val;
					if (num != 0)
					{
						val = localDevice.mBlockClient.ReadBlockDataAsync(operation, target, block, bulk_xfer_delay_ms, session).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>>, <ReadBlockDataAsync>d__9>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
				}
				catch (System.Exception exception)
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
		private struct <ReadBlockListAsync>d__6 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>> <>t__builder;

			public LocalDevice <>4__this;

			public AsyncOperation operation;

			public IDevice target;

			private TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>> <>u__1;

			private void MoveNext()
			{
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>> result;
				try
				{
					TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>> val;
					if (num != 0)
					{
						val = localDevice.mBlockClient.ReadBlockListAsync(operation, target).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>>, <ReadBlockListAsync>d__6>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
				}
				catch (System.Exception exception)
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
		private struct <ReadBlockPropertiesAsync>d__7 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Tuple<RESPONSE, IBlock>> <>t__builder;

			public LocalDevice <>4__this;

			public AsyncOperation operation;

			public IDevice target;

			public BLOCK_ID block;

			private TaskAwaiter<Tuple<RESPONSE, IBlock>> <>u__1;

			private void MoveNext()
			{
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				Tuple<RESPONSE, IBlock> result;
				try
				{
					TaskAwaiter<Tuple<RESPONSE, IBlock>> val;
					if (num != 0)
					{
						val = localDevice.mBlockClient.ReadBlockPropertiesAsync(operation, target, block).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, IBlock>>, <ReadBlockPropertiesAsync>d__7>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, IBlock>>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
				}
				catch (System.Exception exception)
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
		private struct <ReadPidAsync>d__148 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Tuple<RESPONSE?, UInt48?>> <>t__builder;

			public LocalDevice <>4__this;

			public AsyncOperation operation;

			public IDevice target;

			public PID id;

			private TaskAwaiter<Tuple<RESPONSE?, UInt48?>> <>u__1;

			private void MoveNext()
			{
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				Tuple<RESPONSE?, UInt48?> result;
				try
				{
					TaskAwaiter<Tuple<RESPONSE?, UInt48?>> val;
					if (num != 0)
					{
						val = localDevice.mPidClient.ReadPidAsync(operation, target, id).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE?, UInt48?>>, <ReadPidAsync>d__148>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Tuple<RESPONSE?, UInt48?>>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
				}
				catch (System.Exception exception)
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
		private struct <ReadPidAsync>d__149 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Tuple<RESPONSE?, UInt48?>> <>t__builder;

			public LocalDevice <>4__this;

			public AsyncOperation operation;

			public IDevice target;

			public PID id;

			public bool withadd;

			public ushort add;

			private TaskAwaiter<Tuple<RESPONSE?, UInt48?>> <>u__1;

			private void MoveNext()
			{
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				Tuple<RESPONSE?, UInt48?> result;
				try
				{
					TaskAwaiter<Tuple<RESPONSE?, UInt48?>> val;
					if (num != 0)
					{
						val = localDevice.mPidClient.ReadPidAsync(operation, target, id, withadd, add).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE?, UInt48?>>, <ReadPidAsync>d__149>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Tuple<RESPONSE?, UInt48?>>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
				}
				catch (System.Exception exception)
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
		private struct <ReadPidAsync>d__150 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Tuple<RESPONSE?, UInt48?>> <>t__builder;

			public LocalDevice <>4__this;

			public AsyncOperation operation;

			public PidInfo pidInfo;

			private TaskAwaiter<Tuple<RESPONSE?, UInt48?>> <>u__1;

			private void MoveNext()
			{
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				Tuple<RESPONSE?, UInt48?> result;
				try
				{
					TaskAwaiter<Tuple<RESPONSE?, UInt48?>> val;
					if (num != 0)
					{
						val = localDevice.mPidClient.ReadPidAsync(operation, pidInfo).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE?, UInt48?>>, <ReadPidAsync>d__150>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Tuple<RESPONSE?, UInt48?>>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
				}
				catch (System.Exception exception)
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
		private struct <ReadPidListAsync>d__147 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Tuple<RESPONSE?, PidList>> <>t__builder;

			public LocalDevice <>4__this;

			public AsyncOperation operation;

			public IDevice target;

			private TaskAwaiter<Tuple<RESPONSE?, PidList>> <>u__1;

			private void MoveNext()
			{
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				Tuple<RESPONSE?, PidList> result;
				try
				{
					TaskAwaiter<Tuple<RESPONSE?, PidList>> val;
					if (num != 0)
					{
						val = localDevice.mPidClient.ReadPidListAsync(operation, target).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE?, PidList>>, <ReadPidListAsync>d__147>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Tuple<RESPONSE?, PidList>>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
				}
				catch (System.Exception exception)
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
		private struct <RecalculateBlockCrcAsync>d__8 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Tuple<RESPONSE, uint?>> <>t__builder;

			public LocalDevice <>4__this;

			public AsyncOperation operation;

			public IDevice target;

			public BLOCK_ID block;

			private TaskAwaiter<Tuple<RESPONSE, uint?>> <>u__1;

			private void MoveNext()
			{
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				Tuple<RESPONSE, uint?> result;
				try
				{
					TaskAwaiter<Tuple<RESPONSE, uint?>> val;
					if (num != 0)
					{
						val = localDevice.mBlockClient.RecalculateBlockCrcAsync(operation, target, block).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE, uint?>>, <RecalculateBlockCrcAsync>d__8>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Tuple<RESPONSE, uint?>>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
				}
				catch (System.Exception exception)
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
		private struct <TransmitRequestAsync>d__2 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Tuple<RESPONSE, MessageBuffer>> <>t__builder;

			public IBusEndpoint target;

			public REQUEST request;

			public Func<LocalDeviceRxEvent, RESPONSE?> validator;

			public LocalDevice <>4__this;

			private <>c__DisplayClass2_0 <>8__1;

			public PAYLOAD payload;

			public AsyncOperation operation;

			private ReusableSubscription <rx_listener>5__2;

			private int <delay>5__3;

			private TaskAwaiter <>u__1;

			private TaskAwaiter<System.Threading.Tasks.Task> <>u__2;

			private void MoveNext()
			{
				//IL_0163: Unknown result type (might be due to invalid IL or missing references)
				//IL_0168: Unknown result type (might be due to invalid IL or missing references)
				//IL_016f: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0203: Unknown result type (might be due to invalid IL or missing references)
				//IL_011e: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0130: Unknown result type (might be due to invalid IL or missing references)
				//IL_0135: Unknown result type (might be due to invalid IL or missing references)
				//IL_01db: Unknown result type (might be due to invalid IL or missing references)
				//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0149: Unknown result type (might be due to invalid IL or missing references)
				//IL_014a: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				Tuple<RESPONSE, MessageBuffer> result;
				try
				{
					if ((uint)num <= 1u)
					{
						goto IL_00da;
					}
					<>8__1 = new <>c__DisplayClass2_0();
					<>8__1.target = target;
					<>8__1.request = request;
					<>8__1.validator = validator;
					if (<>8__1.target.IsOnline)
					{
						<>8__1.mutex = new object();
						<>8__1.tcs = new TaskCompletionSource<RESPONSE>();
						<>8__1.retained_message = null;
						<>8__1.message_sent = false;
						<rx_listener>5__2 = localDevice.ReusableSubscriptionPool.Get();
						<rx_listener>5__2.SetDelegate(localDevice, delegate(LocalDeviceRxEvent rx)
						{
							if (!<>8__1.message_sent)
							{
								return false;
							}
							if (rx.SourceAddress != <>8__1.target.Address)
							{
								return false;
							}
							if ((byte)rx.MessageType != 129)
							{
								return false;
							}
							if (rx.MessageData != (byte)<>8__1.request)
							{
								return false;
							}
							if (<>8__1.validator != null)
							{
								lock (<>8__1.mutex)
								{
									RESPONSE? rESPONSE = <>8__1.validator?.Invoke(rx);
									if (rESPONSE.HasValue)
									{
										<>8__1.validator = null;
										if (rESPONSE == RESPONSE.SUCCESS)
										{
											MessageBuffer val3 = ResourcePool<MessageBuffer>.GetObject();
											val3.CopyFrom((IMessage)(object)rx);
											<>8__1.retained_message = val3;
										}
										<>8__1.tcs.SetResult(rESPONSE.Value);
										return true;
									}
								}
							}
							return false;
						});
						goto IL_00da;
					}
					result = Tuple.Create<RESPONSE, MessageBuffer>(RESPONSE.FAILED, (MessageBuffer)null);
					goto end_IL_000e;
					IL_00da:
					try
					{
						TaskAwaiter<System.Threading.Tasks.Task> val;
						if (num != 0)
						{
							if (num != 1)
							{
								<delay>5__3 = 500;
								goto IL_021a;
							}
							val = <>u__2;
							<>u__2 = default(TaskAwaiter<System.Threading.Tasks.Task>);
							num = (<>1__state = -1);
							goto IL_0212;
						}
						TaskAwaiter val2 = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_017e;
						IL_021a:
						if (localDevice.IsOnline && <>8__1.target.IsOnline && !operation.IsCancellationRequested && !((System.Threading.Tasks.Task)<>8__1.tcs.Task).IsCompleted)
						{
							if (!localDevice.Transmit29((byte)128, <>8__1.request, <>8__1.target, payload))
							{
								val2 = System.Threading.Tasks.Task.Delay(5).GetAwaiter();
								if (!((TaskAwaiter)(ref val2)).IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val2;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <TransmitRequestAsync>d__2>(ref val2, ref this);
									return;
								}
								goto IL_017e;
							}
							<>8__1.message_sent = true;
							val = System.Threading.Tasks.Task.WhenAny((System.Threading.Tasks.Task)<>8__1.tcs.Task, System.Threading.Tasks.Task.Delay(<delay>5__3, operation.CancellationToken)).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__2 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<System.Threading.Tasks.Task>, <TransmitRequestAsync>d__2>(ref val, ref this);
								return;
							}
							goto IL_0212;
						}
						goto end_IL_00da;
						IL_017e:
						((TaskAwaiter)(ref val2)).GetResult();
						goto IL_021a;
						IL_0212:
						val.GetResult();
						goto IL_021a;
						end_IL_00da:;
					}
					catch (TimeoutException)
					{
					}
					catch (OperationCanceledException)
					{
					}
					finally
					{
						if (num < 0)
						{
							((Object)<rx_listener>5__2).ReturnToPool();
							object obj = <>8__1.mutex;
							bool flag = false;
							try
							{
								Monitor.Enter(obj, ref flag);
								<>8__1.validator = null;
							}
							finally
							{
								if (num < 0 && flag)
								{
									Monitor.Exit(obj);
								}
							}
						}
					}
					result = ((<>8__1.retained_message != null) ? Tuple.Create<RESPONSE, MessageBuffer>(RESPONSE.SUCCESS, <>8__1.retained_message) : (((System.Threading.Tasks.Task)<>8__1.tcs.Task).IsCompleted ? Tuple.Create<RESPONSE, MessageBuffer>(<>8__1.tcs.Task.Result, (MessageBuffer)null) : ((!operation.IsCancellationRequested) ? Tuple.Create<RESPONSE, MessageBuffer>(RESPONSE.FAILED, (MessageBuffer)null) : Tuple.Create<RESPONSE, MessageBuffer>(RESPONSE.CANCELLED, (MessageBuffer)null))));
					end_IL_000e:;
				}
				catch (System.Exception exception)
				{
					<>1__state = -2;
					<>8__1 = null;
					<rx_listener>5__2 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<>8__1 = null;
				<rx_listener>5__2 = null;
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
		private struct <WriteBlockDataAsync>d__11 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<RESPONSE> <>t__builder;

			public LocalDevice <>4__this;

			public AsyncOperation operation;

			public IBlock block;

			public System.Collections.Generic.IReadOnlyList<byte> data;

			public int bulk_xfer_delay_ms;

			public ISessionClient session;

			private TaskAwaiter<RESPONSE> <>u__1;

			private void MoveNext()
			{
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				RESPONSE result;
				try
				{
					TaskAwaiter<RESPONSE> val;
					if (num != 0)
					{
						val = localDevice.mBlockClient.WriteBlockDataAsync(operation, block, data, bulk_xfer_delay_ms, session).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<RESPONSE>, <WriteBlockDataAsync>d__11>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<RESPONSE>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
				}
				catch (System.Exception exception)
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
		private struct <WritePidAsync>d__151 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Tuple<RESPONSE?, UInt48?>> <>t__builder;

			public LocalDevice <>4__this;

			public AsyncOperation operation;

			public IDevice target;

			public PID id;

			public UInt48 value;

			public ISessionClient session;

			private TaskAwaiter<Tuple<RESPONSE?, UInt48?>> <>u__1;

			private void MoveNext()
			{
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				Tuple<RESPONSE?, UInt48?> result;
				try
				{
					TaskAwaiter<Tuple<RESPONSE?, UInt48?>> val;
					if (num != 0)
					{
						val = localDevice.mPidClient.WritePidAsync(operation, target, id, value, session).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE?, UInt48?>>, <WritePidAsync>d__151>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Tuple<RESPONSE?, UInt48?>>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
				}
				catch (System.Exception exception)
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
		private struct <WritePidAsync>d__152 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Tuple<RESPONSE?, Int48?>> <>t__builder;

			public LocalDevice <>4__this;

			public AsyncOperation operation;

			public IDevice target;

			public PID id;

			public Int48 value;

			public ISessionClient session;

			private TaskAwaiter<Tuple<RESPONSE?, UInt48?>> <>u__1;

			private void MoveNext()
			{
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				LocalDevice localDevice = <>4__this;
				Tuple<RESPONSE?, Int48?> result2;
				try
				{
					TaskAwaiter<Tuple<RESPONSE?, UInt48?>> val;
					if (num != 0)
					{
						val = localDevice.mPidClient.WritePidAsync(operation, target, id, (UInt48)Int48.op_Implicit(value), session).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<RESPONSE?, UInt48?>>, <WritePidAsync>d__152>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Tuple<RESPONSE?, UInt48?>>);
						num = (<>1__state = -1);
					}
					Tuple<RESPONSE?, UInt48?> result = val.GetResult();
					result2 = (result.Item2.HasValue ? Tuple.Create<RESPONSE?, Int48?>(result.Item1, (Int48?)(Int48)UInt48.op_Implicit(result.Item2.Value)) : Tuple.Create<RESPONSE?, Int48?>(result.Item1, (Int48?)null));
				}
				catch (System.Exception exception)
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

		private readonly ResourcePool<ReusableSubscription> ReusableSubscriptionPool = new ResourcePool<ReusableSubscription>();

		private BlockClient mBlockClient;

		private BlockServer mBlockServer;

		private static readonly TimeSpan PRODUCT_STATUS_MESSAGE_PERIOD = TimeSpan.FromSeconds(1.0);

		private static readonly TimeSpan NETWORK_MESSAGE_PERIOD = TimeSpan.FromSeconds(1.0);

		private static readonly TimeSpan CIRCUIT_ID_MESSAGE_PERIOD = TimeSpan.FromSeconds(1.0);

		private static readonly TimeSpan DEVICE_ID_MESSAGE_PERIOD = TimeSpan.FromSeconds(1.0);

		private static readonly TimeSpan ADDRESS_CLAIM_TIMEOUT = TimeSpan.FromSeconds(1.0);

		private static readonly TimeSpan ADDRESS_DETECTED_TIMEOUT = TimeSpan.FromSeconds(5.0);

		private bool _isNotAcceptingCommands;

		private readonly LocalProduct mLocalProduct;

		private byte? mDeviceCapabilities;

		protected readonly SubscriptionManager Subscriptions;

		private AddressClaimManager AddressClaim;

		private MuteManager mMuteManager;

		private InMotionLockoutManager mInMotionLockoutManager;

		private Timer ProductStatusMsgTime = new Timer(true);

		private Timer NetworkMsgTime = new Timer(true);

		private Timer CircuitIDMsgTime = new Timer(true);

		private Timer DeviceIdMsgTime = new Timer(true);

		private Timer DeviceStatusMsgTime = new Timer(true);

		private readonly Timer Uptime = new Timer(true);

		private PAYLOAD? LastDeviceStatusTx;

		private bool mEnableDevice;

		private int TxBytesSent;

		private int TxMessagesSent;

		private int RxBytesReceived;

		private int RxMessagesReceived;

		private readonly LocalDeviceOnlineEvent LocalDeviceOnlineEvent;

		private readonly LocalDeviceOfflineEvent LocalDeviceOfflineEvent;

		private readonly LocalDeviceRxEvent LocalDeviceRxEvent;

		private PidClient mPidClient;

		private PidServer mPidServer;

		private RequestServer mRequestServer;

		private SessionServer mSessionServer;

		public IAdapter Adapter => Product.Adapter;

		public ADDRESS Address => AddressClaim.Address;

		public IProduct Product => mLocalProduct;

		public MAC MAC => Product.MAC;

		public IDS_CAN_VERSION_NUMBER ProtocolVersion => Product.ProtocolVersion;

		public PRODUCT_ID ProductID => Product.ProductID;

		public byte ProductInstance => Product.ProductInstance;

		[field: CompilerGenerated]
		public DEVICE_TYPE DeviceType
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		} = DEVICE_TYPE.op_Implicit((byte)0);

		[field: CompilerGenerated]
		public int DeviceInstance
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public FUNCTION_NAME FunctionName
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		} = FUNCTION_NAME.UNKNOWN;

		[field: CompilerGenerated]
		public int FunctionInstance
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public CIRCUIT_ID CircuitID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public bool IsOnline => Address.IsValidDeviceAddress;

		public string SoftwarePartNumber => mLocalProduct.SoftwarePartNumber;

		[field: CompilerGenerated]
		public PAYLOAD DeviceStatus
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			protected set;
		}

		[field: CompilerGenerated]
		public LOCAL_DEVICE_OPTIONS Options
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public LocalTextConsole TextConsole
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		ITextConsole IDevice.TextConsole => TextConsole;

		public byte? DeviceCapabilities
		{
			get
			{
				return mDeviceCapabilities;
			}
			protected set
			{
				//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				if ((byte)ProtocolVersion <= 6)
				{
					if (value.HasValue)
					{
						throw new ArgumentException($"Failed to set IDS_CAN.LocalDevice().DeviceCapabilities = {value}.  DeviceCapabilities must be set to null when using IDS-CAN {ProtocolVersion}");
					}
				}
				else if (!value.HasValue)
				{
					throw new ArgumentException($"Failed to set IDS_CAN.LocalDevice().DeviceCapabilities = {value}.  DeviceCapabilities must be set to a valid value when using IDS-CAN {ProtocolVersion}");
				}
				mDeviceCapabilities = value;
			}
		}

		public bool IsNotAcceptingCommands
		{
			get
			{
				return _isNotAcceptingCommands;
			}
			protected set
			{
				if (_isNotAcceptingCommands != value)
				{
					_isNotAcceptingCommands = value;
					if (_isNotAcceptingCommands)
					{
						mSessionServer.CloseCommandSessions();
					}
				}
			}
		}

		[field: CompilerGenerated]
		public IEventPublisher Events
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public NETWORK_STATUS NetworkStatus
		{
			get
			{
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				byte b = 0;
				if (mSessionServer.IsAnySessionOpen)
				{
					b |= 4;
				}
				if (mInMotionLockoutManager != null)
				{
					IN_MOTION_LOCKOUT_LEVEL iN_MOTION_LOCKOUT_LEVEL = (mInMotionLockoutManager.IsInContention ? mInMotionLockoutManager.ProposedLevel : mInMotionLockoutManager.LockoutLevel);
					b |= (byte)(((byte)iN_MOTION_LOCKOUT_LEVEL & 3) << 3);
				}
				UInt48? val = mPidServer.ReadPidValue(PID.CLOUD_CAPABILITIES);
				if (val.HasValue && UInt48.op_Implicit(val.Value) != 0L)
				{
					b |= 0x40;
				}
				if (IsHazardousDevice)
				{
					b |= 0x80;
				}
				return b;
			}
		}

		public bool IsInMotionLockoutInContention => mInMotionLockoutManager?.IsInContention ?? false;

		public bool IsMuted => mMuteManager.IsMuted;

		public bool IsEnabled => mEnableDevice;

		public bool EnableDevice
		{
			get
			{
				return mEnableDevice;
			}
			set
			{
				mEnableDevice = value;
				AddressClaim.Enabled = EnableDevice && !IsMuted;
			}
		}

		protected virtual bool IsHazardousDevice => false;

		protected virtual IN_MOTION_LOCKOUT_LEVEL IsOkToClearInMotionLockout => (byte)0;

		private UInt48 SoftwareBuildDateTime => (((((((((UInt48.op_Implicit((byte)25) << 8) | UInt48.op_Implicit((byte)10)) << 8) | UInt48.op_Implicit((byte)21)) << 8) | UInt48.op_Implicit((byte)14)) << 8) | UInt48.op_Implicit((byte)9)) << 8) | UInt48.op_Implicit((byte)14);

		[AsyncStateMachine(typeof(<TransmitRequestAsync>d__2))]
		public async System.Threading.Tasks.Task<Tuple<RESPONSE, MessageBuffer>> TransmitRequestAsync(AsyncOperation operation, IBusEndpoint target, REQUEST request, PAYLOAD payload, Func<LocalDeviceRxEvent, RESPONSE?> validator)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (!target.IsOnline)
			{
				return Tuple.Create<RESPONSE, MessageBuffer>(RESPONSE.FAILED, (MessageBuffer)null);
			}
			object mutex = new object();
			TaskCompletionSource<RESPONSE> tcs = new TaskCompletionSource<RESPONSE>();
			MessageBuffer retained_message = null;
			bool message_sent = false;
			ReusableSubscription rx_listener = ReusableSubscriptionPool.Get();
			rx_listener.SetDelegate(this, delegate(LocalDeviceRxEvent rx)
			{
				if (!message_sent)
				{
					return false;
				}
				if (rx.SourceAddress != target.Address)
				{
					return false;
				}
				if ((byte)rx.MessageType != 129)
				{
					return false;
				}
				if (rx.MessageData != (byte)request)
				{
					return false;
				}
				if (validator != null)
				{
					lock (mutex)
					{
						RESPONSE? rESPONSE = validator?.Invoke(rx);
						if (rESPONSE.HasValue)
						{
							validator = null;
							if (rESPONSE == RESPONSE.SUCCESS)
							{
								MessageBuffer val = ResourcePool<MessageBuffer>.GetObject();
								val.CopyFrom((IMessage)(object)rx);
								retained_message = val;
							}
							tcs.SetResult(rESPONSE.Value);
							return true;
						}
					}
				}
				return false;
			});
			try
			{
				int delay = 500;
				while (IsOnline && target.IsOnline && !operation.IsCancellationRequested && !((System.Threading.Tasks.Task)tcs.Task).IsCompleted)
				{
					if (!Transmit29((byte)128, request, target, payload))
					{
						await System.Threading.Tasks.Task.Delay(5);
						continue;
					}
					message_sent = true;
					await System.Threading.Tasks.Task.WhenAny((System.Threading.Tasks.Task)tcs.Task, System.Threading.Tasks.Task.Delay(delay, operation.CancellationToken));
				}
			}
			catch (TimeoutException)
			{
			}
			catch (OperationCanceledException)
			{
			}
			finally
			{
				((Object)rx_listener).ReturnToPool();
				lock (mutex)
				{
					validator = null;
				}
			}
			if (retained_message != null)
			{
				return Tuple.Create<RESPONSE, MessageBuffer>(RESPONSE.SUCCESS, retained_message);
			}
			if (((System.Threading.Tasks.Task)tcs.Task).IsCompleted)
			{
				return Tuple.Create<RESPONSE, MessageBuffer>(tcs.Task.Result, (MessageBuffer)null);
			}
			if (operation.IsCancellationRequested)
			{
				return Tuple.Create<RESPONSE, MessageBuffer>(RESPONSE.CANCELLED, (MessageBuffer)null);
			}
			return Tuple.Create<RESPONSE, MessageBuffer>(RESPONSE.FAILED, (MessageBuffer)null);
		}

		private void InitBlockSupport()
		{
			mBlockClient = new BlockClient(this);
			mBlockServer = new BlockServer(this);
		}

		[AsyncStateMachine(typeof(<ReadBlockListAsync>d__6))]
		public async System.Threading.Tasks.Task<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>> ReadBlockListAsync(AsyncOperation operation, IDevice target)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await mBlockClient.ReadBlockListAsync(operation, target);
		}

		[AsyncStateMachine(typeof(<ReadBlockPropertiesAsync>d__7))]
		public async System.Threading.Tasks.Task<Tuple<RESPONSE, IBlock>> ReadBlockPropertiesAsync(AsyncOperation operation, IDevice target, BLOCK_ID block)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await mBlockClient.ReadBlockPropertiesAsync(operation, target, block);
		}

		[AsyncStateMachine(typeof(<RecalculateBlockCrcAsync>d__8))]
		public async System.Threading.Tasks.Task<Tuple<RESPONSE, uint?>> RecalculateBlockCrcAsync(AsyncOperation operation, IDevice target, BLOCK_ID block)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await mBlockClient.RecalculateBlockCrcAsync(operation, target, block);
		}

		[AsyncStateMachine(typeof(<ReadBlockDataAsync>d__9))]
		public async System.Threading.Tasks.Task<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> ReadBlockDataAsync(AsyncOperation operation, IDevice target, BLOCK_ID block, int bulk_xfer_delay_ms, ISessionClient session)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await mBlockClient.ReadBlockDataAsync(operation, target, block, bulk_xfer_delay_ms, session);
		}

		[AsyncStateMachine(typeof(<ReadBlockDataAsync>d__10))]
		public async System.Threading.Tasks.Task<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> ReadBlockDataAsync(AsyncOperation operation, IBlock block, int bulk_xfer_delay_ms, ISessionClient session)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await mBlockClient.ReadBlockDataAsync(operation, block, bulk_xfer_delay_ms, session);
		}

		[AsyncStateMachine(typeof(<WriteBlockDataAsync>d__11))]
		public async System.Threading.Tasks.Task<RESPONSE> WriteBlockDataAsync(AsyncOperation operation, IBlock block, System.Collections.Generic.IReadOnlyList<byte> data, int bulk_xfer_delay_ms, ISessionClient session)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await mBlockClient.WriteBlockDataAsync(operation, block, data, bulk_xfer_delay_ms, session);
		}

		protected void AddBlock(LocalBlock block)
		{
			mBlockServer.Add(block);
		}

		public LocalDevice(LocalProduct product, DEVICE_TYPE device_type, int device_instance, FUNCTION_NAME function_name, int function_instance, byte? capabilties, LOCAL_DEVICE_OPTIONS options)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Expected O, but got Unknown
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Expected O, but got Unknown
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Expected O, but got Unknown
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Expected O, but got Unknown
			mLocalProduct = product;
			Options = options;
			System.Collections.Generic.IEnumerator<ILocalDevice> enumerator = product.GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					ILocalDevice current = enumerator.Current;
					if (current.DeviceType == device_type && current.DeviceInstance == device_instance)
					{
						throw new ArgumentException($"Cannon construct LocalDevice({device_type} #{device_instance}), as it already exists within the LocalProduct");
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			if ((byte)ProtocolVersion <= 6)
			{
				if (capabilties.HasValue)
				{
					throw new ArgumentException($"Failed to construct IDS_CAN.Devices.LocalDevice(). DEVICE_ID.DeviceCapabilities = {ByteExtensions.HexString(capabilties.Value)}h.  DeviceCapabilities must be set to null when using IDS-CAN {ProtocolVersion}");
				}
			}
			else if (!capabilties.HasValue)
			{
				throw new ArgumentException($"Failed to construct IDS_CAN.Devices.LocalDevice(). DEVICE_ID.DeviceCapabilities = null.  DeviceCapabilities must be a valid value when using IDS-CAN {ProtocolVersion}");
			}
			DeviceType = device_type;
			DeviceInstance = device_instance;
			FunctionName = function_name;
			FunctionInstance = function_instance;
			mDeviceCapabilities = capabilties;
			IsNotAcceptingCommands = false;
			Events = (IEventPublisher)new EventPublisher($"LocalDevice[{DeviceType}].Events");
			((DisposableManager)this).AddDisposable((IDisposable)(object)Events);
			LocalDeviceOnlineEvent = new LocalDeviceOnlineEvent(this);
			LocalDeviceOfflineEvent = new LocalDeviceOfflineEvent(this);
			LocalDeviceRxEvent = new LocalDeviceRxEvent(this);
			Subscriptions = new SubscriptionManager();
			((DisposableManager)this).AddDisposable((IDisposable)(object)Subscriptions);
			InitRequestServer();
			AddressClaim = new AddressClaimManager(this);
			((DisposableManager)this).AddDisposable((IDisposable)(object)AddressClaim);
			mMuteManager = new MuteManager(this);
			InitSessionSupport();
			if (!((System.Enum)options).HasFlag((System.Enum)LOCAL_DEVICE_OPTIONS.IGNORE_IN_MOTION_LOCKOUT))
			{
				mInMotionLockoutManager = new InMotionLockoutManager(this);
			}
			InitPidSupport();
			InitBlockSupport();
			mLocalProduct.AddDevice(this);
			((IEventSender)Adapter).Events.Subscribe<AdapterOpenedEvent>((Action<AdapterOpenedEvent>)OnAdapterOpened, (SubscriptionType)0, Subscriptions);
			((IEventSender)Adapter).Events.Subscribe<AdapterClosedEvent>((Action<AdapterClosedEvent>)OnAdapterClosed, (SubscriptionType)0, Subscriptions);
			((IEventSender)Adapter).Events.Subscribe<AdapterRxEvent>((Action<AdapterRxEvent>)OnAdapterRx, (SubscriptionType)0, Subscriptions);
			PeriodicTask val = new PeriodicTask(new Action(BackgroundTask), TimeSpan.FromMilliseconds(40.0), TimeSpan.FromMilliseconds(500.0), (Type)0, true);
			((DisposableManager)this).AddDisposable((IDisposable)(object)val);
		}

		public override void Dispose(bool disposing)
		{
			((DisposableManager)this).Dispose(disposing);
			if (disposing)
			{
				EnableDevice = false;
				mLocalProduct.RemoveDevice(this);
				TextConsole = null;
			}
		}

		protected LocalTextConsole CreateTextConsole(TEXT_CONSOLE_SIZE size)
		{
			if (((Disposable)this).IsDisposed)
			{
				return null;
			}
			TextConsole = new LocalTextConsole(this, size);
			((DisposableManager)this).AddDisposable((IDisposable)(object)TextConsole);
			return TextConsole;
		}

		protected void IncreaseInMotionLockoutLevel(IN_MOTION_LOCKOUT_LEVEL level)
		{
			mInMotionLockoutManager?.IncreaseLockoutLevel(level);
		}

		private void OnAdapterOpened(AdapterOpenedEvent e)
		{
			if (!((Disposable)this).IsDisposed)
			{
				AddressClaim.OnAdapterOpened(e);
			}
		}

		private void OnAdapterClosed(AdapterClosedEvent e)
		{
			if (!((Disposable)this).IsDisposed)
			{
				AddressClaim.OnAdapterClosed(e);
			}
		}

		private void OnAdapterRx(AdapterRxEvent rx)
		{
			if (((Disposable)this).IsDisposed)
			{
				return;
			}
			RxMessagesReceived++;
			RxBytesReceived += rx.Count;
			AddressClaim.OnAdapterRx(rx);
			mInMotionLockoutManager?.OnAdapterRx(rx);
			if (rx.TargetAddress == Address || rx.TargetAddress == ADDRESS.BROADCAST)
			{
				OnLocalDeviceRxEvent(rx);
				if ((byte)rx.MessageType == 128)
				{
					mRequestServer.ProcessRequest(rx);
				}
				LocalDeviceRxEvent.Publish(rx);
			}
		}

		protected virtual void OnLocalDeviceRxEvent(AdapterRxEvent rx)
		{
		}

		private void OnAddressChanged(ADDRESS address, ADDRESS prev)
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed)
			{
				return;
			}
			if (address.IsValidDeviceAddress)
			{
				mLocalProduct.SuggestNewProductAddress(address);
				TxBytesSent = 0;
				TxMessagesSent = 0;
				RxBytesReceived = 0;
				RxMessagesReceived = 0;
				((Event)LocalDeviceOnlineEvent).Publish();
				((IEventSender)Adapter).Events.Publish<LocalDeviceOnlineEvent>(LocalDeviceOnlineEvent);
				ProductStatusMsgTime.ElapsedTime = PRODUCT_STATUS_MESSAGE_PERIOD;
				NetworkMsgTime.ElapsedTime = NETWORK_MESSAGE_PERIOD;
				CircuitIDMsgTime.ElapsedTime = CIRCUIT_ID_MESSAGE_PERIOD;
				DeviceIdMsgTime.ElapsedTime = DEVICE_ID_MESSAGE_PERIOD;
				DeviceStatusMsgTime.ElapsedTime = TimeSpan.FromDays(999.0);
			}
			else if (prev.IsValidDeviceAddress)
			{
				if (mLocalProduct.Address == prev)
				{
					mLocalProduct.ChooseNewProductAddress();
				}
				LocalDeviceOfflineEvent.Publish(prev);
				((IEventSender)Adapter).Events.Publish<LocalDeviceOfflineEvent>(LocalDeviceOfflineEvent);
			}
		}

		private void BackgroundTask()
		{
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed)
			{
				return;
			}
			OnBackgroundTask();
			mInMotionLockoutManager?.BackgroundTask();
			mMuteManager.BackgroundTask();
			AddressClaim.BackgroundTask();
			mSessionServer.BackgroundTask();
			if (!((IAdapter)Adapter).IsConnected || !IsOnline)
			{
				return;
			}
			if (Product.ProductInstance == (byte)Address && ProductStatusMsgTime.ElapsedTime >= PRODUCT_STATUS_MESSAGE_PERIOD && Transmit11((byte)6, PAYLOAD.FromArgs(new object[1] { (byte)Product.SoftwareUpdateState })))
			{
				ProductStatusMsgTime.Reset();
			}
			if (NetworkMsgTime.ElapsedTime >= NETWORK_MESSAGE_PERIOD && Transmit11((byte)0, PAYLOAD.FromArgs(new object[3]
			{
				(byte)NetworkStatus,
				(byte)ProtocolVersion,
				(UInt48)MAC
			})))
			{
				NetworkMsgTime.Reset();
			}
			if (DeviceIdMsgTime.ElapsedTime >= DEVICE_ID_MESSAGE_PERIOD)
			{
				DEVICE_ID deviceID = this.GetDeviceID();
				if (deviceID.ProductInstance != 0)
				{
					bool flag = false;
					if ((!deviceID.DeviceCapabilities.HasValue) ? Transmit11((byte)2, PAYLOAD.FromArgs(new object[5]
					{
						PRODUCT_ID.op_Implicit(deviceID.ProductID),
						deviceID.ProductInstance,
						DEVICE_TYPE.op_Implicit(deviceID.DeviceType),
						FUNCTION_NAME.op_Implicit(deviceID.FunctionName),
						(byte)((deviceID.DeviceInstance << 4) | (deviceID.FunctionInstance & 0xF))
					})) : Transmit11((byte)2, PAYLOAD.FromArgs(new object[6]
					{
						PRODUCT_ID.op_Implicit(deviceID.ProductID),
						deviceID.ProductInstance,
						DEVICE_TYPE.op_Implicit(deviceID.DeviceType),
						FUNCTION_NAME.op_Implicit(deviceID.FunctionName),
						(byte)((deviceID.DeviceInstance << 4) | (deviceID.FunctionInstance & 0xF)),
						deviceID.DeviceCapabilities.Value
					})))
					{
						DeviceIdMsgTime.Reset();
					}
				}
			}
			if (CircuitIDMsgTime.ElapsedTime >= CIRCUIT_ID_MESSAGE_PERIOD)
			{
				CIRCUIT_ID circuitID = CircuitID;
				if (Transmit11((byte)1, PAYLOAD.FromArgs(new object[1] { (uint)circuitID })))
				{
					CircuitIDMsgTime.Reset();
				}
			}
			PAYLOAD deviceStatus = DeviceStatus;
			PAYLOAD? lastDeviceStatusTx = LastDeviceStatusTx;
			TimeSpan val = ((!lastDeviceStatusTx.HasValue || !(deviceStatus == lastDeviceStatusTx.GetValueOrDefault())) ? (mSessionServer.IsAnySessionOpen ? TimeSpan.FromMilliseconds(50.0) : TimeSpan.FromMilliseconds(333.0)) : (mSessionServer.IsAnySessionOpen ? TimeSpan.FromMilliseconds(250.0) : TimeSpan.FromMilliseconds(1000.0)));
			if (DeviceStatusMsgTime.ElapsedTime >= val)
			{
				PAYLOAD deviceStatus2 = DeviceStatus;
				if (Transmit11((byte)3, deviceStatus2))
				{
					LastDeviceStatusTx = deviceStatus2;
					DeviceStatusMsgTime.Reset();
				}
			}
		}

		protected virtual void OnBackgroundTask()
		{
		}

		private bool Transmit(ID id, PAYLOAD payload = default(PAYLOAD))
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			if (!((IAdapter<MessageBuffer>)Adapter).Transmit(id, payload))
			{
				return false;
			}
			TxMessagesSent++;
			TxBytesSent += ((PAYLOAD)(ref payload)).Length;
			return true;
		}

		public bool Transmit11(MESSAGE_TYPE type, PAYLOAD payload = default(PAYLOAD))
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			if (!type.IsBroadcast)
			{
				throw new ArgumentException("MESSAGE_TYPE parameter must be 11-bit broadcast type");
			}
			if (!IsOnline)
			{
				return false;
			}
			return Transmit(new CAN_ID(type, Address), payload);
		}

		public bool Transmit29(MESSAGE_TYPE type, byte ext_data, IBusEndpoint target, PAYLOAD payload = default(PAYLOAD))
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (target.Adapter != Adapter)
			{
				throw new InvalidOperationException("Cannot transmit message between two IBusEndpoints on different adapters");
			}
			return Transmit29(type, ext_data, target.Address, payload);
		}

		public bool Transmit29(MESSAGE_TYPE type, byte ext_data, ADDRESS target, PAYLOAD payload = default(PAYLOAD))
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			if (target == null || !target.IsValidAddress)
			{
				return false;
			}
			if (!type.IsPointToPoint)
			{
				throw new ArgumentException("MESSAGE_TYPE parameter must be 11-bit broadcast type");
			}
			if (!IsOnline)
			{
				return false;
			}
			return Transmit(new CAN_ID(type, Address, target, ext_data), payload);
		}

		private void InitPidSupport()
		{
			mPidClient = new PidClient(this);
			mPidServer = new PidServer(this);
			AddPID(PID.CAN_ADAPTER_MAC, (Func<UInt48>)([CompilerGenerated] () => MAC));
			AddPID(PID.IDS_CAN_CIRCUIT_ID, [CompilerGenerated] () => UInt48.op_Implicit((uint)CircuitID), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				CircuitID = (uint)arg;
			});
			AddPID(PID.IDS_CAN_FUNCTION_NAME, [CompilerGenerated] () => UInt48.op_Implicit(FUNCTION_NAME.op_Implicit(FunctionName)), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				FunctionName = FUNCTION_NAME.op_Implicit((ushort)arg);
			});
			AddPID(PID.IDS_CAN_FUNCTION_INSTANCE, [CompilerGenerated] () => UInt48.op_Implicit((byte)FunctionInstance), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0004: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				FunctionInstance = (int)(arg & UInt48.op_Implicit((byte)15));
			});
			AddPID(PID.IDS_CAN_NUM_DEVICES_ON_NETWORK, (Func<UInt48>)([CompilerGenerated] () => UInt48.op_Implicit((uint)Adapter.Devices.NumDevicesDetectedOnNetwork)));
			AddPID(PID.CAN_BYTES_TX, (Func<UInt48>)([CompilerGenerated] () => (UInt48)TxBytesSent));
			AddPID(PID.CAN_BYTES_RX, (Func<UInt48>)([CompilerGenerated] () => (UInt48)RxBytesReceived));
			AddPID(PID.CAN_MESSAGES_TX, (Func<UInt48>)([CompilerGenerated] () => (UInt48)TxMessagesSent));
			AddPID(PID.CAN_MESSAGES_RX, (Func<UInt48>)([CompilerGenerated] () => (UInt48)RxMessagesReceived));
			AddPID(PID.SYSTEM_UPTIME_MS, (Func<UInt48>)([CompilerGenerated] () =>
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				TimeSpan elapsedTime = Uptime.ElapsedTime;
				return (UInt48)((TimeSpan)(ref elapsedTime)).TotalMilliseconds;
			}));
			AddPID(PID.TIME_ZONE, (Func<UInt48>)([CompilerGenerated] () => UInt48.op_Implicit(Adapter.Clock.TIME_ZONE)));
			AddPID(PID.RTC_TIME_SEC, [CompilerGenerated] () => UInt48.op_Implicit(Adapter.Clock.RTC_TIME_SEC), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Adapter.Clock.RTC_TIME_SEC = (byte)arg;
			});
			AddPID(PID.RTC_TIME_MIN, [CompilerGenerated] () => UInt48.op_Implicit(Adapter.Clock.RTC_TIME_MIN), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Adapter.Clock.RTC_TIME_MIN = (byte)arg;
			});
			AddPID(PID.RTC_TIME_HOUR, [CompilerGenerated] () => UInt48.op_Implicit(Adapter.Clock.RTC_TIME_HOUR), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Adapter.Clock.RTC_TIME_HOUR = (byte)arg;
			});
			AddPID(PID.RTC_TIME_DAY, [CompilerGenerated] () => UInt48.op_Implicit(Adapter.Clock.RTC_TIME_DAY), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Adapter.Clock.RTC_TIME_DAY = (byte)arg;
			});
			AddPID(PID.RTC_TIME_MONTH, [CompilerGenerated] () => UInt48.op_Implicit(Adapter.Clock.RTC_TIME_MONTH), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Adapter.Clock.RTC_TIME_MONTH = (byte)arg;
			});
			AddPID(PID.RTC_TIME_YEAR, [CompilerGenerated] () => UInt48.op_Implicit(Adapter.Clock.RTC_TIME_YEAR), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Adapter.Clock.RTC_TIME_YEAR = (ushort)arg;
			});
			AddPID(PID.RTC_EPOCH_SEC, [CompilerGenerated] () => UInt48.op_Implicit(Adapter.Clock.RTC_EPOCH_SEC), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Adapter.Clock.RTC_EPOCH_SEC = (uint)arg;
			});
			AddPID(PID.RTC_SET_TIME_SEC, (Func<UInt48>)([CompilerGenerated] () => UInt48.op_Implicit(Adapter.Clock.RTC_SET_TIME_SEC)));
			AddPID(PID.SOFTWARE_BUILD_DATE_TIME, (Func<UInt48>)([CompilerGenerated] () => SoftwareBuildDateTime));
		}

		[AsyncStateMachine(typeof(<ReadPidListAsync>d__147))]
		public async System.Threading.Tasks.Task<Tuple<RESPONSE?, PidList>> ReadPidListAsync(AsyncOperation operation, IDevice target)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await mPidClient.ReadPidListAsync(operation, target);
		}

		[AsyncStateMachine(typeof(<ReadPidAsync>d__148))]
		public async System.Threading.Tasks.Task<Tuple<RESPONSE?, UInt48?>> ReadPidAsync(AsyncOperation operation, IDevice target, PID id)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await mPidClient.ReadPidAsync(operation, target, id);
		}

		[AsyncStateMachine(typeof(<ReadPidAsync>d__149))]
		public async System.Threading.Tasks.Task<Tuple<RESPONSE?, UInt48?>> ReadPidAsync(AsyncOperation operation, IDevice target, PID id, bool withadd, ushort add)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await mPidClient.ReadPidAsync(operation, target, id, withadd, add);
		}

		[AsyncStateMachine(typeof(<ReadPidAsync>d__150))]
		public async System.Threading.Tasks.Task<Tuple<RESPONSE?, UInt48?>> ReadPidAsync(AsyncOperation operation, PidInfo pidInfo)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return await mPidClient.ReadPidAsync(operation, pidInfo);
		}

		[AsyncStateMachine(typeof(<WritePidAsync>d__151))]
		public async System.Threading.Tasks.Task<Tuple<RESPONSE?, UInt48?>> WritePidAsync(AsyncOperation operation, IDevice target, PID id, UInt48 value, ISessionClient session)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			return await mPidClient.WritePidAsync(operation, target, id, value, session);
		}

		[AsyncStateMachine(typeof(<WritePidAsync>d__152))]
		public async System.Threading.Tasks.Task<Tuple<RESPONSE?, Int48?>> WritePidAsync(AsyncOperation operation, IDevice target, PID id, Int48 value, ISessionClient session)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			Tuple<RESPONSE?, UInt48?> val = await mPidClient.WritePidAsync(operation, target, id, (UInt48)Int48.op_Implicit(value), session);
			if (!val.Item2.HasValue)
			{
				return Tuple.Create<RESPONSE?, Int48?>(val.Item1, (Int48?)null);
			}
			return Tuple.Create<RESPONSE?, Int48?>(val.Item1, (Int48?)(Int48)UInt48.op_Implicit(val.Item2.Value));
		}

		protected void AddPID(PID id, Func<UInt48> read_delegate)
		{
			mPidServer.Add(id, read_delegate, null);
		}

		protected void AddPID(PID id, Action<UInt48> write_delegate)
		{
			mPidServer.Add(id, null, write_delegate);
		}

		protected void AddPID(PID id, Func<UInt48> read_delgate, Action<UInt48> write_delegate)
		{
			mPidServer.Add(id, read_delgate, write_delegate);
		}

		private void InitRequestServer()
		{
			mRequestServer = new RequestServer(this);
		}

		private void InitSessionSupport()
		{
			mSessionServer = new SessionServer(this);
			((DisposableManager)this).AddDisposable((IDisposable)(object)mSessionServer);
			System.Collections.Generic.IEnumerator<SESSION_ID> enumerator = SESSION_ID.GetEnumerator().GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					SESSION_ID current = enumerator.Current;
					mSessionServer.AddSessionSupport(current);
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		protected ISession AddLocalSessionSupport(SESSION_ID session)
		{
			return mSessionServer.AddSessionSupport(session);
		}

		protected ISession GetLocalSession(SESSION_ID id)
		{
			return mSessionServer[id];
		}

		protected ADDRESS GetLocalSessionClientAddress(SESSION_ID id)
		{
			ISession localSession = GetLocalSession(id);
			if (localSession != null && localSession.IsOpen)
			{
				return localSession.Client.Address;
			}
			return ADDRESS.INVALID;
		}
	}
	[Flags]
	public enum BLOCK_FLAGS : byte
	{
		NONE = 0,
		READABLE = 1,
		WRITABLE = 2,
		USE_BUFFER_MINIMUM_SIZE = 4,
		USE_SET_START_ADDRESS = 8,
		USE_SET_SIZE = 0x10,
		USE_ERASE_DISABLED = 0x20
	}
	public interface IBlock
	{
		IDevice Device { get; }

		BLOCK_ID ID { get; }

		BLOCK_FLAGS Flags { get; }

		ulong Capacity { get; }

		ulong Size { get; }

		uint CRC { get; set; }

		ulong StartAddress { get; set; }

		ulong SetSize { get; set; }

		SESSION_ID ReadSessionID { get; }

		SESSION_ID WriteSessionID { get; }
	}
	public static class IBlockExtensions
	{
		public static bool IsReadable(this IBlock block)
		{
			return block.ReadSessionID != null;
		}

		public static bool IsWritable(this IBlock block)
		{
			return block.WriteSessionID != null;
		}
	}
	internal class SetBlock : IBlock
	{
		[field: CompilerGenerated]
		public IDevice Device
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public BLOCK_ID ID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public BLOCK_FLAGS Flags
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public ulong Capacity
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public ulong Size
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public uint CRC
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ulong StartAddress
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ulong SetSize
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public SESSION_ID ReadSessionID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public SESSION_ID WriteSessionID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public SetBlock(IDevice device, BLOCK_ID id, BLOCK_FLAGS flags, SESSION_ID read_id, SESSION_ID write_id, ulong capacity, ulong size, uint crc, ulong startaddress)
		{
			Device = device;
			ID = id;
			Flags = flags;
			ReadSessionID = read_id;
			WriteSessionID = write_id;
			Capacity = capacity;
			Size = size;
			CRC = crc;
			StartAddress = startaddress;
		}
	}
	public interface IBlockClient
	{
		System.Threading.Tasks.Task<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<BLOCK_ID>>> ReadBlockListAsync(AsyncOperation operation, IDevice target);

		System.Threading.Tasks.Task<Tuple<RESPONSE, IBlock>> ReadBlockPropertiesAsync(AsyncOperation operation, IDevice target, BLOCK_ID block);

		System.Threading.Tasks.Task<Tuple<RESPONSE, uint?>> RecalculateBlockCrcAsync(AsyncOperation operation, IDevice target, BLOCK_ID block);

		System.Threading.Tasks.Task<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> ReadBlockDataAsync(AsyncOperation operation, IDevice target, BLOCK_ID block, int bulk_xfer_delay_ms, ISessionClient session);

		System.Threading.Tasks.Task<Tuple<RESPONSE, System.Collections.Generic.IReadOnlyList<byte>>> ReadBlockDataAsync(AsyncOperation operation, IBlock block, int bulk_xfer_delay_ms, ISessionClient session);

		System.Threading.Tasks.Task<RESPONSE> WriteBlockDataAsync(AsyncOperation operation, IBlock block, System.Collections.Generic.IReadOnlyList<byte> data, int bulk_xfer_delay_ms, ISessionClient session);
	}
	public interface ILocalBlock : IBlock
	{
		new LocalDevice Device { get; }

		System.Collections.Generic.IReadOnlyList<byte> Data { get; }

		bool WriteData(System.Collections.Generic.IReadOnlyList<byte> data);
	}
	[Flags]
	public enum LOCAL_DEVICE_OPTIONS : uint
	{
		NONE = 0u,
		IGNORE_IN_MOTION_LOCKOUT = 1u
	}
	public interface ILocalDevice : IDevice, IBusEndpoint, IUniqueDeviceInfo, IUniqueProductInfo, ILocalDeviceAsyncMessaging, IPidClient, IBlockClient, IEventSender, IDisposableManager, IDisposable, System.IDisposable
	{
		bool IsMuted { get; }

		bool IsEnabled { get; }

		LOCAL_DEVICE_OPTIONS Options { get; }

		new LocalTextConsole TextConsole { get; }

		bool IsInMotionLockoutInContention { get; }

		bool Transmit11(MESSAGE_TYPE type, PAYLOAD payload = default(PAYLOAD));

		bool Transmit29(MESSAGE_TYPE type, byte ext_data, IBusEndpoint target, PAYLOAD payload = default(PAYLOAD));

		bool Transmit29(MESSAGE_TYPE type, byte ext_data, ADDRESS target, PAYLOAD payload = default(PAYLOAD));
	}
	public class PidInfo
	{
		public readonly IDevice Device;

		public readonly PID ID;

		public ushort PID_Address;

		public readonly byte Flags;

		public bool IsReadable => (Flags & 1) != 0;

		public bool IsWritable => (Flags & 2) != 0;

		public bool IsNonVolatile => (Flags & 4) != 0;

		public bool IsWithAddress => (Flags & 8) != 0;

		public string Name => ID.Name;

		public PidInfo(IDevice device, PID id, byte flags)
		{
			Device = device;
			ID = id;
			Flags = flags;
		}
	}
	[DefaultMember("Item")]
	public class PidList : System.Collections.Generic.IReadOnlyCollection<PidInfo>, System.Collections.Generic.IEnumerable<PidInfo>, System.Collections.IEnumerable
	{
		public readonly IDevice Device;

		private readonly Dictionary<ushort, PidInfo> Dict = new Dictionary<ushort, PidInfo>();

		public int Count => Dict.Count;

		public PidInfo this[PID id]
		{
			get
			{
				PidInfo result = default(PidInfo);
				if (Dict.TryGetValue(id.Value, ref result))
				{
					return result;
				}
				return null;
			}
		}

		public System.Collections.Generic.IEnumerator<PidInfo> GetEnumerator()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return (System.Collections.Generic.IEnumerator<PidInfo>)(object)Dict.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return (System.Collections.IEnumerator)(object)Dict.Values.GetEnumerator();
		}

		public PidList(IDevice device, System.Collections.Generic.IEnumerable<PidInfo> list)
		{
			Device = device;
			System.Collections.Generic.IEnumerator<PidInfo> enumerator = list.GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					PidInfo current = enumerator.Current;
					Dict.Add(current.ID.Value, current);
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		public bool Contains(PID id)
		{
			return Dict.ContainsKey(id.Value);
		}
	}
	public interface IPidClient
	{
		System.Threading.Tasks.Task<Tuple<RESPONSE?, PidList>> ReadPidListAsync(AsyncOperation operation, IDevice tgtDevice);

		System.Threading.Tasks.Task<Tuple<RESPONSE?, UInt48?>> ReadPidAsync(AsyncOperation operation, IDevice tgtDevice, PID id);

		System.Threading.Tasks.Task<Tuple<RESPONSE?, UInt48?>> ReadPidAsync(AsyncOperation operation, IDevice tgtDevice, PID id, bool withadd, ushort add);

		System.Threading.Tasks.Task<Tuple<RESPONSE?, UInt48?>> ReadPidAsync(AsyncOperation operation, PidInfo pidInfo);

		System.Threading.Tasks.Task<Tuple<RESPONSE?, UInt48?>> WritePidAsync(AsyncOperation operation, IDevice tgtDevice, PID id, UInt48 value, ISessionClient session);
	}
	public class LocalProduct : Disposable, IProduct, IBusEndpoint, IUniqueProductInfo, System.Collections.Generic.IEnumerable<IDevice>, System.Collections.IEnumerable, System.Collections.Generic.IEnumerable<ILocalDevice>
	{
		private static readonly TimeSpan SOFTWARE_UPDATE_TIMEOUT = TimeSpan.FromSeconds(60.0);

		private ConcurrentDictionary<ulong, LocalDevice> Devices = new ConcurrentDictionary<ulong, LocalDevice>();

		public bool IsSoftwareUpdateAvailable;

		public readonly Timer SoftwareUpdateAuthorizeTimer = new Timer(true);

		[field: CompilerGenerated]
		public string Name
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public string UniqueName
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IAdapter Adapter
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public PRODUCT_ID ProductID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IDS_CAN_VERSION_NUMBER ProtocolVersion
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public byte ProductInstance => Address;

		[field: CompilerGenerated]
		public ADDRESS Address
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		} = ADDRESS.BROADCAST;

		public int AssemblyPartNumber => ProductID.AssemblyPartNumber;

		[field: CompilerGenerated]
		public MAC MAC
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public int DeviceCount => Devices.Count;

		public bool IsOnline => ProductInstance != 0;

		[field: CompilerGenerated]
		public string SoftwarePartNumber
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public SOFTWARE_UPDATE_STATE SoftwareUpdateState
		{
			get
			{
				if (IsSoftwareUpdateAvailable)
				{
					return (byte)((!IsSoftwareUpdateAuthorized) ? 1 : 2);
				}
				return (byte)0;
			}
		}

		public bool IsSoftwareUpdateAuthorized
		{
			get
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				if (SoftwareUpdateAuthorizeTimer.IsRunning)
				{
					if (SoftwareUpdateAuthorizeTimer.ElapsedTime <= SOFTWARE_UPDATE_TIMEOUT)
					{
						return true;
					}
					SoftwareUpdateAuthorizeTimer.Stop();
				}
				return false;
			}
			set
			{
				if (value)
				{
					SoftwareUpdateAuthorizeTimer.Reset();
				}
				else
				{
					SoftwareUpdateAuthorizeTimer.Stop();
				}
			}
		}

		public System.Collections.Generic.IEnumerator<ILocalDevice> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerator<ILocalDevice>)((System.Collections.Generic.IEnumerable<LocalDevice>)Devices.Values).GetEnumerator();
		}

		System.Collections.Generic.IEnumerator<IDevice> System.Collections.Generic.IEnumerable<IDevice>.GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerator<IDevice>)GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator();
		}

		public LocalProduct(IAdapter adapter, MAC mac, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER protocol_version, string software_part_number)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			if ((PhysicalAddress)(object)mac == (PhysicalAddress)null)
			{
				mac = new MAC();
				((PhysicalAddress)mac).SetRandomMACValue();
			}
			Adapter = adapter;
			MAC = mac;
			ProductID = product_id;
			ProtocolVersion = protocol_version;
			SoftwarePartNumber = software_part_number;
			Name = ((object)ProductID).ToString();
			UniqueName = $"{ProductID} MAC[{MAC}]";
			SoftwareUpdateAuthorizeTimer.Stop();
			if (adapter.LocalProducts is LocalProductManager localProductManager)
			{
				localProductManager.Add(this);
			}
		}

		public override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			if (Adapter?.LocalProducts is LocalProductManager localProductManager)
			{
				localProductManager.Remove(this);
			}
			System.Collections.Generic.IEnumerator<ILocalDevice> enumerator = GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					((System.IDisposable)enumerator.Current)?.Dispose();
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			Devices.Clear();
			Devices = null;
			Adapter = null;
		}

		public LocalDevice CreateDevice(DEVICE_TYPE device_type, int device_instance, FUNCTION_NAME function_name, int function_instance, byte? capabilties, LOCAL_DEVICE_OPTIONS options)
		{
			return new LocalDevice(this, device_type, device_instance, function_name, function_instance, capabilties, options);
		}

		public override string ToString()
		{
			return Name;
		}

		public void EnableAllDevices()
		{
			System.Collections.Generic.IEnumerator<LocalDevice> enumerator = ((System.Collections.Generic.IEnumerable<LocalDevice>)Devices.Values).GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					enumerator.Current.EnableDevice = true;
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		public void DisableAllDevices()
		{
			System.Collections.Generic.IEnumerator<LocalDevice> enumerator = ((System.Collections.Generic.IEnumerable<LocalDevice>)Devices.Values).GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					enumerator.Current.EnableDevice = false;
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		public void AddDevice(LocalDevice device)
		{
			Devices.TryAdd(device.GetDeviceUniqueID(), device);
		}

		public void RemoveDevice(LocalDevice device)
		{
			LocalDevice localDevice = default(LocalDevice);
			Devices.TryRemove(device.GetDeviceUniqueID(), ref localDevice);
		}

		internal void ChooseNewProductAddress()
		{
			System.Collections.Generic.IEnumerator<ILocalDevice> enumerator = GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					ADDRESS address = enumerator.Current.Address;
					if (address.IsValidDeviceAddress)
					{
						Address = address;
						return;
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			Address = ADDRESS.BROADCAST;
		}

		internal void SuggestNewProductAddress(ADDRESS address)
		{
			if (address.IsValidDeviceAddress && !Address.IsValidDeviceAddress)
			{
				Address = address;
			}
		}
	}
	public interface ILocalProductManager : System.Collections.Generic.IEnumerable<LocalProduct>, System.Collections.IEnumerable
	{
		LocalProduct GetProductAtAddress(ADDRESS address);
	}
	internal class LocalProductManager : Disposable, ILocalProductManager, System.Collections.Generic.IEnumerable<LocalProduct>, System.Collections.IEnumerable
	{
		private Adapter Adapter;

		private ConcurrentDictionary<ulong, LocalProduct> Products = new ConcurrentDictionary<ulong, LocalProduct>();

		public System.Collections.Generic.IEnumerator<LocalProduct> GetEnumerator()
		{
			return ((System.Collections.Generic.IEnumerable<LocalProduct>)Products.Values).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator();
		}

		public LocalProductManager(Adapter adapter)
		{
			Adapter = adapter;
		}

		public override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			Adapter = null;
			ConcurrentDictionary<ulong, LocalProduct> val = Interlocked.Exchange<ConcurrentDictionary<ulong, LocalProduct>>(ref Products, (ConcurrentDictionary<ulong, LocalProduct>)null);
			System.Collections.Generic.IEnumerator<LocalProduct> enumerator = ((System.Collections.Generic.IEnumerable<LocalProduct>)(val?.Values)).GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					LocalProduct current = enumerator.Current;
					if (current != null)
					{
						((Disposable)current).Dispose();
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			val?.Clear();
		}

		public LocalProduct GetProductAtAddress(ADDRESS address)
		{
			System.Collections.Generic.IEnumerator<LocalProduct> enumerator = GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					LocalProduct current = enumerator.Current;
					if (current?.Address == address)
					{
						return current;
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			return null;
		}

		public void Add(LocalProduct product)
		{
			if (product?.Adapter == Adapter)
			{
				Products?.TryAdd(product.GetProductUniqueID(), product);
			}
		}

		public void Remove(LocalProduct product)
		{
			LocalProduct localProduct = default(LocalProduct);
			Products?.TryRemove(product.GetProductUniqueID(), ref localProduct);
		}
	}
	public class MAC : PhysicalAddress
	{
		public MAC()
			: base(6)
		{
		}

		public MAC(byte[] buffer)
			: base(6)
		{
			((PhysicalAddress)this).CopyFrom(buffer);
		}

		public MAC(IPhysicalAddress mac)
			: base(6)
		{
			((PhysicalAddress)this).CopyFrom(mac);
		}

		public MAC(MAC mac)
			: base(6)
		{
			((PhysicalAddress)this).CopyFrom((IPhysicalAddress)(object)mac);
		}

		public MAC(UInt48 value)
			: base(6)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			base.Buffer[0] = (byte)(value >> 40);
			base.Buffer[1] = (byte)(value >> 32);
			base.Buffer[2] = (byte)(value >> 24);
			base.Buffer[3] = (byte)(value >> 16);
			base.Buffer[4] = (byte)(value >> 8);
			base.Buffer[5] = (byte)(value >> 0);
		}

		public bool UnloadFromMessage(AdapterRxEvent rx)
		{
			if ((byte)rx.MessageType == 0 && rx.Count == 8)
			{
				for (int i = 0; i < base.Buffer.Length; i++)
				{
					base.Buffer[i] = rx[i + 2];
				}
				return true;
			}
			return false;
		}

		public static implicit operator UInt48(MAC a)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			return (((((((((UInt48.op_Implicit(((PhysicalAddress)a)[0]) << 8) | UInt48.op_Implicit(((PhysicalAddress)a)[1])) << 8) | UInt48.op_Implicit(((PhysicalAddress)a)[2])) << 8) | UInt48.op_Implicit(((PhysicalAddress)a)[3])) << 8) | UInt48.op_Implicit(((PhysicalAddress)a)[4])) << 8) | UInt48.op_Implicit(((PhysicalAddress)a)[5]);
		}
	}
	internal struct TIME_MESSAGE_PAYLOAD
	{
		[field: CompilerGenerated]
		public uint Epoch
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ushort TimeSinceClockSet
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public byte TimeZone
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public PAYLOAD Payload
		{
			private get
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				return PAYLOAD.FromArgs(new object[4]
				{
					Epoch,
					TimeSinceClockSet,
					TimeZone,
					(byte)0
				});
			}
			set
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				if (((PAYLOAD)(ref value)).Length != 8)
				{
					throw new ArgumentOutOfRangeException("Payload.Length must be 8 bytes");
				}
				Epoch = CommExtensions.GetUINT32((IByteList)(object)value, 0);
				TimeSinceClockSet = CommExtensions.GetUINT16((IByteList)(object)value, 4);
				TimeZone = ((PAYLOAD)(ref value))[6];
			}
		}

		public TIME_MESSAGE_PAYLOAD(uint epoch, ushort time_since_clock_set, byte time_zone)
		{
			Epoch = epoch;
			TimeSinceClockSet = time_since_clock_set;
			TimeZone = time_zone;
		}

		public TIME_MESSAGE_PAYLOAD(PAYLOAD payload)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (((PAYLOAD)(ref payload)).Length != 8)
			{
				throw new ArgumentOutOfRangeException("payload.Length must be 8 bytes");
			}
			Epoch = CommExtensions.GetUINT32((IByteList)(object)payload, 0);
			TimeSinceClockSet = CommExtensions.GetUINT16((IByteList)(object)payload, 4);
			TimeZone = ((PAYLOAD)(ref payload))[6];
		}

		public static implicit operator PAYLOAD(TIME_MESSAGE_PAYLOAD p)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return p.Payload;
		}
	}
	public interface INetworkTime
	{
		bool IsValid { get; }

		bool IsTimeAuthority { get; }

		System.DateTime CurrentDateTime { get; set; }

		System.DateTime TimeLastSet { get; }

		byte TIME_ZONE { get; }

		byte RTC_TIME_SEC { get; set; }

		byte RTC_TIME_MIN { get; set; }

		byte RTC_TIME_HOUR { get; set; }

		byte RTC_TIME_DAY { get; set; }

		byte RTC_TIME_MONTH { get; set; }

		ushort RTC_TIME_YEAR { get; set; }

		uint RTC_EPOCH_SEC { get; set; }

		uint RTC_SET_TIME_SEC { get; }

		ushort TIME_SINCE_CLOCK_SET { get; }

		void SetTime(int year, int month, int day, int hour, int minute, int second);
	}
	public class NetworkTime : Adapter.BackgroundTaskObject, INetworkTime
	{
		private static readonly TimeSpan TIME_MESSAGE_TX_PERIOD = TimeSpan.FromSeconds(1.0);

		private static readonly TimeSpan MINIMUM_AUTHORITY_TIME = TimeSpan.FromSeconds(5.0);

		private Timer TimeSinceLastClockAuthoritySeen = new Timer(true);

		private Timer AuthorityTime = new Timer(true);

		private Timer TimeSinceLastTx = new Timer(true);

		private TimeSpan NetworkClockMissingTimeout = TimeSpan.FromSeconds(10.0);

		private uint LastTxEpoch;

		private Timer Stopwatch = new Timer(true);

		private static readonly System.DateTime Jan_1_2000 = new System.DateTime(2000, 1, 1, 0, 0, 0);

		[field: CompilerGenerated]
		public bool IsValid
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public bool IsTimeAuthority
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public System.DateTime CurrentDateTime
		{
			get
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				return Jan_1_2000 + TimeSpan.FromSeconds((double)RTC_EPOCH_SEC);
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				TimeSpan val = value - Jan_1_2000;
				RTC_EPOCH_SEC = (uint)((TimeSpan)(ref val)).TotalSeconds;
			}
		}

		public System.DateTime TimeLastSet => Jan_1_2000 + TimeSpan.FromSeconds((double)RTC_SET_TIME_SEC);

		[field: CompilerGenerated]
		public byte TIME_ZONE
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public byte RTC_TIME_SEC
		{
			get
			{
				return (byte)CurrentDateTime.Second;
			}
			set
			{
				SetTime(RTC_TIME_YEAR, RTC_TIME_MONTH, RTC_TIME_DAY, RTC_TIME_HOUR, RTC_TIME_MIN, value);
			}
		}

		public byte RTC_TIME_MIN
		{
			get
			{
				return (byte)CurrentDateTime.Minute;
			}
			set
			{
				SetTime(RTC_TIME_YEAR, RTC_TIME_MONTH, RTC_TIME_DAY, RTC_TIME_HOUR, value, RTC_TIME_SEC);
			}
		}

		public byte RTC_TIME_HOUR
		{
			get
			{
				return (byte)CurrentDateTime.Hour;
			}
			set
			{
				SetTime(RTC_TIME_YEAR, RTC_TIME_MONTH, RTC_TIME_DAY, value, RTC_TIME_MIN, RTC_TIME_SEC);
			}
		}

		public byte RTC_TIME_DAY
		{
			get
			{
				return (byte)CurrentDateTime.Day;
			}
			set
			{
				SetTime(RTC_TIME_YEAR, RTC_TIME_MONTH, value, RTC_TIME_HOUR, RTC_TIME_MIN, RTC_TIME_SEC);
			}
		}

		public byte RTC_TIME_MONTH
		{
			get
			{
				return (byte)CurrentDateTime.Month;
			}
			set
			{
				SetTime(RTC_TIME_YEAR, value, RTC_TIME_DAY, RTC_TIME_HOUR, RTC_TIME_MIN, RTC_TIME_SEC);
			}
		}

		public ushort RTC_TIME_YEAR
		{
			get
			{
				return (ushort)CurrentDateTime.Year;
			}
			set
			{
				SetTime(value, RTC_TIME_MONTH, RTC_TIME_DAY, RTC_TIME_HOUR, RTC_TIME_MIN, RTC_TIME_SEC);
			}
		}

		public uint RTC_EPOCH_SEC
		{
			get
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				uint rTC_SET_TIME_SEC = RTC_SET_TIME_SEC;
				TimeSpan elapsedTime = Stopwatch.ElapsedTime;
				return rTC_SET_TIME_SEC + (uint)((TimeSpan)(ref elapsedTime)).TotalSeconds;
			}
			set
			{
				RTC_SET_TIME_SEC = value;
				Stopwatch.Reset();
				IsValid = true;
				IsTimeAuthority = true;
				CalcRandomClockMissingTimeout();
				AuthorityTime.Reset();
			}
		}

		[field: CompilerGenerated]
		public uint RTC_SET_TIME_SEC
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public ushort TIME_SINCE_CLOCK_SET
		{
			get
			{
				if (!IsValid)
				{
					return 65535;
				}
				uint num = RTC_EPOCH_SEC - RTC_SET_TIME_SEC;
				if (num < 16384)
				{
					return (ushort)(0 | num);
				}
				uint num2 = num / 60;
				if (num2 < 16384)
				{
					return (ushort)(0x4000 | num2);
				}
				uint num3 = num2 / 60;
				if (num3 < 16384)
				{
					return (ushort)(0x8000 | num3);
				}
				uint num4 = num3 / 24;
				if (num4 < 16383)
				{
					return (ushort)(0xC000 | num4);
				}
				return 65534;
			}
		}

		public NetworkTime(Adapter adapter)
			: base(adapter)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			IsValid = false;
			IsTimeAuthority = false;
			TIME_ZONE = 0;
			CalcRandomClockMissingTimeout();
			((Adapter)adapter).Events.Subscribe<AdapterRxEvent>((Action<AdapterRxEvent>)OnAdapterRx, (SubscriptionType)0, base.Subscriptions);
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IsTimeAuthority = false;
				IsValid = false;
				((Disposable)base.Subscriptions).Dispose();
			}
		}

		private void CalcRandomClockMissingTimeout()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			NetworkClockMissingTimeout = TimeSpan.FromSeconds(5.0 + 5.0 * ThreadLocalRandom.NextDouble());
		}

		public void SetTime(int year, int month, int day, int hour, int minute, int second)
		{
			CurrentDateTime = new System.DateTime(year, month, day, hour, minute, second);
		}

		private void OnAdapterRx(AdapterRxEvent rx)
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed || (byte)rx.MessageType != 7 || rx.Count != 8)
			{
				return;
			}
			TimeSinceLastClockAuthoritySeen.Reset();
			ILocalDevice localHost = base.LocalHost;
			if ((localHost != null && localHost.IsOnline && rx.SourceAddress == base.LocalHost?.Address) || (IsTimeAuthority && AuthorityTime.ElapsedTime < MINIMUM_AUTHORITY_TIME))
			{
				return;
			}
			TIME_MESSAGE_PAYLOAD tIME_MESSAGE_PAYLOAD = new TIME_MESSAGE_PAYLOAD(rx.Payload);
			bool num = tIME_MESSAGE_PAYLOAD.TimeSinceClockSet <= TIME_SINCE_CLOCK_SET;
			bool flag = Math.Abs((double)tIME_MESSAGE_PAYLOAD.Epoch - (double)RTC_EPOCH_SEC) < 30.0;
			if (num || flag)
			{
				IsTimeAuthority = false;
				uint num2 = 1u;
				switch (tIME_MESSAGE_PAYLOAD.TimeSinceClockSet & 0xC000)
				{
				case 0:
					num2 = 1u;
					break;
				case 16384:
					num2 = 60u;
					break;
				case 32768:
					num2 = 3600u;
					break;
				case 49152:
					num2 = 86400u;
					break;
				}
				uint num3 = (uint)(((tIME_MESSAGE_PAYLOAD.TimeSinceClockSet & 0x3FFF) + 1) * (int)num2 - 1);
				uint num4 = tIME_MESSAGE_PAYLOAD.Epoch - num3;
				if (num4 > RTC_SET_TIME_SEC || tIME_MESSAGE_PAYLOAD.Epoch < RTC_SET_TIME_SEC)
				{
					RTC_SET_TIME_SEC = num4;
				}
				Stopwatch.ElapsedTime = TimeSpan.FromSeconds((double)(tIME_MESSAGE_PAYLOAD.Epoch - RTC_SET_TIME_SEC));
				TIME_ZONE = rx[6];
				IsValid = tIME_MESSAGE_PAYLOAD.TimeSinceClockSet != 65535;
			}
			else
			{
				IsTimeAuthority = true;
			}
		}

		public override void BackgroundTask()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			if (!IsTimeAuthority)
			{
				if (TimeSinceLastClockAuthoritySeen.ElapsedTime < NetworkClockMissingTimeout)
				{
					return;
				}
				IsTimeAuthority = true;
			}
			ILocalDevice localHost = base.LocalHost;
			if (localHost != null && localHost.IsOnline && (RTC_EPOCH_SEC != LastTxEpoch || TimeSinceLastTx.ElapsedTime >= TIME_MESSAGE_TX_PERIOD))
			{
				TIME_MESSAGE_PAYLOAD tIME_MESSAGE_PAYLOAD = new TIME_MESSAGE_PAYLOAD(RTC_EPOCH_SEC, TIME_SINCE_CLOCK_SET, TIME_ZONE);
				if (base.LocalHost.Transmit11((byte)7, tIME_MESSAGE_PAYLOAD))
				{
					LastTxEpoch = tIME_MESSAGE_PAYLOAD.Epoch;
					TimeSinceLastTx.Reset();
				}
			}
		}
	}
	public interface IDevicePID : IEventSender
	{
		PID ID { get; }

		byte Flags { get; }

		IRemoteDevice Device { get; }

		string Name { get; }

		bool IsReadable { get; }

		bool IsWritable { get; }

		bool IsNonVolatile { get; }

		bool IsWithAddress { get; }

		ulong Value { get; }

		uint Data { get; }

		ushort Address { get; }

		bool IsValueValid { get; }

		string ValueString { get; }

		bool RequestRead();

		bool RequestRead(ushort address);

		System.Threading.Tasks.Task<bool> ReadAsync(AsyncOperation obj);

		System.Threading.Tasks.Task<bool> ReadAsync(ushort address, AsyncOperation obj);

		System.Threading.Tasks.Task<bool> WriteAsync(ulong value, ISessionClient session, AsyncOperation obj);

		System.Threading.Tasks.Task<bool> WriteAsync(ushort address, uint data, ISessionClient session, AsyncOperation obj);
	}
	internal class DevicePID : Disposable, IDevicePID, IEventSender
	{
		private class AsyncSignal
		{
			public bool OperationComplete;
		}

		private enum PID_WRITE_STATE
		{
			OPEN_SESSION,
			WRITE,
			VERIFY
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ReadAsync>d__52 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public AsyncOperation operation;

			public DevicePID <>4__this;

			private AsyncSignal <signal>5__2;

			private Timer <tx_timer>5__3;

			private Timer <progress_timer>5__4;

			private bool <skip>5__5;

			private TimeSpan <ReadRequestTime>5__6;

			private TimeSpan <ReportProgressTime>5__7;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0162: Unknown result type (might be due to invalid IL or missing references)
				//IL_0167: Unknown result type (might be due to invalid IL or missing references)
				//IL_016f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Expected O, but got Unknown
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Expected O, but got Unknown
				//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0124: Unknown result type (might be due to invalid IL or missing references)
				//IL_0129: Unknown result type (might be due to invalid IL or missing references)
				//IL_012d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
				//IL_0147: Unknown result type (might be due to invalid IL or missing references)
				//IL_0149: Unknown result type (might be due to invalid IL or missing references)
				//IL_0218: Unknown result type (might be due to invalid IL or missing references)
				//IL_021e: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DevicePID devicePID = <>4__this;
				bool result;
				try
				{
					if (num != 0)
					{
						operation.ReportProgress(0f, "Reading...");
						<signal>5__2 = new AsyncSignal();
						if (devicePID.ReadSignals == null)
						{
							object obj = Lock;
							bool flag = false;
							try
							{
								Monitor.Enter(obj, ref flag);
								if (devicePID.ReadSignals == null)
								{
									devicePID.ReadSignals = new ConcurrentQueue<AsyncSignal>();
								}
							}
							finally
							{
								if (num < 0 && flag)
								{
									Monitor.Exit(obj);
								}
							}
						}
						devicePID.ReadSignals.Enqueue(<signal>5__2);
						<tx_timer>5__3 = new Timer(true);
						<progress_timer>5__4 = new Timer(true);
						<tx_timer>5__3.ElapsedTime = TimeSpan.FromSeconds(1.0);
						<progress_timer>5__4.ElapsedTime = TimeSpan.FromSeconds(1.0);
						<skip>5__5 = true;
						<ReadRequestTime>5__6 = TimeSpan.FromMilliseconds(330.0);
						<ReportProgressTime>5__7 = TimeSpan.FromMilliseconds(250.0);
						goto IL_024a;
					}
					ConfiguredTaskAwaiter val = <>u__1;
					<>u__1 = default(ConfiguredTaskAwaiter);
					num = (<>1__state = -1);
					goto IL_017e;
					IL_024a:
					if (!<signal>5__2.OperationComplete)
					{
						operation.ThrowIfCancellationRequested();
						if (<skip>5__5)
						{
							<skip>5__5 = false;
							goto IL_0185;
						}
						ConfiguredTaskAwaitable val2 = System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
						val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <ReadAsync>d__52>(ref val, ref this);
							return;
						}
						goto IL_017e;
					}
					operation.ReportProgress(100f, "Success!");
					result = true;
					goto end_IL_000e;
					IL_0185:
					if (((Disposable)devicePID).IsDisposed)
					{
						operation.ReportProgress("ReadAsync failed: PID is disposed");
						result = false;
					}
					else if (!devicePID.IsReadable)
					{
						operation.ReportProgress("ReadAsync failed: PID is not readable");
						result = false;
					}
					else
					{
						if (devicePID.Device.IsOnline)
						{
							if (<tx_timer>5__3.ElapsedTime >= <ReadRequestTime>5__6 && devicePID.RequestRead())
							{
								<tx_timer>5__3.Reset();
							}
							if (<progress_timer>5__4.ElapsedTime > <ReportProgressTime>5__7)
							{
								<progress_timer>5__4.Reset();
								operation.ReportProgress(0f, "Reading...");
							}
							goto IL_024a;
						}
						operation.ReportProgress("ReadAsync failed: Device is offline");
						result = false;
					}
					goto end_IL_000e;
					IL_017e:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
					goto IL_0185;
					end_IL_000e:;
				}
				catch (System.Exception exception)
				{
					<>1__state = -2;
					<signal>5__2 = null;
					<tx_timer>5__3 = null;
					<progress_timer>5__4 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<signal>5__2 = null;
				<tx_timer>5__3 = null;
				<progress_timer>5__4 = null;
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
		private struct <ReadAsync>d__53 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public DevicePID <>4__this;

			public AsyncOperation operation;

			public ushort address;

			private AsyncSignal <signal>5__2;

			private Timer <tx_timer>5__3;

			private Timer <progress_timer>5__4;

			private bool <skip>5__5;

			private TimeSpan <ReadRequestTime>5__6;

			private TimeSpan <ReportProgressTime>5__7;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0171: Unknown result type (might be due to invalid IL or missing references)
				//IL_0176: Unknown result type (might be due to invalid IL or missing references)
				//IL_017e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				//IL_009d: Expected O, but got Unknown
				//IL_009f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a9: Expected O, but got Unknown
				//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_0100: Unknown result type (might be due to invalid IL or missing references)
				//IL_0105: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0202: Unknown result type (might be due to invalid IL or missing references)
				//IL_0133: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				//IL_013c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0141: Unknown result type (might be due to invalid IL or missing references)
				//IL_022d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0233: Unknown result type (might be due to invalid IL or missing references)
				//IL_0156: Unknown result type (might be due to invalid IL or missing references)
				//IL_0158: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DevicePID devicePID = <>4__this;
				bool result;
				try
				{
					ConfiguredTaskAwaiter val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(ConfiguredTaskAwaiter);
						num = (<>1__state = -1);
						goto IL_018d;
					}
					if (devicePID.IsWithAddress)
					{
						operation.ReportProgress(0f, "Reading...");
						<signal>5__2 = new AsyncSignal();
						if (devicePID.ReadSignals == null)
						{
							object obj = Lock;
							bool flag = false;
							try
							{
								Monitor.Enter(obj, ref flag);
								if (devicePID.ReadSignals == null)
								{
									devicePID.ReadSignals = new ConcurrentQueue<AsyncSignal>();
								}
							}
							finally
							{
								if (num < 0 && flag)
								{
									Monitor.Exit(obj);
								}
							}
						}
						devicePID.ReadSignals.Enqueue(<signal>5__2);
						<tx_timer>5__3 = new Timer(true);
						<progress_timer>5__4 = new Timer(true);
						<tx_timer>5__3.ElapsedTime = TimeSpan.FromSeconds(1.0);
						<progress_timer>5__4.ElapsedTime = TimeSpan.FromSeconds(1.0);
						<skip>5__5 = true;
						<ReadRequestTime>5__6 = TimeSpan.FromMilliseconds(330.0);
						<ReportProgressTime>5__7 = TimeSpan.FromMilliseconds(250.0);
						goto IL_025f;
					}
					result = false;
					goto end_IL_000e;
					IL_025f:
					if (!<signal>5__2.OperationComplete)
					{
						operation.ThrowIfCancellationRequested();
						if (<skip>5__5)
						{
							<skip>5__5 = false;
							goto IL_0194;
						}
						ConfiguredTaskAwaitable val2 = System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
						val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <ReadAsync>d__53>(ref val, ref this);
							return;
						}
						goto IL_018d;
					}
					operation.ReportProgress(100f, "Success!");
					result = true;
					goto end_IL_000e;
					IL_018d:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
					goto IL_0194;
					IL_0194:
					if (((Disposable)devicePID).IsDisposed)
					{
						operation.ReportProgress("ReadAsync failed: PID is disposed");
						result = false;
					}
					else if (!devicePID.IsReadable)
					{
						operation.ReportProgress("ReadAsync failed: PID is not readable");
						result = false;
					}
					else
					{
						if (devicePID.Device.IsOnline)
						{
							if (<tx_timer>5__3.ElapsedTime >= <ReadRequestTime>5__6 && devicePID.RequestRead(address))
							{
								<tx_timer>5__3.Reset();
							}
							if (<progress_timer>5__4.ElapsedTime > <ReportProgressTime>5__7)
							{
								<progress_timer>5__4.Reset();
								operation.ReportProgress(0f, "Reading...");
							}
							goto IL_025f;
						}
						operation.ReportProgress("ReadAsync failed: Device is offline");
						result = false;
					}
					end_IL_000e:;
				}
				catch (System.Exception exception)
				{
					<>1__state = -2;
					<signal>5__2 = null;
					<tx_timer>5__3 = null;
					<progress_timer>5__4 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<signal>5__2 = null;
				<tx_timer>5__3 = null;
				<progress_timer>5__4 = null;
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
		private struct <WriteAsync>d__55 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public ISessionClient session;

			public AsyncOperation operation;

			public DevicePID <>4__this;

			public ulong value;

			private PID_WRITE_STATE <state>5__2;

			private Timer <StateTime>5__3;

			private Timer <tx_timer>5__4;

			private Timer <progress_timer>5__5;

			private bool <skip>5__6;

			private double <basepercent>5__7;

			private TimeSpan <retryTime>5__8;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_013d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0142: Unknown result type (might be due to invalid IL or missing references)
				//IL_014a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_007a: Expected O, but got Unknown
				//IL_007c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Expected O, but got Unknown
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Expected O, but got Unknown
				//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_0104: Unknown result type (might be due to invalid IL or missing references)
				//IL_0108: Unknown result type (might be due to invalid IL or missing references)
				//IL_010d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0122: Unknown result type (might be due to invalid IL or missing references)
				//IL_0124: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_0210: Unknown result type (might be due to invalid IL or missing references)
				//IL_0215: Unknown result type (might be due to invalid IL or missing references)
				//IL_0229: Unknown result type (might be due to invalid IL or missing references)
				//IL_022f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0312: Unknown result type (might be due to invalid IL or missing references)
				//IL_0318: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DevicePID devicePID = <>4__this;
				bool result;
				try
				{
					if (num != 0)
					{
						<state>5__2 = PID_WRITE_STATE.OPEN_SESSION;
						if (session != null)
						{
							operation.ReportProgress(0f, $"Opening {session.SessionID} session...");
						}
						<StateTime>5__3 = new Timer(true);
						<tx_timer>5__4 = new Timer(true);
						<progress_timer>5__5 = new Timer(true);
						<tx_timer>5__4.ElapsedTime = TimeSpan.FromSeconds(1.0);
						<skip>5__6 = true;
						<basepercent>5__7 = 0.0;
						float num2 = 0f;
						<retryTime>5__8 = TimeSpan.FromMilliseconds(250.0);
						goto IL_00db;
					}
					ConfiguredTaskAwaiter val = <>u__1;
					<>u__1 = default(ConfiguredTaskAwaiter);
					num = (<>1__state = -1);
					goto IL_0159;
					IL_00db:
					operation.ThrowIfCancellationRequested();
					if (<skip>5__6)
					{
						<skip>5__6 = false;
						goto IL_0160;
					}
					ConfiguredTaskAwaitable val2 = System.Threading.Tasks.Task.Delay(10).ConfigureAwait(false);
					val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
					if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val;
						<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <WriteAsync>d__55>(ref val, ref this);
						return;
					}
					goto IL_0159;
					IL_0160:
					if (((Disposable)devicePID).IsDisposed)
					{
						operation.ReportProgress("WritePIDAsync failed: object is disposed");
						result = false;
					}
					else if (!devicePID.Device.IsOnline)
					{
						operation.ReportProgress("Failed: Device went offline");
						result = false;
					}
					else
					{
						if (<state>5__2 != PID_WRITE_STATE.VERIFY || !devicePID.IsValueValid || devicePID.Value != value)
						{
							double num3 = <basepercent>5__7;
							double num4 = 100.0 - <basepercent>5__7;
							TimeSpan elapsedTime = <StateTime>5__3.ElapsedTime;
							double num5 = num4 * (double)((TimeSpan)(ref elapsedTime)).Ticks;
							elapsedTime = operation.ElapsedTime;
							float num2 = (float)(num3 + num5 / (double)((TimeSpan)(ref elapsedTime)).Ticks);
							if (<progress_timer>5__5.ElapsedTime > <retryTime>5__8)
							{
								operation.ReportProgress(num2, operation.Status);
								<progress_timer>5__5.Reset();
							}
							if (session != null)
							{
								session.TryOpenSession();
								if (!session.IsOpen)
								{
									goto IL_00db;
								}
							}
							if (<state>5__2 == PID_WRITE_STATE.OPEN_SESSION)
							{
								<state>5__2 = PID_WRITE_STATE.WRITE;
								<StateTime>5__3.Reset();
								<basepercent>5__7 = num2;
								operation.ReportProgress(num2, $"Writing PID {devicePID.ID} = {value}...");
								<progress_timer>5__5.Reset();
							}
							if (<tx_timer>5__4.ElapsedTime >= <retryTime>5__8 && devicePID.RequestWrite((long)value))
							{
								<tx_timer>5__4.Reset();
								if (<state>5__2 == PID_WRITE_STATE.WRITE)
								{
									<state>5__2 = PID_WRITE_STATE.VERIFY;
									<StateTime>5__3.Reset();
								}
							}
							goto IL_00db;
						}
						operation.ReportProgress(100f, "Success!");
						result = true;
					}
					goto end_IL_000e;
					IL_0159:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
					goto IL_0160;
					end_IL_000e:;
				}
				catch (System.Exception exception)
				{
					<>1__state = -2;
					<StateTime>5__3 = null;
					<tx_timer>5__4 = null;
					<progress_timer>5__5 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<StateTime>5__3 = null;
				<tx_timer>5__4 = null;
				<progress_timer>5__5 = null;
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
		private struct <WriteAsync>d__56 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public DevicePID <>4__this;

			public ISessionClient session;

			public AsyncOperation operation;

			public ushort address;

			public uint data;

			private PID_WRITE_STATE <state>5__2;

			private Timer <StateTime>5__3;

			private Timer <tx_timer>5__4;

			private Timer <progress_timer>5__5;

			private bool <skip>5__6;

			private double <basepercent>5__7;

			private TimeSpan <retryTime>5__8;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_014c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0151: Unknown result type (might be due to invalid IL or missing references)
				//IL_0159: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Expected O, but got Unknown
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0095: Expected O, but got Unknown
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a1: Expected O, but got Unknown
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_010e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0113: Unknown result type (might be due to invalid IL or missing references)
				//IL_0117: Unknown result type (might be due to invalid IL or missing references)
				//IL_011c: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0200: Unknown result type (might be due to invalid IL or missing references)
				//IL_0211: Unknown result type (might be due to invalid IL or missing references)
				//IL_0216: Unknown result type (might be due to invalid IL or missing references)
				//IL_022a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0230: Unknown result type (might be due to invalid IL or missing references)
				//IL_0131: Unknown result type (might be due to invalid IL or missing references)
				//IL_0133: Unknown result type (might be due to invalid IL or missing references)
				//IL_0313: Unknown result type (might be due to invalid IL or missing references)
				//IL_0319: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DevicePID devicePID = <>4__this;
				bool result;
				try
				{
					ConfiguredTaskAwaiter val;
					if (num == 0)
					{
						val = <>u__1;
						<>u__1 = default(ConfiguredTaskAwaiter);
						num = (<>1__state = -1);
						goto IL_0168;
					}
					if (devicePID.IsWithAddress)
					{
						<state>5__2 = PID_WRITE_STATE.OPEN_SESSION;
						if (session != null)
						{
							operation.ReportProgress(0f, $"Opening {session.SessionID} session...");
						}
						<StateTime>5__3 = new Timer(true);
						<tx_timer>5__4 = new Timer(true);
						<progress_timer>5__5 = new Timer(true);
						<tx_timer>5__4.ElapsedTime = TimeSpan.FromSeconds(1.0);
						<skip>5__6 = true;
						<basepercent>5__7 = 0.0;
						float num2 = 0f;
						<retryTime>5__8 = TimeSpan.FromMilliseconds(250.0);
						goto IL_00ea;
					}
					result = false;
					goto end_IL_000e;
					IL_0168:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
					goto IL_016f;
					IL_00ea:
					operation.ThrowIfCancellationRequested();
					if (<skip>5__6)
					{
						<skip>5__6 = false;
						goto IL_016f;
					}
					ConfiguredTaskAwaitable val2 = System.Threading.Tasks.Task.Delay(10).ConfigureAwait(false);
					val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
					if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val;
						<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <WriteAsync>d__56>(ref val, ref this);
						return;
					}
					goto IL_0168;
					IL_016f:
					if (((Disposable)devicePID).IsDisposed)
					{
						operation.ReportProgress("WritePIDAsync failed: object is disposed");
						result = false;
					}
					else if (!devicePID.Device.IsOnline)
					{
						operation.ReportProgress("Failed: Device went offline");
						result = false;
					}
					else
					{
						if (<state>5__2 != PID_WRITE_STATE.VERIFY || !devicePID.IsValueValid)
						{
							double num3 = <basepercent>5__7;
							double num4 = 100.0 - <basepercent>5__7;
							TimeSpan elapsedTime = <StateTime>5__3.ElapsedTime;
							double num5 = num4 * (double)((TimeSpan)(ref elapsedTime)).Ticks;
							elapsedTime = operation.ElapsedTime;
							float num2 = (float)(num3 + num5 / (double)((TimeSpan)(ref elapsedTime)).Ticks);
							if (<progress_timer>5__5.ElapsedTime > <retryTime>5__8)
							{
								operation.ReportProgress(num2, operation.Status);
								<progress_timer>5__5.Reset();
							}
							if (session != null)
							{
								session.TryOpenSession();
								if (!session.IsOpen)
								{
									goto IL_00ea;
								}
							}
							if (<state>5__2 == PID_WRITE_STATE.OPEN_SESSION)
							{
								<state>5__2 = PID_WRITE_STATE.WRITE;
								<StateTime>5__3.Reset();
								<basepercent>5__7 = num2;
								operation.ReportProgress(num2, $"Writing PID {devicePID.ID} = {address}...");
								<progress_timer>5__5.Reset();
							}
							if (<tx_timer>5__4.ElapsedTime >= <retryTime>5__8 && devicePID.RequestWrite(address, data))
							{
								<tx_timer>5__4.Reset();
								if (<state>5__2 == PID_WRITE_STATE.WRITE)
								{
									<state>5__2 = PID_WRITE_STATE.VERIFY;
									<StateTime>5__3.Reset();
								}
							}
							goto IL_00ea;
						}
						operation.ReportProgress(100f, "Success!");
						result = true;
					}
					end_IL_000e:;
				}
				catch (System.Exception exception)
				{
					<>1__state = -2;
					<StateTime>5__3 = null;
					<tx_timer>5__4 = null;
					<progress_timer>5__5 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<StateTime>5__3 = null;
				<tx_timer>5__4 = null;
				<progress_timer>5__5 = null;
				<>t__builder.SetResult(result);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		private readonly PIDUpdatedEvent PIDUpdatedEvent;

		private readonly PIDValueChangedEvent PIDValueChangedEvent;

		private ulong LastReadValue;

		private bool ValueValid;

		private static readonly object Lock = new object();

		private ConcurrentQueue<AsyncSignal> ReadSignals;

		[field: CompilerGenerated]
		public IEventPublisher Events
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IRemoteDevice Device
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public PID ID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public byte Flags
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public string Name => ID.Name;

		public bool IsReadable => (Flags & 1) != 0;

		public bool IsWritable => (Flags & 2) != 0;

		public bool IsNonVolatile => (Flags & 4) != 0;

		public bool IsWithAddress => (Flags & 8) != 0;

		public ulong Value
		{
			get
			{
				if (!IsValueValid)
				{
					return 0uL;
				}
				return LastReadValue;
			}
		}

		public uint Data
		{
			get
			{
				if (!IsValueValid)
				{
					return 0u;
				}
				return (uint)LastReadValue;
			}
		}

		public ushort Address
		{
			get
			{
				if (!IsValueValid)
				{
					return 0;
				}
				return (ushort)(LastReadValue >> 32);
			}
		}

		public bool IsValueValid
		{
			get
			{
				ValueValid &= Device.IsOnline;
				return ValueValid;
			}
		}

		public string ValueString
		{
			get
			{
				if (!IsValueValid)
				{
					return "UNKNOWN";
				}
				return ID.FormatValue(Value);
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public DevicePID(IRemoteDevice device, PID id, byte flags)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			Device = device;
			ID = id;
			Flags = flags;
			Events = (IEventPublisher)new EventPublisher("IDS.Core.IDS_CAN.PID.Events");
			PIDUpdatedEvent = new PIDUpdatedEvent(this);
			PIDValueChangedEvent = new PIDValueChangedEvent(this);
		}

		public DevicePID(IRemoteDevice device, PID id)
		{
			Device = device;
			ID = id;
			Flags = 255;
			Events = null;
			PIDUpdatedEvent = null;
			PIDValueChangedEvent = null;
		}

		public static implicit operator PID(DevicePID value)
		{
			return value.ID;
		}

		public bool RequestRead()
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed)
			{
				return false;
			}
			if (!IsReadable)
			{
				return false;
			}
			if (!Device.Adapter.LocalHost.IsOnline)
			{
				return false;
			}
			if (!Device.IsOnline)
			{
				return false;
			}
			return Device.Adapter.LocalHost.Transmit29((byte)128, 17, Device, PAYLOAD.FromArgs(new object[1] { PID.op_Implicit(ID) }));
		}

		public bool RequestRead(ushort Address)
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed)
			{
				return false;
			}
			if (!IsReadable)
			{
				return false;
			}
			if (!Device.Adapter.LocalHost.IsOnline)
			{
				return false;
			}
			if (!Device.IsOnline)
			{
				return false;
			}
			return Device.Adapter.LocalHost.Transmit29((byte)128, 17, Device, PAYLOAD.FromArgs(new object[2]
			{
				PID.op_Implicit(ID),
				Address
			}));
		}

		private bool RequestWrite(long value)
		{
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed)
			{
				return false;
			}
			if (!IsWritable)
			{
				return false;
			}
			if (!Device.Adapter.LocalHost.IsOnline)
			{
				return false;
			}
			if (!Device.IsOnline)
			{
				return false;
			}
			return Device.Adapter.LocalHost.Transmit29((byte)128, 17, Device, PAYLOAD.FromArgs(new object[3]
			{
				PID.op_Implicit(ID),
				(ushort)(value >> 32),
				(uint)value
			}));
		}

		private bool RequestWrite(ushort address, uint data)
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed)
			{
				return false;
			}
			if (!IsWritable)
			{
				return false;
			}
			if (!Device.Adapter.LocalHost.IsOnline)
			{
				return false;
			}
			if (!Device.IsOnline)
			{
				return false;
			}
			return Device.Adapter.LocalHost.Transmit29((byte)128, 17, Device, PAYLOAD.FromArgs(new object[3]
			{
				PID.op_Implicit(ID),
				address,
				data
			}));
		}

		public void OnMessageRx(IMessage message)
		{
			if (((Disposable)this).IsDisposed)
			{
				return;
			}
			ulong num = 0uL;
			for (int i = 2; i < ((IByteList)message).Length; i++)
			{
				num <<= 8;
				num += ((System.Collections.Generic.IReadOnlyList<byte>)message)[i];
			}
			ulong lastReadValue = LastReadValue;
			LastReadValue = num;
			bool isValueValid = IsValueValid;
			ValueValid = true;
			if (PIDUpdatedEvent != null)
			{
				PIDUpdatedEvent.Publish(num);
			}
			if (PIDValueChangedEvent != null && (lastReadValue != LastReadValue || !isValueValid))
			{
				PIDValueChangedEvent.Publish(num);
			}
			if (ReadSignals != null)
			{
				AsyncSignal asyncSignal = default(AsyncSignal);
				while (ReadSignals.TryDequeue(ref asyncSignal))
				{
					asyncSignal.OperationComplete = true;
				}
			}
		}

		[AsyncStateMachine(typeof(<ReadAsync>d__52))]
		public async System.Threading.Tasks.Task<bool> ReadAsync(AsyncOperation operation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			operation.ReportProgress(0f, "Reading...");
			AsyncSignal signal = new AsyncSignal();
			if (ReadSignals == null)
			{
				lock (Lock)
				{
					if (ReadSignals == null)
					{
						ReadSignals = new ConcurrentQueue<AsyncSignal>();
					}
				}
			}
			ReadSignals.Enqueue(signal);
			Timer tx_timer = new Timer(true);
			Timer progress_timer = new Timer(true);
			tx_timer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			progress_timer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			bool skip = true;
			TimeSpan ReadRequestTime = TimeSpan.FromMilliseconds(330.0);
			TimeSpan ReportProgressTime = TimeSpan.FromMilliseconds(250.0);
			while (!signal.OperationComplete)
			{
				operation.ThrowIfCancellationRequested();
				if (!skip)
				{
					await System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
				}
				else
				{
					skip = false;
				}
				if (((Disposable)this).IsDisposed)
				{
					operation.ReportProgress("ReadAsync failed: PID is disposed");
					return false;
				}
				if (!IsReadable)
				{
					operation.ReportProgress("ReadAsync failed: PID is not readable");
					return false;
				}
				if (!Device.IsOnline)
				{
					operation.ReportProgress("ReadAsync failed: Device is offline");
					return false;
				}
				if (tx_timer.ElapsedTime >= ReadRequestTime && RequestRead())
				{
					tx_timer.Reset();
				}
				if (progress_timer.ElapsedTime > ReportProgressTime)
				{
					progress_timer.Reset();
					operation.ReportProgress(0f, "Reading...");
				}
			}
			operation.ReportProgress(100f, "Success!");
			return true;
		}

		[AsyncStateMachine(typeof(<ReadAsync>d__53))]
		public async System.Threading.Tasks.Task<bool> ReadAsync(ushort address, AsyncOperation operation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			if (!IsWithAddress)
			{
				return false;
			}
			operation.ReportProgress(0f, "Reading...");
			AsyncSignal signal = new AsyncSignal();
			if (ReadSignals == null)
			{
				lock (Lock)
				{
					if (ReadSignals == null)
					{
						ReadSignals = new ConcurrentQueue<AsyncSignal>();
					}
				}
			}
			ReadSignals.Enqueue(signal);
			Timer tx_timer = new Timer(true);
			Timer progress_timer = new Timer(true);
			tx_timer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			progress_timer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			bool skip = true;
			TimeSpan ReadRequestTime = TimeSpan.FromMilliseconds(330.0);
			TimeSpan ReportProgressTime = TimeSpan.FromMilliseconds(250.0);
			while (!signal.OperationComplete)
			{
				operation.ThrowIfCancellationRequested();
				if (!skip)
				{
					await System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
				}
				else
				{
					skip = false;
				}
				if (((Disposable)this).IsDisposed)
				{
					operation.ReportProgress("ReadAsync failed: PID is disposed");
					return false;
				}
				if (!IsReadable)
				{
					operation.ReportProgress("ReadAsync failed: PID is not readable");
					return false;
				}
				if (!Device.IsOnline)
				{
					operation.ReportProgress("ReadAsync failed: Device is offline");
					return false;
				}
				if (tx_timer.ElapsedTime >= ReadRequestTime && RequestRead(address))
				{
					tx_timer.Reset();
				}
				if (progress_timer.ElapsedTime > ReportProgressTime)
				{
					progress_timer.Reset();
					operation.ReportProgress(0f, "Reading...");
				}
			}
			operation.ReportProgress(100f, "Success!");
			return true;
		}

		[AsyncStateMachine(typeof(<WriteAsync>d__55))]
		public async System.Threading.Tasks.Task<bool> WriteAsync(ulong value, ISessionClient session, AsyncOperation operation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			PID_WRITE_STATE state = PID_WRITE_STATE.OPEN_SESSION;
			if (session != null)
			{
				operation.ReportProgress(0f, $"Opening {session.SessionID} session...");
			}
			Timer StateTime = new Timer(true);
			Timer tx_timer = new Timer(true);
			Timer progress_timer = new Timer(true);
			tx_timer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			bool skip = true;
			double basepercent = 0.0;
			TimeSpan retryTime = TimeSpan.FromMilliseconds(250.0);
			while (true)
			{
				operation.ThrowIfCancellationRequested();
				if (!skip)
				{
					await System.Threading.Tasks.Task.Delay(10).ConfigureAwait(false);
				}
				else
				{
					skip = false;
				}
				if (((Disposable)this).IsDisposed)
				{
					operation.ReportProgress("WritePIDAsync failed: object is disposed");
					return false;
				}
				if (!Device.IsOnline)
				{
					operation.ReportProgress("Failed: Device went offline");
					return false;
				}
				if (state == PID_WRITE_STATE.VERIFY && IsValueValid && Value == value)
				{
					break;
				}
				double num = basepercent;
				double num2 = 100.0 - basepercent;
				TimeSpan elapsedTime = StateTime.ElapsedTime;
				double num3 = num2 * (double)((TimeSpan)(ref elapsedTime)).Ticks;
				elapsedTime = operation.ElapsedTime;
				float num4 = (float)(num + num3 / (double)((TimeSpan)(ref elapsedTime)).Ticks);
				if (progress_timer.ElapsedTime > retryTime)
				{
					operation.ReportProgress(num4, operation.Status);
					progress_timer.Reset();
				}
				if (session != null)
				{
					session.TryOpenSession();
					if (!session.IsOpen)
					{
						continue;
					}
				}
				if (state == PID_WRITE_STATE.OPEN_SESSION)
				{
					state = PID_WRITE_STATE.WRITE;
					StateTime.Reset();
					basepercent = num4;
					operation.ReportProgress(num4, $"Writing PID {ID} = {value}...");
					progress_timer.Reset();
				}
				if (tx_timer.ElapsedTime >= retryTime && RequestWrite((long)value))
				{
					tx_timer.Reset();
					if (state == PID_WRITE_STATE.WRITE)
					{
						state = PID_WRITE_STATE.VERIFY;
						StateTime.Reset();
					}
				}
			}
			operation.ReportProgress(100f, "Success!");
			return true;
		}

		[AsyncStateMachine(typeof(<WriteAsync>d__56))]
		public async System.Threading.Tasks.Task<bool> WriteAsync(ushort address, uint data, ISessionClient session, AsyncOperation operation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			if (!IsWithAddress)
			{
				return false;
			}
			PID_WRITE_STATE state = PID_WRITE_STATE.OPEN_SESSION;
			if (session != null)
			{
				operation.ReportProgress(0f, $"Opening {session.SessionID} session...");
			}
			Timer StateTime = new Timer(true);
			Timer tx_timer = new Timer(true);
			Timer progress_timer = new Timer(true);
			tx_timer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			bool skip = true;
			double basepercent = 0.0;
			TimeSpan retryTime = TimeSpan.FromMilliseconds(250.0);
			while (true)
			{
				operation.ThrowIfCancellationRequested();
				if (!skip)
				{
					await System.Threading.Tasks.Task.Delay(10).ConfigureAwait(false);
				}
				else
				{
					skip = false;
				}
				if (((Disposable)this).IsDisposed)
				{
					operation.ReportProgress("WritePIDAsync failed: object is disposed");
					return false;
				}
				if (!Device.IsOnline)
				{
					operation.ReportProgress("Failed: Device went offline");
					return false;
				}
				if (state == PID_WRITE_STATE.VERIFY && IsValueValid)
				{
					break;
				}
				double num = basepercent;
				double num2 = 100.0 - basepercent;
				TimeSpan elapsedTime = StateTime.ElapsedTime;
				double num3 = num2 * (double)((TimeSpan)(ref elapsedTime)).Ticks;
				elapsedTime = operation.ElapsedTime;
				float num4 = (float)(num + num3 / (double)((TimeSpan)(ref elapsedTime)).Ticks);
				if (progress_timer.ElapsedTime > retryTime)
				{
					operation.ReportProgress(num4, operation.Status);
					progress_timer.Reset();
				}
				if (session != null)
				{
					session.TryOpenSession();
					if (!session.IsOpen)
					{
						continue;
					}
				}
				if (state == PID_WRITE_STATE.OPEN_SESSION)
				{
					state = PID_WRITE_STATE.WRITE;
					StateTime.Reset();
					basepercent = num4;
					operation.ReportProgress(num4, $"Writing PID {ID} = {address}...");
					progress_timer.Reset();
				}
				if (tx_timer.ElapsedTime >= retryTime && RequestWrite(address, data))
				{
					tx_timer.Reset();
					if (state == PID_WRITE_STATE.WRITE)
					{
						state = PID_WRITE_STATE.VERIFY;
						StateTime.Reset();
					}
				}
			}
			operation.ReportProgress(100f, "Success!");
			return true;
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((System.IDisposable)Events)?.Dispose();
				ReadSignals = null;
			}
		}
	}
	public struct PIDValue
	{
		[field: CompilerGenerated]
		public IRemoteDevice Device
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public PID ID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public bool IsValueValid
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public ulong Value
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public uint Data
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public ushort Address
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public string ValueString
		{
			get
			{
				if (!IsValueValid)
				{
					return "UNKNOWN";
				}
				return ID.FormatValue(Value);
			}
		}

		internal PIDValue(IRemoteDevice device, PID id)
		{
			Device = device;
			ID = id;
			IsValueValid = false;
			Value = 0uL;
			Address = 0;
			Data = 0u;
		}

		internal PIDValue(IRemoteDevice device, PID id, ulong value, ushort address, uint data)
		{
			Device = device;
			ID = id;
			IsValueValid = true;
			Value = value;
			Address = address;
			Data = data;
		}
	}
	public interface IPIDManager : System.Collections.Generic.IEnumerable<IDevicePID>, System.Collections.IEnumerable
	{
		IRemoteDevice Device { get; }

		int Count { get; }

		bool DeviceQueryComplete { get; }

		void QueryDevice();

		bool Contains(PID id);

		IDevicePID GetPID(PID id);

		System.Threading.Tasks.Task<PIDValue> ReadAsync(PID id, AsyncOperation operation);

		System.Threading.Tasks.Task<PIDValue> ReadAsync(PID id, ushort address, AsyncOperation operation);
	}
	[DefaultMember("Item")]
	internal class PIDManager : RemoteDevice.Child, IPIDManager, System.Collections.Generic.IEnumerable<IDevicePID>, System.Collections.IEnumerable
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ReadAsync>d__16 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<PIDValue> <>t__builder;

			public PIDManager <>4__this;

			public PID id;

			public AsyncOperation operation;

			private DevicePID <pid>5__2;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				PIDManager pIDManager = <>4__this;
				PIDValue result;
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						if (pIDManager.Device == null || !pIDManager.Device.IsOnline)
						{
							goto IL_0120;
						}
						<pid>5__2 = pIDManager[id];
						if (<pid>5__2 == null && !pIDManager.HiddenPIDs.TryGetValue(id, ref <pid>5__2))
						{
							<pid>5__2 = new DevicePID(pIDManager.Device, id);
							pIDManager.HiddenPIDs.Add(id, <pid>5__2);
						}
						val = <pid>5__2.ReadAsync(operation).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <ReadAsync>d__16>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<bool>);
						num = (<>1__state = -1);
					}
					if (!val.GetResult())
					{
						<pid>5__2 = null;
						goto IL_0120;
					}
					result = new PIDValue(pIDManager.Device, id, <pid>5__2.Value, 0, 0u);
					goto end_IL_000e;
					IL_0120:
					result = new PIDValue(pIDManager.Device, id);
					end_IL_000e:;
				}
				catch (System.Exception exception)
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
		private struct <ReadAsync>d__17 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<PIDValue> <>t__builder;

			public PIDManager <>4__this;

			public PID id;

			public ushort address;

			public AsyncOperation operation;

			private DevicePID <pid>5__2;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				//IL_00df: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				PIDManager pIDManager = <>4__this;
				PIDValue result;
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						if (pIDManager.Device == null || !pIDManager.Device.IsOnline)
						{
							goto IL_013a;
						}
						<pid>5__2 = pIDManager[id];
						if (<pid>5__2 == null && !pIDManager.HiddenPIDs.TryGetValue(id, ref <pid>5__2))
						{
							<pid>5__2 = new DevicePID(pIDManager.Device, id);
							pIDManager.HiddenPIDs.Add(id, <pid>5__2);
						}
						val = <pid>5__2.ReadAsync(address, operation).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <ReadAsync>d__17>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<bool>);
						num = (<>1__state = -1);
					}
					if (!val.GetResult())
					{
						<pid>5__2 = null;
						goto IL_013a;
					}
					result = new PIDValue(pIDManager.Device, id, <pid>5__2.Value, <pid>5__2.Address, <pid>5__2.Data);
					goto end_IL_000e;
					IL_013a:
					result = new PIDValue(pIDManager.Device, id);
					end_IL_000e:;
				}
				catch (System.Exception exception)
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

		private static readonly List<DevicePID> EmptyList = new List<DevicePID>();

		private static readonly TimeSpan READ_LIST_TX_RETRY_TIME = TimeSpan.FromMilliseconds(500.0);

		private readonly Dictionary<PID, DevicePID> Dictionary = new Dictionary<PID, DevicePID>();

		private readonly Dictionary<PID, DevicePID> HiddenPIDs = new Dictionary<PID, DevicePID>();

		private readonly List<DevicePID> SortedList = new List<DevicePID>();

		private readonly Timer TxTimer = new Timer(true);

		private bool NeedsRead = true;

		private bool ReadListFromDevice;

		private ushort PidIndex;

		private ushort ReportedCount;

		public int Count
		{
			get
			{
				if (NeedsRead)
				{
					return 0;
				}
				return SortedList.Count;
			}
		}

		public bool DeviceQueryComplete => !NeedsRead;

		private DevicePID this[PID id]
		{
			get
			{
				DevicePID result = default(DevicePID);
				Dictionary.TryGetValue(id, ref result);
				return result;
			}
		}

		public PIDManager(RemoteDevice device)
			: base(device)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			Clear();
			((IEventSender)base.Adapter).Events.Subscribe<TransmitTurnEvent>((Action<TransmitTurnEvent>)OnTransmitNextMessage, (SubscriptionType)0, Subscriptions);
			ReadListFromDevice = ((System.Enum)base.Adapter.Options).HasFlag((System.Enum)ADAPTER_OPTIONS.AUTO_READ_DEVICE_PID_LIST);
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Clear();
			}
		}

		public System.Collections.Generic.IEnumerator<IDevicePID> GetEnumerator()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (NeedsRead)
			{
				return (System.Collections.Generic.IEnumerator<IDevicePID>)(object)EmptyList.GetEnumerator();
			}
			return (System.Collections.Generic.IEnumerator<IDevicePID>)(object)SortedList.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator();
		}

		[AsyncStateMachine(typeof(<ReadAsync>d__16))]
		public async System.Threading.Tasks.Task<PIDValue> ReadAsync(PID id, AsyncOperation operation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			if (base.Device != null && base.Device.IsOnline)
			{
				DevicePID pid = this[id];
				if (pid == null && !HiddenPIDs.TryGetValue(id, ref pid))
				{
					pid = new DevicePID(base.Device, id);
					HiddenPIDs.Add(id, pid);
				}
				if (await pid.ReadAsync(operation))
				{
					return new PIDValue(base.Device, id, pid.Value, 0, 0u);
				}
			}
			return new PIDValue(base.Device, id);
		}

		[AsyncStateMachine(typeof(<ReadAsync>d__17))]
		public async System.Threading.Tasks.Task<PIDValue> ReadAsync(PID id, ushort address, AsyncOperation operation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			if (base.Device != null && base.Device.IsOnline)
			{
				DevicePID pid = this[id];
				if (pid == null && !HiddenPIDs.TryGetValue(id, ref pid))
				{
					pid = new DevicePID(base.Device, id);
					HiddenPIDs.Add(id, pid);
				}
				if (await pid.ReadAsync(address, operation))
				{
					return new PIDValue(base.Device, id, pid.Value, pid.Address, pid.Data);
				}
			}
			return new PIDValue(base.Device, id);
		}

		public void QueryDevice()
		{
			if (NeedsRead)
			{
				ReadListFromDevice = true;
			}
		}

		public bool Contains(PID id)
		{
			return this[id] != null;
		}

		public IDevicePID GetPID(PID id)
		{
			return this[id];
		}

		public bool IsPIDSupported(PID id)
		{
			return Dictionary.ContainsKey(id);
		}

		private void Clear()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			TxTimer.ElapsedTime = TimeSpan.FromSeconds(-0.25);
			PidIndex = 0;
			SortedList.Clear();
			Enumerator<PID, DevicePID> enumerator = Dictionary.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					((Disposable)enumerator.Current).Dispose();
				}
			}
			finally
			{
				((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
			Dictionary.Clear();
			enumerator = HiddenPIDs.Values.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					((Disposable)enumerator.Current).Dispose();
				}
			}
			finally
			{
				((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
			HiddenPIDs.Clear();
		}

		public override void BackgroundTask()
		{
		}

		public override void OnDeviceTx(AdapterRxEvent tx)
		{
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			if (!base.Device.IsOnline || (byte)tx.MessageType != 129 || tx.TargetAddress != base.Adapter.LocalHost.Address)
			{
				return;
			}
			switch (tx.MessageData)
			{
			case 17:
				if (tx.Count >= 2)
				{
					ushort uINT = CommExtensions.GetUINT16((IByteList)(object)tx, 0);
					this[PID.op_Implicit(uINT)]?.OnMessageRx((IMessage)(object)tx);
					DevicePID devicePID2 = default(DevicePID);
					if (HiddenPIDs.TryGetValue(PID.op_Implicit(uINT), ref devicePID2))
					{
						devicePID2.OnMessageRx((IMessage)(object)tx);
					}
				}
				break;
			case 16:
				if (!ReadListFromDevice)
				{
					break;
				}
				if (tx.Count == 1)
				{
					NeedsRead = (ReadListFromDevice = false);
					ReportedCount = 0;
					SortedList.Clear();
					Dictionary.Clear();
					_ = tx[0];
				}
				else
				{
					if (tx.Count != 8 || CommExtensions.GetUINT16((IByteList)(object)tx, 0) != PidIndex)
					{
						break;
					}
					int num = 2;
					int i = PidIndex * 2;
					if (PidIndex == 0)
					{
						ReportedCount = CommExtensions.GetUINT16((IByteList)(object)tx, num);
						SortedList.Clear();
						Dictionary.Clear();
						num += 3;
					}
					else
					{
						i--;
					}
					for (; i < ReportedCount; i++)
					{
						if (num >= 8)
						{
							break;
						}
						PID val = PID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)tx, num));
						if (val != PID.UNKNOWN)
						{
							if (Dictionary.ContainsKey(val))
							{
								Clear();
								return;
							}
							DevicePID devicePID = new DevicePID(base.Device, val, tx[num + 2]);
							SortedList.Add(devicePID);
							Dictionary.Add(val, devicePID);
						}
						num += 3;
					}
					if (i >= ReportedCount)
					{
						SortedList.Sort((Comparison<DevicePID>)((DevicePID first, DevicePID second) => first.ID.Value.CompareTo(second.ID.Value)));
						NeedsRead = (ReadListFromDevice = false);
					}
					PidIndex++;
					TxTimer.ElapsedTime = TimeSpan.FromSeconds(1.0);
				}
				break;
			}
		}

		private void OnTransmitNextMessage(TransmitTurnEvent message)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			if (ReadListFromDevice && base.Adapter.LocalHost.IsOnline && base.Device.IsOnline && !(TxTimer.ElapsedTime < READ_LIST_TX_RETRY_TIME))
			{
				message.Handled = base.Adapter.LocalHost.Transmit29((byte)128, 16, base.Device, PAYLOAD.FromArgs(new object[1] { PidIndex }));
				if (message.Handled)
				{
					TxTimer.Reset();
				}
			}
		}
	}
	public interface IUniqueProductInfo
	{
		MAC MAC { get; }

		PRODUCT_ID ProductID { get; }
	}
	public interface IProduct : IBusEndpoint, IUniqueProductInfo, System.Collections.Generic.IEnumerable<IDevice>, System.Collections.IEnumerable
	{
		string Name { get; }

		string UniqueName { get; }

		IDS_CAN_VERSION_NUMBER ProtocolVersion { get; }

		byte ProductInstance { get; }

		int AssemblyPartNumber { get; }

		int DeviceCount { get; }

		SOFTWARE_UPDATE_STATE SoftwareUpdateState { get; }
	}
	public static class ProductExtensions
	{
		public static ulong GetProductUniqueID(this IUniqueProductInfo product)
		{
			return ((((((((((((ulong)((PhysicalAddress)product.MAC)[0] << 8) | ((PhysicalAddress)product.MAC)[1]) << 8) | ((PhysicalAddress)product.MAC)[2]) << 8) | ((PhysicalAddress)product.MAC)[3]) << 8) | ((PhysicalAddress)product.MAC)[4]) << 8) | ((PhysicalAddress)product.MAC)[5]) << 16) | PRODUCT_ID.op_Implicit(product.ProductID);
		}
	}
	public interface IProductManager : System.Collections.Generic.IEnumerable<IRemoteProduct>, System.Collections.IEnumerable
	{
		IAdapter Adapter { get; }

		int Count { get; }

		IRemoteProduct GetProduct(ADDRESS address);

		IRemoteProduct GetProduct(ulong unique_id);
	}
	internal class ProductManager : Disposable, IProductManager, System.Collections.Generic.IEnumerable<IRemoteProduct>, System.Collections.IEnumerable
	{
		[CompilerGenerated]
		private sealed class <GetEnumerator>d__12 : System.Collections.Generic.IEnumerator<IRemoteProduct>, System.Collections.IEnumerator, System.IDisposable
		{
			private int <>1__state;

			private IRemoteProduct <>2__current;

			public ProductManager <>4__this;

			private System.Collections.Generic.IEnumerator<RemoteProduct> <>7__wrap1;

			IRemoteProduct System.Collections.Generic.IEnumerator<IRemoteProduct>.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			object System.Collections.IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			[DebuggerHidden]
			public <GetEnumerator>d__12(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}

			[DebuggerHidden]
			void System.IDisposable.Dispose()
			{
				int num = <>1__state;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						<>m__Finally1();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					int num = <>1__state;
					ProductManager productManager = <>4__this;
					switch (num)
					{
					default:
						return false;
					case 0:
						<>1__state = -1;
						<>7__wrap1 = ((System.Collections.Generic.IEnumerable<RemoteProduct>)productManager.Products.Values).GetEnumerator();
						<>1__state = -3;
						break;
					case 1:
						<>1__state = -3;
						break;
					}
					while (((System.Collections.IEnumerator)<>7__wrap1).MoveNext())
					{
						RemoteProduct current = <>7__wrap1.Current;
						if (current != null && current.IsOnline)
						{
							<>2__current = current;
							<>1__state = 1;
							return true;
						}
					}
					<>m__Finally1();
					<>7__wrap1 = null;
					return false;
				}
				catch
				{
					//try-fault
					((System.IDisposable)this).Dispose();
					throw;
				}
			}

			bool System.Collections.IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void <>m__Finally1()
			{
				<>1__state = -1;
				if (<>7__wrap1 != null)
				{
					((System.IDisposable)<>7__wrap1).Dispose();
				}
			}

			[DebuggerHidden]
			void System.Collections.IEnumerator.Reset()
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotSupportedException();
			}
		}

		private readonly ConcurrentDictionary<ulong, RemoteProduct> Products = new ConcurrentDictionary<ulong, RemoteProduct>();

		private readonly ProductListChangedEvent ProductListChangedEvent;

		private readonly SubscriptionManager Subscriptions = new SubscriptionManager();

		[field: CompilerGenerated]
		public IAdapter Adapter
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public int Count => Products.Count;

		public int TotalProducts => Products.Count;

		public int TotalDevices
		{
			get
			{
				int num = 0;
				System.Collections.Generic.IEnumerator<IRemoteProduct> enumerator = GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						RemoteProduct remoteProduct = (RemoteProduct)enumerator.Current;
						num += remoteProduct.DeviceCount;
					}
					return num;
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
			}
		}

		public ProductManager(Adapter adapter)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			Adapter = adapter;
			((IDisposableManager)Adapter).AddDisposable((IDisposable)(object)this);
			ProductListChangedEvent = new ProductListChangedEvent(this);
			((IEventSender)Adapter).Events.Subscribe<AdapterOpenedEvent>((Action<AdapterOpenedEvent>)OnAdapterOpened, (SubscriptionType)1, Subscriptions);
			((IEventSender)Adapter).Events.Subscribe<AdapterClosedEvent>((Action<AdapterClosedEvent>)OnAdapterClosed, (SubscriptionType)1, Subscriptions);
			((IEventSender)Adapter).Events.Subscribe<RemoteDeviceOnlineEvent>((Action<RemoteDeviceOnlineEvent>)OnRemoteDeviceOnline, (SubscriptionType)1, Subscriptions);
			((IEventSender)Adapter).Events.Subscribe<RemoteDeviceOfflineEvent>((Action<RemoteDeviceOfflineEvent>)OnRemoteDeviceOffline, (SubscriptionType)1, Subscriptions);
			((IEventSender)Adapter).Events.Subscribe<AdapterRxEvent>((Action<AdapterRxEvent>)OnAdapterRx, (SubscriptionType)1, Subscriptions);
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Clear();
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator();
		}

		[IteratorStateMachine(typeof(<GetEnumerator>d__12))]
		public System.Collections.Generic.IEnumerator<IRemoteProduct> GetEnumerator()
		{
			System.Collections.Generic.IEnumerator<RemoteProduct> enumerator = ((System.Collections.Generic.IEnumerable<RemoteProduct>)Products.Values).GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					RemoteProduct current = enumerator.Current;
					if (current != null && current.IsOnline)
					{
						yield return current;
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		private void PublishChangeEvent()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			if (!((Disposable)this).IsDisposed)
			{
				System.Threading.Tasks.Task.Run((Action)([CompilerGenerated] () =>
				{
					((Event)ProductListChangedEvent).Publish();
				}));
			}
		}

		public IRemoteProduct GetProduct(ADDRESS address)
		{
			return Adapter.Devices.GetDeviceByAddress(address)?.Product;
		}

		public IRemoteProduct GetProduct(ulong unique_id)
		{
			RemoteProduct result = default(RemoteProduct);
			Products.TryGetValue(unique_id, ref result);
			return result;
		}

		private void OnAdapterOpened(AdapterOpenedEvent e)
		{
			Clear();
		}

		private void OnAdapterClosed(AdapterClosedEvent e)
		{
			Clear();
		}

		private void OnRemoteDeviceOnline(RemoteDeviceOnlineEvent message)
		{
			PublishChangeEvent();
		}

		private void OnRemoteDeviceOffline(RemoteDeviceOfflineEvent message)
		{
			if (((Disposable)this).IsDisposed)
			{
				return;
			}
			if (!((IAdapter)Adapter).IsConnected)
			{
				Clear();
				return;
			}
			if (message != null && message.Device?.Product?.DeviceCount <= 0)
			{
				ulong productUniqueID = message.Device.Product.GetProductUniqueID();
				RemoteProduct remoteProduct = default(RemoteProduct);
				if (Products.TryGetValue(productUniqueID, ref remoteProduct) && message.Device.Product == remoteProduct)
				{
					RemoteProduct remoteProduct2 = default(RemoteProduct);
					Products.TryRemove(productUniqueID, ref remoteProduct2);
				}
			}
			PublishChangeEvent();
		}

		private void OnAdapterRx(AdapterRxEvent rx)
		{
			if (GetProduct(rx.SourceAddress) is RemoteProduct remoteProduct && remoteProduct.Address == rx.SourceAddress)
			{
				remoteProduct.OnProductTx(rx);
			}
		}

		private void Clear()
		{
			if (!Products.IsEmpty)
			{
				Products.Clear();
				PublishChangeEvent();
			}
		}

		internal RemoteProduct LocateOrCreateProductForDevice(RemoteDevice device)
		{
			ulong productUniqueID = device.GetProductUniqueID();
			RemoteProduct orAdd = default(RemoteProduct);
			Products.TryGetValue(productUniqueID, ref orAdd);
			if (orAdd == null)
			{
				orAdd = Products.GetOrAdd(productUniqueID, (Func<ulong, RemoteProduct>)((ulong k) => new RemoteProduct(device.Adapter, device.ProductID, device.MAC, device.ProtocolVersion, device.ProductInstance)));
			}
			orAdd?.UpdateInstance(device.ProductInstance);
			return orAdd;
		}

		public override string ToString()
		{
			try
			{
				string text = $"Network contains {TotalDevices} devices in {TotalProducts} products";
				System.Collections.Generic.IEnumerator<IRemoteProduct> enumerator = GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						RemoteProduct remoteProduct = (RemoteProduct)enumerator.Current;
						text = text + "\n\t" + remoteProduct.Name + " [";
						int num = 0;
						System.Collections.Generic.IEnumerator<IRemoteDevice> enumerator2 = remoteProduct.GetEnumerator();
						try
						{
							while (((System.Collections.IEnumerator)enumerator2).MoveNext())
							{
								IRemoteDevice current = enumerator2.Current;
								if (num++ > 0)
								{
									text += ", ";
								}
								text += current.ToShortString(show_address: false);
							}
						}
						finally
						{
							((System.IDisposable)enumerator2)?.Dispose();
						}
						text += "]";
					}
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
				return text;
			}
			catch (System.Exception)
			{
			}
			return null;
		}
	}
	public interface IRemoteDevice : IDevice, IBusEndpoint, IUniqueDeviceInfo, IUniqueProductInfo, IEventSender
	{
		new IRemoteProduct Product { get; }

		bool IsCircuitIDWriteable { get; }

		IClientSessionManager Sessions { get; }

		IPIDManager PIDs { get; }

		IBLOCKManager BLOCKs { get; }

		ITreeNode TreeNode { get; }

		void QueryPartNumber();

		bool IsPIDSupported(PID id);

		bool IsSessionSupported(SESSION_ID id);

		IDevicePID GetPID(PID id);

		IDeviceBLOCK GetBLOCK(BLOCK_ID id);
	}
	internal class RemoteDevice : Adapter.Object, IRemoteDevice, IDevice, IBusEndpoint, IUniqueDeviceInfo, IUniqueProductInfo, IEventSender
	{
		public abstract class Child : Disposable
		{
			protected readonly SubscriptionManager Subscriptions;

			protected readonly RemoteDevice mDevice;

			[field: CompilerGenerated]
			public IAdapter Adapter
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			public IRemoteDevice Device => mDevice;

			public Child(RemoteDevice device)
			{
				mDevice = device;
				Subscriptions = device.Subscriptions;
				Adapter = Device.Adapter;
				device.Children.Add(this);
			}

			public abstract void BackgroundTask();

			public abstract void OnDeviceTx(AdapterRxEvent tx);
		}

		public abstract class ChildNode : Child
		{
			protected readonly ITreeNode TreeNode;

			public string Text
			{
				get
				{
					return TreeNode.Text;
				}
				set
				{
					TreeNode.Text = value;
				}
			}

			public System.Enum Icon
			{
				get
				{
					return TreeNode.Icon;
				}
				set
				{
					TreeNode.Icon = value;
				}
			}

			public ChildNode(RemoteDevice device)
				: base(device)
			{
				TreeNode = TreeNode.Create((object)this);
				base.Device.TreeNode.AddChild(TreeNode);
			}

			public override void Dispose(bool disposing)
			{
				if (disposing)
				{
					((System.IDisposable)TreeNode).Dispose();
				}
			}
		}

		private class MacNode : ChildNode
		{
			public MacNode(RemoteDevice device)
				: base(device)
			{
				base.Text = $"MAC: {device.MAC}";
				base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.INFO;
			}

			public override void BackgroundTask()
			{
			}

			public override void OnDeviceTx(AdapterRxEvent rx)
			{
			}
		}

		private class ProtocolNode : ChildNode
		{
			public ProtocolNode(RemoteDevice device)
				: base(device)
			{
				base.Text = $"Protocol {device.ProtocolVersion}";
				base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.INFO;
			}

			public override void BackgroundTask()
			{
			}

			public override void OnDeviceTx(AdapterRxEvent rx)
			{
			}
		}

		private class DeviceIdNode : ChildNode
		{
			public DeviceIdNode(RemoteDevice device)
				: base(device)
			{
				DeviceIDChanged(device.GetDeviceID());
			}

			public void DeviceIDChanged(DEVICE_ID id)
			{
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				base.Text = $"Device ID: {id}";
				base.Icon = (id.IsValid ? IDS.Core.IDS_CAN.Adapter.ICON.INFO : IDS.Core.IDS_CAN.Adapter.ICON.EXCLAMATION);
				base.Device.TreeNode.Text = base.Device.ToShortString(show_address: true);
				base.Device.TreeNode.Icon = (System.Enum)(object)id.Icon;
			}

			public override void BackgroundTask()
			{
			}

			public override void OnDeviceTx(AdapterRxEvent rx)
			{
			}
		}

		private class CircuitIdNode : ChildNode
		{
			public CircuitIdNode(RemoteDevice device)
				: base(device)
			{
				base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.INFO;
				CircuitIDChanged();
			}

			public void CircuitIDChanged()
			{
				base.Text = $"Circuit ID: {base.Device.CircuitID}";
			}

			public override void BackgroundTask()
			{
			}

			public override void OnDeviceTx(AdapterRxEvent rx)
			{
			}
		}

		private class PartNumberNode : ChildNode
		{
			private static readonly StringBuilder StringBuilder = new StringBuilder(50);

			private readonly Timer LastTxTime = new Timer(true);

			private string mPartNumber;

			private bool ReadPartNumber;

			public string PartNumber
			{
				get
				{
					return mPartNumber;
				}
				private set
				{
					if (!(mPartNumber != value))
					{
						return;
					}
					mPartNumber = value;
					if (value == null || value.Length == 0 || value == "")
					{
						base.Text = "Part Number: UNKNOWN";
						base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.EXCLAMATION;
						return;
					}
					base.Text = "Part Number: " + value;
					if (char.IsDigit(mPartNumber[0]) && char.IsDigit(mPartNumber[1]) && char.IsDigit(mPartNumber[2]) && char.IsDigit(mPartNumber[3]) && char.IsDigit(mPartNumber[4]))
					{
						base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.INFO;
					}
					else
					{
						base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.QUESTION;
					}
				}
			}

			public PartNumberNode(RemoteDevice device)
				: base(device)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Expected O, but got Unknown
				base.Text = "Part Number: <not queried>";
				ReadPartNumber = ((System.Enum)base.Adapter.Options).HasFlag((System.Enum)ADAPTER_OPTIONS.AUTO_READ_DEVICE_PART_NUMBER);
				((IEventSender)base.Adapter).Events.Subscribe<TransmitTurnEvent>((Action<TransmitTurnEvent>)OnTransmitNextMessage, (SubscriptionType)0, Subscriptions);
			}

			public void QueryDevice()
			{
				ReadPartNumber = true;
			}

			public override void BackgroundTask()
			{
			}

			public override void OnDeviceTx(AdapterRxEvent rx)
			{
				if ((byte)rx.MessageType != 129 || rx.MessageData != 0 || rx.Count != 8)
				{
					return;
				}
				lock (StringBuilder)
				{
					StringBuilder.Length = 0;
					for (int i = 0; i < 8; i++)
					{
						char c = (char)rx[i];
						if (c == '\0')
						{
							break;
						}
						StringBuilder.Append(c);
					}
					PartNumber = ((object)StringBuilder).ToString();
				}
			}

			private void OnTransmitNextMessage(TransmitTurnEvent message)
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				if (base.Device.IsOnline && mPartNumber == null && ReadPartNumber && base.Adapter.LocalHost.IsOnline && !(LastTxTime.ElapsedTime < PART_NUMBER_TIMEOUT))
				{
					message.Handled = base.Adapter.LocalHost.Transmit29((byte)128, 0, base.Device);
					if (message.Handled)
					{
						LastTxTime.Reset();
					}
				}
			}
		}

		private class MessageNode : ChildNode
		{
			public readonly MESSAGE_TYPE MessageType;

			public readonly Timer Age = new Timer(true);

			public readonly string BaseText;

			private PAYLOAD? mPayload;

			public PAYLOAD? Payload
			{
				get
				{
					return mPayload;
				}
				protected set
				{
					//IL_002a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0031: Unknown result type (might be due to invalid IL or missing references)
					PAYLOAD? val = mPayload;
					PAYLOAD? val2 = value;
					if (val.HasValue != val2.HasValue || (val.HasValue && val.GetValueOrDefault() != val2.GetValueOrDefault()))
					{
						mPayload = value;
						base.Text = BaseText + FormatMessage();
					}
				}
			}

			public MessageNode(RemoteDevice device, MESSAGE_TYPE message_type, string base_text)
				: base(device)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Expected O, but got Unknown
				MessageType = message_type;
				BaseText = base_text + ": ";
				Reset();
				base.Text = BaseText + FormatMessage();
			}

			protected void Reset()
			{
				Payload = null;
				base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.EXCLAMATION;
			}

			public override void BackgroundTask()
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				if (Payload.HasValue)
				{
					TimeSpan elapsedTime = Age.ElapsedTime;
					double totalSeconds = ((TimeSpan)(ref elapsedTime)).TotalSeconds;
					if (totalSeconds > 2.5)
					{
						Reset();
					}
					else if ((Adapter.ICON)(object)base.Icon == IDS.Core.IDS_CAN.Adapter.ICON.LED_GREEN && totalSeconds > 1.5)
					{
						base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.LED_YELLOW;
					}
				}
			}

			protected virtual string FormatMessage()
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				PAYLOAD? payload = Payload;
				object obj;
				if (!payload.HasValue)
				{
					obj = null;
				}
				else
				{
					PAYLOAD valueOrDefault = payload.GetValueOrDefault();
					obj = ((PAYLOAD)(ref valueOrDefault)).ToString(true);
				}
				if (obj == null)
				{
					obj = "???";
				}
				return (string)obj;
			}

			protected virtual void UpdateStatus(AdapterRxEvent rx)
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				Age.Reset();
				base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.LED_GREEN;
				Payload = rx.Payload;
			}

			public override void OnDeviceTx(AdapterRxEvent rx)
			{
				if (rx.MessageType == MessageType)
				{
					UpdateStatus(rx);
				}
			}
		}

		private class NetworkStatusNode : MessageNode
		{
			private Timer LastRxTime = new Timer(true);

			private DeviceInMotionLockoutLevelChangedEvent LockoutEvent;

			[field: CompilerGenerated]
			public NETWORK_STATUS NetworkStatus
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			public NetworkStatusNode(RemoteDevice device)
				: base(device, (byte)0, "Network status")
			{
			}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown


			protected override string FormatMessage()
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				PAYLOAD? payload = base.Payload;
				if (payload.HasValue)
				{
					PAYLOAD valueOrDefault = payload.GetValueOrDefault();
					if (((PAYLOAD)(ref valueOrDefault)).Length >= 1)
					{
						payload = base.Payload;
						if (!payload.HasValue)
						{
							return null;
						}
						valueOrDefault = payload.GetValueOrDefault();
						return ByteExtensions.HexString(((PAYLOAD)(ref valueOrDefault))[0]);
					}
				}
				return "???";
			}

			protected override void UpdateStatus(AdapterRxEvent rx)
			{
				//IL_009d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a7: Expected O, but got Unknown
				base.UpdateStatus(rx);
				if ((byte)rx.MessageType != 0 || rx.Count != 8)
				{
					return;
				}
				LastRxTime.Reset();
				NETWORK_STATUS prev = NetworkStatus;
				NetworkStatus = new NETWORK_STATUS(rx[0], rx[1]);
				if (prev.InMotionLockoutLevel != NetworkStatus.InMotionLockoutLevel)
				{
					if (LockoutEvent == null)
					{
						LockoutEvent = new DeviceInMotionLockoutLevelChangedEvent(base.Device);
					}
					System.Threading.Tasks.Task.Run((Action)delegate
					{
						LockoutEvent.Publish(NetworkStatus.InMotionLockoutLevel, prev.InMotionLockoutLevel);
					});
				}
			}
		}

		private class StatusDetailNode : ChildNode
		{
			public StatusDetailNode(RemoteDevice device, ITreeNode parentNode, string label)
				: base(device)
			{
				base.Text = label;
				base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.INFO;
				parentNode.AddChild(TreeNode);
			}

			public override void BackgroundTask()
			{
			}

			public override void OnDeviceTx(AdapterRxEvent rx)
			{
			}
		}

		private class EnhDeviceStatusNode : MessageNode
		{
			private Timer LastRxTime = new Timer(true);

			private IDeviceStatusParams devType;

			private System.Collections.Generic.IEnumerable<MemberInfo> members;

			private readonly List<StatusDetailNode> _subNodes = new List<StatusDetailNode>();

			private object objRef;

			public EnhDeviceStatusNode(RemoteDevice device)
				: base(device, (byte)3, "Device status")
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Expected O, but got Unknown
				//IL_013f: Unknown result type (might be due to invalid IL or missing references)
				switch (DEVICE_TYPE.op_Implicit(base.Device.GetDeviceID().DeviceType))
				{
				case 47:
					devType = new AWNING_SENSOR_STATUS_PARAMS();
					break;
				case 25:
					devType = new TEMPERATURE_SENSOR_STATUS_PARAMS();
					break;
				case 30:
				case 31:
				case 32:
				case 33:
					devType = new RELAY_TYPE_2_STATUS_PARAMS();
					break;
				case 10:
					devType = new TANK_SENSOR_STATUS_PARAMS();
					break;
				case 20:
					devType = new DIMMABLE_LIGHT_STATUS_PARAMS();
					break;
				}
				if (devType == null)
				{
					return;
				}
				List<MemberInfo> val = Enumerable.ToList<MemberInfo>(Enumerable.Where<MemberInfo>((System.Collections.Generic.IEnumerable<MemberInfo>)((object)devType).GetType().GetMembers((BindingFlags)20), (Func<MemberInfo, bool>)((MemberInfo m) => (((int)m.MemberType == 4 && ((FieldInfo)m).FieldType == typeof(string)) || ((int)m.MemberType == 16 && ((PropertyInfo)m).PropertyType == typeof(string))) && System.Attribute.IsDefined(m, typeof(DeviceDisplayAttribute)))));
				members = (System.Collections.Generic.IEnumerable<MemberInfo>)val;
				System.Collections.Generic.IEnumerator<MemberInfo> enumerator = members.GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						MemberInfo current = enumerator.Current;
						DeviceDisplayAttribute deviceDisplayAttribute = (DeviceDisplayAttribute)System.Attribute.GetCustomAttribute(current, typeof(DeviceDisplayAttribute));
						string text = ((deviceDisplayAttribute != null) ? deviceDisplayAttribute.DisplayName : current.Name);
						PropertyInfo val2 = (PropertyInfo)(object)((current is PropertyInfo) ? current : null);
						object obj = ((val2 != null) ? val2.GetValue((object)devType) : ((FieldInfo)current).GetValue((object)devType));
						StatusDetailNode statusDetailNode = new StatusDetailNode(device, TreeNode, $"{text}: {obj}");
						_subNodes.Add(statusDetailNode);
					}
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
			}

			protected override void UpdateStatus(AdapterRxEvent rx)
			{
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				base.UpdateStatus(rx);
				if ((byte)rx.MessageType != 3)
				{
					return;
				}
				LastRxTime.Reset();
				if (members == null || devType == null)
				{
					return;
				}
				devType.SetPayload(rx.Payload);
				System.Collections.Generic.IEnumerator<MemberInfo> enumerator = members.GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						MemberInfo current = enumerator.Current;
						PropertyInfo val = (PropertyInfo)(object)((current is PropertyInfo) ? current : null);
						object obj = ((val != null) ? val.GetValue((object)devType) : ((FieldInfo)current).GetValue((object)devType));
						DeviceDisplayAttribute deviceDisplayAttribute = (DeviceDisplayAttribute)System.Attribute.GetCustomAttribute(current, typeof(DeviceDisplayAttribute));
						string label = ((deviceDisplayAttribute != null) ? deviceDisplayAttribute.DisplayName : current.Name);
						StatusDetailNode statusDetailNode = Enumerable.FirstOrDefault<StatusDetailNode>((System.Collections.Generic.IEnumerable<StatusDetailNode>)_subNodes, (Func<StatusDetailNode, bool>)((StatusDetailNode node) => node.Text.StartsWith(label + ":")));
						if (statusDetailNode != null)
						{
							statusDetailNode.Text = $"{label}: {obj}";
						}
					}
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
			}
		}

		private static readonly TimeSpan PART_NUMBER_TIMEOUT = TimeSpan.FromSeconds(1.0);

		private static readonly TimeSpan DEVICE_STATUS_TIMEOUT = TimeSpan.FromSeconds(5.0);

		private readonly List<Child> Children = new List<Child>();

		private readonly MacNode mMacNode;

		private readonly ProtocolNode mProtocolNode;

		private readonly CircuitIdNode mCircuitIdNode;

		private readonly DeviceIdNode mDeviceIdNode;

		private readonly PartNumberNode mPartNumberNode;

		private readonly NetworkStatusNode mNetworkStatusNode;

		private readonly RemoteProduct mProduct;

		private readonly EnhDeviceStatusNode mEnhStatusNode;

		private DeviceIDChangedEvent DeviceIDChangedEvent;

		private CIRCUIT_ID mCircuitID;

		private PAYLOAD mDeviceStatus;

		private Timer DeviceStatusAge = new Timer(true);

		private CircuitIDChangedEvent CircuitIDChangedEvent;

		[field: CompilerGenerated]
		public ADDRESS Address
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public MAC MAC
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IDS_CAN_VERSION_NUMBER ProtocolVersion
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public PRODUCT_ID ProductID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public byte ProductInstance
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public DEVICE_TYPE DeviceType
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public int DeviceInstance
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public FUNCTION_NAME FunctionName
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public int FunctionInstance
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public byte? DeviceCapabilities
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public ITreeNode TreeNode
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IPIDManager PIDs
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IBLOCKManager BLOCKs
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IClientSessionManager Sessions
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public ITextConsole TextConsole
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public NETWORK_STATUS NetworkStatus => mNetworkStatusNode.NetworkStatus;

		public string SoftwarePartNumber => mPartNumberNode.PartNumber;

		[field: CompilerGenerated]
		public bool IsOnline
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public IRemoteProduct Product => mProduct;

		IProduct IDevice.Product => mProduct;

		[field: CompilerGenerated]
		public IEventPublisher Events
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public CIRCUIT_ID CircuitID
		{
			get
			{
				return mCircuitID;
			}
			set
			{
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Expected O, but got Unknown
				if ((uint)mCircuitID != (uint)value)
				{
					CIRCUIT_ID prev = mCircuitID;
					mCircuitID = value;
					mCircuitIdNode?.CircuitIDChanged();
					if (CircuitIDChangedEvent == null)
					{
						CircuitIDChangedEvent = new CircuitIDChangedEvent(this);
					}
					System.Threading.Tasks.Task.Run((Action)delegate
					{
						CircuitIDChangedEvent.Publish(prev);
					});
				}
			}
		}

		public PAYLOAD DeviceStatus
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return mDeviceStatus;
			}
			set
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				DeviceStatusAge.Reset();
				if (mDeviceStatus != value)
				{
					mDeviceStatus = value;
				}
			}
		}

		private DEVICE_ID DeviceID
		{
			set
			{
				//IL_0150: Unknown result type (might be due to invalid IL or missing references)
				//IL_015a: Expected O, but got Unknown
				if (value.ProductID != ProductID || value.DeviceType != DeviceType || value.DeviceInstance != DeviceInstance || value.ProductInstance == 0)
				{
					return;
				}
				mProduct.UpdateInstance(value.ProductInstance);
				if (value.ProductInstance != ProductInstance || value.FunctionName != FunctionName || value.FunctionInstance != FunctionInstance || value.DeviceCapabilities != DeviceCapabilities)
				{
					ProductInstance = value.ProductInstance;
					FunctionName = value.FunctionName;
					FunctionInstance = value.FunctionInstance;
					DeviceCapabilities = value.DeviceCapabilities;
					mDeviceIdNode?.DeviceIDChanged(value);
					if (DeviceIDChangedEvent == null)
					{
						DeviceIDChangedEvent = new DeviceIDChangedEvent(this);
					}
					System.Threading.Tasks.Task.Run((Action)([CompilerGenerated] () =>
					{
						((Event)DeviceIDChangedEvent).Publish();
					}));
				}
			}
		}

		public bool IsCircuitIDWriteable => GetPID(PID.IDS_CAN_CIRCUIT_ID)?.IsWritable ?? false;

		public bool IsPIDSupported(PID id)
		{
			return PIDs.Contains(id);
		}

		public IDevicePID GetPID(PID id)
		{
			return PIDs.GetPID(id);
		}

		public bool IsBLOCKSupported(BLOCK_ID id)
		{
			return BLOCKs.Contains(id);
		}

		public IDeviceBLOCK GetBLOCK(BLOCK_ID id)
		{
			return BLOCKs.GetBLOCK(id);
		}

		public bool IsSessionSupported(SESSION_ID id)
		{
			return Sessions.Contains(id);
		}

		public void QueryPartNumber()
		{
			mPartNumberNode.QueryDevice();
		}

		internal RemoteDevice(Adapter adapter, ADDRESS address, MAC mac, IDS_CAN_VERSION_NUMBER protocol, DEVICE_ID device_id, CIRCUIT_ID circuit_id, PAYLOAD device_status)
			: base(adapter)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected O, but got Unknown
			Address = address;
			MAC = new MAC(mac);
			ProtocolVersion = protocol;
			ProductID = device_id.ProductID;
			ProductInstance = device_id.ProductInstance;
			DeviceType = device_id.DeviceType;
			DeviceInstance = device_id.DeviceInstance;
			FunctionName = device_id.FunctionName;
			FunctionInstance = device_id.FunctionInstance;
			DeviceCapabilities = device_id.DeviceCapabilities;
			mCircuitID = circuit_id;
			mDeviceStatus = device_status;
			DeviceStatusAge.Reset();
			Events = (IEventPublisher)new EventPublisher($"RemoteDevice[{address}].Events");
			((DisposableManager)this).AddDisposable((IDisposable)(object)Events);
			TreeNode = TreeNode.Create((object)this);
			((DisposableManager)this).AddDisposable((IDisposable)(object)TreeNode);
			mMacNode = new MacNode(this);
			mProtocolNode = new ProtocolNode(this);
			mCircuitIdNode = new CircuitIdNode(this);
			mDeviceIdNode = new DeviceIdNode(this);
			mPartNumberNode = new PartNumberNode(this);
			mNetworkStatusNode = new NetworkStatusNode(this);
			Sessions = new ClientSessionManager(this);
			PIDs = new PIDManager(this);
			BLOCKs = new BLOCKManager(this);
			TextConsole = new RemoteTextConsole(this);
			mEnhStatusNode = new EnhDeviceStatusNode(this);
			mProduct = (base.Adapter.Products as ProductManager)?.LocateOrCreateProductForDevice(this);
			IsOnline = true;
			TreeNode.Text = this.ToShortString(show_address: true);
			((IAdapter)base.Adapter).TreeNode.AddChild(TreeNode);
			mProduct.AddOrUpdateDevice(this);
			this.GetDeviceID();
		}

		public override void Dispose(bool disposing)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (!disposing)
			{
				return;
			}
			GoOffline();
			Enumerator<Child> enumerator = Children.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					((Disposable)enumerator.Current).Dispose();
				}
			}
			finally
			{
				((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
			Children.Clear();
		}

		public void GoOffline()
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			if (!IsOnline)
			{
				return;
			}
			Extensions.RemoveFromParent(TreeNode);
			this.GetDeviceID();
			IsOnline = false;
			SubscriptionManager subscriptions = base.Subscriptions;
			if (subscriptions != null)
			{
				subscriptions.CancelAllSubscriptions();
			}
			mProduct.RemoveOfflineDevice(this);
			if (!((Disposable)this).IsDisposed)
			{
				System.Threading.Tasks.Task.Run((Action)([CompilerGenerated] () =>
				{
					RemoteDeviceOfflineEvent remoteDeviceOfflineEvent = new RemoteDeviceOfflineEvent((IEventSender)(object)this, this);
					remoteDeviceOfflineEvent.Publish(Address);
					((IEventSender)base.Adapter).Events.Publish<RemoteDeviceOfflineEvent>(remoteDeviceOfflineEvent);
				}));
				System.Threading.Tasks.Task.Delay(60000).ContinueWith((Action<System.Threading.Tasks.Task>)([CompilerGenerated] (System.Threading.Tasks.Task t) =>
				{
					((Disposable)this).Dispose();
				}));
			}
		}

		public void OnAdapterMessageRx(AdapterRxEvent rx)
		{
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			if (!IsOnline || ((Disposable)this).IsDisposed || !((IAdapter)base.Adapter).IsConnected)
			{
				return;
			}
			switch (rx.MessageType)
			{
			case 1:
				if (rx.Count >= 4)
				{
					CircuitID = CommExtensions.GetUINT32((IByteList)(object)rx, 0);
				}
				break;
			case 2:
				if (rx.Count == 8)
				{
					DeviceID = new DEVICE_ID(PRODUCT_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0)), rx[2], DEVICE_TYPE.op_Implicit(rx[3]), rx[6] >> 4, FUNCTION_NAME.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 4)), rx[6] & 0xF, rx[7]);
				}
				else if (rx.Count == 7)
				{
					DeviceID = new DEVICE_ID(PRODUCT_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 0)), rx[2], DEVICE_TYPE.op_Implicit(rx[3]), rx[6] >> 4, FUNCTION_NAME.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)rx, 4)), rx[6] & 0xF, null);
				}
				break;
			case 3:
				DeviceStatus = rx.Payload;
				break;
			}
			Enumerator<Child> enumerator = Children.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnDeviceTx(rx);
				}
			}
			finally
			{
				((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
		}

		public override string ToString()
		{
			return ((object)this.GetDeviceID()/*cast due to .constrained prefix*/).ToString();
		}

		public void BackgroundTask()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			if (((Disposable)this).IsDisposed || !((IAdapter)base.Adapter).IsConnected)
			{
				return;
			}
			Enumerator<Child> enumerator = Children.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.BackgroundTask();
				}
			}
			finally
			{
				((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
			PAYLOAD deviceStatus = DeviceStatus;
			if (((PAYLOAD)(ref deviceStatus)).Length > 0 && DeviceStatusAge.ElapsedTime >= DEVICE_STATUS_TIMEOUT)
			{
				((PAYLOAD)(ref mDeviceStatus)).Length = 0;
			}
		}
	}
	public interface IRemoteProduct : IProduct, IBusEndpoint, IUniqueProductInfo, System.Collections.Generic.IEnumerable<IDevice>, System.Collections.IEnumerable, System.Collections.Generic.IEnumerable<IRemoteDevice>
	{
		IDTCManager DTCs { get; }
	}
	internal class RemoteProduct : IRemoteProduct, IProduct, IBusEndpoint, IUniqueProductInfo, System.Collections.Generic.IEnumerable<IDevice>, System.Collections.IEnumerable, System.Collections.Generic.IEnumerable<IRemoteDevice>
	{
		private readonly ConcurrentDictionary<ulong, RemoteDevice> Devices = new ConcurrentDictionary<ulong, RemoteDevice>();

		private readonly DTCManager DTCManager;

		private readonly ulong ProductUniqueID;

		[field: CompilerGenerated]
		public string Name
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public string UniqueName
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IAdapter Adapter
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public MAC MAC
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IDS_CAN_VERSION_NUMBER ProtocolVersion
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public PRODUCT_ID ProductID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public byte ProductInstance
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public ADDRESS Address => ProductInstance;

		public int AssemblyPartNumber => ProductID.AssemblyPartNumber;

		public int DeviceCount => Devices.Count;

		public bool IsOnline => !Devices.IsEmpty;

		[field: CompilerGenerated]
		public SOFTWARE_UPDATE_STATE SoftwareUpdateState
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public IDTCManager DTCs
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public System.Collections.Generic.IEnumerator<IRemoteDevice> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerator<IRemoteDevice>)((System.Collections.Generic.IEnumerable<RemoteDevice>)Devices.Values).GetEnumerator();
		}

		System.Collections.Generic.IEnumerator<IDevice> System.Collections.Generic.IEnumerable<IDevice>.GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerator<IDevice>)GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator();
		}

		public RemoteProduct(IAdapter adapter, PRODUCT_ID product_id, MAC mac, IDS_CAN_VERSION_NUMBER protocol_version, byte product_instance)
		{
			Adapter = adapter;
			ProductID = product_id;
			MAC = new MAC(mac);
			ProtocolVersion = protocol_version;
			ProductInstance = product_instance;
			Name = ((object)ProductID).ToString();
			UniqueName = $"{ProductID} MAC[{MAC}]";
			DTCManager = new DTCManager(this);
			DTCs = DTCManager;
			ProductUniqueID = this.GetProductUniqueID();
		}

		public virtual string ToString()
		{
			return Name;
		}

		public void OnProductTx(AdapterRxEvent tx)
		{
			if ((byte)tx.MessageType == 6 && tx.Count >= 1)
			{
				SoftwareUpdateState = (byte)(tx[0] & 3);
			}
			DTCManager.OnProductTx(tx);
		}

		internal void AddOrUpdateDevice(RemoteDevice device)
		{
			UpdateInstance(device.ProductInstance);
			RemoteDevice prev = null;
			lock (Devices)
			{
				Devices.AddOrUpdate(device.GetDeviceUniqueID(), device, (Func<ulong, RemoteDevice, RemoteDevice>)delegate(ulong key, RemoteDevice value)
				{
					prev = value;
					return device;
				});
			}
			if (prev != device)
			{
				prev?.GoOffline();
			}
		}

		internal void UpdateInstance(byte instance)
		{
			if (instance > 0)
			{
				ProductInstance = instance;
			}
		}

		internal void RemoveOfflineDevice(RemoteDevice device)
		{
			ulong deviceUniqueID = device.GetDeviceUniqueID();
			lock (Devices)
			{
				RemoteDevice remoteDevice = default(RemoteDevice);
				if (Devices.TryGetValue(deviceUniqueID, ref remoteDevice) && remoteDevice == device)
				{
					RemoteDevice remoteDevice2 = default(RemoteDevice);
					Devices.TryRemove(deviceUniqueID, ref remoteDevice2);
				}
			}
		}
	}
	public interface ISession
	{
		SESSION_ID SessionID { get; }

		IBusEndpoint Host { get; }

		IBusEndpoint Client { get; }

		bool IsOpen { get; }

		TimeSpan OpenTime { get; }
	}
	public interface ISessionClient : IEventSender, ISession, IDisposable, System.IDisposable
	{
		bool IsValid { get; }

		bool TryOpenSession();

		bool CloseSession();
	}
	public interface IClientSessionManager : System.Collections.Generic.IEnumerable<SESSION_ID>, System.Collections.IEnumerable
	{
		IRemoteDevice Device { get; }

		int Count { get; }

		bool DeviceQueryComplete { get; }

		void QueryDevice();

		bool Contains(SESSION_ID id);

		ISessionClient GetSession(ILocalDevice localhost, SESSION_ID session_id);
	}
	internal class SessionClient : Disposable, ISessionClient, IEventSender, ISession, IDisposable, System.IDisposable
	{
		private class BusEndpoint : IBusEndpoint
		{
			[field: CompilerGenerated]
			public IAdapter Adapter
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			[field: CompilerGenerated]
			public ADDRESS Address
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				set;
			}

			public bool IsOnline
			{
				get
				{
					if (((IAdapter)Adapter).IsConnected)
					{
						return Address.IsValidDeviceAddress;
					}
					return false;
				}
			}

			public BusEndpoint(IAdapter adapter, ADDRESS address)
			{
				Adapter = adapter;
				Address = address;
			}
		}

		private static readonly TimeSpan MESSAGE_RETRY_TIME = TimeSpan.FromMilliseconds(500.0);

		private readonly IRemoteDevice RemoteHost;

		private readonly ILocalDevice LocalHost;

		private readonly BusEndpoint mHost;

		private readonly BusEndpoint mClient;

		private readonly Timer mOpenTime = new Timer(true);

		private readonly Timer LastTxTime = new Timer(true);

		private bool mIsOpen;

		private RESPONSE CloseReason;

		private readonly ClientSessionOpenEvent SessionOpenEvent;

		private readonly ClientSessionClosedEvent SessionClosedEvent;

		private SubscriptionManager Subscriptions = new SubscriptionManager();

		[field: CompilerGenerated]
		public IEventPublisher Events
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public SESSION_ID SessionID
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public IBusEndpoint Host => mHost;

		public IBusEndpoint Client => mClient;

		public TimeSpan OpenTime
		{
			get
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				if (!IsOpen)
				{
					return TimeSpan.Zero;
				}
				return mOpenTime.ElapsedTime;
			}
		}

		public bool IsValid
		{
			get
			{
				if (((Disposable)this).IsDisposed)
				{
					return false;
				}
				if (!RemoteHost.IsOnline || RemoteHost.Address != Host.Address)
				{
					((Disposable)this).Dispose();
					return false;
				}
				if (Client.Address != LocalHost.Address || !LocalHost.IsOnline)
				{
					TerminateSession(RESPONSE.CONDITIONS_NOT_CORRECT);
					mClient.Address = LocalHost.Address;
				}
				return true;
			}
		}

		public bool IsOpen
		{
			get
			{
				if (!IsValid)
				{
					return false;
				}
				return mIsOpen;
			}
			set
			{
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				if (((Disposable)this).IsDisposed)
				{
					value = false;
				}
				if (mIsOpen != value)
				{
					mOpenTime.Reset();
					mIsOpen = value;
					if (value)
					{
						((Event)SessionOpenEvent).Publish();
						((IEventSender)LocalHost.Adapter).Events.Publish<ClientSessionOpenEvent>(SessionOpenEvent);
					}
					else
					{
						((Event)SessionClosedEvent).Publish();
						((IEventSender)LocalHost.Adapter).Events.Publish<ClientSessionClosedEvent>(SessionClosedEvent);
						LastTxTime.ElapsedTime = TimeSpan.FromSeconds(1.0);
					}
				}
			}
		}

		public SessionClient(SESSION_ID session_id, IRemoteDevice host, ILocalDevice client)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			SessionID = session_id;
			RemoteHost = host;
			LocalHost = client;
			mHost = new BusEndpoint(host.Adapter, host.Address);
			mClient = new BusEndpoint(client.Adapter, ADDRESS.INVALID);
			LastTxTime.ElapsedTime = TimeSpan.FromSeconds(1.0);
			Events = (IEventPublisher)new EventPublisher("IDS.Core.IDS_CAN.Devices.ClientSession.Events");
			SessionOpenEvent = new ClientSessionOpenEvent(this);
			SessionClosedEvent = new ClientSessionClosedEvent(this);
			((IEventSender)LocalHost.Adapter).Events.Subscribe<AdapterRxEvent>((Action<AdapterRxEvent>)OnAdapterRx, (SubscriptionType)0, Subscriptions);
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				TerminateSession(RESPONSE.CONDITIONS_NOT_CORRECT);
				mHost.Address = ADDRESS.INVALID;
				mClient.Address = ADDRESS.INVALID;
				((System.IDisposable)Events).Dispose();
				((Disposable)Subscriptions).Dispose();
			}
		}

		private void TerminateSession(RESPONSE reason)
		{
			if (mIsOpen)
			{
				CloseReason = reason;
				IsOpen = false;
			}
		}

		public bool TryOpenSession()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			if (!IsValid)
			{
				return false;
			}
			if (!LocalHost.IsOnline)
			{
				TerminateSession(RESPONSE.CONDITIONS_NOT_CORRECT);
				return false;
			}
			if (LastTxTime.ElapsedTime < MESSAGE_RETRY_TIME)
			{
				return mIsOpen;
			}
			if (!mIsOpen)
			{
				if (LocalHost.Transmit29((byte)128, 66, Host, PAYLOAD.FromArgs(new object[1] { SessionID.Value })))
				{
					LastTxTime.Reset();
				}
			}
			else if (LocalHost.Transmit29((byte)128, 68, Host, PAYLOAD.FromArgs(new object[1] { SessionID.Value })))
			{
				LastTxTime.Reset();
			}
			return mIsOpen;
		}

		public bool CloseSession()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (IsOpen)
			{
				return LocalHost.Transmit29((byte)128, 69, Host, PAYLOAD.FromArgs(new object[1] { SessionID.Value }));
			}
			return false;
		}

		private void OnAdapterRx(AdapterRxEvent rx)
		{
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			if (!IsValid || rx.SourceAddress != Host.Address || rx.TargetAddress != Client.Address || (byte)rx.MessageType != 129)
			{
				return;
			}
			switch (rx.MessageData)
			{
			case 66:
				if (rx.Count == 6 && SESSION_ID.op_Implicit(SessionID) == CommExtensions.GetUINT16((IByteList)(object)rx, 0))
				{
					uint uINT = CommExtensions.GetUINT32((IByteList)(object)rx, 2);
					uint num = SessionID.Encrypt(uINT);
					LocalHost.Transmit29((byte)128, 67, Host, PAYLOAD.FromArgs(new object[2] { SessionID.Value, num }));
				}
				break;
			case 67:
				if (rx.Count == 2 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == SESSION_ID.op_Implicit(SessionID))
				{
					IsOpen = true;
				}
				break;
			case 68:
				if (rx.Count == 3 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == SESSION_ID.op_Implicit(SessionID))
				{
					TerminateSession((RESPONSE)rx[2]);
				}
				break;
			case 69:
				if (rx.Count == 3 && CommExtensions.GetUINT16((IByteList)(object)rx, 0) == SESSION_ID.op_Implicit(SessionID))
				{
					TerminateSession((RESPONSE)rx[2]);
				}
				break;
			}
		}
	}
	internal class ClientSessionManager : RemoteDevice.ChildNode, IClientSessionManager, System.Collections.Generic.IEnumerable<SESSION_ID>, System.Collections.IEnumerable
	{
		private class SessionInstanceManager : Disposable
		{
			private class DeviceMgr : Disposable
			{
				private readonly Dictionary<SESSION_ID, SessionClient> Sessions = new Dictionary<SESSION_ID, SessionClient>();

				private readonly IRemoteDevice RemoteDevice;

				private readonly ILocalDevice LocalHost;

				public DeviceMgr(IRemoteDevice remotedevice, ILocalDevice localhost)
				{
					RemoteDevice = remotedevice;
					LocalHost = localhost;
				}

				public ISessionClient GetSession(SESSION_ID session_id)
				{
					if (((Disposable)this).IsDisposed)
					{
						return null;
					}
					if (!session_id.IsValid)
					{
						return null;
					}
					SessionClient sessionClient = default(SessionClient);
					if (Sessions.TryGetValue(session_id, ref sessionClient) && ((Disposable)sessionClient).IsDisposed)
					{
						Sessions.Remove(session_id);
						sessionClient = null;
					}
					if (sessionClient == null)
					{
						sessionClient = new SessionClient(session_id, RemoteDevice, LocalHost);
						Sessions.Add(session_id, sessionClient);
					}
					return sessionClient;
				}

				public void CloseAll()
				{
					//IL_000b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					Enumerator<SESSION_ID, SessionClient> enumerator = Sessions.Values.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							((Disposable)enumerator.Current).Dispose();
						}
					}
					finally
					{
						((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
					}
					Sessions.Clear();
				}

				public override void Dispose(bool disposing)
				{
					if (disposing)
					{
						CloseAll();
					}
				}
			}

			private Dictionary<ILocalDevice, DeviceMgr> Managers = new Dictionary<ILocalDevice, DeviceMgr>();

			private IRemoteDevice RemoteDevice;

			public SessionInstanceManager(IRemoteDevice device)
			{
				RemoteDevice = device;
			}

			public ISessionClient GetSession(ILocalDevice localhost, SESSION_ID session_id)
			{
				if (!session_id.IsValid)
				{
					return null;
				}
				if (((IDisposable)localhost).IsDisposed)
				{
					return null;
				}
				if (!RemoteDevice.IsOnline)
				{
					return null;
				}
				DeviceMgr deviceMgr = default(DeviceMgr);
				if (!Managers.TryGetValue(localhost, ref deviceMgr))
				{
					deviceMgr = new DeviceMgr(RemoteDevice, localhost);
					Managers.Add(localhost, deviceMgr);
					((IDisposableManager)localhost).AddDisposable((IDisposable)(object)deviceMgr);
				}
				return deviceMgr.GetSession(session_id);
			}

			public void CloseAll()
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Enumerator<ILocalDevice, DeviceMgr> enumerator = Managers.Values.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.CloseAll();
					}
				}
				finally
				{
					((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
			}

			public override void Dispose(bool disposing)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				if (!disposing)
				{
					return;
				}
				Enumerator<ILocalDevice, DeviceMgr> enumerator = Managers.Values.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						((Disposable)enumerator.Current).Dispose();
					}
				}
				finally
				{
					((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
				RemoteDevice = null;
			}
		}

		private static readonly List<SESSION_ID> EmptyList = new List<SESSION_ID>();

		private static readonly TimeSpan MESSAGE_RETRY_TIME = TimeSpan.FromMilliseconds(500.0);

		private readonly List<SESSION_ID> SupportedSessionList = new List<SESSION_ID>();

		private SessionInstanceManager SessionInstances;

		private bool ReadingSessionsFromDevice;

		private ushort SessionIndex;

		private ushort ReportedCount;

		private Timer TxTimer = new Timer(true);

		public int Count
		{
			get
			{
				if (!DeviceQueryComplete)
				{
					return 0;
				}
				return SupportedSessionList.Count;
			}
		}

		[field: CompilerGenerated]
		public bool DeviceQueryComplete
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public ClientSessionManager(RemoteDevice device)
			: base(device)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			SessionInstances = new SessionInstanceManager(device);
			Clear();
			ReadingSessionsFromDevice = ((System.Enum)base.Adapter.Options).HasFlag((System.Enum)ADAPTER_OPTIONS.AUTO_READ_DEVICE_SESSION_LIST);
			((IEventSender)base.Adapter).Events.Subscribe<TransmitTurnEvent>((Action<TransmitTurnEvent>)OnTransmitNextMessage, (SubscriptionType)0, Subscriptions);
		}

		public override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				Clear();
				ReadingSessionsFromDevice = false;
				SessionInstances.CloseAll();
				((Disposable)SessionInstances).Dispose();
			}
		}

		public ISessionClient GetSession(ILocalDevice localhost, SESSION_ID session_id)
		{
			return SessionInstances.GetSession(localhost, session_id);
		}

		public System.Collections.Generic.IEnumerator<SESSION_ID> GetEnumerator()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			return (System.Collections.Generic.IEnumerator<SESSION_ID>)(object)(DeviceQueryComplete ? SupportedSessionList.GetEnumerator() : EmptyList.GetEnumerator());
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)GetEnumerator();
		}

		public void QueryDevice()
		{
			if (!DeviceQueryComplete)
			{
				ReadingSessionsFromDevice = true;
			}
		}

		public bool Contains(SESSION_ID id)
		{
			return SupportedSessionList.Contains(id);
		}

		private void Clear()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			TxTimer.ElapsedTime = TimeSpan.FromMilliseconds(-250.0);
			SessionIndex = 0;
			ClearList();
			base.Text = "Supported sessions: UNKNOWN";
			base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.QUESTION;
		}

		private void ClearList()
		{
			DeviceQueryComplete = false;
			SupportedSessionList.Clear();
		}

		public override void BackgroundTask()
		{
		}

		public override void OnDeviceTx(AdapterRxEvent tx)
		{
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			if (!base.Device.IsOnline || (byte)tx.MessageType != 129 || tx.TargetAddress != base.Adapter.LocalHost.Address || tx.MessageData != 64 || !ReadingSessionsFromDevice)
			{
				return;
			}
			if (tx.Count == 1)
			{
				ClearList();
				DeviceQueryComplete = true;
				ReadingSessionsFromDevice = false;
				ReportedCount = 0;
				RESPONSE rESPONSE = (RESPONSE)tx[0];
				base.Text = $"Supported sessions: {rESPONSE}";
				base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.EXCLAMATION;
			}
			else
			{
				if (tx.Count != 8 || CommExtensions.GetUINT16((IByteList)(object)tx, 0) != SessionIndex)
				{
					return;
				}
				int num = 2;
				int i = SessionIndex * 3;
				if (SessionIndex == 0)
				{
					ReportedCount = CommExtensions.GetUINT16((IByteList)(object)tx, num);
					ClearList();
					num += 2;
				}
				else
				{
					i--;
				}
				for (; i < ReportedCount; i++)
				{
					if (num >= 8)
					{
						break;
					}
					SESSION_ID val = SESSION_ID.op_Implicit(CommExtensions.GetUINT16((IByteList)(object)tx, num));
					if (val != SESSION_ID.UNKNOWN)
					{
						if (SupportedSessionList.Contains(val))
						{
							Clear();
							return;
						}
						SupportedSessionList.Add(val);
					}
					num += 2;
				}
				if (i >= ReportedCount)
				{
					SupportedSessionList.Sort((Comparison<SESSION_ID>)((SESSION_ID first, SESSION_ID second) => first.Value.CompareTo(second.Value)));
					DeviceQueryComplete = true;
					ReadingSessionsFromDevice = false;
					base.Text = $"Supported sessions: {Count}";
					base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.INFO;
				}
				SessionIndex++;
				TxTimer.ElapsedTime = TimeSpan.FromSeconds(1.0);
			}
		}

		private void OnTransmitNextMessage(TransmitTurnEvent message)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			if (ReadingSessionsFromDevice && base.Adapter.LocalHost.IsOnline && base.Device.IsOnline && !(TxTimer.ElapsedTime < MESSAGE_RETRY_TIME))
			{
				message.Handled = base.Adapter.LocalHost.Transmit29((byte)128, 64, base.Device, PAYLOAD.FromArgs(new object[1] { SessionIndex }));
				if (message.Handled)
				{
					TxTimer.Reset();
				}
			}
		}
	}
	public struct TEXT_CONSOLE_SIZE
	{
		public int Width;

		public int Height;

		public TEXT_CONSOLE_SIZE(int w, int h)
		{
			Width = w;
			Height = h;
		}

		public bool Equals(TEXT_CONSOLE_SIZE s)
		{
			if (Width == s.Width)
			{
				return Height == s.Height;
			}
			return false;
		}

		public static bool operator ==(TEXT_CONSOLE_SIZE s1, TEXT_CONSOLE_SIZE s2)
		{
			return s1.Equals(s2);
		}

		public static bool operator !=(TEXT_CONSOLE_SIZE s1, TEXT_CONSOLE_SIZE s2)
		{
			return !s1.Equals(s2);
		}

		public bool Equals(object obj)
		{
			if (obj is TEXT_CONSOLE_SIZE)
			{
				return Equals((TEXT_CONSOLE_SIZE)obj);
			}
			return false;
		}

		public int GetHashCode()
		{
			return (Width << 16) + Height;
		}

		public string ToString()
		{
			return Width + " x " + Height;
		}
	}
	public interface ITextConsole
	{
		IDevice Device { get; }

		bool IsDetected { get; }

		System.Collections.Generic.IReadOnlyList<string> Lines { get; }

		TEXT_CONSOLE_SIZE Size { get; }
	}
	public class TEXT_CONSOLE
	{
		private const ushort NULLCH = 32;

		public const int MAX_HEIGHT = 32;

		public const int MAX_WIDTH = 64;

		public static readonly ushort[] UnicodeCharset;

		static TEXT_CONSOLE()
		{
			ushort[] array = new ushort[256];
			RuntimeHelpers.InitializeArray((System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			UnicodeCharset = array;
		}
	}
	internal class RemoteTextConsole : RemoteDevice.ChildNode, ITextConsole
	{
		public struct ROWCOL
		{
			public readonly byte Value;

			public readonly int Row;

			public readonly int Column;

			public ROWCOL(byte value)
			{
				Value = value;
				Row = (value >> 3) & 0x1F;
				Column = (value & 7) * 8;
			}

			public string ToString()
			{
				return string.Concat(new string[5]
				{
					"[",
					Row.ToString(),
					",",
					Column.ToString(),
					"]"
				});
			}
		}

		private class FrameScanner
		{
			private enum STATE
			{
				RESYNC,
				GET_SIZE,
				WAIT_EOF
			}

			private ROWCOL mCurrentRowCol = new ROWCOL(0);

			private TEXT_CONSOLE_SIZE mStableSize = new TEXT_CONSOLE_SIZE(0, 0);

			private TEXT_CONSOLE_SIZE mNextFrameSize = new TEXT_CONSOLE_SIZE(0, 0);

			private STATE mState;

			private bool mSizeChanged;

			private Timer mFrameTimeout = new Timer(true);

			private Timer mMessageTimeout = new Timer(true);

			private STATE State
			{
				get
				{
					return mState;
				}
				set
				{
					if (mState != value)
					{
						mState = value;
					}
				}
			}

			public TEXT_CONSOLE_SIZE Size => mStableSize;

			public bool SizeChanged
			{
				get
				{
					bool result = mSizeChanged;
					mSizeChanged = false;
					return result;
				}
			}

			public bool CheckForTimeout()
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				if (mStableSize.Width <= 0 && mStableSize.Height <= 0)
				{
					return false;
				}
				if (mFrameTimeout.ElapsedTime < FRAME_TIMEOUT)
				{
					return false;
				}
				mStableSize = (mNextFrameSize = new TEXT_CONSOLE_SIZE(0, 0));
				State = STATE.RESYNC;
				mSizeChanged = true;
				return true;
			}

			public bool DetectFrame(ROWCOL rc, int payload_bytes)
			{
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				ROWCOL rOWCOL = mCurrentRowCol;
				mCurrentRowCol = rc;
				int num = rc.Row + 1;
				int num2 = rc.Column + payload_bytes;
				if (mMessageTimeout.ElapsedTime > MESSAGE_TIMEOUT)
				{
					State = STATE.RESYNC;
				}
				mMessageTimeout.Reset();
				switch (State)
				{
				case STATE.RESYNC:
					if (rc.Value == 0)
					{
						State = STATE.GET_SIZE;
					}
					return false;
				case STATE.GET_SIZE:
					mNextFrameSize = new TEXT_CONSOLE_SIZE(num2, num);
					State = STATE.WAIT_EOF;
					break;
				case STATE.WAIT_EOF:
					if (num2 > mNextFrameSize.Width || num > mNextFrameSize.Height)
					{
						State = STATE.RESYNC;
						return false;
					}
					if (rc.Value > rOWCOL.Value && (mNextFrameSize.Width != num2 || mNextFrameSize.Height != num))
					{
						State = STATE.RESYNC;
						return false;
					}
					break;
				}
				if (rc.Value > 0)
				{
					return false;
				}
				mFrameTimeout.Reset();
				mSizeChanged |= mStableSize != mNextFrameSize;
				mStableSize = mNextFrameSize;
				State = STATE.GET_SIZE;
				mNextFrameSize = new TEXT_CONSOLE_SIZE(0, 0);
				return true;
			}
		}

		[DefaultMember("Item")]
		private class LineBuilder
		{
			private readonly byte[] Char;

			private readonly byte[] UTF16;

			private readonly byte[] LatchedUTF16;

			private string mText = "";

			private bool mNeedsRebuild;

			[field: CompilerGenerated]
			public bool NeedsLatch
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			private int EOL
			{
				get
				{
					for (int i = 0; i < Char.Length; i++)
					{
						if (Char[i] == 0)
						{
							return i;
						}
					}
					return Char.Length - 1;
				}
			}

			public string Text
			{
				get
				{
					if (mNeedsRebuild)
					{
						lock (LatchedUTF16)
						{
							mText = Encoding.Unicode.GetString(LatchedUTF16, 0, EOL * 2);
							mNeedsRebuild = false;
						}
					}
					return mText;
				}
			}

			public byte this[int index]
			{
				get
				{
					return Char[index];
				}
				set
				{
					if (Char[index] != value)
					{
						Char[index] = value;
						ushort num = TEXT_CONSOLE.UnicodeCharset[value & 0xFF];
						UTF16[2 * index] = (byte)num;
						UTF16[2 * index + 1] = (byte)(num >> 8);
						NeedsLatch = true;
					}
				}
			}

			public LineBuilder(int capacity)
			{
				Char = new byte[capacity + 1];
				UTF16 = new byte[Char.Length * 2];
				LatchedUTF16 = new byte[UTF16.Length];
			}

			public void Latch()
			{
				if (NeedsLatch)
				{
					lock (LatchedUTF16)
					{
						System.Array.Copy((System.Array)UTF16, (System.Array)LatchedUTF16, UTF16.Length);
						NeedsLatch = false;
						mNeedsRebuild = true;
					}
				}
			}

			public void Clear()
			{
				lock (LatchedUTF16)
				{
					System.Array.Clear((System.Array)Char, 0, Char.Length);
					System.Array.Clear((System.Array)UTF16, 0, UTF16.Length);
					System.Array.Clear((System.Array)LatchedUTF16, 0, LatchedUTF16.Length);
					NeedsLatch = false;
					mText = "";
					mNeedsRebuild = false;
				}
			}
		}

		private static readonly TimeSpan FRAME_TIMEOUT = TimeSpan.FromSeconds(4.0);

		private static readonly TimeSpan MESSAGE_TIMEOUT = TimeSpan.FromSeconds(1.75);

		private static readonly string[] Empty = new string[0];

		private readonly TextConsoleSizeChangedEvent SizeChangedEvent;

		private readonly TextConsoleTextChangedEvent TextChangedEvent;

		private FrameScanner FrameScan = new FrameScanner();

		private LineBuilder[] Line = new LineBuilder[32];

		private string[] Strings = Empty;

		private bool RebuildStrings;

		IDevice ITextConsole.Device => base.Device;

		public bool IsDetected
		{
			get
			{
				TEXT_CONSOLE_SIZE size = Size;
				if (size.Width > 0)
				{
					return size.Height > 0;
				}
				return false;
			}
		}

		public System.Collections.Generic.IReadOnlyList<string> Lines
		{
			get
			{
				if (RebuildStrings)
				{
					lock (Line)
					{
						RebuildStrings = false;
						int height = Size.Height;
						if (Strings.Length != height)
						{
							Strings = new string[height];
						}
						for (int i = 0; i < Strings.Length; i++)
						{
							Strings[i] = Line[i].Text;
						}
					}
				}
				return Strings;
			}
		}

		public TEXT_CONSOLE_SIZE Size => FrameScan?.Size ?? default(TEXT_CONSOLE_SIZE);

		private bool AnyTextChanged
		{
			get
			{
				bool flag = false;
				int height = Size.Height;
				for (int i = 0; i < height; i++)
				{
					flag |= Line[i].NeedsLatch;
				}
				return flag;
			}
		}

		public RemoteTextConsole(RemoteDevice device)
			: base(device)
		{
			SizeChangedEvent = new TextConsoleSizeChangedEvent(device);
			TextChangedEvent = new TextConsoleTextChangedEvent(device);
			for (int i = 0; i < Line.Length; i++)
			{
				Line[i] = new LineBuilder(64);
			}
			Extensions.RemoveFromParent(TreeNode);
			base.Text = "Text console: not detected";
			base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.CROSS;
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				FrameScan = null;
				Strings = null;
				Line = null;
			}
		}

		private void RaiseSizeChanged()
		{
			if (Size.Height > 0)
			{
				base.Text = "Text console: " + ((object)Size/*cast due to .constrained prefix*/).ToString();
				base.Icon = IDS.Core.IDS_CAN.Adapter.ICON.CHECKMARK;
				if (TreeNode.Parent == null)
				{
					base.Device.TreeNode.AddChild(TreeNode);
				}
			}
			else
			{
				Extensions.RemoveFromParent(TreeNode);
			}
			((Event)SizeChangedEvent).Publish();
			((IEventSender)base.Adapter).Events.Publish<TextConsoleSizeChangedEvent>(SizeChangedEvent);
		}

		private void RaiseTextChanged()
		{
			((Event)TextChangedEvent).Publish();
			((IEventSender)base.Adapter).Events.Publish<TextConsoleTextChangedEvent>(TextChangedEvent);
		}

		public override void BackgroundTask()
		{
			if (!FrameScan.CheckForTimeout())
			{
				return;
			}
			lock (Line)
			{
				RebuildStrings = false;
				for (int i = 0; i < Line.Length; i++)
				{
					Line[i].Clear();
				}
				Strings = Empty;
			}
			RaiseSizeChanged();
			RaiseTextChanged();
		}

		public override void OnDeviceTx(AdapterRxEvent tx)
		{
			if ((byte)tx.MessageType != 132)
			{
				return;
			}
			ROWCOL rc = new ROWCOL(tx.MessageData);
			LineBuilder lineBuilder = Line[rc.Row];
			for (int i = 0; i < tx.Count; i++)
			{
				lineBuilder[rc.Column + i] = tx[i];
			}
			if (tx.Count < 8)
			{
				lineBuilder[rc.Column + tx.Count] = 0;
			}
			if (!FrameScan.DetectFrame(rc, tx.Count))
			{
				return;
			}
			bool sizeChanged = FrameScan.SizeChanged;
			bool flag = AnyTextChanged;
			if (sizeChanged)
			{
				RaiseSizeChanged();
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			lock (Line)
			{
				int height = Size.Height;
				for (int j = 0; j < height; j++)
				{
					Line[j].Latch();
				}
				RebuildStrings = true;
			}
			RaiseTextChanged();
		}
	}
	public class LocalTextConsole : Disposable, ITextConsole
	{
		private static readonly TimeSpan FRAME_TRANSMIT_PERIOD = TimeSpan.FromSeconds(1.0);

		private PeriodicTask mBackgroundTask;

		private readonly string[] _strings;

		private readonly byte[][] _bytes;

		private readonly int _maxColIndex = -1;

		private readonly int _maxRowIndex;

		private bool _restart = true;

		private Timer TimeSinceLastFrameTx = new Timer(true);

		private int txRow = -1;

		private int txCol = -1;

		public bool IsDetected => true;

		IDevice ITextConsole.Device => Device;

		[field: CompilerGenerated]
		public ILocalDevice Device
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public TEXT_CONSOLE_SIZE Size
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public System.Collections.Generic.IReadOnlyList<string> Lines
		{
			get
			{
				return _strings;
			}
			set
			{
				for (int i = 0; i < _strings.Length; i++)
				{
					string text = string.Empty;
					if (i < ((System.Collections.Generic.IReadOnlyCollection<string>)value)?.Count)
					{
						text = value[i];
					}
					if (text == null)
					{
						text = string.Empty;
					}
					if (text != _strings[i])
					{
						lock (_strings)
						{
							_strings[i] = text;
							_bytes[i] = null;
							_restart = true;
						}
					}
				}
			}
		}

		public LocalTextConsole(ILocalDevice localDevice, TEXT_CONSOLE_SIZE size)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			Device = localDevice;
			Size = size;
			_maxColIndex = size.Width / 8 - 1;
			if (size.Width % 8 > 0)
			{
				_maxColIndex++;
			}
			_maxRowIndex = size.Height - 1;
			_strings = new string[size.Height];
			_bytes = new byte[size.Height][];
			for (int i = 0; i < _strings.Length; i++)
			{
				_strings[i] = string.Empty;
			}
			mBackgroundTask = new PeriodicTask(new Action(BackgroundTask), TimeSpan.FromMilliseconds(5.0), TimeSpan.FromMilliseconds(500.0), (Type)0, true);
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((Disposable)mBackgroundTask).Dispose();
			}
		}

		private void BackgroundTask()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (!Device.IsOnline)
			{
				return;
			}
			if (_restart || TimeSinceLastFrameTx.ElapsedTime >= FRAME_TRANSMIT_PERIOD)
			{
				txRow = _maxRowIndex;
				txCol = _maxColIndex;
				_restart = false;
				TimeSinceLastFrameTx.Reset();
			}
			if (txRow >= 0 && TransmitTextMessage(txRow, txCol))
			{
				if (txCol > 0)
				{
					txCol--;
					return;
				}
				txCol = _maxColIndex;
				txRow--;
			}
		}

		private byte ByteAt(int x, int y)
		{
			if (y >= _strings.Length)
			{
				return 32;
			}
			if (_strings[y] == null)
			{
				return 32;
			}
			if (_strings[y] == string.Empty)
			{
				return 32;
			}
			byte[] array = _bytes[y];
			if (array == null)
			{
				array = (_bytes[y] = Encoding.UTF8.GetBytes(_strings[y]));
			}
			if (x >= array.Length)
			{
				return 32;
			}
			return array[x];
		}

		private bool TransmitTextMessage(int txRow, int txCol)
		{
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			lock (_strings)
			{
				int num = txCol * 8;
				int num2 = (txCol + 1) * 8;
				int num3 = 8;
				if (num2 > Size.Width)
				{
					num3 = 8 - (num2 - Size.Width);
				}
				PAYLOAD payload = default(PAYLOAD);
				((PAYLOAD)(ref payload))..ctor(num3);
				for (int i = 0; i < ((PAYLOAD)(ref payload)).Length; i++)
				{
					((PAYLOAD)(ref payload))[i] = ByteAt(num + i, txRow);
				}
				byte ext_data = (byte)((txRow << 3) | txCol);
				return Device.Transmit29((byte)132, ext_data, ADDRESS.BROADCAST, payload);
			}
		}
	}
}
namespace IDS.Core.IDS_CAN.Devices
{
	public class AC_POWER_MONITOR : LocalDevice
	{
		private float _measuredVoltage;

		private float _measuredCurrent;

		private byte _powerQuality;

		[field: CompilerGenerated]
		public uint SHORE_POWER_AMP_RATING
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public float MeasuredVoltage
		{
			get
			{
				return _measuredVoltage;
			}
			set
			{
				if (_measuredVoltage != value)
				{
					_measuredVoltage = value;
					UpdateStatus();
				}
			}
		}

		public float MeasuredCurrent
		{
			get
			{
				return _measuredCurrent;
			}
			set
			{
				if (_measuredCurrent != value)
				{
					_measuredCurrent = value;
					UpdateStatus();
				}
			}
		}

		public byte PowerQuality
		{
			get
			{
				return _powerQuality;
			}
			set
			{
				if (_powerQuality != value)
				{
					_powerQuality = value;
					UpdateStatus();
				}
			}
		}

		public bool NotAcceptingCommands
		{
			get
			{
				return base.IsNotAcceptingCommands;
			}
			set
			{
				base.IsNotAcceptingCommands = value;
			}
		}

		public AC_POWER_MONITOR(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), DEVICE_TYPE.op_Implicit((byte)26), 0, FUNCTION_NAME.op_Implicit((ushort)259), 0, (byte)0, options)
		{
			Init();
		}

		private void Init()
		{
			AddPID(PID.SHORE_POWER_AMP_RATING, [CompilerGenerated] () => UInt48.op_Implicit(SHORE_POWER_AMP_RATING), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				SHORE_POWER_AMP_RATING = (uint)arg;
			});
			UpdateStatus();
		}

		private void UpdateStatus()
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			int num = (int)Math.Round((double)(MeasuredVoltage * 256f));
			if (num > 2147483647)
			{
				num = 32767;
			}
			if (num < -2147483648)
			{
				num = -32768;
			}
			int num2 = (int)Math.Round((double)(MeasuredCurrent * 256f));
			if (num2 > 2147483647)
			{
				num2 = 2147483647;
			}
			if (num2 < -2147483648)
			{
				num2 = -2147483648;
			}
			base.DeviceStatus = PAYLOAD.FromArgs(new object[3]
			{
				(short)num,
				(short)num2,
				PowerQuality
			});
		}
	}
	public class AWNING_SENSOR_STATUS_PARAMS : IDeviceStatusParams
	{
		private PAYLOAD payload;

		private ushort mAngle;

		[DeviceDisplay("Angle")]
		public string AngleDisplay
		{
			get
			{
				double num = (double)(short)mAngle / 128.0;
				return $"{num:0.00}°";
			}
		}

		public ushort Angle
		{
			get
			{
				return mAngle;
			}
			set
			{
				if (mAngle != value)
				{
					mAngle = value;
					((PAYLOAD)(ref payload))[0] = (byte)(value >> 8);
					((PAYLOAD)(ref payload))[1] = (byte)(value & 0xFF);
				}
			}
		}

		[DeviceDisplay("High Wind Alert")]
		public string HighWindAlertDisplay
		{
			get
			{
				bool flag = (HighWindAlert & 0x80) != 0;
				int num = HighWindAlert & 0x7F;
				return $"Active = {flag}, Count = {num}";
			}
		}

		public byte HighWindAlert
		{
			get
			{
				return ((PAYLOAD)(ref payload))[2];
			}
			set
			{
				((PAYLOAD)(ref payload))[2] = value;
			}
		}

		[DeviceDisplay("Medium Wind Alert")]
		public string MediumWindAlertDisplay
		{
			get
			{
				bool flag = (MediumWindAlert & 0x80) != 0;
				int num = MediumWindAlert & 0x7F;
				return $"Active = {flag}, Count = {num}";
			}
		}

		public byte MediumWindAlert
		{
			get
			{
				return ((PAYLOAD)(ref payload))[3];
			}
			set
			{
				((PAYLOAD)(ref payload))[3] = value;
			}
		}

		[DeviceDisplay("Low Wind Alert")]
		public string LowWindAlertDisplay
		{
			get
			{
				bool flag = (LowWindAlert & 0x80) != 0;
				int num = LowWindAlert & 0x7F;
				return $"Active = {flag}, Count = {num}";
			}
		}

		public byte LowWindAlert
		{
			get
			{
				return ((PAYLOAD)(ref payload))[4];
			}
			set
			{
				((PAYLOAD)(ref payload))[4] = value;
			}
		}

		[DeviceDisplay("Battery Level")]
		public string BatteryLevelDisplay => $"{BatteryLevel}%";

		public byte BatteryLevel
		{
			get
			{
				return ((PAYLOAD)(ref payload))[5];
			}
			set
			{
				((PAYLOAD)(ref payload))[5] = value;
			}
		}

		[DeviceDisplay("Low Battery Alert")]
		public string LowBattAlertDisplay
		{
			get
			{
				bool flag = (LowBattAlert & 0x80) != 0;
				int num = LowBattAlert & 0x7F;
				return $"Active = {flag}, Count = {num}";
			}
		}

		public byte LowBattAlert
		{
			get
			{
				return ((PAYLOAD)(ref payload))[6];
			}
			set
			{
				((PAYLOAD)(ref payload))[6] = value;
			}
		}

		[DeviceDisplay("Misc Status")]
		public string MiscStatusDisplay => "Audible Alert = " + (MiscStatus & 3) switch
		{
			0 => "not sounding", 
			1 => "Pre-movement active", 
			2 => "Intra-movement active", 
			_ => "Reserved", 
		} + "\nPerceived Awning State = " + ((MiscStatus & 0xC) >> 2) switch
		{
			0 => "Unknown/Unavailable", 
			1 => "Extending", 
			2 => "Retracting", 
			_ => "Stationary", 
		};

		public byte MiscStatus
		{
			get
			{
				return ((PAYLOAD)(ref payload))[7];
			}
			set
			{
				((PAYLOAD)(ref payload))[7] = value;
			}
		}

		public AWNING_SENSOR_STATUS_PARAMS()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			payload = new PAYLOAD(8);
			Angle = 0;
			HighWindAlert = 0;
			MediumWindAlert = 0;
			LowWindAlert = 0;
			BatteryLevel = 0;
			LowBattAlert = 0;
			MiscStatus = 0;
		}

		public void SetPayload(PAYLOAD pl)
		{
			if (((PAYLOAD)(ref pl)).Length >= 2)
			{
				Angle = (ushort)((((PAYLOAD)(ref pl))[0] << 8) | ((PAYLOAD)(ref pl))[1]);
			}
			if (((PAYLOAD)(ref pl)).Length >= 3)
			{
				HighWindAlert = ((PAYLOAD)(ref pl))[2];
			}
			if (((PAYLOAD)(ref pl)).Length >= 4)
			{
				MediumWindAlert = ((PAYLOAD)(ref pl))[3];
			}
			if (((PAYLOAD)(ref pl)).Length >= 5)
			{
				LowWindAlert = ((PAYLOAD)(ref pl))[4];
			}
			if (((PAYLOAD)(ref pl)).Length >= 6)
			{
				BatteryLevel = ((PAYLOAD)(ref pl))[5];
			}
			if (((PAYLOAD)(ref pl)).Length >= 7)
			{
				LowBattAlert = ((PAYLOAD)(ref pl))[6];
			}
			if (((PAYLOAD)(ref pl)).Length >= 8)
			{
				MiscStatus = ((PAYLOAD)(ref pl))[7];
			}
		}

		public PAYLOAD GetPayload()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return payload;
		}
	}
	public class AWNING_SENSOR_SIMULATOR_INTERFACE : LocalDevice
	{
		private AWNING_SENSOR_STATUS_PARAMS mStatus;

		public ushort AngleDegrees
		{
			get
			{
				return mStatus.Angle;
			}
			set
			{
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				if (mStatus.Angle != value)
				{
					mStatus.Angle = value;
					base.DeviceStatus = mStatus.GetPayload();
				}
			}
		}

		public byte HighWindAlert
		{
			get
			{
				return mStatus.HighWindAlert;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.HighWindAlert = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public byte MediumWindAlert
		{
			get
			{
				return mStatus.MediumWindAlert;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.MediumWindAlert = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public byte LowWindAlert
		{
			get
			{
				return mStatus.LowWindAlert;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.LowWindAlert = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public byte BatteryLevel
		{
			get
			{
				return mStatus.BatteryLevel;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.BatteryLevel = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public byte LowBattAlert
		{
			get
			{
				return mStatus.LowBattAlert;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.LowBattAlert = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public byte MiscStatus
		{
			get
			{
				return mStatus.MiscStatus;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.MiscStatus = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public AWNING_SENSOR_SIMULATOR_INTERFACE(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), DEVICE_TYPE.op_Implicit((byte)47), 0, FUNCTION_NAME.op_Implicit((ushort)361), 0, (byte)0, options)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			mStatus = new AWNING_SENSOR_STATUS_PARAMS();
			base.DeviceStatus = mStatus.GetPayload();
		}
	}
	public class DC_POWER_MONITOR : LocalDevice
	{
		private float _measuredVoltage;

		private float _measuredCurrent;

		private byte _chargingCapacity;

		private bool _isDischarging;

		private ushort _estimatedTimeToDischarge;

		[field: CompilerGenerated]
		public uint BATTERY_CAPACITY_AMP_HOURS
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public float MeasuredVoltage
		{
			get
			{
				return _measuredVoltage;
			}
			set
			{
				if (_measuredVoltage != value)
				{
					_measuredVoltage = value;
					UpdateStatus();
				}
			}
		}

		public float MeasuredCurrent
		{
			get
			{
				return _measuredCurrent;
			}
			set
			{
				if (_measuredCurrent != value)
				{
					_measuredCurrent = value;
					UpdateStatus();
				}
			}
		}

		public byte ChargingCapacity
		{
			get
			{
				return _chargingCapacity;
			}
			set
			{
				if (_chargingCapacity != value)
				{
					_chargingCapacity = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDischarging
		{
			get
			{
				return _isDischarging;
			}
			set
			{
				if (_isDischarging != value)
				{
					_isDischarging = value;
					UpdateStatus();
				}
			}
		}

		public ushort EstimatedTimeToDischarge
		{
			get
			{
				return _estimatedTimeToDischarge;
			}
			set
			{
				if (_estimatedTimeToDischarge != value)
				{
					_estimatedTimeToDischarge = value;
					UpdateStatus();
				}
			}
		}

		public bool NotAcceptingCommands
		{
			get
			{
				return base.IsNotAcceptingCommands;
			}
			set
			{
				base.IsNotAcceptingCommands = value;
			}
		}

		public DC_POWER_MONITOR(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), DEVICE_TYPE.op_Implicit((byte)27), 0, FUNCTION_NAME.op_Implicit((ushort)255), 0, (byte)0, options)
		{
			Init();
		}

		private void Init()
		{
			AddPID(PID.BATTERY_CAPACITY_AMP_HOURS, [CompilerGenerated] () => UInt48.op_Implicit(BATTERY_CAPACITY_AMP_HOURS), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				BATTERY_CAPACITY_AMP_HOURS = (uint)arg;
			});
			IsDischarging = true;
			UpdateStatus();
		}

		private void UpdateStatus()
		{
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			int num = (int)Math.Round((double)(MeasuredVoltage * 256f));
			if (num > 2147483647)
			{
				num = 32767;
			}
			if (num < -2147483648)
			{
				num = -32768;
			}
			int num2 = (int)Math.Round((double)(MeasuredCurrent * 256f));
			if (num2 > 2147483647)
			{
				num2 = 2147483647;
			}
			if (num2 < -2147483648)
			{
				num2 = -2147483648;
			}
			byte b = ChargingCapacity;
			if (!IsDischarging)
			{
				b |= 0x80;
			}
			base.DeviceStatus = PAYLOAD.FromArgs(new object[4]
			{
				(ushort)num,
				(ushort)num2,
				b,
				EstimatedTimeToDischarge
			});
		}
	}
	public enum DIMMABLE_MODE : byte
	{
		OFF_00 = 0,
		ON_01 = 1,
		BLINK_02 = 2,
		SWELL_TRIANGLE_03 = 3,
		RESTORE_7F = 127
	}
	public class DIMMABLE_LIGHT_STATUS_PARAMS : IDeviceStatusParams
	{
		private PAYLOAD payload;

		[DeviceDisplay("Mode")]
		public string ModeDisplay => Mode switch
		{
			0 => "OFF", 
			1 => "ON/DIMMING", 
			2 => "BLINK", 
			3 => "SWELL_TRIANGLE", 
			_ => "INVALID", 
		};

		public byte Mode
		{
			get
			{
				return ((PAYLOAD)(ref payload))[0];
			}
			set
			{
				((PAYLOAD)(ref payload))[0] = value;
			}
		}

		[DeviceDisplay("Max Brightness")]
		public string MaxBrightnessDisplay
		{
			get
			{
				if (MaxBrightness == 0)
				{
					return "Off";
				}
				return $"{MaxBrightness}";
			}
		}

		public byte MaxBrightness
		{
			get
			{
				return ((PAYLOAD)(ref payload))[1];
			}
			set
			{
				((PAYLOAD)(ref payload))[1] = value;
			}
		}

		[DeviceDisplay("Sleep Timer")]
		public string SleepTimerDisplay
		{
			get
			{
				if (AutoOffTimeStatus == 0)
				{
					return "Infinite (disabled)";
				}
				return $"{AutoOffTimeStatus} minutes";
			}
		}

		public byte AutoOffTimeStatus
		{
			get
			{
				return ((PAYLOAD)(ref payload))[2];
			}
			set
			{
				((PAYLOAD)(ref payload))[2] = value;
			}
		}

		[DeviceDisplay("Current Brightness")]
		public string CurrentBrightnessDisplay
		{
			get
			{
				if (CurrentBrightness == 0)
				{
					return "Off";
				}
				return $"{CurrentBrightness}";
			}
		}

		public byte CurrentBrightness
		{
			get
			{
				return ((PAYLOAD)(ref payload))[3];
			}
			set
			{
				((PAYLOAD)(ref payload))[3] = value;
			}
		}

		[DeviceDisplay("Cycle Time T1")]
		public string T1Display => $"{T1} ms";

		public ushort T1
		{
			get
			{
				return (ushort)((((PAYLOAD)(ref payload))[4] << 8) | ((PAYLOAD)(ref payload))[5]);
			}
			set
			{
				((PAYLOAD)(ref payload))[4] = (byte)(value >> 8);
				((PAYLOAD)(ref payload))[5] = (byte)(value & 0xFF);
			}
		}

		[DeviceDisplay("Cycle Time T2")]
		public string T2Display => $"{T2} ms";

		public ushort T2
		{
			get
			{
				return (ushort)((((PAYLOAD)(ref payload))[6] << 8) | ((PAYLOAD)(ref payload))[7]);
			}
			set
			{
				((PAYLOAD)(ref payload))[6] = (byte)(value >> 8);
				((PAYLOAD)(ref payload))[7] = (byte)(value & 0xFF);
			}
		}

		public DIMMABLE_LIGHT_STATUS_PARAMS()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			payload = new PAYLOAD(8);
			Mode = 0;
			MaxBrightness = 0;
			AutoOffTimeStatus = 0;
			CurrentBrightness = 0;
			T1 = 0;
			T2 = 0;
		}

		public void SetPayload(PAYLOAD pl)
		{
			if (((PAYLOAD)(ref pl)).Length >= 1)
			{
				Mode = ((PAYLOAD)(ref pl))[0];
			}
			if (((PAYLOAD)(ref pl)).Length >= 2)
			{
				MaxBrightness = ((PAYLOAD)(ref pl))[1];
			}
			if (((PAYLOAD)(ref pl)).Length >= 3)
			{
				AutoOffTimeStatus = ((PAYLOAD)(ref pl))[2];
			}
			if (((PAYLOAD)(ref pl)).Length >= 4)
			{
				CurrentBrightness = ((PAYLOAD)(ref pl))[3];
			}
			if (((PAYLOAD)(ref pl)).Length >= 6)
			{
				T1 = (ushort)((((PAYLOAD)(ref pl))[4] << 8) | ((PAYLOAD)(ref pl))[5]);
			}
			if (((PAYLOAD)(ref pl)).Length >= 8)
			{
				T2 = (ushort)((((PAYLOAD)(ref pl))[6] << 8) | ((PAYLOAD)(ref pl))[7]);
			}
		}

		public PAYLOAD GetPayload()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return payload;
		}
	}
	public class DIMMABLE_LIGHT_SIMULATOR_INTERFACE : LocalDevice
	{
		private DIMMABLE_LIGHT_STATUS_PARAMS mStatus;

		private Timer DimTimer = new Timer(true);

		private Timer SleepTimer = new Timer(true);

		private DIMMABLE_MODE LastActiveMode = DIMMABLE_MODE.ON_01;

		private TimeSpan AutoOffTimeThreshold = TimeSpan.FromMinutes(255.0);

		private DIMMABLE_MODE Simulator_Mode
		{
			get
			{
				return (DIMMABLE_MODE)mStatus.Mode;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.Mode = (byte)value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		private byte Simulator_MaxBrightness
		{
			get
			{
				return mStatus.MaxBrightness;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.MaxBrightness = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		private byte Simulator_AutoOffTime
		{
			get
			{
				return mStatus.AutoOffTimeStatus;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.AutoOffTimeStatus = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		private byte Simulator_CurrentBrightness
		{
			get
			{
				return mStatus.CurrentBrightness;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.CurrentBrightness = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		private ushort Simulator_T1
		{
			get
			{
				return mStatus.T1;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.T1 = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		private ushort Simulator_T2
		{
			get
			{
				return mStatus.T2;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.T2 = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public DIMMABLE_LIGHT_SIMULATOR_INTERFACE(IAdapter adapter, FUNCTION_NAME name, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), DEVICE_TYPE.op_Implicit((byte)20), 0, name, 0, (byte)0, options)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			mStatus = new DIMMABLE_LIGHT_STATUS_PARAMS();
			base.DeviceStatus = mStatus.GetPayload();
		}

		protected override void OnLocalDeviceRxEvent(AdapterRxEvent rx)
		{
			base.OnLocalDeviceRxEvent(rx);
			if (!((Disposable)this).IsDisposed && base.Address.IsValidDeviceAddress && rx.TargetAddress == base.Address && (byte)rx.MessageType == 130 && rx.Count == 8 && rx.SourceAddress == GetLocalSessionClientAddress(SESSION_ID.op_Implicit((ushort)4)))
			{
				Simulator_Mode = UnloadCommandMessage(rx);
				if (Simulator_Mode != DIMMABLE_MODE.OFF_00 && Simulator_Mode != DIMMABLE_MODE.RESTORE_7F)
				{
					LastActiveMode = Simulator_Mode;
				}
			}
		}

		private DIMMABLE_MODE UnloadCommandMessage(AdapterRxEvent rx)
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			DIMMABLE_MODE dIMMABLE_MODE = (DIMMABLE_MODE)rx[0];
			switch (dIMMABLE_MODE)
			{
			case DIMMABLE_MODE.OFF_00:
				return DIMMABLE_MODE.OFF_00;
			case DIMMABLE_MODE.RESTORE_7F:
				if (Simulator_Mode == DIMMABLE_MODE.OFF_00)
				{
					SleepTimer.Reset();
				}
				return LastActiveMode;
			default:
				if (rx[1] == 0)
				{
					return DIMMABLE_MODE.OFF_00;
				}
				if (Simulator_MaxBrightness != rx[1])
				{
					Simulator_MaxBrightness = rx[1];
				}
				if (rx[2] != 0)
				{
					AutoOffTimeThreshold = TimeSpan.FromMinutes((double)(int)rx[2]);
					SleepTimer.Reset();
				}
				if (dIMMABLE_MODE == DIMMABLE_MODE.BLINK_02 || dIMMABLE_MODE == DIMMABLE_MODE.SWELL_TRIANGLE_03)
				{
					Simulator_T1 = (ushort)((rx[3] << 8) | rx[4]);
					Simulator_T2 = (ushort)((rx[5] << 8) | rx[6]);
				}
				return dIMMABLE_MODE;
			}
		}

		protected override void OnBackgroundTask()
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			base.OnBackgroundTask();
			if (Simulator_Mode != DIMMABLE_MODE.OFF_00 && ((TimeSpan)(ref AutoOffTimeThreshold)).Ticks > 0 && Simulator_AutoOffTime > 0 && SleepTimer.ElapsedTime >= AutoOffTimeThreshold)
			{
				Simulator_Mode = DIMMABLE_MODE.OFF_00;
			}
			TimeSpan val = DimTimer.ElapsedTime;
			double num = ((TimeSpan)(ref val)).TotalSeconds;
			if (num >= (double)(Simulator_T1 + Simulator_T2))
			{
				DimTimer.Reset();
				num = 0.0;
			}
			switch (Simulator_Mode)
			{
			case DIMMABLE_MODE.OFF_00:
				Simulator_CurrentBrightness = 0;
				break;
			case DIMMABLE_MODE.ON_01:
				Simulator_CurrentBrightness = Simulator_MaxBrightness;
				break;
			case DIMMABLE_MODE.BLINK_02:
				Simulator_CurrentBrightness = (byte)((num <= (double)(int)Simulator_T1) ? Simulator_MaxBrightness : 0);
				break;
			case DIMMABLE_MODE.SWELL_TRIANGLE_03:
				if (num < (double)(int)Simulator_T1)
				{
					Simulator_CurrentBrightness = (byte)((double)(int)Simulator_MaxBrightness * num / (double)(int)Simulator_T1);
					break;
				}
				num -= (double)(int)Simulator_T1;
				Simulator_CurrentBrightness = (byte)((double)(int)Simulator_MaxBrightness * ((double)(int)Simulator_T2 - num) / (double)(int)Simulator_T2);
				break;
			}
			if (Simulator_Mode == DIMMABLE_MODE.OFF_00)
			{
				Simulator_AutoOffTime = 0;
				return;
			}
			if (((TimeSpan)(ref AutoOffTimeThreshold)).Ticks == 0L)
			{
				Simulator_AutoOffTime = 0;
				return;
			}
			val = AutoOffTimeThreshold - SleepTimer.ElapsedTime;
			int num2 = (int)((TimeSpan)(ref val)).TotalMinutes;
			if (num2 <= 0)
			{
				num2 = 0;
			}
			if (num2 >= 254)
			{
				num2 = 254;
			}
			Simulator_AutoOffTime = (byte)num2;
		}
	}
	public enum GENERATOR_GENIE_STATE : byte
	{
		OFF,
		PRIMING,
		STARTING,
		RUNNING,
		STOPPING
	}
	public class GENERATOR_GENIE : LocalDevice
	{
		public enum COMMAND_MODE : byte
		{
			OFF = 1,
			ON,
			PRIME
		}

		private struct COMMAND
		{
			private int Value;

			public COMMAND_MODE Option
			{
				get
				{
					return (COMMAND_MODE)GetBits(3, 6);
				}
				set
				{
					SetBits((int)value, 3, 6);
				}
			}

			private bool GetBit(byte bit)
			{
				return (Value & bit) != 0;
			}

			private void SetBit(bool val, byte bit)
			{
				if (val)
				{
					Value |= bit;
				}
				else
				{
					Value &= ~bit;
				}
			}

			private int GetBits(int bit, int shift)
			{
				return (Value >> shift) & bit;
			}

			private void SetBits(int val, int bit, int shift)
			{
				val <<= shift;
				bit <<= shift;
				Value = (Value & ~bit) | (val & bit);
			}

			public static implicit operator int(COMMAND s)
			{
				return s.Value;
			}

			public static implicit operator byte(COMMAND s)
			{
				return (byte)s.Value;
			}

			public static implicit operator COMMAND(byte b)
			{
				COMMAND result = default(COMMAND);
				result.Value = b;
				return result;
			}

			public static implicit operator COMMAND(int i)
			{
				COMMAND result = default(COMMAND);
				result.Value = i;
				return result;
			}
		}

		private GENERATOR_GENIE_STATE _state;

		private bool _hasTemperatureSensor;

		private bool _isTemperatureSensorNotSupported;

		private float _batteryVoltage;

		private float _temperature_C;

		private COMMAND _command = 0;

		[field: CompilerGenerated]
		public ushort GENERATOR_QUIET_HOURS_START_TIME
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ushort GENERATOR_QUIET_HOURS_END_TIME
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public byte GENERATOR_QUIET_HOURS_ENABLED
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public int GENERATOR_AUTO_START_LOW_VOLTAGE
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public int GENERATOR_AUTO_START_HI_TEMP_C
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ushort GENERATOR_AUTO_RUN_DURATION_MINUTES
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public ushort GENERATOR_AUTO_RUN_MIN_OFF_TIME_MINUTES
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public bool NotAcceptingCommands
		{
			get
			{
				return base.IsNotAcceptingCommands;
			}
			set
			{
				base.IsNotAcceptingCommands = value;
			}
		}

		public GENERATOR_GENIE_STATE State
		{
			get
			{
				return _state;
			}
			set
			{
				if (_state != value)
				{
					_state = value;
					UpdateStatus();
				}
			}
		}

		public bool HasTemperatureSensor
		{
			get
			{
				return _hasTemperatureSensor;
			}
			set
			{
				if (_hasTemperatureSensor != value)
				{
					_hasTemperatureSensor = value;
					UpdateStatus();
				}
			}
		}

		public bool IsTemperatureSensorNotSupported
		{
			get
			{
				return _isTemperatureSensorNotSupported;
			}
			set
			{
				if (_isTemperatureSensorNotSupported != value)
				{
					_isTemperatureSensorNotSupported = value;
					UpdateStatus();
				}
			}
		}

		public float BatteryVoltage
		{
			get
			{
				return _batteryVoltage;
			}
			set
			{
				if (_batteryVoltage != value)
				{
					_batteryVoltage = value;
					UpdateStatus();
				}
			}
		}

		public float Temperature_C
		{
			get
			{
				return _temperature_C;
			}
			set
			{
				if (_temperature_C != value)
				{
					_temperature_C = value;
					UpdateStatus();
				}
			}
		}

		private COMMAND Command
		{
			get
			{
				return _command;
			}
			set
			{
				if ((int)_command != (int)value)
				{
					_command = value;
					switch ((uint)(byte)_command)
					{
					case 1u:
						State = GENERATOR_GENIE_STATE.OFF;
						break;
					case 2u:
						State = GENERATOR_GENIE_STATE.STARTING;
						break;
					case 3u:
						State = GENERATOR_GENIE_STATE.PRIMING;
						break;
					}
					UpdateStatus();
				}
			}
		}

		public GENERATOR_GENIE(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), DEVICE_TYPE.op_Implicit((byte)24), 0, FUNCTION_NAME.op_Implicit((ushort)95), 0, (byte)0, options)
		{
			Init();
		}

		private void Init()
		{
			BatteryVoltage = 0f;
			AddPID(PID.GENERATOR_QUIET_HOURS_START_TIME, [CompilerGenerated] () => UInt48.op_Implicit(GENERATOR_QUIET_HOURS_START_TIME), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				GENERATOR_QUIET_HOURS_START_TIME = (ushort)arg;
			});
			AddPID(PID.GENERATOR_QUIET_HOURS_END_TIME, [CompilerGenerated] () => UInt48.op_Implicit(GENERATOR_QUIET_HOURS_END_TIME), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				GENERATOR_QUIET_HOURS_END_TIME = (ushort)arg;
			});
			AddPID(PID.GENERATOR_QUIET_HOURS_ENABLED, [CompilerGenerated] () => UInt48.op_Implicit(GENERATOR_QUIET_HOURS_ENABLED), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				GENERATOR_QUIET_HOURS_ENABLED = (byte)arg;
			});
			AddPID(PID.GENERATOR_AUTO_START_LOW_VOLTAGE, [CompilerGenerated] () => UInt48.op_Implicit((uint)GENERATOR_AUTO_START_LOW_VOLTAGE), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				GENERATOR_AUTO_START_LOW_VOLTAGE = (int)arg;
			});
			AddPID(PID.GENERATOR_AUTO_START_HI_TEMP_C, [CompilerGenerated] () => UInt48.op_Implicit((uint)GENERATOR_AUTO_START_HI_TEMP_C), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				GENERATOR_AUTO_START_HI_TEMP_C = (int)arg;
			});
			AddPID(PID.GENERATOR_AUTO_RUN_DURATION_MINUTES, [CompilerGenerated] () => UInt48.op_Implicit(GENERATOR_AUTO_RUN_DURATION_MINUTES), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				GENERATOR_AUTO_RUN_DURATION_MINUTES = (ushort)arg;
			});
			AddPID(PID.GENERATOR_AUTO_RUN_MIN_OFF_TIME_MINUTES, [CompilerGenerated] () => UInt48.op_Implicit(GENERATOR_AUTO_RUN_MIN_OFF_TIME_MINUTES), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				GENERATOR_AUTO_RUN_MIN_OFF_TIME_MINUTES = (ushort)arg;
			});
			UpdateStatus();
		}

		private void UpdateStatus()
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			int num = (int)Math.Round((double)(BatteryVoltage * 256f));
			if (num > 32767)
			{
				num = 32767;
			}
			if (num < -32768)
			{
				num = -32768;
			}
			int num2 = (IsTemperatureSensorNotSupported ? 32768 : ((!HasTemperatureSensor) ? 32767 : ((int)Math.Round((double)(Temperature_C * 256f)))));
			base.DeviceStatus = PAYLOAD.FromArgs(new object[3]
			{
				(byte)State,
				(short)num,
				(short)num2
			});
		}

		protected override void OnLocalDeviceRxEvent(AdapterRxEvent rx)
		{
			base.OnLocalDeviceRxEvent(rx);
			if ((byte)rx.MessageType == 130 && rx.TargetAddress == base.Address && rx.SourceAddress == GetLocalSessionClientAddress(SESSION_ID.op_Implicit((ushort)4)) && rx.Count == 1)
			{
				Command = rx[0];
			}
		}
	}
	public class LATCHING_H_BRIDGE_TYPE_2_SIM_INTERFACE : RELAY_TYPE_2_SIM_INTERFACE
	{
		public enum COMMAND_MODE : byte
		{
			STOP,
			FORWARD,
			REVERSE,
			CLEAR_OUTPUT_DISABLED_LATCH
		}

		private struct COMMAND
		{
			private int Value;

			public COMMAND_MODE Option
			{
				get
				{
					return (COMMAND_MODE)GetBits(3, 6);
				}
				set
				{
					SetBits((int)value, 3, 6);
				}
			}

			private int GetBits(int bit, int shift)
			{
				return (Value >> shift) & bit;
			}

			private void SetBits(int val, int bit, int shift)
			{
				val <<= shift;
				bit <<= shift;
				Value = (Value & ~bit) | (val & bit);
			}

			public static implicit operator int(COMMAND s)
			{
				return s.Value;
			}

			public static implicit operator byte(COMMAND s)
			{
				return (byte)s.Value;
			}

			public static implicit operator COMMAND(byte b)
			{
				COMMAND result = default(COMMAND);
				result.Value = b;
				return result;
			}

			public static implicit operator COMMAND(int i)
			{
				COMMAND result = default(COMMAND);
				result.Value = i;
				return result;
			}
		}

		private COMMAND _command = 0;

		private COMMAND Command
		{
			get
			{
				return _command;
			}
			set
			{
				_command = value;
				switch ((uint)(byte)_command)
				{
				case 0u:
					if (!base.OutputDisabledLatch && (base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.FORWARD_EXTEND_CLOCKWISE_OUT_UP || base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.REVERSE_RETRACT_COUNTERCLOCKWISE_IN_DOWN))
					{
						base.OutputState = RELAY_TYPE_2_OUTPUT_STATE.OFF_STOP;
					}
					break;
				case 1u:
					if (!base.OutputDisabledLatch && (base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.OFF_STOP || base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.REVERSE_RETRACT_COUNTERCLOCKWISE_IN_DOWN) && base.ForwardCommandAllowed)
					{
						base.OutputState = RELAY_TYPE_2_OUTPUT_STATE.FORWARD_EXTEND_CLOCKWISE_OUT_UP;
					}
					break;
				case 2u:
					if (!base.OutputDisabledLatch && (base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.OFF_STOP || base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.FORWARD_EXTEND_CLOCKWISE_OUT_UP) && base.ReverseCommandAllowed)
					{
						base.OutputState = RELAY_TYPE_2_OUTPUT_STATE.REVERSE_RETRACT_COUNTERCLOCKWISE_IN_DOWN;
					}
					break;
				case 3u:
					if (base.OutputDisabledLatch)
					{
						base.OutputDisabledLatch = false;
					}
					break;
				}
				UpdateStatus();
			}
		}

		public LATCHING_H_BRIDGE_TYPE_2_SIM_INTERFACE(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(adapter, software_part_number, product_id, version, new DEVICE_ID(product_id, 0, DEVICE_TYPE.op_Implicit((byte)32), 0, FUNCTION_NAME.op_Implicit((ushort)5), 0, (byte)0), options)
		{
			Init();
		}

		protected override void Init()
		{
			UpdateStatus();
		}

		protected override void UpdateDeviceCapabilities()
		{
			byte b = 0;
			byte b2 = Convert.ToByte(base.Supports_SoftwareConfigurableFuse);
			b |= b2;
			b2 = Convert.ToByte(base.Supports_CoarsePosition);
			b |= (byte)(b2 << 1);
			b2 = Convert.ToByte(base.Supports_FinePosition);
			b |= (byte)(b2 << 2);
			b2 = Convert.ToByte(base.PhysicalSwitchType);
			b |= (byte)(b2 << 3);
			base.DeviceCapabilities = b;
		}

		protected override void UpdateStatus()
		{
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			PAYLOAD deviceStatus = default(PAYLOAD);
			((PAYLOAD)(ref deviceStatus))..ctor(6);
			byte b = 0;
			byte b2 = Convert.ToByte((object)base.OutputState);
			b |= b2;
			b2 = Convert.ToByte(base.OutputDisabledLatch);
			b |= (byte)(b2 << 5);
			b2 = Convert.ToByte(base.ReverseCommandAllowed);
			b |= (byte)(b2 << 6);
			b2 = Convert.ToByte(base.ForwardCommandAllowed);
			b |= (byte)(b2 << 7);
			((PAYLOAD)(ref deviceStatus))[0] = b;
			((PAYLOAD)(ref deviceStatus))[1] = base.OutputPositionPct;
			uint num = (uint)Math.Round((double)(base.CurrentDraw * 256f));
			if (num > 65535)
			{
				num = 65535u;
			}
			if (num < 0)
			{
				num = 0u;
			}
			((PAYLOAD)(ref deviceStatus))[2] = (byte)(num >> 8);
			((PAYLOAD)(ref deviceStatus))[3] = (byte)num;
			if (base.OutputDisabledLatch)
			{
				((PAYLOAD)(ref deviceStatus))[4] = (byte)(base.UserMessage >> 8);
				((PAYLOAD)(ref deviceStatus))[5] = (byte)base.UserMessage;
			}
			base.DeviceStatus = deviceStatus;
		}

		protected override void OnLocalDeviceRxEvent(AdapterRxEvent rx)
		{
			base.OnLocalDeviceRxEvent(rx);
			if ((byte)rx.MessageType == 130 && rx.TargetAddress == base.Address && rx.SourceAddress == GetLocalSessionClientAddress(SESSION_ID.op_Implicit((ushort)4)) && rx.Count == 0)
			{
				Command = rx.MessageData;
			}
		}
	}
	public class LATCHING_RELAY_TYPE_2_SIM_INTERFACE : RELAY_TYPE_2_SIM_INTERFACE
	{
		public enum COMMAND_MODE : byte
		{
			OFF = 0,
			ON = 1,
			CLEAR_OUTPUT_DISABLED_LATCH = 3
		}

		private struct COMMAND
		{
			private int Value;

			public COMMAND_MODE Option
			{
				get
				{
					return (COMMAND_MODE)GetBits(3, 6);
				}
				set
				{
					SetBits((int)value, 3, 6);
				}
			}

			private int GetBits(int bit, int shift)
			{
				return (Value >> shift) & bit;
			}

			private void SetBits(int val, int bit, int shift)
			{
				val <<= shift;
				bit <<= shift;
				Value = (Value & ~bit) | (val & bit);
			}

			public static implicit operator int(COMMAND s)
			{
				return s.Value;
			}

			public static implicit operator byte(COMMAND s)
			{
				return (byte)s.Value;
			}

			public static implicit operator COMMAND(byte b)
			{
				COMMAND result = default(COMMAND);
				result.Value = b;
				return result;
			}

			public static implicit operator COMMAND(int i)
			{
				COMMAND result = default(COMMAND);
				result.Value = i;
				return result;
			}
		}

		public RELAY_TYPE_2_STATUS_PARAMS StatusParams;

		private COMMAND _command = 0;

		private COMMAND Command
		{
			get
			{
				return _command;
			}
			set
			{
				_command = value;
				switch ((uint)(byte)_command)
				{
				case 0u:
					if (!base.OutputDisabledLatch && base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.ON)
					{
						base.OutputState = RELAY_TYPE_2_OUTPUT_STATE.OFF_STOP;
					}
					break;
				case 1u:
					if (!base.OutputDisabledLatch && base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.OFF_STOP && base.OnCommandAllowed)
					{
						base.OutputState = RELAY_TYPE_2_OUTPUT_STATE.ON;
					}
					break;
				case 3u:
					if (base.OutputDisabledLatch)
					{
						base.OutputDisabledLatch = false;
					}
					break;
				}
				UpdateStatus();
			}
		}

		public LATCHING_RELAY_TYPE_2_SIM_INTERFACE(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(adapter, software_part_number, product_id, version, new DEVICE_ID(product_id, 0, DEVICE_TYPE.op_Implicit((byte)30), 0, FUNCTION_NAME.op_Implicit((ushort)7), 0, (byte)0), options)
		{
			StatusParams = new RELAY_TYPE_2_STATUS_PARAMS();
			Init();
		}

		protected override void Init()
		{
			UpdateStatus();
		}

		protected override void UpdateDeviceCapabilities()
		{
			byte b = 0;
			byte b2 = Convert.ToByte(base.Supports_SoftwareConfigurableFuse);
			b |= b2;
			b2 = Convert.ToByte(base.Supports_CoarsePosition);
			b |= (byte)(b2 << 1);
			b2 = Convert.ToByte(base.Supports_FinePosition);
			b |= (byte)(b2 << 2);
			b2 = Convert.ToByte(base.PhysicalSwitchType);
			b |= (byte)(b2 << 3);
			base.DeviceCapabilities = b;
		}

		protected override void UpdateStatus()
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			StatusParams._OutputState = (byte)base.OutputState;
			StatusParams.OutputPositionPct = base.OutputPositionPct;
			StatusParams.CurrentDraw = (ushort)Math.Round((double)(base.CurrentDraw * 256f));
			StatusParams.UserMessage = base.UserMessage;
			base.DeviceStatus = StatusParams.GetPayload();
		}

		protected void UpdateFromPayload(PAYLOAD pl)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			StatusParams.SetPayload(pl);
		}

		protected override void OnLocalDeviceRxEvent(AdapterRxEvent rx)
		{
			base.OnLocalDeviceRxEvent(rx);
			if ((byte)rx.MessageType == 130 && rx.TargetAddress == base.Address && rx.SourceAddress == GetLocalSessionClientAddress(SESSION_ID.op_Implicit((ushort)4)) && rx.Count == 0)
			{
				Command = rx.MessageData;
			}
		}
	}
	public enum LEVELER_TYPE_1_LED_STATE : byte
	{
		OFF,
		BLINK_2_Sec,
		BLINK_1_Sec,
		BLINK_500_MillSec,
		BLINK_250_MillSec,
		BLINK,
		INVERSE_BLINK,
		ON
	}
	public class LEVELER_TYPE_1 : LocalDevice
	{
		private LEVELER_TYPE_1_LED_STATE _indicator_FrontLED;

		private LEVELER_TYPE_1_LED_STATE _indicator_RearLED;

		private LEVELER_TYPE_1_LED_STATE _indicator_LeftLED;

		private LEVELER_TYPE_1_LED_STATE _indicator_RightLED;

		private LEVELER_TYPE_1_LED_STATE _indicator_LevelLED;

		private LEVELER_TYPE_1_LED_STATE _indicator_PowerLED;

		private LEVELER_TYPE_1_LED_STATE _indicator_RetractLED;

		private LEVELER_TYPE_1_LED_STATE _indicator_Buzzer;

		private static byte StateLines;

		public LEVELER_TYPE_1_LED_STATE Indicator_FrontLED
		{
			get
			{
				return _indicator_FrontLED;
			}
			set
			{
				if (_indicator_FrontLED != value)
				{
					_indicator_FrontLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_1_LED_STATE Indicator_RearLED
		{
			get
			{
				return _indicator_RearLED;
			}
			set
			{
				if (_indicator_RearLED != value)
				{
					_indicator_RearLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_1_LED_STATE Indicator_LeftLED
		{
			get
			{
				return _indicator_LeftLED;
			}
			set
			{
				if (_indicator_LeftLED != value)
				{
					_indicator_LeftLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_1_LED_STATE Indicator_RightLED
		{
			get
			{
				return _indicator_RightLED;
			}
			set
			{
				if (_indicator_RightLED != value)
				{
					_indicator_RightLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_1_LED_STATE Indicator_LevelLED
		{
			get
			{
				return _indicator_LevelLED;
			}
			set
			{
				if (_indicator_LevelLED != value)
				{
					_indicator_LevelLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_1_LED_STATE Indicator_PowerLED
		{
			get
			{
				return _indicator_PowerLED;
			}
			set
			{
				if (_indicator_PowerLED != value)
				{
					_indicator_PowerLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_1_LED_STATE Indicator_RetractLED
		{
			get
			{
				return _indicator_RetractLED;
			}
			set
			{
				if (_indicator_RetractLED != value)
				{
					_indicator_RetractLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_1_LED_STATE Indicator_Buzzer
		{
			get
			{
				return _indicator_Buzzer;
			}
			set
			{
				if (_indicator_Buzzer != value)
				{
					_indicator_Buzzer = value;
					UpdateStatus();
				}
			}
		}

		[field: CompilerGenerated]
		public string Command
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public string ConsoleTextLine1
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public string ConsoleTextLine2
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public LEVELER_TYPE_1(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), DEVICE_TYPE.op_Implicit((byte)7), 0, FUNCTION_NAME.op_Implicit((ushort)109), 1, (byte)0, options)
		{
			Init();
			AddPID(PID.BATTERY_VOLTAGE, (Func<UInt48>)(() => UInt48.op_Implicit(819187u)));
		}

		private void Init()
		{
			UpdateStatus();
		}

		private void UpdateStatus()
		{
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			PAYLOAD deviceStatus = default(PAYLOAD);
			((PAYLOAD)(ref deviceStatus))..ctor(3);
			uint num = (uint)(Convert.ToByte((object)Indicator_Buzzer) << 21);
			num |= (uint)(Convert.ToByte((object)Indicator_RetractLED) << 18);
			num |= (uint)(Convert.ToByte((object)Indicator_PowerLED) << 15);
			num |= (uint)(Convert.ToByte((object)Indicator_LeftLED) << 12);
			num |= (uint)(Convert.ToByte((object)Indicator_LevelLED) << 9);
			num |= (uint)(Convert.ToByte((object)Indicator_RearLED) << 6);
			num |= (uint)(Convert.ToByte((object)Indicator_RightLED) << 3);
			num |= Convert.ToByte((object)Indicator_FrontLED);
			((PAYLOAD)(ref deviceStatus))[0] = (byte)(num >> 16);
			((PAYLOAD)(ref deviceStatus))[1] = (byte)(num >> 8);
			((PAYLOAD)(ref deviceStatus))[2] = (byte)num;
			base.DeviceStatus = deviceStatus;
		}

		public void TransmitTextMessage()
		{
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			byte ext_data = 0;
			byte[] array = null;
			string text = ((StateLines <= 1) ? ConsoleTextLine1 : ConsoleTextLine2);
			switch (StateLines)
			{
			case 0:
			case 2:
				ext_data = StateLines;
				if (text != null)
				{
					array = Encoding.ASCII.GetBytes(text);
					if (array != null)
					{
						if (array.Length != 0)
						{
							num = ((array.Length >= 8) ? 8 : array.Length);
						}
						else
						{
							num = 1;
							array = Encoding.ASCII.GetBytes(" ");
						}
					}
				}
				StateLines++;
				break;
			case 1:
			case 3:
				ext_data = StateLines;
				if (text != null)
				{
					array = Encoding.ASCII.GetBytes(text);
					if (array != null)
					{
						if (array.Length != 0)
						{
							int num2 = 0;
							for (int i = 8; i < array.Length; i++)
							{
								array[num2++] = array[i];
							}
							num = ((num2 >= 8) ? 8 : num2);
						}
						else
						{
							num = 1;
							array = Encoding.ASCII.GetBytes(" ");
						}
					}
				}
				StateLines++;
				if (StateLines > 3)
				{
					StateLines = 0;
				}
				break;
			default:
				StateLines = 0;
				break;
			}
			if (num == 0)
			{
				return;
			}
			PAYLOAD payload = default(PAYLOAD);
			((PAYLOAD)(ref payload))..ctor(num);
			for (int j = 0; j < ((PAYLOAD)(ref payload)).Length; j++)
			{
				if (array != null)
				{
					((PAYLOAD)(ref payload))[j] = array[j];
				}
			}
			Transmit29((byte)131, ext_data, ADDRESS.BROADCAST, payload);
		}

		protected override void OnLocalDeviceRxEvent(AdapterRxEvent rx)
		{
			base.OnLocalDeviceRxEvent(rx);
			if ((byte)rx.MessageType != 130 || rx.TargetAddress != base.Address)
			{
				return;
			}
			TransmitTextMessage();
			if (rx.SourceAddress == GetLocalSessionClientAddress(SESSION_ID.op_Implicit((ushort)4)) && rx.Count == 2)
			{
				string text = string.Empty;
				ushort num = (ushort)((rx[0] << 8) | rx[1]);
				if ((num & 1) != 0)
				{
					text += " RIGHT ";
				}
				if ((num & 2) != 0)
				{
					text += " LEFT ";
				}
				if ((num & 4) != 0)
				{
					text += " REAR ";
				}
				if ((num & 8) != 0)
				{
					text += " FRONT ";
				}
				if ((num & 0x10) != 0)
				{
					text += " AUTO_LEVEL ";
				}
				if ((num & 0x20) != 0)
				{
					text += " RETRACT ";
				}
				if ((num & 0x40) != 0)
				{
					text += " ENTER ";
				}
				if ((num & 0x80) != 0)
				{
					text += " MENU_DOWN ";
				}
				if ((num & 0x100) != 0)
				{
					text += " RESERVED ";
				}
				if ((num & 0x200) != 0)
				{
					text += " ON_OFF ";
				}
				if ((num & 0x400) != 0)
				{
					text += " MENU_UP ";
				}
				Command = text;
			}
		}

		public override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
	public enum LEVELER_TYPE_3_SCREEN : byte
	{
		DEFAULT,
		IDLE_MOTORIZED,
		IDLE_TOWABLE,
		AUTO,
		MANUAL,
		SELECT,
		YES_NO,
		INFO,
		ALERT,
		UNKNOWN_09,
		UNKNOWN_0A,
		UNKNOWN_0B,
		UNKNOWN_0C,
		UNKNOWN_0D,
		UNKNOWN_0E,
		UNKNOWN_0F,
		UNKNOWN_10,
		UNKNOWN_11,
		UNKNOWN_12,
		UNKNOWN_13,
		UNKNOWN_14,
		UNKNOWN_15,
		UNKNOWN_16,
		UNKNOWN_17,
		UNKNOWN_18,
		UNKNOWN_19,
		UNKNOWN_1A,
		UNKNOWN_1B,
		UNKNOWN_1C,
		UNKNOWN_1D,
		UNKNOWN_1E,
		UNKNOWN_1F,
		UNKNOWN_20,
		UNKNOWN_21,
		UNKNOWN_22,
		UNKNOWN_23,
		UNKNOWN_24,
		UNKNOWN_25,
		UNKNOWN_26,
		UNKNOWN_27,
		UNKNOWN_28,
		UNKNOWN_29,
		UNKNOWN_2A,
		UNKNOWN_2B,
		UNKNOWN_2C,
		UNKNOWN_2D,
		UNKNOWN_2E,
		UNKNOWN_2F,
		UNKNOWN_30,
		UNKNOWN_31,
		UNKNOWN_32,
		UNKNOWN_33,
		UNKNOWN_34,
		UNKNOWN_35,
		UNKNOWN_36,
		UNKNOWN_37,
		UNKNOWN_38,
		UNKNOWN_39,
		UNKNOWN_3A,
		UNKNOWN_3B,
		UNKNOWN_3C,
		UNKNOWN_3D,
		UNKNOWN_3E,
		UNKNOWN_3F,
		UNKNOWN_40,
		UNKNOWN_41,
		UNKNOWN_42,
		UNKNOWN_43,
		UNKNOWN_44,
		UNKNOWN_45,
		UNKNOWN_46,
		UNKNOWN_47,
		UNKNOWN_48,
		UNKNOWN_49,
		UNKNOWN_4A,
		UNKNOWN_4B,
		UNKNOWN_4C,
		UNKNOWN_4D,
		UNKNOWN_4E,
		UNKNOWN_4F,
		UNKNOWN_50,
		UNKNOWN_51,
		UNKNOWN_52,
		UNKNOWN_53,
		UNKNOWN_54,
		UNKNOWN_55,
		UNKNOWN_56,
		UNKNOWN_57,
		UNKNOWN_58,
		UNKNOWN_59,
		UNKNOWN_5A,
		UNKNOWN_5B,
		UNKNOWN_5C,
		UNKNOWN_5D,
		UNKNOWN_5E,
		UNKNOWN_5F,
		UNKNOWN_60,
		UNKNOWN_61,
		UNKNOWN_62,
		UNKNOWN_63,
		UNKNOWN_64,
		UNKNOWN_65,
		UNKNOWN_66,
		UNKNOWN_67,
		UNKNOWN_68,
		UNKNOWN_69,
		UNKNOWN_6A,
		UNKNOWN_6B,
		UNKNOWN_6C,
		UNKNOWN_6D,
		UNKNOWN_6E,
		UNKNOWN_6F,
		UNKNOWN_70,
		UNKNOWN_71,
		UNKNOWN_72,
		UNKNOWN_73,
		UNKNOWN_74,
		UNKNOWN_75,
		UNKNOWN_76,
		UNKNOWN_77,
		UNKNOWN_78,
		UNKNOWN_79,
		UNKNOWN_7A,
		UNKNOWN_7B,
		UNKNOWN_7C,
		UNKNOWN_7D,
		UNKNOWN_7E,
		UNKNOWN_7F,
		UNKNOWN_80,
		UNKNOWN_81,
		UNKNOWN_82,
		UNKNOWN_83,
		UNKNOWN_84,
		UNKNOWN_85,
		UNKNOWN_86,
		UNKNOWN_87,
		UNKNOWN_88,
		UNKNOWN_89,
		UNKNOWN_8A,
		UNKNOWN_8B,
		UNKNOWN_8C,
		UNKNOWN_8D,
		UNKNOWN_8E,
		UNKNOWN_8F,
		UNKNOWN_90,
		UNKNOWN_91,
		UNKNOWN_92,
		UNKNOWN_93,
		UNKNOWN_94,
		UNKNOWN_95,
		UNKNOWN_96,
		UNKNOWN_97,
		UNKNOWN_98,
		UNKNOWN_99,
		UNKNOWN_9A,
		UNKNOWN_9B,
		UNKNOWN_9C,
		UNKNOWN_9D,
		UNKNOWN_9E,
		UNKNOWN_9F,
		UNKNOWN_A0,
		UNKNOWN_A1,
		UNKNOWN_A2,
		UNKNOWN_A3,
		UNKNOWN_A4,
		UNKNOWN_A5,
		UNKNOWN_A6,
		UNKNOWN_A7,
		UNKNOWN_A8,
		UNKNOWN_A9,
		UNKNOWN_AA,
		UNKNOWN_AB,
		UNKNOWN_AC,
		UNKNOWN_AD,
		UNKNOWN_AE,
		UNKNOWN_AF,
		UNKNOWN_B0,
		UNKNOWN_B1,
		UNKNOWN_B2,
		UNKNOWN_B3,
		UNKNOWN_B4,
		UNKNOWN_B5,
		UNKNOWN_B6,
		UNKNOWN_B7,
		UNKNOWN_B8,
		UNKNOWN_B9,
		UNKNOWN_BA,
		UNKNOWN_BB,
		UNKNOWN_BC,
		UNKNOWN_BD,
		UNKNOWN_BE,
		UNKNOWN_BF,
		UNKNOWN_C0,
		UNKNOWN_C1,
		UNKNOWN_C2,
		UNKNOWN_C3,
		UNKNOWN_C4,
		UNKNOWN_C5,
		UNKNOWN_C6,
		UNKNOWN_C7,
		UNKNOWN_C8,
		UNKNOWN_C9,
		UNKNOWN_CA,
		UNKNOWN_CB,
		UNKNOWN_CC,
		UNKNOWN_CD,
		UNKNOWN_CE,
		UNKNOWN_CF,
		UNKNOWN_D0,
		UNKNOWN_D1,
		UNKNOWN_D2,
		UNKNOWN_D3,
		UNKNOWN_D4,
		UNKNOWN_D5,
		UNKNOWN_D6,
		UNKNOWN_D7,
		UNKNOWN_D8,
		UNKNOWN_D9,
		UNKNOWN_DA,
		UNKNOWN_DB,
		UNKNOWN_DC,
		UNKNOWN_DD,
		UNKNOWN_DE,
		UNKNOWN_DF,
		UNKNOWN_E0,
		UNKNOWN_E1,
		UNKNOWN_E2,
		UNKNOWN_E3,
		UNKNOWN_E4,
		UNKNOWN_E5,
		UNKNOWN_E6,
		UNKNOWN_E7,
		UNKNOWN_E8,
		UNKNOWN_E9,
		UNKNOWN_EA,
		UNKNOWN_EB,
		UNKNOWN_EC,
		UNKNOWN_ED,
		UNKNOWN_EE,
		UNKNOWN_EF,
		UNKNOWN_F0,
		UNKNOWN_F1,
		UNKNOWN_F2,
		UNKNOWN_F3,
		UNKNOWN_F4,
		UNKNOWN_F5,
		UNKNOWN_F6,
		UNKNOWN_F7,
		UNKNOWN_F8,
		UNKNOWN_F9,
		UNKNOWN_FA,
		UNKNOWN_FB,
		UNKNOWN_FC,
		UNKNOWN_FD,
		UNKNOWN_FE,
		UNKNOWN_FF
	}
	public enum LEVELER_TYPE_3_LED_STATE : byte
	{
		OFF,
		BLINK,
		INVERSE_BLINK,
		ON
	}
	public class LEVELER_TYPE_3 : LocalDevice
	{
		private LEVELER_TYPE_3_SCREEN _screen;

		private string[] Lines = new string[6];

		private bool _isDisabledButton_ReservedBit15;

		private bool _isDisabledButton_ReservedBit14;

		private bool _isDisabledButton_ReservedBit13;

		private bool _isDisabledButton_EnterSetup;

		private bool _isDisabledButton_AutoHitch;

		private bool _isDisabledButton_MenuUp;

		private bool _isDisabledButton_Back;

		private bool _isDisabledButton_Extend;

		private bool _isDisabledButton_MenuDown;

		private bool _isDisabledButton_Enter;

		private bool _isDisabledButton_Retract;

		private bool _isDisabledButton_AutoLevel;

		private bool _isDisabledButton_Front;

		private bool _isDisabledButton_Rear;

		private bool _isDisabledButton_Left;

		private bool _isDisabledButton_Right;

		private short _blinkRateOnCount;

		private short _blinkRateOffCount;

		private LEVELER_TYPE_3_LED_STATE _indicator_FrontLED;

		private LEVELER_TYPE_3_LED_STATE _indicator_RearLED;

		private LEVELER_TYPE_3_LED_STATE _indicator_LeftLED;

		private LEVELER_TYPE_3_LED_STATE _indicator_RightLED;

		private LEVELER_TYPE_3_LED_STATE _indicator_LevelLED;

		private LEVELER_TYPE_3_LED_STATE _indicator_ExtendLED;

		private LEVELER_TYPE_3_LED_STATE _indicator_RetractLED;

		private LEVELER_TYPE_3_LED_STATE _indicator_Buzzer;

		private LEVELER_TYPE_3_SCREEN _command_Screen;

		private bool _command_Button_EnterSetup;

		private bool _command_Button_AutoHitch;

		private bool _command_Button_MenuUp;

		private bool _command_Button_Back;

		private bool _command_Button_Extend;

		private bool _command_Button_MenuDown;

		private bool _command_Button_Enter;

		private bool _command_Button_Retract;

		private bool _command_Button_AutoLevel;

		private bool _command_Button_Front;

		private bool _command_Button_Rear;

		private bool _command_Button_Left;

		private bool _command_Button_Right;

		public LEVELER_TYPE_3_SCREEN Screen
		{
			get
			{
				return _screen;
			}
			set
			{
				if (_screen != value)
				{
					_screen = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_ReservedBit15
		{
			get
			{
				return _isDisabledButton_ReservedBit15;
			}
			set
			{
				if (_isDisabledButton_ReservedBit15 != value)
				{
					_isDisabledButton_ReservedBit15 = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_ReservedBit14
		{
			get
			{
				return _isDisabledButton_ReservedBit14;
			}
			set
			{
				if (_isDisabledButton_ReservedBit14 != value)
				{
					_isDisabledButton_ReservedBit14 = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_ReservedBit13
		{
			get
			{
				return _isDisabledButton_ReservedBit13;
			}
			set
			{
				if (_isDisabledButton_ReservedBit13 != value)
				{
					_isDisabledButton_ReservedBit13 = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_EnterSetup
		{
			get
			{
				return _isDisabledButton_EnterSetup;
			}
			set
			{
				if (_isDisabledButton_EnterSetup != value)
				{
					_isDisabledButton_EnterSetup = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_AutoHitch
		{
			get
			{
				return _isDisabledButton_AutoHitch;
			}
			set
			{
				if (_isDisabledButton_AutoHitch != value)
				{
					_isDisabledButton_AutoHitch = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_MenuUp
		{
			get
			{
				return _isDisabledButton_MenuUp;
			}
			set
			{
				if (_isDisabledButton_MenuUp != value)
				{
					_isDisabledButton_MenuUp = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_Back
		{
			get
			{
				return _isDisabledButton_Back;
			}
			set
			{
				if (_isDisabledButton_Back != value)
				{
					_isDisabledButton_Back = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_Extend
		{
			get
			{
				return _isDisabledButton_Extend;
			}
			set
			{
				if (_isDisabledButton_Extend != value)
				{
					_isDisabledButton_Extend = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_MenuDown
		{
			get
			{
				return _isDisabledButton_MenuDown;
			}
			set
			{
				if (_isDisabledButton_MenuDown != value)
				{
					_isDisabledButton_MenuDown = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_Enter
		{
			get
			{
				return _isDisabledButton_Enter;
			}
			set
			{
				if (_isDisabledButton_Enter != value)
				{
					_isDisabledButton_Enter = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_Retract
		{
			get
			{
				return _isDisabledButton_Retract;
			}
			set
			{
				if (_isDisabledButton_Retract != value)
				{
					_isDisabledButton_Retract = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_AutoLevel
		{
			get
			{
				return _isDisabledButton_AutoLevel;
			}
			set
			{
				if (_isDisabledButton_AutoLevel != value)
				{
					_isDisabledButton_AutoLevel = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_Front
		{
			get
			{
				return _isDisabledButton_Front;
			}
			set
			{
				if (_isDisabledButton_Front != value)
				{
					_isDisabledButton_Front = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_Rear
		{
			get
			{
				return _isDisabledButton_Rear;
			}
			set
			{
				if (_isDisabledButton_Rear != value)
				{
					_isDisabledButton_Rear = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_Left
		{
			get
			{
				return _isDisabledButton_Left;
			}
			set
			{
				if (_isDisabledButton_Left != value)
				{
					_isDisabledButton_Left = value;
					UpdateStatus();
				}
			}
		}

		public bool IsDisabledButton_Right
		{
			get
			{
				return _isDisabledButton_Right;
			}
			set
			{
				if (_isDisabledButton_Right != value)
				{
					_isDisabledButton_Right = value;
					UpdateStatus();
				}
			}
		}

		public short BlinkRateOnCount
		{
			get
			{
				return _blinkRateOnCount;
			}
			set
			{
				if (_blinkRateOnCount != value)
				{
					_blinkRateOnCount = value;
					UpdateStatus();
				}
			}
		}

		public short BlinkRateOffCount
		{
			get
			{
				return _blinkRateOffCount;
			}
			set
			{
				if (_blinkRateOffCount != value)
				{
					_blinkRateOffCount = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_3_LED_STATE Indicator_FrontLED
		{
			get
			{
				return _indicator_FrontLED;
			}
			set
			{
				if (_indicator_FrontLED != value)
				{
					_indicator_FrontLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_3_LED_STATE Indicator_RearLED
		{
			get
			{
				return _indicator_RearLED;
			}
			set
			{
				if (_indicator_RearLED != value)
				{
					_indicator_RearLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_3_LED_STATE Indicator_LeftLED
		{
			get
			{
				return _indicator_LeftLED;
			}
			set
			{
				if (_indicator_LeftLED != value)
				{
					_indicator_LeftLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_3_LED_STATE Indicator_RightLED
		{
			get
			{
				return _indicator_RightLED;
			}
			set
			{
				if (_indicator_RightLED != value)
				{
					_indicator_RightLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_3_LED_STATE Indicator_LevelLED
		{
			get
			{
				return _indicator_LevelLED;
			}
			set
			{
				if (_indicator_LevelLED != value)
				{
					_indicator_LevelLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_3_LED_STATE Indicator_ExtendLED
		{
			get
			{
				return _indicator_ExtendLED;
			}
			set
			{
				if (_indicator_ExtendLED != value)
				{
					_indicator_ExtendLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_3_LED_STATE Indicator_RetractLED
		{
			get
			{
				return _indicator_RetractLED;
			}
			set
			{
				if (_indicator_RetractLED != value)
				{
					_indicator_RetractLED = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_3_LED_STATE Indicator_Buzzer
		{
			get
			{
				return _indicator_Buzzer;
			}
			set
			{
				if (_indicator_Buzzer != value)
				{
					_indicator_Buzzer = value;
					UpdateStatus();
				}
			}
		}

		public LEVELER_TYPE_3_SCREEN Command_Screen
		{
			get
			{
				return _command_Screen;
			}
			private set
			{
				if (_command_Screen != value)
				{
					_command_Screen = value;
				}
			}
		}

		public bool Command_Button_EnterSetup
		{
			get
			{
				return _command_Button_EnterSetup;
			}
			private set
			{
				if (_command_Button_EnterSetup != value)
				{
					_command_Button_EnterSetup = value;
				}
			}
		}

		public bool Command_Button_AutoHitch
		{
			get
			{
				return _command_Button_AutoHitch;
			}
			private set
			{
				if (_command_Button_AutoHitch != value)
				{
					_command_Button_AutoHitch = value;
				}
			}
		}

		public bool Command_Button_MenuUp
		{
			get
			{
				return _command_Button_MenuUp;
			}
			private set
			{
				if (_command_Button_MenuUp != value)
				{
					_command_Button_MenuUp = value;
				}
			}
		}

		public bool Command_Button_Back
		{
			get
			{
				return _command_Button_Back;
			}
			private set
			{
				if (_command_Button_Back != value)
				{
					_command_Button_Back = value;
				}
			}
		}

		public bool Command_Button_Extend
		{
			get
			{
				return _command_Button_Extend;
			}
			private set
			{
				if (_command_Button_Extend != value)
				{
					_command_Button_Extend = value;
				}
			}
		}

		public bool Command_Button_MenuDown
		{
			get
			{
				return _command_Button_MenuDown;
			}
			private set
			{
				if (_command_Button_MenuDown != value)
				{
					_command_Button_MenuDown = value;
				}
			}
		}

		public bool Command_Button_Enter
		{
			get
			{
				return _command_Button_Enter;
			}
			private set
			{
				if (_command_Button_Enter != value)
				{
					_command_Button_Enter = value;
				}
			}
		}

		public bool Command_Button_Retract
		{
			get
			{
				return _command_Button_Retract;
			}
			private set
			{
				if (_command_Button_Retract != value)
				{
					_command_Button_Retract = value;
				}
			}
		}

		public bool Command_Button_AutoLevel
		{
			get
			{
				return _command_Button_AutoLevel;
			}
			private set
			{
				if (_command_Button_AutoLevel != value)
				{
					_command_Button_AutoLevel = value;
				}
			}
		}

		public bool Command_Button_Front
		{
			get
			{
				return _command_Button_Front;
			}
			private set
			{
				if (_command_Button_Front != value)
				{
					_command_Button_Front = value;
				}
			}
		}

		public bool Command_Button_Rear
		{
			get
			{
				return _command_Button_Rear;
			}
			private set
			{
				if (_command_Button_Rear != value)
				{
					_command_Button_Rear = value;
				}
			}
		}

		public bool Command_Button_Left
		{
			get
			{
				return _command_Button_Left;
			}
			private set
			{
				if (_command_Button_Left != value)
				{
					_command_Button_Left = value;
				}
			}
		}

		public bool Command_Button_Right
		{
			get
			{
				return _command_Button_Right;
			}
			private set
			{
				if (_command_Button_Right != value)
				{
					_command_Button_Right = value;
				}
			}
		}

		public TEXT_CONSOLE_SIZE TextConsoleSize
		{
			get
			{
				if (base.TextConsole == null)
				{
					return default(TEXT_CONSOLE_SIZE);
				}
				return base.TextConsole.Size;
			}
			set
			{
				int num = Lines.Length;
				int w = 32;
				if (value.Height > 0 && value.Height <= Lines.Length)
				{
					num = value.Height;
					if (value.Width > 0 && value.Width <= 32)
					{
						w = value.Width;
					}
				}
				CreateTextConsole(new TEXT_CONSOLE_SIZE(w, num));
				for (int i = 0; i < num; i++)
				{
					Lines[i] = $"Line #{i}";
				}
				base.TextConsole.Lines = Lines;
			}
		}

		public LEVELER_TYPE_3(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), DEVICE_TYPE.op_Implicit((byte)17), 0, FUNCTION_NAME.op_Implicit((ushort)109), 3, (byte)0, options)
		{
			Init();
			AddPID(PID.BATTERY_VOLTAGE, (Func<UInt48>)(() => UInt48.op_Implicit(819187u)));
		}

		private void Init()
		{
			UpdateStatus();
		}

		private void UpdateStatus()
		{
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			PAYLOAD deviceStatus = default(PAYLOAD);
			((PAYLOAD)(ref deviceStatus))..ctor(6);
			((PAYLOAD)(ref deviceStatus))[0] = (byte)Screen;
			byte b = 0;
			byte b2 = Convert.ToByte(IsDisabledButton_EnterSetup);
			b |= (byte)(b2 << 4);
			b2 = Convert.ToByte(IsDisabledButton_AutoHitch);
			b |= (byte)(b2 << 3);
			b2 = Convert.ToByte(IsDisabledButton_MenuUp);
			b |= (byte)(b2 << 2);
			b2 = Convert.ToByte(IsDisabledButton_Back);
			b |= (byte)(b2 << 1);
			b2 = Convert.ToByte(IsDisabledButton_Extend);
			b |= b2;
			((PAYLOAD)(ref deviceStatus))[1] = b;
			b = 0;
			b2 = Convert.ToByte(IsDisabledButton_MenuDown);
			b |= (byte)(b2 << 7);
			b2 = Convert.ToByte(IsDisabledButton_Enter);
			b |= (byte)(b2 << 6);
			b2 = Convert.ToByte(IsDisabledButton_Retract);
			b |= (byte)(b2 << 5);
			b2 = Convert.ToByte(IsDisabledButton_AutoLevel);
			b |= (byte)(b2 << 4);
			b2 = Convert.ToByte(IsDisabledButton_Front);
			b |= (byte)(b2 << 3);
			b2 = Convert.ToByte(IsDisabledButton_Rear);
			b |= (byte)(b2 << 2);
			b2 = Convert.ToByte(IsDisabledButton_Left);
			b |= (byte)(b2 << 1);
			b2 = Convert.ToByte(IsDisabledButton_Right);
			b |= b2;
			((PAYLOAD)(ref deviceStatus))[2] = b;
			b = 0;
			b2 = Convert.ToByte(BlinkRateOnCount);
			b |= (byte)(b2 << 4);
			b2 = Convert.ToByte(BlinkRateOffCount);
			b |= b2;
			((PAYLOAD)(ref deviceStatus))[3] = b;
			b = 0;
			b2 = Convert.ToByte((object)Indicator_FrontLED);
			b |= (byte)(b2 << 6);
			b2 = Convert.ToByte((object)Indicator_RearLED);
			b |= (byte)(b2 << 4);
			b2 = Convert.ToByte((object)Indicator_LeftLED);
			b |= (byte)(b2 << 2);
			b2 = Convert.ToByte((object)Indicator_RightLED);
			b |= b2;
			((PAYLOAD)(ref deviceStatus))[4] = b;
			b = 0;
			b2 = Convert.ToByte((object)Indicator_LevelLED);
			b |= (byte)(b2 << 6);
			b2 = Convert.ToByte((object)Indicator_ExtendLED);
			b |= (byte)(b2 << 4);
			b2 = Convert.ToByte((object)Indicator_RetractLED);
			b |= (byte)(b2 << 2);
			b2 = Convert.ToByte((object)Indicator_Buzzer);
			b |= b2;
			((PAYLOAD)(ref deviceStatus))[5] = b;
			base.DeviceStatus = deviceStatus;
		}

		protected override void OnLocalDeviceRxEvent(AdapterRxEvent rx)
		{
			base.OnLocalDeviceRxEvent(rx);
			if ((byte)rx.MessageType == 130 && rx.TargetAddress == base.Address && rx.SourceAddress == GetLocalSessionClientAddress(SESSION_ID.op_Implicit((ushort)4)) && rx.Count == 3)
			{
				Command_Screen = (LEVELER_TYPE_3_SCREEN)rx[0];
				Command_Button_EnterSetup = (rx[1] & 0x10) != 0;
				Command_Button_AutoHitch = (rx[1] & 8) != 0;
				Command_Button_MenuUp = (rx[1] & 4) != 0;
				Command_Button_Back = (rx[1] & 2) != 0;
				Command_Button_Extend = (rx[1] & 1) != 0;
				Command_Button_MenuDown = (rx[2] & 0x80) != 0;
				Command_Button_Enter = (rx[2] & 0x40) != 0;
				Command_Button_Retract = (rx[2] & 0x20) != 0;
				Command_Button_AutoLevel = (rx[2] & 0x10) != 0;
				Command_Button_Front = (rx[2] & 8) != 0;
				Command_Button_Rear = (rx[2] & 4) != 0;
				Command_Button_Left = (rx[2] & 2) != 0;
				Command_Button_Right = (rx[2] & 1) != 0;
			}
		}

		public override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
	public class LEVELER_TYPE_4 : LocalDevice
	{
		public enum CHASSIS_TYPE : byte
		{
			CLASS_A,
			CLASS_C,
			FIFTH_WHEEL,
			TRAVEL_TRAILER
		}

		public enum JACK_CONFIGURATION : byte
		{
			THREE_JACKS,
			FOUR_JACKS,
			FOUR_JACKS_PLUS_TONGUE_JACK,
			SIX_JACKS
		}

		public struct CAPABILITIES
		{
			public byte Value;

			public bool IsJackPositionSupported
			{
				get
				{
					return (Value & 1) != 0;
				}
				set
				{
					if (value)
					{
						Value |= 1;
					}
					else
					{
						Value &= 254;
					}
				}
			}

			public JACK_CONFIGURATION JackConfiguration
			{
				get
				{
					return (JACK_CONFIGURATION)((Value >> 1) & 3);
				}
				set
				{
					Value = (byte)((uint)(Value & -7) | ((uint)(value & JACK_CONFIGURATION.SIX_JACKS) << 1));
				}
			}

			public CHASSIS_TYPE ChassisType
			{
				get
				{
					return (CHASSIS_TYPE)((Value >> 4) & 2);
				}
				set
				{
					Value = (byte)((uint)(Value & -49) | ((uint)(value & CHASSIS_TYPE.TRAVEL_TRAILER) << 4));
				}
			}

			public CAPABILITIES(byte value)
			{
				Value = value;
			}

			public static CAPABILITIES Create(bool isJackPositionSupported, JACK_CONFIGURATION jackConfiguration, CHASSIS_TYPE chassisType)
			{
				return new CAPABILITIES
				{
					IsJackPositionSupported = isJackPositionSupported,
					JackConfiguration = jackConfiguration,
					ChassisType = chassisType
				};
			}
		}

		public enum ENHANCED_MODE : byte
		{
			SCREEN_CONTEXT,
			ABORT_OPERATION,
			BACK,
			HOME
		}

		public enum UI_MODE : byte
		{
			IDLE,
			AUTO,
			MANUAL,
			MANUAL_CONSOLE,
			ZERO,
			INFO,
			YES_NO,
			FAULT_GENERIC,
			FAULT_JACK_MANUAL,
			FAULT_JACK_MANUAL_CONSOLE,
			MANUAL_AIR_BAG_CONTROL,
			AUTO_HITCH,
			AUTO_RETRACT,
			AUTO_RETRACT_FRONT,
			AUTO_RETRACT_REAR,
			HOME_JACKS,
			MODE_10,
			MODE_11,
			MODE_12,
			MODE_13,
			MODE_14,
			MODE_15,
			MODE_16,
			MODE_17,
			MODE_18,
			MODE_19,
			MODE_1A,
			MODE_1B,
			MODE_1C,
			MODE_1D,
			MODE_1E,
			MODE_1F,
			MODE_20,
			MODE_21,
			MODE_22,
			MODE_23,
			MODE_24,
			MODE_25,
			MODE_26,
			MODE_27,
			MODE_28,
			MODE_29,
			MODE_2A,
			MODE_2B,
			MODE_2C,
			MODE_2D,
			MODE_2E,
			MODE_2F,
			MODE_30,
			MODE_31,
			MODE_32,
			MODE_33,
			MODE_34,
			MODE_35,
			MODE_36,
			MODE_37,
			MODE_38,
			MODE_39,
			MODE_3A,
			MODE_3B,
			MODE_3C,
			MODE_3D,
			MODE_3E,
			MODE_3F,
			MODE_40,
			MODE_41,
			MODE_42,
			MODE_43,
			MODE_44,
			MODE_45,
			MODE_46,
			MODE_47,
			MODE_48,
			MODE_49,
			MODE_4A,
			MODE_4B,
			MODE_4C,
			MODE_4D,
			MODE_4E,
			MODE_4F,
			MODE_50,
			MODE_51,
			MODE_52,
			MODE_53,
			MODE_54,
			MODE_55,
			MODE_56,
			MODE_57,
			MODE_58,
			MODE_59,
			MODE_5A,
			MODE_5B,
			MODE_5C,
			MODE_5D,
			MODE_5E,
			MODE_5F,
			MODE_60,
			MODE_61,
			MODE_62,
			MODE_63,
			MODE_64,
			MODE_65,
			MODE_66,
			MODE_67,
			MODE_68,
			MODE_69,
			MODE_6A,
			MODE_6B,
			MODE_6C,
			MODE_6D,
			MODE_6E,
			MODE_6F,
			MODE_70,
			MODE_71,
			MODE_72,
			MODE_73,
			MODE_74,
			MODE_75,
			MODE_76,
			MODE_77,
			MODE_78,
			MODE_79,
			MODE_7A,
			MODE_7B,
			MODE_7C,
			MODE_7D,
			MODE_7E,
			MODE_7F,
			MODE_80,
			MODE_81,
			MODE_82,
			MODE_83,
			MODE_84,
			MODE_85,
			MODE_86,
			MODE_87,
			MODE_88,
			MODE_89,
			MODE_8A,
			MODE_8B,
			MODE_8C,
			MODE_8D,
			MODE_8E,
			MODE_8F,
			MODE_90,
			MODE_91,
			MODE_92,
			MODE_93,
			MODE_94,
			MODE_95,
			MODE_96,
			MODE_97,
			MODE_98,
			MODE_99,
			MODE_9A,
			MODE_9B,
			MODE_9C,
			MODE_9D,
			MODE_9E,
			MODE_9F,
			MODE_A0,
			MODE_A1,
			MODE_A2,
			MODE_A3,
			MODE_A4,
			MODE_A5,
			MODE_A6,
			MODE_A7,
			MODE_A8,
			MODE_A9,
			MODE_AA,
			MODE_AB,
			MODE_AC,
			MODE_AD,
			MODE_AE,
			MODE_AF,
			MODE_B0,
			MODE_B1,
			MODE_B2,
			MODE_B3,
			MODE_B4,
			MODE_B5,
			MODE_B6,
			MODE_B7,
			MODE_B8,
			MODE_B9,
			MODE_BA,
			MODE_BB,
			MODE_BC,
			MODE_BD,
			MODE_BE,
			MODE_BF,
			MODE_C0,
			MODE_C1,
			MODE_C2,
			MODE_C3,
			MODE_C4,
			MODE_C5,
			MODE_C6,
			MODE_C7,
			MODE_C8,
			MODE_C9,
			MODE_CA,
			MODE_CB,
			MODE_CC,
			MODE_CD,
			MODE_CE,
			MODE_CF,
			MODE_D0,
			MODE_D1,
			MODE_D2,
			MODE_D3,
			MODE_D4,
			MODE_D5,
			MODE_D6,
			MODE_D7,
			MODE_D8,
			MODE_D9,
			MODE_DA,
			MODE_DB,
			MODE_DC,
			MODE_DD,
			MODE_DE,
			MODE_DF,
			MODE_E0,
			MODE_E1,
			MODE_E2,
			MODE_E3,
			MODE_E4,
			MODE_E5,
			MODE_E6,
			MODE_E7,
			MODE_E8,
			MODE_E9,
			MODE_EA,
			MODE_EB,
			MODE_EC,
			MODE_ED,
			MODE_EE,
			MODE_EF,
			MODE_F0,
			MODE_F1,
			MODE_F2,
			MODE_F3,
			MODE_F4,
			MODE_F5,
			MODE_F6,
			MODE_F7,
			MODE_F8,
			MODE_F9,
			MODE_FA,
			MODE_FB,
			MODE_FC,
			MODE_FD,
			MODE_FE,
			MODE_FF
		}

		private const double MAX_JACK_STROKE_INCHES = 15.0;

		private const double GROUND_INCHES = 7.5;

		private const byte IS_LEVEL = 1;

		private const byte JACKS_ARE_FULLY_RETRACTED = 2;

		private const byte JACKS_ARE_GROUNDED = 4;

		private const byte JACKS_ARE_MOVING = 8;

		private const byte EXCESS_ANGLE_DETECTED = 16;

		private const byte EXCESS_TWIST_DETECTED = 32;

		private const double MAX_ANGLE_DEGREES = 15.96875;

		private const double MIN_ANGLE_DEGREES = -15.96875;

		private uint ButtonCommand;

		private uint? LastButtonCommand;

		private string[] Lines = new string[6];

		private Timer AngleTime = new Timer(true);

		private Timer UiModeTime = new Timer(true);

		private double LFJackStrokeInches;

		private double RFJackStrokeInches;

		private double LMJackStrokeInches;

		private double RMJackStrokeInches;

		private double LRJackStrokeInches;

		private double RRJackStrokeInches;

		private double TongueJackStrokeInches;

		private bool _IsManualChecked;

		public new CAPABILITIES DeviceCapabilities => new CAPABILITIES(base.DeviceCapabilities.GetValueOrDefault());

		[field: CompilerGenerated]
		public ENHANCED_MODE CommandEnhanced
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public UI_MODE CommandLEVELER_UI_MODE
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public uint CommandUI_Button_State
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public bool IsManualChecked
		{
			get
			{
				return _IsManualChecked;
			}
			set
			{
				SetDeviceStatusFlag(0, 1, value: false);
				SetDeviceStatusFlag(0, 2, value: false);
				SetDeviceStatusFlag(0, 4, value: false);
				SetDeviceStatusFlag(0, 8, value: false);
				SetDeviceStatusFlag(0, 16, value: false);
				SetDeviceStatusFlag(0, 32, value: false);
				_IsManualChecked = value;
			}
		}

		public bool IsLevel
		{
			get
			{
				return GetDeviceStatusFlag(0, 1);
			}
			set
			{
				SetDeviceStatusFlag(0, 1, value);
			}
		}

		public bool JacksAreFullyRetracted
		{
			get
			{
				return GetDeviceStatusFlag(0, 2);
			}
			set
			{
				SetDeviceStatusFlag(0, 2, value);
			}
		}

		public bool JacksAreGrounded
		{
			get
			{
				return GetDeviceStatusFlag(0, 4);
			}
			set
			{
				SetDeviceStatusFlag(0, 4, value);
			}
		}

		public bool JacksAreMoving
		{
			get
			{
				return GetDeviceStatusFlag(0, 8);
			}
			set
			{
				SetDeviceStatusFlag(0, 8, value);
			}
		}

		public bool ExcessAngleDetected
		{
			get
			{
				return GetDeviceStatusFlag(0, 16);
			}
			set
			{
				SetDeviceStatusFlag(0, 16, value);
			}
		}

		public bool ExcessTwistDetected
		{
			get
			{
				return GetDeviceStatusFlag(0, 32);
			}
			set
			{
				SetDeviceStatusFlag(0, 32, value);
			}
		}

		public UI_MODE LevelerUIMode
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				PAYLOAD deviceStatus = base.DeviceStatus;
				return (UI_MODE)((PAYLOAD)(ref deviceStatus))[1];
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				PAYLOAD deviceStatus = base.DeviceStatus;
				((PAYLOAD)(ref deviceStatus))[1] = (byte)value;
				base.DeviceStatus = deviceStatus;
			}
		}

		public bool Button01Enabled
		{
			get
			{
				return GetDeviceStatusFlag(4, 1);
			}
			set
			{
				SetDeviceStatusFlag(4, 1, value);
			}
		}

		public bool Button02Enabled
		{
			get
			{
				return GetDeviceStatusFlag(4, 2);
			}
			set
			{
				SetDeviceStatusFlag(4, 2, value);
			}
		}

		public bool Button03Enabled
		{
			get
			{
				return GetDeviceStatusFlag(4, 4);
			}
			set
			{
				SetDeviceStatusFlag(4, 4, value);
			}
		}

		public bool Button04Enabled
		{
			get
			{
				return GetDeviceStatusFlag(4, 8);
			}
			set
			{
				SetDeviceStatusFlag(4, 8, value);
			}
		}

		public bool Button05Enabled
		{
			get
			{
				return GetDeviceStatusFlag(4, 16);
			}
			set
			{
				SetDeviceStatusFlag(4, 16, value);
			}
		}

		public bool Button06Enabled
		{
			get
			{
				return GetDeviceStatusFlag(4, 32);
			}
			set
			{
				SetDeviceStatusFlag(4, 32, value);
			}
		}

		public bool Button07Enabled
		{
			get
			{
				return GetDeviceStatusFlag(4, 64);
			}
			set
			{
				SetDeviceStatusFlag(4, 64, value);
			}
		}

		public bool Button08Enabled
		{
			get
			{
				return GetDeviceStatusFlag(4, 128);
			}
			set
			{
				SetDeviceStatusFlag(4, 128, value);
			}
		}

		public bool Button09Enabled
		{
			get
			{
				return GetDeviceStatusFlag(3, 1);
			}
			set
			{
				SetDeviceStatusFlag(3, 1, value);
			}
		}

		public bool Button10Enabled
		{
			get
			{
				return GetDeviceStatusFlag(3, 2);
			}
			set
			{
				SetDeviceStatusFlag(3, 2, value);
			}
		}

		public bool Button11Enabled
		{
			get
			{
				return GetDeviceStatusFlag(3, 4);
			}
			set
			{
				SetDeviceStatusFlag(3, 4, value);
			}
		}

		public bool Button12Enabled
		{
			get
			{
				return GetDeviceStatusFlag(3, 8);
			}
			set
			{
				SetDeviceStatusFlag(3, 8, value);
			}
		}

		public bool Button13Enabled
		{
			get
			{
				return GetDeviceStatusFlag(3, 16);
			}
			set
			{
				SetDeviceStatusFlag(3, 16, value);
			}
		}

		public bool Button14Enabled
		{
			get
			{
				return GetDeviceStatusFlag(3, 32);
			}
			set
			{
				SetDeviceStatusFlag(3, 32, value);
			}
		}

		public bool Button15Enabled
		{
			get
			{
				return GetDeviceStatusFlag(3, 64);
			}
			set
			{
				SetDeviceStatusFlag(3, 64, value);
			}
		}

		public bool Button16Enabled
		{
			get
			{
				return GetDeviceStatusFlag(3, 128);
			}
			set
			{
				SetDeviceStatusFlag(3, 128, value);
			}
		}

		public bool Button17Enabled
		{
			get
			{
				return GetDeviceStatusFlag(2, 1);
			}
			set
			{
				SetDeviceStatusFlag(2, 1, value);
			}
		}

		public bool Button18Enabled
		{
			get
			{
				return GetDeviceStatusFlag(2, 2);
			}
			set
			{
				SetDeviceStatusFlag(2, 2, value);
			}
		}

		public bool Button19Enabled
		{
			get
			{
				return GetDeviceStatusFlag(2, 4);
			}
			set
			{
				SetDeviceStatusFlag(2, 4, value);
			}
		}

		public bool Button20Enabled
		{
			get
			{
				return GetDeviceStatusFlag(2, 8);
			}
			set
			{
				SetDeviceStatusFlag(2, 8, value);
			}
		}

		public bool Button21Enabled
		{
			get
			{
				return GetDeviceStatusFlag(2, 16);
			}
			set
			{
				SetDeviceStatusFlag(2, 16, value);
			}
		}

		public bool Button22Enabled
		{
			get
			{
				return GetDeviceStatusFlag(2, 32);
			}
			set
			{
				SetDeviceStatusFlag(2, 32, value);
			}
		}

		public bool Button23Enabled
		{
			get
			{
				return GetDeviceStatusFlag(2, 64);
			}
			set
			{
				SetDeviceStatusFlag(2, 64, value);
			}
		}

		public bool Button24Enabled
		{
			get
			{
				return GetDeviceStatusFlag(2, 128);
			}
			set
			{
				SetDeviceStatusFlag(2, 128, value);
			}
		}

		public double XAngleDegrees
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				PAYLOAD deviceStatus = base.DeviceStatus;
				double num = ((PAYLOAD)(ref deviceStatus))[5] >> 4;
				num += (double)(((PAYLOAD)(ref deviceStatus))[6] >> 3) / 32.0;
				if ((((PAYLOAD)(ref deviceStatus))[6] & 4) != 0)
				{
					num = 0.0 - num;
				}
				return num;
			}
			set
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				Tuple<int, int, bool> val = DecomposeAngle(value);
				PAYLOAD deviceStatus = base.DeviceStatus;
				((PAYLOAD)(ref deviceStatus))[5] = (byte)((val.Item1 << 4) | (((PAYLOAD)(ref deviceStatus))[5] & 0xF));
				((PAYLOAD)(ref deviceStatus))[6] = (byte)((val.Item2 << 3) | (val.Item3 ? 4 : 0) | (((PAYLOAD)(ref deviceStatus))[6] & 3));
				base.DeviceStatus = deviceStatus;
			}
		}

		public double YAngleDegrees
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				PAYLOAD deviceStatus = base.DeviceStatus;
				double num = ((PAYLOAD)(ref deviceStatus))[5] & 0xF;
				num += (double)(((PAYLOAD)(ref deviceStatus))[7] >> 3) / 32.0;
				if ((((PAYLOAD)(ref deviceStatus))[7] & 4) != 0)
				{
					num = 0.0 - num;
				}
				return num;
			}
			set
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				Tuple<int, int, bool> val = DecomposeAngle(value);
				PAYLOAD deviceStatus = base.DeviceStatus;
				((PAYLOAD)(ref deviceStatus))[5] = (byte)((((PAYLOAD)(ref deviceStatus))[5] & 0xF0) | (val.Item1 & 0xF));
				((PAYLOAD)(ref deviceStatus))[7] = (byte)((val.Item2 << 3) | (val.Item3 ? 4 : 0) | (((PAYLOAD)(ref deviceStatus))[7] & 3));
				base.DeviceStatus = deviceStatus;
			}
		}

		public byte Bubble
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				PAYLOAD deviceStatus = base.DeviceStatus;
				return (byte)(((((PAYLOAD)(ref deviceStatus))[6] & 3) << 2) | (((PAYLOAD)(ref deviceStatus))[7] & 3));
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				PAYLOAD deviceStatus = base.DeviceStatus;
				((PAYLOAD)(ref deviceStatus))[6] = (byte)((((PAYLOAD)(ref deviceStatus))[6] & 0xFC) | ((value >> 2) & 3));
				((PAYLOAD)(ref deviceStatus))[7] = (byte)((((PAYLOAD)(ref deviceStatus))[7] & 0xFC) | (value & 3));
				base.DeviceStatus = deviceStatus;
			}
		}

		public TEXT_CONSOLE_SIZE TextConsoleSize
		{
			get
			{
				if (base.TextConsole == null)
				{
					return default(TEXT_CONSOLE_SIZE);
				}
				return base.TextConsole.Size;
			}
			set
			{
				int num = Lines.Length;
				int w = 32;
				if (value.Height > 0 && value.Height <= Lines.Length)
				{
					num = value.Height;
					if (value.Width > 0 && value.Width <= 32)
					{
						w = value.Width;
					}
				}
				CreateTextConsole(new TEXT_CONSOLE_SIZE(w, num));
				for (int i = 0; i < num; i++)
				{
					Lines[i] = $"Line #{i}";
				}
				base.TextConsole.Lines = Lines;
			}
		}

		private UInt48 LEVELER_AUTO_MODE_PROGRESS
		{
			get
			{
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				uint num = (uint)LevelerUIMode;
				if (LevelerUIMode == UI_MODE.AUTO)
				{
					uint num2 = 10u;
					TimeSpan elapsedTime = UiModeTime.ElapsedTime;
					uint num3 = Math.Min(num2, (uint)((TimeSpan)(ref elapsedTime)).TotalSeconds / 5);
					num |= num2 << 8;
					num |= num3 << 16;
				}
				return UInt48.op_Implicit(num);
			}
		}

		public LEVELER_TYPE_4(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, CAPABILITIES capabilities, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), DEVICE_TYPE.op_Implicit((byte)40), 0, FUNCTION_NAME.op_Implicit((ushort)109), 4, capabilities.Value, options)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			base.DeviceStatus = new PAYLOAD(8);
			AddPID(PID.BATTERY_VOLTAGE, (Func<UInt48>)(() => UInt48.op_Implicit(819187u)));
			AddPID(PID.LEVELER_UI_SUPPORTED_FEATURES, (Func<UInt48>)(() => UInt48.op_Implicit(4294967295u)));
			AddPID(PID.LEVELER_SENSOR_TOPOLOGY, (Func<UInt48>)(() => UInt48.op_Implicit((byte)1)));
			AddPID(PID.LEVELER_DRIVE_TYPE, (Func<UInt48>)(() => UInt48.op_Implicit((byte)1)));
			AddPID(PID.LEVELER_AUTO_MODE_PROGRESS, (Func<UInt48>)([CompilerGenerated] () => LEVELER_AUTO_MODE_PROGRESS));
			if (capabilities.IsJackPositionSupported)
			{
				switch (capabilities.JackConfiguration)
				{
				case JACK_CONFIGURATION.FOUR_JACKS:
				case JACK_CONFIGURATION.FOUR_JACKS_PLUS_TONGUE_JACK:
					AddPID(PID.LEFT_FRONT_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(LFJackStrokeInches * 65536.0)));
					AddPID(PID.RIGHT_FRONT_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(RFJackStrokeInches * 65536.0)));
					AddPID(PID.LEFT_REAR_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(LRJackStrokeInches * 65536.0)));
					AddPID(PID.RIGHT_REAR_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(RRJackStrokeInches * 65536.0)));
					AddPID(PID.LEFT_FRONT_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.RIGHT_FRONT_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.LEFT_REAR_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.RIGHT_REAR_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					break;
				case JACK_CONFIGURATION.SIX_JACKS:
					AddPID(PID.LEFT_FRONT_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(LFJackStrokeInches * 65536.0)));
					AddPID(PID.RIGHT_FRONT_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(RFJackStrokeInches * 65536.0)));
					AddPID(PID.LEFT_MIDDLE_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(LMJackStrokeInches * 65536.0)));
					AddPID(PID.RIGHT_MIDDLE_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(RMJackStrokeInches * 65536.0)));
					AddPID(PID.LEFT_REAR_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(LRJackStrokeInches * 65536.0)));
					AddPID(PID.RIGHT_REAR_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(RRJackStrokeInches * 65536.0)));
					AddPID(PID.LEFT_FRONT_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.RIGHT_FRONT_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.LEFT_MIDDLE_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.RIGHT_MIDDLE_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.LEFT_REAR_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.RIGHT_REAR_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					break;
				case JACK_CONFIGURATION.THREE_JACKS:
					AddPID(PID.LEFT_FRONT_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(LFJackStrokeInches * 65536.0)));
					AddPID(PID.RIGHT_FRONT_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(LFJackStrokeInches * 65536.0)));
					AddPID(PID.LEFT_REAR_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(LRJackStrokeInches * 65536.0)));
					AddPID(PID.RIGHT_REAR_JACK_STROKE_INCHES, (Func<UInt48>)([CompilerGenerated] () => (UInt48)(int)Math.Round(RRJackStrokeInches * 65536.0)));
					AddPID(PID.LEFT_FRONT_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.RIGHT_FRONT_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.LEFT_MIDDLE_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.RIGHT_MIDDLE_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.LEFT_REAR_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					AddPID(PID.RIGHT_REAR_JACK_MAX_STROKE_INCHES, (Func<UInt48>)(() => (UInt48)(int)Math.Round(983040.0)));
					break;
				}
			}
			SetUiMode(UI_MODE.IDLE, force_update: true);
		}

		private bool GetDeviceStatusFlag(byte index, byte flag)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			PAYLOAD deviceStatus = base.DeviceStatus;
			return (((PAYLOAD)(ref deviceStatus))[(int)index] & flag) != 0;
		}

		private void SetDeviceStatusFlag(byte index, byte flag, bool value)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			PAYLOAD deviceStatus = base.DeviceStatus;
			if (value)
			{
				((PAYLOAD)(ref deviceStatus))[(int)index] = (byte)(((PAYLOAD)(ref deviceStatus))[(int)index] | flag);
			}
			else
			{
				((PAYLOAD)(ref deviceStatus))[(int)index] = (byte)(((PAYLOAD)(ref deviceStatus))[(int)index] & ~flag);
			}
			base.DeviceStatus = deviceStatus;
		}

		private Tuple<int, int, bool> DecomposeAngle(double angle)
		{
			bool flag = angle < 0.0;
			if (flag)
			{
				angle = 0.0 - angle;
			}
			if (angle > 15.96875)
			{
				angle = 15.96875;
			}
			int num = (int)Math.Round(32.0 * angle);
			int num2 = num & 0x1F;
			int num3 = num >> 5;
			if (num3 > 15)
			{
				return Tuple.Create<int, int, bool>(15, 31, flag);
			}
			return Tuple.Create<int, int, bool>(num3, num2, flag);
		}

		protected override void OnBackgroundTask()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			base.OnBackgroundTask();
			if (!GetLocalSessionClientAddress(SESSION_ID.op_Implicit((ushort)4)).IsValidDeviceAddress)
			{
				if (!IsManualChecked)
				{
					SetUiMode(UI_MODE.IDLE);
				}
				ButtonCommand = 0u;
			}
			if (!IsManualChecked)
			{
				TimeSpan elapsedTime = AngleTime.ElapsedTime;
				double num = ((TimeSpan)(ref elapsedTime)).TotalSeconds / 5.0;
				XAngleDegrees = 15.0 * Math.Cos(3.0 * num);
				YAngleDegrees = 15.0 * Math.Sin(4.0 * num);
				IsLevel = Math.Abs(XAngleDegrees) <= 1.0 && Math.Abs(YAngleDegrees) <= 1.0;
				ExcessAngleDetected = Math.Abs(XAngleDegrees) > 12.0 || Math.Abs(YAngleDegrees) > 12.0;
				ExcessTwistDetected = Math.Abs(XAngleDegrees) > 10.0 && Math.Abs(YAngleDegrees) > 10.0;
			}
			switch (LevelerUIMode)
			{
			case UI_MODE.AUTO:
				if (!IsManualChecked)
				{
					JacksAreMoving = true;
				}
				break;
			case UI_MODE.MANUAL:
			case UI_MODE.MANUAL_CONSOLE:
			case UI_MODE.ZERO:
			case UI_MODE.FAULT_JACK_MANUAL:
			case UI_MODE.FAULT_JACK_MANUAL_CONSOLE:
				if ((ButtonCommand & 1) != 0)
				{
					RFJackStrokeInches += 0.01;
				}
				if ((ButtonCommand & 2) != 0)
				{
					RFJackStrokeInches -= 0.01;
				}
				if ((ButtonCommand & 4) != 0)
				{
					LFJackStrokeInches += 0.01;
				}
				if ((ButtonCommand & 8) != 0)
				{
					LFJackStrokeInches -= 0.01;
				}
				if ((ButtonCommand & 0x10) != 0)
				{
					RRJackStrokeInches += 0.01;
				}
				if ((ButtonCommand & 0x20) != 0)
				{
					RRJackStrokeInches -= 0.01;
				}
				if ((ButtonCommand & 0x40) != 0)
				{
					LRJackStrokeInches += 0.01;
				}
				if ((ButtonCommand & 0x80) != 0)
				{
					LRJackStrokeInches -= 0.01;
				}
				if ((ButtonCommand & 0x100) != 0)
				{
					TongueJackStrokeInches += 0.01;
				}
				if ((ButtonCommand & 0x200) != 0)
				{
					TongueJackStrokeInches -= 0.01;
				}
				if ((ButtonCommand & 0x400) != 0)
				{
					RMJackStrokeInches += 0.01;
				}
				if ((ButtonCommand & 0x800) != 0)
				{
					RMJackStrokeInches -= 0.01;
				}
				if ((ButtonCommand & 0x1000) != 0)
				{
					LMJackStrokeInches += 0.01;
				}
				if ((ButtonCommand & 0x2000) != 0)
				{
					LMJackStrokeInches -= 0.01;
				}
				break;
			}
			if (!IsManualChecked)
			{
				JacksAreFullyRetracted = LFJackStrokeInches <= 0.0 && RFJackStrokeInches <= 0.0 && LMJackStrokeInches <= 0.0 && RMJackStrokeInches <= 0.0 && LRJackStrokeInches <= 0.0 && RRJackStrokeInches <= 0.0;
			}
			if (!IsManualChecked)
			{
				JacksAreGrounded = LFJackStrokeInches >= 7.5 && RFJackStrokeInches >= 7.5 && LMJackStrokeInches >= 7.5 && RMJackStrokeInches >= 7.5 && LRJackStrokeInches >= 7.5 && RRJackStrokeInches >= 7.5;
			}
		}

		protected override void OnLocalDeviceRxEvent(AdapterRxEvent rx)
		{
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			base.OnLocalDeviceRxEvent(rx);
			if ((byte)rx.MessageType != 130 || rx.TargetAddress != base.Address || rx.SourceAddress != GetLocalSessionClientAddress(SESSION_ID.op_Implicit((ushort)4)) || rx.SourceDevice == null)
			{
				return;
			}
			switch (rx.MessageData)
			{
			case 0:
				if (rx.Length != 4)
				{
					break;
				}
				CommandEnhanced = (ENHANCED_MODE)rx.MessageData;
				CommandLEVELER_UI_MODE = (UI_MODE)rx[0];
				CommandUI_Button_State = (uint)((rx[1] << 16) | (rx[2] << 8) | rx[3]);
				if (IsManualChecked)
				{
					break;
				}
				if (rx.SourceDevice.ProductID.Name == "Simulated Product" && (uint)rx[0] == (uint)LevelerUIMode)
				{
					uint buttonCommand = ButtonCommand;
					ButtonCommand = UInt24.op_Implicit(CommExtensions.GetUINT24((IByteList)(object)rx, 1));
					uint num = (ButtonCommand ^ buttonCommand) & ButtonCommand;
					switch (LevelerUIMode)
					{
					case UI_MODE.IDLE:
						if ((num & 1) != 0)
						{
							SetUiMode(UI_MODE.AUTO);
						}
						if ((num & 2) != 0)
						{
							SetUiMode(UI_MODE.AUTO);
						}
						if ((num & 4) != 0)
						{
							SetUiMode(UI_MODE.AUTO);
						}
						if ((num & 8) != 0)
						{
							SetUiMode(UI_MODE.AUTO);
						}
						if ((num & 0x10) != 0)
						{
							SetUiMode(UI_MODE.AUTO);
						}
						if ((num & 0x20) != 0)
						{
							SetUiMode(UI_MODE.MANUAL);
						}
						if ((num & 0x40) != 0)
						{
							SetUiMode(UI_MODE.MANUAL_AIR_BAG_CONTROL);
						}
						if ((num & 0x80) != 0)
						{
							SetUiMode(UI_MODE.ZERO);
						}
						if ((num & 0x100) != 0)
						{
							SetUiMode(UI_MODE.MANUAL);
						}
						if ((num & 0x200) != 0)
						{
							SetUiMode(UI_MODE.YES_NO);
						}
						break;
					case UI_MODE.MANUAL:
					case UI_MODE.MANUAL_CONSOLE:
						if (!IsManualChecked)
						{
							JacksAreMoving = (ButtonCommand & 0x3FFFF) != 0;
						}
						break;
					case UI_MODE.ZERO:
						if (!IsManualChecked)
						{
							JacksAreMoving = (ButtonCommand & 0x3FFFF) != 0;
						}
						if ((num & 0x40000) != 0)
						{
							SetUiMode(UI_MODE.IDLE);
						}
						break;
					case UI_MODE.INFO:
						if ((num & 1) != 0)
						{
							SetUiMode(UI_MODE.IDLE);
						}
						break;
					case UI_MODE.YES_NO:
						if ((num & 1) != 0)
						{
							SetUiMode(UI_MODE.IDLE);
						}
						if ((num & 2) != 0)
						{
							SetUiMode(UI_MODE.IDLE);
						}
						break;
					case UI_MODE.FAULT_GENERIC:
						if ((num & 1) != 0)
						{
							SetUiMode(UI_MODE.IDLE);
						}
						break;
					case UI_MODE.FAULT_JACK_MANUAL:
					case UI_MODE.FAULT_JACK_MANUAL_CONSOLE:
						if (!IsManualChecked)
						{
							JacksAreMoving = (ButtonCommand & 0x3FFFFF) != 0;
						}
						if ((num & 0x400000) != 0)
						{
							SetUiMode(UI_MODE.AUTO);
						}
						break;
					case UI_MODE.AUTO:
					case UI_MODE.MANUAL_AIR_BAG_CONTROL:
						break;
					}
				}
				else if (rx.SourceDevice.ProductID.Name != "Simulated Product")
				{
					ButtonCommand = UInt24.op_Implicit(CommExtensions.GetUINT24((IByteList)(object)rx, 1));
					SetUiMode((UI_MODE)rx[0]);
				}
				break;
			case 1:
				if (rx.Length == 0)
				{
					CommandEnhanced = (ENHANCED_MODE)rx.MessageData;
					SetUiMode(UI_MODE.IDLE);
				}
				break;
			case 2:
				if (rx.Length == 1 && (uint)rx[0] == (uint)LevelerUIMode)
				{
					CommandEnhanced = (ENHANCED_MODE)rx.MessageData;
					CommandLEVELER_UI_MODE = (UI_MODE)rx[0];
					if (LevelerUIMode != UI_MODE.IDLE)
					{
						SetUiMode(LevelerUIMode - 1);
					}
				}
				break;
			case 3:
				if (rx.Length == 0)
				{
					CommandEnhanced = (ENHANCED_MODE)rx.MessageData;
					SetUiMode(UI_MODE.IDLE);
				}
				break;
			}
		}

		public void SetUiMode(UI_MODE mode, bool force_update = false)
		{
			if (force_update || LevelerUIMode != mode)
			{
				LevelerUIMode = mode;
				UiModeTime.Reset();
				ButtonCommand = 0u;
				if (!IsManualChecked)
				{
					JacksAreMoving = false;
				}
				switch (mode)
				{
				case UI_MODE.IDLE:
					LFJackStrokeInches = 0.0;
					RFJackStrokeInches = 0.0;
					LMJackStrokeInches = 0.0;
					RMJackStrokeInches = 0.0;
					LRJackStrokeInches = 0.0;
					RRJackStrokeInches = 0.0;
					TongueJackStrokeInches = 0.0;
					break;
				case UI_MODE.AUTO:
				case UI_MODE.MANUAL:
				case UI_MODE.MANUAL_CONSOLE:
				case UI_MODE.ZERO:
					LFJackStrokeInches = 8.5;
					RFJackStrokeInches = 8.5;
					LMJackStrokeInches = 8.5;
					RMJackStrokeInches = 8.5;
					LRJackStrokeInches = 8.5;
					RRJackStrokeInches = 8.5;
					TongueJackStrokeInches = 8.5;
					break;
				}
				int num = 0;
				num = mode switch
				{
					UI_MODE.IDLE => 10, 
					UI_MODE.AUTO => 0, 
					UI_MODE.MANUAL => 10, 
					UI_MODE.MANUAL_CONSOLE => 10, 
					UI_MODE.ZERO => 11, 
					UI_MODE.INFO => 1, 
					UI_MODE.YES_NO => 2, 
					UI_MODE.FAULT_GENERIC => 1, 
					UI_MODE.FAULT_JACK_MANUAL => 15, 
					UI_MODE.FAULT_JACK_MANUAL_CONSOLE => 15, 
					UI_MODE.MANUAL_AIR_BAG_CONTROL => 2, 
					_ => 1, 
				};
				Button01Enabled = num >= 1;
				Button02Enabled = num >= 2;
				Button03Enabled = num >= 3;
				Button04Enabled = num >= 4;
				Button05Enabled = num >= 5;
				Button06Enabled = num >= 6;
				Button07Enabled = num >= 7;
				Button08Enabled = num >= 8;
				Button09Enabled = num >= 9;
				Button10Enabled = num >= 10;
				Button11Enabled = num >= 11;
				Button12Enabled = num >= 12;
				Button13Enabled = num >= 13;
				Button14Enabled = num >= 14;
				Button15Enabled = num >= 15;
				Button16Enabled = num >= 16;
				Button17Enabled = num >= 17;
				Button18Enabled = num >= 18;
				Button19Enabled = num >= 19;
				Button20Enabled = num >= 20;
				Button21Enabled = num >= 21;
				Button22Enabled = num >= 22;
				Button23Enabled = num >= 23;
				Button24Enabled = num >= 24;
			}
		}
	}
	public class MOMENTARY_H_BRIDGE_TYPE_2_SIM_INTERFACE : RELAY_TYPE_2_SIM_INTERFACE
	{
		public enum COMMAND_MODE : byte
		{
			STOP,
			FORWARD,
			REVERSE,
			CLEAR_OUTPUT_DISABLED_LATCH,
			HOME_RESET,
			AUTO_FORWARD,
			AUTO_REVERSE
		}

		private struct COMMAND
		{
			private int Value;

			public COMMAND_MODE Option
			{
				get
				{
					return (COMMAND_MODE)GetBits(3, 6);
				}
				set
				{
					SetBits((int)value, 3, 6);
				}
			}

			private int GetBits(int bit, int shift)
			{
				return (Value >> shift) & bit;
			}

			private void SetBits(int val, int bit, int shift)
			{
				val <<= shift;
				bit <<= shift;
				Value = (Value & ~bit) | (val & bit);
			}

			public static implicit operator int(COMMAND s)
			{
				return s.Value;
			}

			public static implicit operator byte(COMMAND s)
			{
				return (byte)s.Value;
			}

			public static implicit operator COMMAND(byte b)
			{
				COMMAND result = default(COMMAND);
				result.Value = b;
				return result;
			}

			public static implicit operator COMMAND(int i)
			{
				COMMAND result = default(COMMAND);
				result.Value = i;
				return result;
			}
		}

		private COMMAND _command = 0;

		private COMMAND Command
		{
			get
			{
				return _command;
			}
			set
			{
				_command = value;
				switch ((uint)(byte)_command)
				{
				case 0u:
					if (!base.OutputDisabledLatch && (base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.FORWARD_EXTEND_CLOCKWISE_OUT_UP || base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.REVERSE_RETRACT_COUNTERCLOCKWISE_IN_DOWN))
					{
						base.OutputState = RELAY_TYPE_2_OUTPUT_STATE.OFF_STOP;
					}
					break;
				case 1u:
					if (!base.OutputDisabledLatch && (base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.OFF_STOP || base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.REVERSE_RETRACT_COUNTERCLOCKWISE_IN_DOWN) && base.ForwardCommandAllowed)
					{
						base.OutputState = RELAY_TYPE_2_OUTPUT_STATE.FORWARD_EXTEND_CLOCKWISE_OUT_UP;
					}
					break;
				case 2u:
					if (!base.OutputDisabledLatch && (base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.OFF_STOP || base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.FORWARD_EXTEND_CLOCKWISE_OUT_UP) && base.ReverseCommandAllowed)
					{
						base.OutputState = RELAY_TYPE_2_OUTPUT_STATE.REVERSE_RETRACT_COUNTERCLOCKWISE_IN_DOWN;
					}
					break;
				case 3u:
					if (base.OutputDisabledLatch)
					{
						base.OutputDisabledLatch = false;
					}
					break;
				}
				UpdateStatus();
			}
		}

		public MOMENTARY_H_BRIDGE_TYPE_2_SIM_INTERFACE(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(adapter, software_part_number, product_id, version, new DEVICE_ID(product_id, 0, DEVICE_TYPE.op_Implicit((byte)33), 0, FUNCTION_NAME.op_Implicit((ushort)105), 0, (byte)0), options)
		{
			Init();
			AddPID(PID.EXTENDED_DEVICE_CAPABILITIES, [CompilerGenerated] () => UInt48.op_Implicit(base.EXTENDEDDEVICECAPABILITIES), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				base.EXTENDEDDEVICECAPABILITIES = (byte)arg;
			});
			AddPID(PID.CLOUD_CAPABILITIES, [CompilerGenerated] () => UInt48.op_Implicit(base.CLOUDCAPABILITIES), [CompilerGenerated] (UInt48 arg) =>
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				base.CLOUDCAPABILITIES = (byte)arg;
			});
		}

		protected override void Init()
		{
			UpdateStatus();
		}

		protected override void UpdateDeviceCapabilities()
		{
			byte b = 0;
			byte b2 = Convert.ToByte(base.Supports_SoftwareConfigurableFuse);
			b |= b2;
			b2 = Convert.ToByte(base.Supports_CoarsePosition);
			b |= (byte)(b2 << 1);
			b2 = Convert.ToByte(base.Supports_FinePosition);
			b |= (byte)(b2 << 2);
			b2 = Convert.ToByte(base.PhysicalSwitchType);
			b |= (byte)(b2 << 3);
			b2 = Convert.ToByte(base.Supports_Homing);
			b |= (byte)(b2 << 5);
			base.DeviceCapabilities = b;
		}

		protected override void UpdateStatus()
		{
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			PAYLOAD deviceStatus = default(PAYLOAD);
			((PAYLOAD)(ref deviceStatus))..ctor(6);
			byte b = 0;
			byte b2 = Convert.ToByte((object)base.OutputState);
			b |= b2;
			b2 = Convert.ToByte(base.OutputDisabledLatch);
			b |= (byte)(b2 << 5);
			b2 = Convert.ToByte(base.ReverseCommandAllowed);
			b |= (byte)(b2 << 6);
			b2 = Convert.ToByte(base.ForwardCommandAllowed);
			b |= (byte)(b2 << 7);
			((PAYLOAD)(ref deviceStatus))[0] = b;
			((PAYLOAD)(ref deviceStatus))[1] = base.OutputPositionPct;
			uint num = (uint)Math.Round((double)(base.CurrentDraw * 256f));
			if (num > 65535)
			{
				num = 65535u;
			}
			if (num < 0)
			{
				num = 0u;
			}
			((PAYLOAD)(ref deviceStatus))[2] = (byte)(num >> 8);
			((PAYLOAD)(ref deviceStatus))[3] = (byte)num;
			if (base.OutputDisabledLatch)
			{
				((PAYLOAD)(ref deviceStatus))[4] = (byte)(base.UserMessage >> 8);
				((PAYLOAD)(ref deviceStatus))[5] = (byte)base.UserMessage;
			}
			base.DeviceStatus = deviceStatus;
		}

		protected override void OnLocalDeviceRxEvent(AdapterRxEvent rx)
		{
			base.OnLocalDeviceRxEvent(rx);
			if (rx.TargetAddress != base.Address)
			{
				return;
			}
			if ((byte)rx.MessageType == 130)
			{
				if (rx.SourceAddress == GetLocalSessionClientAddress(SESSION_ID.op_Implicit((ushort)4)) && rx.Count == 0)
				{
					Command = rx.MessageData;
				}
			}
			else if ((byte)rx.MessageType == 128 && (byte)(REQUEST)rx.MessageData == 69 && GetLocalSession(SESSION_ID.op_Implicit((ushort)4))?.Client?.Address == rx.SourceAddress && (base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.FORWARD_EXTEND_CLOCKWISE_OUT_UP || base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.REVERSE_RETRACT_COUNTERCLOCKWISE_IN_DOWN))
			{
				base.OutputState = RELAY_TYPE_2_OUTPUT_STATE.OFF_STOP;
			}
		}
	}
	public class MOMENTARY_RELAY_TYPE_2_SIM_INTERFACE : RELAY_TYPE_2_SIM_INTERFACE
	{
		public enum COMMAND_MODE : byte
		{
			OFF = 0,
			ON = 1,
			CLEAR_OUTPUT_DISABLED_LATCH = 3
		}

		private struct COMMAND
		{
			private int Value;

			public COMMAND_MODE Option
			{
				get
				{
					return (COMMAND_MODE)GetBits(3, 6);
				}
				set
				{
					SetBits((int)value, 3, 6);
				}
			}

			private int GetBits(int bit, int shift)
			{
				return (Value >> shift) & bit;
			}

			private void SetBits(int val, int bit, int shift)
			{
				val <<= shift;
				bit <<= shift;
				Value = (Value & ~bit) | (val & bit);
			}

			public static implicit operator int(COMMAND s)
			{
				return s.Value;
			}

			public static implicit operator byte(COMMAND s)
			{
				return (byte)s.Value;
			}

			public static implicit operator COMMAND(byte b)
			{
				COMMAND result = default(COMMAND);
				result.Value = b;
				return result;
			}

			public static implicit operator COMMAND(int i)
			{
				COMMAND result = default(COMMAND);
				result.Value = i;
				return result;
			}
		}

		private COMMAND _command = 0;

		private COMMAND Command
		{
			get
			{
				return _command;
			}
			set
			{
				_command = value;
				switch ((uint)(byte)_command)
				{
				case 0u:
					if (!base.OutputDisabledLatch && base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.ON)
					{
						base.OutputState = RELAY_TYPE_2_OUTPUT_STATE.OFF_STOP;
					}
					break;
				case 1u:
					if (!base.OutputDisabledLatch && base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.OFF_STOP && base.OnCommandAllowed)
					{
						base.OutputState = RELAY_TYPE_2_OUTPUT_STATE.ON;
					}
					break;
				case 3u:
					if (base.OutputDisabledLatch)
					{
						base.OutputDisabledLatch = false;
					}
					break;
				}
				UpdateStatus();
			}
		}

		public MOMENTARY_RELAY_TYPE_2_SIM_INTERFACE(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(adapter, software_part_number, product_id, version, new DEVICE_ID(product_id, 0, DEVICE_TYPE.op_Implicit((byte)31), 0, FUNCTION_NAME.op_Implicit((ushort)7), 0, (byte)0), options)
		{
			Init();
		}

		protected override void Init()
		{
			UpdateStatus();
		}

		protected override void UpdateDeviceCapabilities()
		{
			byte b = 0;
			byte b2 = Convert.ToByte(base.Supports_SoftwareConfigurableFuse);
			b |= b2;
			b2 = Convert.ToByte(base.Supports_CoarsePosition);
			b |= (byte)(b2 << 1);
			b2 = Convert.ToByte(base.Supports_FinePosition);
			b |= (byte)(b2 << 2);
			b2 = Convert.ToByte(base.PhysicalSwitchType);
			b |= (byte)(b2 << 3);
			b2 = Convert.ToByte(base.Supports_Homing);
			b |= (byte)(b2 << 5);
			base.DeviceCapabilities = b;
		}

		protected override void UpdateStatus()
		{
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			PAYLOAD deviceStatus = default(PAYLOAD);
			((PAYLOAD)(ref deviceStatus))..ctor(6);
			byte b = 0;
			byte b2 = Convert.ToByte((object)base.OutputState);
			b |= b2;
			b2 = Convert.ToByte(base.OutputDisabledLatch);
			b |= (byte)(b2 << 5);
			b2 = Convert.ToByte(base.OnCommandAllowed);
			b |= (byte)(b2 << 7);
			((PAYLOAD)(ref deviceStatus))[0] = b;
			((PAYLOAD)(ref deviceStatus))[1] = base.OutputPositionPct;
			uint num = (uint)Math.Round((double)(base.CurrentDraw * 256f));
			if (num > 65535)
			{
				num = 65535u;
			}
			if (num < 0)
			{
				num = 0u;
			}
			((PAYLOAD)(ref deviceStatus))[2] = (byte)(num >> 8);
			((PAYLOAD)(ref deviceStatus))[3] = (byte)num;
			if (base.OutputDisabledLatch)
			{
				((PAYLOAD)(ref deviceStatus))[4] = (byte)(base.UserMessage >> 8);
				((PAYLOAD)(ref deviceStatus))[5] = (byte)base.UserMessage;
			}
			base.DeviceStatus = deviceStatus;
		}

		protected override void OnLocalDeviceRxEvent(AdapterRxEvent rx)
		{
			base.OnLocalDeviceRxEvent(rx);
			if (rx.TargetAddress != base.Address)
			{
				return;
			}
			if ((byte)rx.MessageType == 130)
			{
				if (rx.SourceAddress == GetLocalSessionClientAddress(SESSION_ID.op_Implicit((ushort)4)) && rx.Count == 0)
				{
					Command = rx.MessageData;
				}
			}
			else if ((byte)rx.MessageType == 128 && (byte)(REQUEST)rx.MessageData == 69 && GetLocalSession(SESSION_ID.op_Implicit((ushort)4))?.Client?.Address == rx.SourceAddress && base.OutputState == RELAY_TYPE_2_OUTPUT_STATE.ON)
			{
				base.OutputState = RELAY_TYPE_2_OUTPUT_STATE.OFF_STOP;
			}
		}
	}
	public class REAL_TIME_CLOCK : LocalDevice
	{
		public bool IsValid => base.Adapter.Clock.IsValid;

		public System.DateTime CurrentDateTime
		{
			get
			{
				return base.Adapter.Clock.CurrentDateTime;
			}
			set
			{
				base.Adapter.Clock.CurrentDateTime = value;
			}
		}

		public System.DateTime TimeLastSet => base.Adapter.Clock.TimeLastSet;

		public byte TIME_ZONE => base.Adapter.Clock.TIME_ZONE;

		public byte RTC_TIME_SEC
		{
			get
			{
				return base.Adapter.Clock.RTC_TIME_SEC;
			}
			set
			{
				base.Adapter.Clock.RTC_TIME_SEC = value;
			}
		}

		public byte RTC_TIME_MIN
		{
			get
			{
				return base.Adapter.Clock.RTC_TIME_MIN;
			}
			set
			{
				base.Adapter.Clock.RTC_TIME_MIN = value;
			}
		}

		public byte RTC_TIME_HOUR
		{
			get
			{
				return base.Adapter.Clock.RTC_TIME_HOUR;
			}
			set
			{
				base.Adapter.Clock.RTC_TIME_HOUR = value;
			}
		}

		public byte RTC_TIME_DAY
		{
			get
			{
				return base.Adapter.Clock.RTC_TIME_DAY;
			}
			set
			{
				base.Adapter.Clock.RTC_TIME_DAY = value;
			}
		}

		public byte RTC_TIME_MONTH
		{
			get
			{
				return base.Adapter.Clock.RTC_TIME_MONTH;
			}
			set
			{
				base.Adapter.Clock.RTC_TIME_MONTH = value;
			}
		}

		public ushort RTC_TIME_YEAR
		{
			get
			{
				return base.Adapter.Clock.RTC_TIME_YEAR;
			}
			set
			{
				base.Adapter.Clock.RTC_TIME_YEAR = value;
			}
		}

		public uint RTC_EPOCH_SEC
		{
			get
			{
				return base.Adapter.Clock.RTC_EPOCH_SEC;
			}
			set
			{
				base.Adapter.Clock.RTC_EPOCH_SEC = value;
			}
		}

		[field: CompilerGenerated]
		public uint RTC_SET_TIME_SEC
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public ushort TIME_SINCE_CLOCK_SET => base.Adapter.Clock.TIME_SINCE_CLOCK_SET;

		public REAL_TIME_CLOCK(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), DEVICE_TYPE.op_Implicit((byte)14), 0, FUNCTION_NAME.op_Implicit((ushort)150), 0, (byte)0, options)
		{
		}

		public void SetTime(int year, int month, int day, int hour, int minute, int second)
		{
			base.Adapter.Clock.SetTime(year, month, day, hour, minute, second);
		}

		protected override void OnBackgroundTask()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			base.OnBackgroundTask();
			base.DeviceStatus = new TIME_MESSAGE_PAYLOAD(RTC_EPOCH_SEC, TIME_SINCE_CLOCK_SET, TIME_ZONE);
		}
	}
	public enum RELAY_TYPE_2_OUTPUT_STATE : byte
	{
		OFF_STOP,
		ON,
		FORWARD_EXTEND_CLOCKWISE_OUT_UP,
		REVERSE_RETRACT_COUNTERCLOCKWISE_IN_DOWN
	}
	public class RELAY_TYPE_2_STATUS_PARAMS : IDeviceStatusParams
	{
		private PAYLOAD payload;

		private byte mOutputState;

		private byte mOutputPositionPct;

		private ushort mCurrentDraw;

		private ushort mUserMessage;

		[DeviceDisplay("Output State")]
		public string OutputDisplay
		{
			get
			{
				string text = ((object)(RELAY_TYPE_2_OUTPUT_STATE)(mOutputState & 0xF)/*cast due to .constrained prefix*/).ToString();
				if ((mOutputState & 0x20) != 0)
				{
					text += "\nUSER_CLEAR_REQUIRED";
				}
				if ((mOutputState & 0x40) == 0)
				{
					text += "\nREVERSE_COMMAND_NOT_ALLOWED";
				}
				if ((mOutputState & 0x80) == 0)
				{
					text += "\nFORWARD/ON_COMMAND_NOT_ALLOWED";
				}
				return text ?? "";
			}
		}

		public byte _OutputState
		{
			get
			{
				return mOutputState;
			}
			set
			{
				if (mOutputState != value)
				{
					mOutputState = value;
					((PAYLOAD)(ref payload))[0] = value;
				}
			}
		}

		[DeviceDisplay("Output Position")]
		public string OutputPositionDisplay => $"{mOutputPositionPct}%";

		public byte OutputPositionPct
		{
			get
			{
				return mOutputPositionPct;
			}
			set
			{
				mOutputPositionPct = value;
				((PAYLOAD)(ref payload))[1] = value;
			}
		}

		[DeviceDisplay("Current Draw")]
		public string CurrentDrawDisplay
		{
			get
			{
				float num = (float)(int)mCurrentDraw / 256f;
				return $"{num:0.##} A";
			}
		}

		public ushort CurrentDraw
		{
			get
			{
				return mCurrentDraw;
			}
			set
			{
				mCurrentDraw = value;
				((PAYLOAD)(ref payload))[2] = (byte)(value >> 8);
				((PAYLOAD)(ref payload))[3] = (byte)(value & 0xFF);
			}
		}

		[DeviceDisplay("User Message")]
		public string UserMessageDisplay => ((object)(DTC_ID)mUserMessage/*cast due to .constrained prefix*/).ToString();

		public ushort UserMessage
		{
			get
			{
				return mUserMessage;
			}
			set
			{
				mUserMessage = value;
				((PAYLOAD)(ref payload))[4] = (byte)(value >> 8);
				((PAYLOAD)(ref payload))[5] = (byte)(value & 0xFF);
			}
		}

		public RELAY_TYPE_2_STATUS_PARAMS()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			payload = new PAYLOAD(6);
			_OutputState = 0;
			OutputPositionPct = 0;
			CurrentDraw = 0;
			UserMessage = 0;
		}

		public PAYLOAD GetPayload()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return payload;
		}

		public void SetPayload(PAYLOAD pl)
		{
			_OutputState = ((PAYLOAD)(ref pl))[0];
			OutputPositionPct = ((PAYLOAD)(ref pl))[1];
			CurrentDraw = (ushort)((((PAYLOAD)(ref pl))[2] << 8) + ((PAYLOAD)(ref pl))[3]);
			UserMessage = (ushort)((((PAYLOAD)(ref pl))[4] << 8) + ((PAYLOAD)(ref pl))[5]);
		}
	}
	public class RELAY_TYPE_2_SIM_INTERFACE : LocalDevice
	{
		public enum PHYSICAL_SWITCH_TYPE : byte
		{
			NO_PHYSICAL_SWITCH,
			DIMMABLE_SWITCH,
			TOGGLE_SWITCH,
			MOMENTARY_SWITCH
		}

		public RELAY_TYPE_2_STATUS_PARAMS mStatusMessageParams;

		private bool _pidsAdded;

		private bool _supports_SoftwareConfigurableFuse;

		private bool _supports_CoarsePosition;

		private bool _supports_FinePosition;

		private bool _supports_Homing;

		private byte _physicalSwitchType;

		private bool _outputDisabledLatch;

		public RELAY_TYPE_2_OUTPUT_STATE OutputState
		{
			get
			{
				return (RELAY_TYPE_2_OUTPUT_STATE)(mStatusMessageParams._OutputState & 0xF);
			}
			set
			{
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				mStatusMessageParams._OutputState &= 240;
				mStatusMessageParams._OutputState |= (byte)(value & (RELAY_TYPE_2_OUTPUT_STATE)15);
				base.DeviceStatus = mStatusMessageParams.GetPayload();
			}
		}

		public bool OnCommandAllowed
		{
			get
			{
				return (mStatusMessageParams._OutputState & 0x80) != 0;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				if (value)
				{
					mStatusMessageParams._OutputState |= 128;
				}
				else
				{
					mStatusMessageParams._OutputState &= 127;
				}
				base.DeviceStatus = mStatusMessageParams.GetPayload();
			}
		}

		public bool ForwardCommandAllowed
		{
			get
			{
				return (mStatusMessageParams._OutputState & 0x80) != 0;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				if (value)
				{
					mStatusMessageParams._OutputState |= 128;
				}
				else
				{
					mStatusMessageParams._OutputState &= 127;
				}
				base.DeviceStatus = mStatusMessageParams.GetPayload();
			}
		}

		public bool ReverseCommandAllowed
		{
			get
			{
				return (mStatusMessageParams._OutputState & 0x40) != 0;
			}
			set
			{
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				if (ReverseCommandAllowed != value)
				{
					if (value)
					{
						mStatusMessageParams._OutputState |= 64;
					}
					else
					{
						mStatusMessageParams._OutputState &= 191;
					}
					base.DeviceStatus = mStatusMessageParams.GetPayload();
				}
			}
		}

		public bool UserClearRequired => (mStatusMessageParams._OutputState & 0x20) != 0;

		public byte OutputPositionPct
		{
			get
			{
				return mStatusMessageParams.OutputPositionPct;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatusMessageParams.OutputPositionPct = value;
				base.DeviceStatus = mStatusMessageParams.GetPayload();
			}
		}

		public float CurrentDraw
		{
			get
			{
				return (int)mStatusMessageParams.CurrentDraw;
			}
			set
			{
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				mStatusMessageParams.CurrentDraw = (ushort)((double)value * 256.0 + 0.5);
				base.DeviceStatus = mStatusMessageParams.GetPayload();
			}
		}

		public ushort UserMessage
		{
			get
			{
				return mStatusMessageParams.UserMessage;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatusMessageParams.UserMessage = value;
				base.DeviceStatus = mStatusMessageParams.GetPayload();
			}
		}

		public bool Supports_SoftwareConfigurableFuse
		{
			get
			{
				return _supports_SoftwareConfigurableFuse;
			}
			set
			{
				if (_supports_SoftwareConfigurableFuse == value)
				{
					return;
				}
				if (value && !_pidsAdded)
				{
					AddPID(PID.SOFTWARE_FUSE_RATING_AMPS, [CompilerGenerated] () => UInt48.op_Implicit(SOFTWARE_FUSE_RATING_AMPS), [CompilerGenerated] (UInt48 arg) =>
					{
						//IL_0001: Unknown result type (might be due to invalid IL or missing references)
						SOFTWARE_FUSE_RATING_AMPS = (uint)arg;
					});
					AddPID(PID.SOFTWARE_FUSE_MAX_RATING_AMPS, [CompilerGenerated] () => UInt48.op_Implicit(SOFTWARE_FUSE_MAX_RATING_AMPS), [CompilerGenerated] (UInt48 arg) =>
					{
						//IL_0001: Unknown result type (might be due to invalid IL or missing references)
						SOFTWARE_FUSE_MAX_RATING_AMPS = (uint)arg;
					});
					_pidsAdded = true;
				}
				_supports_SoftwareConfigurableFuse = value;
				UpdateDeviceCapabilities();
			}
		}

		public bool Supports_CoarsePosition
		{
			get
			{
				return _supports_CoarsePosition;
			}
			set
			{
				if (_supports_CoarsePosition != value)
				{
					_supports_CoarsePosition = value;
					UpdateDeviceCapabilities();
				}
			}
		}

		public bool Supports_FinePosition
		{
			get
			{
				return _supports_FinePosition;
			}
			set
			{
				if (_supports_FinePosition != value)
				{
					_supports_FinePosition = value;
					UpdateDeviceCapabilities();
				}
			}
		}

		public bool Supports_Homing
		{
			get
			{
				return _supports_Homing;
			}
			set
			{
				if (_supports_Homing != value)
				{
					_supports_Homing = value;
					UpdateDeviceCapabilities();
				}
			}
		}

		public byte PhysicalSwitchType
		{
			get
			{
				return _physicalSwitchType;
			}
			set
			{
				if (_physicalSwitchType != value)
				{
					_physicalSwitchType = value;
					UpdateDeviceCapabilities();
				}
			}
		}

		public bool OutputDisabledLatch
		{
			get
			{
				return _outputDisabledLatch;
			}
			set
			{
				if (_outputDisabledLatch != value)
				{
					_outputDisabledLatch = value;
				}
			}
		}

		[field: CompilerGenerated]
		public uint SOFTWARE_FUSE_RATING_AMPS
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public uint SOFTWARE_FUSE_MAX_RATING_AMPS
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public byte EXTENDEDDEVICECAPABILITIES
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public byte CLOUDCAPABILITIES
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public bool NotAcceptingCommands
		{
			get
			{
				return base.IsNotAcceptingCommands;
			}
			set
			{
				base.IsNotAcceptingCommands = value;
			}
		}

		public RELAY_TYPE_2_SIM_INTERFACE(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, DEVICE_ID deviceId, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), deviceId.DeviceType, 0, deviceId.FunctionName, deviceId.FunctionInstance, deviceId.DeviceCapabilities, options)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			mStatusMessageParams = new RELAY_TYPE_2_STATUS_PARAMS();
			base.DeviceStatus = mStatusMessageParams.GetPayload();
		}

		protected virtual void Init()
		{
		}

		protected virtual void UpdateDeviceCapabilities()
		{
		}

		protected virtual void UpdateStatus()
		{
		}
	}
	public enum SETEC_POWER_MANAGER_OPERATING_MODE : byte
	{
		OFF,
		ON
	}
	public class SETEC_POWER_MANAGER : LocalDevice
	{
		public enum COMMAND_OPERATING_MODE : byte
		{
			OFF,
			ON
		}

		private struct COMMAND
		{
			private int Value;

			public COMMAND_OPERATING_MODE Option
			{
				get
				{
					return (COMMAND_OPERATING_MODE)GetBits(3, 6);
				}
				set
				{
					SetBits((int)value, 3, 6);
				}
			}

			private bool GetBit(byte bit)
			{
				return (Value & bit) != 0;
			}

			private void SetBit(bool val, byte bit)
			{
				if (val)
				{
					Value |= bit;
				}
				else
				{
					Value &= ~bit;
				}
			}

			private int GetBits(int bit, int shift)
			{
				return (Value >> shift) & bit;
			}

			private void SetBits(int val, int bit, int shift)
			{
				val <<= shift;
				bit <<= shift;
				Value = (Value & ~bit) | (val & bit);
			}

			public static implicit operator int(COMMAND s)
			{
				return s.Value;
			}

			public static implicit operator byte(COMMAND s)
			{
				return (byte)s.Value;
			}

			public static implicit operator COMMAND(byte b)
			{
				COMMAND result = default(COMMAND);
				result.Value = b;
				return result;
			}

			public static implicit operator COMMAND(int i)
			{
				COMMAND result = default(COMMAND);
				result.Value = i;
				return result;
			}
		}

		private SETEC_POWER_MANAGER_OPERATING_MODE _operatingMode;

		private COMMAND _command = 0;

		public SETEC_POWER_MANAGER_OPERATING_MODE OperatingMode
		{
			get
			{
				return _operatingMode;
			}
			set
			{
				if (_operatingMode != value)
				{
					_operatingMode = value;
					UpdateStatus();
				}
			}
		}

		public bool NotAcceptingCommands
		{
			get
			{
				return base.IsNotAcceptingCommands;
			}
			set
			{
				base.IsNotAcceptingCommands = value;
			}
		}

		private COMMAND Command
		{
			get
			{
				return _command;
			}
			set
			{
				if ((int)_command != (int)value)
				{
					_command = value;
					switch ((uint)(byte)_command)
					{
					case 0u:
						OperatingMode = SETEC_POWER_MANAGER_OPERATING_MODE.OFF;
						break;
					case 1u:
						OperatingMode = SETEC_POWER_MANAGER_OPERATING_MODE.ON;
						break;
					}
					UpdateStatus();
				}
			}
		}

		public SETEC_POWER_MANAGER(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), DEVICE_TYPE.op_Implicit((byte)28), 0, FUNCTION_NAME.op_Implicit((ushort)262), 0, (byte)0, options)
		{
			Init();
		}

		private void Init()
		{
			UpdateStatus();
		}

		private void UpdateStatus()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			base.DeviceStatus = PAYLOAD.FromArgs(new object[1] { (byte)OperatingMode });
		}

		protected override void OnLocalDeviceRxEvent(AdapterRxEvent rx)
		{
			base.OnLocalDeviceRxEvent(rx);
			if ((byte)rx.MessageType == 130 && rx.TargetAddress == base.Address && rx.SourceAddress == GetLocalSessionClientAddress(SESSION_ID.op_Implicit((ushort)4)) && rx.Count == 1)
			{
				Command = rx[0];
			}
		}
	}
	public class TANK_SENSOR_STATUS_PARAMS : IDeviceStatusParams
	{
		private PAYLOAD payload;

		[DeviceDisplay("Fill Level")]
		public string FillLevelDisplay
		{
			get
			{
				if (FillLevel <= 100)
				{
					return $"{FillLevel}%";
				}
				return "INVALID";
			}
		}

		public byte FillLevel
		{
			get
			{
				return ((PAYLOAD)(ref payload))[0];
			}
			set
			{
				((PAYLOAD)(ref payload))[0] = value;
			}
		}

		[DeviceDisplay("Battery Level")]
		public string BatteryLevelDisplay
		{
			get
			{
				if (BatteryLevel <= 100)
				{
					return $"{BatteryLevel}%";
				}
				return "Unknown/Not Supported";
			}
		}

		public byte BatteryLevel
		{
			get
			{
				return ((PAYLOAD)(ref payload))[1];
			}
			set
			{
				((PAYLOAD)(ref payload))[1] = value;
			}
		}

		[DeviceDisplay("Measurement Quality")]
		public string MeasurementQualityDisplay
		{
			get
			{
				if (MeasurementQuality == 255)
				{
					return "Not Supported";
				}
				if (MeasurementQuality > 100)
				{
					return "Unknown/Invalid";
				}
				return $"{MeasurementQuality}%";
			}
		}

		public byte MeasurementQuality
		{
			get
			{
				return ((PAYLOAD)(ref payload))[2];
			}
			set
			{
				((PAYLOAD)(ref payload))[2] = value;
			}
		}

		[DeviceDisplay("X Acceleration (G)")]
		public string XAccelerationDisplay
		{
			get
			{
				sbyte b = (sbyte)((PAYLOAD)(ref payload))[3];
				if (b == -128)
				{
					return "Unknown/Invalid";
				}
				return $"{(double)b / 1024.0:0.000} G";
			}
		}

		public sbyte XAcceleration
		{
			get
			{
				return (sbyte)((PAYLOAD)(ref payload))[3];
			}
			set
			{
				((PAYLOAD)(ref payload))[3] = (byte)value;
			}
		}

		[DeviceDisplay("Y Acceleration (G)")]
		public string YAccelerationDisplay
		{
			get
			{
				sbyte b = (sbyte)((PAYLOAD)(ref payload))[4];
				if (b == -128)
				{
					return "Unknown/Invalid";
				}
				return $"{(double)b / 1024.0:0.000} G";
			}
		}

		public sbyte YAcceleration
		{
			get
			{
				return (sbyte)((PAYLOAD)(ref payload))[4];
			}
			set
			{
				((PAYLOAD)(ref payload))[4] = (byte)value;
			}
		}

		[DeviceDisplay("Tank Level Alert")]
		public string TankLevelAlertDisplay
		{
			get
			{
				bool flag = (TankLevelAlert & 0x80) != 0;
				int num = TankLevelAlert & 0x7F;
				return $"Active = {flag}, Count = {num}";
			}
		}

		public byte TankLevelAlert
		{
			get
			{
				return ((PAYLOAD)(ref payload))[5];
			}
			set
			{
				((PAYLOAD)(ref payload))[5] = value;
			}
		}

		[DeviceDisplay("User Message")]
		public string UserMessageDisplay => ((object)(DTC_ID)UserMessage/*cast due to .constrained prefix*/).ToString();

		public ushort UserMessage
		{
			get
			{
				return (ushort)((((PAYLOAD)(ref payload))[6] << 8) | ((PAYLOAD)(ref payload))[7]);
			}
			set
			{
				((PAYLOAD)(ref payload))[6] = (byte)(value >> 8);
				((PAYLOAD)(ref payload))[7] = (byte)(value & 0xFF);
			}
		}

		public TANK_SENSOR_STATUS_PARAMS()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			payload = new PAYLOAD(8);
			FillLevel = 0;
			BatteryLevel = 101;
			MeasurementQuality = 255;
			XAcceleration = -128;
			YAcceleration = -128;
			TankLevelAlert = 0;
			UserMessage = 0;
		}

		public void SetPayload(PAYLOAD pl)
		{
			if (((PAYLOAD)(ref pl)).Length >= 1)
			{
				FillLevel = ((PAYLOAD)(ref pl))[0];
			}
			if (((PAYLOAD)(ref pl)).Length >= 2)
			{
				BatteryLevel = ((PAYLOAD)(ref pl))[1];
			}
			if (((PAYLOAD)(ref pl)).Length >= 3)
			{
				MeasurementQuality = ((PAYLOAD)(ref pl))[2];
			}
			if (((PAYLOAD)(ref pl)).Length >= 4)
			{
				XAcceleration = (sbyte)((PAYLOAD)(ref pl))[3];
			}
			if (((PAYLOAD)(ref pl)).Length >= 5)
			{
				YAcceleration = (sbyte)((PAYLOAD)(ref pl))[4];
			}
			if (((PAYLOAD)(ref pl)).Length >= 6)
			{
				TankLevelAlert = ((PAYLOAD)(ref pl))[5];
			}
			if (((PAYLOAD)(ref pl)).Length >= 8)
			{
				UserMessage = (ushort)((((PAYLOAD)(ref pl))[6] << 8) | ((PAYLOAD)(ref pl))[7]);
			}
		}

		public PAYLOAD GetPayload()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return payload;
		}
	}
	public class TANK_SENSOR : LocalDevice
	{
		private TANK_SENSOR_STATUS_PARAMS mStatus;

		private bool mIsFuelSensor;

		private bool mIsHighPrecision;

		private bool mSupportsTankAlerts;

		private bool mSupportsBattery;

		private bool mSupportsTankCapacity;

		private bool mIsMopekaType;

		public bool IsFuelSensor
		{
			get
			{
				return mIsFuelSensor;
			}
			set
			{
				mIsFuelSensor = value;
				UpdateDeviceCapabilities();
			}
		}

		public bool IsHighPrecision
		{
			get
			{
				return mIsHighPrecision;
			}
			set
			{
				mIsHighPrecision = value;
				UpdateDeviceCapabilities();
			}
		}

		public bool SupportsTankAlerts
		{
			get
			{
				return mSupportsTankAlerts;
			}
			set
			{
				mSupportsTankAlerts = value;
				UpdateDeviceCapabilities();
			}
		}

		public bool SupportsBattery
		{
			get
			{
				return mSupportsBattery;
			}
			set
			{
				mSupportsBattery = value;
				UpdateDeviceCapabilities();
			}
		}

		public bool SupportsTankCapacity
		{
			get
			{
				return mSupportsTankCapacity;
			}
			set
			{
				mSupportsTankCapacity = value;
				UpdateDeviceCapabilities();
			}
		}

		public bool IsMopekaType
		{
			get
			{
				return mIsMopekaType;
			}
			set
			{
				mIsMopekaType = value;
				UpdateDeviceCapabilities();
			}
		}

		public byte FillLevel
		{
			get
			{
				return mStatus.FillLevel;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.FillLevel = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public byte BatteryLevel
		{
			get
			{
				return mStatus.BatteryLevel;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.BatteryLevel = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public byte MeasurementQuality
		{
			get
			{
				return mStatus.MeasurementQuality;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.MeasurementQuality = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public sbyte XAcceleration
		{
			get
			{
				return mStatus.XAcceleration;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.XAcceleration = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public sbyte YAcceleration
		{
			get
			{
				return mStatus.YAcceleration;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.YAcceleration = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public byte TankLevelAlert
		{
			get
			{
				return mStatus.TankLevelAlert;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.TankLevelAlert = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public ushort UserMessage
		{
			get
			{
				return mStatus.UserMessage;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatus.UserMessage = value;
				base.DeviceStatus = mStatus.GetPayload();
			}
		}

		public TANK_SENSOR(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), DEVICE_TYPE.op_Implicit((byte)10), 0, FUNCTION_NAME.op_Implicit((ushort)67), 0, (byte)0, options)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			mStatus = new TANK_SENSOR_STATUS_PARAMS();
			base.DeviceStatus = mStatus.GetPayload();
			UpdateDeviceCapabilities();
		}

		protected void UpdateDeviceCapabilities()
		{
			byte b = 0;
			b |= Convert.ToByte(mIsFuelSensor);
			b |= (byte)(Convert.ToByte(mIsHighPrecision) << 1);
			b |= (byte)(Convert.ToByte(mSupportsTankAlerts) << 2);
			b |= (byte)(Convert.ToByte(mSupportsBattery) << 3);
			b |= (byte)(Convert.ToByte(mSupportsTankCapacity) << 4);
			b |= (byte)(Convert.ToByte(mIsMopekaType) << 5);
			base.DeviceCapabilities = b;
		}
	}
	public class TEMPERATURE_SENSOR_STATUS_PARAMS : IDeviceStatusParams
	{
		private PAYLOAD payload;

		private short mTemperatureC;

		private byte mBatteryVoltage;

		private byte mBatteryLevel;

		private byte mLowBattAlert;

		private byte mHighTempAlert;

		private byte mLowTempAlert;

		private byte mTempInRangeAlert;

		[DeviceDisplay("Temperature (C)")]
		public string TemperatureCDisplay
		{
			get
			{
				if (TemperatureC < -32768 || TemperatureC > 32512)
				{
					return "INVALID";
				}
				double num = (double)mTemperatureC / 256.0;
				return $"{num:0.00}°C";
			}
		}

		public short TemperatureC
		{
			get
			{
				return mTemperatureC;
			}
			set
			{
				mTemperatureC = value;
				((PAYLOAD)(ref payload))[0] = (byte)(mTemperatureC >> 8);
				((PAYLOAD)(ref payload))[1] = (byte)mTemperatureC;
			}
		}

		public byte BatteryVoltage
		{
			get
			{
				return mBatteryVoltage;
			}
			set
			{
				byte b = (((PAYLOAD)(ref payload))[2] = value);
				mBatteryVoltage = b;
			}
		}

		public byte BatteryLevel
		{
			get
			{
				return mBatteryLevel;
			}
			set
			{
				byte b = (((PAYLOAD)(ref payload))[3] = value);
				mBatteryLevel = b;
			}
		}

		public byte LowBattAlert
		{
			get
			{
				return mLowBattAlert;
			}
			set
			{
				byte b = (((PAYLOAD)(ref payload))[4] = value);
				mLowBattAlert = b;
			}
		}

		public byte HighTempAlert
		{
			get
			{
				return mHighTempAlert;
			}
			set
			{
				byte b = (((PAYLOAD)(ref payload))[5] = value);
				mHighTempAlert = b;
			}
		}

		public byte LowTempAlert
		{
			get
			{
				return mLowTempAlert;
			}
			set
			{
				byte b = (((PAYLOAD)(ref payload))[6] = value);
				mLowTempAlert = b;
			}
		}

		public byte TempInRangeAlert
		{
			get
			{
				return mTempInRangeAlert;
			}
			set
			{
				byte b = (((PAYLOAD)(ref payload))[7] = value);
				mTempInRangeAlert = b;
			}
		}

		public PAYLOAD GetPayload()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return payload;
		}

		public TEMPERATURE_SENSOR_STATUS_PARAMS()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			payload = new PAYLOAD(8);
			TemperatureC = 0;
			BatteryVoltage = 0;
			BatteryLevel = 0;
			LowBattAlert = 0;
			HighTempAlert = 0;
			LowTempAlert = 0;
			TempInRangeAlert = 0;
		}

		public void SetPayload(PAYLOAD pl)
		{
			TemperatureC = (short)((short)(((PAYLOAD)(ref pl))[0] << 8) + ((PAYLOAD)(ref pl))[1]);
			BatteryVoltage = ((PAYLOAD)(ref pl))[2];
			BatteryLevel = ((PAYLOAD)(ref pl))[3];
			LowBattAlert = ((PAYLOAD)(ref pl))[4];
			HighTempAlert = ((PAYLOAD)(ref pl))[5];
			LowTempAlert = ((PAYLOAD)(ref pl))[6];
			TempInRangeAlert = ((PAYLOAD)(ref pl))[7];
		}

		public System.Collections.Generic.IEnumerable<MemberInfo> GetMembers()
		{
			return Enumerable.Where<MemberInfo>((System.Collections.Generic.IEnumerable<MemberInfo>)base.GetType().GetMembers((BindingFlags)20), (Func<MemberInfo, bool>)((MemberInfo m) => (int)m.MemberType == 16 || (int)m.MemberType == 4));
		}
	}
	public class TEMPERATURE_SENSOR_SIMULATOR_INTERFACE : LocalDevice
	{
		public TEMPERATURE_SENSOR_STATUS_PARAMS mStatusMessageParams;

		public short TemperatureC
		{
			get
			{
				return mStatusMessageParams.TemperatureC;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatusMessageParams.TemperatureC = value;
				base.DeviceStatus = mStatusMessageParams.GetPayload();
			}
		}

		public byte BatteryVoltage
		{
			get
			{
				return mStatusMessageParams.BatteryVoltage;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatusMessageParams.BatteryVoltage = value;
				base.DeviceStatus = mStatusMessageParams.GetPayload();
			}
		}

		public byte BatteryLevel
		{
			get
			{
				return mStatusMessageParams.BatteryLevel;
			}
			set
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				mStatusMessageParams.BatteryLevel = value;
				base.DeviceStatus = mStatusMessageParams.GetPayload();
			}
		}

		public TEMPERATURE_SENSOR_SIMULATOR_INTERFACE(IAdapter adapter, string software_part_number, PRODUCT_ID product_id, IDS_CAN_VERSION_NUMBER version, LOCAL_DEVICE_OPTIONS options, MAC mac = null)
			: base(new LocalProduct(adapter, mac, product_id, version, software_part_number), DEVICE_TYPE.op_Implicit((byte)25), 0, FUNCTION_NAME.op_Implicit((ushort)166), 0, (byte)0, options)
		{
			mStatusMessageParams = new TEMPERATURE_SENSOR_STATUS_PARAMS();
			mStatusMessageParams.TemperatureC = 6144;
		}
	}
}
