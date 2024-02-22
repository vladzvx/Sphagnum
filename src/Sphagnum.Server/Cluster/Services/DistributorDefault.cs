using Sphagnum.Server.Cluster.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
