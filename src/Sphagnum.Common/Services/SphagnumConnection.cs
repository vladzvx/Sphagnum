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
    internal class SphagnumConnection
    {
        protected readonly IConnection _connection;
        private readonly ChannelsPool _pool = new ChannelsPool(100);
        private readonly ConcurrentDictionary<Guid, Channel<byte[]>> sendingItems = new ConcurrentDictionary<Guid, Channel<byte[]>>();
        private readonly Func<Func<byte[], Task>> _messagesProcessorFactory;
        private readonly Func<byte[], Task> _messagesProcessor;

        public SphagnumConnection(Func<IConnection> connectionsFactory, Func<Func<byte[], Task>> messagesProcessorFactory)
        {
            _connection = connectionsFactory(); // new SocketConnection(new Socket(SocketType.Stream, ProtocolType.Tcp));
            _messagesProcessorFactory = messagesProcessorFactory;
            _messagesProcessor = _messagesProcessorFactory();
            RecievingTask();
        }

        private SphagnumConnection(IConnection socket, Func<Func<byte[], Task>> messagesProcessorFactory)
        {
            _connection = socket;
            _messagesProcessorFactory = messagesProcessorFactory;
            _messagesProcessor = _messagesProcessorFactory();
        }

        public bool Connected => _connection.Connected;

        public Task ConnectAsync(string host, int port)
        {
            return _connection.ConnectAsync(host, port);
        }

        public async virtual Task<SphagnumConnection> AcceptAsync()
        {
            var socket = await _connection.AcceptAsync();
            return new SphagnumConnection(socket, _messagesProcessorFactory);
        }

        public void Bind(int port)
        {
            _connection.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Close()
        {
            _connection.Close();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public void Listen(int backlog)
        {
            _connection.Listen(backlog);
        }

        public async ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            int res;
            if (buffer.Span[4] != (byte)MessageType.MessageAccepted)
            {
                var channel = _pool.Get();
                sendingItems.TryAdd(MessageParser.GetMessageId(buffer), channel);
                res = await _connection.SendAsync(buffer, SocketFlags.None, cancellationToken);
                var message = await channel.Reader.WaitToReadAsync(cancellationToken);// todo обработка сообщения
                _pool.Return(channel);
                return res;
            }
            else
            {
                res = await _connection.SendAsync(buffer, SocketFlags.None, cancellationToken);
            }
            return res;
        }

        private async ValueTask<byte[]> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            var lengthBuffer = new byte[4];
            await _connection.ReceiveAsync(lengthBuffer, SocketFlags.Peek, cancellationToken);
            var length = BitConverter.ToInt32(lengthBuffer, 0);
            var result = new byte[length];
            await _connection.ReceiveAsync(result, SocketFlags.None, cancellationToken);
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
                            _connection.Close();
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
            _connection.Close();
            _connection.Dispose();
        }
    }
}
