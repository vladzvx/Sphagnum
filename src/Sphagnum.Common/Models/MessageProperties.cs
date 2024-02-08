using System;

namespace Sphagnum.Common.Models
{
    [Flags]
    internal enum MessageProperties : ushort
    {
        None = 0,
        HasRoutingKey = 1,
        HasPayload = 2,
        HasExchange = 4,
    }
}
