using AutoFixture;
using Sphagnum.Client;
using Sphagnum.Common.Messaging.Contracts;
using Sphagnum.Common.UnitTests.Services;
using Sphagnum.Server.Broker.Services;

namespace Sphagnum.FuncTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var fixture = new Fixture();
            var factory = new TestConnectionFactory();

            var manager = new ConnectionsManager();
            var processor = new MessagesProcessor();
            var mainConnection = await factory.CreateConnection(false) as TestConnection;
            Assert.IsNotNull(mainConnection);
            factory.SetCurrentConnection(mainConnection);


            var server = new ConnectionsReciever(factory, manager, processor);
            await server.StartAsync(CancellationToken.None);

            factory.SetCurrentConnection(null);
            var connection1 = await factory.CreateConnection(true) as TestConnection;
            Assert.IsNotNull(connection1);

            factory.SetCurrentConnection(connection1);

            var client = new ClientDefault(factory);
            await mainConnection.AddInputConnection(connection1);
            var data = fixture.CreateMany<byte>(11).ToArray();

            await Task.Delay(100);
            await client.Publish(new Common.Messaging.Contracts.Messages.Message("default", RoutingKey.Empty, data));
            Assert.Pass();
            await Task.Delay(10000);
        }
    }
}