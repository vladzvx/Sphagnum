using Sphagnum.Common.Contracts.Login;
using Sphagnum.Common.Services;

namespace Sphagnum.Common.UnitTests.Services
{
    internal class TestConnectionFactory : ConnectionFactory
    {
        internal override SphagnumConnection CreateDefault(Func<Func<byte[], Task>> messagesProcessorFactory)
        {
            return new SphagnumConnection(() => new TestConnection(), messagesProcessorFactory);
        }

        internal override Task<SphagnumConnection> CreateDefaultConnected(Func<Func<byte[], Task>> messagesProcessorFactory)
        {
            return Task.FromResult(new SphagnumConnection(() => new TestConnection(), messagesProcessorFactory));
        }
    }
}
