using Brochure.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Brochure.Core.Server
{
    public class AuthManager : IAuthManager
    {
        private IAuth _auth;
        public AuthManager(IAuth auth)
        {
            _auth = auth;
        }
        public void AddAuthDic(IEnumerable<AuthModel> key)
        {
            _auth.Auths.AddRange(key);
        }

        public void RemoveAuthDic(IEnumerable<string> key)
        {
            var keyList = key.ToList();
            _auth.Auths.Remove(t => keyList.Contains(t.Key));
        }

        public bool HasAuth(string key)
        {
            return _auth.Auths.Any(t => t.Key == key);
        }
    }
}
