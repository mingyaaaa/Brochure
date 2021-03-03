using System.Threading.Tasks;
using Brochure.Core;
using Brochure.User.Repository;
using Brochure.User.Services.Imps;
using Brochure.User.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
        public UserPlugin(System.IServiceProvider service) : base(service)
        {

        }

        /// <summary>
        /// Startings the async.
        /// </summary>
        /// <param name="errorMsg">The error msg.</param>
        /// <returns>A Task.</returns>
        public override Task<bool> StartingAsync(out string errorMsg)
        {
            var pluginService = this.Context.GetPluginContext<PluginServiceCollectionContext>();
            pluginService.AddScoped<IUserDal, UserDal>();
            pluginService.AddScoped<IUserRepository, UserRepository>();
            return base.StartingAsync(out errorMsg);
        }
    }
}