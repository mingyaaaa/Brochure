using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    /// <summary>
    /// 模块接口  用于一些功能实现
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Configs the module.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>A Task.</returns>
        Task ConfigModule(IServiceCollection services);
        /// <summary>
        /// Initializations the.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>A Task.</returns>
        Task Initialization(IServiceProvider services);
    }
}