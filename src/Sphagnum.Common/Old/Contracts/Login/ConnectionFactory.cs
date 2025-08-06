using Sphagnum.Common.Infrastructure.Contracts;
using Sphagnum.Common.Infrastructure.Services;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Sphagnum.Common.Old.Contracts.Login
{
    public class ConnectionFactory
    {
        public int Port { get; set; }
        public string Hostname { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRights UserRights { get; set; }

        internal virtual async Task<IConnection> CreateDefaultConnected()
        {
            var conn = new SocketConnection(new Socket(SocketType.Stream, ProtocolType.Tcp));
            await conn.ConnectAsync(Hostname, Port);
            return conn;
        }
    }
}
