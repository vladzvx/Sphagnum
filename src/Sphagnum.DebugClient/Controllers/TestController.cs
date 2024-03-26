using Microsoft.AspNetCore.Mvc;
using Sphagnum.Common.Contracts.Messaging;
using Sphagnum.Common.Contracts.Messaging.Messages;

namespace Sphagnum.DebugClient.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly IMessagingClient _connection;
        private static readonly Task? rec;

        public TestController(IMessagingClient connection)
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
            var t1 = _connection.Publish(new OutgoingMessage("test", payload1)).AsTask();
            var t2 = _connection.Publish(new OutgoingMessage("test", payload2)).AsTask();
            await Task.WhenAll(t1, t2);
        }
    }
}
