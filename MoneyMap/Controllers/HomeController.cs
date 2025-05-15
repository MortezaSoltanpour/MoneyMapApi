using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MoneyMap.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {

        [HttpGet("GetApiForTest")]
        public async Task<IActionResult> GetApiForTest()
        {
            return Ok();
        }
    }
}