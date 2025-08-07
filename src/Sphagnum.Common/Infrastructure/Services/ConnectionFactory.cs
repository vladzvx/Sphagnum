using Sphagnum.Common.Infrastructure.Contracts;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Sphagnum.Common.Infrastructure.Services
{
    public class ConnectionFactory
    {
        public int Port { get; set; }
        public string Hostname { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        internal virtual async Task<IConnection> CreateConnection(bool connected = true)
        {
            var conn = new SocketConnection(new Socket(SocketType.Stream, ProtocolType.Tcp));
            if (connected)
            {
                await conn.ConnectAsync(Hostname, Port);
            }

            return conn;
        }
    }
}
