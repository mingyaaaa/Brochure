using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.PluginsDI
{
    internal class GlobLock
    {
        public static object LockObj = new object();
    }
}