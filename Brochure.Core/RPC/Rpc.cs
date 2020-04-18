using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.Configuration;
using AspectCore.DynamicProxy;
using Brochure.Abstract;
using Brochure.Core.Interceptor;
using Polly;

namespace Brochure.Core.RPC
{
    public class Rpc<T> : IRpcProxy<T> where T : class
    {
        public Rpc (IRpcProxyFactory factory)
        {
            RpcServiceIns = factory.CreateRpcProxy<T> ();
        }
        public T RpcServiceIns { get; }

    }
    public interface IRpcProxyFactory
    {
        T CreateRpcProxy<T> () where T : class;
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
        public T CreateRpcProxy<T> () where T : class
        {
            //创建代理类  其代理类具有熔断的功能
            var builder = new ProxyGeneratorBuilder ();
            builder.Configure (t => t.Interceptors.AddTyped<PollyInterceptor> (new object[] { option }));
            var type = typeof (T);
            var proxyCreate = builder.Build ();
            return proxyCreate.CreateClassProxy<T> ();
        }

    }

}