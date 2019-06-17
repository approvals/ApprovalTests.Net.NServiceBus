using Newtonsoft.Json;
using NServiceBus.Extensibility;
using NServiceBus.ObjectBuilder;
using NServiceBus.Testing;
using ObjectApproval;

namespace NServiceBus.ApprovalTests
{
    public static class TestContextVerifier
    {
        static TestContextVerifier()
        {
            SerializerBuilder.IgnoreMember<TestableInvokeHandlerContext>(x => x.MessageHandler);
            SerializerBuilder.IgnoreMember<TestableInvokeHandlerContext>(x => x.MessageBeingHandled);
            SerializerBuilder.IgnoreMember<TestableInvokeHandlerContext>(x => x.MessageMetadata);
            SerializerBuilder.IgnoreMember<IMessageProcessingContext>(x => x.ReplyToAddress);
            SerializerBuilder.IgnoreMember(typeof(IEndpointInstance), "EndpointStopped");
            SerializerBuilder.IgnoreMember<TestableOutgoingLogicalMessageContext>(x => x.RoutingStrategies);
            SerializerBuilder.IgnoreMember<TestableOutgoingPhysicalMessageContext>(x => x.RoutingStrategies);
            SerializerBuilder.IgnoreMember<TestableRoutingContext>(x => x.RoutingStrategies);
            SerializerBuilder.IgnoreInstance<ContextBag>(x => !ContextBagHelper.HasContent(x));
            SerializerBuilder.IgnoreMembersWithType<IBuilder>();
        }

        static JsonSerializerSettings BuildSerializer()
        {
            var settings = SerializerBuilder.BuildSettings();
            var converters = settings.Converters;
            converters.Add(new ContextBagConverter());
            converters.Add(new SendOptionsConverter());
            converters.Add(new ExtendableOptionsConverter());
            converters.Add(new UnsubscriptionConverter());
            converters.Add(new TimeoutMessageConverter());
            converters.Add(new SubscriptionConverter());
            converters.Add(new OutgoingMessageConverter());
            return settings;
        }

        public static void Verify(this TestableAuditContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        static void InnerVerify(object context, object state)
        {
            Guard.AgainstNull(context, nameof(context));
            if (state == null)
            {
                ObjectApprover.VerifyWithJson(context, jsonSerializerSettings: BuildSerializer());
                return;
            }

            var wrapper = new ContextWrapper
            {
                NsbTestContext = context,
                ExtraState = state
            };
            ObjectApprover.VerifyWithJson(wrapper, jsonSerializerSettings: BuildSerializer());
        }

        public static void Verify(this TestableBatchDispatchContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableBehaviorContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableDispatchContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableEndpointInstance context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableForwardingContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableIncomingLogicalMessageContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableIncomingPhysicalMessageContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableInvokeHandlerContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableMessageHandlerContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableMessageProcessingContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableMessageSession context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableOutgoingContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableOutgoingLogicalMessageContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableOutgoingPhysicalMessageContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableOutgoingPublishContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableOutgoingReplyContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableOutgoingSendContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestablePipelineContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableRoutingContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableSubscribeContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableTransportReceiveContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestableUnsubscribeContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(this TestingLoggerFactory context, object state = null)
        {
            InnerVerify(context, state);
        }
    }
}