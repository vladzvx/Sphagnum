using Sphagnum.Common.Infrastructure.Contracts;
using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;

namespace Sphagnum.Server.Tests.Services
{
    /// <summary>
    /// Класс имитирующий передачу данных через сокет.
    /// </summary>
    internal class TestConnection : IConnection
    {
        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
        public Guid ConnectionId { get; private set; }

        public bool Connected { get; set; }

        public event Action<Guid>? ConnectionClosed;

        public Channel<byte[]> _channel = Channel.CreateUnbounded<byte[]>();
        public Channel<TestConnection> _newConnectionsChannel = Channel.CreateUnbounded<TestConnection>();

        public TestConnection(Guid id)
        {
            ConnectionId = id;
        }

        public TestConnection()
        {
            ConnectionId = Guid.NewGuid();
        }

        public void Bind(EndPoint endPoint)
        {
        }

        public void Close()
        {
            Connected = false;
            CancellationTokenSource.Cancel();
            ConnectionClosed?.Invoke(ConnectionId);
        }

        public Task ConnectAsync(string host, int port)
        {
            Connected = true;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            CancellationTokenSource.Cancel();
            ConnectionClosed?.Invoke(ConnectionId);
        }

        public void Listen(int backlog)
        {

        }

        public async ValueTask<int> ReceiveAsync(Memory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default)
        {
            var canRead = await _channel.Reader.WaitToReadAsync(cancellationToken);
            if (canRead)
            {
                if (socketFlags == SocketFlags.Peek)
                {
                    if (_channel.Reader.TryPeek(out var data))
                    {
                        int i;
                        for (i = 0; i < buffer.Length && i < data.Length; i++)
                        {
                            buffer.Span[i] = data[i];
                        }
                        return i;
                    }
                }
                else
                {
                    var data = await _channel.Reader.ReadAsync(cancellationToken);
                    for (var i = 0; i < buffer.Length && i < data.Length; i++)
                    {
                        buffer.Span[i] = data[i];
                    }
                    return data.Length;
                }
            }
            return 0;
        }

        public async ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default)
        {
            var data = new byte[buffer.Length];
            buffer.CopyTo(data);
            await _channel.Writer.WriteAsync(data, cancellationToken);
            return data.Length;
        }

        public async Task<IConnection> AcceptAsync()
        {
            return await _newConnectionsChannel.Reader.ReadAsync();
        }

        internal async Task AddInputConnection(Guid? connectionId = null)
        {
            await _newConnectionsChannel.Writer.WriteAsync(connectionId.HasValue ? new TestConnection(connectionId.Value) : new TestConnection());
        }
    }
}
