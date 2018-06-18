using System.Collections;
using System.Collections.Generic;
using Brochure.Core.Extends;
using System;
using Brochure.Core.Interfaces;

namespace Brochure.Core.Implements
{
    /// <summary>
    /// 文档类型
    /// </summary>
    /// <typeparam name="BDocument"></typeparam>
    public class BDocument : IBDocument, IBConverables<string>
    {
        private readonly IDictionary<string, object> _dic;
        /// <summary>
        /// 添加时执行
        /// </summary>
        public Action AddHander;
        /// <summary>
        /// 修改数据时执行
        /// </summary>
        public Action UpdateHander;
        /// <summary>
        /// 移除数据是执行
        /// </summary>
        public Action RemoveHander;
        public BDocument(object obj)
        {
            _dic = obj.AsDictionary();
        }
        public BDocument()
        {
            _dic = new Dictionary<string, object>();
        }
        /// <summary>
        /// 获取集合的个数
        /// </summary>
        /// <returns></returns>
        public int Count { get { return _dic.Count; } }
        /// <summary>
        /// 获取Key的集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Keys { get { return _dic.Keys; } }
        /// <summary>
        /// 获取Value的集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> Values { get { return _dic.Values; } }

        public object Current { get; set; }

        /// <summary>
        /// 获取或设置值
        /// </summary>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                if (!_dic.ContainsKey(key))
                    return null;
                return _dic[key];
            }
            set
            {
                var isAdd = false;
                if (!_dic.ContainsKey(key))
                    isAdd = true;
                _dic[key] = value;
                if (isAdd)
                    AddHander?.Invoke();
                else
                    UpdateHander?.Invoke();
            }
        }
        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void Add(string key, object obj)
        {
            _dic.Add(key, obj);
            AddHander?.Invoke();
        }
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dic.GetEnumerator();
        }
        /// <summary>
        /// 根据Key值移除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void Remove(string key)
        {
            if (_dic.ContainsKey(key))
                _dic.Remove(key);
            RemoveHander?.Invoke();
        }
        /// <summary>
        /// 批量移除数据
        /// </summary>
        /// <param name="keys"></param>
        public void RemoveMany(string[] keys)
        {
            foreach (var item in keys)
            {
                if (_dic.ContainsKey(item))
                    _dic.Remove(item);
            }
            RemoveHander?.Invoke();
        }

        public bool MoveNext()
        {
            return _dic.GetEnumerator().MoveNext();
        }

        public void Reset()
        {
            _dic.GetEnumerator().Reset();
        }


        public string Conver()
        {
            return _dic.ToString();
        }
    }
}