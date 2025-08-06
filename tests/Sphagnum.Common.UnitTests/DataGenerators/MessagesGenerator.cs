using Sphagnum.Common.Messaging.Contracts;
using Sphagnum.Common.Messaging.Contracts.Messages;
using System.Security.Cryptography;

namespace Sphagnum.Common.UnitTests.DataGenerators
{
    internal static class MessagesGenerator
    {
        public static Message GetRandomMessage(Guid? messageId = null, bool emptyKey = false, bool emptyPayload = false)
        {
            var exchangeName = RandomNumberGenerator.GetInt32(10000000).ToString();
            var payload = !emptyPayload ? RandomNumberGenerator.GetBytes(RandomNumberGenerator.GetInt32(0, 1000)) : [];
            var routingKeysBytes = RandomNumberGenerator.GetBytes(3);
            if (messageId.HasValue)
            {
                return new Message(messageId.Value, exchangeName, !emptyKey ? new RoutingKey(routingKeysBytes[0], routingKeysBytes[1], routingKeysBytes[2]) : new RoutingKey(0, 0, 0), payload);
            }
            else
            {
                return new Message(exchangeName, !emptyKey ? new RoutingKey(routingKeysBytes[0], routingKeysBytes[1], routingKeysBytes[2]) : new RoutingKey(0, 0, 0), payload);
            }
        }
    }
}
