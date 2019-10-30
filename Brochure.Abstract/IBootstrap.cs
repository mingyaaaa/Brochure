namespace Brochure.Abstract
{
    public interface IBootstrap
    {
        //加载插件
        void Start ();

        //退出程序
        void Exit (IPlugins[] plugins);
    }
}