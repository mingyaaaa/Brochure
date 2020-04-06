using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core.Core;
using Brochure.Core.Models;
using Brochure.System;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Core
{
    public static class IServiceCollectionExtensions
    {

        /// <summary>
        /// 初始化程序
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static IServiceCollection AddBrochureService(this IServiceCollection service, Func<IPluginOption, bool> pluginAction)
        {
            //初始化程序
            service.InitApplication();
            //加载插件
            var pluginManager = service.GetServiceInstance<IPluginManagers>();
            pluginManager.ResolverPlugins(service, t =>
            {
                var r = pluginAction(t);
                return Task.FromResult(r);
            });
            return service;
        }

        internal static IServiceCollection InitApplication(this IServiceCollection service)
        {

            //工具类初始化
            var utilInit = new UtilApplicationInit(service, null);
            utilInit.Init();
            return service;

        }

        public static IEnumerable<T> GetServiceInstances<T>(this IServiceCollection services)
        {
            var type = typeof(T);
            return services.Where(t => t.ServiceType == type).Select(t => (T)t.ImplementationInstance);
        }

        public static T GetServiceInstance<T>(this IServiceCollection services)
        {
            var type = typeof(T);
            var instance = (T)services.FirstOrDefault(t => t.ServiceType == type)?.ImplementationInstance;
            if (instance != null)
                return instance;
            var instanceType = services.FirstOrDefault(t => t.ServiceType == type)?.ImplementationType;
            if (instanceType != null)
            {
                instance = (T)Activator.CreateInstance(instanceType);
                if (instance != null)
                    return instance;
            }
            var instanceFactory = services.FirstOrDefault(t => t.ServiceType == type)?.ImplementationFactory;
            if (instanceFactory != null)
            {
                instance = (T)instanceFactory.Invoke(services.BuildServiceProvider());
            }
            return (T)instance;
        }
    }
}