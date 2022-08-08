using Brochure.Authority.Entities;
using Brochure.ORM.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Authority.Extensions
{
    internal static class ApplicationBuildExtensions
    {
        public static async Task<bool> InitDb(this IApplicationBuilder application)
        {
            var log = application.ApplicationServices.GetRequiredService<ILogger<AuthorityPlugin>>();
            try
            {
                using var scope = application.ApplicationServices.CreateScope();
                var dbTable = scope.ServiceProvider.GetRequiredService<DbTable>();
                await dbTable.CreateTableAsync<UserRoleEntity>();
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