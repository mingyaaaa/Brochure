using AspectCore.Injector;
using System;

namespace Brochure.Core
{
    public static class IServerManagerExtend
    {
        /// <summary>
        /// 添加依赖来注入   生命周期 ：瞬时
        /// </summary>
        /// <param name="serviceType">接口类</param>
        /// <param name="implementationType">实现类</param>
        /// <returns></returns>
        public static IServerManager AddTransient(this IServerManager serverManager, Type serviceType, Type implementationType)
        {
            serverManager.Services.AddType(serviceType, implementationType);
            return serverManager;
        }



        //public IServerManager AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        //{
        //    _services.AddType(serviceType, implementationFactory);
        //   return serverManager;
        //}

        /// <summary>
        /// 添加依赖来注入   生命周期 ：瞬时
        /// </summary>
        /// <returns></returns>
        public static IServerManager AddTransient<TService, TImplementation>(this IServerManager serverManager)
            where TService : class where TImplementation : class, TService
        {
            serverManager.Services.AddType<TService, TImplementation>();
            return serverManager;
        }

        /// <summary>
        /// 添加依赖来注入   生命周期 ：瞬时 
        /// </summary>
        /// <param name="serviceType">实现类</param>
        /// <returns></returns>
        public static IServerManager AddTransient(this IServerManager serverManager, Type serviceType)
        {
            serverManager.Services.AddType(serviceType);
            return serverManager;
        }
        /// <summary>
        /// 添加依赖来注入   生命周期 ：瞬时
        /// </summary>
        /// <returns></returns>
        public static IServerManager AddTransient<TService>(this IServerManager serverManager) where TService : class
        {
            serverManager.Services.AddType<TService>();
            return serverManager;
        }


        //public IServerManager AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
        //    where TService : class
        //{
        //    _services.AddType<TService>(implementationFactory);
        //   return serverManager;
        //}

        //public IServerManager AddTransient<TService, TImplementation>(
        //    Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
        //    where TImplementation : class, TService
        //{
        //    _services.AddTransient<TService, TImplementation>(implementationFactory);
        //   return serverManager;
        //}

        /// <summary>
        /// 添加依赖注入  生命周期 作用域
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public static IServerManager AddScoped(this IServerManager serverManager, Type serviceType, Type implementationType)
        {
            serverManager.Services.AddType(serviceType, implementationType, Lifetime.Scoped);
            return serverManager;
        }

        //public IServerManager AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        //{
        //    _services.AddScoped(serviceType, implementationFactory);
        //   return serverManager;
        //}

        /// <summary>
        /// 添加依赖注入  生命周期 作用域
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        public static IServerManager AddScoped<TService, TImplementation>(this IServerManager serverManager)
            where TService : class where TImplementation : class, TService
        {
            serverManager.Services.AddType<TService, TImplementation>(Lifetime.Scoped);
            return serverManager;
        }

        /// <summary>
        /// 添加依赖注入  生命周期 作用域
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public static IServerManager AddScoped(this IServerManager serverManager, Type serviceType)
        {
            serverManager.Services.AddType(serviceType, Lifetime.Scoped);
            return serverManager;
        }

        /// <summary>
        /// 添加依赖注入  生命周期 作用域
        /// </summary>
        public static IServerManager AddScoped<TService>(this IServerManager serverManager) where TService : class
        {
            serverManager.Services.AddType<TService>(Lifetime.Scoped);
            return serverManager;
        }


        //public IServerManager AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory)
        //    where TService : class
        //{
        //    _services.AddScoped<TService>(implementationFactory);
        //   return serverManager;
        //}


        //public IServerManager AddScoped<TService, TImplementation>(
        //    Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
        //    where TImplementation : class, TService
        //{
        //    _services.AddScoped<TService, TImplementation>(implementationFactory);
        //   return serverManager;
        //}

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public static IServerManager AddSingleton(this IServerManager serverManager, Type serviceType, Type implementationType)
        {
            serverManager.Services.AddInstance(serviceType, implementationType);
            return serverManager;
        }


        //public IServerManager AddSingleton(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        //{
        //    _services.AddSingleton(serviceType, implementationFactory);
        //   return serverManager;
        //}


        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        public static IServerManager AddSingleton<TService, TImplementation>(this IServerManager serverManager)
            where TService : class where TImplementation : class, TService
        {
            serverManager.Services.AddType<TService, TImplementation>(Lifetime.Singleton);
            return serverManager;
        }

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public static IServerManager AddSingleton(this IServerManager serverManager, Type serviceType)
        {
            serverManager.Services.AddInstance(serviceType);
            return serverManager;
        }


        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public static IServerManager AddSingleton<TService>(this IServerManager serverManager) where TService : class
        {
            serverManager.Services.AddType<TService>(Lifetime.Singleton);
            return serverManager;
        }


        //public IServerManager AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
        //    where TService : class
        //{
        //    _services.AddSingleton<TService>();
        //   return serverManager;
        //}


        //public IServerManager AddSingleton<TService, TImplementation>(
        //    Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
        //    where TImplementation : class, TService
        //{
        //    _services.AddSingleton<TService, TImplementation>(implementationFactory);
        //   return serverManager;
        //}


        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationInstance"></param>
        /// <returns></returns>
        public static IServerManager AddSingleton(this IServerManager serverManager, Type serviceType, object implementationInstance)
        {
            serverManager.Services.AddInstance(serviceType, implementationInstance);
            return serverManager;
        }


        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="implementationInstance"></param>
        /// <returns></returns>
        public static IServerManager AddSingleton<TService>(this IServerManager serverManager, TService implementationInstance) where TService : class
        {
            serverManager.Services.AddInstance(implementationInstance);
            return serverManager;
        }
    }
}
