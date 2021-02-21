using System.Collections;
using System.Collections.Generic;

namespace Brochure.Abstract
{
    public interface IRecord : IEnumerator
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        void Add(string key, object obj);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string key] { get; set; }

        /// <summary>
        /// 获取健的信息
        /// </summary>
        IEnumerable<string> Keys { get; }

        /// <summary>
        /// 获取值的信息
        /// </summary>
        IEnumerable<object> Values { get; }

        /// <summary>
        /// 移除健
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// 是否包含key的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ContainsKey(string key);
    }
}