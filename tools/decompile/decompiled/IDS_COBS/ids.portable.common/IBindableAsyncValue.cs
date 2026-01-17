using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public interface IBindableAsyncValue<TValue> : INotifyPropertyChanged, ICommonDisposable, global::System.IDisposable
{
	bool HasValueBeenLoaded { get; }

	bool IsValueInvalid { get; }

	bool IsReading { get; }

	bool IsWriting { get; }

	TValue LastValue { get; }

	TValue Value { get; set; }

	global::System.Threading.Tasks.Task LoadAsync(CancellationToken cancellationToken);

	global::System.Threading.Tasks.Task SaveAsync(CancellationToken cancellationToken);

	void Dispose(bool isDisposing);
}
