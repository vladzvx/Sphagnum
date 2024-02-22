using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sphagnum.Server.Storage.Messages.Contracts
{
    internal interface IMessagesStorage
    {
        ValueTask LogMessage(ReadOnlyMemory<byte> message);
    }
}
