using Brochure.Core.RPC;
using Castle.DynamicProxy;
using Polly;
using System;
using System.Threading.Tasks;

namespace Brochure.Core.Interceptor
{
    /// <summary>
    /// The polly interceptor.
    /// </summary>
    public class PollyInterceptor : IInterceptor
    {
        private readonly PollyOption option;

        /// <summary>
        /// Initializes a new instance of the <see cref="PollyInterceptor"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        public PollyInterceptor(PollyOption option)
        {
            this.option = option ??
                throw new ArgumentNullException(nameof(option));
        }

        public async void Intercept(IInvocation invocation)
        {
            var policy = Policy.Handle<Exception>().RetryAsync(this.option.RetryCount);
            await policy.ExecuteAsync(() => Task.Run(invocation.Proceed));
        }
    }
}