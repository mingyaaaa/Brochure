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
        private readonly DbContext dbContext;

        public UserController (IUserRepository repository, DbContext dbContext)
        {
            this.repository = repository;
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> AddUser ([FromQuery] UserModel user)
        {
            var entity = user.GetEntrity ();
            var model = await repository.AddAsync (entity);
            return new JsonResult (model);
        }

        public async Task<IActionResult> DeleteUser ([FromQuery] string[] userIds)
        {
            var r = await repository.DeleteUserByUserIdAsync (userIds);
            return new JsonResult (r);
        }

        public async Task<IActionResult> UpdateUser ([FromQuery] string userId, [FromBody] UserModel model)
        {
            var query = new Query<UserEntrity> (this.dbContext.GetDbProvider ()).WhereAnd (t => t.Id == userId);
            var r = await repository.UpdateUserAsync (model, query);
            return new JsonResult (r);
        }
    }
}