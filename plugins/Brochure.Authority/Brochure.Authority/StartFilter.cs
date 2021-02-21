using System;
using Brochure.Core.Server;
using Microsoft.AspNetCore.Builder;

namespace Brochure.Authority
{
    public class StartConfig : IStarupConfigure
    {
        public void Configure (Guid guid, IApplicationBuilder app)
        {
            // app.UseIdentityServer ();
            app.IntertMiddle ("author-authentication", guid, 100, () => app.UseAuthentication ());

            app.IntertMiddle ("author-authorization", guid, 101, () => app.UseAuthorization ());

        }
    }
}