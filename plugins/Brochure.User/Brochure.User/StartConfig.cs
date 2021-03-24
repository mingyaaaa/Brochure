using Brochure.Core.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.User
{
    public class StartConfig : IStarupConfigure
    {
        public void Configure(Guid key, IApplicationBuilder builder)
        {
            var a = builder.ApplicationServices.GetService<IConfigureOptions<SwaggerGenOptions>>();
            builder.IntertMiddle("user_doc", key, 8, () => builder.UseSwagger());
            builder.IntertMiddle("user_doc", key, 8, () => builder.UseSwaggerUI(t =>
            {
                t.SwaggerEndpoint("/swagger/user_v1/swagger.json", "User");
            }));
        }
    }
}
