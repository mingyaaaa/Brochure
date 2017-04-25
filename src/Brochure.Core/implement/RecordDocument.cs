using System;
using System.Collections;
using System.Collections.Generic;
using Brochure.Core.Extends;

namespace Brochure.Core
{
    public class RecordDocument : IDocument
    {
        private IDictionary<string, object> _dic;

        public RecordDocument(object data)
        {
            _dic = data.AsDictionary();
        }

        public RecordDocument(IDictionary<string, object> dic)
        {
            _dic = dic ?? new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get { return _dic[key]; }
            set { _dic[key] = value; }
        }

        public int Count => _dic.Count;

        public bool IsReadOnly => _dic.IsReadOnly;

        public ICollection<string> Keys => _dic.Keys;

        public ICollection<object> Values => _dic.Values;

        public void Add(KeyValuePair<string, object> item)
        {
            _dic.Add(item);
        }

        public void Add(string key, object value)
        {
            _dic.Add(key, value);
        }

        public void Clear()
        {
            _dic.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dic.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _dic.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dic.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dic.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _dic.Remove(item);
        }

        public bool Remove(string key)
        {
            return _dic.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _dic.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dic.GetEnumerator();
        }
    }
}