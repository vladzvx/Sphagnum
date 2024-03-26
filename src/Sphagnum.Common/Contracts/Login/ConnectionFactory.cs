using Sphagnum.Common.Contracts.Infrastructure;
using Sphagnum.Common.Services;
using Sphagnum.Common.Utils;
using System;
using System.Threading.Tasks;

namespace Sphagnum.Common.Contracts.Login
{
    public class ConnectionFactory
    {
        public int Port { get; set; }
        public string Hostname { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRights UserRights { get; set; }

        internal virtual async Task<IConnection> CreateDefaultConnected(Func<Func<byte[], Task>> messagesProcessorFactory)
        {
            var conn = new SocketConnection(messagesProcessorFactory);
            await conn.ConnectAsync(Hostname, Port);
            return conn;
        }

        internal virtual IConnection CreateDefault(Func<Func<byte[], Task>> messagesProcessorFactoryessor)
        {
            return new SocketConnection(messagesProcessorFactoryessor);
        }
    }
}
