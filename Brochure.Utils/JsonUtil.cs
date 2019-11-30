using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Brochure.Abstract;
namespace Brochure.Utils
{
    public class JsonUtil : IJsonUtil
    {
        /// <summary>
        /// 读取Json文本 文件不存在返回null
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IRecord ReadJsonFile (string filePath)
        {
            if (!File.Exists (filePath))
                return null;
            var jsonStr = File.ReadAllText (filePath);
            var record = JsonSerializer.Deserialize<IRecord> (jsonStr);
            return record;
        }

        public bool ArrayJsonValid (string str)
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

        public bool ObjectJsonValid (string str)
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

        public T ConverToJson<T> (string str)
        {
            return JsonSerializer.Deserialize<T> (str);
        }

        public string ConverToJsonString (object o)
        {
            return JsonSerializer.Serialize (o);
        }

        public T ReadJsonFile<T> (string filePath)
        {
            if (!File.Exists (filePath))
                return (T) (object) null;
            var jsonStr = File.ReadAllText (filePath);
            return JsonSerializer.Deserialize<T> (jsonStr);
        }

        public T ConverToObject<T> (string str)
        {
            return JsonSerializer.Deserialize<T> (str);
        }

        public string ConverToString (object obj)
        {
            return JsonSerializer.Serialize (obj);
        }
    }
}