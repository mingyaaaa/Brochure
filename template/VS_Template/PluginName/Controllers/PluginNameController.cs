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
    [ApiExplorerSettings(GroupName = "$safeprojectname$_v1")]
    [Authorize]
    public class $safeprojectname$Controller : ControllerBase
    {
        private readonly I$safeprojectname$Dal _dal;
        private readonly IObjectFactory _objectFactory;

        public $safeprojectname$Controller(I$safeprojectname$Dal dal, IObjectFactory objectFactory)
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
        [ProducesResponseType(typeof(Result<Rsp$safeprojectname$Model>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add$safeprojectname$([FromBody] ReqAdd$safeprojectname$Model user)
        {
            var entiry = _objectFactory.Create<ReqAdd$safeprojectname$Model, $safeprojectname$Entrity>(user);
            var r = await _dal.InsertAndGet(entiry);
            if (r == null)
                return this.JsonError(500, "添加错误");
            var rsp = _objectFactory.Create<$safeprojectname$Entrity, Rsp$safeprojectname$Model>(r);
            return this.JsonData(rsp);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userIds">The user ids.</param>
        /// <returns>A Task.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete$safeprojectname$([FromQuery] string[] userIds)
        {
            var r = await _dal.Delete$safeprojectname$(userIds);
            return this.JsonData(r);
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="model">The model.</param>
        /// <returns>A Task.</returns>
        [HttpPatch]
        public async Task<IActionResult> Update$safeprojectname$([FromQuery] string userId, [FromBody] ReqUpdate$safeprojectname$Model model)
        {
            var record = _objectFactory.Create(model);
            var r = await _dal.Update$safeprojectname$(userId, record);
            return this.JsonData(r);
        }
    }
}