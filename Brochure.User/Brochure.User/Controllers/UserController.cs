using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.ORM;
using Brochure.ORM.Querys;
using Brochure.User.Entrities;
using Brochure.User.Models;
using Brochure.User.Repository;
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
        private readonly IUserRepository repository;
        private readonly IObjectFactory objectFactory;

        public UserController (IUserRepository repository, IObjectFactory objectFactory)
        {
            this.repository = repository;
            this.objectFactory = objectFactory;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddUser ([FromQuery] UserModel user)
        {
            var entity = objectFactory.Create<UserModel, UserEntrity> (user);
            var r = await repository.InsetAndGet (entity);
            if (r == null)
                return Problem ("添加错误");
            return new JsonResult (r);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser ([FromQuery] string[] userIds)
        {
            var r = await repository.DeleteMany (userIds);
            return new JsonResult (r);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser ([FromQuery] string userId, [FromBody] UserModel model)
        {
            var entity = objectFactory.Create<UserModel, UserEntrity> (model);
            var r = await repository.Update (userId, entity);
            return new JsonResult (r);
        }

        //查询服务
    }
}