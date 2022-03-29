using Brochure.Core;
using Brochure.Core.Server;
using Microsoft.Extensions.DependencyInjection;
using PluginTemplate.Dals;
using PluginTemplate.Repositorys;

namespace PluginTemplate
{
    public class LoginPlugin : Plugins
    {
        public override void ConfigureService(IServiceCollection services)
        {
            var fileName = Path.GetFileNameWithoutExtension(this.AssemblyName);
            //// 获取xml文件名
            // 获取xml文件路径
            var xmlPath = Path.Combine(AppContext.BaseDirectory, "Plugins", fileName);
            services.ConfigurePluginSwaggerGen("Login", "Login_v1", xmlPath);
            services.AddScoped<ILoginDal, LoginDal>();
            services.AddScoped<ILoginRepository, LoginRepository>();
        }
    }
}