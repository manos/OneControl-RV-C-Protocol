using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Utils;

namespace IDS.Portable.Common.ObservableCollection;

public class CollectionSyncContainer<TCollection, TDataModel, TViewModel> : CommonDisposable where TCollection : global::System.Collections.Generic.ICollection<TViewModel>, new()
{
	public const int MaxSyncTimeWarningMs = 150;

	private const string LogTag = "CollectionSyncContainer";

	private IContainerDataSourceBase? _dataSource;

	private Func<TDataModel, bool> _dataSourceFilter = (Func<TDataModel, bool>)(object)(Func<bool, bool>)((TDataModel datamodel) => true);

	private Func<TDataModel, TViewModel> _viewModelFactory = (Func<TDataModel, TViewModel>)(object)(Func<TViewModel, _003F>)((TDataModel datamodel) => default(TViewModel));

	private Dictionary<TDataModel, TViewModel> _viewModelDict = (Dictionary<TDataModel, TViewModel>)(object)new Dictionary<TViewModel, _003F>();

	private TCollection _collection;

	protected const int ContainerDataSourceNotifyEventBatchDelayMs = 250;

	protected const int ContainerDataSourceNotifyEventBatchMaxDelayMs = 1000;

	private Watchdog? _containerDataSourceNotifyEventBatchWatchdog;

	protected virtual Func<TDataModel, bool> CurrentDataSourceFilter => _dataSourceFilter;

	protected virtual Func<TDataModel, TViewModel> CurrentViewModelFactory => _viewModelFactory;

	public TCollection Collection => _collection;

	protected virtual bool AutoDataSourceSyncOnConstruction => true;

	protected virtual Watchdog ContainerDataSourceNotifyEventBatchWatchdog
	{
		get
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			return _containerDataSourceNotifyEventBatchWatchdog ?? (_containerDataSourceNotifyEventBatchWatchdog = new Watchdog(250, 1000, new Action(DataSourceSync), autoStartOnFirstPet: true));
		}
		private set
		{
			_containerDataSourceNotifyEventBatchWatchdog = value;
		}
	}

	public CollectionSyncContainer(TCollection collection, IContainerDataSource<TDataModel> dataSource, Func<TDataModel, TViewModel> viewModelFactory, Func<TDataModel, bool> dataSourceFilter)
		: this(collection, (IContainerDataSourceBase)dataSource, viewModelFactory, dataSourceFilter)
	{
	}

	public CollectionSyncContainer(IContainerDataSource<TDataModel> dataSource, Func<TDataModel, TViewModel> viewModelFactory, Func<TDataModel, bool> dataSourceFilter)
		: this(new TCollection(), dataSource, viewModelFactory, dataSourceFilter)
	{
	}

	public CollectionSyncContainer(TCollection collection, IContainerDataSource dataSource, Func<TDataModel, TViewModel> viewModelFactory, Func<TDataModel, bool> dataSourceFilter)
		: this(collection, (IContainerDataSourceBase)dataSource, viewModelFactory, dataSourceFilter)
	{
	}

	public CollectionSyncContainer(IContainerDataSource dataSource, Func<TDataModel, TViewModel> viewModelFactory, Func<TDataModel, bool> dataSourceFilter)
		: this(new TCollection(), dataSource, viewModelFactory, dataSourceFilter)
	{
	}

	private CollectionSyncContainer(TCollection collection, IContainerDataSourceBase dataSource, Func<TDataModel, TViewModel> viewModelFactory, Func<TDataModel, bool> dataSourceFilter)
	{
		_collection = collection;
		_dataSource = dataSource;
		_viewModelFactory = viewModelFactory;
		_dataSourceFilter = dataSourceFilter;
		dataSource.ContainerDataSourceNotifyEvent += OnContainerDataSourceNotifyEvent;
		if (AutoDataSourceSyncOnConstruction)
		{
			DataSourceSync();
		}
	}

	protected CollectionSyncContainer(TCollection collection, IContainerDataSource dataSource, Func<TDataModel, TViewModel> viewModelFactory)
		: this(collection, (IContainerDataSourceBase)dataSource, viewModelFactory)
	{
	}

	protected CollectionSyncContainer(IContainerDataSource dataSource, Func<TDataModel, TViewModel> viewModelFactory)
		: this(new TCollection(), dataSource, viewModelFactory)
	{
	}

	protected CollectionSyncContainer(TCollection collection, IContainerDataSource<TDataModel> dataSource, Func<TDataModel, TViewModel> viewModelFactory)
		: this(collection, (IContainerDataSourceBase)dataSource, viewModelFactory)
	{
	}

	protected CollectionSyncContainer(IContainerDataSource<TDataModel> dataSource, Func<TDataModel, TViewModel> viewModelFactory)
		: this(new TCollection(), dataSource, viewModelFactory)
	{
	}

	private CollectionSyncContainer(TCollection collection, IContainerDataSourceBase dataSource, Func<TDataModel, TViewModel> viewModelFactory)
	{
		_collection = collection;
		_dataSource = dataSource;
		_viewModelFactory = viewModelFactory;
		dataSource.ContainerDataSourceNotifyEvent += OnContainerDataSourceNotifyEvent;
		if (AutoDataSourceSyncOnConstruction)
		{
			DataSourceSync();
		}
	}

	protected CollectionSyncContainer(TCollection collection, IContainerDataSource dataSource)
		: this(collection, (IContainerDataSourceBase)dataSource)
	{
	}

	protected CollectionSyncContainer(IContainerDataSource dataSource)
		: this(new TCollection(), dataSource)
	{
	}

	protected CollectionSyncContainer(TCollection collection, IContainerDataSource<TDataModel> dataSource)
		: this(collection, (IContainerDataSourceBase)dataSource)
	{
	}

	protected CollectionSyncContainer(IContainerDataSource<TDataModel> dataSource)
		: this(new TCollection(), dataSource)
	{
	}

	private CollectionSyncContainer(TCollection collection, IContainerDataSourceBase dataSource)
	{
		_collection = collection;
		_dataSource = dataSource;
		dataSource.ContainerDataSourceNotifyEvent += OnContainerDataSourceNotifyEvent;
		if (AutoDataSourceSyncOnConstruction)
		{
			DataSourceSync();
		}
	}

	public virtual void OnSyncStart(TCollection collection)
	{
	}

	public virtual void OnSyncEnd(TCollection collection)
	{
	}

	public virtual void OnViewModelAdded(TDataModel dataModel, TViewModel viewModel)
	{
	}

	public virtual void OnViewModelRemoved(TDataModel dataModel, TViewModel viewModel)
	{
	}

	public virtual void OnDataModelAssociated(TDataModel dataModel, TViewModel viewModel)
	{
	}

	public virtual void OnDataModelDissociated(TDataModel dataModel, TViewModel viewModel)
	{
	}

	public void DataSourceSync()
	{
		if (base.IsDisposed)
		{
			ClearAll();
		}
		else
		{
			if (CurrentDataSourceFilter == null)
			{
				return;
			}
			List<TDataModel> val = null;
			IContainerDataSourceBase dataSource = _dataSource;
			if (!(dataSource is IContainerDataSource<TDataModel> containerDataSource))
			{
				if (!(dataSource is IContainerDataSource containerDataSource2))
				{
					TaggedLog.Error("CollectionSyncContainer", "Error: Unexpected _dataSource type '{0}'", ((object)_dataSource)?.GetType());
					return;
				}
				val = containerDataSource2.FindContainerDataMatchingFilter<TDataModel>(CurrentDataSourceFilter);
			}
			else
			{
				val = containerDataSource.FindContainerDataMatchingFilter(CurrentDataSourceFilter);
			}
			DataSourceSync(val);
		}
	}

	public void ClearAll()
	{
		DataSourceSync((List<TDataModel>)(object)new List<_003F>());
	}

	private unsafe void DataSourceSync(List<TDataModel> dataModelList)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		if (dataModelList == null)
		{
			return;
		}
		MainThread.RequestMainThreadAction((Action)delegate
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			PerformanceTimer performanceTimer = new PerformanceTimer("CollectionSyncContainer", ((MemberInfo)((object)this).GetType()).Name + ".DataSourceSync", TimeSpan.FromMilliseconds(150.0), PerformanceTimerOption.AutoStartOnCreate);
			using (new PerformanceTimer("CollectionSyncContainer", ((MemberInfo)((object)this).GetType()).Name + ".OnSyncStart", TimeSpan.FromMilliseconds(150.0), PerformanceTimerOption.AutoStartOnCreate))
			{
				OnSyncStart(_collection);
			}
			using ((_collection as BaseObservableCollection<TViewModel>)?.SuppressEvents())
			{
				List<TDataModel> val = null;
				Enumerator<TDataModel, TViewModel> enumerator = ((KeyCollection<TViewModel, _003F>)(object)((Dictionary<TViewModel, _003F>)(object)_viewModelDict).Keys).GetEnumerator();
				try
				{
					while (((Enumerator<TViewModel, _003F>*)(&enumerator))->MoveNext())
					{
						TDataModel current = ((Enumerator<TViewModel, _003F>*)(&enumerator))->Current;
						if (!((List<_003F>)(object)dataModelList).Contains(current))
						{
							if (val == null)
							{
								val = (List<TDataModel>)(object)new List<_003F>();
							}
							((List<_003F>)(object)val).Add(current);
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
				Enumerator<TDataModel> enumerator2;
				if (val != null)
				{
					enumerator2 = ((List<_003F>)(object)val).GetEnumerator();
					try
					{
						while (((Enumerator<_003F>*)(&enumerator2))->MoveNext())
						{
							TDataModel current2 = ((Enumerator<_003F>*)(&enumerator2))->Current;
							TViewModel val2 = ((Dictionary<TViewModel, _003F>)(object)_viewModelDict)[(TViewModel)current2];
							((Dictionary<TViewModel, _003F>)(object)_viewModelDict).Remove((TViewModel)current2);
							OnDataModelDissociated(current2, val2);
							if (!((Dictionary<TViewModel, _003F>)(object)_viewModelDict).ContainsValue(val2))
							{
								_collection.TryRemove(val2);
								OnViewModelRemoved(current2, val2);
							}
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
					}
				}
				enumerator2 = ((List<_003F>)(object)dataModelList).GetEnumerator();
				try
				{
					while (((Enumerator<_003F>*)(&enumerator2))->MoveNext())
					{
						TDataModel current3 = ((Enumerator<_003F>*)(&enumerator2))->Current;
						if (((Dictionary<TViewModel, _003F>)(object)_viewModelDict).ContainsKey((TViewModel)current3))
						{
							continue;
						}
						TViewModel val3 = ((Func<TViewModel, _003F>)(object)CurrentViewModelFactory).Invoke((TViewModel)current3);
						if (val3 == null)
						{
							continue;
						}
						((Dictionary<TViewModel, _003F>)(object)_viewModelDict)[(TViewModel)current3] = val3;
						OnDataModelAssociated(current3, val3);
						if (((global::System.Collections.Generic.ICollection<_003F>)_collection/*cast due to .constrained prefix*/).Contains(val3))
						{
							continue;
						}
						global::System.Runtime.CompilerServices.DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new global::System.Runtime.CompilerServices.DefaultInterpolatedStringHandler(2, 3);
						defaultInterpolatedStringHandler.AppendFormatted(((MemberInfo)((object)this).GetType()).Name);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						defaultInterpolatedStringHandler.AppendFormatted("OnViewModelAdded");
						defaultInterpolatedStringHandler.AppendLiteral(".");
						ref TDataModel reference = ref current3;
						TDataModel val4 = default(TDataModel);
						object obj;
						if (val4 == null)
						{
							val4 = reference;
							reference = ref val4;
							if (val4 == null)
							{
								obj = null;
								goto IL_02e3;
							}
						}
						obj = ((MemberInfo)((object)reference/*cast due to .constrained prefix*/).GetType()).Name;
						goto IL_02e3;
						IL_02e3:
						defaultInterpolatedStringHandler.AppendFormatted((string)obj);
						using (new PerformanceTimer("CollectionSyncContainer", defaultInterpolatedStringHandler.ToStringAndClear(), TimeSpan.FromMilliseconds(150.0), PerformanceTimerOption.AutoStartOnCreate))
						{
							((global::System.Collections.Generic.ICollection<_003F>)_collection/*cast due to .constrained prefix*/).Add(val3);
							OnViewModelAdded(current3, val3);
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
				}
				using (new PerformanceTimer("CollectionSyncContainer", ((MemberInfo)((object)this).GetType()).Name + ".OnSyncEnd", TimeSpan.FromMilliseconds(150.0), PerformanceTimerOption.AutoStartOnCreate))
				{
					OnSyncEnd(_collection);
				}
				performanceTimer.TryDispose();
			}
		});
	}

	protected virtual void OnContainerDataSourceNotifyEvent(object sender, EventArgs args)
	{
		if (args is IContainerDataSourceNotifyRefreshBatchable containerDataSourceNotifyRefreshBatchable)
		{
			if (containerDataSourceNotifyRefreshBatchable.IsBatchRequested)
			{
				ContainerDataSourceNotifyEventBatchWatchdog?.TryPet(autoReset: true);
			}
			else
			{
				DataSourceSync();
			}
		}
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (_dataSource != null)
			{
				_dataSource.ContainerDataSourceNotifyEvent -= OnContainerDataSourceNotifyEvent;
			}
		}
		catch
		{
		}
		_containerDataSourceNotifyEventBatchWatchdog?.TryDispose();
		_containerDataSourceNotifyEventBatchWatchdog = null;
		ClearAll();
		_dataSource = null;
	}
}
