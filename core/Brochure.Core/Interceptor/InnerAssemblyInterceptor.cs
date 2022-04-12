using Castle.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace Brochure.Core.Interceptor
{
    /// <summary>
    /// The inner assembly interceptor.
    /// </summary>
    public class InnerAssemblyInterceptor : AsyncInterceptorBase
    {
        private readonly object impObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="InnerAssemblyInterceptor"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public InnerAssemblyInterceptor(object obj)
        {
            this.impObj = obj;
        }

        public void Intercept(IInvocation context)
        {
            var impType = impObj.GetType();
            var proxyMethod = context.Method;
            var impMethod = Array.Find(impType.GetMethods(), t => t.Name == proxyMethod.Name);
            if (impMethod == null)
                throw new Exception($"{impType}实现类中没有{proxyMethod.Name}方法");
            context.ReturnValue = impMethod.Invoke(impObj, context.Arguments);
        }

        protected override Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            throw new NotImplementedException();
        }

        protected override Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            throw new NotImplementedException();
        }
    }
}