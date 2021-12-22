using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.Server.Extensions
{
    public static class SwaggerGenOptionsExtensions
    {
        public static void ConfigPluginSwaggerGen(this SwaggerOptions options, string name, string version, string xmlPath)
        {
        }
    }
}