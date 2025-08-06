//using Sphagnum.Common.Messaging.Utils;
//using Sphagnum.Common.Old.Utils.Enums;
//using Sphagnum.Common.UnitTests.Comparers;
//using Sphagnum.Common.UnitTests.DataGenerators;

//namespace Sphagnum.Common.UnitTests.Old
//{
//    public class MessageParserTests
//    {
//        [Test]
//        public void PackUnpackIncomingMessage_WithPayload()
//        {
//            var count = 0;
//            while (count < 100)
//            {
//                var message = MessagesGenerator.GetRandomIncommingMessage();
//                var bytes = MessageParserold.PackMessage(message);
//                var message2 = MessageParserold.UnpackIncomingMessage(bytes);
//                Assert.That(MessagesComparer.Compare(message, message2), Is.True);

//                count++;
//            }
//        }

//        [Test]
//        public void PackUnpackIncomingMessage_WithEmptyPayload()
//        {
//            var count = 0;
//            while (count < 100)
//            {
//                var message = MessagesGenerator.GetRandoIncommingMessage(true);
//                var bytes = MessageParserold.PackMessage(message);
//                var message2 = MessageParserold.UnpackIncomingMessage(bytes);
//                Assert.That(MessagesComparer.Compare(message, message2), Is.True);
//                Assert.IsTrue((MessageFlags)BitConverter.ToUInt16(bytes.AsSpan(5, 2)) == MessageFlags.HasPayload);
//                count++;
//            }
//        }

//        [Test]
//        public void PackUnpackOutgoingMessageGetMessageId_WithRoutingKeyAndPayload()
//        {
//            var count = 0;
//            while (count < 100)
//            {
//                var id = Guid.NewGuid();
//                var message = MessagesGenerator.GetRandomOutgoingMessage();
//                var bytesForFlags = MessageParserold.PackMessage(message);
//                var flags = (MessageFlags)BitConverter.ToUInt16(bytesForFlags.AsSpan(5, 2));
//                var bytes = MessageParserold.Pack(message, id, flags, bytesForFlags.Length);

//                var message2 = MessageParserold.UnpackOutgoingMessage(bytes);
//                Assert.That(MessagesComparer.Compare(message, message2), Is.True);
//                var id2 = MessageParserold.GetMessageId(bytes);
//                Assert.That(id, Is.EqualTo(id2));
//                count++;
//            }
//        }

//        [Test]
//        public void PackUnpackOutgoingMessageGetMessageId_WithEmptyRoutingKeyAndEmptyPayload()
//        {
//            var count = 0;
//            while (count < 100)
//            {
//                var id = Guid.NewGuid();
//                var message = MessagesGenerator.GetRandomOutgoingMessage(true, true);
//                var bytesForFlags = MessageParserold.PackMessage(message);
//                var flags = (MessageFlags)BitConverter.ToUInt16(bytesForFlags.AsSpan(5, 2));
//                var bytes = MessageParserold.Pack(message, id, flags, bytesForFlags.Length);

//                var message2 = MessageParserold.UnpackOutgoingMessage(bytes);
//                Assert.That(MessagesComparer.Compare(message, message2), Is.True);
//                var id2 = MessageParserold.GetMessageId(bytes);
//                Assert.That(id, Is.EqualTo(id2));
//                count++;
//            }
//        }

//        [Test]
//        public void PackUnpackOutgoingMessage_WithRoutingKeyAndPayload()
//        {
//            var count = 0;
//            while (count < 100)
//            {
//                var message = MessagesGenerator.GetRandomOutgoingMessage();
//                var bytes = MessageParserold.PackMessage(message);
//                var message2 = MessageParserold.UnpackOutgoingMessage(bytes);
//                Assert.That(MessagesComparer.Compare(message, message2), Is.True);
//                var bytes2 = MessageParserold.PackMessage(message2);
//                var message3 = MessageParserold.UnpackOutgoingMessage(bytes2);
//                Assert.That(MessagesComparer.ComparePayloads(message2, message3), Is.True);
//                var bytes3 = MessageParserold.PackMessage(message2);
//                var message4 = MessageParserold.UnpackOutgoingMessage(bytes3);
//                Assert.That(MessagesComparer.ComparePayloads(message3, message4), Is.True);
//                count++;
//            }
//        }

//        [Test]
//        public void PackUnpackOutgoingMessage_WithRoutingKeyAndEmptyPayload()
//        {
//            var count = 0;
//            while (count < 100)
//            {
//                var message = MessagesGenerator.GetRandomOutgoingMessage(false, true);
//                var bytes = MessageParserold.PackMessage(message);
//                var message2 = MessageParserold.UnpackOutgoingMessage(bytes);
//                Assert.That(MessagesComparer.Compare(message, message2), Is.True);
//                var bytes2 = MessageParserold.PackMessage(message2);
//                var message3 = MessageParserold.UnpackOutgoingMessage(bytes2);
//                Assert.That(MessagesComparer.ComparePayloads(message2, message3), Is.True);
//                var bytes3 = MessageParserold.PackMessage(message2);
//                var message4 = MessageParserold.UnpackOutgoingMessage(bytes3);
//                Assert.That(MessagesComparer.ComparePayloads(message3, message4), Is.True);
//                count++;
//            }
//        }

//        [Test]
//        public void PackUnpackOutgoingMessage_WithEmptyRoutingKey()
//        {
//            var count = 0;
//            while (count < 100)
//            {
//                var message = MessagesGenerator.GetRandomOutgoingMessage(true);
//                var bytes = MessageParserold.PackMessage(message);
//                var message2 = MessageParserold.UnpackOutgoingMessage(bytes);
//                Assert.That(MessagesComparer.Compare(message, message2), Is.True);
//                var bytes2 = MessageParserold.PackMessage(message2);
//                var message3 = MessageParserold.UnpackOutgoingMessage(bytes2);
//                Assert.That(MessagesComparer.ComparePayloads(message2, message3), Is.True);
//                var bytes3 = MessageParserold.PackMessage(message2);
//                var message4 = MessageParserold.UnpackOutgoingMessage(bytes3);
//                Assert.That(MessagesComparer.ComparePayloads(message3, message4), Is.True);
//                count++;
//            }
//        }

//        [Test]
//        public void PackUnpackOutgoingMessage_WithEmptyRoutingKeyAndEmptyPayload()
//        {
//            var count = 0;
//            while (count < 100)
//            {
//                var message = MessagesGenerator.GetRandomOutgoingMessage(true, true);
//                var bytes = MessageParserold.PackMessage(message);
//                var message2 = MessageParserold.UnpackOutgoingMessage(bytes);
//                Assert.That(MessagesComparer.Compare(message, message2), Is.True);
//                var bytes2 = MessageParserold.PackMessage(message2);
//                var message3 = MessageParserold.UnpackOutgoingMessage(bytes2);
//                Assert.That(MessagesComparer.ComparePayloads(message2, message3), Is.True);
//                var bytes3 = MessageParserold.PackMessage(message2);
//                var message4 = MessageParserold.UnpackOutgoingMessage(bytes3);
//                Assert.That(MessagesComparer.ComparePayloads(message3, message4), Is.True);
//                count++;
//            }
//        }
//    }
//}