using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.Server.Main.Controllers
{
    [Route ("api/v1/[controller]")]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAction1 ()
        {
            return new ContentResult () { Content = "aaa" };
        }
    }
}