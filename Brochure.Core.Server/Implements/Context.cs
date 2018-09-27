using Brochure.Core.Interfaces;

namespace Brochure.Core.Server
{
    public class Context : IContext
    {
        public IAuthManager PluginAuth { get; }
    }
}
