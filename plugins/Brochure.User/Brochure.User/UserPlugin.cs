using System.Threading.Tasks;
using Brochure.Core;
using Brochure.Core.Extenstions;
using Brochure.User.Repository;
using Brochure.User.Services.Imps;
using Brochure.User.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
            Context.Services.ConfigureSwaggerGen(t =>
            {
                t.SwaggerDoc("user_v1", new OpenApiInfo { Title = "User", Version = "user_v1" });
            });
            Context.Services.Configure<SwaggerUIOptions>(t =>
            {
                t.SwaggerEndpoint("/swagger/user_v1/swagger.json", "User");
            });
            return base.StartingAsync(out errorMsg);
        }
    }
}