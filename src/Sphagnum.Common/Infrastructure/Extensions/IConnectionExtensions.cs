using Sphagnum.Common.Infrastructure.Contracts;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sphagnum.Common.Infrastructure.Extensions
{
    internal static class IConnectionExtensions
    {
        public static async ValueTask<byte[]> ReceiveAsync(this IConnection connection, CancellationToken cancellationToken = default)
        {
            var lengthBuffer = new byte[5];
            await connection.ReceiveAsync(lengthBuffer, SocketFlags.Peek, cancellationToken);
            var length = BitConverter.ToInt32(lengthBuffer, 0);
            var result = new byte[length];
            await connection.ReceiveAsync(result, SocketFlags.None, cancellationToken);
            return result;
        }
    }
}
