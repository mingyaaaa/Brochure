using Brochure.Abstract;
using Brochure.Abstract.Models;
using Brochure.Core.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plugin.Abstract.RequestModel;
using Plugin.Abstract.ResponseModel;
using PluginTemplate.Dals;

namespace PluginTemplate.Controllers
{
    /// <summary>
    /// The login controller.
    /// </summary>
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Login_v1")]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly ILoginDal _dal;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        /// <param name="dal">The dal.</param>
        public LoginController(ILoginDal dal)
        {
            _dal = dal;
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<RspLoginModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AccountLogin([FromBody] ReqLoginModel user)
        {
            var result = await _dal.AccountLogin(user);
            return this.JsonData(result);
        }
    }
}