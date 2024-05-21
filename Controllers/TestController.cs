using Microsoft.AspNetCore.Mvc;

namespace AutoStopAPI.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<int> Get()
        {
            return 1;
        }
    }
}
