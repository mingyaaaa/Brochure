using System;
using System.Threading.Tasks;
using Brochure.Abstract;

namespace Brochure.Core
{
    public class ApplicationOption
    {
        public ApplicationOption()
        {
        }

        public Func<IPluginOption, bool> OnPluginLoad { get; set; }
    }
}
