using System;
using Newtonsoft.Json;
using NServiceBus.Testing;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class UnsubscriptionConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var unsubscription = (Unsubscription) value;
        writer.WriteStartObject();

        writer.WritePropertyName("MessageType");
        serializer.Serialize(writer, unsubscription.Message);
        var options = unsubscription.Options;
        if (options.HasValue())
        {
            writer.WritePropertyName("Options");
            serializer.Serialize(writer, options);
        }

        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type type)
    {
        return typeof(Unsubscription).IsAssignableFrom(type);
    }
}