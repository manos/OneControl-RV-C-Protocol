using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IDS.Portable.Common.Manifest;

public class ManifestLogReceiverPlayer : IManifestLogReceiverPlayer
{
	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CReplay_003Ed__4 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<uint> _003C_003Et__builder;

		public CancellationToken cancellationToken;

		public ManifestLogReceiverPlayer _003C_003E4__this;

		public float speedFactor;

		public IManifestLogReceiver manifestLogReceiver;

		private Dictionary<string, IManifestProduct> _003ClastSeenProductDict_003E5__2;

		private uint _003ClogEntriesProcessed_003E5__3;

		private Enumerator<IManifestLogEntry> _003C_003E7__wrap3;

		private IManifestLogEntry _003ClogEntry_003E5__5;

		private ConfiguredTaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			ManifestLogReceiverPlayer manifestLogReceiverPlayer = _003C_003E4__this;
			uint result;
			try
			{
				global::System.DateTime dateTime = default(global::System.DateTime);
				if (num != 0)
				{
					((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
					object locker = manifestLogReceiverPlayer._locker;
					bool flag = false;
					List<IManifestLogEntry> val;
					try
					{
						Monitor.Enter(locker, ref flag);
						val = new List<IManifestLogEntry>(manifestLogReceiverPlayer._webLogEntries);
					}
					finally
					{
						if (num < 0 && flag)
						{
							Monitor.Exit(locker);
						}
					}
					_003ClastSeenProductDict_003E5__2 = new Dictionary<string, IManifestProduct>();
					_003ClogEntriesProcessed_003E5__3 = 0u;
					dateTime = default(global::System.DateTime);
					_003C_003E7__wrap3 = val.GetEnumerator();
				}
				try
				{
					if (num != 0)
					{
						goto IL_0395;
					}
					ConfiguredTaskAwaiter val2 = _003C_003Eu__1;
					_003C_003Eu__1 = default(ConfiguredTaskAwaiter);
					num = (_003C_003E1__state = -1);
					goto IL_0176;
					IL_017d:
					if (_003ClogEntry_003E5__5.Manifest != null)
					{
						global::System.Collections.Generic.IEnumerator<IManifestProduct> enumerator = _003ClogEntry_003E5__5.Manifest.Products.GetEnumerator();
						try
						{
							while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
							{
								IManifestProduct current = enumerator.Current;
								_003ClastSeenProductDict_003E5__2[current.UniqueID] = current;
							}
						}
						finally
						{
							if (num < 0)
							{
								((global::System.IDisposable)enumerator)?.Dispose();
							}
						}
						manifestLogReceiver?.LogManifest(_003ClogEntry_003E5__5.Manifest);
					}
					else
					{
						if (_003ClogEntry_003E5__5.ProductUniqueID == null)
						{
							throw new global::System.Exception($"Invalid log entry {_003ClogEntriesProcessed_003E5__3}, expected a Manifest or Product specifier");
						}
						if (!_003ClastSeenProductDict_003E5__2.ContainsKey(_003ClogEntry_003E5__5.ProductUniqueID))
						{
							throw new global::System.Exception($"Invalid log entry {_003ClogEntriesProcessed_003E5__3}, no information found for product {_003ClogEntry_003E5__5.ProductUniqueID}");
						}
						IManifestProduct product = _003ClastSeenProductDict_003E5__2[_003ClogEntry_003E5__5.ProductUniqueID];
						switch (_003ClogEntry_003E5__5.DTCsType)
						{
						case ManifestDTCListType.Current:
							manifestLogReceiver?.LogCurrentDTCs(product, _003ClogEntry_003E5__5.DTCs);
							break;
						case ManifestDTCListType.Delta:
							manifestLogReceiver?.LogChangedDTCs(product, _003ClogEntry_003E5__5.DTCs);
							break;
						case ManifestDTCListType.None:
							throw new global::System.Exception($"Invalid log entry {_003ClogEntriesProcessed_003E5__3}, expected Current or Delta DTC type");
						}
					}
					dateTime = _003ClogEntry_003E5__5.Timestamp;
					_003ClogEntriesProcessed_003E5__3++;
					_003ClogEntry_003E5__5 = null;
					goto IL_0395;
					IL_0395:
					while (_003C_003E7__wrap3.MoveNext())
					{
						_003ClogEntry_003E5__5 = _003C_003E7__wrap3.Current;
						if (_003ClogEntry_003E5__5 == null)
						{
							continue;
						}
						goto IL_009c;
					}
					goto end_IL_0075;
					IL_0176:
					((ConfiguredTaskAwaiter)(ref val2)).GetResult();
					goto IL_017d;
					IL_009c:
					if (_003ClogEntriesProcessed_003E5__3 == 0)
					{
						dateTime = _003ClogEntry_003E5__5.Timestamp;
					}
					TimeSpan val3 = _003ClogEntry_003E5__5.Timestamp - dateTime;
					int num2 = ((!((double)speedFactor < 0.001)) ? ((int)(((((TimeSpan)(ref val3)).TotalMilliseconds < 0.0) ? 0.0 : ((TimeSpan)(ref val3)).TotalMilliseconds) / (double)speedFactor)) : 0);
					if (num2 > 0)
					{
						ConfiguredTaskAwaitable val4 = global::System.Threading.Tasks.Task.Delay(num2, cancellationToken).ConfigureAwait(false);
						val2 = ((ConfiguredTaskAwaitable)(ref val4)).GetAwaiter();
						if (!((ConfiguredTaskAwaiter)(ref val2)).IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, _003CReplay_003Ed__4>(ref val2, ref this);
							return;
						}
						goto IL_0176;
					}
					goto IL_017d;
					end_IL_0075:;
				}
				finally
				{
					if (num < 0)
					{
						((global::System.IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
					}
				}
				_003C_003E7__wrap3 = default(Enumerator<IManifestLogEntry>);
				result = _003ClogEntriesProcessed_003E5__3;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003ClastSeenProductDict_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003ClastSeenProductDict_003E5__2 = null;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	private const string LogTag = "ManifestLogReceiverPlayer";

	private global::System.Collections.Generic.IEnumerable<IManifestLogEntry>? _webLogEntries;

	private readonly object _locker = new object();

	public void LoadWebServiceLog(global::System.Collections.Generic.IEnumerable<IManifestLogEntry> webServiceLogEntries)
	{
		if (webServiceLogEntries == null)
		{
			throw new global::System.Exception("Invalid manifest");
		}
		lock (_locker)
		{
			_webLogEntries = webServiceLogEntries;
		}
	}

	[AsyncStateMachine(typeof(_003CReplay_003Ed__4))]
	public async global::System.Threading.Tasks.Task<uint> Replay(IManifestLogReceiver manifestLogReceiver, CancellationToken cancellationToken, float speedFactor = 1f)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		((CancellationToken)(ref cancellationToken)).ThrowIfCancellationRequested();
		List<IManifestLogEntry> val;
		lock (_locker)
		{
			val = new List<IManifestLogEntry>(_webLogEntries);
		}
		Dictionary<string, IManifestProduct> lastSeenProductDict = new Dictionary<string, IManifestProduct>();
		uint logEntriesProcessed = 0u;
		global::System.DateTime dateTime = default(global::System.DateTime);
		Enumerator<IManifestLogEntry> enumerator = val.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				IManifestLogEntry logEntry = enumerator.Current;
				if (logEntry == null)
				{
					continue;
				}
				if (logEntriesProcessed == 0)
				{
					dateTime = logEntry.Timestamp;
				}
				TimeSpan val2 = logEntry.Timestamp - dateTime;
				int num = ((!((double)speedFactor < 0.001)) ? ((int)(((((TimeSpan)(ref val2)).TotalMilliseconds < 0.0) ? 0.0 : ((TimeSpan)(ref val2)).TotalMilliseconds) / (double)speedFactor)) : 0);
				if (num > 0)
				{
					await global::System.Threading.Tasks.Task.Delay(num, cancellationToken).ConfigureAwait(false);
				}
				if (logEntry.Manifest != null)
				{
					global::System.Collections.Generic.IEnumerator<IManifestProduct> enumerator2 = logEntry.Manifest.Products.GetEnumerator();
					try
					{
						while (((global::System.Collections.IEnumerator)enumerator2).MoveNext())
						{
							IManifestProduct current = enumerator2.Current;
							lastSeenProductDict[current.UniqueID] = current;
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator2)?.Dispose();
					}
					manifestLogReceiver?.LogManifest(logEntry.Manifest);
				}
				else
				{
					if (logEntry.ProductUniqueID == null)
					{
						throw new global::System.Exception($"Invalid log entry {logEntriesProcessed}, expected a Manifest or Product specifier");
					}
					if (!lastSeenProductDict.ContainsKey(logEntry.ProductUniqueID))
					{
						throw new global::System.Exception($"Invalid log entry {logEntriesProcessed}, no information found for product {logEntry.ProductUniqueID}");
					}
					IManifestProduct product = lastSeenProductDict[logEntry.ProductUniqueID];
					switch (logEntry.DTCsType)
					{
					case ManifestDTCListType.Current:
						manifestLogReceiver?.LogCurrentDTCs(product, logEntry.DTCs);
						break;
					case ManifestDTCListType.Delta:
						manifestLogReceiver?.LogChangedDTCs(product, logEntry.DTCs);
						break;
					case ManifestDTCListType.None:
						throw new global::System.Exception($"Invalid log entry {logEntriesProcessed}, expected Current or Delta DTC type");
					}
				}
				dateTime = logEntry.Timestamp;
				logEntriesProcessed++;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
		return logEntriesProcessed;
	}
}
