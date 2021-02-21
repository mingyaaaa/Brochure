using System.Collections.Generic;
using Brochure.Abstract;

namespace Brochure.Utils
{
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
    }
}