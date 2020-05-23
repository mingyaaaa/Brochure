using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Brochure.Core;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Brochure.Authority
{
    public class AuthorityPlugin : Plugins
    {
        public AuthorityPlugin (IServiceProvider service) : base (service) { }

        public override Task<bool> StartingAsync (out string errorMsg)
        {
            errorMsg = string.Empty;
            Context.AddAuthentication (t =>
                t.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme).AddJwtBearer (t =>
            {
                t.RequireHttpsMetadata = true;
                t.SaveToken = true;
                t.TokenValidationParameters = new TokenValidationParameters ()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes ("123")),
                    ValidIssuer = "server1",
                    ValidAudience = "client1"
                };
            });
            // Context.AddIdentityServer ().AddDeveloperSigningCredential ()
            //     .AddInMemoryClients (InitMemoryData.GetClients ())
            //     .AddInMemoryApiResources (InitMemoryData.GetApiResources ());
            // Context.AddSingleton<AuthorityService.AuthorityService.AuthorityServiceBase, Services.AuthorityService> ();
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
                AllowedScopes = { "api3" },
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