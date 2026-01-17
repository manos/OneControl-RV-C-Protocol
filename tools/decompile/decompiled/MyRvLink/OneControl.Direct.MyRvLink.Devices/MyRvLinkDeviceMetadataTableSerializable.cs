using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using IDS.Portable.Common;
using IDS.Portable.LogicalDevice.Json;
using Newtonsoft.Json;
using ids.portable.common.Extensions;

namespace OneControl.Direct.MyRvLink.Devices;

[JsonObject(/*Could not decode attribute arguments.*/)]
public class MyRvLinkDeviceMetadataTableSerializable : JsonSerializable<MyRvLinkDeviceTableSerializable>
{
	public const string LogTag = "MyRvLinkDeviceMetadataTableSerializable";

	public static string BaseFilename = "MyRvLinkDeviceTable";

	public static string BaseFilenameExtension = "json";

	private const string PrefixFilename = "MetadataV1";

	public static readonly string SaveFolder = Environment.GetFolderPath((SpecialFolder)5);

	[JsonProperty]
	[field: CompilerGenerated]
	public uint DeviceMetadataTableCrc
	{
		[CompilerGenerated]
		get;
	}

	[JsonProperty]
	[field: CompilerGenerated]
	public global::System.Collections.Generic.IReadOnlyList<MyRvLinkDeviceMetadataSerializable> DevicesMetadataSerializable
	{
		[CompilerGenerated]
		get;
	}

	private static string FilenamePattern => BaseFilename + "MetadataV1*." + BaseFilenameExtension;

	[JsonConstructor]
	public MyRvLinkDeviceMetadataTableSerializable(uint deviceMetadataTableCrc, global::System.Collections.Generic.IReadOnlyList<MyRvLinkDeviceMetadataSerializable> devicesMetadataSerializable)
	{
		DeviceMetadataTableCrc = deviceMetadataTableCrc;
		DevicesMetadataSerializable = (global::System.Collections.Generic.IReadOnlyList<MyRvLinkDeviceMetadataSerializable>)Enumerable.ToList<MyRvLinkDeviceMetadataSerializable>((global::System.Collections.Generic.IEnumerable<MyRvLinkDeviceMetadataSerializable>)devicesMetadataSerializable);
	}

	public global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDeviceMetadata> TryDecode()
	{
		return (global::System.Collections.Generic.IReadOnlyList<IMyRvLinkDeviceMetadata>)Enumerable.ToList<IMyRvLinkDeviceMetadata>(Enumerable.Select<MyRvLinkDeviceMetadataSerializable, IMyRvLinkDeviceMetadata>((global::System.Collections.Generic.IEnumerable<MyRvLinkDeviceMetadataSerializable>)DevicesMetadataSerializable, (Func<MyRvLinkDeviceMetadataSerializable, IMyRvLinkDeviceMetadata>)((MyRvLinkDeviceMetadataSerializable device) => device.TryDecode())));
	}

	private static string MakeFilename(string deviceSourceToken, uint deviceTableCrc)
	{
		return $"{BaseFilename}{"MetadataV1"}_{deviceSourceToken}_{deviceTableCrc:x8}.{BaseFilenameExtension}";
	}

	public bool TrySave(string deviceSourceToken)
	{
		string text = MakeFilename(deviceSourceToken, DeviceMetadataTableCrc);
		try
		{
			TaggedLog.Information("MyRvLinkDeviceMetadataTableSerializable", "Saving MyRvLink Device Metadata Table " + text, global::System.Array.Empty<object>());
			string text2 = JsonConvert.SerializeObject((object)this, (Formatting)1);
			FileExtension.SaveText(text, text2, (FileIoLocation)0);
			return true;
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Error("MyRvLinkDeviceMetadataTableSerializable", "Unable to save MyRvLink Device Metadata Table " + text + ": " + ex.Message, global::System.Array.Empty<object>());
			return false;
		}
	}

	public static bool TryLoad(string deviceSourceToken, uint deviceMetadataTableCrc, out MyRvLinkDeviceMetadataTableSerializable? deviceMetadataTableSerializable)
	{
		string text = MakeFilename(deviceSourceToken, deviceMetadataTableCrc);
		try
		{
			deviceMetadataTableSerializable = null;
			string text2 = FileExtension.LoadText(text, (FileIoLocation)0);
			if (string.IsNullOrWhiteSpace(text2))
			{
				throw new global::System.Exception("json is null or empty");
			}
			TaggedLog.Information("MyRvLinkDeviceMetadataTableSerializable", "Loaded MyRvLink Device Metadata Table " + text, global::System.Array.Empty<object>());
			deviceMetadataTableSerializable = JsonConvert.DeserializeObject<MyRvLinkDeviceMetadataTableSerializable>(text2);
		}
		catch (FileNotFoundException)
		{
			deviceMetadataTableSerializable = null;
		}
		catch (global::System.Exception ex2)
		{
			TaggedLog.Warning("MyRvLinkDeviceMetadataTableSerializable", "Unable to load MyRvLink Device Metadata Table: " + ex2.Message, global::System.Array.Empty<object>());
			deviceMetadataTableSerializable = null;
		}
		return deviceMetadataTableSerializable != null;
	}

	public static void TryClearCache()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			FileInfo[] files = new DirectoryInfo(SaveFolder).GetFiles(FilenamePattern);
			foreach (FileInfo val in files)
			{
				TaggedLog.Information("MyRvLinkDeviceMetadataTableSerializable", "Removing MyRvLink Device Metadata Table `" + ((FileSystemInfo)val).Name + "`", global::System.Array.Empty<object>());
				FileExtension.TryDelete(((FileSystemInfo)val).Name, (FileIoLocation)0);
			}
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Information("MyRvLinkDeviceMetadataTableSerializable", "Unable to copy all MyRvLink Metadata files " + ex.Message, global::System.Array.Empty<object>());
		}
	}
}
