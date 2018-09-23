using Brochure.Core.Interfaces;

namespace Brochure.Core.Server.Implements
{
    public class Context : IContext
    {
        public IAuthManager PluginAuth { get; }
    }
}
