using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace IDS.Portable.Common;

public struct SemaphoreSlimLock : global::System.IDisposable
{
	private readonly SemaphoreSlim _semaphore;

	private int _isDisposed;

	[field: CompilerGenerated]
	public bool HasLock
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public bool IsDisposed => _isDisposed != 0;

	public SemaphoreSlimLock(SemaphoreSlim semaphore, bool hasLock)
	{
		_semaphore = semaphore;
		_isDisposed = 0;
		HasLock = hasLock;
	}

	public void Dispose()
	{
		if (!IsDisposed && Interlocked.Exchange(ref _isDisposed, 1) == 0 && HasLock)
		{
			_semaphore.Release();
			HasLock = false;
		}
	}
}
