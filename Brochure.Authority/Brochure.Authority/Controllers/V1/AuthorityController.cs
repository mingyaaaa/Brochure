using System;
using System.Net;
using System.Threading.Tasks;
using Brochure.Authority.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.Authority.Controllers.V1
{
    [Route ("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        private readonly AuthorityService.AuthorityService.AuthorityServiceBase serviceBase;

        public AuthorityController (AuthorityService.AuthorityService.AuthorityServiceBase serviceBase)
        {
            this.serviceBase = serviceBase;
        }

        [HttpPost ("login")]
        [AllowAnonymous]
        public IActionResult Login (LoginModel model)
        {
            return Ok ();
        }

        [HttpGet]
        public IActionResult GetAction ()
        {
            return new ContentResult () { Content = "aaa" };
        }
    }
}