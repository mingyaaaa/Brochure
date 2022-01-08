using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    /// <summary>
    /// The plugin option.
    /// </summary>
    public interface IPluginOption
    {
        /// <summary>
        /// 插件信息
        /// </summary>
        IPlugins Plugin { get; }
    }
}
