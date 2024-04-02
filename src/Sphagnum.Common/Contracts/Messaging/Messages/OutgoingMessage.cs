using System;

namespace Sphagnum.Common.Contracts.Messaging.Messages
{
    public readonly struct OutgoingMessage
    {
        public readonly string Exchange;

        public readonly RoutingKey RoutingKey;

        public readonly ReadOnlyMemory<byte> Payload;

        public OutgoingMessage(string exchange, RoutingKey routingKey, ReadOnlyMemory<byte> payload)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
            Payload = payload;
        }

        public OutgoingMessage(string exchange, ReadOnlyMemory<byte> payload)
        {
            Exchange = exchange;
            RoutingKey = new RoutingKey();
            Payload = payload;
        }
    }
}
