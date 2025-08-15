using Sphagnum.Common.Infrastructure.Contracts;
using Sphagnum.Common.Infrastructure.Extensions;
using Sphagnum.Common.Infrastructure.Services;
using Sphagnum.Common.Messaging.Contracts;
using Sphagnum.Common.Messaging.Contracts.Messages;
using Sphagnum.Common.Messaging.Utils;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Sphagnum.Client
{
    public sealed class ClientDefault : IDisposable
    {
        private readonly Task _recievingTask;
        private readonly IConnection _connection;
        private readonly Channel<byte[]> _commonMessagesChannel = Channel.CreateUnbounded<byte[]>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        public ClientDefault(ConnectionFactory factory)
        {
            Task.Delay(10000).Wait();
            _connection = factory.CreateConnection().Result;
            _recievingTask = RecivingTask();
        }

        private async Task RecivingTask()
        {
            while (!_cts.IsCancellationRequested)
            {
                var data = await _connection.ReceiveAsync(_cts.Token);
                if (MessageParser.GetMessageType(data) == MessageType.Common)
                {
                    await _commonMessagesChannel.Writer.WriteAsync(data);
                }
            }
        }

        public async ValueTask Ack(Guid messageId)
        {
            await Task.Delay(1000);
        }

        public ValueTask Nack(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Guid> Publish(Message message)
        {
            var bytes = MessageParser.PackMessage(message);
            await _connection.SendAsync(bytes.AsMemory(), System.Net.Sockets.SocketFlags.None);
            return MessageParser.GetMessageId(bytes);
        }

        public async ValueTask<Message> Consume(CancellationToken cancellationToken)
        {
            var result = await _commonMessagesChannel.Reader.ReadAsync(cancellationToken);
            return MessageParser.UnpackMessage(result);
        }

        public ValueTask Reject(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}
