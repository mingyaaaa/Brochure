using System;
using System.Threading.Tasks;
using Brochure.Abstract.Models;
using Brochure.Authority.Abstract;
using Brochure.Core.PluginsDI;
using Brochure.User.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace Brochure.Authority.Services
{
    /// <summary>
    /// The login service.
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly IAccountService _accountService;
        private readonly IDistributedCache _distributedCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginService"/> class.
        /// </summary>
        public LoginService(IAccountService accountService, IDistributedCache distributedCache)
        {
            _accountService = accountService;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// Logins the.
        /// </summary>
        /// <param name="loginModel">The login model.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<Result<LoginUserModel>> Login(LoginModel loginModel)
        {
            var loginResult = await _accountService.VerifyAccount(loginModel.UseName, loginModel.Passward);
            if (loginResult.Code == 0)
            {
                //todo 查询角色，用户信息，部门信息，等
            }
            return new Result<LoginUserModel>(loginResult.Code, loginResult.Msg);
        }

        /// <summary>
        /// Refreshes the token.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A ValueTask.</returns>
        public ValueTask<Result> RefreshToken(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifies the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>A ValueTask.</returns>
        public ValueTask<Result> VerifyToken(string token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifies the user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A ValueTask.</returns>
        public ValueTask<Result> VerifyUserName(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Changes the to result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>An Result.</returns>
        private Result ChangeToResult(SignInResult result)
        {
            if (result.IsLockedOut)
            {
                return new Result(1, "账户被锁定");
            }
            if (result.IsNotAllowed)
            {
                return new Result(1, "不允许登录");
            }
            else
            {
                return Result.OK;
            }
        }
    }
}