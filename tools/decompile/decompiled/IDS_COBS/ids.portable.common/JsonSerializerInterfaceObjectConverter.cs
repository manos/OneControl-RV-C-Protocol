using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IDS.Portable.Common;

public class JsonSerializerInterfaceObjectConverter<TJsonSerializerInterface> : JsonSerializerInterfaceObjectConverter<TJsonSerializerInterface, TJsonSerializerInterface> where TJsonSerializerInterface : IJsonSerializerClass
{
}
public class JsonSerializerInterfaceObjectConverter<TJsonSerializerInterface, TJsonSerializerDefaultClass> : JsonConverter where TJsonSerializerInterface : IJsonSerializerClass where TJsonSerializerDefaultClass : TJsonSerializerInterface
{
	public const string LogTag = "JsonSerializerInterfaceObjectConverter";

	public override bool CanWrite => false;

	public override bool CanRead => true;

	public override bool CanConvert(global::System.Type objectType)
	{
		return objectType == typeof(TJsonSerializerInterface);
	}

	public virtual global::System.Type? DefaultConstructionType()
	{
		return null;
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		throw new InvalidOperationException("Use default serialization.");
	}

	public override object? ReadJson(JsonReader reader, global::System.Type objectType, object? existingValue, JsonSerializer serializer)
	{
		JObject obj = JObject.Load(reader);
		JToken obj2 = obj["SerializerClass"];
		global::System.Type type = ResolveSerializer((obj2 != null) ? Extensions.Value<string>((global::System.Collections.Generic.IEnumerable<JToken>)obj2) : null);
		return ((JToken)obj).ToObject(type ?? DefaultConstructionType());
	}

	public static global::System.Type? ResolveSerializer(string serializerClassName)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		global::System.Type type = null;
		try
		{
			if (string.IsNullOrEmpty(serializerClassName))
			{
				throw new ArgumentException("Invalid/unknown serialization class specified");
			}
			type = TypeRegistry.Lookup(serializerClassName);
			if (type == (global::System.Type)null)
			{
				type = typeof(TJsonSerializerDefaultClass);
				TaggedLog.Error("JsonSerializerInterfaceObjectConverter", "ReadJson unable to resolve serializerClass from TypeRegistry for {0} of type {1}, using default serializer {2} in its place", serializerClassName, typeof(TJsonSerializerInterface), type);
			}
			if (!Enumerable.Contains<global::System.Type>((global::System.Collections.Generic.IEnumerable<global::System.Type>)type.GetInterfaces(), typeof(TJsonSerializerInterface)))
			{
				type = typeof(TJsonSerializerDefaultClass);
				TaggedLog.Error("JsonSerializerInterfaceObjectConverter", "ReadJson serializerClassName for {0} of type {1}, doesn't implement {2}, using default serializer {3} in its place", serializerClassName, typeof(TJsonSerializerInterface), typeof(TJsonSerializerInterface), type);
			}
		}
		catch (ArgumentNullException)
		{
		}
		catch (global::System.Exception ex2)
		{
			TaggedLog.Error("JsonSerializerInterfaceObjectConverter", "Unable to resolve ResolveSerializer for {0}: {1}", serializerClassName, ex2.Message);
		}
		return type;
	}
}
