using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    public interface IModuleLoader
    {
        void LoadModule(IServiceCollection services, Assembly assembly);
    }
}