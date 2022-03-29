using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Microsoft.Extensions.Configuration;

namespace Brochure.Utils
{
    /// <summary>
    /// The json util.
    /// </summary>
    public class JsonUtil : IJsonUtil
    {
        /// <summary>
        /// 读取Json文本 文件不存在返回null
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IRecord ReadJsonFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;
            var jsonStr = File.ReadAllText(filePath);
            var record = JsonSerializer.Deserialize<IRecord>(jsonStr);
            return record;
        }

        /// <summary>
        /// Arrays the json valid. 只是判断是否是[] 暂不能通过该方法校验Json格式
        /// </summary>
        /// <param name="str">The str.</param>
        /// <returns>A bool.</returns>
        public bool ArrayJsonValid(string str)
        {
            if (str == null)
                return false;
            return str.StartsWith("[") && str.EndsWith("]");
        }

        /// <summary>
        /// Objects the json valid.只是判断是否是{} ，暂不能通过该方法校验Json格式
        /// </summary>
        /// <param name="str">The str.</param>
        /// <returns>A bool.</returns>
        public bool ObjectJsonValid(string str)
        {
            if (str == null)
                return false;
            return str.StartsWith("{") && str.EndsWith("}");
        }

        /// <summary>
        /// Convers the to json.
        /// </summary>
        /// <param name="str">The str.</param>
        /// <returns>A T.</returns>
        public T ConverToJson<T>(string str)
        {
            return JsonSerializer.Deserialize<T>(str);
        }

        /// <summary>
        /// Convers the to json string.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns>A string.</returns>
        public string ConverToJsonString(object o)
        {
            return JsonSerializer.Serialize(o);
        }

        /// <summary>
        /// Reads the json file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>A T.</returns>
        public T ReadJsonFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return (T)(object)null;
            var jsonStr = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonStr);
        }

        /// <summary>
        /// Convers the to object.
        /// </summary>
        /// <param name="str">The str.</param>
        /// <returns>A T.</returns>
        public T ConverToObject<T>(string str)
        {
            return JsonSerializer.Deserialize<T>(str);
        }

        /// <summary>
        /// Convers the to string.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A string.</returns>
        public string ConverToString(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        /// <summary>
        /// Gets the.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A T.</returns>
        public T Get<T>(string path)
        {
            return new ConfigurationBuilder().AddJsonFile(path).Build().Get<T>();
        }

        /// <summary>
        /// Are the json. 此方法 性能较差 不建议频繁使用
        /// </summary>
        /// <param name="str">The str.</param>
        /// <returns>A bool.</returns>
        public bool IsJson(string str)
        {
            try
            {
                JsonDocument.Parse(str);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}