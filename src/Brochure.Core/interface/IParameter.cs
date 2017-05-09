using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Brochure.Core
{
    public interface IParameter
    {
        DbParameter Parameter { get; }
    }
}
