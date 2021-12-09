using System;
using System.Threading.Tasks;

namespace Brochure.Abstract
{
    public interface IPluginLoader
    {
        ValueTask LoadPlugin(IServiceProvider service);

        ValueTask<bool> UnLoad(Guid key);
    }
}