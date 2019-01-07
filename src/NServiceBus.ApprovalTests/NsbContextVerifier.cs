using Newtonsoft.Json;
using NServiceBus.ObjectBuilder;
using NServiceBus.Testing;
using ObjectApproval;

namespace NServiceBus.ApprovalTests
{
    public static class TestContextVerifier
    {
        static TestContextVerifier()
        {
            SerializerBuilder.AddIgnore<TestableInvokeHandlerContext>(x => x.MessageHandler);
            SerializerBuilder.AddIgnore<TestableInvokeHandlerContext>(x => x.MessageBeingHandled);
            SerializerBuilder.AddIgnore<TestableInvokeHandlerContext>(x => x.MessageMetadata);
            SerializerBuilder.AddIgnore<TestableInvokeHandlerContext>(x => x.HandleCurrentMessageLaterWasCalled);
            SerializerBuilder.AddIgnore<TestableOutgoingLogicalMessageContext>(x => x.RoutingStrategies);
            SerializerBuilder.AddIgnore<TestableOutgoingPhysicalMessageContext>(x => x.RoutingStrategies);
            SerializerBuilder.AddIgnore<TestableRoutingContext>(x => x.RoutingStrategies);
            SerializerBuilder.AddIgnore<IBuilder>();
        }

        static JsonSerializerSettings BuildSerializer()
        {
            var settings = SerializerBuilder.BuildSettings();
            var converters = settings.Converters;
            converters.Add(new ContextBagConverter());
            converters.Add(new ExtendableOptionsConverter());
            return settings;
        }

        public static void Verify(TestableAuditContext context, object state = null)
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

        public static void Verify(TestableBatchDispatchContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableBehaviorContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableDispatchContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableEndpointInstance context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableForwardingContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableIncomingLogicalMessageContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableIncomingPhysicalMessageContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableInvokeHandlerContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableMessageHandlerContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableMessageProcessingContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableMessageSession context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableOutgoingContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableOutgoingLogicalMessageContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableOutgoingPhysicalMessageContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableOutgoingPublishContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableOutgoingReplyContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableOutgoingSendContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestablePipelineContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableRoutingContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableSubscribeContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableTransportReceiveContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        public static void Verify(TestableUnsubscribeContext context, object state = null)
        {
            InnerVerify(context, state);
        }

        //public static void Verify(TestingLoggerFactory context, object state = null)
        //{
        //    var wrapper = new ContextWrapper {Context = context, State = state};
        //    ObjectApprover.VerifyWithJson(wrapper,
        //        jsonSerializerSettings: jsonSerializerSettings);
        //}
    }
}