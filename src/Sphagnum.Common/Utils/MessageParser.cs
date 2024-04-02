using Sphagnum.Common.Contracts.Messaging;
using Sphagnum.Common.Contracts.Messaging.Messages;
using Sphagnum.Common.Utils.Models;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sphagnum.Common.Utils
{
    /// <summary>
    /// Порядок передачи:
    /// 1. MessageSize, 4 байта
    /// 2. MessageType, 1 байт
    /// 3. MessageFlags, 2 байта
    /// 4. Id сообщения, если есть, 16 байт
    /// 5. ExchangeNameLength, если есть, 1 байт
    /// 6. ExchangeName, если есть, ExchangeNameLength байт, Utf8
    /// 7. RoutingKey, если есть, 3 байта
    /// 8. PayloadSize, если есть - 4 байта
    /// 9. Payload, если есть, PayloadSize байт
    /// </summary>
    internal static class MessageParser
    {
        public static OutgoingMessage UnpackOutgoingMessage(byte[] bytes)
        {
            if ((MessageType)bytes[4] != MessageType.Common)
            {
                throw new ArgumentException("Uncorrect message type! 1 (MessageType.Common) expected!");
            }
            var exchangeName = GetExchangeName(bytes);
            var routingKey = GetRoutingKey(bytes);
            var payload = GetPayload(bytes);
            return new OutgoingMessage(exchangeName, routingKey, payload);
        }

        public static IncommingMessage UnpackIncomingMessage(byte[] bytes)
        {
            if ((MessageType)bytes[4] != MessageType.Common)
            {
                throw new ArgumentException("Uncorrect message type! 1 (MessageType.Common) expected!");
            }
            var id = GetMessageId(bytes);
            var payload = GetPayload(bytes);
            return new IncommingMessage(id, payload);
        }

        public static byte[] PackMessage(IncommingMessage message)
        {
            var result = new byte[27 + message.Payload.Length];
            result[4] = (byte)MessageType.Common;
            var flags = MessageFlags.HasPayload;
            BitConverter.TryWriteBytes(result.AsSpan(5, 2), (ushort)flags);
            message.MessageId.TryWriteBytes(result.AsSpan(7));
            BitConverter.TryWriteBytes(result.AsSpan(23), message.Payload.Length);
            message.Payload.CopyTo(result.AsMemory(27));
            BitConverter.TryWriteBytes(result.AsSpan(0, 4), result.Length);
            return result;
        }

        public static byte[] PackReplyMessage(MessageType messageType)
        {
            var res = new byte[23];
            res[4] = (byte)messageType;
            res[0] = 23;
            return res;
        }

        public static byte[] PackReplyMessage(MessageType messageType, Guid parentMessageId)
        {
            var res = new byte[23];
            res[4] = (byte)messageType;
            res[0] = 23;
            parentMessageId.TryWriteBytes(res.AsSpan(7));
            return res;
        }

        public static byte[] PackMessage(AuthMessage message)
        {
            var result = new byte[27 + message.Payload.Length];
            result[4] = (byte)MessageType.Auth;
            var flags = MessageFlags.HasPayload;
            BitConverter.TryWriteBytes(result.AsSpan(5, 2), (ushort)flags);
            message.MessageId.TryWriteBytes(result.AsSpan(7));
            BitConverter.TryWriteBytes(result.AsSpan(23), message.Payload.Length);
            message.Payload.CopyTo(result.AsMemory(27));
            BitConverter.TryWriteBytes(result.AsSpan(0, 4), result.Length);
            return result;
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

            var flags = MessageFlags.HasExchange;
            int count = 23;
            if (message.Payload.Length > 0)
            {
                flags |= MessageFlags.HasPayload;
                count += message.Payload.Length;
                count += 4;
            }
            if (!message.RoutingKey.IsEmpry)
            {
                flags |= MessageFlags.HasRoutingKey;
                count += 3;
            }

            var exchangeNameBytes = Encoding.UTF8.GetBytes(message.Exchange);// todo перевести на более оптимальный метод, не аллоцирующий лишнего.
            count += exchangeNameBytes.Length;
            count++;
            return Pack(message, Guid.NewGuid(), flags, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Guid GetMessageId(byte[] bytes)
        {
            var slice = bytes.AsSpan(7, 16);
            return new Guid(slice);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Guid GetMessageId(Span<byte> bytes)
        {
            var slice = bytes.Slice(7, 16);
            return new Guid(slice);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Guid GetMessageId(ReadOnlyMemory<byte> bytes)
        {
            var slice = bytes.Slice(7, 16);
            return new Guid(slice.Span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void CopyId(ReadOnlySpan<byte> bytes, byte[] buffer)
        {
            var slice = bytes.Slice(7, 16);
            slice.CopyTo(buffer.AsSpan(7, 16));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static MessageType GetMessageType(Span<byte> bytes)
        {
            return (MessageType)bytes[4];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte[] GetPayload(Span<byte> bytes)
        {
            var result = Array.Empty<byte>();
            if (HasPayload(bytes))
            {
                var shift = 23;
                if (HasExchange(bytes))//todo проверить бенчмарком, как работает инлайн
                {
                    shift += bytes[23];
                    shift++;
                }
                if (HasKey(bytes))
                {
                    shift += 3;
                }

                var payloadSize = BitConverter.ToInt32(bytes[shift..]);
                if (payloadSize > 0)
                {
                    result = bytes.Slice(shift + 4, payloadSize).ToArray();
                }
            }
            return result;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetPayloadStart(Span<byte> bytes)
        {
            if (HasPayload(bytes))
            {
                var shift = 27;
                if (HasExchange(bytes))//todo проверить бенчмарком, как работает инлайн
                {
                    shift += bytes[23];
                    shift += 1;
                }
                if (HasKey(bytes))
                {
                    shift += 3;
                }
                return shift;
            }
            return -1;
        }

        internal static byte[] Pack(OutgoingMessage message, Guid id, MessageFlags flags, int count)
        {
            var result = new byte[count];
            result[4] = (byte)MessageType.Common;
            var shift = 5;
            BitConverter.TryWriteBytes(result.AsSpan(shift), (ushort)flags);//2. flags
            shift += 2;
            id.TryWriteBytes(result.AsSpan(shift));//3. id
            shift += 16;
            if ((flags & MessageFlags.HasExchange) == MessageFlags.HasExchange)
            {
                var exchangeBytes = Encoding.UTF8.GetBytes(message.Exchange);
                BitConverter.TryWriteBytes(result.AsSpan(shift), (byte)(message.Exchange.Length));//4. ExchangeNameLength
                shift += 1;
                exchangeBytes.CopyTo(result.AsSpan(shift));//5. ExchangeName
                shift += exchangeBytes.Length;
            }
            if ((flags & MessageFlags.HasRoutingKey) == MessageFlags.HasRoutingKey)//6. RoutingKey
            {
                result[shift] = message.RoutingKey.Part1;
                shift++;
                result[shift] = message.RoutingKey.Part2;
                shift++;
                result[shift] = message.RoutingKey.Part3;
                shift++;
            }
            if ((flags & MessageFlags.HasPayload) == MessageFlags.HasPayload)
            {
                BitConverter.TryWriteBytes(result.AsSpan(shift), message.Payload.Length);//7. PayloadSize
                shift += 4;
                message.Payload.CopyTo(result.AsMemory(shift));//8. Payload
            }
            BitConverter.TryWriteBytes(result.AsSpan(0, 4), result.Length);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasKey(Span<byte> bytes)
        {
            var value = BitConverter.ToUInt16(bytes.Slice(5, 2));
            return (((MessageFlags)value & MessageFlags.HasRoutingKey) == MessageFlags.HasRoutingKey);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasPayload(Span<byte> bytes)
        {
            var value = BitConverter.ToUInt16(bytes.Slice(5, 2));
            return (((MessageFlags)value & MessageFlags.HasPayload) == MessageFlags.HasPayload);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasExchange(Span<byte> bytes)
        {
            var value = BitConverter.ToUInt16(bytes.Slice(5, 2));
            return (((MessageFlags)value & MessageFlags.HasExchange) == MessageFlags.HasExchange);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetExchangeName(Span<byte> bytes)
        {
            var hasExchange = HasExchange(bytes);
            if (!hasExchange)
            {
                throw new ArgumentException("bytes must contains exchange name!");
            }
            return Encoding.UTF8.GetString(bytes.Slice(24, bytes[23]));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RoutingKey GetRoutingKey(Span<byte> bytes)
        {
            var length = bytes[23];
            RoutingKey key;
            if (HasKey(bytes))
            {
                var routingKeyShift = 23 + length + 1;

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
    }
}
