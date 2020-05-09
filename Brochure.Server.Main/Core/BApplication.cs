using System;
using Brochure.Core;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Server.Main.Core
{
    public class BApplication : IBApplication
    {
        public IServiceCollection Services { get; set; }

        public ApplicationPartManager ApplicationPartManager { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

    }
}