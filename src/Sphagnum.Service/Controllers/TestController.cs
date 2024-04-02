using Microsoft.AspNetCore.Mvc;

namespace Sphagnum.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get(string text)
        {
            return text;
        }
    }
}
