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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Brochure.Core
{
    public static class IServiceCollectionExtensions
    {

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static IServiceCollection AddBrochureService(this IServiceCollection service, Action<ApplicationOption> appAction)
        {
            var option = new ApplicationOption();
            appAction(option);
            //初始化程序
            service.InitApplicationCore();
            //加载插件
            var pluginManager = service.GetServiceInstance<IPluginManagers>();

            pluginManager.ResolverPlugins(service, t =>
            {
                var r = option.OnPluginLoad?.Invoke(t) ?? true;
                return Task.FromResult(r);
            });
            return service;
        }

        internal static IServiceCollection InitApplicationCore(this IServiceCollection service)
        {
            //工具类初始化
            var utilInit = new UtilApplicationInit(service);
            utilInit.Init();
            return service;

        }

        /// <summary>
        /// 获取服务对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetServiceInstances<T>(this IServiceCollection services)
        {
            var type = typeof(T);
            return services.Where(t => t.ServiceType == type).Select(t => (T)t.ImplementationInstance);
        }

        /// <summary>
        /// 获取服务对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 加载依赖注入对象
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        public static void InitService(this IServiceCollection services, Assembly assembly)
        {
            var allTypes = assembly.GetTypes();
            foreach (var item in allTypes)
            {
                //添加单实现服务
                var type = item.GetInterface(nameof(ISingleton));
                if (type != null)
                {
                    var baseTypes = GetBaseTypeOrInterface(item);
                    foreach (var baseType in baseTypes)
                        AddService(services, baseType, item, ServiceLifetime.Singleton);
                    continue;
                }
                type = item.GetInterface(nameof(IScope));
                if (type != null)
                {
                    var baseTypes = GetBaseTypeOrInterface(item);
                    foreach (var baseType in baseTypes)
                        AddService(services, baseType, item, ServiceLifetime.Scoped);
                    continue;
                }
                type = item.GetInterface(nameof(ITransient));
                if (type != null)
                {
                    var baseTypes = GetBaseTypeOrInterface(item);
                    foreach (var baseType in baseTypes)
                        AddService(services, baseType, item, ServiceLifetime.Transient);
                    continue;
                }
                //添加多实现服务
                type = item.GetInterface(nameof(IMutiSingleton));
                if (type != null)
                {
                    var baseTypes = GetBaseTypeOrInterface(item);
                    foreach (var baseType in baseTypes)
                        AddMutiService(services, baseType, item, ServiceLifetime.Singleton);
                    continue;
                }
                type = item.GetInterface(nameof(IMutiScope));
                if (type != null)
                {
                    var baseTypes = GetBaseTypeOrInterface(item);
                    foreach (var baseType in baseTypes)
                        AddMutiService(services, baseType, item, ServiceLifetime.Scoped);
                    continue;
                }
                type = item.GetInterface(nameof(IMutiTransient));
                if (type != null)
                {
                    var baseTypes = GetBaseTypeOrInterface(item);
                    foreach (var baseType in baseTypes)
                        AddMutiService(services, baseType, item, ServiceLifetime.Transient);
                    continue;
                }
            }
        }

        private static IEnumerable<Type> GetBaseTypeOrInterface(Type type)
        {
            var result = new List<Type>();
            var interfaceTypes = type.GetInterfaces();
            var nameList = new[] { nameof(ITransient), nameof(IScope), nameof(ISingleton) ,
            nameof(IMutiScope),nameof(IMutiSingleton),nameof(IMutiTransient)};
            foreach (var item in interfaceTypes)
            {
                if (nameList.Contains(item.Name))
                    continue;
                result.Add(item);
            }
            if (type.BaseType != typeof(object) && type.BaseType != null)
                result.Add(type.BaseType);
            return result;
        }

        private static void AddService(IServiceCollection services, Type baseType, Type impType, ServiceLifetime serviceLifetime)
        {
            if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.TryAddSingleton(baseType, impType);
            }
            else if (serviceLifetime == ServiceLifetime.Scoped)
            {
                services.TryAddScoped(baseType, impType);

            }
            else if (serviceLifetime == ServiceLifetime.Transient)
            {
                services.AddTransient(baseType, impType);
            }
        }

        private static void AddMutiService(IServiceCollection services, Type baseType, Type impType, ServiceLifetime serviceLifetime)
        {
            if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton(baseType, impType);
            }
            else if (serviceLifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped(baseType, impType);

            }
            else if (serviceLifetime == ServiceLifetime.Transient)
            {
                services.AddTransient(baseType, impType);
            }
        }

    }
}