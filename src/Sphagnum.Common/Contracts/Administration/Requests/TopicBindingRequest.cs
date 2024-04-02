using Sphagnum.Common.Contracts.Messaging;

namespace Sphagnum.Common.Contracts.Administration.Requests
{
    public readonly ref struct TopicBindingRequest
    {
        public readonly string TopicName;
        public readonly string ExchangeName;
        public readonly RoutingKey RoutingKey;
    }
}
