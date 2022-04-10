using Brochure.Abstract.Models;
using Login;
using Plugin.Abstract.RequestModel;
using Plugin.Abstract.ResponseModel;

namespace PluginTemplate.Dals
{
    /// <summary>
    /// The login dal.
    /// </summary>
    internal class LoginDal : ILoginDal
    {
        private readonly LoginPolicyFacory _loginPolicy;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginDal"/> class.
        /// </summary>
        /// <param name="loginPolicy">The login policy.</param>
        public LoginDal(LoginPolicyFacory loginPolicy)
        {
            _loginPolicy = loginPolicy;
        }

        /// <summary>
        /// Accounts the login.
        /// </summary>
        /// <param name="reqLoginModel">The req login model.</param>
        /// <returns>A ValueTask.</returns>
        public ValueTask<RspLoginModel> AccountLogin(ReqLoginModel reqLoginModel)
        {
            var policy = _loginPolicy.CreatePolicy(reqLoginModel.LoginType);
            if (policy == null)
                throw new NotSupportedException(reqLoginModel.LoginType);
            return policy.Login(reqLoginModel);
        }
    }
}