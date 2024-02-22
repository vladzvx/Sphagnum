using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sphagnum.Common.Contracts.Infrastructure
{
    internal interface IConnection : IDisposable
    {
        Task ConnectAsync(string host, int port);
        Task<IConnection> AcceptAsync();
        IConnection Accept();
        //todo прописать бросаемые исключения
        void Bind(int port);
        //todo прописать бросаемые исключения
        void Listen(int backlog);
        //todo прописать бросаемые исключения
        ValueTask<byte[]> ReceiveAsync(CancellationToken cancellationToken = default);
        //todo прописать бросаемые исключения
        ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default);
        //todo прописать бросаемые исключения
        void Close();
        bool Connected { get; }
    }
}
