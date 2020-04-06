using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    public interface IPluginOption
    {
        /// <summary>
        /// 插件信息
        /// </summary>
        IPlugins Plugin { get; }
    }
}
