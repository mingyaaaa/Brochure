using Brochure.Authority.Token;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Authority.Sessions
{
    public interface IUserTokenStore
    {
        ValueTask SetUserToken(string key, UserToken token);

        ValueTask<bool> TryGetUserToken(string key, out UserToken userToken);

        ValueTask TryRemove(string key, out UserToken token);
    }

    public class MemoryChacheUserTokenStore : IUserTokenStore
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryChacheUserTokenStore(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public ValueTask SetUserToken(string key, UserToken token)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> TryGetUserToken(string key, out UserToken userToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask TryRemove(string key, out UserToken token)
        {
            throw new NotImplementedException();
        }
    }
}