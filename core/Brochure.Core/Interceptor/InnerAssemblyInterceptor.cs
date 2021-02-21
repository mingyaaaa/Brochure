using System;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace Brochure.Core.Interceptor
{
    public class InnerAssemblyInterceptor : AbstractInterceptor
    {
        private readonly object impObj;

        public InnerAssemblyInterceptor (object obj)
        {
            this.impObj = obj;
        }
        public override Task Invoke (AspectContext context, AspectDelegate next)
        {
            var impType = impObj.GetType ();
            var proxyMethod = context.ProxyMethod;
            var impMethod = Array.Find (impType.GetMethods (), t => t.Name == proxyMethod.Name);
            if (impMethod == null)
                throw new Exception ($"{impType}实现类中没有{proxyMethod.Name}方法");
            context.ReturnValue = impMethod.Invoke (impObj, context.Parameters);
            return Task.CompletedTask;
        }
    }
}