using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core;
using Brochure.Core.Server;
using Brochure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Server.Main.Controllers
{
    /// <summary>
    /// The test controller.
    /// </summary>
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class TestController : ControllerBase
    {
        private readonly IPluginLoader pluginLoader;
        private readonly IReflectorUtil reflectorUtil;
        private readonly IPluginManagers pluginManagers;
        private readonly IMvcBuilder mvcBuilder;
        private readonly IApplicationBuilder applicationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestController"/> class.
        /// </summary>
        /// <param name="pluginLoader">The plugin loader.</param>
        /// <param name="application">The application.</param>
        /// <param name="reflectorUtil">The reflector util.</param>
        /// <param name="pluginManagers">The plugin managers.</param>
        public TestController(
            IPluginLoader pluginLoader,
            IReflectorUtil reflectorUtil,
            IPluginManagers pluginManagers, IMvcBuilder mvcBuilder, IApplicationBuilder applicationBuilder)
        {
            this.pluginLoader = pluginLoader;
            this.reflectorUtil = reflectorUtil;
            this.pluginManagers = pluginManagers;
            this.mvcBuilder = mvcBuilder;
            this.applicationBuilder = applicationBuilder;
        }

        /// <summary>
        /// Tests the load plugin.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet("loadPlugin")]
        public async Task<IActionResult> TestLoadPlugin()
        {
            var path = Path.Combine(pluginManagers.GetBasePluginsPath(), "Brochure.Authority", "plugin.config");
            var p = await pluginLoader.LoadPlugin(path);
            if (!(p is Plugins plugin))
                return new ContentResult() { Content = "bbb" };

            var startConfigs = reflectorUtil.GetObjectOfBase<IStarupConfigure>(plugin.Assembly);
            //         var context = new PluginMiddleContext(app.ServiceProvider, plugin.Key);
            //  plugin.Context.Services.Add(context);
            foreach (var item in startConfigs)
            {
                item.Configure(plugin.Key, applicationBuilder);
            }
            mvcBuilder.AddApplicationPart(plugin.Assembly);

            PluginActionDescriptorChangeProvider.Instance.HasChanged = true;
            PluginActionDescriptorChangeProvider.Instance.TokenSource.Cancel();
            return new ContentResult() { Content = "aaa" };
        }

        /// <summary>
        /// Uns the load plugin.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet("unLoadPlugin")]
        public async Task<IActionResult> UnLoadPlugin()
        {
            var plugins = pluginManagers.GetPlugins();

            foreach (var item in plugins)
            {
                await pluginLoader.UnLoad(item.Key);
                var part = mvcBuilder.PartManager.ApplicationParts.FirstOrDefault(t => t.Name == item.Assembly.GetName().Name);
                mvcBuilder.PartManager.ApplicationParts.Remove(part);
            }

            PluginActionDescriptorChangeProvider.Instance.HasChanged = true;
            PluginActionDescriptorChangeProvider.Instance.TokenSource.Cancel();
            return new ContentResult() { Content = "aaa" };
        }

        //[Authorize]
        //public IActionResult TestAuth()
        //{
        //    return Ok();
        //}
    }
}