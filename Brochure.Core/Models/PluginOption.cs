using System;
using System.Reflection;
using Brochure.Abstract;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Models
{
    public class PluginOption : IPluginOption
    {

        public PluginOption(IPlugins plugins)
        {
            this.Plugin = plugins;
        }

        public IPlugins Plugin { get; }

    }
}