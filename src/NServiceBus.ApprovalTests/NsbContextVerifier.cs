using System.Collections.Generic;
using Newtonsoft.Json;
using NServiceBus.Testing;
using ObjectApproval;

namespace NServiceBus.ApprovalTests
{
    public static class NsbContextVerifier
    {
        static JsonSerializerSettings jsonSerializerSettings;

        static NsbContextVerifier()
        {
            SerializerBuilder.AddIgnore<TestableInvokeHandlerContext>(x=> x.MessageHandler);
            SerializerBuilder.AddIgnore<TestableInvokeHandlerContext>(x=> x.MessageBeingHandled);
            SerializerBuilder.AddIgnore<TestableInvokeHandlerContext>(x=> x.MessageMetadata);
            jsonSerializerSettings = SerializerBuilder.BuildSettings();
            var converters = jsonSerializerSettings.Converters;
            converters.Add(new ExtendableOptionsConverter());
          //  converters.Add(new MessageMetadataConverter());
          //  converters.Add(new TransportOperationConverter());
        }

        public static void Verify(TestableAuditContext context, object state = null)
        {
            var wrapper = new ContextWrapper {Context = context, State = state};
            ObjectApprover.VerifyWithJson(wrapper ,
                jsonSerializerSettings: jsonSerializerSettings);
        }

        public static void Verify(TestableBatchDispatchContext context, object state = null)
        {
            var target = BuildTarget(state);
            ContextAdder.AddContext(context, target);

            ObjectApprover.VerifyWithJson(target,
                jsonSerializerSettings: jsonSerializerSettings);
        }

        public static void Verify(TestableBehaviorContext context, object state = null)
        {
            var target = BuildTarget(state);
            ContextAdder.AddContext(context, target);

            ObjectApprover.VerifyWithJson(target,
                jsonSerializerSettings: jsonSerializerSettings);
        }

        public static void Verify(TestableDispatchContext context, object state = null)
        {
            var target = BuildTarget(state);
            ContextAdder.AddContext(context, target);

            ObjectApprover.VerifyWithJson(target,
                jsonSerializerSettings: jsonSerializerSettings);
        }

        public static void Verify(TestableEndpointInstance context, object state = null)
        {
            var target = BuildTarget(state);
            ContextAdder.AddContext(context, target);

            ObjectApprover.VerifyWithJson(target,
                jsonSerializerSettings: jsonSerializerSettings);
        }

        public static void Verify(TestableForwardingContext context, object state = null)
        {
            var target = BuildTarget(state);
            ContextAdder.AddContext(context, target);

            ObjectApprover.VerifyWithJson(target,
                jsonSerializerSettings: jsonSerializerSettings);
        }

        public static void Verify(TestableIncomingContext context, object state = null)
        {
            var target = BuildTarget(state);
            ContextAdder.AddContext(context, target);

            ObjectApprover.VerifyWithJson(target,
                jsonSerializerSettings: jsonSerializerSettings);
        }

        public static void Verify(TestableInvokeHandlerContext context, object state = null)
        {
            var wrapper = new ContextWrapper {Context = context, State = state};

            ObjectApprover.VerifyWithJson(wrapper,
                jsonSerializerSettings: jsonSerializerSettings);
        }

        public static void Verify(TestableMessageSession context, object state = null)
        {
            var target = BuildTarget(state);
            ContextAdder.AddContext(context, target);

            ObjectApprover.VerifyWithJson(target,
                jsonSerializerSettings: jsonSerializerSettings);
        }

        static Dictionary<string, object> BuildTarget(object state)
        {
            if (state == null)
            {
                return new Dictionary<string, object>();
            }

            return new Dictionary<string, object>
            {
                {"State", state}
            };
        }
    }

    class ContextWrapper
    {
        public object Context;
        public object State;
    }
}