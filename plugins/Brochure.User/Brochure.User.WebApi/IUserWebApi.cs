using Brochure.User.Abstract.RequestModel;
using Brochure.User.Abstract.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;

namespace Brochure.User.WebApi
{
    /// <summary>
    /// The user web api.
    /// </summary>
    public interface IUserWebApi
    {
        /// <summary>
        /// Adds the.
        /// </summary>
        /// <param name="reqUserModel">The req user model.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        Task<RspUserModel> Add(ReqAddUserModel reqUserModel);

        /// <summary>
        /// Deletes the.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A Task.</returns>
        [HttpDelete]
        Task<int> Delete(IEnumerable<string> userId);

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="model">The model.</param>
        /// <returns>A Task.</returns>
        [HttpPatch]
        Task<int> Update(string userId, ReqUpdateUserModel model);
    }
}
