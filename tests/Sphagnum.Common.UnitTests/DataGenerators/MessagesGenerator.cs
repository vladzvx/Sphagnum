using Sphagnum.Common.Contracts.Messaging;
using Sphagnum.Common.Contracts.Messaging.Messages;
using System.Security.Cryptography;

namespace Sphagnum.Common.UnitTests.DataGenerators
{
    internal static class MessagesGenerator
    {
        public static OutgoingMessage GetRandomOutgoingMessage(bool emptyKey = false, bool emptyPayload = false)
        {
            var exchangeName = RandomNumberGenerator.GetInt32(10000000).ToString();
            var payload = !emptyPayload ? RandomNumberGenerator.GetBytes(RandomNumberGenerator.GetInt32(0, 1000)) : [];
            var routingKeysBytes = RandomNumberGenerator.GetBytes(3);
            return new OutgoingMessage(exchangeName, !emptyKey ? new RoutingKey(routingKeysBytes[0], routingKeysBytes[1], routingKeysBytes[2]) : new RoutingKey(0, 0, 0), payload);
        }

        public static IncommingMessage GetRandoIncommingMessage(bool emptyPayload = false)
        {
            var payload = !emptyPayload ? RandomNumberGenerator.GetBytes(RandomNumberGenerator.GetInt32(0, 1000)) : [];
            return new IncommingMessage(Guid.NewGuid(), payload);
        }
    }
}
