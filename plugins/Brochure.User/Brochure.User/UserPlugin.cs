using System;
using System.IO;
using Brochure.Core;
using Brochure.Core.Server;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Brochure.User.Repository;
using Brochure.User.Services.Imps;
using Brochure.User.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace Brochure.User
{
    /// <summary>
    /// The user plugin.
    /// </summary>
    public class UserPlugin : Plugins
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPlugin"/> class.
        /// </summary>
        public UserPlugin()
        {
        }

        /// <summary>
        /// Configures the service.
        /// </summary>
        /// <param name="services">The services.</param>
        public override void ConfigureService(IServiceCollection services)
        {
            services.AddScoped<IUserDal, UserDal>();
            services.AddScoped<IUserRepository, UserRepository>();
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
            var fileName = Path.GetFileNameWithoutExtension(this.AssemblyName);
            //// 获取xml文件名
            // 获取xml文件路径
            var xmlPath = Path.Combine(AppContext.BaseDirectory, "Plugins", fileName);
            services.ConfigurePluginSwaggerGen("User", "user_v1", xmlPath);
        }
    }
}