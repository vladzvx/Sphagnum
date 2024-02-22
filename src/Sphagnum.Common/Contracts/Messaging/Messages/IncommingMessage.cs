using System;

namespace Sphagnum.Common.Contracts.Messaging.Messages
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
