using System;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Core
{
    public interface IApplication
    {
        IServiceCollection Services { get; }
    }
}
