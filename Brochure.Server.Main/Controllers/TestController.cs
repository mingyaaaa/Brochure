using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Server.Main.Core;
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

        public TestController (IPluginManagers pluginManager, IBApplication application)
        {
            this.pluginManager = pluginManager;
            this.application = application;
        }

        [HttpGet]
        public async Task<IActionResult> GetAction1 ()
        {
            return new ContentResult () { Content = "aaa" };
        }

        [HttpGet ("loadPlugin")]
        public async Task<IActionResult> TestLoadPlugin (string id)
        {
            return new ContentResult () { Content = "aaa" };
        }

        [HttpGet ("unLoadPlugin")]
        public async Task<IActionResult> TestUnLoadPlugin ()
        {
            return new ContentResult () { Content = "aaa" };
        }
    }
}