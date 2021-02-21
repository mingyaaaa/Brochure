using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    public interface IBApplication
    {

        IServiceProvider ServiceProvider { get; }

        IApplicationBuilder Builder { get; }
    }
}