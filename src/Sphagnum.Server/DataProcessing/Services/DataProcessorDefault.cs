using Sphagnum.Server.DataProcessing.Contracts;
using System.Collections.Concurrent;

namespace Sphagnum.Server.DataProcessing.Services
{
    internal class DataProcessorDefault : IDataProcessor
    {
        private readonly ConcurrentDictionary<string, Func<byte[], Task>> _processors = new();

        public ValueTask PutMessage(ReadOnlySpan<byte> message)
        {
            return ValueTask.CompletedTask;
        }

        public bool RegisterCoprocessor(string key, Func<byte[], Task> func)
        {
            return _processors.TryAdd(key, func);
        }

        public Func<byte[], Task>? UnregisterCoprocessor(string key)
        {
            if (_processors.TryRemove(key, out Func<byte[], Task>? res))
            {
                return res;
            }
            return null;
        }
    }
}
