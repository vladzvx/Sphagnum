using Sphagnum.Abstractions.Messaging.Models;
using System;
using System.Threading.Tasks;

namespace Sphagnum.Abstractions.Messaging
{
    public interface IMessagingConnection
    {
        public ValueTask<Guid> RegisterConsumer(ConsumerSettings recieverSettings);
        public ValueTask UnregisterConsumer(Guid guid);
        public ValueTask<Guid> Publish(OutgoingMessage message);
        public ValueTask Ack(Guid messageId);
        public ValueTask Nack(Guid messageId);
        public ValueTask Reject(Guid messageId);
    }
}
