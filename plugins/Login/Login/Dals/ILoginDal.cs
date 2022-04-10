using Brochure.Abstract;
using Brochure.Abstract.Models;
using Plugin.Abstract.RequestModel;
using Plugin.Abstract.ResponseModel;
using PluginTemplate.Entrities;

namespace PluginTemplate.Dals
{
    /// <summary>
    /// The login dal.
    /// </summary>
    public interface ILoginDal
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns>A ValueTask.</returns>
        ValueTask<RspLoginModel> AccountLogin(ReqLoginModel reqLoginModel);
    }
}