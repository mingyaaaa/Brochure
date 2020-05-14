using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    public interface IPluginManagers
    {
        /// <summary>
        /// 解析插件
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        Task ResolverPlugins (IServiceCollection services);

        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="service"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<IPlugins> LoadPlugin (IServiceProvider service, string path);
        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="service"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<IPlugins> LoadPlugin (IServiceCollection service, string path);
        /// <summary>
        /// 注册插件
        /// </summary>
        /// <param name="plugin"></param>
        void Regist (IPlugins plugin);

        /// <summary>
        /// 卸载插件程序集
        /// </summary>
        /// <param name="plugin"></param>
        Task UnLoadPlugin (IPlugins plugin);

        /// <summary>
        /// 移除插件
        /// </summary>
        /// <param name="plugin"></param>
        Task Remove (IPlugins plugin);

        /// <summary>
        /// 获取插件详情
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IPlugins GetPlugin (Guid key);

        /// <summary>
        /// 获取当前插件
        /// </summary>
        /// <returns></returns>
        List<IPlugins> GetPlugins ();

        /// <summary>
        /// 判断插件是否存在
        /// </summary>
        /// <returns></returns>
        bool IsExistPlugins (Guid id);

        /// <summary>
        /// 获取插件路径
        /// </summary>
        /// <returns></returns>
        string GetBasePluginsPath ();

        /// <summary>
        /// 获取当前插件版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        long GetPluginVersion (Guid key);

    }
}