using Microsoft.AspNetCore.Mvc;
using Sphagnum.Client;
using Sphagnum.Common.Messaging.Contracts;

namespace Sphagnum.DebugClient.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly ClientDefault _connection;
        private static readonly Task? rec;

        public TestController(ClientDefault connection)
        {
            _connection = connection;
        }

        [HttpGet]
        public string test()
        {
            return "Ok!";
        }


        [HttpGet]
        public async Task Send(int size)
        {
            var payload1 = new byte[size];
            var payload2 = new byte[size];

            for (int i = 0; i < size; i++)
            {
                payload1[i] = 1;
                payload2[i] = 2;
            }
            var t1 = _connection.Publish(new Common.Messaging.Contracts.Messages.Message("test", RoutingKey.Empty, payload1)).AsTask();
            //var t2 = _connection.Publish(new Message("test", RoutingKey.Empty, payload2)).AsTask();
            await Task.WhenAll(t1);
        }
    }
}
