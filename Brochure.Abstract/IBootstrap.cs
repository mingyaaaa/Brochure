using System.Threading.Tasks;

namespace Brochure.Abstract
{
    public interface IBootstrap
    {
        //加载插件
        Task Start ();

        //退出程序
        Task Exit (IPlugins[] plugins);
    }
}