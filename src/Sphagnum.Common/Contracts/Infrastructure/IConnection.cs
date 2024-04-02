using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sphagnum.Common.Contracts.Infrastructure
{
    internal interface IConnection : IDisposable
    {
        Task ConnectAsync(string host, int port);
        Task<IConnection> AcceptAsync();
        //todo прописать бросаемые исключения
        void Bind(EndPoint endPoint);

        ValueTask<int> ReceiveAsync(Memory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default);

        //todo прописать бросаемые исключения
        void Listen(int backlog);
        //todo прописать бросаемые исключения
        ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default);
        //todo прописать бросаемые исключения
        void Close();
        bool Connected { get; }
    }
}
