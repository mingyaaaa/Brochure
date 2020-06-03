using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Core
{
    public static class ApplicationOptionExtensions
    {
        public static void AddLog (this ApplicationOption applicationOption)
        {
            Log.Logger = applicationOption.Services.GetServiceInstance<ILoggerFactory> ()?.CreateLogger ("Brochure");
            Log.Services = applicationOption.Services;
        }
    }
}