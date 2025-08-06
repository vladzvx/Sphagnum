using Sphagnum.Common.Messaging.Contracts;
using Sphagnum.Common.Messaging.Utils;
using Sphagnum.Common.UnitTests.DataGenerators;

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
                var message = MessagesGenerator.GetRandomMessage();
                var bytes = MessageParser.PackMessage(message);
                var message2 = MessageParser.UnpackMessage(bytes);
                var bytes2 = MessageParser.PackMessage(message2);
                var f1 = (MessageFlags)BitConverter.ToUInt16(bytes.AsSpan(5, 2));
                var f2 = (MessageFlags)BitConverter.ToUInt16(bytes2.AsSpan(5, 2));
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
                var message = MessagesGenerator.GetRandomMessage(null, false, true);
                var bytes = MessageParser.PackMessage(message);
                var message2 = MessageParser.UnpackMessage(bytes);
                var bytes2 = MessageParser.PackMessage(message2);
                var f1 = (MessageFlags)BitConverter.ToUInt16(bytes.AsSpan(5, 2));
                var f2 = (MessageFlags)BitConverter.ToUInt16(bytes2.AsSpan(5, 2));
                Assert.IsTrue(f1 == f2);
                Assert.That(Comparers.MessagesComparer.Compare(message, message2), Is.True);

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
                var message = MessagesGenerator.GetRandomMessage(null, true, true);
                var bytes = MessageParser.PackMessage(message);
                var message2 = MessageParser.UnpackMessage(bytes);
                var bytes2 = MessageParser.PackMessage(message2);
                var f1 = (MessageFlags)BitConverter.ToUInt16(bytes.AsSpan(5, 2));
                var f2 = (MessageFlags)BitConverter.ToUInt16(bytes2.AsSpan(5, 2));
                Assert.IsTrue(f1 == f2);
                Assert.That(Comparers.MessagesComparer.Compare(message, message2), Is.True);
                count++;
            }
        }
    }
}