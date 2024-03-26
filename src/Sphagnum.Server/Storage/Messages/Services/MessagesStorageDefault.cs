using Sphagnum.Server.Storage.Messages.Contracts;

namespace Sphagnum.Server.Storage.Messages.Services
{
    internal class MessagesStorageDefault : IMessagesStorage
    {
        public ValueTask LogMessage(ReadOnlyMemory<byte> message)
        {
            return ValueTask.CompletedTask;
        }
    }
}
