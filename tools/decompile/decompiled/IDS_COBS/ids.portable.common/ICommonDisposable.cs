using System;

namespace IDS.Portable.Common;

public interface ICommonDisposable : global::System.IDisposable
{
	bool IsDisposed { get; }

	void TryDispose();
}
