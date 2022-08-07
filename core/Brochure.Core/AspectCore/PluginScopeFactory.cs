using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.AspectCore
{
    internal class AspectPluginScopeFactory : IPluginScopeFactory
    {
        private readonly IServiceResolver _serviceResolver;

        public AspectPluginScopeFactory(IServiceResolver serviceResolver)
        {
            _serviceResolver = serviceResolver;
        }

        public IPluginScope CreateScope(IServiceCollection services)
        {
            var scope = _serviceResolver.CreateScope(t =>
               {
                   var serviceContext = services.ToScopeServiceContext(t.Configuration);
                   foreach (var item in serviceContext)
                       t.Add(item);
               });
            return new AspectPluginScope(scope);
        }
    }
}