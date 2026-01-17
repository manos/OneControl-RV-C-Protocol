using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common.COBS;

public class CobsStream : Stream, IAsyncValueStream
{
	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CReadAsync_003Ed__13 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncValueTaskMethodBuilder<int> _003C_003Et__builder;

		public int offset;

		public int count;

		public CancellationToken cancellationToken;

		public CobsStream _003C_003E4__this;

		public TimeSpan? readTimeout;

		public byte[] buffer;

		private ValueTaskAwaiter<int> _003C_003Eu__1;

		private TaskAwaiter<int> _003C_003Eu__2;

		private void MoveNext()
		{
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			CobsStream cobsStream = _003C_003E4__this;
			int result;
			try
			{
				TaskAwaiter<int> val;
				if (num != 0)
				{
					if (num != 1)
					{
						if (offset < 0)
						{
							throw new ArgumentOutOfRangeException("offset");
						}
						if (count < 0)
						{
							throw new ArgumentOutOfRangeException("count");
						}
						((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
						goto IL_02a9;
					}
					val = _003C_003Eu__2;
					_003C_003Eu__2 = default(TaskAwaiter<int>);
					num = (_003C_003E1__state = -1);
					goto IL_01c0;
				}
				ValueTaskAwaiter<int> val2 = _003C_003Eu__1;
				_003C_003Eu__1 = default(ValueTaskAwaiter<int>);
				num = (_003C_003E1__state = -1);
				goto IL_0144;
				IL_01c0:
				int num2 = val.GetResult();
				goto IL_01c9;
				IL_01c9:
				object locker = cobsStream._locker;
				bool flag = false;
				try
				{
					Monitor.Enter(locker, ref flag);
					if (cobsStream._isDisposed)
					{
						throw new ObjectDisposedException("CobsStream");
					}
					if (num2 > 0)
					{
						cobsStream._readBufferOffset = 0;
						cobsStream._readBufferSize = num2;
					}
					while (cobsStream._readBufferOffset < cobsStream._readBufferSize)
					{
						global::System.Collections.Generic.IReadOnlyList<byte> readOnlyList = cobsStream._cobsDecoder.DecodeByte(cobsStream._readBuffer[cobsStream._readBufferOffset++]);
						if (readOnlyList == null)
						{
							continue;
						}
						if (((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count > count)
						{
							throw new ArgumentOutOfRangeException("buffer", "Given buffer not big enough to store decoded message");
						}
						for (int i = 0; i < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count; i++)
						{
							buffer[i] = readOnlyList[i];
						}
						result = ((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count;
						goto end_IL_000e;
					}
				}
				finally
				{
					if (num < 0 && flag)
					{
						Monitor.Exit(locker);
					}
				}
				goto IL_02a9;
				IL_0144:
				num2 = val2.GetResult();
				goto IL_01c9;
				IL_02a9:
				if (!((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
				{
					bool flag2 = false;
					num2 = 0;
					locker = cobsStream._locker;
					flag = false;
					try
					{
						Monitor.Enter(locker, ref flag);
						if (cobsStream._isDisposed)
						{
							throw new ObjectDisposedException("CobsStream");
						}
						if (cobsStream._readBufferOffset >= cobsStream._readBufferSize || cobsStream._readBufferOffset < 0 || cobsStream._readBufferSize < 0)
						{
							flag2 = true;
						}
					}
					finally
					{
						if (num < 0 && flag)
						{
							Monitor.Exit(locker);
						}
					}
					if (flag2)
					{
						if (cobsStream._stream is IAsyncValueStream asyncValueStream)
						{
							val2 = asyncValueStream.ReadAsync(cobsStream._readBuffer, 0, cobsStream._readBuffer.Length, cancellationToken, readTimeout).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val2;
								_003C_003Et__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<int>, _003CReadAsync_003Ed__13>(ref val2, ref this);
								return;
							}
							goto IL_0144;
						}
						val = cobsStream._stream.ReadAsync(cobsStream._readBuffer, 0, cobsStream._readBuffer.Length, cancellationToken).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__2 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<int>, _003CReadAsync_003Ed__13>(ref val, ref this);
							return;
						}
						goto IL_01c0;
					}
					goto IL_01c9;
				}
				((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
				result = 0;
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
	private struct _003CWriteAsync_003Ed__15 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder _003C_003Et__builder;

		public int offset;

		public int count;

		public CobsStream _003C_003E4__this;

		public byte[] buffer;

		public CancellationToken cancellationToken;

		private TaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			CobsStream cobsStream = _003C_003E4__this;
			try
			{
				TaskAwaiter val2;
				if (num != 0)
				{
					if (offset < 0)
					{
						throw new ArgumentOutOfRangeException("offset");
					}
					if (count < 0)
					{
						throw new ArgumentOutOfRangeException("count");
					}
					if (cobsStream._isDisposed)
					{
						throw new ObjectDisposedException("CobsStream");
					}
					ArraySegment<byte> val = default(ArraySegment<byte>);
					val._002Ector(buffer, offset, count);
					global::System.Collections.Generic.IReadOnlyList<byte> readOnlyList = cobsStream._cobsEncoder.Encode((global::System.Collections.Generic.IReadOnlyList<byte>)(object)val);
					for (int i = 0; i < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count; i++)
					{
						cobsStream._writeBuffer[i] = readOnlyList[i];
					}
					val2 = cobsStream._stream.WriteAsync(cobsStream._writeBuffer, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count, cancellationToken).GetAwaiter();
					if (!((TaskAwaiter)(ref val2)).IsCompleted)
					{
						num = (_003C_003E1__state = 0);
						_003C_003Eu__1 = val2;
						((AsyncTaskMethodBuilder)(ref _003C_003Et__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, _003CWriteAsync_003Ed__15>(ref val2, ref this);
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

	private const string LogTag = "CobsStream";

	private readonly object _locker = new object();

	private bool _isDisposed;

	private readonly Stream _stream;

	private readonly CobsEncoder _cobsEncoder;

	private readonly CobsDecoder _cobsDecoder;

	private int _readBufferOffset;

	private int _readBufferSize;

	private readonly byte[] _readBuffer = new byte[255];

	private readonly byte[] _writeBuffer = new byte[382];

	public override bool CanRead
	{
		get
		{
			if (_cobsDecoder != null)
			{
				return _stream.CanRead;
			}
			return false;
		}
	}

	public override bool CanWrite
	{
		get
		{
			if (_cobsEncoder != null)
			{
				return _stream.CanWrite;
			}
			return false;
		}
	}

	public override bool CanSeek => false;

	public override long Length
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotSupportedException();
		}
	}

	public override long Position
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotSupportedException();
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotSupportedException();
		}
	}

	public CobsStream(Stream stream, CobsEncoder encoder, CobsDecoder decoder)
	{
		_stream = stream;
		_cobsEncoder = encoder;
		_cobsDecoder = decoder;
	}

	public override void Close()
	{
		_stream.Close();
	}

	protected override void Dispose(bool disposing)
	{
		lock (_locker)
		{
			((Stream)this).Close();
			_isDisposed = true;
		}
		((Stream)this).Dispose(disposing);
	}

	public override global::System.Threading.Tasks.Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return ReadAsync(buffer, offset, count, cancellationToken, null).AsTask();
	}

	[AsyncStateMachine(typeof(_003CReadAsync_003Ed__13))]
	public global::System.Threading.Tasks.ValueTask<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken, TimeSpan? readTimeout)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_003CReadAsync_003Ed__13 _003CReadAsync_003Ed__ = default(_003CReadAsync_003Ed__13);
		_003CReadAsync_003Ed__._003C_003Et__builder = AsyncValueTaskMethodBuilder<int>.Create();
		_003CReadAsync_003Ed__._003C_003E4__this = this;
		_003CReadAsync_003Ed__.buffer = buffer;
		_003CReadAsync_003Ed__.offset = offset;
		_003CReadAsync_003Ed__.count = count;
		_003CReadAsync_003Ed__.cancellationToken = cancellationToken;
		_003CReadAsync_003Ed__.readTimeout = readTimeout;
		_003CReadAsync_003Ed__._003C_003E1__state = -1;
		_003CReadAsync_003Ed__._003C_003Et__builder.Start<_003CReadAsync_003Ed__13>(ref _003CReadAsync_003Ed__);
		return _003CReadAsync_003Ed__._003C_003Et__builder.Task;
	}

	[AsyncStateMachine(typeof(_003CWriteAsync_003Ed__15))]
	public override global::System.Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		_003CWriteAsync_003Ed__15 _003CWriteAsync_003Ed__ = default(_003CWriteAsync_003Ed__15);
		_003CWriteAsync_003Ed__._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
		_003CWriteAsync_003Ed__._003C_003E4__this = this;
		_003CWriteAsync_003Ed__.buffer = buffer;
		_003CWriteAsync_003Ed__.offset = offset;
		_003CWriteAsync_003Ed__.count = count;
		_003CWriteAsync_003Ed__.cancellationToken = cancellationToken;
		_003CWriteAsync_003Ed__._003C_003E1__state = -1;
		((AsyncTaskMethodBuilder)(ref _003CWriteAsync_003Ed__._003C_003Et__builder)).Start<_003CWriteAsync_003Ed__15>(ref _003CWriteAsync_003Ed__);
		return ((AsyncTaskMethodBuilder)(ref _003CWriteAsync_003Ed__._003C_003Et__builder)).Task;
	}

	public override global::System.Threading.Tasks.Task FlushAsync(CancellationToken cancellationToken)
	{
		((Stream)this).Flush();
		return global::System.Threading.Tasks.Task.FromResult<bool>(true);
	}

	public override void Flush()
	{
		lock (_locker)
		{
			_readBufferOffset = 0;
			_readBufferSize = 0;
		}
	}

	public override void SetLength(long value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException("Use ReadAsync");
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException("Use ReadAsync");
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException("Use ReadAsync");
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException("Use WriteAsync");
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException("Use WriteAsync");
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException("Use WriteAsync");
	}
}
