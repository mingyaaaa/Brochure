using System.Text;
using Brochure.Core;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Proto.UserRpc;

namespace Brochure.Authority
{
    /// <summary>
    /// The authority plugin.
    /// </summary>
    public class AuthorityPlugin : Plugins
    {
        /// <summary>
        /// Configures the service.
        /// </summary>
        /// <param name="services">The services.</param>
        public override void ConfigureService(IServiceCollection services)
        {
            services.AddAuthentication(t =>
            {
                t.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                t.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(t =>
            {
            });
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddIdentityCore<Proto.UserRpc.User>().AddRoleStore<Proto.RolesGrpc.Roles>();
            services.AddSingleton<AuthorityService.AuthorityService.AuthorityServiceBase, Services.AuthorityService>();
            AddDb(services);
        }

        /// <summary>
        /// Adds the db.
        /// </summary>
        /// <param name="services">The services.</param>
        private void AddDb(IServiceCollection services)
        {
            services.AddDbCore(t => t.AddMySql(p =>
            {
                var mysqlBuilder = new MySqlConnectionStringBuilder();
                var dbSection = PluginOption.Configuration.GetSection("db");
                mysqlBuilder.Database = dbSection.GetValue<string>("database");
                mysqlBuilder.UserID = dbSection.GetValue<string>("user");
                mysqlBuilder.Password = dbSection.GetValue<string>("pwd");
                mysqlBuilder.Server = dbSection.GetValue<string>("server");
                p.ConnectionString = mysqlBuilder.ToString();
            }));
        }
    }
}