using System;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public interface IAsyncValueStream
{
	global::System.Threading.Tasks.ValueTask<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken, TimeSpan? readTimeout);
}
