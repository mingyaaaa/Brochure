using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Microsoft.Extensions.Configuration;

namespace Brochure.Utils
{
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
        /// Arrays the json valid.
        /// </summary>
        /// <param name="str">The str.</param>
        /// <returns>A bool.</returns>
        public bool ArrayJsonValid(string str)
        {
            try
            {
                var json = JsonDocument.Parse(str);
                return json.RootElement.ValueKind == JsonValueKind.Array;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Objects the json valid.
        /// </summary>
        /// <param name="str">The str.</param>
        /// <returns>A bool.</returns>
        public bool ObjectJsonValid(string str)
        {
            try
            {
                var json = JsonDocument.Parse(str);
                return json.RootElement.ValueKind == JsonValueKind.Object;
            }
            catch (System.Exception)
            {
                return false;
            }
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
        /// Merges the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="configuration1">The configuration1.</param>
        /// <returns>An IConfiguration.</returns>
        public IConfiguration MergeConfiguration(IConfiguration configuration, IConfiguration configuration1)
        {
            var children = configuration.GetChildren();
            var r = new ConfigurationBuilder();
            foreach (var item in children)
            {
                var section = configuration1.GetSection(item.Key);
                if (section == null)
                    continue;
                var sectionConfiguration = MergeConfiguration(item, section);
                r.AddConfiguration(sectionConfiguration);
            }
            return r.Build();
        }
    }
}