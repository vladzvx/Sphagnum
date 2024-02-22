using Sphagnum.Common;
using Sphagnum.Common.Contracts.Infrastructure;
using Sphagnum.Common.Contracts.Login;
using Sphagnum.Common.Contracts.Messaging;
using Sphagnum.Common.Contracts.Messaging.Messages;
using Sphagnum.Common.Services;
using Sphagnum.Common.Utils;
using Sphagnum.Common.Utils.Models;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Sphagnum.Client
{
    public class ClientDefault : IMessagingClient, IDisposable
    {
        private readonly IConnection _connection;
        private readonly ConnectionFactory _connectionFactory;
        private readonly Channel<byte[]> _commonMessagesChannel = Channel.CreateUnbounded<byte[]>(); 
        private readonly Channel<byte[]> _systemMessagesChannel = Channel.CreateUnbounded<byte[]>(); 
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        public ClientDefault(ConnectionFactory factory)
        {
            _connectionFactory = factory;
            _connection = factory.CreateDefaultConnected().Result;
            Auth().Wait();
            MessagesRecievingWorker();
        }

        private async Task Auth()
        {
            await _connection.SendAsync(MessageParser.PackMessage(new AuthMessage(_connectionFactory.Login, _connectionFactory.Password, _connectionFactory.UserRights)));
            var response = await _connection.ReceiveAsync();
            var messageType = MessageParser.GetMessageType(response);
            if (messageType == Common.Utils.Models.MessageType.AuthSuccessfull)
            {
                return;
            }
            throw new Exception("Auth failed!");
        }

        public ValueTask Ack(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public ValueTask Nack(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Guid> Publish(OutgoingMessage message)
        {
            var bytes = MessageParser.PackMessage(message);
            await _connection.SendAsync(bytes);
            return MessageParser.GetMessageId(bytes);
        }

        public async ValueTask<IncommingMessage> Consume(CancellationToken cancellationToken)
        {
            var result = await _commonMessagesChannel.Reader.ReadAsync(cancellationToken);
            return MessageParser.UnpackIncomingMessage(result);
        }

        public ValueTask Reject(Guid messageId)
        {
            throw new NotImplementedException();
        }

        private async Task MessagesRecievingWorker()
        {
            while (_connection.Connected && !_cts.IsCancellationRequested)
            {
                var data = await _connection.ReceiveAsync(_cts.Token);
                var messageType = MessageParser.GetMessageType(data);
                if (messageType == Common.Utils.Models.MessageType.Common)
                {
                    await _commonMessagesChannel.Writer.WriteAsync(data, _cts.Token);
                }
                else
                {
                    await _systemMessagesChannel.Writer.WriteAsync(data,_cts.Token);
                }
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}
