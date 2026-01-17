using System;
using System.Runtime.CompilerServices;

namespace IDS.Portable.Common.ObservableCollection;

public class ContainerDataSourceNotifyRefresh : EventArgs, IContainerDataSourceNotifyRefreshBatchable
{
	public static readonly ContainerDataSourceNotifyRefresh Default = new ContainerDataSourceNotifyRefresh();

	public static readonly ContainerDataSourceNotifyRefresh DefaultBatched = new ContainerDataSourceNotifyRefresh(batchRequest: true);

	[field: CompilerGenerated]
	public bool IsBatchRequested
	{
		[CompilerGenerated]
		get;
	}

	protected ContainerDataSourceNotifyRefresh(bool batchRequest = false)
	{
		IsBatchRequested = batchRequest;
	}
}
