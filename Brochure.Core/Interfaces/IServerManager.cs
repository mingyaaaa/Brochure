using AspectCore.Injector;

namespace Brochure.Core
{
    public interface IServerManager
    {
        IServiceContainer Services { get; }
        IServiceResolver BuildProvider();
    }
}
