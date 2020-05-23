using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Server.Main.Abstract.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Server.Main.Core
{
    public class MiddleManager : IMiddleManager
    {
        private int pluginCount = -1;
        public MiddleManager (IPluginManagers pluginManagers)
        {
            middleCollection = new List<Tuple<int, Func<object>>> ();
            this.pluginManagers = pluginManagers;
        }
        private readonly List<Tuple<int, Func<object>>> middleCollection;
        private readonly IPluginManagers pluginManagers;

        public void AddMiddle (Func<RequestDelegate, RequestDelegate> middle)
        {
            var count = middleCollection.Count;
            middleCollection.Add (Tuple.Create<int, Func<object>> (count + 1, () => middle)); //顺序从1开始
        }

        public void IntertMiddle (int index, Func<RequestDelegate, RequestDelegate> middle)
        {
            middleCollection.Add (Tuple.Create<int, Func<object>> (index, () => middle));
        }
        public void AddMiddle (Action action)
        {
            var count = middleCollection.Count;
            middleCollection.Add (Tuple.Create<int, Func<object>> (count + 1, () =>
            {
                action.Invoke ();
                return null;
            })); //顺序从1开始
        }

        public void IntertMiddle (int index, Action action)
        {
            middleCollection.Add (Tuple.Create<int, Func<object>> (index, () =>
            {
                action.Invoke ();
                return null;
            })); //顺序从1开始
        }

        public int GetMiddleCount ()
        {
            return middleCollection.Count;
        }
        public RequestDelegate UseMiddle ()
        {

            return StartPipe ();
        }
        private RequestDelegate Default404EndPipe ()
        {
            return context =>
            {
                // If we reach the end of the pipeline, but we have an endpoint, then something unexpected has happened.
                // This could happen if user code sets an endpoint, but they forgot to add the UseEndpoint middleware.
                var endpoint = context.GetEndpoint ();
                var endpointRequestDelegate = endpoint?.RequestDelegate;
                if (endpointRequestDelegate != null)
                {
                    var message =
                        $"The request reached the end of the pipeline without executing the endpoint: '{endpoint.DisplayName}'. " +
                        $"Please register the EndpointMiddleware using '{nameof(IApplicationBuilder)}.UseEndpoints(...)' if using " +
                        $"routing.";
                    throw new InvalidOperationException (message);
                }
                context.Response.StatusCode = 404;
                return Task.CompletedTask;
            };
        }

        private RequestDelegate StartPipe ()
        {
            RequestDelegate app = null;
            return t =>
            {
                var count = pluginManagers.GetPlugins ().Count;
                if (pluginCount != count)
                {
                    app = Default404EndPipe ();
                    var list = middleCollection.OrderBy (t => t.Item1).ToList ();
                    middleCollection.Clear ();
                    //注入所有的Fun<RequestDelegate,RequestDelegate>
                    foreach (var tuple in list)
                    {
                        var r = tuple.Item2.Invoke ();
                        if (r != null && r is Func<RequestDelegate, RequestDelegate>)
                            AddMiddle (r as Func<RequestDelegate, RequestDelegate>);
                    }
                    var reqList = middleCollection.OrderByDescending (t => t.Item1).Select (t => t.Item2).ToList ();
                    foreach (var item in reqList)
                    {
                        var r = item ();
                        var type = typeof (Func<,>);
                        if (r is Func<RequestDelegate, RequestDelegate> f)
                            app = f (app);
                    }
                    pluginCount = count;
                    middleCollection.Clear ();
                    middleCollection.AddRange (list);
                }
                app.Invoke (t);
                return Task.CompletedTask;
            };
        }
    }
}