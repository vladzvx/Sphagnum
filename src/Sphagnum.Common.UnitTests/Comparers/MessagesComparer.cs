using Sphagnum.Common.Contracts.Messaging.Messages;

namespace Sphagnum.Common.UnitTests.Comparers
{
    internal static class MessagesComparer
    {
        public static bool Compare(OutgoingMessage message1, OutgoingMessage message2)
        {
            var res = true;
            res &= message1.Exchange == message2.Exchange;
            res &= message1.RoutingKey.Part1 == message2.RoutingKey.Part1;
            res &= message1.RoutingKey.Part2 == message2.RoutingKey.Part2;
            res &= message1.RoutingKey.Part3 == message2.RoutingKey.Part3;
            res &= message1.Payload.Length == message2.Payload.Length;
            res &= ComparePayloads(message1, message2);
            return res;
        }

        public static bool Compare(IncommingMessage message1, IncommingMessage message2)
        {
            var res = true;
            res &= message1.MessageId == message2.MessageId;
            res &= ComparePayloads(message1.Payload.ToArray(), message2.Payload.ToArray());
            return res;
        }

        public static bool ComparePayloads(OutgoingMessage message1, OutgoingMessage message2)
        {
            var payload1 = message1.Payload.ToArray();
            var payload2 = message2.Payload.ToArray();
            return ComparePayloads(payload1, payload2);
        }

        public static bool ComparePayloads(byte[] payload1, byte[] payload2)
        {
            var res = true;
            if (res)
            {
                for (int i = 0; i < payload1.Length; i++)
                {
                    res &= payload1[i] == payload2[i];
                }
            }
            return res;
        }
    }
}
