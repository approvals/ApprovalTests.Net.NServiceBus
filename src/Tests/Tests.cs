using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.ApprovalTests;
using NServiceBus.DelayedDelivery;
using NServiceBus.DeliveryConstraints;
using NServiceBus.Pipeline;
using NServiceBus.Routing;
using NServiceBus.Testing;
using NServiceBus.Transport;
using NServiceBus.Unicast.Messages;
using Xunit;
using Xunit.Abstractions;

public class Tests :
    XunitLoggingBase
{
    [Fact]
    public void ExtraState()
    {
        var context = new TestableAuditContext();
        context.AddedAuditData.Add("Key", "Value");
        context.Verify(new {Property="Value"});
    }

    [Fact]
    public void AuditContext()
    {
        var context = new TestableAuditContext();
        context.AddedAuditData.Add("Key", "Value");
        context.Verify();
    }

    [Fact]
    public void BatchDispatchContext()
    {
        var context = new TestableBatchDispatchContext();
        context.Operations.Add(BuildTransportOperation());
        context.Verify();
    }


    [Fact]
    public void BehaviorContext()
    {
        var context = new TestableBehaviorContextImp();
        context.Extensions.AddDeliveryConstraint(new DelayDeliveryWith(TimeSpan.FromDays(1)));
        context.Verify();
    }

    public class TestableBehaviorContextImp : TestableBehaviorContext
    {
    }

    [Fact]
    public void DispatchContext()
    {
        var context = new TestableDispatchContext();
        context.Operations.Add(BuildTransportOperation());
        context.Verify();
    }

    [Fact]
    public async Task EndpointInstance()
    {
        var context = new TestableEndpointInstance();
        await context.Stop();
        context.Verify();
    }

    [Fact]
    public void ForwardingContext()
    {
        var context = new TestableForwardingContext
        {
            Address = "The address",
            Message = BuildOutgoingMessage()
        };
        context.Verify();
    }

    [Fact]
    public void IncomingLogicalMessageContext()
    {
        var context = new TestableIncomingLogicalMessageContext
        {
            Message = BuildLogicalMessage(),
            Headers = new Dictionary<string, string> {{"Key", "Value"}}
        };
        context.Verify();
    }

    [Fact]
    public void IncomingPhysicalMessageContext()
    {
        var context = new TestableIncomingPhysicalMessageContext
        {
            Message = BuildIncomingMessage(),
        };
        context.Verify();
    }

    [Fact]
    public void InvokeHandlerContext()
    {
        var context = new TestableInvokeHandlerContext
        {
            Headers = new Dictionary<string, string> {{"Key", "Value"}},
        };
        context.Verify();
    }

    [Fact]
    public void MessageHandlerContext()
    {
        var context = new TestableMessageHandlerContext();
        context.Verify();
    }

    [Fact]
    public async Task MessageProcessingContext()
    {
        var context = new TestableMessageProcessingContext();
        context.MessageHeaders.Add("Key", "Value");
        await context.Publish(new PublishMessage {Property = "Value"});
        await context.Reply(new ReplyMessage {Property = "Value"});
        await context.Send(new SendMessage {Property = "Value"});
        await context.ForwardCurrentMessageTo("newDestination");
        context.Verify();
    }

    [Fact]
    public async Task MessageSession()
    {
        var context = new TestableMessageSession();
        var subscribeOptions = new SubscribeOptions();
        subscribeOptions.RequireImmediateDispatch();
        await context.Subscribe(typeof(MyMessage), subscribeOptions);
        var unsubscribeOptions = new UnsubscribeOptions();
        unsubscribeOptions.RequireImmediateDispatch();
        await context.Unsubscribe(typeof(MyMessage), unsubscribeOptions);
        context.Verify();
    }

    [Fact]
    public void OutgoingContext()
    {
        var context = new TestableOutgoingContext();
        context.Headers.Add("Key", "Value");
        context.Verify();
    }

    [Fact]
    public void OutgoingLogicalMessageContext()
    {
        var context = new TestableOutgoingLogicalMessageContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        context.Verify();
    }

    [Fact]
    public void OutgoingPhysicalMessageContext()
    {
        var context = new TestableOutgoingPhysicalMessageContext
        {
            Body = new byte[] {1}
        };
        context.Verify();
    }

    [Fact]
    public void OutgoingPublishContext()
    {
        var context = new TestableOutgoingPublishContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        context.Verify();
    }

    [Fact]
    public void OutgoingReplyContext()
    {
        var context = new TestableOutgoingReplyContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        context.Verify();
    }

    [Fact]
    public void OutgoingSendContext()
    {
        var context = new TestableOutgoingSendContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        context.Verify();
    }

    [Fact]
    public async Task PipelineContext()
    {
        var context = new TestablePipelineContext();
        await context.Publish(new PublishMessage { Property = "Value" });
        await context.Send(new SendMessage { Property = "Value" });
        var options = new SendOptions();
        options.DelayDeliveryWith(TimeSpan.FromDays(1));
        await context.Send(new SendMessage {Property = "ValueWithDelay"},options);
        context.Verify();
    }

    [Fact]
    public void RoutingContext()
    {
        var context = new TestableRoutingContext
        {
            Message = BuildOutgoingMessage()
        };
        context.Verify();
    }

    [Fact]
    public void SubscribeContext()
    {
        var context = new TestableSubscribeContext
        {
            EventType = typeof(MyMessage)
        };
        context.Verify();
    }

    [Fact]
    public void TransportReceiveContext()
    {
        var context = new TestableTransportReceiveContext
        {
            Message = BuildIncomingMessage(), ReceiveOperationAborted = true
        };
        context.Verify();
    }

    [Fact]
    public void UnsubscribeContext()
    {
        var context = new TestableUnsubscribeContext
        {
            EventType = typeof(MyMessage)
        };
        context.Verify();
    }

    static TransportOperation BuildTransportOperation()
    {
        var outgoingMessage = BuildOutgoingMessage();
        return new TransportOperation(outgoingMessage,
            new UnicastAddressTag("destination"),
            DispatchConsistency.Isolated,
            new List<DeliveryConstraint> {new DelayDeliveryWith(TimeSpan.FromDays(1))});
    }

    static OutgoingMessage BuildOutgoingMessage()
    {
        return new OutgoingMessage("MessageId", new Dictionary<string, string> {{"key", "value"}}, new byte[] {1});
    }

    static OutgoingLogicalMessage BuildOutgoingLogicalMessage()
    {
        return new OutgoingLogicalMessage(typeof(MyMessage), new MyMessage {Property = "Value"});
    }

    static IncomingMessage BuildIncomingMessage()
    {
        return new IncomingMessage("MessageId", new Dictionary<string, string> {{"key", "value"}}, new byte[] {1});
    }

    static LogicalMessage BuildLogicalMessage()
    {
        return new LogicalMessage(new MessageMetadata(typeof(MyMessage)), new MyMessage {Property = "Value"});
    }

    public Tests(ITestOutputHelper output) : 
        base(output)
    {
    }
}

public class MyMessage
{
    public string Property { get; set; }
}

public class PublishMessage
{
    public string Property { get; set; }
}

public class ReplyMessage
{
    public string Property { get; set; }
}

public class SendMessage
{
    public string Property { get; set; }
}