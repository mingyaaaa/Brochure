using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Brochure.Abstract.Models;
using Brochure.Authority.Abstract;
using Brochure.Authority.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.Authority.Controllers.V1
{
    /// <summary>
    /// The authority controller.
    /// </summary>
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        private readonly ILoginService loginService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorityController"/> class.
        /// </summary>
        /// <param name="loginService">The login service.</param>
        public AuthorityController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        /// <summary>
        /// Logins the.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return url.</param>
        /// <returns>A Task.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model, [FromQuery] string? returnUrl = null)
        {
            if (HttpContext.User?.Identity?.IsAuthenticated ?? false)
            {
                returnUrl = null;
            }
            var loginResult = await loginService.Login(model);
            SignIn(new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity()), JwtBearerDefaults.AuthenticationScheme);
            return Ok();
        }

        /// <summary>
        /// Logouts the.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            return Ok();
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <returns>An IActionResult.</returns>
        [HttpGet]
        public IActionResult GetAction()
        {
            return new ContentResult() { Content = "aaa" };
        }
    }
}