using Brochure.Core;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Brochure.Roles.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace Brochure.Roles
{
    /// <summary>
    /// The role plugin.
    /// </summary>
    public class RolePlugin : Plugins
    {
        public override void ConfigureService(IServiceCollection services)
        {
            services.AddScoped<IRolesDal, RoleDal>();
            services.AddDbCore(p => p.AddMySql(t =>
            {
                var mysqlBuilder = new MySqlConnectionStringBuilder();
                var dbSection = Configuration.GetSection("db");
                mysqlBuilder.Database = dbSection.GetValue<string>("database");
                mysqlBuilder.UserID = dbSection.GetValue<string>("user");
                mysqlBuilder.Password = dbSection.GetValue<string>("pwd");
                mysqlBuilder.Server = dbSection.GetValue<string>("server");
                t.ConnectionString = mysqlBuilder.ToString();
            }));
        }

        public override void ConfigApplication(IApplicationBuilder serviceProvider)
        {
            base.ConfigApplication(serviceProvider);
            serviceProvider.InitDb().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}