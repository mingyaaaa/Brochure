using Autofac;
using Brochure.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.Server.Core
{
    internal class PluginScopeControllerActivator : ServiceBasedControllerActivator
    {
        private IPluginManagers _pluginManagers;
        private ILifetimeScope _lifetimeScope;

        public PluginScopeControllerActivator(IPluginManagers pluginManagers)
        {
            _pluginManagers = pluginManagers;
        }

        public object Create(ControllerContext context)
        {
            var controllerType = context.ActionDescriptor.ControllerTypeInfo.AsType();
            var plugin = _pluginManagers.GetPlugins().FirstOrDefault(t => t.Assembly == controllerType.Assembly);
            if (plugin == null)
                base.Create(context);
            _lifetimeScope = plugin.Scope.BeginLifetimeScope();
            return _lifetimeScope.Resolve(controllerType);
        }

        public void Release(ControllerContext context, object controller)
        {
            _lifetimeScope?.Dispose();
        }
    }
}