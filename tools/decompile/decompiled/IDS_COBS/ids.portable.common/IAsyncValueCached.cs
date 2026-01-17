using System;

namespace IDS.Portable.Common;

public interface IAsyncValueCached<TValue>
{
	TValue Value { get; set; }

	ValueTuple<TValue, AsyncValueCachedState> ValueAndState { get; }

	bool HasValue { get; }

	bool NeedsUpdate { get; }

	bool IsAsyncUpdating { get; }

	AsyncValueCachedState State { get; }

	AsyncValueCachedOperation<TValue> AsyncUpdateStart(TValue value);

	void AsyncUpdateComplete(AsyncValueCachedOperation<TValue>? asyncUpdateOperation);

	void AsyncUpdateFailed(AsyncValueCachedOperation<TValue>? asyncUpdateOperation);

	void InvalidateCache();
}
