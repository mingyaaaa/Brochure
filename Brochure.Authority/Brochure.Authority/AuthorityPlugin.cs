using System.Collections.Generic;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Brochure.Core;
using IdentityServer4.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Authority
{
    public class AuthorityPlugin : Plugins
    {
        public AuthorityPlugin (IServiceCollection serviceDescriptor) : base (serviceDescriptor) { }

        public override Task<bool> StartingAsync (out string errorMsg)
        {
            errorMsg = string.Empty;
            ServiceDescriptor.AddIdentityServer ().AddDeveloperSigningCredential ()
                .AddInMemoryClients (InitMemoryData.GetClients ())
                .AddInMemoryApiResources (InitMemoryData.GetApiResources ());
            ServiceDescriptor.AddSingleton<AuthorityService.AuthorityService.AuthorityServiceBase, Services.AuthorityService> ();
            return Task.FromResult (true);
        }
    }
    public static class InitMemoryData
    {
        public static IEnumerable<Client> GetClients ()
        {
            var result = new List<Client>
            {
                new Client ()
                {
                ClientId = "client1",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret ("123456".Sha256 ()) },
                AllowedScopes = { "api1" },
                },
                new Client ()
                {
                ClientId = "2",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret ("123456".Sha256 ()) },
                AllowedScopes = { "2" },
                },
                new Client ()
                {
                ClientId = "3",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret ("123456".Sha256 ()) },
                AllowedScopes = { "api3" },
                }
            };
            return result;
        }
        public static IEnumerable<ApiResource> GetApiResources ()
        {
            var result = new List<ApiResource> ()
            {
                new ApiResource ("api1"),
                new ApiResource ("api2"),
                new ApiResource ("api3")
            };
            return result;
        }
    }
}