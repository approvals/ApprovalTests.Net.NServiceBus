using System;
using System.Linq;
using Newtonsoft.Json;
using NServiceBus.Transport;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class TransportOperationConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var options = (TransportOperation) value;
        writer.WriteStartObject();

        writer.WritePropertyName("AddressTag");
        serializer.Serialize(writer, options.AddressTag);
        writer.WritePropertyName("RequiredDispatchConsistency");
        serializer.Serialize(writer, options.RequiredDispatchConsistency);
        writer.WritePropertyName("Message");
        serializer.Serialize(writer, options.Message);
        if (options.DeliveryConstraints.Any())
        {
            writer.WritePropertyName("DeliveryConstraints");
            serializer.Serialize(writer, options.DeliveryConstraints);
        }
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type type)
    {
        return typeof(TransportOperation).IsAssignableFrom(type);
    }
}