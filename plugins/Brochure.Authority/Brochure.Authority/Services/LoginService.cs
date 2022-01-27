using System;
using System.Threading.Tasks;
using Brochure.Abstract.Models;
using Brochure.Authority.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Brochure.Authority.Services
{
    /// <summary>
    /// The login service.
    /// </summary>
    public class LoginService : ILoginService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginService"/> class.
        /// </summary>
        public LoginService()
        {
        }

        /// <summary>
        /// Logins the.
        /// </summary>
        /// <param name="loginModel">The login model.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<IResult> Login(LoginModel loginModel)
        {
            return Result.OK;
            //   return ChangeToResult(result);
        }

        /// <summary>
        /// Refreshes the token.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A ValueTask.</returns>
        public ValueTask<IResult> RefreshToken(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifies the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>A ValueTask.</returns>
        public ValueTask<IResult> VerifyToken(string token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifies the user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A ValueTask.</returns>
        public ValueTask<IResult> VerifyUserName(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Changes the to result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>An IResult.</returns>
        private IResult ChangeToResult(SignInResult result)
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