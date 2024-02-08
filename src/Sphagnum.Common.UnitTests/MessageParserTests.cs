using Sphagnum.Common.Models;
using Sphagnum.Common.UnitTests.DataGenerators;
using Sphagnum.Common.Utils;

namespace Sphagnum.Common.UnitTests
{
    public class MessageParserTests
    {
        [Test]
        public void PackUnpackIncomingMessage_WithRoutingKeyAndPayload()
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
        public void PackUnpackOutgoingMessageGetMessageId_WithRoutingKeyAndPayload()
        {
            var count = 0;
            while (count < 100)
            {
                var id = Guid.NewGuid();
                var message = MessagesGenerator.GetRandomOutgoingMessage();
                var bytesForProps = MessageParser.PackMessage(message);
                var props = (MessageProperties)BitConverter.ToUInt16(bytesForProps.AsSpan(0, 2));
                var bytes = MessageParser.Pack(message, id, props, bytesForProps.Length);

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
                var bytesForProps = MessageParser.PackMessage(message);
                var props = (MessageProperties)BitConverter.ToUInt16(bytesForProps.AsSpan(0, 2));
                var bytes = MessageParser.Pack(message, id, props, bytesForProps.Length);

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