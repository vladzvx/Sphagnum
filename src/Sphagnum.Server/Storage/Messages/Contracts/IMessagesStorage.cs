namespace Sphagnum.Server.Storage.Messages.Contracts
{
    internal interface IMessagesStorage
    {
        ValueTask LogMessage(ReadOnlyMemory<byte> message);
    }
}
