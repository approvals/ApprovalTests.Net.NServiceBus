using System;
using System.Linq;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Extensibility;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class ExtendableOptionsConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var options = (ExtendableOptions) value;
        writer.WriteStartObject();

        writer.WritePropertyName("MessageId");
        serializer.Serialize(writer, options.GetMessageId());
        var headers = options.GetHeaders();
        if (headers.Any())
        {
            writer.WritePropertyName("Headers");
            serializer.Serialize(writer, headers);
        }
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type type)
    {
        return typeof(ExtendableOptions).IsAssignableFrom(type);
    }
}