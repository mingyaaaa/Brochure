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
    /// <summary>
    /// The plugin scope controller activator.
    /// </summary>
    internal class PluginScopeControllerActivator : IControllerActivator
    {
        private IPluginManagers _pluginManagers;
        private readonly IControllerActivator _controllerActivator;
        private ILifetimeScope _lifetimeScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginScopeControllerActivator"/> class.
        /// </summary>
        /// <param name="pluginManagers">The plugin managers.</param>
        /// <param name="controllerActivator"></param>
        public PluginScopeControllerActivator(IPluginManagers pluginManagers, IControllerActivator controllerActivator)
        {
            _pluginManagers = pluginManagers;
            _controllerActivator = controllerActivator;
        }

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>An object.</returns>
        public object Create(ControllerContext context)
        {
            var controllerType = context.ActionDescriptor.ControllerTypeInfo.AsType();
            var plugin = _pluginManagers.GetPlugins().FirstOrDefault(t => t.Assembly == controllerType.Assembly);
            if (plugin == null)
                _controllerActivator.Create(context);
            _lifetimeScope = plugin.Scope.BeginLifetimeScope();
            return _lifetimeScope.Resolve(controllerType);
        }

        /// <summary>
        /// Releases the.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="controller">The controller.</param>
        public void Release(ControllerContext context, object controller)
        {
            _lifetimeScope?.Dispose();
        }
    }
}