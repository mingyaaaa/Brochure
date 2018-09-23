using System.Collections.Generic;

namespace Brochure.Core
{
    public interface IAuthManager
    {
        void AddAuthDic(IEnumerable<string> key);
        void RemoveAuthDic(IEnumerable<string> key);
        bool HasAuth(string key);
    }
}
