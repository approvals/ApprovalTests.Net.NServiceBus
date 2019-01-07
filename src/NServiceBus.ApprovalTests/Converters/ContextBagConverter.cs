using System;
using Newtonsoft.Json;
using NServiceBus.Extensibility;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class ContextBagConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        if (ContextBagHelper.TryGetStash(value, out var stash))
        {
            writer.WritePropertyName("Stash");
            serializer.Serialize(writer, stash);
        }
        if (ContextBagHelper.TryGetParentBag(value, out var parentBag))
        {
            writer.WritePropertyName("Parent");
            serializer.Serialize(writer, parentBag);
        }

        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type type)
    {
        return typeof(ContextBag).IsAssignableFrom(type);
    }
}