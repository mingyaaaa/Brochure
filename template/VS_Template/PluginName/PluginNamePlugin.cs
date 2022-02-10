using Brochure.Core;
using Brochure.Core.Server;
using Microsoft.Extensions.DependencyInjection;
using PluginTemplate.Dals;
using PluginTemplate.Repositorys;

namespace PluginTemplate
{
    public class $safeprojectname$Plugin : Plugins
    {
        public override void ConfigureService(IServiceCollection services)
        {
            var fileName = Path.GetFileNameWithoutExtension(this.AssemblyName);
            //// 获取xml文件名
            // 获取xml文件路径
            var xmlPath = Path.Combine(AppContext.BaseDirectory, "Plugins", fileName);
            services.ConfigurePluginSwaggerGen("$safeprojectname$", "$safeprojectname$_v1", xmlPath);
            services.AddScoped<I$safeprojectname$Dal, $safeprojectname$Dal>();
            services.AddScoped<I$safeprojectname$Repository, $safeprojectname$Repository>();
        }
    }
}