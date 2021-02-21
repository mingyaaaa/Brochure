using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.Configuration;
using AspectCore.DynamicProxy;
using Brochure.Abstract;
using Brochure.Core.Interceptor;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Brochure.Core.RPC
{
    public class Rpc<T> : IRpcProxy<T> where T : class
    {
        public Rpc (IRpcProxyFactory factory, params object[] args)
        {
            Ins = factory.CreateRpcProxy<T> (args);
        }
        public T Ins { get; }
    }
    public interface IRpcProxyFactory
    {
        T CreateRpcProxy<T> (params object[] args) where T : class;
    }

    /// <summary>
    /// 熔断代理类
    /// </summary>
    public class RpcPollyProxyFactory : IRpcProxyFactory
    {
        private readonly PollyOption option;

        public RpcPollyProxyFactory (PollyOption option)
        {
            this.option = option;
        }
        public T CreateRpcProxy<T> (params object[] args) where T : class
        {
            //创建代理类                                                                                                             
            var builder = new ProxyGeneratorBuilder ();
            builder.Configure (t => t.Interceptors.AddTyped<PollyInterceptor> (new object[] { option }));
            var type = typeof (T);
            var proxyCreate = builder.Build ();
            return proxyCreate.CreateClassProxy<T> (args);
        }
    }

    public class RpcMemoryProxyFactory : IRpcProxyFactory
    {
        private readonly object targetObj;

        public RpcMemoryProxyFactory (object targetObj)
        {
            this.targetObj = targetObj;
        }
        public RpcMemoryProxyFactory (Type type)
        {
            this.targetObj = Activator.CreateInstance (type);
        }
        public RpcMemoryProxyFactory (Func<object> func)
        {
            targetObj = func.Invoke ();
        }
        public T CreateRpcProxy<T> (params object[] args) where T : class
        {
            var builder = new ProxyGeneratorBuilder ();
            builder.Configure (t => t.Interceptors.AddTyped<InnerAssemblyInterceptor> (new object[] { targetObj }));
            var proxyCreate = builder.Build ();
            return proxyCreate.CreateClassProxy<T> (args);
        }
    }
}