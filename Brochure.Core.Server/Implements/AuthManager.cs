using System.Collections.Generic;

namespace Brochure.Core.Server
{
    public class AuthManager : IAuthManager
    {
        private IAuth _auth;
        public AuthManager(IAuth auth)
        {
            _auth = auth;
        }
        public void AddAuthDic(IEnumerable<string> key)
        {
            _auth.Auths.AddRange(key);
        }

        public void RemoveAuthDic(IEnumerable<string> key)
        {
            _auth.Auths.RemoveRange(key);
        }

        public bool HasAuth(string key)
        {
            return _auth.Auths.Contains(key);
        }
    }
}
