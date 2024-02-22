using Sphagnum.Common.Contracts.Infrastructure;
using Sphagnum.Common.Contracts.Login;
using Sphagnum.Common.Utils.Models;
using Sphagnum.Common.Utils;
using Sphagnum.Server.Storage.Users.Contracts;
using Sphagnum.Server.Storage.Users.Services;
using Sphagnum.Server.Storage.Messages.Contracts;
using Sphagnum.Server.Cluster.Contracts;
using Sphagnum.Server.DataProcessing.Contracts;
using Sphagnum.Common;

namespace Sphagnum.Server.Broker.Services
{
    internal class BrokerDefaultBase(ConnectionFactory connectionFactory, IMessagesStorage messagesStorage, IDistributor distributor, IDataProcessor dataProcessor)
    {
        private readonly IConnection _connection = connectionFactory.CreateDefault();
        private readonly CancellationTokenSource _cts = new();
        private Task? _acceptationTask;

        private readonly IAuthInfoStorage _authInfoStorage = new AuthInfoStorageBase();
        private readonly IMessagesStorage _messagesStorage = messagesStorage;
        private readonly IDistributor _distributor = distributor;
        private readonly IDataProcessor _dataProcessor = dataProcessor;

        public Task StartAsync()
        {
            _connection.Bind(8081);
            _connection.Listen(1000);
            _acceptationTask = AcceptationWorker(_cts.Token);
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }

        private async Task AcceptationWorker(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var acceptedSocket = await _connection.AcceptAsync();
                RecievingTask(acceptedSocket, token);
            }
        }

        private async ValueTask<bool> CheckRights(byte[] buffer, CancellationToken token)
        {
            var messageType = MessageParser.GetMessageType(buffer);
            if (messageType == MessageType.Auth)
            {
                var payloadStart = MessageParser.GetPayloadStart(buffer);
                var rights = (UserRights)BitConverter.ToInt16(buffer.AsSpan(Constants.HashedUserDataSizeInfBytes + Constants.HashedUserDataSizeInfBytes + payloadStart, 2));
                var isRecievingAllowed = await _authInfoStorage.CheckRights(
                    buffer.AsSpan(payloadStart, Constants.HashedUserDataSizeInfBytes),
                    buffer.AsSpan(payloadStart + Constants.HashedUserDataSizeInfBytes, Constants.HashedUserDataSizeInfBytes),
                    rights,
                    token
                    );
                return isRecievingAllowed;
            }
            return false;
        }

        private async Task RecievingTask(IConnection connection, CancellationToken token)
        {
            var successRespBuffer = new byte[23];
            successRespBuffer[4] = (byte)MessageType.MessageAccepted;
            successRespBuffer[0] = (byte)23;

            var mess = await connection.ReceiveAsync(token);
            var isRecievingAllowed = await CheckRights(mess, token);
            if (isRecievingAllowed)
            {
                var authResp = MessageParser.PackReplyMessage(MessageType.AuthSuccessfull);
                MessageParser.CopyId(mess, authResp);
                await connection.SendAsync(authResp);
                while (connection.Connected)
                {
                    try
                    {
                        var message = await connection.ReceiveAsync(token);
                        await _messagesStorage.LogMessage(message);
                        await _distributor.DistributeData(message);
                        MessageParser.CopyId(message, successRespBuffer);
                        await connection.SendAsync(successRespBuffer, token);
                        await _dataProcessor.PutMessage(message);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            connection.Close();
            connection.Dispose();
        }
    }
}
