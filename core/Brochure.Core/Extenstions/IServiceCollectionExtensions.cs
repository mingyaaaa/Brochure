using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core.AspectCore;
using Brochure.Core.PluginsDI;
using Brochure.Core.RPC;
using Brochure.Utils;
using Brochure.Utils.SystemUtils;
using Grpc.AspNetCore.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.Reflection;

namespace Brochure.Core
{
    /// <summary>
    /// The i service collection extensions.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="service"></param>
        /// <param name="appAction"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddBrochureCore(this IServiceCollection service, Action<ApplicationOption> appAction = null, IConfiguration configuration = null)
        {
            //加载一些基本的工具类
            //工具类初始化
            service.TryAddSingleton<IPluginManagers>(new PluginManagers());
            service.TryAddSingleton<IJsonUtil>(new JsonUtil());
            service.TryAddSingleton<IReflectorUtil>(new ReflectorUtil());
            service.TryAddSingleton<IObjectFactory>(new ObjectFactory());
            service.TryAddSingleton<ISysDirectory>(new SysDirectory());
            service.TryAddSingleton<IFile, FileUtils>();
            service.TryAddSingleton<IHostEnvironment, HostingEnvironment>();
            service.AddTransient<IPluginContext, PluginContext>();
            service.TryAddSingleton<IPluginLoadAction, DefaultLoadAction>();
            service.TryAddSingleton<IPluginLoader, PluginLoader>();
            service.TryAddSingleton<IPluginLoadContextProvider, PluginLoadContextProvider>();
            service.TryAddSingleton<IPluginUnLoadAction, DefaultUnLoadAction>();
            service.TryAddSingleton<IPluginConfigurationLoad, PluginConfigurationLoad>();
            service.TryAddSingleton(typeof(ISingletonService<>), typeof(SingletonService<>));
            service.TryAddScoped(typeof(IScopeService<>), typeof(ScopeService<>));
            service.TryAddTransient(typeof(ITransientService<>), typeof(TransientService<>));
            service.TryAddSingleton<IPluginScopeFactory, AspectPluginScopeFactory>();
            var option = new ApplicationOption(service, configuration);
            appAction?.Invoke(option);
            service.TryAddSingleton(option);
            return service;
        }

        /// <summary>
        /// 添加拦截器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBrochureInterceptor(this IServiceCollection services)
        {
            //   services.ConfigureDynamicProxy(configure);
            return services;
        }

        /// <summary>
        /// Adds the grpc service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configureOptions">The configure options.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddGrpcService(this IServiceCollection services, Action<GrpcServiceOptions>? configureOptions = null)
        {
            services.AddGrpc(configureOptions);
            return services;
        }

        /// <summary>
        /// Adds the grpc client.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The config.</param>
        /// <returns>An IServiceCollection.</returns>
        //public static IServiceCollection AddGrpcClient<T>(this IServiceCollection services, Action<PollyOption>? config = null) where T : class
        //{
        //    //todo  此处获取数据有问题
        //    var type = typeof(T);
        //    var memoryTypeName = type.Name.TrimEnd("Client".ToArray()) + "Base";
        //    var memoryType = Type.GetType(memoryTypeName);
        //    var grpcService = services.GetServiceInistaceType(memoryType);
        //    if (grpcService == null)
        //    {
        //        var pollyOption = new PollyOption() { RetryCount = 3 };
        //        config?.Invoke(pollyOption);
        //        services.AddSingleton<IRpcProxy<T>>(new Rpc<T>(new RpcPollyProxyFactory(pollyOption)));
        //    }
        //    else
        //    {
        //        services.AddSingleton<IRpcProxy<T>>(new Rpc<T>(new RpcMemoryProxyFactory(memoryType)));
        //    }
        //    return services;
        //}

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
        //public static T GetServiceInstance<T>(this IServiceCollection services)
        //{
        //    var type = typeof(T);
        //    var instance = (T)services.FirstOrDefault(t => t.ServiceType == type)?.ImplementationInstance;
        //    if (instance != null)
        //        return instance;
        //    var provider = services.BuildServiceProvider();
        //    instance = provider.GetService<T>();
        //    if (instance != null)
        //        return instance;
        //    var instanceType = services.FirstOrDefault(t => t.ServiceType == type)?.ImplementationType;
        //    if (instanceType != null)
        //    {
        //        instance = (T)Activator.CreateInstance(instanceType);
        //        if (instance != null)
        //            return instance;
        //    }
        //    var instanceFactory = services.FirstOrDefault(t => t.ServiceType == type)?.ImplementationFactory;
        //    if (instanceFactory != null)
        //    {
        //        instance = (T)instanceFactory.Invoke(services.BuildServiceProvider());
        //    }
        //    return (T)instance;
        //}

        /// <summary>
        /// Gets the service inistace type.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="type">The type.</param>
        /// <returns>A Type.</returns>
        //public static Type GetServiceInistaceType(this IServiceCollection services, Type type)
        //{
        //    var instance = services.FirstOrDefault(t => t.ServiceType == type)?.ImplementationInstance;
        //    if (instance != null)
        //        return instance.GetType();
        //    var instanceType = services.FirstOrDefault(t => t.ServiceType == type)?.ImplementationType;
        //    if (instanceType != null)
        //    {
        //        return instanceType;
        //    }
        //    var instanceFactory = services.FirstOrDefault(t => t.ServiceType == type)?.ImplementationFactory;
        //    if (instanceFactory != null)
        //    {
        //        instance = instanceFactory.Invoke(services.BuildServiceProvider());
        //    }
        //    return instance.GetType();
        //}

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
                        AddService(services, baseType, item, ServiceLifetime.Singleton);
                    continue;
                }
                type = item.GetInterface(nameof(IMutiScope));
                if (type != null)
                {
                    var baseTypes = GetBaseTypeOrInterface(item);
                    foreach (var baseType in baseTypes)
                        AddService(services, baseType, item, ServiceLifetime.Scoped);
                    continue;
                }
                type = item.GetInterface(nameof(IMutiTransient));
                if (type != null)
                {
                    var baseTypes = GetBaseTypeOrInterface(item);
                    foreach (var baseType in baseTypes)
                        AddService(services, baseType, item, ServiceLifetime.Transient);
                }
            }
        }

        /// <summary>
        /// Gets the base type or interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A list of Types.</returns>
        private static IEnumerable<Type> GetBaseTypeOrInterface(Type type)
        {
            var result = new List<Type>();
            var interfaceTypes = type.GetInterfaces();
            var nameList = new[]
            {
                nameof (ITransient), nameof (IScope), nameof (ISingleton),
                nameof (IMutiScope), nameof (IMutiSingleton), nameof (IMutiTransient)
            };
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

        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="baseType">The base type.</param>
        /// <param name="impType">The imp type.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
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
    }
}