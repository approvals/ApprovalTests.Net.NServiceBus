using System.Threading.Tasks;
using NServiceBus.ApprovalTests;
using NServiceBus.Testing;
using Xunit;

public class MessageHandlerTests
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
}