using System;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    public interface IBApplication
    {
        IServiceCollection Services { get; }

        IServiceProvider ServiceProvider { get; }
    }
}