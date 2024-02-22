using Sphagnum.Common.Contracts.Messaging.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sphagnum.Common.Contracts.Messaging
{
    public interface IMessagingClient
    {
        public ValueTask<IncommingMessage> Consume(CancellationToken cancellationToken);
        public ValueTask<Guid> Publish(OutgoingMessage message);
        public ValueTask Ack(Guid messageId);
        public ValueTask Nack(Guid messageId);
        public ValueTask Reject(Guid messageId);
    }
}
