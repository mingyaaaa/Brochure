using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Abstract
{
    /// <summary>
    /// 模块接口  用于一些功能实现
    /// </summary>
    public interface IModule
    {
        Task Initialization (IServiceCollection services);
    }
}