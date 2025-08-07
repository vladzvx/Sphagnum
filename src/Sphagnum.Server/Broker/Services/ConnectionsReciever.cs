using Microsoft.Extensions.Hosting;
using Sphagnum.Common.Infrastructure.Contracts;
using Sphagnum.Common.Infrastructure.Services;
using System.Net;

namespace Sphagnum.Server.Broker.Services
{
    public class ConnectionsReciever : IHostedService
    {
        private readonly int _port;
        private readonly CancellationTokenSource _cts = new();
        private readonly IConnection _connection;
        private readonly ConnectionsManager _manager;
        private readonly MessagesProcessor _processor;
        public ConnectionsReciever(ConnectionFactory connectionFactory, ConnectionsManager manager, MessagesProcessor processor)
        {
            _port = connectionFactory.Port;
            _manager = manager;
            _connection = connectionFactory.CreateConnection(false).Result;
            _processor = processor;
        }


        public Task StopAsync()
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }

        private async Task AcceptationWorker()
        {

            while (!_cts.IsCancellationRequested)
            {
                var acceptedSocket = await _connection.AcceptAsync();
                _manager.AddConnection(acceptedSocket);
                _processor.AddConnection(acceptedSocket);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _connection.Bind(new IPEndPoint(IPAddress.Any, _port));
            _connection?.Listen(1000); //todo разобраться что делает этот параметр.
            var _ = AcceptationWorker();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            _cts.Cancel();
            return Task.CompletedTask;
        }
    }
}
