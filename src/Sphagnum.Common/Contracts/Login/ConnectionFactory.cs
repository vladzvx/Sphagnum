using Sphagnum.Common.Services;
using System;
using System.Net.Sockets;
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

        internal virtual async Task<SphagnumConnection> CreateDefaultConnected(Func<Func<byte[], Task>> messagesProcessorFactory)
        {
            var conn = new SphagnumConnection(() => new SocketConnection(new Socket(SocketType.Stream, ProtocolType.Tcp)), messagesProcessorFactory);
            await conn.ConnectAsync(Hostname, Port);
            return conn;
        }

        internal virtual SphagnumConnection CreateDefault(Func<Func<byte[], Task>> messagesProcessorFactory)
        {
            return new SphagnumConnection(() => new SocketConnection(new Socket(SocketType.Stream, ProtocolType.Tcp)), messagesProcessorFactory);
        }
    }
}
