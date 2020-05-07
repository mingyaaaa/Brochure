using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.Authority.Controllers.V1
{
    [Route ("api/v1/[controller]")]
    public class AuthorityController : Controller
    {
        public AuthorityController ()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetAction ()
        {
            return new ContentResult () { Content = "aaa" };
        }
    }
}