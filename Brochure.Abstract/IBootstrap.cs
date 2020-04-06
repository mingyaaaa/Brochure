using System.Threading.Tasks;

namespace Brochure.Abstract
{
    /// <summary>
    /// 程序启动退出接口
    /// </summary>
    public interface IBootstrap
    {
        /// <summary>
        /// 启动程序
        /// </summary>
        /// <returns></returns>
        Task Start();

        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="plugins"></param>
        /// <returns></returns>
        Task Exit(IPlugins[] plugins);
    }
}