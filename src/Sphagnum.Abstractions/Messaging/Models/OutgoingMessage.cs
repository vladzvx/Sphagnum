using Sphagnum.Abstractions.Administration;
using System;

namespace Sphagnum.Abstractions.Messaging.Models
{
    public readonly struct OutgoingMessage
    {
        public readonly string Exchange;

        public readonly RoutingKey RoutingKey;

        public readonly ReadOnlyMemory<byte> Payload;

        public OutgoingMessage(string exchange, RoutingKey routingKey, ReadOnlyMemory<byte> payload)
        {
            this.Exchange = exchange;
            this.RoutingKey = routingKey;
            this.Payload = payload;
        }
    }
}
