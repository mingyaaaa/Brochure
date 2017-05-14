using System;
using System.Collections.Generic;
using System.Text;

namespace Brochure.Core
{
    public interface ISetting
    {
        string ConnectString { get; }
        string PreParamString { get; }
    }
}
