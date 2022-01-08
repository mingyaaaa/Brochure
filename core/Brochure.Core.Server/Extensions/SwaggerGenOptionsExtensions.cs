using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.Server.Extensions
{
    /// <summary>
    /// The swagger gen options extensions.
    /// </summary>
    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Configs the plugin swagger gen.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <param name="xmlPath">The xml path.</param>
        public static void ConfigPluginSwaggerGen(this SwaggerOptions options, string name, string version, string xmlPath)
        {
        }
    }
}