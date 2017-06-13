
namespace Brochure.Core
{
    /*
    模型接口用于定义数据的传递形式
     */
    public interface IModel
    {
        IView ConverToDataModel();
        IEntrity ConverToDatabase();
    }
}