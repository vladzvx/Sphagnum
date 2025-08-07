using Sphagnum.Common.Infrastructure.Contracts;
using Sphagnum.Common.Messaging.Contracts.Messages;
using Sphagnum.Common.Messaging.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sphagnum.Common.Messaging.Extensions;

namespace Sphagnum.Server.Broker.Services
{
    public class MessagesProcessor
    {
        private readonly ConcurrentDictionary<Guid, IConnection> Connections = new();
        internal void AddConnection(IConnection connection)
        {
            connection.ConnectionClosed += (id) =>
            {
                Connections.TryRemove(id, out var conn);
            };
            ProcessMessages(connection);
        }

        internal static async Task ProcessMessages(IConnection connection)
        { 
            while (!connection.CancellationTokenSource.IsCancellationRequested)
            {
                var data = await connection.ReceiveAsync(connection.CancellationTokenSource.Token);
                var mess = MessageParser.UnpackMessage(data);
            }
        }
    }
}
