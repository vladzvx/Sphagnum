using Sphagnum.Client;
using Sphagnum.Common.Contracts.Login;
using Sphagnum.Server.Broker.Services;

namespace Sphagnum.Server.FuncTests
{
    public class Tests
    {
        private BrokerDefaultBase? server;
        private ClientDefault? client1;
        [SetUp]
        public async Task Setup()
        {
            var connectionFactory = new ConnectionFactory()
            {
                Hostname = "localhost",
                Port = 8081,
                Login = "root",
                Password = "root",
                UserRights = UserRights.All,
            };
            server = BrokerDefaultBase.Create(connectionFactory);
            await server.StartAsync(connectionFactory.Port);
            client1 = new ClientDefault(connectionFactory);
        }

        [Test]
        public async Task Test1()
        {
            await client1.Publish(new Common.Contracts.Messaging.Messages.OutgoingMessage("111", new byte[3] { 3, 3, 3 }));
        }

        [Test]
        public async Task Test2()
        {
            await client1.Publish(new Common.Contracts.Messaging.Messages.OutgoingMessage("111", new byte[3] { 3, 3, 3 }));
        }
    }
}