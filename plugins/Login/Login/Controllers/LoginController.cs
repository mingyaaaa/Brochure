using Brochure.Abstract;
using Brochure.Abstract.Models;
using Brochure.Core.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plugin.Abstract.RequestModel;
using Plugin.Abstract.ResponseModel;
using PluginTemplate.Dals;
using PluginTemplate.Entrities;

namespace PluginTemplate.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Login_v1")]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly ILoginDal _dal;
        private readonly IObjectFactory _objectFactory;

        public LoginController(ILoginDal dal, IObjectFactory objectFactory)
        {
            _dal = dal;
            _objectFactory = objectFactory;
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<RspLoginModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddLogin([FromBody] ReqAddLoginModel user)
        {
            var entiry = _objectFactory.Create<ReqAddLoginModel, LoginEntrity>(user);
            var r = await _dal.InsertAndGet(entiry);
            if (r == null)
                return this.JsonError(500, "添加错误");
            var rsp = _objectFactory.Create<LoginEntrity, RspLoginModel>(r);
            return this.JsonData(rsp);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userIds">The user ids.</param>
        /// <returns>A Task.</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteLogin([FromQuery] string[] userIds)
        {
            var r = await _dal.DeleteLogin(userIds);
            return this.JsonData(r);
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="model">The model.</param>
        /// <returns>A Task.</returns>
        [HttpPatch]
        public async Task<IActionResult> UpdateLogin([FromQuery] string userId, [FromBody] ReqUpdateLoginModel model)
        {
            var record = _objectFactory.Create(model);
            var r = await _dal.UpdateLogin(userId, record);
            return this.JsonData(r);
        }
    }
}