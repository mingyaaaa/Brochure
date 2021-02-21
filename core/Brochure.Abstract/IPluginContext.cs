using System;
using System.Collections.Generic;

namespace Brochure.Abstract
{
    public interface IPluginContextDescript { }

    public interface IPluginContext : ICollection<IPluginContextDescript>
    {
        T GetPluginContext<T> ();
    }
}