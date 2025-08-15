using Microsoft.Extensions.Hosting;
using Sphagnum.Common.Infrastructure.Contracts;
using Sphagnum.Common.Infrastructure.Services;
using System.Net;

namespace Sphagnum.Server.Broker.Services
{
    public class BrokerService : IHostedService
    {
        private readonly ConnectionsManager _manager;
        public BrokerService(ConnectionsManager manager)
        {
            _manager = manager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
