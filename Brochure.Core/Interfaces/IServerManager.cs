using AspectCore.Injector;
using System;

namespace Brochure.Core
{
    public interface IServerManager
    {
        IServiceContainer Services { get; }
        IServiceProvider BuildProvider();
    }
}
