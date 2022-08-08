using Brochure.Core;
using Brochure.Roles.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Roles
{
    /// <summary>
    /// The role plugin.
    /// </summary>
    public class RolePlugin : Plugins
    {
        public override void ConfigureService(IServiceCollection services)
        {
            services.AddScoped<IRolesDal, RoleDal>();
        }

        public override void ConfigApplication(IApplicationBuilder serviceProvider)
        {
            base.ConfigApplication(serviceProvider);
            serviceProvider.InitDb().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}