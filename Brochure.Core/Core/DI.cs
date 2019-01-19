using AspectCore.Injector;

namespace Brochure.Core.Core
{
    public static class DI
    {
        public static IServerManager ServerManager { get; set; }
        public static IServiceResolver ServiceProvider { get; set; }
    }
}
