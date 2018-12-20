using AspectCore.DynamicProxy;
using LogServer.Server;
using System;
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
            var logger = new RpcClient<ILogService.Client>(LogServer.ServiceKey.Key);
            try
            {
                await context.Invoke(next);
            }
            catch (System.Exception e)
            {
                logger.Client.Error(new Log(_message, DateTime.Now.ToString(), e.StackTrace));
            }
        }
    }
}
