using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Brochure.Server.Main.Abstract.Interfaces
{
    public interface IMiddleManager
    {

        void AddMiddle (Guid pluginId, Func<RequestDelegate, RequestDelegate> middle);
        void IntertMiddle (Guid pluginId, int index, Func<RequestDelegate, RequestDelegate> middle);

        void AddMiddle (Guid pluginId, Action action);
        void IntertMiddle (Guid pluginId, int index, Action action);

        void RemovePluginMiddle (Guid guid);
        IReadOnlyList<RequestDelegateProxy> GetMiddlesList ();

        void AddRange (IEnumerable<RequestDelegateProxy> proxy);
        void Reset ();
    }
}