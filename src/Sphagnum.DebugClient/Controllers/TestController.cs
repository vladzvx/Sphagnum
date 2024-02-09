using Microsoft.AspNetCore.Mvc;
using Sphagnum.Abstractions;
using System.Text;

namespace Sphagnum.DebugClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController(ISphagnumClient connection) : ControllerBase
    {
        private readonly ISphagnumClient _connection = connection;

        [HttpGet]
        public async Task Send(string text)
        {
            var payload = Encoding.UTF8.GetBytes(text);
            await _connection.Publish(new Abstractions.Messaging.Models.OutgoingMessage("test", payload));
        }
    }
}
