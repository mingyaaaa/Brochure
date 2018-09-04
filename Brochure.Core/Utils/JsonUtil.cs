using Newtonsoft.Json;
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
    }
}
