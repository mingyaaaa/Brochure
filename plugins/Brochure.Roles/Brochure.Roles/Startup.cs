using Brochure.Core.Server;
using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Brochure.Roles.Entrities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Roles
{
    public class Startup : IStarupConfigure
    {
        public void Configure(Guid key, IApplicationBuilder builder)
        {
            InitDb(builder);
        }

        private void InitDb(IApplicationBuilder applicationBuilder)
        {
            var log = applicationBuilder.ApplicationServices.GetService<ILogger<Startup>>();
            try
            {
                using var scope = applicationBuilder.ApplicationServices.CreateScope();
                var dbTable = scope.ServiceProvider.GetService<DbTable>();
                dbTable.CreateTableAsync<RoleEntity>();
                log.LogInformation("创建RoleEntity数据成功");
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
            }
        }
    }
}