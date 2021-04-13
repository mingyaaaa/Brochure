using Microsoft.Extensions.Configuration;

namespace Brochure.Abstract.Utils
{
    /// <summary>
    /// The json util.
    /// </summary>
    public interface IJsonUtil
    {
        /// <summary>
        /// 读文件 转化为对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ReadJsonFile<T>(string filePath);

        /// <summary>
        ///  判断是否是json数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        bool ArrayJsonValid(string str);

        /// <summary>
        /// 判断是否是json对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        bool ObjectJsonValid(string str);

        /// <summary>
        /// json对象转化为 对象
        /// </summary>
        /// <param name="str"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ConverToObject<T>(string str);

        /// <summary>
        /// 对象转化为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string ConverToString(object obj);

        /// <summary>
        /// 将json转化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        T Get<T>(string path);

        /// <summary>
        /// Merges the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="configuration1">The configuration1.</param>
        /// <returns>An IConfiguration.</returns>
        IConfiguration MergeConfiguration(IConfiguration configuration, IConfiguration configuration1);
    }
}