using System.Threading.Tasks;
using NServiceBus.ApprovalTests;
using NServiceBus.Testing;
using Xunit;
using Xunit.Abstractions;

public class MessageHandlerTests:
    XunitApprovalBase
{
    #region HandlerTest
    [Fact]
    public async Task VerifyHandlerResult()
    {
        var handler = new MyHandler();
        var context = new TestableMessageHandlerContext();

        await handler.Handle(new MyRequest(), context)
            .ConfigureAwait(false);

        context.Verify();
    }
    #endregion

    public MessageHandlerTests(ITestOutputHelper output) :
        base(output)
    {
    }
}