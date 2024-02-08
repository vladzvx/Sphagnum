using Sphagnum.Abstractions.Administration;
using Sphagnum.Abstractions.Messaging.Models;
using System;
using System.Collections.Generic;

namespace Sphagnum.Common.Models
{
    internal readonly ref struct Message
    {
        public readonly string Exchange;

        public readonly DateTime? ExpirationTime;

        public readonly RoutingKey RoutingKey;

        public readonly IReadOnlyCollection<byte> Payload;

        public readonly Guid MessageId;
    }
}
