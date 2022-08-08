using Brochure.ORM.Database;
using Brochure.Roles.Entrities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Roles.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        internal static async Task<bool> InitDb(this IApplicationBuilder application)
        {
            var log = application.ApplicationServices.GetRequiredService<ILogger<RolePlugin>>();
            try
            {
                using var scope = application.ApplicationServices.CreateScope();
                var dbTable = scope.ServiceProvider.GetRequiredService<DbTable>();
                await dbTable.CreateTableAsync<RoleEntity>();
                log.LogInformation("创建RoleEntity数据成功");
                return true;
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}