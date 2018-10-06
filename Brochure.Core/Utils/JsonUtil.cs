using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Brochure.Core
{
    public class JsonUtil
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
    }
}
