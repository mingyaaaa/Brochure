using System;
using System.Net;
using System.Threading.Tasks;
using Brochure.Authority.Models;
using Brochure.Authority.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.Authority.Controllers.V1
{
    [Route ("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        private readonly ILoginService loginService;

        public AuthorityController (ILoginService loginService)
        {
            this.loginService = loginService;
        }

        [HttpPost ("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login (LoginModel model)
        {
            var loginResult = await loginService.Login (model);
            return Ok ();
        }

        [HttpGet]
        public IActionResult GetAction ()
        {
            return new ContentResult () { Content = "aaa" };
        }
    }
}