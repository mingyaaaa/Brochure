
namespace Brochure.Core
{
    /*
    用于映射界面中显示的字段，建议用string类型方便界面字符信息的格式化 
    */
    public interface IView
    {
        IModel ConverToDataModel();
    }
}