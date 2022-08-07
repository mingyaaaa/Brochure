using AspectCore.DependencyInjection;
using Brochure.Abstract;

namespace Brochure.Core.AspectCore
{
    internal class AspectPluginScope : IPluginScope
    {
        private readonly IServiceResolver _serviceResolver;

        public AspectPluginScope(IServiceResolver serviceResolver)
        {
            _serviceResolver = serviceResolver;
        }

        public void Dispose()
        {
            _serviceResolver.Dispose();
        }
    }
}