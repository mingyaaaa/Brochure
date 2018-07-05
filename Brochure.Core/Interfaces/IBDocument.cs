using System;
using System.Collections;
using System.Collections.Generic;

namespace Brochure.Core.Interfaces
{
    public interface IBDocument : IEnumerator
    {
        void Add (string key, object obj);

        object this [string key] { get; set; }
        int Count { get; }

        IEnumerable<string> Keys { get; }

        IEnumerable<object> Values { get; }
        void Remove (string key);
    }
}