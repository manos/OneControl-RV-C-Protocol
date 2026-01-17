using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using IDS.Portable.Common;
using IDS.Portable.LogicalDevice.Json;
using Newtonsoft.Json;
using OneControl.Direct.MyRvLink.Devices;
using ids.portable.common.Extensions;

namespace OneControl.Direct.MyRvLink.Cache;

[JsonObject(/*Could not decode attribute arguments.*/)]
public class DeviceTableIdCacheSerializable : JsonSerializable<DeviceTableIdCacheSerializable>
{
	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CTryLoadAsync_003Ed__19 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<DeviceTableIdCacheSerializable> _003C_003Et__builder;

		public string deviceSourceToken;

		private string _003Cfilename_003E5__2;

		private TaskAwaiter<string> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DeviceTableIdCacheSerializable result2;
			try
			{
				if (num != 0)
				{
					_003Cfilename_003E5__2 = MakeFilename(deviceSourceToken);
				}
				DeviceTableIdCacheSerializable deviceTableIdCacheSerializable;
				try
				{
					TaskAwaiter<string> val;
					if (num != 0)
					{
						val = FileExtension.LoadTextAsync(_003Cfilename_003E5__2, (FileIoLocation)0).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, _003CTryLoadAsync_003Ed__19>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<string>);
						num = (_003C_003E1__state = -1);
					}
					string result = val.GetResult();
					if (string.IsNullOrWhiteSpace(result))
					{
						throw new global::System.Exception("json is null or empty");
					}
					TaggedLog.Information("DeviceTableIdCacheSerializable", "Loaded MyRvLink Device Table " + _003Cfilename_003E5__2, global::System.Array.Empty<object>());
					deviceTableIdCacheSerializable = JsonConvert.DeserializeObject<DeviceTableIdCacheSerializable>(result);
					if (deviceTableIdCacheSerializable.DeviceSourceToken != deviceSourceToken)
					{
						throw new global::System.Exception("Device source tokens don't match " + deviceTableIdCacheSerializable.DeviceSourceToken + " != " + deviceSourceToken);
					}
				}
				catch (FileNotFoundException)
				{
					deviceTableIdCacheSerializable = null;
				}
				catch (global::System.Exception ex2)
				{
					TaggedLog.Warning("DeviceTableIdCacheSerializable", "Unable to load MyRvLink Device Table: " + ex2.Message, global::System.Array.Empty<object>());
					deviceTableIdCacheSerializable = null;
				}
				result2 = deviceTableIdCacheSerializable;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003Cfilename_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003Cfilename_003E5__2 = null;
			_003C_003Et__builder.SetResult(result2);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CTrySaveAsync_003Ed__18 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<bool> _003C_003Et__builder;

		public DeviceTableIdCacheSerializable _003C_003E4__this;

		private string _003Cfilename_003E5__2;

		private TaskAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			DeviceTableIdCacheSerializable deviceTableIdCacheSerializable = _003C_003E4__this;
			bool result;
			try
			{
				if (num != 0)
				{
					_003Cfilename_003E5__2 = MakeFilename(deviceTableIdCacheSerializable.DeviceSourceToken);
				}
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						TaggedLog.Information("DeviceTableIdCacheSerializable", "Saving MyRvLink Device Table " + _003Cfilename_003E5__2, global::System.Array.Empty<object>());
						string text = JsonConvert.SerializeObject((object)deviceTableIdCacheSerializable, (Formatting)1);
						val = FileExtension.SaveTextAsync(_003Cfilename_003E5__2, text, (FileIoLocation)0, default(CancellationToken)).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, _003CTrySaveAsync_003Ed__18>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
					}
					((TaskAwaiter)(ref val)).GetResult();
					result = true;
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Error("DeviceTableIdCacheSerializable", "Unable to save MyRvLink Device Table " + _003Cfilename_003E5__2 + ": " + ex.Message, global::System.Array.Empty<object>());
					result = false;
				}
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003Cfilename_003E5__2 = null;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003Cfilename_003E5__2 = null;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	public const string LogTag = "DeviceTableIdCacheSerializable";

	public static string BaseFilename = "MyRvLinkDeviceTableIdCache";

	public static string BaseFilenameExtension = "json";

	private const string PrefixFilename = "V1";

	[JsonProperty]
	[field: CompilerGenerated]
	public string DeviceSourceToken
	{
		[CompilerGenerated]
		get;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	protected ConcurrentDictionary<byte, MyRvLinkDeviceTableSerializable> DeviceTableDictionary
	{
		[CompilerGenerated]
		get;
	}

	private static string FilenamePattern => BaseFilename + "V1*." + BaseFilenameExtension;

	public DeviceTableIdCacheSerializable(string deviceSourceToken)
		: this(deviceSourceToken, new ConcurrentDictionary<byte, MyRvLinkDeviceTableSerializable>())
	{
	}

	[JsonConstructor]
	public DeviceTableIdCacheSerializable(string deviceSourceToken, ConcurrentDictionary<byte, MyRvLinkDeviceTableSerializable> deviceTableDictionary)
	{
		DeviceSourceToken = deviceSourceToken ?? "Unknown";
		DeviceTableDictionary = deviceTableDictionary ?? new ConcurrentDictionary<byte, MyRvLinkDeviceTableSerializable>();
	}

	public MyRvLinkDeviceTableSerializable? GetDeviceTableIdSerializableForTableId(byte deviceTableId)
	{
		return DictionaryExtension.TryGetValue<byte, MyRvLinkDeviceTableSerializable>(DeviceTableDictionary, deviceTableId);
	}

	public MyRvLinkDeviceTableSerializable? GetFirstDeviceTableIdSerializableForCrc(uint deviceTableCrc)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		global::System.Collections.Generic.IEnumerator<KeyValuePair<byte, MyRvLinkDeviceTableSerializable>> enumerator = DeviceTableDictionary.GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				KeyValuePair<byte, MyRvLinkDeviceTableSerializable> current = enumerator.Current;
				if (current.Value.DeviceTableCrc == deviceTableCrc)
				{
					return current.Value;
				}
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		return null;
	}

	public void Update(byte deviceTableId, MyRvLinkDeviceTableSerializable deviceTableSerializable)
	{
		DeviceTableDictionary[deviceTableId] = deviceTableSerializable;
	}

	private static string MakeFilename(string deviceSourceToken)
	{
		return $"{BaseFilename}{"V1"}_{deviceSourceToken}.{BaseFilenameExtension}";
	}

	[AsyncStateMachine(typeof(_003CTrySaveAsync_003Ed__18))]
	public async global::System.Threading.Tasks.Task<bool> TrySaveAsync()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		string filename = MakeFilename(DeviceSourceToken);
		try
		{
			TaggedLog.Information("DeviceTableIdCacheSerializable", "Saving MyRvLink Device Table " + filename, global::System.Array.Empty<object>());
			string text = JsonConvert.SerializeObject((object)this, (Formatting)1);
			await FileExtension.SaveTextAsync(filename, text, (FileIoLocation)0, default(CancellationToken));
			return true;
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Error("DeviceTableIdCacheSerializable", "Unable to save MyRvLink Device Table " + filename + ": " + ex.Message, global::System.Array.Empty<object>());
			return false;
		}
	}

	[AsyncStateMachine(typeof(_003CTryLoadAsync_003Ed__19))]
	public static async global::System.Threading.Tasks.Task<DeviceTableIdCacheSerializable?> TryLoadAsync(string deviceSourceToken)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		string filename = MakeFilename(deviceSourceToken);
		DeviceTableIdCacheSerializable deviceTableIdCacheSerializable;
		try
		{
			string obj = await FileExtension.LoadTextAsync(filename, (FileIoLocation)0);
			if (string.IsNullOrWhiteSpace(obj))
			{
				throw new global::System.Exception("json is null or empty");
			}
			TaggedLog.Information("DeviceTableIdCacheSerializable", "Loaded MyRvLink Device Table " + filename, global::System.Array.Empty<object>());
			deviceTableIdCacheSerializable = JsonConvert.DeserializeObject<DeviceTableIdCacheSerializable>(obj);
			if (deviceTableIdCacheSerializable.DeviceSourceToken != deviceSourceToken)
			{
				throw new global::System.Exception("Device source tokens don't match " + deviceTableIdCacheSerializable.DeviceSourceToken + " != " + deviceSourceToken);
			}
		}
		catch (FileNotFoundException)
		{
			deviceTableIdCacheSerializable = null;
		}
		catch (global::System.Exception ex2)
		{
			TaggedLog.Warning("DeviceTableIdCacheSerializable", "Unable to load MyRvLink Device Table: " + ex2.Message, global::System.Array.Empty<object>());
			deviceTableIdCacheSerializable = null;
		}
		return deviceTableIdCacheSerializable;
	}
}
