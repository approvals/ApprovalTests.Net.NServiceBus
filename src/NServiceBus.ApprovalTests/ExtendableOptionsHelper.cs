using System.Linq;
using NServiceBus;
using NServiceBus.Extensibility;

static class ExtendableOptionsHelper
{
    public static bool HasValue(this ExtendableOptions options)
    {
        var messageId = options.GetMessageId();
        if (messageId != null)
        {
            return true;
        }

        if (options is SendOptions sendOptions)
        {
            if (sendOptions.GetDeliveryDate().HasValue || sendOptions.GetDeliveryDelay().HasValue)
            {
                return true;
            }
        }
        var headers = options.GetHeaders();
        if (headers.Any())
        {
            return true;
        }

        var extensions = options.GetExtensions();
        if (extensions != null)
        {
            return ContextBagHelper.HasContent(extensions);
        }

        return false;
    }
}