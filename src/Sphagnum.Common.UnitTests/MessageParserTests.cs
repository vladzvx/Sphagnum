using Sphagnum.Common.UnitTests.DataGenerators;
using Sphagnum.Common.Utils;
using Sphagnum.Common.Utils.Models;

namespace Sphagnum.Common.UnitTests
{
    public class MessageParserTests
    {
        [Test]
        public void PackUnpackIncomingMessage_WithPayload()
        {
            var count = 0;
            while (count < 100)
            {
                var message = MessagesGenerator.GetRandoIncommingMessage();
                var bytes = MessageParser.PackMessage(message);
                var message2 = MessageParser.UnpackIncomingMessage(bytes);
                Assert.That(Comparers.MessagesComparer.Compare(message, message2), Is.True);

                count++;
            }
        }

        [Test]
        public void PackUnpackIncomingMessage_WithEmptyPayload()
        {
            var count = 0;
            while (count < 100)
            {
                var message = MessagesGenerator.GetRandoIncommingMessage(true);
                var bytes = MessageParser.PackMessage(message);
                var message2 = MessageParser.UnpackIncomingMessage(bytes);
                Assert.That(Comparers.MessagesComparer.Compare(message, message2), Is.True);
                Assert.IsTrue((MessageFlags)BitConverter.ToUInt16(bytes.AsSpan(5, 2)) == MessageFlags.HasPayload);
                count++;
            }
        }

        [Test]
        public void PackUnpackOutgoingMessageGetMessageId_WithRoutingKeyAndPayload()
        {
            var count = 0;
            while (count < 100)
            {
                var id = Guid.NewGuid();
                var message = MessagesGenerator.GetRandomOutgoingMessage();
                var bytesForFlags = MessageParser.PackMessage(message);
                var flags = (MessageFlags)BitConverter.ToUInt16(bytesForFlags.AsSpan(5, 2));
                var bytes = MessageParser.Pack(message, id, flags, bytesForFlags.Length);

                var message2 = MessageParser.UnpackOutgoingMessage(bytes);
                Assert.That(Comparers.MessagesComparer.Compare(message, message2), Is.True);
                var id2 = MessageParser.GetMessageId(bytes);
                Assert.That(id, Is.EqualTo(id2));
                count++;
            }
        }

        [Test]
        public void PackUnpackOutgoingMessageGetMessageId_WithEmptyRoutingKeyAndEmptyPayload()
        {
            var count = 0;
            while (count < 100)
            {
                var id = Guid.NewGuid();
                var message = MessagesGenerator.GetRandomOutgoingMessage(true, true);
                var bytesForFlags = MessageParser.PackMessage(message);
                var flags = (MessageFlags)BitConverter.ToUInt16(bytesForFlags.AsSpan(5, 2));
                var bytes = MessageParser.Pack(message, id, flags, bytesForFlags.Length);

                var message2 = MessageParser.UnpackOutgoingMessage(bytes);
                Assert.That(Comparers.MessagesComparer.Compare(message, message2), Is.True);
                var id2 = MessageParser.GetMessageId(bytes);
                Assert.That(id, Is.EqualTo(id2));
                count++;
            }
        }

        [Test]
        public void PackUnpackOutgoingMessage_WithRoutingKeyAndPayload()
        {
            var count = 0;
            while (count < 100)
            {
                var message = MessagesGenerator.GetRandomOutgoingMessage();
                var bytes = MessageParser.PackMessage(message);
                var message2 = MessageParser.UnpackOutgoingMessage(bytes);
                Assert.That(Comparers.MessagesComparer.Compare(message, message2), Is.True);
                var bytes2 = MessageParser.PackMessage(message2);
                var message3 = MessageParser.UnpackOutgoingMessage(bytes2);
                Assert.That(Comparers.MessagesComparer.ComparePayloads(message2, message3), Is.True);
                var bytes3 = MessageParser.PackMessage(message2);
                var message4 = MessageParser.UnpackOutgoingMessage(bytes3);
                Assert.That(Comparers.MessagesComparer.ComparePayloads(message3, message4), Is.True);
                count++;
            }
        }

        [Test]
        public void PackUnpackOutgoingMessage_WithRoutingKeyAndEmptyPayload()
        {
            var count = 0;
            while (count < 100)
            {
                var message = MessagesGenerator.GetRandomOutgoingMessage(false, true);
                var bytes = MessageParser.PackMessage(message);
                var message2 = MessageParser.UnpackOutgoingMessage(bytes);
                Assert.That(Comparers.MessagesComparer.Compare(message, message2), Is.True);
                var bytes2 = MessageParser.PackMessage(message2);
                var message3 = MessageParser.UnpackOutgoingMessage(bytes2);
                Assert.That(Comparers.MessagesComparer.ComparePayloads(message2, message3), Is.True);
                var bytes3 = MessageParser.PackMessage(message2);
                var message4 = MessageParser.UnpackOutgoingMessage(bytes3);
                Assert.That(Comparers.MessagesComparer.ComparePayloads(message3, message4), Is.True);
                count++;
            }
        }

        [Test]
        public void PackUnpackOutgoingMessage_WithEmptyRoutingKey()
        {
            var count = 0;
            while (count < 100)
            {
                var message = MessagesGenerator.GetRandomOutgoingMessage(true);
                var bytes = MessageParser.PackMessage(message);
                var message2 = MessageParser.UnpackOutgoingMessage(bytes);
                Assert.That(Comparers.MessagesComparer.Compare(message, message2), Is.True);
                var bytes2 = MessageParser.PackMessage(message2);
                var message3 = MessageParser.UnpackOutgoingMessage(bytes2);
                Assert.That(Comparers.MessagesComparer.ComparePayloads(message2, message3), Is.True);
                var bytes3 = MessageParser.PackMessage(message2);
                var message4 = MessageParser.UnpackOutgoingMessage(bytes3);
                Assert.That(Comparers.MessagesComparer.ComparePayloads(message3, message4), Is.True);
                count++;
            }
        }

        [Test]
        public void PackUnpackOutgoingMessage_WithEmptyRoutingKeyAndEmptyPayload()
        {
            var count = 0;
            while (count < 100)
            {
                var message = MessagesGenerator.GetRandomOutgoingMessage(true, true);
                var bytes = MessageParser.PackMessage(message);
                var message2 = MessageParser.UnpackOutgoingMessage(bytes);
                Assert.That(Comparers.MessagesComparer.Compare(message, message2), Is.True);
                var bytes2 = MessageParser.PackMessage(message2);
                var message3 = MessageParser.UnpackOutgoingMessage(bytes2);
                Assert.That(Comparers.MessagesComparer.ComparePayloads(message2, message3), Is.True);
                var bytes3 = MessageParser.PackMessage(message2);
                var message4 = MessageParser.UnpackOutgoingMessage(bytes3);
                Assert.That(Comparers.MessagesComparer.ComparePayloads(message3, message4), Is.True);
                count++;
            }
        }
    }
}