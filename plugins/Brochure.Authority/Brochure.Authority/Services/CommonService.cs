using Brochure.Authority.Models;
using Brochure.Core.PluginsDI;
using Brochure.Roles.Abstract;
using Brochure.User.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Authority.Services
{
    public interface ICommonService
    {
        Task<UserInfoModel?> GetUserInfo(string userId);
    }

    internal class CommonService : ICommonService
    {
        private readonly IScopeService<IUserService> _scopeService;
        private readonly IScopeService<IRolesService> _roleServiceScope;

        public CommonService(IScopeService<IUserService> userScopeService, IScopeService<IRolesService> roleScopeService)
        {
            _scopeService = userScopeService;
            _roleServiceScope = roleScopeService;
        }

        public async Task<UserInfoModel?> GetUserInfo(string userId)
        {
            if (!_scopeService.Value.TryGetTarget(out var service))
                return null;
            var users = await service.GetUsers(new List<string>() { userId });
            var user = users.FirstOrDefault();
            return new UserInfoModel()
            {
                UserId = userId,
                UserName = user?.Name ?? "未知名称",
            };
        }
    }
}