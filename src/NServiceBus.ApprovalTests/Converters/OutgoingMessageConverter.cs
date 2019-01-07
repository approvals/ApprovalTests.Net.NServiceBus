using System;
using Newtonsoft.Json;
using NServiceBus.Testing;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class OutgoingMessageConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        var message = OutgoingMessageHelper.GetMessage(value);

        writer.WritePropertyName("Message");
        serializer.Serialize(writer, message);

        writer.WritePropertyName("MessageType");
        var type = message.GetType();
        serializer.Serialize(writer, type.FullName);

        var options = OutgoingMessageHelper.GetOptions(value);
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
        var baseType = type.BaseType;
        if (baseType != null && baseType.IsGenericType)
        {
            var genericTypeDefinition = baseType.GetGenericTypeDefinition();
            return genericTypeDefinition==typeof(OutgoingMessage<,>);
        }

        return false;
    }
}