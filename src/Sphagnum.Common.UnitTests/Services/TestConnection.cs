using Sphagnum.Common.Contracts.Infrastructure;
using System;
using System.Threading.Channels;

namespace Sphagnum.Common.UnitTests.Services
{
    internal class TestConnection : IConnection
    {
        private readonly Channel<byte[]> _channel = Channel.CreateUnbounded<byte[]>();
        public int BufferSize = Constants.PayloadRecieverBufferSize;
        public bool Connected => true;

        public IConnection Accept()
        {
            return new TestConnection();
        }

        public Task<IConnection> AcceptAsync()
        {
            return Task.FromResult<IConnection>(new TestConnection());
        }

        public void Bind(int port)
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

        public ValueTask<byte[]> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            return _channel.Reader.ReadAsync(cancellationToken);
        }

        public async ValueTask<int> SendAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
        {
            await _channel.Writer.WriteAsync(data.ToArray(), cancellationToken);
            return data.Length;
        }
    }
}
