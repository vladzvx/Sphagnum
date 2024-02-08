namespace Sphagnum.Abstractions.Administration.Requests
{
    public readonly struct TopicBindingRequest
    {
        public readonly string TopicName;
        public readonly string ExchangeName;
        public readonly RoutingKey RoutingKey;
    }
}
