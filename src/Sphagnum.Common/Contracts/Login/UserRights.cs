using System;

namespace Sphagnum.Common.Contracts.Login
{
    [Flags]
    public enum UserRights : ushort
    {
        None = 0,
        MessagesConsuming = 1,
        MessagesPublishing = 2,
        TopicCreating = 4,
        TopicDeleting = 8,
        TopicBinding = 16,
        ExchangeCreating = 32,
        ExchangeDeleting = 64,

        All = MessagesConsuming | MessagesPublishing | TopicCreating | TopicDeleting | TopicBinding | ExchangeCreating | ExchangeDeleting,
    }
}
