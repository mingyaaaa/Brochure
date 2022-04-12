using Brochure.Abstract;
using Brochure.Core.Interceptor;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Brochure.Core.RPC
{
    /// <summary>
    /// The rpc.
    /// </summary>
    public class Rpc<T> : IRpcProxy<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="args">The args.</param>
        public Rpc(IRpcProxyFactory factory, params object[] args)
        {
            Ins = factory.CreateRpcProxy<T>(args);
        }

        /// <summary>
        /// Gets the ins.
        /// </summary>
        public T Ins { get; }
    }

    /// <summary>
    /// The rpc proxy factory.
    /// </summary>
    public interface IRpcProxyFactory
    {
        /// <summary>
        /// Creates the rpc proxy.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>A T.</returns>
        T CreateRpcProxy<T>(params object[] args) where T : class;
    }

    /// <summary>
    /// 熔断代理类
    /// </summary>
    public class RpcPollyProxyFactory : IRpcProxyFactory
    {
        private readonly PollyOption option;

        /// <summary>
        /// Initializes a new instance of the <see cref="RpcPollyProxyFactory"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        public RpcPollyProxyFactory(PollyOption option)
        {
            this.option = option;
        }

        /// <summary>
        /// Creates the rpc proxy.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>A T.</returns>
        public T CreateRpcProxy<T>(params object[] args) where T : class
        {
            //创建代理类
            //var builder = new ProxyGeneratorBuilder();
            //builder.Configure(t => t.Interceptors.AddTyped<PollyInterceptor>(new object[] { option }));
            //var type = typeof(T);
            //var proxyCreate = builder.Build();
            //return proxyCreate.CreateClassProxy<T>(args);
            //todo
            return default(T);
        }
    }

    /// <summary>
    /// The rpc memory proxy factory.
    /// </summary>
    public class RpcMemoryProxyFactory : IRpcProxyFactory
    {
        private readonly object targetObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="RpcMemoryProxyFactory"/> class.
        /// </summary>
        /// <param name="targetObj">The target obj.</param>
        public RpcMemoryProxyFactory(object targetObj)
        {
            this.targetObj = targetObj;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RpcMemoryProxyFactory"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public RpcMemoryProxyFactory(Type type)
        {
            this.targetObj = Activator.CreateInstance(type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RpcMemoryProxyFactory"/> class.
        /// </summary>
        /// <param name="func">The func.</param>
        public RpcMemoryProxyFactory(Func<object> func)
        {
            targetObj = func.Invoke();
        }

        /// <summary>
        /// Creates the rpc proxy.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>A T.</returns>
        public T CreateRpcProxy<T>(params object[] args) where T : class
        {
            //var builder = new ProxyGeneratorBuilder();
            //builder.Configure(t => t.Interceptors.AddTyped<InnerAssemblyInterceptor>(new object[] { targetObj }));
            //var proxyCreate = builder.Build();
            //return proxyCreate.CreateClassProxy<T>(args);
            //todo
            return default(T);
        }
    }
}