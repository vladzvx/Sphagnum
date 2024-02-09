using Sphagnum.Abstractions.Messaging;
using Sphagnum.Abstractions.Messaging.Models;
using System;
using System.Threading.Tasks;

namespace Sphagnum.Client
{
    public class Connection : IMessagingConnection
    {
        public ValueTask Ack(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public ValueTask Nack(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Guid> Publish(OutgoingMessage message)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Guid> RegisterConsumer(ConsumerSettings recieverSettings)
        {
            throw new NotImplementedException();
        }

        public ValueTask Reject(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public ValueTask UnregisterConsumer(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
