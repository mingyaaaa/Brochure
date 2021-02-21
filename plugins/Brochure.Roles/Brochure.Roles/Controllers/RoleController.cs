using System.Threading.Tasks;
using Brochure.Roles.Models;
using Brochure.Roles.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.Roles.Controllers
{
    [Route ("api/v1/[controller]")]
    public class RoleController : ControllerBase
    {

        private readonly IRoleRepository repository;
        public RoleController (IRoleRepository repository)
        {
            this.repository = repository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddRole ([FromQuery] RoleModel role)
        {
            var entity = role.GetEntrity ();
            var r = await repository.InsetAndGet (entity);
            if (r == null)
                return Problem ("添加错误");
            return new JsonResult (r);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteRole ([FromQuery] string[] roleIds)
        {
            var r = await repository.DeleteMany (roleIds);
            return new JsonResult (r);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateRole ([FromQuery] string roleId, [FromBody] RoleModel model)
        {
            var entity = model.GetEntrity ();
            var r = await repository.Update (roleId, entity);
            return new JsonResult (r);
        }

    }
}