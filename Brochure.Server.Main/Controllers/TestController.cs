using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.Core;
using Brochure.Server.Main.Abstract.Interfaces;
using Brochure.Server.Main.Core;
using Brochure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Server.Main.Controllers
{
    [Route ("api/v1/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IPluginManagers pluginManager;
        private readonly IBApplication application;
        private readonly IReflectorUtil reflectorUtil;

        public TestController (
            IPluginManagers pluginManager,
            IBApplication application,
            IReflectorUtil reflectorUtil)
        {
            this.pluginManager = pluginManager;
            this.application = application;
            this.reflectorUtil = reflectorUtil;
        }

        [HttpGet]
        public IActionResult GetAction1 ()
        {
            throw new Exception ("aaaa");
            return new ContentResult () { Content = "aaa" };
        }

        [HttpGet ("loadPlugin")]
        public async Task<IActionResult> TestLoadPlugin ()
        {
            var path = Path.Combine (pluginManager.GetBasePluginsPath (), "Brochure.Authority", "plugin.config");
            var p = await pluginManager.LoadPlugin (application.ServiceProvider, path);
            if (!(p is Plugins plugin))
                return new ContentResult () { Content = "bbb" };
            if (application is BApplication app)
            {
                var startConfigs = reflectorUtil.GetObjectOfBase<IStarupConfigure> (plugin.Assembly);
                var context = new PluginMiddleContext (app.ServiceProvider, plugin.Key);
                plugin.Context.Add (context);
                foreach (var item in startConfigs)
                {
                    item.Configure (plugin.Key, context);
                }
                app.ApplicationPartManager.ApplicationParts.Add (new AssemblyPart (plugin.Assembly));
            }
            PluginActionDescriptorChangeProvider.Instance.HasChanged = true;
            PluginActionDescriptorChangeProvider.Instance.TokenSource.Cancel ();
            return new ContentResult () { Content = "aaa" };
        }

        [HttpGet ("unLoadPlugin")]
        public async Task<IActionResult> UnLoadPlugin ()
        {
            var plugins = pluginManager.GetPlugins ();
            if (application is BApplication app)
            {
                foreach (var item in plugins)
                {
                    await pluginManager.UnLoadPlugin (item);
                    var part = app.ApplicationPartManager.ApplicationParts.FirstOrDefault (t => t.Name == item.Assembly.GetName ().Name);
                    app.ApplicationPartManager.ApplicationParts.Remove (part);
                }
            }
            PluginActionDescriptorChangeProvider.Instance.HasChanged = true;
            PluginActionDescriptorChangeProvider.Instance.TokenSource.Cancel ();
            return new ContentResult () { Content = "aaa" };
        }

        [Authorize]
        public IActionResult TestAuth ()
        {
            return Ok ();
        }
    }
}