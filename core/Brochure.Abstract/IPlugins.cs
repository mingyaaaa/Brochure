using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Brochure.Abstract
{
    public interface IPlugins
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        IPluginContext Context { get; }

        /// <summary>
        /// 插件唯一健
        /// </summary>
        Guid Key { get; }

        /// <summary>
        /// 插件名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 插件版本
        /// </summary>
        string Version { get; }

        /// <summary>
        /// 插件作者
        /// </summary>
        string Author { get; }

        /// <summary>
        /// 插件程序集名称
        /// </summary>
        string AssemblyName { get; }

        /// <summary>
        /// 插件程序集
        /// </summary>
        Assembly Assembly { get; }

        /// <summary>
        /// 插件加载顺序
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 依赖的插件
        /// </summary>
        List<Guid> DependencesKey { get; }

        /// <summary>
        /// 授权的插件
        /// </summary>
        List<Guid> AuthKey { get; }

        /// <summary>
        /// Configures the service.
        /// </summary>
        /// <param name="services">The services.</param>
        void ConfigureService(IServiceCollection services);

        /// <summary>
        /// 启动插件
        /// </summary>
        /// <returns></returns>
        Task StartAsync();

        /// <summary>
        /// 退出卸载插件
        /// </summary>
        /// <returns></returns>
        Task ExitAsync();

        /// <summary>
        /// 插件加载前执行
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        Task<bool> StartingAsync();

        /// <summary>
        /// 退出插件前执行
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        Task<bool> ExitingAsync();
    }
}