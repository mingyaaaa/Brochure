using System.Threading.Tasks;
using Brochure.Roles.Models;
using Brochure.Roles.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.Roles.Controllers
{
    /// <summary>
    /// The role controller.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public RoleController(IRoleRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>A Task.</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddRole([FromQuery] RoleModel role)
        {
            var entity = role.GetEntrity();
            var r = await repository.InsertAndGetAsync(entity);
            if (r == null)
                return Problem("添加错误");
            return new JsonResult(r);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleIds">The role ids.</param>
        /// <returns>A Task.</returns>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteRole([FromQuery] string[] roleIds)
        {
            var r = await repository.DeleteManyAsync(roleIds);
            return new JsonResult(r);
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <param name="model">The model.</param>
        /// <returns>A Task.</returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromQuery] string roleId, [FromBody] RoleModel model)
        {
            var entity = model.GetEntrity();
            var r = await repository.UpdateAsync(roleId, entity);
            return new JsonResult(r);
        }
    }
}