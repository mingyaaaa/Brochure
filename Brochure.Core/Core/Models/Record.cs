using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json;
using Brochure.Abstract;

namespace Brochure.Core
{
    /// <summary>
    /// 文档类型
    /// </summary>
    /// <typeparam name="BDocument"></typeparam>
    [Serializable]
    public class Record : IRecord, ISerializable
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

        public Record (IDictionary<string, object> dictionary)
        {
            _dic = dictionary;
        }

        public Record ()
        {
            _dic = new Dictionary<string, object> ();
        }

        public Record (SerializationInfo info, StreamingContext context)
        {
            _dic = new Dictionary<string, object> ();
            foreach (var item in info)
            {
                _dic[item.Name] = item.Value;
            }
        }

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
        public object this [string key]
        {
            get
            {
                if (!_dic.ContainsKey (key))
                    return null;
                return _dic[key];
            }
            set
            {
                var isAdd = false;
                if (!_dic.ContainsKey (key))
                    isAdd = true;
                _dic[key] = value;
                if (isAdd)
                    AddHander?.Invoke ();
                else
                    UpdateHander?.Invoke ();
            }
        }

        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void Add (string key, object obj)
        {
            _dic.Add (key, obj);
            AddHander?.Invoke ();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator ()
        {
            return _dic.GetEnumerator ();
        }

        /// <summary>
        /// 根据Key值移除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void Remove (string key)
        {
            if (_dic.ContainsKey (key))
                _dic.Remove (key);
            RemoveHander?.Invoke ();
        }

        /// <summary>
        /// 批量移除数据
        /// </summary>
        /// <param name="keys"></param>
        public void RemoveMany (string[] keys)
        {
            foreach (var item in keys)
            {
                if (_dic.ContainsKey (item))
                    _dic.Remove (item);
            }
            RemoveHander?.Invoke ();
        }

        public bool MoveNext ()
        {
            return _dic.GetEnumerator ().MoveNext ();
        }

        public void Reset ()
        {
            _dic.GetEnumerator ().Reset ();
        }

        public bool ContainsKey (string key)
        {
            return _dic.ContainsKey (key);
        }

        public override string ToString ()
        {
            if (_dic == null)
                return string.Empty;
            return JsonSerializer.Serialize (_dic);
        }

        public void GetObjectData (SerializationInfo info, StreamingContext context)
        {
            foreach (var item in _dic)
            {
                info.AddValue (item.Key, item.Value);
            }
        }

        public object ConvertFromObject (object obj)
        {
            return new Record (obj.AsDictionary ());
        }
    }

}