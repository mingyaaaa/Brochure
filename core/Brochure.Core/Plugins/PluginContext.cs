using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brochure.Abstract;
using Brochure.Abstract.PluginDI;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    /// <summary>
    /// 插件上下文 用于存储主程序服务以及 自身服务
    /// </summary>
    public class PluginContext : IPluginContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginContext"/> class.
        /// </summary>
        public PluginContext()
        {
            Services = new ServiceCollection();
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        public IServiceCollection Services { get; }
    }
}