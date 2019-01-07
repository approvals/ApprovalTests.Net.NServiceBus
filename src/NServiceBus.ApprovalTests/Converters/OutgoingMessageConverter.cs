using System;
using Newtonsoft.Json;
using NServiceBus.Testing;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class OutgoingMessageConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        WriteBaseMembers(writer, value, serializer);

        writer.WriteEndObject();
    }

    public static void WriteBaseMembers(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var message = OutgoingMessageHelper.GetMessage(value);

        writer.WritePropertyName("MessageType");
        var type = message.GetType();
        serializer.Serialize(writer, type.FullName);

        writer.WritePropertyName("Message");
        serializer.Serialize(writer, message);

        var options = OutgoingMessageHelper.GetOptions(value);
        if (options.HasValue())
        {
            writer.WritePropertyName("Options");
            serializer.Serialize(writer, options);
        }
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
            var typeDefinition = baseType.GetGenericTypeDefinition();
            return typeDefinition == typeof(OutgoingMessage<,>);
        }

        return false;
    }
}