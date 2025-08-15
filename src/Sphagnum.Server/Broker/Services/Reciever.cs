using Sphagnum.Common.Infrastructure.Contracts;
using Sphagnum.Common.Infrastructure.Services;
using Sphagnum.Server.Storage.Contracts.Wal.Interfaces;
using System.Collections.Concurrent;
using System.Net;
using Sphagnum.Common.Infrastructure.Extensions;

namespace Sphagnum.Server.Broker.Services
{
    public class Reciever
    {
        public readonly CancellationTokenSource Cts = new();

        private readonly IConnection _listeningConnection;
        private readonly IWalWriter _walWriter;
        private readonly ConcurrentDictionary<Guid, IConnection> Connections = new();
        public Reciever(ConnectionFactory connectionFactory, IWalWriter walWriter)
        {
            _listeningConnection = connectionFactory.CreateConnection(false).Result;
            _listeningConnection.Bind(new IPEndPoint(IPAddress.Any, connectionFactory.Port));
            _listeningConnection.Listen(1000); //todo разобраться что делает этот параметр.
            _walWriter = walWriter;
            var _ = AcceptationWorker();
        }

        internal void AddConnection(IConnection connection)
        {
            Connections[connection.ConnectionId] = connection;
            connection.ConnectionClosed += (id) =>
            {
                Connections.TryRemove(id, out var conn);
            };
            ProcessMessages(connection);
        }

        private async Task AcceptationWorker()
        {
            while (!Cts.IsCancellationRequested)
            {
                var acceptedSocket = await _listeningConnection.AcceptAsync();
                AddConnection(acceptedSocket);
            }
        }

        internal async Task ProcessMessages(IConnection connection)
        {
            while (!connection.CancellationTokenSource.IsCancellationRequested)
            {
                var data = await connection.ReceiveAsync(connection.CancellationTokenSource.Token);
                await _walWriter.WriteData(data);
            }
        }
    }
}
