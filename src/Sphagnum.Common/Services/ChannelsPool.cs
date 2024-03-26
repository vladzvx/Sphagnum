using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Sphagnum.Common.Services
{
    internal class ChannelsPool
    {
        private readonly int _poolSize;
        public ChannelsPool(int poolSize)
        {
            _poolSize = poolSize;
        }

        private readonly ConcurrentQueue<Channel<byte[]>> channels = new ConcurrentQueue<Channel<byte[]>>();

        public Channel<byte[]> Get()
        {
            if (channels.TryDequeue(out var channel))
            {
                return channel;
            }
            else
            {
                return Channel.CreateBounded<byte[]>(1);
            }
        }

        public void Return(Channel<byte[]> channel)
        {
            if (channels.Count < _poolSize)
            {
                channels.Enqueue(channel);
            }
        }
    }
}
