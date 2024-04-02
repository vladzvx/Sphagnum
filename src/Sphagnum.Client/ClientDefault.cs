using Sphagnum.Common.Contracts.Login;
using Sphagnum.Common.Contracts.Messaging;
using Sphagnum.Common.Contracts.Messaging.Messages;
using Sphagnum.Common.Services;
using Sphagnum.Common.Utils;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Sphagnum.Client
{
    public class ClientDefault : IMessagingClient, IDisposable
    {
        private readonly SphagnumConnection _connection;
        private readonly ConnectionFactory _connectionFactory;
        private readonly Channel<byte[]> _commonMessagesChannel = Channel.CreateUnbounded<byte[]>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        public ClientDefault(ConnectionFactory factory)
        {
            _connectionFactory = factory;
            _connection = factory.CreateDefaultConnected(() => async (mess) =>
            {
                await _commonMessagesChannel.Writer.WriteAsync(mess);
            }).Result;
            Auth().Wait();
        }

        private async Task<byte[]> ReceiveAsync()
        {
            return await _commonMessagesChannel.Reader.ReadAsync(_cts.Token);
        }

        private async Task Auth()
        {
            await _connection.SendAsync(MessageParser.PackMessage(new AuthMessage(_connectionFactory.Login, _connectionFactory.Password, _connectionFactory.UserRights)));
            var response = await ReceiveAsync();
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

        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}
