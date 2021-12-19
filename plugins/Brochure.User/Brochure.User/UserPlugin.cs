using System;
using System.IO;
using Brochure.Core;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Brochure.User.Repository;
using Brochure.User.Services.Imps;
using Brochure.User.Services.Interfaces;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using Swashbuckle.AspNetCore.SwaggerGen;

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

        public override void ConfigureService(IServiceCollection services)
        {
            services.AddScoped<IUserDal, UserDal>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddDbCore(p => p.AddMySql(t =>
            {
                var mysqlBuilder = new MySqlConnectionStringBuilder();
                var dbSection = PluginConfiguration.GetSection("db");
                mysqlBuilder.Database = dbSection.GetValue<string>("database");
                mysqlBuilder.UserID = dbSection.GetValue<string>("user");
                mysqlBuilder.Password = dbSection.GetValue<string>("pwd");
                mysqlBuilder.Server = dbSection.GetValue<string>("server");
                t.ConnectionString = mysqlBuilder.ToString();
            }));
            services.ConfigureSwaggerGen(t =>
            {
                t.SwaggerDoc("user_v1", new OpenApiInfo { Title = "User", Version = "user_v1" });
                t.AddServer(new OpenApiServer()
                {
                    Url = "",
                    Description = "User"
                });
                t.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return controllerAction.ControllerName + "-" + controllerAction.ActionName;
                });

                var fileName = Path.GetFileNameWithoutExtension(this.AssemblyName);
                // 获取xml文件名
                var xmlFile = $"{fileName}.xml";
                // 获取xml文件路径
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "Plugins", fileName, xmlFile);
                if (File.Exists(xmlPath))
                {
                    // 添加控制器层注释，true表示显示控制器注释
                    t.IncludeXmlComments(xmlPath, true);
                }
            });
            services.Configure<Knife4UIOptions>(t =>
            {
                t.SwaggerEndpoint("/user_v1/api-docs", "User");
            });
        }
    }
}