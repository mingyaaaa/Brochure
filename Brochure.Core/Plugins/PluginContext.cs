using System;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    /// <summary>
    /// 插件上下文 用于存储主程序服务以及 自身服务
    /// </summary>
    public class PluginContext : ServiceCollection
    {
        public PluginContext (IServiceProvider mainServiceProvider)
        {
            MainService = mainServiceProvider;
        }
        public IServiceProvider MainService { get; }
    }
}