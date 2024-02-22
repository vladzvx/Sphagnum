using Sphagnum.Server.Storage.Messages.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sphagnum.Server.Storage.Messages.Services
{
    internal class MessagesStorageDefault : IMessagesStorage
    {
        public ValueTask LogMessage(ReadOnlyMemory<byte> message)
        {
            return ValueTask.CompletedTask;
        }
    }
}
