using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace Brochure.Core
{
    public static class JsonUtil
    {
        /// <summary>
        /// 读取Json文本 文件不存在返回null
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static IRecord ReadJson(string filePath)
        {
            if (!File.Exists(filePath))
                return null;
            var jsonStr = File.ReadAllText(filePath);
            var record = JsonConvert.DeserializeObject<IRecord>(jsonStr);
            return record;
        }

        public static List<IRecord> ReadArrayJson(string filePath)
        {
            if (!File.Exists(filePath))
                return null;
            var jsonStr = File.ReadAllText(filePath);
            var record = JsonConvert.DeserializeObject<List<IRecord>>(jsonStr);
            return record;
        }

        public static bool ArrayJsonValid(string str)
        {
            try
            {
                JArray.Parse(str);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public static bool ObjectJsonValid(string str)
        {
            try
            {
                JToken.Parse(str);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
