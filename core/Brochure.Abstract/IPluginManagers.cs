namespace Brochure.Abstract
{
    /// <summary>
    /// The plugin managers.
    /// </summary>
    public interface IPluginManagers
    {
        /// <summary>
        /// 注册插件
        /// </summary>
        /// <param name="plugin"></param>
        void Regist(IPlugins plugin);

        /// <summary>
        /// 移除插件
        /// </summary>
        /// <param name="plugin"></param>
        ValueTask Remove(IPlugins plugin);

        /// <summary>
        /// 移除插件
        /// </summary>
        /// <param name="key"></param>
        ValueTask Remove(Guid key);

        /// <summary>
        /// 获取插件详情
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IPlugins GetPlugin(Guid key);

        /// <summary>
        /// 获取当前插件
        /// </summary>
        /// <returns></returns>
        List<IPlugins> GetPlugins();

        /// <summary>
        /// 判断插件是否存在
        /// </summary>
        /// <returns></returns>
        bool IsExistPlugins(Guid id);

        /// <summary>
        /// 获取插件路径
        /// </summary>
        /// <returns></returns>
        string GetBasePluginsPath();

        /// <summary>
        /// 获取当前插件版本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long GetPluginVersion(Guid key);
    }
}