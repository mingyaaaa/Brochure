using System;
namespace Brochure.Core.Core
{
    public class DI
    {
        public static IServerManager ServerManager { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }
    }
}
