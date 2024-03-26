using Sphagnum.Common.Contracts.Infrastructure;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sphagnum.Common.Services
{
    internal class SocketConnection : IConnection
    {
        private readonly Socket _socket;

        public SocketConnection(Socket socket)
        {
            _socket = socket;
        }

        public bool Connected => _socket.Connected;

        public async Task<IConnection> AcceptAsync()
        {
            var socket = await _socket.AcceptAsync();
            return new SocketConnection(socket);
        }

        public void Bind(EndPoint endPoint)
        {
            _socket.Bind(endPoint);
        }

        public void Close()
        {
            _socket.Close();
        }

        public Task ConnectAsync(string host, int port)
        {
            return _socket.ConnectAsync(host, port);
        }

        public void Dispose()
        {
            _socket.Dispose();
        }

        public void Listen(int backlog)
        {
            _socket.Listen(backlog);
        }

        public ValueTask<int> ReceiveAsync(Memory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default)
        {
            return _socket.ReceiveAsync(buffer, socketFlags, cancellationToken);
        }

        public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default)
        {
            return _socket.SendAsync(buffer, socketFlags, cancellationToken);
        }
    }
}
