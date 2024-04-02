using Sphagnum.Common.Contracts.Login;
using Sphagnum.Common.Services;
using Sphagnum.Server.Cluster.Contracts;
using Sphagnum.Server.Cluster.Services;
using Sphagnum.Server.DataProcessing.Contracts;
using Sphagnum.Server.DataProcessing.Services;
using Sphagnum.Server.Storage.Messages.Contracts;
using Sphagnum.Server.Storage.Messages.Services;
using Sphagnum.Server.Storage.Users.Contracts;
using Sphagnum.Server.Storage.Users.Services;

namespace Sphagnum.Server.Broker.Services
{
    internal class BrokerDefaultBase(ConnectionFactory connectionFactory, IMessagesStorage messagesStorage, IDistributor distributor, IDataProcessor dataProcessor)
    {
        private readonly SphagnumConnection _connection;
        private readonly CancellationTokenSource _cts = new();
        private Task? _acceptationTask;

        private readonly IAuthInfoStorage _authInfoStorage = new AuthInfoStorageBase();
        private readonly IMessagesStorage _messagesStorage = messagesStorage;
        private readonly IDistributor _distributor = distributor;
        private readonly IDataProcessor _dataProcessor = dataProcessor;
        private readonly ConnectionFactory _connectionFactory = connectionFactory;

        public Task StartAsync(int port)
        {
            _connectionFactory.CreateDefault(() =>
            {
                var processor = new MessagesProcessor(_authInfoStorage, _messagesStorage, _distributor, _dataProcessor);
                return processor.ProcessMessage;
            });
            _connection.Bind(port);
            _connection.Listen(1000); //todo разобраться что делает этот параметр.
            _acceptationTask = AcceptationWorker(_cts.Token);
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }

        internal static BrokerDefaultBase Create(ConnectionFactory connectionFactory)
        {
            return new BrokerDefaultBase(
                connectionFactory,
                new MessagesStorageDefault(),
                new DistributorDefault(),
                new DataProcessorDefault()
                );
        }


        private async Task AcceptationWorker(CancellationToken token)
        {

            while (!token.IsCancellationRequested)
            {
                var acceptedSocket = await _connection.AcceptAsync();
            }
        }
    }
}
