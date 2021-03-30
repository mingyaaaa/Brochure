using Brochure.Core.Server;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using System;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.User
{
    public class StartConfig : IStarupConfigure
    {
        public void Configure(Guid key, IApplicationBuilder builder)
        {
            // 添加Swagger有关中间件
            //builder.IntertMiddle("user_swagger", Guid.Empty, 7, () => builder.UseSwagger());
            //builder.IntertMiddle("user_swaggerUI", Guid.Empty, 8, () => builder.UseSwaggerUI(c =>
            //{
            //    //  c.SwaggerEndpoint("/swagger/user_v1/swagger.json", "User");
            //}));
        }
    }
}
