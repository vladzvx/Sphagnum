using AutoFixture;
using Sphagnum.Common.Messaging.Extensions;
using Sphagnum.Server.Tests.Services;
using System.Security.Cryptography;

namespace Sphagnum.Server.Tests
{
    public class TestConnectionTests
    {

        [Test]
        public async Task IsNewConnectionCretesCorrect()
        {
            var connection = new TestConnection();
            var connectionTask = connection.AcceptAsync();
            var id = Guid.NewGuid();
            await Task.WhenAll(connection.AddInputConnection(id), connectionTask);
            Assert.That(id, Is.EqualTo(connectionTask.Result.ConnectionId));
        }

        /// <summary>
        /// Проверяет сценарий, когда несколько раз подряд читаются данные
        /// с флагом peek - то есть глянуть что лежит на поверхности, не вычитывая.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SendRecieveData_WithPeekSocketFlag_MultipleReading()
        {
            var connection = new TestConnection();
            var buffer1 = new byte[5];
            var recievingTask = connection.ReceiveAsync(buffer1, System.Net.Sockets.SocketFlags.Peek);
            var fix = new Fixture();
            var data = fix.CreateMany<byte>(11).ToArray();
            var sendingTask = connection.SendAsync(data, System.Net.Sockets.SocketFlags.None);
            await Task.WhenAll(sendingTask.AsTask(), recievingTask.AsTask());
            for (int i = 0; i < 5; i++)
            {
                Assert.That(buffer1[i] == data[i]);
            }
            Assert.That(recievingTask.Result == 5);
            var buffer2 = new byte[5];
            var count = await connection.ReceiveAsync(buffer2, System.Net.Sockets.SocketFlags.Peek);
            Assert.That(recievingTask.Result == count);
            for (int i = 0; i < 5; i++)
            {
                Assert.That(buffer2[i] == data[i]);
            }

            var buffer3 = new byte[5];
            var count3 = await connection.ReceiveAsync(buffer3, System.Net.Sockets.SocketFlags.Peek);
            Assert.That(recievingTask.Result == count3);
            for (int i = 0; i < 5; i++)
            {
                Assert.That(buffer3[i] == data[i]);
            }
        }

        /// <summary>
        /// Проверяет сценарий, когда несколько раз подряд читаются данные
        /// с флагом peek - то есть глянуть что лежит на поверхности, не вычитывая. При этом, в процессе работы продолжают писаться новые данные.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SendRecieveData_WithPeekSocketFlag_MultipleReadingWithWriting()
        {
            var connection = new TestConnection();
            var buffer1 = new byte[5];
            var recievingTask = connection.ReceiveAsync(buffer1, System.Net.Sockets.SocketFlags.Peek);
            var fix = new Fixture();
            var data = fix.CreateMany<byte>(RandomNumberGenerator.GetInt32(5, 100)).ToArray();
            var sendingTask = connection.SendAsync(data, System.Net.Sockets.SocketFlags.None);
            await Task.WhenAll(sendingTask.AsTask(), recievingTask.AsTask());
            for (int i = 0; i < 5; i++)
            {
                Assert.That(buffer1[i] == data[i]);
            }
            Assert.That(recievingTask.Result == 5);
            var buffer2 = new byte[5];
            var count = await connection.ReceiveAsync(buffer2, System.Net.Sockets.SocketFlags.Peek);
            Assert.That(recievingTask.Result == count);
            for (int i = 0; i < 5; i++)
            {
                Assert.That(buffer2[i] == data[i]);
            }

            for (int c = 0; c < 20; c++)
            {
                var data2 = fix.CreateMany<byte>(RandomNumberGenerator.GetInt32(5, 100)).ToArray();
                var sendingCount = await connection.SendAsync(data2, System.Net.Sockets.SocketFlags.None);
                var buffer3 = new byte[5];
                var count3 = await connection.ReceiveAsync(buffer3, System.Net.Sockets.SocketFlags.Peek);
                Assert.That(recievingTask.Result == count3);
                for (int i = 0; i < 5; i++)
                {
                    Assert.That(buffer3[i] == data[i]);
                }
            }
        }

        /// <summary>
        /// Проверяет сценарий, чтения-отправления данных.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SendRecieveData_WithNoneSocketFlag()
        {
            var fix = new Fixture();

            var connection = new TestConnection();

            for (int c = 0; c < 20; c++)
            {
                int dataSize = RandomNumberGenerator.GetInt32(5, 100);
                var buffer1 = new byte[dataSize];
                var recievingTask = connection.ReceiveAsync(buffer1, System.Net.Sockets.SocketFlags.None);
                var data = fix.CreateMany<byte>(dataSize).ToArray();


                var sendingTask = connection.SendAsync(data, System.Net.Sockets.SocketFlags.None);

                await Task.WhenAll(sendingTask.AsTask(), recievingTask.AsTask());
                for (int i = 0; i < dataSize; i++)
                {
                    Assert.That(buffer1[i] == data[i]);
                }
                Assert.IsTrue(recievingTask.Result == sendingTask.Result);
            }
        }

        /// <summary>
        /// Проверяет сценарий, чтения-отправления данных c помощью метода расширения.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SendRecieveData_WithExtensionMethod()
        {
            var fix = new Fixture();

            var connection = new TestConnection();

            for (int c = 0; c < 20; c++)
            {
                int dataSize = RandomNumberGenerator.GetInt32(5, 100);

                var recievingTask = connection.ReceiveAsync(CancellationToken.None);
                var payload = fix.CreateMany<byte>(dataSize).ToArray();
                var length = BitConverter.GetBytes(dataSize);
                var data = new byte[dataSize + 4];
                length.CopyTo(data, 0);
                payload.CopyTo(data, 4);

                var sendingTask = connection.SendAsync(data, System.Net.Sockets.SocketFlags.None);

                await Task.WhenAll(sendingTask.AsTask(), recievingTask.AsTask());
                for (int i = 0; i < dataSize; i++)
                {
                    Assert.That(recievingTask.Result[i] == data[i]);
                }
            }
        }
    }
}