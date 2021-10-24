using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Organization.Abstract.RequestModel;
using Brochure.Organization.DAL.Interfaces;
using Brochure.Organization.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.Organization.Controllers
{
    [Route ("api/v1/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrgsDal _orgsDal;
        private readonly IObjectFactory _objectFactory;

        public OrganizationController (IOrgsDal orgsDal,IObjectFactory objectFactory)
        {
            _orgsDal = orgsDal;
            _objectFactory = objectFactory;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddOraginzation ([FromQuery] ReqAddOrgModel oraginzation)
        {
            var r = await _orgsDal.InsertOrgs (new[] { oraginzation});
            if (r == null)
                return Problem ("添加错误");
            return new JsonResult (r);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteOraginzation ([FromQuery] string[] oraginzationIds)
        {
            var r = await _orgsDal.DeleteOrgRtnErrorIds (oraginzationIds);
            return new JsonResult (r);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateOraginzation ([FromQuery] string oraginzationId, [FromBody] ReqUpdateOrgModel model)
        {
            var record = _objectFactory.Create(model);
            var r = await _orgsDal.UpdateOrg (oraginzationId, record);
            return new JsonResult (r);
        }

    }
}