namespace Sphagnum.Server.Cluster.Contracts
{
    public interface IDistributor
    {
        ValueTask DistributeData(ReadOnlySpan<byte> data);
    }
}
