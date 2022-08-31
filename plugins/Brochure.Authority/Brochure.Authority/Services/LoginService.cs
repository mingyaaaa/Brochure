using System;
using System.Text.Json;
using System.Threading.Tasks;
using Brochure.Abstract.Models;
using Brochure.Abstract.Utils;
using Brochure.Authority.Abstract;
using Brochure.Authority.Models;
using Brochure.Core.PluginsDI;
using Brochure.Extensions;
using Brochure.Roles.Abstract;
using Brochure.User.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;

namespace Brochure.Authority.Services
{
    /// <summary>
    /// The login service.
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly IAccountService _accountService;
        private readonly IDistributedCache _distributedCache;
        private readonly IScopeService<IUserService> _userServiceScope;
        private readonly IScopeService<IRolesService> _roleServiceScope;
        private readonly IJsonUtil _jsonUtil;
        private readonly IOptions<AuthConfig> _options;

        /// <summary>
        /// The login service key.
        /// </summary>
        private const string LoginServiceKey = "auth";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginService"/> class.
        /// </summary>
        public LoginService(IAccountService accountService,
            IDistributedCache distributedCache,
            IScopeService<IUserService> userServiceScope,
            IScopeService<IRolesService> roleServiceScope,
            IJsonUtil jsonUtil,
            IOptions<AuthConfig> options)
        {
            _accountService = accountService;
            _distributedCache = distributedCache;
            _userServiceScope = userServiceScope;
            _roleServiceScope = roleServiceScope;
            _jsonUtil = jsonUtil;
            _options = options;
        }

        /// <summary>
        /// Logins the.
        /// </summary>
        /// <param name="loginModel">The login model.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<Result<LoginUserModel>> Login(LoginModel loginModel)
        {
            var loginInfoResult = await GetFailLoginInfo(loginModel.UseName);
            if (loginInfoResult.IsSuccess && loginInfoResult.Data != null)
            {
                var loginInfo = loginInfoResult.Data;
                if (loginInfo.FailCount > _options.Value.LoginErrorCount)
                {
                    return new Result<LoginUserModel>((int)ErrorCode.UserLock, ErrorCode.UserLock.GetDescript());
                }
            }
            var userModel = new LoginUserModel();
            var loginResult = await _accountService.VerifyAccount(loginModel.UseName, loginModel.Passward);
            if (loginResult.Code == 0)
            {
                //查询用户信息
                if (_userServiceScope.Value.TryGetTarget(out var userService))
                {
                    var user = await userService.GetUser(loginResult.Data);
                    userModel.UserId = user?.UserId;
                    userModel.UserName = user?.Name;
                }
                //查询角色信息
                if (_roleServiceScope.Value.TryGetTarget(out var rolesService))
                {
                    var roles = await rolesService.GetRolesByUserId(loginResult.Data);
                    userModel.Roles = roles.Select(t => new AccountRoleModel() { RoleId = t.RoleId, RoleName = t.RoleName });
                }
                //todo 查询部门信息
                var key = $"{LoginServiceKey}:userid:{userModel.UserId}";
                await _distributedCache.SetStringAsync(key, _jsonUtil.ConverToString(userModel), new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMilliseconds(_options.Value.LoginExpireTime) });

                //清除统计数据
                await ClearFailLoginInfo(loginModel.UseName);
            }
            else
            {
                await UpdateLoginFailCount(loginModel.UseName);
                userModel = null;
            }
            return new Result<LoginUserModel>(userModel, loginResult.Code, loginResult.Msg);
        }

        private async Task UpdateLoginFailCount(string userName)
        {
            var key = $"{LoginServiceKey}:username:{userName}";
            var str = await _distributedCache.GetStringAsync(key);
            var loginInfo = _jsonUtil.ConverToObject<LoginInfo>(str);
            loginInfo.FailCount++;
            await _distributedCache.SetStringAsync(key, _jsonUtil.ConverToString(loginInfo), new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.Value.LoginLockTime) });
        }

        public async ValueTask<Result<LoginInfo>> GetFailLoginInfo(string userName)
        {
            var key = $"{LoginServiceKey}:username:{userName}";
            var str = await _distributedCache.GetStringAsync(key);
            var loginInfo = _jsonUtil.ConverToObject<LoginInfo>(str);
            return new Result<LoginInfo>(loginInfo);
        }

        private async ValueTask ClearFailLoginInfo(string userName)
        {
            await _distributedCache.RemoveAsync(userName);
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