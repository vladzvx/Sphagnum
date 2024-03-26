using Microsoft.Extensions.Hosting;
using Sphagnum.Common.Contracts.Login;
using Sphagnum.Server.Broker.Services;

namespace Sphagnum.Server
{
    public class BrokerHost : IHostedService
    {
        private readonly BrokerDefaultBase _broker;

        public BrokerHost(ConnectionFactory connectionFactory)
        {
            _broker = BrokerDefaultBase.Create(connectionFactory);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _broker.StartAsync(8081);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _broker.StopAsync();
        }
    }
}
