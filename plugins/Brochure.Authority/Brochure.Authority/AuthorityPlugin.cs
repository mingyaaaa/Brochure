using Brochure.Authority.Extensions;
using Brochure.Core;
using Brochure.Core.Server;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

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

        public override void ConfigApplication(IApplicationBuilder application)
        {
            application.IntertMiddle("author-authentication", Key, 100, () => application.UseAuthentication());

            application.IntertMiddle("author-authorization", Key, 101, () => application.UseAuthorization());
            application.InitDb().ConfigureAwait(false).GetAwaiter().GetResult();
            base.ConfigApplication(application);
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
                var dbSection = Configuration.GetSection("db");
                mysqlBuilder.Database = dbSection.GetValue<string>("database");
                mysqlBuilder.UserID = dbSection.GetValue<string>("user");
                mysqlBuilder.Password = dbSection.GetValue<string>("pwd");
                mysqlBuilder.Server = dbSection.GetValue<string>("server");
                p.ConnectionString = mysqlBuilder.ToString();
            }));
        }
    }
}