using System.Threading.Tasks;
using Brochure.ORM;
using Brochure.ORM.Querys;
using Brochure.User.Entrities;
using Brochure.User.Models;
using Brochure.User.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.User.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route ("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository repository;

        public UserController (IUserRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> AddUser ([FromQuery] UserModel user)
        {
            var entity = user.GetEntrity ();
            var r = await repository.InsetAndGet (entity);
            if (r == null)
                return Problem ("添加错误");
            return new JsonResult (r);
        }

        public async Task<IActionResult> DeleteUser ([FromQuery] string[] userIds)
        {
            var r = await repository.DeleteMany (userIds);
            return new JsonResult (r);
        }

        public async Task<IActionResult> UpdateUser ([FromQuery] string userId, [FromBody] UserModel model)
        {
            var entity = model.GetEntrity ();
            var r = await repository.Update (userId, entity);
            return new JsonResult (r);
        }
    }
}