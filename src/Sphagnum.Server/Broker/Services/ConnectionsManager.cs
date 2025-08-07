using Sphagnum.Common.Infrastructure.Contracts;
using System.Collections.Concurrent;

namespace Sphagnum.Server.Broker.Services
{
    public class ConnectionsManager
    {
        private readonly ConcurrentDictionary<Guid, IConnection> Connections = new();
        public ConnectionsManager()
        {

        }

        internal void AddConnection(IConnection connection)
        {
            connection.ConnectionClosed += (id) =>
            {
                Connections.TryRemove(id, out var conn);
            };
        }
    }
}
