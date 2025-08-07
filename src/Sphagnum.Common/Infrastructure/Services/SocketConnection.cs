using Sphagnum.Common.Infrastructure.Contracts;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sphagnum.Common.Infrastructure.Services
{
    internal class SocketConnection : IConnection
    {
        public Guid ConnectionId { get; private set; } = Guid.NewGuid();
        public bool Connected => _socket.Connected;
        public CancellationTokenSource CancellationTokenSource { get; private set; } = new CancellationTokenSource();

        public event Action<Guid>? ConnectionClosed;

        private readonly Socket _socket;

        public SocketConnection(Socket socket)
        {
            _socket = socket;
        }

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
            ConnectionClosed?.Invoke(ConnectionId);
            _socket.Close();
        }

        public Task ConnectAsync(string host, int port)
        {
            return _socket.ConnectAsync(host, port);
        }

        public void Dispose()
        {
            ConnectionClosed?.Invoke(ConnectionId);
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
