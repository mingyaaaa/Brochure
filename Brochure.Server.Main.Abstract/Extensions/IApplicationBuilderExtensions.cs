using System;
using Brochure.Server.Main.Abstract.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Server.Main.Abstract.Extensions
{
    public static class IApplicationBuilderExtensions
    {

        public static void AddMiddle (this IApplicationBuilder application, Guid pluginId, Func<RequestDelegate, RequestDelegate> middleware)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager> ();
            middle.AddMiddle (pluginId, middleware);
        }

        public static void IntertMiddle (this IApplicationBuilder application, Guid pluginId, int index, Func<RequestDelegate, RequestDelegate> middleware)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager> ();
            middle.IntertMiddle (pluginId, index, middleware);
        }

        public static void AddMiddle (this IApplicationBuilder application, Guid pluginId, Action action)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager> ();
            middle.AddMiddle (pluginId, action);
        }

        public static void IntertMiddle (this IApplicationBuilder application, Guid pluginId, int index, Action action)
        {
            var middle = application.ApplicationServices.GetService<IMiddleManager> ();
            middle.IntertMiddle (pluginId, index, action);
        }

    }
}