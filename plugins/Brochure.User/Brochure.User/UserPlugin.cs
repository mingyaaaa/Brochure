using System.Threading.Tasks;
using Brochure.Core;
using Brochure.User.Repository;
using Brochure.User.Services;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.User
{
    public class UserPlugin : Plugins
    {
        public UserPlugin (System.IServiceProvider service) : base (service)
        {

        }

        public override Task<bool> StartingAsync (out string errorMsg)
        {
            var pluginService = this.Context.GetPluginContext<PluginServiceCollectionContext> ();
            pluginService.AddScoped<IUserDal, UserDal> ();
            pluginService.AddScoped<IUserRepository, UserRepository> ();
            return base.StartingAsync (out errorMsg);
        }
    }
}