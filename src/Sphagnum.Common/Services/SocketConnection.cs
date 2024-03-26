using Sphagnum.Common.Contracts.Infrastructure;
using Sphagnum.Common.Exceptions;
using Sphagnum.Common.Utils;
using Sphagnum.Common.Utils.Models;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Sphagnum.Common.Services
{
    internal class SocketConnection : IConnection
    {
        protected readonly Socket _socket;
        private readonly ChannelsPool _pool = new ChannelsPool(100);
        private readonly ConcurrentDictionary<Guid, Channel<byte[]>> sendingItems = new ConcurrentDictionary<Guid, Channel<byte[]>>();
        private readonly Func<Func<byte[], Task>> _messagesProcessorFactory;
        private readonly Func<byte[], Task> _messagesProcessor;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public SocketConnection(Func<Func<byte[], Task>> messagesProcessorFactory)
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _messagesProcessorFactory = messagesProcessorFactory;
            _messagesProcessor = _messagesProcessorFactory();
            RecievingTask();
        }

        public SocketConnection(Socket socket, Func<Func<byte[], Task>> messagesProcessorFactory)
        {
            _socket = socket;
            _messagesProcessorFactory = messagesProcessorFactory;
            _messagesProcessor = _messagesProcessorFactory();
        }

        public bool Connected => _socket.Connected;

        public Task ConnectAsync(string host, int port)
        {
            return _socket.ConnectAsync(host, port);
        }

        public async virtual Task<IConnection> AcceptAsync()
        {
            var socket = await _socket.AcceptAsync();
            return new SocketConnection(socket, _messagesProcessorFactory);
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

        public async ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            int res;
            if (buffer.Span[4] != (byte)MessageType.MessageAccepted)
            {
                var channel = _pool.Get();
                sendingItems.TryAdd(MessageParser.GetMessageId(buffer), channel);
                res = await _socket.SendAsync(buffer, SocketFlags.None, cancellationToken);
                var message = await channel.Reader.WaitToReadAsync(cancellationToken);// todo обработка сообщения
                _pool.Return(channel);
                return res;
            }
            else
            {
                res = await _socket.SendAsync(buffer, SocketFlags.None, cancellationToken);
            }
            return res;
        }

        private async ValueTask<byte[]> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            var lengthBuffer = new byte[4];
            await _socket.ReceiveAsync(lengthBuffer, SocketFlags.Peek, cancellationToken);
            var length = BitConverter.ToInt32(lengthBuffer, 0);
            var result = new byte[length];
            await _socket.ReceiveAsync(result, SocketFlags.None, cancellationToken);
            return result;
        }

        private async Task RecievingTask(CancellationToken token = default)
        {
            var successRespBuffer = new byte[23];
            successRespBuffer[4] = (byte)MessageType.MessageAccepted;
            successRespBuffer[0] = 23;
            while (Connected)
            {
                try
                {
                    var message = await ReceiveAsync(token);
                    if (message[4] != (byte)MessageType.MessageAccepted)
                    {
                        try
                        {
                            await _messagesProcessor(message);
                            MessageParser.CopyId(message, successRespBuffer);
                            await SendAsync(successRespBuffer, token);
                        }
                        catch (AuthException ex)
                        {
                            await SendAsync(successRespBuffer, token);
                            await Task.Delay(1000);
                            _socket.Close();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (sendingItems.TryRemove(MessageParser.GetMessageId(message), out var channel))
                    {
                        await channel.Writer.WriteAsync(message);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            _socket.Close();
            _socket.Dispose();
        }
    }
}
