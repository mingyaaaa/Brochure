using System.Runtime.Loader;
using System.Threading.Tasks;
using Brochure.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Authority
{
    public class AuthorityPlugin : Plugins
    {
        public AuthorityPlugin(AssemblyLoadContext assemblyContext, IServiceCollection serviceDescriptor) : base(assemblyContext, serviceDescriptor) { }

        public override Task<bool> StartingAsync(out string errorMsg)
        {
            errorMsg = string.Empty;
            ServiceDescriptor.AddSingleton<AuthorityService.AuthorityService.AuthorityServiceBase, Services.AuthorityService>();
            return Task.FromResult(true);
        }
    }
}