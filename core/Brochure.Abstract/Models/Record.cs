using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Brochure.Abstract.Models
{
    /// <summary>
    /// 文档类型
    /// </summary>
    public class Record : IRecord
    {
        private readonly IDictionary<string, object> _dic;

        /// <summary>
        /// Initializes a new instance of the <see cref="Record"/> class.
        /// </summary>
        public Record()
        {
            _dic = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Record"/> class.
        /// </summary>
        /// <param name="dic">The dic.</param>
        public Record(IDictionary<string, object> dic)
        {
            _dic = dic;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                _dic.TryGetValue(key, out var value);
                return value;
            }
            set
            {
                if (_dic.ContainsKey(key))
                    _dic[key] = value;
                else
                    _dic.Add(key, value);
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        public ICollection<string> Keys => _dic.Keys;

        /// <summary>
        /// Gets the values.
        /// </summary>
        public ICollection<object> Values => _dic.Values;

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count => _dic.Count;

        /// <summary>
        /// Gets a value indicating whether read is only.
        /// </summary>
        public bool IsReadOnly => _dic.IsReadOnly;

        /// <summary>
        /// Adds the.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, object value)
        {
            _dic.Add(key, value);
        }

        /// <summary>
        /// Adds the.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<string, object> item)
        {
            _dic.Add(item);
        }

        /// <summary>
        /// Clears the.
        /// </summary>
        public void Clear()
        {
            _dic.Clear();
        }

        /// <summary>
        /// Contains the.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A bool.</returns>
        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dic.Contains(item);
        }

        /// <summary>
        /// Contains the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A bool.</returns>
        public bool ContainsKey(string key)
        {
            return _dic.ContainsKey(key);
        }

        /// <summary>
        /// Copies the to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">The array index.</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dic.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An IEnumerator.</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dic.GetEnumerator();
        }

        /// <summary>
        /// Removes the.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A bool.</returns>
        public bool Remove(string key)
        {
            return _dic.Remove(key);
        }

        /// <summary>
        /// Removes the.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A bool.</returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>A bool.</returns>
        public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
        {
            return TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An IEnumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}