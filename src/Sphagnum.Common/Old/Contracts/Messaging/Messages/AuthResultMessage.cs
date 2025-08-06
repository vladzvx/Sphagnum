using System;

namespace Sphagnum.Common.Old.Contracts.Messaging.Messages
{
    internal readonly ref struct AuthResultMessage
    {
        public readonly ReadOnlyMemory<byte> Payload;
    }
}
