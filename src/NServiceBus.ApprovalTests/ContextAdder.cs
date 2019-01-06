using System.Collections.Generic;
using System.Linq;
using NServiceBus.Testing;

static class ContextAdder
{
    public static void AddContext(TestableMessageSession context, Dictionary<string, object> target)
    {
        AddContext((TestablePipelineContext) context, target);
        Add(target, "Subscriptions", context.Subscriptions);
        Add(target, "Unsubscriptions", context.Unsubscription);
    }

    public static void AddContext(TestableEndpointInstance context, Dictionary<string, object> target)
    {
        AddContext((TestableMessageSession) context, target);
        target.Add("EndpointStopped", context.EndpointStopped);
    }

    public static void AddContext(TestablePipelineContext context, Dictionary<string, object> target)
    {
        Add(target, "PublishedMessages", context.PublishedMessages);
        Add(target, "SentMessages", context.SentMessages);
        Add(target, "TimeoutMessages", context.TimeoutMessages);
    }

    static void Add<T>(Dictionary<string, object> target, string key, T[] items)
    {
        if (items.Any())
        {
            target.Add(key, items);
        }
    }

    public static void AddContext(TestableBatchDispatchContext context, Dictionary<string, object> target)
    {
        AddContext((TestableBehaviorContext) context, target);
        if (context.Operations.Any())
        {
            target.Add("Operations", context.Operations);
        }
    }
    public static void AddContext(TestableAuditContext context, Dictionary<string, object> target)
    {
        AddContext((TestableBehaviorContext) context, target);
        target.Add("AuditAddress", context.AuditAddress);
        target.Add("AddedAuditData", context.AddedAuditData);
        target.Add("Message", context.Message);
    }
    public static void AddContext(TestableDispatchContext context, Dictionary<string, object> target)
    {
        AddContext((TestableBehaviorContext) context, target);
        if (context.Operations.Any())
        {
            target.Add("Operations", context.Operations);
        }
    }
    public static void AddContext(TestableForwardingContext context, Dictionary<string, object> target)
    {
        AddContext((TestableBehaviorContext) context, target);
        target.Add("Address", context.Address);
        target.Add("Message", context.Message);
    }

    public static void AddContext(TestableBehaviorContext context, Dictionary<string, object> target)
    {
    }
    public static void AddContext(TestableInvokeHandlerContext context, Dictionary<string, object> target)
    {
        AddContext((TestableIncomingContext) context, target);
        if (context.Headers.Any())
        {
            target.Add("Headers", context.Headers);
        }

        target.Add("Message", context.MessageBeingHandled);
        target.Add("MessageMetadata", context.MessageMetadata);
    }

    public static void AddContext(TestableIncomingContext context, Dictionary<string, object> target)
    {
        AddContext((TestableMessageProcessingContext) context, target);
    }

    public static void AddContext(TestableMessageProcessingContext context, Dictionary<string, object> target)
    {
        AddContext((TestablePipelineContext) context, target);
        if (context.MessageHeaders.Any())
        {
            target.Add("MessageHeaders", context.MessageHeaders);
        }
        if (context.RepliedMessages.Any())
        {
            target.Add("RepliedMessages", context.RepliedMessages);
        }
    }
}