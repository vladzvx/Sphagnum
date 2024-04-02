using Sphagnum.Common.Contracts.Infrastructure;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace Sphagnum.Common.UnitTests.Services
{
    internal class TestConnection : IConnection
    {
        private readonly ConcurrentQueue<byte[]> _queue = new();
        public bool Connected => true;

        public IConnection Accept()
        {
            return new TestConnection();
        }

        public Task<IConnection> AcceptAsync()
        {
            return Task.FromResult<IConnection>(new TestConnection());
        }

        public void Bind(EndPoint endPoint)
        {
        }

        public void Close()
        {

        }

        public Task ConnectAsync(string host, int port)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }

        public void Listen(int backlog)
        {

        }

        public async ValueTask<int> ReceiveAsync(Memory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default)
        {
            var res = new byte[buffer.Length];
            await Receive(res, socketFlags, cancellationToken);
            res.CopyTo(buffer);
            return res.Length;
        }

        public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default)
        {
            _queue.Enqueue(buffer.Span.ToArray());
            return ValueTask.FromResult(buffer.Length);
        }

        private async ValueTask<int> Receive(byte[] buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default, int counter = 0)
        {
            if (counter > 200)
            {
                throw new TimeoutException();
            }

            if (socketFlags == SocketFlags.Peek ? _queue.TryPeek(out byte[]? result) : _queue.TryDequeue(out result))
            {
                result.CopyTo(buffer, 0);
                return result.Length;
            }
            else
            {
                await Task.Delay(100, cancellationToken);
                counter++;
                await Receive(buffer, socketFlags, cancellationToken, counter);
            }
            throw new TimeoutException();
        }
    }
}
