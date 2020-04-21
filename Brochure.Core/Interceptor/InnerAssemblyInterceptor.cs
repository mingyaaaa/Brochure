using System.Linq;
using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace Brochure.Core.Interceptor
{
    public class InnerAssemblyInterceptor : AbstractInterceptor
    {
        private readonly Type impType;

        public InnerAssemblyInterceptor(Type type)
        {
            this.impType = type;
        }
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var proxyMethod = context.ProxyMethod;
            var impMethod = Array.Find(impType.GetMethods(), t => t.Name == proxyMethod.Name);
            if (impMethod == null)
                throw new Exception($"{impType}实现类中没有{proxyMethod.Name}方法");
            context.ReturnValue = impMethod.Invoke(context.Proxy, context.Parameters);
            return Task.CompletedTask;
        }
    }
}