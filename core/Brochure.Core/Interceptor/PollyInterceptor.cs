using AspectCore.DynamicProxy;
using Brochure.Core.RPC;
using Polly;

namespace Brochure.Core.Interceptor
{
    /// <summary>
    /// The polly interceptor.
    /// </summary>
    public class PollyInterceptor : AbstractInterceptor
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

        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="next">The next.</param>
        /// <returns>A Task.</returns>
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var policy = Policy.Handle<Exception>().RetryAsync(this.option.RetryCount);
            await policy.ExecuteAsync(() => next?.Invoke(context));
        }
    }
}