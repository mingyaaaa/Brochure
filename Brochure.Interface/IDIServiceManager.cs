using System;

namespace Brochure.Interface
{
    public interface IDIServiceManager
    {
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
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        IDIServiceManager AddScoped<TService, TImplementation>()
           where TService : class where TImplementation : TService;

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
        IDIServiceManager AddScoped<TService>(TService serviceType) where TService : class;

        /// <summary>
        /// 添加依赖注入  生命周期 作用域
        /// </summary>
        IDIServiceManager AddScoped<TService>(Func<object, TService> implementationFactory)
           where TService : class;

        IDIServiceManager AddScoped<TService, TImpService>(TImpService implementationInstance) where TService : class where TImpService : TService;

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
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        IDIServiceManager AddSingleton<TService, TImplementation>()
           where TService : class where TImplementation : TService;

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        IDIServiceManager AddSingleton(Type serviceType);

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        IDIServiceManager AddSingleton<TService>(TService serviceType) where TService : class;

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
        IDIServiceManager AddSingleton<TService>(Func<object, TService> implementationFactory)
           where TService : class;

        /// <summary>
        /// 添加依赖注入  生命周期 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="implementationInstance"></param>
        /// <returns></returns>
        IDIServiceManager AddSingleton<TService, TImpService>(TImpService implementationInstance) where TService : class where TImpService : TService;

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
        /// <returns></returns>
        IDIServiceManager AddTransient<TService>(TService serviceType) where TService : class;

        /// <summary>
        /// 添加依赖来注入   生命周期 ：瞬时
        /// </summary>
        /// <returns></returns>
        IDIServiceManager AddTransient<TService, TImplementation>()
           where TService : class where TImplementation : TService;

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
        IDIServiceManager AddTransient<TService>(Func<object, TService> implementationFactory)
              where TService : class;

        IDIServiceManager AddTransient<TService, TImpService>(TImpService implementationInstance) where TService : class where TImpService : TService;
        IServiceProvider BuildServiceProvider();
    }
}
