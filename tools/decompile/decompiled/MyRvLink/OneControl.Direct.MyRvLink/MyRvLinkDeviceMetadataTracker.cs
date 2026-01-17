using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using IDS.Portable.Common;
using IDS.Portable.LogicalDevice;
using OneControl.Direct.MyRvLink.Devices;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkDeviceMetadataTracker : CommonDisposable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass20_0
	{
		[StructLayout((LayoutKind)3)]
		private struct _003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public _003C_003Ec__DisplayClass20_0 _003C_003E4__this;

			private ConfiguredTaskAwaiter<List<IMyRvLinkDeviceMetadata>> _003C_003Eu__1;

			private void MoveNext()
			{
				//IL_0288: Unknown result type (might be due to invalid IL or missing references)
				//IL_028d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0295: Unknown result type (might be due to invalid IL or missing references)
				//IL_023f: Unknown result type (might be due to invalid IL or missing references)
				//IL_024a: Unknown result type (might be due to invalid IL or missing references)
				//IL_024f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0253: Unknown result type (might be due to invalid IL or missing references)
				//IL_0258: Unknown result type (might be due to invalid IL or missing references)
				//IL_026d: Unknown result type (might be due to invalid IL or missing references)
				//IL_026f: Unknown result type (might be due to invalid IL or missing references)
				int num = _003C_003E1__state;
				_003C_003Ec__DisplayClass20_0 _003C_003Ec__DisplayClass20_ = _003C_003E4__this;
				try
				{
					if (num != 0)
					{
						_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataList = new List<IMyRvLinkDeviceMetadata>();
						try
						{
							if (MyRvLinkDeviceMetadataTableSerializable.TryLoad(((ILogicalDeviceSource)_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceTracker.MyRvLinkService).DeviceSourceToken, _003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataTableCrc, out MyRvLinkDeviceMetadataTableSerializable deviceMetadataTableSerializable) && deviceMetadataTableSerializable != null)
							{
								_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataList = Enumerable.ToList<IMyRvLinkDeviceMetadata>((global::System.Collections.Generic.IEnumerable<IMyRvLinkDeviceMetadata>)deviceMetadataTableSerializable.TryDecode());
								if (_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceTracker.DeviceList.Count != _003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataList.Count)
								{
									_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataList.Clear();
									TaggedLog.Warning(_003C_003Ec__DisplayClass20_._003C_003E4__this.LogTag, $"{_003C_003Ec__DisplayClass20_._003C_003E4__this.LogPrefix} Cached metadata contained {_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataList.Count} but we were expecting {_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceTracker.DeviceList.Count} devices", global::System.Array.Empty<object>());
								}
								else
								{
									TaggedLog.Information(_003C_003Ec__DisplayClass20_._003C_003E4__this.LogTag, $"{_003C_003Ec__DisplayClass20_._003C_003E4__this.LogPrefix} Getting Cached Devices for 0x{_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataTableCrc:x8} has {_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataList.Count} devices", global::System.Array.Empty<object>());
									_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceTracker.UpdateMetadata(_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataList, _003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataTableCrc);
								}
							}
						}
						catch (global::System.Exception ex)
						{
							TaggedLog.Error(_003C_003Ec__DisplayClass20_._003C_003E4__this.LogTag, _003C_003Ec__DisplayClass20_._003C_003E4__this.LogPrefix + " Unable to load device Metadata " + ex.Message, global::System.Array.Empty<object>());
						}
						if (_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataList.Count != 0)
						{
							goto IL_0388;
						}
					}
					try
					{
						ConfiguredTaskAwaiter<List<IMyRvLinkDeviceMetadata>> val;
						if (num != 0)
						{
							val = _003C_003Ec__DisplayClass20_._003C_003E4__this.GetDevicesMetadataAsync(_003C_003Ec__DisplayClass20_.commandCancellationToken).ConfigureAwait(false).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<List<IMyRvLinkDeviceMetadata>>, _003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = _003C_003Eu__1;
							_003C_003Eu__1 = default(ConfiguredTaskAwaiter<List<IMyRvLinkDeviceMetadata>>);
							num = (_003C_003E1__state = -1);
						}
						List<IMyRvLinkDeviceMetadata> result = val.GetResult();
						_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataList = result;
						_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceTracker.UpdateMetadata(_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataList, _003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataTableCrc);
						if (_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceTracker.IsDeviceLoadComplete)
						{
							new MyRvLinkDeviceMetadataTableSerializable(_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataTableCrc, (global::System.Collections.Generic.IReadOnlyList<MyRvLinkDeviceMetadataSerializable>)Enumerable.ToList<MyRvLinkDeviceMetadataSerializable>(Enumerable.Select<IMyRvLinkDeviceMetadata, MyRvLinkDeviceMetadataSerializable>((global::System.Collections.Generic.IEnumerable<IMyRvLinkDeviceMetadata>)_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceMetadataList, (Func<IMyRvLinkDeviceMetadata, MyRvLinkDeviceMetadataSerializable>)((IMyRvLinkDeviceMetadata device) => new MyRvLinkDeviceMetadataSerializable(device))))).TrySave(((ILogicalDeviceSource)_003C_003Ec__DisplayClass20_._003C_003E4__this.DeviceTracker.MyRvLinkService).DeviceSourceToken);
						}
					}
					catch (global::System.Exception ex2)
					{
						TaggedLog.Debug(_003C_003Ec__DisplayClass20_._003C_003E4__this.LogTag, _003C_003Ec__DisplayClass20_._003C_003E4__this.LogPrefix + " Get Devices Metadata failed: " + ex2.Message, global::System.Array.Empty<object>());
					}
					goto IL_0388;
					IL_0388:
					CancellationTokenSource? commandGetDevicesMetadataTcs = _003C_003Ec__DisplayClass20_._003C_003E4__this._commandGetDevicesMetadataTcs;
					if (commandGetDevicesMetadataTcs != null)
					{
						CancellationTokenSourceExtension.TryCancelAndDispose(commandGetDevicesMetadataTcs);
					}
					_003C_003Ec__DisplayClass20_._003C_003E4__this._commandGetDevicesMetadataTcs = null;
				}
				catch (global::System.Exception exception)
				{
					_003C_003E1__state = -2;
					((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetStateMachine(stateMachine);
			}
		}

		public MyRvLinkDeviceMetadataTracker _003C_003E4__this;

		public CancellationToken commandCancellationToken;

		[AsyncStateMachine(typeof(_003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed))]
		internal global::System.Threading.Tasks.Task? _003CGetDevicesMetadataIfNeeded_003Eb__0()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			_003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed _003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed = default(_003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed);
			_003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
			_003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed._003C_003E4__this = this;
			_003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed._003C_003E1__state = -1;
			((AsyncTaskMethodBuilder)(ref _003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed._003C_003Et__builder)).Start<_003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed>(ref _003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed);
			return ((AsyncTaskMethodBuilder)(ref _003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed._003C_003Et__builder)).Task;
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDevicesMetadataAsync_003Ed__21 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<List<IMyRvLinkDeviceMetadata>> _003C_003Et__builder;

		public MyRvLinkDeviceMetadataTracker _003C_003E4__this;

		public CancellationToken cancellationToken;

		private MyRvLinkCommandGetDevicesMetadata _003CcommandGetDevicesMetadata_003E5__2;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			MyRvLinkDeviceMetadataTracker myRvLinkDeviceMetadataTracker = _003C_003E4__this;
			List<IMyRvLinkDeviceMetadata> result2;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					_003CcommandGetDevicesMetadata_003E5__2 = new MyRvLinkCommandGetDevicesMetadata(myRvLinkDeviceMetadataTracker.MyRvLinkService.GetNextCommandId(), myRvLinkDeviceMetadataTracker.DeviceTracker.DeviceTableId, 0, 255);
					val = myRvLinkDeviceMetadataTracker.MyRvLinkService.SendCommandAsync(_003CcommandGetDevicesMetadata_003E5__2, cancellationToken, MyRvLinkSendCommandOption.None).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CGetDevicesMetadataAsync_003Ed__21>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
					num = (_003C_003E1__state = -1);
				}
				IMyRvLinkCommandResponse result = val.GetResult();
				if (result is MyRvLinkCommandResponseFailure)
				{
					throw new MyRvLinkException($"Failed {result}");
				}
				if (_003CcommandGetDevicesMetadata_003E5__2.ResponseState == MyRvLinkResponseState.Failed)
				{
					throw new MyRvLinkException($"Get Device Metadata Command Failed {_003CcommandGetDevicesMetadata_003E5__2.ResponseState}");
				}
				if (_003CcommandGetDevicesMetadata_003E5__2.ResponseReceivedMetadataTableCrc != myRvLinkDeviceMetadataTracker.DeviceMetadataTableCrc)
				{
					throw new MyRvLinkException("Response didn't match expected Device Metadata Table CRC, discarding response");
				}
				List<IMyRvLinkDeviceMetadata> devicesMetadata = _003CcommandGetDevicesMetadata_003E5__2.DevicesMetadata;
				if (devicesMetadata == null || devicesMetadata.Count == 0)
				{
					throw new MyRvLinkException("No Devices Found");
				}
				if (devicesMetadata.Count != myRvLinkDeviceMetadataTracker.DeviceTracker.DeviceList.Count)
				{
					throw new MyRvLinkException("Count of devices don't match between metadata and device list!");
				}
				result2 = devicesMetadata;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003CcommandGetDevicesMetadata_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003CcommandGetDevicesMetadata_003E5__2 = null;
			_003C_003Et__builder.SetResult(result2);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	private string LogPrefix;

	private CancellationTokenSource? _commandGetDevicesMetadataTcs;

	[field: CompilerGenerated]
	protected virtual string LogTag
	{
		[CompilerGenerated]
		get;
	} = "MyRvLinkDeviceMetadataTracker";

	public IDirectConnectionMyRvLink MyRvLinkService => DeviceTracker.MyRvLinkService;

	[field: CompilerGenerated]
	public MyRvLinkDeviceTracker DeviceTracker
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public uint DeviceMetadataTableCrc
	{
		[CompilerGenerated]
		get;
	}

	[field: CompilerGenerated]
	public List<IMyRvLinkDeviceMetadata> DeviceMetadataList
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	} = new List<IMyRvLinkDeviceMetadata>();

	public bool IsActive
	{
		get
		{
			if (!((CommonDisposable)this).IsDisposed && DeviceMetadataTableCrc == MyRvLinkService.GatewayInfo?.DeviceMetadataTableCrc)
			{
				return DeviceTracker.IsActive;
			}
			return false;
		}
	}

	public MyRvLinkDeviceMetadataTracker(MyRvLinkDeviceTracker deviceTracker, uint deviceMetadataTableCrc)
	{
		LogPrefix = deviceTracker.MyRvLinkService.LogPrefix;
		DeviceTracker = deviceTracker;
		DeviceMetadataTableCrc = deviceMetadataTableCrc;
	}

	public void GetDevicesMetadataIfNeeded()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		_003C_003Ec__DisplayClass20_0 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass20_0();
		CS_0024_003C_003E8__locals4._003C_003E4__this = this;
		if (!IsActive)
		{
			CancellationTokenSource? commandGetDevicesMetadataTcs = _commandGetDevicesMetadataTcs;
			if (commandGetDevicesMetadataTcs != null)
			{
				CancellationTokenSourceExtension.TryCancelAndDispose(commandGetDevicesMetadataTcs);
			}
		}
		else if (DeviceTracker.DeviceList.Count > 0 && DeviceMetadataList.Count <= 0 && _commandGetDevicesMetadataTcs == null)
		{
			CancellationTokenSource val = new CancellationTokenSource();
			CancellationTokenSource obj = Interlocked.Exchange<CancellationTokenSource>(ref _commandGetDevicesMetadataTcs, val);
			if (obj != null)
			{
				CancellationTokenSourceExtension.TryCancelAndDispose(obj);
			}
			CS_0024_003C_003E8__locals4.commandCancellationToken = val.Token;
			global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(_003C_003Ec__DisplayClass20_0._003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed))] () =>
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				_003C_003Ec__DisplayClass20_0._003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed _003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed = default(_003C_003Ec__DisplayClass20_0._003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed);
				_003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
				_003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed._003C_003E4__this = CS_0024_003C_003E8__locals4;
				_003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed._003C_003E1__state = -1;
				((AsyncTaskMethodBuilder)(ref _003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed._003C_003Et__builder)).Start<_003C_003Ec__DisplayClass20_0._003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed>(ref _003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed);
				return ((AsyncTaskMethodBuilder)(ref _003C_003CGetDevicesMetadataIfNeeded_003Eb__0_003Ed._003C_003Et__builder)).Task;
			}), CS_0024_003C_003E8__locals4.commandCancellationToken);
		}
	}

	[AsyncStateMachine(typeof(_003CGetDevicesMetadataAsync_003Ed__21))]
	private async global::System.Threading.Tasks.Task<List<IMyRvLinkDeviceMetadata>> GetDevicesMetadataAsync(CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		MyRvLinkCommandGetDevicesMetadata commandGetDevicesMetadata = new MyRvLinkCommandGetDevicesMetadata(MyRvLinkService.GetNextCommandId(), DeviceTracker.DeviceTableId, 0, 255);
		IMyRvLinkCommandResponse myRvLinkCommandResponse = await MyRvLinkService.SendCommandAsync(commandGetDevicesMetadata, cancellationToken, MyRvLinkSendCommandOption.None);
		if (myRvLinkCommandResponse is MyRvLinkCommandResponseFailure)
		{
			throw new MyRvLinkException($"Failed {myRvLinkCommandResponse}");
		}
		if (commandGetDevicesMetadata.ResponseState == MyRvLinkResponseState.Failed)
		{
			throw new MyRvLinkException($"Get Device Metadata Command Failed {commandGetDevicesMetadata.ResponseState}");
		}
		if (commandGetDevicesMetadata.ResponseReceivedMetadataTableCrc != DeviceMetadataTableCrc)
		{
			throw new MyRvLinkException("Response didn't match expected Device Metadata Table CRC, discarding response");
		}
		List<IMyRvLinkDeviceMetadata> devicesMetadata = commandGetDevicesMetadata.DevicesMetadata;
		if (devicesMetadata == null || devicesMetadata.Count == 0)
		{
			throw new MyRvLinkException("No Devices Found");
		}
		if (devicesMetadata.Count != DeviceTracker.DeviceList.Count)
		{
			throw new MyRvLinkException("Count of devices don't match between metadata and device list!");
		}
		return devicesMetadata;
	}

	public override void Dispose(bool disposing)
	{
		CancellationTokenSource? commandGetDevicesMetadataTcs = _commandGetDevicesMetadataTcs;
		if (commandGetDevicesMetadataTcs != null)
		{
			CancellationTokenSourceExtension.TryCancelAndDispose(commandGetDevicesMetadataTcs);
		}
	}
}
