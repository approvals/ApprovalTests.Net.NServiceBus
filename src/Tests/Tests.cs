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

public class Tests
{
    [Fact]
    public void ExtraState()
    {
        var context = new TestableAuditContext();
        context.AddedAuditData.Add("Key", "Value");
        TestContextVerifier.Verify(context, new {Property="Value"});
    }

    [Fact]
    public void AuditContext()
    {
        var context = new TestableAuditContext();
        context.AddedAuditData.Add("Key", "Value");
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void BatchDispatchContext()
    {
        var context = new TestableBatchDispatchContext();
        context.Operations.Add(BuildTransportOperation());
        TestContextVerifier.Verify(context);
    }


    [Fact]
    public void BehaviorContext()
    {
        var context = new TestableBehaviorContextImp();
        context.Extensions.AddDeliveryConstraint(new DelayDeliveryWith(TimeSpan.FromDays(1)));
        TestContextVerifier.Verify(context);
    }

    public class TestableBehaviorContextImp : TestableBehaviorContext
    {
    }

    [Fact]
    public void DispatchContext()
    {
        var context = new TestableDispatchContext();
        context.Operations.Add(BuildTransportOperation());
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public async Task EndpointInstance()
    {
        var context = new TestableEndpointInstance();
        await context.Stop();
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void ForwardingContext()
    {
        var context = new TestableForwardingContext
        {
            Address = "The address",
            Message = BuildOutgoingMessage()
        };
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void IncomingLogicalMessageContext()
    {
        var context = new TestableIncomingLogicalMessageContext
        {
            Message = BuildLogicalMessage(),
            Headers = new Dictionary<string, string> {{"Key", "Value"}}
        };
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void IncomingPhysicalMessageContext()
    {
        var context = new TestableIncomingPhysicalMessageContext
        {
            Message = BuildIncomingMessage(),
        };
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void InvokeHandlerContext()
    {
        var context = new TestableInvokeHandlerContext
        {
            Headers = new Dictionary<string, string> {{"Key", "Value"}},
        };
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void MessageHandlerContext()
    {
        var context = new TestableMessageHandlerContext();
        TestContextVerifier.Verify(context);
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
        TestContextVerifier.Verify(context);
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
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void OutgoingContext()
    {
        var context = new TestableOutgoingContext();
        context.Headers.Add("Key", "Value");
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void OutgoingLogicalMessageContext()
    {
        var context = new TestableOutgoingLogicalMessageContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void OutgoingPhysicalMessageContext()
    {
        var context = new TestableOutgoingPhysicalMessageContext
        {
            Body = new byte[] {1}
        };
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void OutgoingPublishContext()
    {
        var context = new TestableOutgoingPublishContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void OutgoingReplyContext()
    {
        var context = new TestableOutgoingReplyContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void OutgoingSendContext()
    {
        var context = new TestableOutgoingSendContext
        {
            Message = BuildOutgoingLogicalMessage()
        };
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public async Task PipelineContext()
    {
        var context = new TestablePipelineContext();
        await context.Publish(new PublishMessage {Property = "Value"});
        await context.Send(new SendMessage {Property = "Value"});
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void RoutingContext()
    {
        var context = new TestableRoutingContext
        {
            Message = BuildOutgoingMessage()
        };
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void SubscribeContext()
    {
        var context = new TestableSubscribeContext
        {
            EventType = typeof(MyMessage)
        };
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void TransportReceiveContext()
    {
        var context = new TestableTransportReceiveContext
        {
            Message = BuildIncomingMessage(), ReceiveOperationAborted = true
        };
        TestContextVerifier.Verify(context);
    }

    [Fact]
    public void UnsubscribeContext()
    {
        var context = new TestableUnsubscribeContext
        {
            EventType = typeof(MyMessage)
        };
        TestContextVerifier.Verify(context);
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