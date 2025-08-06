using Sphagnum.Common.Infrastructure.Contracts;
using Sphagnum.Common.Infrastructure.Services;
using Sphagnum.Common.Messaging.Contracts;
using Sphagnum.Common.Messaging.Contracts.Messages;
using Sphagnum.Common.Messaging.Extensions;
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

        //private async Task Auth()
        //{
        //    await _connection.SendAsync(MessageParser.PackMessage(new AuthMessage(_connectionFactory.Login, _connectionFactory.Password, _connectionFactory.UserRights)));
        //    var response = await ReceiveAsync();
        //    var messageType = MessageParser.GetMessageType(response);
        //    if (messageType == MessageType.AuthSuccessfull)
        //    {
        //        return;
        //    }
        //    throw new Exception("Auth failed!");
        //}

        public ValueTask Ack(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public ValueTask Nack(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Guid> Publish(Message message)
        {
            var bytes = MessageParserold.PackMessage(message);
            await _connection.SendAsync(bytes.AsMemory(), System.Net.Sockets.SocketFlags.None);
            return MessageParserold.GetMessageId(bytes);
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
