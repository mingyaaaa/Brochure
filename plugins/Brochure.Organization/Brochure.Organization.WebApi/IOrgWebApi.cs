using Brochure.Abstract;
using Brochure.Organization.Abstract.RequestModel;
using Brochure.Organization.Abstract.ResponesModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;

namespace Brochure.Organization.WebApi
{
    /// <summary>
    /// The org web api.
    /// </summary>
    public interface IOrgWebApi
    {
        /// <summary>
        /// Gets the org model.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        Task<RspOrgModel> GetOrgModel(string id);

        /// <summary>
        /// Lists the.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<RspOrgModel>> List();

        /// <summary>
        /// Adds the org model.
        /// </summary>
        /// <param name="reqAddOrgModel">The req add org model.</param>
        /// <returns>A Task.</returns>
        Task<RspOrgModel> AddOrgModel(ReqAddOrgModel reqAddOrgModel);

        /// <summary>
        /// Deletes the org model.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>A Task.</returns>
        Task<int> DeleteOrgModel(IEnumerable<string> ids);

        /// <summary>
        /// Updates the org.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="updateOrgModel">The update org model.</param>
        /// <returns>A Task.</returns>
        Task<RspOrgModel> UpdateOrg(string id,ReqUpdateOrgModel updateOrgModel);

    }
}
