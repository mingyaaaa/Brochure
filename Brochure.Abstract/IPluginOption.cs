using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    public interface IPluginOption
    {
        IPlugins Plugin { get; }
    }
}
