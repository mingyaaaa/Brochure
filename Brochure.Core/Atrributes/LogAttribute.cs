using AspectCore.DynamicProxy;
using Brochure.Core.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Brochure.Core.Atrributes
{
    public class LogAttribute : AbstractInterceptorAttribute
    {
        private string _message;

        public LogAttribute(string message)
        {
            _message = message;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var logger = DI.ServiceProvider.GetService<ILoggerFactory>().CreateLogger(nameof(LogAttribute));
            try
            {
                await context.Invoke(next);
            }
            catch (System.Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }
    }
}
