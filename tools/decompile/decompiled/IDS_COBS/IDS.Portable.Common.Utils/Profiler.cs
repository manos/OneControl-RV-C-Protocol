using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace IDS.Portable.Common.Utils;

public static class Profiler
{
	public const string LogTag = "Profiler";

	private const string LogMessageTemplate = "Stopwatch {0} took {1}{2}";

	private static readonly ConcurrentDictionary<string, Stopwatch> watches = new ConcurrentDictionary<string, Stopwatch>();

	public static void Start(object view)
	{
		Start(((MemberInfo)view.GetType()).Name);
	}

	public static void Start(string tag, string comment = "")
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0044: Expected O, but got Unknown
		if (!string.IsNullOrEmpty(comment))
		{
			comment = " : " + comment;
		}
		TaggedLog.Debug("Profiler", "Starting Stopwatch {0}{1}", tag, comment);
		ConcurrentDictionary<string, Stopwatch> obj = watches;
		Stopwatch val = new Stopwatch();
		Stopwatch val2 = val;
		obj[tag] = val;
		val2.Start();
	}

	public static void Stop(string tag, string comment = "")
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		Stopwatch val = default(Stopwatch);
		if (watches.TryRemove(tag, ref val))
		{
			if (!string.IsNullOrEmpty(comment))
			{
				comment = " : " + comment;
			}
			TaggedLog.Debug("Profiler", "Stopwatch {0} took {1}{2}", tag, (val != null) ? new TimeSpan?(val.Elapsed) : ((TimeSpan?)null), comment);
			if (val != null)
			{
				val.Stop();
			}
		}
	}

	public static void Split(string tag, string comment = "")
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		Stopwatch val = default(Stopwatch);
		if (watches.TryGetValue(tag, ref val))
		{
			if (!string.IsNullOrEmpty(comment))
			{
				comment = " : " + comment;
			}
			TaggedLog.Debug("Profiler", "Stopwatch {0} took {1}{2}", tag, (val != null) ? new TimeSpan?(val.Elapsed) : ((TimeSpan?)null), comment);
		}
	}
}
