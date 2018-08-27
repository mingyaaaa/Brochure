using Brochure.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Brochure.Core.Implements
{
    public class Plugins : IPlugins
    {
        public Plugins()
        {
        }
        public void Start()
        {
        }

        public void Exit()
        {
        }

        public Guid Key { get; }
        public List<Guid> DependencesKey { get; }
    }
}
