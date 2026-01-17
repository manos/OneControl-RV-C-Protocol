using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ids.portable.common.Exceptions;

namespace ids.portable.common.Extensions;

public static class FileExtension
{
	public enum FileIoLocation
	{
		DocumentFolder
	}

	public enum FileType
	{
		Document,
		Video,
		File
	}

	public static SpecialFolder GetFolderLocation(this FileIoLocation location)
	{
		return (SpecialFolder)5;
	}

	public static string GetFullFilePath(string baseFilename, FileIoLocation location = FileIoLocation.DocumentFolder)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Path.Combine(Environment.GetFolderPath(location.GetFolderLocation()), baseFilename);
	}

	public static void Delete(this string filename, FileIoLocation location = FileIoLocation.DocumentFolder)
	{
		File.Delete(GetFullFilePath(filename, location));
	}

	public static bool TryDelete(this string filename, FileIoLocation location = FileIoLocation.DocumentFolder)
	{
		try
		{
			filename.Delete(location);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static void SaveText(this string filename, string text, FileIoLocation location = FileIoLocation.DocumentFolder)
	{
		try
		{
			File.WriteAllText(GetFullFilePath(filename, location), text);
		}
		catch (global::System.Exception ex)
		{
			throw new FileExtensionException($"Error saving text to {filename} in {location} location: {ex.Message}", ex);
		}
	}

	public static global::System.Threading.Tasks.Task SaveTextAsync(this string filename, string text, FileIoLocation location = FileIoLocation.DocumentFolder, CancellationToken cancellationToken = default(CancellationToken))
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		return global::System.Threading.Tasks.Task.Run((Action)delegate
		{
			filename.SaveText(text, location);
		}, cancellationToken);
	}

	public static string LoadText(this string filename, FileIoLocation location = FileIoLocation.DocumentFolder)
	{
		return File.ReadAllText(GetFullFilePath(filename, location));
	}

	public static global::System.Threading.Tasks.Task<string> LoadTextAsync(this string filename, FileIoLocation location = FileIoLocation.DocumentFolder)
	{
		return global::System.Threading.Tasks.Task.Run<string>((Func<string>)(() => filename.LoadText(location)));
	}

	public static global::System.Threading.Tasks.Task MoveAsync(this string fromFilename, string toFilename, FileIoLocation location = FileIoLocation.DocumentFolder)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		if (string.IsNullOrEmpty(fromFilename) || string.IsNullOrEmpty(toFilename))
		{
			throw new ArgumentException("Invalid To/From Filename - null or empty string");
		}
		string fromFilenameFull = GetFullFilePath(fromFilename, location);
		string toFilenameFull = GetFullFilePath(toFilename, location);
		return global::System.Threading.Tasks.Task.Run((Action)delegate
		{
			File.Move(fromFilenameFull, toFilenameFull);
		});
	}

	public static string LoadTextFromAssemblyResource(this Assembly assembly, string location)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		string result = string.Empty;
		Stream manifestResourceStream = assembly.GetManifestResourceStream(location);
		try
		{
			if (manifestResourceStream == null)
			{
				return result;
			}
			StreamReader val = new StreamReader(manifestResourceStream);
			try
			{
				result = ((TextReader)val).ReadToEnd();
				((TextReader)val).Close();
			}
			finally
			{
				((global::System.IDisposable)val)?.Dispose();
			}
			manifestResourceStream.Close();
			return result;
		}
		finally
		{
			((global::System.IDisposable)manifestResourceStream)?.Dispose();
		}
	}

	public static FileType GetFileType(this FileInfo fileInfo)
	{
		return ((FileSystemInfo)fileInfo).Extension.GetFileTypeFromFileName();
	}

	public static FileType GetFileTypeFromFileName(this string fileName)
	{
		string extension = Path.GetExtension(fileName);
		extension = extension.TrimStart('.');
		if (!(extension == "mp4") && !(extension == "avi"))
		{
			if (extension == "pdf" || extension == "doc" || extension == "txt")
			{
				return FileType.Document;
			}
			return FileType.File;
		}
		return FileType.Video;
	}
}
