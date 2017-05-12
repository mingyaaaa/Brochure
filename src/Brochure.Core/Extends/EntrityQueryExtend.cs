using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Brochure.Core.Abstract;
using Brochure.Core.implement;
using Brochure.Core.Query;

namespace Brochure.Core.Extends
{
    public static class EntrityQueryExtend
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// 等于条件（对象中包含条件的值）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entrity">实体对象</param>
        /// <param name="expr">字段名称</param>
        /// <returns></returns>
        public static EntrieyQuery Equal<T>(this T entrity, Expression<Func<T, object>> expr) where T : IEntrity
        {
            var randomstr = Random.Next(0, 9999);
            var tuple = entrity.GetPropertyValueTuple(expr);
            IDocument doc = new RecordDocument();
            var dicStr = tuple.Item1 + randomstr;
            string str = $" {entrity.TableName}.{tuple.Item1} = {ConstString.SqlServerPre + dicStr} ";//拼接字符串
            doc.Add(dicStr, tuple.Item2);//存储字符串参数
            EntrieyQuery entrieyQuery = new EntrieyQuery(str, doc, entrity);
            return entrieyQuery;
        }

        /// <summary>
        /// 等于条件（对象中不包含条件的值）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entrity">实体对象</param>
        /// <param name="expr">字段名称</param>
        /// <param name="value">条件值</param>
        /// <returns></returns>
        public static EntrieyQuery Equal<T>(this T entrity, Expression<Func<T, object>> expr, object value) where T : IEntrity
        {
            var randomstr = Random.Next(0, 9999);//确保参数不会重复
            var property = entrity.GetPropertyName(expr);
            IDocument doc = new RecordDocument();
            var dicStr = property + randomstr;
            string str = $" {entrity.TableName}.{property} = {ConstString.SqlServerPre + dicStr} ";
            doc.Add(dicStr, value);
            EntrieyQuery entrieyQuery = new EntrieyQuery(str, doc, entrity);
            return entrieyQuery;
        }
        /// <summary>
        /// between条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entrity">对象</param>
        /// <param name="expr">字段名称</param>
        /// <param name="min">最大值</param>
        /// <param name="max">最小值</param>
        /// <returns></returns>
        public static EntrieyQuery Between<T>(this T entrity, Expression<Func<T, object>> expr, double min, double max) where T : IEntrity
        {
            var randomstr = Random.Next(0, 9999);
            var property = entrity.GetPropertyName(expr);
            var dicStr = property + randomstr;
            string str = $" {entrity.TableName}.{property} between {ConstString.SqlServerPre + ConstString.Min + dicStr } and {ConstString.SqlServerPre + ConstString.Max + dicStr } ";
            IDocument doc = new RecordDocument();
            doc.Add(ConstString.Min + dicStr, min);
            doc.Add(ConstString.Max + dicStr, max);
            return new EntrieyQuery(str, doc, entrity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entrity"></param>
        /// <param name="expr"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static EntrieyQuery In<T>(this T entrity, Expression<Func<T, object>> expr, params object[] array) where T : IEntrity
        {
            var property = entrity.GetPropertyName(expr);
            string str = $" {entrity.TableName}.{property} in  ";
            List<string> paramlist = new List<string>();
            IDocument doc = new RecordDocument();
            for (int i = 0; i < array.Length; i++)
            {
                var randomstr = Random.Next(0, 9999);
                string tstr = property + randomstr;
                doc.Add(tstr, array[i]);
                paramlist.Add(ConstString.SqlServerPre + tstr);
            }
            str = string.Format(str + " [{0}] ", paramlist.ToString(","));
            return new EntrieyQuery(str, doc, entrity);
        }
        /// <summary>
        /// 获取更新的参数和值（该对象中包含要更新的值）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entrity">实体对象</param>
        /// <param name="expr">对象字段</param>
        /// <returns></returns>
        public static BaseBuild UpdateParam<T>(this T entrity, params Expression<Func<T, object>>[] expr) where T : IEntrity
        {
            var doc = new RecordDocument();
            foreach (var item in expr)
            {
                var tuple = entrity.GetPropertyValueTuple(item);
                doc.Add(tuple.Item1, tuple.Item2);
            }
            return new EntrieyQuery("", doc, entrity);
        }
        /// <summary>
        /// 获取查询参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entrity">实体对象</param>
        /// <param name="expr">对象字段</param>
        /// <returns></returns>
        public static List<string> SelectParam<T>(this T entrity, params Expression<Func<T, object>>[] expr) where T : IEntrity
        {
            List<string> result = new List<string>();
            foreach (var item in expr)
            {
                var tuple = entrity.GetPropertyValueTuple(item);
                result.Add($"{entrity}.{tuple.Item1}");
            }
            return result;
        }
        /// <summary>
        /// 获取单个字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entrity">对象</param>
        /// <param name="expr">名称</param>
        /// <returns></returns>
        public static string Param<T>(this T entrity, Expression<Func<T, object>> expr) where T : IEntrity
        {
            var property = entrity.GetPropertyName(expr);
            return $"{entrity.TableName}.{property}";
        }
    }
}
