using Brochure.Core;
using Brochure.Core.Server;
using Login;
using Login.LoginPolicys;
using Microsoft.Extensions.DependencyInjection;
using PluginTemplate.Dals;

namespace PluginTemplate
{
    /// <summary>
    /// The login plugin.
    /// </summary>
    public class LoginPlugin : Plugins
    {
        /// <summary>
        /// Configures the service.
        /// </summary>
        /// <param name="services">The services.</param>
        public override void ConfigureService(IServiceCollection services)
        {
            var fileName = Path.GetFileNameWithoutExtension(this.AssemblyName);
            //// 获取xml文件名
            // 获取xml文件路径
            var xmlPath = Path.Combine(AppContext.BaseDirectory, "Plugins", fileName);
            services.ConfigurePluginSwaggerGen("Login", "Login_v1", xmlPath);
            services.AddScoped<ILoginDal, LoginDal>();
            services.AddScoped<LoginPolicyFacory>();
            services.AddSingleton<ILoginPolicy, UserPasswordPolicy>();
        }
    }
}