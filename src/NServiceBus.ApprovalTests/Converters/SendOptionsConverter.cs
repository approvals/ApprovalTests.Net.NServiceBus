using System;
using System.Linq;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Extensibility;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class SendOptionsConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var options = (SendOptions) value;
        writer.WriteStartObject();

        var deliveryDate = options.GetDeliveryDate();
        if (deliveryDate != null)
        {
            writer.WritePropertyName("DeliveryDate");
            serializer.Serialize(writer, deliveryDate);
        }
        var deliveryDelay = options.GetDeliveryDelay();
        if (deliveryDelay != null)
        {
            writer.WritePropertyName("DeliveryDelay");
            serializer.Serialize(writer, deliveryDelay);
        }
        ExtendableOptionsConverter.WriteBaseMembers(writer, serializer, options);

        writer.WriteEndObject();
    }

    public static void WriteBaseMembers(JsonWriter writer, JsonSerializer serializer, ExtendableOptions options)
    {
        var messageId = options.GetMessageId();
        if (messageId != null)
        {
            writer.WritePropertyName("MessageId");
            serializer.Serialize(writer, messageId);
        }

        var headers = options.GetCleanedHeaders();
        if (headers.Any())
        {
            writer.WritePropertyName("Headers");
            serializer.Serialize(writer, headers);
        }

        var extensions = options.GetExtensions();
        if (extensions != null)
        {
            if (ContextBagHelper.HasContent(extensions))
            {
                writer.WritePropertyName("Extensions");
                serializer.Serialize(writer, extensions);
            }
        }
    }

    public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type type)
    {
        return typeof(SendOptions).IsAssignableFrom(type);
    }
}