using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Core;
using Serilog.Events;

namespace IDS.Portable.Common;

public class TagEnricher : Singleton<TagEnricher>, ILogEventEnricher
{
	public const string PropertyKey = "Tag";

	private TagEnricher()
	{
	}

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		string text = Enumerable.FirstOrDefault<string>(Enumerable.Select<KeyValuePair<string, LogEventPropertyValue>, string>(Enumerable.Where<KeyValuePair<string, LogEventPropertyValue>>((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, LogEventPropertyValue>>)logEvent.Properties, (Func<KeyValuePair<string, LogEventPropertyValue>, bool>)((KeyValuePair<string, LogEventPropertyValue> kvp) => kvp.Key == "SourceContext")), (Func<KeyValuePair<string, LogEventPropertyValue>, string>)((KeyValuePair<string, LogEventPropertyValue> kvp) => kvp.Value.ToString("l", (IFormatProvider)null)))) ?? "";
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Tag", (object)text, false));
	}
}
