﻿using System;
using System.Collections.Generic;

namespace Brochure.Core
{
    public interface IPlugins
    {
        void Start();
        void Exit();
        Guid Key { get; }
        string Name { get; }
        long Version { get; }
        string Author { get; }
        string AssemblyName { get; }
        List<Guid> DependencesKey { get; }

    }
}
