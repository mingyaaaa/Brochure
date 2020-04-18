using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Brochure.Core.RPC;
using Polly;

namespace Brochure.Core.Interceptor
{
    public class PollyInterceptor : AbstractInterceptor
    {
        private readonly PollyOption option;

        public PollyInterceptor (PollyOption option)
        {
            this.option = option ??
                throw new ArgumentNullException (nameof (option));
        }
        public override async Task Invoke (AspectContext context, AspectDelegate next)
        {
            var policy = Policy.Handle<Exception> ().RetryAsync (this.option.RetryCount);
            await policy.ExecuteAsync (() => next?.Invoke (context));
        }
    }

}