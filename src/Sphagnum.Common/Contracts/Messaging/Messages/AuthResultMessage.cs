using Sphagnum.Common.Utils.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sphagnum.Common.Contracts.Messaging.Messages
{
    internal readonly ref struct AuthResultMessage
    {
        public readonly ReadOnlyMemory<byte> Payload;
    }
}
