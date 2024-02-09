using Sphagnum.Abstractions.Administration;
using System;

namespace Sphagnum.Abstractions.Messaging.Models
{
    public readonly ref struct OutgoingMessage
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
