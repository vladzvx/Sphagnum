using System;

namespace Sphagnum.Common.Utils.Models
{
    [Flags]
    internal enum MessageFlags : ushort
    {
        None = 0,
        HasRoutingKey = 1,
        HasPayload = 2,
        HasExchange = 4,
    }
}
