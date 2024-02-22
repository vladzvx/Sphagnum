using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sphagnum.Server.Cluster.Contracts
{
    public interface IDistributor
    {
        ValueTask DistributeData(ReadOnlySpan<byte> data);
    }
}
