using System;
using System.Collections.Generic;

namespace Brochure.Core.Interfaces
{
    public interface IPlugins
    {
        void Start();
        void Exit();
        Guid Key { get; }
        List<Guid> DependencesKey { get; }
    }
}
