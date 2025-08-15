using Sphagnum.Common.Infrastructure.Contracts;
using Sphagnum.Common.Infrastructure.Extensions;
using Sphagnum.Server.Storage.Contracts.Wal.Interfaces;
using System.Collections.Concurrent;

namespace Sphagnum.Server.Broker.Services
{
    public class MessagesProcessor
    {
        private readonly IWalWriter _walWriter;
        private readonly ConcurrentDictionary<Guid, IConnection> Connections = new();

        public MessagesProcessor(IWalWriter walWriter)
        {
            _walWriter = walWriter;
        }

        internal void AddConnection(IConnection connection)
        {
            connection.ConnectionClosed += (id) =>
            {
                Connections.TryRemove(id, out var conn);
            };
            var _ = ProcessMessages(connection);
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
