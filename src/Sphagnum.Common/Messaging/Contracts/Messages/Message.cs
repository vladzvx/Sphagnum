using System;

namespace Sphagnum.Common.Messaging.Contracts.Messages
{
    public readonly struct Message
    {
        public readonly Guid MessageId;

        public readonly ReadOnlyMemory<byte> Payload;

        public readonly string Exchange;

        public readonly RoutingKey RoutingKey;

        public Message(string exchange, RoutingKey routingKey, ReadOnlyMemory<byte> payload)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
            Payload = payload;
            RoutingKey = routingKey;
            MessageId = Guid.NewGuid();
        }

        internal Message(Guid messageId, string exchange, RoutingKey routingKey, ReadOnlyMemory<byte> payload)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
            Payload = payload;
            MessageId = messageId;
        }
    }
}
