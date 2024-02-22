using Sphagnum.Common.Contracts.Infrastructure;
using Sphagnum.Common.Contracts.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sphagnum.Common.UnitTests.Services
{
    internal class TestConnectionFactory : ConnectionFactory
    {
        internal override IConnection CreateDefault()
        {
            return new TestConnection();
        }

        internal override Task<IConnection> CreateDefaultConnected()
        {
            return Task.FromResult<IConnection>(new TestConnection());
        }
    }
}
