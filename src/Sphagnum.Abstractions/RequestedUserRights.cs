using System;

namespace Sphagnum.Abstractions
{
    [Flags]
    public enum RequestedUserRights
    {
        MessagesConsuming = 1,
        MessagesPublishing = 2,
        TopicCreating = 4,
        TopicDeleting = 8,
        TopicBinding = 16,
        ExchangeCreating = 32,
        ExchangeDeleting = 64,
    }
}
