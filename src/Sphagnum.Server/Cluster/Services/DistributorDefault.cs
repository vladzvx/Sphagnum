using Sphagnum.Server.Cluster.Contracts;

namespace Sphagnum.Server.Cluster.Services
{
    internal class DistributorDefault : IDistributor
    {
        public ValueTask DistributeData(ReadOnlySpan<byte> data)
        {
            return ValueTask.CompletedTask;
        }
    }
}
