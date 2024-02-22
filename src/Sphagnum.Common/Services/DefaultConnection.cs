using Sphagnum.Common.Contracts.Infrastructure;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sphagnum.Common.Services
{
    internal class DefaultConnection : IConnection
    {
        private readonly Socket _socket;

        public DefaultConnection()
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public DefaultConnection(Socket socket)
        {
            _socket = socket;
        }

        public bool Connected => _socket.Connected;

        public Task ConnectAsync(string host, int port)
        {
            return _socket.ConnectAsync(host, port);
        }

        public IConnection Accept()
        {
            var socket = _socket.Accept();
            return new DefaultConnection(socket);
        }

        public async Task<IConnection> AcceptAsync()
        {
            var socket = await _socket.AcceptAsync();
            return new DefaultConnection(socket);
        }

        public void Bind(int port)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Close()
        {
            _socket.Close();
        }

        public void Dispose()
        {
            _socket.Dispose();
        }

        public void Listen(int backlog)
        {
            _socket.Listen(backlog);
        }

        public ValueTask<int> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return _socket.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);
        }

        public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return _socket.SendAsync(buffer, SocketFlags.None, cancellationToken);
        }

        public async ValueTask<byte[]> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            var lengthBuffer = new byte[4];
            await _socket.ReceiveAsync(lengthBuffer, SocketFlags.Peek, cancellationToken);
            var length = BitConverter.ToInt32(lengthBuffer, 0);
            var result = new byte[length];
            await _socket.ReceiveAsync(result, SocketFlags.None, cancellationToken);
            return result;
        }
    }
}
