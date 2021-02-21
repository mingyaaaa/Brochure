using System;
using System.Threading.Tasks;
using Brochure.Orgainzation.Models;
using Brochure.Orgainzation.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.Orgainzation.Controllers
{
    [Route ("api/v1/[controller]")]
    public class OraginzationController : ControllerBase
    {
        private readonly IOraginzationRepository repository;

        public OraginzationController (IOraginzationRepository repository)
        {
            this.repository = repository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddOraginzation ([FromQuery] OraginzationModel oraginzation)
        {
            var entity = oraginzation.GetEntrity ();
            var r = await repository.InsetAndGet (entity);
            if (r == null)
                return Problem ("添加错误");
            return new JsonResult (r);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteOraginzation ([FromQuery] string[] oraginzationIds)
        {
            var r = await repository.DeleteMany (oraginzationIds);
            return new JsonResult (r);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateOraginzation ([FromQuery] string oraginzationId, [FromBody] OraginzationModel model)
        {
            var entity = model.GetEntrity ();
            var r = await repository.Update (oraginzationId, entity);
            return new JsonResult (r);
        }

    }
}