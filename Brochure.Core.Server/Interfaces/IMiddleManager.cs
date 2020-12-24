using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Brochure.Core.Server
{
    public interface IMiddleManager
    {
        Action<Func<RequestDelegate, RequestDelegate>> MiddleAction { get; set; }

        void AddMiddle(string middleName, Guid pluginId, Func<RequestDelegate, RequestDelegate> middle);
        void IntertMiddle(string middleName, Guid pluginId, int index, Func<RequestDelegate, RequestDelegate> middle);

        void RemovePluginMiddle(Guid guid);
        IReadOnlyList<RequestDelegateProxy> GetMiddlesList();

        void AddRange(IEnumerable<RequestDelegateProxy> proxy);
        void Reset();
    }
}