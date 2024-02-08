using Sphagnum.Abstractions.Administration;
using Sphagnum.Abstractions.Messaging.Models;
using Sphagnum.Common.Models;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sphagnum.Common.Utils
{
    /// <summary>
    /// Порядок хранения:
    /// 1. MessageProperties, 2 байта
    /// 2. Id сообщения, если есть, 16 байт
    /// 3. ExchangeNameLength, если есть, 1 байт
    /// 4. ExchangeName, если есть, ExchangeNameLength байт, Utf8
    /// 5. RoutingKey, если есть, 3 байта
    /// 6. PayloadSize, если есть - 4 байта
    /// 7. Payload, если есть, PayloadSize байт
    /// </summary>
    internal static class MessageParser
    {
        public static OutgoingMessage UnpackOutgoingMessage(byte[] bytes)
        {
            var exchangeName = GetExchangeName(bytes);
            var routingKey = GetRoutingKey(bytes);
            var payload = GetPayload(bytes);
            return new OutgoingMessage(exchangeName, routingKey, payload);
        }

        public static IncommingMessage UnpackIncomingMessage(byte[] bytes)
        {
            var id = GetMessageId(bytes);
            var payload = GetPayload(bytes);
            return new IncommingMessage(id, payload);
        }

        public static byte[] PackMessage(OutgoingMessage message)
        {
            if (string.IsNullOrEmpty(message.Exchange) || string.IsNullOrWhiteSpace(message.Exchange))
            {
                throw new ArgumentException("Bad exchange name!");
            }
            else if (Encoding.UTF8.GetByteCount(message.Exchange) > 255)
            {
                throw new ArgumentException("Exchange name in UTF8 encoding must allocate < 256 bytes!");
            }

            var props = MessageProperties.HasExchange;
            int count = 18;
            if (message.Payload.Length > 0)
            {
                props |= MessageProperties.HasPayload;
                count += message.Payload.Length;
                count += 4;
            }
            if (!message.RoutingKey.IsEmpry)
            {
                props |= MessageProperties.HasRoutingKey;
                count += 3;
            }

            var exchangeNameBytes = Encoding.UTF8.GetBytes(message.Exchange);// todo перевести на более оптимальный метод, не аллоцирующий лишнего.
            count += exchangeNameBytes.Length;
            count++;
            return Pack(message, Guid.NewGuid(), props, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Guid GetMessageId(Span<byte> bytes)
        {
            var slice = bytes.Slice(2, 16);
            return new Guid(slice);
        }

        internal static byte[] Pack(OutgoingMessage message, Guid id, MessageProperties props, int count)
        {
            var result = new byte[count];
            var shift = 0;
            BitConverter.TryWriteBytes(result.AsSpan(shift), (ushort)props);//1. props
            shift += 2;
            id.TryWriteBytes(result.AsSpan(shift));//2. id
            shift += 16;
            if ((props & MessageProperties.HasExchange) == MessageProperties.HasExchange)
            {
                var exchangeBytes = Encoding.UTF8.GetBytes(message.Exchange);
                BitConverter.TryWriteBytes(result.AsSpan(shift), (byte)(message.Exchange.Length));//3. ExchangeNameLength
                shift += 1;
                exchangeBytes.CopyTo(result.AsSpan(shift));//4. ExchangeName
                shift += exchangeBytes.Length;
            }
            if ((props & MessageProperties.HasRoutingKey) == MessageProperties.HasRoutingKey)//5. RoutingKey
            {
                result[shift] = message.RoutingKey.Part1;
                shift++;
                result[shift] = message.RoutingKey.Part2;
                shift++;
                result[shift] = message.RoutingKey.Part3;
                shift++;
            }
            if ((props & MessageProperties.HasPayload) == MessageProperties.HasPayload)
            {
                BitConverter.TryWriteBytes(result.AsSpan(shift), message.Payload.Length);//6. PayloadSize
                shift += 4;
                message.Payload.CopyTo(result.AsMemory(shift));//7. Payload
            }
            return result;
        }

        internal static byte[] PackMessage(IncommingMessage message)
        {
            var result = new byte[18 + message.Payload.Length + 4];
            var props = MessageProperties.HasPayload;
            BitConverter.TryWriteBytes(result.AsSpan(), (ushort)props);
            message.MessageId.TryWriteBytes(result.AsSpan(2));
            BitConverter.TryWriteBytes(result.AsSpan(18), message.Payload.Length);
            message.Payload.CopyTo(result.AsMemory(22));
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasKey(Span<byte> bytes)
        {
            return (((MessageProperties)bytes[0] & MessageProperties.HasRoutingKey) == MessageProperties.HasRoutingKey);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasPayload(Span<byte> bytes)
        {
            return (((MessageProperties)bytes[0] & MessageProperties.HasPayload) == MessageProperties.HasPayload);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasExchange(Span<byte> bytes)
        {
            return (((MessageProperties)bytes[0] & MessageProperties.HasExchange) == MessageProperties.HasExchange);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetExchangeName(Span<byte> bytes)
        {
            var hasExchange = HasExchange(bytes);
            if (!hasExchange)
            {
                throw new ArgumentException("bytes must contains exchange name!");
            }
            return Encoding.UTF8.GetString(bytes.Slice(19, bytes[18]));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RoutingKey GetRoutingKey(Span<byte> bytes)
        {
            var length = bytes[18];
            RoutingKey key;
            if (HasKey(bytes))
            {
                var routingKeyShift = 18 + length + 1;

                var routingKeyPart1 = bytes[routingKeyShift];
                var routingKeyPart2 = bytes[routingKeyShift + 1];
                var routingKeyPart3 = bytes[routingKeyShift + 2];
                key = new RoutingKey(routingKeyPart1, routingKeyPart2, routingKeyPart3);
            }
            else
            {
                key = new RoutingKey();
            }
            return key;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] GetPayload(Span<byte> bytes)
        {
            var result = Array.Empty<byte>();
            if (HasPayload(bytes))
            {
                var shift = 18;
                if (HasExchange(bytes))//todo проверить бенчмарком, как работает инлайн
                {
                    shift += bytes[18];
                    shift++;
                }
                if (HasKey(bytes))
                {
                    shift += 3;
                }

                var payloadSize = BitConverter.ToInt32(bytes[shift..]);
                if (payloadSize > 0)
                {
                    result = bytes.Slice(shift+4, payloadSize).ToArray();
                }
            }
            return result;

        }
    }
}
