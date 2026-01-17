using System.Runtime.CompilerServices;
using IDS.Portable.Common;
using IDS.Portable.LogicalDevice;

namespace OneControl.Direct.MyRvLink;

internal class MyRvLinkCommandContext<TCommandCode> where TCommandCode : struct
{
	public const int CommandTimeoutMs = 3000;

	public const int FailureAssumedTimeMs = 500;

	public const int AssumeSuccessAfterConsecutiveMessagesSentWithoutError = 3;

	private IMyRvLinkCommandResponseFailure? _lastFailureResponse;

	public ushort SentCommandId
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public TCommandCode SentCommandCode
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public byte[] SentCommandData
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	} = new byte[0];

	public long SentAtTimestampMs
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public int ConsecutiveAssumedSuccessCount
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public long LastFailureReceivedAtTimestamp
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public bool LastSentCommandReceivedError
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public bool IsErrorActive
	{
		get
		{
			lock (this)
			{
				if (SentCommandId == 0)
				{
					return false;
				}
				if (_lastFailureResponse == null)
				{
					return false;
				}
				if (ConsecutiveAssumedSuccessCount >= 3)
				{
					return false;
				}
				return true;
			}
		}
	}

	public IMyRvLinkCommandResponseFailure? ActiveFailure
	{
		get
		{
			if (!IsErrorActive || _lastFailureResponse == null)
			{
				return null;
			}
			if (LogicalDeviceFreeRunningTimer.ElapsedMilliseconds - LastFailureReceivedAtTimestamp >= 500)
			{
				return new MyRvLinkCommandResponseFailureAssumed(SentCommandId, _lastFailureResponse.FailureCode);
			}
			return _lastFailureResponse;
		}
	}

	public bool CanResendCommand(TCommandCode commandCode, IDeviceCommandPacket? command = null)
	{
		lock (this)
		{
			if (SentCommandId == 0)
			{
				return false;
			}
			if (!((object)commandCode/*cast due to .constrained prefix*/).Equals((object)SentCommandCode))
			{
				return false;
			}
			if (LogicalDeviceFreeRunningTimer.ElapsedMilliseconds - SentAtTimestampMs > 3000)
			{
				return false;
			}
			if (command != null && !ArrayCommon.ArraysEqual<byte>(SentCommandData, ((IDeviceDataPacket)command).CopyCurrentData()))
			{
				return false;
			}
			return true;
		}
	}

	public void ClearLastSentCommandReceivedError()
	{
		LastSentCommandReceivedError = false;
	}

	public void SentCommand(ushort commandId, TCommandCode commandCode, IDeviceCommandPacket? command = null)
	{
		lock (this)
		{
			SentCommandId = commandId;
			SentCommandCode = commandCode;
			SentCommandData = ((command == null) ? new byte[0] : ((IDeviceDataPacket)command).CopyCurrentData());
			SentAtTimestampMs = LogicalDeviceFreeRunningTimer.ElapsedMilliseconds;
			ConsecutiveAssumedSuccessCount++;
			ClearLastSentCommandReceivedError();
		}
	}

	public void ProcessResponse(IMyRvLinkCommandResponse commandResponse)
	{
		lock (this)
		{
			if (!(commandResponse is IMyRvLinkCommandResponseFailure lastFailureResponse))
			{
				if (commandResponse is IMyRvLinkCommandResponseSuccess)
				{
					_lastFailureResponse = null;
				}
			}
			else
			{
				ConsecutiveAssumedSuccessCount = 0;
				_lastFailureResponse = lastFailureResponse;
				LastFailureReceivedAtTimestamp = LogicalDeviceFreeRunningTimer.ElapsedMilliseconds;
				LastSentCommandReceivedError = true;
			}
		}
	}
}
