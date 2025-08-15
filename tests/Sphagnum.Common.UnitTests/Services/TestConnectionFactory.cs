using Sphagnum.Common.Infrastructure.Contracts;
using Sphagnum.Common.Infrastructure.Services;

namespace Sphagnum.Common.UnitTests.Services
{
    internal class TestConnectionFactory : ConnectionFactory
    {
        public IConnection? CurrentConnection { get; private set; }
        internal override Task<IConnection> CreateConnection(bool connected = true)
        {
            return Task.FromResult(CurrentConnection ?? (IConnection)(new TestConnection()));
        }

        public void SetCurrentConnection(IConnection? connection)
        {
            CurrentConnection = connection;
        }
    }
}
