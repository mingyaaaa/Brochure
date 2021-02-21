using System;
using Brochure.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Server
{
    public class BApplication : IBApplication
    {
        public ApplicationPartManager ApplicationPartManager { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IApplicationBuilder Builder { get; set; }
    }
}