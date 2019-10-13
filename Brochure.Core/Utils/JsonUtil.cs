using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Brochure.Core
{
    public static class JsonUtil
    {
        /// <summary>
        /// 读取Json文本 文件不存在返回null
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static IRecord ReadJsonFile (string filePath)
        {
            if (!File.Exists (filePath))
                return null;
            var jsonStr = File.ReadAllText (filePath);
            var record = JsonSerializer.Deserialize<IRecord> (jsonStr);
            return record;
        }

        public static List<IRecord> ReadArrayJsonFile (string filePath)
        {
            if (!File.Exists (filePath))
                return null;
            var jsonStr = File.ReadAllText (filePath);
            var record = JsonSerializer.Deserialize<List<IRecord>> (jsonStr);
            return record;
        }

        public static bool ArrayJsonValid (string str)
        {
            try
            {
                var json = JsonDocument.Parse (str);
                return json.RootElement.ValueKind == JsonValueKind.Array;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public static bool ObjectJsonValid (string str)
        {
            try
            {
                var json = JsonDocument.Parse (str);
                return json.RootElement.ValueKind == JsonValueKind.Object;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public static T ConverToJson<T> (string str)
        {
            return JsonSerializer.Deserialize<T> (str);
        }

        public static string ConverToJsonString (object o)
        {
            return JsonSerializer.Serialize (o);
        }
    }
}