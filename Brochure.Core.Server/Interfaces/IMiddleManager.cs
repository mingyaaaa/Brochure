using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Brochure.Core.Server
{
    public interface IMiddleManager
    {

        void AddMiddle (string middleName, Guid pluginId, Func<RequestDelegate, RequestDelegate> middle);
        void IntertMiddle (string middleName, Guid pluginId, int index, Func<RequestDelegate, RequestDelegate> middle);

        void AddMiddle (string middleName, Guid pluginId, Action action);
        void IntertMiddle (string middleName, Guid pluginId, int index, Action action);

        void RemovePluginMiddle (Guid guid);
        IReadOnlyList<RequestDelegateProxy> GetMiddlesList ();

        void AddRange (IEnumerable<RequestDelegateProxy> proxy);
        void Reset ();
    }
}