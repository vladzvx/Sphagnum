using Sphagnum.Common.Messaging.Contracts;
using Sphagnum.Common.Messaging.Utils;
using Sphagnum.Common.Old.Contracts;
using Sphagnum.Common.Old.Contracts.Login;
using Sphagnum.Common.Old.Exceptions;
using Sphagnum.Server.Cluster.Contracts;
using Sphagnum.Server.DataProcessing.Contracts;
using Sphagnum.Server.Storage.Messages.Contracts;
using Sphagnum.Server.Storage.Users.Contracts;

namespace Sphagnum.Server.Broker.Services
{
    internal class MessagesProcessor
    {
        private bool AuthOk = true;

        private readonly IAuthInfoStorage _authInfoStorage;
        private readonly IMessagesStorage _messagesStorage;
        private readonly IDistributor _distributor;
        private readonly IDataProcessor _dataProcessor;
        public MessagesProcessor(IAuthInfoStorage authInfoStorage, IMessagesStorage messagesStorage, IDistributor distributor, IDataProcessor dataProcessor)
        {
            _authInfoStorage = authInfoStorage;
            _messagesStorage = messagesStorage;
            _distributor = distributor;
            _dataProcessor = dataProcessor;
        }

        internal async Task ProcessMessage(byte[] message)
        {
            if (AuthOk)
            {
                await _messagesStorage.LogMessage(message);
                await _distributor.DistributeData(message);
                await _dataProcessor.PutMessage(message);
            }
            else if (await CheckRights(message))
            {
                AuthOk = true;
                await _messagesStorage.LogMessage(message);
                await _distributor.DistributeData(message);
                await _dataProcessor.PutMessage(message);
            }
            else
            {
                throw new AuthException();
            }
        }

        private async ValueTask<bool> CheckRights(byte[] buffer)
        {
            var messageType = MessageParserold.GetMessageType(buffer);
            if (messageType == MessageType.Auth)
            {
                var payloadStart = MessageParserold.GetPayloadStart(buffer);
                var rights = (UserRights)BitConverter.ToInt16(buffer.AsSpan(Constants.HashedUserDataSizeInfBytes + Constants.HashedUserDataSizeInfBytes + payloadStart, 2));
                var isRecievingAllowed = await _authInfoStorage.CheckRights(
                    buffer.AsSpan(payloadStart, Constants.HashedUserDataSizeInfBytes),
                    buffer.AsSpan(payloadStart + Constants.HashedUserDataSizeInfBytes, Constants.HashedUserDataSizeInfBytes),
                    rights,
                    new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token
                    );
                return isRecievingAllowed;
            }
            return false;
        }
    }
}
