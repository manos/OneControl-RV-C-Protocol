using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public interface IAsyncValueBatchedReader<TValue>
{
	global::System.Threading.Tasks.Task<TValue> ReadValueAsync(CancellationToken cancellationToken, bool forceUpdate);
}
