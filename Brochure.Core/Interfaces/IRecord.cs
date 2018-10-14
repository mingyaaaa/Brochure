using System.Collections;
using System.Collections.Generic;

namespace Brochure.Core
{
    public interface IRecord : IEnumerator
    {
        void Add(string key, object obj);

        object this[string key] { get; set; }

        IEnumerable<string> Keys { get; }

        IEnumerable<object> Values { get; }
        void Remove(string key);
        bool ContainsKey(string key);
    }
}
