using Brochure.Abstract;
using Brochure.Abstract.Models;
using Brochure.Core.Server.Extensions;
using Brochure.User.Abstract.RequestModel;
using Brochure.User.Abstract.ResponseModel;
using Brochure.User.Entrities;
using Brochure.User.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Brochure.User.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "user_v1")]
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
        [ProducesResponseType(typeof(Result<RspUserModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddUser([FromBody] ReqAddUserModel user)
        {
            var entiry = objectFactory.Create<ReqAddUserModel, UserEntrity>(user);
            var r = await userDal.InsertAndGet(entiry);
            if (r == null)
                return this.JsonError(500, "添加错误");
            var rsp = objectFactory.Create<UserEntrity, RspUserModel>(r);
            return this.JsonData(rsp);
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
            return this.JsonData(r);
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
            return this.JsonData(r);
        }

        //查询服务
    }
}