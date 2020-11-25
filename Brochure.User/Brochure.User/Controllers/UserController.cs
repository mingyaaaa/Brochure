using System.Collections.Generic;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Abstract.Models;
using Brochure.User.Entrities;
using Brochure.User.Models;
using Brochure.User.Repository;
using Brochure.User.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.User.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    [Route ("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IObjectFactory objectFactory;
        private readonly IUserDal userDal;

        public UserController (IUserDal userDal, IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
            this.userDal = userDal;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddUser ([FromQuery] UserModel user)
        {
            var r = await userDal.InsertUsers (new List<UserModel> () { user });
            if (r == null)
                return Problem ("添加错误");
            return new JsonResult (r);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser ([FromQuery] string[] userIds)
        {
            var r = await userDal.DeleteUsers (userIds);
            return new JsonResult (r);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser ([FromQuery] string userId, [FromBody] UserModel model)
        {
            var record = objectFactory.Create (model);
            var r = await userDal.UpdateUser (userId, record);
            return new JsonResult (r);
        }

        //查询服务
    }
}