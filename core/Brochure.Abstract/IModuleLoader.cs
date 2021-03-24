using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    public interface IModuleLoader
    {
        void LoadModule(IServiceProvider provider, IServiceCollection services, Assembly assembly);
    }
}