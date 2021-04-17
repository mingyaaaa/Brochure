using System;
using System.IO;
using System.Threading.Tasks;
using Brochure.Core;
using Brochure.Core.Extenstions;
using Brochure.ORM.MySql;
using Brochure.User.Repository;
using Brochure.User.Services.Imps;
using Brochure.User.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

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
        /// <param name="service">The service.</param>
        public UserPlugin()
        {

        }

        /// <summary>
        /// Startings the async.
        /// </summary>
        /// <param name="errorMsg">The error msg.</param>
        /// <returns>A Task.</returns>
        public override Task<bool> StartingAsync(out string errorMsg)
        {
            Context.Services.AddScoped<IUserDal, UserDal>();
            Context.Services.AddScoped<IUserRepository, UserRepository>();
            Context.Services.AddMySql(t =>
            {
                var mysqlBuilder = new MySqlConnectionStringBuilder();
                var dbSection = PluginConfiguration.GetSection("db");
                mysqlBuilder.Database = dbSection.GetValue<string>("database");
                mysqlBuilder.UserID = dbSection.GetValue<string>("user");
                mysqlBuilder.Password = dbSection.GetValue<string>("pwd");
                mysqlBuilder.Server = dbSection.GetValue<string>("server");
                t.ConnectionString = mysqlBuilder.ToString();
            });
            Context.Services.ConfigureSwaggerGen(t =>
            {
                t.SwaggerDoc("user_v1", new OpenApiInfo { Title = "User", Version = "user_v1" });
                var fileName = Path.GetFileNameWithoutExtension(this.AssemblyName);
                // 获取xml文件名
                var xmlFile = $"{fileName}.xml";
                // 获取xml文件路径
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "Plugins", fileName, xmlFile);
                // 添加控制器层注释，true表示显示控制器注释
                t.IncludeXmlComments(xmlPath, true);
            });
            Context.Services.Configure<SwaggerUIOptions>(t =>
            {
                t.SwaggerEndpoint("/swagger/user_v1/swagger.json", "User");
            });
            return base.StartingAsync(out errorMsg);
        }
    }
}