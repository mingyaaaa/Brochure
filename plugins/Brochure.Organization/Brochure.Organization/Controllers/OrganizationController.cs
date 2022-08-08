using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Abstract.Extensions;
using Brochure.Core.Server.Extensions;
using Brochure.Organization.Abstract.RequestModel;
using Brochure.Organization.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brochure.Organization.Controllers
{
    /// <summary>
    /// The organization controller.
    /// </summary>
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "org_v1")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrgsDal _orgsDal;
        private readonly IObjectFactory _objectFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationController"/> class.
        /// </summary>
        /// <param name="orgsDal">The orgs dal.</param>
        /// <param name="objectFactory">The object factory.</param>
        public OrganizationController(IOrgsDal orgsDal, IObjectFactory objectFactory)
        {
            _orgsDal = orgsDal;
            _objectFactory = objectFactory;
        }

        /// <summary>
        /// Adds the oraginzation.
        /// </summary>
        /// <param name="oraginzation">The oraginzation.</param>
        /// <returns>A Task.</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddOraginzation([FromQuery] ReqAddOrgModel oraginzation)
        {
            var r = await _orgsDal.InsertOrgs(new[] { oraginzation });
            if (r == -1)
                return this.JsonError(500, "添加错误");
            return this.JsonData(r);
        }

        /// <summary>
        /// Deletes the oraginzation.
        /// </summary>
        /// <param name="oraginzationIds">The oraginzation ids.</param>
        /// <returns>A Task.</returns>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteOraginzation([FromQuery] string[] oraginzationIds)
        {
            var r = await _orgsDal.DeleteOrgRtnErrorIds(oraginzationIds);
            return this.JsonData(r);
        }

        /// <summary>
        /// Updates the oraginzation.
        /// </summary>
        /// <param name="oraginzationId">The oraginzation id.</param>
        /// <param name="model">The model.</param>
        /// <returns>A Task.</returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateOraginzation([FromQuery] string oraginzationId, [FromBody] ReqUpdateOrgModel model)
        {
            var record = model.As<IRecord>();
            var r = await _orgsDal.UpdateOrg(oraginzationId, record);
            return this.JsonData(r);
        }
    }
}