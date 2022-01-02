using Brochure.Core;
using Brochure.Core.Server;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Brochure.Organization
{
    public class OrganizationPlugin : Plugins
    {
        public OrganizationPlugin()
        {
        }

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
            services.ConfigurePluginSwaggerGen("Organization", "org_v1", xmlPath);
        }
    }
}