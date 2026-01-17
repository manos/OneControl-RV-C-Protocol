using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IDS.Portable.Common;

public class JsonSerializerInterfaceListConverter<TJsonSerializerInterface> : JsonSerializerInterfaceListConverter<TJsonSerializerInterface, TJsonSerializerInterface> where TJsonSerializerInterface : IJsonSerializerClass
{
}
public class JsonSerializerInterfaceListConverter<TJsonSerializerInterface, TJsonSerializerDefaultClass> : JsonConverter where TJsonSerializerInterface : IJsonSerializerClass where TJsonSerializerDefaultClass : TJsonSerializerInterface
{
	public const string LogTag = "JsonSerializerInterfaceListConverter";

	public override bool CanWrite => false;

	public override bool CanRead => true;

	public virtual global::System.Type? DefaultConstructionType()
	{
		return null;
	}

	public override bool CanConvert(global::System.Type objectType)
	{
		return objectType == typeof(List<TJsonSerializerInterface>);
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		throw new InvalidOperationException("Use default serialization.");
	}

	public override object ReadJson(JsonReader reader, global::System.Type objectType, object? existingValue, JsonSerializer serializer)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		JToken val = JToken.Load(reader);
		if ((int)val.Type != 2)
		{
			throw new JsonSerializationException($"Unexpected JSON format encountered {val}");
		}
		List<TJsonSerializerInterface> val2 = new List<TJsonSerializerInterface>();
		global::System.Collections.Generic.IEnumerator<JToken> enumerator = val.Children().GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				JToken current = enumerator.Current;
				try
				{
					global::System.Type type = JsonSerializerInterfaceObjectConverter<TJsonSerializerInterface, TJsonSerializerDefaultClass>.ResolveSerializer((string)current[(object)"SerializerClass"]);
					if (current.ToObject(type ?? DefaultConstructionType()) is TJsonSerializerInterface val3)
					{
						val2.Add(val3);
					}
				}
				catch (global::System.Exception ex)
				{
					TaggedLog.Error("JsonSerializerInterfaceListConverter", "Invalid item token {0}: {1}", current, ex.Message);
				}
			}
			return val2;
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
	}
}
