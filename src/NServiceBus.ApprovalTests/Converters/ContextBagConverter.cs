using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NServiceBus.Extensibility;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

class ContextBagConverter : JsonConverter
{
    static FieldInfo stashField;
    static FieldInfo parentBagField;

    static ContextBagConverter()
    {
        var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        stashField = typeof(ContextBag).GetField("stash", bindingFlags);
        parentBagField = typeof(ContextBag).GetField("parentBag", bindingFlags);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        var stash = (Dictionary<string, object>)stashField.GetValue(value);
        if (stash.Any())
        {
            writer.WritePropertyName("Stash");
            serializer.Serialize(writer, stash);
        }
        var parentBag = (ContextBag)parentBagField.GetValue(value);
        if (parentBag!= null)
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