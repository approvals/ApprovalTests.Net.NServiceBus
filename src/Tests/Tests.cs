using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.ApprovalTests;
using NServiceBus.Testing;
using Xunit;

public class Tests
{
    [Fact]
    public void AssertAllContextHandled()
    {
        var handler = new Handler();
        var context = new TestableMessageHandlerContext();

        await handler.Handle(new MyMessage{Property = "Value"}, context);
        NsbContextVerifier.Verify(context);
    }
    [Fact]
    public async Task MessageHandlerContext()
    {
        var handler = new Handler();
        var context = new TestableMessageHandlerContext();

        await handler.Handle(new MyMessage{Property = "Value"}, context);
        NsbContextVerifier.Verify(context);
    }
}

public class Handler :
    IHandleMessages<MyMessage>
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await context.Publish(new PublishMessage {Property = "Value"});
        await context.Reply(new ReplyMessage {Property = "Value"});
        await context.Send(new SendMessage {Property = "Value"});
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