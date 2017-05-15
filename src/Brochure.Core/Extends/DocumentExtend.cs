using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Brochure.Core.Helper;

namespace Brochure.Core
{
    public static class DocumentExtend
    {
        /// <summary>
        /// 字典中的字段为小写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static T ToEntrityObject<T>(this IDocument doc) where T : IEntrity
        {
            var obj = ObjectHelper.CreateInstance<T>();
            var type = obj.GetType();
            var properties = type.GetRuntimeProperties();
            foreach (var item in properties)
            {
                if (item.CanWrite)
                    item.SetValue(obj, doc[item.Name.ToLower()]);
            }
            return obj;
        }
        public static async Task<T> ToEntrityObjectAsync<T>(this IDocument doc)
        {
            var aa = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(doc));
            var obj = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(aa));
            return obj;
        }

        public static object GetDocumentValue<T>(this IDocument doc, Expression<Func<T, object>> expr)
        {
            var key = ObjectHelper.GetPropertyName(expr);
            return doc[key];
        }

        public static void Merger(this IDictionary<string, object> dic, IDictionary<string, object> dictionary)
        {
            foreach (var item in dictionary)
            {
                if (!dic.Contains(item))
                    dic.Add(item);
            }
        }
    }
}
