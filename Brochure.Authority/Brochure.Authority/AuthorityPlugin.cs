using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Brochure.Core;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Authority
{
    public class AuthorityPlugin : Plugins
    {
        public AuthorityPlugin(AssemblyLoadContext assemblyContext, IServiceCollection serviceDescriptor) : base(assemblyContext, serviceDescriptor) { }

        public override Task<bool> StartingAsync()
        {
            ServiceDescriptor.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddJwtBearerClientAuthentication();
            ServiceDescriptor.AddSingleton<IStartupFilter, StartFilter>();
            return base.StartingAsync();
        }
    }

    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource ("api1", "我的 API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",

						// 没有交互性用户，使用 clientid/secret 实现认证。
						AllowedGrantTypes = GrantTypes.ClientCredentials,

						// 用于认证的密码
						ClientSecrets = {
                            new Secret ("secret".Sha256 ())
                        },
						// 客户端有权访问的范围（Scopes）
						AllowedScopes = { "api1" }
                }
            };
        }
    }
}