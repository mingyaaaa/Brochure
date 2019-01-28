using System;

namespace Brochure.Interface
{
    public interface IDIServiceManager
    {
        /// <summary>
        /// 添加依赖来注入   生命周期 ：瞬时
        /// </summary>
        /// <param name="serviceType">接口类</param>
        /// <param name="implementationType">实现类</param>
        /// <returns></returns>
        IDIServiceManager AddTransient(Type serviceType, Type implementationType);

        /// <summary>
        /// 添加依赖来注入   生命周期 ：瞬时
        /// </summary>
        /// <param name="serviceType">接口类</param>
        /// <param name="implementationType">实现类</param>
        IDIServiceManager AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        /// <summary>
        /// 添加依赖来注入   生命周期 ：瞬时
        /// </summary>
        /// <returns></returns>
        IDIServiceManager AddTransient<TService, TImplementation>()
           where TService : class where TImplementation : class, TService;

        /// <summary>
        /// 添加依赖来注入   生命周期 ：瞬时
        /// </summary>
        /// <param name="serviceType">实现类</param>
        /// <returns></returns>
        IDIServiceManager AddTransient(Type serviceType);

        /// <summary>
        /// 添加依赖来注入   生命周期 ：瞬时
        /// </summary>
        /// <returns></returns>
        IDIServiceManager AddTransient<TService>() where TService : class;

        /// <summary>
        /// 添加依赖来注入   生命周期 ：瞬时
        /// </summary>
        /// <returns></returns>
        IDIServiceManager AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
              where TService : class;

        /// <summary>
        /// 添加依赖来注入   生命周期 ：瞬时
        /// </summary>
        /// <returns></returns>
        IDIServiceManager AddTransient<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// 添加依赖注入  生命周期 作用域
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        IDIServiceManager AddScoped(Type serviceType, Type implementationType);

        /// <summary>
        /// 添加依赖注入  生命周期 作用域
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        IDIServiceManager AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        /// <summary>
        /// 添加依赖注入  生命周期 作用域
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        IDIServiceManager AddScoped<TService, TImplementation>()
           where TService : class where TImplementation : class, TService;

        /// <summary>
        /// 添加依赖注入  生命周期 作用域
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        IDIServiceManager AddScoped(Type serviceType);

        /// <summary>
        /// 添加依赖注入  生命周期 作用域
        /// </summary>
        IDIServiceManager AddScoped<TService>() where TService : class;

        /// <summary>
        /// 添加依赖注入  生命周期 作用域
        /// </summary>
        IDIServiceManager AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory)
           where TService : class;

        /// <summary>
        /// 添加依赖注入  生命周期 作用域
        /// </summary>
        IDIServiceManager AddScoped<TService, TImplementation>(
          Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
          where TImplementation : class, TService;

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        IDIServiceManager AddSingleton(Type serviceType, Type implementationType);

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        IDIServiceManager AddSingleton(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        IDIServiceManager AddSingleton<TService, TImplementation>()
           where TService : class where TImplementation : class, TService;

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        IDIServiceManager AddSingleton(Type serviceType);

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        IDIServiceManager AddSingleton<TService>() where TService : class;

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        IDIServiceManager AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
           where TService : class;

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        IDIServiceManager AddSingleton<TService, TImplementation>(
           Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
           where TImplementation : class, TService;

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationInstance"></param>
        /// <returns></returns>
        IDIServiceManager AddSingleton(Type serviceType, object implementationInstance);

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="implementationInstance"></param>
        /// <returns></returns>
        IDIServiceManager AddSingleton<TService>(TService implementationInstance) where TService : class;
    }
}
