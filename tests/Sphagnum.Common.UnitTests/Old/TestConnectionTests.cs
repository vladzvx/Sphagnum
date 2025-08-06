//using Sphagnum.Common.UnitTests.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Sphagnum.Common.UnitTests
//{
//    public class TestConnectionTests
//    {
//        [Test]
//        public async Task SendRecieve8BytesWithBufferSize10()
//        {
//            var connection = new TestConnection
//            {
//                BufferSize = 10
//            };
//            var data = new byte[8];
//            Array.Fill<byte>(data, 1);
//            _ = await connection.SendAsync(data);
//            var forResult = new List<byte>();
//            for (int i = 0; i < 1; i++)
//            {
//                var buffer = new byte[connection.BufferSize];
//                _ = await connection.ReceiveAsync(buffer);
//                for (int j = 0; j < buffer.Length; j++)
//                {
//                    var el = buffer[j];
//                    if (el == 1)
//                    {
//                        forResult.Add(el);
//                    }
//                }
//                Array.Clear(buffer);
//            }

//            Assert.IsTrue(data.Length == forResult.Count);
//        }

//        [Test]
//        public async Task SendRecieve10BytesWithBufferSize10()
//        {
//            var connection = new TestConnection
//            {
//                BufferSize = 10
//            };
//            var data = new byte[10];
//            Array.Fill<byte>(data, 1);
//            _ = await connection.SendAsync(data);
//            var forResult = new List<byte>();
//            for (int i = 0; i < 1; i++)
//            {
//                var buffer = new byte[connection.BufferSize];
//                _ = await connection.ReceiveAsync(buffer);
//                for (int j = 0; j < buffer.Length; j++)
//                {
//                    var el = buffer[j];
//                    if (el == 1)
//                    {
//                        forResult.Add(el);
//                    }
//                }
//                Array.Clear(buffer);
//            }

//            Assert.IsTrue(data.Length == forResult.Count);
//        }

//        [Test]
//        public async Task SendRecieve11BytesWithBufferSize10()
//        {
//            var connection = new TestConnection
//            {
//                BufferSize = 10
//            };
//            var data = new byte[11];
//            Array.Fill<byte>(data, 1);
//            _ = await connection.SendAsync(data);
//            var forResult = new List<byte>();
//            for (int i = 0; i < 2; i++)
//            {
//                var buffer = new byte[connection.BufferSize];
//                _ = await connection.ReceiveAsync(buffer);
//                for (int j = 0; j < buffer.Length; j++)
//                {
//                    var el = buffer[j];
//                    if (el==1)
//                    {
//                        forResult.Add(el);
//                    }
//                }
//                Array.Clear(buffer);
//            }

//            Assert.IsTrue(data.Length == forResult.Count);
//        }

//        [Test]
//        public async Task SendRecieve31BytesWithBufferSize10()
//        {
//            var connection = new TestConnection
//            {
//                BufferSize = 10
//            };
//            var data = new byte[31];
//            Array.Fill<byte>(data, 1);
//            _ = await connection.SendAsync(data);
//            var forResult = new List<byte>();
//            for (int i = 0; i < 4; i++)
//            {
//                var buffer = new byte[connection.BufferSize];
//                _ = await connection.ReceiveAsync(buffer);
//                for (int j = 0; j < buffer.Length; j++)
//                {
//                    var el = buffer[j];
//                    if (el == 1)
//                    {
//                        forResult.Add(el);
//                    }
//                }
//                Array.Clear(buffer);
//            }

//            Assert.IsTrue(data.Length == forResult.Count);
//        }

//        [Test]
//        public async Task SendRecieve30BytesWithBufferSize10()
//        {
//            var connection = new TestConnection
//            {
//                BufferSize = 10
//            };
//            var data = new byte[30];
//            Array.Fill<byte>(data, 1);
//            _ = await connection.SendAsync(data);
//            var forResult = new List<byte>();
//            for (int i = 0; i < 3; i++)
//            {
//                var buffer = new byte[connection.BufferSize];
//                _ = await connection.ReceiveAsync(buffer);
//                for (int j = 0; j < buffer.Length; j++)
//                {
//                    var el = buffer[j];
//                    if (el == 1)
//                    {
//                        forResult.Add(el);
//                    }
//                }
//                Array.Clear(buffer);
//            }

//            Assert.IsTrue(data.Length == forResult.Count);
//        }
//    }

//}
