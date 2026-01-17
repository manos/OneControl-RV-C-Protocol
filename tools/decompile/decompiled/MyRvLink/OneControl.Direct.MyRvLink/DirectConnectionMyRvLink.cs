using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IDS.Core;
using IDS.Core.Collections;
using IDS.Core.IDS_CAN;
using IDS.Core.Types;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using IDS.Portable.LogicalDevice.BlockTransfer;
using IDS.Portable.LogicalDevice.FirmwareUpdate;
using IDS.Portable.LogicalDevice.LogicalDevice;
using IDS.Portable.LogicalDevice.LogicalDeviceSource;
using OneControl.Devices;
using OneControl.Devices.AccessoryGateway;
using OneControl.Devices.BootLoader;
using OneControl.Devices.Leveler.Type5;
using OneControl.Devices.LightRgb;
using OneControl.Direct.MyRvLink.Cache;
using OneControl.Direct.MyRvLink.Events;
using ids.portable.common;
using ids.portable.common.Metrics;

namespace OneControl.Direct.MyRvLink;

public abstract class DirectConnectionMyRvLink : ILogicalDeviceSourceDirectBlockTransfer, ILogicalDeviceSourceDirect, ILogicalDeviceSource, IBlockTransfer, IDirectCommandLeveler1, IDirectCommandLeveler3, IDirectCommandLeveler4, IDirectConnectionMyRvLink, ILogicalDeviceSourceDirectConnectionMyRvLink, ILogicalDeviceSourceDirectConnection, ILogicalDeviceSourceConnection, ILogicalDeviceSourceDirectAccessoryGateway, ILogicalDeviceSourceDirectPid, IDirectCommandClimateZone, IDirectCommandGeneratorGenie, IDirectCommandLeveler5, IDirectCommandLightDimmable, IDirectCommandLightRgb, ILogicalDeviceSourceDirectPidList, ILogicalDeviceSourceDirectMetadata, ILogicalDeviceSourceDirectDtc, ILogicalDeviceSourceDirectFirmwareUpdateDevice, IFirmwareUpdateDevice, IDirectConnectionMyRvLinkMetrics, IDirectCommandMovement, ILogicalDeviceSourceDirectRealTimeClock, ILogicalDeviceSourceDirectRemoveOfflineDevices, ILogicalDeviceSourceDirectRename, IDirectManagerMyRvLinkRvStatus, ILogicalDeviceSourceDirectVoltage, ILogicalDeviceSourceDirectSoftwareUpdateAuthorization, IDirectCommandSwitch, ILogicalDeviceSourceDirectSwitchMasterControllable
{
	public class BlockWriteTimeTracker
	{
		public enum TrackId
		{
			None,
			ProgressAck,
			BufferCopy,
			UpdateAndSendCommand,
			WaitingForResponse,
			WaitingForResponsePollDelay,
			ProcessResponse,
			Finish
		}

		private readonly Dictionary<TrackId, Stopwatch> _timeTracking;

		private TrackId _currentlyTracking;

		private Stopwatch _totalTime;

		public BlockWriteTimeTracker()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			_totalTime = Stopwatch.StartNew();
			_timeTracking = new Dictionary<TrackId, Stopwatch>();
			_currentlyTracking = TrackId.None;
			global::System.Collections.Generic.IEnumerator<TrackId> enumerator = EnumExtensions.GetValues<TrackId>().GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					TrackId current = enumerator.Current;
					_timeTracking.Add(current, new Stopwatch());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
		}

		public void SwitchTrackingTo(TrackId track)
		{
			if (track != _currentlyTracking)
			{
				_timeTracking[_currentlyTracking].Stop();
				_currentlyTracking = track;
				_timeTracking[_currentlyTracking].Start();
			}
		}

		public void Stop()
		{
			_timeTracking[_currentlyTracking].Stop();
			_currentlyTracking = TrackId.None;
		}

		public virtual string ToString()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder val = new StringBuilder();
			StringBuilder val2 = val;
			StringBuilder obj = val2;
			AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(23, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    TotalTime: ");
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<float>((float)_totalTime.ElapsedMilliseconds / 1000f, "F2");
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" seconds");
			obj.AppendLine(ref val3);
			global::System.Collections.Generic.IEnumerator<TrackId> enumerator = EnumExtensions.GetValues<TrackId>().GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					TrackId current = enumerator.Current;
					val2 = val;
					StringBuilder obj2 = val2;
					val3 = new AppendInterpolatedStringHandler(16, 3, val2);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    ");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<TrackId>(current);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(": ");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<float>((float)_timeTracking[current].ElapsedMilliseconds / 1000f, "F2");
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(" seconds ");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<double>((double)((float)_timeTracking[current].ElapsedMilliseconds / (float)_totalTime.ElapsedMilliseconds) * 100.0, "F2");
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("%");
					obj2.AppendLine(ref val3);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
			return ((object)val).ToString();
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass164_0
	{
		public ILogicalDeviceFirmwareUpdateDevice logicalDeviceToReflash;

		internal bool _003CTryRemoveRefreshBootLoaderWhenOfflineAsync_003Eb__0(ILogicalDevice d)
		{
			return (object)d == logicalDeviceToReflash;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass16_0
	{
		public ConcurrentQueue<IMyRvLinkCommandResponse> queue;

		public Action<IMyRvLinkCommandResponse> _003C_003E9__0;

		internal void _003CDeviceBlockWriteAsync_003Eb__0(IMyRvLinkCommandResponse response)
		{
			queue.Enqueue(response);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass176_0
	{
		public MyRvLinkCommandContext<HBridgeCommand> commandContext;

		internal void _003CSendDirectCommandRelayMomentaryImpl_003Eb__0(IMyRvLinkCommandResponse response)
		{
			commandContext.ProcessResponse(response);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass183_0
	{
		public DirectConnectionMyRvLink _003C_003E4__this;

		public FUNCTION_NAME toName;

		public byte toFunctionInstance;

		internal IMyRvLinkCommand _003CRenameLogicalDevice_003Eb__0(ValueTuple<byte, byte> myRvLinkDevice)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			return new MyRvLinkCommandRenameDevice(_003C_003E4__this.GetNextCommandId(), myRvLinkDevice.Item1, myRvLinkDevice.Item2, toName, toFunctionInstance, SESSION_ID.op_Implicit((ushort)2));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass23_0
	{
		public MyRvLinkCommandContext<LogicalDeviceLevelerButtonType3> commandContext;

		internal void _003CSendDirectCommandLeveler3_003Eb__0(IMyRvLinkCommandResponse response)
		{
			commandContext.ProcessResponse(response);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass25_0
	{
		public MyRvLinkCommandContext<LevelerCommandCode> commandContext;

		internal void _003CSendDirectCommandLeveler4_003Eb__0(IMyRvLinkCommandResponse response)
		{
			commandContext.ProcessResponse(response);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CDeviceBlockWriteAsync_003Ed__16 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public CancellationToken cancellationToken;

		public Func<ILogicalDeviceTransferProgress, bool> progressAck;

		public global::System.Collections.Generic.IReadOnlyList<byte> data;

		public BlockTransferBlockId blockId;

		private _003C_003Ec__DisplayClass16_0 _003C_003E8__1;

		private ValueTuple<byte, byte>? _003CmyRvLinkDevice_003E5__2;

		private ushort _003CcommandId_003E5__3;

		private Stopwatch _003Ctimer_003E5__4;

		private int _003CtotalRetryAmount_003E5__5;

		private int _003CcurrentCommandRetryAmount_003E5__6;

		private byte[] _003CsendDataChunk_003E5__7;

		private bool _003CfirstCommand_003E5__8;

		private MyRvLinkCommandDeviceBlockWriteData _003CcommandData_003E5__9;

		private int _003CbytesSent_003E5__10;

		private uint _003CcalculatedCrc32_003E5__11;

		private BlockWriteTimeTracker _003CtimeTracker_003E5__12;

		private int _003Citerations_003E5__13;

		private global::System.Threading.Tasks.Task<IMyRvLinkCommandResponse> _003CfinishResultTask_003E5__14;

		private int _003CsendDataSize_003E5__15;

		private bool _003CwaitForNewMessage_003E5__16;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private TaskAwaiter<bool> _003C_003Eu__2;

		private TaskAwaiter _003C_003Eu__3;

		private void MoveNext()
		{
			//IL_0398: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_0769: Unknown result type (might be due to invalid IL or missing references)
			//IL_076e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0776: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b49: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b14: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b19: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ace: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bd5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0be0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c01: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b2e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c92: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c9d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ca8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cbe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a57: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ccf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_054f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_072a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0734: Unknown result type (might be due to invalid IL or missing references)
			//IL_0739: Unknown result type (might be due to invalid IL or missing references)
			//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_070e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0713: Unknown result type (might be due to invalid IL or missing references)
			//IL_071e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Unknown result type (might be due to invalid IL or missing references)
			//IL_074e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0750: Unknown result type (might be due to invalid IL or missing references)
			//IL_096c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0977: Unknown result type (might be due to invalid IL or missing references)
			//IL_0982: Unknown result type (might be due to invalid IL or missing references)
			//IL_098d: Unknown result type (might be due to invalid IL or missing references)
			//IL_099e: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_041e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			try
			{
				_ = 6;
				try
				{
					TaskAwaiter val3;
					TaskAwaiter<bool> val2;
					TaskAwaiter<IMyRvLinkCommandResponse> val;
					IMyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure;
					IMyRvLinkCommandResponse myRvLinkCommandResponse = default(IMyRvLinkCommandResponse);
					IMyRvLinkCommandResponse result;
					switch (num)
					{
					default:
						_003C_003E8__1 = new _003C_003Ec__DisplayClass16_0();
						directConnectionMyRvLink._firmwareUpdateInProgress = true;
						if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
						{
							throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
						}
						_003CmyRvLinkDevice_003E5__2 = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
						if (!_003CmyRvLinkDevice_003E5__2.HasValue)
						{
							throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
						}
						_003CcommandId_003E5__3 = directConnectionMyRvLink.GetNextCommandId();
						((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
						_003Ctimer_003E5__4 = Stopwatch.StartNew();
						_003CtotalRetryAmount_003E5__5 = 0;
						_003CcurrentCommandRetryAmount_003E5__6 = 0;
						_003CsendDataChunk_003E5__7 = new byte[128];
						_003CfirstCommand_003E5__8 = true;
						_003CcommandData_003E5__9 = null;
						_003C_003E8__1.queue = new ConcurrentQueue<IMyRvLinkCommandResponse>();
						_003CbytesSent_003E5__10 = 0;
						_003CcalculatedCrc32_003E5__11 = 4294967295u;
						_003CtimeTracker_003E5__12 = new BlockWriteTimeTracker();
						_003Citerations_003E5__13 = 0;
						goto IL_09c4;
					case 0:
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
						num = (_003C_003E1__state = -1);
						goto IL_03b4;
					case 1:
						val2 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_0455;
					case 2:
						val2 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_04e0;
					case 3:
						val3 = _003C_003Eu__3;
						_003C_003Eu__3 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_05a0;
					case 4:
						val2 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_0785;
					case 5:
						val2 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_0a8c;
					case 6:
						{
							val = _003C_003Eu__1;
							_003C_003Eu__1 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
							num = (_003C_003E1__state = -1);
							break;
						}
						IL_0785:
						val2.GetResult();
						_003CcurrentCommandRetryAmount_003E5__6++;
						_003CtotalRetryAmount_003E5__5++;
						goto IL_09b4;
						IL_04e0:
						val2.GetResult();
						goto IL_09c4;
						IL_0666:
						TaggedLog.Debug("DirectConnectionMyRvLink", $"DeviceBlockWrite received failure: {myRvLinkCommandResponseFailure}. CurrentRetryAmount: {_003CcurrentCommandRetryAmount_003E5__6} totalRetryAmount: {_003CtotalRetryAmount_003E5__5}", global::System.Array.Empty<object>());
						if (myRvLinkCommandResponseFailure.IsCommandCompleted)
						{
							throw new BlockTransferWriteFailedException(logicalDevice, blockId, (ILogicalDeviceTransferProgress)(object)new LogicalDeviceTransferProgress((UInt48)_003CbytesSent_003E5__10, (UInt48)_003CtotalRetryAmount_003E5__5, (UInt48)_003CcurrentCommandRetryAmount_003E5__6, TimeSpan.FromMilliseconds((double)_003Ctimer_003E5__4.ElapsedMilliseconds)), (global::System.Exception)null);
						}
						val2 = TaskExtension.TryDelay(200, cancellationToken).GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (_003C_003E1__state = 4);
							_003C_003Eu__2 = val2;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CDeviceBlockWriteAsync_003Ed__16>(ref val2, ref this);
							return;
						}
						goto IL_0785;
						IL_04ed:
						_003CwaitForNewMessage_003E5__16 = true;
						goto IL_09b4;
						IL_09c4:
						if (_003CbytesSent_003E5__10 < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)data).Count && !((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
						{
							_003Citerations_003E5__13++;
							if (_003Citerations_003E5__13 % 100 == 0)
							{
								TaggedLog.Information("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} {"DeviceBlockWriteAsync"} Stats{Environment.NewLine}{_003CtimeTracker_003E5__12}", global::System.Array.Empty<object>());
							}
							_003CtimeTracker_003E5__12.SwitchTrackingTo(BlockWriteTimeTracker.TrackId.ProgressAck);
							if (!progressAck.Invoke((ILogicalDeviceTransferProgress)(object)new LogicalDeviceTransferProgress((UInt48)_003CbytesSent_003E5__10, (UInt48)_003CtotalRetryAmount_003E5__5, (UInt48)_003CcurrentCommandRetryAmount_003E5__6, TimeSpan.FromMilliseconds((double)_003Ctimer_003E5__4.ElapsedMilliseconds))))
							{
								throw new OperationCanceledException();
							}
							_003CtimeTracker_003E5__12.SwitchTrackingTo(BlockWriteTimeTracker.TrackId.BufferCopy);
							_003CsendDataSize_003E5__15 = ((((global::System.Collections.Generic.IReadOnlyCollection<byte>)data).Count - _003CbytesSent_003E5__10 < 128) ? (((global::System.Collections.Generic.IReadOnlyCollection<byte>)data).Count - _003CbytesSent_003E5__10) : 128);
							ArrayExtension.Clear<byte>(_003CsendDataChunk_003E5__7);
							ReadOnlyList.ToExistingArray<byte>(data, _003CbytesSent_003E5__10, _003CsendDataChunk_003E5__7, 0, _003CsendDataSize_003E5__15);
							_003CtimeTracker_003E5__12.SwitchTrackingTo(BlockWriteTimeTracker.TrackId.UpdateAndSendCommand);
							if (_003CfirstCommand_003E5__8)
							{
								_003CcommandData_003E5__9 = new MyRvLinkCommandDeviceBlockWriteData(_003CcommandId_003E5__3, _003CmyRvLinkDevice_003E5__2.Value.Item1, _003CmyRvLinkDevice_003E5__2.Value.Item2, blockId, (uint)_003CbytesSent_003E5__10, (byte)_003CsendDataSize_003E5__15, _003CsendDataChunk_003E5__7);
								_003CfirstCommand_003E5__8 = false;
								val = directConnectionMyRvLink.SendCommandAsync(_003CcommandData_003E5__9, cancellationToken, MyRvLinkSendCommandOption.DontWaitForResponse, delegate(IMyRvLinkCommandResponse response)
								{
									_003C_003E8__1.queue.Enqueue(response);
								}).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (_003C_003E1__state = 0);
									_003C_003Eu__1 = val;
									((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CDeviceBlockWriteAsync_003Ed__16>(ref val, ref this);
									return;
								}
								goto IL_03b4;
							}
							_003CcommandData_003E5__9.UpdateCommand((uint)_003CbytesSent_003E5__10, (byte)_003CsendDataSize_003E5__15, _003CsendDataChunk_003E5__7);
							val2 = directConnectionMyRvLink.ResendRunningCommandAsync(_003CcommandId_003E5__3, cancellationToken).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (_003C_003E1__state = 1);
								_003C_003Eu__2 = val2;
								((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CDeviceBlockWriteAsync_003Ed__16>(ref val2, ref this);
								return;
							}
							goto IL_0455;
						}
						((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
						_003CtimeTracker_003E5__12.SwitchTrackingTo(BlockWriteTimeTracker.TrackId.Finish);
						_003CfinishResultTask_003E5__14 = directConnectionMyRvLink.WaitForRunningCommandToComplete(_003CcommandData_003E5__9.ClientCommandId, cancellationToken);
						_003CcommandData_003E5__9.UpdateCommand(4294967295u, 0);
						val2 = directConnectionMyRvLink.ResendRunningCommandAsync(_003CcommandId_003E5__3, cancellationToken).GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (_003C_003E1__state = 5);
							_003C_003Eu__2 = val2;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CDeviceBlockWriteAsync_003Ed__16>(ref val2, ref this);
							return;
						}
						goto IL_0a8c;
						IL_051c:
						if (_003CwaitForNewMessage_003E5__16 && !((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
						{
							_003CtimeTracker_003E5__12.SwitchTrackingTo(BlockWriteTimeTracker.TrackId.WaitingForResponsePollDelay);
							val3 = global::System.Threading.Tasks.Task.Delay(5, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val3)).IsCompleted)
							{
								num = (_003C_003E1__state = 3);
								_003C_003Eu__3 = val3;
								((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CDeviceBlockWriteAsync_003Ed__16>(ref val3, ref this);
								return;
							}
							goto IL_05a0;
						}
						goto IL_09c4;
						IL_05a0:
						((TaskAwaiter)(ref val3)).GetResult();
						goto IL_09b4;
						IL_09b4:
						while (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
						{
							_003CtimeTracker_003E5__12.SwitchTrackingTo(BlockWriteTimeTracker.TrackId.WaitingForResponse);
							if (!_003C_003E8__1.queue.TryDequeue(ref myRvLinkCommandResponse))
							{
								goto IL_051c;
							}
							((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
							_003CtimeTracker_003E5__12.SwitchTrackingTo(BlockWriteTimeTracker.TrackId.ProcessResponse);
							TaggedLog.Debug("DirectConnectionMyRvLink", "Processing Response", global::System.Array.Empty<object>());
							_003CwaitForNewMessage_003E5__16 = false;
							if (!(myRvLinkCommandResponse is MyRvLinkCommandResponseSuccessNoWait))
							{
								myRvLinkCommandResponseFailure = myRvLinkCommandResponse as IMyRvLinkCommandResponseFailure;
								if (myRvLinkCommandResponseFailure == null)
								{
									if (myRvLinkCommandResponse is MyRvLinkDeviceBlockWriteDataCommandResponse myRvLinkDeviceBlockWriteDataCommandResponse)
									{
										uint num2 = Crc32Le.Calculate(_003CcalculatedCrc32_003E5__11, (global::System.Collections.Generic.IReadOnlyList<byte>)_003CsendDataChunk_003E5__7, _003CsendDataSize_003E5__15, 0u);
										if (num2 != myRvLinkDeviceBlockWriteDataCommandResponse.Crc32)
										{
											TaggedLog.Debug("DirectConnectionMyRvLink", $"DeviceBlockWrite received success, but Crc was bad. Calculated Crc: {num2} Received Crc: {myRvLinkDeviceBlockWriteDataCommandResponse.Crc32} CurrentRetryAmount: {_003CcurrentCommandRetryAmount_003E5__6} totalRetryAmount: {_003CtotalRetryAmount_003E5__5}", global::System.Array.Empty<object>());
											_003CcurrentCommandRetryAmount_003E5__6++;
											_003CtotalRetryAmount_003E5__5++;
										}
										else
										{
											_003CcalculatedCrc32_003E5__11 = num2;
											TaggedLog.Debug("DirectConnectionMyRvLink", $"DeviceBlockWrite received success, Crc is GOOD. Calculated Crc: {_003CcalculatedCrc32_003E5__11} Received Crc: {myRvLinkDeviceBlockWriteDataCommandResponse.Crc32} CurrentRetryAmount: {_003CcurrentCommandRetryAmount_003E5__6} totalRetryAmount: {_003CtotalRetryAmount_003E5__5}", global::System.Array.Empty<object>());
											_003CcurrentCommandRetryAmount_003E5__6 = 0;
											_003CbytesSent_003E5__10 += 128;
										}
										continue;
									}
									TaggedLog.Debug("DirectConnectionMyRvLink", $"DeviceBlockWrite received unexpected response: {myRvLinkCommandResponse}", global::System.Array.Empty<object>());
									throw new BlockTransferWriteFailedException(logicalDevice, blockId, (ILogicalDeviceTransferProgress)(object)new LogicalDeviceTransferProgress((UInt48)_003CbytesSent_003E5__10, (UInt48)_003CtotalRetryAmount_003E5__5, (UInt48)_003CcurrentCommandRetryAmount_003E5__6, TimeSpan.FromMilliseconds((double)_003Ctimer_003E5__4.ElapsedMilliseconds)), (global::System.Exception)null);
								}
								goto IL_0666;
							}
							TaggedLog.Debug("DirectConnectionMyRvLink", $"Starting block transfer (1st block sent). CurrentRetryAmount: {_003CcurrentCommandRetryAmount_003E5__6} totalRetryAmount: {_003CtotalRetryAmount_003E5__5}", global::System.Array.Empty<object>());
							_003CwaitForNewMessage_003E5__16 = true;
						}
						goto IL_09c4;
						IL_03b4:
						result = val.GetResult();
						_003C_003E8__1.queue.Enqueue(result);
						goto IL_04ed;
						IL_0a8c:
						if (!val2.GetResult())
						{
							throw new BlockTransferWriteFailedException(logicalDevice, blockId, (ILogicalDeviceTransferProgress)(object)new LogicalDeviceTransferProgress((UInt48)_003CbytesSent_003E5__10, (UInt48)_003CtotalRetryAmount_003E5__5, (UInt48)_003CcurrentCommandRetryAmount_003E5__6, TimeSpan.FromMilliseconds((double)_003Ctimer_003E5__4.ElapsedMilliseconds)), (global::System.Exception)null);
						}
						((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
						TaggedLog.Debug("DirectConnectionMyRvLink", directConnectionMyRvLink.LogPrefix + " Waiting for Response to finish the write", global::System.Array.Empty<object>());
						val = _003CfinishResultTask_003E5__14.GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 6);
							_003C_003Eu__1 = val;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CDeviceBlockWriteAsync_003Ed__16>(ref val, ref this);
							return;
						}
						break;
						IL_0455:
						if (!val2.GetResult())
						{
							_003CcommandId_003E5__3 = directConnectionMyRvLink.GetNextCommandId();
							_003CtotalRetryAmount_003E5__5++;
							val2 = TaskExtension.TryDelay(200, cancellationToken).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (_003C_003E1__state = 2);
								_003C_003Eu__2 = val2;
								((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CDeviceBlockWriteAsync_003Ed__16>(ref val2, ref this);
								return;
							}
							goto IL_04e0;
						}
						goto IL_04ed;
					}
					IMyRvLinkCommandResponse result2 = val.GetResult();
					if ((int)result2.CommandResult != 0 || result2 is IMyRvLinkCommandResponseFailure)
					{
						TaggedLog.Error("DirectConnectionMyRvLink", $"DeviceBlockWrite received a failure on the finish write command. Result: {result2}", global::System.Array.Empty<object>());
						throw new BlockTransferWriteFailedException(logicalDevice, blockId, (ILogicalDeviceTransferProgress)(object)new LogicalDeviceTransferProgress((UInt48)((global::System.Collections.Generic.IReadOnlyCollection<byte>)data).Count, (UInt48)_003CtotalRetryAmount_003E5__5, (UInt48)_003CcurrentCommandRetryAmount_003E5__6, TimeSpan.FromMilliseconds((double)_003Ctimer_003E5__4.ElapsedMilliseconds)), (global::System.Exception)null);
					}
					_003CtimeTracker_003E5__12.Stop();
					TaggedLog.Information("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} {"DeviceBlockWriteAsync"} Stats{Environment.NewLine}{_003CtimeTracker_003E5__12}", global::System.Array.Empty<object>());
					_003CtimeTracker_003E5__12.SwitchTrackingTo(BlockWriteTimeTracker.TrackId.ProgressAck);
					if (!progressAck.Invoke((ILogicalDeviceTransferProgress)(object)new LogicalDeviceTransferProgress((UInt48)_003CbytesSent_003E5__10, (UInt48)_003CtotalRetryAmount_003E5__5, (UInt48)_003CcurrentCommandRetryAmount_003E5__6, TimeSpan.FromMilliseconds((double)_003Ctimer_003E5__4.ElapsedMilliseconds))))
					{
						throw new OperationCanceledException();
					}
					_003C_003E8__1 = null;
					_003Ctimer_003E5__4 = null;
					_003CsendDataChunk_003E5__7 = null;
					_003CcommandData_003E5__9 = null;
					_003CtimeTracker_003E5__12 = null;
					_003CfinishResultTask_003E5__14 = null;
				}
				finally
				{
					if (num < 0)
					{
						directConnectionMyRvLink._firmwareUpdateInProgress = false;
					}
				}
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

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CFirmwareUpdateAuthorizationAsync_003Ed__163 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public ILogicalDevice logicalDevice;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public CancellationToken cancellationToken;

		private TaskAwaiter<CommandResult> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			try
			{
				TaskAwaiter<CommandResult> val;
				if (num != 0)
				{
					TaggedLog.Information("DirectConnectionMyRvLink", $"Requesting OTA Authorization for {logicalDevice}", global::System.Array.Empty<object>());
					val = directConnectionMyRvLink.SendSoftwareUpdateAuthorizationAsync(logicalDevice, cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CommandResult>, _003CFirmwareUpdateAuthorizationAsync_003Ed__163>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<CommandResult>);
					num = (_003C_003E1__state = -1);
				}
				if ((int)val.GetResult() != 0)
				{
					throw new FirmwareUpdateNotAuthorizedException(logicalDevice, (global::System.Exception)null);
				}
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

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDeviceBlockCapacityAsync_003Ed__9 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<ulong> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public BlockTransferBlockId blockId;

		public CancellationToken cancellationToken;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			ulong result2;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
					}
					ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!myRvDeviceFromLogicalDevice.HasValue)
					{
						throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
					}
					MyRvLinkCommandGetDeviceBlockProperties command = new MyRvLinkCommandGetDeviceBlockProperties(directConnectionMyRvLink.GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)3);
					val = directConnectionMyRvLink.SendCommandAsync(command, cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CGetDeviceBlockCapacityAsync_003Ed__9>(ref val, ref this);
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
				if (result is IMyRvLinkCommandResponseFailure failure)
				{
					throw new MyRvLinkCommandResponseFailureException(failure);
				}
				if (!(result is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse))
				{
					throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
				}
				result2 = myRvLinkGetDeviceBlockPropertyCommandResponse.BlockCapacity;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result2);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDeviceBlockCrcAsync_003Ed__11 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<uint> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public bool recalculate;

		public BlockTransferBlockId blockId;

		public CancellationToken cancellationToken;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			uint crc;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
					}
					ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!myRvDeviceFromLogicalDevice.HasValue)
					{
						throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
					}
					ushort nextCommandId = directConnectionMyRvLink.GetNextCommandId();
					MyRvLinkCommandGetDeviceBlockProperties command = ((!recalculate) ? new MyRvLinkCommandGetDeviceBlockProperties(nextCommandId, myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)5) : new MyRvLinkCommandGetDeviceBlockProperties(nextCommandId, myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)6));
					val = directConnectionMyRvLink.SendCommandAsync(command, cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CGetDeviceBlockCrcAsync_003Ed__11>(ref val, ref this);
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
				if (result is IMyRvLinkCommandResponseFailure failure)
				{
					throw new MyRvLinkCommandResponseFailureException(failure);
				}
				if (!(result is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse))
				{
					throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
				}
				crc = myRvLinkGetDeviceBlockPropertyCommandResponse.Crc;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(crc);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDeviceBlockListAsync_003Ed__5 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<global::System.Collections.Generic.IReadOnlyList<BlockTransferBlockId>> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public CancellationToken cancellationToken;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			global::System.Collections.Generic.IReadOnlyList<BlockTransferBlockId> blockIds;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
					}
					ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!myRvDeviceFromLogicalDevice.HasValue)
					{
						throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
					}
					MyRvLinkCommandGetDeviceBlockList command = new MyRvLinkCommandGetDeviceBlockList(directConnectionMyRvLink.GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2);
					val = directConnectionMyRvLink.SendCommandAsync(command, cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CGetDeviceBlockListAsync_003Ed__5>(ref val, ref this);
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
				if (result is IMyRvLinkCommandResponseFailure failure)
				{
					throw new MyRvLinkCommandResponseFailureException(failure);
				}
				if (!(result is MyRvLinkGetDeviceBlockListCommandResponse myRvLinkGetDeviceBlockListCommandResponse))
				{
					throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
				}
				blockIds = myRvLinkGetDeviceBlockListCommandResponse.BlockIds;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(blockIds);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDeviceBlockPropertyFlagsAsync_003Ed__6 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<BlockTransferPropertyFlags> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public BlockTransferBlockId blockId;

		public CancellationToken cancellationToken;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			BlockTransferPropertyFlags flags;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
					}
					ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!myRvDeviceFromLogicalDevice.HasValue)
					{
						throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
					}
					MyRvLinkCommandGetDeviceBlockProperties command = new MyRvLinkCommandGetDeviceBlockProperties(directConnectionMyRvLink.GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)0);
					val = directConnectionMyRvLink.SendCommandAsync(command, cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CGetDeviceBlockPropertyFlagsAsync_003Ed__6>(ref val, ref this);
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
				if (result is IMyRvLinkCommandResponseFailure failure)
				{
					throw new MyRvLinkCommandResponseFailureException(failure);
				}
				if (!(result is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse))
				{
					throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
				}
				flags = myRvLinkGetDeviceBlockPropertyCommandResponse.Flags;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(flags);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDeviceBlockPropertyReadSessionIdAsync_003Ed__7 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<LogicalDeviceSessionType> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public BlockTransferBlockId blockId;

		public CancellationToken cancellationToken;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			LogicalDeviceSessionType readSessionId;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
					}
					ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!myRvDeviceFromLogicalDevice.HasValue)
					{
						throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
					}
					MyRvLinkCommandGetDeviceBlockProperties command = new MyRvLinkCommandGetDeviceBlockProperties(directConnectionMyRvLink.GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)1);
					val = directConnectionMyRvLink.SendCommandAsync(command, cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CGetDeviceBlockPropertyReadSessionIdAsync_003Ed__7>(ref val, ref this);
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
				if (result is IMyRvLinkCommandResponseFailure failure)
				{
					throw new MyRvLinkCommandResponseFailureException(failure);
				}
				if (!(result is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse))
				{
					throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
				}
				readSessionId = myRvLinkGetDeviceBlockPropertyCommandResponse.ReadSessionId;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(readSessionId);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDeviceBlockPropertyWriteSessionIdAsync_003Ed__8 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<LogicalDeviceSessionType> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public BlockTransferBlockId blockId;

		public CancellationToken cancellationToken;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			LogicalDeviceSessionType writeSessionId;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
					}
					ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!myRvDeviceFromLogicalDevice.HasValue)
					{
						throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
					}
					MyRvLinkCommandGetDeviceBlockProperties command = new MyRvLinkCommandGetDeviceBlockProperties(directConnectionMyRvLink.GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)2);
					val = directConnectionMyRvLink.SendCommandAsync(command, cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CGetDeviceBlockPropertyWriteSessionIdAsync_003Ed__8>(ref val, ref this);
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
				if (result is IMyRvLinkCommandResponseFailure failure)
				{
					throw new MyRvLinkCommandResponseFailureException(failure);
				}
				if (!(result is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse))
				{
					throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
				}
				writeSessionId = myRvLinkGetDeviceBlockPropertyCommandResponse.WriteSessionId;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(writeSessionId);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDeviceBlockSizeAsync_003Ed__10 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<ulong> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public BlockTransferBlockId blockId;

		public CancellationToken cancellationToken;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			ulong result2;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
					}
					ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!myRvDeviceFromLogicalDevice.HasValue)
					{
						throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
					}
					MyRvLinkCommandGetDeviceBlockProperties command = new MyRvLinkCommandGetDeviceBlockProperties(directConnectionMyRvLink.GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)4);
					val = directConnectionMyRvLink.SendCommandAsync(command, cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CGetDeviceBlockSizeAsync_003Ed__10>(ref val, ref this);
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
				if (result is IMyRvLinkCommandResponseFailure failure)
				{
					throw new MyRvLinkCommandResponseFailureException(failure);
				}
				if (!(result is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse))
				{
					throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
				}
				result2 = myRvLinkGetDeviceBlockPropertyCommandResponse.CurrentBlockSize;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result2);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDeviceBlockStartAddressAsync_003Ed__12 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<uint> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public BlockTransferBlockId blockId;

		public CancellationToken cancellationToken;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			uint blockStartAddress;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
					}
					ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!myRvDeviceFromLogicalDevice.HasValue)
					{
						throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
					}
					MyRvLinkCommandGetDeviceBlockProperties command = new MyRvLinkCommandGetDeviceBlockProperties(directConnectionMyRvLink.GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)7);
					val = directConnectionMyRvLink.SendCommandAsync(command, cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CGetDeviceBlockStartAddressAsync_003Ed__12>(ref val, ref this);
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
				if (result is IMyRvLinkCommandResponseFailure failure)
				{
					throw new MyRvLinkCommandResponseFailureException(failure);
				}
				if (!(result is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse))
				{
					throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
				}
				blockStartAddress = myRvLinkGetDeviceBlockPropertyCommandResponse.BlockStartAddress;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(blockStartAddress);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDevicePidListAsync_003Ed__144 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<IReadOnlyDictionary<Pid, PidAccess>> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public Pid startPidId;

		public Pid endPidId;

		public CancellationToken cancellationToken;

		private MyRvLinkCommandGetDevicePidList _003Ccommand_003E5__2;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			IReadOnlyDictionary<Pid, PidAccess> pidDict;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsStarted)
					{
						throw new MyRvLinkDeviceServiceNotStartedException(directConnectionMyRvLink, "Unable to Get PID List");
					}
					if (!directConnectionMyRvLink.IsConnected)
					{
						throw new MyRvLinkDeviceServiceNotConnectedException(directConnectionMyRvLink, "Unable to Get PID List");
					}
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new MyRvLinkDeviceOfflineException(directConnectionMyRvLink, logicalDevice);
					}
					ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!myRvDeviceFromLogicalDevice.HasValue)
					{
						throw new MyRvLinkDeviceNotFoundException(directConnectionMyRvLink, logicalDevice);
					}
					_003Ccommand_003E5__2 = new MyRvLinkCommandGetDevicePidList(directConnectionMyRvLink.GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, startPidId, endPidId);
					val = directConnectionMyRvLink.SendCommandAsync(_003Ccommand_003E5__2, cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CGetDevicePidListAsync_003Ed__144>(ref val, ref this);
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
				if (result is IMyRvLinkCommandResponseFailure failure)
				{
					throw new MyRvLinkCommandResponseFailureException(failure);
				}
				if (!(result is MyRvLinkCommandGetDevicePidListResponseCompleted))
				{
					throw new MyRvLinkException($"Failed to Get PID Values from {logicalDevice}: Unknown result");
				}
				pidDict = (IReadOnlyDictionary<Pid, PidAccess>)(object)_003Ccommand_003E5__2.PidDict;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003Ccommand_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003Ccommand_003E5__2 = null;
			_003C_003Et__builder.SetResult(pidDict);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CGetDtcValuesAsync_003Ed__150 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<IReadOnlyDictionary<DTC_ID, DtcValue>> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public DTC_ID startDtcId;

		public DTC_ID endDtcId;

		public ILogicalDevice logicalDevice;

		public CancellationToken cancellationToken;

		public LogicalDeviceDtcFilter dtcFilter;

		private ValueTuple<byte, byte>? _003CmyRvLinkDevice_003E5__2;

		private TaskSerialLock _003C_003E7__wrap2;

		private TaskAwaiter<TaskSerialLock> _003C_003Eu__1;

		private MyRvLinkCommandGetProductDtcValues _003Ccommand_003E5__4;

		private TaskAwaiter _003C_003Eu__2;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__3;

		private void MoveNext()
		{
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			IReadOnlyDictionary<DTC_ID, DtcValue> dtcDict;
			try
			{
				TaskAwaiter<TaskSerialLock> val;
				if (num != 0)
				{
					if ((uint)(num - 1) <= 1u)
					{
						goto IL_017e;
					}
					if (!directConnectionMyRvLink.IsStarted)
					{
						throw new MyRvLinkDeviceServiceNotStartedException(directConnectionMyRvLink, $"Unable to Get DTC value {startDtcId} - {endDtcId}");
					}
					if (!directConnectionMyRvLink.IsConnected)
					{
						throw new MyRvLinkDeviceServiceNotConnectedException(directConnectionMyRvLink, $"Unable to Get DTC value {startDtcId} - {endDtcId}");
					}
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new MyRvLinkDeviceOfflineException(directConnectionMyRvLink, logicalDevice);
					}
					_003CmyRvLinkDevice_003E5__2 = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!_003CmyRvLinkDevice_003E5__2.HasValue)
					{
						throw new MyRvLinkDeviceNotFoundException(directConnectionMyRvLink, logicalDevice);
					}
					val = directConnectionMyRvLink._dtcSerialQueue.GetLock(cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<TaskSerialLock>, _003CGetDtcValuesAsync_003Ed__150>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<TaskSerialLock>);
					num = (_003C_003E1__state = -1);
				}
				TaskSerialLock result = val.GetResult();
				_003C_003E7__wrap2 = result;
				goto IL_017e;
				IL_017e:
				try
				{
					TaskAwaiter val2;
					TaskAwaiter<IMyRvLinkCommandResponse> val3;
					if (num != 1)
					{
						if (num != 2)
						{
							if (directConnectionMyRvLink._dtcThrottleStopwatch.IsRunning && directConnectionMyRvLink._dtcThrottleStopwatch.ElapsedMilliseconds < 500)
							{
								long num2 = 500 - directConnectionMyRvLink._dtcThrottleStopwatch.ElapsedMilliseconds;
								if (num2 > 0)
								{
									TaggedLog.Information("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} DTC Get Value Request Throttled for {num2}ms", global::System.Array.Empty<object>());
									val2 = global::System.Threading.Tasks.Task.Delay((int)num2, cancellationToken).GetAwaiter();
									if (!((TaskAwaiter)(ref val2)).IsCompleted)
									{
										num = (_003C_003E1__state = 1);
										_003C_003Eu__2 = val2;
										_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CGetDtcValuesAsync_003Ed__150>(ref val2, ref this);
										return;
									}
									goto IL_027b;
								}
							}
							goto IL_0282;
						}
						val3 = _003C_003Eu__3;
						_003C_003Eu__3 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
						num = (_003C_003E1__state = -1);
						goto IL_0335;
					}
					val2 = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_027b;
					IL_027b:
					((TaskAwaiter)(ref val2)).GetResult();
					goto IL_0282;
					IL_0282:
					directConnectionMyRvLink._dtcThrottleStopwatch.Stop();
					_003Ccommand_003E5__4 = new MyRvLinkCommandGetProductDtcValues(directConnectionMyRvLink.GetNextCommandId(), _003CmyRvLinkDevice_003E5__2.Value.Item1, _003CmyRvLinkDevice_003E5__2.Value.Item2, dtcFilter, startDtcId, endDtcId);
					val3 = directConnectionMyRvLink.SendCommandAsync(_003Ccommand_003E5__4, cancellationToken).GetAwaiter();
					if (!val3.IsCompleted)
					{
						num = (_003C_003E1__state = 2);
						_003C_003Eu__3 = val3;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CGetDtcValuesAsync_003Ed__150>(ref val3, ref this);
						return;
					}
					goto IL_0335;
					IL_0335:
					IMyRvLinkCommandResponse result2 = val3.GetResult();
					if (result2 is IMyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
					{
						if (myRvLinkCommandResponseFailure.FailureCode == MyRvLinkCommandResponseFailureCode.TooManyCommandsRunning)
						{
							directConnectionMyRvLink._dtcThrottleStopwatch.Restart();
						}
						throw new MyRvLinkCommandResponseFailureException(myRvLinkCommandResponseFailure);
					}
					if (!(result2 is MyRvLinkCommandGetProductDtcValuesResponseCompleted))
					{
						throw new MyRvLinkException($"Failed to Get DTC Values from {logicalDevice}: Unknown result");
					}
					dtcDict = (IReadOnlyDictionary<DTC_ID, DtcValue>)(object)_003Ccommand_003E5__4.DtcDict;
				}
				finally
				{
					if (num < 0 && _003C_003E7__wrap2 != null)
					{
						((global::System.IDisposable)_003C_003E7__wrap2).Dispose();
					}
				}
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(dtcDict);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CJumpToBootloaderAsync_003Ed__161 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<ILogicalDeviceReflashBootloader> _003C_003Et__builder;

		public ILogicalDeviceJumpToBootloader logicalDevice;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public CancellationToken cancellationToken;

		public TimeSpan holdTime;

		private bool _003ClookForErrorDetails_003E5__2;

		private TaskAwaiter<LogicalDeviceJumpToBootState> _003C_003Eu__1;

		private TaskAwaiter _003C_003Eu__2;

		private int _003Cattempt_003E5__3;

		private void MoveNext()
		{
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Invalid comparison between Unknown and I4
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Invalid comparison between Unknown and I4
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Invalid comparison between Unknown and I4
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Invalid comparison between Unknown and I4
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Invalid comparison between Unknown and I4
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Invalid comparison between Unknown and I4
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Invalid comparison between Unknown and I4
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Invalid comparison between Unknown and I4
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Invalid comparison between Unknown and I4
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Invalid comparison between Unknown and I4
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Invalid comparison between Unknown and I4
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Invalid comparison between Unknown and I4
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Invalid comparison between Unknown and I4
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Invalid comparison between Unknown and I4
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0449: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Invalid comparison between Unknown and I4
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Invalid comparison between Unknown and I4
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Invalid comparison between Unknown and I4
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			ILogicalDeviceReflashBootloader result;
			try
			{
				TaskAwaiter val;
				TaskAwaiter<LogicalDeviceJumpToBootState> val2;
				LogicalDeviceJumpToBootState result2;
				ILogicalDeviceReflashBootloader associatedLogicalDeviceBootloader;
				switch (num)
				{
				default:
				{
					ILogicalDeviceJumpToBootloader obj = logicalDevice;
					ILogicalDeviceReflashBootloader val3 = (ILogicalDeviceReflashBootloader)(object)((obj is ILogicalDeviceReflashBootloader) ? obj : null);
					if (val3 != null)
					{
						result = val3;
						break;
					}
					associatedLogicalDeviceBootloader = GetAssociatedLogicalDeviceBootloader((ILogicalDevice)(object)logicalDevice);
					if (associatedLogicalDeviceBootloader != null && (int)((IDevicesCommon)associatedLogicalDeviceBootloader).ActiveConnection == 1)
					{
						result = associatedLogicalDeviceBootloader;
						break;
					}
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
					{
						throw new MyRvLinkDeviceOfflineException(directConnectionMyRvLink, (ILogicalDevice)(object)logicalDevice);
					}
					val2 = logicalDevice.JumpToBootPid.ReadJumpToBootStateAsync(cancellationToken).GetAwaiter();
					if (!val2.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val2;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<LogicalDeviceJumpToBootState>, _003CJumpToBootloaderAsync_003Ed__161>(ref val2, ref this);
						return;
					}
					goto IL_00e0;
				}
				case 0:
					val2 = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<LogicalDeviceJumpToBootState>);
					num = (_003C_003E1__state = -1);
					goto IL_00e0;
				case 1:
					val = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_01f1;
				case 2:
					val = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_02f4;
				case 3:
					{
						try
						{
							if (num != 3)
							{
								val2 = logicalDevice.JumpToBootPid.ReadJumpToBootStateAsync(cancellationToken).GetAwaiter();
								if (!val2.IsCompleted)
								{
									num = (_003C_003E1__state = 3);
									_003C_003Eu__1 = val2;
									_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<LogicalDeviceJumpToBootState>, _003CJumpToBootloaderAsync_003Ed__161>(ref val2, ref this);
									return;
								}
							}
							else
							{
								val2 = _003C_003Eu__1;
								_003C_003Eu__1 = default(TaskAwaiter<LogicalDeviceJumpToBootState>);
								num = (_003C_003E1__state = -1);
							}
							result2 = val2.GetResult();
							if ((int)result2 <= 1432778632)
							{
								if ((int)result2 == 0 || ((int)result2 != 287454020 && (int)result2 != 1432778632))
								{
									goto IL_03d4;
								}
							}
							else
							{
								if ((int)result2 > -1431651397)
								{
									if ((int)result2 != -858989091)
									{
										_ = -286326785;
									}
									goto IL_03d4;
								}
								if ((int)result2 != -1717986919)
								{
									if ((int)result2 == -1431651397)
									{
									}
									goto IL_03d4;
								}
							}
							goto end_IL_0307;
							IL_03d4:
							TaggedLog.Information("DirectConnectionMyRvLink", $"Failed to enter bootloader mode because {result2} for {logicalDevice}", global::System.Array.Empty<object>());
							throw new FirmwareUpdateBootloaderException($"Failed to enter bootloader mode because {result2}", (global::System.Exception)null);
							end_IL_0307:;
						}
						catch
						{
							_003ClookForErrorDetails_003E5__2 = false;
						}
						goto IL_045b;
					}
					IL_02f4:
					((TaskAwaiter)(ref val)).GetResult();
					if (_003ClookForErrorDetails_003E5__2)
					{
						goto case 3;
					}
					goto IL_045b;
					IL_00e0:
					result2 = val2.GetResult();
					if ((int)result2 <= 1432778632)
					{
						if ((int)result2 != 0)
						{
							if ((int)result2 == 287454020)
							{
								goto IL_014b;
							}
							if ((int)result2 == 1432778632)
							{
							}
						}
					}
					else if ((int)result2 <= -1431651397)
					{
						if ((int)result2 != -1717986919 && (int)result2 == -1431651397)
						{
							goto IL_014b;
						}
					}
					else if ((int)result2 == -858989091 || (int)result2 == -286326785)
					{
						goto IL_014b;
					}
					TaggedLog.Information("DirectConnectionMyRvLink", $"Unable to request to put device in bootloader mode because of it's current state {result2} for {logicalDevice}", global::System.Array.Empty<object>());
					throw new FirmwareUpdateBootloaderException($"Unable to request to put device in bootloader mode because of it's current state {result2}", (global::System.Exception)null);
					IL_014b:
					TaggedLog.Information("DirectConnectionMyRvLink", $"Performing Jump To Boot {logicalDevice}", global::System.Array.Empty<object>());
					val = logicalDevice.JumpToBootPid.WriteRequestJumpToBoot(holdTime, cancellationToken).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (_003C_003E1__state = 1);
						_003C_003Eu__2 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CJumpToBootloaderAsync_003Ed__161>(ref val, ref this);
						return;
					}
					goto IL_01f1;
					IL_045b:
					associatedLogicalDeviceBootloader = GetAssociatedLogicalDeviceBootloader((ILogicalDevice)(object)logicalDevice);
					if (associatedLogicalDeviceBootloader != null && (int)((IDevicesCommon)associatedLogicalDeviceBootloader).ActiveConnection == 1)
					{
						TaggedLog.Information("DirectConnectionMyRvLink", $"Found Bootloader Device {associatedLogicalDeviceBootloader}", global::System.Array.Empty<object>());
						result = associatedLogicalDeviceBootloader;
						break;
					}
					_003Cattempt_003E5__3++;
					goto IL_04be;
					IL_04be:
					if (_003Cattempt_003E5__3 < 20)
					{
						((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
						val = global::System.Threading.Tasks.Task.Delay(1000, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (_003C_003E1__state = 2);
							_003C_003Eu__2 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CJumpToBootloaderAsync_003Ed__161>(ref val, ref this);
							return;
						}
						goto IL_02f4;
					}
					TaggedLog.Information("DirectConnectionMyRvLink", $"Failed to enter/find bootloader for {logicalDevice}", global::System.Array.Empty<object>());
					throw new FirmwareUpdateBootloaderException("Unable to find Bootloader Device", (global::System.Exception)null);
					IL_01f1:
					((TaskAwaiter)(ref val)).GetResult();
					_003ClookForErrorDetails_003E5__2 = true;
					_003Cattempt_003E5__3 = 0;
					goto IL_04be;
				}
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CPidReadAsync_003Ed__140 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<UInt48> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public Pid pid;

		public ILogicalDevice logicalDevice;

		public CancellationToken cancellationToken;

		private ValueTuple<byte, byte>? _003CmyRvLinkDevice_003E5__2;

		private TaskSerialLock _003C_003E7__wrap2;

		private TaskAwaiter<TaskSerialLock> _003C_003Eu__1;

		private MyRvLinkCommandGetDevicePid _003Ccommand_003E5__4;

		private TaskAwaiter<bool> _003C_003Eu__2;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__3;

		private void MoveNext()
		{
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			UInt48 pidValue;
			try
			{
				TaskAwaiter<TaskSerialLock> val;
				if (num != 0)
				{
					if ((uint)(num - 1) <= 1u)
					{
						goto IL_015f;
					}
					if (!directConnectionMyRvLink.IsStarted)
					{
						throw new MyRvLinkDeviceServiceNotStartedException(directConnectionMyRvLink, $"Unable to Read PID value {pid}");
					}
					if (!directConnectionMyRvLink.IsConnected)
					{
						throw new MyRvLinkDeviceServiceNotConnectedException(directConnectionMyRvLink, $"Unable to Read PID value {pid}");
					}
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new MyRvLinkDeviceOfflineException(directConnectionMyRvLink, logicalDevice);
					}
					if (directConnectionMyRvLink._firmwareUpdateInProgress)
					{
						throw new MyRvLinkPidReadException("Can't perform Pid reads while a firmware update is in progress!");
					}
					_003CmyRvLinkDevice_003E5__2 = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!_003CmyRvLinkDevice_003E5__2.HasValue)
					{
						throw new MyRvLinkDeviceNotFoundException(directConnectionMyRvLink, logicalDevice);
					}
					val = directConnectionMyRvLink._pidSerialQueue.GetLock(cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<TaskSerialLock>, _003CPidReadAsync_003Ed__140>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<TaskSerialLock>);
					num = (_003C_003E1__state = -1);
				}
				TaskSerialLock result = val.GetResult();
				_003C_003E7__wrap2 = result;
				goto IL_015f;
				IL_015f:
				try
				{
					TaskAwaiter<IMyRvLinkCommandResponse> val2;
					TaskAwaiter<bool> val3;
					if (num != 1)
					{
						if (num == 2)
						{
							val2 = _003C_003Eu__3;
							_003C_003Eu__3 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
							num = (_003C_003E1__state = -1);
							goto IL_0296;
						}
						TimeSpan elapsed = directConnectionMyRvLink._pidLastOperationTimer.Elapsed;
						int num2 = 100 - (int)((TimeSpan)(ref elapsed)).TotalMilliseconds;
						if (num2 <= 0)
						{
							goto IL_01ef;
						}
						val3 = TaskExtension.TryDelay(num2, cancellationToken).GetAwaiter();
						if (!val3.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__2 = val3;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CPidReadAsync_003Ed__140>(ref val3, ref this);
							return;
						}
					}
					else
					{
						val3 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
					}
					val3.GetResult();
					goto IL_01ef;
					IL_0296:
					IMyRvLinkCommandResponse result2 = val2.GetResult();
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} PidReadAsync Response {result2} Last operation was performed {directConnectionMyRvLink._pidLastOperationTimer.ElapsedMilliseconds}ms ago", global::System.Array.Empty<object>());
					directConnectionMyRvLink._pidLastOperationTimer.Restart();
					if (result2 is IMyRvLinkCommandResponseFailure failure)
					{
						throw new MyRvLinkCommandResponseFailureException(failure);
					}
					if (!(result2 is MyRvLinkCommandGetDevicePidResponseCompleted myRvLinkCommandGetDevicePidResponseCompleted))
					{
						throw new MyRvLinkCommandResponseFailureException(new MyRvLinkCommandResponseFailure(_003Ccommand_003E5__4.ClientCommandId, MyRvLinkCommandResponseFailureCode.InvalidResponse));
					}
					if (PidExtension.IsAutoCacheingPid(pid))
					{
						logicalDevice.SetCachedPidRawValue(pid, myRvLinkCommandGetDevicePidResponseCompleted.PidValue);
					}
					pidValue = myRvLinkCommandGetDevicePidResponseCompleted.PidValue;
					goto end_IL_015f;
					IL_01ef:
					((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
					_003Ccommand_003E5__4 = new MyRvLinkCommandGetDevicePid(directConnectionMyRvLink.GetNextCommandId(), _003CmyRvLinkDevice_003E5__2.Value.Item1, _003CmyRvLinkDevice_003E5__2.Value.Item2, pid);
					val2 = directConnectionMyRvLink.SendCommandAsync(_003Ccommand_003E5__4, cancellationToken).GetAwaiter();
					if (!val2.IsCompleted)
					{
						num = (_003C_003E1__state = 2);
						_003C_003Eu__3 = val2;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CPidReadAsync_003Ed__140>(ref val2, ref this);
						return;
					}
					goto IL_0296;
					end_IL_015f:;
				}
				finally
				{
					if (num < 0 && _003C_003E7__wrap2 != null)
					{
						((global::System.IDisposable)_003C_003E7__wrap2).Dispose();
					}
				}
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(pidValue);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CPidReadAsync_003Ed__142 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<uint> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public Pid pid;

		public ushort address;

		public ILogicalDevice logicalDevice;

		public CancellationToken cancellationToken;

		private ValueTuple<byte, byte>? _003CmyRvLinkDevice_003E5__2;

		private TaskSerialLock _003C_003E7__wrap2;

		private TaskAwaiter<TaskSerialLock> _003C_003Eu__1;

		private MyRvLinkCommandGetDevicePidWithAddress _003Ccommand_003E5__4;

		private TaskAwaiter<bool> _003C_003Eu__2;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__3;

		private void MoveNext()
		{
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			uint pidValue;
			try
			{
				TaskAwaiter<TaskSerialLock> val;
				if (num != 0)
				{
					if ((uint)(num - 1) <= 1u)
					{
						goto IL_0191;
					}
					if (!directConnectionMyRvLink.IsStarted)
					{
						throw new MyRvLinkDeviceServiceNotStartedException(directConnectionMyRvLink, $"Unable to Read PID value {pid} with Address {address}");
					}
					if (!directConnectionMyRvLink.IsConnected)
					{
						throw new MyRvLinkDeviceServiceNotConnectedException(directConnectionMyRvLink, $"Unable to Read PID value {pid} with Address {address}");
					}
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new MyRvLinkDeviceOfflineException(directConnectionMyRvLink, logicalDevice);
					}
					if (directConnectionMyRvLink._firmwareUpdateInProgress)
					{
						throw new MyRvLinkPidReadException("Can't perform Pid reads while a firmware update is in progress!");
					}
					_003CmyRvLinkDevice_003E5__2 = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!_003CmyRvLinkDevice_003E5__2.HasValue)
					{
						throw new MyRvLinkDeviceNotFoundException(directConnectionMyRvLink, logicalDevice);
					}
					val = directConnectionMyRvLink._pidSerialQueue.GetLock(cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<TaskSerialLock>, _003CPidReadAsync_003Ed__142>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<TaskSerialLock>);
					num = (_003C_003E1__state = -1);
				}
				TaskSerialLock result = val.GetResult();
				_003C_003E7__wrap2 = result;
				goto IL_0191;
				IL_0191:
				try
				{
					TaskAwaiter<IMyRvLinkCommandResponse> val2;
					TaskAwaiter<bool> val3;
					if (num != 1)
					{
						if (num == 2)
						{
							val2 = _003C_003Eu__3;
							_003C_003Eu__3 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
							num = (_003C_003E1__state = -1);
							goto IL_02ce;
						}
						TimeSpan elapsed = directConnectionMyRvLink._pidLastOperationTimer.Elapsed;
						int num2 = 100 - (int)((TimeSpan)(ref elapsed)).TotalMilliseconds;
						if (num2 <= 0)
						{
							goto IL_0221;
						}
						val3 = TaskExtension.TryDelay(num2, cancellationToken).GetAwaiter();
						if (!val3.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__2 = val3;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CPidReadAsync_003Ed__142>(ref val3, ref this);
							return;
						}
					}
					else
					{
						val3 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
					}
					val3.GetResult();
					goto IL_0221;
					IL_02ce:
					IMyRvLinkCommandResponse result2 = val2.GetResult();
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} PidReadAsync Response {result2} Last operation was performed {directConnectionMyRvLink._pidLastOperationTimer.ElapsedMilliseconds}ms ago", global::System.Array.Empty<object>());
					directConnectionMyRvLink._pidLastOperationTimer.Restart();
					if (result2 is IMyRvLinkCommandResponseFailure failure)
					{
						throw new MyRvLinkCommandResponseFailureException(failure);
					}
					if (!(result2 is MyRvLinkCommandGetDevicePidWithAddressResponseCompleted myRvLinkCommandGetDevicePidWithAddressResponseCompleted))
					{
						throw new MyRvLinkCommandResponseFailureException(new MyRvLinkCommandResponseFailure(_003Ccommand_003E5__4.ClientCommandId, MyRvLinkCommandResponseFailureCode.InvalidResponse));
					}
					pidValue = myRvLinkCommandGetDevicePidWithAddressResponseCompleted.PidValue;
					goto end_IL_0191;
					IL_0221:
					((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
					_003Ccommand_003E5__4 = new MyRvLinkCommandGetDevicePidWithAddress(directConnectionMyRvLink.GetNextCommandId(), _003CmyRvLinkDevice_003E5__2.Value.Item1, _003CmyRvLinkDevice_003E5__2.Value.Item2, pid, address);
					val2 = directConnectionMyRvLink.SendCommandAsync(_003Ccommand_003E5__4, cancellationToken).GetAwaiter();
					if (!val2.IsCompleted)
					{
						num = (_003C_003E1__state = 2);
						_003C_003Eu__3 = val2;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CPidReadAsync_003Ed__142>(ref val2, ref this);
						return;
					}
					goto IL_02ce;
					end_IL_0191:;
				}
				finally
				{
					if (num < 0 && _003C_003E7__wrap2 != null)
					{
						((global::System.IDisposable)_003C_003E7__wrap2).Dispose();
					}
				}
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(pidValue);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CPidWriteAsync_003Ed__141 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public Pid pid;

		public ILogicalDevice logicalDevice;

		public CancellationToken cancellationToken;

		public LogicalDeviceSessionType pidWriteAccess;

		public UInt48 pidValue;

		private ValueTuple<byte, byte>? _003CmyRvLinkDevice_003E5__2;

		private TaskSerialLock _003C_003E7__wrap2;

		private TaskAwaiter<TaskSerialLock> _003C_003Eu__1;

		private MyRvLinkCommandSetDevicePid _003Ccommand_003E5__4;

		private TaskAwaiter<bool> _003C_003Eu__2;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__3;

		private void MoveNext()
		{
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			try
			{
				TaskAwaiter<TaskSerialLock> val;
				if (num != 0)
				{
					if ((uint)(num - 1) <= 1u)
					{
						goto IL_015d;
					}
					if (!directConnectionMyRvLink.IsStarted)
					{
						throw new MyRvLinkDeviceServiceNotStartedException(directConnectionMyRvLink, $"Unable to Write PID value {pid}");
					}
					if (!directConnectionMyRvLink.IsConnected)
					{
						throw new MyRvLinkDeviceServiceNotConnectedException(directConnectionMyRvLink, $"Unable to Write PID value {pid}");
					}
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new MyRvLinkDeviceOfflineException(directConnectionMyRvLink, logicalDevice);
					}
					if (directConnectionMyRvLink._firmwareUpdateInProgress)
					{
						throw new MyRvLinkPidWriteException("Can't perform Pid writes while a firmware update is in progress!");
					}
					_003CmyRvLinkDevice_003E5__2 = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!_003CmyRvLinkDevice_003E5__2.HasValue)
					{
						throw new MyRvLinkDeviceNotFoundException(directConnectionMyRvLink, logicalDevice);
					}
					val = directConnectionMyRvLink._pidSerialQueue.GetLockAsync(cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<TaskSerialLock>, _003CPidWriteAsync_003Ed__141>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<TaskSerialLock>);
					num = (_003C_003E1__state = -1);
				}
				TaskSerialLock result = val.GetResult();
				_003C_003E7__wrap2 = result;
				goto IL_015d;
				IL_015d:
				try
				{
					TaskAwaiter<IMyRvLinkCommandResponse> val2;
					TaskAwaiter<bool> val3;
					if (num != 1)
					{
						if (num == 2)
						{
							val2 = _003C_003Eu__3;
							_003C_003Eu__3 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
							num = (_003C_003E1__state = -1);
							goto IL_02af;
						}
						TimeSpan elapsed = directConnectionMyRvLink._pidLastOperationTimer.Elapsed;
						int num2 = 100 - (int)((TimeSpan)(ref elapsed)).TotalMilliseconds;
						if (num2 <= 0)
						{
							goto IL_01ed;
						}
						val3 = TaskExtension.TryDelay(num2, cancellationToken).GetAwaiter();
						if (!val3.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__2 = val3;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CPidWriteAsync_003Ed__141>(ref val3, ref this);
							return;
						}
					}
					else
					{
						val3 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
					}
					val3.GetResult();
					goto IL_01ed;
					IL_02af:
					IMyRvLinkCommandResponse result2 = val2.GetResult();
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} PidWriteAsync Response {result2} Last operation was performed {directConnectionMyRvLink._pidLastOperationTimer.ElapsedMilliseconds}ms ago", global::System.Array.Empty<object>());
					directConnectionMyRvLink._pidLastOperationTimer.Restart();
					if (result2 is IMyRvLinkCommandResponseFailure failure)
					{
						throw new MyRvLinkCommandResponseFailureException(failure);
					}
					if (!(result2 is MyRvLinkCommandSetDevicePidResponseCompleted myRvLinkCommandSetDevicePidResponseCompleted))
					{
						throw new MyRvLinkCommandResponseFailureException(new MyRvLinkCommandResponseFailure(_003Ccommand_003E5__4.ClientCommandId, MyRvLinkCommandResponseFailureCode.InvalidResponse));
					}
					if (!myRvLinkCommandSetDevicePidResponseCompleted.IsCommandCompleted)
					{
						throw new MyRvLinkCommandResponseFailureException(new MyRvLinkCommandResponseFailure(_003Ccommand_003E5__4.ClientCommandId, MyRvLinkCommandResponseFailureCode.InvalidResponse));
					}
					if (PidExtension.IsAutoCacheingPid(pid))
					{
						logicalDevice.SetCachedPidRawValue(pid, myRvLinkCommandSetDevicePidResponseCompleted.PidValue);
					}
					goto end_IL_015d;
					IL_01ed:
					((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
					SESSION_ID sessionId = LogicalDevicePidWriteAccessExtension.ToIdsCanSessionId(pidWriteAccess);
					_003Ccommand_003E5__4 = new MyRvLinkCommandSetDevicePid(directConnectionMyRvLink.GetNextCommandId(), _003CmyRvLinkDevice_003E5__2.Value.Item1, _003CmyRvLinkDevice_003E5__2.Value.Item2, pid, sessionId, pidValue, pidWriteAccess);
					val2 = directConnectionMyRvLink.SendCommandAsync(_003Ccommand_003E5__4, cancellationToken).GetAwaiter();
					if (!val2.IsCompleted)
					{
						num = (_003C_003E1__state = 2);
						_003C_003Eu__3 = val2;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CPidWriteAsync_003Ed__141>(ref val2, ref this);
						return;
					}
					goto IL_02af;
					end_IL_015d:;
				}
				finally
				{
					if (num < 0 && _003C_003E7__wrap2 != null)
					{
						((global::System.IDisposable)_003C_003E7__wrap2).Dispose();
					}
				}
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

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CPidWriteAsync_003Ed__143 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public Pid pid;

		public ushort address;

		public ILogicalDevice logicalDevice;

		public CancellationToken cancellationToken;

		public LogicalDeviceSessionType pidWriteAccess;

		public uint pidValue;

		private ValueTuple<byte, byte>? _003CmyRvLinkDevice_003E5__2;

		private TaskSerialLock _003C_003E7__wrap2;

		private TaskAwaiter<TaskSerialLock> _003C_003Eu__1;

		private MyRvLinkCommandSetDevicePidWithAddress _003Ccommand_003E5__4;

		private TaskAwaiter<bool> _003C_003Eu__2;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__3;

		private void MoveNext()
		{
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			try
			{
				TaskAwaiter<TaskSerialLock> val;
				if (num != 0)
				{
					if ((uint)(num - 1) <= 1u)
					{
						goto IL_018f;
					}
					if (!directConnectionMyRvLink.IsStarted)
					{
						throw new MyRvLinkDeviceServiceNotStartedException(directConnectionMyRvLink, $"Unable to Write PID value {pid} with Address {address}");
					}
					if (!directConnectionMyRvLink.IsConnected)
					{
						throw new MyRvLinkDeviceServiceNotConnectedException(directConnectionMyRvLink, $"Unable to Write PID value {pid} with Address {address}");
					}
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new MyRvLinkDeviceOfflineException(directConnectionMyRvLink, logicalDevice);
					}
					if (directConnectionMyRvLink._firmwareUpdateInProgress)
					{
						throw new MyRvLinkPidWriteException("Can't perform Pid writes while a firmware update is in progress!");
					}
					_003CmyRvLinkDevice_003E5__2 = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!_003CmyRvLinkDevice_003E5__2.HasValue)
					{
						throw new MyRvLinkDeviceNotFoundException(directConnectionMyRvLink, logicalDevice);
					}
					val = directConnectionMyRvLink._pidSerialQueue.GetLock(cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<TaskSerialLock>, _003CPidWriteAsync_003Ed__143>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<TaskSerialLock>);
					num = (_003C_003E1__state = -1);
				}
				TaskSerialLock result = val.GetResult();
				_003C_003E7__wrap2 = result;
				goto IL_018f;
				IL_018f:
				try
				{
					TaskAwaiter<IMyRvLinkCommandResponse> val2;
					TaskAwaiter<bool> val3;
					if (num != 1)
					{
						if (num == 2)
						{
							val2 = _003C_003Eu__3;
							_003C_003Eu__3 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
							num = (_003C_003E1__state = -1);
							goto IL_02e7;
						}
						TimeSpan elapsed = directConnectionMyRvLink._pidLastOperationTimer.Elapsed;
						int num2 = 100 - (int)((TimeSpan)(ref elapsed)).TotalMilliseconds;
						if (num2 <= 0)
						{
							goto IL_021f;
						}
						val3 = TaskExtension.TryDelay(num2, cancellationToken).GetAwaiter();
						if (!val3.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__2 = val3;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CPidWriteAsync_003Ed__143>(ref val3, ref this);
							return;
						}
					}
					else
					{
						val3 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
					}
					val3.GetResult();
					goto IL_021f;
					IL_02e7:
					IMyRvLinkCommandResponse result2 = val2.GetResult();
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} PidWriteAsync Response {result2} Last operation was performed {directConnectionMyRvLink._pidLastOperationTimer.ElapsedMilliseconds}ms ago", global::System.Array.Empty<object>());
					directConnectionMyRvLink._pidLastOperationTimer.Restart();
					if (result2 is IMyRvLinkCommandResponseFailure failure)
					{
						throw new MyRvLinkCommandResponseFailureException(failure);
					}
					if (!(result2 is MyRvLinkCommandSetDevicePidWithAddressResponseCompleted myRvLinkCommandSetDevicePidWithAddressResponseCompleted))
					{
						throw new MyRvLinkCommandResponseFailureException(new MyRvLinkCommandResponseFailure(_003Ccommand_003E5__4.ClientCommandId, MyRvLinkCommandResponseFailureCode.InvalidResponse));
					}
					if (!myRvLinkCommandSetDevicePidWithAddressResponseCompleted.IsCommandCompleted)
					{
						throw new MyRvLinkCommandResponseFailureException(new MyRvLinkCommandResponseFailure(_003Ccommand_003E5__4.ClientCommandId, MyRvLinkCommandResponseFailureCode.InvalidResponse));
					}
					goto end_IL_018f;
					IL_021f:
					((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
					SESSION_ID sessionId = LogicalDevicePidWriteAccessExtension.ToIdsCanSessionId(pidWriteAccess);
					_003Ccommand_003E5__4 = new MyRvLinkCommandSetDevicePidWithAddress(directConnectionMyRvLink.GetNextCommandId(), _003CmyRvLinkDevice_003E5__2.Value.Item1, _003CmyRvLinkDevice_003E5__2.Value.Item2, pid, sessionId, address, pidValue, pidWriteAccess);
					val2 = directConnectionMyRvLink.SendCommandAsync(_003Ccommand_003E5__4, cancellationToken).GetAwaiter();
					if (!val2.IsCompleted)
					{
						num = (_003C_003E1__state = 2);
						_003C_003Eu__3 = val2;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CPidWriteAsync_003Ed__143>(ref val2, ref this);
						return;
					}
					goto IL_02e7;
					end_IL_018f:;
				}
				finally
				{
					if (num < 0 && _003C_003E7__wrap2 != null)
					{
						((global::System.IDisposable)_003C_003E7__wrap2).Dispose();
					}
				}
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

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CRemoveOfflineDevicesAsync_003Ed__181 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public bool enableConfigurationMode;

		public CancellationToken cancellationToken;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsStarted || !directConnectionMyRvLink.IsConnected)
					{
						throw new MyRvLinkException("Unable to Remove Offline Devices as service isn't started");
					}
					MyRvLinkGatewayInformation gatewayInfo = directConnectionMyRvLink.GatewayInfo;
					if (gatewayInfo == null)
					{
						throw new MyRvLinkException("Unable to Remove Offline Devices as no gateway information is available yet");
					}
					directConnectionMyRvLink._deviceTracker?.RemoveOfflineDevices();
					MyRvLinkCommandRemoveOfflineDevices command = new MyRvLinkCommandRemoveOfflineDevices(directConnectionMyRvLink.GetNextCommandId(), gatewayInfo.DeviceTableId, !enableConfigurationMode);
					val = directConnectionMyRvLink.SendCommandAsync(command, cancellationToken, MyRvLinkSendCommandOption.DontWaitForResponse).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CRemoveOfflineDevicesAsync_003Ed__181>(ref val, ref this);
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
				if (result is IMyRvLinkCommandResponseFailure)
				{
					throw new MyRvLinkException("Failed to Remove Offline Devices: " + ((object)result).ToString());
				}
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

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CRenameLogicalDevice_003Ed__183 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public FUNCTION_NAME toName;

		public byte toFunctionInstance;

		public ILogicalDevice logicalDevice;

		public CancellationToken cancellationToken;

		private TaskAwaiter<CommandResult> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			try
			{
				TaskAwaiter<CommandResult> val;
				if (num != 0)
				{
					_003C_003Ec__DisplayClass183_0 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass183_0
					{
						_003C_003E4__this = _003C_003E4__this,
						toName = toName,
						toFunctionInstance = toFunctionInstance
					};
					if (logicalDevice == null)
					{
						throw new ArgumentNullException("logicalDevice");
					}
					val = directConnectionMyRvLink.SendCommandAsync(logicalDevice, (ValueTuple<byte, byte> myRvLinkDevice) => new MyRvLinkCommandRenameDevice(CS_0024_003C_003E8__locals3._003C_003E4__this.GetNextCommandId(), myRvLinkDevice.Item1, myRvLinkDevice.Item2, CS_0024_003C_003E8__locals3.toName, CS_0024_003C_003E8__locals3.toFunctionInstance, SESSION_ID.op_Implicit((ushort)2)), cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<CommandResult>, _003CRenameLogicalDevice_003Ed__183>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<CommandResult>);
					num = (_003C_003E1__state = -1);
				}
				CommandResult result = val.GetResult();
				if ((int)result != 0)
				{
					throw new global::System.Exception($"Rename failed because of {result}");
				}
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

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CResendRunningCommandAsync_003Ed__97 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public ushort commandId;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public CancellationToken cancellationToken;

		private TaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			bool result;
			try
			{
				TaskAwaiter val;
				if (num == 0)
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_01c3;
				}
				if (commandId == 0 || commandId == 65535)
				{
					result = false;
				}
				else if (commandId != directConnectionMyRvLink._currentCommandId)
				{
					result = false;
				}
				else
				{
					object obj = directConnectionMyRvLink._lock;
					bool flag = false;
					MyRvLinkCommandTracker myRvLinkCommandTracker;
					try
					{
						Monitor.Enter(obj, ref flag);
						directConnectionMyRvLink.FlushCompletedCommands();
						myRvLinkCommandTracker = DictionaryExtension.TryGetValue<int, MyRvLinkCommandTracker>((IReadOnlyDictionary<int, MyRvLinkCommandTracker>)(object)directConnectionMyRvLink._commandActiveDict, (int)commandId);
						if (myRvLinkCommandTracker != null)
						{
							myRvLinkCommandTracker.ResetTimer();
							goto end_IL_0050;
						}
						TaggedLog.Warning("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} Unable to {"ResendRunningCommandAsync"} because command tracker for {commandId} not found.", global::System.Array.Empty<object>());
						result = false;
						goto end_IL_000e;
						end_IL_0050:;
					}
					finally
					{
						if (num < 0 && flag)
						{
							Monitor.Exit(obj);
						}
					}
					if (!myRvLinkCommandTracker.IsCompleted)
					{
						TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} Resend command {myRvLinkCommandTracker.Command}", global::System.Array.Empty<object>());
						directConnectionMyRvLink.UpdateFrequencyMetricForCommandSend(myRvLinkCommandTracker.Command.CommandType);
						val = directConnectionMyRvLink.SendCommandRawAsync(myRvLinkCommandTracker.Command, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CResendRunningCommandAsync_003Ed__97>(ref val, ref this);
							return;
						}
						goto IL_01c3;
					}
					result = false;
				}
				goto end_IL_000e;
				IL_01c3:
				((TaskAwaiter)(ref val)).GetResult();
				result = true;
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CResendRunningCommandWaitForResponseAsync_003Ed__98 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<IMyRvLinkCommandResponse> _003C_003Et__builder;

		public ushort commandId;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public CancellationToken cancellationToken;

		private MyRvLinkCommandTracker _003CcommandTracker_003E5__2;

		private ConfiguredTaskAwaiter _003C_003Eu__1;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__2;

		private void MoveNext()
		{
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			IMyRvLinkCommandResponse result;
			try
			{
				if ((uint)num <= 1u)
				{
					goto IL_016a;
				}
				if (commandId == 0 || commandId == 65535)
				{
					result = null;
				}
				else if (commandId != directConnectionMyRvLink._currentCommandId)
				{
					result = null;
				}
				else
				{
					object obj = directConnectionMyRvLink._lock;
					bool flag = false;
					try
					{
						Monitor.Enter(obj, ref flag);
						directConnectionMyRvLink.FlushCompletedCommands();
						_003CcommandTracker_003E5__2 = DictionaryExtension.TryGetValue<int, MyRvLinkCommandTracker>((IReadOnlyDictionary<int, MyRvLinkCommandTracker>)(object)directConnectionMyRvLink._commandActiveDict, (int)commandId);
						if (_003CcommandTracker_003E5__2 != null)
						{
							_003CcommandTracker_003E5__2.ResetTimer();
							goto end_IL_0050;
						}
						TaggedLog.Warning("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} Unable to {"ResendRunningCommandAsync"} because command tracker for {commandId} not found.", global::System.Array.Empty<object>());
						result = null;
						goto end_IL_000e;
						end_IL_0050:;
					}
					finally
					{
						if (num < 0 && flag)
						{
							Monitor.Exit(obj);
						}
					}
					if (!_003CcommandTracker_003E5__2.IsCompleted)
					{
						TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} Resend command {_003CcommandTracker_003E5__2.Command}", global::System.Array.Empty<object>());
						goto IL_016a;
					}
					result = null;
				}
				goto end_IL_000e;
				IL_016a:
				try
				{
					TaskAwaiter<IMyRvLinkCommandResponse> val;
					ConfiguredTaskAwaiter val3;
					if (num != 0)
					{
						if (num == 1)
						{
							val = _003C_003Eu__2;
							_003C_003Eu__2 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
							num = (_003C_003E1__state = -1);
							goto IL_025f;
						}
						directConnectionMyRvLink.UpdateFrequencyMetricForCommandSend(_003CcommandTracker_003E5__2.Command.CommandType);
						ConfiguredTaskAwaitable val2 = directConnectionMyRvLink.SendCommandRawAsync(_003CcommandTracker_003E5__2.Command, cancellationToken).ConfigureAwait(false);
						val3 = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val3)).IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val3;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, _003CResendRunningCommandWaitForResponseAsync_003Ed__98>(ref val3, ref this);
							return;
						}
					}
					else
					{
						val3 = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
					}
					((ConfiguredTaskAwaiter)(ref val3)).GetResult();
					val = _003CcommandTracker_003E5__2.WaitForAnyResponse().GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 1);
						_003C_003Eu__2 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CResendRunningCommandWaitForResponseAsync_003Ed__98>(ref val, ref this);
						return;
					}
					goto IL_025f;
					IL_025f:
					IMyRvLinkCommandResponse result2 = val.GetResult();
					if (result2 is IMyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
					{
						TaggedLog.Warning("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} ResendRunningCommandWaitForResponseAsync Failed with response: {myRvLinkCommandResponseFailure}", global::System.Array.Empty<object>());
					}
					result = result2;
				}
				catch (TimeoutException)
				{
					TaggedLog.Warning("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} ResendRunningCommandWaitForResponseAsync Timeout {_003CcommandTracker_003E5__2.Command}", global::System.Array.Empty<object>());
					result = _003CcommandTracker_003E5__2.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandTimeout);
				}
				catch (OperationCanceledException)
				{
					TaggedLog.Warning("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} ResendRunningCommandWaitForResponseAsync Canceled {_003CcommandTracker_003E5__2.Command}", global::System.Array.Empty<object>());
					result = _003CcommandTracker_003E5__2.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandAborted);
				}
				catch (global::System.Exception ex3)
				{
					TaggedLog.Warning("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} ResendRunningCommandWaitForResponseAsync Failure {_003CcommandTracker_003E5__2.Command}: {ex3.Message}", global::System.Array.Empty<object>());
					result = _003CcommandTracker_003E5__2.TrySetFailure(MyRvLinkCommandResponseFailureCode.Other);
				}
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003CcommandTracker_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003CcommandTracker_003E5__2 = null;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CSendCommandAsync_003Ed__95 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<IMyRvLinkCommandResponse> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public IMyRvLinkCommand command;

		public CancellationToken cancellationToken;

		public TimeSpan commandTimeout;

		public MyRvLinkSendCommandOption commandOption;

		public Action<IMyRvLinkCommandResponse> responseAction;

		private MyRvLinkCommandTracker _003CcommandTracker_003E5__2;

		private BackgroundOperation _003CkeepAliveBackgroundOperation_003E5__3;

		private ConfiguredTaskAwaiter _003C_003Eu__1;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__2;

		private void MoveNext()
		{
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_036d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_0416: Unknown result type (might be due to invalid IL or missing references)
			//IL_041e: Unknown result type (might be due to invalid IL or missing references)
			//IL_047a: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			IMyRvLinkCommandResponse result;
			try
			{
				if (num == 0)
				{
					goto IL_00dd;
				}
				if ((uint)(num - 1) <= 2u)
				{
					goto IL_02f2;
				}
				TaggedLog.Information("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} Adapter SendCommand {command}", global::System.Array.Empty<object>());
				_003CcommandTracker_003E5__2 = null;
				if (command == null)
				{
					TaggedLog.Debug("DirectConnectionMyRvLink", directConnectionMyRvLink.LogPrefix + " SendCommandAsync Failed because given command was NULL.", global::System.Array.Empty<object>());
					result = new MyRvLinkCommandResponseFailure(0, MyRvLinkCommandResponseFailureCode.InvalidCommand);
				}
				else
				{
					if (directConnectionMyRvLink.IsStarted && directConnectionMyRvLink.IsConnected)
					{
						if (command.ClientCommandId == 65535)
						{
							goto IL_00dd;
						}
						object obj = directConnectionMyRvLink._lock;
						bool flag = false;
						try
						{
							Monitor.Enter(obj, ref flag);
							directConnectionMyRvLink.FlushCompletedCommands();
							if (directConnectionMyRvLink._commandActiveDict.ContainsKey((int)command.ClientCommandId))
							{
								TaggedLog.Debug("DirectConnectionMyRvLink", directConnectionMyRvLink.LogPrefix + " SendCommandAsync failed because it is current running use ResendRunningCommandAsync to resend a command.", global::System.Array.Empty<object>());
								result = new MyRvLinkCommandResponseFailure(command.ClientCommandId, MyRvLinkCommandResponseFailureCode.CommandAlreadyRunning);
							}
							else
							{
								if (directConnectionMyRvLink._commandActiveDict.Count + 1 < 20)
								{
									int num2 = (int)((TimeSpan)(ref commandTimeout)).TotalMilliseconds;
									if (((global::System.Enum)commandOption).HasFlag((global::System.Enum)MyRvLinkSendCommandOption.ExtendedWait))
									{
										num2 = Math.Max(num2, 16000);
									}
									_003CcommandTracker_003E5__2 = new MyRvLinkCommandTracker(command, cancellationToken, num2, responseAction);
									directConnectionMyRvLink._commandActiveDict[(int)command.ClientCommandId] = _003CcommandTracker_003E5__2;
									goto end_IL_01e1;
								}
								result = new MyRvLinkCommandResponseFailure(command.ClientCommandId, MyRvLinkCommandResponseFailureCode.Offline);
							}
							goto end_IL_000e;
							end_IL_01e1:;
						}
						finally
						{
							if (num < 0 && flag)
							{
								Monitor.Exit(obj);
							}
						}
						_003CkeepAliveBackgroundOperation_003E5__3 = null;
						goto IL_02f2;
					}
					result = new MyRvLinkCommandResponseFailure(command.ClientCommandId, MyRvLinkCommandResponseFailureCode.Offline);
				}
				goto end_IL_000e;
				IL_00dd:
				ConfiguredTaskAwaitable val;
				ConfiguredTaskAwaiter val2;
				try
				{
					if (num != 0)
					{
						directConnectionMyRvLink.UpdateFrequencyMetricForCommandSend(command.CommandType);
						val = directConnectionMyRvLink.SendCommandRawAsync(command, cancellationToken).ConfigureAwait(false);
						val2 = ((ConfiguredTaskAwaitable)(ref val)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, _003CSendCommandAsync_003Ed__95>(ref val2, ref this);
							return;
						}
					}
					else
					{
						val2 = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
					}
					((ConfiguredTaskAwaiter)(ref val2)).GetResult();
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} SendCommandRawAsync Failed {command}: {ex.Message}", global::System.Array.Empty<object>());
				}
				result = directConnectionMyRvLink._responseSuccessNoResponse;
				goto end_IL_000e;
				IL_02f2:
				try
				{
					TaskAwaiter<IMyRvLinkCommandResponse> val3;
					switch (num)
					{
					default:
						directConnectionMyRvLink.UpdateFrequencyMetricForCommandSend(command.CommandType);
						val = directConnectionMyRvLink.SendCommandRawAsync(command, cancellationToken).ConfigureAwait(false);
						val2 = ((ConfiguredTaskAwaitable)(ref val)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__1 = val2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, _003CSendCommandAsync_003Ed__95>(ref val2, ref this);
							return;
						}
						goto IL_0384;
					case 1:
						val2 = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0384;
					case 2:
						val3 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
						num = (_003C_003E1__state = -1);
						goto IL_042d;
					case 3:
						{
							val3 = _003C_003Eu__2;
							_003C_003Eu__2 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
							num = (_003C_003E1__state = -1);
							break;
						}
						IL_042d:
						result = val3.GetResult();
						goto end_IL_02f2;
						IL_0384:
						((ConfiguredTaskAwaiter)(ref val2)).GetResult();
						if (!((global::System.Enum)commandOption).HasFlag((global::System.Enum)MyRvLinkSendCommandOption.DontWaitForResponse))
						{
							if (((global::System.Enum)commandOption).HasFlag((global::System.Enum)MyRvLinkSendCommandOption.WaitForAnyResponse))
							{
								val3 = _003CcommandTracker_003E5__2.WaitForAnyResponse().GetAwaiter();
								if (!val3.IsCompleted)
								{
									num = (_003C_003E1__state = 2);
									_003C_003Eu__2 = val3;
									_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CSendCommandAsync_003Ed__95>(ref val3, ref this);
									return;
								}
								goto IL_042d;
							}
							val3 = _003CcommandTracker_003E5__2.WaitAsync().GetAwaiter();
							if (!val3.IsCompleted)
							{
								num = (_003C_003E1__state = 3);
								_003C_003Eu__2 = val3;
								_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CSendCommandAsync_003Ed__95>(ref val3, ref this);
								return;
							}
							break;
						}
						result = new MyRvLinkCommandResponseSuccessNoWait(command.ClientCommandId);
						goto end_IL_02f2;
					}
					result = val3.GetResult();
					end_IL_02f2:;
				}
				catch (TimeoutException)
				{
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} SendCommandAsync Timeout {command}", global::System.Array.Empty<object>());
					result = _003CcommandTracker_003E5__2.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandTimeout);
				}
				catch (OperationCanceledException)
				{
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} SendCommandAsync Canceled {command}", global::System.Array.Empty<object>());
					result = _003CcommandTracker_003E5__2.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandAborted);
				}
				catch (global::System.Exception ex4)
				{
					TaggedLog.Warning("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} SendCommandAsync Failure {command}: {ex4.Message}", global::System.Array.Empty<object>());
					result = _003CcommandTracker_003E5__2.TrySetFailure(MyRvLinkCommandResponseFailureCode.Other);
				}
				finally
				{
					if (num < 0)
					{
						BackgroundOperation obj2 = _003CkeepAliveBackgroundOperation_003E5__3;
						if (obj2 != null)
						{
							obj2.Stop();
						}
					}
				}
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003CcommandTracker_003E5__2 = null;
				_003CkeepAliveBackgroundOperation_003E5__3 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003CcommandTracker_003E5__2 = null;
			_003CkeepAliveBackgroundOperation_003E5__3 = null;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CSendCommandAsync_003Ed__96 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<CommandResult> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public Func<ValueTuple<byte, byte>, IMyRvLinkCommand> commandFactory;

		public CancellationToken cancellationToken;

		public MyRvLinkSendCommandOption commandOption;

		public Action<IMyRvLinkCommandResponse> responseAction;

		private IMyRvLinkCommand _003CmyRvLinkCommand_003E5__2;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			CommandResult result;
			try
			{
				try
				{
					TaskAwaiter<IMyRvLinkCommandResponse> val;
					if (num == 0)
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
						num = (_003C_003E1__state = -1);
						goto IL_00dc;
					}
					MyRvLinkDeviceTracker? deviceTracker = directConnectionMyRvLink._deviceTracker;
					if (deviceTracker == null || !deviceTracker.IsLogicalDeviceOnline(logicalDevice))
					{
						result = (CommandResult)6;
					}
					else
					{
						ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
						if (myRvDeviceFromLogicalDevice.HasValue)
						{
							_003CmyRvLinkCommand_003E5__2 = commandFactory.Invoke(myRvDeviceFromLogicalDevice.Value);
							val = directConnectionMyRvLink.SendCommandAsync(_003CmyRvLinkCommand_003E5__2, cancellationToken, commandOption, responseAction).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CSendCommandAsync_003Ed__96>(ref val, ref this);
								return;
							}
							goto IL_00dc;
						}
						result = (CommandResult)6;
					}
					goto end_IL_0011;
					IL_00dc:
					IMyRvLinkCommandResponse result2 = val.GetResult();
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} Sent command {_003CmyRvLinkCommand_003E5__2} received response {result2}", global::System.Array.Empty<object>());
					result = result2.CommandResult;
					end_IL_0011:;
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Warning("DirectConnectionMyRvLink", directConnectionMyRvLink.LogPrefix + " Sending command failed " + ex.Message, global::System.Array.Empty<object>());
					result = (CommandResult)7;
				}
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CSendDirectCommandLeveler1_003Ed__21 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<CommandResult> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDeviceLevelerType1 logicalDevice;

		public LogicalDeviceLevelerCommandType1 command;

		public CancellationToken cancelToken;

		private ValueTuple<byte, byte>? _003CmyRvLinkDevice_003E5__2;

		private long _003CnowTimestampMs_003E5__3;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__2;

		private void MoveNext()
		{
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			CommandResult result;
			try
			{
				if ((uint)num <= 1u)
				{
					goto IL_0059;
				}
				if (!directConnectionMyRvLink.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
				{
					result = (CommandResult)6;
				}
				else
				{
					_003CmyRvLinkDevice_003E5__2 = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice((ILogicalDevice)(object)logicalDevice);
					if (_003CmyRvLinkDevice_003E5__2.HasValue)
					{
						_003CnowTimestampMs_003E5__3 = LogicalDeviceFreeRunningTimer.ElapsedMilliseconds;
						goto IL_0059;
					}
					result = (CommandResult)6;
				}
				goto end_IL_000e;
				IL_0059:
				try
				{
					TaskAwaiter<IMyRvLinkCommandResponse> val;
					TaskAwaiter<bool> val2;
					ValueTuple<ushort, LogicalDeviceLevelerCommandType1, long> lastSentLeveler1Command;
					if (num != 0)
					{
						if (num == 1)
						{
							val = _003C_003Eu__2;
							_003C_003Eu__2 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
							num = (_003C_003E1__state = -1);
							goto IL_0222;
						}
						lastSentLeveler1Command = directConnectionMyRvLink._lastSentLeveler1Command;
						long num2 = _003CnowTimestampMs_003E5__3 - lastSentLeveler1Command.Item3;
						if (lastSentLeveler1Command.Item1 == 0 || lastSentLeveler1Command.Item2 != command || num2 >= 1000 || !ArrayCommon.ArraysEqual<byte>(directConnectionMyRvLink._lastSentLeveler1CommandData, ((LogicalDeviceCommandPacket)command).CopyCurrentData()))
						{
							goto IL_0161;
						}
						directConnectionMyRvLink._lastSentLeveler1Command = new ValueTuple<ushort, LogicalDeviceLevelerCommandType1, long>(lastSentLeveler1Command.Item1, command, _003CnowTimestampMs_003E5__3);
						directConnectionMyRvLink._lastSentLeveler1CommandData = ((LogicalDeviceCommandPacket)command).CopyCurrentData();
						val2 = directConnectionMyRvLink.ResendRunningCommandAsync(lastSentLeveler1Command.Item1, cancelToken).GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CSendDirectCommandLeveler1_003Ed__21>(ref val2, ref this);
							return;
						}
					}
					else
					{
						val2 = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
					}
					if (!val2.GetResult())
					{
						goto IL_0161;
					}
					result = (CommandResult)0;
					goto end_IL_0059;
					IL_0222:
					result = val.GetResult().CommandResult;
					goto end_IL_0059;
					IL_0161:
					ushort nextCommandId = directConnectionMyRvLink.GetNextCommandId();
					lastSentLeveler1Command = (directConnectionMyRvLink._lastSentLeveler1Command = new ValueTuple<ushort, LogicalDeviceLevelerCommandType1, long>(nextCommandId, command, _003CnowTimestampMs_003E5__3));
					directConnectionMyRvLink._lastSentLeveler1CommandData = ((LogicalDeviceCommandPacket)command).CopyCurrentData();
					MyRvLinkCommandLeveler1ButtonCommand myRvLinkCommandLeveler1ButtonCommand = new MyRvLinkCommandLeveler1ButtonCommand(nextCommandId, _003CmyRvLinkDevice_003E5__2.Value.Item1, _003CmyRvLinkDevice_003E5__2.Value.Item2, command);
					val = directConnectionMyRvLink.SendCommandAsync(myRvLinkCommandLeveler1ButtonCommand, cancelToken, MyRvLinkSendCommandOption.DontWaitForResponse).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 1);
						_003C_003Eu__2 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CSendDirectCommandLeveler1_003Ed__21>(ref val, ref this);
						return;
					}
					goto IL_0222;
					end_IL_0059:;
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Warning("DirectConnectionMyRvLink", "Sending command failed " + ex.Message, global::System.Array.Empty<object>());
					result = (CommandResult)7;
				}
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CSendDirectCommandLeveler3_003Ed__23 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<CommandResult> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDeviceLevelerType3 logicalDevice;

		public LogicalDeviceLevelerCommandType3 command;

		public CancellationToken cancelToken;

		private _003C_003Ec__DisplayClass23_0 _003C_003E8__1;

		private ValueTuple<byte, byte>? _003CmyRvLinkDevice_003E5__2;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			CommandResult result;
			try
			{
				if (num == 0)
				{
					goto IL_004d;
				}
				if (!directConnectionMyRvLink.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
				{
					result = (CommandResult)6;
				}
				else
				{
					_003CmyRvLinkDevice_003E5__2 = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice((ILogicalDevice)(object)logicalDevice);
					if (_003CmyRvLinkDevice_003E5__2.HasValue)
					{
						goto IL_004d;
					}
					result = (CommandResult)6;
				}
				goto end_IL_000e;
				IL_004d:
				try
				{
					TaskAwaiter<bool> val;
					if (num == 0)
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_01ca;
					}
					_003C_003E8__1 = new _003C_003Ec__DisplayClass23_0();
					DirectConnectionMyRvLink directConnectionMyRvLink2 = directConnectionMyRvLink;
					bool flag = false;
					try
					{
						Monitor.Enter((object)directConnectionMyRvLink2, ref flag);
						object obj = default(object);
						if (((ILogicalDevice)logicalDevice).CustomData.TryGetValue("DirectConnectionMyRvLink.IDirectCommandLeveler3", ref obj) && obj is MyRvLinkCommandContext<LogicalDeviceLevelerButtonType3> commandContext)
						{
							_003C_003E8__1.commandContext = commandContext;
						}
						else
						{
							((ILogicalDevice)logicalDevice).CustomData["DirectConnectionMyRvLink.IDirectCommandLeveler3"] = (_003C_003E8__1.commandContext = new MyRvLinkCommandContext<LogicalDeviceLevelerButtonType3>());
						}
					}
					finally
					{
						if (num < 0 && flag)
						{
							Monitor.Exit((object)directConnectionMyRvLink2);
						}
					}
					if (!_003C_003E8__1.commandContext.LastSentCommandReceivedError)
					{
						flag = _003C_003E8__1.commandContext.CanResendCommand(command.ButtonsPressed, (IDeviceCommandPacket?)(object)command);
						if (flag)
						{
							val = directConnectionMyRvLink.ResendRunningCommandAsync(_003C_003E8__1.commandContext.SentCommandId, cancelToken).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CSendDirectCommandLeveler3_003Ed__23>(ref val, ref this);
								return;
							}
							goto IL_01ca;
						}
						goto IL_01d3;
					}
					TaggedLog.Warning("DirectConnectionMyRvLink", "Leveler 3 last sent command received an error!", global::System.Array.Empty<object>());
					_003C_003E8__1.commandContext.ClearLastSentCommandReceivedError();
					IMyRvLinkCommandResponseFailure? activeFailure = _003C_003E8__1.commandContext.ActiveFailure;
					result = (CommandResult)((activeFailure == null) ? 7 : ((int)activeFailure.CommandResult));
					goto end_IL_004d;
					IL_01d3:
					if (flag)
					{
						_003C_003E8__1.commandContext.SentCommand(_003C_003E8__1.commandContext.SentCommandId, command.ButtonsPressed, (IDeviceCommandPacket?)(object)command);
						IMyRvLinkCommandResponseFailure? activeFailure2 = _003C_003E8__1.commandContext.ActiveFailure;
						result = (CommandResult)((activeFailure2 != null) ? ((int)activeFailure2.CommandResult) : 0);
					}
					else
					{
						ushort nextCommandId = directConnectionMyRvLink.GetNextCommandId();
						MyRvLinkCommandLeveler3ButtonCommand myRvLinkCommandLeveler3ButtonCommand = new MyRvLinkCommandLeveler3ButtonCommand(nextCommandId, _003CmyRvLinkDevice_003E5__2.Value.Item1, _003CmyRvLinkDevice_003E5__2.Value.Item2, command);
						directConnectionMyRvLink.SendCommandAsync(myRvLinkCommandLeveler3ButtonCommand, cancelToken, TimeSpan.FromMilliseconds(2500.0), MyRvLinkSendCommandOption.DontWaitForResponse, delegate(IMyRvLinkCommandResponse response)
						{
							_003C_003E8__1.commandContext.ProcessResponse(response);
						});
						_003C_003E8__1.commandContext.SentCommand(nextCommandId, command.ButtonsPressed, (IDeviceCommandPacket?)(object)command);
						IMyRvLinkCommandResponseFailure? activeFailure3 = _003C_003E8__1.commandContext.ActiveFailure;
						result = (CommandResult)((activeFailure3 != null) ? ((int)activeFailure3.CommandResult) : 0);
					}
					goto end_IL_004d;
					IL_01ca:
					flag = val.GetResult();
					goto IL_01d3;
					end_IL_004d:;
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Warning("DirectConnectionMyRvLink", "Sending command failed " + ex.Message, global::System.Array.Empty<object>());
					result = (CommandResult)7;
				}
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CSendDirectCommandLeveler4_003Ed__25 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<CommandResult> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDeviceLevelerType4 logicalDevice;

		public ILogicalDeviceLevelerCommandType4 command;

		public CancellationToken cancelToken;

		private _003C_003Ec__DisplayClass25_0 _003C_003E8__1;

		private ValueTuple<byte, byte>? _003CmyRvLinkDevice_003E5__2;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			CommandResult result;
			try
			{
				if (num == 0)
				{
					goto IL_004d;
				}
				if (!directConnectionMyRvLink.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
				{
					result = (CommandResult)6;
				}
				else
				{
					_003CmyRvLinkDevice_003E5__2 = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice((ILogicalDevice)(object)logicalDevice);
					if (_003CmyRvLinkDevice_003E5__2.HasValue)
					{
						goto IL_004d;
					}
					result = (CommandResult)6;
				}
				goto end_IL_000e;
				IL_004d:
				try
				{
					TaskAwaiter<bool> val;
					if (num == 0)
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_01ca;
					}
					_003C_003E8__1 = new _003C_003Ec__DisplayClass25_0();
					DirectConnectionMyRvLink directConnectionMyRvLink2 = directConnectionMyRvLink;
					bool flag = false;
					try
					{
						Monitor.Enter((object)directConnectionMyRvLink2, ref flag);
						object obj = default(object);
						if (((ILogicalDevice)logicalDevice).CustomData.TryGetValue("DirectConnectionMyRvLink.IDirectCommandLeveler4", ref obj) && obj is MyRvLinkCommandContext<LevelerCommandCode> commandContext)
						{
							_003C_003E8__1.commandContext = commandContext;
						}
						else
						{
							((ILogicalDevice)logicalDevice).CustomData["DirectConnectionMyRvLink.IDirectCommandLeveler4"] = (_003C_003E8__1.commandContext = new MyRvLinkCommandContext<LevelerCommandCode>());
						}
					}
					finally
					{
						if (num < 0 && flag)
						{
							Monitor.Exit((object)directConnectionMyRvLink2);
						}
					}
					if (!_003C_003E8__1.commandContext.LastSentCommandReceivedError)
					{
						flag = _003C_003E8__1.commandContext.CanResendCommand(command.Command, (IDeviceCommandPacket?)(object)command);
						if (flag)
						{
							val = directConnectionMyRvLink.ResendRunningCommandAsync(_003C_003E8__1.commandContext.SentCommandId, cancelToken).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CSendDirectCommandLeveler4_003Ed__25>(ref val, ref this);
								return;
							}
							goto IL_01ca;
						}
						goto IL_01d3;
					}
					TaggedLog.Warning("DirectConnectionMyRvLink", "Leveler 4 last sent command received an error!", global::System.Array.Empty<object>());
					_003C_003E8__1.commandContext.ClearLastSentCommandReceivedError();
					IMyRvLinkCommandResponseFailure? activeFailure = _003C_003E8__1.commandContext.ActiveFailure;
					result = (CommandResult)((activeFailure == null) ? 7 : ((int)activeFailure.CommandResult));
					goto end_IL_004d;
					IL_01d3:
					if (flag)
					{
						_003C_003E8__1.commandContext.SentCommand(_003C_003E8__1.commandContext.SentCommandId, command.Command, (IDeviceCommandPacket?)(object)command);
						IMyRvLinkCommandResponseFailure? activeFailure2 = _003C_003E8__1.commandContext.ActiveFailure;
						result = (CommandResult)((activeFailure2 != null) ? ((int)activeFailure2.CommandResult) : 0);
					}
					else
					{
						ushort nextCommandId = directConnectionMyRvLink.GetNextCommandId();
						MyRvLinkCommandLeveler4ButtonCommand myRvLinkCommandLeveler4ButtonCommand = new MyRvLinkCommandLeveler4ButtonCommand(nextCommandId, _003CmyRvLinkDevice_003E5__2.Value.Item1, _003CmyRvLinkDevice_003E5__2.Value.Item2, command);
						directConnectionMyRvLink.SendCommandAsync(myRvLinkCommandLeveler4ButtonCommand, cancelToken, TimeSpan.FromMilliseconds(2500.0), MyRvLinkSendCommandOption.DontWaitForResponse, delegate(IMyRvLinkCommandResponse response)
						{
							_003C_003E8__1.commandContext.ProcessResponse(response);
						});
						_003C_003E8__1.commandContext.SentCommand(nextCommandId, command.Command, (IDeviceCommandPacket?)(object)command);
						IMyRvLinkCommandResponseFailure? activeFailure3 = _003C_003E8__1.commandContext.ActiveFailure;
						result = (CommandResult)((activeFailure3 != null) ? ((int)activeFailure3.CommandResult) : 0);
					}
					goto end_IL_004d;
					IL_01ca:
					flag = val.GetResult();
					goto IL_01d3;
					end_IL_004d:;
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Warning("DirectConnectionMyRvLink", "Sending command failed " + ex.Message, global::System.Array.Empty<object>());
					result = (CommandResult)7;
				}
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CSendDirectCommandLeveler5Async_003Ed__134 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<LevelerCommandResultType5> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDeviceLevelerType5 logicalDevice;

		public ILogicalDeviceLevelerCommandType5 command;

		public CancellationToken cancelToken;

		private ValueTuple<byte, byte>? _003CmyRvLinkDevice_003E5__2;

		private MyRvLinkCommandContext<LevelerCommandCode> _003CcommandContext_003E5__3;

		private ushort _003CcommandId_003E5__4;

		private ConfiguredTaskAwaiter<IMyRvLinkCommandResponse?> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0492: Unknown result type (might be due to invalid IL or missing references)
			//IL_0497: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_0466: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			LevelerCommandResultType5 result;
			try
			{
				if ((uint)num <= 1u)
				{
					goto IL_0064;
				}
				MyRvLinkDeviceTracker? deviceTracker = directConnectionMyRvLink._deviceTracker;
				if (deviceTracker == null || !deviceTracker.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
				{
					result = new LevelerCommandResultType5((CommandResult)6);
				}
				else
				{
					_003CmyRvLinkDevice_003E5__2 = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice((ILogicalDevice)(object)logicalDevice);
					if (_003CmyRvLinkDevice_003E5__2.HasValue)
					{
						goto IL_0064;
					}
					result = new LevelerCommandResultType5((CommandResult)6);
				}
				goto end_IL_000e;
				IL_0064:
				try
				{
					bool flag;
					ConfiguredTaskAwaiter<IMyRvLinkCommandResponse> val;
					if (num != 0)
					{
						if (num == 1)
						{
							val = _003C_003Eu__1;
							_003C_003Eu__1 = default(ConfiguredTaskAwaiter<IMyRvLinkCommandResponse>);
							num = (_003C_003E1__state = -1);
							goto IL_02fc;
						}
						DirectConnectionMyRvLink directConnectionMyRvLink2 = directConnectionMyRvLink;
						flag = false;
						try
						{
							Monitor.Enter((object)directConnectionMyRvLink2, ref flag);
							object obj = default(object);
							if (((ILogicalDevice)logicalDevice).CustomData.TryGetValue("DirectConnectionMyRvLink.IDirectCommandLeveler5", ref obj) && obj is MyRvLinkCommandContext<LevelerCommandCode> myRvLinkCommandContext)
							{
								_003CcommandContext_003E5__3 = myRvLinkCommandContext;
							}
							else
							{
								((ILogicalDevice)logicalDevice).CustomData["DirectConnectionMyRvLink.IDirectCommandLeveler5"] = (_003CcommandContext_003E5__3 = new MyRvLinkCommandContext<LevelerCommandCode>());
							}
						}
						finally
						{
							if (num < 0 && flag)
							{
								Monitor.Exit((object)directConnectionMyRvLink2);
							}
						}
						if (_003CcommandContext_003E5__3.LastSentCommandReceivedError)
						{
							_003CcommandContext_003E5__3.ClearLastSentCommandReceivedError();
						}
						flag = _003CcommandContext_003E5__3.CanResendCommand(command.Command, (IDeviceCommandPacket?)(object)command);
						if (!flag)
						{
							goto IL_01a4;
						}
						val = directConnectionMyRvLink.ResendRunningCommandWaitForResponseAsync(_003CcommandContext_003E5__3.SentCommandId, cancelToken).ConfigureAwait(false).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<IMyRvLinkCommandResponse>, _003CSendDirectCommandLeveler5Async_003Ed__134>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(ConfiguredTaskAwaiter<IMyRvLinkCommandResponse>);
						num = (_003C_003E1__state = -1);
					}
					IMyRvLinkCommandResponse result2 = val.GetResult();
					flag = result2 != null;
					goto IL_01a4;
					IL_02fc:
					IMyRvLinkCommandResponse result3 = val.GetResult();
					if (!(result3 is IMyRvLinkCommandResponseSuccess))
					{
						if (!(result3 is MyRvLinkCommandLeveler5ResponseFailure myRvLinkCommandLeveler5ResponseFailure))
						{
							if (result3 is IMyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
							{
								_003CcommandContext_003E5__3.ProcessResponse(result3);
								TaggedLog.Information("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} - Leveler 5 command failure generic {myRvLinkCommandResponseFailure}", global::System.Array.Empty<object>());
								result = new LevelerCommandResultType5((CommandResult)7);
							}
							else
							{
								_003CcommandContext_003E5__3.ProcessResponse(new MyRvLinkCommandResponseFailure(_003CcommandId_003E5__4, MyRvLinkCommandResponseFailureCode.CommandFailed));
								TaggedLog.Information("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} - Leveler 5 command failure unknown {result3}", global::System.Array.Empty<object>());
								result = new LevelerCommandResultType5((CommandResult)7);
							}
						}
						else
						{
							_003CcommandContext_003E5__3.ProcessResponse(result3);
							TaggedLog.Information("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} - Leveler 5 command failure {myRvLinkCommandLeveler5ResponseFailure}", global::System.Array.Empty<object>());
							result = new LevelerCommandResultType5((CommandResult)7, myRvLinkCommandLeveler5ResponseFailure.LevelerFault);
						}
					}
					else
					{
						_003CcommandContext_003E5__3.ProcessResponse(result3);
						result = new LevelerCommandResultType5((CommandResult)0);
					}
					goto end_IL_0064;
					IL_01a4:
					if (!flag)
					{
						_003CcommandId_003E5__4 = directConnectionMyRvLink.GetNextCommandId();
						MyRvLinkCommandLeveler5 myRvLinkCommandLeveler = new MyRvLinkCommandLeveler5(_003CcommandId_003E5__4, _003CmyRvLinkDevice_003E5__2.Value.Item1, _003CmyRvLinkDevice_003E5__2.Value.Item2, command);
						_003CcommandContext_003E5__3.SentCommand(_003CcommandId_003E5__4, command.Command, (IDeviceCommandPacket?)(object)command);
						val = directConnectionMyRvLink.SendCommandAsync(myRvLinkCommandLeveler, cancelToken, TimeSpan.FromMilliseconds(2500.0), MyRvLinkSendCommandOption.WaitForAnyResponse).ConfigureAwait(false).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<IMyRvLinkCommandResponse>, _003CSendDirectCommandLeveler5Async_003Ed__134>(ref val, ref this);
							return;
						}
						goto IL_02fc;
					}
					_003CcommandContext_003E5__3.SentCommand(_003CcommandContext_003E5__3.SentCommandId, command.Command, (IDeviceCommandPacket?)(object)command);
					LevelerCommandResultType5 val2 = ((result2 is IMyRvLinkCommandResponseSuccess) ? new LevelerCommandResultType5((CommandResult)0) : ((result2 is MyRvLinkCommandLeveler5ResponseFailure myRvLinkCommandLeveler5ResponseFailure2) ? new LevelerCommandResultType5((CommandResult)7, myRvLinkCommandLeveler5ResponseFailure2.LevelerFault) : ((!(result2 is IMyRvLinkCommandResponseFailure)) ? new LevelerCommandResultType5((CommandResult)7) : new LevelerCommandResultType5((CommandResult)7))));
					result = val2;
					end_IL_0064:;
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Warning("DirectConnectionMyRvLink", directConnectionMyRvLink.LogPrefix + " Sending command failed " + ex.Message, global::System.Array.Empty<object>());
					result = new LevelerCommandResultType5((CommandResult)7);
				}
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CSendDirectCommandRelayMomentary_003Ed__175 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<CommandResult> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDeviceRelayHBridge logicalDevice;

		public HBridgeCommand command;

		public CancellationToken cancelToken;

		private CommandResult _003Cresult_003E5__2;

		private global::System.Collections.Generic.IEnumerator<ILogicalDeviceSourceCommandMonitor> _003C_003E7__wrap2;

		private TaskAwaiter _003C_003Eu__1;

		private TaskAwaiter<CommandResult> _003C_003Eu__2;

		private void MoveNext()
		{
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			CommandResult result2;
			try
			{
				TaskAwaiter val2;
				TaskAwaiter<CommandResult> val;
				CommandResult result;
				switch (num)
				{
				default:
					_003C_003E7__wrap2 = directConnectionMyRvLink.CommandMonitors.GetEnumerator();
					goto case 0;
				case 0:
					try
					{
						if (num != 0)
						{
							goto IL_00bf;
						}
						val2 = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_00b8;
						IL_00b8:
						((TaskAwaiter)(ref val2)).GetResult();
						goto IL_00bf;
						IL_00bf:
						while (((global::System.Collections.IEnumerator)_003C_003E7__wrap2).MoveNext())
						{
							ILogicalDeviceSourceCommandMonitor current = _003C_003E7__wrap2.Current;
							ILogicalDeviceSourceCommandMonitorMovement val3 = (ILogicalDeviceSourceCommandMonitorMovement)(object)((current is ILogicalDeviceSourceCommandMonitorMovement) ? current : null);
							if (val3 == null)
							{
								continue;
							}
							val2 = val3.WillSendCommandRelayMomentaryAsync((ILogicalDeviceSource)(object)directConnectionMyRvLink, logicalDevice, command, cancelToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val2)).IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val2;
								_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CSendDirectCommandRelayMomentary_003Ed__175>(ref val2, ref this);
								return;
							}
							goto IL_00b8;
						}
					}
					finally
					{
						if (num < 0 && _003C_003E7__wrap2 != null)
						{
							((global::System.IDisposable)_003C_003E7__wrap2).Dispose();
						}
					}
					_003C_003E7__wrap2 = null;
					val = directConnectionMyRvLink.SendDirectCommandRelayMomentaryImpl(logicalDevice, command, cancelToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 1);
						_003C_003Eu__2 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CommandResult>, _003CSendDirectCommandRelayMomentary_003Ed__175>(ref val, ref this);
						return;
					}
					goto IL_0159;
				case 1:
					val = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter<CommandResult>);
					num = (_003C_003E1__state = -1);
					goto IL_0159;
				case 2:
					break;
					IL_0159:
					result = val.GetResult();
					_003Cresult_003E5__2 = result;
					_003C_003E7__wrap2 = directConnectionMyRvLink.CommandMonitors.GetEnumerator();
					break;
				}
				try
				{
					if (num != 2)
					{
						goto IL_0213;
					}
					val2 = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_020c;
					IL_020c:
					((TaskAwaiter)(ref val2)).GetResult();
					goto IL_0213;
					IL_0213:
					while (((global::System.Collections.IEnumerator)_003C_003E7__wrap2).MoveNext())
					{
						ILogicalDeviceSourceCommandMonitor current2 = _003C_003E7__wrap2.Current;
						ILogicalDeviceSourceCommandMonitorMovement val4 = (ILogicalDeviceSourceCommandMonitorMovement)(object)((current2 is ILogicalDeviceSourceCommandMonitorMovement) ? current2 : null);
						if (val4 == null)
						{
							continue;
						}
						val2 = val4.DidSendCommandRelayMomentaryAsync((ILogicalDeviceSource)(object)directConnectionMyRvLink, logicalDevice, command, _003Cresult_003E5__2, cancelToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (_003C_003E1__state = 2);
							_003C_003Eu__1 = val2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CSendDirectCommandRelayMomentary_003Ed__175>(ref val2, ref this);
							return;
						}
						goto IL_020c;
					}
				}
				finally
				{
					if (num < 0 && _003C_003E7__wrap2 != null)
					{
						((global::System.IDisposable)_003C_003E7__wrap2).Dispose();
					}
				}
				_003C_003E7__wrap2 = null;
				result2 = _003Cresult_003E5__2;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result2);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CSendDirectCommandRelayMomentaryImpl_003Ed__176 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<CommandResult> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDeviceRelayHBridge logicalDevice;

		public HBridgeCommand command;

		public CancellationToken cancelToken;

		private _003C_003Ec__DisplayClass176_0 _003C_003E8__1;

		private ValueTuple<byte, byte>? _003CmyRvLinkDevice_003E5__2;

		private TaskAwaiter<bool> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			CommandResult result;
			try
			{
				if (num == 0)
				{
					goto IL_0059;
				}
				MyRvLinkDeviceTracker? deviceTracker = directConnectionMyRvLink._deviceTracker;
				if (deviceTracker == null || !deviceTracker.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
				{
					result = (CommandResult)6;
				}
				else
				{
					_003CmyRvLinkDevice_003E5__2 = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice((ILogicalDevice)(object)logicalDevice);
					if (_003CmyRvLinkDevice_003E5__2.HasValue)
					{
						goto IL_0059;
					}
					result = (CommandResult)6;
				}
				goto end_IL_000e;
				IL_0059:
				try
				{
					TaskAwaiter<bool> val;
					if (num == 0)
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_01cc;
					}
					_003C_003E8__1 = new _003C_003Ec__DisplayClass176_0();
					DirectConnectionMyRvLink directConnectionMyRvLink2 = directConnectionMyRvLink;
					bool flag = false;
					try
					{
						Monitor.Enter((object)directConnectionMyRvLink2, ref flag);
						object obj = default(object);
						if (((ILogicalDevice)logicalDevice).CustomData.TryGetValue("DirectConnectionMyRvLink.IDirectCommandMovement", ref obj) && obj is MyRvLinkCommandContext<HBridgeCommand> commandContext)
						{
							_003C_003E8__1.commandContext = commandContext;
						}
						else
						{
							((ILogicalDevice)logicalDevice).CustomData["DirectConnectionMyRvLink.IDirectCommandMovement"] = (_003C_003E8__1.commandContext = new MyRvLinkCommandContext<HBridgeCommand>());
						}
					}
					finally
					{
						if (num < 0 && flag)
						{
							Monitor.Exit((object)directConnectionMyRvLink2);
						}
					}
					if (!_003C_003E8__1.commandContext.LastSentCommandReceivedError)
					{
						flag = _003C_003E8__1.commandContext.CanResendCommand(command);
						if (flag)
						{
							val = directConnectionMyRvLink.ResendRunningCommandAsync(_003C_003E8__1.commandContext.SentCommandId, cancelToken).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CSendDirectCommandRelayMomentaryImpl_003Ed__176>(ref val, ref this);
								return;
							}
							goto IL_01cc;
						}
						goto IL_01d5;
					}
					TaggedLog.Warning("DirectConnectionMyRvLink", "{LogPrefix} Momentary relay last sent command received an error!", global::System.Array.Empty<object>());
					_003C_003E8__1.commandContext.ClearLastSentCommandReceivedError();
					IMyRvLinkCommandResponseFailure? activeFailure = _003C_003E8__1.commandContext.ActiveFailure;
					result = (CommandResult)((activeFailure == null) ? 7 : ((int)activeFailure.CommandResult));
					goto end_IL_0059;
					IL_01d5:
					if (flag)
					{
						_003C_003E8__1.commandContext.SentCommand(_003C_003E8__1.commandContext.SentCommandId, command);
						IMyRvLinkCommandResponseFailure? activeFailure2 = _003C_003E8__1.commandContext.ActiveFailure;
						result = (CommandResult)((activeFailure2 != null) ? ((int)activeFailure2.CommandResult) : 0);
					}
					else
					{
						ushort nextCommandId = directConnectionMyRvLink.GetNextCommandId();
						MyRvLinkCommandActionMovement myRvLinkCommandActionMovement = new MyRvLinkCommandActionMovement(nextCommandId, _003CmyRvLinkDevice_003E5__2.Value.Item1, _003CmyRvLinkDevice_003E5__2.Value.Item2, ((ILogicalDevice)logicalDevice).LogicalId, command);
						directConnectionMyRvLink.SendCommandAsync(myRvLinkCommandActionMovement, cancelToken, TimeSpan.FromMilliseconds(2500.0), MyRvLinkSendCommandOption.DontWaitForResponse, delegate(IMyRvLinkCommandResponse response)
						{
							_003C_003E8__1.commandContext.ProcessResponse(response);
						});
						_003C_003E8__1.commandContext.SentCommand(nextCommandId, command);
						IMyRvLinkCommandResponseFailure? activeFailure3 = _003C_003E8__1.commandContext.ActiveFailure;
						result = (CommandResult)((activeFailure3 != null) ? ((int)activeFailure3.CommandResult) : 0);
					}
					goto end_IL_0059;
					IL_01cc:
					flag = val.GetResult();
					goto IL_01d5;
					end_IL_0059:;
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Warning("DirectConnectionMyRvLink", directConnectionMyRvLink.LogPrefix + " Sending command failed " + ex.Message, global::System.Array.Empty<object>());
					result = (CommandResult)7;
				}
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CSetRealTimeClockTimeAsync_003Ed__179 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public global::System.DateTime dateTime;

		public CancellationToken cancellationToken;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			bool result2;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsStarted || !directConnectionMyRvLink.IsConnected)
					{
						throw new MyRvLinkException("Unable to set RTC because DirectConnectionMyRvLink isn't started or connected");
					}
					if (directConnectionMyRvLink._firmwareUpdateInProgress)
					{
						throw new MyRvLinkPidWriteException("Can't perform Pid writes while a firmware update is in progress!");
					}
					MyRvLinkCommandSetRealTimeClock command = new MyRvLinkCommandSetRealTimeClock(directConnectionMyRvLink.GetNextCommandId(), dateTime);
					val = directConnectionMyRvLink.SendCommandAsync(command, cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CSetRealTimeClockTimeAsync_003Ed__179>(ref val, ref this);
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
				if (result is IMyRvLinkCommandResponseFailure failure)
				{
					throw new MyRvLinkCommandResponseFailureException(failure);
				}
				if (!(result is IMyRvLinkCommandResponseSuccess))
				{
					throw new MyRvLinkException("Failed to set RTC: Unknown result");
				}
				result2 = true;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result2);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CStartDeviceBlockTransferAsync_003Ed__13 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public BlockTransferBlockId blockId;

		public BlockTransferStartOptions options;

		public uint? startAddress;

		public uint? size;

		public CancellationToken cancellationToken;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
					}
					ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!myRvDeviceFromLogicalDevice.HasValue)
					{
						throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
					}
					MyRvLinkCommandStartDeviceBlockTransfer command = new MyRvLinkCommandStartDeviceBlockTransfer(directConnectionMyRvLink.GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, options, startAddress, size);
					val = directConnectionMyRvLink.SendCommandAsync(command, cancellationToken, MyRvLinkSendCommandOption.ExtendedWait).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CStartDeviceBlockTransferAsync_003Ed__13>(ref val, ref this);
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
				if ((int)result.CommandResult != 0)
				{
					throw new MyRvLinkException($"Failed to start block transfer, CommandResult: {result.CommandResult}");
				}
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

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CStopDeviceBlockTransferAsync_003Ed__14 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public BlockTransferBlockId blockId;

		public BlockTransferStopOptions options;

		public CancellationToken cancellationToken;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			try
			{
				TaskAwaiter<IMyRvLinkCommandResponse> val;
				if (num != 0)
				{
					if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
					}
					ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = directConnectionMyRvLink.GetMyRvDeviceFromLogicalDevice(logicalDevice);
					if (!myRvDeviceFromLogicalDevice.HasValue)
					{
						throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
					}
					MyRvLinkCommandStopDeviceBlockTransfer command = new MyRvLinkCommandStopDeviceBlockTransfer(directConnectionMyRvLink.GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, options);
					val = directConnectionMyRvLink.SendCommandAsync(command, cancellationToken, MyRvLinkSendCommandOption.ExtendedWait).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CStopDeviceBlockTransferAsync_003Ed__14>(ref val, ref this);
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
				if ((int)result.CommandResult != 0)
				{
					throw new MyRvLinkException($"Failed to start block transfer, CommandResult: {result.CommandResult}");
				}
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

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CTryGetFirmwareUpdateSupportAsync_003Ed__158 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<FirmwareUpdateSupport> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDevice logicalDevice;

		public CancellationToken cancelToken;

		private TaskAwaiter<global::System.Collections.Generic.IReadOnlyList<BlockTransferBlockId>> _003C_003Eu__1;

		private TaskAwaiter<BlockTransferPropertyFlags> _003C_003Eu__2;

		private void MoveNext()
		{
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			FirmwareUpdateSupport result;
			try
			{
				_ = 1;
				try
				{
					TaskAwaiter<global::System.Collections.Generic.IReadOnlyList<BlockTransferBlockId>> val;
					if (num == 0)
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<global::System.Collections.Generic.IReadOnlyList<BlockTransferBlockId>>);
						num = (_003C_003E1__state = -1);
						goto IL_0130;
					}
					TaskAwaiter<BlockTransferPropertyFlags> val2;
					if (num == 1)
					{
						val2 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<BlockTransferPropertyFlags>);
						num = (_003C_003E1__state = -1);
						goto IL_01aa;
					}
					if (!directConnectionMyRvLink.IsStarted)
					{
						throw new MyRvLinkDeviceServiceNotStartedException(directConnectionMyRvLink, "Device Source Not Started");
					}
					if (!directConnectionMyRvLink.IsConnected)
					{
						throw new MyRvLinkDeviceServiceNotConnectedException(directConnectionMyRvLink, "Device Source Not Connected");
					}
					if (logicalDevice is ILocapOtaAccessoryDevice)
					{
						TaggedLog.Information("DirectConnectionMyRvLink", $"ILocapOtaAccessory device found, updating not supported through RvLink Device Source. Logical device: {logicalDevice}", global::System.Array.Empty<object>());
						result = (FirmwareUpdateSupport)4;
					}
					else if (!directConnectionMyRvLink.IsLogicalDeviceOnline(logicalDevice))
					{
						result = (FirmwareUpdateSupport)3;
					}
					else
					{
						ILogicalDevice obj = logicalDevice;
						ILogicalDeviceJumpToBootloader val3 = (ILogicalDeviceJumpToBootloader)(object)((obj is ILogicalDeviceJumpToBootloader) ? obj : null);
						if (val3 == null || !val3.IsJumpToBootRequiredForFirmwareUpdate)
						{
							val = directConnectionMyRvLink.GetDeviceBlockListAsync(logicalDevice, cancelToken).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<global::System.Collections.Generic.IReadOnlyList<BlockTransferBlockId>>, _003CTryGetFirmwareUpdateSupportAsync_003Ed__158>(ref val, ref this);
								return;
							}
							goto IL_0130;
						}
						result = (FirmwareUpdateSupport)2;
					}
					goto end_IL_0013;
					IL_0130:
					if (Enumerable.Contains<BlockTransferBlockId>((global::System.Collections.Generic.IEnumerable<BlockTransferBlockId>)val.GetResult(), (BlockTransferBlockId)3))
					{
						val2 = directConnectionMyRvLink.GetDeviceBlockPropertyFlagsAsync(logicalDevice, (BlockTransferBlockId)3, cancelToken).GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__2 = val2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<BlockTransferPropertyFlags>, _003CTryGetFirmwareUpdateSupportAsync_003Ed__158>(ref val2, ref this);
							return;
						}
						goto IL_01aa;
					}
					result = (FirmwareUpdateSupport)4;
					goto end_IL_0013;
					IL_01aa:
					BlockTransferPropertyFlags result2 = val2.GetResult();
					result = ((!((global::System.Enum)result2).HasFlag((global::System.Enum)(object)(BlockTransferPropertyFlags)8)) ? ((FirmwareUpdateSupport)4) : (((global::System.Enum)result2).HasFlag((global::System.Enum)(object)(BlockTransferPropertyFlags)16) ? ((FirmwareUpdateSupport)1) : ((FirmwareUpdateSupport)4)));
					end_IL_0013:;
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Warning("DirectConnectionMyRvLink", "Unable to determine if firmware update is supported: " + ex.Message, global::System.Array.Empty<object>());
					result = (FirmwareUpdateSupport)0;
				}
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CTryRemoveRefreshBootLoaderWhenOfflineAsync_003Ed__164 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public ILogicalDeviceFirmwareUpdateDevice logicalDeviceToReflash;

		public DirectConnectionMyRvLink _003C_003E4__this;

		private _003C_003Ec__DisplayClass164_0 _003C_003E8__1;

		public CancellationToken cancellationToken;

		private bool _003CisOnline_003E5__2;

		private global::System.DateTime _003CstartTime_003E5__3;

		private TaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			bool result;
			try
			{
				if (num != 0)
				{
					_003C_003E8__1 = new _003C_003Ec__DisplayClass164_0();
					_003C_003E8__1.logicalDeviceToReflash = logicalDeviceToReflash;
					_003CisOnline_003E5__2 = true;
				}
				try
				{
					TaskAwaiter val;
					if (num == 0)
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_00eb;
					}
					if (DEVICE_TYPE.op_Implicit(((ILogicalDevice)_003C_003E8__1.logicalDeviceToReflash).LogicalId.DeviceType) == 50)
					{
						_003CstartTime_003E5__3 = global::System.DateTime.Now;
						goto IL_006b;
					}
					result = false;
					goto end_IL_0035;
					IL_00eb:
					((TaskAwaiter)(ref val)).GetResult();
					if (!_003CisOnline_003E5__2)
					{
						TimeSpan val2 = global::System.DateTime.Now - _003CstartTime_003E5__3;
						if (!(((TimeSpan)(ref val2)).TotalMilliseconds < 30000.0) && ((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
						{
							goto IL_0134;
						}
					}
					goto IL_006b;
					IL_006b:
					_003CisOnline_003E5__2 = directConnectionMyRvLink.IsLogicalDeviceOnline((ILogicalDevice?)(object)_003C_003E8__1.logicalDeviceToReflash);
					if (_003CisOnline_003E5__2)
					{
						val = global::System.Threading.Tasks.Task.Delay(1000, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CTryRemoveRefreshBootLoaderWhenOfflineAsync_003Ed__164>(ref val, ref this);
							return;
						}
						goto IL_00eb;
					}
					goto IL_0134;
					IL_0134:
					if (_003CisOnline_003E5__2 || ((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
					{
						result = false;
					}
					else
					{
						ILogicalDeviceManager deviceManager = ((ILogicalDevice)_003C_003E8__1.logicalDeviceToReflash).DeviceService.DeviceManager;
						if (deviceManager != null)
						{
							deviceManager.RemoveLogicalDevice((Func<ILogicalDevice, bool>)((ILogicalDevice d) => (object)d == _003C_003E8__1.logicalDeviceToReflash));
						}
						result = true;
					}
					end_IL_0035:;
				}
				catch (global::System.Exception ex)
				{
					ILogicalDeviceFirmwareUpdateDevice obj = _003C_003E8__1.logicalDeviceToReflash;
					TaggedLog.Error("DirectConnectionMyRvLink", "Unable to Remove Device " + ((obj != null) ? ((IDevicesCommon)obj).DeviceName : null) + ". " + ex.Message, global::System.Array.Empty<object>());
					result = false;
				}
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003E8__1 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003E8__1 = null;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CTrySwitchAllMasterControllable_003Ed__191 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public bool allOn;

		public global::System.Collections.Generic.IEnumerable<ILogicalDevice> logicalDeviceList;

		public CancellationToken cancellationToken;

		private string _003CoperationText_003E5__2;

		private MyRvLinkCommandActionSwitch _003Ccommand_003E5__3;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			bool result;
			try
			{
				if (num == 0)
				{
					goto IL_0260;
				}
				MyRvLinkDeviceTracker deviceTracker = default(MyRvLinkDeviceTracker);
				List<byte> val = default(List<byte>);
				if (!directConnectionMyRvLink.IsStarted || !directConnectionMyRvLink.IsConnected)
				{
					result = false;
				}
				else
				{
					_003CoperationText_003E5__2 = (allOn ? "On" : "Off");
					TaggedLog.Debug("DirectConnectionMyRvLink", directConnectionMyRvLink.LogPrefix + " All Lights " + _003CoperationText_003E5__2, global::System.Array.Empty<object>());
					if (!directConnectionMyRvLink.IsConnected)
					{
						TaggedLog.Debug("DirectConnectionMyRvLink", directConnectionMyRvLink.LogPrefix + " Unable to Turn " + _003CoperationText_003E5__2 + " All lights because not connected", global::System.Array.Empty<object>());
						result = false;
					}
					else
					{
						deviceTracker = directConnectionMyRvLink._deviceTracker;
						if (deviceTracker != null)
						{
							global::System.Collections.Generic.IEnumerable<ILogicalDeviceSwitchable> enumerable = Enumerable.Where<ILogicalDeviceSwitchable>(Enumerable.OfType<ILogicalDeviceSwitchable>((global::System.Collections.IEnumerable)logicalDeviceList), (Func<ILogicalDeviceSwitchable, bool>)((ILogicalDeviceSwitchable switchable) => ((ILogicalDeviceSwitchableReadonly)switchable).IsMasterSwitchControllable));
							val = new List<byte>();
							global::System.Collections.Generic.IEnumerator<ILogicalDeviceSwitchable> enumerator = enumerable.GetEnumerator();
							try
							{
								while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
								{
									ILogicalDeviceSwitchable current = enumerator.Current;
									MyRvLinkDeviceTracker? deviceTracker2 = directConnectionMyRvLink._deviceTracker;
									if (deviceTracker2 == null || !deviceTracker2.IsLogicalDeviceOnline((ILogicalDevice?)(object)current))
									{
										TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} Unable to Turn {_003CoperationText_003E5__2} Light {current} because it isn't online", global::System.Array.Empty<object>());
										continue;
									}
									byte? myRvDeviceIdFromLogicalDevice = deviceTracker.GetMyRvDeviceIdFromLogicalDevice((ILogicalDevice)(object)current);
									if (!myRvDeviceIdFromLogicalDevice.HasValue)
									{
										TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} Unable to Turn {_003CoperationText_003E5__2} Light {current} because it isn't associated with {"DirectConnectionMyRvLink"}", global::System.Array.Empty<object>());
									}
									else
									{
										val.Add(myRvDeviceIdFromLogicalDevice.Value);
									}
								}
							}
							finally
							{
								if (num < 0)
								{
									((global::System.IDisposable)enumerator)?.Dispose();
								}
							}
							goto IL_0260;
						}
						TaggedLog.Debug("DirectConnectionMyRvLink", directConnectionMyRvLink.LogPrefix + " Unable to Turn " + _003CoperationText_003E5__2 + " All lights because devices not yet loaded", global::System.Array.Empty<object>());
						result = false;
					}
				}
				goto end_IL_000e;
				IL_0260:
				try
				{
					TaskAwaiter<IMyRvLinkCommandResponse> val2;
					if (num == 0)
					{
						val2 = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
						num = (_003C_003E1__state = -1);
						goto IL_0305;
					}
					if (val.Count != 0)
					{
						_003Ccommand_003E5__3 = new MyRvLinkCommandActionSwitch(directConnectionMyRvLink.GetNextCommandId(), deviceTracker.DeviceTableId, allOn ? MyRvLinkCommandActionSwitchState.On : MyRvLinkCommandActionSwitchState.Off, val.ToArray());
						val2 = directConnectionMyRvLink.SendCommandAsync(_003Ccommand_003E5__3, cancellationToken).GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CTrySwitchAllMasterControllable_003Ed__191>(ref val2, ref this);
							return;
						}
						goto IL_0305;
					}
					result = false;
					goto end_IL_0260;
					IL_0305:
					IMyRvLinkCommandResponse result2 = val2.GetResult();
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} TrySwitchAllMasterControllable Completed for\n{_003Ccommand_003E5__3}", global::System.Array.Empty<object>());
					if ((int)result2.CommandResult != 0)
					{
						throw new MyRvLinkException("Failed to turn all lights " + _003CoperationText_003E5__2);
					}
					result = true;
					end_IL_0260:;
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} Unable to Turn {_003CoperationText_003E5__2} Lights: {ex.Message}", global::System.Array.Empty<object>());
					result = false;
				}
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003CoperationText_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003CoperationText_003E5__2 = null;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CUpdateFirmwareAsync_003Ed__159 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public ILogicalDeviceFirmwareUpdateSession firmwareUpdateSession;

		public IReadOnlyDictionary<FirmwareUpdateOption, object> options;

		public CancellationToken cancellationToken;

		public global::System.Collections.Generic.IReadOnlyList<byte> data;

		public Func<ILogicalDeviceTransferProgress, bool> progressAck;

		private ILogicalDeviceJumpToBootloader _003CjumpToBootLogicalDevice_003E5__2;

		private TaskAwaiter _003C_003Eu__1;

		private TaskAwaiter<ILogicalDeviceReflashBootloader> _003C_003Eu__2;

		private void MoveNext()
		{
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			try
			{
				ILogicalDeviceFirmwareUpdateDevice val3;
				TaskAwaiter val;
				TaskAwaiter<ILogicalDeviceReflashBootloader> val2;
				TimeSpan val4 = default(TimeSpan);
				switch (num)
				{
				default:
					if (!directConnectionMyRvLink.IsStarted)
					{
						TaggedLog.Warning("DirectConnectionMyRvLink", "Unable to Update Firmware because Logical Device Service Not Started DirectConnectionMyRvLink", global::System.Array.Empty<object>());
						throw new MyRvLinkDeviceServiceNotStartedException(directConnectionMyRvLink, "Unable to Update Firmware");
					}
					if (!directConnectionMyRvLink.IsConnected)
					{
						TaggedLog.Warning("DirectConnectionMyRvLink", "Unable to Update Firmware because Logical Device Service Not Connected DirectConnectionMyRvLink", global::System.Array.Empty<object>());
						throw new MyRvLinkDeviceServiceNotConnectedException(directConnectionMyRvLink, "Unable to Update Firmware");
					}
					if (((ICommonDisposable)firmwareUpdateSession).IsDisposed)
					{
						TaggedLog.Warning("DirectConnectionMyRvLink", "Unable to Update Firmware because update session is disposed DirectConnectionMyRvLink", global::System.Array.Empty<object>());
						throw new FirmwareUpdateSessionDisposedException((global::System.Exception)null);
					}
					val3 = firmwareUpdateSession.LogicalDevice;
					if (options == null)
					{
						options = (IReadOnlyDictionary<FirmwareUpdateOption, object>)(object)new Dictionary<FirmwareUpdateOption, object>();
					}
					_003CjumpToBootLogicalDevice_003E5__2 = (ILogicalDeviceJumpToBootloader)(object)((val3 is ILogicalDeviceJumpToBootloader) ? val3 : null);
					if (_003CjumpToBootLogicalDevice_003E5__2 != null && _003CjumpToBootLogicalDevice_003E5__2.IsJumpToBootRequiredForFirmwareUpdate)
					{
						if (FirmwareUpdateOptionExtension.IsDeviceAuthorizationRequired(options))
						{
							val = directConnectionMyRvLink.FirmwareUpdateAuthorizationAsync((ILogicalDevice)(object)val3, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CUpdateFirmwareAsync_003Ed__159>(ref val, ref this);
								return;
							}
							goto IL_014b;
						}
						goto IL_0152;
					}
					goto IL_0210;
				case 0:
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_014b;
				case 1:
					val2 = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter<ILogicalDeviceReflashBootloader>);
					num = (_003C_003E1__state = -1);
					goto IL_0208;
				case 2:
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						break;
					}
					IL_014b:
					((TaskAwaiter)(ref val)).GetResult();
					goto IL_0152;
					IL_0152:
					if (!FirmwareUpdateOptionExtension.TryGetJumpToBootHoldTime(options, ref val4))
					{
						val4 = TimeSpan.FromMilliseconds(10000.0);
						TaggedLog.Debug("DirectConnectionMyRvLink", $"Jump to boot time not specified but required so using a default time of {val4}", global::System.Array.Empty<object>());
					}
					val2 = directConnectionMyRvLink.JumpToBootloaderAsync(_003CjumpToBootLogicalDevice_003E5__2, val4, cancellationToken).GetAwaiter();
					if (!val2.IsCompleted)
					{
						num = (_003C_003E1__state = 1);
						_003C_003Eu__2 = val2;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<ILogicalDeviceReflashBootloader>, _003CUpdateFirmwareAsync_003Ed__159>(ref val2, ref this);
						return;
					}
					goto IL_0208;
					IL_0210:
					val = directConnectionMyRvLink.UpdateFirmwareInternalAsync(val3, data, progressAck, cancellationToken, options).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (_003C_003E1__state = 2);
						_003C_003Eu__1 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CUpdateFirmwareAsync_003Ed__159>(ref val, ref this);
						return;
					}
					break;
					IL_0208:
					val3 = (ILogicalDeviceFirmwareUpdateDevice)(object)val2.GetResult();
					goto IL_0210;
				}
				((TaskAwaiter)(ref val)).GetResult();
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003CjumpToBootLogicalDevice_003E5__2 = null;
				((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003CjumpToBootLogicalDevice_003E5__2 = null;
			((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetResult();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CUpdateFirmwareInternalAsync_003Ed__160 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public ILogicalDeviceFirmwareUpdateDevice logicalDeviceToReflash;

		public IReadOnlyDictionary<FirmwareUpdateOption, object> options;

		public DirectConnectionMyRvLink _003C_003E4__this;

		public CancellationToken cancellationToken;

		public global::System.Collections.Generic.IReadOnlyList<byte> data;

		public Func<ILogicalDeviceTransferProgress, bool> progressAck;

		private uint _003CstartAddress_003E5__2;

		private TaskAwaiter<FirmwareUpdateSupport> _003C_003Eu__1;

		private TaskAwaiter _003C_003Eu__2;

		private TaskAwaiter<bool> _003C_003Eu__3;

		private void MoveNext()
		{
			//IL_0530: Expected O, but got Unknown
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_0561: Expected O, but got Unknown
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_059c: Expected O, but got Unknown
			//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d7: Expected O, but got Unknown
			//IL_0606: Unknown result type (might be due to invalid IL or missing references)
			//IL_060e: Expected O, but got Unknown
			//IL_0632: Expected O, but got Unknown
			//IL_067d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0453: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04de: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Invalid comparison between Unknown and I4
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_0414: Unknown result type (might be due to invalid IL or missing references)
			//IL_041e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04be: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			try
			{
				_ = 6;
				try
				{
					TaskAwaiter<FirmwareUpdateSupport> val3;
					TaskAwaiter val2;
					TaskAwaiter<bool> val;
					FirmwareUpdateSupport result;
					BlockTransferStartOptions val5;
					BlockTransferStopOptions val6;
					switch (num)
					{
					default:
					{
						ILogicalDeviceFirmwareUpdateDevice obj = logicalDeviceToReflash;
						ILogicalDeviceJumpToBootloader val4 = (ILogicalDeviceJumpToBootloader)(object)((obj is ILogicalDeviceJumpToBootloader) ? obj : null);
						if (val4 != null && val4.IsJumpToBootRequiredForFirmwareUpdate)
						{
							throw new ArgumentException("Given logical device must be in bootloader mode for re-flashing", "logicalDeviceToReflash");
						}
						if (((ILogicalDevice)logicalDeviceToReflash).LogicalId.ProductId == PRODUCT_ID.CAN_RE_FLASH_BOOTLOADER)
						{
							throw new FirmwareUpdateBootloaderException($"Device not configured properly as product id is being reported as {((ILogicalDevice)logicalDeviceToReflash).LogicalId.ProductId}", (global::System.Exception)null);
						}
						if (!FirmwareUpdateOptionExtension.TryGetStartAddress(options, ref _003CstartAddress_003E5__2))
						{
							throw new FirmwareUpdateMissingRequiredOptionException((ILogicalDevice)(object)logicalDeviceToReflash, (FirmwareUpdateOption)1, (global::System.Exception)null);
						}
						if (!directConnectionMyRvLink.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDeviceToReflash))
						{
							throw new MyRvLinkDeviceOfflineException(directConnectionMyRvLink, (ILogicalDevice)(object)logicalDeviceToReflash);
						}
						val3 = directConnectionMyRvLink.TryGetFirmwareUpdateSupportAsync((ILogicalDevice)(object)logicalDeviceToReflash, cancellationToken).GetAwaiter();
						if (!val3.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val3;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<FirmwareUpdateSupport>, _003CUpdateFirmwareInternalAsync_003Ed__160>(ref val3, ref this);
							return;
						}
						goto IL_0151;
					}
					case 0:
						val3 = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<FirmwareUpdateSupport>);
						num = (_003C_003E1__state = -1);
						goto IL_0151;
					case 1:
						val2 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_01db;
					case 2:
						val2 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_02e9;
					case 3:
						val2 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_0374;
					case 4:
						val2 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_03eb;
					case 5:
						val2 = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						goto IL_046f;
					case 6:
						{
							val = _003C_003Eu__3;
							_003C_003Eu__3 = default(TaskAwaiter<bool>);
							num = (_003C_003E1__state = -1);
							break;
						}
						IL_046f:
						((TaskAwaiter)(ref val2)).GetResult();
						if (DEVICE_TYPE.op_Implicit(((ILogicalDevice)logicalDeviceToReflash).LogicalId.DeviceType) == 50)
						{
							val = directConnectionMyRvLink.TryRemoveRefreshBootLoaderWhenOfflineAsync(logicalDeviceToReflash, cancellationToken).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (_003C_003E1__state = 6);
								_003C_003Eu__3 = val;
								((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CUpdateFirmwareInternalAsync_003Ed__160>(ref val, ref this);
								return;
							}
							break;
						}
						goto end_IL_0013;
						IL_0151:
						result = val3.GetResult();
						if ((int)result != 1)
						{
							throw new FirmwareUpdateNotSupportedException((ILogicalDevice)(object)logicalDeviceToReflash, result, (global::System.Exception)null);
						}
						if (FirmwareUpdateOptionExtension.IsDeviceAuthorizationRequired(options))
						{
							val2 = directConnectionMyRvLink.FirmwareUpdateAuthorizationAsync((ILogicalDevice)(object)logicalDeviceToReflash, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val2)).IsCompleted)
							{
								num = (_003C_003E1__state = 1);
								_003C_003Eu__2 = val2;
								((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CUpdateFirmwareInternalAsync_003Ed__160>(ref val2, ref this);
								return;
							}
							goto IL_01db;
						}
						goto IL_01e2;
						IL_01e2:
						val5 = (BlockTransferStartOptions)30;
						TaggedLog.Information("DirectConnectionMyRvLink", $"Block Transfer Starting: {3}, {val5}, Address: 0x{_003CstartAddress_003E5__2:X} Size: {((global::System.Collections.Generic.IReadOnlyCollection<byte>)data).Count}", global::System.Array.Empty<object>());
						val2 = directConnectionMyRvLink.StartDeviceBlockTransferAsync((ILogicalDevice)(object)logicalDeviceToReflash, (BlockTransferBlockId)3, val5, cancellationToken, _003CstartAddress_003E5__2, (uint)((global::System.Collections.Generic.IReadOnlyCollection<byte>)data).Count).GetAwaiter();
						if (!((TaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (_003C_003E1__state = 2);
							_003C_003Eu__2 = val2;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CUpdateFirmwareInternalAsync_003Ed__160>(ref val2, ref this);
							return;
						}
						goto IL_02e9;
						IL_02e9:
						((TaskAwaiter)(ref val2)).GetResult();
						TaggedLog.Information("DirectConnectionMyRvLink", "Block Transfer In Progress", global::System.Array.Empty<object>());
						val2 = directConnectionMyRvLink.DeviceBlockWriteAsync((ILogicalDevice)(object)logicalDeviceToReflash, (BlockTransferBlockId)3, data, progressAck, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (_003C_003E1__state = 3);
							_003C_003Eu__2 = val2;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CUpdateFirmwareInternalAsync_003Ed__160>(ref val2, ref this);
							return;
						}
						goto IL_0374;
						IL_03eb:
						((TaskAwaiter)(ref val2)).GetResult();
						goto IL_03f2;
						IL_0374:
						((TaskAwaiter)(ref val2)).GetResult();
						if (FirmwareUpdateOptionExtension.IsDeviceAuthorizationRequired(options))
						{
							val2 = directConnectionMyRvLink.FirmwareUpdateAuthorizationAsync((ILogicalDevice)(object)logicalDeviceToReflash, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val2)).IsCompleted)
							{
								num = (_003C_003E1__state = 4);
								_003C_003Eu__2 = val2;
								((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CUpdateFirmwareInternalAsync_003Ed__160>(ref val2, ref this);
								return;
							}
							goto IL_03eb;
						}
						goto IL_03f2;
						IL_03f2:
						val6 = (BlockTransferStopOptions)6;
						TaggedLog.Information("DirectConnectionMyRvLink", "Block Transfer Ending", global::System.Array.Empty<object>());
						val2 = directConnectionMyRvLink.StopDeviceBlockTransferAsync((ILogicalDevice)(object)logicalDeviceToReflash, (BlockTransferBlockId)3, val6, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (_003C_003E1__state = 5);
							_003C_003Eu__2 = val2;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CUpdateFirmwareInternalAsync_003Ed__160>(ref val2, ref this);
							return;
						}
						goto IL_046f;
						IL_01db:
						((TaskAwaiter)(ref val2)).GetResult();
						goto IL_01e2;
					}
					if (!val.GetResult())
					{
						ILogicalDeviceFirmwareUpdateDevice obj2 = logicalDeviceToReflash;
						TaggedLog.Error("DirectConnectionMyRvLink", ((obj2 != null) ? ((IDevicesCommon)obj2).DeviceName : null) + " is not removed.", global::System.Array.Empty<object>());
					}
					end_IL_0013:;
				}
				catch (BlockTransferNotSupportedException ex)
				{
					BlockTransferNotSupportedException ex2 = ex;
					TaggedLog.Warning("DirectConnectionMyRvLink", "Unable to Update Firmware Block Transfer Not Supported: " + ((global::System.Exception)(object)ex2).Message, global::System.Array.Empty<object>());
					throw new FirmwareUpdateNotSupportedException((ILogicalDevice)(object)logicalDeviceToReflash, (FirmwareUpdateSupport)0, (global::System.Exception)(object)ex2);
				}
				catch (BlockTransferBlockTooSmallException ex3)
				{
					BlockTransferBlockTooSmallException ex4 = ex3;
					TaggedLog.Warning("DirectConnectionMyRvLink", "Unable to Update Firmware Block Transfer Size Too Small: " + ((global::System.Exception)(object)ex4).Message, global::System.Array.Empty<object>());
					throw new FirmwareUpdateTooSmallException((ILogicalDevice)(object)logicalDeviceToReflash, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)data).Count, (global::System.Exception)(object)ex4);
				}
				catch (BlockTransferBlockTooBigException ex5)
				{
					BlockTransferBlockTooBigException ex6 = ex5;
					TaggedLog.Warning("DirectConnectionMyRvLink", "Unable to Update Firmware Block Transfer Size Too Big: " + ((global::System.Exception)(object)ex6).Message, global::System.Array.Empty<object>());
					throw new FirmwareUpdateTooBigException((ILogicalDevice)(object)logicalDeviceToReflash, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)data).Count, (global::System.Exception)(object)ex6);
				}
				catch (BlockTransferWriteFailedException ex7)
				{
					BlockTransferWriteFailedException ex8 = ex7;
					TaggedLog.Warning("DirectConnectionMyRvLink", "Unable to Update Firmware Block Transfer Write Failed: " + ((global::System.Exception)(object)ex8).Message, global::System.Array.Empty<object>());
					throw new FirmwareUpdateFailedException((ILogicalDevice)(object)logicalDeviceToReflash, ex8.Progress, (global::System.Exception)(object)ex8);
				}
				catch (FirmwareUpdateNotAuthorizedException ex9)
				{
					FirmwareUpdateNotAuthorizedException ex10 = ex9;
					TaggedLog.Warning("DirectConnectionMyRvLink", "Unable to Update Firmware Block Transfer Not Authorized: " + ((global::System.Exception)(object)ex10).Message, global::System.Array.Empty<object>());
					throw;
				}
				catch (FirmwareUpdateException ex11)
				{
					FirmwareUpdateException ex12 = ex11;
					TaggedLog.Warning("DirectConnectionMyRvLink", "Unable to Update Firmware " + ((global::System.Exception)(object)ex12).Message, global::System.Array.Empty<object>());
					throw;
				}
				catch (global::System.Exception ex13)
				{
					TaggedLog.Error("DirectConnectionMyRvLink", "Unable to Update Firmware " + ex13.Message, global::System.Array.Empty<object>());
					throw new BlockTransferException("Unable to Update Firmware", ex13);
				}
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

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CWaitForRunningCommandToComplete_003Ed__99 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<IMyRvLinkCommandResponse> _003C_003Et__builder;

		public ushort commandId;

		public DirectConnectionMyRvLink _003C_003E4__this;

		private MyRvLinkCommandTracker _003CcommandTracker_003E5__2;

		private TaskAwaiter<IMyRvLinkCommandResponse> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLink directConnectionMyRvLink = _003C_003E4__this;
			IMyRvLinkCommandResponse result;
			try
			{
				if (num == 0)
				{
					goto IL_010f;
				}
				if (commandId != 0 && commandId != 65535)
				{
					object obj = directConnectionMyRvLink._lock;
					bool flag = false;
					try
					{
						Monitor.Enter(obj, ref flag);
						directConnectionMyRvLink.FlushCompletedCommands();
						_003CcommandTracker_003E5__2 = DictionaryExtension.TryGetValue<int, MyRvLinkCommandTracker>((IReadOnlyDictionary<int, MyRvLinkCommandTracker>)(object)directConnectionMyRvLink._commandActiveDict, (int)commandId);
						if (_003CcommandTracker_003E5__2 != null)
						{
							_003CcommandTracker_003E5__2.ResetTimer();
							goto end_IL_0047;
						}
						TaggedLog.Warning("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} Unable to {"WaitForRunningCommandToComplete"} because command tracker for {commandId} not found.", global::System.Array.Empty<object>());
						result = new MyRvLinkCommandResponseFailure(commandId, MyRvLinkCommandResponseFailureCode.InvalidCommand);
						goto end_IL_000e;
						end_IL_0047:;
					}
					finally
					{
						if (num < 0 && flag)
						{
							Monitor.Exit(obj);
						}
					}
					goto IL_010f;
				}
				result = new MyRvLinkCommandResponseFailure(commandId, MyRvLinkCommandResponseFailureCode.InvalidCommand);
				goto end_IL_000e;
				IL_010f:
				try
				{
					TaskAwaiter<IMyRvLinkCommandResponse> val;
					if (num != 0)
					{
						TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} Wait for command to complete {_003CcommandTracker_003E5__2.Command}", global::System.Array.Empty<object>());
						val = _003CcommandTracker_003E5__2.WaitAsync().GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IMyRvLinkCommandResponse>, _003CWaitForRunningCommandToComplete_003Ed__99>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<IMyRvLinkCommandResponse>);
						num = (_003C_003E1__state = -1);
					}
					result = val.GetResult();
				}
				catch (TimeoutException)
				{
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} SendCommandAsync Timeout {_003CcommandTracker_003E5__2.Command}", global::System.Array.Empty<object>());
					result = _003CcommandTracker_003E5__2.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandTimeout);
				}
				catch (OperationCanceledException)
				{
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} SendCommandAsync Canceled {_003CcommandTracker_003E5__2.Command}", global::System.Array.Empty<object>());
					result = _003CcommandTracker_003E5__2.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandAborted);
				}
				catch (global::System.Exception ex3)
				{
					TaggedLog.Warning("DirectConnectionMyRvLink", $"{directConnectionMyRvLink.LogPrefix} SendCommandAsync Failure {_003CcommandTracker_003E5__2.Command}: {ex3.Message}", global::System.Array.Empty<object>());
					result = _003CcommandTracker_003E5__2.TrySetFailure(MyRvLinkCommandResponseFailureCode.Other);
				}
				end_IL_000e:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003CcommandTracker_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003CcommandTracker_003E5__2 = null;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	private const int MaxDataBlockSize = 128;

	private const int MillisecondsPerSecond = 1000;

	private const int ErrorDelayMilliseconds = 200;

	private const uint WriteFinishedAddressOffset = 4294967295u;

	private bool _firmwareUpdateInProgress;

	private const int CommandLeveler1ResendTimeoutMs = 1000;

	private byte[] _lastSentLeveler1CommandData = new byte[0];

	private ValueTuple<ushort, LogicalDeviceLevelerCommandType1, long> _lastSentLeveler1Command = new ValueTuple<ushort, LogicalDeviceLevelerCommandType1, long>((ushort)0, (LogicalDeviceLevelerCommandType1)null, 0L);

	private const int Leveler3CommandTimeout = 2500;

	public const int Leveler4CommandTimeout = 2500;

	private const string LogTag = "DirectConnectionMyRvLink";

	public const byte DeviceIdUnknown = 255;

	private readonly object _lock = new object();

	private MyRvLinkVersionTracker? _versionTracker;

	private MyRvLinkDeviceTracker? _deviceTracker;

	private readonly TimeSpan _reloadDevicesCheckTime = TimeSpan.FromMilliseconds(10000.0);

	private readonly TimeSpan _takeDevicesOfflineCheckTime = TimeSpan.FromMilliseconds(4000.0);

	private readonly TimeSpan _failureTimeout = TimeSpan.FromMilliseconds(500.0);

	private Timer? _takeDevicesOfflineTimer;

	private global::System.DateTime? _realTimeClock;

	private MyRvLinkGatewayInformation? _gatewayInfo;

	protected bool IsStarted;

	public const int CommandQueueLimit = 20;

	public const int CommandTimeoutMs = 8000;

	public const int CommandTimeoutExtendedMs = 16000;

	public const int CommandCompletedCacheSize = 100;

	public const ushort CommandIdInvalid = 0;

	public const ushort CommandIdStart = 1;

	public const ushort CommandIdNoResponse = 65535;

	private readonly Dictionary<int, MyRvLinkCommandTracker> _commandActiveDict = new Dictionary<int, MyRvLinkCommandTracker>();

	private readonly FixedSizedConcurrentQueue<MyRvLinkCommandTracker> _commandCompletedQueue = new FixedSizedConcurrentQueue<MyRvLinkCommandTracker>(100);

	private readonly MyRvLinkCommandResponseSuccessNoResponse _responseSuccessNoResponse = new MyRvLinkCommandResponseSuccessNoResponse();

	private ushort _nextCommandId = 1;

	private ushort _currentCommandId = 1;

	private bool _isConnectionOpened;

	private const int Leveler5CommandTimeout = 2500;

	private readonly TaskSerialQueue _pidSerialQueue = new TaskSerialQueue(200);

	private const int PidOperationDelayMs = 100;

	private Stopwatch _pidLastOperationTimer = Stopwatch.StartNew();

	private readonly TaskSerialQueue _dtcSerialQueue = new TaskSerialQueue(100);

	private readonly Stopwatch _dtcThrottleStopwatch = new Stopwatch();

	private const int DtcThrottleTimeMs = 500;

	private readonly FrequencyMetrics _receivedEventMetrics = new FrequencyMetrics();

	public const int MaxWaitTimeForFlashToCompleteMs = 30000;

	public const int WaitTimeForFlashToCompleteMs = 1000;

	public const int WaitForRebootIntoBootLoaderDelayMs = 1000;

	public const int WaitForRebootIntoBootloaderAttempts = 20;

	public const int DefaultJumpToBootMs = 10000;

	private readonly Dictionary<MyRvLinkCommandType, FrequencyMetrics> _metricsForCommandSends = new Dictionary<MyRvLinkCommandType, FrequencyMetrics>();

	private readonly Dictionary<MyRvLinkCommandType, FrequencyMetrics> _metricsForCommandFailures = new Dictionary<MyRvLinkCommandType, FrequencyMetrics>();

	private readonly Dictionary<MyRvLinkEventType, FrequencyMetrics> _metricsForEvents = new Dictionary<MyRvLinkEventType, FrequencyMetrics>();

	private const int RelayMovementCommandTimeout = 2500;

	private float? _voltage;

	private float? _temperature;

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
	public DeviceTableIdCache DeviceTableIdCache
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

	[field: CompilerGenerated]
	public ConcurrentHashSet<ILogicalDeviceSourceCommandMonitor> CommandMonitors
	{
		[CompilerGenerated]
		get;
	} = new ConcurrentHashSet<ILogicalDeviceSourceCommandMonitor>();

	public global::System.DateTime? RealTimeClock
	{
		get
		{
			if (!IsConnected || !IsStarted)
			{
				return null;
			}
			return _realTimeClock;
		}
		internal set
		{
			_realTimeClock = value;
		}
	}

	public MyRvLinkGatewayInformation? GatewayInfo
	{
		get
		{
			return _gatewayInfo;
		}
		private set
		{
			lock (_lock)
			{
				if (value != null)
				{
					HasMinimumExpectedProtocolVersion = GatewayVersionSupportLevelExtension.IsMinimumRequiredVersion(value.ProtocolVersionMajor);
					if (!HasMinimumExpectedProtocolVersion)
					{
						TaggedLog.Error("DirectConnectionMyRvLink", $"{LogPrefix} Gateway Minimum Protocol Version is {5} but received {value.ProtocolVersionMajor}", global::System.Array.Empty<object>());
						value = null;
					}
					else
					{
						MyRvLinkVersionTracker versionTracker = _versionTracker;
						if (versionTracker == null || !versionTracker.IsVersionSupported || !versionTracker.IsGatewayVersionValid(value))
						{
							value = null;
						}
					}
				}
				if (object.Equals((object)_gatewayInfo, (object)value))
				{
					return;
				}
				if (_gatewayInfo != null)
				{
					_ = _gatewayInfo.DeviceTableId;
					_ = _gatewayInfo.DeviceTableCrc;
				}
				_gatewayInfo = value;
				if (value == null)
				{
					HasMinimumExpectedProtocolVersion = false;
					MyRvLinkDeviceTracker? deviceTracker = _deviceTracker;
					if (deviceTracker != null)
					{
						((CommonDisposable)deviceTracker).TryDispose();
					}
					return;
				}
				TaggedLog.Information("DirectConnectionMyRvLink", $"{LogPrefix} GatewayInfo: {value}", global::System.Array.Empty<object>());
				_deviceTracker?.UpdateDeviceIdIfNeeded(value.DeviceTableId, value.DeviceTableCrc);
				if (_deviceTracker == null || ((CommonDisposable)_deviceTracker).IsDisposed || _deviceTracker.DeviceTableId != value.DeviceTableId || _deviceTracker.DeviceTableCrc != value.DeviceTableCrc)
				{
					MyRvLinkDeviceTracker? deviceTracker2 = _deviceTracker;
					if (deviceTracker2 != null)
					{
						((CommonDisposable)deviceTracker2).TryDispose();
					}
					_deviceTracker = new MyRvLinkDeviceTracker(this, value.DeviceTableId, value.DeviceTableCrc);
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} Created new Device Tracker {_deviceTracker}", global::System.Array.Empty<object>());
				}
				_deviceTracker.UpdateMetadataIfNeeded(value.DeviceMetadataTableCrc);
			}
		}
	}

	public bool IsFirmwareVersionSupported
	{
		get
		{
			if (HasMinimumExpectedProtocolVersion)
			{
				return _versionTracker?.IsVersionSupported ?? false;
			}
			return false;
		}
	}

	[field: CompilerGenerated]
	public bool HasMinimumExpectedProtocolVersion
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	[field: CompilerGenerated]
	protected Func<int, IMyRvLinkCommand?> GetPendingCommand
	{
		[CompilerGenerated]
		get;
	}

	public IN_MOTION_LOCKOUT_LEVEL InTransitLockoutLevel => _deviceTracker?.CachedInMotionLockoutLevel ?? IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)0);

	[field: CompilerGenerated]
	public ILogicalDeviceSessionManager? SessionManager
	{
		[CompilerGenerated]
		get;
	}

	public bool IsConnected
	{
		get
		{
			return _isConnectionOpened;
		}
		protected set
		{
			lock (_lock)
			{
				if (_isConnectionOpened == value)
				{
					return;
				}
				_isConnectionOpened = value;
				if (_isConnectionOpened)
				{
					lock (_lock)
					{
						_metricsForCommandSends.Clear();
						_metricsForCommandFailures.Clear();
						_metricsForEvents.Clear();
						_receivedEventMetrics.Clear();
					}
				}
				else
				{
					RealTimeClock = null;
				}
				AbortAllPendingCommands();
				TaggedLog.Debug("DirectConnectionMyRvLink", LogPrefix + " Connection Status Changed to " + (value ? "Opened" : "Closed") + ", cleared any pending commands", global::System.Array.Empty<object>());
			}
		}
	}

	public abstract string DeviceSourceToken { get; }

	public abstract global::System.Collections.Generic.IEnumerable<ILogicalDeviceTag> DeviceSourceTags { get; }

	public virtual bool AllowAutoOfflineLogicalDeviceRemoval => true;

	public virtual bool IsDeviceSourceActive => IsConnected;

	public global::System.DateTime GetRealTimeClockTime => RealTimeClock ?? global::System.DateTime.MinValue;

	[method: CompilerGenerated]
	public abstract event Action<ILogicalDeviceSourceDirectConnection>? DidConnectEvent;

	[method: CompilerGenerated]
	public abstract event Action<ILogicalDeviceSourceDirectConnection>? DidDisconnectEvent;

	[method: CompilerGenerated]
	public abstract event UpdateDeviceSourceReachabilityEventHandler UpdateDeviceSourceReachabilityEvent;

	[AsyncStateMachine(typeof(_003CGetDeviceBlockListAsync_003Ed__5))]
	public async global::System.Threading.Tasks.Task<global::System.Collections.Generic.IReadOnlyList<BlockTransferBlockId>> GetDeviceBlockListAsync(ILogicalDevice logicalDevice, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
		}
		ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!myRvDeviceFromLogicalDevice.HasValue)
		{
			throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
		}
		MyRvLinkCommandGetDeviceBlockList command = new MyRvLinkCommandGetDeviceBlockList(GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2);
		IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
		if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure failure))
		{
			if (myRvLinkCommandResponse is MyRvLinkGetDeviceBlockListCommandResponse myRvLinkGetDeviceBlockListCommandResponse)
			{
				return myRvLinkGetDeviceBlockListCommandResponse.BlockIds;
			}
			throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
		}
		throw new MyRvLinkCommandResponseFailureException(failure);
	}

	[AsyncStateMachine(typeof(_003CGetDeviceBlockPropertyFlagsAsync_003Ed__6))]
	public async global::System.Threading.Tasks.Task<BlockTransferPropertyFlags> GetDeviceBlockPropertyFlagsAsync(ILogicalDevice logicalDevice, BlockTransferBlockId blockId, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
		}
		ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!myRvDeviceFromLogicalDevice.HasValue)
		{
			throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
		}
		MyRvLinkCommandGetDeviceBlockProperties command = new MyRvLinkCommandGetDeviceBlockProperties(GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)0);
		IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
		if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure failure))
		{
			if (myRvLinkCommandResponse is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse)
			{
				return myRvLinkGetDeviceBlockPropertyCommandResponse.Flags;
			}
			throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
		}
		throw new MyRvLinkCommandResponseFailureException(failure);
	}

	[AsyncStateMachine(typeof(_003CGetDeviceBlockPropertyReadSessionIdAsync_003Ed__7))]
	public async global::System.Threading.Tasks.Task<LogicalDeviceSessionType> GetDeviceBlockPropertyReadSessionIdAsync(ILogicalDevice logicalDevice, BlockTransferBlockId blockId, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
		}
		ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!myRvDeviceFromLogicalDevice.HasValue)
		{
			throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
		}
		MyRvLinkCommandGetDeviceBlockProperties command = new MyRvLinkCommandGetDeviceBlockProperties(GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)1);
		IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
		if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure failure))
		{
			if (myRvLinkCommandResponse is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse)
			{
				return myRvLinkGetDeviceBlockPropertyCommandResponse.ReadSessionId;
			}
			throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
		}
		throw new MyRvLinkCommandResponseFailureException(failure);
	}

	[AsyncStateMachine(typeof(_003CGetDeviceBlockPropertyWriteSessionIdAsync_003Ed__8))]
	public async global::System.Threading.Tasks.Task<LogicalDeviceSessionType> GetDeviceBlockPropertyWriteSessionIdAsync(ILogicalDevice logicalDevice, BlockTransferBlockId blockId, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
		}
		ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!myRvDeviceFromLogicalDevice.HasValue)
		{
			throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
		}
		MyRvLinkCommandGetDeviceBlockProperties command = new MyRvLinkCommandGetDeviceBlockProperties(GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)2);
		IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
		if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure failure))
		{
			if (myRvLinkCommandResponse is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse)
			{
				return myRvLinkGetDeviceBlockPropertyCommandResponse.WriteSessionId;
			}
			throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
		}
		throw new MyRvLinkCommandResponseFailureException(failure);
	}

	[AsyncStateMachine(typeof(_003CGetDeviceBlockCapacityAsync_003Ed__9))]
	public async global::System.Threading.Tasks.Task<ulong> GetDeviceBlockCapacityAsync(ILogicalDevice logicalDevice, BlockTransferBlockId blockId, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
		}
		ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!myRvDeviceFromLogicalDevice.HasValue)
		{
			throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
		}
		MyRvLinkCommandGetDeviceBlockProperties command = new MyRvLinkCommandGetDeviceBlockProperties(GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)3);
		IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
		if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure failure))
		{
			if (myRvLinkCommandResponse is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse)
			{
				return myRvLinkGetDeviceBlockPropertyCommandResponse.BlockCapacity;
			}
			throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
		}
		throw new MyRvLinkCommandResponseFailureException(failure);
	}

	[AsyncStateMachine(typeof(_003CGetDeviceBlockSizeAsync_003Ed__10))]
	public async global::System.Threading.Tasks.Task<ulong> GetDeviceBlockSizeAsync(ILogicalDevice logicalDevice, BlockTransferBlockId blockId, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
		}
		ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!myRvDeviceFromLogicalDevice.HasValue)
		{
			throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
		}
		MyRvLinkCommandGetDeviceBlockProperties command = new MyRvLinkCommandGetDeviceBlockProperties(GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)4);
		IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
		if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure failure))
		{
			if (myRvLinkCommandResponse is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse)
			{
				return myRvLinkGetDeviceBlockPropertyCommandResponse.CurrentBlockSize;
			}
			throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
		}
		throw new MyRvLinkCommandResponseFailureException(failure);
	}

	[AsyncStateMachine(typeof(_003CGetDeviceBlockCrcAsync_003Ed__11))]
	public async global::System.Threading.Tasks.Task<uint> GetDeviceBlockCrcAsync(ILogicalDevice logicalDevice, BlockTransferBlockId blockId, bool recalculate, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
		}
		ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!myRvDeviceFromLogicalDevice.HasValue)
		{
			throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
		}
		ushort nextCommandId = GetNextCommandId();
		MyRvLinkCommandGetDeviceBlockProperties command = ((!recalculate) ? new MyRvLinkCommandGetDeviceBlockProperties(nextCommandId, myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)5) : new MyRvLinkCommandGetDeviceBlockProperties(nextCommandId, myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)6));
		IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
		if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure failure))
		{
			if (myRvLinkCommandResponse is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse)
			{
				return myRvLinkGetDeviceBlockPropertyCommandResponse.Crc;
			}
			throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
		}
		throw new MyRvLinkCommandResponseFailureException(failure);
	}

	[AsyncStateMachine(typeof(_003CGetDeviceBlockStartAddressAsync_003Ed__12))]
	public async global::System.Threading.Tasks.Task<uint> GetDeviceBlockStartAddressAsync(ILogicalDevice logicalDevice, BlockTransferBlockId blockId, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			throw new LogicalDeviceException($"Logical device {logicalDevice} is offline.", (global::System.Exception)null);
		}
		ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!myRvDeviceFromLogicalDevice.HasValue)
		{
			throw new MyRvLinkException($"No matching RvLink device for logical device {logicalDevice}.");
		}
		MyRvLinkCommandGetDeviceBlockProperties command = new MyRvLinkCommandGetDeviceBlockProperties(GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, blockId, (BlockTransferPropertyId)7);
		IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
		if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure failure))
		{
			if (myRvLinkCommandResponse is MyRvLinkGetDeviceBlockPropertyCommandResponse myRvLinkGetDeviceBlockPropertyCommandResponse)
			{
				return myRvLinkGetDeviceBlockPropertyCommandResponse.BlockStartAddress;
			}
			throw new MyRvLinkException($"Failed to Get Block IDs from {logicalDevice}: Unknown result");
		}
		throw new MyRvLinkCommandResponseFailureException(failure);
	}

	[AsyncStateMachine(typeof(_003CStartDeviceBlockTransferAsync_003Ed__13))]
	public global::System.Threading.Tasks.Task StartDeviceBlockTransferAsync(ILogicalDevice logicalDevice, BlockTransferBlockId blockId, BlockTransferStartOptions options, CancellationToken cancellationToken, uint? startAddress = null, uint? size = null)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_003CStartDeviceBlockTransferAsync_003Ed__13 _003CStartDeviceBlockTransferAsync_003Ed__ = default(_003CStartDeviceBlockTransferAsync_003Ed__13);
		_003CStartDeviceBlockTransferAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CStartDeviceBlockTransferAsync_003Ed__._003C_003E4__this = this;
		_003CStartDeviceBlockTransferAsync_003Ed__.logicalDevice = logicalDevice;
		_003CStartDeviceBlockTransferAsync_003Ed__.blockId = blockId;
		_003CStartDeviceBlockTransferAsync_003Ed__.options = options;
		_003CStartDeviceBlockTransferAsync_003Ed__.cancellationToken = cancellationToken;
		_003CStartDeviceBlockTransferAsync_003Ed__.startAddress = startAddress;
		_003CStartDeviceBlockTransferAsync_003Ed__.size = size;
		_003CStartDeviceBlockTransferAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CStartDeviceBlockTransferAsync_003Ed__._003C_003Et__builder)).Start<_003CStartDeviceBlockTransferAsync_003Ed__13>(ref _003CStartDeviceBlockTransferAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CStartDeviceBlockTransferAsync_003Ed__._003C_003Et__builder)).Task;
	}

	[AsyncStateMachine(typeof(_003CStopDeviceBlockTransferAsync_003Ed__14))]
	public global::System.Threading.Tasks.Task StopDeviceBlockTransferAsync(ILogicalDevice logicalDevice, BlockTransferBlockId blockId, BlockTransferStopOptions options, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_003CStopDeviceBlockTransferAsync_003Ed__14 _003CStopDeviceBlockTransferAsync_003Ed__ = default(_003CStopDeviceBlockTransferAsync_003Ed__14);
		_003CStopDeviceBlockTransferAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CStopDeviceBlockTransferAsync_003Ed__._003C_003E4__this = this;
		_003CStopDeviceBlockTransferAsync_003Ed__.logicalDevice = logicalDevice;
		_003CStopDeviceBlockTransferAsync_003Ed__.blockId = blockId;
		_003CStopDeviceBlockTransferAsync_003Ed__.options = options;
		_003CStopDeviceBlockTransferAsync_003Ed__.cancellationToken = cancellationToken;
		_003CStopDeviceBlockTransferAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CStopDeviceBlockTransferAsync_003Ed__._003C_003Et__builder)).Start<_003CStopDeviceBlockTransferAsync_003Ed__14>(ref _003CStopDeviceBlockTransferAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CStopDeviceBlockTransferAsync_003Ed__._003C_003Et__builder)).Task;
	}

	public global::System.Threading.Tasks.Task<global::System.Collections.Generic.IReadOnlyList<byte>> DeviceBlockReadAsync(ILogicalDevice logicalDevice, BlockTransferBlockId blockId, CancellationToken cancellationToken)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotImplementedException();
	}

	[AsyncStateMachine(typeof(_003CDeviceBlockWriteAsync_003Ed__16))]
	public global::System.Threading.Tasks.Task DeviceBlockWriteAsync(ILogicalDevice logicalDevice, BlockTransferBlockId blockId, global::System.Collections.Generic.IReadOnlyList<byte> data, Func<ILogicalDeviceTransferProgress, bool> progressAck, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		_003CDeviceBlockWriteAsync_003Ed__16 _003CDeviceBlockWriteAsync_003Ed__ = default(_003CDeviceBlockWriteAsync_003Ed__16);
		_003CDeviceBlockWriteAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CDeviceBlockWriteAsync_003Ed__._003C_003E4__this = this;
		_003CDeviceBlockWriteAsync_003Ed__.logicalDevice = logicalDevice;
		_003CDeviceBlockWriteAsync_003Ed__.blockId = blockId;
		_003CDeviceBlockWriteAsync_003Ed__.data = data;
		_003CDeviceBlockWriteAsync_003Ed__.progressAck = progressAck;
		_003CDeviceBlockWriteAsync_003Ed__.cancellationToken = cancellationToken;
		_003CDeviceBlockWriteAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CDeviceBlockWriteAsync_003Ed__._003C_003Et__builder)).Start<_003CDeviceBlockWriteAsync_003Ed__16>(ref _003CDeviceBlockWriteAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CDeviceBlockWriteAsync_003Ed__._003C_003Et__builder)).Task;
	}

	[AsyncStateMachine(typeof(_003CSendDirectCommandLeveler1_003Ed__21))]
	public async global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandLeveler1(ILogicalDeviceLevelerType1 logicalDevice, LogicalDeviceLevelerCommandType1 command, CancellationToken cancelToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
		{
			return (CommandResult)6;
		}
		ValueTuple<byte, byte>? myRvLinkDevice = GetMyRvDeviceFromLogicalDevice((ILogicalDevice)(object)logicalDevice);
		if (!myRvLinkDevice.HasValue)
		{
			return (CommandResult)6;
		}
		long nowTimestampMs = LogicalDeviceFreeRunningTimer.ElapsedMilliseconds;
		try
		{
			ValueTuple<ushort, LogicalDeviceLevelerCommandType1, long> lastSentLeveler1Command = _lastSentLeveler1Command;
			long num = nowTimestampMs - lastSentLeveler1Command.Item3;
			if (lastSentLeveler1Command.Item1 != 0 && lastSentLeveler1Command.Item2 == command && num < 1000 && ArrayCommon.ArraysEqual<byte>(_lastSentLeveler1CommandData, ((LogicalDeviceCommandPacket)command).CopyCurrentData()))
			{
				_lastSentLeveler1Command = new ValueTuple<ushort, LogicalDeviceLevelerCommandType1, long>(lastSentLeveler1Command.Item1, command, nowTimestampMs);
				_lastSentLeveler1CommandData = ((LogicalDeviceCommandPacket)command).CopyCurrentData();
				if (await ResendRunningCommandAsync(lastSentLeveler1Command.Item1, cancelToken))
				{
					return (CommandResult)0;
				}
			}
			ushort nextCommandId = GetNextCommandId();
			_lastSentLeveler1Command = new ValueTuple<ushort, LogicalDeviceLevelerCommandType1, long>(nextCommandId, command, nowTimestampMs);
			_lastSentLeveler1CommandData = ((LogicalDeviceCommandPacket)command).CopyCurrentData();
			MyRvLinkCommandLeveler1ButtonCommand command2 = new MyRvLinkCommandLeveler1ButtonCommand(nextCommandId, myRvLinkDevice.Value.Item1, myRvLinkDevice.Value.Item2, command);
			return (await SendCommandAsync(command2, cancelToken, MyRvLinkSendCommandOption.DontWaitForResponse)).CommandResult;
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Warning("DirectConnectionMyRvLink", "Sending command failed " + ex.Message, global::System.Array.Empty<object>());
			return (CommandResult)7;
		}
	}

	[AsyncStateMachine(typeof(_003CSendDirectCommandLeveler3_003Ed__23))]
	public async global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandLeveler3(ILogicalDeviceLevelerType3 logicalDevice, LogicalDeviceLevelerCommandType3 command, CancellationToken cancelToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
		{
			return (CommandResult)6;
		}
		ValueTuple<byte, byte>? myRvLinkDevice = GetMyRvDeviceFromLogicalDevice((ILogicalDevice)(object)logicalDevice);
		if (!myRvLinkDevice.HasValue)
		{
			return (CommandResult)6;
		}
		try
		{
			MyRvLinkCommandContext<LogicalDeviceLevelerButtonType3> commandContext;
			lock (this)
			{
				object obj = default(object);
				if (((ILogicalDevice)logicalDevice).CustomData.TryGetValue("DirectConnectionMyRvLink.IDirectCommandLeveler3", ref obj) && obj is MyRvLinkCommandContext<LogicalDeviceLevelerButtonType3> myRvLinkCommandContext)
				{
					commandContext = myRvLinkCommandContext;
				}
				else
				{
					((ILogicalDevice)logicalDevice).CustomData["DirectConnectionMyRvLink.IDirectCommandLeveler3"] = (commandContext = new MyRvLinkCommandContext<LogicalDeviceLevelerButtonType3>());
				}
			}
			if (commandContext.LastSentCommandReceivedError)
			{
				TaggedLog.Warning("DirectConnectionMyRvLink", "Leveler 3 last sent command received an error!", global::System.Array.Empty<object>());
				commandContext.ClearLastSentCommandReceivedError();
				IMyRvLinkCommandResponseFailure? activeFailure = commandContext.ActiveFailure;
				return (CommandResult)((activeFailure == null) ? 7 : ((int)activeFailure.CommandResult));
			}
			bool flag = commandContext.CanResendCommand(command.ButtonsPressed, (IDeviceCommandPacket?)(object)command);
			if (flag)
			{
				flag = await ResendRunningCommandAsync(commandContext.SentCommandId, cancelToken);
			}
			if (flag)
			{
				commandContext.SentCommand(commandContext.SentCommandId, command.ButtonsPressed, (IDeviceCommandPacket?)(object)command);
				IMyRvLinkCommandResponseFailure? activeFailure2 = commandContext.ActiveFailure;
				return (CommandResult)((activeFailure2 != null) ? ((int)activeFailure2.CommandResult) : 0);
			}
			ushort nextCommandId = GetNextCommandId();
			MyRvLinkCommandLeveler3ButtonCommand command2 = new MyRvLinkCommandLeveler3ButtonCommand(nextCommandId, myRvLinkDevice.Value.Item1, myRvLinkDevice.Value.Item2, command);
			SendCommandAsync(command2, cancelToken, TimeSpan.FromMilliseconds(2500.0), MyRvLinkSendCommandOption.DontWaitForResponse, delegate(IMyRvLinkCommandResponse response)
			{
				commandContext.ProcessResponse(response);
			});
			commandContext.SentCommand(nextCommandId, command.ButtonsPressed, (IDeviceCommandPacket?)(object)command);
			IMyRvLinkCommandResponseFailure? activeFailure3 = commandContext.ActiveFailure;
			return (CommandResult)((activeFailure3 != null) ? ((int)activeFailure3.CommandResult) : 0);
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Warning("DirectConnectionMyRvLink", "Sending command failed " + ex.Message, global::System.Array.Empty<object>());
			return (CommandResult)7;
		}
	}

	[AsyncStateMachine(typeof(_003CSendDirectCommandLeveler4_003Ed__25))]
	public async global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandLeveler4(ILogicalDeviceLevelerType4 logicalDevice, ILogicalDeviceLevelerCommandType4 command, CancellationToken cancelToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
		{
			return (CommandResult)6;
		}
		ValueTuple<byte, byte>? myRvLinkDevice = GetMyRvDeviceFromLogicalDevice((ILogicalDevice)(object)logicalDevice);
		if (!myRvLinkDevice.HasValue)
		{
			return (CommandResult)6;
		}
		try
		{
			MyRvLinkCommandContext<LevelerCommandCode> commandContext;
			lock (this)
			{
				object obj = default(object);
				if (((ILogicalDevice)logicalDevice).CustomData.TryGetValue("DirectConnectionMyRvLink.IDirectCommandLeveler4", ref obj) && obj is MyRvLinkCommandContext<LevelerCommandCode> myRvLinkCommandContext)
				{
					commandContext = myRvLinkCommandContext;
				}
				else
				{
					((ILogicalDevice)logicalDevice).CustomData["DirectConnectionMyRvLink.IDirectCommandLeveler4"] = (commandContext = new MyRvLinkCommandContext<LevelerCommandCode>());
				}
			}
			if (commandContext.LastSentCommandReceivedError)
			{
				TaggedLog.Warning("DirectConnectionMyRvLink", "Leveler 4 last sent command received an error!", global::System.Array.Empty<object>());
				commandContext.ClearLastSentCommandReceivedError();
				IMyRvLinkCommandResponseFailure? activeFailure = commandContext.ActiveFailure;
				return (CommandResult)((activeFailure == null) ? 7 : ((int)activeFailure.CommandResult));
			}
			bool flag = commandContext.CanResendCommand(command.Command, (IDeviceCommandPacket?)(object)command);
			if (flag)
			{
				flag = await ResendRunningCommandAsync(commandContext.SentCommandId, cancelToken);
			}
			if (flag)
			{
				commandContext.SentCommand(commandContext.SentCommandId, command.Command, (IDeviceCommandPacket?)(object)command);
				IMyRvLinkCommandResponseFailure? activeFailure2 = commandContext.ActiveFailure;
				return (CommandResult)((activeFailure2 != null) ? ((int)activeFailure2.CommandResult) : 0);
			}
			ushort nextCommandId = GetNextCommandId();
			MyRvLinkCommandLeveler4ButtonCommand command2 = new MyRvLinkCommandLeveler4ButtonCommand(nextCommandId, myRvLinkDevice.Value.Item1, myRvLinkDevice.Value.Item2, command);
			SendCommandAsync(command2, cancelToken, TimeSpan.FromMilliseconds(2500.0), MyRvLinkSendCommandOption.DontWaitForResponse, delegate(IMyRvLinkCommandResponse response)
			{
				commandContext.ProcessResponse(response);
			});
			commandContext.SentCommand(nextCommandId, command.Command, (IDeviceCommandPacket?)(object)command);
			IMyRvLinkCommandResponseFailure? activeFailure3 = commandContext.ActiveFailure;
			return (CommandResult)((activeFailure3 != null) ? ((int)activeFailure3.CommandResult) : 0);
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Warning("DirectConnectionMyRvLink", "Sending command failed " + ex.Message, global::System.Array.Empty<object>());
			return (CommandResult)7;
		}
	}

	protected DirectConnectionMyRvLink(ILogicalDeviceService deviceService, string logPrefix, List<ILogicalDeviceTag> gatewayTagList)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		LogPrefix = "[" + logPrefix + "]";
		DeviceService = deviceService ?? throw new ArgumentNullException("deviceService");
		ConnectionTagList = (global::System.Collections.Generic.IReadOnlyList<ILogicalDeviceTag>)(((gatewayTagList != null) ? Enumerable.ToList<ILogicalDeviceTag>((global::System.Collections.Generic.IEnumerable<ILogicalDeviceTag>)gatewayTagList) : null) ?? new List<ILogicalDeviceTag>());
		SessionManager = (ILogicalDeviceSessionManager?)(object)new MyRvLinkSessionManager(this);
		DeviceTableIdCache = new DeviceTableIdCache(this);
		GetPendingCommand = [CompilerGenerated] (int commandId) =>
		{
			lock (_lock)
			{
				MyRvLinkCommandTracker myRvLinkCommandTracker = default(MyRvLinkCommandTracker);
				if (!_commandActiveDict.TryGetValue(commandId, ref myRvLinkCommandTracker))
				{
					return (IMyRvLinkCommand?)null;
				}
				return myRvLinkCommandTracker.Command;
			}
		};
	}

	public bool IsLogicalDeviceOnline(ILogicalDevice? logicalDevice)
	{
		return _deviceTracker?.IsLogicalDeviceOnline(logicalDevice) ?? false;
	}

	public IN_MOTION_LOCKOUT_LEVEL GetLogicalDeviceInTransitLockoutLevel(ILogicalDevice? logicalDevice)
	{
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			return IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)0);
		}
		return _deviceTracker?.GetLogicalDeviceInTransitLockoutLevel(logicalDevice) ?? IN_MOTION_LOCKOUT_LEVEL.op_Implicit((byte)0);
	}

	public bool IsLogicalDeviceHazardous(ILogicalDevice? logicalDevice)
	{
		return IN_MOTION_LOCKOUT_LEVEL.op_Implicit(GetLogicalDeviceInTransitLockoutLevel(logicalDevice)) != 0;
	}

	public bool IsLogicalDeviceSupported(ILogicalDevice? logicalDevice)
	{
		if (!(logicalDevice is ILogicalDeviceSimulated))
		{
			if (logicalDevice is ILogicalDeviceMyRvLink)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public virtual void Start()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		lock (_lock)
		{
			if (!IsStarted)
			{
				_deviceTracker?.UpdateOnlineStatus(null);
			}
			Timer? takeDevicesOfflineTimer = _takeDevicesOfflineTimer;
			if (takeDevicesOfflineTimer != null)
			{
				IDisposableExtensions.TryDispose((global::System.IDisposable)takeDevicesOfflineTimer);
			}
			_takeDevicesOfflineTimer = new Timer(new TimerCallback(TakeDevicesOfflineCheck), (object)null, _takeDevicesOfflineCheckTime, _takeDevicesOfflineCheckTime);
			MyRvLinkVersionTracker? versionTracker = _versionTracker;
			if (versionTracker != null)
			{
				((CommonDisposable)versionTracker).TryDispose();
			}
			_versionTracker = new MyRvLinkVersionTracker(this);
			IsStarted = true;
		}
	}

	public virtual void Stop()
	{
		lock (_lock)
		{
			Timer? takeDevicesOfflineTimer = _takeDevicesOfflineTimer;
			if (takeDevicesOfflineTimer != null)
			{
				IDisposableExtensions.TryDispose((global::System.IDisposable)takeDevicesOfflineTimer);
			}
			_takeDevicesOfflineTimer = null;
			IsStarted = false;
			RealTimeClock = null;
			AbortAllPendingCommands();
			_deviceTracker?.UpdateOnlineStatus(null);
			MyRvLinkVersionTracker? versionTracker = _versionTracker;
			if (versionTracker != null)
			{
				((CommonDisposable)versionTracker).TryDispose();
			}
			_versionTracker = null;
		}
	}

	private void TakeDevicesOfflineCheck(object state)
	{
		if (IsStarted)
		{
			_deviceTracker?.TakeDevicesOfflineIfNeeded();
		}
	}

	public ushort GetNextCommandId()
	{
		lock (_lock)
		{
			ushort nextCommandId = _nextCommandId;
			ushort nextCommandId2;
			do
			{
				nextCommandId2 = _nextCommandId;
				_nextCommandId++;
			}
			while (_commandActiveDict.ContainsKey((int)nextCommandId2) && _nextCommandId != nextCommandId);
			if (_nextCommandId == nextCommandId)
			{
				throw new MyRvLinkException("GetNextCommandId failed because no Command Id's are available");
			}
			if (_nextCommandId == 65535)
			{
				_nextCommandId = 1;
			}
			if (_nextCommandId == 0)
			{
				_nextCommandId = 1;
			}
			_currentCommandId = nextCommandId2;
			return nextCommandId2;
		}
	}

	public global::System.Threading.Tasks.Task<IMyRvLinkCommandResponse> SendCommandAsync(IMyRvLinkCommand command, CancellationToken cancellationToken, MyRvLinkSendCommandOption commandOption = MyRvLinkSendCommandOption.None, Action<IMyRvLinkCommandResponse>? responseAction = null)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return SendCommandAsync(command, cancellationToken, TimeSpan.FromMilliseconds(8000.0), commandOption, responseAction);
	}

	[AsyncStateMachine(typeof(_003CSendCommandAsync_003Ed__95))]
	public async global::System.Threading.Tasks.Task<IMyRvLinkCommandResponse> SendCommandAsync(IMyRvLinkCommand command, CancellationToken cancellationToken, TimeSpan commandTimeout, MyRvLinkSendCommandOption commandOption = MyRvLinkSendCommandOption.None, Action<IMyRvLinkCommandResponse>? responseAction = null)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		TaggedLog.Information("DirectConnectionMyRvLink", $"{LogPrefix} Adapter SendCommand {command}", global::System.Array.Empty<object>());
		MyRvLinkCommandTracker commandTracker = null;
		if (command == null)
		{
			TaggedLog.Debug("DirectConnectionMyRvLink", LogPrefix + " SendCommandAsync Failed because given command was NULL.", global::System.Array.Empty<object>());
			return new MyRvLinkCommandResponseFailure(0, MyRvLinkCommandResponseFailureCode.InvalidCommand);
		}
		if (!IsStarted || !IsConnected)
		{
			return new MyRvLinkCommandResponseFailure(command.ClientCommandId, MyRvLinkCommandResponseFailureCode.Offline);
		}
		if (command.ClientCommandId == 65535)
		{
			try
			{
				UpdateFrequencyMetricForCommandSend(command.CommandType);
				await SendCommandRawAsync(command, cancellationToken).ConfigureAwait(false);
			}
			catch (global::System.Exception ex)
			{
				TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} SendCommandRawAsync Failed {command}: {ex.Message}", global::System.Array.Empty<object>());
			}
			return _responseSuccessNoResponse;
		}
		lock (_lock)
		{
			FlushCompletedCommands();
			if (_commandActiveDict.ContainsKey((int)command.ClientCommandId))
			{
				TaggedLog.Debug("DirectConnectionMyRvLink", LogPrefix + " SendCommandAsync failed because it is current running use ResendRunningCommandAsync to resend a command.", global::System.Array.Empty<object>());
				return new MyRvLinkCommandResponseFailure(command.ClientCommandId, MyRvLinkCommandResponseFailureCode.CommandAlreadyRunning);
			}
			if (_commandActiveDict.Count + 1 >= 20)
			{
				return new MyRvLinkCommandResponseFailure(command.ClientCommandId, MyRvLinkCommandResponseFailureCode.Offline);
			}
			int num = (int)((TimeSpan)(ref commandTimeout)).TotalMilliseconds;
			if (((global::System.Enum)commandOption).HasFlag((global::System.Enum)MyRvLinkSendCommandOption.ExtendedWait))
			{
				num = Math.Max(num, 16000);
			}
			commandTracker = new MyRvLinkCommandTracker(command, cancellationToken, num, responseAction);
			_commandActiveDict[(int)command.ClientCommandId] = commandTracker;
		}
		BackgroundOperation keepAliveBackgroundOperation = null;
		try
		{
			UpdateFrequencyMetricForCommandSend(command.CommandType);
			await SendCommandRawAsync(command, cancellationToken).ConfigureAwait(false);
			if (((global::System.Enum)commandOption).HasFlag((global::System.Enum)MyRvLinkSendCommandOption.DontWaitForResponse))
			{
				return new MyRvLinkCommandResponseSuccessNoWait(command.ClientCommandId);
			}
			if (((global::System.Enum)commandOption).HasFlag((global::System.Enum)MyRvLinkSendCommandOption.WaitForAnyResponse))
			{
				return await commandTracker.WaitForAnyResponse();
			}
			return await commandTracker.WaitAsync();
		}
		catch (TimeoutException)
		{
			TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} SendCommandAsync Timeout {command}", global::System.Array.Empty<object>());
			return commandTracker.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandTimeout);
		}
		catch (OperationCanceledException)
		{
			TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} SendCommandAsync Canceled {command}", global::System.Array.Empty<object>());
			return commandTracker.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandAborted);
		}
		catch (global::System.Exception ex4)
		{
			TaggedLog.Warning("DirectConnectionMyRvLink", $"{LogPrefix} SendCommandAsync Failure {command}: {ex4.Message}", global::System.Array.Empty<object>());
			return commandTracker.TrySetFailure(MyRvLinkCommandResponseFailureCode.Other);
		}
		finally
		{
			if (keepAliveBackgroundOperation != null)
			{
				keepAliveBackgroundOperation.Stop();
			}
		}
	}

	[AsyncStateMachine(typeof(_003CSendCommandAsync_003Ed__96))]
	public async global::System.Threading.Tasks.Task<CommandResult> SendCommandAsync(ILogicalDevice logicalDevice, Func<ValueTuple<byte, byte>, IMyRvLinkCommand> commandFactory, CancellationToken cancellationToken, MyRvLinkSendCommandOption commandOption = MyRvLinkSendCommandOption.None, Action<IMyRvLinkCommandResponse>? responseAction = null)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(_deviceTracker?.IsLogicalDeviceOnline(logicalDevice) ?? false))
			{
				return (CommandResult)6;
			}
			ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
			if (!myRvDeviceFromLogicalDevice.HasValue)
			{
				return (CommandResult)6;
			}
			IMyRvLinkCommand myRvLinkCommand = commandFactory.Invoke(myRvDeviceFromLogicalDevice.Value);
			IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(myRvLinkCommand, cancellationToken, commandOption, responseAction);
			TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} Sent command {myRvLinkCommand} received response {myRvLinkCommandResponse}", global::System.Array.Empty<object>());
			return myRvLinkCommandResponse.CommandResult;
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Warning("DirectConnectionMyRvLink", LogPrefix + " Sending command failed " + ex.Message, global::System.Array.Empty<object>());
			return (CommandResult)7;
		}
	}

	[AsyncStateMachine(typeof(_003CResendRunningCommandAsync_003Ed__97))]
	public async global::System.Threading.Tasks.Task<bool> ResendRunningCommandAsync(ushort commandId, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (commandId == 0 || commandId == 65535)
		{
			return false;
		}
		if (commandId != _currentCommandId)
		{
			return false;
		}
		MyRvLinkCommandTracker myRvLinkCommandTracker;
		lock (_lock)
		{
			FlushCompletedCommands();
			myRvLinkCommandTracker = DictionaryExtension.TryGetValue<int, MyRvLinkCommandTracker>((IReadOnlyDictionary<int, MyRvLinkCommandTracker>)(object)_commandActiveDict, (int)commandId);
			if (myRvLinkCommandTracker == null)
			{
				TaggedLog.Warning("DirectConnectionMyRvLink", $"{LogPrefix} Unable to {"ResendRunningCommandAsync"} because command tracker for {commandId} not found.", global::System.Array.Empty<object>());
				return false;
			}
			myRvLinkCommandTracker.ResetTimer();
		}
		if (myRvLinkCommandTracker.IsCompleted)
		{
			return false;
		}
		TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} Resend command {myRvLinkCommandTracker.Command}", global::System.Array.Empty<object>());
		UpdateFrequencyMetricForCommandSend(myRvLinkCommandTracker.Command.CommandType);
		await SendCommandRawAsync(myRvLinkCommandTracker.Command, cancellationToken);
		return true;
	}

	[AsyncStateMachine(typeof(_003CResendRunningCommandWaitForResponseAsync_003Ed__98))]
	public async global::System.Threading.Tasks.Task<IMyRvLinkCommandResponse?> ResendRunningCommandWaitForResponseAsync(ushort commandId, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (commandId == 0 || commandId == 65535)
		{
			return null;
		}
		if (commandId != _currentCommandId)
		{
			return null;
		}
		MyRvLinkCommandTracker commandTracker;
		lock (_lock)
		{
			FlushCompletedCommands();
			commandTracker = DictionaryExtension.TryGetValue<int, MyRvLinkCommandTracker>((IReadOnlyDictionary<int, MyRvLinkCommandTracker>)(object)_commandActiveDict, (int)commandId);
			if (commandTracker == null)
			{
				TaggedLog.Warning("DirectConnectionMyRvLink", $"{LogPrefix} Unable to {"ResendRunningCommandAsync"} because command tracker for {commandId} not found.", global::System.Array.Empty<object>());
				return null;
			}
			commandTracker.ResetTimer();
		}
		if (commandTracker.IsCompleted)
		{
			return null;
		}
		TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} Resend command {commandTracker.Command}", global::System.Array.Empty<object>());
		try
		{
			UpdateFrequencyMetricForCommandSend(commandTracker.Command.CommandType);
			await SendCommandRawAsync(commandTracker.Command, cancellationToken).ConfigureAwait(false);
			IMyRvLinkCommandResponse obj = await commandTracker.WaitForAnyResponse();
			if (obj is IMyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
			{
				TaggedLog.Warning("DirectConnectionMyRvLink", $"{LogPrefix} ResendRunningCommandWaitForResponseAsync Failed with response: {myRvLinkCommandResponseFailure}", global::System.Array.Empty<object>());
			}
			return obj;
		}
		catch (TimeoutException)
		{
			TaggedLog.Warning("DirectConnectionMyRvLink", $"{LogPrefix} ResendRunningCommandWaitForResponseAsync Timeout {commandTracker.Command}", global::System.Array.Empty<object>());
			return commandTracker.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandTimeout);
		}
		catch (OperationCanceledException)
		{
			TaggedLog.Warning("DirectConnectionMyRvLink", $"{LogPrefix} ResendRunningCommandWaitForResponseAsync Canceled {commandTracker.Command}", global::System.Array.Empty<object>());
			return commandTracker.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandAborted);
		}
		catch (global::System.Exception ex3)
		{
			TaggedLog.Warning("DirectConnectionMyRvLink", $"{LogPrefix} ResendRunningCommandWaitForResponseAsync Failure {commandTracker.Command}: {ex3.Message}", global::System.Array.Empty<object>());
			return commandTracker.TrySetFailure(MyRvLinkCommandResponseFailureCode.Other);
		}
	}

	[AsyncStateMachine(typeof(_003CWaitForRunningCommandToComplete_003Ed__99))]
	public async global::System.Threading.Tasks.Task<IMyRvLinkCommandResponse> WaitForRunningCommandToComplete(ushort commandId, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (commandId == 0 || commandId == 65535)
		{
			return new MyRvLinkCommandResponseFailure(commandId, MyRvLinkCommandResponseFailureCode.InvalidCommand);
		}
		MyRvLinkCommandTracker commandTracker;
		lock (_lock)
		{
			FlushCompletedCommands();
			commandTracker = DictionaryExtension.TryGetValue<int, MyRvLinkCommandTracker>((IReadOnlyDictionary<int, MyRvLinkCommandTracker>)(object)_commandActiveDict, (int)commandId);
			if (commandTracker == null)
			{
				TaggedLog.Warning("DirectConnectionMyRvLink", $"{LogPrefix} Unable to {"WaitForRunningCommandToComplete"} because command tracker for {commandId} not found.", global::System.Array.Empty<object>());
				return new MyRvLinkCommandResponseFailure(commandId, MyRvLinkCommandResponseFailureCode.InvalidCommand);
			}
			commandTracker.ResetTimer();
		}
		try
		{
			TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} Wait for command to complete {commandTracker.Command}", global::System.Array.Empty<object>());
			return await commandTracker.WaitAsync();
		}
		catch (TimeoutException)
		{
			TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} SendCommandAsync Timeout {commandTracker.Command}", global::System.Array.Empty<object>());
			return commandTracker.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandTimeout);
		}
		catch (OperationCanceledException)
		{
			TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} SendCommandAsync Canceled {commandTracker.Command}", global::System.Array.Empty<object>());
			return commandTracker.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandAborted);
		}
		catch (global::System.Exception ex3)
		{
			TaggedLog.Warning("DirectConnectionMyRvLink", $"{LogPrefix} SendCommandAsync Failure {commandTracker.Command}: {ex3.Message}", global::System.Array.Empty<object>());
			return commandTracker.TrySetFailure(MyRvLinkCommandResponseFailureCode.Other);
		}
	}

	protected abstract global::System.Threading.Tasks.Task SendCommandRawAsync(IMyRvLinkCommand command, CancellationToken cancellationToken);

	private void FlushCompletedCommands()
	{
		lock (_lock)
		{
			Enumerable.ToList<KeyValuePair<int, MyRvLinkCommandTracker>>(Enumerable.Where<KeyValuePair<int, MyRvLinkCommandTracker>>((global::System.Collections.Generic.IEnumerable<KeyValuePair<int, MyRvLinkCommandTracker>>)_commandActiveDict, (Func<KeyValuePair<int, MyRvLinkCommandTracker>, bool>)((KeyValuePair<int, MyRvLinkCommandTracker> commandTrackerEntry) => commandTrackerEntry.Value.IsCompleted))).ForEach((Action<KeyValuePair<int, MyRvLinkCommandTracker>>)([CompilerGenerated] (KeyValuePair<int, MyRvLinkCommandTracker> commandTrackerEntry) =>
			{
				((CommonDisposable)commandTrackerEntry.Value).TryDispose();
				_commandActiveDict.Remove(commandTrackerEntry.Key);
				_commandCompletedQueue.TryAdd(commandTrackerEntry.Value);
			}));
		}
	}

	private void AbortAllPendingCommands()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			MyRvLinkDeviceTracker? deviceTracker = _deviceTracker;
			if (deviceTracker != null && deviceTracker.DeviceList.Count == 0)
			{
				((CommonDisposable)_deviceTracker).Dispose();
				_deviceTracker = null;
			}
			Enumerator<int, MyRvLinkCommandTracker> enumerator = _commandActiveDict.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, MyRvLinkCommandTracker> current = enumerator.Current;
					if (!current.Value.IsCompleted)
					{
						current.Value.TrySetFailure(MyRvLinkCommandResponseFailureCode.CommandAborted);
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
			_commandActiveDict.Clear();
		}
	}

	public void DebugDumpRunningCommands()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			if (_commandActiveDict.Count == 0)
			{
				TaggedLog.Information("DirectConnectionMyRvLink", LogPrefix + " No Commands", global::System.Array.Empty<object>());
				return;
			}
			Enumerator<int, MyRvLinkCommandTracker> enumerator = _commandActiveDict.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, MyRvLinkCommandTracker> current = enumerator.Current;
					TaggedLog.Information("DirectConnectionMyRvLink", $"{LogPrefix} {current.Key}: {current.Value}", global::System.Array.Empty<object>());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
		}
	}

	public ValueTuple<byte, byte>? GetMyRvDeviceFromLogicalDevice(ILogicalDevice logicalDevice)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		MyRvLinkDeviceTracker deviceTracker = _deviceTracker;
		byte? b = deviceTracker?.GetMyRvDeviceIdFromLogicalDevice(logicalDevice);
		if (!b.HasValue)
		{
			return null;
		}
		return new ValueTuple<byte, byte>(deviceTracker.DeviceTableId, b.Value);
	}

	public ILogicalDevice? GetLogicalDeviceFromMyRvDevice(byte deviceTableId, byte deviceId)
	{
		return _deviceTracker?.GetLogicalDeviceFromMyRvDevice(deviceTableId, deviceId);
	}

	public global::System.Collections.Generic.IEnumerable<ILogicalDeviceTag> MakeDeviceSourceTags(ILogicalDevice? logicalDevice)
	{
		return DeviceSourceTags;
	}

	public virtual LogicalDeviceReachability DeviceSourceReachability(ILogicalDevice logicalDevice)
	{
		if (logicalDevice.IsAssociatedWithDeviceSource((ILogicalDeviceSource)(object)this))
		{
			if (IsDeviceSourceActive)
			{
				if (IsLogicalDeviceOnline(logicalDevice))
				{
					return (LogicalDeviceReachability)1;
				}
				return (LogicalDeviceReachability)0;
			}
			return (LogicalDeviceReachability)0;
		}
		return (LogicalDeviceReachability)2;
	}

	public override string ToString()
	{
		return "DirectConnectionMyRvLink " + LogPrefix;
	}

	public global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandAccessoryGatewayAsync(ILogicalDeviceAccessoryGateway logicalDevice, LogicalDeviceAccessoryGatewayCommand command, CancellationToken cancelToken)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return SendCommandAsync((ILogicalDevice)(object)logicalDevice, (ValueTuple<byte, byte> myRvLinkDevice) => new MyRvLinkCommandActionAccessoryGateway(GetNextCommandId(), myRvLinkDevice.Item1, myRvLinkDevice.Item2, command), cancelToken);
	}

	public global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandClimateZoneAsync(ILogicalDeviceClimateZone logicalDevice, LogicalDeviceClimateZoneCommand command, CancellationToken cancelToken)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return SendCommandAsync((ILogicalDevice)(object)logicalDevice, (ValueTuple<byte, byte> myRvLinkDevice) => new MyRvLinkCommandActionHvac(GetNextCommandId(), myRvLinkDevice.Item1, myRvLinkDevice.Item2, command), cancelToken);
	}

	public global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandGeneratorGenie(ILogicalDeviceGeneratorGenie logicalDevice, GeneratorGenieCommand command, CancellationToken cancelToken)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return SendCommandAsync((ILogicalDevice)(object)logicalDevice, (ValueTuple<byte, byte> myRvLinkDevice) => new MyRvLinkCommandActionGeneratorGenie(GetNextCommandId(), myRvLinkDevice.Item1, myRvLinkDevice.Item2, command), cancelToken);
	}

	[AsyncStateMachine(typeof(_003CSendDirectCommandLeveler5Async_003Ed__134))]
	public async global::System.Threading.Tasks.Task<LevelerCommandResultType5> SendDirectCommandLeveler5Async(ILogicalDeviceLevelerType5 logicalDevice, ILogicalDeviceLevelerCommandType5 command, CancellationToken cancelToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!(_deviceTracker?.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice) ?? false))
		{
			return new LevelerCommandResultType5((CommandResult)6);
		}
		ValueTuple<byte, byte>? myRvLinkDevice = GetMyRvDeviceFromLogicalDevice((ILogicalDevice)(object)logicalDevice);
		if (!myRvLinkDevice.HasValue)
		{
			return new LevelerCommandResultType5((CommandResult)6);
		}
		try
		{
			MyRvLinkCommandContext<LevelerCommandCode> commandContext;
			lock (this)
			{
				object obj = default(object);
				if (((ILogicalDevice)logicalDevice).CustomData.TryGetValue("DirectConnectionMyRvLink.IDirectCommandLeveler5", ref obj) && obj is MyRvLinkCommandContext<LevelerCommandCode> myRvLinkCommandContext)
				{
					commandContext = myRvLinkCommandContext;
				}
				else
				{
					ConcurrentDictionary<string, object> customData = ((ILogicalDevice)logicalDevice).CustomData;
					MyRvLinkCommandContext<LevelerCommandCode> myRvLinkCommandContext2;
					commandContext = (myRvLinkCommandContext2 = new MyRvLinkCommandContext<LevelerCommandCode>());
					customData["DirectConnectionMyRvLink.IDirectCommandLeveler5"] = myRvLinkCommandContext2;
				}
			}
			if (commandContext.LastSentCommandReceivedError)
			{
				commandContext.ClearLastSentCommandReceivedError();
			}
			bool flag = commandContext.CanResendCommand(command.Command, (IDeviceCommandPacket?)(object)command);
			IMyRvLinkCommandResponse myRvLinkCommandResponse = default(IMyRvLinkCommandResponse);
			if (flag)
			{
				myRvLinkCommandResponse = await ResendRunningCommandWaitForResponseAsync(commandContext.SentCommandId, cancelToken).ConfigureAwait(false);
				flag = myRvLinkCommandResponse != null;
			}
			if (flag)
			{
				commandContext.SentCommand(commandContext.SentCommandId, command.Command, (IDeviceCommandPacket?)(object)command);
				return (myRvLinkCommandResponse is IMyRvLinkCommandResponseSuccess) ? new LevelerCommandResultType5((CommandResult)0) : ((myRvLinkCommandResponse is MyRvLinkCommandLeveler5ResponseFailure myRvLinkCommandLeveler5ResponseFailure) ? new LevelerCommandResultType5((CommandResult)7, myRvLinkCommandLeveler5ResponseFailure.LevelerFault) : ((!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure)) ? new LevelerCommandResultType5((CommandResult)7) : new LevelerCommandResultType5((CommandResult)7)));
			}
			ushort commandId = GetNextCommandId();
			MyRvLinkCommandLeveler5 command2 = new MyRvLinkCommandLeveler5(commandId, myRvLinkDevice.Value.Item1, myRvLinkDevice.Value.Item2, command);
			commandContext.SentCommand(commandId, command.Command, (IDeviceCommandPacket?)(object)command);
			IMyRvLinkCommandResponse myRvLinkCommandResponse2 = await SendCommandAsync(command2, cancelToken, TimeSpan.FromMilliseconds(2500.0), MyRvLinkSendCommandOption.WaitForAnyResponse).ConfigureAwait(false);
			if (!(myRvLinkCommandResponse2 is IMyRvLinkCommandResponseSuccess))
			{
				if (!(myRvLinkCommandResponse2 is MyRvLinkCommandLeveler5ResponseFailure myRvLinkCommandLeveler5ResponseFailure2))
				{
					if (myRvLinkCommandResponse2 is IMyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure)
					{
						commandContext.ProcessResponse(myRvLinkCommandResponse2);
						TaggedLog.Information("DirectConnectionMyRvLink", $"{LogPrefix} - Leveler 5 command failure generic {myRvLinkCommandResponseFailure}", global::System.Array.Empty<object>());
						return new LevelerCommandResultType5((CommandResult)7);
					}
					commandContext.ProcessResponse(new MyRvLinkCommandResponseFailure(commandId, MyRvLinkCommandResponseFailureCode.CommandFailed));
					TaggedLog.Information("DirectConnectionMyRvLink", $"{LogPrefix} - Leveler 5 command failure unknown {myRvLinkCommandResponse2}", global::System.Array.Empty<object>());
					return new LevelerCommandResultType5((CommandResult)7);
				}
				commandContext.ProcessResponse(myRvLinkCommandResponse2);
				TaggedLog.Information("DirectConnectionMyRvLink", $"{LogPrefix} - Leveler 5 command failure {myRvLinkCommandLeveler5ResponseFailure2}", global::System.Array.Empty<object>());
				return new LevelerCommandResultType5((CommandResult)7, myRvLinkCommandLeveler5ResponseFailure2.LevelerFault);
			}
			commandContext.ProcessResponse(myRvLinkCommandResponse2);
			return new LevelerCommandResultType5((CommandResult)0);
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Warning("DirectConnectionMyRvLink", LogPrefix + " Sending command failed " + ex.Message, global::System.Array.Empty<object>());
			return new LevelerCommandResultType5((CommandResult)7);
		}
	}

	public global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandLightDimmable(ILogicalDeviceLightDimmable logicalDevice, LogicalDeviceLightDimmableCommand command, CancellationToken cancelToken)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return SendCommandAsync((ILogicalDevice)(object)logicalDevice, (ValueTuple<byte, byte> myRvLinkDevice) => new MyRvLinkCommandActionDimmable(GetNextCommandId(), myRvLinkDevice.Item1, myRvLinkDevice.Item2, command), cancelToken);
	}

	public global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandLightRgb(ILogicalDeviceLightRgb logicalDevice, LogicalDeviceLightRgbCommand command, CancellationToken cancelToken)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return SendCommandAsync((ILogicalDevice)(object)logicalDevice, (ValueTuple<byte, byte> myRvLinkDevice) => new MyRvLinkCommandActionRgb(GetNextCommandId(), myRvLinkDevice.Item1, myRvLinkDevice.Item2, command), cancelToken);
	}

	[AsyncStateMachine(typeof(_003CPidReadAsync_003Ed__140))]
	public async global::System.Threading.Tasks.Task<UInt48> PidReadAsync(ILogicalDevice logicalDevice, Pid pid, Action<float, string> readProgress, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (!IsStarted)
		{
			throw new MyRvLinkDeviceServiceNotStartedException(this, $"Unable to Read PID value {pid}");
		}
		if (!IsConnected)
		{
			throw new MyRvLinkDeviceServiceNotConnectedException(this, $"Unable to Read PID value {pid}");
		}
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			throw new MyRvLinkDeviceOfflineException(this, logicalDevice);
		}
		if (_firmwareUpdateInProgress)
		{
			throw new MyRvLinkPidReadException("Can't perform Pid reads while a firmware update is in progress!");
		}
		ValueTuple<byte, byte>? myRvLinkDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!myRvLinkDevice.HasValue)
		{
			throw new MyRvLinkDeviceNotFoundException(this, logicalDevice);
		}
		TaskSerialLock val = await _pidSerialQueue.GetLock(cancellationToken);
		try
		{
			TimeSpan elapsed = _pidLastOperationTimer.Elapsed;
			int num = 100 - (int)((TimeSpan)(ref elapsed)).TotalMilliseconds;
			if (num > 0)
			{
				await TaskExtension.TryDelay(num, cancellationToken);
			}
			((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
			MyRvLinkCommandGetDevicePid command = new MyRvLinkCommandGetDevicePid(GetNextCommandId(), myRvLinkDevice.Value.Item1, myRvLinkDevice.Value.Item2, pid);
			IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
			TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} PidReadAsync Response {myRvLinkCommandResponse} Last operation was performed {_pidLastOperationTimer.ElapsedMilliseconds}ms ago", global::System.Array.Empty<object>());
			_pidLastOperationTimer.Restart();
			if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure failure))
			{
				if (myRvLinkCommandResponse is MyRvLinkCommandGetDevicePidResponseCompleted myRvLinkCommandGetDevicePidResponseCompleted)
				{
					if (PidExtension.IsAutoCacheingPid(pid))
					{
						logicalDevice.SetCachedPidRawValue(pid, myRvLinkCommandGetDevicePidResponseCompleted.PidValue);
					}
					return myRvLinkCommandGetDevicePidResponseCompleted.PidValue;
				}
				throw new MyRvLinkCommandResponseFailureException(new MyRvLinkCommandResponseFailure(command.ClientCommandId, MyRvLinkCommandResponseFailureCode.InvalidResponse));
			}
			throw new MyRvLinkCommandResponseFailureException(failure);
		}
		finally
		{
			((global::System.IDisposable)val)?.Dispose();
		}
	}

	[AsyncStateMachine(typeof(_003CPidWriteAsync_003Ed__141))]
	public global::System.Threading.Tasks.Task PidWriteAsync(ILogicalDevice logicalDevice, Pid pid, UInt48 pidValue, LogicalDeviceSessionType pidWriteAccess, Action<float, string> writeProgress, CancellationToken cancellationToken)
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
		_003CPidWriteAsync_003Ed__141 _003CPidWriteAsync_003Ed__ = default(_003CPidWriteAsync_003Ed__141);
		_003CPidWriteAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CPidWriteAsync_003Ed__._003C_003E4__this = this;
		_003CPidWriteAsync_003Ed__.logicalDevice = logicalDevice;
		_003CPidWriteAsync_003Ed__.pid = pid;
		_003CPidWriteAsync_003Ed__.pidValue = pidValue;
		_003CPidWriteAsync_003Ed__.pidWriteAccess = pidWriteAccess;
		_003CPidWriteAsync_003Ed__.cancellationToken = cancellationToken;
		_003CPidWriteAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CPidWriteAsync_003Ed__._003C_003Et__builder)).Start<_003CPidWriteAsync_003Ed__141>(ref _003CPidWriteAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CPidWriteAsync_003Ed__._003C_003Et__builder)).Task;
	}

	[AsyncStateMachine(typeof(_003CPidReadAsync_003Ed__142))]
	public async global::System.Threading.Tasks.Task<uint> PidReadAsync(ILogicalDevice logicalDevice, Pid pid, ushort address, Action<float, string> readProgress, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (!IsStarted)
		{
			throw new MyRvLinkDeviceServiceNotStartedException(this, $"Unable to Read PID value {pid} with Address {address}");
		}
		if (!IsConnected)
		{
			throw new MyRvLinkDeviceServiceNotConnectedException(this, $"Unable to Read PID value {pid} with Address {address}");
		}
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			throw new MyRvLinkDeviceOfflineException(this, logicalDevice);
		}
		if (_firmwareUpdateInProgress)
		{
			throw new MyRvLinkPidReadException("Can't perform Pid reads while a firmware update is in progress!");
		}
		ValueTuple<byte, byte>? myRvLinkDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!myRvLinkDevice.HasValue)
		{
			throw new MyRvLinkDeviceNotFoundException(this, logicalDevice);
		}
		TaskSerialLock val = await _pidSerialQueue.GetLock(cancellationToken);
		try
		{
			TimeSpan elapsed = _pidLastOperationTimer.Elapsed;
			int num = 100 - (int)((TimeSpan)(ref elapsed)).TotalMilliseconds;
			if (num > 0)
			{
				await TaskExtension.TryDelay(num, cancellationToken);
			}
			((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
			MyRvLinkCommandGetDevicePidWithAddress command = new MyRvLinkCommandGetDevicePidWithAddress(GetNextCommandId(), myRvLinkDevice.Value.Item1, myRvLinkDevice.Value.Item2, pid, address);
			IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
			TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} PidReadAsync Response {myRvLinkCommandResponse} Last operation was performed {_pidLastOperationTimer.ElapsedMilliseconds}ms ago", global::System.Array.Empty<object>());
			_pidLastOperationTimer.Restart();
			if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure failure))
			{
				if (myRvLinkCommandResponse is MyRvLinkCommandGetDevicePidWithAddressResponseCompleted myRvLinkCommandGetDevicePidWithAddressResponseCompleted)
				{
					return myRvLinkCommandGetDevicePidWithAddressResponseCompleted.PidValue;
				}
				throw new MyRvLinkCommandResponseFailureException(new MyRvLinkCommandResponseFailure(command.ClientCommandId, MyRvLinkCommandResponseFailureCode.InvalidResponse));
			}
			throw new MyRvLinkCommandResponseFailureException(failure);
		}
		finally
		{
			((global::System.IDisposable)val)?.Dispose();
		}
	}

	[AsyncStateMachine(typeof(_003CPidWriteAsync_003Ed__143))]
	public global::System.Threading.Tasks.Task PidWriteAsync(ILogicalDevice logicalDevice, Pid pid, ushort address, uint pidValue, LogicalDeviceSessionType pidWriteAccess, Action<float, string> writeProgress, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		_003CPidWriteAsync_003Ed__143 _003CPidWriteAsync_003Ed__ = default(_003CPidWriteAsync_003Ed__143);
		_003CPidWriteAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CPidWriteAsync_003Ed__._003C_003E4__this = this;
		_003CPidWriteAsync_003Ed__.logicalDevice = logicalDevice;
		_003CPidWriteAsync_003Ed__.pid = pid;
		_003CPidWriteAsync_003Ed__.address = address;
		_003CPidWriteAsync_003Ed__.pidValue = pidValue;
		_003CPidWriteAsync_003Ed__.pidWriteAccess = pidWriteAccess;
		_003CPidWriteAsync_003Ed__.cancellationToken = cancellationToken;
		_003CPidWriteAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CPidWriteAsync_003Ed__._003C_003Et__builder)).Start<_003CPidWriteAsync_003Ed__143>(ref _003CPidWriteAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CPidWriteAsync_003Ed__._003C_003Et__builder)).Task;
	}

	[AsyncStateMachine(typeof(_003CGetDevicePidListAsync_003Ed__144))]
	public async global::System.Threading.Tasks.Task<IReadOnlyDictionary<Pid, PidAccess>> GetDevicePidListAsync(ILogicalDevice logicalDevice, CancellationToken cancellationToken, Pid startPidId = (Pid)0, Pid endPidId = (Pid)0)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (!IsStarted)
		{
			throw new MyRvLinkDeviceServiceNotStartedException(this, "Unable to Get PID List");
		}
		if (!IsConnected)
		{
			throw new MyRvLinkDeviceServiceNotConnectedException(this, "Unable to Get PID List");
		}
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			throw new MyRvLinkDeviceOfflineException(this, logicalDevice);
		}
		ValueTuple<byte, byte>? myRvDeviceFromLogicalDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!myRvDeviceFromLogicalDevice.HasValue)
		{
			throw new MyRvLinkDeviceNotFoundException(this, logicalDevice);
		}
		MyRvLinkCommandGetDevicePidList command = new MyRvLinkCommandGetDevicePidList(GetNextCommandId(), myRvDeviceFromLogicalDevice.Value.Item1, myRvDeviceFromLogicalDevice.Value.Item2, startPidId, endPidId);
		IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
		if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure failure))
		{
			if (myRvLinkCommandResponse is MyRvLinkCommandGetDevicePidListResponseCompleted)
			{
				return (IReadOnlyDictionary<Pid, PidAccess>)(object)command.PidDict;
			}
			throw new MyRvLinkException($"Failed to Get PID Values from {logicalDevice}: Unknown result");
		}
		throw new MyRvLinkCommandResponseFailureException(failure);
	}

	public global::System.Threading.Tasks.Task<string> GetSoftwarePartNumberAsync(ILogicalDevice logicalDevice, CancellationToken cancelToken)
	{
		IMyRvLinkDeviceForLogicalDevice myRvLinkDeviceForLogicalDevice = _deviceTracker?.GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!(myRvLinkDeviceForLogicalDevice is MyRvLinkDeviceHost myRvLinkDeviceHost))
		{
			if (myRvLinkDeviceForLogicalDevice is MyRvLinkDeviceIdsCan myRvLinkDeviceIdsCan)
			{
				return global::System.Threading.Tasks.Task.FromResult<string>(myRvLinkDeviceIdsCan.MetaData?.SoftwarePartNumber ?? throw new MyRvLinkException($"Metadata not loaded for {logicalDevice}"));
			}
			throw new MyRvLinkException($"IDS CAN device not found for {logicalDevice}");
		}
		return global::System.Threading.Tasks.Task.FromResult<string>(myRvLinkDeviceHost.MetaData?.SoftwarePartNumber ?? throw new MyRvLinkException($"Metadata not loaded for {logicalDevice}"));
	}

	public Version? GetDeviceProtocolVersion(ILogicalDevice logicalDevice)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		IMyRvLinkDeviceForLogicalDevice myRvLinkDeviceForLogicalDevice = _deviceTracker?.GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!(myRvLinkDeviceForLogicalDevice is MyRvLinkDeviceHost myRvLinkDeviceHost))
		{
			if (myRvLinkDeviceForLogicalDevice is MyRvLinkDeviceIdsCan myRvLinkDeviceIdsCan)
			{
				IDS_CAN_VERSION_NUMBER val = IDS_CAN_VERSION_NUMBER.op_Implicit((myRvLinkDeviceIdsCan.MetaData ?? throw new MyRvLinkException($"Metadata not loaded for {logicalDevice}")).IdsCanVersion);
				return new Version(val.Major, val.Minor);
			}
			throw new MyRvLinkException($"IDS CAN device not found for {logicalDevice}");
		}
		IDS_CAN_VERSION_NUMBER val2 = IDS_CAN_VERSION_NUMBER.op_Implicit((myRvLinkDeviceHost.MetaData ?? throw new MyRvLinkException($"Metadata not loaded for {logicalDevice}")).IdsCanVersion);
		return new Version(val2.Major, val2.Minor);
	}

	[AsyncStateMachine(typeof(_003CGetDtcValuesAsync_003Ed__150))]
	public async global::System.Threading.Tasks.Task<IReadOnlyDictionary<DTC_ID, DtcValue>> GetDtcValuesAsync(ILogicalDevice logicalDevice, LogicalDeviceDtcFilter dtcFilter, DTC_ID startDtcId, DTC_ID endDtcId, CancellationToken cancellationToken)
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
		if (!IsStarted)
		{
			throw new MyRvLinkDeviceServiceNotStartedException(this, $"Unable to Get DTC value {startDtcId} - {endDtcId}");
		}
		if (!IsConnected)
		{
			throw new MyRvLinkDeviceServiceNotConnectedException(this, $"Unable to Get DTC value {startDtcId} - {endDtcId}");
		}
		if (!IsLogicalDeviceOnline(logicalDevice))
		{
			throw new MyRvLinkDeviceOfflineException(this, logicalDevice);
		}
		ValueTuple<byte, byte>? myRvLinkDevice = GetMyRvDeviceFromLogicalDevice(logicalDevice);
		if (!myRvLinkDevice.HasValue)
		{
			throw new MyRvLinkDeviceNotFoundException(this, logicalDevice);
		}
		TaskSerialLock val = await _dtcSerialQueue.GetLock(cancellationToken);
		try
		{
			if (_dtcThrottleStopwatch.IsRunning && _dtcThrottleStopwatch.ElapsedMilliseconds < 500)
			{
				long num = 500 - _dtcThrottleStopwatch.ElapsedMilliseconds;
				if (num > 0)
				{
					TaggedLog.Information("DirectConnectionMyRvLink", $"{LogPrefix} DTC Get Value Request Throttled for {num}ms", global::System.Array.Empty<object>());
					await global::System.Threading.Tasks.Task.Delay((int)num, cancellationToken);
				}
			}
			_dtcThrottleStopwatch.Stop();
			MyRvLinkCommandGetProductDtcValues command = new MyRvLinkCommandGetProductDtcValues(GetNextCommandId(), myRvLinkDevice.Value.Item1, myRvLinkDevice.Value.Item2, dtcFilter, startDtcId, endDtcId);
			IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
			if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure myRvLinkCommandResponseFailure))
			{
				if (myRvLinkCommandResponse is MyRvLinkCommandGetProductDtcValuesResponseCompleted)
				{
					return (IReadOnlyDictionary<DTC_ID, DtcValue>)(object)command.DtcDict;
				}
				throw new MyRvLinkException($"Failed to Get DTC Values from {logicalDevice}: Unknown result");
			}
			if (myRvLinkCommandResponseFailure.FailureCode == MyRvLinkCommandResponseFailureCode.TooManyCommandsRunning)
			{
				_dtcThrottleStopwatch.Restart();
			}
			throw new MyRvLinkCommandResponseFailureException(myRvLinkCommandResponseFailure);
		}
		finally
		{
			((global::System.IDisposable)val)?.Dispose();
		}
	}

	protected void OnReceivedEvent(IMyRvLinkEvent myRvLinkEvent)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (myRvLinkEvent == null)
		{
			throw new ArgumentNullException("myRvLinkEvent");
		}
		if (!IsStarted)
		{
			throw new MyRvLinkDeviceServiceNotStartedException(this, "OnReceivedEvent called when service is stopped");
		}
		if (!IsConnected)
		{
			throw new MyRvLinkDeviceServiceNotConnectedException(this, "OnReceivedEvent called when connection isn't open");
		}
		_receivedEventMetrics.Update();
		UpdateFrequencyMetricForEvent(myRvLinkEvent.EventType);
		MyRvLinkVersionTracker versionTracker = _versionTracker;
		if (versionTracker == null)
		{
			throw new MyRvLinkException("OnReceivedEvent No Version Tracker Setup!");
		}
		versionTracker.GetVersionIfNeeded();
		if (!(myRvLinkEvent is MyRvLinkGatewayInformation gatewayInfo))
		{
			if (!(myRvLinkEvent is MyRvLinkRealTimeClock myRvLinkRealTimeClock))
			{
				if (!(myRvLinkEvent is MyRvLinkRvStatus rvStatus))
				{
					if (!(myRvLinkEvent is MyRvLinkHostDebug))
					{
						if (myRvLinkEvent is IMyRvLinkCommandEvent myRvLinkCommandEvent)
						{
							if (!HasMinimumExpectedProtocolVersion)
							{
								return;
							}
							ushort commandId = myRvLinkCommandEvent.ClientCommandId;
							MyRvLinkCommandTracker myRvLinkCommandTracker = default(MyRvLinkCommandTracker);
							if (!_commandActiveDict.TryGetValue((int)commandId, ref myRvLinkCommandTracker))
							{
								MyRvLinkCommandTracker myRvLinkCommandTracker2 = Enumerable.FirstOrDefault<MyRvLinkCommandTracker>((global::System.Collections.Generic.IEnumerable<MyRvLinkCommandTracker>)_commandCompletedQueue, (Func<MyRvLinkCommandTracker, bool>)((MyRvLinkCommandTracker commandTracker) => commandTracker.Command.ClientCommandId == commandId));
								if (myRvLinkCommandTracker2 != null)
								{
									TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} Received event for command 0x{commandId:X4} that no longer exists.  Discarding event{Environment.NewLine}Command: {myRvLinkCommandTracker2}{Environment.NewLine} Event Discarded: {myRvLinkCommandEvent}", global::System.Array.Empty<object>());
								}
								else
								{
									TaggedLog.Warning("DirectConnectionMyRvLink", $"{LogPrefix} Received event for command 0x{commandId:X4} that no longer exists.  Discarding event for UNKNOWN COMMAND{Environment.NewLine}{myRvLinkCommandEvent}", global::System.Array.Empty<object>());
								}
							}
							else if (myRvLinkCommandEvent is IMyRvLinkCommandResponse myRvLinkCommandResponse)
							{
								if (myRvLinkCommandTracker.IsCompleted)
								{
									TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} Received event for command 0x{commandId:X} that has already been completed.  Discarding event{Environment.NewLine}{myRvLinkCommandResponse}", global::System.Array.Empty<object>());
									return;
								}
								if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure))
								{
									if (!(myRvLinkCommandResponse is MyRvLinkCommandGetProductDtcValuesResponse) && !(myRvLinkCommandResponse is MyRvLinkCommandGetDevicePidResponseCompleted))
									{
										if (myRvLinkCommandResponse is IMyRvLinkCommandResponseSuccess myRvLinkCommandResponseSuccess)
										{
											TaggedLog.Information("DirectConnectionMyRvLink", $"{LogPrefix} Event Received Success: Command(0x{myRvLinkCommandResponse.ClientCommandId:X4}) {((MemberInfo)((object)myRvLinkEvent).GetType()).Name} Completed: {myRvLinkCommandResponse.IsCommandCompleted}{Environment.NewLine}{myRvLinkCommandResponseSuccess}", global::System.Array.Empty<object>());
										}
										else
										{
											TaggedLog.Error("DirectConnectionMyRvLink", LogPrefix + " Event Received Other: " + ((MemberInfo)((object)myRvLinkEvent).GetType()).Name, global::System.Array.Empty<object>());
										}
									}
								}
								else
								{
									TaggedLog.Warning("DirectConnectionMyRvLink", $"{LogPrefix} Event Received FAILURE: {myRvLinkEvent}", global::System.Array.Empty<object>());
									UpdateFrequencyMetricForCommandFailure(myRvLinkCommandTracker.Command.CommandType);
								}
								bool flag = myRvLinkCommandTracker.Command.ProcessResponse(myRvLinkCommandResponse);
								myRvLinkCommandTracker.ProcessResponse(myRvLinkCommandResponse, flag);
								if (flag || myRvLinkCommandEvent.IsCommandCompleted)
								{
									FlushCompletedCommands();
								}
							}
							else
							{
								TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} Event Received for unknown Command Event {myRvLinkCommandEvent}: {myRvLinkEvent}", global::System.Array.Empty<object>());
							}
						}
						else
						{
							if (!versionTracker.IsVersionSupported)
							{
								return;
							}
							MyRvLinkDeviceTracker deviceTracker = _deviceTracker;
							if (deviceTracker == null || !deviceTracker.IsDeviceLoadComplete || !deviceTracker.IsActive || myRvLinkEvent is MyRvLinkGatewayInformation || myRvLinkEvent is IMyRvLinkCommandEvent || myRvLinkEvent is MyRvLinkRealTimeClock || myRvLinkEvent is MyRvLinkRvStatus || myRvLinkEvent is MyRvLinkHostDebug)
							{
								return;
							}
							if (!(myRvLinkEvent is MyRvLinkDeviceOnlineStatus deviceOnlineStatus))
							{
								if (!(myRvLinkEvent is MyRvLinkDeviceLockStatus deviceLockStatus))
								{
									if (!(myRvLinkEvent is MyRvLinkRelayBasicLatchingStatusType1 latchingRelayStatus))
									{
										if (!(myRvLinkEvent is MyRvLinkRelayBasicLatchingStatusType2 latchingRelayStatus2))
										{
											if (!(myRvLinkEvent is MyRvLinkRelayHBridgeMomentaryStatusType1 momentaryRelayStatus))
											{
												if (!(myRvLinkEvent is MyRvLinkRelayHBridgeMomentaryStatusType2 momentaryRelayStatus2))
												{
													if (!(myRvLinkEvent is MyRvLinkTankSensorStatus tankSensorStatus))
													{
														if (!(myRvLinkEvent is MyRvLinkTankSensorStatusV2 tankSensorStatus2))
														{
															if (!(myRvLinkEvent is MyRvLinkDimmableLightStatus dimmableLightStatus))
															{
																if (!(myRvLinkEvent is MyRvLinkRgbLightStatus rgbLightStatus))
																{
																	if (!(myRvLinkEvent is MyRvLinkHvacStatus hvacStatus))
																	{
																		if (!(myRvLinkEvent is MyRvLinkHourMeterStatus tankSensorStatus3))
																		{
																			if (!(myRvLinkEvent is MyRvLinkGeneratorGenieStatus generatorGenieStatus))
																			{
																				if (!(myRvLinkEvent is MyRvLinkCloudGatewayStatus cloudGatewayStatus))
																				{
																					if (!(myRvLinkEvent is MyRvLinkLeveler4Status levelerStatus))
																					{
																						if (!(myRvLinkEvent is MyRvLinkLeveler5Status levelerStatus2))
																						{
																							if (!(myRvLinkEvent is MyRvLinkLevelerType5ExtendedStatus progressStatus))
																							{
																								if (!(myRvLinkEvent is MyRvLinkLevelerConsoleText levelerConsoleText))
																								{
																									if (!(myRvLinkEvent is MyRvLinkLeveler3Status levelerStatus3))
																									{
																										if (!(myRvLinkEvent is MyRvLinkLeveler1Status levelerStatus4))
																										{
																											if (!(myRvLinkEvent is MyRvLinkTemperatureSensorStatus temperatureSensorStatus))
																											{
																												if (!(myRvLinkEvent is MyRvLinkJaycoTbbStatus jaycoTbbStatus))
																												{
																													if (!(myRvLinkEvent is MyRvLinkMonitorPanelStatus monitorPanelStatus))
																													{
																														if (!(myRvLinkEvent is MyRvLinkDeviceSessionStatus deviceOnlineStatus2))
																														{
																															if (!(myRvLinkEvent is MyRvLinkAwningSensorStatus awningSensorStatus))
																															{
																																if (!(myRvLinkEvent is MyRvLinkAccessoryGatewayStatus accessoryGatewayStatus))
																																{
																																	if (!(myRvLinkEvent is MyRvLinkBrakingSystemStatus absStatus))
																																	{
																																		if (!(myRvLinkEvent is MyRvLinkBatteryMonitorStatus batteryMonitorStatus))
																																		{
																																			if (!(myRvLinkEvent is MyRvLinkBootLoaderStatus bootLoaderStatus))
																																			{
																																				if (!(myRvLinkEvent is MyRvLinkDoorLockStatus doorLockStatus))
																																				{
																																					if (myRvLinkEvent is MyRvLinkDimmableLightStatusExtended dimmableLightStatusExtended)
																																					{
																																						deviceTracker.UpdateDimmableLightExtended(dimmableLightStatusExtended);
																																						return;
																																					}
																																					TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} Received Unhandled Event is being ignored{Environment.NewLine} {myRvLinkEvent}", global::System.Array.Empty<object>());
																																				}
																																				else
																																				{
																																					deviceTracker.UpdateDoorLockStatus(doorLockStatus);
																																				}
																																			}
																																			else
																																			{
																																				deviceTracker.UpdateBootLoaderStatus(bootLoaderStatus);
																																			}
																																		}
																																		else
																																		{
																																			deviceTracker.UpdateBatteryMonitorStatus(batteryMonitorStatus);
																																		}
																																	}
																																	else
																																	{
																																		deviceTracker.UpdateBrakingSystemStatus(absStatus);
																																	}
																																}
																																else
																																{
																																	deviceTracker.UpdateAccessoryGatewayStatus(accessoryGatewayStatus);
																																}
																															}
																															else
																															{
																																deviceTracker.UpdateAwningSensorStatus(awningSensorStatus);
																															}
																														}
																														else
																														{
																															deviceTracker.UpdateSessionStatus(deviceOnlineStatus2);
																														}
																													}
																													else
																													{
																														deviceTracker.UpdateMonitorPanelStatus(monitorPanelStatus);
																													}
																												}
																												else
																												{
																													deviceTracker.UpdateJaycoTbbStatus(jaycoTbbStatus);
																												}
																											}
																											else
																											{
																												deviceTracker.UpdateTemperatureSensorStatus(temperatureSensorStatus);
																											}
																										}
																										else
																										{
																											deviceTracker.UpdateLeveler1Status(levelerStatus4);
																										}
																									}
																									else
																									{
																										deviceTracker.UpdateLeveler3Status(levelerStatus3);
																									}
																								}
																								else
																								{
																									deviceTracker.UpdateLevelerConsoleText(levelerConsoleText);
																								}
																							}
																							else
																							{
																								deviceTracker.UpdateAutoOperationProgressStatus(progressStatus);
																							}
																						}
																						else
																						{
																							deviceTracker.UpdateLeveler5Status(levelerStatus2);
																						}
																					}
																					else
																					{
																						deviceTracker.UpdateLeveler4Status(levelerStatus);
																					}
																				}
																				else
																				{
																					deviceTracker.UpdateCloudGatewayStatus(cloudGatewayStatus);
																				}
																			}
																			else
																			{
																				deviceTracker.UpdateGeneratorGenieStatus(generatorGenieStatus);
																			}
																		}
																		else
																		{
																			deviceTracker.UpdateHourMeterStatus(tankSensorStatus3);
																		}
																	}
																	else
																	{
																		deviceTracker.UpdateHvacStatus(hvacStatus);
																	}
																}
																else
																{
																	deviceTracker.UpdateRgbLightStatus(rgbLightStatus);
																}
															}
															else
															{
																_deviceTracker?.UpdateDimmableLightStatus(dimmableLightStatus);
															}
														}
														else
														{
															deviceTracker.UpdateTankSensorStatusV2(tankSensorStatus2);
														}
													}
													else
													{
														deviceTracker.UpdateTankSensorStatus(tankSensorStatus);
													}
												}
												else
												{
													deviceTracker.UpdateRelayHBridgeMomentaryStatus(momentaryRelayStatus2);
												}
											}
											else
											{
												deviceTracker.UpdateRelayHBridgeMomentaryStatus(momentaryRelayStatus);
											}
										}
										else
										{
											deviceTracker.UpdateRelayBasicLatchingStatus(latchingRelayStatus2);
										}
									}
									else
									{
										deviceTracker.UpdateRelayBasicLatchingStatus(latchingRelayStatus);
									}
								}
								else
								{
									deviceTracker.UpdateLockStatus(deviceLockStatus);
								}
							}
							else
							{
								deviceTracker.UpdateOnlineStatus(deviceOnlineStatus);
							}
						}
					}
					else
					{
						TaggedLog.Information("DirectConnectionMyRvLink", $"{LogPrefix} Event Received: {myRvLinkEvent}", global::System.Array.Empty<object>());
					}
				}
				else if (HasMinimumExpectedProtocolVersion)
				{
					UpdateRvStatus(rvStatus);
				}
			}
			else if (HasMinimumExpectedProtocolVersion)
			{
				RealTimeClock = myRvLinkRealTimeClock.DateTime;
			}
		}
		else
		{
			GatewayInfo = gatewayInfo;
			_deviceTracker?.GetDevicesIfNeeded();
			_deviceTracker?.GetDevicesMetadataIfNeeded();
		}
	}

	[AsyncStateMachine(typeof(_003CTryGetFirmwareUpdateSupportAsync_003Ed__158))]
	public async global::System.Threading.Tasks.Task<FirmwareUpdateSupport> TryGetFirmwareUpdateSupportAsync(ILogicalDevice logicalDevice, CancellationToken cancelToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		_ = 1;
		try
		{
			if (!IsStarted)
			{
				throw new MyRvLinkDeviceServiceNotStartedException(this, "Device Source Not Started");
			}
			if (!IsConnected)
			{
				throw new MyRvLinkDeviceServiceNotConnectedException(this, "Device Source Not Connected");
			}
			if (logicalDevice is ILocapOtaAccessoryDevice)
			{
				TaggedLog.Information("DirectConnectionMyRvLink", $"ILocapOtaAccessory device found, updating not supported through RvLink Device Source. Logical device: {logicalDevice}", global::System.Array.Empty<object>());
				return (FirmwareUpdateSupport)4;
			}
			if (!IsLogicalDeviceOnline(logicalDevice))
			{
				return (FirmwareUpdateSupport)3;
			}
			ILogicalDeviceJumpToBootloader val = (ILogicalDeviceJumpToBootloader)(object)((logicalDevice is ILogicalDeviceJumpToBootloader) ? logicalDevice : null);
			if (val != null && val.IsJumpToBootRequiredForFirmwareUpdate)
			{
				return (FirmwareUpdateSupport)2;
			}
			if (!Enumerable.Contains<BlockTransferBlockId>((global::System.Collections.Generic.IEnumerable<BlockTransferBlockId>)(await GetDeviceBlockListAsync(logicalDevice, cancelToken)), (BlockTransferBlockId)3))
			{
				return (FirmwareUpdateSupport)4;
			}
			BlockTransferPropertyFlags val2 = await GetDeviceBlockPropertyFlagsAsync(logicalDevice, (BlockTransferBlockId)3, cancelToken);
			if (!((global::System.Enum)val2).HasFlag((global::System.Enum)(object)(BlockTransferPropertyFlags)8))
			{
				return (FirmwareUpdateSupport)4;
			}
			if (!((global::System.Enum)val2).HasFlag((global::System.Enum)(object)(BlockTransferPropertyFlags)16))
			{
				return (FirmwareUpdateSupport)4;
			}
			return (FirmwareUpdateSupport)1;
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Warning("DirectConnectionMyRvLink", "Unable to determine if firmware update is supported: " + ex.Message, global::System.Array.Empty<object>());
			return (FirmwareUpdateSupport)0;
		}
	}

	[AsyncStateMachine(typeof(_003CUpdateFirmwareAsync_003Ed__159))]
	public global::System.Threading.Tasks.Task UpdateFirmwareAsync(ILogicalDeviceFirmwareUpdateSession firmwareUpdateSession, global::System.Collections.Generic.IReadOnlyList<byte> data, Func<ILogicalDeviceTransferProgress, bool> progressAck, CancellationToken cancellationToken, IReadOnlyDictionary<FirmwareUpdateOption, object>? options = null)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_003CUpdateFirmwareAsync_003Ed__159 _003CUpdateFirmwareAsync_003Ed__ = default(_003CUpdateFirmwareAsync_003Ed__159);
		_003CUpdateFirmwareAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CUpdateFirmwareAsync_003Ed__._003C_003E4__this = this;
		_003CUpdateFirmwareAsync_003Ed__.firmwareUpdateSession = firmwareUpdateSession;
		_003CUpdateFirmwareAsync_003Ed__.data = data;
		_003CUpdateFirmwareAsync_003Ed__.progressAck = progressAck;
		_003CUpdateFirmwareAsync_003Ed__.cancellationToken = cancellationToken;
		_003CUpdateFirmwareAsync_003Ed__.options = options;
		_003CUpdateFirmwareAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CUpdateFirmwareAsync_003Ed__._003C_003Et__builder)).Start<_003CUpdateFirmwareAsync_003Ed__159>(ref _003CUpdateFirmwareAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CUpdateFirmwareAsync_003Ed__._003C_003Et__builder)).Task;
	}

	[AsyncStateMachine(typeof(_003CUpdateFirmwareInternalAsync_003Ed__160))]
	private global::System.Threading.Tasks.Task UpdateFirmwareInternalAsync(ILogicalDeviceFirmwareUpdateDevice logicalDeviceToReflash, global::System.Collections.Generic.IReadOnlyList<byte> data, Func<ILogicalDeviceTransferProgress, bool> progressAck, CancellationToken cancellationToken, IReadOnlyDictionary<FirmwareUpdateOption, object> options)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_003CUpdateFirmwareInternalAsync_003Ed__160 _003CUpdateFirmwareInternalAsync_003Ed__ = default(_003CUpdateFirmwareInternalAsync_003Ed__160);
		_003CUpdateFirmwareInternalAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CUpdateFirmwareInternalAsync_003Ed__._003C_003E4__this = this;
		_003CUpdateFirmwareInternalAsync_003Ed__.logicalDeviceToReflash = logicalDeviceToReflash;
		_003CUpdateFirmwareInternalAsync_003Ed__.data = data;
		_003CUpdateFirmwareInternalAsync_003Ed__.progressAck = progressAck;
		_003CUpdateFirmwareInternalAsync_003Ed__.cancellationToken = cancellationToken;
		_003CUpdateFirmwareInternalAsync_003Ed__.options = options;
		_003CUpdateFirmwareInternalAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CUpdateFirmwareInternalAsync_003Ed__._003C_003Et__builder)).Start<_003CUpdateFirmwareInternalAsync_003Ed__160>(ref _003CUpdateFirmwareInternalAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CUpdateFirmwareInternalAsync_003Ed__._003C_003Et__builder)).Task;
	}

	[AsyncStateMachine(typeof(_003CJumpToBootloaderAsync_003Ed__161))]
	public async global::System.Threading.Tasks.Task<ILogicalDeviceReflashBootloader> JumpToBootloaderAsync(ILogicalDeviceJumpToBootloader logicalDevice, TimeSpan holdTime, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		ILogicalDeviceReflashBootloader val = (ILogicalDeviceReflashBootloader)(object)((logicalDevice is ILogicalDeviceReflashBootloader) ? logicalDevice : null);
		if (val != null)
		{
			return val;
		}
		ILogicalDeviceReflashBootloader associatedLogicalDeviceBootloader = GetAssociatedLogicalDeviceBootloader((ILogicalDevice)(object)logicalDevice);
		if (associatedLogicalDeviceBootloader != null && (int)((IDevicesCommon)associatedLogicalDeviceBootloader).ActiveConnection == 1)
		{
			return associatedLogicalDeviceBootloader;
		}
		if (!IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice))
		{
			throw new MyRvLinkDeviceOfflineException(this, (ILogicalDevice)(object)logicalDevice);
		}
		LogicalDeviceJumpToBootState val2 = await logicalDevice.JumpToBootPid.ReadJumpToBootStateAsync(cancellationToken);
		if ((int)val2 <= 1432778632)
		{
			if ((int)val2 != 0)
			{
				if ((int)val2 == 287454020)
				{
					goto IL_014b;
				}
				if ((int)val2 == 1432778632)
				{
				}
			}
		}
		else if ((int)val2 <= -1431651397)
		{
			if ((int)val2 != -1717986919 && (int)val2 == -1431651397)
			{
				goto IL_014b;
			}
		}
		else if ((int)val2 == -858989091 || (int)val2 == -286326785)
		{
			goto IL_014b;
		}
		TaggedLog.Information("DirectConnectionMyRvLink", $"Unable to request to put device in bootloader mode because of it's current state {val2} for {logicalDevice}", global::System.Array.Empty<object>());
		throw new FirmwareUpdateBootloaderException($"Unable to request to put device in bootloader mode because of it's current state {val2}", (global::System.Exception)null);
		IL_014b:
		TaggedLog.Information("DirectConnectionMyRvLink", $"Performing Jump To Boot {logicalDevice}", global::System.Array.Empty<object>());
		await logicalDevice.JumpToBootPid.WriteRequestJumpToBoot(holdTime, cancellationToken);
		bool lookForErrorDetails = true;
		for (int attempt = 0; attempt < 20; attempt++)
		{
			((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
			await global::System.Threading.Tasks.Task.Delay(1000, cancellationToken);
			if (lookForErrorDetails)
			{
				try
				{
					val2 = await logicalDevice.JumpToBootPid.ReadJumpToBootStateAsync(cancellationToken);
					if ((int)val2 <= 1432778632)
					{
						if ((int)val2 == 0 || ((int)val2 != 287454020 && (int)val2 != 1432778632))
						{
							goto IL_03d4;
						}
					}
					else
					{
						if ((int)val2 > -1431651397)
						{
							if ((int)val2 != -858989091)
							{
								_ = -286326785;
							}
							goto IL_03d4;
						}
						if ((int)val2 != -1717986919)
						{
							if ((int)val2 == -1431651397)
							{
							}
							goto IL_03d4;
						}
					}
					goto end_IL_0372;
					IL_03d4:
					TaggedLog.Information("DirectConnectionMyRvLink", $"Failed to enter bootloader mode because {val2} for {logicalDevice}", global::System.Array.Empty<object>());
					throw new FirmwareUpdateBootloaderException($"Failed to enter bootloader mode because {val2}", (global::System.Exception)null);
					end_IL_0372:;
				}
				catch
				{
					lookForErrorDetails = false;
				}
			}
			associatedLogicalDeviceBootloader = GetAssociatedLogicalDeviceBootloader((ILogicalDevice)(object)logicalDevice);
			if (associatedLogicalDeviceBootloader != null && (int)((IDevicesCommon)associatedLogicalDeviceBootloader).ActiveConnection == 1)
			{
				TaggedLog.Information("DirectConnectionMyRvLink", $"Found Bootloader Device {associatedLogicalDeviceBootloader}", global::System.Array.Empty<object>());
				return associatedLogicalDeviceBootloader;
			}
		}
		TaggedLog.Information("DirectConnectionMyRvLink", $"Failed to enter/find bootloader for {logicalDevice}", global::System.Array.Empty<object>());
		throw new FirmwareUpdateBootloaderException("Unable to find Bootloader Device", (global::System.Exception)null);
	}

	public static ILogicalDeviceReflashBootloader? GetAssociatedLogicalDeviceBootloader(ILogicalDevice logicalDevice)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		List<ILogicalDeviceReflashBootloader> val = logicalDevice.DeviceService.DeviceManager.FindLogicalDevices<ILogicalDeviceReflashBootloader>((Func<ILogicalDeviceReflashBootloader, bool>)((ILogicalDeviceReflashBootloader ld) => (PhysicalAddress)(object)((ILogicalDevice)ld).LogicalId.ProductMacAddress == (PhysicalAddress)(object)logicalDevice.LogicalId.ProductMacAddress));
		int count = val.Count;
		if (count <= 1)
		{
			if (count == 1)
			{
				return Enumerable.First<ILogicalDeviceReflashBootloader>((global::System.Collections.Generic.IEnumerable<ILogicalDeviceReflashBootloader>)val);
			}
			return null;
		}
		throw new LogicalDeviceException("Multiple matching Bootloader's found, there should be only up to 1", (global::System.Exception)null);
	}

	[AsyncStateMachine(typeof(_003CFirmwareUpdateAuthorizationAsync_003Ed__163))]
	private global::System.Threading.Tasks.Task FirmwareUpdateAuthorizationAsync(ILogicalDevice logicalDevice, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		_003CFirmwareUpdateAuthorizationAsync_003Ed__163 _003CFirmwareUpdateAuthorizationAsync_003Ed__ = default(_003CFirmwareUpdateAuthorizationAsync_003Ed__163);
		_003CFirmwareUpdateAuthorizationAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CFirmwareUpdateAuthorizationAsync_003Ed__._003C_003E4__this = this;
		_003CFirmwareUpdateAuthorizationAsync_003Ed__.logicalDevice = logicalDevice;
		_003CFirmwareUpdateAuthorizationAsync_003Ed__.cancellationToken = cancellationToken;
		_003CFirmwareUpdateAuthorizationAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CFirmwareUpdateAuthorizationAsync_003Ed__._003C_003Et__builder)).Start<_003CFirmwareUpdateAuthorizationAsync_003Ed__163>(ref _003CFirmwareUpdateAuthorizationAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CFirmwareUpdateAuthorizationAsync_003Ed__._003C_003Et__builder)).Task;
	}

	[AsyncStateMachine(typeof(_003CTryRemoveRefreshBootLoaderWhenOfflineAsync_003Ed__164))]
	private async global::System.Threading.Tasks.Task<bool> TryRemoveRefreshBootLoaderWhenOfflineAsync(ILogicalDeviceFirmwareUpdateDevice logicalDeviceToReflash, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DEVICE_TYPE.op_Implicit(((ILogicalDevice)logicalDeviceToReflash).LogicalId.DeviceType) != 50)
			{
				return false;
			}
			global::System.DateTime startTime = global::System.DateTime.Now;
			bool isOnline;
			while (true)
			{
				isOnline = IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDeviceToReflash);
				if (!isOnline)
				{
					break;
				}
				await global::System.Threading.Tasks.Task.Delay(1000, cancellationToken);
				if (!isOnline)
				{
					TimeSpan val = global::System.DateTime.Now - startTime;
					if (!(((TimeSpan)(ref val)).TotalMilliseconds < 30000.0) && ((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
					{
						break;
					}
				}
			}
			if (isOnline || ((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
			{
				return false;
			}
			ILogicalDeviceManager deviceManager = ((ILogicalDevice)logicalDeviceToReflash).DeviceService.DeviceManager;
			if (deviceManager != null)
			{
				deviceManager.RemoveLogicalDevice((Func<ILogicalDevice, bool>)((ILogicalDevice d) => (object)d == logicalDeviceToReflash));
			}
			return true;
		}
		catch (global::System.Exception ex)
		{
			ILogicalDeviceFirmwareUpdateDevice obj = logicalDeviceToReflash;
			TaggedLog.Error("DirectConnectionMyRvLink", "Unable to Remove Device " + ((obj != null) ? ((IDevicesCommon)obj).DeviceName : null) + ". " + ex.Message, global::System.Array.Empty<object>());
			return false;
		}
	}

	public IFrequencyMetricsReadonly GetFrequencyMetricForCommandSend(MyRvLinkCommandType commandType)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		lock (_lock)
		{
			FrequencyMetrics result = default(FrequencyMetrics);
			if (_metricsForCommandSends.TryGetValue(commandType, ref result))
			{
				return (IFrequencyMetricsReadonly)(object)result;
			}
			_metricsForCommandSends[commandType] = new FrequencyMetrics();
			return (IFrequencyMetricsReadonly)(object)_metricsForCommandSends[commandType];
		}
	}

	public IFrequencyMetricsReadonly GetFrequencyMetricForCommandFailure(MyRvLinkCommandType commandType)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		lock (_lock)
		{
			FrequencyMetrics result = default(FrequencyMetrics);
			if (_metricsForCommandFailures.TryGetValue(commandType, ref result))
			{
				return (IFrequencyMetricsReadonly)(object)result;
			}
			_metricsForCommandFailures[commandType] = new FrequencyMetrics();
			return (IFrequencyMetricsReadonly)(object)_metricsForCommandFailures[commandType];
		}
	}

	public IFrequencyMetricsReadonly GetFrequencyMetricForEvent(MyRvLinkEventType eventType)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		lock (_lock)
		{
			FrequencyMetrics result = default(FrequencyMetrics);
			if (_metricsForEvents.TryGetValue(eventType, ref result))
			{
				return (IFrequencyMetricsReadonly)(object)result;
			}
			_metricsForEvents[eventType] = new FrequencyMetrics();
			return (IFrequencyMetricsReadonly)(object)_metricsForEvents[eventType];
		}
	}

	private void UpdateFrequencyMetricForCommandSend(MyRvLinkCommandType commandType)
	{
		IFrequencyMetricsReadonly frequencyMetricForCommandSend = GetFrequencyMetricForCommandSend(commandType);
		FrequencyMetrics val = (FrequencyMetrics)(object)((frequencyMetricForCommandSend is FrequencyMetrics) ? frequencyMetricForCommandSend : null);
		if (val != null)
		{
			val.Update();
		}
	}

	private void UpdateFrequencyMetricForCommandFailure(MyRvLinkCommandType commandType)
	{
		IFrequencyMetricsReadonly frequencyMetricForCommandFailure = GetFrequencyMetricForCommandFailure(commandType);
		FrequencyMetrics val = (FrequencyMetrics)(object)((frequencyMetricForCommandFailure is FrequencyMetrics) ? frequencyMetricForCommandFailure : null);
		if (val != null)
		{
			val.Update();
		}
	}

	private void UpdateFrequencyMetricForEvent(MyRvLinkEventType eventType)
	{
		IFrequencyMetricsReadonly frequencyMetricForEvent = GetFrequencyMetricForEvent(eventType);
		FrequencyMetrics val = (FrequencyMetrics)(object)((frequencyMetricForEvent is FrequencyMetrics) ? frequencyMetricForEvent : null);
		if (val != null)
		{
			val.Update();
		}
	}

	[AsyncStateMachine(typeof(_003CSendDirectCommandRelayMomentary_003Ed__175))]
	public async global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandRelayMomentary(ILogicalDeviceRelayHBridge logicalDevice, HBridgeCommand command, CancellationToken cancelToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		global::System.Collections.Generic.IEnumerator<ILogicalDeviceSourceCommandMonitor> enumerator = CommandMonitors.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ILogicalDeviceSourceCommandMonitor current = enumerator.Current;
				ILogicalDeviceSourceCommandMonitorMovement val = (ILogicalDeviceSourceCommandMonitorMovement)(object)((current is ILogicalDeviceSourceCommandMonitorMovement) ? current : null);
				if (val != null)
				{
					await val.WillSendCommandRelayMomentaryAsync((ILogicalDeviceSource)(object)this, logicalDevice, command, cancelToken);
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		CommandResult result = await SendDirectCommandRelayMomentaryImpl(logicalDevice, command, cancelToken);
		enumerator = CommandMonitors.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ILogicalDeviceSourceCommandMonitor current2 = enumerator.Current;
				ILogicalDeviceSourceCommandMonitorMovement val2 = (ILogicalDeviceSourceCommandMonitorMovement)(object)((current2 is ILogicalDeviceSourceCommandMonitorMovement) ? current2 : null);
				if (val2 != null)
				{
					await val2.DidSendCommandRelayMomentaryAsync((ILogicalDeviceSource)(object)this, logicalDevice, command, result, cancelToken);
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		return result;
	}

	[AsyncStateMachine(typeof(_003CSendDirectCommandRelayMomentaryImpl_003Ed__176))]
	private async global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandRelayMomentaryImpl(ILogicalDeviceRelayHBridge logicalDevice, HBridgeCommand command, CancellationToken cancelToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!(_deviceTracker?.IsLogicalDeviceOnline((ILogicalDevice?)(object)logicalDevice) ?? false))
		{
			return (CommandResult)6;
		}
		ValueTuple<byte, byte>? myRvLinkDevice = GetMyRvDeviceFromLogicalDevice((ILogicalDevice)(object)logicalDevice);
		if (!myRvLinkDevice.HasValue)
		{
			return (CommandResult)6;
		}
		try
		{
			MyRvLinkCommandContext<HBridgeCommand> commandContext;
			lock (this)
			{
				object obj = default(object);
				if (((ILogicalDevice)logicalDevice).CustomData.TryGetValue("DirectConnectionMyRvLink.IDirectCommandMovement", ref obj) && obj is MyRvLinkCommandContext<HBridgeCommand> myRvLinkCommandContext)
				{
					commandContext = myRvLinkCommandContext;
				}
				else
				{
					((ILogicalDevice)logicalDevice).CustomData["DirectConnectionMyRvLink.IDirectCommandMovement"] = (commandContext = new MyRvLinkCommandContext<HBridgeCommand>());
				}
			}
			if (commandContext.LastSentCommandReceivedError)
			{
				TaggedLog.Warning("DirectConnectionMyRvLink", "{LogPrefix} Momentary relay last sent command received an error!", global::System.Array.Empty<object>());
				commandContext.ClearLastSentCommandReceivedError();
				IMyRvLinkCommandResponseFailure? activeFailure = commandContext.ActiveFailure;
				return (CommandResult)((activeFailure == null) ? 7 : ((int)activeFailure.CommandResult));
			}
			bool flag = commandContext.CanResendCommand(command);
			if (flag)
			{
				flag = await ResendRunningCommandAsync(commandContext.SentCommandId, cancelToken);
			}
			if (flag)
			{
				commandContext.SentCommand(commandContext.SentCommandId, command);
				IMyRvLinkCommandResponseFailure? activeFailure2 = commandContext.ActiveFailure;
				return (CommandResult)((activeFailure2 != null) ? ((int)activeFailure2.CommandResult) : 0);
			}
			ushort nextCommandId = GetNextCommandId();
			MyRvLinkCommandActionMovement command2 = new MyRvLinkCommandActionMovement(nextCommandId, myRvLinkDevice.Value.Item1, myRvLinkDevice.Value.Item2, ((ILogicalDevice)logicalDevice).LogicalId, command);
			SendCommandAsync(command2, cancelToken, TimeSpan.FromMilliseconds(2500.0), MyRvLinkSendCommandOption.DontWaitForResponse, delegate(IMyRvLinkCommandResponse response)
			{
				commandContext.ProcessResponse(response);
			});
			commandContext.SentCommand(nextCommandId, command);
			IMyRvLinkCommandResponseFailure? activeFailure3 = commandContext.ActiveFailure;
			return (CommandResult)((activeFailure3 != null) ? ((int)activeFailure3.CommandResult) : 0);
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Warning("DirectConnectionMyRvLink", LogPrefix + " Sending command failed " + ex.Message, global::System.Array.Empty<object>());
			return (CommandResult)7;
		}
	}

	[AsyncStateMachine(typeof(_003CSetRealTimeClockTimeAsync_003Ed__179))]
	public async global::System.Threading.Tasks.Task<bool> SetRealTimeClockTimeAsync(global::System.DateTime dateTime, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (!IsStarted || !IsConnected)
		{
			throw new MyRvLinkException("Unable to set RTC because DirectConnectionMyRvLink isn't started or connected");
		}
		if (_firmwareUpdateInProgress)
		{
			throw new MyRvLinkPidWriteException("Can't perform Pid writes while a firmware update is in progress!");
		}
		MyRvLinkCommandSetRealTimeClock command = new MyRvLinkCommandSetRealTimeClock(GetNextCommandId(), dateTime);
		IMyRvLinkCommandResponse myRvLinkCommandResponse = await SendCommandAsync(command, cancellationToken);
		if (!(myRvLinkCommandResponse is IMyRvLinkCommandResponseFailure failure))
		{
			if (myRvLinkCommandResponse is IMyRvLinkCommandResponseSuccess)
			{
				return true;
			}
			throw new MyRvLinkException("Failed to set RTC: Unknown result");
		}
		throw new MyRvLinkCommandResponseFailureException(failure);
	}

	public global::System.Threading.Tasks.Task RemoveOfflineDevicesAsync(CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return RemoveOfflineDevicesAsync(enableConfigurationMode: false, cancellationToken);
	}

	[AsyncStateMachine(typeof(_003CRemoveOfflineDevicesAsync_003Ed__181))]
	public global::System.Threading.Tasks.Task RemoveOfflineDevicesAsync(bool enableConfigurationMode, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		_003CRemoveOfflineDevicesAsync_003Ed__181 _003CRemoveOfflineDevicesAsync_003Ed__ = default(_003CRemoveOfflineDevicesAsync_003Ed__181);
		_003CRemoveOfflineDevicesAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CRemoveOfflineDevicesAsync_003Ed__._003C_003E4__this = this;
		_003CRemoveOfflineDevicesAsync_003Ed__.enableConfigurationMode = enableConfigurationMode;
		_003CRemoveOfflineDevicesAsync_003Ed__.cancellationToken = cancellationToken;
		_003CRemoveOfflineDevicesAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CRemoveOfflineDevicesAsync_003Ed__._003C_003Et__builder)).Start<_003CRemoveOfflineDevicesAsync_003Ed__181>(ref _003CRemoveOfflineDevicesAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CRemoveOfflineDevicesAsync_003Ed__._003C_003Et__builder)).Task;
	}

	public bool IsLogicalDeviceRenameSupported(ILogicalDevice? logicalDevice)
	{
		return IsLogicalDeviceSupported(logicalDevice);
	}

	[AsyncStateMachine(typeof(_003CRenameLogicalDevice_003Ed__183))]
	public global::System.Threading.Tasks.Task RenameLogicalDevice(ILogicalDevice? logicalDevice, FUNCTION_NAME toName, byte toFunctionInstance, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_003CRenameLogicalDevice_003Ed__183 _003CRenameLogicalDevice_003Ed__ = default(_003CRenameLogicalDevice_003Ed__183);
		_003CRenameLogicalDevice_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CRenameLogicalDevice_003Ed__._003C_003E4__this = this;
		_003CRenameLogicalDevice_003Ed__.logicalDevice = logicalDevice;
		_003CRenameLogicalDevice_003Ed__.toName = toName;
		_003CRenameLogicalDevice_003Ed__.toFunctionInstance = toFunctionInstance;
		_003CRenameLogicalDevice_003Ed__.cancellationToken = cancellationToken;
		_003CRenameLogicalDevice_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CRenameLogicalDevice_003Ed__._003C_003Et__builder)).Start<_003CRenameLogicalDevice_003Ed__183>(ref _003CRenameLogicalDevice_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CRenameLogicalDevice_003Ed__._003C_003Et__builder)).Task;
	}

	public float? GetTemperature()
	{
		return _temperature;
	}

	public global::System.Threading.Tasks.Task<float?> TryGetVoltageAsync(CancellationToken cancellationToken)
	{
		return global::System.Threading.Tasks.Task.FromResult<float?>(_voltage);
	}

	private void UpdateRvStatus(MyRvLinkRvStatus rvStatus)
	{
		if (rvStatus == null)
		{
			TaggedLog.Debug("DirectConnectionMyRvLink", LogPrefix + " Ignoring DirectConnectionMyRvLink, RvStatus is null.", global::System.Array.Empty<object>());
			_voltage = null;
			_temperature = null;
		}
		else
		{
			_voltage = rvStatus.GetVoltage();
			_temperature = rvStatus.GetTemperature();
		}
	}

	public global::System.Threading.Tasks.Task<CommandResult> SendSoftwareUpdateAuthorizationAsync(ILogicalDevice logicalDevice, CancellationToken cancellationToken)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return SendCommandAsync(logicalDevice, [CompilerGenerated] (ValueTuple<byte, byte> myRvLinkDevice) => new MyRvLinkCommandSoftwareUpdateAuthorization(GetNextCommandId(), myRvLinkDevice.Item1, myRvLinkDevice.Item2), cancellationToken);
	}

	public global::System.Threading.Tasks.Task<CommandResult> SendDirectCommandRelayBasicSwitch(ILogicalDeviceSwitchable logicalDevice, bool turnOn, CancellationToken cancelToken)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		return SendCommandAsync((ILogicalDevice)(object)logicalDevice, MakeCommand, cancelToken);
		IMyRvLinkCommand MakeCommand(ValueTuple<byte, byte> myRvLinkDevice)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			if (logicalDevice is LogicalDeviceGeneratorGenie)
			{
				return new MyRvLinkCommandActionGeneratorGenie(GetNextCommandId(), command: (GeneratorGenieCommand)((!turnOn) ? 1 : 2), deviceTableId: myRvLinkDevice.Item1, deviceId: myRvLinkDevice.Item2);
			}
			if (logicalDevice != null)
			{
				return new MyRvLinkCommandActionSwitch(GetNextCommandId(), switchState: turnOn ? MyRvLinkCommandActionSwitchState.On : MyRvLinkCommandActionSwitchState.Off, deviceTableId: myRvLinkDevice.Item1, switchDeviceIdList: new byte[1] { myRvLinkDevice.Item2 });
			}
			throw new MyRvLinkException("Unsupported device for DirectConnectionMyRvLink");
		}
	}

	[AsyncStateMachine(typeof(_003CTrySwitchAllMasterControllable_003Ed__191))]
	public async global::System.Threading.Tasks.Task<bool> TrySwitchAllMasterControllable(global::System.Collections.Generic.IEnumerable<ILogicalDevice> logicalDeviceList, bool allOn, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!IsStarted || !IsConnected)
		{
			return false;
		}
		string operationText = (allOn ? "On" : "Off");
		TaggedLog.Debug("DirectConnectionMyRvLink", LogPrefix + " All Lights " + operationText, global::System.Array.Empty<object>());
		if (!IsConnected)
		{
			TaggedLog.Debug("DirectConnectionMyRvLink", LogPrefix + " Unable to Turn " + operationText + " All lights because not connected", global::System.Array.Empty<object>());
			return false;
		}
		MyRvLinkDeviceTracker deviceTracker = _deviceTracker;
		if (deviceTracker == null)
		{
			TaggedLog.Debug("DirectConnectionMyRvLink", LogPrefix + " Unable to Turn " + operationText + " All lights because devices not yet loaded", global::System.Array.Empty<object>());
			return false;
		}
		global::System.Collections.Generic.IEnumerable<ILogicalDeviceSwitchable> enumerable = Enumerable.Where<ILogicalDeviceSwitchable>(Enumerable.OfType<ILogicalDeviceSwitchable>((global::System.Collections.IEnumerable)logicalDeviceList), (Func<ILogicalDeviceSwitchable, bool>)((ILogicalDeviceSwitchable switchable) => ((ILogicalDeviceSwitchableReadonly)switchable).IsMasterSwitchControllable));
		List<byte> val = new List<byte>();
		global::System.Collections.Generic.IEnumerator<ILogicalDeviceSwitchable> enumerator = enumerable.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				ILogicalDeviceSwitchable current = enumerator.Current;
				if (!(_deviceTracker?.IsLogicalDeviceOnline((ILogicalDevice?)(object)current) ?? false))
				{
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} Unable to Turn {operationText} Light {current} because it isn't online", global::System.Array.Empty<object>());
					continue;
				}
				byte? myRvDeviceIdFromLogicalDevice = deviceTracker.GetMyRvDeviceIdFromLogicalDevice((ILogicalDevice)(object)current);
				if (!myRvDeviceIdFromLogicalDevice.HasValue)
				{
					TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} Unable to Turn {operationText} Light {current} because it isn't associated with {"DirectConnectionMyRvLink"}", global::System.Array.Empty<object>());
				}
				else
				{
					val.Add(myRvDeviceIdFromLogicalDevice.Value);
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		try
		{
			if (val.Count == 0)
			{
				return false;
			}
			MyRvLinkCommandActionSwitch command = new MyRvLinkCommandActionSwitch(GetNextCommandId(), deviceTracker.DeviceTableId, allOn ? MyRvLinkCommandActionSwitchState.On : MyRvLinkCommandActionSwitchState.Off, val.ToArray());
			IMyRvLinkCommandResponse obj = await SendCommandAsync(command, cancellationToken);
			TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} TrySwitchAllMasterControllable Completed for\n{command}", global::System.Array.Empty<object>());
			if ((int)obj.CommandResult != 0)
			{
				throw new MyRvLinkException("Failed to turn all lights " + operationText);
			}
			return true;
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Debug("DirectConnectionMyRvLink", $"{LogPrefix} Unable to Turn {operationText} Lights: {ex.Message}", global::System.Array.Empty<object>());
			return false;
		}
	}
}
