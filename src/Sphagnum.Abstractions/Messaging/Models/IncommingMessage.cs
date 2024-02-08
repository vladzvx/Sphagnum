using System;
using System.Collections.Generic;

namespace Sphagnum.Abstractions.Messaging.Models
{
    public readonly struct IncommingMessage
    {
        public readonly Guid MessageId;

        public readonly ReadOnlyMemory<byte> Payload;

        public IncommingMessage(Guid messageId, ReadOnlyMemory<byte> payload)
        {
            MessageId = messageId;
            Payload = payload;
        }
    }
}
