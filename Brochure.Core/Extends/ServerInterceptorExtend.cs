using AspectCore.Configuration;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using System;
using System.Threading.Tasks;

namespace Brochure.Core
{
    public static class ServerInterceptorExtend
    {
        /// <summary>
        ///注册拦截器 
        ///TypedInterceptor
        ///标记在接口，类或者方法上的特性拦截器或者使用上面config.Interceptors.AddTypeInterceptors<CustomInterceptor>();
        ///配置的全局拦截器，这类拦截器对于每个方法具有唯一的实例
        ///</summary>
        /// <param name="interceptorCollection"></param>
        /// <param name="interceptorType"></param>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public static IServerManager AddTypeInterceptors(this IServerManager serverManager,
            Type interceptorType, params AspectPredicate[] predicates)
        {
            serverManager.Services.Configure(config =>
            {
                config.Interceptors.AddTyped(interceptorType, predicates);
            });
            return serverManager;
        }

        /// <summary>
        ///注册拦截器 
        ///TypedInterceptor
        ///标记在接口，类或者方法上的特性拦截器或者使用上面config.Interceptors.AddTypeInterceptors<CustomInterceptor>();
        ///配置的全局拦截器，这类拦截器对于每个方法具有唯一的实例
        ///</summary>
        public static IServerManager AddTypeInterceptors(this IServerManager serverManager, Type interceptorType, object[] args,
            params AspectPredicate[] predicates)
        {
            serverManager.Services.Configure(config => { config.Interceptors.AddTyped(interceptorType, args, predicates); });
            return serverManager;
        }

        /// <summary>
        ///注册拦截器 
        ///TypedInterceptor
        ///标记在接口，类或者方法上的特性拦截器或者使用上面config.Interceptors.AddTypeInterceptors<CustomInterceptor>();
        ///配置的全局拦截器，这类拦截器对于每个方法具有唯一的实例
        ///</summary>
        public static IServerManager AddTypeInterceptors<TInterceptor>(this IServerManager serverManager,
            params AspectPredicate[] predicates) where TInterceptor : AspectCore.DynamicProxy.IInterceptor
        {
            serverManager.Services.Configure(config => { config.Interceptors.AddTyped<IInterceptor>(predicates); });
            return serverManager;
        }

        /// <summary>
        ///注册拦截器 
        ///TypedInterceptor
        ///标记在接口，类或者方法上的特性拦截器或者使用上面config.Interceptors.AddTypeInterceptors<CustomInterceptor>();
        ///配置的全局拦截器，这类拦截器对于每个方法具有唯一的实例
        ///</summary>
        public static IServerManager AddTypeInterceptors<TInterceptor>(this IServerManager serverManager, object[] args,
            params AspectPredicate[] predicates) where TInterceptor : AspectCore.DynamicProxy.IInterceptor
        {
            serverManager.Services.Configure(config => { config.Interceptors.AddTyped<TInterceptor>(args, predicates); });
            return serverManager;
        }

        /// <summary>
        ///注册拦截器 
        ///TypedInterceptor
        /// 注册到DI并从DI激活使用的拦截器。这类拦截器的生命周期同注册到DI时的生命周期一致
        ///</summary>
        public static IServerManager AddServerInterceptors(this IServerManager serverManager, Type interceptorType,
            params AspectPredicate[] predicates)
        {
            serverManager.Services.Configure(config => { config.Interceptors.AddServiced(interceptorType, predicates); });
            return serverManager;
        }

        /// <summary>
        ///注册拦截器 
        ///TypedInterceptor
        /// 注册到DI并从DI激活使用的拦截器。这类拦截器的生命周期同注册到DI时的生命周期一致
        ///</summary>
        public static IServerManager AddServerInterceptors<TInterceptor>(this IServerManager serverManager,
            params AspectPredicate[] predicates) where TInterceptor : AspectCore.DynamicProxy.IInterceptor
        {
            serverManager.Services.Configure(config => { config.Interceptors.AddServiced<TInterceptor>(predicates); });
            return serverManager;
        }

        /// <summary>
        ///注册拦截器 
        /// 在使用全局的拦截器配置时，我们也可以不定义具体的拦截器类，
        /// 而直接使用签名为Func<AspectDelegate, AspectDelegate>
        /// 或Func<AspectContext, AspectDelegate, Task>的委托来执行拦截，如下面：
        ///</summary>
        public static IServerManager AddDelegateInterceptor(this IServerManager serverManager,
            Func<AspectDelegate, AspectDelegate> aspectDelegate,
            int order, params AspectPredicate[] predicates)
        {
            serverManager.Services.Configure(config => { config.Interceptors.AddDelegate(aspectDelegate, order, predicates); });
            return serverManager;
        }

        /// <summary>
        ///注册拦截器 
        /// 在使用全局的拦截器配置时，我们也可以不定义具体的拦截器类，
        /// 而直接使用签名为Func<AspectDelegate, AspectDelegate>
        /// 或Func<AspectContext, AspectDelegate, Task>的委托来执行拦截，如下面：
        ///</summary>
        public static IServerManager AddDelegateInterceptor(this IServerManager serverManager,
            Func<AspectDelegate, AspectDelegate> aspectDelegate,
            params AspectPredicate[] predicates)
        {
            serverManager.Services.Configure(config => { config.Interceptors.AddDelegate(aspectDelegate, predicates); });
            return serverManager;
        }

        /// <summary>
        ///注册拦截器 
        /// 在使用全局的拦截器配置时，我们也可以不定义具体的拦截器类，
        /// 而直接使用签名为Func<AspectDelegate, AspectDelegate>
        /// 或Func<AspectContext, AspectDelegate, Task>的委托来执行拦截，如下面：
        ///</summary>
        public static IServerManager AddDelegateInterceptor(this IServerManager serverManager,
            Func<AspectContext, AspectDelegate, Task> aspectDelegate, int order,
            params AspectPredicate[] predicates)
        {
            serverManager.Services.Configure(config => { config.Interceptors.AddDelegate(aspectDelegate, order, predicates); });
            return serverManager;
        }

        /// <summary>
        ///注册拦截器 
        /// 在使用全局的拦截器配置时，我们也可以不定义具体的拦截器类，
        /// 而直接使用签名为Func<AspectDelegate, AspectDelegate>
        /// 或Func<AspectContext, AspectDelegate, Task>的委托来执行拦截，如下面：
        ///</summary>
        public static IServerManager AddDelegateInterceptor(this IServerManager serverManager,
            Func<AspectContext, AspectDelegate, Task> aspectDelegate,
            params AspectPredicate[] predicates)
        {
            serverManager.Services.Configure(config => { config.Interceptors.AddDelegate(aspectDelegate, predicates); });
            return serverManager;
        }

    }
}
