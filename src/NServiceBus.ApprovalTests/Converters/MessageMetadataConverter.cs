using System;
using System.Linq;
using Newtonsoft.Json;
using NServiceBus.Unicast.Messages;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class MessageMetadataConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var options = (MessageMetadata) value;
        writer.WriteStartObject();
        if (options.MessageHierarchy.Any())
        {
            writer.WritePropertyName("MessageHierarchy");
            serializer.Serialize(writer, options.MessageHierarchy);
        }
        writer.WritePropertyName("MessageType");
        serializer.Serialize(writer, options.MessageType);
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type type)
    {
        return typeof(MessageMetadata).IsAssignableFrom(type);
    }
}