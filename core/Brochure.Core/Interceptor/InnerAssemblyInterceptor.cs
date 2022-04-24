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

        /// <summary>
        /// Intercepts the.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Intercept(IInvocation context)
        {
            var impType = impObj.GetType();
            var proxyMethod = context.Method;
            var impMethod = Array.Find(impType.GetMethods(), t => t.Name == proxyMethod.Name);
            if (impMethod == null)
                throw new Exception($"{impType}实现类中没有{proxyMethod.Name}方法");
            context.ReturnValue = impMethod.Invoke(impObj, context.Arguments);
        }

        /// <summary>
        /// Intercepts the async.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="proceedInfo">The proceed info.</param>
        /// <param name="proceed">The proceed.</param>
        /// <returns>A Task.</returns>
        protected override Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Intercepts the async.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="proceedInfo">The proceed info.</param>
        /// <param name="proceed">The proceed.</param>
        /// <returns>A Task.</returns>
        protected override Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            throw new NotImplementedException();
        }
    }
}