using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using IDS.Portable.Common;
using IDS.Portable.Common.COBS;
using IDS.Portable.Common.Extensions;
using IDS.Portable.LogicalDevice;
using OneControl.Direct.MyRvLink;

namespace OneControl.Direct.MyRvLinkTcpIp;

public class DirectConnectionMyRvLinkTcpIp : DirectConnectionMyRvLink, ILogicalDeviceSourceDirect, ILogicalDeviceSource
{
	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CBackgroundOperationAsync_003Ed__30 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public DirectConnectionMyRvLinkTcpIp _003C_003E4__this;

		public CancellationToken cancellationToken;

		private byte[] _003CreadBuffer_003E5__2;

		private TaskAwaiter<Stream?> _003C_003Eu__1;

		private TaskAwaiter<bool> _003C_003Eu__2;

		private object _003C_003E7__wrap2;

		private int _003C_003E7__wrap3;

		private object _003C_003E7__wrap4;

		private int _003C_003E7__wrap5;

		private TaskAwaiter<int> _003C_003Eu__3;

		private TaskAwaiter _003C_003Eu__4;

		private void MoveNext()
		{
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_045e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0463: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0585: Unknown result type (might be due to invalid IL or missing references)
			//IL_058a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_059f: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLinkTcpIp directConnectionMyRvLinkTcpIp = _003C_003E4__this;
			try
			{
				TaskAwaiter<Stream> val3;
				TaskAwaiter<bool> val2;
				TaskAwaiter val;
				UpdateDeviceSourceReachabilityEventHandler? obj2;
				object obj3;
				switch (num)
				{
				default:
					directConnectionMyRvLinkTcpIp._003C_003En__0();
					_003CreadBuffer_003E5__2 = new byte[255];
					TaggedLog.Debug("DirectConnectionMyRvLinkTcpIp", $"{((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).LogPrefix} Starting {"DirectConnectionMyRvLinkTcpIp"} for {directConnectionMyRvLinkTcpIp.ConnectionIpAddress})", global::System.Array.Empty<object>());
					((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).IsConnected = false;
					goto IL_0543;
				case 0:
					val3 = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<Stream>);
					num = (_003C_003E1__state = -1);
					goto IL_0130;
				case 1:
					val2 = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter<bool>);
					num = (_003C_003E1__state = -1);
					goto IL_01ef;
				case 2:
				case 3:
					try
					{
						if (num != 2)
						{
							if (num == 3)
							{
								val2 = _003C_003Eu__2;
								_003C_003Eu__2 = default(TaskAwaiter<bool>);
								num = (_003C_003E1__state = -1);
								goto IL_047a;
							}
							_003C_003E7__wrap5 = 0;
						}
						try
						{
							if (num != 2)
							{
								directConnectionMyRvLinkTcpIp.DidConnectEvent?.Invoke((ILogicalDeviceSourceDirectConnection)(object)directConnectionMyRvLinkTcpIp);
								UpdateDeviceSourceReachabilityEventHandler? obj4 = directConnectionMyRvLinkTcpIp.UpdateDeviceSourceReachabilityEvent;
								if (obj4 != null)
								{
									obj4.Invoke((ILogicalDeviceSourceDirect)(object)directConnectionMyRvLinkTcpIp);
								}
								if (!(directConnectionMyRvLinkTcpIp.OpenStream is CobsStream))
								{
									throw new MyRvLinkTcpIpServiceException("Expected COBS Stream");
								}
								goto IL_0378;
							}
							TaskAwaiter<int> val4 = _003C_003Eu__3;
							_003C_003Eu__3 = default(TaskAwaiter<int>);
							num = (_003C_003E1__state = -1);
							goto IL_02f5;
							IL_02f5:
							int result = val4.GetResult();
							IMyRvLinkEvent val5 = Singleton<MyRvLinkEventFactory>.Instance.TryDecodeEvent((global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(_003CreadBuffer_003E5__2, 0, result), ((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).IsFirmwareVersionSupported, ((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).GetPendingCommand);
							if (val5 == null)
							{
								if (((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).IsFirmwareVersionSupported)
								{
									TaggedLog.Error("DirectConnectionMyRvLinkTcpIp", ((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).LogPrefix + " Ignoring event because unable to decode it: " + ArrayExtension.DebugDump((global::System.Collections.Generic.IReadOnlyList<byte>)_003CreadBuffer_003E5__2, 0, _003CreadBuffer_003E5__2.Length, " ", false), global::System.Array.Empty<object>());
								}
							}
							else
							{
								((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).OnReceivedEvent(val5);
							}
							goto IL_0378;
							IL_0378:
							if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
							{
								val4 = directConnectionMyRvLinkTcpIp.ReadDataAsync(_003CreadBuffer_003E5__2, 0, _003CreadBuffer_003E5__2.Length, cancellationToken, 8000).GetAwaiter();
								if (!val4.IsCompleted)
								{
									num = (_003C_003E1__state = 2);
									_003C_003Eu__3 = val4;
									((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<int>, _003CBackgroundOperationAsync_003Ed__30>(ref val4, ref this);
									return;
								}
								goto IL_02f5;
							}
						}
						catch (global::System.Exception ex)
						{
							_003C_003E7__wrap4 = ex;
							_003C_003E7__wrap5 = 1;
						}
						int num2 = _003C_003E7__wrap5;
						if (num2 == 1)
						{
							global::System.Exception ex2 = (global::System.Exception)_003C_003E7__wrap4;
							TaggedLog.Debug("DirectConnectionMyRvLinkTcpIp", $"{((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).LogPrefix} Error Reading from BLE Connection {directConnectionMyRvLinkTcpIp.ConnectionIpAddress}): {ex2.Message}", global::System.Array.Empty<object>());
							val2 = TaskExtension.TryDelay(1000, cancellationToken).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (_003C_003E1__state = 3);
								_003C_003Eu__2 = val2;
								((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CBackgroundOperationAsync_003Ed__30>(ref val2, ref this);
								return;
							}
							goto IL_047a;
						}
						goto IL_0482;
						IL_047a:
						val2.GetResult();
						goto IL_0482;
						IL_0482:
						_003C_003E7__wrap4 = null;
					}
					catch (object obj5)
					{
						_003C_003E7__wrap2 = obj5;
					}
					directConnectionMyRvLinkTcpIp.DidDisconnectEvent?.Invoke((ILogicalDeviceSourceDirectConnection)(object)directConnectionMyRvLinkTcpIp);
					val = directConnectionMyRvLinkTcpIp.TryCloseStreamAsync().GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (_003C_003E1__state = 4);
						_003C_003Eu__4 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CBackgroundOperationAsync_003Ed__30>(ref val, ref this);
						return;
					}
					goto IL_0500;
				case 4:
					val = _003C_003Eu__4;
					_003C_003Eu__4 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_0500;
				case 5:
					{
						val = _003C_003Eu__4;
						_003C_003Eu__4 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
						break;
					}
					IL_0543:
					if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
					{
						TaggedLog.Debug("DirectConnectionMyRvLinkTcpIp", ((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).LogPrefix + " TryOpenStreamAsync Started", global::System.Array.Empty<object>());
						val3 = directConnectionMyRvLinkTcpIp.TryOpenStreamAsync(cancellationToken).GetAwaiter();
						if (!val3.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val3;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<Stream>, _003CBackgroundOperationAsync_003Ed__30>(ref val3, ref this);
							return;
						}
						goto IL_0130;
					}
					if (((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).IsConnected)
					{
						directConnectionMyRvLinkTcpIp.DidDisconnectEvent?.Invoke((ILogicalDeviceSourceDirectConnection)(object)directConnectionMyRvLinkTcpIp);
						UpdateDeviceSourceReachabilityEventHandler? obj = directConnectionMyRvLinkTcpIp.UpdateDeviceSourceReachabilityEvent;
						if (obj != null)
						{
							obj.Invoke((ILogicalDeviceSourceDirect)(object)directConnectionMyRvLinkTcpIp);
						}
					}
					val = directConnectionMyRvLinkTcpIp.TryCloseStreamAsync().GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (_003C_003E1__state = 5);
						_003C_003Eu__4 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CBackgroundOperationAsync_003Ed__30>(ref val, ref this);
						return;
					}
					break;
					IL_0500:
					((TaskAwaiter)(ref val)).GetResult();
					obj2 = directConnectionMyRvLinkTcpIp.UpdateDeviceSourceReachabilityEvent;
					if (obj2 != null)
					{
						obj2.Invoke((ILogicalDeviceSourceDirect)(object)directConnectionMyRvLinkTcpIp);
					}
					obj3 = _003C_003E7__wrap2;
					if (obj3 != null)
					{
						ExceptionDispatchInfo.Capture((obj3 as global::System.Exception) ?? throw obj3).Throw();
					}
					_003C_003E7__wrap2 = null;
					goto IL_0543;
					IL_01ef:
					val2.GetResult();
					goto IL_0543;
					IL_0130:
					if (val3.GetResult() == null || directConnectionMyRvLinkTcpIp.OpenStream == null)
					{
						TaggedLog.Debug("DirectConnectionMyRvLinkTcpIp", $"{((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).LogPrefix} TryOpenStreamAsync Unable to open stream, IsCancellationRequested={((CancellationToken)(ref cancellationToken)).IsCancellationRequested}", global::System.Array.Empty<object>());
						val2 = TaskExtension.TryDelay(1000, cancellationToken).GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__2 = val2;
							((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CBackgroundOperationAsync_003Ed__30>(ref val2, ref this);
							return;
						}
						goto IL_01ef;
					}
					TaggedLog.Debug("DirectConnectionMyRvLinkTcpIp", ((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).LogPrefix + " TryOpenStreamAsync Success", global::System.Array.Empty<object>());
					_003C_003E7__wrap2 = null;
					_003C_003E7__wrap3 = 0;
					goto case 2;
				}
				((TaskAwaiter)(ref val)).GetResult();
				directConnectionMyRvLinkTcpIp._003C_003En__1();
				TaggedLog.Debug("DirectConnectionMyRvLinkTcpIp", $"{((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).LogPrefix} Stopped {"DirectConnectionMyRvLinkTcpIp"} for {directConnectionMyRvLinkTcpIp.ConnectionIpAddress})", global::System.Array.Empty<object>());
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003CreadBuffer_003E5__2 = null;
				((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003CreadBuffer_003E5__2 = null;
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
	private struct _003CReadDataAsync_003Ed__19 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<int> _003C_003Et__builder;

		public DirectConnectionMyRvLinkTcpIp _003C_003E4__this;

		public byte[] buffer;

		public int offset;

		public int count;

		public CancellationToken cancellationToken;

		public int readTimeoutMs;

		private global::System.Threading.Tasks.Task<int> _003CreadTask_003E5__2;

		private TaskAwaiter<global::System.Threading.Tasks.Task> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLinkTcpIp directConnectionMyRvLinkTcpIp = _003C_003E4__this;
			int result2;
			try
			{
				TaskAwaiter<global::System.Threading.Tasks.Task> val;
				if (num != 0)
				{
					Stream openStream = directConnectionMyRvLinkTcpIp.OpenStream;
					if (openStream == null)
					{
						throw new MyRvLinkTcpIpServiceException("Stream isn't connected/opened");
					}
					_003CreadTask_003E5__2 = openStream.ReadAsync(buffer, offset, count, cancellationToken);
					val = global::System.Threading.Tasks.Task.WhenAny((global::System.Threading.Tasks.Task)_003CreadTask_003E5__2, global::System.Threading.Tasks.Task.Delay(readTimeoutMs, cancellationToken)).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<global::System.Threading.Tasks.Task>, _003CReadDataAsync_003Ed__19>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter<global::System.Threading.Tasks.Task>);
					num = (_003C_003E1__state = -1);
				}
				global::System.Threading.Tasks.Task result = val.GetResult();
				if (!result.IsCompleted || result != _003CreadTask_003E5__2)
				{
					throw new TimeoutException();
				}
				result2 = _003CreadTask_003E5__2.Result;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003CreadTask_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003CreadTask_003E5__2 = null;
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
	private struct _003CSendCommandRawAsync_003Ed__31 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public IMyRvLinkCommand command;

		public DirectConnectionMyRvLinkTcpIp _003C_003E4__this;

		public CancellationToken cancellationToken;

		private TaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLinkTcpIp directConnectionMyRvLinkTcpIp = _003C_003E4__this;
			try
			{
				TaskAwaiter val;
				if (num != 0)
				{
					byte[] array = Enumerable.ToArray<byte>((global::System.Collections.Generic.IEnumerable<byte>)command.Encode());
					TaggedLog.Debug("DirectConnectionMyRvLinkTcpIp", ((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).LogPrefix + " WRITE DATA " + ArrayExtension.DebugDump((global::System.Collections.Generic.IReadOnlyList<byte>)array, 0, array.Length, " ", false), global::System.Array.Empty<object>());
					val = directConnectionMyRvLinkTcpIp.WriteDataAsync(array, 0, array.Length, cancellationToken).GetAwaiter();
					if (!((TaskAwaiter)(ref val)).IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CSendCommandRawAsync_003Ed__31>(ref val, ref this);
						return;
					}
				}
				else
				{
					val = _003C_003Eu__1;
					_003C_003Eu__1 = default(TaskAwaiter);
					num = (_003C_003E1__state = -1);
				}
				((TaskAwaiter)(ref val)).GetResult();
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
	private struct _003CTryOpenStreamAsync_003Ed__15 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<Stream> _003C_003Et__builder;

		public DirectConnectionMyRvLinkTcpIp _003C_003E4__this;

		public CancellationToken cancellationToken;

		private object _003C_003E7__wrap1;

		private int _003C_003E7__wrap2;

		private TaskAwaiter _003C_003Eu__1;

		private TaskAwaiter<bool> _003C_003Eu__2;

		private void MoveNext()
		{
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected O, but got Unknown
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Expected O, but got Unknown
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DirectConnectionMyRvLinkTcpIp directConnectionMyRvLinkTcpIp = _003C_003E4__this;
			Stream openStream;
			try
			{
				TaskAwaiter<bool> val;
				if ((uint)num > 1u)
				{
					if (num == 2)
					{
						val = _003C_003Eu__2;
						_003C_003Eu__2 = default(TaskAwaiter<bool>);
						num = (_003C_003E1__state = -1);
						goto IL_0297;
					}
					_003C_003E7__wrap2 = 0;
				}
				try
				{
					TaskAwaiter val2;
					if (num != 0)
					{
						if (num == 1)
						{
							val2 = _003C_003Eu__1;
							_003C_003Eu__1 = default(TaskAwaiter);
							num = (_003C_003E1__state = -1);
							goto IL_0132;
						}
						val2 = directConnectionMyRvLinkTcpIp.TryCloseStreamAsync().GetAwaiter();
						if (!((TaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CTryOpenStreamAsync_003Ed__15>(ref val2, ref this);
							return;
						}
					}
					else
					{
						val2 = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
					}
					((TaskAwaiter)(ref val2)).GetResult();
					TcpClient? tcpIpConnection = directConnectionMyRvLinkTcpIp._tcpIpConnection;
					if (tcpIpConnection != null)
					{
						IDisposableExtensions.TryDispose((global::System.IDisposable)tcpIpConnection);
					}
					directConnectionMyRvLinkTcpIp._tcpIpConnection = new TcpClient();
					directConnectionMyRvLinkTcpIp._tcpIpConnection.ReceiveTimeout = 8000;
					directConnectionMyRvLinkTcpIp._tcpIpConnection.SendTimeout = 4000;
					directConnectionMyRvLinkTcpIp._tcpIpConnection.NoDelay = true;
					val2 = directConnectionMyRvLinkTcpIp._tcpIpConnection.ConnectAsync(directConnectionMyRvLinkTcpIp.ConnectionIpAddress, 6969).GetAwaiter();
					if (!((TaskAwaiter)(ref val2)).IsCompleted)
					{
						num = (_003C_003E1__state = 1);
						_003C_003Eu__1 = val2;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CTryOpenStreamAsync_003Ed__15>(ref val2, ref this);
						return;
					}
					goto IL_0132;
					IL_0132:
					((TaskAwaiter)(ref val2)).GetResult();
					directConnectionMyRvLinkTcpIp._tcpIpStream = directConnectionMyRvLinkTcpIp._tcpIpConnection.GetStream();
					if (directConnectionMyRvLinkTcpIp._tcpIpStream == null)
					{
						throw new global::System.Exception("Unable to open Stream as BLE is disabled");
					}
					((Stream)directConnectionMyRvLinkTcpIp._tcpIpStream).ReadTimeout = 8000;
					((Stream)directConnectionMyRvLinkTcpIp._tcpIpStream).WriteTimeout = 4000;
					directConnectionMyRvLinkTcpIp._cobsStream = new CobsStream((Stream)(object)directConnectionMyRvLinkTcpIp._tcpIpStream, directConnectionMyRvLinkTcpIp.CobsEncoder, directConnectionMyRvLinkTcpIp.CobsDecoder);
					((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).IsConnected = true;
				}
				catch (OperationCanceledException)
				{
				}
				catch (TimeoutException)
				{
				}
				catch (global::System.Exception ex3)
				{
					_003C_003E7__wrap1 = ex3;
					_003C_003E7__wrap2 = 1;
				}
				int num2 = _003C_003E7__wrap2;
				if (num2 == 1)
				{
					global::System.Exception ex4 = (global::System.Exception)_003C_003E7__wrap1;
					TaggedLog.Debug("DirectConnectionMyRvLinkTcpIp", $"{((DirectConnectionMyRvLink)directConnectionMyRvLinkTcpIp).LogPrefix} Error Connecting to BLE Connection {directConnectionMyRvLinkTcpIp.ConnectionIpAddress}): {ex4.Message}", global::System.Array.Empty<object>());
					val = TaskExtension.TryDelay(1000, cancellationToken).GetAwaiter();
					if (!val.IsCompleted)
					{
						num = (_003C_003E1__state = 2);
						_003C_003Eu__2 = val;
						_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, _003CTryOpenStreamAsync_003Ed__15>(ref val, ref this);
						return;
					}
					goto IL_0297;
				}
				goto IL_029f;
				IL_0297:
				val.GetResult();
				goto IL_029f;
				IL_029f:
				_003C_003E7__wrap1 = null;
				openStream = directConnectionMyRvLinkTcpIp.OpenStream;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(openStream);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	private const string LogTag = "DirectConnectionMyRvLinkTcpIp";

	public const int DefaultPort = 6969;

	private TcpClient? _tcpIpConnection;

	private readonly BackgroundOperation _backgroundOperation;

	protected readonly CobsEncoder CobsEncoder = new CobsEncoder(true, true, (byte)0, 6);

	protected readonly CobsDecoder CobsDecoder = new CobsDecoder(true, (byte)0, 6);

	private CobsStream? _cobsStream;

	private NetworkStream? _tcpIpStream;

	private const int ConnectionErrorRetryTimeMs = 1000;

	private const int ConnectionReceiveDataTimeMs = 8000;

	private const int ConnectionSendDataTimeMs = 4000;

	[CompilerGenerated]
	private Action<ILogicalDeviceSourceDirectConnection>? m_DidConnectEvent;

	[CompilerGenerated]
	private Action<ILogicalDeviceSourceDirectConnection>? m_DidDisconnectEvent;

	private const int TagCount = 1;

	private readonly ILogicalDeviceTag[] _deviceSourceTags = (ILogicalDeviceTag[])(object)new ILogicalDeviceTag[1];

	[CompilerGenerated]
	private UpdateDeviceSourceReachabilityEventHandler? m_UpdateDeviceSourceReachabilityEvent;

	[field: CompilerGenerated]
	public string ConnectionIpAddress
	{
		[CompilerGenerated]
		get;
	}

	protected Stream? OpenStream => (Stream?)(object)_cobsStream;

	[field: CompilerGenerated]
	public override string DeviceSourceToken
	{
		[CompilerGenerated]
		get;
	}

	public override global::System.Collections.Generic.IEnumerable<ILogicalDeviceTag> DeviceSourceTags => _deviceSourceTags;

	public override event Action<ILogicalDeviceSourceDirectConnection>? DidConnectEvent
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

	public override event Action<ILogicalDeviceSourceDirectConnection>? DidDisconnectEvent
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

	public override event UpdateDeviceSourceReachabilityEventHandler UpdateDeviceSourceReachabilityEvent
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

	public DirectConnectionMyRvLinkTcpIp(ILogicalDeviceService deviceService, string connectionIpAddress, string deviceSourceToken, ILogicalDeviceTag? logicalDeviceTag = null)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
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
		((DirectConnectionMyRvLink)this)._002Ector(deviceService, connectionIpAddress, obj);
		ConnectionIpAddress = connectionIpAddress;
		TaggedLog.Debug("DirectConnectionMyRvLinkTcpIp", $"{((DirectConnectionMyRvLink)this).LogPrefix} CREATED {this}", global::System.Array.Empty<object>());
		DeviceSourceToken = deviceSourceToken;
		_deviceSourceTags[0] = MakeDeviceSourceTag();
		_tcpIpStream = null;
		_cobsStream = null;
		_backgroundOperation = new BackgroundOperation(new BackgroundOperationFunc(BackgroundOperationAsync));
	}

	public DirectConnectionMyRvLinkTcpIp(ILogicalDeviceService deviceService, IEndPointConnectionTcpIp connection, string deviceSourceToken, ILogicalDeviceTag? logicalDeviceTag)
		: this(deviceService, connection.ConnectionIpAddress, deviceSourceToken, logicalDeviceTag)
	{
	}

	public override void Start()
	{
		if (!((BackgroundOperationBase)_backgroundOperation).StartedOrWillStart)
		{
			TaggedLog.Information("DirectConnectionMyRvLinkTcpIp", ((DirectConnectionMyRvLink)this).LogPrefix + " Starting " + ConnectionIpAddress, global::System.Array.Empty<object>());
			_backgroundOperation.Start();
		}
	}

	public override void Stop()
	{
		TaggedLog.Information("DirectConnectionMyRvLinkTcpIp", ((DirectConnectionMyRvLink)this).LogPrefix + " Stopping " + ConnectionIpAddress, global::System.Array.Empty<object>());
		_backgroundOperation.Stop();
		((DirectConnectionMyRvLink)this).Stop();
	}

	[AsyncStateMachine(typeof(_003CTryOpenStreamAsync_003Ed__15))]
	protected virtual async global::System.Threading.Tasks.Task<Stream?> TryOpenStreamAsync(CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			await TryCloseStreamAsync();
			TcpClient? tcpIpConnection = _tcpIpConnection;
			if (tcpIpConnection != null)
			{
				IDisposableExtensions.TryDispose((global::System.IDisposable)tcpIpConnection);
			}
			_tcpIpConnection = new TcpClient();
			_tcpIpConnection.ReceiveTimeout = 8000;
			_tcpIpConnection.SendTimeout = 4000;
			_tcpIpConnection.NoDelay = true;
			await _tcpIpConnection.ConnectAsync(ConnectionIpAddress, 6969);
			_tcpIpStream = _tcpIpConnection.GetStream();
			if (_tcpIpStream == null)
			{
				throw new global::System.Exception("Unable to open Stream as BLE is disabled");
			}
			((Stream)_tcpIpStream).ReadTimeout = 8000;
			((Stream)_tcpIpStream).WriteTimeout = 4000;
			_cobsStream = new CobsStream((Stream)(object)_tcpIpStream, CobsEncoder, CobsDecoder);
			((DirectConnectionMyRvLink)this).IsConnected = true;
		}
		catch (OperationCanceledException)
		{
		}
		catch (TimeoutException)
		{
		}
		catch (global::System.Exception ex3)
		{
			TaggedLog.Debug("DirectConnectionMyRvLinkTcpIp", $"{((DirectConnectionMyRvLink)this).LogPrefix} Error Connecting to BLE Connection {ConnectionIpAddress}): {ex3.Message}", global::System.Array.Empty<object>());
			await TaskExtension.TryDelay(1000, cancellationToken);
		}
		return OpenStream;
	}

	protected virtual global::System.Threading.Tasks.Task TryCloseStreamAsync()
	{
		((DirectConnectionMyRvLink)this).IsConnected = false;
		try
		{
			CobsStream? cobsStream = _cobsStream;
			if (cobsStream != null)
			{
				((Stream)cobsStream).Close();
			}
		}
		catch
		{
		}
		try
		{
			NetworkStream? tcpIpStream = _tcpIpStream;
			if (tcpIpStream != null)
			{
				((Stream)tcpIpStream).Close();
			}
		}
		catch
		{
		}
		try
		{
			_tcpIpConnection.Close();
		}
		catch
		{
		}
		CobsStream? cobsStream2 = _cobsStream;
		if (cobsStream2 != null)
		{
			IDisposableExtensions.TryDispose((global::System.IDisposable)cobsStream2);
		}
		_cobsStream = null;
		NetworkStream? tcpIpStream2 = _tcpIpStream;
		if (tcpIpStream2 != null)
		{
			IDisposableExtensions.TryDispose((global::System.IDisposable)tcpIpStream2);
		}
		_tcpIpStream = null;
		return global::System.Threading.Tasks.Task.CompletedTask;
	}

	[AsyncStateMachine(typeof(_003CReadDataAsync_003Ed__19))]
	protected virtual async global::System.Threading.Tasks.Task<int> ReadDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken, int readTimeoutMs)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		Stream openStream = OpenStream;
		if (openStream == null)
		{
			throw new MyRvLinkTcpIpServiceException("Stream isn't connected/opened");
		}
		global::System.Threading.Tasks.Task<int> readTask = openStream.ReadAsync(buffer, offset, count, cancellationToken);
		global::System.Threading.Tasks.Task task = await global::System.Threading.Tasks.Task.WhenAny((global::System.Threading.Tasks.Task)readTask, global::System.Threading.Tasks.Task.Delay(readTimeoutMs, cancellationToken));
		if (!task.IsCompleted || task != readTask)
		{
			throw new TimeoutException();
		}
		return readTask.Result;
	}

	protected virtual global::System.Threading.Tasks.Task WriteDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		return (OpenStream ?? throw new MyRvLinkTcpIpServiceException("Stream isn't connected/opened")).WriteAsync(buffer, offset, count, cancellationToken);
	}

	[AsyncStateMachine(typeof(_003CBackgroundOperationAsync_003Ed__30))]
	protected global::System.Threading.Tasks.Task BackgroundOperationAsync(CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		_003CBackgroundOperationAsync_003Ed__30 _003CBackgroundOperationAsync_003Ed__ = default(_003CBackgroundOperationAsync_003Ed__30);
		_003CBackgroundOperationAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CBackgroundOperationAsync_003Ed__._003C_003E4__this = this;
		_003CBackgroundOperationAsync_003Ed__.cancellationToken = cancellationToken;
		_003CBackgroundOperationAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CBackgroundOperationAsync_003Ed__._003C_003Et__builder)).Start<_003CBackgroundOperationAsync_003Ed__30>(ref _003CBackgroundOperationAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CBackgroundOperationAsync_003Ed__._003C_003Et__builder)).Task;
	}

	[AsyncStateMachine(typeof(_003CSendCommandRawAsync_003Ed__31))]
	protected override global::System.Threading.Tasks.Task SendCommandRawAsync(IMyRvLinkCommand command, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		_003CSendCommandRawAsync_003Ed__31 _003CSendCommandRawAsync_003Ed__ = default(_003CSendCommandRawAsync_003Ed__31);
		_003CSendCommandRawAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CSendCommandRawAsync_003Ed__._003C_003E4__this = this;
		_003CSendCommandRawAsync_003Ed__.command = command;
		_003CSendCommandRawAsync_003Ed__.cancellationToken = cancellationToken;
		_003CSendCommandRawAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CSendCommandRawAsync_003Ed__._003C_003Et__builder)).Start<_003CSendCommandRawAsync_003Ed__31>(ref _003CSendCommandRawAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CSendCommandRawAsync_003Ed__._003C_003Et__builder)).Task;
	}

	public override string ToString()
	{
		return $"{"DirectConnectionMyRvLinkTcpIp"} {((DirectConnectionMyRvLink)this).LogPrefix} Connection: {ConnectionIpAddress}) Gateway: {((object)((DirectConnectionMyRvLink)this).GatewayInfo)?.ToString() ?? "Gateway Connection Info Not Loaded Yet"} Tags: {LogicalDeviceTagManager.DebugTagsAsString((global::System.Collections.Generic.IEnumerable<ILogicalDeviceTag>)((DirectConnectionMyRvLink)this).ConnectionTagList)}";
	}

	private ILogicalDeviceTag MakeDeviceSourceTag()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		return (ILogicalDeviceTag)new LogicalDeviceTagSourceMyRvLinkTcpIp(ConnectionIpAddress);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003C_003En__0()
	{
		((DirectConnectionMyRvLink)this).Start();
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003C_003En__1()
	{
		((DirectConnectionMyRvLink)this).Stop();
	}
}
