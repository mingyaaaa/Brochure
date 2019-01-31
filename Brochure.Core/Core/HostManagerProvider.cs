using Brochure.Interface;

namespace Brochure.Core
{
    public class HostManagerProvider
    {
        private IHostManager _hostManager;
        public HostManagerProvider(IHostManager hostManager)
        {
            _hostManager = hostManager;
        }
        public IHostManager GetHostManager()
        {
            return _hostManager;
        }
    }
}
