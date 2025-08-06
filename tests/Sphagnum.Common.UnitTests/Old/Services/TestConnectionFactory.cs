//using Sphagnum.Common.Old.Contracts.Login;
//using Sphagnum.Common.Old.Services;

//namespace Sphagnum.Common.UnitTests.Old.Services
//{
//    internal class TestConnectionFactory : ConnectionFactory
//    {
//        internal override SphagnumConnectionOld CreateDefault(Func<Func<byte[], Task>> messagesProcessorFactory)
//        {
//            return new SphagnumConnectionOld(() => new TestConnection(), messagesProcessorFactory);
//        }

//        internal override Task<SphagnumConnectionOld> CreateDefaultConnected(Func<Func<byte[], Task>> messagesProcessorFactory)
//        {
//            return Task.FromResult(new SphagnumConnectionOld(() => new TestConnection(), messagesProcessorFactory));
//        }
//    }
//}
