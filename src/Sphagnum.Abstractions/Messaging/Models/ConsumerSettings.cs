using System;
using System.Threading.Tasks;

namespace Sphagnum.Abstractions.Messaging.Models
{
    public readonly ref struct ConsumerSettings
    {
        public readonly Func<IncommingMessage, Task> RecievingFunc;
        public readonly string TopicName;
        public readonly bool AutoAck;
    }
}
