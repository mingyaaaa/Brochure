using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.User.Abstract.RequestModel;
using Brochure.User.Abstract.ResponseModel;
using Brochure.User.Services.Interfaces;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.User.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "user_v1")]
    [ProducesResponseType(typeof(ActionResult), StatusCodes.Status500InternalServerError)]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IObjectFactory objectFactory;
        private readonly IUserDal userDal;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userDal">The user dal.</param>
        /// <param name="objectFactory">The object factory.</param>
        public UserController(IUserDal userDal, IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
            this.userDal = userDal;
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ReqAddUserModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<RspUserModel>> AddUser([FromBody] ReqAddUserModel user)
        {
            var r = await userDal.InsertUsers(new List<ReqAddUserModel>() { user });
            if (r == null)
                return Problem("添加错误");
            return new JsonResult(r);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userIds">The user ids.</param>
        /// <returns>A Task.</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] string[] userIds)
        {
            var r = await userDal.DeleteUsers(userIds);
            return new JsonResult(r);
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="model">The model.</param>
        /// <returns>A Task.</returns>
        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromQuery] string userId, [FromBody] ReqUpdateUserModel model)
        {
            var record = objectFactory.Create(model);
            var r = await userDal.UpdateUser(userId, record);
            return new JsonResult(r);
        }

        //查询服务
    }
}