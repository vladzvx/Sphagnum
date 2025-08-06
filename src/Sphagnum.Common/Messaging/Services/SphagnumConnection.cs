using Sphagnum.Common.Infrastructure.Contracts;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sphagnum.Common.Messaging.Services
{
    internal class SphagnumConnection
    {
        private readonly IConnection _connection;
        public bool Connected => throw new NotImplementedException();

        public void Close()
        {
            throw new NotImplementedException();
        }

        public Task ConnectAsync(string host, int port)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> ReceiveAsync(Memory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
