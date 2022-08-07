using AspectCore.DynamicProxy;

namespace Brochure.Core.Interceptor
{
    /// <summary>
    /// The inner assembly interceptor.
    /// </summary>
    public class InnerAssemblyInterceptor : AbstractInterceptor
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
        /// Invokes the.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="next">The next.</param>
        /// <returns>A Task.</returns>
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var impType = impObj.GetType();
            var proxyMethod = context.ProxyMethod;
            var impMethod = Array.Find(impType.GetMethods(), t => t.Name == proxyMethod.Name);
            if (impMethod == null)
                throw new Exception($"{impType}实现类中没有{proxyMethod.Name}方法");
            context.ReturnValue = impMethod.Invoke(impObj, context.Parameters);
            return Task.CompletedTask;
        }
    }
}