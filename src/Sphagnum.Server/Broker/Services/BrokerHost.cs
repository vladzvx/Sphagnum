using Microsoft.Extensions.Hosting;
using Sphagnum.Common.Contracts.Login;
using Sphagnum.Common.Services;
using Sphagnum.Server.Broker.Services;
using Sphagnum.Server.Cluster.Services;
using Sphagnum.Server.DataProcessing.Services;
using Sphagnum.Server.Storage.Messages.Services;

namespace Sphagnum.Server
{
    public class BrokerHost : IHostedService
    {
        private readonly BrokerDefaultBase _broker;

        public BrokerHost(ConnectionFactory connectionFactory)
        {
            _broker = new BrokerDefaultBase(
                connectionFactory,
                new MessagesStorageDefault(),
                new DistributorDefault(),
                new DataProcessorDefault()
                );
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _broker.StartAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _broker.StopAsync();
        }
    }
}
