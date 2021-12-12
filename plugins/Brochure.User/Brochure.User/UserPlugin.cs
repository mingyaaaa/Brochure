using System;
using System.IO;
using System.Threading.Tasks;
using Brochure.Core;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Brochure.User.Repository;
using Brochure.User.Services.Imps;
using Brochure.User.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
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
                var fileName = Path.GetFileNameWithoutExtension(this.AssemblyName);
                // ��ȡxml�ļ���
                var xmlFile = $"{fileName}.xml";
                // ��ȡxml�ļ�·��
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "Plugins", fileName, xmlFile);
                // ��ӿ�������ע�ͣ�true��ʾ��ʾ������ע��
                t.IncludeXmlComments(xmlPath, true);
            });
            services.Configure<SwaggerUIOptions>(t =>
            {
                t.SwaggerEndpoint("/swagger/user_v1/swagger.json", "User");
            });
        }
    }
}